namespace MultiCalculator.Models
{
	public class PluginPackage
	{
		public string Name { get; set; } = string.Empty;

		public List<PluginButton> Buttons { get; set; } = [];
	}
}
