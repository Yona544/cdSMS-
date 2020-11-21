Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Image
Imports System.Drawing.Bitmap
Imports System.IO
Imports System.Drawing.Drawing2D
'Imports TropoCSharp.Tropo
'Imports TropoCSharp.Structs
Imports System.Collections.Generic
Imports System.Net
Imports System.Data.OleDb
Imports System.Xml
Imports Twilio.Rest.Api.V2010.Account

Public Class CommonFunctions
    Sub BindPagingInGrid(ByRef GridRef As DataGrid, ByVal e As DataGridItemEventArgs, Optional ByVal ShowFirstLastImage As Boolean = False, Optional ByVal ShowPrevNextImage As Boolean = False, Optional ByVal ShowFirstLastText As Boolean = False, Optional ByVal ShowPrevNextText As Boolean = False, Optional ByVal ImageFirst As String = "", Optional ByVal ImageLast As String = "", Optional ByVal TextFirst As String = "First", Optional ByVal TextLast As String = "Last", Optional ByVal ImagePrev As String = "", Optional ByVal ImageNext As String = "", Optional ByVal TextPrev As String = "Previous", Optional ByVal TextNext As String = "Next", Optional ByVal TextCSS As String = "", Optional ByVal UsedIn As String = "FrontEnd", Optional ByVal Direction As String = "ltr")
        If e.Item.ItemType = ListItemType.Pager Then
            Dim CurrentPage As Integer = GridRef.CurrentPageIndex
            If GridRef.PageCount > 1 Then
                GridRef.PagerStyle.Visible = True
                Dim btnPrev As LinkButton = New LinkButton
                Dim btnNext As LinkButton = New LinkButton
                Dim btnFirst As LinkButton = New LinkButton
                Dim btnLast As LinkButton = New LinkButton

                If Len(Trim(TextCSS)) > 0 Then
                    btnPrev.CssClass = TextCSS
                    btnNext.CssClass = TextCSS
                    btnFirst.CssClass = TextCSS
                    btnLast.CssClass = TextCSS

                    btnPrev.CausesValidation = False
                    btnNext.CausesValidation = False
                    btnFirst.CausesValidation = False
                    btnLast.CausesValidation = False

                End If

                If ShowPrevNextImage = True Then
                    If CurrentPage <> GridRef.PageCount - 1 Then
                        Dim sImgNext As String = "<img alt='Next' border='0' src='" + ImageNext + "'>"
                        btnNext.Text = sImgNext
                        btnNext.CommandArgument = (CurrentPage + 2).ToString()
                        btnNext.CommandName = "Page"
                        btnNext.Enabled = True
                    Else
                        Dim sImgNext As String = "<img alt='Next' border='0' src='" + ImageNext + "'>"
                        btnNext.Text = sImgNext
                        btnNext.Enabled = False
                    End If
                    If CurrentPage > 0 Then
                        Dim sImgPre As String = "<img alt='Previous' border='0' src='" + ImagePrev + "'>"
                        btnPrev.Text = sImgPre
                        btnPrev.CommandName = "Page"
                        btnPrev.CommandArgument = (CurrentPage).ToString()
                        btnPrev.Enabled = True
                    Else
                        Dim sImgPre As String = "<img alt='Previous' border='0' src='" + ImagePrev + "'>"
                        btnPrev.Text = sImgPre
                        btnPrev.Enabled = False
                    End If
                ElseIf ShowPrevNextText = True Then
                    If CurrentPage <> GridRef.PageCount - 1 Then
                        Dim sTextNext As String = TextNext
                        btnNext.Text = sTextNext
                        btnNext.CommandArgument = (CurrentPage + 2).ToString()
                        btnNext.CommandName = "Page"
                        btnNext.Enabled = True
                    Else
                        Dim sTextNext As String = TextNext
                        btnNext.Text = sTextNext
                        btnNext.Enabled = False
                    End If
                    If CurrentPage > 0 Then
                        Dim sTextPre As String = TextPrev
                        btnPrev.Text = sTextPre
                        btnPrev.CommandName = "Page"
                        btnPrev.CommandArgument = (CurrentPage).ToString()
                        btnPrev.Enabled = True
                    Else
                        Dim sTextPre As String = TextPrev
                        btnPrev.Text = sTextPre
                        btnPrev.Enabled = False
                    End If
                End If

                If ShowFirstLastImage = True Then

                    If CurrentPage <> GridRef.PageCount - 1 Then
                        Dim sImgNext As String = "<img alt='Last' border='0' src='" & ImageLast & "'>"
                        btnLast.CommandArgument = GridRef.PageCount.ToString()
                        btnLast.CommandName = "Page"
                        btnLast.Text = sImgNext
                        btnLast.Enabled = True
                    Else
                        Dim sImgNext As String = "<img alt='Last' border='0' src='" & ImageLast & "'>"
                        btnLast.Text = sImgNext
                        btnLast.Enabled = False
                    End If

                    If CurrentPage <> 0 Then
                        Dim sImgPre As String = "<img alt='First' border='0' src='" & ImageFirst & "'>"
                        btnFirst.CommandArgument = "1"
                        btnFirst.CommandName = "Page"
                        btnFirst.Text = sImgPre
                        btnFirst.Enabled = True
                    Else
                        Dim sImgPre As String = "<img alt='First' border='0' src='" & ImageFirst & "'>"
                        btnFirst.Text = sImgPre
                        btnFirst.Enabled = False
                    End If

                ElseIf ShowFirstLastText = True Then

                    If CurrentPage <> GridRef.PageCount - 1 Then
                        btnLast.CommandArgument = GridRef.PageCount.ToString()
                        btnLast.CommandName = "Page"
                        btnLast.Text = TextLast
                        btnLast.Enabled = True
                    Else
                        btnLast.Text = TextLast
                        btnLast.Enabled = False
                    End If

                    If CurrentPage <> 0 Then
                        btnFirst.CommandArgument = "1"
                        btnFirst.CommandName = "Page"
                        btnFirst.Text = TextFirst
                        btnFirst.Enabled = True
                    Else
                        btnFirst.Text = TextFirst
                        btnFirst.Enabled = False
                    End If

                End If
                Dim spLabel1 As Literal = New Literal
                Dim spLabel2 As Literal = New Literal
                Dim spLabel3 As Literal = New Literal
                Dim spLabel4 As Literal = New Literal
                Dim StartExtraTag As Literal = New Literal

                Dim startingCount As Integer = e.Item.Cells(0).Controls.Count
                Dim StartIndex As Integer = 0

                If UsedIn = "FrontEnd" Then
                    StartExtraTag.Text = "</td></tr><tr><td colspan='17' align='center'"
                    If Len(Trim(TextCSS)) > 0 Then
                        StartExtraTag.Text = StartExtraTag.Text & " class='" & TextCSS & "'"
                    End If
                    StartExtraTag.Text = StartExtraTag.Text & "><table align='center' border='0' dir='rtl' celpedding='2' celspecing='1' width='10%'><tr><td><nobr>"
                    StartIndex = 0
                Else
                    StartExtraTag.Text = "<table align='center' border='0' dir='rtl' celpedding='2' celspecing='1' width='10%'><tr><td><nobr>"
                    StartIndex = 1
                End If


                'EndExtraTag.Text = ""

                spLabel1.Text = "&nbsp;&nbsp;&nbsp;"
                spLabel2.Text = "&nbsp;&nbsp;&nbsp;"
                spLabel3.Text = "&nbsp;&nbsp;&nbsp;"
                spLabel4.Text = "&nbsp;&nbsp;&nbsp;"
                e.Item.Cells(0).Controls.AddAt(0, spLabel1)
                e.Item.Cells(0).Controls.AddAt(0, btnPrev)
                e.Item.Cells(0).Controls.AddAt(e.Item.Cells(0).Controls.Count, spLabel2)
                e.Item.Cells(0).Controls.AddAt(e.Item.Cells(0).Controls.Count, btnNext)
                'Add These 2 Images at the First and last Position 
                e.Item.Cells(0).Controls.AddAt(0, spLabel3)
                e.Item.Cells(0).Controls.AddAt(0, btnFirst)
                e.Item.Cells(0).Controls.AddAt(e.Item.Cells(0).Controls.Count, spLabel4)
                e.Item.Cells(0).Controls.AddAt(e.Item.Cells(0).Controls.Count, btnLast)
                e.Item.Cells(0).Controls.AddAt(0, StartExtraTag)

                Dim EndExtraTag As Literal = New Literal
                EndExtraTag.Text = "</td></tr></table>"

                Dim ControlCount As Integer = startingCount + (startingCount * 2) + 3

                If Direction = "rtl" Then
                    For i As Integer = StartIndex To ControlCount Step 2

                        Dim st As Literal = New Literal
                        st.Text = "</nobr></td><td align='center' "
                        If Len(Trim(TextCSS)) > 0 Then
                            st.Text = st.Text & " class='" & TextCSS & "'"
                        End If
                        st.Text = st.Text & "><nobr>"
                        Try
                            e.Item.Cells(0).Controls.AddAt(i, st)
                        Catch ex As Exception

                        End Try

                    Next

                End If


                e.Item.Cells(0).Controls.AddAt(e.Item.Cells(0).Controls.Count, EndExtraTag)

            Else
                GridRef.PagerStyle.Visible = False
            End If
        End If
    End Sub
    Public Sub runXmlFile(Path As String, ActualFileName As String, CallerNumber As String)
        Dim myRSSFeed As New DataSet

        Try
            'Dim wr As FileWebRequest = CType(WebRequest.Create(Path), FileWebRequest)
            'wr.Timeout = 10000 ' 10 seconds
            ''get the response object.
            'Dim resp As WebResponse = wr.GetResponse()
            'Dim stream As Stream = resp.GetResponseStream()
            'Dim reader As XmlTextReader = New XmlTextReader(stream)
            'reader.XmlResolver = Nothing
            'contentxml()

            myRSSFeed.ReadXml(Path)


        Catch ex As Exception
            Exit Sub
        End Try

        Dim customers As DataTable = myRSSFeed.Tables("customers")
        Dim customer As DataTable = myRSSFeed.Tables("customer")
        Dim SMSMessage_ID As Integer = 0

        Dim SMSVoiceText As String = ""
        Dim SMSVoiceGender As String = ""
        Dim SMSVoiceAge As Integer = 0
        Dim SMSVoiceRate As String = ""
        Dim VoiceOBj As VoiceClass = New VoiceClass
        Dim SMSVoiceTextalter As String = ""

        If (customers.Rows.Count > 0) Then
            Try
                SMSMessage_ID = customers.Rows(0)("SMSMessage_ID").ToString()
            Catch ex As Exception
                SMSMessage_ID = 0
            End Try
            If SMSMessage_ID > 0 Then
                Dim rec As OleDbDataReader = VoiceOBj.GetVoiceReader(" where id=" & SMSMessage_ID)
                While rec.Read
                    SMSVoiceText = rec.Item("VoiceText")
                End While

                rec.Close()
            End If

        End If
        If (customer.Rows.Count > 0) Then
            Dim callNo As String = ""
            Dim smsNo As String = ""
            For Each row As DataRow In customer.Rows
                SMSVoiceTextalter = SMSVoiceText
                Dim Message_ID As Integer = 0
                'Dim SMSMessage_ID As Integer = 0
                Dim VoiceText As String = ""
                Dim VoiceGender As String = ""
                Dim VoiceAge As Integer = 0
                Dim VoiceRate As String = ""
                Dim TropoVoice As String = ""




                Dim VoiceTextalter As String = ""


                For Each column As DataColumn In customer.Columns
                    If column.ColumnName = "Message_ID" Then
                        Message_ID = row(column).ToString()
                        If Message_ID > 0 Then
                            Dim rec As OleDbDataReader = VoiceOBj.GetVoiceReader(" where id=" & Message_ID)
                            While rec.Read
                                VoiceText = rec.Item("VoiceText")
                                VoiceGender = rec.Item("VoiceGender")
                                VoiceAge = rec.Item("VoiceAge")
                                VoiceRate = rec.Item("VoiceRate")
                                TropoVoice = rec.Item("TropoVoice")
                            End While
                            rec.Close()
                            VoiceTextalter = VoiceText
                        End If


                    End If


                    If column.ColumnName = "CALL" Then
                        callNo = row(column).ToString()
                    End If
                    If column.ColumnName = "sms" Then
                        smsNo = row(column).ToString()
                    End If

                    Dim colname As String = column.ColumnName

                    'THis is FOR VOICE PART---- replace variable and add SSML for VOICE
                    If Len(callNo) > 0 Then
                        VoiceTextalter = VoiceTextalter.Replace("{" & colname & "}~SSML~number", say_as(row(column).ToString(), "number"))
                        VoiceTextalter = VoiceTextalter.Replace("{" & colname & "}~SSML~currency", say_as(row(column).ToString(), "currency"))
                        VoiceTextalter = VoiceTextalter.Replace("{" & colname & "}~SSML~digits", say_as(row(column).ToString(), "digits"))
                        VoiceTextalter = VoiceTextalter.Replace("{" & colname & "}~SSML~phone", say_as(row(column).ToString(), "telephone"))
                        VoiceTextalter = VoiceTextalter.Replace("{" & colname & "}~SSML~date", say_as(row(column).ToString(), "date"))
                        VoiceTextalter = VoiceTextalter.Replace("{" & colname & "}~SSML~time", say_as(row(column).ToString(), "time"))
                        VoiceTextalter = VoiceTextalter.Replace("{" & colname & "}", row(column).ToString())

                    End If
                    'THis is FOR SMS PART---- replace variable only add SSML for VOICE only Not Support in SMS
                    If Len(smsNo) > 0 Then
                        SMSVoiceTextalter = SMSVoiceTextalter.Replace("{" & colname & "}", row(column).ToString())
                    End If
                Next

                Dim MP3fistpos As Integer = 1
                Dim MP3secpos As Integer = 1
                Do Until MP3fistpos = 0
                    MP3fistpos = InStr(VoiceTextalter, "{MP3~")
                    If MP3fistpos > 0 Then
                        MP3secpos = InStr(MP3fistpos, VoiceTextalter, "}")
                        Dim MP3seekpos As Integer = MP3secpos - MP3fistpos

                        Dim MP3CUTstr As String = VoiceTextalter.Substring(MP3fistpos, MP3seekpos - 1)
                        Dim arr As String() = MP3CUTstr.Split("~")
                        If arr(0).ToString = "MP3" Then
                            Dim Filename As String = ""
                            Filename = System.Configuration.ConfigurationManager.AppSettings("BasePath") & "files/MP3/" & arr(1).ToString & ".mp3"
                            VoiceTextalter = VoiceTextalter.Replace("{MP3~" & arr(1).ToString & "}", Filename)
                        End If
                    End If
                Loop

                Dim Nfistpos As Integer = 1
                Dim Nsecpos As Integer = 1
                Do Until Nfistpos = 0
                    Nfistpos = InStr(VoiceTextalter, "{")
                    If Nfistpos > 0 Then
                        Nsecpos = InStr(Nfistpos, VoiceTextalter, "}")
                        Dim Nseekpos As Integer = Nsecpos - Nfistpos
                        Dim NCUTstr As String = VoiceTextalter.Substring(Nfistpos, Nseekpos - 1)
                        If Len(NCUTstr) > 0 Then
                            VoiceTextalter = VoiceTextalter.Replace("{" & NCUTstr & "}~SSML~number", "")
                            VoiceTextalter = VoiceTextalter.Replace("{" & NCUTstr & "}~SSML~currency", "")
                            VoiceTextalter = VoiceTextalter.Replace("{" & NCUTstr & "}~SSML~digits", "")
                            VoiceTextalter = VoiceTextalter.Replace("{" & NCUTstr & "}~SSML~phone", "")
                            VoiceTextalter = VoiceTextalter.Replace("{" & NCUTstr & "}~SSML~date", "")
                            VoiceTextalter = VoiceTextalter.Replace("{" & NCUTstr & "}~SSML~time", "")
                            VoiceTextalter = VoiceTextalter.Replace("{" & NCUTstr & "}", "")
                        End If
                    End If
                Loop

                If SMSMessage_ID > 0 Then
                    If Len(smsNo) > 0 Then
                        Dim NSMSfistpos As Integer = 1
                        Dim NSMSsecpos As Integer = 1
                        Do Until NSMSfistpos = 0
                            NSMSfistpos = InStr(SMSVoiceTextalter, "{")
                            If NSMSfistpos > 0 Then
                                NSMSsecpos = InStr(NSMSfistpos, SMSVoiceTextalter, "}")
                                Dim NSseekpos As Integer = NSMSsecpos - NSMSfistpos
                                Dim NCUTstr As String = SMSVoiceTextalter.Substring(NSMSfistpos, NSseekpos - 1)
                                If Len(NCUTstr) > 0 Then
                                    SMSVoiceTextalter = SMSVoiceTextalter.Replace("{" & NCUTstr & "}", "")
                                End If
                            End If
                        Loop
                    End If
                End If




                'create call
                If Message_ID > 0 Then

                    If Len(callNo) > 0 Then


                        Dim timeUtc = DateTime.UtcNow
                        Dim easternZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
                        Dim easternTime As DateTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone)


                        Dim Message As String = Environment.NewLine
                        Message = Message & "============"
                        Message = Message & Environment.NewLine
                        Message = Message & easternTime
                        'Message = Message & DateString & " " & TimeString
                        'Message = Message & "============"
                        'Message = Message & Environment.NewLine
                        'Message = Message & easternTime
                        Message = Message & Environment.NewLine
                        Message = Message & "network=Call"
                        Message = Message & Environment.NewLine
                        Message = Message & "Msg=" & VoiceTextalter
                        Message = Message & Environment.NewLine
                        Message = Message & "xmlfile=" & ActualFileName
                        Message = Message & Environment.NewLine
                        Message = Message & "sendToNumber=" & callNo


                        Dim Temppath As String = HttpContext.Current.Server.MapPath("ErrorLog/ErrorLog.txt")
                        Using writer As New StreamWriter(Temppath, True)
                            writer.WriteLine(Message)
                            writer.Close()
                        End Using


                        Dim tmpstr As String = VoiceTextalter
                        'Dim Mainarr() As String
                        Dim Mainarr As ArrayList = New ArrayList
                        Dim fistpos As Integer = 1
                        Dim secpos As Integer = 1
                        Dim tmpremaingstr As String = tmpstr
                        Do Until fistpos = 0

                            fistpos = InStr(tmpremaingstr, "http://")
                            If fistpos > 0 Then
                                secpos = InStr(fistpos, tmpremaingstr, ".mp3")
                                Dim seekpos As Integer = secpos - fistpos
                                Dim tmpFirstval As String = tmpremaingstr.Substring(0, fistpos - 1)
                                Dim tmpSecoundval As String = tmpremaingstr.Substring(fistpos - 1, seekpos + 4)
                                'Mainarr(cnt) = tmpFirstval
                                If Len(tmpFirstval) > 0 Then
                                    Mainarr.Add(tmpFirstval)
                                End If
                                If Len(tmpSecoundval) > 0 Then
                                    Mainarr.Add(tmpSecoundval)
                                End If
                                tmpremaingstr = tmpremaingstr.Substring(secpos + 3, tmpremaingstr.Length - (secpos + 3))
                            End If
                        Loop
                        If Len(tmpremaingstr) > 0 Then
                            Mainarr.Add(tmpremaingstr)
                        End If

                        Dim XmlString As String = "<Response>"
                        For Each num As String In Mainarr
                            If LCase(Left(num, 7)) = "http://" Then
                                XmlString = XmlString & "<Play>" & num & "</Play>"
                            Else
                                If Len(TropoVoice) > 0 Then
                                    XmlString = XmlString & "<Say voice=''" & TropoVoice & "''>"
                                Else
                                    XmlString = XmlString & "<Say voice=''" & VoiceGender & "''>"
                                End If

                                XmlString = XmlString & "<prosody rate=''" & VoiceRate & "''>"
                                XmlString = XmlString & num
                                XmlString = XmlString & "</prosody></Say>"
                            End If
                        Next
                        XmlString = XmlString & "</Response>"

                        VoiceOBj.insertQuery("insert into VoiceXML(xmlCont,XMLFileName) values('" & XmlString & "','" & ActualFileName & "')")

                        Dim VoiceXMLID As Integer = VoiceOBj.GetMaxidVoiceXML()



                        Try
                            Dim accountSid As String = System.Configuration.ConfigurationManager.AppSettings.Item("accountSid")
                            Dim authToken As String = System.Configuration.ConfigurationManager.AppSettings.Item("authToken")
                            Dim _fromNumber As String = ""

                            If CallerNumber.Length > 0 Then
                                _fromNumber = CallerNumber
                            Else
                                _fromNumber = System.Configuration.ConfigurationManager.AppSettings.Item("fromNumber")
                            End If


                            Twilio.TwilioClient.Init(accountSid, authToken)
                            Dim tonumber = New Twilio.Types.PhoneNumber(callNo)
                            Dim fromNumber = New Twilio.Types.PhoneNumber(_fromNumber)

                            VoiceOBj.insertQuery("insert into callLog(network,msg,xmlfile,entrydate) values('Call','" & VoiceTextalter.Replace("'", """") & "','" & ActualFileName & "','" & easternTime & "')")

                            CallResource.Create(url:=New Uri(System.Configuration.ConfigurationManager.AppSettings("BasePath") & "/GenerateCallXML.aspx?id=" & VoiceXMLID), from:=fromNumber, [to]:=tonumber)


                        Catch ex As Exception
                            VoiceOBj.insertQuery("insert into Errorlog(Errormessage,entrydate) values('APPLICATION:->" & ex.Message.ToString.Replace("'", "''") & "','" & easternTime & "')")

                            Dim errMessage As String = Environment.NewLine
                            errMessage = errMessage & "============"
                            errMessage = errMessage & Environment.NewLine
                            errMessage = errMessage & easternTime
                            errMessage = errMessage & Environment.NewLine
                            errMessage = errMessage & "Error=" & ex.Message.ToString

                            Dim errTemppath As String = HttpContext.Current.Server.MapPath("ErrorLog/ErrorLog.txt")
                            Using writer As New StreamWriter(errTemppath, True)
                                writer.WriteLine(errMessage)
                                writer.Close()
                            End Using
                        End Try



                    End If

                End If
                If SMSMessage_ID > 0 Then
                    If Len(smsNo) > 0 Then

                        Dim timeUtc = DateTime.UtcNow
                        Dim easternZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
                        Dim easternTime As DateTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone)

                        Dim Message As String = Environment.NewLine
                        Message = Message & "============"
                        Message = Message & Environment.NewLine
                        'Message = Message & DateString & " " & TimeString
                        'Message = Message & "============"
                        'Message = Message & Environment.NewLine
                        Message = Message & easternTime
                        Message = Message & Environment.NewLine
                        Message = Message & "network=SMS"
                        Message = Message & Environment.NewLine
                        Message = Message & "Msg=" & SMSVoiceTextalter
                        Message = Message & Environment.NewLine
                        Message = Message & "xmlfile=" & ActualFileName
                        Message = Message & Environment.NewLine
                        Message = Message & "sendToNumber=" & callNo


                        Dim Temppath As String = HttpContext.Current.Server.MapPath("ErrorLog/ErrorLog.txt")
                        Using writer As New StreamWriter(Temppath, True)
                            writer.WriteLine(Message)
                            writer.Close()
                        End Using

                        Try
                            Dim accountSid As String = System.Configuration.ConfigurationManager.AppSettings.Item("accountSid")
                            Dim authToken As String = System.Configuration.ConfigurationManager.AppSettings.Item("authToken")

                            Dim _fromNumber As String = ""

                            If CallerNumber.Length > 0 Then
                                _fromNumber = CallerNumber
                            Else
                                _fromNumber = System.Configuration.ConfigurationManager.AppSettings.Item("fromNumber")
                            End If


                            Twilio.TwilioClient.Init(accountSid, authToken)
                            Dim tonumber = New Twilio.Types.PhoneNumber(smsNo)
                            Dim fromNumber = New Twilio.Types.PhoneNumber(_fromNumber)


                            VoiceOBj.insertQuery("insert into callLog(network,msg,xmlfile,entrydate) values('Message','" & SMSVoiceTextalter.Replace("'", """") & "','" & ActualFileName & "','" & easternTime & "')")

                            '       Dim Msg As String = HttpUtility.UrlEncode(SMSVoiceTextalter)
                            MessageResource.Create(body:=SMSVoiceTextalter, from:=fromNumber, [to]:=tonumber)
                        Catch ex As Exception
                            'Dim VoiceOBj As VoiceClass = New VoiceClass
                            VoiceOBj.insertQuery("insert into Errorlog(Errormessage,entrydate) values('APPLICATION:->" & ex.Message.ToString.Replace("'", "''") & "','" & easternTime & "')")

                            Dim errMessage As String = Environment.NewLine
                            errMessage = errMessage & "============"
                            'errMessage = errMessage & DateString & " " & TimeString
                            errMessage = errMessage & Environment.NewLine
                            errMessage = errMessage & easternTime
                            errMessage = errMessage & Environment.NewLine
                            errMessage = errMessage & "Error=" & ex.Message.ToString

                            Dim errTemppath As String = HttpContext.Current.Server.MapPath("ErrorLog/ErrorLog.txt")
                            Using writer As New StreamWriter(errTemppath, True)
                                writer.WriteLine(errMessage)
                                writer.Close()
                            End Using
                        End Try
                    End If
                End If
            Next
        End If
        myRSSFeed.Dispose()
        myRSSFeed = Nothing

        'Try

        'Catch ex As Exception

        '    Dim VoiceOBj As VoiceClass = New VoiceClass
        '    VoiceOBj.insertQuery("insert into Errorlog(Errormessage) values('APPLICATION:->" & ex.Message.ToString.Replace("'", "''") & "')")

        '    Dim Message As String = Environment.NewLine
        '    Message = Message & "============"
        '    Message = Message & DateString & " " & TimeString
        '    Message = Message & Environment.NewLine
        '    Message = Message & "Error=" & ex.Message.ToString

        '    Dim Temppath As String = HttpContext.Current.Server.MapPath("ErrorLog/ErrorLog.txt")
        '    Using writer As New StreamWriter(Temppath, True)
        '        writer.WriteLine(Message)
        '        writer.Close()
        '    End Using

        '    myRSSFeed.Dispose()
        '    myRSSFeed = Nothing

        'End Try
    End Sub
    Public Function say_as(ByVal value As String, ByVal type As String) As String
        If Len(value) > 0 Then
            Dim ssml_start As String = "" '"<?xml version='1.0'?><speak>"
            Dim ssml_end As String = "</say-as>" '"</say-as></speak>"
            Dim ssml As String = "<say-as interpret-as=''vxml:" + type + "''>" + value + ""
            Dim complete_string As String = ssml_start + ssml + ssml_end
            Return complete_string
        Else
            Return ""
        End If

    End Function
End Class
