using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultiCalculator.Helpers
{
    class SettingsHelper
    {
        public static void ChangeUsername(string newUsername)
        {
			// Please put in some LINQ verification to ensure that username is unique
			((App)Application.Current).Username = newUsername;
		}
		
		public static void ChangeTheme(string selectedTheme)
        {
			switch (selectedTheme)
			{
				case "Dark":
					ChangeThemeByResourceDictionary("DarkTheme.xaml");
					break;
				case "Sandy":
					ChangeThemeByResourceDictionary("SandyTheme.xaml");
					break;
				case "Pink":
					ChangeThemeByResourceDictionary("PinkTheme.xaml");
					break;
				case "Winter":
					ChangeThemeByResourceDictionary("WinterTheme.xaml");
					break;
				case "Default":
				default:
					Application.Current.Resources.MergedDictionaries.Clear();
					break;
			}
		}

		private static void ChangeThemeByResourceDictionary(string themeResourceDictionary)
		{
			var themeDict = new ResourceDictionary
			{
				Source = new Uri($"pack://application:,,,/Themes/{themeResourceDictionary}", UriKind.Absolute)
			};

			Application.Current.Resources.MergedDictionaries.Clear();
			Application.Current.Resources.MergedDictionaries.Add(themeDict);
		}
	}
}
