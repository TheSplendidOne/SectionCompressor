using System;
using System.IO;

namespace GZipTest
{
    public interface ISectionWriter : IDisposable
    {
        void WriteNext(MemoryStream writingStream);
    }
}
