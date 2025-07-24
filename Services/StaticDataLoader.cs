using System.IO;
using System.Text.Json;

namespace bg3_loca_text.Services
{
	class StaticDataLoader : IStaticDataLoader
	{
		private Dictionary<string, List<string>> _lstagData = [];

		public List<string> GetGeneralTooltips(bool forceReload = false)
		{
			if (forceReload || _lstagData.Count == 0)
			{
				LoadLSTagDataInternal();
			}

			return _lstagData["GeneralTooltips"];
		}

		public List<string> GetImageTooltips(bool forceReload = false)
		{
			if (forceReload || _lstagData.Count == 0)
			{
				LoadLSTagDataInternal();
			}

			return _lstagData["ImageTooltips"];
		}

		private void LoadLSTagDataInternal()
		{
			using FileStream stream = File.OpenRead(@"./Resources/LSTagData.json");
			_lstagData = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(stream) ?? [];
		}
	}
}
