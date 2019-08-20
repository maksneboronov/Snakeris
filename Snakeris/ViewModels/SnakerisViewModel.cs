using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using MVVM.Core.NotifyProperyChanged;
using MVVM.Core.RelayCommand;

namespace Snakeris
{
	public class SnakerisViewModel : NotifyPropertyChanged
	{
		public ICommand ChangeDirection { get; }
		public ICommand StartGame { get; }

		public ObservableCollection<BlockViewModel> Blocks { get; private set; } = new ObservableCollection<BlockViewModel>();
		public BlockViewModel Head { get; set; }
		public AppleViewModel Apple { get; set; }

		public int Size
		{
			get => _size;

			set
			{
				this.UpdateProperty(value, ref _size);
				Width = value * _blockWidth;
				Height = (value + 10) * _blockWidth;
				_blockFactory.SetSquare(_blockWidth, value, value + 10);
			}
		}
		public int BlockSize
		{
			get => _blockWidth;

			set
			{
				var prev = _blockWidth;
				this.UpdateProperty(value, ref _blockWidth);
				Size = (int)((_size * prev / value) );
				_blockFactory.SetSquare(_blockWidth, Size, Size + 10);
			}
		}
		[Raisable("WindowWidth")]
		public int Width { get => _width; set => this.UpdateProperty(value, ref _width); }
		[Raisable("WindowHeight")]
		public int Height { get => _height; set => this.UpdateProperty(value, ref _height); }

		public int WindowHeight => Height + 55;
		public int WindowWidth => Width + 45;

		public bool IsStart { get => _isStart; set => this.UpdateProperty(value, ref _isStart); }
		public int Points { get => _points; set => this.UpdateProperty(value, ref _points); }
		public int MaxPoints { get => _maxPoints; set => this.UpdateProperty(value, ref _maxPoints); }

		public SnakerisViewModel(ICommandFactory commFact, ITimerService timerServ, IBlockFactory blockFactory)
		{
			if (commFact == null)
			{
				throw new ArgumentNullException(nameof(commFact));
			}
			_blockFactory = blockFactory ?? throw new ArgumentNullException(nameof(blockFactory));
			_timerServ = timerServ ?? throw new ArgumentNullException(nameof(timerServ));
			_timerServ.Create(TimeSpan.FromMilliseconds(150), MovingTimer);

			ChangeDirection = commFact.CreateCommand<Direction>(OnChangeDirection, o => _isStart && _canChangeDirection);
			StartGame = commFact.CreateCommand(OnStart);

			Size = 40;
		}

		private void OnStart()
		{
			IsStart = !IsStart;

			if (IsStart)
			{
				Reset();
				_timerServ.Start();
			}
			else
			{
				Blocks.Clear();
				_fallenBlocks.Clear();
				_timerServ.Stop();

				if (Points > MaxPoints)
				{
					MaxPoints = Points;
				}

				Points = 0;
			}
		}

		private void Reset()
		{
			NewSnake();
			Apple = _blockFactory.GetApple(Head, _fallenBlocks);

			Blocks.Insert(0, Apple);

			if (_isFalling)
			{
				_timerServ.ChangeTick(FallingBlockTimer, MovingTimer);
				_isFalling = false;
			}
		}

		private void NewSnake()
		{
			_direction = Direction.Up;

			foreach (var block in Blocks.Where(i => i != null && !(i is AppleViewModel)))
			{
				_fallenBlocks.Add(block);
				block.Next = null;
			}

			Head = _blockFactory.GetBlockFromSquare(_size / 2, 5);
			var r2 = _blockFactory.GetNextBlock(Head, Head.X / _blockWidth, (Head.Y + _blockWidth) / _blockWidth);
			var r3 = _blockFactory.GetNextBlock(r2, r2.X / _blockWidth, (r2.Y + _blockWidth) / _blockWidth);
			var r4 = _blockFactory.GetNextBlock(r3, r3.X / _blockWidth, (r3.Y + _blockWidth) / _blockWidth);

			Blocks.Add(Head);
			Blocks.Add(r2);
			Blocks.Add(r3);
			Blocks.Add(r4);
		}

		private void OnChangeDirection(Direction newDirection)
		{
			if (_isFalling && (newDirection == Direction.Left || newDirection == Direction.Right))
			{
				var offset = (int)newDirection * _blockWidth;
				if (Head.IsCollideWithWall(Width, Height, newDirection) || BlockViewModel.IsCollide(Head, _fallenBlocks, newDirection))
				{
					return;
				}
				Head.ForEach(i => i.X += offset);
				return;
			}
			if ((int)newDirection * -1 != (int)_direction)
			{
				_direction = newDirection;
			}

			_canChangeDirection = false;
		}

		private void MovingTimer(object sender, EventArgs args)
		{
			if (Head.IsCollideWithWall(Width, Height, _direction))
			{
				OnStart();
				return;
			}

			int nextPosX = Head.X, nextPosY = Head.Y;
			(var x, var y) = BlockViewModel.GetOffsetTuple(_direction, _blockWidth);
			Head.Y += y;
			Head.X += x;

			Head.Next.ForEach(curr =>
			{
				var PosX = curr.X;
				curr.X = nextPosX;
				nextPosX = PosX;

				var PosY = curr.Y;
				curr.Y = nextPosY;
				nextPosY = PosY;
			});

			if (Apple.IsEaten(Head))
			{
				Points += 10;
				Blocks[0] = new AppleViewModel() { X = -100000, Y = -100000 };
				_isFalling = true;
				_direction = Direction.Down;
				Blocks.ForEach(i => { if (i != null) i.Fill = Color.FromRgb(35, 156, 2); });
				_timerServ.ChangeTick(MovingTimer, FallingBlockTimer);
			}

			_canChangeDirection = true;
		}

		private void FallingBlockTimer(object sender, EventArgs args)
		{
			_canChangeDirection = false;
			IsReset();
			Head.ForEach(i => i.Y += _blockWidth);
			_canChangeDirection = true;
		}

		private void IsReset()
		{
			if (!(Head.IsCollideWithWall(Width, Height, Direction.Down) || BlockViewModel.IsCollide(Head, _fallenBlocks, _direction)))
			{
				return;
			}

			for (var i = Height; i > 0; i -= Size)
			{
				var line = Blocks.Where(b => b.Y == i).ToList();
				if (line.Count() == Size)
				{
					while (line.Any())
					{
						Points += Size / 10;
						Blocks.Remove(line.First());
						line.Remove(line.First());
					}

					Blocks.Where(b => b.Y < i).ToList().ForEach(b => b.Y += _blockWidth);

					i += Size;
				}
			}

			Points += 4;
			NewSnake();
			Apple = _blockFactory.GetApple(Head, _fallenBlocks);
			Blocks[0] = Apple;
			Blocks.ForEach(i => i.Fill = Color.FromRgb(127, 255, 212));

			_isFalling = false;
			_timerServ.ChangeTick(FallingBlockTimer, MovingTimer);
		}

		private List<BlockViewModel> _fallenBlocks = new List<BlockViewModel>();
		private bool _canChangeDirection = true;
		private Direction _direction = Direction.Up;
		private bool _isFalling = false;
		private bool _isStart = false;
		private int _width;
		private int _height;
		private int _blockWidth = 10;
		private int _size = 20;
		private int _points = 0;
		private int _maxPoints = 0;

		private ITimerService _timerServ;
		private IBlockFactory _blockFactory;
	}
}
