using System.Collections.Generic;

namespace FlashFx.IO.Writers
{
    public class AMF3DoubleVectorWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF3TypeCode.NumberVector);

            writer.WriteAMF3DoubleVector(data as IList<double>);
        }
    }
}
