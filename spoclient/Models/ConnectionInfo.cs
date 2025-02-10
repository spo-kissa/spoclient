using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using spoclient.Pages;

namespace spoclient.Models
{
    public class ConnectionInfo
    {
        public string Header { get; set; }

        public IconSource? Icon { get; set; }


        public HomePage Content { get; }


        public ConnectionInfo()
        {
            Header = "新しい接続";
            Content = new HomePage();
        }
    }
}
