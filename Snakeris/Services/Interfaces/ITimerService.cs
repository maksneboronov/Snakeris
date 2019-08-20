using System;

namespace Snakeris
{
	public interface ITimerService
	{
		void Start();
		void Stop();
		void Create(TimeSpan time, EventHandler action);
		void ChangeTick(EventHandler oldTick, EventHandler newTick);

		event EventHandler Tick;
	}
}
