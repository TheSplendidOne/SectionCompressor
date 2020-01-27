using System;
using System.IO;

namespace GZipTest
{
    public class CCompressedSectionReader : CBinaryFileSectionReader
    {
        public CCompressedSectionReader(String path) : base(path)
        {
        }

        public override MemoryStream ReadNext()
        {
            Int32 sectionSize = _reader.ReadInt32();
            Byte[] sectionData = new Byte[sectionSize];
            _reader.Read(sectionData, 0, sectionSize);
            return new MemoryStream(sectionData);
        }
    }
}
