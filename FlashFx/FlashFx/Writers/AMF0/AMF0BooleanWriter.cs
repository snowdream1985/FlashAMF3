namespace FlashFx.IO.Writers
{
    public class AMF0BooleanWriter : IAMFWriter
    {
        public bool IsPrimitive { get { return true; } }

        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(AMF0TypeCode.Boolean);

            writer.WriteBoolean((bool)data);
        }
    }
}
