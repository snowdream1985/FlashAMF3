using System;
using System.IO;
using System.IO.Compression;
using FlashFx.IO;
using System.Net;
using System.Text;
using FlashFx.AMF3;
using flash.net;

namespace flash.utils
{
    /// <summary>
    /// ByteArray 类提供用于优化读取、写入以及处理二进制数据的方法和属性.
    /// </summary>
    public class ByteArray : IDataInput, IDataOutput
    {
        private MemoryStream _memoryStream;

        public DataOutput dataOutput;

        public DataInput dataInput;

        private ObjectEncoding _objectEncoding;

        public ByteArray()
            : this(new MemoryStream())
        {

        }

        public ByteArray(byte[] buffer)
            : this(new MemoryStream(buffer))
        {

        }

        public ByteArray(MemoryStream memoryStream)
        {
            _memoryStream = memoryStream;

            Reset();

            objectEncoding = ObjectEncoding.AMF3;
        }

        public void Reset()
        {
            AMFReader amfReader = new AMFReader(_memoryStream);

            AMFWriter amfWriter = new AMFWriter(_memoryStream);

            dataOutput = new DataOutput(amfWriter);

            dataInput = new DataInput(amfReader);
        }

        /// <summary>
        /// ByteArray 对象的长度(以字节为单位).
        /// </summary>
        public int length { get { return (int)_memoryStream.Length; } }

        /// <summary>
        /// 将文件指针的当前位置(以字节为单位)移动或返回到 ByteArray 对象中.
        /// </summary>
        public int position
        {
            get { return (int)_memoryStream.Position; }

            set { _memoryStream.Position = value; }
        }

        /// <summary>
        /// 可从字节数组的当前位置到数组末尾读取的数据的字节数.
        /// </summary>
        public int bytesAvailable { get { return length - position; } }

        public ObjectEncoding objectEncoding
        {
            get { return _objectEncoding; }

            set
            {
                _objectEncoding = value;

                dataInput.objectEncoding = value;

                dataOutput.ObjectEncoding = value;
            }
        }

        public MemoryStream memoryStream { get { return _memoryStream; } }

        #region IDataInput Members

        /// <summary>
        /// 从字节流中读取布尔值. 
        /// </summary>
        /// <returns></returns>
        public bool readBoolean()
        {
            return dataInput.readBoolean();
        }

        /// <summary>
        /// 从字节流中读取带符号的字节. 
        /// </summary>
        /// <returns></returns>
        public byte readByte()
        {
            return dataInput.readByte();
        }

        /// <summary>
        /// 从字节流中读取 length 参数指定的数据字节数. 
        /// </summary>
        public void readBytes(byte[] bytes, int offset, int length)
        {
            dataInput.readBytes(bytes, offset, length);
        }

        public void readBytes(ByteArray bytes, int offset, int length)
        {
            int tmp = bytes.position;

            int count = length != 0 ? length : bytesAvailable;

            for (int i = 0; i < count; i++)
            {
                bytes._memoryStream.Position = i + offset;

                bytes._memoryStream.WriteByte(readByte());
            }

            bytes.position = tmp;
        }

        public void readBytes(ByteArray bytes)
        {
            readBytes(bytes, 0, 0);
        }

        /// <summary>
        /// 从字节流中读取一个 IEEE 754 双精度（64 位）浮点数. 
        /// </summary>
        /// <returns></returns>
        public double readDouble()
        {
            return dataInput.readDouble();
        }

        /// <summary>
        /// 从字节流中读取一个 IEEE 754 单精度（32 位）浮点数. 
        /// </summary>
        /// <returns></returns>
        public float readFloat()
        {
            return dataInput.readFloat();
        }

        /// <summary>
        /// 从字节流中读取一个带符号的 32 位整数. 
        /// </summary>
        /// <returns></returns>
        public int readInt()
        {
            return dataInput.readInt();
        }

        /// <summary>
        /// 从字节数组中读取一个以 AMF 序列化格式进行编码的对象. 
        /// </summary>
        /// <returns></returns>
        public object readObject()
        {
            return dataInput.readObject();
        }

        /// <summary>
        /// 从字节流中读取一个带符号的 16 位整数. 
        /// </summary>
        /// <returns></returns>
        public short readShort()
        {
            return dataInput.readShort();
        }

        /// <summary>
        /// 从字节流中读取无符号的字节. 
        /// </summary>
        /// <returns></returns>
        public byte readUnsignedByte()
        {
            return dataInput.readUnsignedByte();
        }

        /// <summary>
        /// 从字节流中读取一个无符号的 32 位整数. 
        /// </summary>
        /// <returns></returns>
        public uint readUnsignedInt()
        {
            return dataInput.readUnsignedInt();
        }

        /// <summary>
        /// 从字节流中读取一个无符号的 16 位整数. 
        /// </summary>
        /// <returns></returns>
        public ushort readUnsignedShort()
        {
            return dataInput.readUnsignedShort();
        }

        /// <summary>
        /// 从字节流中读取一个 UTF-8 字符串. 
        /// </summary>
        /// <returns></returns>
        public string readUTF()
        {
            return dataInput.readUTF();
        }

        /// <summary>
        /// 从字节流中读取一个由 length 参数指定的 UTF-8 字节序列，并返回一个字符串. 
        /// </summary>
        public string readUTFBytes(int length)
        {
            return dataInput.readUTFBytes(length);
        }

        #endregion

        #region IDataOutput Members

        /// <summary>
        /// 在字节流中写入一个布尔值.
        /// </summary>
        public void writeBoolean(bool value)
        {
            dataOutput.writeBoolean(value);
        }

        /// <summary>
        /// 在字节流中写入一个字节.
        /// </summary>
        public void writeByte(byte value)
        {
            dataOutput.writeByte(value);
        }

        /// <summary>
        /// 将来自指定字节数组、字节数、起始偏移（基于零的索引）字节的长度字节数序列写入字节流.
        /// </summary>
        public void writeBytes(byte[] bytes, int offset, int length)
        {
            dataOutput.writeBytes(bytes, offset, length);
        }

        /// <summary>
        /// 在字节流中写入一个 IEEE 754 双精度（64 位）浮点数.
        /// </summary>
        public void writeDouble(double value)
        {
            dataOutput.writeDouble(value);
        }

        /// <summary>
        /// 在字节流中写入一个 IEEE 754 单精度（32 位）浮点数.
        /// </summary>
        public void writeFloat(float value)
        {
            dataOutput.writeFloat(value);
        }

        /// <summary>
        /// 在字节流中写入一个带符号的 32 位整数.
        /// </summary>
        public void writeInt(int value)
        {
            dataOutput.writeInt(value);
        }

        /// <summary>
        /// 将对象以 AMF 序列化格式写入字节数组.
        /// </summary>
        public void writeObject(object value)
        {
            dataOutput.writeObject(value);
        }

        /// <summary>
        /// 在字节流中写入一个 16 位整数.
        /// </summary>
        public void writeShort(short value)
        {
            dataOutput.writeShort(value);
        }

        /// <summary>
        /// 在字节流中写入一个无符号的 32 位整数.
        /// </summary>
        public void writeUnsignedInt(uint value)
        {
            dataOutput.writeUnsignedInt(value);
        }

        /// <summary>
        /// 将 UTF-8 字符串写入字节流
        /// </summary>
        public void writeUTF(string value)
        {
            dataOutput.writeUTF(value);
        }

        /// <summary>
        /// 将 UTF-8 字符串写入字节流.
        /// </summary>
        public void writeUTFBytes(string value)
        {
            dataOutput.writeUTFBytes(value);
        }

        #endregion

        /// <summary>
        /// 压缩字节数组.
        /// </summary>
        public void compress()
        {
            compress("zlib");
        }

        /// <summary>
        /// 使用 deflate 压缩算法压缩字节数组.
        /// </summary>
        public void deflate()
        {
            compress();
        }

        /// <summary>
        /// 压缩字节数组.
        /// </summary>
        public void compress(string algorithm = "deflate")
        {
            byte[] buffer = _memoryStream.ToArray();

            MemoryStream ms = new MemoryStream();

            if (algorithm == "zlib")
            {
                ms.WriteByte(0x78);
                ms.WriteByte(0x9C);
            }

            DeflateStream deflateStream = new DeflateStream(ms, CompressionMode.Compress, true);

            deflateStream.Write(buffer, 0, buffer.Length);

            deflateStream.Close();

            _memoryStream.Close();

            if (algorithm == "zlib")
            {
                ms.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Adler32(buffer))), 0, 4);
            }

            _memoryStream = ms;

            Reset();
        }

        private static int Adler32(byte[] buffer)
        {
            if (buffer == null)
            {
                return 1;
            }

            const int ModAdler = 65521;

            uint s1 = 1, s2 = 0;

            foreach (byte b in buffer)
            {
                s1 = (s1 + b) % ModAdler;

                s2 = (s2 + s1) % ModAdler;
            }

            return unchecked((int)((s2 << 16) + s1));
        }

        /// <summary>
        /// 使用 deflate 压缩算法将字节数组解压缩
        /// </summary>
        public void inflate()
        {
            uncompress();
        }

        /// <summary>
        /// 解压缩字节数组.
        /// </summary>
        public void uncompress()
        {
            uncompress("zlib");
        }

        /// <summary>
        /// 解压缩字节数组.
        /// </summary>
        private void uncompress(string algorithm = "deflate")
        {
            position = 0;

            if (algorithm == "zlib")
            {
                int firstByte = _memoryStream.ReadByte();

                int secondByte = _memoryStream.ReadByte();

                if (((firstByte == 0x78) && (secondByte == 0x9C)) || ((firstByte == 0x78) && (secondByte == 0xDA)) || ((firstByte == 0x58) && (secondByte == 0x85)))
                {

                }
                else
                {
                    position = 0;
                }
            }

            DeflateStream deflateStream = new DeflateStream(_memoryStream, CompressionMode.Decompress, false);

            MemoryStream ms = new MemoryStream();

            deflateStream.CopyTo(ms);

            deflateStream.Close();

            _memoryStream.Close();

            _memoryStream.Dispose();

            _memoryStream = ms;

            _memoryStream.Position = 0;

            Reset();
        }

        /// <summary>
        /// 清除字节数组的内容，并将 length 和 position 属性重置为 0.
        /// </summary>
        public void clear()
        {
            _memoryStream = new MemoryStream();

            Reset();
        }

        /// <summary>
        /// 将字节数组转换为字符串.
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            return ToString();
        }

        /// <summary>
        /// 将流内容写入字节数组.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return _memoryStream.ToArray();
        }

        /// <summary>
        /// 将字节数组转换为字符串.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Encoding.UTF8.GetString(ToArray());
        }
    }
}
