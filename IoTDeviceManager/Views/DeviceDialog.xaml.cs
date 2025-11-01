using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace IoTDeviceManager.Views
{
    public partial class DeviceDialog : Window
    {
        public DeviceDialog()
        {
            InitializeComponent();

            // Add soft shadow
            Resources["DialogShadow"] = new DropShadowEffect
            {
                Color = Colors.Black,
                Opacity = 0.6,
                BlurRadius = 20,
                Direction = 270,
                ShadowDepth = 4
            };

            Loaded += (_, __) => NameBox.Focus();
        }

        private void OnConfirmClick(object sender, RoutedEventArgs e)
        {
            dynamic ctx = DataContext;
            string name = (string)(ctx?.Name ?? "");
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(this, "Please enter a device name.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                NameBox.Focus();
                return;
            }

            DialogResult = true;
        }
    }
}
