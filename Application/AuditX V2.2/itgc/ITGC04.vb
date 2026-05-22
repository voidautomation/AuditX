Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop

Module ITGC04

    ' Helper function to set clipboard text in STA thread
    Private Sub SetClipboardText(text As String)
        Dim thread As New Threading.Thread(Sub()
                                               System.Windows.Forms.Clipboard.SetText(text)
                                           End Sub)
        thread.SetApartmentState(Threading.ApartmentState.STA)
        thread.Start()
        thread.Join()
    End Sub

    ' Helper function to build and copy values to clipboard
    Private Sub CopyValuesToClipboard(settingValue As String)
        Dim rawValues() As String = settingValue.Split(";"c)
        Dim valueList As New List(Of String)
        For Each value As String In rawValues
            Dim trimmed As String = value.Trim()
            If trimmed <> "" Then
                valueList.Add(trimmed)
            End If
        Next
        Dim clipboardText As String = String.Join(vbCrLf, valueList)
        SetClipboardText(clipboardText)
        Threading.Thread.Sleep(500)
    End Sub

    ' Subroutine to check critical transaction execution
    Sub ExecuteITGC04()
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
            Logger.LogMessage("ITGC04 Execution Started!", True)

            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").text = "/nSE16"
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 1
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/ctxtDATABROWSE-TABLENAME").text = "CDHDR"
            session.findById("wnd[0]/usr/ctxtDATABROWSE-TABLENAME").caretPosition = 5
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 2
            stepNum += 1
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/txtI4-LOW").text = ""
            session.findById("wnd[0]/usr/ctxtI5-LOW").text = reportStartDate
            session.findById("wnd[0]/usr/ctxtI5-HIGH").text = reportEndDate
            session.findById("wnd[0]/usr/ctxtI6-LOW").text = My.Settings.Report_FTIME
            session.findById("wnd[0]/usr/ctxtI6-HIGH").text = My.Settings.Report_TTIME
            session.findById("wnd[0]/usr/ctxtI7-LOW").setFocus
            session.findById("wnd[0]/usr/ctxtI7-LOW").caretPosition = 0
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)                        ' Step 3
            stepNum += 1
            session.findById("wnd[0]/usr/btn%_I7_%_APP_%-VALU_PUSH").press

            ' ---- Clipboard method for TCodes ----
            CopyValuesToClipboard(My.Settings.ITGC04_TCodeList)
            session.findById("wnd[1]/tbar[0]/btn[24]").press()     ' Upload from clipboard
            Threading.Thread.Sleep(500)
            session.findById("wnd[1]/tbar[0]/btn[8]").press()      ' Confirm
            Threading.Thread.Sleep(waitTime)
            ' -------------------------------------

            Takescreenshot(WordDoc, stepNum)                        ' Step 4
            stepNum += 1

            session.findById("wnd[0]/usr/txtMAX_SEL").text = ""
            session.findById("wnd[0]/usr/txtMAX_SEL").setFocus
            session.findById("wnd[0]/usr/txtMAX_SEL").caretPosition = 11
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/tbar[1]/btn[8]").press

            Dim LogMessage1 As String = ""
            Try
                LogMessage1 = session.FindById("wnd[0]/sbar").Text
            Catch ex As Exception
                Logger.LogMessage("Error retrieving log message: " & ex.Message, False)
                Logger.LogException(ex, "Error retrieving log message in ITGC04")
            End Try

            If LogMessage1.Contains("No table entries found for specified key") Then
                stepNum = 401
                Threading.Thread.Sleep(waitTime)
                Takescreenshot(WordDoc, stepNum)                    ' Step 401
            Else
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
                    Takescreenshot(WordDoc, stepNum)                ' Step 5+

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
                Dim fileNameWithExt As String = $"{controlITGC} {systemname} CDHDR Audit Export.xls"
                Dim exportPath As String = FileUtils.GetUniqueFileName(folderPath, fileNameWithExt)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").Text = Path.GetFileName(exportPath)
                session.FindById("wnd[1]/usr/ctxtDY_FILENAME").caretPosition = 5
                session.FindById("wnd[1]/tbar[0]/btn[0]").Press()
            End If

            ' =============================== Report gather ====================================
            Dim ReprtOutputPath As String
            If Not String.IsNullOrEmpty(ReportMonth) Then
                Dim downloadsPath As String = Path.Combine(
                    My.Settings.DownloadDestination,
                    "ITGC REPORT")
                ReprtOutputPath = downloadsPath
            Else
                ReprtOutputPath = String.Empty
            End If

            If LogMessage1.Contains("No table entries found for specified key") Then
                ' GREEN ZONE
                SaveITGCComment(
                    controlID:=controlITGC,
                    description:=controlName,
                    systemName:=systemname,
                    isGreen:=True,
                    comment:="No table entries found for specified key",
                    baseFolder:=ReprtOutputPath,
                    forMonth:=ReportMonth)
            Else
                ' RED ZONE
                SaveITGCComment(
                    controlID:=controlITGC,
                    description:=controlName,
                    systemName:=systemname,
                    isGreen:=False,
                    comment:="Table entries found for specified key",
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