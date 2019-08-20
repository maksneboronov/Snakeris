using System;

namespace Snakeris
{
	public class GameRandomService : IRandomService
	{
		public GameRandomService(int blockWidth, int toX, int toY)
		{
			_toX = toX - 1;
			_toY = toY - 1;
			_blockWidth = blockWidth;
		}

		public int GetRandomX() => _rand.Next(0, _toX) * _blockWidth;
		public int GetRandomY() => _rand.Next(0, _toY) * _blockWidth;

		private Random _rand = new Random();
		private readonly int _blockWidth;
		private readonly int _toX, _toY;
	}
}
