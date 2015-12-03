using flash.net;
using FlashFx.IO;

namespace FlashFx.AMF3
{
    public class DataOutput : IDataOutput
    {
        public AMFWriter amfWriter;

        private ObjectEncoding _objectEncoding;

        public DataOutput(AMFWriter amfWriter)
        {
            this.amfWriter = amfWriter;

            _objectEncoding = ObjectEncoding.AMF3;
        }

        public ObjectEncoding ObjectEncoding
        {
            get { return _objectEncoding; }

            set { _objectEncoding = value; }
        }

        #region IDataOutput Members

        /// <summary>
        /// 写入布尔值.
        /// </summary>
        public void writeBoolean(bool value)
        {
            amfWriter.WriteBoolean(value);
        }

        /// <summary>
        /// 写入一个字节.
        /// </summary>
        public void writeByte(byte value)
        {
            amfWriter.WriteByte(value);
        }

        /// <summary>
        /// 在指定的字节数组 bytes 中，从 offset（使用从零开始的索引）指定的字节开始，向文件流、字节流或字节数组中写入一个长度由 length 指定的字节序列.
        /// </summary>
        public void writeBytes(byte[] bytes, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                amfWriter.WriteByte(bytes[i]);
            }
        }

        /// <summary>
        /// 写入 IEEE 754 双精度（64 位）浮点数.
        /// </summary>
        public void writeDouble(double value)
        {
            amfWriter.WriteDouble(value);
        }

        /// <summary>
        /// 写入 IEEE 754 单精度（32 位）浮点数.
        /// </summary>
        public void writeFloat(float value)
        {
            amfWriter.WriteFloat(value);
        }

        /// <summary>
        /// 写入一个带符号的 32 位整数.
        /// </summary>
        public void writeInt(int value)
        {
            amfWriter.WriteInt32(value);
        }

        /// <summary>
        /// 以 AMF 序列化格式将对象写入文件流、字节流或字节数组中.
        /// </summary>
        public void writeObject(object value)
        {
            if (_objectEncoding == ObjectEncoding.AMF0)
            {
                amfWriter.WriteData(ObjectEncoding.AMF0, value);
            }
            else if (_objectEncoding == ObjectEncoding.AMF3)
            {
                amfWriter.WriteAMF3Data(value);
            }
        }

        /// <summary>
        /// 写入一个 16 位整数.
        /// </summary>
        public void writeShort(short value)
        {
            amfWriter.WriteShort(value);
        }

        /// <summary>
        /// 写入一个无符号的 32 位整数.
        /// </summary>
        public void writeUnsignedInt(uint value)
        {
            amfWriter.WriteInt32((int)value);
        }

        /// <summary>
        /// 将 UTF-8 字符串写入文件流、字节流或字节数组中.
        /// </summary>
        public void writeUTF(string value)
        {
            amfWriter.WriteUTF(value);
        }

        /// <summary>
        /// 写入一个 UTF-8 字符串.
        /// </summary>
        public void writeUTFBytes(string value)
        {
            amfWriter.WriteUTFBytes(value);
        }

        #endregion
    }
}
