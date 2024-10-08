﻿using MultiCalculator.Helpers;
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

        public void SettingsControls_Loaded(object sender, RoutedEventArgs e)
        {
            var savedUsername = ((App)Application.Current).Username ?? string.Empty;
			UsernameInput.Text = savedUsername;
		}

		private void ThemeChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedTheme = (string)ThemeChoice.SelectedItem;
            SettingsHelper.ChangeTheme(selectedTheme);
        }

		private void UsernameInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			var newUsername = (string)UsernameInput.Text;
			SettingsHelper.ChangeUsername(newUsername);
        }

		private void ImFeelingLucky_Click(object sender, RoutedEventArgs e)
		{
            var themesList = new Themes();
            var randomTheme = themesList.GetRandomTheme();
            ThemeChoice.SelectedItem = randomTheme;
            SettingsHelper.ChangeTheme(randomTheme);
		}
	}

	class Themes : ObservableCollection<string>
    {
        private readonly Random random = new Random();
        public Themes()
        {
            Add("Default");
            Add("Dark");
            Add("Miami");
            Add("Terra");
            Add("Strawberry");
            Add("Aokiji");
            Add("Akainu");
            Add("Kizaru");
            Add("Matrix");
            Add("Joker");
            Add("Spiderman");
            Add("Sandy");
            Add("Winter");
            Add("Pink");
        }

        public string GetRandomTheme()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("No themes!");
            }
            var index = random.Next(Count);
            return this[index];
        }
    }
}
