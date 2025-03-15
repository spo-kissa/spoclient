using System;

namespace spoclient.Terminals
{
    public class SshStateChangedEventArgs : EventArgs
    {
        public SshConnectionState State {  get; private set; }


        public SshStateChangedEventArgs()
        {
        }


        public SshStateChangedEventArgs(SshConnectionState state)
        {
            State = state;
        }
    }
}
