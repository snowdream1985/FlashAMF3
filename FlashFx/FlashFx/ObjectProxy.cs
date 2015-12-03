using flash.utils;
using FlashFx.AMF3;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlashFx.IO
{
    public class ObjectProxy : IObjectProxy
    {
        public bool GetIsExternalizable(object instance)
        {
            return instance is IExternalizable;
        }

        public bool GetIsDynamic(object instance)
        {
            return instance is ASObject;
        }

        public ClassDefinition GetClassDefinition(object instance)
        {
            Type type = instance.GetType();

            List<ClassMember> classMemberList = new List<ClassMember>();

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];

                string name = fieldInfo.Name;

                ClassMember classMember = new ClassMember(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, fieldInfo.MemberType, null);

                classMemberList.Add(classMember);
            }

            ClassMember[] classMembers = classMemberList.ToArray();

            string customClassName = type.FullName;

            customClassName = AMFWriter.GetCustomClass(customClassName);

            ClassDefinition classDefinition = new ClassDefinition(customClassName, classMembers, GetIsExternalizable(instance), GetIsDynamic(instance));

            return classDefinition;
        }

        public object GetValue(object instance, ClassMember member)
        {
            Type type = instance.GetType();

            FieldInfo fieldInfo = type.GetField(member.Name, member.BindingFlags);

            return fieldInfo.GetValue(instance);
        }

        public void SetValue(object instance, ClassMember member, object value)
        {
            Type type = instance.GetType();

            FieldInfo fieldInfo = type.GetField(member.Name, member.BindingFlags);

            fieldInfo.SetValue(instance, value);
        }
    }
}
