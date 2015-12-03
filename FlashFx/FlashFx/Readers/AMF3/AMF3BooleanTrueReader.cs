namespace FlashFx.IO.Readers
{
    public class AMF3BooleanTrueReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return true;
        }
    }
}
