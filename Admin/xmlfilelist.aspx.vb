Imports System.IO

Partial Class Admin_xmlfilelist
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TopControl As admin_include_top = New admin_include_top
        TopControl = Me.FindControl("top")
        TopControl.MenuNumber = 8.1
        If Not Page.IsPostBack Then

            Dim URObj As security = New security
            Dim GetRights As Boolean = URObj.IsAutherised(Session.Contents("userid"), "Files")
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
            Qry = " WHERE filetype='XML' "
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
        dgDiscount.DataSource = CatObj.GetVoicefileDataSet(Qry & Sortby)
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



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim Qry As String = ""

        If Len(Trim(txtCategoryName.Text)) > 0 Then
            Qry = Qry & " filename like '%" & Trim(txtCategoryName.Text) & "%' AND "
        End If




        If Len(Trim(Qry)) > 0 Then
            Qry = " WHERE filetype='XML' and " & Qry.Remove(Len(Qry) - 4, 4)
        Else
            Qry = " WHERE filetype='XML' "
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

                Try
                    Dim Photo As String = CatObj.getfilename(" where id=" & dgDiscount.DataKeys(i))
                    If File.Exists(Server.MapPath("..\files\XML\") & Photo) Then
                        File.Delete(Server.MapPath("..\files\XML\") & Photo)
                    End If
                Catch ex As Exception

                End Try

            End If
        Next
        If delstr.EndsWith(",") = True Then
            delstr = delstr.Remove(delstr.Length - 1, 1)
        Else
            delstr = "0"
        End If
        CatObj.Deletefile(delstr)
        BindData()
    End Sub
    Protected Sub dgDiscount_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDiscount.DeleteCommand
        If e.CommandName = "Delete" Then
            Dim Id As Integer = e.CommandArgument
            Dim VoiceOBJ As VoiceClass = New VoiceClass
            Try
                Dim Photo As String = VoiceOBJ.getfilename(" where id=" & Id)
                If File.Exists(Server.MapPath("..\files\XML\") & Photo) Then
                    File.Delete(Server.MapPath("..\files\XML\") & Photo)
                End If
            Catch ex As Exception
            End Try
            VoiceOBJ.Deletefile(Id)
            BindData()
        
        End If
    End Sub
    Protected Sub dgDiscount_EditCommand(source As Object, e As DataGridCommandEventArgs) Handles dgDiscount.EditCommand
        If e.CommandName = "Edit" Then
            Dim Id As Integer = e.CommandArgument
            Dim XMLObj As VoiceClass = New VoiceClass
            Dim ComFun As CommonFunctions = New CommonFunctions
            Dim FileName As String = XMLObj.getfilename(" where id=" & Id)

            Dim path As String = Server.MapPath("..\files\XML\") & FileName
            ComFun.runXmlFile(path, FileName)
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "alert", "alert('" & FileName & " executed successfully.');", True)
        End If
    End Sub
End Class