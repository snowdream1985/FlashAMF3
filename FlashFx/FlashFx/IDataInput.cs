namespace FlashFx.AMF3
{
    /// <summary>
    /// IDataInput �ӿ��ṩһ�����ڶ�ȡ���������ݵķ���.
    /// </summary>
    public interface IDataInput
    {
        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ����ֵ. 
        /// </summary>
        /// <returns></returns>
        bool readBoolean();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�����ŵ��ֽ�. 
        /// </summary>
        /// <returns></returns>
        byte readByte();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ length ����ָ���������ֽ���. 
        /// </summary>
        void readBytes(byte[] bytes, int offset, int length);

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ IEEE 754 ˫���ȸ�����. 
        /// </summary>
        /// <returns></returns>
        double readDouble();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ IEEE 754 �����ȸ�����. 
        /// </summary>
        /// <returns></returns>
        float readFloat();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�����ŵ� 32 λ����. 
        /// </summary>
        /// <returns></returns>
        int readInt();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�� AMF ���л���ʽ����Ķ���. 
        /// </summary>
        /// <returns></returns>
        object readObject();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�����ŵ� 16 λ����. 
        /// </summary>
        /// <returns></returns>
        short readShort();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�޷��ŵ��ֽ�. 
        /// </summary>
        /// <returns></returns>
        byte readUnsignedByte();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�޷��ŵ� 32 λ����. 
        /// </summary>
        /// <returns></returns>
        uint readUnsignedInt();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ�޷��ŵ� 16 λ����. 
        /// </summary>
        /// <returns></returns>
        ushort readUnsignedShort();

        /// <summary>
        /// ���ļ������ֽ������ֽ������ж�ȡ UTF-8 �ַ���. 
        /// </summary>
        /// <returns></returns>
        string readUTF();

        /// <summary>
        /// ���ֽ������ֽ������ж�ȡ UTF-8 �ֽ����У�������һ���ַ���. 
        /// </summary>
        string readUTFBytes(int length);
    }
}
