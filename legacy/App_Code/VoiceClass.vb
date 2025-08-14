Imports Microsoft.VisualBasic

Imports System.Data
Imports System.Data.OleDb

Public Class VoiceClass
    Public Class VoiceData
        Public ID As Integer
        Public Name As String
        Public VoiceText As String
        Public VoiceGender As String
        Public VoiceAge As Integer = 0
        Public VoiceRate As String
        Public TropoVoice As String
        Public IsActive As Boolean
        Public VoiceType As String
    End Class

    Public Class VoiceFileData
        Public Id As Integer
        Public FileName As String
        Public FileType As String
        Public EntryDate As String
        Public Description As String
        Public Taglist As String
        Public CallerNumber As String
    End Class

    Public Function AddVoice(ByVal DisData As VoiceData) As Double
      
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("Insert into Voice(Name,VoiceText,VoiceGender,VoiceAge,VoiceRate,IsActive,TropoVoice,VoiceType)values ('" & DisData.Name & "','" & DisData.VoiceText & "' ,'" & DisData.VoiceGender & "'," & DisData.VoiceAge & ",'" & DisData.VoiceRate & "'," & DisData.IsActive & ",'" & DisData.TropoVoice & "','" & DisData.VoiceType & "')", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()

    End Function
    Public Function EditVoice(ByVal DisData As VoiceData) As Double
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("UPDATE Voice set Name='" & DisData.Name & "',VoiceText='" & DisData.VoiceText & "' ,VoiceGender='" & DisData.VoiceGender & "',VoiceAge=" & DisData.VoiceAge & ",VoiceRate='" & DisData.VoiceRate & "',IsActive=" & DisData.IsActive & ",TropoVoice='" & DisData.TropoVoice & "',VoiceType='" & DisData.VoiceType & "' where id=" & DisData.ID & "", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()

    End Function
    Public Function GetVoiceReader(ByVal WhereQry As String) As OleDbDataReader
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("select * from Voice" & WhereQry, conn)
        cmd.CommandType = CommandType.Text
        conn.Open()
        Dim rec As OleDbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        Return rec
    End Function
    Public Function GetDiscountDetails(ByVal ID As Integer) As VoiceData
        Dim DisData As VoiceData = New VoiceData
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("select * from Voice where ID=" & ID, conn)
        cmd.CommandType = CommandType.Text
        conn.Open()

        Dim rec As OleDbDataReader = cmd.ExecuteReader
        With DisData
            While rec.Read
                .ID = rec.Item("ID")
                .Name = Trim(rec.Item("Name"))
                .VoiceText = rec.Item("VoiceText")
                .VoiceGender = rec.Item("VoiceGender")
                .VoiceAge = Trim(rec.Item("VoiceAge").ToString)
                .VoiceRate = Trim(rec.Item("VoiceRate"))
                .IsActive = rec.Item("IsActive")
                .TropoVoice = rec.Item("TropoVoice")
                .VoiceType = rec.Item("VoiceType")
            End While
        End With
        rec.Close()
        cmd.Dispose()
        conn.Close()
        Return DisData
    End Function
    Public Function GetVoiceDataSet(ByVal WhereQry As String) As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim Adt As OleDbDataAdapter = New OleDbDataAdapter("select * from Voice " & WhereQry, conn)
        Dim DS As DataSet = New DataSet
        Adt.Fill(DS)
        Return DS
    End Function
    Public Sub SetActiveDiscount(ByVal CatId As String, ByVal AllCatId As String)
        Dim myConn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        myConn.Open()
        Dim myCommand As OleDbCommand = New OleDbCommand("UPDATE Voice SET isactive=false WHERE ID IN (" & AllCatId & ")", myConn)
        myCommand.CommandType = CommandType.Text
        'myConn.Open()
        myCommand.ExecuteNonQuery()
        Dim myCommand1 As OleDbCommand = New OleDbCommand("UPDATE Voice SET isactive=true WHERE ID IN (" & CatId & ")", myConn)
        myCommand1.CommandType = CommandType.Text
        myCommand1.ExecuteNonQuery()
        myConn.Close()
    End Sub
    Public Sub DeleteDiscount(ByVal CatId As String)
        Dim Conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("delete from Voice where ID in (" & CatId & ")", Conn)
        cmd.CommandType = CommandType.Text
        cmd.ExecuteNonQuery()
        Conn.Close()
    End Sub

    Public Sub IncreaseCount(ByVal ID As Integer)
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("update Voice set totaldiscount = totaldiscount +1 where ID=" & ID, conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub
    Public Sub deletefiledetail(ByVal filename As String, ByVal filetype As String)
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("DELETE FROM Voicefile WHERE Filename='" & filename & "' AND filetype='" & filetype & "'", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub
    Public Sub insertfiledetail(ByVal filename As String, ByVal filetype As String, Description As String, Taglist As String, CallerNumber As String)

        Dim timeUtc = DateTime.UtcNow
        Dim easternZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
        Dim easternTime As DateTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone)


        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("insert into Voicefile(Filename,filetype,Description,Taglist,CallerNumber,Entrydate,UpdatedDate) values('" & filename & "','" & filetype & "','" & Description & "','" & Taglist & "','" & CallerNumber & "','" & easternTime & "','" & easternTime & "')", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub
    Public Function CheckDuplicate(ByVal search As String) As Integer
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select count(*) from Voice " & search, conn)
        cmd.CommandType = Data.CommandType.Text
        Dim Cnt As Integer = 0
        Try
            Cnt = cmd.ExecuteScalar
        Catch ex As Exception
            Cnt = 0
        End Try
        conn.Close()
        If Cnt > 0 Then
            Return -2
        End If
        Return Cnt

        Return Cnt
    End Function
    Public Function GetMaxid() As Integer
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("select max(id) from Voice", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        Dim result As Integer
        Try
            result = cmd.ExecuteScalar
        Catch ex As Exception
            result = 0
        End Try

        conn.Close()
        Return result

    End Function

    Public Sub insertQuery(ByVal Qry As String)
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand(Qry, conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub
    Public Function GetVoicefileDataSet(ByVal WhereQry As String) As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim Adt As OleDbDataAdapter = New OleDbDataAdapter("select * from Voicefile " & WhereQry, conn)
        Dim DS As DataSet = New DataSet
        Adt.Fill(DS)
        Return DS
    End Function
    Public Sub Deletefile(ByVal CatId As String)
        Dim Conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("delete from Voicefile where ID in (" & CatId & ")", Conn)
        cmd.CommandType = CommandType.Text
        cmd.ExecuteNonQuery()
        Conn.Close()
    End Sub
    Public Function getfilename(ByVal search As String) As String
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select fileName from Voicefile " & search, conn)
        cmd.CommandType = Data.CommandType.Text
        Dim filename As String = ""
        Try
            filename = cmd.ExecuteScalar
        Catch ex As Exception
            filename = ""
        End Try
        Return filename
    End Function


    Public Function GetFileDetails(ByVal search As String) As VoiceFileData
        Dim VFdata As VoiceFileData = New VoiceFileData
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select * from Voicefile " & search, conn)
        cmd.CommandType = Data.CommandType.Text
        Dim rec As OleDbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        While rec.Read
            VFdata.FileName = Trim(rec.Item("Filename"))
            VFdata.FileType = Trim(rec.Item("filetype"))
            VFdata.Description = Trim(IIf(IsDBNull(rec.Item("Description")), "", rec.Item("Description")))
            VFdata.EntryDate = Trim(rec.Item("EntryDate"))
            VFdata.Taglist = Trim(IIf(IsDBNull(rec.Item("Taglist")), "", rec.Item("Taglist")))
            VFdata.CallerNumber = Trim(IIf(IsDBNull(rec.Item("CallerNumber")), "", rec.Item("CallerNumber")))
        End While
        rec.Close()
        cmd.Dispose()
        conn.Close()
        Return VFdata
    End Function

    Public Sub saveAsVoice(ByVal id As Integer)

        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        'Dim cmd As OleDbCommand = New OleDbCommand("insert into Voice( Name,VoiceText,VoiceGender,VoiceAge,VoiceRate,Isactive,TropoVoice)   select ('copy '+name ),VoiceText,VoiceGender,VoiceAge,VoiceRate,isactive,TropoVoice from voice where id=" & id & "", conn)
        Dim cmd As OleDbCommand = New OleDbCommand("insert into Voice( Name,VoiceText,VoiceGender,VoiceAge,VoiceRate,Isactive,TropoVoice,VoiceType)   select ('copy '+name ),VoiceText,VoiceGender,VoiceAge,VoiceRate,isactive,TropoVoice,VoiceType from voice where id=" & id & "", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()

    End Sub

    Public Function GetMaxidVoiceXML() As Integer
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("select max(id) from VoiceXML", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        Dim result As Integer
        Try
            result = cmd.ExecuteScalar
        Catch ex As Exception
            result = 0
        End Try

        conn.Close()
        Return result

    End Function
    Public Function GetxmlCont(VoiceXMLid As Integer) As String
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("select xmlCont from VoiceXML  where ID=" & VoiceXMLid.ToString(), conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        Dim result As String
        Try
            result = cmd.ExecuteScalar
        Catch ex As Exception
            result = 0
        End Try

        conn.Close()
        Return result

    End Function
    Public Sub EditVoiceFile(ByVal VoiceData As VoiceFileData)

        Dim timeUtc = DateTime.UtcNow
        Dim easternZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
        Dim easternTime As DateTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone)

        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("UPDATE Voicefile set Description='" & VoiceData.Description & "',CallerNumber='" & VoiceData.CallerNumber & "',Taglist='" & VoiceData.Taglist & "',UpdatedDate='" & easternTime & "'  where id=" & VoiceData.Id & "", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()

    End Sub
End Class