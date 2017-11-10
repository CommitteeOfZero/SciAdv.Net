using System;

namespace SC3Tools
{
    public class ExtractingStringsFailed : Exception
    {
        public ExtractingStringsFailed()
        {
        }

        public ExtractingStringsFailed(string message) : base(message)
        {
        }

        public ExtractingStringsFailed(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class StringReplacementFailed : Exception
    {
        public StringReplacementFailed()
        {
        }

        public StringReplacementFailed(string message) : base(message)
        {
        }

        public StringReplacementFailed(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
