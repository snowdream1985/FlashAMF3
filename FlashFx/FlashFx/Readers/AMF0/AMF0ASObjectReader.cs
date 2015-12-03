namespace FlashFx.IO.Readers
{
    public class AMF0ASObjectReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadASObject();
        }
    }
}
