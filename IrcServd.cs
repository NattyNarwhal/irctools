using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using Meebey.SmartIrc4net;

namespace IrcTools {
	public class IrcServd {
		// Networking
		public static IrcClient irc = new IrcClient();
		public static TcpListener tl = new TcpListener(IPAddress.Any, 4242);
		// Cache
		public static IList<ChannelInfo> ChannelCache = null;
		public static IList<WhoInfo> UserCache = null;
	
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
				UserCache = irc.GetWhoList("*");
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
						RegenChannelCache();
					}
					// Get channels from cache
					foreach (ChannelInfo i in ChannelCache) {
						sw.WriteLine(i.Channel + "\t" + i.UserCount + "\t" + i.Topic);
					}
				} if (cmd[0] == "Motd") {
					foreach (string m in irc.Motd) {
						sw.WriteLine(m);
					}
				} if (cmd[0] == "UserList") {
					// Regen cache, in case it doesn't exist
					if (UserCache == null) {
						RegenUserCache();
					}
					// we output seperating information by a tab char
					// outp is (nick + realname + hostmask + away + oper)
					// easily parsable into tables or labels or just a postprocessing into a human readable string
					foreach (var u in UserCache) {
						sw.WriteLine(u.Nick + "\t" + u.Realname  + "\t" + u.Ident + "@" + u.Host + "\t" + u.IsAway.ToString() + "\t" + u.IsIrcOp.ToString());
					}
				} if (cmd[0] == "GetUser") {
					// Regen cache, in case it doesn't exist
					if (UserCache == null) {
						RegenUserCache();
					}
					// on the other hand, it's a bit much if you want one
					// user - we have a function that will cut the others out for you
					// the function return nothing to the stream if we don't find anyone
					foreach (var u in UserCache) {
						// case insensitivity
						if (u.Nick.ToLower() == cmd[1].ToLower()) {
							sw.WriteLine(u.Nick + "\t" + u.Realname  + "\t" + u.Ident + "@" + u.Host + "\t" + u.IsAway.ToString() + "\t" + u.IsIrcOp.ToString());
						}
					}
				}
			}
		}
		
		public static void OnRawMessage(object sender, IrcEventArgs e)
		{
			System.Console.WriteLine("{RAW} "+e.Data.RawMessage);
		}
		
		// Cache regen
		public static void RegenUserCache() {
			Console.WriteLine("{DBG} Generating new user cache");
			UserCache = irc.GetWhoList("*");
		}
		public static void RegenChannelCache() {
			Console.WriteLine("{DBG} Generating new channel cache");
			ChannelCache = irc.GetChannelList("*");
		}
	}
}
