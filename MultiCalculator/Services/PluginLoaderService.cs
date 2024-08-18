using MultiCalculator.Abstractions;
using MultiCalculator.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
namespace MultiCalculator.Services
{
	public class PluginLoaderService
	{
		public static List<IToken> LoadPluginsFromFolder(string folderPath)
		{ 
			try
			{
				var plugins = new List<IToken>();

				var dllFiles = Directory.GetFiles(folderPath, "*.dll");

				foreach (var dll in dllFiles)
				{
					var assembly = Assembly.LoadFrom(dll);
					var types = assembly.GetTypes()
										.Where(t => typeof(IToken).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

					foreach (var type in types)
					{
						var plugin = (IToken?)Activator.CreateInstance(type);
						plugins.Add(plugin!);
					}
				}

				return plugins;
			}
			catch
			{
				throw new MultiCalculatorException("error loading dlls");
			}
		}

		public static List<IToken> LoadPluginsFromFile(string filePath)
		{
			try
			{
				var plugins = new List<IToken>();
				var assembly = Assembly.LoadFrom(filePath);
				var types = assembly.GetTypes()
									.Where(t => typeof(IToken).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

				foreach (var type in types)
				{
					var plugin = (IToken?)Activator.CreateInstance(type);
					plugins.Add(plugin!);
				}

				return plugins;
			}
			catch
			{
				throw new MultiCalculatorException("error loading dll");
			}
		}
	}
}
