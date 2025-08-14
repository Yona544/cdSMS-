Imports System.Data.OleDb
Imports System.Text
Partial Class admin_include_top
    Inherits System.Web.UI.UserControl
    Public MenuNumber As Double
    Public MenuScript As String = ""

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'to load link navigation for login users

       


        If Len(Session.Contents("boothid")) > 0 Then
            Dim select_substr As String = "select_sub"
            MenuScript = MenuScript & "<ul class='current' style>"
            select_substr = "select_sub show"
            MenuScript = MenuScript & "<ul class='select'>"
            MenuScript = MenuScript & "<li><a href='#nogo'><b>" & "Events" & "</b><!--[if IE 7]><!--></a><!--<![endif]--> <!--[if lte IE 6]><table><tr><td><![endif]-->"
            MenuScript = MenuScript & "<div  class='" & select_substr & "' style='width:" & "" & "px;padding-left:" & "" & "px;'>"
            MenuScript = MenuScript & "<ul class='sub'>"
            MenuScript = MenuScript & "<li><a  runat='server' href='" & "Eventsmain.aspx?id=" & Session.Contents("boothid") & "' style='color:#94b52c;'>" & "Manage Events" & "</a></li>"
            MenuScript = MenuScript & " </ul>"
            MenuScript = MenuScript & "</div>"
            MenuScript = MenuScript & "<!--[if lte IE 6]></td></tr></table></a><![endif]-->"
            MenuScript = MenuScript & "</li>"
            MenuScript = MenuScript & " </ul>"
            MenuScript = MenuScript & "<div class='nav-divider'>"
            MenuScript = MenuScript & "&nbsp;</div>"

            
        Else
            If Len(Session.Contents("userid")) > 0 And IsNumeric(Session.Contents("userid")) Then
                'Dim Sec As security = New security
                ''Dim srData As OleDbDataReader
                'dgRightAdminLinks.DataSource = Sec.GetRights(Session.Contents("userid"))
                'dgRightAdminLinks.DataBind()
            Else
                FormsAuthentication.SignOut()
                Session.Abandon()
                Response.Redirect("login.aspx?nosession")
            End If

            'ltSTime.Text = Today.Now.ToString
            'ScriptManager.RegisterStartupScript(Page, GetType(Page), "d", "alert('" & MenuNumber & "');", True)
            Dim strURL As String = Request.Url.ToString.Substring(Request.Url.ToString.LastIndexOf("/")).ToLower
            Dim Padding(20) As String
            Dim PrevMenu As String = ""
            Dim Count As String = 0
            Padding(0) = "0"
            Dim Sec As security = New security
            Dim Rec As OleDbDataReader = Sec.GetRightsParent(" where rightsid in (select rightsid from usersrights where userid=" & Session.Contents("userid") & ") and parentid=0 order by orderno")
            Dim MenuWidth As Integer = 0
            While Rec.Read
                Dim calcpad As Integer = 0
                If Count > 0 Then
                    Dim submenucount As Integer = Sec.getTotalSubMenu(Rec.Item("rightsid"))
                    calcpad = Padding(Count - 1) + 50 + (Len(Trim(PrevMenu)) - 1) * 8 + 4 ' for each char it takes 8px width more
                    Padding(Count) = calcpad
                    If submenucount > 3 And (calcpad - (submenucount * 50)) > 0 Then
                        calcpad = calcpad - (submenucount * 50)
                    End If
                End If
                MenuWidth = 1202 - calcpad
                PrevMenu = Rec.Item("Description")
                Dim MenuIntegerNumber As String
                Try
                    MenuIntegerNumber = MenuNumber.ToString.Substring(0, MenuNumber.ToString.LastIndexOf("."))
                Catch ex As Exception
                    MenuIntegerNumber = "0"
                End Try
                Dim select_substr As String = "select_sub"

                Dim recSub As OleDbDataReader = Sec.GetRightsFromCat(" where  ParentID=" & Rec.Item("rightsID") & " and Isactive=true order by orderno ")
                If MenuIntegerNumber = Convert.ToDouble(Rec.Item("Menuid")) Then
                    MenuScript = MenuScript & "<ul class='current' style>"
                    select_substr = "select_sub show"
                Else
                    MenuScript = MenuScript & "<ul class='select'>"
                End If

                MenuScript = MenuScript & "<li><a href='#nogo'><b>" & Rec.Item("Description") & "</b><!--[if IE 7]><!--></a><!--<![endif]--> <!--[if lte IE 6]><table><tr><td><![endif]-->"
                MenuScript = MenuScript & "<div  class='" & select_substr & "' style='width:" & MenuWidth & "px;padding-left:" & calcpad & "px;'>"
                MenuScript = MenuScript & "<ul class='sub'>"

                While recSub.Read
                    If recSub.Item("menuid") = MenuNumber Then
                        MenuScript = MenuScript & "<li><a  runat='server' href='" & recSub.Item("pagename") & "' style='color:#94b52c;'>" & recSub.Item("Description") & "</a></li>"
                    Else
                        MenuScript = MenuScript & "<li><a  runat='server' href='" & recSub.Item("pagename") & "'>" & recSub.Item("Description") & "</a></li>"
                    End If
                End While

                MenuScript = MenuScript & " </ul>"
                MenuScript = MenuScript & "</div>"
                MenuScript = MenuScript & "<!--[if lte IE 6]></td></tr></table></a><![endif]-->"
                MenuScript = MenuScript & "</li>"
                MenuScript = MenuScript & " </ul>"
                MenuScript = MenuScript & "<div class='nav-divider'>"
                MenuScript = MenuScript & "&nbsp;</div>"
                recSub.Close()
                Count = Count + 1
            End While
            Rec.Close()
        End If

        

   
    End Sub
End Class
