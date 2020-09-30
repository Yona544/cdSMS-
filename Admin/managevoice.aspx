<%@ Page Language="VB" AutoEventWireup="false" CodeFile="managevoice.aspx.vb" Inherits="admin_managevoice" %>

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
    <link rel="stylesheet" href="css/tab.css" type="text/css" media="screen" title="default" />
    <link rel="stylesheet" href="css/Calendar.css" type="text/css" media="screen" title="default" />
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
                    Voice Management</h1>
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
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr valign="top">
                                    <td width="80%">
                                        <!--  start table-content  -->
                                        <div id="table-content">
                                            <!--  start product-table ..................................................................................... -->
                                            <table class="gridtable" id="Table2" cellspacing="1" cellpadding="4" width="100%"
                                                border="0">
                                                <tbody>
                                                    <tr align="left" class="tblheader">
                                                        <td colspan="2" height="24">
                                                            <asp:Label ID="lblAddEdit" runat="server"></asp:Label>&nbsp;
                                                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr class="tblrow">
                                                        <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                            Type
                                                        </td>
                                                        <td valign="middle" align="left" class="tblrow">
                                                            <asp:RadioButton ID="rdCall" Checked="true" runat="server" GroupName="Voicetype"
                                                                runat="server" Text="Call" AutoPostBack="true" /><br />
                                                            <asp:RadioButton ID="rdSMS" runat="server" GroupName="Voicetype" Text="SMS" AutoPostBack="true" /><br />
                                                        </td>
                                                    </tr>
                                                    <tr class="tblrow">
                                                        <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                            Name
                                                        </td>
                                                        <td valign="middle" align="left" class="tblrow">
                                                            <asp:TextBox ID="txtname" runat="server" CssClass="inp-form"></asp:TextBox><asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtname" ValidationGroup="DisFields"
                                                                SetFocusOnError="true" Display="None" ErrorMessage="Please Enter Name"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="tblaltrow">
                                                        <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                            Voice Text
                                                        </td>
                                                        <td valign="middle" align="left" class="tblrow">
                                                            <asp:TextBox ID="txtVoiceText" TextMode="MultiLine" Columns="150" Rows="8" runat="server"
                                                                CssClass="form-textarea550"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                                                                    runat="server" ControlToValidate="txtVoiceText" ValidationGroup="DisFields" SetFocusOnError="true"
                                                                    Display="None" ErrorMessage="Please Enter Voice Text"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                               <%--     <asp:PlaceHolder ID="pnlCallOnly" runat="server">
                                                        <tr class="tblrow">
                                                            <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                                Select Voice
                                                            </td>
                                                            <td valign="middle" align="left" class="tblrow">
                                                                <asp:RadioButton ID="rdSystemVoice" runat="server" AutoPostBack="true" GroupName="TROPO"
                                                                    Text="System Voice" />
                                                                <asp:RadioButton ID="rdCustomVoice" runat="server" AutoPostBack="true" GroupName="TROPO"
                                                                    Text="Custom Voice" />
                                                            </td>
                                                        </tr>
                                                        <asp:PlaceHolder ID="pnlTropovoice" runat="server" Visible="false">--%>
                                                            <tr class="tblrow">
                                                                <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                                    Voice
                                                                </td>
                                                                <td valign="middle" align="left" class="tblrow">
                                                                    <asp:DropDownList ID="ddlVoice" runat="server">
                                                                        <asp:ListItem Value="man" Text="man"></asp:ListItem>
                                                                        <asp:ListItem Value="woman" Text="woman"></asp:ListItem>
                                                                        <asp:ListItem Value="alice" Text="alice"></asp:ListItem>
                                                                        <%--<asp:ListItem Value="Allison" Text="UsEnglishFemale-->Allison"></asp:ListItem>
                                                                        <asp:ListItem Value="Susan" Text="UsEnglishFemale-->Susan"></asp:ListItem>
                                                                        <asp:ListItem Value="Vanessa" Text="UsEnglishFemale-->Vanessa"></asp:ListItem>
                                                                        <asp:ListItem Value="Veronica" Text="UsEnglishFemale-->Veronica"></asp:ListItem>
                                                                        <asp:ListItem Value="Dave" Text="UsEnglishMale-->Dave"></asp:ListItem>
                                                                        <asp:ListItem Value="Steven" Text="UsEnglishMale-->Steven"></asp:ListItem>
                                                                        <asp:ListItem Value="Victor" Text="UsEnglishMale-->Victor"></asp:ListItem>
                                                                        <asp:ListItem Value="Elizabeth" Text="BritishEnglishFemale-->Elizabeth"></asp:ListItem>
                                                                        <asp:ListItem Value="Kate" Text="BritishEnglishFemale-->Kate"></asp:ListItem>
                                                                        <asp:ListItem Value="Simon" Text="BritishEnglishMale-->Simon"></asp:ListItem>
                                                                        <asp:ListItem Value="Grace" Text="AustralianEnglishFemale-->Grace"></asp:ListItem>
                                                                        <asp:ListItem Value="Alan" Text="AustralianEnglishMale-->Alan"></asp:ListItem>
                                                                        <asp:ListItem Value="Montserrat" Text="CatalanFemale-->Montserrat"></asp:ListItem>
                                                                        <asp:ListItem Value="Jordi" Text="CatalanMale-->Jordi"></asp:ListItem>
                                                                        <asp:ListItem Value="Frida" Text="DanishFemale-->Frida"></asp:ListItem>
                                                                        <asp:ListItem Value="Magnus" Text="DanishMale-->Magnus"></asp:ListItem>
                                                                        <asp:ListItem Value="Saskia" Text="DutchFemale-->Saskia"></asp:ListItem>
                                                                        <asp:ListItem Value="Willem" Text="DutchMale-->Willem"></asp:ListItem>
                                                                        <asp:ListItem Value="Milla" Text="FinnishFemale-->Milla"></asp:ListItem>
                                                                        <asp:ListItem Value="Mikko" Text="FinnishMale-->Mikko"></asp:ListItem>
                                                                        <asp:ListItem Value="Florence" Text="FrenchFemale-->Florence"></asp:ListItem>
                                                                        <asp:ListItem Value="Juliette" Text="FrenchFemale-->Juliette"></asp:ListItem>
                                                                        <asp:ListItem Value="Bernard" Text="FrenchMale-->Bernard"></asp:ListItem>
                                                                        <asp:ListItem Value="Charlotte" Text="FrenchCanadianFemale-->Charlotte"></asp:ListItem>
                                                                        <asp:ListItem Value="Olivier" Text="FrenchCanadianMale-->Olivier"></asp:ListItem>
                                                                        <asp:ListItem Value="Carmela" Text="GalacianFemale-->Carmela"></asp:ListItem>
                                                                        <asp:ListItem Value="Katrin" Text="GermanFemale-->Katrin"></asp:ListItem>
                                                                        <asp:ListItem Value="Stefan" Text="GermanMale-->Stefan"></asp:ListItem>
                                                                        <asp:ListItem Value="Afroditi" Text="GreekFemale Afroditi"></asp:ListItem>
                                                                        <asp:ListItem Value="Nikos" Text="GreekMale-->Nikos"></asp:ListItem>
                                                                        <asp:ListItem Value="Giulia" Text="ItalianFemale_Giulia-->Giulia"></asp:ListItem>
                                                                        <asp:ListItem Value="Paola" Text="ItalianFemale-->Paola"></asp:ListItem>
                                                                        <asp:ListItem Value="Silvana" Text="ItalianFemale-->Silvana"></asp:ListItem>
                                                                        <asp:ListItem Value="Valentina" Text="ItalianFemale-->Valentina"></asp:ListItem>
                                                                        <asp:ListItem Value="Luca" Text="ItalianMale-->Luca"></asp:ListItem>
                                                                        <asp:ListItem Value="Marcello" Text="ItalianMale-->Marcello"></asp:ListItem>
                                                                        <asp:ListItem Value="Matteo" Text="ItalianMale-->Matteo"></asp:ListItem>
                                                                        <asp:ListItem Value="Roberto" Text="ItalianMale-->Roberto"></asp:ListItem>
                                                                        <asp:ListItem Value="Linlin" Text="ChineseMandarinMale-->Linlin"></asp:ListItem>
                                                                        <asp:ListItem Value="Lisheng" Text="ChineseMandarinMale-->Lisheng"></asp:ListItem>
                                                                        <asp:ListItem Value="Vilde" Text="NorwegianFemale-->Vilde"></asp:ListItem>
                                                                        <asp:ListItem Value="Henrik" Text="NorwegianMale-->Henrik"></asp:ListItem>
                                                                        <asp:ListItem Value="Zosia" Text="PolishFemale-->Zosia"></asp:ListItem>
                                                                        <asp:ListItem Value="Krzysztof" Text="PolishMale-->Krzysztof"></asp:ListItem>
                                                                        <asp:ListItem Value="Olga" Text="RussianFemale-->Olga"></asp:ListItem>
                                                                        <asp:ListItem Value="Dmitri" Text="RussianMale-->Dmitri"></asp:ListItem>
                                                                        <asp:ListItem Value="Carmen" Text="SpanishCastilianFemale-->Carmen"></asp:ListItem>
                                                                        <asp:ListItem Value="Leonor" Text="SpanishCastilianFemale-->Leonor"></asp:ListItem>
                                                                        <asp:ListItem Value="Jorge" Text="SpanishCastilianMale-->Jorge"></asp:ListItem>
                                                                        <asp:ListItem Value="Juan" Text="SpanishCastilianMale-->Juan"></asp:ListItem>
                                                                        <asp:ListItem Value="Diego" Text="SpanishArgentineMale-->Diego"></asp:ListItem>
                                                                        <asp:ListItem Value="Francisca" Text="SpanishChileanFemale-->Francisca"></asp:ListItem>
                                                                        <asp:ListItem Value="Soledad" Text="SpanishMexicanFemale-->Soledad"></asp:ListItem>
                                                                        <asp:ListItem Value="Ximena" Text="SpanishMexicanFemale-->Ximena"></asp:ListItem>
                                                                        <asp:ListItem Value="Esperanza" Text="SpanishMexicanFemale-->Esperanza"></asp:ListItem>
                                                                        <asp:ListItem Value="Carlos" Text="SpanishMexicanMale-->Carlos"></asp:ListItem>
                                                                        <asp:ListItem Value="Amalia" Text="PortugeseFemale-->Amalia"></asp:ListItem>
                                                                        <asp:ListItem Value="Eusebio" Text="PortugeseMale-->Eusebio"></asp:ListItem>
                                                                        <asp:ListItem Value="Fernanda" Text="PortugeseBrazilianFemale-->Fernanda"></asp:ListItem>
                                                                        <asp:ListItem Value="Gabriela" Text="PortugeseBrazilianFemale-->Gabriela"></asp:ListItem>
                                                                        <asp:ListItem Value="Felipe" Text="PortugeseBrazilianMale-->Felipe"></asp:ListItem>
                                                                        <asp:ListItem Value="Annika" Text="SwedishFemale-->Annika"></asp:ListItem>
                                                                        <asp:ListItem Value="Sven" Text="SwedishMale-->Sven"></asp:ListItem>
                                                                        <asp:ListItem Value="Empar" Text="ValencianMale-->Empar"></asp:ListItem>--%>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                             <%--   </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="pnlVoiceCreate" runat="server" Visible="false">
                                                            <tr class="tblrow">
                                                                <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                                    Voice Gender
                                                                </td>
                                                                <td valign="middle" align="left" class="tblrow">
                                                                    <asp:RadioButton ID="rdmale" Checked="true" runat="server" GroupName="Voice" runat="server"
                                                                        Text="Male" /><br />
                                                                    <asp:RadioButton ID="rdfemail" runat="server" GroupName="Voice" Text="Female" /><br />
                                                                    <asp:RadioButton ID="rdneutral" runat="server" GroupName="Voice" Text="Neutral" /><br />
                                                                </td>
                                                            </tr>
                                                            <%--<tr class="tblaltrow">
                                                            <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                                Voice Age
                                                            </td>
                                                            <td valign="middle" align="left" class="tblrow">
                                                                <asp:TextBox ID="txtVoiceAge" runat="server" CssClass="inp-form"></asp:TextBox><asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtVoiceAge" ValidationGroup="DisFields"
                                                                    SetFocusOnError="True" Display="None" ErrorMessage="Please Enter Voice Age"></asp:RequiredFieldValidator>
                                                                <asp:CompareValidator ID="cvTotalAllowed" runat="server" Display="None" ControlToValidate="txtVoiceAge"
                                                                    ValidationGroup="DisFields" ErrorMessage="Enter Valid Age" Operator="DataTypeCheck"
                                                                    Type="Integer" SetFocusOnError="True"></asp:CompareValidator>
                                                            </td>
                                                        </tr> 
                                                        </asp:PlaceHolder>--%>
                                                        <tr class="tblrow">
                                                            <td valign="middle" align="left" width="20%" class="tblrowheader">
                                                                Voice Rate
                                                            </td>
                                                            <td valign="middle" align="left" class="tblrow">
                                                                <asp:RadioButton ID="rdVoiceRatexslow" Checked="true" runat="server" GroupName="Voicerate"
                                                                    Text="x-slow" /><br />
                                                                <asp:RadioButton ID="rdslow" runat="server" GroupName="Voicerate" Text="slow" /><br />
                                                                <asp:RadioButton ID="rdmedium" runat="server" GroupName="Voicerate" Text="medium" />
                                                                <br />
                                                                <asp:RadioButton ID="rdfast" runat="server" GroupName="Voicerate" Text="fast" /><br />
                                                                <asp:RadioButton ID="rdxfast" runat="server" GroupName="Voicerate" Text="x-fast" /><br />
                                                                <asp:RadioButton ID="rddefault" runat="server" GroupName="Voicerate" Text="default" /><br />
                                                            </td>
                                                        </tr>
                                                    </asp:PlaceHolder>
                                                    <tr class="tblrow">
                                                        <td align="left" class="tblrowheader">
                                                            Active
                                                        </td>
                                                        <td valign="middle" align="left">
                                                            <asp:CheckBox ID="chkActiveMain" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div class="clear">
                                            </div>
                                            <div id="actions-box">
                                                <br />
                                                <br />
                                                <asp:HiddenField ID="hlDiscountId" runat="server" />
                                                <center>
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="DisFields" CssClass="myButton">
                                                    </asp:Button></center>
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="DisFields"
                                                    ShowSummary="false" ShowMessageBox="true" />
                                            </div>
                                            <!--  end product-table................................... -->
                                        </div>
                                        <!--  end content-table  -->
                                        <!--  start actions-box ............................................... -->
                                        <!-- end actions-box........... -->
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                            </table>
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
