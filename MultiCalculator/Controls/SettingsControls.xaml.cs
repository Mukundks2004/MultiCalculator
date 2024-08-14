using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiCalculator.Controls
{
    /// <summary>
    /// Interaction logic for SettingsControls.xaml
    /// </summary>
    public partial class SettingsControls : UserControl
    {
        public SettingsControls()
        {
            InitializeComponent();
        }

		private void ThemeChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            var selectedTheme = (string)ThemeChoice.SelectedItem;
            switch (selectedTheme)
            {
                case "Dark":
                    ChangeTheme("DarkTheme.xaml");
                    break;
                case "Default":
                default:
                    Application.Current.Resources.MergedDictionaries.Clear();
                    break;
            }
        }

        void ChangeTheme(string themeResourceDictionary)
        {
            var themeDict = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/Themes/{themeResourceDictionary}", UriKind.Absolute)
            };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(themeDict);
        }
    }

    class Themes : ObservableCollection<string>
    {
        public Themes()
        {
            Add("Default");
            Add("Dark");
        }
    }
}
