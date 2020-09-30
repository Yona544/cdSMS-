Imports System.Xml
Imports System.IO
Partial Class Admin_xmledit
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim RecId As Integer = 0
            Try
                RecId = Request.QueryString("id")
            Catch ex As Exception
                RecId = 0
            End Try
            If Not IsNumeric(RecId) Or RecId = Nothing Then
                RecId = 0
            End If

            Dim XMLObj As VoiceClass = New VoiceClass
            Dim FileName As String = XMLObj.getfilename(" where id=" & RecId)

            Dim path As String = Server.MapPath("..\files\XML\") & FileName


            ltFilename.Text = FileName

            Dim contentxml As XmlDocument = New XmlDocument()
            Try
                contentxml.Load(path)
            Catch ex As Exception

            End Try

            


        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click



        Dim RecId As Integer = 0
        Try
            RecId = Request.QueryString("id")
        Catch ex As Exception
            RecId = 0
        End Try
        If Not IsNumeric(RecId) Or RecId = Nothing Then
            RecId = 0
        End If

        Dim XMLObj As VoiceClass = New VoiceClass
        Dim FileName As String = XMLObj.getfilename(" where id=" & RecId)

        Dim path As String = Server.MapPath("..\files\XML\") & FileName

        File.WriteAllText(path, txtXml.Text)

        Response.Redirect("xmlfilelist.aspx")
    End Sub
End Class
