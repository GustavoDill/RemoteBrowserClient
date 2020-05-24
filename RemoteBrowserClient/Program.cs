using System;
using System.Threading;

namespace RemoteBrowserClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var loginCode = "RemoteBrowser#CODE#";
            RemoteBrowserClient client = new RemoteBrowserClient("192.168.0.106", 4782);
            Console.Write("Connecting to server...");
            try
            {
                client.Connect();
                client.SendString(loginCode);
                Thread.Sleep(1000);
                string rec = "";
                client.ClientSocket.ReceiveTimeout = 3000;
                try { rec = client.ReceiveString(); } catch { }
                client.ClientSocket.ReceiveTimeout = 0;
                if (rec == "Code rejected")
                    try { client.Disconnect(); Console.WriteLine("\tCode rejected"); Thread.Sleep(2000); Environment.Exit(1); } catch { Console.WriteLine("\tCode rejected"); Thread.Sleep(2000); Environment.Exit(1); }
            }
            catch
            {

            }
            Thread.Sleep(1250);
            Console.WriteLine(client.Connected ? "\tConnected" : "\tFailed");
            Thread.Sleep(1250);
            if (!client.Connected)
                Environment.Exit(0);
            L:
            Console.Write(">");
            var d = Console.ReadLine();
            if (d == "exit")
                Environment.Exit(0);
            var list = client.ListDirectory(d);
            Console.WriteLine("Files");
            foreach (var f in list.Files)
                Console.WriteLine("\t" + f);
            Console.WriteLine("Directories");
            foreach (var dc in list.Directories)
                Console.WriteLine("\t" + dc);
            goto L;
        }
    }
}
