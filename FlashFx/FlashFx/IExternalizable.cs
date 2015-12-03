namespace FlashFx.AMF3
{
    /// <summary>
    /// ������뵽��������ʱ��IExternalizable �ӿ��ṩ�������л��Ŀ���.
    /// </summary>
    public interface IExternalizable
    {
        /// <summary>
        /// ��ʵ�ִ˷���,�Ա�ͨ������ IDataInput �ӿڵķ���,����������������н���. 
        /// </summary>
        void readExternal(IDataInput input);

        /// <summary>
        ///��ʵ�ִ˷���,�Ա�ͨ������ IDataOutput �ӿڵķ���,����������뵽��������.
        /// </summary>
        void writeExternal(IDataOutput output);
    }
}
