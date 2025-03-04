using System;

namespace MessageBoxSlim.Avalonia.Enums
{
    [Flags]
    public enum ButtonEnum : int
    {
        Ok = 1,

        Yes = 2,

        No = 4,

        Cancel = 8,

        Abort = 16
    }
}