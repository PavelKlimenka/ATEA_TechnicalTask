namespace CLI.Utils.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : 
            base("Invalid input: " + message) { }
    }
}
