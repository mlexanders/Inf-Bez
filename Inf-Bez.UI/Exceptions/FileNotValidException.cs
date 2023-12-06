namespace InfBez.Ui.Exceptions
{
    public class FileNotValidException : Exception
    {
        public FileNotValidException(string? message) : base(message)
        {
        }

        public FileNotValidException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
