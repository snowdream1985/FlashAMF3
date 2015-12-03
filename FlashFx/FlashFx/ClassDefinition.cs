namespace FlashFx.AMF3
{
    public sealed class ClassDefinition
    {
        private string _className;

        private ClassMember[] _members;

        private bool _externalizable;

        private bool _dynamic;

        public static ClassMember[] EmptyClassMembers = new ClassMember[0];

        public ClassDefinition(string className, ClassMember[] members, bool externalizable, bool dynamic)
        {
            _className = className;

            _members = members;

            _externalizable = externalizable;

            _dynamic = dynamic;
        }

        public string ClassName { get { return _className; } }

        public int MemberCount { get { return _members == null ? 0 : _members.Length; } }

        public ClassMember[] Members { get { return _members; } }

        public bool IsExternalizable { get { return _externalizable; } }

        public bool IsDynamic { get { return _dynamic; } }

        public bool IsTypedObject { get { return (!string.IsNullOrEmpty(_className)); } }
    }
}
