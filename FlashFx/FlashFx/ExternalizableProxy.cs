using FlashFx.AMF3;
using System;

namespace FlashFx.IO
{
    public class ExternalizableProxy : IObjectProxy
    {
        public bool GetIsExternalizable(object instance)
        {
            return true;
        }

        public bool GetIsDynamic(object instance)
        {
            return false;
        }

        public ClassDefinition GetClassDefinition(object instance)
        {
            Type type = instance.GetType();

            string customClassName = type.FullName;

            customClassName = AMFWriter.GetCustomClass(customClassName);

            ClassDefinition classDefinition = new ClassDefinition(customClassName, ClassDefinition.EmptyClassMembers, true, false);

            return classDefinition;
        }

        public object GetValue(object instance, ClassMember member)
        {
            throw new NotSupportedException();
        }

        public void SetValue(object instance, ClassMember member, object value)
        {
            throw new NotSupportedException();
        }
    }
}
