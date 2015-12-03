using flash.utils;
using FlashFx.AMF3;

namespace FlashFx.IO.Writers
{
    public class AMF3ByteArrayWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            if (data is byte[])
            {
                data = new ByteArray((byte[])data);
            }

            if (data is ByteArray)
            {
                writer.WriteByteArray((ByteArray)data);
            }
        }
    }
}
