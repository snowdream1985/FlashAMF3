namespace FlashFx.IO.Writers
{
    public class AMF0StringWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteString((string)data);
        }
    }
}
