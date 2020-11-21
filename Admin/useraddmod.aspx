<%@ Page Language="VB" AutoEventWireup="false" CodeFile="useraddmod.aspx.vb" Inherits="admin_useraddmod" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="include/top.ascx" TagName="top" TagPrefix="uc1" %>
<%@ Register Src="include/bottom.ascx" TagName="bottom" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= System.Configuration.ConfigurationManager.AppSettings.Item("AdminTitle")%>
    </title>
    <link rel="stylesheet" href="css/screen.css" type="text/css" media="screen" title="default" />
    <link rel="stylesheet" href="css/jquery.tag-editor.css">
</head>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <uc1:top ID="top1" runat="server" />
    <div class="clear">
    </div>
    <!-- start content-outer ........................................................................................................................START -->
    <div id="content-outer">
        <!-- start content -->
        <div id="content">
            <!--  start page-heading -->
            <div id="page-heading">
                <h1>
                    Admin Access Management</h1>
            </div>
            <!-- end page-heading -->
            <table border="0" width="100%" cellpadding="0" cellspacing="0" id="content-table">
                <tr>
                    <th rowspan="3" class="sized">
                        <img src="images/side_shadowleft.jpg" width="20" height="300" alt="" />
                    </th>
                    <th class="topleft">
                    </th>
                    <td id="tbl-border-top">
                        &nbsp;
                    </td>
                    <th class="topright">
                    </th>
                    <th rowspan="3" class="sized">
                        <img src="images/side_shadowright.jpg" width="20" height="300" alt="" />
                    </th>
                </tr>
                <tr>
                    <td id="tbl-border-left">
                    </td>
                    <td>
                        <!--  start content-table-inner ...................................................................... START -->
                        <div class="content-table-inner">
                            <!--  start table-content  -->
                            <div id="table-content">
                                <!--  start product-table ..................................................................................... -->
                             <%--   <asp:UpdatePanel runat="server" ID="update1" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                        <table class="gridtable" id="Table2" cellspacing="1" cellpadding="4" width="100%"
                                            border="0">
                                            <tbody>
                                                <tr align="left" class="tblheader">
                                                    <td colspan="2" height="24">
                                                        <asp:Label ID="lblAddEdit" runat="server"></asp:Label>&nbsp;User
                                                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="tblrow">
                                                    <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                        Username
                                                    </td>
                                                    <td valign="middle" align="left" class="tblrow">
                                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="inp-form300"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="valUserName" runat="server" ControlToValidate="txtUserName" Display="None"
                                                            ErrorMessage="Please Enter UserName" ValidationGroup="EventsFields"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr class="tblaltrow">
                                                    <td valign="middle" align="left" class="tblrowheader">
                                                        Password
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="inp-form300"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="valPassword" runat="server" ControlToValidate="txtPassword" Display="None"
                                                            ErrorMessage="Please enter Password" ValidationGroup="EventsFields"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="valPassComp" Display="None" runat="server" ControlToValidate="txtPassword1"
                                                            ErrorMessage="Password Mis Match" ControlToCompare="txtPassword" ValidationGroup="EventsFields"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr class="tblrow">
                                                    <td valign="middle" align="left" class="tblrowheader">
                                                        Retype Password&nbsp;
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtPassword1" runat="server" TextMode="Password" CssClass="inp-form300"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="valRetype" runat="server" ControlToValidate="txtPassword1" Display="None"
                                                            ErrorMessage="Please enter Password" ValidationGroup="EventsFields"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr class="tblaltrow">
                                                    <td valign="top" align="left" class="tblrowheader">
                                                        Name
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="inp-form300"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="valName" runat="server" ControlToValidate="txtName" Display="None" ErrorMessage="Please Enter your Name"
                                                            ValidationGroup="EventsFields"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr class="tblrow">
                                                    <td align="left" class="tblrowheader">
                                                        Email
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="inp-form300"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="valEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter Email"
                                                            Display="none" ValidationGroup="EventsFields"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="valEmailVal" runat="server" Display="None" ControlToValidate="txtEmail"
                                                            ErrorMessage="Please enter Valid Email Address" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            ValidationGroup="EventsFields"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr class="tblrow">
                                                    <td align="left" class="tblrowheader">
                                                        Is Main Admin?
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:CheckBox ID="chkIsMainAdmin" CssClass="checkbox-size" runat="server"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr class="tblrow">
                                                    <td align="left" class="tblrowheader">
                                                        Can manage tags?
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:CheckBox ID="chkTags" CssClass="checkbox-size" runat="server"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr class="tblrow">
                                                    <td align="left" class="tblrowheader">
                                                        Tags
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox TextMode="MultiLine" runat="server" ID="mytags"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <asp:PlaceHolder ID="PhshowRights" runat="server">
                                                    <tr>
                                                        <td class="tblheader" align="center" colspan="2">
                                                            Rights
                                                        </td>
                                                    </tr>
                                                    <asp:Repeater ID="repRights" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="tblrow">
                                                                <td class="tblrowheader">
                                                                    <asp:Label ID="lblRights" Text='<%# trim(Container.DataItem("description").Tostring) %>'
                                                                        runat="server">
                                                                    </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkRights" CssClass="checkbox-size" runat="server" Checked='<%# CheckforRights(Container.DataItem("RCou"),Container.DataItem("rightsid")) %>'>
                                                                    </asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                        <table id="Table1" cellspacing="1" cellpadding="4" border="0" align="center" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="myButton" ValidationGroup="EventsFields">
                                                    </asp:Button><input id="UserID" type="hidden" runat="server" /><input id="AllRightsID"
                                                        type="hidden" runat="server" />
                                                    <asp:ValidationSummary ID="valsum" runat="server" ValidationGroup="EventsFields"
                                                        ShowSummary="false" ShowMessageBox="true" />
                                                </td>
                                            </tr>
                                        </table>
                               <%--     </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                <!--  end product-table................................... -->
                            </div>
                            <!--  end content-table  -->
                            <div class="clear">
                            </div>
                        </div>
                        <!--  end content-table-inner ............................................END  -->
                    </td>
                    <td id="tbl-border-right">
                    </td>
                </tr>
                <tr>
                    <th class="sized bottomleft">
                    </th>
                    <td id="tbl-border-bottom">
                        &nbsp;
                    </td>
                    <th class="sized bottomright">
                    </th>
                </tr>
            </table>
            <div class="clear">
                &nbsp;</div>
        </div>
        <!--  end content -->
        <div class="clear">
            &nbsp;</div>
    </div>
    <!--  end content-outer........................................................END -->
    <div class="clear">
        &nbsp;</div>
    <!-- start footer -->
    <uc2:bottom ID="bottom1" runat="server" />
    <!-- end footer -->
    <script src="https://code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.min.js"></script>
    <script src="js/jquery.caret.min.js"></script>
    <script src="js/jquery.tag-editor.js"></script>
        <script>
            $(function() {
                $('#mytags').tagEditor({
                    delimiter: ', ', /* space and comma */
                    placeholder: 'Enter tags ...'
                });
            })
            </script>
    </form>
</body>
</html>
