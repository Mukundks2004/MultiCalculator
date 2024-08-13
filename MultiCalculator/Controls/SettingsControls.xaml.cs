using System;
using System.Collections.Generic;
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
            ChangeTheme("DarkTheme.xaml");
        }

        void ChangeTheme(string themeResourceDictionary)
        {
            var themeDict = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/{themeResourceDictionary}", UriKind.Absolute)
            };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(themeDict);
        }
    }
}
