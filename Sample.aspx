<%@ Page Language="C#" %>
<%@ Register TagPrefix="irc" TagName="Motd" Src="Motd.ascx" %>
<%@ Register TagPrefix="irc" TagName="ChannelList" Src="ChannelList.ascx" %>
<%@ Register TagPrefix="irc" TagName="UserList" Src="UserList.ascx" %>
<html>
	<head>
		<title>Control Sampler</title>
	</head>
	<body>
		<!-- Note that the server refers to the server running IrcServd (the exposer), NOT your IRC server! -->
		<h1>Motd</h1>
		<irc:Motd Server="127.0.0.1" Port="4242" id="Motd1" runat="server" />
		<h1>Channel list</h1>
		<irc:ChannelList Server="127.0.0.1" Port="4242" id="CList1" runat="server" />
		<h1>User list</h1>
		<irc:UserList Server="127.0.0.1" Port="4242" id="UList1" runat="server" />
	</body>
</html>
