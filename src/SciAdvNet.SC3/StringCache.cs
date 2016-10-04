using SciAdvNet.SC3.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SciAdvNet.SC3
{
    internal sealed class StringCache
    {
        private readonly SC3Module _module;
        private readonly SC3StringDecoder _decoder;
        private readonly Dictionary<StringHandle, SC3String> _strings = new Dictionary<StringHandle, SC3String>();

        public StringCache(SC3Module module)
        {
            _module = module;
            _decoder = new SC3StringDecoder(module.Game);
        }

        public SC3String GetString(StringHandle handle)
        {
            SC3String sc3String;
            if (_strings.TryGetValue(handle, out sc3String))
            {
                return sc3String;
            }

            try
            {
                sc3String = _decoder.DecodeString(handle.RawData.ToArray());
            }
            catch (InvalidDataException e)
            {
                throw ExceptionUtils.StringDecodingFailed(handle.Id, handle.Offset, e);
            }

            _strings[handle] = sc3String;
            return sc3String;
        }
    }
}
