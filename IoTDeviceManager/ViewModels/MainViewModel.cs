using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using IoTDeviceManager.Helpers;
using IoTDeviceManager.Models;
using IoTDeviceManager.Services;

namespace IoTDeviceManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly DeviceService _service;
        private readonly IDialogService _dialog;

        private Device? _selectedDevice;

        private RelayCommand AddCmd { get; }
        private RelayCommand UpdateCmd { get; }
        private RelayCommand DeleteCmd { get; }
        private RelayCommand ToggleStatusCmd { get; }
        private RelayCommand ClearLogsCmd { get; }
        private RelayCommand LoadMockCmd { get; }

        public ICommand AddCommand => AddCmd;
        public ICommand UpdateCommand => UpdateCmd;
        public ICommand DeleteCommand => DeleteCmd;
        public ICommand ToggleStatusCommand => ToggleStatusCmd;
        public ICommand ClearLogsCommand => ClearLogsCmd;

        public ObservableCollection<Device> Devices { get; } = new();
        public ObservableCollection<LogEntry> Logs { get; } = new();

        public Device? SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (_selectedDevice != value)
                {
                    _selectedDevice = value;
                    OnPropertyChanged(nameof(SelectedDevice));
                    UpdateCmd.RaiseCanExecuteChanged();
                    DeleteCmd.RaiseCanExecuteChanged();
                    ToggleStatusCmd.RaiseCanExecuteChanged();
                }
            }
        }

        public MainViewModel() : this(new DialogService()) { }

        public MainViewModel(IDialogService dialogService)
        {
            _dialog = dialogService;
            _service = new DeviceService();

            //  1) Telemetry results: just animate (no log here)
            _service.TelemetryReceived += (s, payload) =>
            {
                var (device, error) = payload;
                if (error == null)
                {
                    device.RecentlyUpdated = true;
                    var dt = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                    dt.Tick += (s2, e2) => { device.RecentlyUpdated = false; dt.Stop(); };
                    dt.Start();
                }
                // If error != null, no UI animation; logging is handled via LogProduced below.
            };

            //  2) Log lines authored by the communicator/backend
            _service.LogProduced += (s, log) =>
            {
                var (action, message) = log;
                Logs.Insert(0, new LogEntry { Action = action, Message = message });
            };

            AddCmd = new RelayCommand(_ => AddDevice(), _ => true);
            UpdateCmd = new RelayCommand(_ => UpdateDevice(), _ => SelectedDevice != null);
            DeleteCmd = new RelayCommand(_ => DeleteDevice(), _ => SelectedDevice != null);
            ToggleStatusCmd = new RelayCommand(_ => ToggleStatus(), _ => SelectedDevice != null);
            ClearLogsCmd = new RelayCommand(_ => Logs.Clear());
            LoadMockCmd = new RelayCommand(_ => EnsureSeeds());

            EnsureSeeds();
        }

        public void EnsureSeeds()
        {
            if (Devices.Count > 0) return;
            Devices.Clear();
            foreach (var d in _service.SeedDevices) Devices.Add(d);
            if (Devices.Count > 0) SelectedDevice = Devices[0];
        }

        private void AddDevice()
        {
            var edit = new DeviceEditModel
            {
                DialogTitle = "Add Device",
                Name = "",
                Type = "Sensor"
            };
            if (_dialog.ShowDeviceDialog(edit))
            {
                var d = new Device { Name = edit.Name.Trim(), Type = string.IsNullOrWhiteSpace(edit.Type) ? "Sensor" : edit.Type };
                Devices.Add(d);
                Logs.Insert(0, new LogEntry { Action = "Add", Message = $"Added {d.Name} ({d.Type})." });
                SelectedDevice = d;
            }
        }

        private void UpdateDevice()
        {
            if (SelectedDevice == null) return;

            var edit = new DeviceEditModel
            {
                DialogTitle = "Update Device",
                Name = SelectedDevice.Name,
                Type = SelectedDevice.Type
            };
            if (_dialog.ShowDeviceDialog(edit))
            {
                SelectedDevice.Name = edit.Name.Trim();
                SelectedDevice.Type = string.IsNullOrWhiteSpace(edit.Type) ? "Sensor" : edit.Type;
                Logs.Insert(0, new LogEntry { Action = "Update", Message = $"Updated {SelectedDevice.Name}." });
            }
        }

        private void DeleteDevice()
        {
            if (SelectedDevice == null) return;
            var name = SelectedDevice.Name;
            Devices.Remove(SelectedDevice);
            SelectedDevice = null;
            Logs.Insert(0, new LogEntry { Action = "Delete", Message = $"Deleted {name}." });
        }

        private void ToggleStatus()
        {
            if (SelectedDevice == null) return;
            SelectedDevice.IsOnline = !SelectedDevice.IsOnline;
            Logs.Insert(0, new LogEntry { Action = "ToggleStatus", Message = $"{SelectedDevice.Name} is now {SelectedDevice.Status}." });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public void Dispose() => _service.Dispose();
    }
}
