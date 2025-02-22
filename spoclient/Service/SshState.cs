using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spoclient.Service
{
    public enum SshState
    {
        Disconnected,
        Connecting,
        Connected,
        FailToConnect,
        ErrorConnecting,
        TerminalError,
    }
}
