<%@ page title="Home Page" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" inherits="_Default, App_Web_0eysbfcj" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <br />
    <br />


    <asp:FileUpload ID="uploadAudioFile" runat="server" Height="48px" Width="497px" />

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

<asp:Button ID="sendBtn" runat="server" CausesValidation="False" OnClick="SendFile_Click" Text="SendFile" Height="55px" Width="131px" />
    
    <br />
    <br />
    <br />
    <br />
    &nbsp;<input id="directoryInput" runat="server" type="text" text="Browse" />
    &nbsp;&nbsp;
    <asp:Button ID="Button2" runat="server" Text="SendMultiple" OnClick="SendMultiple_Click" Height="63px" Width="259px" />
    <br />
    <br />
    <br />
    
<br />
    <asp:Label ID="resultLabel" runat="server"></asp:Label>












</asp:Content>
