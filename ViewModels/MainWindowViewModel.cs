using bg3_loca_text.Core;
using bg3_loca_text.Services;
using System.ComponentModel.DataAnnotations;

namespace bg3_loca_text.ViewModels
{
	internal class MainWindowViewModel([Required] IStaticDataLoader _staticDataLoader) : ObservableObject
	{
		#region Properties
		private bool _isEscapedModeEnabled;

		public bool IsEscapedModeEnabled
		{
			get { return _isEscapedModeEnabled; }
			set
			{
				_isEscapedModeEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isLineBreakModeEnabled;

		public bool IsLineBreakModeEnabled
		{
			get { return _isLineBreakModeEnabled; }
			set
			{
				_isLineBreakModeEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool _isStyleModeEnabled;

		public bool IsStyleModeEnabled
		{
			get { return _isStyleModeEnabled; }
			set
			{
				_isStyleModeEnabled = value;
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

		private int _caretIndex;

		public int CaretIndex
		{
			get { return _caretIndex; }
			set
			{
				_caretIndex = value;
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
		#endregion

		#region Commands
		public RelayCommand AddDescription => new(ExecuteAddDescription);

		private void ExecuteAddDescription(object? obj)
		{
			throw new NotImplementedException();
		}

		public RelayCommand AddLineBreak => new(ExecuteAddLineBreak);

		private void ExecuteAddLineBreak(object? obj)
		{
			throw new NotImplementedException();
		}

		public RelayCommand AddBoldSection => new(ExecuteAddBoldSection);

		private void ExecuteAddBoldSection(object? obj)
		{
			throw new NotImplementedException();
		}

		public RelayCommand AddItalicSection => new(ExecuteAddItalicSection);

		private void ExecuteAddItalicSection(object? obj)
		{
			throw new NotImplementedException();
		}

		public RelayCommand AddStatLSTag => new(ExecuteAddStatLSTag);

		private void ExecuteAddStatLSTag(object? obj)
		{
			throw new NotImplementedException();
		}

		public RelayCommand AddGeneralLSTag => new(ExecuteAddGeneralLSTag);

		private void ExecuteAddGeneralLSTag(object? obj)
		{
			throw new NotImplementedException();
		}

		public RelayCommand AddImageLSTag => new(ExecuteAddImageLSTag);

		private void ExecuteAddImageLSTag(object? obj)
		{
			throw new NotImplementedException();
		}

		public RelayCommand CopyLocaText => new(ExecuteCopyLocaText);

		private void ExecuteCopyLocaText(object? obj)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
