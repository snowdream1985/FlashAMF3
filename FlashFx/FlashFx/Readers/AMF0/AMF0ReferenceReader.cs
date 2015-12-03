namespace FlashFx.IO.Readers
{
    public class AMF0ReferenceReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            int index = reader.ReadUInt16();

            return reader.ReadAMF0ObjectReference(index);
        }
    }
}
