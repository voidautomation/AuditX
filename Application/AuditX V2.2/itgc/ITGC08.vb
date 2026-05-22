Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop

Module ITGC08
    ' Subroutine to check List of Users having Table Maintenance Access in Production

    ' Helper function to set clipboard text in STA thread
    Private Sub SetClipboardText(text As String)
        Dim thread As New Threading.Thread(Sub()
                                               System.Windows.Forms.Clipboard.SetText(text)
                                           End Sub)
        thread.SetApartmentState(Threading.ApartmentState.STA)
        thread.Start()
        thread.Join()
    End Sub

    ' Helper function to build and copy user list to clipboard
    Private Sub CopyUsersToClipboard(settingValue As String)
        Dim rawUsers() As String = settingValue.Split(";"c)
        Dim userList As New List(Of String)
        For Each user As String In rawUsers
            Dim trimmed As String = user.Trim()
            If trimmed <> "" Then
                userList.Add(trimmed)
            End If
        Next
        Dim clipboardText As String = String.Join(vbCrLf, userList)
        SetClipboardText(clipboardText)
        Threading.Thread.Sleep(500)
    End Sub

    Sub ExecuteITGC08()
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
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").Text = "SUIM"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 1
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("02  1      2")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("03  2      7")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").selectItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ensureVisibleHorizontalItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 2
            stepNum += 1
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").clickLink("04  2      8", "1")
            session.findById("wnd[0]/usr/btn%_USER_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpNOSV").select

            ' ---- Clipboard method for Checkpoint 1 ApprovedUsers ----
            CopyUsersToClipboard(My.Settings.ITGC08_ApprovedUsers)
            session.findById("wnd[1]/tbar[0]/btn[24]").press()     ' Upload from clipboard
            Threading.Thread.Sleep(500)
            session.findById("wnd[1]/tbar[0]/btn[8]").press()      ' Confirm
            Threading.Thread.Sleep(waitTime)
            ' ---------------------------------------------------------

            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/btn%_UTYPE_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").Text = "A"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").Text = "S"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").SetFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 1
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 3
            stepNum += 1
            session.findById("wnd[1]").sendVKey(0)
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4").Select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").Text = "S_TCODE"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").SetFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").caretPosition = 7
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL101").Text = "SM30"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL102").Text = "SM31"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").Text = "S_TABU_NAM"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").SetFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").caretPosition = 10
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL201").Text = "02"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").Text = "*"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL213").SetFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL213").caretPosition = 2
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr").VerticalScrollbar.Position = 700
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 4
            stepNum += 1

            session.findById("wnd[0]/tbar[1]/btn[8]").press

            Dim LogMessage1 As String = ""
            Try
                LogMessage1 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC08")
            End Try

            If LogMessage1.Contains("No matching user found") Then
                stepNum = 401
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 401
            Else
                LogMessage1 = "Users having Table Maintenance Access with table name"
                session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim i As Integer = 0, j As Integer = visibleRows

                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 5

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
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} S_TABU_NAM Export.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press
                Try
                    session.findById("wnd[1]/usr/btnALLOW").press
                Catch ex As Exception
                    ' No overwrite prompt appeared; continue execution
                End Try
            End If

            session.findById("wnd[0]/tbar[0]/okcd").Text = "/n"
            session.findById("wnd[0]").sendVKey(0)

            ' Checkpoint 2 -----------------------------------------------------------------------------
            session.findById("wnd[0]/tbar[0]/okcd").Text = "SUIM"
            stepNum = 6
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 6
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("02  1      2")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("03  2      7")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").selectItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ensureVisibleHorizontalItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 7
            stepNum += 1
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").clickLink("04  2      8", "1")
            session.findById("wnd[0]/usr/btn%_USER_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpNOSV").select

            ' ---- Clipboard method for Checkpoint 2 ApprovedUsers ----
            CopyUsersToClipboard(My.Settings.ITGC08_ApprovedUsers)
            session.findById("wnd[1]/tbar[0]/btn[24]").press()     ' Upload from clipboard
            Threading.Thread.Sleep(500)
            session.findById("wnd[1]/tbar[0]/btn[8]").press()      ' Confirm
            Threading.Thread.Sleep(waitTime)
            ' ---------------------------------------------------------

            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/btn%_UTYPE_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").Text = "A"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").Text = "S"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").SetFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 1
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 8
            stepNum += 1
            session.findById("wnd[1]").sendVKey(0)
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4").Select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").Text = "S_TCODE"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").SetFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").caretPosition = 7
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL101").Text = "SM30"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL102").Text = "SM31"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").Text = "S_TABU_DIS"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").SetFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").caretPosition = 10
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL201").Text = "*"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").Text = "02"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL213").SetFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL213").caretPosition = 2
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr").VerticalScrollbar.Position = 700
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 9
            stepNum += 1

            session.findById("wnd[0]/tbar[1]/btn[8]").press

            Dim LogMessage2 As String = ""
            Try
                LogMessage2 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC08")
            End Try

            If LogMessage2.Contains("No matching user found") Then
                stepNum = 402
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 402
            Else
                LogMessage2 = "Users having Table Maintenance Access with table group"
                session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim i As Integer = 0, j As Integer = visibleRows

                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 10

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
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} S_TABU_NAM Export.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.findById("wnd[1]/tbar[0]/btn[0]").press
            End If

            ' =============================== Code for Report gather ====================================
            Dim ReprtOutputPath As String

            If Not String.IsNullOrEmpty(ReportMonth) Then
                Dim downloadsPath As String = Path.Combine(
                    My.Settings.DownloadDestination,
                    "ITGC REPORT")
                ReprtOutputPath = downloadsPath
            Else
                ReprtOutputPath = String.Empty
            End If

            If LogMessage1.Contains("No matching user found") Or
                LogMessage2.Contains("No matching user found") Then
                ' GREEN ZONE
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