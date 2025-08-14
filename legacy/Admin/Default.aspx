<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="include/top.ascx" TagName="top" TagPrefix="uc1" %>
<%@ Register Src="include/bottom.ascx" TagName="bottom" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <% =System.Configuration.ConfigurationManager.AppSettings.Item("AdminTitle") %>
    </title>
    <link rel="stylesheet" href="css/screen.css" type="text/css" media="screen" title="default" />
     
</head>
<body>
    <form id="Form1" runat="server">
    <uc1:top ID="top1" runat="server" />
    <div class="clear">
    </div>
    <!-- start content-outer ........................................................................................................................START -->
    <div id="content-outer">
        <!-- start content -->
        <div id="content">
            <!--  start page-heading -->
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
                    <td align="center">
                        <div class="content-table-inner">
                            <div id="page-heading">
                                <h1>
                                   Welcome to CompuDime Call Manager Administration Area</h1>
                            </div>
                        </div>
                    </td>
                    <td id="tbl-border-right">
                    </td>
                </tr>
                <tr>
                    <th class="sized bottomleft">
                    </th>
                    <td id="tbl-border-bottom">
                    </td>
                    <th class="sized bottomright">
                    </th>
                </tr>
            </table>
            <br />
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
    </form>
</body>
</html>
