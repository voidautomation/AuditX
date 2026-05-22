Imports System.IO
Imports System.Globalization

Public Class ctrlGeneral
    Inherits UserControl

#Region "Private Fields"

    Private ReadOnly _originalValues As New Dictionary(Of String, String)
    Private ReadOnly _helpProvider As New ErrorProvider()

    Private ReadOnly bubbleTip As New ToolTip With {
        .IsBalloon = True,
        .ToolTipIcon = ToolTipIcon.Info,
        .ToolTipTitle = "Information"
    }

#End Region

#Region "Load Event"

    Private Sub ctrlGeneral_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ConfigureHelpProvider()
            ConfigureFocusBubbles()
            LoadAllSettings()
        Catch ex As Exception
            ShowError("An error occurred while loading settings.", ex)
        End Try
    End Sub

#End Region

#Region "Settings Mapping"

    Private Function GetSettingsMap() As Dictionary(Of Control, String)
        Return New Dictionary(Of Control, String) From {
            {txtStartTime, NameOf(My.Settings.Report_FTIME)},
            {txtEndTime, NameOf(My.Settings.Report_TTIME)},
            {lblSAPGUIXML, NameOf(My.Settings.SAPGUIXML)},
            {lblITGCControl, NameOf(My.Settings.ITGCControl)},
            {lblSAPGUIApp, NameOf(My.Settings.SAPLogonPath)},
            {lblDownloadDestination, NameOf(My.Settings.DownloadDestination)},
            {txtITGC01_OssId, NameOf(My.Settings.ITGC01_OSSID)},
            {txtITGC04_TcodeList, NameOf(My.Settings.ITGC04_TCodeList)},
            {txtITGC08_ApprovedUsers, NameOf(My.Settings.ITGC08_ApprovedUsers)},
            {txtITGC09_ApprovedUsers, NameOf(My.Settings.ITGC09_ApprovedUsers)},
            {txtITGC10_ApprovedUserCheck1, NameOf(My.Settings.ITGC10_ApprovedUserscheck1)},
            {txtITGC10_ApprovedUserCheck2, NameOf(My.Settings.ITGC10_ApprovedUserscheck2)},
            {txtITGC10_ApprovedUserCheck3, NameOf(My.Settings.ITGC10_ApprovedUserscheck3)},
            {txtITGC10_ApprovedUserCheck4, NameOf(My.Settings.ITGC10_ApprovedUserscheck4)},
            {txtITGC10_ApprovedUserCheck5, NameOf(My.Settings.ITGC10_ApprovedUserscheck5)},
            {txtITGC11_ApprovedUsers, NameOf(My.Settings.ITGC11_ApprovedUsers)},
            {txtITGC12_ApprovedUsers, NameOf(My.Settings.ITGC12_ApprovedUsers)},
            {txtITGC13_ApprovedUsers, NameOf(My.Settings.ITGC13_ApprovedUsers)},
            {txtITGC15_ApprovedUsers, NameOf(My.Settings.ITGC15_ApprovedUsers)},
            {txtITGC16_ProgramName, NameOf(My.Settings.ITGC16_ProgramName)},
            {txtITGC17_ApprovedUsers, NameOf(My.Settings.ITGC17_ApprovedUsers)}
        }
    End Function

#End Region

#Region "Load & Save"

    Private Sub LoadAllSettings()
        Dim map = GetSettingsMap()

        For Each pair In map
            Dim ctrl = pair.Key
            Dim settingName = pair.Value
            Dim value = My.Settings(settingName)?.ToString()

            SetControlText(ctrl, value)
            _originalValues(settingName) = value
        Next

        ApplyHoverEffect(btnGeneralSave, Color.SeaGreen)
        ApplyHoverEffect(btnRevertDefaults, Color.IndianRed)
    End Sub

    Private Sub SaveAllSettings()
        Dim map = GetSettingsMap()

        For Each pair In map
            Dim ctrl = pair.Key
            Dim settingName = pair.Value

            Dim value = GetControlText(ctrl)

            My.Settings(settingName) = value
            _originalValues(settingName) = value
        Next

        My.Settings.Save()
    End Sub

#End Region

#Region "Change Detection"

    Private Function HasChanges() As Boolean
        Dim map = GetSettingsMap()

        For Each pair In map
            Dim settingName = pair.Value
            Dim currentValue = GetControlText(pair.Key)
            Dim originalValue = If(_originalValues.ContainsKey(settingName),
                                   _originalValues(settingName), "")

            If currentValue <> originalValue Then
                Return True
            End If
        Next

        Return False
    End Function

#End Region

#Region "Validation"

    Private Function ValidateCurrentTab() As Boolean

        If Not IsValidTimeFormat(txtStartTime.Text) Then
            Return ShowValidation("Start Time must be in HH:mm:ss format.", txtStartTime)
        End If

        If Not IsValidTimeFormat(txtEndTime.Text) Then
            Return ShowValidation("End Time must be in HH:mm:ss format.", txtEndTime)
        End If

        Dim requiredControls = New Dictionary(Of Control, String) From {
            {txtITGC01_OssId, "SAP OSS ID"},
            {txtITGC04_TcodeList, "SAP TCODE List"},
            {txtITGC08_ApprovedUsers, "ITGC08 Approved Users"},
            {txtITGC09_ApprovedUsers, "ITGC09 Approved Users"},
            {txtITGC10_ApprovedUserCheck1, "ITGC10 Check 1 Users"},
            {txtITGC10_ApprovedUserCheck2, "ITGC10 Check 2 Users"},
            {txtITGC10_ApprovedUserCheck3, "ITGC10 Check 3 Users"},
            {txtITGC10_ApprovedUserCheck4, "ITGC10 Check 4 Users"},
            {txtITGC10_ApprovedUserCheck5, "ITGC10 Check 5 Users"},
            {txtITGC11_ApprovedUsers, "ITGC11 Approved Users"},
            {txtITGC12_ApprovedUsers, "ITGC12 Approved Users"},
            {txtITGC13_ApprovedUsers, "ITGC13 Approved Users"},
            {txtITGC15_ApprovedUsers, "ITGC15 Approved Users"},
            {txtITGC16_ProgramName, "ITGC16 Program Name"},
            {txtITGC17_ApprovedUsers, "ITGC17 Approved Users"}
        }

        For Each item In requiredControls
            If String.IsNullOrWhiteSpace(GetControlText(item.Key)) Then
                Return ShowValidation($"{item.Value} is required.", item.Key)
            End If
        Next

        Return True
    End Function

    Private Function IsValidTimeFormat(input As String) As Boolean
        Dim parsedTime As DateTime
        Return DateTime.TryParseExact(input.Trim(),
                                      "HH:mm:ss",
                                      CultureInfo.InvariantCulture,
                                      DateTimeStyles.None,
                                      parsedTime)
    End Function

#End Region

#Region "Save Button"

    Private Sub btnGeneralSave_Click(sender As Object, e As EventArgs) Handles btnGeneralSave.Click
        Try
            If Not ValidateCurrentTab() Then Exit Sub

            If Not HasChanges() Then
                MessageBox.Show("No changes detected.",
                                "Information",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                Exit Sub
            End If

            If MessageBox.Show("Do you want to save the changes?",
                               "Confirm Save",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question) = DialogResult.Yes Then

                SaveAllSettings()

                MessageBox.Show("Settings saved successfully.",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            ShowError("Failed to save settings.", ex)
        End Try
    End Sub

#End Region

#Region "Revert Defaults"

    Private Sub btnRevertDefaults_Click(sender As Object, e As EventArgs) Handles btnRevertDefaults.Click
        Try
            If MessageBox.Show("Revert all settings to default values?",
                               "Confirm",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question) = DialogResult.Yes Then

                My.Settings.Reset()
                My.Settings.Save()
                LoadAllSettings()

                MessageBox.Show("Settings reverted to default values.",
                                "Reverted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            ShowError("Failed to revert settings.", ex)
        End Try
    End Sub

#End Region

#Region "File / Folder Selection"

    Private Sub lblSAPGUIApp_Click(sender As Object, e As EventArgs) Handles lblSAPGUIApp.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*"
            If ofd.ShowDialog() = DialogResult.OK Then
                lblSAPGUIApp.Text = ofd.FileName
            End If
        End Using
    End Sub

    Private Sub lblDownloadDestination_Click(sender As Object, e As EventArgs) Handles lblDownloadDestination.Click
        Using fbd As New FolderBrowserDialog()
            If fbd.ShowDialog() = DialogResult.OK Then
                lblDownloadDestination.Text = fbd.SelectedPath
            End If
        End Using
    End Sub

    Private Sub lblSAPGUIXML_Click(sender As Object, e As EventArgs) Handles lblSAPGUIXML.Click
        SelectAndCopyFile(lblSAPGUIXML)
    End Sub

    Private Sub lblITGCControl_Click(sender As Object, e As EventArgs) Handles lblITGCControl.Click
        SelectAndCopyFile(lblITGCControl)
    End Sub

    Private Sub SelectAndCopyFile(targetLabel As Label)
        Using ofd As New OpenFileDialog()
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim relativePath = CopyFileToAppDirectory(ofd.FileName)
                If Not String.IsNullOrEmpty(relativePath) Then
                    targetLabel.Text = relativePath
                End If
            End If
        End Using
    End Sub

    Private Function CopyFileToAppDirectory(sourcePath As String) As String
        Try
            Dim fileName = Path.GetFileName(sourcePath)
            Dim destination = Path.Combine(Application.StartupPath, fileName)

            File.Copy(sourcePath, destination, True)

            Return ".\" & fileName
        Catch ex As Exception
            ShowError("File copy failed.", ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Helpers"

    Private Sub ConfigureHelpProvider()
        _helpProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink
        _helpProvider.Icon = SystemIcons.Information
    End Sub

    Private Sub ConfigureFocusBubbles()
        SetFocusBubble(txtStartTime, "Format: HH:mm:ss")
        SetFocusBubble(txtEndTime, "Format: HH:mm:ss")
    End Sub

    Private Sub SetFocusBubble(txt As TextBox, message As String)
        AddHandler txt.Enter, Sub()
                                  bubbleTip.Show(message, txt, 0, -40, 4000)
                              End Sub

        AddHandler txt.Leave, Sub()
                                  bubbleTip.Hide(txt)
                              End Sub
    End Sub

    Private Sub ApplyHoverEffect(btn As Button, hoverColor As Color)
        Dim defaultColor = btn.BackColor

        AddHandler btn.MouseEnter, Sub() btn.BackColor = hoverColor
        AddHandler btn.MouseLeave, Sub() btn.BackColor = defaultColor
    End Sub

    Private Function ShowValidation(message As String, ctrl As Control) As Boolean
        MessageBox.Show(message,
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)
        ctrl.Focus()
        Return False
    End Function

    Private Sub ShowError(message As String, ex As Exception)
        MessageBox.Show($"{message}{Environment.NewLine}{ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
    End Sub

    Private Sub SetControlText(ctrl As Control, value As String)
        If TypeOf ctrl Is TextBox Then
            DirectCast(ctrl, TextBox).Text = value
        ElseIf TypeOf ctrl Is Label Then
            DirectCast(ctrl, Label).Text = value
        End If
    End Sub

    Private Function GetControlText(ctrl As Control) As String
        If TypeOf ctrl Is TextBox Then
            Return DirectCast(ctrl, TextBox).Text.Trim()
        ElseIf TypeOf ctrl Is Label Then
            Return DirectCast(ctrl, Label).Text.Trim()
        End If
        Return String.Empty
    End Function

#End Region

End Class