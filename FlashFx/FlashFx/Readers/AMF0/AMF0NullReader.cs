namespace FlashFx.IO.Readers
{
    public class AMF0NullReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return null;
        }
    }
}
