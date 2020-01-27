using System;
using System.IO;
using System.Linq;

namespace GZipTest
{
    public static class SArgumentsValidator
    {
        public const String ValidCompressFlag = "COMPRESS";

        public const String ValidDecompressFlag = "DECOMPRESS";

        private static readonly Int32 ValidArgumentsCount = 3;

        private static readonly Predicate<String[]>[] ValidationParts =
        {
            ValidateArgumentsCount,
            ValidateFlag,
            ValidateSourcePath,
            ValidateDestinationDirectoryName,
            ValidateDestinationFile
        };

        public static Boolean ValidateArguments(String[] args)
        {
            try
            {
                return ValidationParts.All(validationPart => validationPart.Invoke(args));
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        private static Boolean ValidateArgumentsCount(String[] args)
        {
            if (args.Length != ValidArgumentsCount)
            {
                Console.WriteLine(
                    $"Illegal arguments count: {args.Length}." +
                    Environment.NewLine +
                    $"Program takes only {ValidArgumentsCount} arguments.");
                return false;
            }
            return true;
        }

        private static Boolean ValidateFlag(String[] args)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (comparer.Compare(args[0], ValidCompressFlag) == 0)
                return true;
            if (comparer.Compare(args[0], ValidDecompressFlag) == 0)
                return true;
            Console.WriteLine(
                $"Unknown program mode: {args[0]}. Available modes: {ValidCompressFlag}, {ValidDecompressFlag}.");
            return false;
        }

        private static Boolean ValidateSourcePath(String[] args)
        {
            if (!File.Exists(args[1]))
            {
                Console.WriteLine($"There is no file at the {args[1]} source path.");
                return false;
            }
            return true;
        }

        private static Boolean ValidateDestinationDirectoryName(String[] args)
        {
            String directoryName = Path.GetDirectoryName(args[2]);
            if (!Directory.Exists(directoryName))
            {
                Console.WriteLine($"Destination directory {directoryName} doesn't exist.");
                return false;
            }
            return true;
        }

        private static Boolean ValidateDestinationFile(String[] args)
        {
            if(File.Exists(args[2]))
            {
                Console.WriteLine($"Destination file {args[2]} already exists.");
                return false;
            }
            return true;
        }
    }
}
