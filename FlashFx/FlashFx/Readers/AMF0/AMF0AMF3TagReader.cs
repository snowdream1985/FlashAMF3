namespace FlashFx.IO.Readers
{
    public class AMF0AMF3TagReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3Data();
        }
    }
}
