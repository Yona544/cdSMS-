
Partial Class admin_managevoicemain
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TopControl As admin_include_top = New admin_include_top
        TopControl = Me.FindControl("top")
        TopControl.MenuNumber = 2.1
        If Not Page.IsPostBack Then

            Dim URObj As security = New security
            Dim GetRights As Boolean = URObj.IsAutherised(Session.Contents("userid"), "Voice")
            If GetRights = False Then
                Response.Redirect("default.aspx")
            End If

            Session.Contents("sess_adminCatsrc") = ""
            Session.Contents("sess_catOrder") = ""
            'Session.Contents("sess_catOrder") = " ORDER BY CategoryId desc"
            BindData()
        End If
    End Sub
    Public Function CheckForDelete(ByVal CatId As Integer) As Boolean
        AllCatIds.Value = AllCatIds.Value & CatId & ","
        Return True
    End Function
    Public Sub BindData()
        'Dim Qry As String = " ORDER BY CategoryId desc"
        Dim Qry As String = ""
        Try
            Qry = Session.Contents("sess_adminCatsrc")
        Catch ex As Exception
            Qry = ""
        End Try

        If Len(Trim(Qry)) <= 0 Then
            Qry = ""
        End If
        AllCatIds.Value = ""
        Dim CatObj As VoiceClass = New VoiceClass
        Dim Sortby As String = ""
        If Len(Session.Contents("sess_catOrder")) > 0 Then
            Sortby = " order by " & Session.Contents("sess_catOrder")
        Else
            Sortby = " order by  id desc "
        End If
        dgDiscount.PageSize = System.Configuration.ConfigurationManager.AppSettings("AdminPageSize")
        dgDiscount.DataSource = CatObj.GetVoiceDataSet(Qry & Sortby)
        dgDiscount.DataBind()
        If dgDiscount.Items.Count <= 0 Then
            lblnorecord.Visible = True
            lblnorecord.Text = "No Record Found"
            ButtonsTR.Visible = False
            dgDiscount.Visible = False
        Else
            dgDiscount.Visible = True
            lblnorecord.Visible = False
            ButtonsTR.Visible = True
        End If
    End Sub

    Protected Sub dgDiscount_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDiscount.ItemCommand
        If e.CommandName = "Copy" Then
            Dim id As Integer = e.CommandArgument
            Dim voiceObj As VoiceClass = New VoiceClass
            voiceObj.saveAsVoice(id)
            Dim RetVal As Integer = 0
            RetVal = voiceObj.GetMaxid()
            Response.Redirect("managevoice.aspx?id=" & RetVal)
        End If
    End Sub
    Protected Sub dgDiscount_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDiscount.PageIndexChanged
        dgDiscount.CurrentPageIndex = e.NewPageIndex
        BindData()

    End Sub
    Protected Sub dgDiscount_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDiscount.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            Dim comfunc As CommonFunctions = New CommonFunctions
            comfunc.BindPagingInGrid(dgDiscount, e, False, False, False, True, "", "", "First", "Last", "", "", "<< Prev ", " Next >>", "PageStyle")
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim CatObj As VoiceClass = New VoiceClass
        Dim i As Integer = 0
        Dim actstr As String = ""
        For i = 0 To dgDiscount.Items.Count - 1
            Dim chkactive As CheckBox = CType(dgDiscount.Items(i).FindControl("chkactive"), CheckBox)
            If chkactive.Checked Then
                actstr = actstr + LTrim(RTrim(Str(dgDiscount.DataKeys(i)))) + ","
            End If
        Next
        If actstr.EndsWith(",") = True Then
            actstr = actstr.Remove(actstr.Length - 1, 1)
        Else
            actstr = "0"
        End If

        If AllCatIds.Value.EndsWith(",") Then
            AllCatIds.Value = AllCatIds.Value.Remove(AllCatIds.Value.Length - 1, 1)
            CatObj.SetActiveDiscount(actstr, AllCatIds.Value)
        End If
        BindData()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim Qry As String = ""

        If Len(Trim(txtCategoryName.Text)) > 0 Then
            Qry = Qry & " name like '%" & Trim(txtCategoryName.Text) & "%' AND "
        End If

        If ddlAvtive.SelectedValue = "0" Then
            Qry = Qry & " IsActive =0 AND "
        ElseIf ddlAvtive.SelectedValue = "1" Then
            Qry = Qry & " IsActive =1 AND "
        End If


        If Len(Trim(Qry)) > 0 Then
            Qry = " WHERE " & Qry.Remove(Len(Qry) - 4, 4)
        End If



        Session.Contents("sess_adminCatsrc") = Qry
        dgDiscount.CurrentPageIndex = 0
        BindData()
    End Sub

    Protected Sub dgDiscount_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgDiscount.SortCommand
        If Trim(Session.Contents("sess_catOrder")) = e.SortExpression Then
            Session.Contents("sess_catOrder") = e.SortExpression & " desc "
        Else
            Session.Contents("sess_catOrder") = e.SortExpression
        End If
        BindData()
    End Sub
    Public Function GetDate(ByVal dt As String) As Object
        Try
            Dim dt1 As Date = Convert.ToDateTime(dt)
            'Return dt1.Day & "/" & dt1.Month & "/" & dt1.Year
            Return dt1.ToString("MM/dd/yyyy")
        Catch ex As Exception
        End Try
    End Function

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim CatObj As VoiceClass = New VoiceClass
        Dim i As Integer = 0
        Dim delstr As String = ""
        For i = 0 To dgDiscount.Items.Count - 1
            Dim actdel As CheckBox = CType(dgDiscount.Items(i).FindControl("chkdelete"), CheckBox)
            If actdel.Checked Then
                delstr = delstr + Str(dgDiscount.DataKeys(i)) + ","
            End If
        Next
        If delstr.EndsWith(",") = True Then
            delstr = delstr.Remove(delstr.Length - 1, 1)
        Else
            delstr = "0"
        End If
        CatObj.DeleteDiscount(delstr)
        BindData()
    End Sub
End Class