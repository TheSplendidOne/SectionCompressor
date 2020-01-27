using System;
using System.IO;

namespace GZipTest
{
    public class CUncompressedSectionReader : CBinaryFileSectionReader
    {
        private static readonly Int32 SectionSize = 1024 * 1024; // 1 Mb

        public CUncompressedSectionReader(String path) : base(path)
        {
        }

        public override MemoryStream ReadNext()
        {
            Byte[] sectionData = new Byte[SectionSize];
            Int32 sectionSize = _reader.Read(sectionData, 0, SectionSize);
            return new MemoryStream(sectionData, 0, sectionSize);
        }
    }
}
