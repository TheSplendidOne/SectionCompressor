namespace GZipTest
{
    public enum EProcessorState
    {
        Created = 1,
        Running,
        Completed,
        WaitingForFault,
        Faulted
    }
}
