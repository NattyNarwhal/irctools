using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using Meebey.SmartIrc4net;

namespace IrcTools {
public class IrcServd {
	public static IrcClient irc = new IrcClient();
	public static TcpListener tl = new TcpListener(IPAddress.Any, 4242);
	public static IList<ChannelInfo> ChannelCache;

	public static void Main(string[]args) {
		// Init threads & networking
		new Thread(new ThreadStart(Server)).Start();
		// Init IRC
		irc.OnRawMessage += new IrcEventHandler(OnRawMessage);
		irc.SupportNonRfc = true;
		irc.Connect(args , 6667);
		irc.Login("lister", "IRC Channel listings");
		irc.Listen();
	}
	
	
	// Handlers, events, and threads
	public static void Server() {
		tl.Start();
		while (true) {
			new Thread(new ParameterizedThreadStart(ServerHandler)).Start(tl.AcceptTcpClient());
		}
	}
	
	public static void ServerHandler(object boxedClient) {
		// Unbox TcpClient
		TcpClient tcm = boxedClient as TcpClient;
		using (Stream s = tcm.GetStream()) {
			StreamReader sr = new StreamReader(s);
			StreamWriter sw = new StreamWriter(s);
			sw.AutoFlush = true;
			
			string[] cmd = sr.ReadLine().Split(new char[]{' '}, 2);
			if (cmd[0] == "ChannelList") {
				foreach (ChannelInfo i in irc.GetChannelList("*")) {
					sw.WriteLine(i.Channel + "\t" + i.UserCount + "\t" + i.Topic);
				}
			}
		}
	}
	
	public static void OnRawMessage(object sender, IrcEventArgs e)
	{
		System.Console.WriteLine("Received: "+e.Data.RawMessage);
	}
}
}
