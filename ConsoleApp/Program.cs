using System;
using System.Threading;
using PomodoroLib;

namespace ConsoleApp
{
    class Program
    {
		private static PomodoroTimer pomodoroTimer;

		static void Main(string[] args)
		{
			pomodoroTimer = new PomodoroTimer(5000, 2000);
			pomodoroTimer.TickHandler += pomodoro_Tick;
			pomodoroTimer.Start();

			do
			{
				Thread.Sleep(1000);
			} while (pomodoroTimer.PhaseRemainingTime > 0);
		}


		static void pomodoro_Tick
		(
			object sender,
			EventArgs e
		)
		{
			Console.Clear();
			Console.Write("{0} {1}",
							  pomodoroTimer.PomodoroPhase.ToString(),
							  pomodoroTimer.PhaseRemainingTime.ToString());
		}
    }
}
