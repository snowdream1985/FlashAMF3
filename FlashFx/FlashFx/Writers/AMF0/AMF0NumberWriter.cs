using System;

namespace FlashFx.IO.Writers
{
    public class AMF0NumberWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF0TypeCode.Number);

            writer.WriteDouble(Convert.ToDouble(data));
        }
    }
}
