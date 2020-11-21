Imports TropoCSharp.Tropo
Imports TropoCSharp.Structs

Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports System.Net
Imports System.Xml
Imports System.Collections.Generic


Partial Class uploadfile
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim VoiceOBj As VoiceClass = New VoiceClass

        Dim Description As String = ""
        Dim Taglist As String = ""
        Dim CallerId As String = ""

        Description = Request.Form("description")
        Taglist = Request.Form("mytags")
        CallerId = Request.Form("txtCaller")

        'Response.Write(Taglist)

        For Each file As String In Request.Files
            Dim hpf As HttpPostedFile = TryCast(Request.Files(file), HttpPostedFile)
            Dim mainfilename As String = hpf.FileName
            If hpf.ContentLength = 0 Then
                Continue For
            End If

            If mainfilename.ToLower.EndsWith(".xml") Then
                Try
                    Dim path As String = Server.MapPath("files\XML\") & mainfilename
                    Try
                        hpf.SaveAs(path)
                    Catch ex As Exception
                        hpf = Nothing
                        Exit Sub
                    End Try
                    VoiceOBj.deletefiledetail(mainfilename, "XML")
                    VoiceOBj.insertfiledetail(mainfilename, "XML", Description, Taglist, CallerId)
                    ''''CODE FOR CREATE CALL AND SMS

                    ''hiteshcomment
                    Dim ComFun As CommonFunctions = New CommonFunctions
                    ComFun.runXmlFile(path, mainfilename, CallerId)


                Catch ex As Exception
                    hpf = Nothing

                End Try
            End If
            Try
                hpf = Nothing
            Catch ex As Exception
            End Try


        Next


        Dim isAdmin As String = Request.Form("hdAdmin")

        If isAdmin = "Admin" Then
            Response.Redirect("Admin/xmlfilelist.aspx")
        End If
    End Sub
    Private Function GetXMLContent(ByVal ContentURL As String) As XmlReader
        Try
            'create an HTTP request.
            'Dim wr As HttpWebRequest = CType(WebRequest.Create(ContentURL),  _
            '   HttpWebRequest)

            Dim wr As FileWebRequest = CType(WebRequest.Create(ContentURL),  _
               FileWebRequest)

            'wr.MaximumAutomaticRedirections = 5

            wr.Timeout = 10000 ' 10 seconds
            'get the response object.
            Dim resp As WebResponse = wr.GetResponse()
            Dim stream As Stream = resp.GetResponseStream()
            ' load XML document

            Dim reader As XmlTextReader = New XmlTextReader(stream)
            reader.XmlResolver = Nothing
            wr = Nothing
            stream = Nothing
            resp = Nothing
            Return reader
        Catch ex As Exception
            'return some error code.
        End Try
    End Function
    Public Function say_as(ByVal value As String, ByVal type As String) As String
        If Len(value) > 0 Then
            Dim ssml_start As String = "" '"<?xml version='1.0'?><speak>"
            Dim ssml_end As String = "</say-as>" '"</say-as></speak>"
            Dim ssml As String = "<say-as interpret-as='vxml:" + type + "'>" + value + ""
            Dim complete_string As String = ssml_start + ssml + ssml_end
            Return complete_string
        Else
            Return ""
        End If
        
    End Function
    
End Class
