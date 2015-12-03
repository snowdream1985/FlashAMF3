namespace FlashFx.IO.Readers
{
    public class AMF3ObjectReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3Object();
        }
    }
}
