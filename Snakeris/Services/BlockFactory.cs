using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Snakeris
{
	public class BlockFactory : IBlockFactory
	{
		public BlockFactory(int blockSize, int width, int height)
			=> SetSquare(blockSize, width, height);

		public void SetSquare(int blockSize, int width, int height)
		{
			_blockSize = blockSize;
			_randService = new GameRandomService(blockSize, width, height);
		}

		public BlockViewModel GetRandomBlock()
			=> new BlockViewModel()
			{
				X = _randService.GetRandomX(),
				Y = _randService.GetRandomY(),
				Size = _blockSize,
				Fill = Color.FromRgb(127, 255, 212)
			};

		public BlockViewModel GetBlockFromSquare(int width, int height)
			=> new BlockViewModel()
			{
				X = width * _blockSize,
				Y = height * _blockSize,
				Size = _blockSize,
				Fill = Color.FromRgb(127, 255, 212)
			};

		public BlockViewModel GetNextBlock(BlockViewModel prev, int width, int height)
		{
			if (prev == null)
			{
				throw new ArgumentNullException(nameof(prev));
			}

			var newBlock = GetBlockFromSquare(width, height);
			prev.Next = newBlock;
			return newBlock;
		}

		public AppleViewModel GetRandomApple()
			=> new AppleViewModel()
			{
			Size = _blockSize / 2,
				Offset = _blockSize / 4,
				X = _randService.GetRandomX(),
				Y = _randService.GetRandomY()
			};

		public AppleViewModel GetApple(BlockViewModel head, List<BlockViewModel> fallenBlocks)
		{
			if (head == null)
			{
				throw new ArgumentNullException(nameof(head));
			}

			var apple = GetRandomApple();

			var curr = head;
			while (curr.Next != null)
			{
				var isCollide = apple.IsEaten(curr) || fallenBlocks.Any(i => apple.IsEaten(i));
				if (isCollide)
				{
					apple.X = _randService.GetRandomX();
					apple.Y = _randService.GetRandomY();
					curr = head;
				}

				curr = curr.Next;
			}

			return apple;
		}

		private IRandomService _randService;
		private int _blockSize;
	}
}
