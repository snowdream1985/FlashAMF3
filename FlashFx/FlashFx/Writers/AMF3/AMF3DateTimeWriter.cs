using System;

namespace FlashFx.IO.Writers
{
    public class AMF3DateTimeWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF3TypeCode.DateTime);

            writer.WriteAMF3DateTime((DateTime)data);
        }
    }
}
