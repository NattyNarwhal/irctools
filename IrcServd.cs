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
	public static IList<ChannelInfo> ChannelCache = null;

	public static void Main(string[]args) {
		// Init IRC
		irc.OnRawMessage += new IrcEventHandler(OnRawMessage);
		irc.SupportNonRfc = true;
		irc.Connect(args , 6667);
		irc.Login("WebServ", "IRC Web Services");
		// Init threads
		new Thread(new ThreadStart(Server)).Start();
		new Thread(new ThreadStart(CacheRegen)).Start();
		irc.Listen();
	}
	
	// Handlers, events, and threads
	public static void Server() {
		tl.Start();
		while (true) {
			new Thread(new ParameterizedThreadStart(ServerHandler)).Start(tl.AcceptTcpClient());
		}
	}
	
	public static void CacheRegen() {
		while (true) {
			Thread.Sleep(900000); // 15 minutes sounds reasonable
			ChannelCache = irc.GetChannelList("*");
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
			// For silent commands, you should write back even if they don't listen - for telnet users
			// rewrite into switch too
			if (cmd[0] == "ChannelList") {
				// If the cache is null, get the channels cached
				if (ChannelCache == null) {
					Console.WriteLine("{DBG} Generating new channel cache");
					ChannelCache = irc.GetChannelList("*");
				}
				// Get channels from cache
				foreach (ChannelInfo i in ChannelCache) {
					sw.WriteLine(i.Channel + "\t" + i.UserCount + "\t" + i.Topic);
				}
			} if (cmd[0] == "Motd") {
				foreach (string m in irc.Motd) {
					sw.WriteLine(m);
				}
			} if (cmd[0] == "GetUser") {
				// GUESS WHAT RETURNS NULLREFERENCEEXCEPTION WHEN YOU TRY TO USE THE PROPERTIES OF IT?
				IrcUser u = irc.GetIrcUser(cmd[1]);
				// we output in plain text, which web UIs can use verbatim
				// sw.WriteLine("Nickname: {0}", u.Nick);
				//sw.WriteLine("Realname: {0}", u.Realname);
				// sw.WriteLine("Hostmask: {0}@{1}", u.Ident, u.Host);
				// sw.WriteLine("Away: {0}", u.IsAway);
				// sw.WriteLine("IRC Operator: {0}", u.IsIrcOp);
				
				// We take a break from your regularly scheduled programming for better stringing
				// maybe it could be done better
				/** string joinedChannels = String.Empty;
				foreach (string c in u.JoinedChannels) {
					joinedChannels += (" " + c);
				} **/
				
				// sw.WriteLine("Channels:", joinedChannels);
			}
		}
	}
	
	public static void OnRawMessage(object sender, IrcEventArgs e)
	{
		System.Console.WriteLine("{RAW} "+e.Data.RawMessage);
	}
}
}
