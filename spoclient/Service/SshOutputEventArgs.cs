using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spoclient.Service
{
    public class SshOutputEventArgs : EventArgs
    {
        public string Output { get; private set; }


        public SshOutputEventArgs(string output)
        {
            Output = output;
        }
    }
}
