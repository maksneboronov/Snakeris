using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Snakeris
{
	public interface IBlock
	{
		int X { get; set; }
		int Y { get; set; }
		int Size { get; set; }
	}

	public interface ICollidable
	{
		bool IsCollide(BlockViewModel rblock, Direction direction);
		bool IsCollideWithWall(int width, int height, Direction direction);
	}

	public interface IEdible
	{
		bool IsEaten(BlockViewModel figures);
	}

	public interface INextable
	{
		BlockViewModel Next { get; set; }
	}

	public interface IForEachable : INextable
	{
		void ForEach(Action<IBlock> action);
		bool Any(Func<IBlock, bool> func);
	}

	public interface IColorable
	{
		Color Fill { get; set; }
	}
}
