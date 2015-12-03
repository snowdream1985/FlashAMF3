namespace FlashFx.AMF3
{
    /// <summary>
    /// IDataOutput 接口提供一组用于写入二进制数据的方法. 
    /// </summary>
    public interface IDataOutput
    {
        /// <summary>
        /// 写入布尔值.
        /// </summary>
        void writeBoolean(bool value);

        /// <summary>
        /// 写入一个字节.
        /// </summary>
        void writeByte(byte value);

        /// <summary>
        /// 在指定的字节数组 bytes 中，从 offset（使用从零开始的索引）指定的字节开始，向文件流、字节流或字节数组中写入一个长度由 length 指定的字节序列.
        /// </summary>
        void writeBytes(byte[] bytes, int offset, int length);

        /// <summary>
        /// 写入 IEEE 754 双精度（64 位）浮点数.
        /// </summary>
        void writeDouble(double value);

        /// <summary>
        /// 写入 IEEE 754 单精度（32 位）浮点数.
        /// </summary>
        void writeFloat(float value);

        /// <summary>
        /// 写入一个带符号的 32 位整数.
        /// </summary>
        void writeInt(int value);

        /// <summary>
        /// 以 AMF 序列化格式将对象写入文件流、字节流或字节数组中.
        /// </summary>
        void writeObject(object value);

        /// <summary>
        /// 写入一个 16 位整数.
        /// </summary>
        void writeShort(short value);

        /// <summary>
        /// 写入一个无符号的 32 位整数.
        /// </summary>
        void writeUnsignedInt(uint value);

        /// <summary>
        /// 将 UTF-8 字符串写入文件流、字节流或字节数组中.
        /// </summary>
        void writeUTF(string value);

        /// <summary>
        /// 写入一个 UTF-8 字符串.
        /// </summary>
        void writeUTFBytes(string value);
    }
}
