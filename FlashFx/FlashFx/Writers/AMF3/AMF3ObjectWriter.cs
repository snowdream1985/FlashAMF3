using flash.utils;
using FlashFx.AMF3;
using System.Collections;

namespace FlashFx.IO.Writers
{
    public class AMF3ObjectWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            if (data is ArrayCollection)
            {
                writer.WriteByte(AMF3TypeCode.Object);

                writer.WriteAMF3Object(data);
            }
            else if (data is IList)
            {
                writer.WriteByte(AMF3TypeCode.Array);

                writer.WriteAMF3Array((IList)data);
            }
            else if (data is IDictionary)
            {
                writer.WriteByte(AMF3TypeCode.Array);

                writer.WriteAMF3AssociativeArray((IDictionary)data);
            }
            else
            {
                writer.WriteByte(AMF3TypeCode.Object);

                writer.WriteAMF3Object(data);
            }
        }
    }
}
