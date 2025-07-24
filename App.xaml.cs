using bg3_loca_text.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace bg3_loca_text
{
	public partial class App : Application
	{
		public App()
		{
			Services = ConfigureServices();
		}

		public new static App Current => (App)Application.Current;

		public IServiceProvider Services { get; }

		private static ServiceProvider ConfigureServices()
		{
			var services = new ServiceCollection();

			services.AddScoped<IStaticDataLoader, StaticDataLoader>();

			return services.BuildServiceProvider();
		}
	}
}
