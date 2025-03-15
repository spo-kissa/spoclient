using Renci.SshNet.Common;

namespace spoclient.Terminals
{
    public class SshIdentification
    {
        public string SoftwareVersion { get; set; }


        public string ProtocolVersion { get; set; }


        public string Comments { get; set; }



        public SshIdentification(SshIdentificationEventArgs e)
        {
            SoftwareVersion = e.SshIdentification.SoftwareVersion;
            ProtocolVersion = e.SshIdentification.ProtocolVersion;
            Comments = e.SshIdentification.Comments;
        }


        public override string ToString()
        {
            string text = "SSH-" + ProtocolVersion + "-" + SoftwareVersion;
            if (Comments != null)
            {
                text = text + " " + Comments;
            }

            return text;
        }
    }
}
