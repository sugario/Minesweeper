using System.Windows;

namespace MSweeper.Windows
{
    static class MenuDialogResult
    {
        public const int NewGame = 0;
        public const int Quit = 1;
        public const int Cancel = 2;
    }

    public partial class MenuWindow : Window
    {
        public int Result { get; set; }

        public MenuWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Result = MenuDialogResult.Cancel;
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            this.Result = MenuDialogResult.NewGame;
            this.Close();
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            this.Result = MenuDialogResult.Quit;
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }   
    }
}
