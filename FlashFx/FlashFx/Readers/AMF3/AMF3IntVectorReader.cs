namespace FlashFx.IO.Readers
{
    public class AMF3IntVectorReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3IntVector();
        }
    }
}
