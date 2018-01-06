<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">       
        <title>Places Path Tracker</title>
        <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBsOBFP1bVFfESwFW4B9tcihA6lOMv96Xs&libraries=places"></script>
        <link href="css/StyleSheet.css" rel="stylesheet" />
        <script src="js/JavaScript.js"></script>
    </head>
    <body onload="SearchPath();">
        <form id="form1" runat="server" class="centerForm">
            <asp:TextBox id="inputSource" placeholder="Enter Source...." runat="server"></asp:TextBox>
            <asp:TextBox id="inputDestination" placeholder="Enter Destination...." runat="server"></asp:TextBox>
            <asp:Button id="showMapButton" runat="server" text="Track Path" OnClientClick="SearchPath(); return false;" cssClass="button button4" />
            <hr/>
        </form>
        <div id="showMap">
        </div>
    </body>
</html>
