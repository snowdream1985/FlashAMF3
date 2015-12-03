using flash.net;
using flash.utils;
namespace FlashFx.IO.Writers
{
    public class AMF0ASObjectWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteASObject(ObjectEncoding.AMF0, (ASObject)data);
        }
    }
}
