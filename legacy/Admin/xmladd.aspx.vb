Imports System.Data.OleDb
Partial Class Admin_xmladd
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtCaller.Value = System.Configuration.ConfigurationManager.AppSettings.Item("fromNumber")
            'Response.Write(Session.Contents("hasTagRights"))
            If Session.Contents("hasTagRights") = "False" Then
                rowTags.Visible = False
            Else
                rowTags.Visible = True
            End If
        End If
    End Sub
End Class
