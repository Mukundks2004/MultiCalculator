using MultiCalculator.Abstractions;

namespace MultiCalculator.Models
{
	public class PluginButton
	{
		public string Name { get; set; } = string.Empty;

		public int XPos { get; set; } = 0;

		public int YPos { get; set; } = 0;

		public int Width { get; set; } = 40;

		public int Height { get; set; } = 40;

		public string DllPath { get; set; } = string.Empty;

		public IToken? Token { get; set; } = null;
	}
}
