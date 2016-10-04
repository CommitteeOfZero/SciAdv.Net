using System;
using System.IO;

namespace SciAdvNet.Vfs
{
    public sealed class SubReadStream : Stream
    {
        private readonly Stream _superStream;
        private readonly long _startInSuperStream;
        private readonly long _endInSuperStream;
        private long _positionInSuperStream;
        private bool _canRead;

        public SubReadStream(Stream superStream, long startPosition, long length)
        {
            _superStream = superStream;
            _startInSuperStream = startPosition;
            _endInSuperStream = startPosition + length;
            _positionInSuperStream = startPosition;
            _canRead = true;
        }

        public override bool CanRead => _superStream.CanRead && _canRead;
        public override bool CanSeek => false;
        public override bool CanWrite => false;

        public override long Length => _endInSuperStream - _startInSuperStream;
        public override long Position
        {
            get { return _positionInSuperStream; }
            set { throw new NotSupportedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (_superStream.Position != _positionInSuperStream)
            {
                _superStream.Position = _positionInSuperStream;
            }
            if (_positionInSuperStream + count > _endInSuperStream)
            {
                count = (int)(_endInSuperStream - _positionInSuperStream);
            }

            int result = _superStream.Read(buffer, offset, count);
            _positionInSuperStream += result;
            return result;
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _canRead = false;
            }

            base.Dispose(disposing);
        }
    }
}
