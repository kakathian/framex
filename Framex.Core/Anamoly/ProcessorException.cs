using System;
using System.Collections.Generic;
using System.Text;

namespace Framex.Core.Anamoly
{
    public class ProcessorException : Exception
    {
        public ProcessorException() :
            base()
        { }

        public ProcessorException(string message) :
            base(message)
        { }

        public ProcessorException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }
}
