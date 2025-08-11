using bg3_loca_text.Core;
using bg3_loca_text.Resources;
using bg3_loca_text.Services;
using bg3_loca_text.Views;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace bg3_loca_text.ViewModels
{
	internal class MainWindowViewModel : ObservableObject
	{
		private const string SELECTION_PLACEHOLDER = "{selection}";
		private const string COMBOBOX_DEFAULT = "Select";
		private readonly IStaticDataLoader _staticDataLoader;
		private readonly IMainWindowView _mainWindowView;

		public MainWindowViewModel([Required] IServiceProvider serviceProvider, IMainWindowView mainWindowView)
		{
			_staticDataLoader = serviceProvider.GetRequiredService<IStaticDataLoader>();
			_mainWindowView = mainWindowView;
			SelectedStatTooltip = StatTooltips[0];
			SelectedGenTooltip = GeneralTooltips[0];
			SelectedImageTooltip = ImageTooltips[0];

			// Set up Filtered view for General Tooltips
			FilteredGeneralTooltips = CollectionViewSource.GetDefaultView(GeneralTooltips);
			FilteredGeneralTooltips.Filter = item =>
			{
				if (string.IsNullOrEmpty(GenTooltipSearchText) || COMBOBOX_DEFAULT.Equals(GenTooltipSearchText))
				{
					return true;
				}
				return ((string)item).Contains(GenTooltipSearchText, StringComparison.OrdinalIgnoreCase);
			};
			GenTooltipSearchText = COMBOBOX_DEFAULT;

			// Set up Filtered view for Image Tooltips
			FilteredImageTooltips = CollectionViewSource.GetDefaultView(ImageTooltips);
			FilteredImageTooltips.Filter = item =>
			{
				if (string.IsNullOrEmpty(ImageTooltipSearchText) || COMBOBOX_DEFAULT.Equals(ImageTooltipSearchText))
				{
					return true;
				}
				return ((ComboBoxIconItemData)item).Name.Contains(ImageTooltipSearchText, StringComparison.OrdinalIgnoreCase);
			};
			ImageTooltipSearchText = COMBOBOX_DEFAULT;
		}

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

		public string LocaText
		{
			get { return _locaText ?? ""; }
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

		private string? _genTooltipSearchText;

		public string? GenTooltipSearchText
		{
			get { return _genTooltipSearchText; }
			set
			{
				_genTooltipSearchText = value;
				OnPropertyChanged();
				FilteredGeneralTooltips.Refresh();

				if (value == null || GeneralTooltips.Any(item => string.Equals(item, value, StringComparison.OrdinalIgnoreCase)))
				{
					_selectedGenTooltip =
						GeneralTooltips.FirstOrDefault(item => string.Equals(item, value, StringComparison.OrdinalIgnoreCase));
				}
				else
				{
					_selectedGenTooltip = null;
				}

				_mainWindowView.FocusGenTooltip();
			}
		}

		private ComboBoxIconItemData? _selectedImageTooltip;

		public ComboBoxIconItemData? SelectedImageTooltip
		{
			get { return _selectedImageTooltip; }
			set
			{
				_selectedImageTooltip = value;
				OnPropertyChanged();
			}
		}

		private string? _imageTooltipSearchText;

		public string? ImageTooltipSearchText
		{
			get { return _imageTooltipSearchText; }
			set
			{
				_imageTooltipSearchText = value;
				OnPropertyChanged();
				FilteredImageTooltips.Refresh();

				if (value == null || ImageTooltips.Any(item => string.Equals(item.Name, value, StringComparison.OrdinalIgnoreCase)))
				{
					SelectedImageTooltip =
						ImageTooltips.FirstOrDefault(item => string.Equals(item.Name, value, StringComparison.OrdinalIgnoreCase));
				}
				else
				{
					SelectedImageTooltip = null;
				}

				_mainWindowView.FocusImageTooltip();
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

		public List<string> GeneralTooltips
		{
			get
			{
				_generalTooltips ??= _staticDataLoader.GetGeneralTooltips();
				return _generalTooltips;
			}
		}

		public ICollectionView FilteredGeneralTooltips { get; private set; }

		private List<ComboBoxIconItemData>? _imageTooltips;

		public List<ComboBoxIconItemData> ImageTooltips
		{
			get
			{
				if (_imageTooltips == null)
				{
					List<string> iconNames = _staticDataLoader.GetImageTooltips();

					_imageTooltips = [.. iconNames.Select(iconName => {
						BitmapImage myBitmapImage = new();
						myBitmapImage.BeginInit();
						myBitmapImage.UriSource = new Uri($"Resources/lstag_icons/INFO_{iconName}.png", UriKind.Relative);
						myBitmapImage.DecodePixelWidth = 32;
						myBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
						myBitmapImage.EndInit();

						return new ComboBoxIconItemData() {
							Name = iconName,
							Icon = myBitmapImage
						};
					})];
				}

				return _imageTooltips;
			}
		}

		public ICollectionView FilteredImageTooltips { get; private set; }

		private List<string>? _statTooltips;

		public List<string> StatTooltips
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

			if (IsEscapedModeEnabled)
			{
				textToAdd = LocaTextUtils.ConvertText(textToAdd);
			}

			UpdateLocaText(textToAdd, textToAdd.Length, 0);
		}

		public RelayCommand AddBoldSection => new(ExecuteAddBoldSection);
		public RelayCommand AddBoldSectionKey => new(ExecuteAddBoldSection, canExecute => IsStyleModeEnabled);

		private void ExecuteAddBoldSection(object? obj)
		{
			string textToAdd = $"<b>{SELECTION_PLACEHOLDER}</b>";

			if (IsEscapedModeEnabled)
			{
				textToAdd = LocaTextUtils.ConvertText(textToAdd);
			}

			ReplaceSelectedLocaText(textToAdd, textToAdd.IndexOf(SELECTION_PLACEHOLDER));
		}

		public RelayCommand AddItalicSection => new(ExecuteAddItalicSection);
		public RelayCommand AddItalicSectionKey => new(ExecuteAddItalicSection, canExecute => IsStyleModeEnabled);

		private void ExecuteAddItalicSection(object? obj)
		{
			string textToAdd = $"<i>{SELECTION_PLACEHOLDER}</i>";

			if (IsEscapedModeEnabled)
			{
				textToAdd = LocaTextUtils.ConvertText(textToAdd);
			}

			ReplaceSelectedLocaText(textToAdd, textToAdd.IndexOf(SELECTION_PLACEHOLDER));
		}

		public RelayCommand AddStatLSTag => new(
			ExecuteAddStatLSTag,
			canExecute => !string.IsNullOrEmpty(SelectedStatTooltip) && !string.IsNullOrEmpty(StatTooltipString)
		);

		private void ExecuteAddStatLSTag(object? obj)
		{
			string lsTagOpen = $"<LSTag Type=\"{SelectedStatTooltip}\" Tooltip=\"{StatTooltipString}\">";
			string lsTagClose = "</LSTag>";

			if (IsEscapedModeEnabled)
			{
				lsTagOpen = LocaTextUtils.ConvertText(lsTagOpen);
				lsTagClose = LocaTextUtils.ConvertText(lsTagClose);
			}

			ReplaceSelectedLocaText(lsTagOpen + SELECTION_PLACEHOLDER + lsTagClose, lsTagOpen.Length);
		}

		public RelayCommand AddGeneralLSTag => new(
			ExecuteAddGeneralLSTag,
			canExecute => !string.IsNullOrEmpty(SelectedGenTooltip) && GeneralTooltips.Contains(SelectedGenTooltip));

		private void ExecuteAddGeneralLSTag(object? obj)
		{
			string lsTagOpen = $"<LSTag Tooltip=\"{SelectedGenTooltip}\">";
			string lsTagClose = "</LSTag>";

			if (IsEscapedModeEnabled)
			{
				lsTagOpen = LocaTextUtils.ConvertText(lsTagOpen);
				lsTagClose = LocaTextUtils.ConvertText(lsTagClose);
			}

			ReplaceSelectedLocaText(lsTagOpen + SELECTION_PLACEHOLDER + lsTagClose, lsTagOpen.Length);
		}

		public RelayCommand AddImageLSTag => new(
			ExecuteAddImageLSTag,
			canExecute => SelectedImageTooltip != null && !string.IsNullOrEmpty(SelectedImageTooltip.Name));

		private void ExecuteAddImageLSTag(object? obj)
		{
			string textToAdd = $"<LSTag Type=\"Image\" Info=\"{SelectedImageTooltip?.Name}\"/>";

			if (IsEscapedModeEnabled)
			{
				textToAdd = LocaTextUtils.ConvertText(textToAdd);
			}

			UpdateLocaText(textToAdd, textToAdd.Length, 0);
		}

		public RelayCommand CopyLocaText => new(ExecuteCopyLocaText, IsLocaTextValid);

		private void ExecuteCopyLocaText(object? obj)
		{
			Clipboard.SetText(LocaText);
		}

		public RelayCommand NavigateToGitHub => new(ExecuteNavigateToGitHub);

		private void ExecuteNavigateToGitHub(object? obj)
		{
			var destinationurl = "https://github.com/xiphiasrex/bg3-loca-text/";
			var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
			{
				UseShellExecute = true,
			};
			System.Diagnostics.Process.Start(sInfo);
		}
		#endregion

		#region private helpers
		private bool IsLocaTextValid(object? arg)
		{
			return LocaTextUtils.ValidateText(LocaText, IsEscapedModeEnabled);
		}

		private string HandleConvertLocaText(bool isEscapeMode)
		{
			return LocaTextUtils.ConvertText(LocaText, isEscapeMode);
		}

		private void UpdateLocaText(string insertionText, int selectionStart, int selectionLength)
		{
			int index = _mainWindowView.GetLocaTextCaretPosition();
			LocaText = LocaText.Insert(index, insertionText);
			_mainWindowView.SetSelectedLocaText(index + selectionStart, selectionLength);
		}

		private void ReplaceSelectedLocaText(string insertionText, int selectionStart)
		{
			_mainWindowView.GetSelectedLocaText(out int position, out int length);
			string selectedText = LocaText.Substring(position, length);
			string tempLoca = LocaText.Remove(position, length);
			insertionText = insertionText.Replace(SELECTION_PLACEHOLDER, selectedText);
			LocaText = tempLoca.Insert(position, insertionText);
			_mainWindowView.SetSelectedLocaText(position + selectionStart, selectedText.Length);
		}
		#endregion
	}

	internal class ComboBoxIconItemData()
	{
		public required string Name { get; set; }
		public required ImageSource Icon { get; set; }
	}
}
