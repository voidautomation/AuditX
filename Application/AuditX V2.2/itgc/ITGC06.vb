Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop

Module ITGC06
    ' Subroutine to check Developer Key/Access in Production
    Sub ExecuteITGC06()
        Dim WordApp As Word.Application = Nothing
        Dim WordDoc As Word.Document = Nothing

        Try
            ' Minimize all windows except SAP
            Dim executionMode As String = Home.cmbExecutionMode.SelectedItem?.ToString()
            If executionMode = "Foreground" Then
                MinimizeAllWindowsExceptSAP()
            End If

            ' Start Word and create a new document
            WordApp = New Word.Application()
            WordApp.Visible = True
            WordDoc = WordApp.Documents.Add()

            ' Get system and control details
            Dim systemname As String = Home.lblSystemId.Text
            Dim controlName As String = Home.lblDescription.Text
            Dim controlID As String = Home.cmbControl.SelectedItem?.ToString().Split("-"c)(0).Trim()

            ' Add header
            With WordDoc.Paragraphs.Last.Range
                .Text = $"{controlID} - {controlName}{vbCrLf}System: {systemname}"
                .Font.Size = 16
                .Font.Bold = True
                .ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter
                .InsertParagraphAfter()
            End With

            ' Reset font and alignment
            With WordDoc.Paragraphs.Last.Range
                .Font.Size = 11
                .Font.Bold = False
                .ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft
            End With

            ' SAP GUI connection
            Dim SapGuiAuto = GetObject("SAPGUI")
            If SapGuiAuto Is Nothing Then
                MessageBox.Show("SAP GUI is not running.")
                Logger.LogMessage("SAP GUI is not running.", False)
                Exit Sub
            End If

            ' Declare variables
            Dim selectSystem As String = Home.cmbSystem.SelectedItem?.ToString()
            Dim controlITGC As String = Home.cmbControl.SelectedItem?.ToString().Split("-"c)(0).Trim()
            Dim folderPath As String = Home.txtFolderPath.Text
            Dim ReportMonth As String = Home.txtReportMonth.Text
            Dim selectedStartDate As DateTime = Home.dtpStart.Value
            Dim reportStartDate As String = selectedStartDate.ToString("dd.MM.yyyy")
            Dim selectedEndDate As DateTime = Home.dtpEnd.Value
            Dim reportEndDate As String = selectedEndDate.ToString("dd.MM.yyyy")
            Dim stepNum As Integer = 1
            Dim waitTime As TimeSpan = TimeSpan.FromSeconds(1)
            Dim fileName As String = $"{controlITGC} {systemname} Audit Report.docx"

            '---------------------------------------------------------------------------------------------------------
            ' Execute SAP steps
            Logger.LogMessage($"{controlID} Execution Started!", True)

            session.findById("wnd[0]").maximize

            ' Checkpoint 1 - DEVACCESS Table Check
            '-----------------------------------------------------------------------------------------------------------------
            session.findById("wnd[0]/tbar[0]/okcd").text = "SE16"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 1
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/ctxtDATABROWSE-TABLENAME").text = "DEVACCESS"
            session.findById("wnd[0]/usr/ctxtDATABROWSE-TABLENAME").caretPosition = 9
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 2
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/txtMAX_SEL").text = ""
            session.findById("wnd[0]/usr/txtMAX_SEL").setFocus
            session.findById("wnd[0]/usr/txtMAX_SEL").caretPosition = 11
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 3
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/tbar[1]/btn[8]").press

            Dim LogMessage1 As String = ""
            Try
                LogMessage1 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, $"Error retrieving log message in {controlID}")
            End Try

            If LogMessage1.Contains("No table entries found for specified key") Then
                stepNum = 401
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 401
            Else
                LogMessage1 = "[DEVACCESS] - Developer Key/Access found in Production"
                Try
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                Catch ex As Exception
                    Logger.LogMessage("Error setting row size: " & ex.Message, False)
                End Try

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").VisibleRowCount
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim i As Integer = 0, j As Integer = visibleRows
                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 4

                    If j < totalRows Then
                        Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")
                        grid.FirstVisibleRow += visibleRows
                        j += visibleRows
                    End If
                    ExecutionTime -= 1
                    i += visibleRows
                Loop
                stepNum += 1

                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} Developer Key User export.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If

            session.findById("wnd[0]/tbar[0]/okcd").Text = "/n"
            session.findById("wnd[0]").sendVKey(0)

            ' Checkpoint 2 - SUIM Debug Change Access Check
            '-----------------------------------------------------------------------------------------------------------------
            stepNum = 5
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").text = "SUIM"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 5
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("02  1      2")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("03  2      7")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").selectItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ensureVisibleHorizontalItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 6
            stepNum += 1
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").clickLink("04  2      8", "1")
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").text = "A"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").caretPosition = 1
            session.findById("wnd[0]").sendVKey(0)
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 7
            stepNum += 1
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4").select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").text = "S_DEVELOP"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").caretPosition = 9
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL111").text = "DEBUG"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL141").text = "02"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL143").text = "03"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL143").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL143").caretPosition = 2
            session.findById("wnd[0]").sendVKey(0)
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 8
            stepNum += 1

            session.findById("wnd[0]/tbar[1]/btn[8]").press
            Dim LogMessage2 As String = ""
            Try
                LogMessage2 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC06")
            End Try

            If LogMessage2.Contains("No matching user found") Then
                stepNum = 402
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 402
            Else
                LogMessage2 = "[DEBUG Change Access] - User found with debug change access"
                Try
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                Catch ex As Exception
                    Logger.LogMessage("Error setting row size: " & ex.Message, False)
                End Try

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim i As Integer = 0, j As Integer = visibleRows
                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 9

                    If j < totalRows Then
                        Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell")
                        grid.FirstVisibleRow += visibleRows
                        j += visibleRows
                    End If
                    ExecutionTime -= 1
                    i += visibleRows
                Loop
                stepNum += 1

                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} User with Debug change export.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If

            ' Checkpoint 3 - Role Check for Debug Access
            '-----------------------------------------------------------------------------------------------------------------
            Dim LogMessage3 As String = ""

            If Not LogMessage2.Contains("No matching user found") Then
                ' Users were found - now check if they have access via role
                Try
                    session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").currentCellRow = -1
                    session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectColumn("BNAME")
                    session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectedRows = ""
                    stepNum = 10
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 10
                    stepNum += 1
                    session.findById("wnd[0]/tbar[1]/btn[23]").press
                    session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell").selectedRows = "0"

                    Try
                        LogMessage3 = session.FindById("wnd[0]/sbar").Text
                    Catch ex As Exception
                        Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                        Logger.LogException(ex, "Error retrieving log message in ITGC06")
                    End Try

                    If LogMessage3.Contains("No matching user found") Then
                        ' -----------------------------------------------------------------------
                        ' No role found - user may have access via DIRECT PROFILE ASSIGNMENT
                        ' Take screenshot and add note in Word document
                        ' -----------------------------------------------------------------------
                        stepNum = 403
                        Threading.Thread.Sleep(waitTime)
                        Takescreenshot(WordDoc, stepNum)            ' Step 403

                        ' Add note in Word document about direct profile assignment
                        With WordDoc.Paragraphs.Last.Range
                            .Text = "Note: No role assignment found for user(s) with Debug Change Access. " &
                                    "User(s) may have access via Direct Profile Assignment. " &
                                    "Please review profile assignments in SU01 for the user(s) listed above."
                            .Font.Size = 11
                            .Font.Bold = True
                            .Font.Color = Word.WdColor.wdColorDarkRed
                            .ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft
                            .InsertParagraphAfter()
                        End With

                        ' Reset font after note
                        With WordDoc.Paragraphs.Last.Range
                            .Font.Size = 11
                            .Font.Bold = False
                            .Font.Color = Word.WdColor.wdColorAutomatic
                        End With

                        LogMessage3 = "[DEBUG Role Check] - No role found. User(s) may have access via direct profile assignment"
                        Logger.LogMessage("No role found for debug access - user may have direct profile assignment", False)

                    Else
                        ' Role found - proceed normally
                        LogMessage3 = "[DEBUG Role Access] - Role found with debug change access"
                        Try
                            session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                            session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                        Catch ex As Exception
                            Logger.LogMessage("Error setting row size: " & ex.Message, False)
                        End Try

                        Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").RowCount
                        Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").VisibleRowCount
                        Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                        Dim i As Integer = 0, j As Integer = visibleRows
                        Do While ExecutionTime > 0
                            session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SelectedRows = i
                            Threading.Thread.Sleep(waitTime)
                            Takescreenshot(WordDoc, stepNum)        ' Step 11+

                            If j < totalRows Then
                                Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")
                                grid.FirstVisibleRow += visibleRows
                                j += visibleRows
                            End If
                            ExecutionTime -= 1
                            i += visibleRows
                        Loop
                        stepNum += 1

                        session.FindById("wnd[0]").SendVKey(45)
                        session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                        session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                        session.findById("wnd[1]/tbar[0]/btn[0]").press
                        session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                        Dim fileNameWithExt As String = $"{controlITGC} {systemname} User's role with Debug change export.xls"
                        Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                        session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                        session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                        session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
                    End If

                Catch ex As Exception
                    ' -----------------------------------------------------------------------
                    ' Exception during role check - likely because no roles exist
                    ' Treat as direct profile assignment
                    ' -----------------------------------------------------------------------
                    Logger.LogMessage("Error during role check - user may have direct profile assignment: " & ex.Message, False)

                    stepNum = 403
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 403

                    ' Add note in Word document
                    With WordDoc.Paragraphs.Last.Range
                        .Text = "Note: Unable to retrieve role assignment for user(s) with Debug Change Access. " &
                                "User(s) may have access via Direct Profile Assignment. " &
                                "Please review profile assignments in SU01 for the user(s) listed above."
                        .Font.Size = 11
                        .Font.Bold = True
                        .Font.Color = Word.WdColor.wdColorDarkRed
                        .ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft
                        .InsertParagraphAfter()
                    End With

                    ' Reset font after note
                    With WordDoc.Paragraphs.Last.Range
                        .Font.Size = 11
                        .Font.Bold = False
                        .Font.Color = Word.WdColor.wdColorAutomatic
                    End With

                    LogMessage3 = "[DEBUG Role Check] - Error retrieving role. User(s) may have access via direct profile assignment"
                End Try
            End If

            ' =============================== Report gather ====================================
            Logger.LogMessage("Gathering report for " & controlID, True)

            Dim ReprtOutputPath As String
            If Not String.IsNullOrEmpty(ReportMonth) Then
                Dim downloadsPath As String = Path.Combine(
                    My.Settings.DownloadDestination,
                    "ITGC REPORT")
                ReprtOutputPath = downloadsPath
            Else
                ReprtOutputPath = String.Empty
            End If

            ' Build final comment based on all three checks
            Dim finalComment As String = $"[Check 1] - {LogMessage1} | [Check 2] - {LogMessage2}"
            If Not String.IsNullOrEmpty(LogMessage3) Then
                finalComment &= $" | [Check 3] - {LogMessage3}"
            End If

            ' Determine Green or Red
            ' Green only if ALL checks pass (no access found anywhere)
            Dim isGreen As Boolean = (
                LogMessage1.Contains("No table entries found for specified key") AndAlso
                LogMessage2.Contains("No matching user found"))

            If isGreen Then
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
            ' ============================================================================================

            session.FindById("wnd[0]/tbar[0]/okcd").Text = "/nex"
            session.FindById("wnd[0]").SendVKey(0)
            DisconnectFromSAP()

            '---------------------------------------------------------------------------------------------------------
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