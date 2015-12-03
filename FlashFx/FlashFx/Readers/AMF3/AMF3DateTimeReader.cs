namespace FlashFx.IO.Readers
{
    public class AMF3DateTimeReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3Date();
        }
    }
}
