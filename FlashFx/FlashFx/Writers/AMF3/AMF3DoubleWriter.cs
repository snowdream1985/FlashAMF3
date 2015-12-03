using System;

namespace FlashFx.IO.Writers
{
    public class AMF3DoubleWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            double value = Convert.ToDouble(data);

            writer.WriteAMF3Double(value);
        }
    }
}
