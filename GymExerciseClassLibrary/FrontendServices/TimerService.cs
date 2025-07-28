using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymExerciseClassLibrary.FrontendServices
{
    public class TimerService
    {
        public IDispatcherTimer Timer { get; set; }
        public Stopwatch Stopwatch { get; set; } = new Stopwatch();

        public TimerService()
        {
            Timer = Application.Current.Dispatcher.CreateTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(66);
            Stopwatch.Reset();
        }
    }
}
