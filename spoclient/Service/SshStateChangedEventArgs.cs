using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spoclient.Service
{
    public class SshStateChangedEventArgs : EventArgs
    {
        public SshState State { get; private set; }

        public string Message { get; private set; } = string.Empty;


        public SshStateChangedEventArgs(SshState state, string? message = null)
        {
            this.State = state;

            if (message != null)
            {
                this.Message = message;
            }
        }
    }
}
