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
		
		sw.WriteLine("UserList");
		string outp = sr.ReadToEnd();
		string[] channels = outp.Split('\n');
		foreach (string ci in channels) {
			// string[] c = ci.Split(new char[]{'\t'}, 5);
			string[] c = ci.Split('\t');
			
			if (c.Length == 1)
				break;
			
			TableRow tr = new TableRow();
			TableCell td_username = new TableCell{Text = c[0]};
			TableCell td_realname  = new TableCell{Text = HttpUtility.HtmlEncode(c[1]});
			TableCell td_hostmask = new TableCell{Text = c[2]};
			TableCell td_presence = new TableCell{Text = c[3]};
			TableCell td_operator = new TableCell{Text = c[4]};
			tr.Cells.Add(td_username);
			tr.Cells.Add(td_realname);
			tr.Cells.Add(td_hostmask);
			tr.Cells.Add(td_presence);
			tr.Cells.Add(td_operator);
			table1.Rows.Add(tr);
		}
	}
}
</script>
<asp:Table id="table1" runat="server">
	<asp:TableHeaderRow>
		<asp:TableCell>Username</asp:TableCell>
		<asp:TableCell>Realname</asp:TableCell>
		<asp:TableCell>Hostmask</asp:TableCell>
		<asp:TableCell>Is Away?</asp:TableCell>
		<asp:TableCell>IRC Oper</asp:TableCell>
	</asp:TableHeaderRow>
</asp:Table>
