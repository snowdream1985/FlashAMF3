using System;
using System.Xml;
using System.Collections;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using FlashFx.AMF3;
using FlashFx.IO.Readers;
using flash.utils;

namespace FlashFx.IO
{
    public class AMFReader : BinaryReader
    {
        /// <summary>
        /// AMF0 Object 引用
        /// </summary>
        public List<object> amf0ObjectReferences;

        /// <summary>
        /// AMF3 Object 引用
        /// </summary>
        public List<object> amf3ObjectReferences;

        /// <summary>
        /// AMF3 String 引用
        /// </summary>
        public List<string> amf3StringReferences;

        /// <summary>
        /// AMF3 Class 引用
        /// </summary>
        public List<ClassDefinition> amf3ClassReferences;

        /// <summary>
        /// AMF类型表
        /// </summary>
        private static IAMFReader[][] AmfTypeTable;

        /// <summary>
        /// 调试内存流
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

        static AMFReader()
        {
            IAMFReader[] amf0Readers = new IAMFReader[]
			{
				new AMF0NumberReader(), /*0*/
				new AMF0BooleanReader(), /*1*/
				new AMF0StringReader(), /*2*/
				new AMF0ASObjectReader(), /*3*/
				new MovieclipMarker(), /*4*/
				new AMF0NullReader(), /*5*/
				new AMF0NullReader(), /*6*/
				new AMF0ReferenceReader(), /*7*/
				new AMF0AssociativeArrayReader(), /*8*/
				new AMFUnknownTagReader(), /*9*/
				new AMF0ArrayReader(), /*10*/
				new AMF0DateTimeReader(), /*11*/
				new AMF0LongStringReader(), /*12*/
				new UnsupportedMarker(),/*13*/
				new AMFUnknownTagReader(),/*14*/
				new AMF0XmlReader(), /*15*/
                new AMF0ObjectReader(), /*16*/
				new AMF0AMF3TagReader() /*17*/
			};

            IAMFReader[] amf3Readers = new IAMFReader[]
			{
				new AMF3NullReader(), /*0*/
				new AMF3NullReader(), /*1*/
				new AMF3BooleanFalseReader(), /*2*/
				new AMF3BooleanTrueReader(), /*3*/
				new AMF3IntegerReader(), /*4*/
				new AMF3NumberReader(), /*5*/
				new AMF3StringReader(), /*6*/
				new AMF3XmlReader(), /*7*/
				new AMF3DateTimeReader(), /*8*/
				new AMF3ArrayReader(),  /*9*/
                new AMF3ObjectReader(), /*10*/
				new AMF3XmlReader(), /*11*/
				new AMF3ByteArrayReader(), /*12*/
				new AMF3IntVectorReader(), /*13*/
				new AMF3UIntVectorReader(), /*14*/
				new AMF3DoubleVectorReader(), /*15*/
				new AMF3ObjectVectorReader(), /*16*/
				new AMFUnknownTagReader()/*17*/
			};

            AmfTypeTable = new IAMFReader[4][] { amf0Readers, null, null, amf3Readers };
        }

        public AMFReader(Stream stream)
            : base(stream)
        {
            Reset();
        }

        public void Reset()
        {
            amf0ObjectReferences = new List<object>();

            amf3ObjectReferences = new List<object>();

            amf3StringReferences = new List<string>();

            amf3ClassReferences = new List<ClassDefinition>();

            //调试
            DebugMemoryStream = new MemoryStream();

            DebugStringBuilder = new StringBuilder();

            IsDebugEnabled = false;
        }

        #region IDataInput Method

        public override bool ReadBoolean()
        {
            bool value = base.ReadBoolean();

            if (IsDebugEnabled)
            {
                if (value)
                {
                    DebugMemoryStream.WriteByte(1);

                    DebugStringBuilder.AppendFormat("01--ReadBoolean:{0}\r\n\r\n", value);
                }
                else
                {
                    DebugMemoryStream.WriteByte(0);

                    DebugStringBuilder.AppendFormat("00--ReadBoolean:{0}\r\n\r\n", value);
                }
            }

            return value;
        }

        public override byte ReadByte()
        {
            byte value = base.ReadByte();

            if (IsDebugEnabled)
            {
                DebugMemoryStream.WriteByte(value);

                DebugStringBuilder.AppendFormat("{0,0:X2}--ReadByte:{0}\r\n\r\n", value);
            }

            return value;
        }

        public override byte[] ReadBytes(int count)
        {
            byte[] value = base.ReadBytes(count);

            if (IsDebugEnabled)
            {
                DebugMemoryStream.Write(value, 0, value.Length);

                for (int i = 0; i < value.Length; i++)
                {
                    DebugStringBuilder.AppendFormat("{0,0:X2} ", value[i]);
                }

                DebugStringBuilder.Append("--ReadBytes\r\n\r\n");
            }

            return value;
        }

        public override short ReadInt16()
        {
            byte[] bytes = ReadBytes(2);

            short value = (short)((bytes[0] << 8) | bytes[1]);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--ReadInt16:{1}\r\n\r\n", debugStringBuilder, value);
            }

            return value;
        }

        public override ushort ReadUInt16()
        {
            byte[] bytes = ReadBytes(2);

            ushort value = (ushort)(((bytes[0] & 0xff) << 8) | (bytes[1] & 0xff));

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--ReadUInt16:{1}\r\n\r\n", debugStringBuilder, value);
            }

            return value;
        }

        public override int ReadInt32()
        {
            byte[] bytes = ReadBytes(4);

            int value = (int)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--ReadInt32:{1}\r\n\r\n", debugStringBuilder, value);
            }

            return value;
        }

        public override double ReadDouble()
        {
            byte[] bytes = ReadBytes(8);

            byte[] reverse = new byte[8];

            for (int i = 7, j = 0; i >= 0; i--, j++)
            {
                reverse[j] = bytes[i];
            }

            double value = BitConverter.ToDouble(reverse, 0);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--ReadDouble:{1}\r\n\r\n", debugStringBuilder, value);
            }

            return value;
        }

        public override float ReadSingle()
        {
            byte[] bytes = ReadBytes(4);

            byte[] invertedBytes = new byte[4];

            for (int i = 3, j = 0; i >= 0; i--, j++)
            {
                invertedBytes[j] = bytes[i];
            }

            float value = BitConverter.ToSingle(invertedBytes, 0);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", bytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--ReadFloat/ReadSingle:{1}\r\n\r\n", debugStringBuilder, value);
            }

            return value;
        }

        public override string ReadString()
        {
            int length = ReadUInt16();

            return ReadUTFBytes(length);
        }

        public string ReadUTFBytes(int length)
        {
            if (length == 0)
            {
                return string.Empty;
            }

            byte[] encodedBytes = ReadBytes(length);

            string decodedString = Encoding.UTF8.GetString(encodedBytes);

            if (IsDebugEnabled)
            {
                StringBuilder debugStringBuilder = new StringBuilder();

                for (int i = 0; i < encodedBytes.Length; i++)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", encodedBytes[i]);
                }

                DebugStringBuilder.AppendFormat("{0}--ReadUTFBytes:Length:{1} \"{2}\"\r\n\r\n", debugStringBuilder, length, decodedString);
            }

            return decodedString;
        }

        #endregion

        #region Reference

        public void AddAMF0ObjectReference(object instance)
        {
            amf0ObjectReferences.Add(instance);
        }

        public object ReadAMF0ObjectReference(int index)
        {
            return amf0ObjectReferences[index];
        }

        public void AddAMF3ObjectReference(object instance)
        {
            amf3ObjectReferences.Add(instance);
        }

        public object ReadAMF3ObjectReference(int index)
        {
            return amf3ObjectReferences[index];
        }

        public void AddAMF3StringReference(string str)
        {
            amf3StringReferences.Add(str);
        }

        public string ReadAMF3StringReference(int index)
        {
            return amf3StringReferences[index];
        }

        public void AddAMF3ClassReference(ClassDefinition classDefinition)
        {
            amf3ClassReferences.Add(classDefinition);
        }

        public ClassDefinition ReadAMF3ClassReference(int index)
        {
            return amf3ClassReferences[index];
        }

        #endregion

        #region AMF0

        public object ReadData()
        {
            byte typeCode = ReadByte();

            return ReadData(typeCode);
        }

        private object ReadData(byte typeMarker)
        {
            if (IsDebugEnabled)
            {
                IAMFReader iAMFReader = AmfTypeTable[0][typeMarker];

                string typename = iAMFReader.GetType().Name;

                DebugStringBuilder.AppendFormat("{0,0:X2}--ReadData:{1} Start\r\n\r\n", typeMarker, typename);
            }

            object value = AmfTypeTable[0][typeMarker].ReadData(this);

            if (IsDebugEnabled)
            {
                IAMFReader iAMFReader = AmfTypeTable[0][typeMarker];

                string typename = iAMFReader.GetType().Name;

                DebugStringBuilder.AppendFormat("{0,0:X2}--ReadData:{1} End\r\n\r\n", typeMarker, typename);
            }

            return value;
        }

        public object ReadObject()
        {
            string typeIdentifier = ReadString();

            object instance = ObjectFactory.CreateInstance(typeIdentifier);

            if (instance != null)
            {
                if (IsDebugEnabled)
                {
                    DebugStringBuilder.AppendFormat("Type Load:{0} Start\r\n\r\n", typeIdentifier);
                }

                AddAMF0ObjectReference(instance);

                string key = ReadString();

                for (byte typeCode = ReadByte(); typeCode != AMF0TypeCode.EndOfObject; typeCode = ReadByte())
                {
                    object value = ReadData(typeCode);

                    SetMember(instance, key, value);

                    key = ReadString();
                }

                if (IsDebugEnabled)
                {
                    DebugStringBuilder.AppendFormat("Type Load:{0} End\r\n\r\n", typeIdentifier);
                }

                return instance;
            }
            else
            {
                if (IsDebugEnabled)
                {
                    DebugStringBuilder.AppendFormat("Type Load ASObject:{0} Start\r\n\r\n", typeIdentifier);
                }

                ASObject asObject = ReadASObject();

                asObject.TypeName = typeIdentifier;

                if (IsDebugEnabled)
                {
                    DebugStringBuilder.AppendFormat("Type Load ASObject:{0} End\r\n\r\n", typeIdentifier);
                }

                return asObject;
            }
        }

        public ASObject ReadASObject()
        {
            ASObject asObject = new ASObject();

            AddAMF0ObjectReference(asObject);

            string key = ReadString();

            for (byte typeCode = ReadByte(); typeCode != AMF0TypeCode.EndOfObject; typeCode = ReadByte())
            {
                asObject[key] = ReadData(typeCode);

                key = ReadString();
            }

            return asObject;
        }

        public Dictionary<string, object> ReadAssociativeArray()
        {
            int length = ReadInt32();

            Dictionary<string, object> result = new Dictionary<string, object>(length);

            AddAMF0ObjectReference(result);

            string key = ReadString();

            for (byte typeCode = ReadByte(); typeCode != AMF0TypeCode.EndOfObject; typeCode = ReadByte())
            {
                object value = ReadData(typeCode);

                result.Add(key, value);

                key = ReadString();
            }

            return result;
        }

        public IList ReadArray()
        {
            int length = ReadInt32();

            object[] array = new object[length];

            AddAMF0ObjectReference(array);

            for (int i = 0; i < length; i++)
            {
                array[i] = ReadData();
            }

            return array;
        }

        public DateTime ReadDateTime()
        {
            double milliseconds = ReadDouble();

            DateTime startDateTime = new DateTime(1970, 1, 1);

            DateTime date = startDateTime.AddMilliseconds(milliseconds);

            int tmp = ReadUInt16();

            if (tmp > 720)
            {
                tmp = (65536 - tmp);
            }

            int tz = tmp / 60;

            date = date.AddHours(tz);

            return date;
        }

        public XmlDocument ReadXmlDocument()
        {
            int length = ReadInt32();

            string text = ReadUTFBytes(length);

            XmlDocument document = new XmlDocument();

            if (!string.IsNullOrEmpty(text))
            {
                document.LoadXml(text);
            }

            return document;
        }

        #endregion

        #region AMF3

        public object ReadAMF3Data()
        {
            byte typeCode = ReadByte();

            return ReadAMF3Data(typeCode);
        }

        public object ReadAMF3Data(byte typeMarker)
        {
            if (IsDebugEnabled)
            {
                IAMFReader iAMFReader = AmfTypeTable[3][typeMarker];

                string typename = iAMFReader.GetType().Name;

                DebugStringBuilder.AppendFormat("{0,0:X2}--ReadAMF3Data:{1} Start\r\n\r\n", typeMarker, typename);
            }

            object value = AmfTypeTable[3][typeMarker].ReadData(this);

            if (IsDebugEnabled)
            {
                IAMFReader iAMFReader = AmfTypeTable[3][typeMarker];

                string typename = iAMFReader.GetType().Name;

                DebugStringBuilder.AppendFormat("{0,0:X2}--ReadAMF3Data:{1} End\r\n\r\n", typeMarker, typename);
            }

            return value;
        }

        public int ReadAMF3IntegerData()
        {
            int acc = ReadByte();

            StringBuilder debugStringBuilder = new StringBuilder();

            if (IsDebugEnabled)
            {
                debugStringBuilder.AppendFormat("{0,0:X2} ", acc);
            }

            int tmp;

            if (acc < 128)
            {
                if (IsDebugEnabled)
                {
                    DebugStringBuilder.AppendFormat("{0}--ReadAMF3IntegerData:{1}\r\n\r\n", debugStringBuilder, acc);
                }

                return acc;
            }
            else
            {
                acc = (acc & 0x7f) << 7;

                tmp = ReadByte();

                if (IsDebugEnabled)
                {
                    debugStringBuilder.AppendFormat("{0,0:X2} ", tmp);
                }

                if (tmp < 128)
                {
                    acc = acc | tmp;
                }
                else
                {
                    acc = (acc | tmp & 0x7f) << 7;

                    tmp = ReadByte();

                    if (IsDebugEnabled)
                    {
                        debugStringBuilder.AppendFormat("{0,0:X2} ", tmp);
                    }

                    if (tmp < 128)
                    {
                        acc = acc | tmp;
                    }
                    else
                    {
                        acc = (acc | tmp & 0x7f) << 8;

                        tmp = ReadByte();

                        if (IsDebugEnabled)
                        {
                            debugStringBuilder.AppendFormat("{0,0:X2} ", tmp);
                        }

                        acc = acc | tmp;
                    }
                }
            }

            int mask = 1 << 28; // mask

            int value = -(acc & mask) | acc;

            if (IsDebugEnabled)
            {
                DebugStringBuilder.AppendFormat("{0}--ReadAMF3IntegerData:{1}\r\n\r\n", debugStringBuilder, value);
            }

            return value;
        }

        public DateTime ReadAMF3Date()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                double milliseconds = ReadDouble();

                DateTime startDateTime = new DateTime(1970, 1, 1);

                DateTime date = startDateTime.AddMilliseconds(milliseconds);

                //时区+8
                date = date.AddHours(8);

                AddAMF3ObjectReference(date);

                return date;
            }
            else
            {
                return (DateTime)ReadAMF3ObjectReference(handle);
            }
        }

        public string ReadAMF3String()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                int length = handle;

                if (length == 0)
                {
                    return string.Empty;
                }

                string str = ReadUTFBytes(length);

                AddAMF3StringReference(str);

                return str;
            }
            else
            {
                return ReadAMF3StringReference(handle);
            }
        }

        public XmlDocument ReadAMF3XmlDocument()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            string xml = string.Empty;

            if (inline)
            {
                if (handle > 0)
                {
                    xml = ReadUTFBytes(handle);
                }

                AddAMF3ObjectReference(xml);
            }
            else
            {
                xml = (string)ReadAMF3ObjectReference(handle);
            }

            XmlDocument xmlDocument = new XmlDocument();

            if (!string.IsNullOrEmpty(xml))
            {
                xmlDocument.LoadXml(xml);
            }

            return xmlDocument;
        }

        public ByteArray ReadAMF3ByteArray()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                int length = handle;

                byte[] buffer = ReadBytes(length);

                ByteArray byteArray = new ByteArray(buffer);

                AddAMF3ObjectReference(byteArray);

                return byteArray;
            }
            else
            {
                return (ByteArray)ReadAMF3ObjectReference(handle);
            }
        }

        public object ReadAMF3Array()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                Dictionary<string, object> hashtable = null;

                string key = ReadAMF3String();

                while (!string.IsNullOrEmpty(key))
                {
                    if (hashtable == null)
                    {
                        hashtable = new Dictionary<string, object>();

                        AddAMF3ObjectReference(hashtable);
                    }

                    object value = ReadAMF3Data();

                    hashtable.Add(key, value);

                    key = ReadAMF3String();
                }

                if (hashtable == null)
                {
                    object[] array = new object[handle];

                    AddAMF3ObjectReference(array);

                    for (int i = 0; i < handle; i++)
                    {
                        byte typeCode = ReadByte();

                        object value = ReadAMF3Data(typeCode);

                        array[i] = value;
                    }

                    return array;
                }
                else
                {
                    for (int i = 0; i < handle; i++)
                    {
                        object value = ReadAMF3Data();

                        hashtable.Add(i.ToString(), value);
                    }

                    return hashtable;
                }
            }
            else
            {
                return ReadAMF3ObjectReference(handle);
            }
        }

        public IList<int> ReadAMF3IntVector()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                List<int> list = new List<int>(handle);

                AddAMF3ObjectReference(list);

                //fixed
                ReadAMF3IntegerData();

                for (int i = 0; i < handle; i++)
                {
                    list.Add(ReadInt32());
                }

                return list;
            }
            else
            {
                return (List<int>)ReadAMF3ObjectReference(handle);
            }
        }

        public IList<uint> ReadAMF3UIntVector()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                List<uint> list = new List<uint>(handle);

                AddAMF3ObjectReference(list);

                //fixed
                ReadAMF3IntegerData();

                for (int i = 0; i < handle; i++)
                {
                    list.Add((uint)ReadInt32());
                }

                return list;
            }
            else
            {
                return (List<uint>)ReadAMF3ObjectReference(handle);
            }
        }

        public IList<double> ReadAMF3DoubleVector()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                List<double> list = new List<double>(handle);

                AddAMF3ObjectReference(list);

                //fixed
                ReadAMF3IntegerData();

                for (int i = 0; i < handle; i++)
                {
                    list.Add(ReadDouble());
                }

                return list;
            }
            else
            {
                return (List<double>)ReadAMF3ObjectReference(handle);
            }
        }

        public IList ReadAMF3ObjectVector()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                List<object> list = new List<object>(handle);

                AddAMF3ObjectReference(list);

                //fixed
                ReadAMF3IntegerData();

                string typeIdentifier = ReadAMF3String();

                for (int i = 0; i < handle; i++)
                {
                    byte typeCode = ReadByte();

                    object obj = ReadAMF3Data(typeCode);

                    list.Add(obj);
                }

                return list;
            }
            else
            {
                return (IList)ReadAMF3ObjectReference(handle);
            }
        }

        private ClassDefinition ReadClassDefinition(int handle)
        {
            ClassDefinition classDefinition = null;

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                string typeIdentifier = ReadAMF3String();

                bool externalizable = ((handle & 1) != 0);

                handle = handle >> 1;

                bool dynamic = ((handle & 1) != 0);

                handle = handle >> 1;

                ClassMember[] members = new ClassMember[handle];

                for (int i = 0; i < handle; i++)
                {
                    string name = ReadAMF3String();

                    ClassMember classMember = new ClassMember(name, BindingFlags.Default, MemberTypes.Custom, null);

                    members[i] = classMember;
                }

                classDefinition = new ClassDefinition(typeIdentifier, members, externalizable, dynamic);

                AddAMF3ClassReference(classDefinition);
            }
            else
            {
                classDefinition = ReadAMF3ClassReference(handle);
            }

            return classDefinition;
        }

        private object ReadAMF3Object(ClassDefinition classDefinition)
        {
            object instance = null;

            if (!string.IsNullOrEmpty(classDefinition.ClassName))
            {
                instance = ObjectFactory.CreateInstance(classDefinition.ClassName);
            }
            else
            {
                instance = new ASObject();
            }

            if (instance == null)
            {
                instance = new ASObject(classDefinition.ClassName);
            }

            AddAMF3ObjectReference(instance);

            if (classDefinition.IsExternalizable)
            {
                if (instance is IExternalizable)
                {
                    IExternalizable externalizable = instance as IExternalizable;

                    DataInput dataInput = new DataInput(this);

                    externalizable.readExternal(dataInput);
                }
                else
                {
                    throw new Exception("readExternal Fail:" + classDefinition.ClassName);
                }
            }
            else
            {
                for (int i = 0; i < classDefinition.MemberCount; i++)
                {
                    string key = classDefinition.Members[i].Name;

                    object value = ReadAMF3Data();

                    SetMember(instance, key, value);
                }

                if (classDefinition.IsDynamic)
                {
                    string key = ReadAMF3String();

                    while (!string.IsNullOrEmpty(key))
                    {
                        object value = ReadAMF3Data();

                        SetMember(instance, key, value);

                        key = ReadAMF3String();
                    }
                }
            }

            return instance;
        }

        public object ReadAMF3Object()
        {
            int handle = ReadAMF3IntegerData();

            bool inline = ((handle & 1) != 0);

            handle = handle >> 1;

            if (inline)
            {
                ClassDefinition classDefinition = ReadClassDefinition(handle);

                return ReadAMF3Object(classDefinition);
            }
            else
            {
                return ReadAMF3ObjectReference(handle);
            }
        }

        #endregion AMF3

        private void SetMember(object instance, string memberName, object value)
        {
            if (instance is ASObject)
            {
                ((ASObject)instance)[memberName] = value;
            }
            else
            {
                if (value != null)
                {
                    Type type = instance.GetType();

                    FieldInfo fieldInfo = type.GetField(memberName, BindingFlags.Public | BindingFlags.Instance);

                    try
                    {
                        if (fieldInfo != null)
                        {
                            if (value.GetType() != fieldInfo.FieldType)
                            {
                                value = Convert.ChangeType(value, fieldInfo.FieldType);
                            }

                            fieldInfo.SetValue(instance, value);
                        }
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }
                else
                {
                    //null 数据类型 不赋值
                }
            }
        }
    }
}
