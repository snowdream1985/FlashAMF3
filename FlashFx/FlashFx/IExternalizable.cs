namespace FlashFx.AMF3
{
    /// <summary>
    /// 将类编码到数据流中时，IExternalizable 接口提供对其序列化的控制.
    /// </summary>
    public interface IExternalizable
    {
        /// <summary>
        /// 类实现此方法,以便通过调用 IDataInput 接口的方法,将其自身从数据流中解码. 
        /// </summary>
        void readExternal(IDataInput input);

        /// <summary>
        ///类实现此方法,以便通过调用 IDataOutput 接口的方法,将其自身编码到数据流中.
        /// </summary>
        void writeExternal(IDataOutput output);
    }
}
