Imports System.Data

Partial Class admin_managevoice
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TopControl As admin_include_top = New admin_include_top
        TopControl = Me.FindControl("top1")
        TopControl.MenuNumber = 2.1
        If Page.IsPostBack = False Then
            'txtMinimumAmount.Attributes.Add("onkeypress", "return isNumberKey(event)")
            'txtAmount.Attributes.Add("onkeypress", "return isNumberKey(event)")
            'txtTotalAllowed.Attributes.Add("onkeypress", "return isNumberKey(event)")

            'rdCustomVoice.Checked = False
            'rdSystemVoice.Checked = False
            'pnlVoiceCreate.Visible = False
            'pnlTropovoice.Visible = False

            Dim URObj As security = New security
            Dim GetRights As Boolean = URObj.IsAutherised(Session.Contents("userid"), "Voice")
            If GetRights = False Then
                Response.Redirect("default.aspx")
            End If

            Dim DisId As Integer = 0
            Try
                DisId = Request.QueryString("id")
            Catch ex As Exception
                DisId = 0
            End Try
            If Not IsNumeric(DisId) Or DisId = Nothing Then
                DisId = 0
            End If
            hlDiscountId.Value = DisId
            'hlShowMainImage.Visible = False
            'lnkRemove.Visible = False
            lblAddEdit.Text = "Add New Voice"
            If DisId > 0 Then
                Dim DisData As VoiceClass.VoiceData = New VoiceClass.VoiceData
                Dim GetData As VoiceClass = New VoiceClass
                DisData = GetData.GetDiscountDetails(hlDiscountId.Value)
                lblAddEdit.Text = "Edit : " & DisData.Name
                txtname.Text = DisData.Name
                'txtVoiceAge.Text = DisData.VoiceAge
                txtVoiceText.Text = DisData.VoiceText
                'If DisData.VoiceGender = "male" Then
                '    rdmale.Checked = True
                'ElseIf DisData.VoiceGender = "female" Then
                '    rdfemail.Checked = True
                'Else
                '    rdneutral.Checked = True
                'End If

                If DisData.VoiceRate = "x-slow" Then
                    rdVoiceRatexslow.Checked = True
                ElseIf DisData.VoiceRate = "slow" Then
                    rdslow.Checked = True
                ElseIf DisData.VoiceRate = "medium" Then
                    rdmedium.Checked = True
                ElseIf DisData.VoiceRate = "fast" Then
                    rdfast.Checked = True
                ElseIf DisData.VoiceRate = "x-fast" Then
                    rdxfast.Checked = True
                ElseIf DisData.VoiceRate = "default" Then
                    rddefault.Checked = True
                End If

                chkActiveMain.Checked = DisData.IsActive
                If DisData.VoiceType = "CALL" Then
                    rdCall.Checked = True
                    rdSMS.Checked = False
                    ' pnlCallOnly.Visible = True

                Else
                    rdSMS.Checked = True
                    rdCall.Checked = False
                    '     pnlCallOnly.Visible = False
                End If

                If Len(DisData.TropoVoice) > 0 Then
                    '     rdSystemVoice.Checked = True
                    '     pnlVoiceCreate.Visible = False
                    '     pnlTropovoice.Visible = True
                    ddlVoice.SelectedValue = DisData.TropoVoice
                Else
                    '   rdCustomVoice.Checked = True
                    '     pnlVoiceCreate.Visible = True
                    '     pnlTropovoice.Visible = False
                    ddlVoice.SelectedValue = "Other"
                End If

            End If
        End If


    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            'If rdCall.Checked = True Then
            '    If rdCustomVoice.Checked = False And rdSystemVoice.Checked = False Then
            '        ScriptManager.RegisterStartupScript(Page, GetType(Page), "successs", "alert('Select System Voice Or Custom Voice First ');", True)
            '        Exit Sub
            '    End If
            'End If

            'If Len(txtVoiceAge.Text) > 0 And Not IsNumeric(txtVoiceAge.Text) Then
            '    ScriptManager.RegisterStartupScript(Page, GetType(Page), "successs", "alert('Enter Only Numeric Voice Age');", True)
            '    Exit Sub
            'End If


            Dim DisData As VoiceClass.VoiceData = New VoiceClass.VoiceData
            Dim GetData As VoiceClass = New VoiceClass

            DisData.ID = hlDiscountId.Value
            DisData.Name = txtname.Text
            'If Len(txtVoiceAge.Text) > 0 And IsNumeric(txtVoiceAge.Text) Then
            '    DisData.VoiceAge = txtVoiceAge.Text
            'Else
            '    DisData.VoiceAge = 0
            'End If

            DisData.VoiceText = txtVoiceText.Text
            'If rdmale.Checked Then
            '    DisData.VoiceGender = "male"
            'ElseIf rdfemail.Checked Then
            '    DisData.VoiceGender = "female"
            'Else
            '    DisData.VoiceGender = "neutral"
            'End If

            If rdVoiceRatexslow.Checked Then
                DisData.VoiceRate = "x-slow"
            ElseIf rdslow.Checked Then
                DisData.VoiceRate = "slow"
            ElseIf rdmedium.Checked Then
                DisData.VoiceRate = "medium"
            ElseIf rdfast.Checked Then
                DisData.VoiceRate = "fast"
            ElseIf rdxfast.Checked Then
                DisData.VoiceRate = "x-fast"
            ElseIf rddefault.Checked Then
                DisData.VoiceRate = "default"
            End If
            'If rdCustomVoice.Checked = True Then
            'DisData.TropoVoice = ""
            'Else
            DisData.TropoVoice = ddlVoice.SelectedValue
            'End If
            DisData.IsActive = chkActiveMain.Checked
            If rdCall.Checked = True Then
                DisData.VoiceType = "CALL"
            Else
                DisData.VoiceType = "SMS"
            End If
            Dim RetVal As Double
            Dim Mode As String
            If hlDiscountId.Value.Length > 0 And IsNumeric(hlDiscountId.Value) Then
                If hlDiscountId.Value > 0 Then
                    RetVal = GetData.CheckDuplicate("  WHERE [Name]='" & DisData.Name & "' AND id<>" & hlDiscountId.Value & " ")
                    If RetVal = -2 Then
                    Else
                        DisData.ID = hlDiscountId.Value
                        RetVal = GetData.EditVoice(DisData)

                        RetVal = hlDiscountId.Value
                        Mode = "Edit"
                    End If
                Else
                    RetVal = GetData.CheckDuplicate("  WHERE [Name]='" & DisData.Name & "'  ")
                    If RetVal = -2 Then
                    Else
                        RetVal = GetData.AddVoice(DisData)
                        Mode = "Add"
                        RetVal = GetData.GetMaxid()
                    End If

                End If
            Else
                RetVal = GetData.CheckDuplicate("  WHERE [Name]='" & DisData.Name & "'  ")
                If RetVal = -2 Then
                Else
                    RetVal = GetData.AddVoice(DisData)
                    Mode = "Add"
                    RetVal = GetData.GetMaxid()
                End If
            End If
            If RetVal = -2 Then
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "successs", "alert('Duplicate Name Found');", True)
                Exit Sub
            Else
                Response.Redirect("managevoicemain.aspx")
            End If
        End If
    End Sub
    Private Sub ClearFormData()
        txtname.Text = ""
        'txtVoiceAge.Text = ""
        txtVoiceText.Text = ""
        rddefault.Checked = True
        'rdmale.Checked = True
        chkActiveMain.Checked = True

        hlDiscountId.Value = ""
        lblAddEdit.Text = "Add"

    End Sub

    Protected Sub ddlVoice_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVoice.Load
        'If ddlVoice.SelectedValue = "Other" Then
        '    pnlVoiceCreate.Visible = True
        'Else
        '    pnlVoiceCreate.Visible = False
        'End If
    End Sub

    'Protected Sub rdCustomVoice_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdCustomVoice.CheckedChanged
    '    If rdCustomVoice.Checked = True Then
    '        pnlVoiceCreate.Visible = True
    '        pnlTropovoice.Visible = False
    '    Else
    '        pnlVoiceCreate.Visible = False
    '        pnlTropovoice.Visible = True
    '    End If
    'End Sub

    'Protected Sub rdSystemVoice_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdSystemVoice.CheckedChanged
    '    If rdSystemVoice.Checked = True Then
    '        pnlVoiceCreate.Visible = False
    '        pnlTropovoice.Visible = True
    '    Else
    '        pnlVoiceCreate.Visible = True
    '        pnlTropovoice.Visible = False
    '    End If
    'End Sub


    'Protected Sub rdCall_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdCall.CheckedChanged
    '    If rdCall.Checked = True Then
    '        pnlCallOnly.Visible = True
    '    Else
    '        pnlCallOnly.Visible = False
    '    End If

    'End Sub

    'Protected Sub rdSMS_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdSMS.CheckedChanged
    '    If rdSMS.Checked = True Then
    '        pnlCallOnly.Visible = False
    '    Else
    '        pnlCallOnly.Visible = True
    '    End If

    'End Sub
End Class
