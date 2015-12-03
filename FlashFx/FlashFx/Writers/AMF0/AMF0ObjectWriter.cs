using flash.net;
using System;
using System.Collections;

namespace FlashFx.IO.Writers
{
    public class AMF0ObjectWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            if (data is IList)
            {
                IList list = data as IList;

                object[] array = new object[list.Count];

                list.CopyTo(array, 0);

                writer.WriteArray(ObjectEncoding.AMF0, array);
            }
            else if (data is IDictionary)
            {
                writer.WriteAssociativeArray(ObjectEncoding.AMF0, (IDictionary)data);
            }
            else
            {
                writer.WriteObject(ObjectEncoding.AMF0, data);
            }
        }
    }
}
