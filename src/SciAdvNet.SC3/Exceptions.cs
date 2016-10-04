using System;
using System.IO;

namespace SciAdvNet.SC3
{
    internal static class ExceptionUtils
    {
        public static InvalidDataException SC3String_UnexpectedByte(byte b)
        {
            string message = $"Unexpected byte in an encoded string: {b:X2}";
            return new InvalidDataException(message);
        }

        public static StringDecodingFailedException StringDecodingFailed(int stringId, int stringOffset, Exception innerException)
        {
            string message = $"Unable to decode an SC3 string. ID in the string table: {stringId}, offset: {stringOffset}.\n";
            message = message + $"Details: {innerException.Message}";
            return new StringDecodingFailedException(message, stringId, stringOffset);
        }
    }

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

        public StringDecodingFailedException(string message, int stringId, int stringOffset)
            : base(message)
        {
            StringId = stringId;
            StringOffset = stringOffset;
        }

        public int StringId { get; }
        public int StringOffset { get; }
    }
}
