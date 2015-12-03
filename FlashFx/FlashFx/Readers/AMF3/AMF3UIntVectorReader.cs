namespace FlashFx.IO.Readers
{
    public class AMF3UIntVectorReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3UIntVector();
        }
    }
}
