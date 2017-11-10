using System;

namespace SciAdvNet.SC3Script.Text
{
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

        public StringDecodingFailedException(int position, string message)
            : base(message)
        {
            Position = position;
        }

        public StringDecodingFailedException(int position, string message, Exception innerException)
            : base(message, innerException)
        {
            Position = position;
        }

        public int Position { get; }
    }
}
