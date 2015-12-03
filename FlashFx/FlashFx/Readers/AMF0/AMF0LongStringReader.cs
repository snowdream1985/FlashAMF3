namespace FlashFx.IO.Readers
{
    public class AMF0LongStringReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            int length = reader.ReadInt32();

            return reader.ReadUTFBytes(length);
        }
    }
}
