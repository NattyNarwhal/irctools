<%@ Control language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net.Sockets" %>
<script runat="server">
public string Server { get; set; } // services server
public int Port { get; set; }

private void Page_Load(Object sender, EventArgs e)
{
	using (Stream s = new TcpClient(Server, Port).GetStream()) {
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
			TableCell td_channel = new TableCell{Text = c[0]}; // Don't need to encode topic names hopefully
			TableCell td_ucount  = new TableCell{Text = c[1]}; // lolwedon'tneedtoencodethis
			TableCell td_topic = new TableCell{Text = HttpUtility.HtmlEncode(c[2])}; // thank you sam
			tr.Cells.Add(td_channel);
			tr.Cells.Add(td_ucount);
			tr.Cells.Add(td_topic);
			table1.Rows.Add(tr);
		}
	}
}
</script>
<asp:Table id="table1" runat="server">
	<asp:TableHeaderRow>
		<asp:TableCell>Channel</asp:TableCell>
		<asp:TableCell>Users</asp:TableCell>
		<asp:TableCell>Topic</asp:TableCell>
	</asp:TableHeaderRow>
</asp:Table>
