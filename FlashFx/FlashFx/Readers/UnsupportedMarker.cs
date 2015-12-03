using System;

namespace FlashFx.IO.Readers
{
   public class UnsupportedMarker : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            throw new Exception("UnsupportedMarker");
        }
    }
}
