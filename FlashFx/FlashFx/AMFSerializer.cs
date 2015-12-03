using flash.net;
using FlashFx.IO;
using System;
using System.IO;

namespace flash.utils
{
    public class AMFSerializer : AMFWriter
    {
        public AMFSerializer(Stream stream)
            : base(stream)
        {

        }

        public void WriteMessage(AMFMessage amfMessage)
        {
            try
            {
                base.WriteShort(amfMessage.Version);

                int headerCount = amfMessage.HeaderCount;

                base.WriteShort(headerCount);

                for (int i = 0; i < headerCount; i++)
                {
                    this.WriteHeader(amfMessage.GetHeaderAt(i), ObjectEncoding.AMF0);
                }

                int bodyCount = amfMessage.BodyCount;

                base.WriteShort(bodyCount);

                for (int i = 0; i < bodyCount; i++)
                {
                    ResponseBody responseBody = amfMessage.GetBodyAt(i) as ResponseBody;

                    if (responseBody != null && !responseBody.IgnoreResults)
                    {
                        if (this.BaseStream.CanSeek)
                        {
                            long position = this.BaseStream.Position;

                            try
                            {
                                responseBody.WriteBody(amfMessage.ObjectEncoding, this);
                            }
                            catch (Exception exception)
                            {
                                throw exception;
                            }
                        }
                        else
                        {
                            responseBody.WriteBody(amfMessage.ObjectEncoding, this);
                        }
                    }
                    else
                    {
                        AMFBody amfBody = amfMessage.GetBodyAt(i);

                        amfBody.WriteBody(amfMessage.ObjectEncoding, this);
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void WriteHeader(AMFHeader header, ObjectEncoding objectEncoding)
        {
            base.Reset();

            base.WriteUTF(header.Name);

            base.WriteBoolean(header.MustUnderstand);

            base.WriteInt32(-1);

            base.WriteData(objectEncoding, header.Content);
        }
    }
}
