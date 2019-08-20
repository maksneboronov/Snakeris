namespace Snakeris
{
	public class AppleViewModel : BlockViewModel, IEdible
	{
		#region IEdible
		public bool IsEaten(BlockViewModel block)
			=> block.X + block.Size > X && block.X < X && block.Y + block.Size > Y && block.Y < Y;
		#endregion

		#region BlockViewModel
		public override int X { get => _x + _offset; set => this.UpdateProperty(value, ref _x); }
		public override int Y { get => _y + _offset; set => this.UpdateProperty(value, ref _y); }
		public int Offset { get => _offset; set => this.UpdateProperty(value, ref _offset); }
		#endregion

		private int _offset;
		private int _x;
		private int _y;
	}
}
