using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MSweeper.Controls
{
    public partial class TimeClock : UserControl
    {
        private int time = 0;

        private readonly DispatcherTimer Timer;

        public TimeClock()
        {
            InitializeComponent();

            Clock.Text = "00:00";

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
        }

        public void Start()
        {
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
        }

        public void Reset()
        {
            Timer.Stop();
            time = 0;
            Clock.Text = "00:00";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            time++;

            String mask = "00";
            Clock.Text = String.Format("{0}:{1}",
                (time / 60).ToString(mask),
                (time % 60).ToString(mask));
        }
    }
}
