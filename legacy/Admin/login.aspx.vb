Imports System.Data.OleDb
Partial Class admin_login
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Page.IsPostBack = False Then
            Dim MemUsername As String = ""
            Dim MemPassword As String = ""

            Try
                MemUsername = Request.QueryString("uname")
                MemPassword = Request.QueryString("pass")

                If MemUsername.Length > 0 And MemPassword.Length > 0 Then
                    Dim sec As security = New security
                    Dim Udata As security.UserData = New security.UserData
                    Udata = sec.UserLogin(MemUsername, MemPassword)

                    Dim RetVal As Integer = 0
                    RetVal = Udata.UserID
                    If RetVal > 0 Then
                        Session.Contents("boothid") = ""
                        'Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1, txtUserName.Text, DateTime.Now, DateTime.Now.AddMinutes(30), True, RetVal, FormsAuthentication.FormsCookiePath)
                        'Dim encTicket As String = FormsAuthentication.Encrypt(ticket)
                        'Response.Cookies.Add(New HttpCookie(FormsAuthentication.FormsCookieName, encTicket))

                        Session.Contents("userid") = RetVal
                        Session.Contents("usertaglist") = Udata.Taglist
                        Session.Contents("userIsAdmin") = Udata.IsMainAdmin
                        Session.Contents("hasTagRights") = Udata.canManageTags
                        Session.Contents("IsAuthorized") = True


                         

                        Response.Redirect("default.aspx")
                    Else
                        lblMessage.Text = "Invalid Login Information"
                    End If
                End If
                

            Catch ex As Exception

            End Try


           
        End If
    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim sec As security = New security
        Dim Udata As security.UserData = New security.UserData
        Udata = sec.UserLogin(txtUserName.Text, txtPassword.Text)

        Dim RetVal As Integer = 0
        RetVal = Udata.UserID
        If RetVal > 0 Then
            Session.Contents("boothid") = ""
            'Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1, txtUserName.Text, DateTime.Now, DateTime.Now.AddMinutes(30), True, RetVal, FormsAuthentication.FormsCookiePath)
            'Dim encTicket As String = FormsAuthentication.Encrypt(ticket)
            'Response.Cookies.Add(New HttpCookie(FormsAuthentication.FormsCookieName, encTicket))
            Session.Contents("userid") = RetVal
            Session.Contents("usertaglist") = Udata.Taglist
            Session.Contents("userIsAdmin") = Udata.IsMainAdmin
            Session.Contents("hasTagRights") = Udata.canManageTags
            Session.Contents("IsAuthorized") = True
            Response.Redirect("default.aspx")
        Else
            lblMessage.Text = "Invalid Login Information"
        End If
    End Sub
End Class
