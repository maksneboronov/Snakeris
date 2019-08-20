using System;
using System.Windows.Threading;

namespace Snakeris
{
	public class TimerService : ITimerService
	{
		public event EventHandler Tick { add => _timer.Tick += value;  remove => _timer.Tick -= value;  }

		public void ChangeTick(EventHandler oldTick, EventHandler newTick)
		{
			Stop();
			_timer.Tick -= oldTick;
			_timer.Tick += newTick;
			Start();
		}

		public void Create(TimeSpan time, EventHandler action)
		{
			if (time == null || action == null)
			{
				throw new ArgumentNullException();
			}

			_timer.Interval = time;
			_timer.Tick += action;
		}

		public void Start()
		{
			if (_timer.IsEnabled)
			{
				return;
			}

			_timer.Start();
		}

		public void Stop()
		{
			if (!_timer.IsEnabled)
			{
				return;
			}

			_timer.Stop();
		}

		private DispatcherTimer _timer = new DispatcherTimer();
	}
}
