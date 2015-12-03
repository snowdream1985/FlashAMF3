namespace FlashFx.IO.Readers
{
    public class AMF3DoubleVectorReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3DoubleVector();
        }
    }
}
