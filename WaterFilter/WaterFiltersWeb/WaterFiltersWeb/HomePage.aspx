<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="WaterFiltersWeb.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Water Filter</title>
    <meta http-equiv="refresh" content="2" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div >
        <div><asp:Label ID="Label13" runat="server" Font-Size="40pt" Text="Power Consumption Values" Height="100px"></asp:Label></div>
        <div style="margin-left:100px;margin-top:10px; width: 400px;">        
           <asp:Label  runat="server" Font-Size="30" Text="Meter-1:" Height="50px" Width="150px"></asp:Label>
           <asp:Label ID="Label1" runat="server" Font-Size="30pt" Text="ON" Height="50px" Width="150px" Font-Overline="False"/>
            <!--<asp:Image Visible="true" runat="server" id="G1" alt="" src="/Images/green.png" height="40" ImageAlign="Top" />
            <asp:Image Visible="false" ViewStateMode="Disabled" runat="server" id="R1" alt="" src="/Images/red.png" height="40"  ImageAlign="Top" />
            -->
        </div>
       <div style="margin-left:100px;margin-top:10px; width: 400px;">         
           <asp:Label  runat="server" Text="Meter-2:" Font-Size="30" Height="50px" Width="150px"></asp:Label>
            <asp:Label ID="Label2" runat="server"  Text="ON" Font-Size="30" Height="50px" Width="150px"></asp:Label>
            <!--<asp:Image runat="server" id="G2" alt="" src="/Images/green.png" height="40"  />
            <asp:Image Visible="false" runat="server" id="R2" alt="" src="/Images/red.png" height="40" ImageAlign="Top" />
           --> </div>
        <div style="margin-left:100px;margin-top:10px;width: 400px;">         
           <asp:Label  runat="server"  Text="Meter-3:" Font-Size="30" Height="50px" Width="150px"></asp:Label>
            <asp:Label ID="Label3" runat="server"  Text="ON" Font-Size="30" Height="50px" Width="150px"></asp:Label>
           <!-- <asp:Image runat="server" id="G3" alt="" src="/Images/green.png" height="40" />
            <asp:Image Visible="false" runat="server" id="R3" alt="" src="/Images/red.png" height="40" ImageAlign="Top" />
          -->  </div>
        <div style="margin-left:100px;margin-top:10px;">
            <asp:Button ID="btnRefresh" Text="Refresh" Width="300" runat="server" OnClick="btnRefresh_Click"/>
        </div>
    <div style="margin-left:300px;margin-top:50px;">
        <asp:Label ID="Label7" runat="server" Font-Size="Small" Text="Developed by: Swayam & Samrin" Height="100px" ></asp:Label></div>
    </div>
    </form>
</body>
</html>