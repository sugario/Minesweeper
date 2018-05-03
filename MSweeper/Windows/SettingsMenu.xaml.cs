using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

using MSweeper.Controls;

namespace MSweeper.Windows
{
    static class SettingsDialogResult
    {
        public const int OK = 0;
        public const int Cancel = 1;
    }

    public partial class SettingsMenu : Window
    {
        #region Fields

        private readonly Settings settings;

        public int Result { get; set; }

        #endregion

        #region Ctor

        public SettingsMenu()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.Result = SettingsDialogResult.Cancel;

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            this.settings = mainWindow.Settings;

            TickDifficulty(settings.RowCount, settings.ColumnCount, settings.MineCount);
        }

        #endregion

        #region radioButton events

        private void TickDifficulty(int rows, int columns, int mines)
        {
            if (rows == 9 && columns == 9 && mines == 10)
            {
                rbBeginner.IsChecked = true;
            }
            else if (rows == 16 && columns == 16 && mines == 40)
            {
                rbIntermediate.IsChecked = true;
            }
            else if (rows == 30 && columns == 16 && mines == 99)
            {
                rbExpert.IsChecked = true;
            }
            else
            {
                rbCustom.IsChecked = true;
            }
        }

        private void rbBeginner_Checked(object sender, RoutedEventArgs e)
        {
            tbRows.Text = "9";
            tbRows.IsReadOnly = true;
            tbRows.Background = Brushes.LightGray;

            tbColumns.Text = "9";
            tbColumns.IsReadOnly = true;
            tbColumns.Background = Brushes.LightGray;

            tbMines.Text = "10";
            tbMines.IsReadOnly = true;
            tbMines.Background = Brushes.LightGray;
        }

        private void rbIntermediate_Checked(object sender, RoutedEventArgs e)
        {
            tbRows.Text = "16";
            tbRows.IsReadOnly = true;
            tbRows.Background = Brushes.LightGray;

            tbColumns.Text = "16";
            tbColumns.IsReadOnly = true;
            tbColumns.Background = Brushes.LightGray;

            tbMines.Text = "40";
            tbMines.IsReadOnly = true;
            tbMines.Background = Brushes.LightGray;
        }

        private void rbExpert_Checked(object sender, RoutedEventArgs e)
        {
            tbRows.Text = "30";
            tbRows.IsReadOnly = true;
            tbRows.Background = Brushes.LightGray;

            tbColumns.Text = "16";
            tbColumns.IsReadOnly = true;
            tbColumns.Background = Brushes.LightGray;

            tbMines.Text = "99";
            tbMines.IsReadOnly = true;
            tbMines.Background = Brushes.LightGray;
        }

        private void rbCustom_Checked(object sender, RoutedEventArgs e)
        {
            tbRows.Text = this.settings.RowCount.ToString();
            tbRows.IsReadOnly = false;
            tbRows.Background = Brushes.White;

            tbColumns.Text = this.settings.ColumnCount.ToString();
            tbColumns.IsReadOnly = false;
            tbColumns.Background = Brushes.White;

            tbMines.Text = this.settings.MineCount.ToString();
            tbMines.IsReadOnly = false;
            tbMines.Background = Brushes.White;
        }

        #endregion

        #region Button Events

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            CheckTextboxesForErrors();

            if(tbRows.BorderBrush != Brushes.Red
                && tbColumns.BorderBrush != Brushes.Red
                && tbMines.BorderBrush != Brushes.Red)
            {
                settings.RowCount = Convert.ToInt32(tbRows.Text);
                settings.ColumnCount = Convert.ToInt32(tbColumns.Text);
                settings.MineCount = Convert.ToInt32(tbMines.Text);

                var mainWindow = (MainWindow)Application.Current.MainWindow;

                mainWindow.Settings = this.settings;

                this.Result = SettingsDialogResult.OK;
                this.Close();
            }
        }

        #endregion

        #region Auxiliary functions

        private void CheckTextboxesForErrors()
        {
            IsAllDigit(tbRows);
            IsAllDigit(tbColumns);
            IsAllDigit(tbMines);

            if (tbRows.BorderBrush != Brushes.Red) IsWithinLimits(tbRows, 1, 40);
            if (tbColumns.BorderBrush != Brushes.Red) IsWithinLimits(tbColumns, 1, 40);

            if (tbRows.BorderBrush != Brushes.Red
                && tbColumns.BorderBrush != Brushes.Red
                && tbMines.BorderBrush != Brushes.Red)
            {
                IsWithinLimits(tbMines, 
                   (int)(Convert.ToInt32(tbRows.Text) * Convert.ToInt32(tbColumns.Text) * 0.02) + 1 , 
                   Convert.ToInt32(tbRows.Text) * Convert.ToInt32(tbColumns.Text) - 1);
            }
        }

        private void IsAllDigit(TextBox tb)
        {
            if (!tb.Text.All(char.IsDigit) || tb.Text.Count() > 9)
            {
                tb.BorderBrush = Brushes.Red;
            }
            else
            {
                tb.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffabadb3"));
            }
        }

        private void IsWithinLimits(TextBox tb, int lowerLimit, int higherLimit)
        {
            var val = Convert.ToInt32(tb.Text);
            if(val >= lowerLimit && val <= higherLimit)
            {
                tb.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffabadb3"));
            }
            else
            {
                tb.BorderBrush = Brushes.Red;
            }
        }

        #endregion
    }
}
