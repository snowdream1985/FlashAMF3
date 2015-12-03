using flash.net;
using FlashFx.IO;
namespace flash.utils
{
    public class AMFBody
    {
        public const string Recordset = "rs://";

        public const string OnResult = "/onResult";

        public const string OnStatus = "/onStatus";

        protected object _content;

        protected string _response;

        protected string _target;

        protected bool _ignoreResults;

        public AMFBody()
        {

        }

        public AMFBody(string target, string response, object content)
        {
            _target = target;

            _response = response;

            _content = content;
        }

        public string Target
        {
            get { return _target; }

            set
            {
                _target = value;
            }
        }

        public bool IsEmptyTarget
        {
            get
            {
                return _target == null || _target == string.Empty || _target == "null";
            }
        }

        public string Response
        {
            get { return _response; }

            set { _response = value; }
        }

        public object Content
        {
            get { return _content; }

            set { _content = value; }
        }

        public bool IgnoreResults
        {
            get { return _ignoreResults; }

            set { _ignoreResults = value; }
        }

        public bool IsRecordsetDelivery
        {
            get
            {
                if (_target.StartsWith(AMFBody.Recordset))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string GetRecordsetArgs()
        {
            if (_target != null)
            {
                if (IsRecordsetDelivery)
                {
                    string args = _target.Substring(AMFBody.Recordset.Length);

                    args = args.Substring(0, args.IndexOf("/"));

                    return args;
                }
            }

            return null;
        }

        public string TypeName
        {
            get
            {
                if (_target != "null")
                {
                    if (!string.IsNullOrEmpty(_target))
                    {
                        if (_target.LastIndexOf('.') != -1)
                        {
                            string target = _target.Substring(0, _target.LastIndexOf('.'));

                            if (IsRecordsetDelivery)
                            {
                                target = target.Substring(AMFBody.Recordset.Length);

                                target = target.Substring(target.IndexOf("/") + 1);

                                target = target.Substring(0, target.LastIndexOf('.'));
                            }

                            return target;
                        }
                    }
                }

                return null;
            }
        }

        public string Method
        {
            get
            {
                if (_target != "null")
                {
                    if (!string.IsNullOrEmpty(_target))
                    {
                        if (_target.LastIndexOf('.') != -1)
                        {
                            string target = _target;

                            if (IsRecordsetDelivery)
                            {
                                target = target.Substring(AMFBody.Recordset.Length);

                                target = target.Substring(target.IndexOf("/") + 1);
                            }

                            if (IsRecordsetDelivery)
                            {
                                target = target.Substring(0, target.LastIndexOf('.'));
                            }

                            string method = target.Substring(target.LastIndexOf('.') + 1);

                            return method;
                        }
                    }
                }

                return null;
            }
        }

        public string Call { get { return TypeName + "." + Method; } }

        internal void WriteBody(ObjectEncoding objectEncoding, AMFWriter writer)
        {
            writer.Reset();

            if (Target == null)
            {
                writer.WriteUTF("null");
            }
            else
            {
                writer.WriteUTF(Target);
            }

            if (Response == null)
            {
                writer.WriteUTF("null");
            }
            else
            {
                writer.WriteUTF(Response);
            }

            writer.WriteInt32(-1);

            WriteBodyData(objectEncoding, writer);
        }

        protected virtual void WriteBodyData(ObjectEncoding objectEncoding, AMFWriter writer)
        {
            object content = Content;

            writer.WriteData(objectEncoding, content);
        }
    }
}
