namespace spoclient.Service
{
    public enum SshServiceState
    {
        Idle,
        RunningCommand,
        GettingExitCode,
    }
}
