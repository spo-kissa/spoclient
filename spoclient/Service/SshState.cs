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
