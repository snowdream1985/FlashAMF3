namespace FlashFx.IO.Readers
{
    public class AMF3StringReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3String();
        }
    }
}
