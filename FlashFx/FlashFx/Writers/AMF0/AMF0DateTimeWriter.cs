using System;

namespace FlashFx.IO.Writers
{
    public class AMF0DateTimeWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF0TypeCode.DateTime);

            writer.WriteDateTime((DateTime)data);
        }
    }
}
