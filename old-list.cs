using System;
using System.Threading;
using Meebey.SmartIrc4net;

public class ChanneLister {
	public static IrcClient irc = new IrcClient();
	public static string TestChan = "#test";

	public static void Main(string[] args) {
		new Thread(new ThreadStart(CommandReader)).Start();
		irc.OnRawMessage += new IrcEventHandler(OnRawMessage);
		irc.SupportNonRfc = true;
		irc.Connect(args , 6667);
		irc.Login("lister", "IRC Channel listings");
		irc.RfcJoin(TestChan);
		irc.Listen();
	}
	
	public static void CommandReader() {
		while (true) {
			string cmd = Console.ReadLine();
			if (cmd == "ls") {
				irc.SendMessage(SendType.Message, TestChan, "Attempting to list channels");
				foreach (var c in irc.GetChannelList("*")) {
					irc.SendMessage(SendType.Message, TestChan, c.Channel + "(users: " + c.UserCount + ")");
					irc.SendMessage(SendType.Message, TestChan, "topic for " + c.Channel  + ": " + c.Topic);
				}
			}
		}
	}
	
	public static void OnRawMessage(object sender, IrcEventArgs e)
	{
		System.Console.WriteLine("Received: "+e.Data.RawMessage);
	}
}
