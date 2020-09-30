<%@ Page Language="VB" AutoEventWireup="false"  EnableEventValidation="false" CodeFile="xmlfilelist.aspx.vb" Inherits="Admin_xmlfilelist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="include/top.ascx" TagName="top" TagPrefix="uc1" %>
<%@ Register Src="include/bottom.ascx" TagName="bottom" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <% =System.Configuration.ConfigurationManager.AppSettings.Item("AdminTitle") %>
    </title>
    <link rel="stylesheet" href="css/screen.css" type="text/css" media="screen" title="default" />
    <link rel="stylesheet" href="css/tab.css" type="text/css" media="screen" title="default" />
    <link rel="stylesheet" href="css/Calendar.css" type="text/css" media="screen" title="default" />
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        } 
    </script>
    <script language="javascript">
        function conformvalidation() {
            if (!(confirm('Are you sure to delete this Item ?'))) {
                return false;
            }
            return true;
        }
        function conformbeforerun() {
            if (!(confirm('Are you sure to run this file?'))) {
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <uc1:top ID="top" runat="server" />
    <div class="clear">
    </div>
    <!-- start content-outer ........................................................................................................................START -->
    <div id="content-outer">
        <!-- start content -->
        <div id="content">
            <!--  start page-heading -->
            <div id="page-heading">
                <h1>
                    XML File MANAGE</h1>
            </div>
            <!-- end page-heading -->
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" id="content-table"
                        height="300">
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
                                    <div id="table-search">
                                        <table class="gridtable" id="Table2" cellspacing="1" cellpadding="4" width="70%"
                                            align="center" border="0">
                                            <tr class="tblheader">
                                                <td valign="middle" align="left" colspan="2">
                                                    Search
                                                </td>
                                            </tr>
                                            <tr class="tblaltrow">
                                                <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                    Name
                                                </td>
                                                <td valign="middle" align="left" class="tblrow">
                                                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="inp-form"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="tblaltrow">
                                                <td valign="middle" align="center" colspan="2">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="myButton" />
                                                </td>
                                            </tr>
                                        </table>
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
                                &nbsp;
                            </td>
                            <th class="sized bottomright">
                            </th>
                        </tr>
                    </table>
                    <br />
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
                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                            <tr>
                                                <td class="page_title" align="right">
                                                  <asp:HyperLink ID="hlNewxml" runat="server" NavigateUrl="xmladd.aspx"> <img src="images/icon_plus.gif" width="21" height="21" alt="" />&nbsp;Add New File</asp:HyperLink>
                                                        <br />
                                                        <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblnorecord" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="page_title" align="left">
                                                    <asp:DataGrid ID="dgDiscount" runat="server" CssClass="gridtable" GridLines="None"
                                                        AllowPaging="true" PagerStyle-Mode="NumericPages" PagerStyle-NextPageText="Next"
                                                        PagerStyle-CssClass="tblrow" PagerStyle-PrevPageText="Prev" PagerStyle-HorizontalAlign="center"
                                                        CellSpacing="1" CellPadding="4" BorderWidth="0" AutoGenerateColumns="False" Width="100%"
                                                        DataKeyField="id" AllowSorting="true">
                                                        <SelectedItemStyle CssClass="tblheader"></SelectedItemStyle>
                                                        <ItemStyle CssClass="tblrow"></ItemStyle>
                                                        <HeaderStyle CssClass="tblheader"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="tblaltrow" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="X" HeaderStyle-CssClass="tblheader">
                                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkdelete" runat="server" Enabled='<%# CheckForDelete(DataBinder.Eval(Container.DataItem, "id"))  %>'
                                                                        Checked="False"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="tblheader" SortExpression="Name">
                                                                <ItemStyle Width="70%" />
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "fileName")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Edit" HeaderStyle-CssClass="tblheader">
                                                                <ItemStyle Width="8%" />
                                                                <ItemTemplate>
                                                                    <a href="xmledit.aspx?id=<%#DataBinder.Eval(Container.DataItem, "id") %>">Edit</a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                             <asp:TemplateColumn HeaderText="Run" HeaderStyle-CssClass="tblheader">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkrun" runat="server" Text="Run" OnClientClick="return conformbeforerun();"
                                                                        CommandName="Edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "id") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Delete" HeaderStyle-CssClass="tblheader">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkRemove" runat="server" Text="Delete" OnClientClick="return conformvalidation();"
                                                                        CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "id") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                            <tr id="ButtonsTR" runat="server">
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Button ID="btnDelete" runat="server" CssClass="myButton" Text="Delete" CausesValidation="False">
                                                                </asp:Button>
                                                            </td>
                                                            <td align="center">
                                                            </td>
                                                            <td align="right">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="AllCatIds" runat="server" />
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <center>
                <asp:UpdateProgress runat="server" ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <img src="images/Loading.gif" alt="" border="0" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </center>
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
    <uc2:bottom ID="bottom" runat="server" />
    <!-- end footer -->
    </form>
</body>
</html>
