/*
�޸�ʱ��:2015.12.03

������QQ:5115147

��ע:
��.Net 4.0�����ϰ汾(Zlibʹ��.Net�����㷨) 
*/

using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;

namespace flash.utils
{
    [Serializable]
    public class ASObject : Dictionary<string, object>
    {
        private string _typeName;

        public ASObject()
        {

        }

        public ASObject(string typeName)
        {
            _typeName = typeName;
        }

        public ASObject(IDictionary<string, object> dictionary)
            : base(dictionary)
        {

        }

        public ASObject(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public string TypeName
        {
            get { return _typeName; }

            set { _typeName = value; }
        }

        public bool IsTypedObject { get { return !string.IsNullOrEmpty(_typeName); } }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (IsTypedObject)
            {
                stringBuilder.Append(TypeName);
            }

            stringBuilder.Append("{");

            int i = 0;

            foreach (KeyValuePair<string, object> entry in this)
            {
                if (i != 0)
                {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(entry.Key);

                i++;
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}
