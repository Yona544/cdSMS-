
Partial Class admin_logout
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        FormsAuthentication.SignOut()
        Session.Contents("userid") = Nothing
        Session.Contents("IsAuthorized") = False
        Response.Redirect("login.aspx")

    End Sub
End Class
