<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:FileUpload ID="uploadAudioFile" runat="server" Height="35px" Width="421px" />

<asp:Button ID="sendBtn" runat="server" CausesValidation="False" OnClick="Button1_Click" Text="Send" />
    
<br />
    <asp:Label ID="resultLabel" runat="server"></asp:Label>












</asp:Content>
