Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop

Module ITGC02
    ' Subroutine to check High level of Privilege Access Control
    Sub ExecuteITGC02()
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
            Logger.LogMessage("ITGC02 Execution Started!", True)
            ' Maximize the SAP window and perform actions
            session.findById("wnd[0]").maximize
            session.findById("wnd[0]/tbar[0]/okcd").text = "/nSCC4"
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number
            stepNum += 1                                            ' Increase the step number
            session.findById("wnd[0]/tbar[0]/btn[0]").press
            Try
                System.Windows.Forms.SendKeys.SendWait("%M") ' % = Alt, M = Utilities
            Catch
                System.Windows.Forms.SendKeys.SendWait("%S") ' % = Alt, S = Utilities
            End Try

            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number
            stepNum += 1                                            ' Increase the step number
            session.findById("wnd[0]/mbar/menu[4]/menu[2]").select
            session.findById("wnd[0]/usr/ctxtDBEG").text = reportStartDate
            session.findById("wnd[0]/usr/ctxtTBEG").text = My.Settings.Report_FTIME ' Setting based report Start time
            session.findById("wnd[0]/usr/ctxtDEND").text = reportEndDate
            session.findById("wnd[0]/usr/ctxtDEND").setFocus
            session.findById("wnd[0]/usr/ctxtDEND").caretPosition = 10
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/usr/ctxtTEND").text = My.Settings.Report_TTIME ' Setting based report end time
            session.findById("wnd[0]/usr/ctxtTEND").setFocus
            session.findById("wnd[0]/usr/ctxtTEND").caretPosition = 8
            Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
            Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number
            stepNum += 1                                            ' Increase the step number
            session.findById("wnd[0]").sendVKey(0)
            session.findById("wnd[0]/tbar[1]/btn[8]").press


            Dim LogMessage1 As String = ""
            Try
                Dim labelObj = session.FindById("wnd[0]/usr/lbl[0,25]")
                If labelObj IsNot Nothing Then
                    LogMessage1 = labelObj.Text
                End If
            Catch ex As Exception
                Logger.LogMessage("Unable to read final status label: " & ex.Message, False)
            End Try

            If LogMessage1.Contains("No logs found for the selected period") Then

                stepNum = 401
                Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
                Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number
                'stepNum += 1                                            ' Increase the step number
            Else
                stepNum = 200
                Threading.Thread.Sleep(waitTime)                        ' Pause to ensure screen updates
                Takescreenshot(WordDoc, stepNum)                        ' Take a screenshot with step number
                'stepNum += 1                                            ' Increase the step number
            End If

            ' =============================== Code for Report gather ====================================
            '====REPORT STORING============================='
            Dim ReprtOutputPath As String

            If Not String.IsNullOrEmpty(ReportMonth) Then
                '            Dim downloadsPath As String = Path.Combine(
                'Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                '"Downloads", "ITGC REPORT")
                Dim downloadsPath As String = Path.Combine(My.Settings.DownloadDestination, "ITGC REPORT")
                ReprtOutputPath = downloadsPath
            Else
                ReprtOutputPath = String.Empty
            End If
            '==============================================
            If LogMessage1.Contains("No logs found for the selected period") Then


                'GREEN ZONE
                SaveITGCComment(
                                 controlID:=controlITGC,
                                 description:=controlName,
                                 systemName:=systemname,
                                 isGreen:=True,
                                 comment:="No Client Opening Found for the selected period",
                                 baseFolder:=ReprtOutputPath,
                                 forMonth:=ReportMonth)
            Else
                ' RED ZONE
                SaveITGCComment(
                                controlID:=controlITGC,
                                description:=controlName,
                                systemName:=systemname,
                                isGreen:=False,
                                comment:="Client Opening Found for the selected period",
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
