using System;
using System.IO;

namespace SciAdvNet.Vfs
{
    public sealed class WrappedStream : Stream
    {
        private readonly Stream _baseStream;
        private bool _isDisposed;

        public WrappedStream(Stream baseStream)
        {
            _baseStream = baseStream;
        }

        public override bool CanRead => !_isDisposed && _baseStream.CanRead;
        public override bool CanSeek => !_isDisposed && _baseStream.CanSeek;
        public override bool CanWrite => !_isDisposed && _baseStream.CanWrite;
        public override long Length => _baseStream.Length;

        public override long Position
        {
            get => _baseStream.Position;
            set => _baseStream.Position = value;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().ToString());
            }
        }

        public override void Flush()
        {
            ThrowIfDisposed();
            _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfDisposed();
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            ThrowIfDisposed();
            _baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            _baseStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
