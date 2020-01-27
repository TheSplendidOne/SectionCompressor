using System;
using System.IO;

namespace GZipTest
{
    public abstract class CBinaryFileSectionReader : ISectionReader
    {
        protected readonly BinaryReader _reader;

        protected CBinaryFileSectionReader(String path)
        {
            _reader = new BinaryReader(new FileStream(path, FileMode.Open));
        }

        public Boolean ReadingIsFinished()
        {
            return _reader.BaseStream.Length == _reader.BaseStream.Position;
        }

        public abstract MemoryStream ReadNext();
    }
}
