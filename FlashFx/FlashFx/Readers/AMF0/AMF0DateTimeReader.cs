namespace FlashFx.IO.Readers
{
    public class AMF0DateTimeReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadDateTime();
        }
    }
}
