using MultiCalculator.Models;
using System.Collections.ObjectModel;

namespace MultiCalculator
{
	public class PluginManager
	{
		private static PluginManager? instance;
		private static readonly object _lock = new();
		public ObservableCollection<PluginPackage> PluginPackages { get; private set; }

		private PluginManager()
		{
			PluginPackages = [];
		}

		public static PluginManager Instance
		{
			//null check applied twice for thread safety :)
			get
			{
				if (instance == null)
				{
					lock (_lock)
					{
						instance ??= new PluginManager();
					}
				}
				return instance;
			}
		}
	}

}
