using bg3_loca_text.Core;
using bg3_loca_text.Resources;
using bg3_loca_text.Services;
using bg3_loca_text.Views;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace bg3_loca_text.ViewModels
{
	internal class MainWindowViewModel([Required] IServiceProvider serviceProvider, IMainWindowView mainWindowView) : ObservableObject
	{
		private const string SELECTION_PLACEHOLDER = "{selection}";
		private readonly IStaticDataLoader _staticDataLoader = serviceProvider.GetRequiredService<IStaticDataLoader>();
		private readonly IMainWindowView _mainWindowView = mainWindowView;

		#region Properties
		public bool IsEscapedModeEnabled
		{
			get
			{ return UserSettings.Default.IsEscapedModeEnabled; }
			set
			{
				if (IsEscapedModeEnabled != value)
				{
					LocaText = HandleConvertLocaText(value);
				}
				UserSettings.Default.IsEscapedModeEnabled = value;
				UserSettings.Default.Save();
				OnPropertyChanged();
			}
		}

		public bool IsLineBreakModeEnabled
		{
			get { return UserSettings.Default.IsLineBreakModeEnabled; }
			set
			{
				UserSettings.Default.IsLineBreakModeEnabled = value;
				UserSettings.Default.Save();
				OnPropertyChanged();
			}
		}

		public bool IsStyleModeEnabled
		{
			get { return UserSettings.Default.IsStyleModeEnabled; }
			set
			{
				UserSettings.Default.IsStyleModeEnabled = value;
				UserSettings.Default.Save();
				OnPropertyChanged();
			}
		}

		private string? _statTooltipString;

		public string? StatTooltipString
		{
			get { return _statTooltipString; }
			set
			{
				_statTooltipString = value;
				OnPropertyChanged();
			}
		}

		private string? _locaText;

		public string? LocaText
		{
			get { return _locaText; }
			set
			{
				_locaText = value;
				OnPropertyChanged();
			}
		}

		private string? _selectedGenTooltip;

		public string? SelectedGenTooltip
		{
			get { return _selectedGenTooltip; }
			set
			{
				_selectedGenTooltip = value;
				OnPropertyChanged();
			}
		}

		private string? _selectedImageTooltip;

		public string? SelectedImageTooltip
		{
			get { return _selectedImageTooltip; }
			set
			{
				_selectedImageTooltip = value;
				OnPropertyChanged();
			}
		}

		private string? _selectedStatTooltip;

		public string? SelectedStatTooltip
		{
			get { return _selectedStatTooltip; }
			set
			{
				_selectedStatTooltip = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region ComboBox Options
		private List<string>? _generalTooltips;

		public List<string>? GeneralTooltips
		{
			get
			{
				_generalTooltips ??= _staticDataLoader.GetGeneralTooltips();
				return _generalTooltips;
			}
		}

		private List<string>? _imageTooltips;

		public List<string>? ImageTooltips
		{
			get
			{
				_imageTooltips ??= _staticDataLoader.GetImageTooltips();
				return _imageTooltips;
			}
		}

		private List<string>? _statTooltips;

		public List<string>? StatTooltips
		{
			get
			{
				_statTooltips ??= _staticDataLoader.GetStatTooltips();
				return _statTooltips;
			}
		}
		#endregion

		#region Commands
		public RelayCommand AddDescription => new(ExecuteAddDescription);

		private void ExecuteAddDescription(object? obj)
		{
			string textToAdd = "[1]";
			UpdateLocaText(textToAdd, 1, 1);
		}

		public RelayCommand AddLineBreak => new(ExecuteAddLineBreak);
		public RelayCommand AddLineBreakKey => new(ExecuteAddLineBreak, canExecute => IsLineBreakModeEnabled);

		private void ExecuteAddLineBreak(object? obj)
		{
			string textToAdd = "<br>";
			UpdateLocaText(textToAdd, 4, 0);
		}

		public RelayCommand AddBoldSection => new(ExecuteAddBoldSection);
		public RelayCommand AddBoldSectionKey => new(ExecuteAddBoldSection, canExecute => IsStyleModeEnabled);

		private void ExecuteAddBoldSection(object? obj)
		{
			string textToAdd = $"<b>{SELECTION_PLACEHOLDER}</b>";
			ReplaceSelectedLocaText(textToAdd, 3);
		}

		public RelayCommand AddItalicSection => new(ExecuteAddItalicSection);
		public RelayCommand AddItalicSectionKey => new(ExecuteAddItalicSection, canExecute => IsStyleModeEnabled);

		private void ExecuteAddItalicSection(object? obj)
		{
			string textToAdd = $"<i>{SELECTION_PLACEHOLDER}</i>";
			ReplaceSelectedLocaText(textToAdd, 3);
		}

		public RelayCommand AddStatLSTag => new(
			ExecuteAddStatLSTag,
			canExecute => !string.IsNullOrEmpty(SelectedStatTooltip) && !string.IsNullOrEmpty(StatTooltipString)
		);

		private void ExecuteAddStatLSTag(object? obj)
		{
			string lsTagOpen = $"<LSTag Tooltip=\"{StatTooltipString}\" Type=\"{SelectedStatTooltip}\">";
			string lsTagClose = "</LSTag>";
			ReplaceSelectedLocaText(lsTagOpen + SELECTION_PLACEHOLDER + lsTagClose, lsTagOpen.Length);
		}

		public RelayCommand AddGeneralLSTag => new(
			ExecuteAddGeneralLSTag,
			canExecute => !string.IsNullOrEmpty(SelectedGenTooltip));

		private void ExecuteAddGeneralLSTag(object? obj)
		{
			string lsTagOpen = $"<LSTag Tooltip=\"{SelectedGenTooltip}\">";
			string lsTagClose = "</LSTag>";
			ReplaceSelectedLocaText(lsTagOpen + SELECTION_PLACEHOLDER + lsTagClose, lsTagOpen.Length);
		}

		public RelayCommand AddImageLSTag => new(
			ExecuteAddImageLSTag,
			canExecute => !string.IsNullOrEmpty(SelectedImageTooltip));

		private void ExecuteAddImageLSTag(object? obj)
		{
			string lsTagOpen = $"<LSTag Info=\"{SelectedImageTooltip}\" Type=\"Image\">";
			string lsTagClose = "</LSTag>";
			ReplaceSelectedLocaText(lsTagOpen + SELECTION_PLACEHOLDER + lsTagClose, lsTagOpen.Length);
		}

		public RelayCommand CopyLocaText => new(ExecuteCopyLocaText, IsLocaTextValid);

		private void ExecuteCopyLocaText(object? obj)
		{
			throw new NotImplementedException();
		}
		#endregion

		private bool IsLocaTextValid(object? arg)
		{
			string checkText = (IsEscapedModeEnabled ? HandleConvertLocaText(false) : LocaText) ?? "";

			int lt = 0;
			int gt = 0;
			foreach (char c in checkText)
			{
				if (c == '<') lt++;
				else if (c == '>') gt--;

				if (gt > lt || lt - gt > 1) return false;
			}

			return lt == gt;
		}

		private string HandleConvertLocaText(bool isEscapeMode)
		{
			if (isEscapeMode)
			{
				return (LocaText ?? "").Replace("<", "&lt;").Replace(">", "&gt;");
			}
			else
			{
				return (LocaText ?? "").Replace("&lt;", "<").Replace("&gt;", ">");
			}
		}

		private void UpdateLocaText(string insertionText, int selectionStart, int selectionLength)
		{
			LocaText ??= "";
			int index = _mainWindowView.GetLocaTextCaretPosition();
			LocaText = LocaText.Insert(index, insertionText);
			_mainWindowView.SetSelectedLocaText(index + selectionStart, selectionLength);
		}

		private void ReplaceSelectedLocaText(string insertionText, int selectionStart)
		{
			LocaText ??= "";
			_mainWindowView.GetSelectedLocaText(out int position, out int length);
			string selectedText = LocaText.Substring(position, length);
			string tempLoca = LocaText.Remove(position, length);
			insertionText = insertionText.Replace(SELECTION_PLACEHOLDER, selectedText);
			LocaText = tempLoca.Insert(position, insertionText);
			_mainWindowView.SetSelectedLocaText(position + selectionStart, selectedText.Length);
		}
	}
}
