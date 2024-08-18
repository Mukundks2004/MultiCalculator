using MultiCalculator.Abstractions;
using System.Windows;

namespace MultiCalculator.Delegates
{
	public delegate void EventHandlerWithToken(object sender, RoutedEventArgs e, IToken? t);
}
