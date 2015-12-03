namespace FlashFx.IO.Readers
{
    public class AMF3NumberReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadDouble();
        }
    }
}
