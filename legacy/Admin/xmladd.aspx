<%@ Page Language="VB" AutoEventWireup="false" CodeFile="xmladd.aspx.vb" Inherits="Admin_xmladd" %>

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

    <uc1:top ID="top1" runat="server" />
    <div class="clear">
    </div>
    <!-- start content-outer ........................................................................................................................START -->
    <div id="content-outer">
        <!-- start content -->
        <div id="content">
            <!--  start page-heading -->
            <div id="page-heading">
                <h1>Add New XML file</h1>
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

                                <form id="form1" action="../uploadfile.aspx" method="post" enctype="multipart/form-data">
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
                                                    <input type="file" name="file" id="file" />
                                                    <input type="hidden" name="hdAdmin" id="hdAdmin" value="Admin" />
                                                </td>
                                            </tr>
                                            <tr class="tblrow">
                                                <td valign="middle" align="left" width="20%" class="tblrowheader">Description
                                                </td>
                                                <td valign="middle" align="left" class="tblrow">
                                                    <textarea id="description" name="description" rows="4" cols="70"></textarea>
                                                </td>
                                            </tr>
                                            <tr class="tblrow">
                                                <td valign="middle" align="left" width="20%" class="tblrowheader">Caller Number
                                                </td>
                                                <td valign="middle" align="left" class="tblrow">
                                                     <input type="text" id="txtCaller" name="txtCaller" runat="server" />
                                                </td>
                                            </tr>
                                            <tr class="tblrow" runat="server" id="rowTags">
                                                <td valign="middle" align="left" width="20%" class="tblrowheader">Tags
                                                </td>
                                                <td valign="middle" align="left" class="tblrow">
                                                    <textarea id="mytags" name="mytags" rows="4"></textarea>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <table id="Table1" cellspacing="1" cellpadding="4" border="0" align="center" width="100%">
                                        <tr>
                                            <td align="center">
                                                <input type="submit" name="submit" value="Submit" class="myButton" />

                                            </td>
                                        </tr>
                                    </table>
                                </form>

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
    <script src="https://code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.min.js"></script>
    <script src="js/jquery.caret.min.js"></script>
    <script src="js/jquery.tag-editor.js"></script>
    <script>
        $(function () {
            $('#mytags').tagEditor({
                delimiter: ', ', /* space and comma */
                placeholder: 'Enter tags ...'
            });
        })
            </script>
</body>
</html>
