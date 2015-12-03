namespace FlashFx.IO.Readers
{
    public class AMF0StringReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadString();
        }
    }
}
