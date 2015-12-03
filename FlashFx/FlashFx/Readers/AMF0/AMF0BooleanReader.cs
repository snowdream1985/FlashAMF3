namespace FlashFx.IO.Readers
{
    public class AMF0BooleanReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadBoolean();
        }
    }
}
