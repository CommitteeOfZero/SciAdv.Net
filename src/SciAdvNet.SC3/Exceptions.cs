using System;

namespace SciAdvNet.SC3
{
    public sealed class UnrecognizedInstructionException : Exception
    {
        public UnrecognizedInstructionException()
        {
        }

        public UnrecognizedInstructionException(string message)
            : base(message)
        {
        }

        public UnrecognizedInstructionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public sealed class StringEncodingFailedException : Exception
    {
        public StringEncodingFailedException()
        {
        }

        public StringEncodingFailedException(string message)
            : base(message)
        {
        }

        public StringEncodingFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public sealed class StringDecodingFailedException : Exception
    {
        public StringDecodingFailedException()
        {
        }

        public StringDecodingFailedException(string message)
            : base(message)
        {
        }

        public StringDecodingFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
