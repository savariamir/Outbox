using System;

namespace Anshan.Domain.Exceptions
{
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}