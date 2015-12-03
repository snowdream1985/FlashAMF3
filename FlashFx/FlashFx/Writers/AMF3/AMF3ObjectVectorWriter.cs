using System.Collections;
using System.Collections.Generic;

namespace FlashFx.IO.Writers
{
    public class AMF3ObjectVectorWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF3TypeCode.ObjectVector);

            //仅处理String,未处理Bool等其它数据类型
            writer.WriteAMF3ObjectVector(data as IList<string>);
        }
    }
}
