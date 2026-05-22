Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop

Module ITGC10
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

    ' Helper to add direct profile assignment note in Word
    Private Sub AddDirectProfileNote(WordDoc As Word.Document, context As String)
        Try
            With WordDoc.Paragraphs.Last.Range
                .Text = $"Note: No role assignment found for user(s) with {context}. " &
                        "User(s) may have access via Direct Profile Assignment. " &
                        "Please review profile assignments in SU01 for the user(s) listed above."
                .Font.Size = 11
                .Font.Bold = True
                .Font.Color = Word.WdColor.wdColorDarkRed
                .ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft
                .InsertParagraphAfter()
            End With
            ' Reset font
            With WordDoc.Paragraphs.Last.Range
                .Font.Size = 11
                .Font.Bold = False
                .Font.Color = Word.WdColor.wdColorAutomatic
            End With
        Catch ex As Exception
            Logger.LogMessage("Warning: Could not add profile note: " & ex.Message, False)
        End Try
    End Sub

    ' Subroutine to check Developer Key/Access in Production
    Sub ExecuteITGC10()
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
            Dim userCount As Integer = 0
            Dim roleCount As Integer = 0

            '---------------------------------------------------------------------------------------------------------
            ' Execute SAP steps
            Logger.LogMessage($"{controlID} Execution Started!", True)

            ' Checkpoint 1 - Ability to create user ID
            '-----------------------------------------------------------------------------------------------------------------
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").text = "SUIM"
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

            ' ---- Clipboard method for ApprovedUsers1 ----
            CopyUsersToClipboard(My.Settings.ITGC10_ApprovedUserscheck1)
            session.findById("wnd[1]/tbar[0]/btn[24]").press()     ' Upload from clipboard
            Threading.Thread.Sleep(500)
            session.findById("wnd[1]/tbar[0]/btn[8]").press()      ' Confirm
            Threading.Thread.Sleep(waitTime)
            ' ---------------------------------------------

            Takescreenshot(WordDoc, stepNum)                        ' Step 3
            stepNum += 1

            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").caretPosition = 0
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/btn%_UTYPE_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").text = "A"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").text = "S"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").setFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 1
            session.findById("wnd[0]").sendVKey(0)
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 4
            stepNum += 1
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4").select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").text = "S_TCODE"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").caretPosition = 7
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL101").text = "SU01"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL102").text = "SU10"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").text = "S_USER_GRP"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").caretPosition = 10
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL201").text = "*"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").text = "01"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL212").text = "02"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL212").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL212").caretPosition = 2
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr").VerticalScrollbar.Position = 700
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 5
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ3").text = "S_USER_AUT"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ3").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ3").caretPosition = 4
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL301").text = "*"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL311").text = "*"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL321").text = "02"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ4").text = "S_USER_PRO"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ4").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ4").caretPosition = 10
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL401").text = "*"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL411").text = "22"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL411").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL411").caretPosition = 2
            session.findById("wnd[0]").sendVKey(0)
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 5 continued
            stepNum += 1

            session.findById("wnd[0]/tbar[1]/btn[8]").press
            Dim LogMessage1 As String = ""
            Try
                LogMessage1 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC10")
            End Try

            If LogMessage1.Contains("No matching user found") Then
                stepNum = 401
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 401
            Else
                LogMessage1 = "User found with Ability to create user ID"
                Try
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                Catch ex As Exception
                    Logger.LogMessage("Error setting row size: " & ex.Message, False)
                End Try

                session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                userCount = totalRows
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim i As Integer = 0, j As Integer = visibleRows
                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 6
                    If j < totalRows Then
                        Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell")
                        grid.FirstVisibleRow += visibleRows
                        j += visibleRows
                    End If
                    ExecutionTime -= 1
                    i += visibleRows
                Loop
                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} User with Ability to create user ID.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If

            '======== Checkpoint 1.1 - Roles with Ability to create user ID ========
            Try
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").currentCellRow = -1
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectColumn("BNAME")
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectedRows = ""
                stepNum = 7
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 7
                stepNum += 1
                session.findById("wnd[0]/tbar[1]/btn[23]").press
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell").selectedRows = "0"

                Dim LogMessageRoles1 As String = ""
                Try
                    LogMessageRoles1 = session.FindById("wnd[0]/sbar").Text
                Catch ex As Exception
                    Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                    Logger.LogException(ex, "Error retrieving log message in ITGC10")
                End Try

                If LogMessageRoles1.Contains("No matching user found") Then
                    stepNum = 402
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 402
                    ' ---- Direct profile assignment note ----
                    AddDirectProfileNote(WordDoc, "Ability to create user ID")
                    LogMessageRoles1 = "No role found - user(s) may have access to create user ID via direct profile assignment"
                    Logger.LogMessage("Check 1.1: No role found - direct profile assignment suspected", False)
                    ' ----------------------------------------
                Else
                    LogMessageRoles1 = "Role found with Ability to create user ID"
                    Try
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                    Catch ex As Exception
                        Logger.LogMessage("Error setting row size: " & ex.Message, False)
                    End Try

                    Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").RowCount
                    Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").VisibleRowCount
                    Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                    roleCount = totalRows
                    If roleCount < userCount Then
                        LogMessageRoles1 &= " Some user have access to create user ID via direct profile assignment"
                    End If
                    Dim i As Integer = 0, j As Integer = visibleRows
                    Do While ExecutionTime > 0
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SelectedRows = i
                        Threading.Thread.Sleep(waitTime)
                        Takescreenshot(WordDoc, stepNum)            ' Step 8
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
                    Dim fileNameWithExt As String = $"{controlITGC} {systemname} User's role with Ability to create user ID export.xls"
                    Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                    session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
                End If
            Catch ex As Exception
                Logger.LogMessage("Error during Checkpoint 1.1: " & ex.Message, False)
                Logger.LogException(ex, "user have access to create user ID via direct profile assignment")
                stepNum = 400
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 400
                AddDirectProfileNote(WordDoc, "Ability to create user ID")
            End Try

            session.findById("wnd[0]/tbar[0]/okcd").Text = "/n"
            session.findById("wnd[0]").sendVKey(0)

            stepNum = 9
            ' Checkpoint 2 - Ability to lock and unlock user ID
            '-----------------------------------------------------------------------------------------------------------------
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").text = "SUIM"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 9
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("02  1      2")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("03  2      7")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").selectItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ensureVisibleHorizontalItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 10
            stepNum += 1
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").clickLink("04  2      8", "1")
            session.findById("wnd[0]/usr/btn%_USER_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpNOSV").select

            ' ---- Clipboard method for ApprovedUsers2 ----
            CopyUsersToClipboard(My.Settings.ITGC10_ApprovedUserscheck2)
            session.findById("wnd[1]/tbar[0]/btn[24]").press()     ' Upload from clipboard
            Threading.Thread.Sleep(500)
            session.findById("wnd[1]/tbar[0]/btn[8]").press()      ' Confirm
            Threading.Thread.Sleep(waitTime)
            ' ---------------------------------------------

            Takescreenshot(WordDoc, stepNum)                        ' Step 11
            stepNum += 1

            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").caretPosition = 0
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/btn%_UTYPE_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").text = "A"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").text = "S"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").setFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 1
            session.findById("wnd[0]").sendVKey(0)
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 12
            stepNum += 1
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4").select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").text = "S_TCODE"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").caretPosition = 7
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL101").text = "SU01"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL102").text = "SU10"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").text = "S_USER_GRP"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").caretPosition = 10
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL201").text = "*"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").text = "02"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL212").text = "05"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL212").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL212").caretPosition = 3
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr").VerticalScrollbar.Position = 700
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 13
            stepNum += 1

            session.findById("wnd[0]/tbar[1]/btn[8]").press
            Dim LogMessage2 As String = ""
            Try
                LogMessage2 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC10")
            End Try

            If LogMessage2.Contains("No matching user found") Then
                stepNum = 403
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 403
            Else
                LogMessage2 = "User found with Ability to lock and unlock user ID"
                Try
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                Catch ex As Exception
                    Logger.LogMessage("Error setting row size: " & ex.Message, False)
                End Try

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                userCount = totalRows
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim i As Integer = 0, j As Integer = visibleRows
                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 14
                    If j < totalRows Then
                        Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell")
                        grid.FirstVisibleRow += visibleRows
                        j += visibleRows
                    End If
                    ExecutionTime -= 1
                    i += visibleRows
                Loop
                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} User with Ability to lock and unlock user ID.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If

            '======== Checkpoint 2.1 - Roles with Ability to lock and unlock user ID ========
            Try
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").currentCellRow = -1
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectColumn("BNAME")
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectedRows = ""
                stepNum = 15
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 15
                stepNum += 1
                session.findById("wnd[0]/tbar[1]/btn[23]").press
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell").selectedRows = "0"

                Dim LogMessageRoles2 As String = ""
                Try
                    LogMessageRoles2 = session.FindById("wnd[0]/sbar").Text
                Catch ex As Exception
                    Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                    Logger.LogException(ex, "Error retrieving log message in ITGC10")
                End Try

                If LogMessageRoles2.Contains("No matching user found") Then
                    stepNum = 404
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 404
                    ' ---- Direct profile assignment note ----
                    AddDirectProfileNote(WordDoc, "Ability to lock and unlock user ID")
                    LogMessageRoles2 = "No role found - user(s) may have access to lock/unlock user ID via direct profile assignment"
                    Logger.LogMessage("Check 2.1: No role found - direct profile assignment suspected", False)
                    ' ----------------------------------------
                Else
                    LogMessageRoles2 = "Role found with Ability to lock and unlock user ID"
                    Try
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                    Catch ex As Exception
                        Logger.LogMessage("Error setting row size: " & ex.Message, False)
                    End Try

                    Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").RowCount
                    Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").VisibleRowCount
                    Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                    roleCount = totalRows
                    Dim i As Integer = 0, j As Integer = visibleRows
                    Do While ExecutionTime > 0
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SelectedRows = i
                        Threading.Thread.Sleep(waitTime)
                        Takescreenshot(WordDoc, stepNum)            ' Step 16
                        If j < totalRows Then
                            Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")
                            grid.FirstVisibleRow += visibleRows
                            j += visibleRows
                        End If
                        ExecutionTime -= 1
                        i += visibleRows
                    Loop
                    session.FindById("wnd[0]").SendVKey(45)
                    session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                    session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                    session.findById("wnd[1]/tbar[0]/btn[0]").press
                    session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                    Dim fileNameWithExt As String = $"{controlITGC} {systemname} User's role with Ability to lock and unlock user ID export.xls"
                    Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                    session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
                End If
                If roleCount < userCount Then
                    LogMessageRoles2 &= " Some user have access to lock/unlock user ID via direct profile assignment"
                End If
            Catch ex As Exception
                Logger.LogMessage("Error during Checkpoint 2.1: " & ex.Message, False)
                Logger.LogException(ex, "user have access to lock/unlock user ID via direct profile assignment")
                stepNum = 400
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 400
                AddDirectProfileNote(WordDoc, "Ability to lock and unlock user ID")
            End Try

            session.findById("wnd[0]/tbar[0]/okcd").Text = "/n"
            session.findById("wnd[0]").sendVKey(0)

            stepNum = 17
            ' Checkpoint 3 - Ability to modify roles in production environment
            '-----------------------------------------------------------------------------------------------------------------
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").text = "SUIM"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 17
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("02  1      2")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("03  2      7")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").selectItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ensureVisibleHorizontalItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 18
            stepNum += 1
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").clickLink("04  2      8", "1")
            session.findById("wnd[0]/usr/btn%_USER_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpNOSV").select

            ' ---- Clipboard method for ApprovedUsers3 ----
            CopyUsersToClipboard(My.Settings.ITGC10_ApprovedUserscheck3)
            session.findById("wnd[1]/tbar[0]/btn[24]").press()     ' Upload from clipboard
            Threading.Thread.Sleep(500)
            session.findById("wnd[1]/tbar[0]/btn[8]").press()      ' Confirm
            Threading.Thread.Sleep(waitTime)
            ' ---------------------------------------------

            Takescreenshot(WordDoc, stepNum)                        ' Step 19
            stepNum += 1

            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").caretPosition = 0
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/btn%_UTYPE_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").text = "A"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").text = "S"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").setFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 1
            session.findById("wnd[0]").sendVKey(0)
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 20
            stepNum += 1
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4").select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").text = "S_TCODE"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").caretPosition = 7
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL101").text = "PFCG"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").text = "S_USER_AGR"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").caretPosition = 10
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL201").text = "*"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").text = "01"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL212").text = "02"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL213").text = ""
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL212").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL212").caretPosition = 2
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr").VerticalScrollbar.Position = 700
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 21
            stepNum += 1

            session.findById("wnd[0]/tbar[1]/btn[8]").press
            Dim LogMessage3 As String = ""
            Try
                LogMessage3 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC10")
            End Try

            If LogMessage3.Contains("No matching user found") Then
                stepNum = 405
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 405
            Else
                LogMessage3 = "User found with Ability to modify roles in production environment"
                Try
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                Catch ex As Exception
                    Logger.LogMessage("Error setting row size: " & ex.Message, False)
                End Try

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                userCount = totalRows
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim i As Integer = 0, j As Integer = visibleRows
                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 22
                    If j < totalRows Then
                        Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell")
                        grid.FirstVisibleRow += visibleRows
                        j += visibleRows
                    End If
                    ExecutionTime -= 1
                    i += visibleRows
                Loop
                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} User with Ability to modify roles in production environment.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If

            Dim LogMessageRoles3 As String = ""
            '======== Checkpoint 3.1 - Roles with Ability to modify roles in production environment ========
            Try
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").currentCellRow = -1
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectColumn("BNAME")
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectedRows = ""
                stepNum = 23
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 23
                stepNum += 1
                session.findById("wnd[0]/tbar[1]/btn[23]").press
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell").selectedRows = "0"

                Try
                    LogMessageRoles3 = session.FindById("wnd[0]/sbar").Text
                Catch ex As Exception
                    Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                    Logger.LogException(ex, "Error retrieving log message in ITGC10")
                End Try

                If LogMessageRoles3.Contains("No matching user found") Then
                    stepNum = 406
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 406
                    ' ---- Direct profile assignment note ----
                    AddDirectProfileNote(WordDoc, "Ability to modify roles in production environment")
                    LogMessageRoles3 = "No role found - user(s) may have access to modify roles via direct profile assignment"
                    Logger.LogMessage("Check 3.1: No role found - direct profile assignment suspected", False)
                    ' ----------------------------------------
                Else
                    LogMessageRoles3 = "Role found with Ability to modify roles in production environment"
                    Try
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                    Catch ex As Exception
                        Logger.LogMessage("Error setting row size: " & ex.Message, False)
                    End Try

                    Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").RowCount
                    Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").VisibleRowCount
                    Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                    roleCount = totalRows
                    Dim i As Integer = 0, j As Integer = visibleRows
                    Do While ExecutionTime > 0
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SelectedRows = i
                        Threading.Thread.Sleep(waitTime)
                        Takescreenshot(WordDoc, stepNum)            ' Step 24
                        If j < totalRows Then
                            Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")
                            grid.FirstVisibleRow += visibleRows
                            j += visibleRows
                        End If
                        ExecutionTime -= 1
                        i += visibleRows
                    Loop
                    session.FindById("wnd[0]").SendVKey(45)
                    session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                    session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                    session.findById("wnd[1]/tbar[0]/btn[0]").press
                    session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                    Dim fileNameWithExt As String = $"{controlITGC} {systemname} User's role with Ability to modify roles in production environment.xls"
                    Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                    session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
                End If
                If roleCount < userCount Then
                    LogMessageRoles3 &= " Some user have access to modify roles via direct profile assignment"
                End If
            Catch ex As Exception
                Logger.LogMessage("Error during Checkpoint 3.1: " & ex.Message, False)
                Logger.LogException(ex, "user have access to modify roles via direct profile assignment")
                stepNum = 400
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 400
                AddDirectProfileNote(WordDoc, "Ability to modify roles in production environment")
            End Try

            session.findById("wnd[0]/tbar[0]/okcd").Text = "/n"
            session.findById("wnd[0]").sendVKey(0)

            stepNum = 25
            ' Checkpoint 4 - Ability to perform role assignments to users
            '-----------------------------------------------------------------------------------------------------------------
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").text = "SUIM"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 25
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("02  1      2")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("03  2      7")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").selectItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ensureVisibleHorizontalItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 26
            stepNum += 1
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").clickLink("04  2      8", "1")
            session.findById("wnd[0]/usr/btn%_USER_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpNOSV").select

            ' ---- Clipboard method for ApprovedUsers4 ----
            CopyUsersToClipboard(My.Settings.ITGC10_ApprovedUserscheck4)
            session.findById("wnd[1]/tbar[0]/btn[24]").press()     ' Upload from clipboard
            Threading.Thread.Sleep(500)
            session.findById("wnd[1]/tbar[0]/btn[8]").press()      ' Confirm
            Threading.Thread.Sleep(waitTime)
            ' ---------------------------------------------

            Takescreenshot(WordDoc, stepNum)                        ' Step 27
            stepNum += 1

            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").caretPosition = 0
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/btn%_UTYPE_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").text = "A"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").text = "S"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").setFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 1
            session.findById("wnd[0]").sendVKey(0)
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 28
            stepNum += 1
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4").select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").text = "S_TCODE"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").caretPosition = 7
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL101").text = "SU01"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL102").text = "SU10"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").text = "S_USER_AGR"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").caretPosition = 10
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL201").text = "*"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").text = "22"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").caretPosition = 3
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr").VerticalScrollbar.Position = 700
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 29
            stepNum += 1

            session.findById("wnd[0]/tbar[1]/btn[8]").press
            Dim LogMessage4 As String = ""
            Try
                LogMessage4 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC10")
            End Try

            If LogMessage4.Contains("No matching user found") Then
                stepNum = 407
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 407
            Else
                LogMessage4 = "User found with Ability to perform role assignments to users"
                Try
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                Catch ex As Exception
                    Logger.LogMessage("Error setting row size: " & ex.Message, False)
                End Try

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                userCount = totalRows
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim i As Integer = 0, j As Integer = visibleRows
                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 30
                    If j < totalRows Then
                        Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell")
                        grid.FirstVisibleRow += visibleRows
                        j += visibleRows
                    End If
                    ExecutionTime -= 1
                    i += visibleRows
                Loop
                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} User with Ability to perform role assignments to users.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If

            '======== Checkpoint 4.1 - Roles with Ability to perform role assignments to users ========
            Try
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").currentCellRow = -1
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectColumn("BNAME")
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectedRows = ""
                stepNum = 31
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 31
                stepNum += 1
                session.findById("wnd[0]/tbar[1]/btn[23]").press
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell").selectedRows = "0"

                Dim LogMessageRoles4 As String = ""
                Try
                    LogMessageRoles4 = session.FindById("wnd[0]/sbar").Text
                Catch ex As Exception
                    Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                    Logger.LogException(ex, "Error retrieving log message in ITGC10")
                End Try

                If LogMessageRoles4.Contains("No matching user found") Then
                    stepNum = 408
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 408
                    ' ---- Direct profile assignment note ----
                    AddDirectProfileNote(WordDoc, "Ability to perform role assignments to users")
                    LogMessageRoles4 = "No role found - user(s) may have access to perform role assignments via direct profile assignment"
                    Logger.LogMessage("Check 4.1: No role found - direct profile assignment suspected", False)
                    ' ----------------------------------------
                Else
                    LogMessageRoles4 = "Role found with Ability to perform role assignments to users"
                    Try
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                    Catch ex As Exception
                        Logger.LogMessage("Error setting row size: " & ex.Message, False)
                    End Try

                    Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").RowCount
                    Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").VisibleRowCount
                    Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                    roleCount = totalRows
                    Dim i As Integer = 0, j As Integer = visibleRows
                    Do While ExecutionTime > 0
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SelectedRows = i
                        Threading.Thread.Sleep(waitTime)
                        Takescreenshot(WordDoc, stepNum)            ' Step 32
                        If j < totalRows Then
                            Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")
                            grid.FirstVisibleRow += visibleRows
                            j += visibleRows
                        End If
                        ExecutionTime -= 1
                        i += visibleRows
                    Loop
                    session.FindById("wnd[0]").SendVKey(45)
                    session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                    session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                    session.findById("wnd[1]/tbar[0]/btn[0]").press
                    session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                    Dim fileNameWithExt As String = $"{controlITGC} {systemname} User's role with Ability to perform role assignments to users.xls"
                    Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                    session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
                End If
                If roleCount < userCount Then
                    LogMessageRoles4 &= " Some user have access to perform role assignments via direct profile assignment"
                End If
            Catch ex As Exception
                Logger.LogMessage("Error during Checkpoint 4.1: " & ex.Message, False)
                Logger.LogException(ex, "user have access to perform role assignments via direct profile assignment")
                stepNum = 400
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 400
                AddDirectProfileNote(WordDoc, "Ability to perform role assignments to users")
            End Try

            session.findById("wnd[0]/tbar[0]/okcd").Text = "/n"
            session.findById("wnd[0]").sendVKey(0)

            stepNum = 33
            ' Checkpoint 5 - Ability to SAP_ALL and SAP_NEW Profile Assignment
            '-----------------------------------------------------------------------------------------------------------------
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").text = "SUIM"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 33
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("02  1      2")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").expandNode("03  2      7")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").selectItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").ensureVisibleHorizontalItem("04  2      8", "1")
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").topNode = ("01  1      1")
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 34
            stepNum += 1
            session.findById("wnd[0]/usr/cntlTREE_CONTROL_CONTAINER/shellcont/shell").clickLink("04  2      8", "1")
            session.findById("wnd[0]/usr/btn%_USER_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpNOSV").select

            ' ---- Clipboard method for ApprovedUsers5 ----
            CopyUsersToClipboard(My.Settings.ITGC10_ApprovedUserscheck5)
            session.findById("wnd[1]/tbar[0]/btn[24]").press()     ' Upload from clipboard
            Threading.Thread.Sleep(500)
            session.findById("wnd[1]/tbar[0]/btn[8]").press()      ' Confirm
            Threading.Thread.Sleep(waitTime)
            ' ---------------------------------------------

            Takescreenshot(WordDoc, stepNum)                        ' Step 35
            stepNum += 1

            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/ctxtUTYPE-LOW").caretPosition = 0
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB1/ssub%_SUBSCREEN_TAB:RSUSR002:1001/btn%_UTYPE_%_APP_%-VALU_PUSH").press
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,0]").text = "A"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").text = "S"
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").setFocus
            session.findById("wnd[1]/usr/tabsTAB_STRIP/tabpSIVA/ssubSCREEN_HEADER:SAPLALDB:3010/tblSAPLALDBSINGLE/ctxtRSCSEL_255-SLOW_I[1,1]").caretPosition = 1
            session.findById("wnd[0]").sendVKey(0)
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 36
            stepNum += 1
            session.findById("wnd[1]/tbar[0]/btn[8]").press
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4").select
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").text = "S_TCODE"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ1").caretPosition = 7
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL101").text = "SU01"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL102").text = "SU10"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").text = "S_USER_PRO"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/ctxtOBJ2").caretPosition = 10
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL201").text = "SAP_ALL"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL202").text = "SAP_NEW"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").text = "22"
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").setFocus
            session.findById("wnd[0]/usr/tabsTABSTRIP_TAB/tabpTAB4/ssub%_SUBSCREEN_TAB:RSUSR002:1004/txtVAL211").caretPosition = 3
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr").VerticalScrollbar.Position = 700
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 37
            stepNum += 1

            session.findById("wnd[0]/tbar[1]/btn[8]").press
            Dim LogMessage5 As String = ""
            Try
                LogMessage5 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC10")
            End Try

            If LogMessage5.Contains("No matching user found") Then
                stepNum = 409
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 409
            Else
                LogMessage5 = "User found with Ability to SAP_ALL and SAP_NEW Profile Assignment."
                Try
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                Catch ex As Exception
                    Logger.LogMessage("Error setting row size: " & ex.Message, False)
                End Try

                Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").RowCount
                Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").VisibleRowCount
                userCount = totalRows
                Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                Dim i As Integer = 0, j As Integer = visibleRows
                Do While ExecutionTime > 0
                    session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").SelectedRows = i
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 38
                    If j < totalRows Then
                        Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell")
                        grid.FirstVisibleRow += visibleRows
                        j += visibleRows
                    End If
                    ExecutionTime -= 1
                    i += visibleRows
                Loop
                session.FindById("wnd[0]").SendVKey(45)
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                session.findById("wnd[1]/tbar[0]/btn[0]").press
                session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} User with Ability to SAP_ALL and SAP_NEW Profile Assignment.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If

            '======== Checkpoint 5.1 - Roles with Ability to SAP_ALL and SAP_NEW Profile Assignment ========
            Try
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").currentCellRow = -1
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectColumn("BNAME")
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell/shellcont[1]/shell").selectedRows = ""
                stepNum = 39
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 39
                stepNum += 1
                session.findById("wnd[0]/tbar[1]/btn[23]").press
                session.findById("wnd[0]/usr/cntlGRID1/shellcont/shell").selectedRows = "0"

                Dim LogMessageRoles5 As String = ""
                Try
                    LogMessageRoles5 = session.FindById("wnd[0]/sbar").Text
                Catch ex As Exception
                    Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                    Logger.LogException(ex, "Error retrieving log message in ITGC10")
                End Try

                If LogMessageRoles5.Contains("No matching user found") Then
                    stepNum = 410
                    Threading.Thread.Sleep(waitTime)
                    Takescreenshot(WordDoc, stepNum)                ' Step 410
                    ' ---- Direct profile assignment note ----
                    AddDirectProfileNote(WordDoc, "Ability to SAP_ALL and SAP_NEW Profile Assignment")
                    LogMessageRoles5 = "No role found - user(s) may have access to SAP_ALL/SAP_NEW via direct profile assignment"
                    Logger.LogMessage("Check 5.1: No role found - direct profile assignment suspected", False)
                    ' ----------------------------------------
                Else
                    LogMessageRoles5 = "Role found with Ability to SAP_ALL and SAP_NEW Profile Assignment."
                    Try
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(1, 251)
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SetRowSize(2, -1)
                    Catch ex As Exception
                        Logger.LogMessage("Error setting row size: " & ex.Message, False)
                    End Try

                    Dim totalRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").RowCount
                    Dim visibleRows As Long = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").VisibleRowCount
                    Dim ExecutionTime As Long = Math.Ceiling(totalRows / visibleRows)
                    roleCount = totalRows
                    Dim i As Integer = 0, j As Integer = visibleRows
                    Do While ExecutionTime > 0
                        session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell").SelectedRows = i
                        Threading.Thread.Sleep(waitTime)
                        Takescreenshot(WordDoc, stepNum)            ' Step 40
                        If j < totalRows Then
                            Dim grid = session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")
                            grid.FirstVisibleRow += visibleRows
                            j += visibleRows
                        End If
                        ExecutionTime -= 1
                        i += visibleRows
                    Loop
                    session.FindById("wnd[0]").SendVKey(45)
                    session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").select
                    session.findById("wnd[1]/usr/subSUBSCREEN_STEPLOOP:SAPLSPO5:0150/sub:SAPLSPO5:0150/radSPOPLI-SELFLAG[1,0]").setFocus
                    session.findById("wnd[1]/tbar[0]/btn[0]").press
                    session.FindById("wnd[1]/usr/ctxtDY_PATH").Text = folderPath
                    Dim fileNameWithExt As String = $"{controlITGC} {systemname} User's role with Ability to SAP_ALL and SAP_NEW Profile Assignment.xls"
                    Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                    session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                    session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
                End If
                If roleCount < userCount Then
                    LogMessageRoles5 &= " Some user have access to SAP_ALL/SAP_NEW via direct profile assignment"
                End If
            Catch ex As Exception
                Logger.LogMessage("Error during Checkpoint 5.1: " & ex.Message, False)
                Logger.LogException(ex, "user have access to SAP_ALL/SAP_NEW via direct profile assignment")
                stepNum = 400
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 400
                AddDirectProfileNote(WordDoc, "Ability to SAP_ALL and SAP_NEW Profile Assignment")
            End Try

            ' =============================== Report gathering ====================================
            Logger.LogMessage("Gathering report for " & controlID, True)

            Dim ReprtOutputPath As String
            If Not String.IsNullOrEmpty(ReportMonth) Then
                Dim downloadsPath As String = Path.Combine(My.Settings.DownloadDestination, "ITGC REPORT")
                ReprtOutputPath = downloadsPath
            Else
                ReprtOutputPath = String.Empty
            End If

            If (LogMessage1.Contains("No matching user found") _
                Or LogMessage2.Contains("No matching user found") _
                Or LogMessage3.Contains("No matching user found") _
                Or LogMessage4.Contains("No matching user found") _
                Or LogMessage5.Contains("No matching user found")) Then
                ' GREEN ZONE
                SaveITGCComment(
                    controlID:=controlITGC,
                    description:=controlName,
                    systemName:=systemname,
                    isGreen:=True,
                    comment:=$"[Check 1] - {LogMessage1} | [Check 2] - {LogMessage2} | [Check 3] - {LogMessage3} | [Check 4] - {LogMessage4} | [Check 5] - {LogMessage5}",
                    baseFolder:=ReprtOutputPath,
                    forMonth:=ReportMonth)
            Else
                ' RED ZONE
                SaveITGCComment(
                    controlID:=controlITGC,
                    description:=controlName,
                    systemName:=systemname,
                    isGreen:=False,
                    comment:=$"[Check 1] - {LogMessage1} | [Check 2] - {LogMessage2} | [Check 3] - {LogMessage3} | [Check 4] - {LogMessage4} | [Check 5] - {LogMessage5}",
                    baseFolder:=ReprtOutputPath,
                    forMonth:=ReportMonth)
            End If
            ' =============================== End of Report gathering ====================================

            session.FindById("wnd[0]/tbar[0]/okcd").Text = "/nex"
            session.FindById("wnd[0]").SendVKey(0)
            DisconnectFromSAP()

            ' Save and close the Word document
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