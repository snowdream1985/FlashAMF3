using System.Reflection;

namespace FlashFx.AMF3
{
    public sealed class ClassMember
    {
        string _name;

        BindingFlags _bindingFlags;

        MemberTypes _memberType;

        object[] _customAttributes;

        public ClassMember(string name, BindingFlags bindingFlags, MemberTypes memberType, object[] customAttributes)
        {
            _name = name;

            _bindingFlags = bindingFlags;

            _memberType = memberType;

            _customAttributes = customAttributes;
        }

        public string Name { get { return _name; } }

        public BindingFlags BindingFlags { get { return _bindingFlags; } }

        public MemberTypes MemberType { get { return _memberType; } }

        public object[] CustomAttributes { get { return _customAttributes; } }
    }
}
