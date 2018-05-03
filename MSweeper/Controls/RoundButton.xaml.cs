using System;
using System.Windows;
using System.Windows.Controls;

namespace MSweeper.Controls
{
    public partial class RoundButton : UserControl
    {
        public RoundButton()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        #region IconPath Property

        public String IconPath
        {
            get { return (String)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        public static readonly DependencyProperty IconPathProperty =
            DependencyProperty.Register("IconPath", typeof(String), typeof(RoundButton));

        #endregion

        #region Tap Event

        public event RoutedEventHandler Tap
        {
            add { AddHandler(TapEvent, value); }
            remove { RemoveHandler(TapEvent, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseTapEvent();
        }

        void RaiseTapEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(RoundButton.TapEvent);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent TapEvent = EventManager.RegisterRoutedEvent(
            "Tap", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RoundButton));

        #endregion
    }
}
