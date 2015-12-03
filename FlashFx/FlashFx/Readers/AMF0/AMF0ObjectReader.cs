namespace FlashFx.IO.Readers
{
    public class AMF0ObjectReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadObject();
        }
    }
}
