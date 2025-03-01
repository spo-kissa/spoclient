using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spoclient.Models
{
    public class SshCommandResult
    {
        public SshCommandResult(SshCommand command)
        {
            this.Command = command.CommandText;
            this.Result = command.Result;
            this.ExitCode = command.ExitStatus;
        }


        public SshCommandResult(string command, string result, int? exitCode)
        {
            this.Command = command;
            this.Result = result;
            this.ExitCode = exitCode;
        }


        public string Command { get; protected set; }


        public string Result { get; protected set; }


        public int? ExitCode { get; protected set; }
    }
}
