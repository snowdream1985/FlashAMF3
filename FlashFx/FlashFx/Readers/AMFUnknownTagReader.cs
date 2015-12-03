using System;

namespace FlashFx.IO.Readers
{
    public class AMFUnknownTagReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            throw new Exception("AMFUnknownTagReader");
        }
    }
}
