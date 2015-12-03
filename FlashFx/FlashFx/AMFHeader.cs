namespace flash.utils
{
    public class AMFHeader
    {
        private object _content;

        private bool _mustUnderstand;

        private string _name;

        public AMFHeader(string name, bool mustUnderstand, object content)
        {
            _name = name;

            _mustUnderstand = mustUnderstand;

            _content = content;
        }

        public string Name { get { return _name; } }

        public object Content { get { return _content; } }

        public bool MustUnderstand { get { return _mustUnderstand; } }
    }
}
