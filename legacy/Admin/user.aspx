<%@ Page Language="VB" AutoEventWireup="false" CodeFile="user.aspx.vb" Inherits="admin_user" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="include/top.ascx" TagName="top" TagPrefix="uc1" %>
<%@ Register Src="include/bottom.ascx" TagName="bottom" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%= System.Configuration.ConfigurationManager.AppSettings.Item("AdminTitle")%></title>

    <link rel="stylesheet" href="css/screen.css" type="text/css" media="screen" title="default" />
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
                                <asp:UpdatePanel runat="server" ID="update1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                            <tr>
                                                <td class="page_title" align="right">
                                                    <asp:HyperLink ID="hlNewUser" runat="server" CssClass="adminlink" NavigateUrl="useraddmod.aspx"> <img src="images/icon_plus.gif" width="21" height="21" alt="" />&nbsp;Add New User</asp:HyperLink>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="page_title" align="left">
                                                    <asp:DataGrid ID="dgUser" runat="server" CssClass="gridtable" GridLines="None" AllowPaging="true"
                                                        PagerStyle-Mode="NumericPages" PagerStyle-NextPageText="Next" PagerStyle-CssClass="tblrow"
                                                        PagerStyle-PrevPageText="Prev" PagerStyle-HorizontalAlign="center" CellSpacing="1"
                                                        CellPadding="4" BorderWidth="0" AutoGenerateColumns="False" Width="100%" DataKeyField="userid" AllowSorting="true"  >
                                                        <SelectedItemStyle CssClass="tblheader"></SelectedItemStyle>
                                                        <ItemStyle CssClass="tblrow"></ItemStyle>
                                                        <HeaderStyle CssClass="tblheader"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="tblaltrow" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="X" HeaderStyle-CssClass="tblheader"  >
                                                                <ItemStyle Width="5%" HorizontalAlign="Center"   />
                                                                <HeaderStyle HorizontalAlign="Center" />  
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkdelete" runat="server" Checked="False"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Name" HeaderStyle-CssClass="tblheader" SortExpression="uname">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# "useraddmod.aspx?id=" &  databinder.eval(container.dataitem,"userid")%>'
                                                                        CssClass="adminlink">
																				<%# databinder.eval(container.dataitem,"uname")%>
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Username" HeaderStyle-CssClass="tblheader" SortExpression="username">
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID='lblUserName' runat="server" Text='<%# databinder.eval(container.dataitem,"username")%>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Email" HeaderStyle-CssClass="tblheader" SortExpression="email" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmail" runat="server" Text='<%# databinder.eval(container.dataitem,"Email")%>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Active" HeaderStyle-CssClass="tblheader" SortExpression="isactive" >
                                                                <ItemStyle Width="60px" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkactive" runat="server" Enabled='<%# CheckForDelete(DataBinder.Eval(Container.DataItem, "Userid"))  %>'
                                                                        Checked='<%# databinder.eval(container.dataitem,"isactive")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Edit" HeaderStyle-CssClass="tblheader">
                                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="hlEdit" runat="server" NavigateUrl='<%# "useraddmod.aspx?id=" &  databinder.eval(container.dataitem,"userid")%>'
                                                                        CssClass="icon-4 info-tooltip"></asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                            <tr>
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
                                                                <asp:Button ID="btnUpdate" runat="server" CssClass="myButton" Text="Update" CausesValidation="False">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="AllProdId" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
    </form>
</body>
</html>
