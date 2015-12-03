using System.Collections.Generic;

namespace FlashFx.IO.Writers
{
    public class AMF3IntVectorWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF3TypeCode.IntVector);

            writer.WriteAMF3IntVector(data as IList<int>);
        }
    }
}
