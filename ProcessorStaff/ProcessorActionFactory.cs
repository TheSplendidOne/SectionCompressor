using System;
using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    public static class SProcessorActionFactory
    {
        public static ProcessorAction Create(CompressionMode compressionMode)
        {
            switch (compressionMode)
            {
                case CompressionMode.Compress:
                    return Compress;
                case CompressionMode.Decompress:
                    return Decompress;
                default:
                    throw new Exception($"Unknown compression mode: {compressionMode}.");
            }
        }

        public static void Compress(MemoryStream source, MemoryStream destination)
        {
            using (GZipStream gZipStream = new GZipStream(destination, CompressionLevel.Optimal, true))
            {
                source.CopyTo(gZipStream);
            }
        }

        public static void Decompress(MemoryStream source, MemoryStream destination)
        {
            using (GZipStream gZipStream = new GZipStream(source, CompressionMode.Decompress, true))
            {
                gZipStream.CopyTo(destination);
            }
        }
    }
}
