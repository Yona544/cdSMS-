
Partial Class admin_useraddmod
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim TopControl As admin_include_top = New admin_include_top
        TopControl = Me.FindControl("top1")
        TopControl.MenuNumber = 5.0
        If Page.IsPostBack = False Then

            Dim URObj As security = New security
            Dim GetRights As Boolean = URObj.IsAutherised(Session.Contents("userid"), "Config")
            If GetRights = False Then
                Response.Redirect("default.aspx")
            End If

            lblAddEdit.Text = "Add "
            lblError.Text = ""
            btnSave.Text = "Add"
            UserID.Value = "0"
            If Request.QueryString.Count > 0 Then
                If Request.QueryString("id").Length > 0 And IsNumeric(Request.QueryString("id")) Then
                    UserID.Value = Request.QueryString("id")
                    lblAddEdit.Text = "Editing"
                    lblError.Text = ""
                    btnSave.Text = "Edit"
                Else
                    UserID.Value = "0"
                End If
            End If
            Dim GetIT As security = New security
            Dim Udata As security.UserData = New security.UserData
            AllRightsID.Value = ""
            repRights.DataSource = GetIT.GetRightsList(UserID.Value)
            repRights.DataBind()
            Udata = GetIT.GetUserDetails(UserID.Value)
            txtUserName.Text = Udata.UserName
            txtPassword.Attributes.Add("value", Udata.Password)
            txtPassword1.Attributes.Add("value", Udata.Password)
            txtName.Text = Udata.Uname
            txtEmail.Text = Udata.Email
            mytags.Text = Udata.Taglist
            chkIsMainAdmin.Checked = Udata.IsMainAdmin
            chkTags.Checked = Udata.canManageTags

            Udata = Nothing
            GetIT = Nothing
        End If
    End Sub
    Public Function CheckforRights(ByVal Rcou As Int16, ByVal RightsID As Integer) As Boolean
        AllRightsID.Value = AllRightsID.Value & RightsID & ","
        If Rcou > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Dim Udata As Security.UserData = New Security.UserData
            Dim Act As Security = New Security
            Dim RetVal As Double
            Udata.Uname = txtName.Text
            Udata.UserName = txtUserName.Text
            Udata.Password = txtPassword.Text
            Udata.Email = txtEmail.Text
            Udata.Taglist = mytags.Text
            Udata.IsMainAdmin = chkIsMainAdmin.Checked
            Udata.canManageTags = chkTags.Checked
            If UserID.Value.Length > 0 And IsNumeric(UserID.Value) Then

                If UserID.Value > 0 Then
                    RetVal = Act.CheckDuplicate("  WHERE USERNAME='" & Udata.UserName & "' AND USERID<>" & UserID.Value & " ")
                    If RetVal = -2 Then
                    Else
                        Udata.UserID = UserID.Value
                        RetVal = Act.EditUser(Udata)
                        RetVal = UserID.Value
                    End If
                Else
                    RetVal = Act.CheckDuplicate("  WHERE USERNAME='" & Udata.UserName & "' ")
                    If RetVal = -2 Then
                    Else
                        RetVal = Act.AddUser(Udata)
                    End If
                    RetVal = Act.GetMaxid()

                End If
            Else
                RetVal = Act.CheckDuplicate("  WHERE USERNAME='" & Udata.UserName & "' ")
                If RetVal = -2 Then
                Else
                    RetVal = Act.AddUser(Udata)
                End If
                RetVal = Act.GetMaxid()
            End If

            If RetVal = -2 Then
                lblError.Text = "Duplicate Username"
            Else
                If AllRightsID.Value.EndsWith(",") Then
                    AllRightsID.Value = AllRightsID.Value.Remove(AllRightsID.Value.Length - 1, 1)
                End If
                Dim arrRights() As String = AllRightsID.Value.Split(",")
                Dim zz As Integer
                Act.DeleteUserRight(RetVal)
                For zz = 0 To repRights.Items.Count - 1
                    Dim chk As CheckBox = CType(repRights.Items(zz).FindControl("chkRights"), CheckBox)
                    If chk.Checked Then
                        Act.InsertUserRight(RetVal, arrRights(zz))
                    End If
                Next
                Response.Redirect("user.aspx")
            End If
        End If
    End Sub
End Class
