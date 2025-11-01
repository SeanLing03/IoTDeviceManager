using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoTDeviceManager.Services
{
    /// Encapsulates simulated device communication (random data + failures)
    /// and emits log messages for telemetry/errors so UI layers don't need
    /// to know how the messages are composed.
    public class SimulatedCommunicator
    {
        private readonly Random _rng = new();

        /// Raised whenever a telemetry or error log should be produced.
        /// (action is "Telemetry" or "Error")
        public event EventHandler<(string action, string message)>? LogProduced;

        /// Poll a device with a timeout. On success/failure, this method raises
        /// LogProduced with a ready-to-display message.
        public async Task<(bool ok, string? payload, string? error)> PollDeviceAsync(
            string deviceName,
            string deviceType,
            TimeSpan timeout,
            CancellationToken? externalToken = null)
        {
            using var cts = externalToken is null ? new CancellationTokenSource(timeout) : null;
            var token = externalToken ?? cts!.Token;

            try
            {
                // Simulate variable transport latency/jitter
                await Task.Delay(_rng.Next(120, 600), token);

                // 15% chance of transport failure
                if (_rng.NextDouble() < 0.15)
                    throw new TimeoutException("Simulated transport timeout");

                // Produce telemetry
                string payload = deviceType switch
                {
                    "Temperature" => $"{20 + _rng.NextDouble() * 15:0.0} °C",
                    "Humidity" => $"{40 + _rng.NextDouble() * 40:0.0} %RH",
                    "Counter" => $"{_rng.Next(1000, 9999)}",
                    _ => $"{_rng.NextDouble():0.000}"
                };

                // Delegate the log text to the communicator (separation of concerns)
                LogProduced?.Invoke(this, ("Telemetry", $"{deviceName}: {payload}"));
                return (true, payload, null);
            }
            catch (OperationCanceledException)
            {
                var err = "Timeout";
                LogProduced?.Invoke(this, ("Error", $"{deviceName}: {err}"));
                return (false, null, err);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                LogProduced?.Invoke(this, ("Error", $"{deviceName}: {err}"));
                return (false, null, err);
            }
        }
    }
}
