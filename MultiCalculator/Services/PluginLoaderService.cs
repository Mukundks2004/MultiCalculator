using MultiCalculator.Abstractions;
using MultiCalculator.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultiCalculator.Services
{
	public class PluginLoader
	{
		public static List<IToken> LoadPlugins(string folderPath)
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
				throw new MultiCalculatorException("dlls contain incompatible code");
			}
		}
	}
}
