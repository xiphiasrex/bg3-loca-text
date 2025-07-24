namespace bg3_loca_text.Views
{
	internal interface IMainWindowView
	{
		public int GetLocaTextCaretPosition();
		public void SetSelectedLocaText(int position, int length);
		public void GetSelectedLocaText(out int position, out int length);
	}
}
