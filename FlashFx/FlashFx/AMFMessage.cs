using System.Collections.Generic;
using System;
using flash.net;
using FlashFx.IO;

namespace flash.utils
{
    public class AMFMessage
    {
        protected ushort _version = 0;

        protected List<AMFBody> _bodies;

        protected List<AMFHeader> _headers;

        public AMFMessage()
            : this(0)
        {

        }

        public AMFMessage(ushort version)
        {
            _version = version;

            _headers = new List<AMFHeader>(1);

            _bodies = new List<AMFBody>(1);
        }

        public ushort Version { get { return _version; } }

        public void AddBody(AMFBody body)
        {
            _bodies.Add(body);
        }

        public void AddHeader(AMFHeader header)
        {
            _headers.Add(header);
        }

        public int BodyCount { get { return _bodies.Count; } }

        public System.Collections.ObjectModel.ReadOnlyCollection<AMFBody> Bodies { get { return _bodies.AsReadOnly(); } }

        public int HeaderCount { get { return _headers.Count; } }

        public AMFBody GetBodyAt(int index)
        {
            return _bodies[index] as AMFBody;
        }

        public AMFHeader GetHeaderAt(int index)
        {
            return _headers[index] as AMFHeader;
        }

        public AMFHeader GetHeader(string header)
        {
            for (int i = 0; _headers != null && i < _headers.Count; i++)
            {
                AMFHeader amfHeader = _headers[i] as AMFHeader;

                if (amfHeader.Name == header)
                {
                    return amfHeader;
                }
            }

            return null;
        }

        public void RemoveHeader(string header)
        {
            for (int i = 0; _headers != null && i < _headers.Count; i++)
            {
                AMFHeader amfHeader = _headers[i] as AMFHeader;

                if (amfHeader.Name == header)
                {
                    _headers.RemoveAt(i);
                }
            }
        }

        public ObjectEncoding ObjectEncoding
        {
            get
            {
                if (_version == 0 || _version == 1)
                {
                    return ObjectEncoding.AMF0;
                }
                else if (_version == 3)
                {
                    return ObjectEncoding.AMF3;
                }
                else
                {
                    throw new Exception("ObjectEncoding version is not 0 1 or 3");
                }
            }
        }
    }
}
