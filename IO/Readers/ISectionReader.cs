using System;
using System.IO;

namespace GZipTest
{
    public interface ISectionReader
    {
        MemoryStream ReadNext();

        Boolean ReadingIsFinished();
    }
}
