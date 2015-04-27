<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="WaterFilterWeb.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Water Filter</title>
    <meta http-equiv="refresh" content="2" > 
</head>
<body>
    <form id="form1" runat="server">
    <div >
        <div><asp:Label ID="Label13" runat="server" Font-Size="60" Text="Water Filter Data" Height="100px" ></asp:Label></div>
        <div style="margin-left:45px;margin-top:10px;">        
           <asp:Label  runat="server" Font-Size="30" Text="Filter-1" Height="50px" Width="200px"></asp:Label>
            <asp:Label ID="Label1" runat="server" Font-Size="30pt" Text="ON" Height="50px" Width="200px" Font-Overline="False"/>
            <asp:Image Visible="true" runat="server" id="G1" alt="" src="/Images/green.png" height="40" ImageAlign="Top" />
            <asp:Image Visible="false" ViewStateMode="Disabled" runat="server" id="R1" alt="" src="/Images/red.png" height="40"  ImageAlign="Top" />
            
        </div>
       <div style="margin-left:45px;margin-top:10px;">        
           <asp:Label  runat="server" Text="Filter-2" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Label ID="Label2" runat="server"  Text="ON" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Image runat="server" id="G2" alt="" src="/Images/green.png" height="40"  />
            <asp:Image Visible="false" runat="server" id="R2" alt="" src="/Images/red.png" height="40" ImageAlign="Top" />
            </div>
        <div style="margin-left:45px;margin-top:10px;">        
           <asp:Label  runat="server"  Text="Filter-3" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Label ID="Label3" runat="server"  Text="ON" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Image runat="server" id="G3" alt="" src="/Images/green.png" height="40" />
            <asp:Image Visible="false" runat="server" id="R3" alt="" src="/Images/red.png" height="40" ImageAlign="Top" />
            </div>
        <div style="margin-left:45px;margin-top:10px;">        
           <asp:Label  runat="server"  Text="Filter-4" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Label ID="Label4" runat="server" Text="ON" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Image runat="server" id="G4" alt="" src="/Images/green.png" height="40"/>
            <asp:Image Visible="false" runat="server" id="R4" alt="" src="/Images/red.png" height="40" ImageAlign="Top" />
            </div>
        <div style="margin-left:45px;margin-top:10px;">        
           <asp:Label runat="server" Text="Filter-5" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Label ID="Label5" runat="server"  Text="ON" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Image runat="server" id="G5" alt="" src="/Images/green.png" height="40" />
            <asp:Image Visible="false" runat="server" id="R5" alt="" src="/Images/red.png" height="40" ImageAlign="Top" />
            </div>
        <div style="margin-left:45px;margin-top:10px;">        
           <asp:Label  runat="server" Text="Filter-6" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Label ID="Label6" runat="server" Text="ON" Font-Size="30" Height="50px" Width="200px"></asp:Label>
            <asp:Image alt="" runat="server" src="/Images/green.png" height="40" id="G6"/>
            <asp:Image Visible="false" runat="server" id="R6" alt="" src="/Images/red.png" height="40" ImageAlign="Top" />
            

        </div>
        <div style="margin-left:100px;margin-top:10px;">
            <asp:Button ID="btnRefresh" Text="Refresh" Width="300" runat="server" OnClick="btnRefresh_Click"/>
        </div>
    <div style="margin-left:500px;margin-top:50px;"><asp:Label ID="Label7" runat="server" Font-Size="Small" Text="Developed by: Srujan Jha" Height="100px" ></asp:Label></div>
    </div>
    </form>
</body>
</html>
