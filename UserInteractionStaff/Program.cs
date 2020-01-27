using System;

namespace GZipTest
{
    public static class SProgram
    {
        public static String[] Arguments { get; private set; }

        private static void Main(String[] args)
        {
            try
            {
                Arguments = args;
                if (SArgumentsValidator.ValidateArguments(args))
                    Run(SProcessorHelper.CreateProcessor(args[0], args[1], args[2]));
                else
                    Environment.Exit(1);
            }
            catch(Exception exception)
            {
                SFatalExceptionHandler.HandleException(exception, false);
                Environment.Exit(1);
            }
        }

        private static void Run(CProcessor processor)
        {
            processor.StateChanged += ProcessorStateChangedEventHandler;
            processor.Start();
        }

        private static void ProcessorStateChangedEventHandler(CProcessor processor)
        {
            switch(processor.State)
            {
                case EProcessorState.Completed:
                    Console.WriteLine("Successfully completed.");
                    Environment.Exit(0);
                    break;
                case EProcessorState.Faulted:
                    SFatalExceptionHandler.HandleException(processor.FaultException, true);
                    Environment.Exit(1);
                    break;
            }
        }
    }
}
