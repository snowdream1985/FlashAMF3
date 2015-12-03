using flash.utils;
using FlashFx.AMF3;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlashFx.IO
{
    public class ASObjectProxy : IObjectProxy
    {
        public bool GetIsExternalizable(object instance)
        {
            return false;
        }

        public bool GetIsDynamic(object instance)
        {
            if (instance != null)
            {
                if (instance is ASObject)
                {
                    return (instance as ASObject).IsTypedObject;
                }

                throw new Exception("GetIsDynamic is not ASObject");
            }

            throw new NullReferenceException();
        }

        public ClassDefinition GetClassDefinition(object instance)
        {
            if (instance is ASObject)
            {
                ClassDefinition classDefinition;

                ASObject aso = instance as ASObject;

                ClassMember[] classMemberList = new ClassMember[aso.Count];

                int i = 0;

                foreach (KeyValuePair<string, object> entry in aso)
                {
                    ClassMember classMember = new ClassMember(entry.Key, BindingFlags.Default, MemberTypes.Custom, null);

                    classMemberList[i] = classMember;

                    i++;
                }

                string customClassName = aso.TypeName;

                classDefinition = new ClassDefinition(customClassName, classMemberList, false, false);

                return classDefinition;
            }

            throw new Exception("GetClassDefinition is not ASObject");
        }

        public object GetValue(object instance, ClassMember member)
        {
            if (instance is ASObject)
            {
                ASObject aso = instance as ASObject;

                if (aso.ContainsKey(member.Name))
                {
                    return aso[member.Name];
                }

                throw new Exception(string.Format("Member:{0} not found in ASObject", member.Name));
            }

            throw new Exception("GetValue instance is not ASObject");
        }

        public void SetValue(object instance, ClassMember member, object value)
        {
            if (instance is ASObject)
            {
                ASObject aso = instance as ASObject;

                aso[member.Name] = value;
            }

            throw new Exception("SetValue instance is not ASObject");
        }
    }
}
