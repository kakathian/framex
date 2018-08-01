using System;
using System.Net;

namespace Framex.Core
{
    public class ValidationException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public FramexError[] Errors { get; }

        public ValidationException(string message, FramexError[] errors)
            : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
            this.Errors = errors;
        }
    }
}
