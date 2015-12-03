namespace FlashFx.IO.Readers
{
    public class AMF3IntegerReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3IntegerData();
        }
    }
}
