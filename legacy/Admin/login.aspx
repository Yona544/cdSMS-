<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="admin_login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <% =System.Configuration.ConfigurationManager.AppSettings.Item("AdminTitle") %>
    </title>
    <link rel="stylesheet" href="css/screen.css" type="text/css" media="screen" title="default" />
</head>
<body id="login-bg">
    <form id="Form1" runat="server">
    
    <!-- Start: login-holder -->
    <div id="login-holder">
        <!-- start logo -->
        <div id="logo-login">
            <a href="default.aspx">
                </a>
        </div>
        <!-- end logo -->
        <div class="clear">
        </div>
        <!--  start loginbox ................................................................................. -->
        <div id="loginbox">
            <!--  start login-inner -->
            <div id="login-inner">
                <center>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#fffffff"></asp:Label></center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <th>
                            Username
                        </th>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="login-inp"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valUserName" runat="server" ErrorMessage="Please Enter Username"
                                ControlToValidate="txtUserName" CssClass="errmsg" ForeColor="White"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Password
                        </th>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="login-inp">admin</asp:TextBox>
                            <asp:RequiredFieldValidator ID="valPassword" runat="server" ErrorMessage="Please Enter Password"
                                ControlToValidate="txtPassword" CssClass="errmsg" ForeColor="White"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                        </th>
                        <td>
                            <asp:Button ID="btnLogin" runat="server" CssClass="submit-login"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
            <!--  end login-inner -->
            <div class="clear">
            </div>
        </div>
        <!--  end loginbox -->
    </div>
    <!-- End: login-holder -->
    </form>
</body>
</html>
