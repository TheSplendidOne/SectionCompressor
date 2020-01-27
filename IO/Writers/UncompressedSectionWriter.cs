using System;
using System.IO;

namespace GZipTest
{
    public class CUncompressedSectionWriter : CBinaryFileSectionWriter
    {
        public CUncompressedSectionWriter(String path) : base(path)
        {
        }

        public override void WriteNext(MemoryStream writingStream)
        {
            writingStream.WriteTo(_writer.BaseStream);
        }
    }
}
