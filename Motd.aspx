<%@ Page language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net.Sockets" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!-- TODO: CodeBehind (this is a rough test, polish it, integratable, etc. -->
<!-- Until we get config and masters and other fancy stuff, do it yourself -->
<script runat="server">
private void Page_Load(Object sender, EventArgs e)
{
	Motd.Font.Name = "monospace"; // should set this in code
	string server = "127.0.0.1";
	int port = 4242;
	using (Stream s = new TcpClient(server, port).GetStream()) {
		StreamReader sr = new StreamReader(s);
		StreamWriter sw = new StreamWriter(s);
		sw.AutoFlush = true;
		
		sw.WriteLine("Motd");
		Motd.Text = sr.ReadToEnd();
	}
}
</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head runat="server">
		<title>Message of the day for (network)</title>
	</head>
	<body>
		<form id="form1" runat="server">
			<pre>
<asp:Label id="Motd" runat="server" />
			</pre>
		</form>
	</body>
</html>
