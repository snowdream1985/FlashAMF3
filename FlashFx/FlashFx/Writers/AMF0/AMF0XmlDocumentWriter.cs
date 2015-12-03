using System.Xml;

namespace FlashFx.IO.Writers
{
    public class AMF0XmlDocumentWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteXmlDocument((XmlDocument)data);
        }
    }
}
