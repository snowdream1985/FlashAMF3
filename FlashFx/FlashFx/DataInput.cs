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
        /// ���ļ������ֽ������ֽ������ж�ȡ����ֵ. 
        /// </summary>
        /// <returns></returns>
        public bool readBoolean()
        {
            return amfReader.ReadBoolean();
        }

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�����ŵ��ֽ�. 
        /// </summary>
        /// <returns></returns>
        public byte readByte()
        {
            return amfReader.ReadByte();
        }

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ length ����ָ���������ֽ���. 
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
        /// ���ļ������ֽ������ֽ������ж�ȡ IEEE 754 ˫���ȸ�����. 
        /// </summary>
        /// <returns></returns>
        public double readDouble()
        {
            return amfReader.ReadDouble();
        }

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ IEEE 754 �����ȸ�����. 
        /// </summary>
        /// <returns></returns>
        public float readFloat()
        {
            return amfReader.ReadSingle();
        }

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�����ŵ� 32 λ����. 
        /// </summary>
        /// <returns></returns>
        public int readInt()
        {
            return amfReader.ReadInt32();
        }

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�� AMF ���л���ʽ����Ķ���. 
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
        /// ���ļ������ֽ������ֽ������ж�ȡ�����ŵ� 16 λ����. 
        /// </summary>
        /// <returns></returns>
        public short readShort()
        {
            return amfReader.ReadInt16();
        }

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�޷��ŵ��ֽ�. 
        /// </summary>
        /// <returns></returns>
        public byte readUnsignedByte()
        {
            return amfReader.ReadByte();
        }

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�޷��ŵ� 32 λ����. 
        /// </summary>
        /// <returns></returns>
        public uint readUnsignedInt()
        {
            return (uint)amfReader.ReadInt32();
        }

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�޷��ŵ� 16 λ����. 
        /// </summary>
        /// <returns></returns>
        public ushort readUnsignedShort()
        {
            return amfReader.ReadUInt16();
        }

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ UTF-8 �ַ���. 
        /// </summary>
        /// <returns></returns>
        public string readUTF()
        {
            return amfReader.ReadString();
        }

        /// <summary>
        /// ���ֽ������ֽ������ж�ȡ UTF-8 �ֽ����У�������һ���ַ���. 
        /// </summary>
        public string readUTFBytes(int length)
        {
            return amfReader.ReadUTFBytes(length);
        }

        #endregion
    }
}
