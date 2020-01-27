using System.IO;
using System.Threading;

namespace GZipTest
{
    public class CMemoryStreamThreadTuple
    {
        public MemoryStream MemoryStream { get; }

        public Thread Thread { get; }

        public CMemoryStreamThreadTuple(MemoryStream memoryStream, Thread thread)
        {
            MemoryStream = memoryStream;
            Thread = thread;
        }

        public void Deconstruct(out MemoryStream memoryStream, out Thread thread)
        {
            memoryStream = MemoryStream;
            thread = Thread;
        }
    }
}
