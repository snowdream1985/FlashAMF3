using flash.net;
using flash.utils;
using FlashFx.AMF3;
using FlashFx.IO.Writers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FlashFx.IO
{
    public class AMFWriter : BinaryWriter
    {
        /// <summary>
        /// AMF0 Object 引用
        /// </summary>
        public Dictionary<object, int> amf0ObjectReferences;

        /// <summary>
        /// AMF3 Object 引用
        /// </summary>
        public Dictionary<object, int> amf3ObjectReferences;

        /// <summary>
        /// AMF3 String 引用
        /// </summary>
        public Dictionary<object, int> amf3StringReferences;

        /// <summary>
        /// AMF3 Class 引用
        /// </summary>
        public Dictionary<ClassDefinition, int> amf3ClassReferences;

        /// <summary>
        /// AMF类型表
        /// </summary>
        private static Dictionary<Type, IAMFWriter>[] AmfWriterTable;

        private static Dictionary<string, string> typeToCustomClass = new Dictionary<string, string>();

        /// <summary>
        /// 调试输出流
        /// </summary>
        public MemoryStream DebugMemoryStream;

        /// <summary>
        /// 调试输出字符串
        /// </summary>
        public StringBuilder DebugStringBuilder;

        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        public bool IsDebugEnabled { get; set; }

        static AMFWriter()
        {
            Dictionary<Type, IAMFWriter> amf0Writers = new Dictionary<Type, IAMFWriter>();

            //AMF0
            AMF0NumberWriter amf0NumberWriter = new AMF0NumberWriter();     /*0*/
            amf0Writers.Add(typeof(SByte), amf0NumberWriter);
            amf0Writers.Add(typeof(Byte), amf0NumberWriter);
            amf0Writers.Add(typeof(Int16), amf0NumberWriter);
            amf0Writers.Add(typeof(UInt16), amf0NumberWriter);
            amf0Writers.Add(typeof(Int32), amf0NumberWriter);
            amf0Writers.Add(typeof(UInt32), amf0NumberWriter);
            amf0Writers.Add(typeof(Int64), amf0NumberWriter);
            amf0Writers.Add(typeof(UInt64), amf0NumberWriter);
            amf0Writers.Add(typeof(Single), amf0NumberWriter);
            amf0Writers.Add(typeof(Double), amf0NumberWriter);
            amf0Writers.Add(typeof(Decimal), amf0NumberWriter);

            amf0Writers.Add(typeof(bool), new AMF0BooleanWriter());         /*1*/

            amf0Writers.Add(typeof(string), new AMF0StringWriter());        /*2*/

            amf0Writers.Add(typeof(ASObject), new AMF0ASObjectWriter());    /*3*/

            amf0Writers.Add(typeof(DBNull), new AMF0NullWriter());   /*5 6*/

            amf0Writers.Add(typeof(Array), new AMF0ArrayWriter());          /*10*/

            amf0Writers.Add(typeof(DateTime), new AMF0DateTimeWriter());    /*11*/

            amf0Writers.Add(typeof(XmlDocument), new AMF0XmlDocumentWriter());  /*15*/


            //AMF3
            Dictionary<Type, IAMFWriter> amf3Writers = new Dictionary<Type, IAMFWriter>();

            amf3Writers.Add(typeof(DBNull), new AMF3DBNullWriter()); /*0 1*/

            amf3Writers.Add(typeof(bool), new AMF3BooleanWriter());         /*2 3*/

            AMF3IntWriter amf3IntWriter = new AMF3IntWriter();              /*4*/
            amf3Writers.Add(typeof(SByte), amf3IntWriter);
            amf3Writers.Add(typeof(Byte), amf3IntWriter);
            amf3Writers.Add(typeof(Int16), amf3IntWriter);
            amf3Writers.Add(typeof(UInt16), amf3IntWriter);
            amf3Writers.Add(typeof(Int32), amf3IntWriter);
            amf3Writers.Add(typeof(UInt32), amf3IntWriter);

            AMF3DoubleWriter amf3DoubleWriter = new AMF3DoubleWriter();     /*5*/
            amf3Writers.Add(typeof(Int64), amf3DoubleWriter);
            amf3Writers.Add(typeof(UInt64), amf3DoubleWriter);
            amf3Writers.Add(typeof(Single), amf3DoubleWriter);
            amf3Writers.Add(typeof(Double), amf3DoubleWriter);
            amf3Writers.Add(typeof(Decimal), amf3DoubleWriter);

            amf3Writers.Add(typeof(string), new AMF3StringWriter());        /*6*/

            amf3Writers.Add(typeof(XmlDocument), new AMF3XmlDocumentWriter());  /*7 11*/

            amf3Writers.Add(typeof(DateTime), new AMF3DateTimeWriter());    /*8*/

            amf3Writers.Add(typeof(Array), new AMF3ArrayWriter());          /*9*/

            amf3Writers.Add(typeof(ASObject), new AMF3ASObjectWriter());    /*10*/

            amf3Writers.Add(typeof(ByteArray), new AMF3ByteArrayWriter());  /*12*/
            amf3Writers.Add(typeof(byte[]), new AMF3ByteArrayWriter());

            amf3Writers.Add(typeof(List<int>), new AMF3IntVectorWriter());  /*13*/
            amf3Writers.Add(typeof(IList<int>), new AMF3IntVectorWriter());

            amf3Writers.Add(typeof(List<uint>), new AMF3UIntVectorWriter());  /*14*/
            amf3Writers.Add(typeof(IList<uint>), new AMF3UIntVectorWriter());

            amf3Writers.Add(typeof(List<double>), new AMF3DoubleVectorWriter());  /*15*/
            amf3Writers.Add(typeof(IList<double>), new AMF3DoubleVectorWriter());

            //未处理bool及其它数据类型
            amf3Writers.Add(typeof(List<string>), new AMF3ObjectVectorWriter());  /*16*/
            amf3Writers.Add(typeof(IList<string>), new AMF3ObjectVectorWriter());

            AmfWriterTable = new Dictionary<Type, IAMFWriter>[4] { amf0Writers, null, null, amf3Writers };

            //类型转换
            typeToCustomClass.Add(typeof(ArrayCollection).FullName, "flex.messaging.io.ArrayCollection");
        }

        public AMFWriter(Stream stream)
            : base(stream)
        {
            Reset();

            //调试
            DebugMemoryStream = new MemoryStream();

            DebugStringBuilder = new StringBuilder();

            IsDebugEnabled = false;
        }

        public void Reset()
        {
            amf0ObjectReferences = new Dictionary<object, int>();

            amf3ObjectReferences = new Dictionary<object, int>();

            amf3StringReferences = new Dictionary<object, int>();

            amf3ClassReferences = new Dictionary<ClassDefinition, int>();
        }

        private void WriteBigEndian(byte[] bytes)
        {
            if (bytes == null)
            {
                return;
            }

            MemoryStream buffer = new MemoryStream();

            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                buffer.WriteByte(bytes[i]);
            }

            WriteBytes(buffer.ToArray());
        }

        #region DataOutput

        public void WriteBoolean(bool value)
        {
            WriteByte(value ? ((byte)1) : ((byte)0));

            if (IsDebugEnabled)
            {
                if (value)
                {
                    DebugStringBuilder.AppendFormat("01--WriteBoolean:{0}\r\n\r\n", value);
                }
                else
                {
                    DebugStringBuilder.AppendFormat("00--WriteBoolean:{0}\r\n\r\n", value);
                }
            }
        }

        public void WriteByte(byte value)
        {
            base.BaseStream.WriteByte(value);

            if (IsDebugEnabled)
            {
                DebugMemoryStream.WriteByte(value);

                DebugStringBuilder.AppendFormat("{0,0:X2}--WriteByte:{0}\r\n\r\n", value);
            }
        }

        public void WriteBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }

            base.Write(buffer);

            if (IsDebugEnabled)
            {
                DebugMemoryStream.Write(buffer, 0, buffer.Length);

                for (int i = 0; i < buffer.Length; i++)
                {
                    DebugStringBuilder.AppendFormat("{0,0:X2} ", buffer[i]);
                }

                DebugStringBuilder.Append("--WriteBytes\r\n");
            }
        }

        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            WriteBigEndian(bytes);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--WriteDouble:{1}\r\n\r\n", debugStringBuilder, value);
            }
        }

        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            WriteBigEndian(bytes);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--WriteFloat:{1}\r\n\r\n", debugStringBuilder, value);
            }
        }

        public void WriteInt32(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            WriteBigEndian(bytes);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--WriteInt32:{1}\r\n\r\n", debugStringBuilder, value);
            }
        }

        public void WriteShort(int value)
        {
            byte[] bytes = BitConverter.GetBytes((ushort)value);

            WriteBigEndian(bytes);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--WriteShort:{1}\r\n\r\n", debugStringBuilder, value);
            }
        }

        public void WriteUTF(string value)
        {
            int length = Encoding.UTF8.GetByteCount(value);

            WriteShort(length);

            byte[] bytes = Encoding.UTF8.GetBytes(value);

            if (bytes.Length > 0)
            {
                WriteBytes(bytes);

                if (IsDebugEnabled)
                {
                    StringBuilder debugStringBuilder = new StringBuilder();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                    }

                    DebugStringBuilder.AppendFormat("{0}--WriteUTF:Length:{1} \"{2}\"\r\n\r\n", debugStringBuilder, length, value);
                }
            }
        }

        public void WriteUTFBytes(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            if (bytes.Length > 0)
            {
                WriteBytes(bytes);

                if (IsDebugEnabled)
                {
                    StringBuilder debugStringBuilder = new StringBuilder();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                    }

                    DebugStringBuilder.AppendFormat("{0}--WriteUTFBytes:{1}\r\n\r\n", debugStringBuilder, value);
                }
            }
        }

        #endregion

        private void AddAMF0ObjectReference(object value)
        {
            amf0ObjectReferences.Add(value, amf0ObjectReferences.Count);
        }

        #region AMF0 Writer

        /// <summary>
        /// AMF0NullReader->5 6
        /// </summary>
        public void WriteNull()
        {
            WriteByte(AMF0TypeCode.Null);

            if (IsDebugEnabled)
            {
                DebugStringBuilder.AppendFormat("{0,0:X2}--WriteNull:{0}\r\n\r\n", AMF0TypeCode.Null);
            }
        }

        /// <summary>
        /// AMF0ReferenceReader->7
        /// </summary>
        /// <param name="value"></param>
        private void WriteReference(object value)
        {
            WriteByte(AMF0TypeCode.Reference);

            if (IsDebugEnabled)
            {
                DebugStringBuilder.AppendFormat("{0,0:X2}--WriteReference:{0}\r\n\r\n", AMF0TypeCode.Reference);
            }

            WriteShort(amf0ObjectReferences[value]);
        }

        /// <summary>
        /// 9
        /// </summary>
        private void WriteEndMarkup()
        {
            WriteShort(0);

            WriteByte(AMF0TypeCode.EndOfObject);

            if (IsDebugEnabled)
            {
                DebugStringBuilder.AppendFormat("{0,0:X2}--WriteEndMarkup:{0}\r\n\r\n", AMF0TypeCode.EndOfObject);
            }
        }

        /// <summary>
        /// AMF0StringReader->2
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            int length = Encoding.UTF8.GetByteCount(value);

            if (length < 65536)
            {
                WriteByte(AMF0TypeCode.String);

                WriteUTF(value);
            }
            else
            {
                WriteByte(AMF0TypeCode.LongString);

                WriteLongUTF(value);
            }
        }

        /// <summary>
        /// AMF0LongStringReader->12
        /// </summary>
        /// <param name="value"></param>
        private void WriteLongUTF(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            WriteInt32(bytes.Length);

            WriteBytes(bytes);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--WriteLongUTF:{1}\r\n\r\n", debugStringBuilder, value);
            }
        }

        /// <summary>
        /// AMF0DateTimeReader->11
        /// </summary>
        /// <param name="value"></param>
        public void WriteDateTime(DateTime value)
        {
            value = value.ToUniversalTime();

            DateTime timeStart = new DateTime(1970, 1, 1);

            TimeSpan span = value.Subtract(timeStart);

            WriteDouble(span.TotalMilliseconds);

            span = TimeZone.CurrentTimeZone.GetUtcOffset(value);

            //转换时区
            WriteShort(65056);
        }

        /// <summary>
        /// AMF0XmlReader->15
        /// </summary>
        /// <param name="value"></param>
        public void WriteXmlDocument(XmlDocument value)
        {
            if (value != null)
            {
                AddAMF0ObjectReference(value);

                WriteByte(AMF0TypeCode.Xml);

                string xml = value.DocumentElement.OuterXml;

                WriteLongUTF(xml);
            }
            else
            {
                WriteNull();
            }
        }

        /// <summary>
        /// AMF0ASObjectReader->3
        /// </summary>
        /// <param name="objectEncoding"></param>
        /// <param name="asObject"></param>
        public void WriteASObject(ObjectEncoding objectEncoding, ASObject asObject)
        {
            if (asObject != null)
            {
                AddAMF0ObjectReference(asObject);

                if (asObject.TypeName == null)
                {
                    WriteByte(AMF0TypeCode.ASObject);
                }
                else
                {
                    WriteByte(AMF0TypeCode.CustomClass);

                    WriteUTF(asObject.TypeName);
                }

                foreach (KeyValuePair<string, object> entry in asObject)
                {
                    WriteUTF(entry.Key);

                    WriteData(objectEncoding, entry.Value);
                }

                WriteEndMarkup();
            }
            else
            {
                WriteNull();
            }
        }

        public void WriteObject(ObjectEncoding objectEncoding, object obj)
        {
            if (obj != null)
            {
                AddAMF0ObjectReference(obj);

                WriteByte(AMF0TypeCode.CustomClass);

                Type type = obj.GetType();

                string customClass = type.FullName;

                customClass = GetCustomClass(customClass);

                WriteUTF(customClass);

                ClassDefinition classDefinition = GetClassDefinition(obj);

                IObjectProxy proxy = ObjectProxyRegistry.Instance.GetObjectProxy(type);

                for (int i = 0; i < classDefinition.MemberCount; i++)
                {
                    ClassMember classMember = classDefinition.Members[i];

                    object memberValue = proxy.GetValue(obj, classMember);

                    WriteUTF(classMember.Name);

                    WriteData(objectEncoding, memberValue);
                }

                WriteEndMarkup();
            }
            else
            {
                WriteNull();
            }
        }

        /// <summary>
        /// AMF0AssociativeArrayReader->8
        /// </summary>
        /// <param name="objectEncoding"></param>
        /// <param name="value"></param>
        public void WriteAssociativeArray(ObjectEncoding objectEncoding, IDictionary value)
        {
            if (value != null)
            {
                AddAMF0ObjectReference(value);

                WriteByte(AMF0TypeCode.AssociativeArray);

                WriteInt32(value.Count);

                foreach (DictionaryEntry entry in value)
                {
                    WriteUTF(entry.Key.ToString());

                    WriteData(objectEncoding, entry.Value);
                }

                WriteEndMarkup();
            }
            else
            {
                WriteNull();
            }
        }

        /// <summary>
        /// AMF0ArrayReader->10
        /// </summary>
        /// <param name="objectEcoding"></param>
        /// <param name="value"></param>
        public void WriteArray(ObjectEncoding objectEcoding, Array value)
        {
            if (value != null)
            {
                AddAMF0ObjectReference(value);

                WriteByte(AMF0TypeCode.Array);

                WriteInt32(value.Length);

                for (int i = 0; i < value.Length; i++)
                {
                    WriteData(objectEcoding, value.GetValue(i));
                }
            }
            else
            {
                WriteNull();
            }
        }

        public void WriteData(ObjectEncoding objectEncoding, object data)
        {
            if (data == null)
            {
                WriteNull();

                return;
            }

            Type type = data.GetType();

            if (amf0ObjectReferences.ContainsKey(data))
            {
                WriteReference(data);

                return;
            }

            IAMFWriter amfWriter = null;

            if (AmfWriterTable[0].ContainsKey(type))
            {
                amfWriter = AmfWriterTable[0][type] as IAMFWriter;
            }

            if (amfWriter == null)
            {
                if (type.BaseType != null)
                {
                    if (AmfWriterTable[0].ContainsKey(type.BaseType))
                    {
                        amfWriter = AmfWriterTable[0][type.BaseType] as IAMFWriter;
                    }
                }
            }

            if (amfWriter == null)
            {
                amfWriter = new AMF0ObjectWriter();
            }

            if (objectEncoding == ObjectEncoding.AMF0)
            {
                amfWriter.WriteData(this, data);
            }
            else
            {
                if (amfWriter.IsPrimitive)
                {
                    amfWriter.WriteData(this, data);
                }
                else
                {
                    WriteByte(AMF0TypeCode.AMF3Tag);

                    WriteAMF3Data(data);
                }
            }
        }

        #endregion

        public static string GetCustomClass(string customClass)
        {
            if (typeToCustomClass.ContainsKey(customClass))
            {
                return typeToCustomClass[customClass];
            }

            return customClass;
        }

        private ClassDefinition GetClassDefinition(object obj)
        {
            ClassDefinition classDefinition = null;

            if (obj is ASObject)
            {
                IObjectProxy proxy = ObjectProxyRegistry.Instance.GetObjectProxy(typeof(ASObject));

                classDefinition = proxy.GetClassDefinition((ASObject)obj);
            }
            else
            {
                IObjectProxy proxy = ObjectProxyRegistry.Instance.GetObjectProxy(obj.GetType());

                classDefinition = proxy.GetClassDefinition(obj);
            }

            return classDefinition;
        }

        #region AMF3 Writer

        /// <summary>
        /// 0 1
        /// </summary>
        public void WriteAMF3Null()
        {
            WriteByte(AMF3TypeCode.Null);

            if (IsDebugEnabled)
            {
                DebugStringBuilder.AppendFormat("{0,0:X2}--WriteAMF3Null:{0}\r\n\r\n", AMF3TypeCode.Null);
            }
        }

        /// <summary>
        /// 2 3
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3Bool(bool value)
        {
            WriteByte((byte)(value ? AMF3TypeCode.BooleanTrue : AMF3TypeCode.BooleanFalse));
        }

        /// <summary>
        /// 4
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3Int(int value)
        {
            if (value >= -268435456 && value <= 268435455)
            {
                WriteByte(AMF3TypeCode.Integer);

                WriteAMF3IntegerData(value);
            }
            else
            {
                WriteAMF3Double((double)value);
            }
        }

        private void WriteAMF3IntegerData(int value)
        {
            MemoryStream memoryStream = new MemoryStream();

            value &= 0x1fffffff;

            if (value < 0x80)
            {
                memoryStream.WriteByte((byte)value);
            }
            else if (value < 0x4000)
            {
                memoryStream.WriteByte((byte)(value >> 7 & 0x7f | 0x80));
                memoryStream.WriteByte((byte)(value & 0x7f));
            }
            else if (value < 0x200000)
            {
                memoryStream.WriteByte((byte)(value >> 14 & 0x7f | 0x80));
                memoryStream.WriteByte((byte)(value >> 7 & 0x7f | 0x80));
                memoryStream.WriteByte((byte)(value & 0x7f));
            }
            else
            {
                memoryStream.WriteByte((byte)(value >> 22 & 0x7f | 0x80));
                memoryStream.WriteByte((byte)(value >> 15 & 0x7f | 0x80));
                memoryStream.WriteByte((byte)(value >> 8 & 0x7f | 0x80));
                memoryStream.WriteByte((byte)(value & 0xff));
            }

            byte[] bytes = memoryStream.ToArray();

            WriteBytes(bytes);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--WriteAMF3IntegerData:{1}\r\n\r\n", debugStringBuilder, value);
            }
        }

        /// <summary>
        /// 5
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3Double(double value)
        {
            WriteByte(AMF3TypeCode.Number);

            WriteDouble(value);
        }

        /// <summary>
        /// 6
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3String(string value)
        {
            WriteByte(AMF3TypeCode.String);

            WriteAMF3UTF(value);
        }

        /// <summary>
        /// 7
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3XmlDocument(XmlDocument value)
        {
            WriteByte(AMF3TypeCode.Xml);

            string xml = string.Empty;

            if (value.DocumentElement != null && value.DocumentElement.OuterXml != null)
            {
                xml = value.DocumentElement.OuterXml;
            }

            if (xml == string.Empty)
            {
                WriteAMF3IntegerData(1);
            }
            else
            {
                if (!amf3ObjectReferences.ContainsKey(value))
                {
                    amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                    int length = Encoding.UTF8.GetByteCount(xml);

                    int handle = length;

                    handle = handle << 1;

                    handle = handle | 1;

                    WriteAMF3IntegerData(handle);

                    byte[] bytes = Encoding.UTF8.GetBytes(xml);

                    WriteBytes(bytes);

                    if (IsDebugEnabled)
                    {
                        StringBuilder debugStringBuilder = new StringBuilder();

                        for (int i = 0; i < bytes.Length; i++)
                        {
                            debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                        }

                        DebugStringBuilder.AppendFormat("{0}--WriteAMF3XmlDocument:Length:{1} \"{2}\"\r\n\r\n", debugStringBuilder, length, value);
                    }
                }
                else
                {
                    int handle = amf3ObjectReferences[value];

                    handle = handle << 1;

                    WriteAMF3IntegerData(handle);
                }
            }
        }

        /// <summary>
        /// 8
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3DateTime(DateTime value)
        {
            if (!amf3ObjectReferences.ContainsKey(value))
            {
                amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                int handle = 1;

                WriteAMF3IntegerData(handle);

                DateTime timeStart = new DateTime(1970, 1, 1);

                value = value.ToUniversalTime();

                TimeSpan span = value.Subtract(timeStart);

                WriteDouble(span.TotalMilliseconds);
            }
            else
            {
                int handle = amf3ObjectReferences[value];

                handle = handle << 1;

                WriteAMF3IntegerData(handle);
            }
        }

        /// <summary>
        /// 9
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3Array(Array value)
        {
            if (!amf3ObjectReferences.ContainsKey(value))
            {
                amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                int handle = value.Length;

                handle = handle << 1;

                handle = handle | 1;

                WriteAMF3IntegerData(handle);

                WriteAMF3UTF(string.Empty);

                for (int i = 0; i < value.Length; i++)
                {
                    WriteAMF3Data(value.GetValue(i));
                }
            }
            else
            {
                int handle = amf3ObjectReferences[value];

                handle = handle << 1;

                WriteAMF3IntegerData(handle);
            }
        }

        /// <summary>
        /// 10
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3Object(object value)
        {
            if (!amf3ObjectReferences.ContainsKey(value))
            {
                amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                ClassDefinition classDefinition = GetClassDefinition(value);

                if (ContainsClassDefinitionReferences(classDefinition))
                {
                    int handle = GetClassDefinitionReferencesIndex(classDefinition);

                    handle = handle << 2;

                    handle = handle | 1;

                    WriteAMF3IntegerData(handle);
                }
                else
                {
                    amf3ClassReferences.Add(classDefinition, amf3ClassReferences.Count);

                    int handle = classDefinition.MemberCount;

                    handle = handle << 1;

                    handle = handle | (classDefinition.IsDynamic ? 1 : 0);

                    handle = handle << 1;

                    handle = handle | (classDefinition.IsExternalizable ? 1 : 0);

                    handle = handle << 2;

                    handle = handle | 3;

                    WriteAMF3IntegerData(handle);

                    WriteAMF3UTF(classDefinition.ClassName);

                    for (int i = 0; i < classDefinition.MemberCount; i++)
                    {
                        string key = classDefinition.Members[i].Name;

                        WriteAMF3UTF(key);
                    }
                }

                if (classDefinition.IsExternalizable)
                {
                    if (value is IExternalizable)
                    {
                        IExternalizable externalizable = value as IExternalizable;

                        DataOutput dataOutput = new DataOutput(this);

                        externalizable.writeExternal(dataOutput);
                    }
                    else
                    {
                        throw new Exception("writeExternal Fail:" + classDefinition.ClassName);
                    }
                }
                else
                {
                    Type type = value.GetType();

                    IObjectProxy proxy = ObjectProxyRegistry.Instance.GetObjectProxy(type);

                    for (int i = 0; i < classDefinition.MemberCount; i++)
                    {
                        object memberValue = proxy.GetValue(value, classDefinition.Members[i]);

                        WriteAMF3Data(memberValue);
                    }

                    if (classDefinition.IsDynamic)
                    {
                        IDictionary dictionary = value as IDictionary;

                        foreach (DictionaryEntry entry in dictionary)
                        {
                            WriteAMF3UTF(entry.Key.ToString());

                            WriteAMF3Data(entry.Value);
                        }

                        WriteAMF3UTF(string.Empty);
                    }
                }
            }
            else
            {
                int handle = amf3ObjectReferences[value];

                handle = handle << 1;

                WriteAMF3IntegerData(handle);
            }
        }

        /// <summary>
        /// 12
        /// </summary>
        /// <param name="byteArray"></param>
        public void WriteByteArray(ByteArray byteArray)
        {
            amf3ObjectReferences.Add(byteArray, amf3ObjectReferences.Count);

            WriteByte(AMF3TypeCode.ByteArray);

            int handle = byteArray.length;

            handle = handle << 1;

            handle = handle | 1;

            WriteAMF3IntegerData(handle);

            WriteBytes(byteArray.memoryStream.ToArray());
        }

        /// <summary>
        /// 13
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3IntVector(IList<int> value)
        {
            if (!amf3ObjectReferences.ContainsKey(value))
            {
                amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                int handle = value.Count;

                handle = handle << 1;

                handle = handle | 1;

                WriteAMF3IntegerData(handle);

                WriteAMF3IntegerData(value.IsReadOnly ? 1 : 0);

                for (int i = 0; i < value.Count; i++)
                {
                    WriteInt32(value[i]);
                }
            }
            else
            {
                int handle = amf3ObjectReferences[value];

                handle = handle << 1;

                WriteAMF3IntegerData(handle);
            }
        }

        /// <summary>
        /// 14
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3UIntVector(IList<uint> value)
        {
            if (!amf3ObjectReferences.ContainsKey(value))
            {
                amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                int handle = value.Count;

                handle = handle << 1;

                handle = handle | 1;

                WriteAMF3IntegerData(handle);

                WriteAMF3IntegerData(value.IsReadOnly ? 1 : 0);

                for (int i = 0; i < value.Count; i++)
                {
                    WriteInt32((int)value[i]);
                }
            }
            else
            {
                int handle = amf3ObjectReferences[value];

                handle = handle << 1;

                WriteAMF3IntegerData(handle);
            }
        }

        /// <summary>
        /// 15
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3DoubleVector(IList<double> value)
        {
            if (!amf3ObjectReferences.ContainsKey(value))
            {
                amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                int handle = value.Count;

                handle = handle << 1;

                handle = handle | 1;

                WriteAMF3IntegerData(handle);

                WriteAMF3IntegerData(value.IsReadOnly ? 1 : 0);

                for (int i = 0; i < value.Count; i++)
                {
                    WriteDouble(value[i]);
                }
            }
            else
            {
                int handle = amf3ObjectReferences[value];

                handle = handle << 1;

                WriteAMF3IntegerData(handle);
            }
        }

        /// <summary>
        /// 16
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3ObjectVector(IList<string> value)
        {
            WriteAMF3ObjectVector(string.Empty, value as IList);
        }

        /// <summary>
        /// 16
        /// </summary>
        /// <param name="value"></param>
        public void WriteAMF3ObjectVector(IList<bool> value)
        {
            WriteAMF3ObjectVector(string.Empty, value as IList);
        }

        /// <summary>
        /// 16
        /// </summary>
        /// <param name="typeIdentifier"></param>
        /// <param name="value"></param>
        private void WriteAMF3ObjectVector(string typeIdentifier, IList value)
        {
            if (!amf3ObjectReferences.ContainsKey(value))
            {
                amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                int handle = value.Count;

                handle = handle << 1;

                handle = handle | 1;

                WriteAMF3IntegerData(handle);

                WriteAMF3IntegerData(value.IsReadOnly ? 1 : 0);

                WriteAMF3String(typeIdentifier);

                for (int i = 0; i < value.Count; i++)
                {
                    WriteAMF3Data(value[i]);
                }
            }
            else
            {
                int handle = amf3ObjectReferences[value];

                handle = handle << 1;

                WriteAMF3IntegerData(handle);
            }
        }

        public void WriteAMF3UTF(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                WriteAMF3IntegerData(1);
            }
            else
            {
                if (!amf3StringReferences.ContainsKey(value))
                {
                    amf3StringReferences.Add(value, amf3StringReferences.Count);

                    int length = Encoding.UTF8.GetByteCount(value);

                    int handle = length;

                    handle = handle << 1;

                    handle = handle | 1;

                    WriteAMF3IntegerData(handle);

                    byte[] bytes = Encoding.UTF8.GetBytes(value);

                    WriteBytes(bytes);

                    if (IsDebugEnabled)
                    {
                        StringBuilder debugStringBuilder = new StringBuilder();

                        for (int i = 0; i < bytes.Length; i++)
                        {
                            debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                        }

                        DebugStringBuilder.AppendFormat("{0}--WriteAMF3UTF:Length:{1} \"{2}\"\r\n\r\n", debugStringBuilder, length, value);
                    }
                }
                else
                {
                    int handle = amf3StringReferences[value];

                    handle = handle << 1;

                    WriteAMF3IntegerData(handle);
                }
            }
        }

        public void WriteAMF3Data(object data)
        {
            if (data == null)
            {
                WriteAMF3Null();

                return;
            }

            if (data is DBNull)
            {
                WriteAMF3Null();

                return;
            }

            Type type = data.GetType();

            IAMFWriter amfWriter = null;

            if (AmfWriterTable[3].ContainsKey(type))
            {
                amfWriter = AmfWriterTable[3][type] as IAMFWriter;
            }

            if (amfWriter == null)
            {
                if (type.BaseType != null)
                {
                    if (AmfWriterTable[3].ContainsKey(type.BaseType))
                    {
                        amfWriter = AmfWriterTable[3][type.BaseType] as IAMFWriter;
                    }
                }
            }

            if (amfWriter == null)
            {
                amfWriter = new AMF3ObjectWriter();
            }

            amfWriter.WriteData(this, data);
        }

        //public bool ContainsClassDefinitionReferences(ClassDefinition classDefinition)
        //{
        //    bool result = true;

        //    if (amf3ClassReferences.Count > 0)
        //    {
        //        foreach (var item in amf3ClassReferences)
        //        {
        //            result = true;

        //            if ((item.Key.ClassName == classDefinition.ClassName) && (item.Key.IsDynamic == classDefinition.IsDynamic) && (item.Key.IsExternalizable == classDefinition.IsExternalizable) && (item.Key.IsTypedObject == classDefinition.IsTypedObject) && (item.Key.MemberCount == classDefinition.MemberCount))
        //            {
        //                for (int i = 0; i < item.Key.MemberCount; i++)
        //                {
        //                    if (item.Key.Members[i].Name != classDefinition.Members[i].Name)
        //                    {
        //                        result = false;

        //                        continue;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                result = false;
        //            }

        //            if (result)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        result = false;
        //    }

        //    return result;
        //}

        //public int GetClassDefinitionReferencesIndex(ClassDefinition classDefinition)
        //{
        //    int value = 0;

        //    bool result = true;

        //    foreach (var item in amf3ClassReferences)
        //    {
        //        result = true;

        //        if ((item.Key.ClassName == classDefinition.ClassName) && (item.Key.IsDynamic == classDefinition.IsDynamic) && (item.Key.IsExternalizable == classDefinition.IsExternalizable) && (item.Key.IsTypedObject == classDefinition.IsTypedObject) && (item.Key.MemberCount == classDefinition.MemberCount))
        //        {
        //            for (int i = 0; i < item.Key.MemberCount; i++)
        //            {
        //                if (item.Key.Members[i].Name != classDefinition.Members[i].Name)
        //                {
        //                    result = false;

        //                    continue;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            result = false;
        //        }

        //        if (result)
        //        {
        //            value = item.Value;

        //            break;
        //        }
        //    }

        //    return value;
        //}

        public bool ContainsClassDefinitionReferences(ClassDefinition classDefinition)
        {
            bool result = false;

            foreach (var item in amf3ClassReferences)
            {
                if ((item.Key.ClassName == classDefinition.ClassName) && (item.Key.IsDynamic == classDefinition.IsDynamic) && (item.Key.IsExternalizable == classDefinition.IsExternalizable) && (item.Key.MemberCount == classDefinition.MemberCount))
                {
                    bool flag = true;

                    for (int i = 0; i < item.Key.MemberCount; i++)
                    {
                        if (item.Key.Members[i].Name != classDefinition.Members[i].Name)
                        {
                            flag = false;

                            break;
                        }
                    }

                    result = flag;

                    if (result)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public int GetClassDefinitionReferencesIndex(ClassDefinition classDefinition)
        {
            int value = 0;

            foreach (var item in amf3ClassReferences)
            {
                bool flag = true;

                if ((item.Key.ClassName == classDefinition.ClassName) && (item.Key.IsDynamic == classDefinition.IsDynamic) && (item.Key.IsExternalizable == classDefinition.IsExternalizable) && (item.Key.MemberCount == classDefinition.MemberCount))
                {
                    for (int i = 0; i < item.Key.MemberCount; i++)
                    {
                        if (item.Key.Members[i].Name != classDefinition.Members[i].Name)
                        {
                            flag = false;

                            break; ;
                        }
                    }
                }
                else
                {
                    flag = false;
                }

                if (flag)
                {
                    value = item.Value;

                    break;
                }
            }

            return value;
        }

        public void WriteAMF3Array(IList value)
        {
            if (!amf3ObjectReferences.ContainsKey(value))
            {
                amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                int handle = value.Count;

                handle = handle << 1;

                handle = handle | 1;

                WriteAMF3IntegerData(handle);

                WriteAMF3UTF(string.Empty);

                for (int i = 0; i < value.Count; i++)
                {
                    WriteAMF3Data(value[i]);
                }
            }
            else
            {
                int handle = amf3ObjectReferences[value];

                handle = handle << 1;

                WriteAMF3IntegerData(handle);
            }
        }

        public void WriteAMF3AssociativeArray(IDictionary value)
        {
            if (!amf3ObjectReferences.ContainsKey(value))
            {
                amf3ObjectReferences.Add(value, amf3ObjectReferences.Count);

                WriteAMF3IntegerData(1);

                foreach (DictionaryEntry entry in value)
                {
                    WriteAMF3UTF(entry.Key.ToString());

                    WriteAMF3Data(entry.Value);
                }

                WriteAMF3UTF(string.Empty);
            }
            else
            {
                int handle = amf3ObjectReferences[value];

                handle = handle << 1;

                WriteAMF3IntegerData(handle);
            }
        }

        #endregion
    }
}
