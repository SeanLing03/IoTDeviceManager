using System.Windows;
using System.Windows.Input;
using IoTDeviceManager.ViewModels;
using IoTDeviceManager.Services;

namespace IoTDeviceManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Pass DialogService explicitly (makes unit testing easy later)
            this.DataContext = new MainViewModel(new DialogService());
            (this.DataContext as MainViewModel)?.EnsureSeeds();
        }

        // Hook DataGrid MouseDoubleClick to open Update dialog
        private void DevicesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            if (vm?.UpdateCommand.CanExecute(null) == true)
                vm.UpdateCommand.Execute(null);
        }
    }
}
