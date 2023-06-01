namespace Anshan.Domain.Exceptions
{
    public class ValidationFailedException : DomainException
    {
        public ValidationFailedException() : base("Validation failed")
        {
        }

        public ValidationFailedException(string message) : base(message)
        {
        }
    }
}