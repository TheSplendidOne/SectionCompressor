using System;
using System.IO;
using System.Linq;

namespace GZipTest
{
    public static class SFatalExceptionHandler
    {
        private static readonly Predicate<Exception>[] KnownExceptionHandlers =
        {
            HandleInvalidDataException,
            HandleDirectoryNotFoundException,
            HandleIoException,
            HandleUnauthorizedAccessException
        };

        public static void HandleException(Exception exception, Boolean deleteTargetFile)
        {
            KnownExceptionHandlers.Any(processor => processor.Invoke(exception));
            HandleDefault(exception, deleteTargetFile);
        }

        private static void HandleDefault(Exception exception, Boolean deleteTargetFile)
        {
            Console.WriteLine(exception.Message);
            try
            {
                if(deleteTargetFile)
                    File.Delete(SProgram.Arguments[2]);
            }
            catch
            {
                Console.WriteLine("Failed to delete destination file. Destination file has an invalid format.");
            }
        }

        private static Boolean HandleInvalidDataException(Exception exception)
        {
            return ShowMessageIfNotNull(exception as InvalidDataException, "Source file has an invalid format.");
        }

        private static Boolean HandleDirectoryNotFoundException(Exception exception)
        {
            return ShowMessageIfNotNull(exception as DirectoryNotFoundException, "No destination directory found.");
        }

        private static Boolean HandleIoException(Exception exception)
        {
            return ShowMessageIfNotNull(exception as IOException, "Not enough memory to run.");
        }

        private static Boolean HandleUnauthorizedAccessException(Exception exception)
        {
            return ShowMessageIfNotNull(exception as UnauthorizedAccessException, "Not enough permission to run.");
        }

        private static Boolean ShowMessageIfNotNull(Exception exception, String message)
        {
            if(exception != null)
            {
                Console.WriteLine(message);
                return true;
            }
            return false;
        }
    }
}
