using System;
using Xunit;
using PomodoroLib;
using System.Threading;

namespace PomodoroTests
{
	public class TimerBehaviour
	{
        bool _timerHasTicked;
        bool _phaseHasChanged;

        const int _testInterval = 10;

        public TimerBehaviour()
        {
            _timerHasTicked = false;
            _phaseHasChanged = false;
        }

		[Fact]
		public void Constructor_Phase_Defaults_To_Work()
		{
			var pom = new PomodoroTimer(0, 0);
            Assert.Equal(PomodoroTimer.PomodoroPhaseEnum.Work, 
                         pom.PomodoroPhase);
		}

		[Fact]
		public void Constructor_Interval_Defaults_To_1000()
		{
			var pomTimer = new PomodoroTimer(0, 0);
			Assert.Equal(1000, pomTimer.Interval);
		}

		[Fact]
		public void Phase_Switches_Work_To_Break()
		{
            var workTime = 20;
			var breakTime = 20;

			var pomTimer = new PomodoroTimer(workTime, breakTime, _testInterval);

            pomTimer.Start();
            Thread.Sleep(workTime + 10);

			Assert.Equal(PomodoroTimer.PomodoroPhaseEnum.Break, 
                         pomTimer.PomodoroPhase);
		}

		[Fact]
		public void Phase_Switches_Break_To_Work()
		{
			var workTime = 20;
			var breakTime = 20;

			var pomTimer = new PomodoroTimer(workTime, breakTime, _testInterval);
            pomTimer.PomodoroPhase = PomodoroTimer.PomodoroPhaseEnum.Break;

			pomTimer.Start();
			Thread.Sleep(breakTime + 10);
            pomTimer.Stop();

			Assert.Equal(PomodoroTimer.PomodoroPhaseEnum.Work, 
                         pomTimer.PomodoroPhase);
		}

        [Fact]
        public void Timer_Counts_Down()
        {
            var countdownTime = 20;

            var pomTimer = new PomodoroTimer(countdownTime, 0, _testInterval);
            pomTimer.Start();
            Thread.Sleep(countdownTime);
            pomTimer.Stop();

            Assert.Equal(0, pomTimer.PhaseRemainingTime);
   		}

		[Fact]
		public void Timer_Stops()
		{
			var countdownTime = 20;
            var timeRemaining = 10;

            var pomTimer = new PomodoroTimer(countdownTime, 10, _testInterval);
            pomTimer.Start();
            Thread.Sleep(10);
            pomTimer.Stop();
			Thread.Sleep(20);

   			Assert.Equal(timeRemaining, pomTimer.PhaseRemainingTime);
		}

        [Fact]
        public void Event_OnTick_Fires()
        {
			var pomTimer = new PomodoroTimer(20, 10, _testInterval);
            pomTimer.TickHandler += pomodoro_Tick;

            pomTimer.Start();
            Thread.Sleep(20);
            pomTimer.Stop();

            Assert.Equal(_timerHasTicked, true);
        }

		[Fact]
		public void Event_OnPhaseChange_Fires()
		{
			var pomTimer = new PomodoroTimer(10, 10, _testInterval);
            pomTimer.PhaseChangeHandler += pomodoro_PhaseChange;

			pomTimer.Start();
			Thread.Sleep(10);
			pomTimer.Stop();

			Assert.Equal(_phaseHasChanged, true);
		}

        /// <summary>
        /// For testing that OnTick Event fires.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void pomodoro_Tick
        (
			object sender,
			EventArgs e
		)
        {
            _timerHasTicked = true;
        }

		/// <summary>
		/// For testing that OnTick Event fires.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void pomodoro_PhaseChange
		(
			object sender,
			EventArgs e
		)
		{
			_phaseHasChanged = true;
		}
    }
}
