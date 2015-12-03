namespace FlashFx.IO.Writers
{
    public class AMF3BooleanWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3Bool((bool)data);
        }
    }
}
