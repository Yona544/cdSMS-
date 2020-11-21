
Partial Class uploadmp3
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim VoiceOBj As VoiceClass = New VoiceClass

        Dim Description As String = ""
        Dim Taglist As String = ""
        Dim CallerId As String = ""

        Description = Request.Form("description")
        Taglist = Request.Form("mytags")
        CallerId = Request.Form("txtCaller")

        For Each file As String In Request.Files
            Dim hpf As HttpPostedFile = TryCast(Request.Files(file), HttpPostedFile)
            Dim filename As String = hpf.FileName
            Dim isAdmin As String = Request.Form("hdAdmin")
            If hpf.ContentLength = 0 Then
                Continue For
            End If
            If filename.ToLower.EndsWith(".mp3") Then
                Try
                    Dim path As String = Server.MapPath("files\mp3\") & filename
                    hpf.SaveAs(path)
                    VoiceOBj.deletefiledetail(filename, "MP3")
                    VoiceOBj.insertfiledetail(filename, "MP3", Description, Taglist, CallerId)
                Catch ex As Exception
                End Try
            End If

            If isAdmin = "Admin" Then
                Response.Redirect("Admin/mp3filelist.aspx")
            End If
        Next
    End Sub
End Class
