namespace FlashFx.IO.Readers
{
    public class AMF3XmlReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3XmlDocument();
        }
    }
}
