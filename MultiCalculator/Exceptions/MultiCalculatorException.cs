namespace MultiCalculator.Exceptions
{
	class MultiCalculatorException : Exception
    {
        public MultiCalculatorException()
        {
        }

        public MultiCalculatorException(string message) : base(message)
        {
        }

        public MultiCalculatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
	}
}
