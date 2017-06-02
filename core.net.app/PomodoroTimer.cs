using System;
using System.Threading;


namespace PomodoroLib
{
    /// <summary>
    /// Constructor
    /// </summary>
    public class PomodoroTimer
    {
		public enum PomodoroPhaseEnum { Work, Break }

		public PomodoroPhaseEnum PomodoroPhase;

        private Timer _timer;
        private int _interval;

        public int WorkTime { get; set; }
        public int BreakTime { get; set; }
        public int PhaseRemainingTime { get; set; }
        public int Interval { get { return _interval; } }

        public event EventHandler TickHandler;
        public event EventHandler PhaseChangeHandler;



        /// <summary>
        /// Initializes a new instance of the <see cref="T:PomodoroLib.Pomodoro"/> class.
        /// </summary>
        /// <param name="WorkTime">Work time in milliseconds</param>
        /// <param name="BreakTime">Break time in milliseconds</param>
        /// <remarks>Interval defaults to 1000 milliseconds</remarks>
        public PomodoroTimer(int WorkTime, int BreakTime)
            : this(WorkTime, BreakTime, 1000)
        {

		}


        public PomodoroTimer(int WorkTime, int BreakTime, int Interval)
        {
			PomodoroPhase = PomodoroPhaseEnum.Work;
			this.PhaseRemainingTime = WorkTime;

			var autoEvent = new AutoResetEvent(false);

			this.WorkTime = WorkTime;
			this.BreakTime = BreakTime;
            _interval = Interval;

			_timer = new Timer(timerCallback, autoEvent, Timeout.Infinite, _interval);
        }

        /// <summary>
        /// Timers the callback.
        /// </summary>
        /// <param name="stateInfo">State info.</param>
        private void timerCallback(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;

            checkPhase();
        }

        protected void OnTick(EventArgs e)
        {
            if (TickHandler != null)
                TickHandler(this, e);
        }

        protected void OnPhaseChange(EventArgs e)
        {
            if (PhaseChangeHandler != null)
                PhaseChangeHandler(this, e);
        }


        /// <summary>
        /// Counts down the timer and switches phase when necessary
        /// </summary>
        private void checkPhase()
        {
            OnTick(EventArgs.Empty);

            PhaseRemainingTime -= _interval;

            if (PhaseRemainingTime == 0)
            {
                switchPhase();
            }
        }

        /// <summary>
        /// Starts the Timer
        /// </summary>
        public void Start()
        {
            _timer.Change(0, _interval);
        }

        /// <summary>
        /// Pauses the Timer
        /// </summary>
		public void Stop()
        {
            _timer.Change(Timeout.Infinite, 0);
        }

        /// <summary>
        /// Switchs the phase from "Break" to "Work" or vice-versa
        /// </summary>
        private void switchPhase()
        {
            if (PomodoroPhase == PomodoroPhaseEnum.Work)
            {
                PomodoroPhase = PomodoroPhaseEnum.Break;
                PhaseRemainingTime = BreakTime;
            }
            else if (PomodoroPhase == PomodoroPhaseEnum.Break)
            {
                PomodoroPhase = PomodoroPhaseEnum.Work;
                PhaseRemainingTime = WorkTime;
            }

            OnPhaseChange(EventArgs.Empty);
        }
    }
}
