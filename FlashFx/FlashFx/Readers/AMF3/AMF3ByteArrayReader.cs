namespace FlashFx.IO.Readers
{
    public class AMF3ByteArrayReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3ByteArray();
        }
    }
}
