Imports System.Data.OleDb
Partial Class GenerateCallXML
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Dim Message_ID As Integer = 0
            Try
                Message_ID = Request.QueryString("id")
            Catch ex As Exception
                Message_ID = 0
            End Try
            Dim VoiceOBj As VoiceClass = New VoiceClass
            If Message_ID > 0 Then
                Dim XmlString As String = VoiceOBj.GetxmlCont(Message_ID)
                Response.Clear()
                Response.ContentType = "text/xml"
                Response.Write(XmlString)
                Response.End()
            End If
        End If
    End Sub
End Class
