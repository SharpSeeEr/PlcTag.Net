using System;
using System.Collections.Generic;
using System.Text;

namespace PlcTag
{
    class TagGetValueException : Exception
    {
        public TagGetValueException()
        {
        }

        public TagGetValueException(string message) : base(message)
        {
        }

        public TagGetValueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
