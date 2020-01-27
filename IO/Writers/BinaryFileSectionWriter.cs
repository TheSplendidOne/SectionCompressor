using System;
using System.IO;

namespace GZipTest
{
    public abstract class CBinaryFileSectionWriter : ISectionWriter
    {
        protected readonly BinaryWriter _writer;

        protected CBinaryFileSectionWriter(String path)
        {
            _writer = new BinaryWriter(new FileStream(path, FileMode.CreateNew));
        }

        public void Dispose()
        {
            _writer.Dispose();
        }

        public abstract void WriteNext(MemoryStream writingStream);
    }
}
