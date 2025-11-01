using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using IoTDeviceManager.Models;

namespace IoTDeviceManager.Services
{
    /// Coordinates simulated connectivity + polling and forwards both
    /// telemetry results and log messages to upper layers.
    public class DeviceService : IDisposable
    {
        private readonly DispatcherTimer _timer;
        private readonly Random _rng = new Random();
        private readonly SimulatedCommunicator _comm = new();

        // device telemetry back to VM (error == null on success)
        public event EventHandler<(Device device, string? error)>? TelemetryReceived;

        // communicator-authored logs (Telemetry/Error) forwarded to VM
        public event EventHandler<(string action, string message)>? LogProduced;

        public IList<Device> SeedDevices { get; } = new List<Device>
        {
            new Device { Name = "Boiler Temp 01",   Type = "Temperature", IsOnline = true  },
            new Device { Name = "Line A Counter",   Type = "Counter",     IsOnline = true  },
            new Device { Name = "Room Humidity",    Type = "Humidity",    IsOnline = false },
            new Device { Name = "Air Cond 2F",      Type = "Temperature", IsOnline = true  },
            new Device { Name = "Cold Room RH",     Type = "Humidity",    IsOnline = true  },
        };

        public DeviceService(TimeSpan? interval = null)
        {
            // Forward communicator logs to consumers of DeviceService
            _comm.LogProduced += (_, log) => LogProduced?.Invoke(this, log);

            _timer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = interval ?? TimeSpan.FromMilliseconds(1500)
            };
            _timer.Tick += OnTick;
            _timer.Start();
        }

        private async void OnTick(object? sender, EventArgs e)
        {
            if (!SeedDevices.Any()) return;

            var device = SeedDevices[_rng.Next(0, SeedDevices.Count)];

            // Randomly simulate connectivity flaps (~10%)
            if (_rng.NextDouble() < 0.10)
                device.IsOnline = !device.IsOnline;

            if (!device.IsOnline)
            {
                // Log comes from "backend" (communicator semantics live below UI)
                LogProduced?.Invoke(this, ("Error", $"{device.Name}: Device unreachable (simulated)."));
                TelemetryReceived?.Invoke(this, (device, "Device unreachable (simulated)."));
                return;
            }

            // Poll using the communicator; it will emit its own Telemetry/Error logs
            var (ok, payload, error) = await _comm.PollDeviceAsync(
                device.Name, device.Type, TimeSpan.FromMilliseconds(800));

            if (ok)
            {
                device.LastData = payload!;
                device.LastSeenUtc = DateTime.UtcNow;
                TelemetryReceived?.Invoke(this, (device, null));
            }
            else
            {
                TelemetryReceived?.Invoke(this, (device, error));
            }
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= OnTick;
        }
    }
}
