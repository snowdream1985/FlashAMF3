namespace FlashFx.IO.Readers
{
    public class AMF0NumberReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadDouble();
        }
    }
}
