namespace FlashFx.IO.Readers
{
    public class AMF0AssociativeArrayReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAssociativeArray();
        }
    }
}
