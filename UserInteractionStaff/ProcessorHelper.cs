using System;
using System.IO.Compression;

namespace GZipTest
{
    public static class SProcessorHelper
    {
        public static CProcessor CreateProcessor(String flag, String sourcePath, String destinationPath)
        {
            CompressionMode mode = GetCompressionMode(flag);
            return new CProcessor(
                GetSectionReader(mode, sourcePath),
                GetSectionWriter(mode, destinationPath),
                SProcessorActionFactory.Create(mode));
        }

        private static CompressionMode GetCompressionMode(String flag)
        {
            switch (flag.ToUpperInvariant())
            {
                case SArgumentsValidator.ValidCompressFlag:
                    return CompressionMode.Compress;
                case SArgumentsValidator.ValidDecompressFlag:
                    return CompressionMode.Decompress;
                default:
                    throw new ArgumentException($"Unknown flag: {flag}.");
            }
        }

        private static ISectionReader GetSectionReader(CompressionMode mode, String sourcePath)
        {
            switch(mode)
            {
                case CompressionMode.Compress:
                    return new CUncompressedSectionReader(sourcePath);
                case CompressionMode.Decompress:
                    return new CCompressedSectionReader(sourcePath);
                default:
                    throw new ArgumentException(GetUnknownCompressionModeMessage(mode));
            }
        }

        private static ISectionWriter GetSectionWriter(CompressionMode mode, String destinationPath)
        {
            switch (mode)
            {
                case CompressionMode.Compress:
                    return new CCompressedSectionWriter(destinationPath);
                case CompressionMode.Decompress:
                    return new CUncompressedSectionWriter(destinationPath);
                default:
                    throw new ArgumentException(GetUnknownCompressionModeMessage(mode));
            }
        }

        private static String GetUnknownCompressionModeMessage(CompressionMode mode)
        {
            return $"Unknown compression mode: {mode}.";
        }
    }
}
