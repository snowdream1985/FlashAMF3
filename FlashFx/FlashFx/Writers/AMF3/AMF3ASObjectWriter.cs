namespace FlashFx.IO.Writers
{
    public class AMF3ASObjectWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return false; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF3TypeCode.Object);

            writer.WriteAMF3Object(data);
        }
    }
}
