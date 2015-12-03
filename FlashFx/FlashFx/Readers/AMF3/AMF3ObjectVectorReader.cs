namespace FlashFx.IO.Readers
{
    public class AMF3ObjectVectorReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3ObjectVector();
        }
    }
}
