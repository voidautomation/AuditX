Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Word

Module ITGC14
    ' Subroutine to check Password Status for the below default SAP ID's

    ' Standard SAP users that need to be validated
    Private ReadOnly STANDARD_USERS As String() = {"DDIC", "SAP*", "SAPCPIC", "EARLYWATCH", "TMSADM"}

    ' Class to hold validation data per row
    Private Class UserValidation
        Public Property ClientId As String
        Public Property UserId As String
        Public Property PasswordStatus As String
        Public Property LockReason As String
        Public Property IsCompliant As Boolean
        Public Property PasswordOK As Boolean
        Public Property LockOK As Boolean
    End Class

    Sub ExecuteITGC14()
        Dim WordApp As Word.Application = Nothing
        Dim WordDoc As Word.Document = Nothing

        Try
            ' Minimize all windows except SAP
            Dim executionMode As String = Home.cmbExecutionMode.SelectedItem?.ToString()
            If executionMode = "Foreground" Then
                MinimizeAllWindowsExceptSAP()
            End If

            WordApp = New Word.Application()
            WordApp.Visible = True
            WordDoc = WordApp.Documents.Add()

            Dim systemname As String = Home.lblSystemId.Text
            Dim controlName As String = Home.lblDescription.Text
            Dim controlID As String = Home.cmbControl.SelectedItem?.ToString().Split("-"c)(0).Trim()

            With WordDoc.Paragraphs.Last.Range
                .Text = $"{controlID} - {controlName}{vbCrLf}System: {systemname}"
                .Font.Size = 16
                .Font.Bold = True
                .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                .InsertParagraphAfter()
            End With
            With WordDoc.Paragraphs.Last.Range
                .Font.Size = 11
                .Font.Bold = False
                .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft
            End With

            Dim SapGuiAuto = GetObject("SAPGUI")
            If SapGuiAuto Is Nothing Then
                MessageBox.Show("SAP GUI is not running.")
                Logger.LogMessage("SAP GUI is not running.", False)
                Exit Sub
            End If

            Dim selectSystem As String = Home.cmbSystem.SelectedItem?.ToString()
            Dim controlITGC As String = Home.cmbControl.SelectedItem?.ToString().Split("-"c)(0).Trim()
            Dim folderPath As String = Home.txtFolderPath.Text
            Dim ReportMonth As String = Home.txtReportMonth.Text
            Dim selectedStartDate As DateTime = Home.dtpStart.Value
            Dim selectedEndDate As DateTime = Home.dtpEnd.Value
            Dim stepNum As Integer = 1
            Dim waitTime As TimeSpan = TimeSpan.FromSeconds(1)
            Dim fileName As String = $"{controlITGC} {systemname} Audit Report.docx"
            Dim userCount As Integer = 0

            ' Validation tracking - all rows
            Dim allValidations As New List(Of UserValidation)
            Dim lastClient As String = ""

            '---------------------------------------------------------------------------------------------------------
            Logger.LogMessage($"{controlID} Execution Started!", True)

            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").text = "SA38"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/ctxtRS38M-PROGRAMM").text = "RSUSR003"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/tbar[1]/btn[8]").press
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)
            stepNum += 1
            session.findById("wnd[0]/tbar[1]/btn[8]").press
            Threading.Thread.Sleep(waitTime)

            Dim LogMessage1 As String = ""
            Try
                LogMessage1 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
            End Try

            ' ============================================================
            ' VALIDATION: Read grid data
            ' ============================================================
            Try
                Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell")

                Try
                    grid.SetRowSize(1, 251)
                    grid.SetRowSize(2, -1)
                Catch
                End Try

                Dim totalRows As Long = grid.RowCount
                Dim visibleRows As Long = grid.VisibleRowCount
                userCount = totalRows

                Logger.LogMessage($"Total rows in grid: {totalRows}", True)

                ' Discover column technical names
                Dim clientCol As String = ""
                Dim userCol As String = ""
                Dim pwdStatusCol As String = ""
                Dim lockReasonCol As String = ""

                Try
                    Dim columnOrder = grid.ColumnOrder
                    Dim colCount As Integer = columnOrder.Count

                    For colIdx As Integer = 0 To colCount - 1
                        Dim colName As String = ""
                        Dim colTitle As String = ""

                        Try
                            colName = columnOrder.Item(colIdx).ToString()
                        Catch
                            Continue For
                        End Try

                        Try
                            Dim titles = grid.GetColumnTitles(colName)
                            If titles.Count > 0 Then
                                colTitle = titles.Item(0).ToString().ToUpper().Trim()
                            End If
                        Catch
                            colTitle = ""
                        End Try

                        Logger.LogMessage($"  Column {colIdx}: Name='{colName}' Title='{colTitle}'", True)

                        If colTitle.Contains("CLIE") AndAlso clientCol = "" Then
                            clientCol = colName
                        ElseIf colTitle = "USER" AndAlso userCol = "" Then
                            userCol = colName
                        ElseIf colTitle.Contains("PASSWORD") AndAlso pwdStatusCol = "" Then
                            pwdStatusCol = colName
                        ElseIf colTitle.Contains("REASON") AndAlso lockReasonCol = "" Then
                            lockReasonCol = colName
                        End If
                    Next

                    Logger.LogMessage($"Detected: Client={clientCol}, User={userCol}, PwdStatus={pwdStatusCol}, LockReason={lockReasonCol}", True)

                Catch colEx As Exception
                    Logger.LogMessage("Error reading column names: " & colEx.Message, False)
                End Try

                ' Loop through all rows
                For rowIdx As Integer = 0 To totalRows - 1
                    Dim clientId As String = ""
                    Dim userId As String = ""
                    Dim pwdStatus As String = ""
                    Dim lockReason As String = ""

                    If clientCol <> "" Then
                        Try
                            clientId = grid.GetCellValue(rowIdx, clientCol).ToString().Trim()
                        Catch
                            clientId = ""
                        End Try
                    End If

                    If clientId = "" Then
                        clientId = lastClient
                    Else
                        lastClient = clientId
                    End If

                    If userCol <> "" Then
                        Try
                            userId = grid.GetCellValue(rowIdx, userCol).ToString().Trim()
                        Catch
                        End Try
                    End If

                    If pwdStatusCol <> "" Then
                        Try
                            pwdStatus = grid.GetCellValue(rowIdx, pwdStatusCol).ToString().Trim()
                        Catch
                        End Try
                    End If

                    If lockReasonCol <> "" Then
                        Try
                            lockReason = grid.GetCellValue(rowIdx, lockReasonCol).ToString().Trim()
                        Catch
                        End Try
                    End If

                    If String.IsNullOrWhiteSpace(userId) Then Continue For

                    ' Skip non-standard users
                    Dim isStandardUser As Boolean = False
                    For Each su In STANDARD_USERS
                        If userId.Trim().ToUpper() = su.ToUpper() Then
                            isStandardUser = True
                            Exit For
                        End If
                    Next

                    If Not isStandardUser Then Continue For

                    ' Determine status
                    Dim validation As New UserValidation()
                    validation.ClientId = clientId
                    validation.UserId = userId
                    validation.PasswordStatus = pwdStatus
                    validation.LockReason = lockReason

                    Dim pwdStatusUpper As String = pwdStatus.ToUpper()
                    Dim lockReasonUpper As String = lockReason.ToUpper()

                    ' "Does not exist" - acceptable
                    If pwdStatusUpper.Contains("DOES NOT EXIST") Then
                        validation.PasswordOK = True
                        validation.LockOK = True
                        validation.IsCompliant = True
                    Else
                        ' Lock validation
                        Dim isLockedByAdmin As Boolean = lockReasonUpper.Contains("LOCKED BY ADMINISTRATOR") OrElse
                                                         lockReasonUpper.Contains("LOCKED GLOBALLY AND LOCALLY BY CUA")

                        validation.LockOK = isLockedByAdmin

                        ' Password validation - OK if locked by admin OR password doesn't exist
                        validation.PasswordOK = isLockedByAdmin

                        validation.IsCompliant = isLockedByAdmin
                    End If

                    allValidations.Add(validation)
                Next

                Logger.LogMessage($"Total standard user records: {allValidations.Count}", True)

                ' Take screenshots
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim ii As Integer = 0, jj As Integer = visibleRows
                Do While ExecutionTime > 0
                    Try
                        grid.SelectedRows = ii
                        Threading.Thread.Sleep(waitTime)
                        Takescreenshot(WordDoc, stepNum)

                        If jj < totalRows Then
                            grid.FirstVisibleRow += visibleRows
                            jj += visibleRows
                        End If
                    Catch ssEx As Exception
                        Logger.LogMessage("Screenshot loop error: " & ssEx.Message, False)
                    End Try
                    ExecutionTime -= 1
                    ii += visibleRows
                Loop

            Catch validationEx As Exception
                Logger.LogMessage("Error during validation: " & validationEx.Message, False)
                Logger.LogException(validationEx, $"Validation error in {controlID}")
            End Try

            ' ============================================================
            ' Add validation results to Word as TABLES per client
            ' ============================================================
            Try
                ' Section header
                With WordDoc.Paragraphs.Last.Range
                    .Text = "Validation Results - Standard SAP User Lock & Password Status"
                    .Font.Size = 13
                    .Font.Bold = True
                    .Font.Color = WdColor.wdColorBlack
                    .ParagraphFormat.SpaceBefore = 12
                    .InsertParagraphAfter()
                End With

                With WordDoc.Paragraphs.Last.Range
                    .Font.Size = 11
                    .Font.Bold = False
                    .Font.Color = WdColor.wdColorAutomatic
                End With

                ' Validation rules note
                With WordDoc.Paragraphs.Last.Range
                    .Text = "Legend: ✓ = Compliant   ✗ = Non-Compliant"
                    .Font.Size = 10
                    .Font.Italic = True
                    .Font.Color = WdColor.wdColorGray50
                    .InsertParagraphAfter()
                End With

                With WordDoc.Paragraphs.Last.Range
                    .Font.Italic = False
                    .Font.Color = WdColor.wdColorAutomatic
                End With

                ' Group validations by client
                Dim clientGroups = allValidations.GroupBy(Function(v) v.ClientId).OrderBy(Function(g) g.Key).ToList()

                If clientGroups.Count = 0 Then
                    With WordDoc.Paragraphs.Last.Range
                        .Text = "No standard user data was retrieved for validation."
                        .Font.Size = 11
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorDarkRed
                        .InsertParagraphAfter()
                    End With
                End If

                ' Create one table per client
                For Each clientGroup In clientGroups
                    Dim clientName As String = If(String.IsNullOrEmpty(clientGroup.Key), "Unknown", clientGroup.Key)
                    Dim users = clientGroup.ToList()

                    ' Client header
                    With WordDoc.Paragraphs.Last.Range
                        .Text = $"Client: {clientName}"
                        .Font.Size = 12
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorDarkBlue
                        .ParagraphFormat.SpaceBefore = 10
                        .ParagraphFormat.SpaceAfter = 4
                        .InsertParagraphAfter()
                    End With

                    With WordDoc.Paragraphs.Last.Range
                        .Font.Size = 11
                        .Font.Bold = False
                        .Font.Color = WdColor.wdColorAutomatic
                    End With

                    ' Create table: header + 1 row per user
                    Dim tbl As Word.Table = WordDoc.Tables.Add(WordDoc.Paragraphs.Last.Range, users.Count + 1, 3)
                    tbl.Borders.Enable = 1
                    tbl.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle
                    tbl.Borders.InsideLineWidth = WdLineWidth.wdLineWidth025pt
                    tbl.Borders.InsideColor = WdColor.wdColorGray50
                    tbl.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle
                    tbl.Borders.OutsideLineWidth = WdLineWidth.wdLineWidth025pt
                    tbl.Borders.OutsideColor = WdColor.wdColorGray50

                    ' Fit table to window width
                    tbl.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent
                    tbl.PreferredWidth = 100
                    tbl.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow)

                    ' Header row
                    With tbl.Cell(1, 1).Range
                        .Text = "User ID"

                        .Font.Size = 10
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                        .ParagraphFormat.SpaceBefore = 0
                        .ParagraphFormat.SpaceAfter = 0
                    End With
                    tbl.Cell(1, 1).Shading.BackgroundPatternColor = WdColor.wdColorGray15

                    With tbl.Cell(1, 2).Range
                        .Text = "Password Status"

                        .Font.Size = 10
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                        .ParagraphFormat.SpaceBefore = 0
                        .ParagraphFormat.SpaceAfter = 0
                    End With
                    tbl.Cell(1, 2).Shading.BackgroundPatternColor = WdColor.wdColorGray15

                    With tbl.Cell(1, 3).Range
                        .Text = "Lock Status"

                        .Font.Size = 10
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                        .ParagraphFormat.SpaceBefore = 0
                        .ParagraphFormat.SpaceAfter = 0
                    End With
                    tbl.Cell(1, 3).Shading.BackgroundPatternColor = WdColor.wdColorGray15

                    ' Header row - vertical center alignment
                    For c As Integer = 1 To 3
                        tbl.Cell(1, c).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter
                    Next

                    ' Data rows
                    Dim rowIdx As Integer = 2
                    For Each user In users
                        With tbl.Cell(rowIdx, 1).Range
                            .Text = user.UserId

                            .Font.Size = 10
                            .Font.Bold = False
                            .Font.Color = WdColor.wdColorBlack
                            .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                            .ParagraphFormat.SpaceBefore = 0
                            .ParagraphFormat.SpaceAfter = 0
                        End With

                        With tbl.Cell(rowIdx, 2).Range
                            If user.PasswordOK Then
                                .Text = "✓"
                                .Font.Color = WdColor.wdColorDarkGreen
                            Else
                                .Text = "✗"
                                .Font.Color = WdColor.wdColorDarkRed
                            End If

                            .Font.Size = 11
                            .Font.Bold = True
                            .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                            .ParagraphFormat.SpaceBefore = 0
                            .ParagraphFormat.SpaceAfter = 0
                        End With

                        With tbl.Cell(rowIdx, 3).Range
                            If user.LockOK Then
                                .Text = "✓"
                                .Font.Color = WdColor.wdColorDarkGreen
                            Else
                                .Text = "✗"
                                .Font.Color = WdColor.wdColorDarkRed
                            End If

                            .Font.Size = 11
                            .Font.Bold = True
                            .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                            .ParagraphFormat.SpaceBefore = 0
                            .ParagraphFormat.SpaceAfter = 0
                        End With

                        ' Vertical alignment center
                        For c As Integer = 1 To 3
                            tbl.Cell(rowIdx, c).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter
                        Next

                        rowIdx += 1
                    Next

                    ' Let Word auto-adjust row heights based on content
                    tbl.Rows.HeightRule = WdRowHeightRule.wdRowHeightAtLeast
                    tbl.Rows.Height = WordApp.CentimetersToPoints(0.65)

                    ' Add small spacer after table
                    Dim spacerRange As Word.Range = WordDoc.Content
                    spacerRange.Collapse(WdCollapseDirection.wdCollapseEnd)
                    spacerRange.InsertParagraphAfter()
                Next

                ' Reset font
                With WordDoc.Paragraphs.Last.Range
                    .Font.Size = 11
                    .Font.Bold = False
                    .Font.Italic = False
                    .Font.Color = WdColor.wdColorAutomatic
                End With

                ' ============================================================
                ' Add Final Summary at the end
                ' ============================================================
                Try
                    ' Calculate summary stats
                    Dim totalChecksLocal As Integer = allValidations.Count
                    Dim violationsLocal = allValidations.Where(Function(v) Not v.IsCompliant).ToList()
                    Dim compliantLocal = allValidations.Where(Function(v) v.IsCompliant).ToList()
                    Dim isCompliantOverall As Boolean = (violationsLocal.Count = 0)
                    Dim totalClients As Integer = allValidations.Select(Function(v) v.ClientId).Distinct().Count()

                    ' Spacer
                    With WordDoc.Paragraphs.Last.Range
                        .Text = ""
                        .ParagraphFormat.SpaceBefore = 12
                        .InsertParagraphAfter()
                    End With

                    ' "Summary" heading
                    With WordDoc.Paragraphs.Last.Range
                        .Text = "Summary"

                        .Font.Size = 12
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.SpaceBefore = 8
                        .ParagraphFormat.SpaceAfter = 6
                        .ParagraphFormat.Borders(WdBorderType.wdBorderBottom).LineStyle = WdLineStyle.wdLineStyleSingle
                        .ParagraphFormat.Borders(WdBorderType.wdBorderBottom).LineWidth = WdLineWidth.wdLineWidth050pt
                        .ParagraphFormat.Borders(WdBorderType.wdBorderBottom).Color = WdColor.wdColorGray50
                        .InsertParagraphAfter()
                    End With

                    ' Reset font and remove border for next paragraph
                    With WordDoc.Paragraphs.Last.Range
                        .Font.Size = 11
                        .Font.Bold = False
                        .Font.Color = WdColor.wdColorAutomatic
                        .ParagraphFormat.Borders(WdBorderType.wdBorderBottom).LineStyle = WdLineStyle.wdLineStyleNone
                    End With

                    ' Expected criteria intro
                    With WordDoc.Paragraphs.Last.Range
                        .Text = "Expected Criteria:"
                        .Font.Name = "Calibri"
                        .Font.Size = 11
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.SpaceBefore = 4
                        .ParagraphFormat.SpaceAfter = 2
                        .InsertParagraphAfter()
                    End With

                    ' Bullet list of criteria
                    With WordDoc.Paragraphs.Last.Range
                        .Text = "•  All standard SAP users (DDIC, SAP*, SAPCPIC, EARLYWATCH, TMSADM) must be locked by administrator in every client." & vbCrLf &
                "•  Standard users must NOT have active passwords (or must be locked)." & vbCrLf &
                "•  ""Locked by unsuccessful logons"" is NOT acceptable as it is only a temporary lock."
                        .Font.Name = "Calibri"
                        .Font.Size = 10
                        .Font.Bold = False
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.SpaceAfter = 6
                        .ParagraphFormat.LeftIndent = WordApp.CentimetersToPoints(0.3)
                        .InsertParagraphAfter()
                    End With

                    ' Audit Result heading
                    With WordDoc.Paragraphs.Last.Range
                        .Text = "Audit Result:"
                        .Font.Name = "Calibri"
                        .Font.Size = 11
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.SpaceBefore = 6
                        .ParagraphFormat.SpaceAfter = 2
                        .ParagraphFormat.LeftIndent = 0
                        .InsertParagraphAfter()
                    End With

                    ' Build status summary
                    Dim statusSummary As String = $"•  Total Clients Reviewed: {totalClients}" & vbCrLf &
                                  $"•  Total Standard User Records Checked: {totalChecksLocal}" & vbCrLf &
                                  $"•  Compliant Records: {compliantLocal.Count}" & vbCrLf &
                                  $"•  Non-Compliant Records: {violationsLocal.Count}"

                    With WordDoc.Paragraphs.Last.Range
                        .Text = statusSummary
                        .Font.Name = "Calibri"
                        .Font.Size = 10
                        .Font.Bold = False
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.SpaceAfter = 6
                        .ParagraphFormat.LeftIndent = WordApp.CentimetersToPoints(0.3)
                        .InsertParagraphAfter()
                    End With

                    ' Overall Status
                    With WordDoc.Paragraphs.Last.Range
                        .Text = "Overall Status:"
                        .Font.Name = "Calibri"
                        .Font.Size = 11
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.SpaceBefore = 6
                        .ParagraphFormat.SpaceAfter = 2
                        .ParagraphFormat.LeftIndent = 0
                        .InsertParagraphAfter()
                    End With

                    Dim overallText As String
                    If isCompliantOverall Then
                        overallText = "PASS - All standard SAP users meet the expected criteria. No action required."
                    Else
                        overallText = $"FAIL - {violationsLocal.Count} non-compliant record(s) identified. Review and remediation required."
                    End If

                    With WordDoc.Paragraphs.Last.Range
                        .Text = overallText
                        .Font.Name = "Calibri"
                        .Font.Size = 11
                        .Font.Bold = True
                        .Font.Color = If(isCompliantOverall, WdColor.wdColorDarkGreen, WdColor.wdColorDarkRed)
                        .ParagraphFormat.LeftIndent = WordApp.CentimetersToPoints(0.3)
                        .ParagraphFormat.SpaceAfter = 6
                        .InsertParagraphAfter()
                    End With

                    ' If violations exist, list them
                    If violationsLocal.Count > 0 Then
                        With WordDoc.Paragraphs.Last.Range
                            .Text = "Non-Compliant Records:"
                            .Font.Name = "Calibri"
                            .Font.Size = 11
                            .Font.Bold = True
                            .Font.Color = WdColor.wdColorBlack
                            .ParagraphFormat.SpaceBefore = 6
                            .ParagraphFormat.SpaceAfter = 2
                            .ParagraphFormat.LeftIndent = 0
                            .InsertParagraphAfter()
                        End With

                        Dim violationDetail As String = ""
                        For Each v In violationsLocal
                            Dim issueDesc As String = ""
                            If Not v.LockOK AndAlso Not v.PasswordOK Then
                                issueDesc = "User is not locked by administrator"
                            ElseIf Not v.LockOK Then
                                issueDesc = "Lock status not acceptable"
                            ElseIf Not v.PasswordOK Then
                                issueDesc = "Password status not acceptable"
                            End If
                            violationDetail &= $"•  Client {v.ClientId} - {v.UserId}: {issueDesc}" & vbCrLf
                        Next

                        With WordDoc.Paragraphs.Last.Range
                            .Text = violationDetail.TrimEnd()
                            .Font.Name = "Calibri"
                            .Font.Size = 10
                            .Font.Bold = False
                            .Font.Color = WdColor.wdColorBlack
                            .ParagraphFormat.LeftIndent = WordApp.CentimetersToPoints(0.3)
                            .ParagraphFormat.SpaceAfter = 4
                            .InsertParagraphAfter()
                        End With
                    End If

                    ' Reset font for any future content
                    With WordDoc.Paragraphs.Last.Range
                        .Font.Size = 11
                        .Font.Bold = False
                        .Font.Color = WdColor.wdColorAutomatic
                        .ParagraphFormat.LeftIndent = 0
                    End With

                Catch summaryEx As Exception
                    Logger.LogMessage("Error adding summary: " & summaryEx.Message, False)
                End Try
            Catch wordEx As Exception
                Logger.LogMessage("Error adding validation tables to Word: " & wordEx.Message, False)
            End Try

            ' Export to Excel
            Try
                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath

                Dim fileNameWithExt As String = $"{controlITGC} {systemname} Password Status for the default SAP ID.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            Catch exportEx As Exception
                Logger.LogMessage("Error during export: " & exportEx.Message, False)
            End Try

            ' =============================== Code for Report gather ====================================
            Logger.LogMessage("Gathering report for " & controlID, True)

            Dim ReprtOutputPath As String
            If Not String.IsNullOrEmpty(ReportMonth) Then
                Dim downloadsPath As String = Path.Combine(My.Settings.DownloadDestination, "ITGC REPORT")
                ReprtOutputPath = downloadsPath
            Else
                ReprtOutputPath = String.Empty
            End If

            ' Build summary comment
            Dim totalChecks As Integer = allValidations.Count
            Dim violations = allValidations.Where(Function(v) Not v.IsCompliant).ToList()
            Dim isCompliant As Boolean = (violations.Count = 0)

            Dim finalComment As String
            If isCompliant Then
                finalComment = $"All {totalChecks} standard SAP user records are properly locked. No violations found."
            Else
                Dim violationSummary = String.Join(" | ", violations.Take(5).Select(Function(v) $"Client {v.ClientId} - {v.UserId}").ToArray())
                finalComment = $"Validation FAILED. Violations: {violations.Count} of {totalChecks}. {violationSummary}"
                If violations.Count > 5 Then
                    finalComment &= $" ... and {violations.Count - 5} more (see Word report)"
                End If
            End If

            If isCompliant Then
                SaveITGCComment(
                    controlID:=controlITGC,
                    description:=controlName,
                    systemName:=systemname,
                    isGreen:=True,
                    comment:=finalComment,
                    baseFolder:=ReprtOutputPath,
                    forMonth:=ReportMonth)
            Else
                SaveITGCComment(
                    controlID:=controlITGC,
                    description:=controlName,
                    systemName:=systemname,
                    isGreen:=False,
                    comment:=finalComment,
                    baseFolder:=ReprtOutputPath,
                    forMonth:=ReportMonth)
            End If

            session.FindById("wnd[0]/tbar[0]/okcd").Text = "/nex"
            session.FindById("wnd[0]").SendVKey(0)
            DisconnectFromSAP()

            EnsureFolderPathExistsAndSave(folderPath, fileName, WordDoc)
            Logger.LogMessage("Execution Finish for " & controlID, True)

        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Logger.LogMessage("An error occurred: " & ex.Message, False)
        Finally
            Try
                If WordDoc IsNot Nothing Then
                    WordDoc.Close(False)
                    Marshal.ReleaseComObject(WordDoc)
                    WordDoc = Nothing
                End If
                If WordApp IsNot Nothing Then
                    WordApp.Quit()
                    Marshal.ReleaseComObject(WordApp)
                    WordApp = Nothing
                End If
            Catch cleanupEx As Exception
                Logger.LogMessage("Error during Word cleanup: " & cleanupEx.Message, False)
            End Try
        End Try
    End Sub
End Module