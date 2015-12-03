using System;

namespace FlashFx.IO.Writers
{
    public class AMF3ArrayWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF3TypeCode.Array);

            writer.WriteAMF3Array((Array)data);
        }
    }
}
