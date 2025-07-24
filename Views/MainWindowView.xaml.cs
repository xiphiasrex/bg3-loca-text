using bg3_loca_text.ViewModels;
using System.Windows;

namespace bg3_loca_text.Views
{
	/// <summary>
	/// Interaction logic for MainWindowView.xaml
	/// </summary>
	public partial class MainWindowView : Window, IMainWindowView
	{
		public MainWindowView()
		{
			MainWindowViewModel viewModel = new(App.Current.Services, this);
			DataContext = viewModel;
			InitializeComponent();
		}

		public int GetLocaTextCaretPosition()
		{
			return LocaTextBox.CaretIndex;
		}

		public void GetSelectedLocaText(out int position, out int length)
		{
			position = LocaTextBox.SelectionStart;
			length = LocaTextBox.SelectionLength;
		}

		public void SetSelectedLocaText(int position, int length)
		{
			LocaTextBox.Focus();
			LocaTextBox.Select(position, length);
		}
	}
}
