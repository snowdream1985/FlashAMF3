namespace FlashFx.IO.Writers
{
    public class AMF3StringWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3String((string)data);
        }
    }
}
