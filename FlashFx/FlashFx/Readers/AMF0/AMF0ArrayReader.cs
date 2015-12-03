namespace FlashFx.IO.Readers
{
    public class AMF0ArrayReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadArray();
        }
    }
}
