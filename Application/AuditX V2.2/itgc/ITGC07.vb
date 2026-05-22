Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop

Module ITGC07
    ' Subroutine to check High level of Privilege Access Control
    Sub ExecuteITGC07()
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
            ' Maximize the SAP window and perform actions
            ' Checpoint 1 ---------------------------------------------------------------------
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").Text = "SUIM"
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 1
            stepNum += 1                                            ' Increase the step number
            session.findById("wnd[0]").sendVKey(0)
            Try
                ' Try Node-specific logic
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ExpandNode("02  1     10")
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").TopNode = "01  1      1"
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ExpandNode("03  2      1")
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").SelectItem("04  2      2", "1")
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").EnsureVisibleHorizontalItem("04  2      2", "1")
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").TopNode = "01  1      1"
                Threading.Thread.Sleep(waitTime)                         ' Pause to ensure screen updates
                Takescreenshot(WordDoc, stepNum)                         ' Take screenshot
                stepNum += 1                                             ' Increase step number
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ClickLink("04  2      2", "1")

            Catch ex As Exception
                ' If Node-specific logic fails, fall back to default navigation
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ExpandNode("02  1     10")
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").TopNode = "01  1      1"
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").SelectItem("03  2      1", "1")
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").EnsureVisibleHorizontalItem("03  2      1", "1")

                Threading.Thread.Sleep(waitTime)                         ' Pause to ensure screen updates
                Takescreenshot(WordDoc, stepNum)                         ' Take screenshot
                stepNum += 1                                             ' Increase step number
                session.FindById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ClickLink("03  2      1", "1")
            End Try                                          ' Increase the step number


            session.findById("wnd[0]/usr/btn%_USER_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/btnRSCSEL_255-SOP_I[0,0]").setFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/btnRSCSEL_255-SOP_I[0,0]").press
            session.findById("wnd[2]/usr/cntlOPTION_CONTAINER/shellcont/shell").currentCellColumn = "TEXT"
            session.findById("wnd[2]/usr/cntlOPTION_CONTAINER/shellcont/shell").doubleClickCurrentCell
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE").columns.elementAt(1).width = 12
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").text = "SAP*"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").text = "DDIC"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").setFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 4
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 3
            stepNum += 1                                            ' Increase the step number

            session.findById("wnd[1]").sendVKey(0)
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpNOSV").Select
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpNOSV/ssubSCREEN_HEADER:SAPLALDB:3030/tblSAPLALDBSINGLE_E/ctxtRSCSEL_255-SLOW_E[1,0]").Text = "SAPOSS*"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpNOSV/ssubSCREEN_HEADER:SAPLALDB:3030/tblSAPLALDBSINGLE_E/ctxtRSCSEL_255-SLOW_E[1,0]").caretPosition = 7
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/usr/ctxtFDATE").Text = reportStartDate
            session.findById("wnd[0]/usr/ctxtFTIME").Text = My.Settings.Report_FTIME ' Setting based report Start time
            session.findById("wnd[0]/usr/ctxtTDATE").Text = reportEndDate
            session.findById("wnd[0]/usr/ctxtTTIME").Text = My.Settings.Report_TTIME ' Setting based report end time
            session.findById("wnd[0]/usr/ctxtTTIME").SetFocus
            session.findById("wnd[0]/usr/ctxtTTIME").caretPosition = 8
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 4
            stepNum += 1                                            ' Increase the step number
            session.findById("wnd[0]/usr").VerticalScrollbar.Position = 700
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR100N:1100/btnBUT_HS").press
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 5
            stepNum += 1                                            ' Increase the step number
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB2").Select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB2/ssub%_SUBSCREEN_TAB:RSUSR100N:1200/btnBUT_RS").press
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 6
            stepNum += 1                                            ' Increase the step number

            session.findById("wnd[0]/tbar[1]/btn[8]").press


            ' Check if "No change documents found to match the specified criteria" message is displayed in GuiUserArea

            Dim LogMessage1 As String = ""
            Try
                LogMessage1 = session.FindById("wnd[0]/sbar").Text

            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC07")
            End Try

            If LogMessage1.Contains("No change documents found to match the specified criteria") Then

                stepNum = 401
                Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
                Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 7, 401
                'stepNum += 1                                            ' Increase the step number
            Else
                LogMessage1 = "Change documents found to match the specified criteria"
                session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)

                'MsgBox("Total Row: " & totalRows & "Visible Row: " & visibleRows & "Execution Time: " & ExecutionTime)
                Dim i As Integer = 0, j As Integer = visibleRows

                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
                    Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 7

                    If j < totalRows Then
                        Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell")
                        grid.FirstVisibleRow += visibleRows
                        j += visibleRows
                    End If
                    ExecutionTime -= 1
                    i += visibleRows
                Loop
                stepNum += 1                                            ' Increase the step number
                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                'session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = $"{controlITGC} {systemname} {My.Settings.ITGC01_Filename}"

                ' if file is already exists, create another file with counter increment
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} Change Document of SAP* and DDIC Users.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If


            session.findById("wnd[0]/tbar[0]/okcd").Text = "/n"
            session.findById("wnd[0]").sendVKey(0)

            ' Checpoint 2 ---------------------------------------------------------------------
            stepNum = 8                                            ' Reset step number for second part
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").Text = "SUIM"
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 8
            stepNum += 1                                            ' Increase the step number

            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("02  1      2")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("03  2      7")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").selectItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ensureVisibleHorizontalItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 9
            stepNum += 1                                            ' Increase the step number
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").clickLink("04  2      8", "1")
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/btn%_UTYPE_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").Text = "A"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").Text = "S"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").SetFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 1
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 10
            stepNum += 1                                            ' Increase the step number
            session.findById("wnd[1]").sendVKey(0)
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB3").Select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB3/ssub%_SUBSCREEN_TAB:RSUSR002:1003/btn%_PROF1_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").Text = "SAP_ALL"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").Text = "SAP_NEW"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").SetFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 7
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 11
            stepNum += 1                                            ' Increase the step number
            session.findById("wnd[1]").sendVKey(0)
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/tbar[1]/btn[8]").press
            Dim LogMessage2 As String = ""
            Try
                LogMessage2 = session.FindById("wnd[0]/sbar").Text

            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC07")
            End Try

            If LogMessage2.Contains("No matching user found") Then

                stepNum = 402
                Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
                Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 11, 402
                'stepNum += 1                                            ' Increase the step number
            Else
                LogMessage2 = "Users with High Level of Privilege Access Control found"
                session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)

                'MsgBox("Total Row: " & totalRows & "Visible Row: " & visibleRows & "Execution Time: " & ExecutionTime)
                Dim i As Integer = 0, j As Integer = visibleRows

                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
                    Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number 12

                    If j < totalRows Then
                        Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell")
                        grid.FirstVisibleRow += visibleRows
                        j += visibleRows
                    End If
                    ExecutionTime -= 1
                    i += visibleRows
                Loop
                stepNum += 1                                            ' Increase the step number
                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                'session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = $"{controlITGC} {systemname} {My.Settings.ITGC01_Filename}"

                ' if file is already exists, create another file with counter increment
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} User Having SAP_ALL and SAP_NEW Profile.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If


            ' =============================== Code for Report gather ====================================
            '====REPORT STORING============================='
            Dim ReprtOutputPath As String

            If Not String.IsNullOrEmpty(ReportMonth) Then
                '            Dim downloadsPath As String = Path.Combine(
                'Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                '"Downloads", "ITGC REPORT")
                Dim downloadsPath As String = Path.Combine(
     My.Settings.DownloadDestination,
     "ITGC REPORT")

                ReprtOutputPath = downloadsPath
            Else
                ReprtOutputPath = String.Empty
            End If
            '==============================================
            If LogMessage1.Contains("No change documents found to match the specified criteria") Or
                LogMessage2.Contains("No matching user found") Then
                'GREEN ZONE
                SaveITGCComment(
                                 controlID:=controlITGC,
                                 description:=controlName,
                                 systemName:=systemname,
                                 isGreen:=True,
                                 comment:=$"[Check 1] - {LogMessage1} | [Check 2] - {LogMessage2}",
                                 baseFolder:=ReprtOutputPath,
                                 forMonth:=ReportMonth)
            Else
                ' RED ZONE
                SaveITGCComment(
                                controlID:=controlITGC,
                                description:=controlName,
                                systemName:=systemname,
                                isGreen:=False,
                                comment:=$"[Check 1] - {LogMessage1} | [Check 2] - {LogMessage2}",
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
            ' Centralized cleanup
            Try
                If WordDoc IsNot Nothing Then
                    WordDoc.Close(False)
                    Marshal.ReleaseComObject(WordDoc)
                    WordDoc = Nothing ' <-- Add this to prevent double-release in future code
                End If
                If WordApp IsNot Nothing Then
                    WordApp.Quit()
                    Marshal.ReleaseComObject(WordApp)
                    WordApp = Nothing ' <-- Add this
                End If
            Catch cleanupEx As Exception
                Logger.LogMessage("Error during Word cleanup: " & cleanupEx.Message, False)
            End Try
        End Try
    End Sub
End Module
