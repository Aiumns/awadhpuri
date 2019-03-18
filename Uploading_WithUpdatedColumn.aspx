<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Uploading_WithUpdatedColumn.aspx.cs" Inherits="Cargo_Uploading_WithUpdatedColumn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" Runat="Server">
   <script language="javascript" type="text/javascript">
       function Validate() {
           var fileupload = document.getElementById("<%=FileUpload1.ClientID%>");

           var FileName = fileupload.value;
           FileExtension = FileName.split('.').pop().toLowerCase();
           if (FileName.indexOf(".") == -1 || FileExtension != "xls" && FileExtension != "xlsx") {
               alert("Upload .xlsx or .xls file only.");
               return false;
           }
           else {
               return true;
           }
       }
       var prm = Sys.WebForms.PageRequestManager.getInstance();
       prm.add_initializeRequest(InitializeRequest);
       prm.add_endRequest(EndRequest);
       var postBackElement;
       function InitializeRequest(sender, args) {
           if (prm.get_isInAsyncPostBack())
               args.set_cancel(true);
           postBackElement = args.get_postBackElement();

           if (postBackElement.id == 'btnReport' || postBackElement.id == 'btnupload' || postBackElement.id == 'btnSample')//---button id
               $get('UpdateProgress1').style.display = 'block';
       }
       function EndRequest(sender, args) {
           if (postBackElement.id == 'btnReport' || postBackElement.id == 'btnupload' || postBackElement.id == 'btnSample')
               $get('UpdateProgress1').style.display = 'none';
       }  
</script>
<center>
<table class="boldtext">
            <caption>             
                <tr>
                    <td  align="center" style="font-size: large; font-weight: bold; font-style: normal; font-variant: normal; text-transform: none; color: #808080">
                        UPLOAD FAIR DETAILS </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:FileUpload ID="FileUpload1" runat="server" />&nbsp;                        
                        <asp:Button ID="btnupload" runat="server" onclick="btnSample_Click" Text="Upload"  OnClientClick="return Validate();" /> 
                    </td>
                </tr>            
                <tr>
                    <td align="center">
                        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="#FF3300"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                       File Should Be in Excel Format, Contains Data in Sheet1.                      
                    </td>
                </tr>
        </caption>        
        </table>
        <asp:Panel ID="pnlComplete" runat="server" Visible="false">
                    <fieldset>
                        <legend>Report</legend>                        
                        <asp:Label ID="lblMessage1" runat="server" Style="list-style-type: none;">                            
                        </asp:Label>
                        <asp:Button id="btnReport" runat="server" Text="Check Details"  CssClass="But"
                            Visible="false" />
                    </fieldset>
        </asp:Panel>
    </center>
</asp:Content>

