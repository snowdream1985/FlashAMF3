using flash.net;
using System;

namespace FlashFx.IO.Writers
{
    public class AMF0ArrayWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteArray(ObjectEncoding.AMF0, (Array)data);
        }
    }
}
