Imports Microsoft.VisualBasic

Imports System.Data
Imports System.Data.OleDb
Public Class security
    Public Class UserData
        Public UserID As Integer
        Public UserName As String = ""
        Public Password As String = ""
        Public Password1 As String = ""
        Public Uname As String = ""
        Public Email As String = ""
        Public Taglist As String = ""
        Public IsMainAdmin As Boolean = False
        Public canManageTags As Boolean = False


    End Class
    Public Function UserLogin(ByVal Username As String, ByVal Password As String) As UserData


        'Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        'conn.Open()
        'Dim cmd As OleDbCommand = New OleDbCommand("SELECT USERID FROM USERS WHERE ISACTIVE=true AND USERNAME='" & Username & "' AND PASSWORD='" & Password & "'", conn)
        'cmd.CommandType = Data.CommandType.Text
        'Dim Cnt As String = cmd.ExecuteScalar
        'conn.Close()
        'Return Cnt


        Dim Udata As UserData = New UserData
        Udata.UserID = 0
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM USERS WHERE ISACTIVE=true AND USERNAME='" & Username & "' AND PASSWORD='" & Password & "'", conn)
        cmd.CommandType = Data.CommandType.Text
        Dim rec As OleDbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        While rec.Read
            Udata.UserName = Trim(rec.Item("username"))
            Udata.Password = Trim(rec.Item("password"))
            Udata.Uname = Trim(rec.Item("uname"))
            Udata.Email = Trim(rec.Item("email"))
            Udata.Taglist = Trim(IIf(IsDBNull(rec.Item("Taglist")), "", rec.Item("Taglist")))
            Udata.IsMainAdmin = IIf(IsDBNull(rec.Item("IsMainAdmin")), False, rec.Item("IsMainAdmin"))
            Udata.canManageTags = IIf(IsDBNull(rec.Item("canManageTags")), False, rec.Item("canManageTags"))
            Udata.UserID = rec.Item("USERID")
        End While
        rec.Close()
        cmd.Dispose()
        conn.Close()
        Return Udata

    End Function

    Public Function GetUserDataSet(ByVal UserSearch As String) As DataSet
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        'Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM User" & UserSearch, conn)
        Dim Adt As OleDbDataAdapter = New OleDbDataAdapter("SELECT * FROM Users" & UserSearch, conn)
        Dim DS As DataSet = New DataSet
        Adt.Fill(DS)
        'conn.Close()
        Return DS
    End Function
    Public Function GetRights(ByVal Userid As Integer, ByVal Rightscatid As Integer) As OleDbDataReader
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM RIGHTS WHERE ISACTIVE=true AND RIGHTSID IN (SELECT RIGHTSID FROM USERSRIGHTS WHERE USERID=" & Userid & ") ORDER BY RIGHTSID ", conn)
        cmd.CommandType = Data.CommandType.Text
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)
    End Function
    Public Sub DeleteUser(ByVal UserIDString As String)
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("delete from users where userid in (" & UserIDString & ")", conn)
        cmd.CommandType = Data.CommandType.Text
        cmd.ExecuteNonQuery()
        cmd.Dispose()
        conn.Close()
    End Sub
    Public Sub SetActiveDeactiveUser(ByVal UserIDString As String, ByVal AllUserId As String)
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("update users set isactive=false where userid in (" & AllUserId & ")", conn)
        cmd.CommandType = Data.CommandType.Text
        cmd.ExecuteNonQuery()
        cmd.CommandText = "update users set ISACTIVE=true where userid in (" & UserIDString & ")"
        cmd.ExecuteNonQuery()
        cmd.Dispose()
        conn.Close()
    End Sub
    Public Function GetUserName(ByVal UserID As Integer) As String
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select Uname from users WHERE UserID=" & UserID & " ", conn)
        cmd.CommandType = Data.CommandType.Text
        Dim Cnt As String = cmd.ExecuteScalar
        conn.Close()
        Return Cnt
    End Function
    Public Function GetRightsList(ByVal UserID As Integer) As DataTable
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select *,(select count(*) from usersrights where userid=" & UserID & " and rightsid=rights.rightsid) as RCou from rights where parentid=0 and ISACTIVE=true", conn)
        cmd.CommandType = Data.CommandType.Text
        Dim _dt As DataTable = New DataTable()
        Dim _adpa As OleDbDataAdapter = New OleDbDataAdapter(cmd)
        _adpa.Fill(_dt)
        Return _dt
    End Function
    Public Function GetUserDetails(ByVal UserID As Integer) As UserData
        Dim Udata As UserData = New UserData
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select * from users where userid=" & UserID, conn)
        cmd.CommandType = Data.CommandType.Text
        Dim rec As OleDbDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        While rec.Read
            Udata.UserName = Trim(rec.Item("username"))
            Udata.Password = Trim(rec.Item("password"))
            Udata.Uname = Trim(rec.Item("uname"))
            Udata.Email = Trim(rec.Item("email"))
            Udata.Taglist = Trim(IIf(IsDBNull(rec.Item("Taglist")), "", rec.Item("Taglist")))
            Udata.IsMainAdmin = IIf(IsDBNull(rec.Item("IsMainAdmin")), False, rec.Item("IsMainAdmin"))
            Udata.canManageTags = IIf(IsDBNull(rec.Item("canManageTags")), False, rec.Item("canManageTags"))
        End While
        rec.Close()
        cmd.Dispose()
        conn.Close()
        Return Udata
    End Function
    Public Function EditUser(ByVal Udata As UserData) As Double
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("UPDATE USERS set USERNAME='" & Udata.UserName & "',[PASSWORD]='" & Udata.Password & "' ,UNAME='" & Udata.Uname & "',EMAIL='" & Udata.Email & "',Taglist='" & Udata.Taglist & "',IsMainAdmin=" & Udata.IsMainAdmin & ",canManageTags=" & Udata.canManageTags & " where USERID=" & Udata.UserID & "", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()
    End Function
    Public Function AddUser(ByVal Udata As UserData) As Double
        
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        'Dim cmd As OleDbCommand = New OleDbCommand("Insert into USERS (USERNAME,PASSWORD,UNAME,EMAIL,IsActive,adminmanage) values ('" & Udata.UserName & "','" & Udata.Password & "' ,'" & Udata.Uname & "','" & Udata.Email & "',true,0)", conn)
        Dim cmd As OleDbCommand = New OleDbCommand("Insert into USERS(USERNAME,[PASSWORD],UNAME,EMAIL,Taglist,IsMainAdmin,canManageTags,adminmanage,isactive)values ('" & Udata.UserName & "','" & Udata.Password & "','" & Udata.Uname & "','" & Udata.Email & "','" & Udata.Taglist & "'," & Udata.IsMainAdmin & "," & Udata.canManageTags & ",true,true)", conn)
        cmd.CommandType = Data.CommandType.Text
        conn.Open()
        cmd.ExecuteNonQuery()
        conn.Close()

    End Function
    Public Sub InsertUserRight(ByVal UserID As Integer, ByVal RightsID As Integer)
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select count(*) from usersrights where userid=" & UserID & " and rightsid=" & RightsID, conn)
        cmd.CommandType = Data.CommandType.Text
        Dim retval As Integer = cmd.ExecuteScalar
        If retval = 0 Then
            cmd.CommandText = "insert into usersrights values(" & UserID & "," & RightsID & ")"
            cmd.ExecuteNonQuery()
        End If
        cmd.Dispose()
        conn.Close()
    End Sub
    Public Sub DeleteUserRight(ByVal UserID As Integer)
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("delete from usersrights where userid=" & UserID, conn)
        cmd.CommandType = Data.CommandType.Text
        cmd.ExecuteNonQuery()
        cmd.Dispose()
        conn.Close()
    End Sub
    Public Function GetRightsCount(ByVal UserId As Integer, ByVal CatId As Integer) As Integer
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select count(*) from usersrights WHERE userid=" & UserId & " and rightsid in (select rightsid from rights where rightscatid= " & CatId & ")", conn)
        cmd.CommandType = Data.CommandType.Text
        Dim Cnt As Integer = 0
        Try
            Cnt = cmd.ExecuteScalar
        Catch ex As Exception
            Cnt = 0
        End Try
        conn.Close()
        Return Cnt
    End Function
    Public Function GetAdminType(ByVal UserId As Integer) As String
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select AdminType from users WHERE userid=" & UserId & " ", conn)
        cmd.CommandType = Data.CommandType.Text
        Dim RetVal As String = ""
        Try
            RetVal = cmd.ExecuteScalar
        Catch ex As Exception
            RetVal = ""
        End Try
        conn.Close()
        Return RetVal
    End Function
    Public Function GetRightsFromCat(ByVal Search As String) As OleDbDataReader
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM rights " & Search, conn)
        cmd.CommandType = Data.CommandType.Text
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)
    End Function
    Public Function GetRightsParent(ByVal search As String) As OleDbDataReader
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select * from Rights " & search, conn)
        cmd.CommandType = Data.CommandType.Text
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)
    End Function
    

    Public Function IsAutherised(ByVal Userid As Integer, ByVal RightsType As String) As Boolean
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("SELECT count(*) FROM RIGHTS WHERE RightsType='" & RightsType & "' AND ISACTIVE=true AND RIGHTSID IN (SELECT RIGHTSID FROM USERSRIGHTS WHERE USERID=" & Userid & ")", conn)
        cmd.CommandType = Data.CommandType.Text
        Dim Cnt As Integer = 0
        Try
            Cnt = cmd.ExecuteScalar
        Catch ex As Exception
            Cnt = 0
        End Try
        conn.Close()

        If Cnt > 0 Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function
    Public Function getTotalSubMenu(ByVal rightsid As Integer) As Integer
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select count(*) from rights WHERE parentid=" & rightsid & " and ISACTIVE=true", conn)
        cmd.CommandType = Data.CommandType.Text
        Dim Cnt As Integer = 0
        Try
            Cnt = cmd.ExecuteScalar
        Catch ex As Exception
            Cnt = 0
        End Try
        conn.Close()
        Return Cnt
    End Function
    Public Function CheckDuplicate(ByVal search As String) As Integer
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        conn.Open()
        Dim cmd As OleDbCommand = New OleDbCommand("select count(*) from users " & search, conn)
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
    End Function

    Public Function GetMaxid() As Integer
        Dim conn As OleDbConnection = New OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ToString)
        Dim cmd As OleDbCommand = New OleDbCommand("select max(userid) from users", conn)
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
End Class


