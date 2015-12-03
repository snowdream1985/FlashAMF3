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
    /// ByteArray ���ṩ�����Ż���ȡ��д���Լ�������������ݵķ���������.
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
        /// ByteArray ����ĳ���(���ֽ�Ϊ��λ).
        /// </summary>
        public int length { get { return (int)_memoryStream.Length; } }

        /// <summary>
        /// ���ļ�ָ��ĵ�ǰλ��(���ֽ�Ϊ��λ)�ƶ��򷵻ص� ByteArray ������.
        /// </summary>
        public int position
        {
            get { return (int)_memoryStream.Position; }

            set { _memoryStream.Position = value; }
        }

        /// <summary>
        /// �ɴ��ֽ�����ĵ�ǰλ�õ�����ĩβ��ȡ�����ݵ��ֽ���.
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
        /// ���ֽ����ж�ȡ����ֵ. 
        /// </summary>
        /// <returns></returns>
        public bool readBoolean()
        {
            return dataInput.readBoolean();
        }

        /// <summary>
        /// ���ֽ����ж�ȡ�����ŵ��ֽ�. 
        /// </summary>
        /// <returns></returns>
        public byte readByte()
        {
            return dataInput.readByte();
        }

        /// <summary>
        /// ���ֽ����ж�ȡ length ����ָ���������ֽ���. 
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
        /// ���ֽ����ж�ȡһ�� IEEE 754 ˫���ȣ�64 λ��������. 
        /// </summary>
        /// <returns></returns>
        public double readDouble()
        {
            return dataInput.readDouble();
        }

        /// <summary>
        /// ���ֽ����ж�ȡһ�� IEEE 754 �����ȣ�32 λ��������. 
        /// </summary>
        /// <returns></returns>
        public float readFloat()
        {
            return dataInput.readFloat();
        }

        /// <summary>
        /// ���ֽ����ж�ȡһ�������ŵ� 32 λ����. 
        /// </summary>
        /// <returns></returns>
        public int readInt()
        {
            return dataInput.readInt();
        }

        /// <summary>
        /// ���ֽ������ж�ȡһ���� AMF ���л���ʽ���б���Ķ���. 
        /// </summary>
        /// <returns></returns>
        public object readObject()
        {
            return dataInput.readObject();
        }

        /// <summary>
        /// ���ֽ����ж�ȡһ�������ŵ� 16 λ����. 
        /// </summary>
        /// <returns></returns>
        public short readShort()
        {
            return dataInput.readShort();
        }

        /// <summary>
        /// ���ֽ����ж�ȡ�޷��ŵ��ֽ�. 
        /// </summary>
        /// <returns></returns>
        public byte readUnsignedByte()
        {
            return dataInput.readUnsignedByte();
        }

        /// <summary>
        /// ���ֽ����ж�ȡһ���޷��ŵ� 32 λ����. 
        /// </summary>
        /// <returns></returns>
        public uint readUnsignedInt()
        {
            return dataInput.readUnsignedInt();
        }

        /// <summary>
        /// ���ֽ����ж�ȡһ���޷��ŵ� 16 λ����. 
        /// </summary>
        /// <returns></returns>
        public ushort readUnsignedShort()
        {
            return dataInput.readUnsignedShort();
        }

        /// <summary>
        /// ���ֽ����ж�ȡһ�� UTF-8 �ַ���. 
        /// </summary>
        /// <returns></returns>
        public string readUTF()
        {
            return dataInput.readUTF();
        }

        /// <summary>
        /// ���ֽ����ж�ȡһ���� length ����ָ���� UTF-8 �ֽ����У�������һ���ַ���. 
        /// </summary>
        public string readUTFBytes(int length)
        {
            return dataInput.readUTFBytes(length);
        }

        #endregion

        #region IDataOutput Members

        /// <summary>
        /// ���ֽ�����д��һ������ֵ.
        /// </summary>
        public void writeBoolean(bool value)
        {
            dataOutput.writeBoolean(value);
        }

        /// <summary>
        /// ���ֽ�����д��һ���ֽ�.
        /// </summary>
        public void writeByte(byte value)
        {
            dataOutput.writeByte(value);
        }

        /// <summary>
        /// ������ָ���ֽ����顢�ֽ�������ʼƫ�ƣ���������������ֽڵĳ����ֽ�������д���ֽ���.
        /// </summary>
        public void writeBytes(byte[] bytes, int offset, int length)
        {
            dataOutput.writeBytes(bytes, offset, length);
        }

        /// <summary>
        /// ���ֽ�����д��һ�� IEEE 754 ˫���ȣ�64 λ��������.
        /// </summary>
        public void writeDouble(double value)
        {
            dataOutput.writeDouble(value);
        }

        /// <summary>
        /// ���ֽ�����д��һ�� IEEE 754 �����ȣ�32 λ��������.
        /// </summary>
        public void writeFloat(float value)
        {
            dataOutput.writeFloat(value);
        }

        /// <summary>
        /// ���ֽ�����д��һ�������ŵ� 32 λ����.
        /// </summary>
        public void writeInt(int value)
        {
            dataOutput.writeInt(value);
        }

        /// <summary>
        /// �������� AMF ���л���ʽд���ֽ�����.
        /// </summary>
        public void writeObject(object value)
        {
            dataOutput.writeObject(value);
        }

        /// <summary>
        /// ���ֽ�����д��һ�� 16 λ����.
        /// </summary>
        public void writeShort(short value)
        {
            dataOutput.writeShort(value);
        }

        /// <summary>
        /// ���ֽ�����д��һ���޷��ŵ� 32 λ����.
        /// </summary>
        public void writeUnsignedInt(uint value)
        {
            dataOutput.writeUnsignedInt(value);
        }

        /// <summary>
        /// �� UTF-8 �ַ���д���ֽ���
        /// </summary>
        public void writeUTF(string value)
        {
            dataOutput.writeUTF(value);
        }

        /// <summary>
        /// �� UTF-8 �ַ���д���ֽ���.
        /// </summary>
        public void writeUTFBytes(string value)
        {
            dataOutput.writeUTFBytes(value);
        }

        #endregion

        /// <summary>
        /// ѹ���ֽ�����.
        /// </summary>
        public void compress()
        {
            compress("zlib");
        }

        /// <summary>
        /// ʹ�� deflate ѹ���㷨ѹ���ֽ�����.
        /// </summary>
        public void deflate()
        {
            compress();
        }

        /// <summary>
        /// ѹ���ֽ�����.
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
        /// ʹ�� deflate ѹ���㷨���ֽ������ѹ��
        /// </summary>
        public void inflate()
        {
            uncompress();
        }

        /// <summary>
        /// ��ѹ���ֽ�����.
        /// </summary>
        public void uncompress()
        {
            uncompress("zlib");
        }

        /// <summary>
        /// ��ѹ���ֽ�����.
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
        /// ����ֽ���������ݣ����� length �� position ��������Ϊ 0.
        /// </summary>
        public void clear()
        {
            _memoryStream = new MemoryStream();

            Reset();
        }

        /// <summary>
        /// ���ֽ�����ת��Ϊ�ַ���.
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            return ToString();
        }

        /// <summary>
        /// ��������д���ֽ�����.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return _memoryStream.ToArray();
        }

        /// <summary>
        /// ���ֽ�����ת��Ϊ�ַ���.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Encoding.UTF8.GetString(ToArray());
        }
    }
}
