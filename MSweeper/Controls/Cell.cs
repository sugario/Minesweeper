using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MSweeper.Controls
{
    class Cell : Button
    {
        #region Fields

        public bool IsMine { get; set; }

        public bool IsFlag { get; set; }

        public int ID { get; set; }
 
        #endregion

        #region Ctor

        public Cell()
        {
            this.Content = " ";

            this.IsMine = false;
            this.IsFlag = false;
        }

        #endregion

        #region Left Click Routed Event

        public static readonly RoutedEvent CellLeftClickEvent =
            EventManager.RegisterRoutedEvent(
                "CellLeftClick",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Cell));

        public event RoutedEventHandler CellLeftClick
        {
            add { AddHandler(CellLeftClickEvent, value); }
            remove { RemoveHandler(CellLeftClickEvent, value); }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(Cell.CellLeftClickEvent));

            base.OnMouseLeftButtonUp(e);
        }

        #endregion

        #region Right Click Routed Event

        public static readonly RoutedEvent CellRightClickEvent =
            EventManager.RegisterRoutedEvent(
                "CellRightClick",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Cell));

        public event RoutedEventHandler CellRightClick
        {
            add { AddHandler(CellRightClickEvent, value); }
            remove { RemoveHandler(CellRightClickEvent, value); }
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(Cell.CellRightClickEvent));

            base.OnMouseRightButtonUp(e);
        }

        #endregion

        #region Text/Ico events
        
        public void TriggerPrimaryExplosion()
        {
            this.IsEnabled = false;

            this.Content = new Image
            {
                Source = new BitmapImage(new Uri(IcoPath.TriggeredMine)),
            };
        }

        public void TriggerSecondaryExplosion()
        {
            this.Content = new Image
            {
                Source = new BitmapImage(new Uri(IcoPath.Mine)),
            };
        }

        public void RaiseFlag()
        {
            this.IsFlag = true;

            this.Content = new Image
            {
                Source = new BitmapImage(new Uri(IcoPath.Flag)),
            };
        }

        public void DropFlag()
        {
            this.IsFlag = false;

            this.Content = " ";
        }

        public void ChangeContentToNumber(int number)
        {
            this.IsEnabled = false;

            if (number == 0)
            {
                this.Content = " ";
                return;
            }

            this.Content = number.ToString();
        }

        public void MarkWrongFlag()
        {
            Grid grid = new Grid();
            this.Content = grid;

            Image flag = new Image
            {
                Source = new BitmapImage(new Uri(IcoPath.Flag)),
                VerticalAlignment = VerticalAlignment.Center
            };

            Image cross = new Image
            {
                Source = new BitmapImage(new Uri(IcoPath.X)),
                VerticalAlignment = VerticalAlignment.Center
            };

            grid.Children.Add(flag);
            grid.Children.Add(cross);
        }

        #endregion 
    }
}
