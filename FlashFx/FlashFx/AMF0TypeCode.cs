namespace FlashFx
{
    /// <summary>
    /// AMF0 data types.
    /// </summary>
    public class AMF0TypeCode
    {
        /// <summary>
        /// AMF Number data type.
        /// </summary>
        public const byte Number = 0;

        /// <summary>
        /// AMF Boolean data type.
        /// </summary>
        public const byte Boolean = 1;

        /// <summary>
        /// AMF String data type.
        /// </summary>
        public const byte String = 2;

        /// <summary>
        /// AMF Vector data type.
        /// </summary>
        public const byte ASObject = 3;

        /// <summary>
        /// AMF null data type.
        /// </summary>
        public const byte Null = 5;

        /// <summary>
        /// AMF undefined data type.
        /// </summary>
        public const byte Undefined = 6;

        /// <summary>
        /// AMF Reference data type.
        /// </summary>
        public const byte Reference = 7;

        /// <summary>
        /// AMF Array data type.
        /// </summary>
        public const byte AssociativeArray = 8;

        /// <summary>
        /// AMF EndOfObject data type.
        /// </summary>
        public const byte EndOfObject = 9;

        /// <summary>
        /// AMF Array data type.
        /// </summary>
        public const byte Array = 10;

        /// <summary>
        /// AMF Date data type.
        /// </summary>
        public const byte DateTime = 11;

        /// <summary>
        /// AMF LongString data type.
        /// </summary>
        public const byte LongString = 12;

        /// <summary>
        /// AMF Xml data type.
        /// </summary>
        public const byte Xml = 15;

        /// <summary>
        /// AMF CustomClass(TypedObject) data type.
        /// </summary>
        public const byte CustomClass = 16;

        /// <summary>
        /// AMF3 data.
        /// </summary>
        public const byte AMF3Tag = 17;
    }
}
