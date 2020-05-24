using CSharpExtendedCommands.Web.Communication;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace RemoteBrowserClient
{
    public class RemoteBrowserClient : TCPClient
    {
        public class DirectoryList
        {
            public DirectoryList(string[] files, string[] directories)
            {
                Files = files;
                Directories = directories;
            }

            public string[] Files { get; }
            public string[] Directories { get; }
        }
        public RemoteBrowserClient(string ip, ushort port)
        {
            Ip = IPAddress.Parse(ip);
            Port = port;
        }
        public void SendCommand(string command, string args = null)
        {
            var s = command.ToUpper();
            s += args != null ? ": \"" + args + "\"" : "";
            SendPackage(new TcpPackage(s));
        }
        public DirectoryList ListDirectory(string dir)
        {
            SendCommand("list-directory", dir);
            var package = ReceivePackage().ToString();
            var filedata = package.Substring(0, package.IndexOf("\nDirectories:\n"));
            var directoryData = package.Substring(filedata.Length + 1);
            List<string> mem = new List<string>();
            foreach (Match m in Regex.Matches(filedata, @"\t(.+);"))
                mem.Add(m.Groups[1].Value);
            string[] files = mem.ToArray();
            mem.Clear();
            foreach (Match m in Regex.Matches(directoryData, @"\t(.+);"))
                mem.Add(m.Groups[1].Value);
            return new DirectoryList(files, mem.ToArray());
        }
    }
}
