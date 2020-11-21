Imports System.Xml
Imports System.IO
Partial Class Admin_mp3edit
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

            If Session.Contents("hasTagRights") = "False" Then
                rowTags.Visible = False
            Else
                rowTags.Visible = True
            End If

            Dim XMLObj As VoiceClass = New VoiceClass
            Dim FileData As VoiceClass.VoiceFileData = New VoiceClass.VoiceFileData
            FileData = XMLObj.GetFileDetails(" where id=" & RecId)

            ltXmlFile.Text = FileData.FileName
            txtDescription.Text = FileData.Description
            txtTags.Text = FileData.Taglist
            txtCaller.Text = FileData.CallerNumber


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
        Dim VoiceData As VoiceClass.VoiceFileData = New VoiceClass.VoiceFileData

        VoiceData.Id = RecId
        VoiceData.Description = txtDescription.Text
        VoiceData.Taglist = txtTags.Text
        VoiceData.CallerNumber = txtCaller.Text

        XMLObj.EditVoiceFile(VoiceData)

        

        Response.Redirect("mp3filelist.aspx")
    End Sub
End Class
