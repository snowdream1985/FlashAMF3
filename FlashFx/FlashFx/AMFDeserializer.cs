using FlashFx.IO;
using System;
using System.IO;

namespace flash.utils
{
    public class AMFDeserializer : AMFReader
    {
        public AMFDeserializer(Stream stream)
            : base(stream)
        {

        }

        public AMFMessage ReadAMFMessage()
        {
            // Version stored in the first two bytes.
            ushort version = base.ReadUInt16();

            AMFMessage message = new AMFMessage(version);

            // Read header count.
            int headerCount = base.ReadUInt16();

            for (int i = 0; i < headerCount; i++)
            {
                message.AddHeader(ReadHeader());
            }

            // Read header count.
            int bodyCount = base.ReadUInt16();

            for (int i = 0; i < bodyCount; i++)
            {
                AMFBody amfBody = ReadBody();

                if (amfBody != null)
                {
                    message.AddBody(amfBody);
                }
            }

            return message;
        }

        private AMFHeader ReadHeader()
        {
            Reset();

            // Read name.
            string name = base.ReadString();

            // Read must understand flag.
            bool mustUnderstand = base.ReadBoolean();

            // Read the length of the header.
            int length = base.ReadInt32();

            // Read content.
            object content = base.ReadData();

            return new AMFHeader(name, mustUnderstand, content);
        }

        private AMFBody ReadBody()
        {
            Reset();

            string target = base.ReadString();

            // Response that the client understands.
            string response = base.ReadString();

            int length = base.ReadInt32();

            try
            {
                object content = base.ReadData();

                AMFBody amfBody = new AMFBody(target, response, content);

                return amfBody;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
