using IoTDeviceManager.Models;

namespace IoTDeviceManager.Services
{
    public interface IDialogService
    {
        bool ShowDeviceDialog(DeviceEditModel model);
    }
}
