namespace CLI.Utilities.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException() : base()
        { }

        public InvalidInputException(string message) : 
            base("Invalid input: " + message) 
        { }

        public InvalidInputException(string message, Exception inner) :
            base("Invalid input" + message, inner) 
        { }
    }
}
