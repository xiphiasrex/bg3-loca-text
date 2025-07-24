using bg3_loca_text.Services;
using bg3_loca_text.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace bg3_loca_text.Views
{
	/// <summary>
	/// Interaction logic for MainWindowView.xaml
	/// </summary>
	public partial class MainWindowView : Window
	{
		public MainWindowView()
		{
			MainWindowViewModel viewModel = new(App.Current.Services.GetRequiredService<IStaticDataLoader>());
			DataContext = viewModel;
			InitializeComponent();
		}
	}
}
