namespace FlashFx.IO.Readers
{
    public class AMF3ArrayReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3Array();
        }
    }
}
