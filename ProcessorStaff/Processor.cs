using System;
using System.IO;
using System.Threading;

namespace GZipTest
{
    public class CProcessor
    {
        private readonly ISectionReader _reader;

        private readonly ISectionWriter _writer;

        private readonly Thread _principalThread;

        private readonly ProcessorAction _processorAction;

        private readonly Mutex _faultMutex = new Mutex();

        public Exception FaultException { get; private set; }

        public EProcessorState State { get; private set; }

        public event Action<CProcessor> StateChanged;

        public CProcessor(ISectionReader reader, ISectionWriter writer, ProcessorAction processorAction)
        {
            _reader = reader;
            _writer = writer;
            _processorAction = processorAction;
            _principalThread = new Thread(Process);
            ChangeState(EProcessorState.Created);
        }

        public void Start()
        {
            _principalThread.Start();
        }

        private void Process()
        {
            try
            {
                ChangeState(EProcessorState.Running);
                Read();
                _writer.Dispose();
            }
            catch (Exception exception)
            {
                SetFaultException(exception);
            }
            finally
            {
                if (State == EProcessorState.WaitingForFault)
                    ChangeState(EProcessorState.Faulted);
                else
                    ChangeState(EProcessorState.Completed);
            }
        }

        private void Read()
        {
            Thread previousWritingThread = null;
            while (!_reader.ReadingIsFinished())
            {
                if (State == EProcessorState.WaitingForFault)
                    break;
                Thread newWritingThread = new Thread(Write);
                newWritingThread.Start(new CMemoryStreamThreadTuple(_reader.ReadNext(), previousWritingThread));
                previousWritingThread = newWritingThread;
            }
            previousWritingThread?.Join();
        }

        private void Write(Object args)
        {
            try
            {
                (MemoryStream source, Thread previousWritingThread) = (CMemoryStreamThreadTuple)args;
                using (MemoryStream destination = new MemoryStream())
                {
                    _processorAction.Invoke(source, destination);
                    previousWritingThread?.Join();
                    _writer.WriteNext(destination);
                }
            }
            catch(Exception exception)
            {
                SetFaultException(exception);
            }
        }

        private void ChangeState(EProcessorState newState)
        {
            try
            {
                if (State != newState)
                {
                    State = newState;
                    StateChanged?.Invoke(this);
                }
            }
            catch (Exception exception)
            {
                SetFaultException(exception);
            }
        }

        private void SetFaultException(Exception faultException)
        {
            if (FaultException == null)
            {
                try
                {
                    _faultMutex.WaitOne();
                    if (FaultException == null)
                    {
                        FaultException = faultException;
                        ChangeState(EProcessorState.WaitingForFault);
                    }
                }
                finally
                {
                    _faultMutex.ReleaseMutex();
                }
            }
        }
    }
}
