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

        public TimerBehaviour()
        {
            _timerHasTicked = false;
            _phaseHasChanged = false;
        }

		[Fact]
		public void Phase_Defaults_To_Work()
		{
			var pom = new PomodoroTimer(0, 0);
            Assert.Equal(PomodoroTimer.PomodoroPhaseEnum.Work, 
                         pom.PomodoroPhase);
		}

		[Fact]
		public void Phase_Switches_Work_To_Break()
		{
            var workTime = 2000;
			var breakTime = 2000;

			var pom = new PomodoroTimer(workTime, breakTime);

            pom.Start();
            Thread.Sleep(workTime + 1000);

			Assert.Equal(PomodoroTimer.PomodoroPhaseEnum.Break, 
                         pom.PomodoroPhase);
		}

		[Fact]
		public void Phase_Switches_Break_To_Work()
		{
			var workTime = 2000;
			var breakTime = 2000;

			var pom = new PomodoroTimer(workTime, breakTime);
            pom.PomodoroPhase = PomodoroTimer.PomodoroPhaseEnum.Break;

			pom.Start();
			Thread.Sleep(breakTime + 1000);

			Assert.Equal(PomodoroTimer.PomodoroPhaseEnum.Work, 
                         pom.PomodoroPhase);
		}

        [Fact]
        public void Timer_Counts_Down()
        {
            var countdownTime = 2000;

            var pom = new PomodoroTimer(countdownTime, 0);
            pom.Start();
            Thread.Sleep(countdownTime);
            pom.Stop();

            Assert.Equal(0, pom.PhaseRemainingTime);
   		}

		[Fact]
		public void Timer_Stops()
		{
			var countdownTime = 2000;
            var timeRemaining = 1000;

            var pomTimer = new PomodoroTimer(countdownTime, 1000);
            pomTimer.Start();
            Thread.Sleep(1000);
            pomTimer.Stop();
			Thread.Sleep(2000);

   			Assert.Equal(timeRemaining, pomTimer.PhaseRemainingTime);
		}

        [Fact]
        public void Event_OnTick_Fires()
        {
			var pomTimer = new PomodoroTimer(20, 10);
            pomTimer.TickHandler += pomodoro_Tick;

            pomTimer.Start();
            Thread.Sleep(20);
            pomTimer.Stop();

            Assert.Equal(_timerHasTicked, true);
        }

		[Fact]
		public void Event_OnPhaseChange_Fires()
		{
			var pomTimer = new PomodoroTimer(1000, 1000);
            pomTimer.PhaseChangeHandler += pomodoro_PhaseChange;

			pomTimer.Start();
			Thread.Sleep(1000);
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
