using System;

namespace IoTDeviceManager.Models
{
    /// Simple audit log entry.
    public class LogEntry
    {
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
        public string Action { get; set; } = string.Empty; // Add, Update, Delete, ToggleStatus, Telemetry, Error
        public string Message { get; set; } = string.Empty;
    }
}
