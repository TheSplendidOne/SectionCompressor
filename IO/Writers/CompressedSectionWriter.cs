using System;
using System.IO;

namespace GZipTest
{
    public class CCompressedSectionWriter : CBinaryFileSectionWriter
    {
        public CCompressedSectionWriter(String path) : base(path)
        {
        }

        public override void WriteNext(MemoryStream writingStream)
        {
            _writer.Write((Int32)writingStream.Length);
            writingStream.WriteTo(_writer.BaseStream);
        }
    }
}
