using System;
using System.ComponentModel;

namespace IoTDeviceManager.Models
{
    /// Represents a minimal IoT device.
    public class Device : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _type = "Sensor";
        private bool _isOnline;
        private string _lastData = "-";
        private DateTime _lastSeenUtc = DateTime.MinValue;
        private bool _recentlyUpdated;

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name
        {
            get => _name;
            set { if (_name != value) { _name = value; OnPropertyChanged(nameof(Name)); } }
        }

        public string Type
        {
            get => _type;
            set { if (_type != value) { _type = value; OnPropertyChanged(nameof(Type)); } }
        }

        public bool IsOnline
        {
            get => _isOnline;
            set { if (_isOnline != value) { _isOnline = value; OnPropertyChanged(nameof(IsOnline)); OnPropertyChanged(nameof(Status)); } }
        }

        /// Computed string for table display.
        public string Status => IsOnline ? "Online" : "Offline";

        /// Mock of the last telemetry payload/value received.
        public string LastData
        {
            get => _lastData;
            set { if (_lastData != value) { _lastData = value; OnPropertyChanged(nameof(LastData)); } }
        }

        /// UTC timestamp of last successful communication.
        public DateTime LastSeenUtc
        {
            get => _lastSeenUtc;
            set { if (_lastSeenUtc != value) { _lastSeenUtc = value; OnPropertyChanged(nameof(LastSeenUtc)); } }
        }

        public bool RecentlyUpdated
        {
            get => _recentlyUpdated;
            set { if (_recentlyUpdated != value) { _recentlyUpdated = value; OnPropertyChanged(nameof(RecentlyUpdated)); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
