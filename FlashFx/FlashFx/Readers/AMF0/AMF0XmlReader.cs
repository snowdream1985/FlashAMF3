namespace FlashFx.IO.Readers
{
    public class AMF0XmlReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadXmlDocument();
        }
    }
}
