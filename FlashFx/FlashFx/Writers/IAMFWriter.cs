namespace FlashFx.IO.Writers
{
    interface IAMFWriter
    {
        bool IsPrimitive { get; }

        void WriteData(AMFWriter writer, object data);
    }
}
