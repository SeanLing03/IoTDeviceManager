using IoTDeviceManager.Models;
using IoTDeviceManager.Views;

namespace IoTDeviceManager.Services
{
    public class DialogService : IDialogService
    {
        public bool ShowDeviceDialog(DeviceEditModel model)
        {
            var win = new DeviceDialog { DataContext = model };
            win.Owner = System.Windows.Application.Current?.MainWindow;
            return win.ShowDialog() == true;
        }
    }
}
