using MultiCalculator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCreator
{
	public class Feigenbaum : INullaryOperation
	{
		public string LatexString { get; init; } = "\\delta";

		public Func<double> Calculate { get; init; } = () => 4.66920160910299;

		public string TokenSymbol { get; init; } = "δ";
	}
}
