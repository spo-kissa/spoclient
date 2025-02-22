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


        public SshStateChangedEventArgs(SshState state)
        {
            this.State = state;
        }
    }
}
