using System;

namespace FlashFx.IO.Writers
{
    public class AMF3IntWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            int value = Convert.ToInt32(data);

            writer.WriteAMF3Int(value);
        }
    }
}
