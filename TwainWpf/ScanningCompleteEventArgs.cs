using System;

namespace TwainWpf
{
    public class ScanningCompleteEventArgs : EventArgs
    {
        public Exception Exception { get; }

        public ScanningCompleteEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
