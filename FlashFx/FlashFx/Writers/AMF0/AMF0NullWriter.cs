namespace FlashFx.IO.Writers
{
    public class AMF0NullWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteNull();
        }
    }
}
