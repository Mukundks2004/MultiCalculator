﻿using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MultiCalculator.Database;
using MultiCalculator.Extensions;
using MultiCalculator.Database.Services;
using MultiCalculator.Database.Repositories;
using MultiCalculator.Database.Models;
using MultiCalculator.Helpers;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public UserModel loggedUser { get; set; }
		readonly IDatabaseService _databaseService;

		public MainWindow(IDatabaseService databaseService)
		{
            InitializeComponent();
			_databaseService = databaseService;
			Icon = new BitmapImage(new Uri("pack://application:,,,./icon.png"));
		}

		void OpenWindowButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button button)
			{
				var tag = button.Tag as string;
				OpenWindow(tag);
			}
		}

		void OpenWindow(string? windowTag)
		{
			Window? windowToOpen = null;

			switch (windowTag)
			{
				case "ScientificCalculator":
					windowToOpen = new ScientificCalculatorWindow(_databaseService, loggedUser);
					break;
				case "Settings":
					windowToOpen = new SettingsWindow();
					break;
				case "PracticeProblems":
					windowToOpen = new PracticeProblemsWindow(_databaseService, loggedUser);
					break;
				case "PluginsWindow":
					windowToOpen = new PluginsWindow();
					break;
				case "Chatbot":
					windowToOpen = new ChatBotWindow(_databaseService, loggedUser);
					break;
				case "History":
					windowToOpen = new HistoryWindow(_databaseService, loggedUser);
					break;
				default:
					MessageBox.Show($"Unknown window tag: {windowTag ?? "Empty"}");
					break;
			}

			if (windowToOpen != null)
			{
				windowToOpen.Show();
			}
		}
	}
}