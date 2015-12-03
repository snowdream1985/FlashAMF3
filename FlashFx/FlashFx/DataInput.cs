using flash.net;
using FlashFx.IO;

namespace FlashFx.AMF3
{
    public class DataInput : IDataInput
    {
        public AMFReader amfReader;

        private ObjectEncoding _objectEncoding;

        public DataInput(AMFReader amfReader)
        {
            this.amfReader = amfReader;

            _objectEncoding = ObjectEncoding.AMF3;
        }

        public ObjectEncoding objectEncoding
        {
            get { return _objectEncoding; }

            set { _objectEncoding = value; }
        }

        #region IDataInput Members

        /// <summary>
        /// 从文件流、字节流或字节数组中读取布尔值. 
        /// </summary>
        /// <returns></returns>
        public bool readBoolean()
        {
            return amfReader.ReadBoolean();
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取带符号的字节. 
        /// </summary>
        /// <returns></returns>
        public byte readByte()
        {
            return amfReader.ReadByte();
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取 length 参数指定的数据字节数. 
        /// </summary>
        public void readBytes(byte[] bytes, int offset, int length)
        {
            byte[] tmp = amfReader.ReadBytes(length);

            for (int i = 0; i < tmp.Length; i++)
            {
                bytes[i + offset] = tmp[i];
            }
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取 IEEE 754 双精度浮点数. 
        /// </summary>
        /// <returns></returns>
        public double readDouble()
        {
            return amfReader.ReadDouble();
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取 IEEE 754 单精度浮点数. 
        /// </summary>
        /// <returns></returns>
        public float readFloat()
        {
            return amfReader.ReadSingle();
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取带符号的 32 位整数. 
        /// </summary>
        /// <returns></returns>
        public int readInt()
        {
            return amfReader.ReadInt32();
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取以 AMF 序列化格式编码的对象. 
        /// </summary>
        /// <returns></returns>
        public object readObject()
        {
            object obj = null;

            if (_objectEncoding == ObjectEncoding.AMF0)
            {
                obj = amfReader.ReadData();
            }
            else if (_objectEncoding == ObjectEncoding.AMF3)
            {
                obj = amfReader.ReadAMF3Data();
            }

            return obj;
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取带符号的 16 位整数. 
        /// </summary>
        /// <returns></returns>
        public short readShort()
        {
            return amfReader.ReadInt16();
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取无符号的字节. 
        /// </summary>
        /// <returns></returns>
        public byte readUnsignedByte()
        {
            return amfReader.ReadByte();
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取无符号的 32 位整数. 
        /// </summary>
        /// <returns></returns>
        public uint readUnsignedInt()
        {
            return (uint)amfReader.ReadInt32();
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取无符号的 16 位整数. 
        /// </summary>
        /// <returns></returns>
        public ushort readUnsignedShort()
        {
            return amfReader.ReadUInt16();
        }

        /// <summary>
        /// 从文件流、字节流或字节数组中读取 UTF-8 字符串. 
        /// </summary>
        /// <returns></returns>
        public string readUTF()
        {
            return amfReader.ReadString();
        }

        /// <summary>
        /// 从字节流或字节数组中读取 UTF-8 字节序列，并返回一个字符串. 
        /// </summary>
        public string readUTFBytes(int length)
        {
            return amfReader.ReadUTFBytes(length);
        }

        #endregion
    }
}
