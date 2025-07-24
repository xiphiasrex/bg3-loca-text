namespace bg3_loca_text.Services
{
	interface IStaticDataLoader
	{
		List<string> GetGeneralTooltips(bool forceReload = false);
		List<string> GetImageTooltips(bool forceReload = false);
		List<string> GetStatTooltips(bool forceReload = false);
	}
}
