using System.Collections.Generic;

namespace Snakeris
{
	public interface IBlockFactory
	{
		void SetSquare(int size, int width, int height);
		BlockViewModel GetRandomBlock();
		BlockViewModel GetBlockFromSquare(int width, int height);
		AppleViewModel GetApple(BlockViewModel head, List<BlockViewModel> fallenBlocks);
		BlockViewModel GetNextBlock(BlockViewModel prev, int width, int height);
	}
}
