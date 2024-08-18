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
				case "Miami":
					ChangeThemeByResourceDictionary("MiamiTheme.xaml");
					break;
				case "Terra":
					ChangeThemeByResourceDictionary("TerraTheme.xaml");
					break;
				case "Strawberry":
					ChangeThemeByResourceDictionary("StrawberryTheme.xaml");
					break;
				case "Aokiji":
					ChangeThemeByResourceDictionary("AokijiTheme.xaml");
					break;
				case "Akainu":
					ChangeThemeByResourceDictionary("AkainuTheme.xaml");
					break;
				case "Kizaru":
					ChangeThemeByResourceDictionary("KizaruTheme.xaml");
					break;
				case "Matrix":
					ChangeThemeByResourceDictionary("MatrixTheme.xaml");
					break;
				case "Joker":
					ChangeThemeByResourceDictionary("JokerTheme.xaml");
					break;
				case "Spiderman":
					ChangeThemeByResourceDictionary("SpidermanTheme.xaml");
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
