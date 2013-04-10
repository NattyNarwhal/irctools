<%@ Control language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net.Sockets" %>
<!-- TODO: CodeBehind (this is a rough test, polish it, integratable, etc. -->
<!-- Until we get config and masters and other fancy stuff, do it yourself -->
<script runat="server">
public string Server { get; set; } // services server
public int Port { get; set; }

private void Page_Load(Object sender, EventArgs e)
{
	// Motd.Font.Name = "monospace"; // should set this in code
	using (Stream s = new TcpClient(Server, Port).GetStream()) {
		StreamReader sr = new StreamReader(s);
		StreamWriter sw = new StreamWriter(s);
		sw.AutoFlush = true;
		
		sw.WriteLine("Motd");
		Motd.Text = sr.ReadToEnd();
	}
}
</script>
<pre><asp:Label id="Motd" runat="server" /></pre>
