<%@ Page language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net.Sockets" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!-- TODO: CodeBehind (this is a rough test, polish it, integratable, etc. -->
<!-- Until we get config and masters and other fancy stuff, do it yourself -->
<script runat="server">
private void Page_Load(Object sender, EventArgs e)
{
	string server = "127.0.0.1";
	int port = 4242;
	using (Stream s = new TcpClient(server, port).GetStream()) {
		StreamReader sr = new StreamReader(s);
		StreamWriter sw = new StreamWriter(s);
		sw.AutoFlush = true;
		
		sw.WriteLine("ChannelList");
		string outp = sr.ReadToEnd();
		string[] channels = outp.Split('\n');
		foreach (string ci in channels) {
			// string[] c = ci.Split(new char[]{'\t'}, 3);
			string[] c = ci.Split('\t');
			
			if (c.Length == 1)
				break;
			
			TableRow tr = new TableRow();
			TableCell td_channel = new TableCell{Text = c[0]};
			TableCell td_ucount  = new TableCell{Text = c[1]};
			TableCell td_topic = new TableCell{Text = c[2]};
			tr.Cells.Add(td_channel);
			tr.Cells.Add(td_ucount);
			tr.Cells.Add(td_topic);
			table1.Rows.Add(tr);
		}
	}
}
</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head runat="server">
		<title>Channel list for (network)</title>
	</head>
	<body>
		<form id="form1" runat="server">
			<asp:Table id="table1" runat="server">
				<asp:TableHeaderRow>
					<asp:TableCell>Channel</asp:TableCell>
					<asp:TableCell>Users</asp:TableCell>
					<asp:TableCell>Topic</asp:TableCell>
				</asp:TableHeaderRow>
			</asp:Table>
		</form>
	</body>
</html>
