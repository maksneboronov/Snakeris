using System.Windows;
using MVVM.Core.RelayCommand;

namespace Snakeris
{

	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.DataContext = new SnakerisViewModel(new RelayCommandFactory(), new TimerService(), new BlockFactory(20, 20, 30));
			InitializeComponent();

		}
	}
}
