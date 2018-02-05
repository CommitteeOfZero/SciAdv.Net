using System;

namespace SciAdvNet.SC3Script
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
}
