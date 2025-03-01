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

        public string? ErrorMessage { get; private set; }


        public SshStateChangedEventArgs(SshState state, string? errorMessage = null)
        {
            this.State = state;

            if (errorMessage != null)
            {
                ErrorMessage = errorMessage;
            }
        }
    }
}
