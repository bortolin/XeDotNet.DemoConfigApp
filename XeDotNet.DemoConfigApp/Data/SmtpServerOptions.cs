using System;

namespace XeDotNet.DemoConfigApp
{
    public class SmtpServerOptions
    {
        public const string SmtpServer = "SmtpServer";

        public string Hostname { get; set; }
        public string Username { get; set; }
        public object Password { get; set; }
        public int Port { get; set; }
    }
}

