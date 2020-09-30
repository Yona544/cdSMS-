<%@ Control Language="VB" AutoEventWireup="false" CodeFile="top.ascx.vb" Inherits="admin_include_top" %>
<script type="text/javascript" src="js/js.js"></script>
<!-- Start: page-top-outer -->
<%--<div id="page-top-outer">
    <!-- Start: page-top -->
    <div id="page-top">
        <!-- start logo -->
        <div id="logo">
            <a href="">
                <img src="images/logo.jpg" width="156" height="40" alt="" border="0" /></a>
        </div>
        <!-- end logo -->
        <!--  start top-search -->
        <!--  end top-search -->
        <div class="clear">
        </div>
    </div>
    <!-- End: page-top -->
</div>--%>
<!-- End: page-top-outer -->
<div class="clear">
    &nbsp;</div>
<!--  start nav-outer-repeat................................................................................................. START -->
<div class="nav-outer-repeat">
    <!--  start nav-outer -->
    <div class="nav-outer">
        <!-- start nav-right -->
        <div id="nav-right">
            <div class="nav-divider">
                &nbsp;</div>
            <a href="logout.aspx" id="logout">
                <img src="images/nav_logout.gif" width="64" height="14" alt="" border="0" /></a>
            <div class="clear">
                &nbsp;</div>
        </div>
        <!-- end nav-right -->
        <!--  start nav -->
        <div class="nav">
            <div class="table" style="width: 1150px;border:0px solid red; ">
                <%=MenuScript%>
                <%--<div class="nav-divider">
                    &nbsp;</div>
                <ul class="select" id="Magazine" runat="server">
                    <li><a href="#nogo"><b>Magazine</b><!--[if IE 7]><!--></a><!--<![endif]-->
                        <!--[if lte IE 6]><table><tr><td><![endif]-->
                        <div id="Magazine_Sub" runat="server" class="select_sub" style="border: solid 0px red;
                            padding-left: 820px;">
                            <ul class="sub">
                                <li><a id="Magazine_81" runat="server" href="../currentmagazine.aspx">Magazine</a></li>
                            </ul>
                        </div>
                        <!--[if lte IE 6]></td></tr></table></a><![endif]-->
                    </li>
                </ul>--%>
                <%--<div class="nav-divider">
                    &nbsp;</div>
                <ul class="select" id="SeedAdmin" runat="server">
                    <li><a href="#nogo"><b>Seed Admin</b><!--[if IE 7]><!--></a><!--<![endif]-->
                        <!--[if lte IE 6]><table><tr><td><![endif]-->
                        <div id="SeedAdmin_sub" runat="server" class="select_sub" style="border: solid 0px red;
                            padding-left: 550px;">
                            <ul class="sub">
                                <li><a id="SeedAdmin_81" runat="server" href="../seed_addr.aspx">Seed Admin</a></li>
                                <li><a id="SeedAdmin_82" runat="server" href="../seed_email.aspx">Manage Seed Email</a></li>
                                <li><a id="SeedAdmin_83" runat="server" href="../seed_name_search.aspx">Seed Name Search</a></li>
                                <li><a id="SeedAdmin_84" runat="server" href="../seed_name_log_report.aspx">Seedname Log report</a></li>
                            </ul>
                        </div>
                        <!--[if lte IE 6]></td></tr></table></a><![endif]-->
                    </li>
                </ul>--%>
                <div class="clear">
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <!--  start nav -->
    </div>
    <div class="clear">
    </div>
    <!--  start nav-outer -->
</div>
<!--  start nav-outer-repeat................................................... END -->
