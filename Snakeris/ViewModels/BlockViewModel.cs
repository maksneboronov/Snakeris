using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MVVM.Core.NotifyProperyChanged;

namespace Snakeris
{
	public class BlockViewModel : NotifyPropertyChanged, IBlock, ICollidable, IForEachable, IColorable
	{
		public virtual int X { get => _x; set => this.UpdateProperty(value, ref _x); }
		public virtual int Y { get => _y; set => this.UpdateProperty(value, ref _y); }
		public virtual int Size { get => _size; set => this.UpdateProperty(value, ref _size); }
		public BlockViewModel Next { get => _next; set => this.UpdateProperty(value, ref _next); }

		#region IColor
		public Color Fill { get => _fill; set => this.UpdateProperty(value, ref _fill); }
		#endregion

		#region IForEachable
		public void ForEach(Action<IBlock> action)
		{
			var curr = this;
			while (curr != null)
			{
				action?.Invoke(curr);
				curr = curr.Next;
			}
		}

		public bool Any(Func<IBlock, bool> func)
		{
			var curr = this;
			while (curr != null)
			{
				if (func?.Invoke(curr) == true)
				{
					return true;
				}
				curr = curr.Next;
			}

			return false;
		}
		#endregion

		#region ICollidable
		public bool IsCollideWithWall(int width, int height, Direction direction)
		{
			(var x, var y) = GetOffsetTuple(direction, _size);
			return Any(i => i.X + x == width || i.Y + y == height || i.X + x < -1 || i.Y + y < -1);
		}

		public bool IsCollide(BlockViewModel rblock, Direction direction)
		{
			if (rblock == null)
			{
				throw new ArgumentNullException(nameof(rblock));
			}

			(var x, var y) = GetOffsetTuple(direction, _size);
			return this.Any(i => rblock.Any(j => i.X + x == j.X && i.Y + y == j.Y));
		}

		public static bool IsCollide(BlockViewModel block, List<BlockViewModel> blocks, Direction direction)
		{
			if (block == null)
			{
				throw new ArgumentNullException(nameof(block));
			}

			if (blocks == null)
			{
				throw new ArgumentNullException(nameof(blocks));
			}

			foreach (var item in blocks)
			{
				if (block.IsCollide(item, direction))
				{
					return true;
				}
			}

			return false;
		}
		#endregion

		public static (int, int) GetOffsetTuple(Direction direction, int size)
			=> (direction == Direction.Right ? size : (direction == Direction.Left ? -size : 0),
				direction == Direction.Down ? size : (direction == Direction.Up ? -size : 0));

		private int _x;
		private int _y;
		private int _size;
		private BlockViewModel _next;
		private Color _fill;
	}
}
