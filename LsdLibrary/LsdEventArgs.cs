using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LsdLibrary
{
    public class LsdDownloadEventArgs : EventArgs
    {
        public string Message { get; }

        public object Result { get; }

        public LsdDownloadEventArgs(string message)
        {
            Message = message;
        }

        public LsdDownloadEventArgs(string message, object result)
        {
            Message = message;
            Result = result;
        }
    }
}
