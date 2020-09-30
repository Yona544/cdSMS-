
Partial Class admin_login
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim sec As Security = New Security
        Dim RetVal As Long = sec.UserLogin(txtUserName.Text, txtPassword.Text)

        If RetVal > 0 Then
            Session.Contents("boothid") = ""
            'Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1, txtUserName.Text, DateTime.Now, DateTime.Now.AddMinutes(30), True, RetVal, FormsAuthentication.FormsCookiePath)
            'Dim encTicket As String = FormsAuthentication.Encrypt(ticket)
            'Response.Cookies.Add(New HttpCookie(FormsAuthentication.FormsCookieName, encTicket))
            Session.Contents("userid") = RetVal
            Session.Contents("IsAuthorized") = True
            Response.Redirect("default.aspx")
        Else
            lblMessage.Text = "Invalid Login Information"
        End If
    End Sub
End Class
