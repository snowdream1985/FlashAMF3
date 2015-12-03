using System.Xml;

namespace FlashFx.IO.Writers
{
    public class AMF3XmlDocumentWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3XmlDocument((XmlDocument)data);
        }
    }
}
