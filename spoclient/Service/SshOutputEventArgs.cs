using System;

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
