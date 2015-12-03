using System.Collections.Generic;

namespace FlashFx.IO.Writers
{
    public class AMF3UIntVectorWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF3TypeCode.UIntVector);

            writer.WriteAMF3UIntVector(data as IList<uint>);
        }
    }
}
