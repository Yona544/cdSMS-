
Partial Class admin_user
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Dim TopControl As admin_include_top = New admin_include_top
        TopControl = Me.FindControl("top1")
        TopControl.MenuNumber = 5.0
        'Put user code to initialize the page here
        If Page.IsPostBack = False Then
            Dim URObj As security = New security
            Dim GetRights As Boolean = URObj.IsAutherised(Session.Contents("userid"), "Config")
            If GetRights = False Then
                Response.Redirect("default.aspx")
            End If
            'Session.Contents("sess_UserOrder") = "  order by USERID desc "
            Session.Contents("sess_UserOrder") = ""
            BindData()
        End If
    End Sub
    Public Sub BindData()
        AllProdId.Value = ""
        Dim ProductsObj As security = New security
        Dim Sortby As String = ""
        If Len(Session.Contents("sess_UserOrder")) > 0 Then
            Sortby = " order by " & Session.Contents("sess_UserOrder")
        Else
            Sortby = " order by  USERID desc "
        End If

        dgUser.PageSize = System.Configuration.ConfigurationManager.AppSettings("AdminPageSize")
        'dgUser.DataSource = ProductsObj.GetUserDataSet(Session.Contents("sess_UserOrder"))
        dgUser.DataSource = ProductsObj.GetUserDataSet(Sortby)
        dgUser.DataBind()
        'Dim i As Integer = 0
        'For i = 0 To dgUser.Items.Count - 1
        '    Dim imgDelete As ImageButton = CType(dgUser.Items(i).FindControl("imgDelete"), ImageButton)
        '    If Not IsNothing(imgDelete) Then
        '        imgDelete.Attributes.Add("onclick", "return conformvalidation();")
        '        imgDelete.AlternateText = Session.Contents("word_Delete")
        '    End If
        'Next
        If dgUser.Items.Count <= 0 Then
            btnUpdate.Visible = False
        Else
            btnUpdate.Visible = True

        End If
    End Sub
    Protected Function CheckForDelete(ByVal ProdId As Integer) As Boolean
        AllProdId.Value = AllProdId.Value & ProdId & ","
        Return True
    End Function

    Protected Sub dgUser_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgUser.DeleteCommand
        Dim UserMas As Security = New Security
        UserMas.DeleteUser(dgUser.DataKeys(e.Item.ItemIndex))
        BindData()
    End Sub
    Protected Sub dgUser_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgUser.PageIndexChanged
        dgUser.CurrentPageIndex = e.NewPageIndex
        BindData()
    End Sub
    Protected Sub dgUser_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUser.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            Dim comfunc As CommonFunctions = New CommonFunctions
            comfunc.BindPagingInGrid(dgUser, e, False, False, False, True, "", "", "First", "Last", "", "", "<< Prev", "Next >>", "PageStyle")
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        System.Threading.Thread.Sleep(System.Configuration.ConfigurationManager.AppSettings("AdminSleep"))

        Dim actstr As String = ""
        Dim i As Integer
        For i = 0 To dgUser.Items.Count - 1

            Dim actchk As CheckBox = CType(dgUser.Items(i).FindControl("chkactive"), CheckBox)
            
            If actchk.Checked Then
                actstr = actstr & dgUser.DataKeys(i) & ","
            End If
        Next
       
        If actstr.EndsWith(",") Then
            actstr = actstr.Remove(actstr.Length - 1, 1)
        Else
            actstr = "0"
        End If
        Dim Act As Security = New Security
        If AllProdId.Value.Length > 0 Then
            AllProdId.Value = AllProdId.Value.Remove(AllProdId.Value.Length - 1, 1)
        End If

        Act.SetActiveDeactiveUser(actstr, AllProdId.Value)

        BindData()
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        System.Threading.Thread.Sleep(System.Configuration.ConfigurationManager.AppSettings("AdminSleep"))
        Dim delstr As String = ""

        Dim i As Integer
        For i = 0 To dgUser.Items.Count - 1
            Dim delchk As CheckBox = CType(dgUser.Items(i).FindControl("chkdelete"), CheckBox)

            If delchk.Checked Then
                delstr = delstr & dgUser.DataKeys(i) & ","
            End If

        Next
        If delstr.EndsWith(",") Then
            delstr = delstr.Remove(delstr.Length - 1, 1)
        Else
            delstr = "0"
        End If

        Dim Act As Security = New Security
        Act.DeleteUser(delstr)
        BindData()
    End Sub

    Protected Sub dgUser_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgUser.SortCommand
        If Trim(Session.Contents("sess_UserOrder")) = e.SortExpression Then
            Session.Contents("sess_UserOrder") = e.SortExpression & " desc "
        Else
            Session.Contents("sess_UserOrder") = e.SortExpression
        End If

        'Session.Contents("sess_UserOrder") = " order by " & e.SortExpression
        BindData()
    End Sub
End Class
