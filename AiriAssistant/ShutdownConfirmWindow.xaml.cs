using System.Windows;

namespace AiriAssistant
{
    public partial class ShutdownConfirmWindow : Window
    {
        public bool IsConfirmed { get; private set; }

        public ShutdownConfirmWindow()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            Close();
        }
    }
}