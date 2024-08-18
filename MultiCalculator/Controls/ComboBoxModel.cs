using MultiCalculator.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MultiCalculator.Controls
{
	public class ComboBoxModel : INotifyPropertyChanged
	{
		public static ObservableCollection<string> Packages
		{
			get
			{
				return new ObservableCollection<string>(PluginManager.Instance.PluginPackages.Select(x => x.Name));
			}
		}

		public static string PackageDropDownText
		{
			get 
			{
				return "Packages1";
			}
			
			set { }
		}

		public ComboBoxModel()
		{
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
