<%@ Page Language="VB" AutoEventWireup="false" validateRequest="false" CodeFile="xmledit.aspx.vb" Inherits="Admin_xmledit" %>

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
  
</head>
<body>
     <form id="Form2" runat="server">
    <uc1:top ID="top1" runat="server" />
    <div class="clear">
    </div>
    <!-- start content-outer ........................................................................................................................START -->
    <div id="content-outer">
        <!-- start content -->
        <div id="content">
            <!--  start page-heading -->
            <div id="page-heading">
                <h1>Edit XML file:&nbsp;<asp:Literal ID="ltFilename" runat="server"></asp:Literal></h1>
            </div>
            <!-- end page-heading -->
            <table border="0" width="100%" cellpadding="0" cellspacing="0" id="content-table">
                <tr>
                    <th rowspan="3" class="sized">
                        <img src="images/side_shadowleft.jpg" width="20" height="300" alt="" />
                    </th>
                    <th class="topleft"></th>
                    <td id="tbl-border-top">&nbsp;
                    </td>
                    <th class="topright"></th>
                    <th rowspan="3" class="sized">
                        <img src="images/side_shadowright.jpg" width="20" height="300" alt="" />
                    </th>
                </tr>
                <tr>
                    <td id="tbl-border-left"></td>
                    <td>
                        <!--  start content-table-inner ...................................................................... START -->
                        <div class="content-table-inner">
                            <!--  start table-content  -->
                            <div id="table-content">
                                <!--  start product-table ..................................................................................... -->

                                
                                    <table class="gridtable" id="Table2" cellspacing="1" cellpadding="4" width="100%"
                                        border="0">
                                        <tbody>
                                            <tr align="left" class="tblheader">
                                                <td colspan="2" height="24">
                                                    <asp:Label ID="lblAddEdit" runat="server"></asp:Label>&nbsp;File
                                                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="tblrow">
                                                <td valign="middle" align="left" width="20%" class="tblrowheader">XML File
                                                </td>
                                                <td valign="middle" align="left" class="tblrow">
                                                    <asp:TextBox ID="txtXml" runat="server" Rows="8" Columns="60" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                        </tbody>
                                    </table>
                                    <table id="Table1" cellspacing="1" cellpadding="4" border="0" align="center" width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit"  CssClass="myButton" />

                                            </td>
                                        </tr>
                                    </table>
                                     
                                    
                                    <!--  end product-table................................... -->
                            </div>
                            <!--  end content-table  -->
                            <div class="clear">
                            </div>
                        </div>
                        <!--  end content-table-inner ............................................END  -->
                    </td>
                    <td id="tbl-border-right"></td>
                </tr>
                <tr>
                    <th class="sized bottomleft"></th>
                    <td id="tbl-border-bottom">&nbsp;
                    </td>
                    <th class="sized bottomright"></th>
                </tr>
            </table>
            <div class="clear">
                &nbsp;
            </div>
        </div>
        <!--  end content -->
        <div class="clear">
            &nbsp;
        </div>
    </div>
    <!--  end content-outer........................................................END -->
    <div class="clear">
        &nbsp;
    </div>
    <!-- start footer -->
    <uc2:bottom ID="bottom1" runat="server" />
    <!-- end footer -->
   
</form>
</body>
</html>
