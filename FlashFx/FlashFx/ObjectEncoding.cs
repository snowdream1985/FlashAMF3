namespace flash.net
{
    public enum ObjectEncoding
    {
        /// <summary>
        /// 指定使用 ActionScript 1.0 和 2.0 的 Action Message Format 来序列化对象.
        /// </summary>
        AMF0 = 0,

        /// <summary>
        /// 指定使用 ActionScript 3.0 的 Action Message Format 来序列化对象.
        /// </summary>
        AMF3 = 3,

        /// <summary>
        /// 默认
        /// </summary>
        DEFAULT = 3
    }
}
