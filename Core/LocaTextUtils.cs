using System.Text;
using System.Text.RegularExpressions;

namespace bg3_loca_text.Core
{
	class LocaTextUtils
	{
		public static string ConvertText(string text, bool isEscapeMode = true)
		{
			StringBuilder sb;

			if (isEscapeMode)
			{
				// & needs converted first to avoid double converting
				text = Regex.Replace(text, "&(?:(?!(gt;|lt;|amp;)))", "&amp;");
				sb = new(text);
				sb.Replace("<", "&lt;");
				sb.Replace(">", "&gt;");
			}
			else
			{
				sb = new(text);
				sb.Replace("&lt;", "<");
				sb.Replace("&gt;", ">");
				sb.Replace("&amp;", "&");
			}

			return sb.ToString();
		}

		public static bool ValidateText(string text, bool isEscaped)
		{
			if (isEscaped)
			{
				text = ConvertText(text, false);
			}

			int lt = 0;
			int gt = 0;
			int i = 0;
			foreach (char c in text)
			{
				i++;
				if (c == '<') lt++;
				else if (c == '>') gt++;

				if (gt > lt || lt - gt > 1)
				{
					return false;
				}
			}

			return lt == gt;
		}
	}
}
