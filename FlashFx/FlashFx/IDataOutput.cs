namespace FlashFx.AMF3
{
    /// <summary>
    /// IDataOutput �ӿ��ṩһ������д����������ݵķ���. 
    /// </summary>
    public interface IDataOutput
    {
        /// <summary>
        /// д�벼��ֵ.
        /// </summary>
        void writeBoolean(bool value);

        /// <summary>
        /// д��һ���ֽ�.
        /// </summary>
        void writeByte(byte value);

        /// <summary>
        /// ��ָ�����ֽ����� bytes �У��� offset��ʹ�ô��㿪ʼ��������ָ�����ֽڿ�ʼ�����ļ������ֽ������ֽ�������д��һ�������� length ָ�����ֽ�����.
        /// </summary>
        void writeBytes(byte[] bytes, int offset, int length);

        /// <summary>
        /// д�� IEEE 754 ˫���ȣ�64 λ��������.
        /// </summary>
        void writeDouble(double value);

        /// <summary>
        /// д�� IEEE 754 �����ȣ�32 λ��������.
        /// </summary>
        void writeFloat(float value);

        /// <summary>
        /// д��һ�������ŵ� 32 λ����.
        /// </summary>
        void writeInt(int value);

        /// <summary>
        /// �� AMF ���л���ʽ������д���ļ������ֽ������ֽ�������.
        /// </summary>
        void writeObject(object value);

        /// <summary>
        /// д��һ�� 16 λ����.
        /// </summary>
        void writeShort(short value);

        /// <summary>
        /// д��һ���޷��ŵ� 32 λ����.
        /// </summary>
        void writeUnsignedInt(uint value);

        /// <summary>
        /// �� UTF-8 �ַ���д���ļ������ֽ������ֽ�������.
        /// </summary>
        void writeUTF(string value);

        /// <summary>
        /// д��һ�� UTF-8 �ַ���.
        /// </summary>
        void writeUTFBytes(string value);
    }
}
