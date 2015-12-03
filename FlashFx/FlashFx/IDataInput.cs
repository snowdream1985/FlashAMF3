namespace FlashFx.AMF3
{
    /// <summary>
    /// IDataInput 接口提供一组用于读取二进制数据的方法.
    /// </summary>
    public interface IDataInput
    {
        /// <summary>
        /// 从文件流、字节流或字节数组中读取布尔值. 
        /// </summary>
        /// <returns></returns>
        bool readBoolean();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取带符号的字节. 
        /// </summary>
        /// <returns></returns>
        byte readByte();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取 length 参数指定的数据字节数. 
        /// </summary>
        void readBytes(byte[] bytes, int offset, int length);

        /// <summary>
        /// 从文件流、字节流或字节数组中读取 IEEE 754 双精度浮点数. 
        /// </summary>
        /// <returns></returns>
        double readDouble();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取 IEEE 754 单精度浮点数. 
        /// </summary>
        /// <returns></returns>
        float readFloat();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取带符号的 32 位整数. 
        /// </summary>
        /// <returns></returns>
        int readInt();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取以 AMF 序列化格式编码的对象. 
        /// </summary>
        /// <returns></returns>
        object readObject();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取带符号的 16 位整数. 
        /// </summary>
        /// <returns></returns>
        short readShort();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取无符号的字节. 
        /// </summary>
        /// <returns></returns>
        byte readUnsignedByte();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取无符号的 32 位整数. 
        /// </summary>
        /// <returns></returns>
        uint readUnsignedInt();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取无符号的 16 位整数. 
        /// </summary>
        /// <returns></returns>
        ushort readUnsignedShort();

        /// <summary>
        /// 从文件流、字节流或字节数组中读取 UTF-8 字符串. 
        /// </summary>
        /// <returns></returns>
        string readUTF();

        /// <summary>
        /// 从字节流或字节数组中读取 UTF-8 字节序列，并返回一个字符串. 
        /// </summary>
        string readUTFBytes(int length);
    }
}
