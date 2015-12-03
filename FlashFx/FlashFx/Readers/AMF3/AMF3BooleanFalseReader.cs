namespace FlashFx.IO.Readers
{
    public class AMF3BooleanFalseReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return false;
        }
    }
}
