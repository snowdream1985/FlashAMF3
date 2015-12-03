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
        /// д�벼��ֵ.
        /// </summary>
        public void writeBoolean(bool value)
        {
            amfWriter.WriteBoolean(value);
        }

        /// <summary>
        /// д��һ���ֽ�.
        /// </summary>
        public void writeByte(byte value)
        {
            amfWriter.WriteByte(value);
        }

        /// <summary>
        /// ��ָ�����ֽ����� bytes �У��� offset��ʹ�ô��㿪ʼ��������ָ�����ֽڿ�ʼ�����ļ������ֽ������ֽ�������д��һ�������� length ָ�����ֽ�����.
        /// </summary>
        public void writeBytes(byte[] bytes, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                amfWriter.WriteByte(bytes[i]);
            }
        }

        /// <summary>
        /// д�� IEEE 754 ˫���ȣ�64 λ��������.
        /// </summary>
        public void writeDouble(double value)
        {
            amfWriter.WriteDouble(value);
        }

        /// <summary>
        /// д�� IEEE 754 �����ȣ�32 λ��������.
        /// </summary>
        public void writeFloat(float value)
        {
            amfWriter.WriteFloat(value);
        }

        /// <summary>
        /// д��һ�������ŵ� 32 λ����.
        /// </summary>
        public void writeInt(int value)
        {
            amfWriter.WriteInt32(value);
        }

        /// <summary>
        /// �� AMF ���л���ʽ������д���ļ������ֽ������ֽ�������.
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
        /// д��һ�� 16 λ����.
        /// </summary>
        public void writeShort(short value)
        {
            amfWriter.WriteShort(value);
        }

        /// <summary>
        /// д��һ���޷��ŵ� 32 λ����.
        /// </summary>
        public void writeUnsignedInt(uint value)
        {
            amfWriter.WriteInt32((int)value);
        }

        /// <summary>
        /// �� UTF-8 �ַ���д���ļ������ֽ������ֽ�������.
        /// </summary>
        public void writeUTF(string value)
        {
            amfWriter.WriteUTF(value);
        }

        /// <summary>
        /// д��һ�� UTF-8 �ַ���.
        /// </summary>
        public void writeUTFBytes(string value)
        {
            amfWriter.WriteUTFBytes(value);
        }

        #endregion
    }
}
