namespace flash.utils
{
    public class ResponseBody : AMFBody
    {
        private AMFBody _requestBody;

        internal ResponseBody()
        {
        }

        public ResponseBody(AMFBody requestBody)
        {
            _requestBody = requestBody;
        }

        public ResponseBody(AMFBody requestBody, object content)
        {
            _requestBody = requestBody;

            _target = requestBody.Response + AMFBody.OnResult;

            _content = content;

            _response = "null";
        }

        public AMFBody RequestBody
        {
            get { return _requestBody; }

            set { _requestBody = value; }
        }
    }
}
