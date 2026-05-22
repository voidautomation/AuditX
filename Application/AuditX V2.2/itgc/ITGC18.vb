Imports System.IO
Imports System.Linq
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Word
Imports Tesseract  ' From NuGet package
Imports DrawingImageFormat = System.Drawing.Imaging.ImageFormat

Module ITGC18

#Region "EXPECTED PARAMETERS"

    Private Enum ComparisonOp
        Equal
        LessThanOrEqual
        GreaterThanOrEqual
    End Enum

    Private Class ParameterRule
        Public Property Name As String
        Public Property ExpectedValue As Double
        Public Property Operator_ As ComparisonOp
        Public Property DisplayText As String
        ''' <summary>
        ''' Optional list of accepted string values (e.g., "0", "false", "off")
        ''' If populated, these are checked directly BEFORE numeric comparison.
        ''' Used for parameters like sapgui/user_scripting that may return TRUE/FALSE or 1/0
        ''' </summary>
        Public Property AcceptedStringValues As List(Of String)
    End Class

    Private Function GetExpectedParameters() As List(Of ParameterRule)
        Dim list As New List(Of ParameterRule)

        list.Add(New ParameterRule With {
            .Name = "login/min_password_lng",
            .ExpectedValue = 8,
            .Operator_ = ComparisonOp.GreaterThanOrEqual,
            .DisplayText = ">= 8"
        })

        list.Add(New ParameterRule With {
            .Name = "login/password_expiration_time",
            .ExpectedValue = 90,
            .Operator_ = ComparisonOp.LessThanOrEqual,
            .DisplayText = "<= 90"
        })

        list.Add(New ParameterRule With {
            .Name = "login/fails_to_user_lock",
            .ExpectedValue = 5,
            .Operator_ = ComparisonOp.LessThanOrEqual,
            .DisplayText = "<= 5"
        })

        list.Add(New ParameterRule With {
            .Name = "login/no_automatic_user_sapstar",
            .ExpectedValue = 1,
            .Operator_ = ComparisonOp.Equal,
            .DisplayText = "= 1"
        })

        list.Add(New ParameterRule With {
            .Name = "rdisp/gui_auto_logout",
            .ExpectedValue = 900,
            .Operator_ = ComparisonOp.LessThanOrEqual,
            .DisplayText = "<= 900"
        })

        list.Add(New ParameterRule With {
            .Name = "auth/rfc_authority_check",
            .ExpectedValue = 1,
            .Operator_ = ComparisonOp.Equal,
            .DisplayText = "= 1"
        })

        list.Add(New ParameterRule With {
            .Name = "rsau/enable",
            .ExpectedValue = 1,
            .Operator_ = ComparisonOp.Equal,
            .DisplayText = "= 1"
        })

        ' ============================================================
        ' FIX: sapgui/user_scripting accepts "0", "FALSE", "false"
        ' SAP may return TRUE/FALSE or 1/0 depending on version/config
        ' Expected = disabled (0 / FALSE) for security compliance
        ' ============================================================
        list.Add(New ParameterRule With {
            .Name = "sapgui/user_scripting",
            .ExpectedValue = 0,
            .Operator_ = ComparisonOp.Equal,
            .DisplayText = "= 0 (FALSE)",
            .AcceptedStringValues = New List(Of String) From {
                "0",
                "false",
                "off",
                "no"
            }
        })

        Return list
    End Function

    Private Class ParameterResult
        Public Property Name As String
        Public Property ExpectedDisplay As String
        Public Property CurrentValue As String
        Public Property IsCompliant As Boolean
    End Class

#End Region

#Region "OCR HELPERS"

    ''' <summary>
    ''' Captures the SAP window screenshot
    ''' </summary>
    Private Function CaptureSAPWindow() As Bitmap
        Try
            ' Get SAP window handle - find by window title
            Dim sapHwnd As IntPtr = FindSAPWindow()
            If sapHwnd = IntPtr.Zero Then
                Logger.LogMessage("SAP window not found", False)
                Return Nothing
            End If

            ' Get window dimensions
            Dim rect As RECT
            GetWindowRect(sapHwnd, rect)
            Dim width As Integer = rect.Right - rect.Left
            Dim height As Integer = rect.Bottom - rect.Top

            ' Create bitmap and capture
            Dim bmp As New Bitmap(width, height)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0, New Size(width, height))
            End Using

            Return bmp
        Catch ex As Exception
            Logger.LogMessage("Screenshot capture failed: " & ex.Message, False)
            Return Nothing
        End Try
    End Function

    ' Win32 API declarations for screenshot
    <StructLayout(LayoutKind.Sequential)>
    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    <DllImport("user32.dll")>
    Private Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Function GetWindowRect(hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function FindWindowEx(parentHandle As IntPtr, childAfter As IntPtr, lclassName As String, windowTitle As String) As IntPtr
    End Function

    Private Function FindSAPWindow() As IntPtr
        ' Try common SAP window class names
        Dim hwnd As IntPtr = FindWindow("SAP_FRONTEND_SESSION", Nothing)
        If hwnd <> IntPtr.Zero Then Return hwnd

        ' Fallback - try by partial title
        Try
            For Each proc In Process.GetProcesses()
                If proc.ProcessName.ToLower().Contains("saplogon") OrElse
                   proc.ProcessName.ToLower().Contains("sapgui") Then
                    If proc.MainWindowHandle <> IntPtr.Zero Then
                        Return proc.MainWindowHandle
                    End If
                End If
            Next
        Catch
        End Try

        Return IntPtr.Zero
    End Function

    ''' <summary>
    ''' Reads Current Value using OCR by capturing the screen and parsing
    ''' </summary>
    Private Function ReadCurrentValueViaOCR(parameterName As String) As String
        Dim currentValue As String = ""

        Try
            ' Take screenshot of SAP window
            Dim bmp As Bitmap = CaptureSAPWindow()
            If bmp Is Nothing Then
                Logger.LogMessage("Failed to capture SAP window", False)
                Return ""
            End If

            ' Save screenshot for debugging
            Dim safeName As String = parameterName.Replace("/", "_")
            Dim screenshotPath As String = Path.Combine(Path.GetTempPath(), $"ocr_{safeName}.png")
            bmp.Save(screenshotPath, System.Drawing.Imaging.ImageFormat.Png)
            Logger.LogMessage($"Screenshot saved: {screenshotPath}", True)

            ' Find tessdata folder
            Dim tessDataPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata")
            If Not Directory.Exists(tessDataPath) Then
                ' Try alternative locations
                tessDataPath = Path.Combine(My.Application.Info.DirectoryPath, "tessdata")
                If Not Directory.Exists(tessDataPath) Then
                    Logger.LogMessage("tessdata folder not found. Please ensure Tesseract is installed correctly.", False)
                    Return ""
                End If
            End If

            ' Run OCR
            Dim ocrText As String = ""
            Try
                Using engine As New TesseractEngine(tessDataPath, "eng", EngineMode.Default)
                    Using img = Pix.LoadFromFile(screenshotPath)
                        Using page = engine.Process(img)
                            ocrText = page.GetText()
                        End Using
                    End Using
                End Using
            Catch ocrEx As Exception
                Logger.LogMessage("OCR engine error: " & ocrEx.Message, False)
                Return ""
            End Try

            ' Save OCR text for debugging
            Dim ocrTextPath As String = Path.Combine(Path.GetTempPath(), $"ocr_text_{safeName}.txt")
            File.WriteAllText(ocrTextPath, ocrText)
            Logger.LogMessage($"OCR text saved: {ocrTextPath}", True)

            ' Parse OCR text to find Current Value
            currentValue = ParseCurrentValueFromOcr(ocrText)
            Logger.LogMessage($"OCR extracted current value: '{currentValue}'", True)

            ' Cleanup bitmap
            bmp.Dispose()

        Catch ex As Exception
            Logger.LogMessage("ReadCurrentValueViaOCR error: " & ex.Message, False)
        End Try

        Return currentValue
    End Function

    ''' <summary>
    ''' Enhanced parser - handles multi-line OCR output
    ''' </summary>
    Private Function ParseCurrentValueFromOcr(ocrText As String) As String
        Try
            If String.IsNullOrEmpty(ocrText) Then Return ""

            Dim lines() As String = ocrText.Split({vbCrLf, vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries)

            Dim foundValue As String = ""
            Dim foundCurrentValueRow As Boolean = False

            ' First pass - find Current Value row in the bottom table
            ' The bottom table has rows: Kernel Default, Standard Profile, Instance Profile, Current Value
            ' We track when we see "Kernel Default" to know we're in the right table

            Dim inValueTable As Boolean = False

            For i As Integer = 0 To lines.Length - 1
                Dim trimmedLine As String = lines(i).Trim()
                Dim lowerLine As String = trimmedLine.ToLower()

                ' Detect entry into the value table
                If lowerLine.Contains("kernel default") OrElse lowerLine.Contains("expansion level") Then
                    inValueTable = True
                End If

                ' Skip section header
                If lowerLine.Contains("current value of parameter") Then
                    Continue For
                End If

                ' Look for the Current Value row
                If lowerLine.StartsWith("current value") AndAlso Not lowerLine.Contains("of parameter") Then
                    ' Extract value from same line
                    Dim idx As Integer = lowerLine.IndexOf("current value")
                    Dim afterText As String = trimmedLine.Substring(idx + "current value".Length).Trim()
                    afterText = afterText.TrimStart(":"c, "|"c, " "c, vbTab)

                    If Not String.IsNullOrEmpty(afterText) Then
                        Dim parts() As String = afterText.Split({" "c, vbTab, "|"c}, StringSplitOptions.RemoveEmptyEntries)
                        If parts.Length > 0 Then
                            foundValue = parts(0).Trim()
                            Logger.LogMessage($"Found Current Value on same line: '{foundValue}'", True)
                        End If
                    Else
                        ' Value might be on the next line
                        If i + 1 < lines.Length Then
                            Dim nextLine As String = lines(i + 1).Trim()
                            If Not String.IsNullOrEmpty(nextLine) Then
                                Dim parts() As String = nextLine.Split({" "c, vbTab}, StringSplitOptions.RemoveEmptyEntries)
                                If parts.Length > 0 Then
                                    foundValue = parts(0).Trim()
                                    Logger.LogMessage($"Found Current Value on next line: '{foundValue}'", True)
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            Return foundValue

        Catch ex As Exception
            Logger.LogMessage("ParseCurrentValueFromOcr error: " & ex.Message, False)
        End Try

        Return ""
    End Function

#End Region

#Region "VALIDATION HELPERS"

    Private Function ValidateValue(currentVal As String, rule As ParameterRule) As Boolean
        Try
            If String.IsNullOrWhiteSpace(currentVal) Then Return False

            Dim trimmedVal As String = currentVal.Trim()

            ' ----------------------------------------------------------------
            ' STEP 1: Check AcceptedStringValues list FIRST (case-insensitive)
            ' This handles sapgui/user_scripting = TRUE/FALSE/0/1 scenarios
            ' ----------------------------------------------------------------
            If rule.AcceptedStringValues IsNot Nothing AndAlso
               rule.AcceptedStringValues.Count > 0 Then

                Dim lowerVal As String = trimmedVal.ToLower()

                ' Check if value is in the accepted (compliant) list
                If rule.AcceptedStringValues.Contains(lowerVal) Then
                    Logger.LogMessage(
                        $"'{trimmedVal}' matched AcceptedStringValues for {rule.Name} → PASS",
                        True)
                    Return True
                End If

                ' If it's a known NON-compliant boolean string, fail early
                ' This catches cases like TRUE/1 when expected is FALSE/0
                Dim knownNonCompliantStrings As New List(Of String) From {
                    "1", "true", "on", "yes"
                }

                If rule.Operator_ = ComparisonOp.Equal AndAlso
                   knownNonCompliantStrings.Contains(lowerVal) Then
                    Logger.LogMessage(
                        $"'{trimmedVal}' is a known non-compliant boolean for {rule.Name} → FAIL",
                        True)
                    Return False
                End If
            End If

            ' ----------------------------------------------------------------
            ' STEP 2: Normalize boolean-like strings to numeric for comparison
            ' ----------------------------------------------------------------
            Dim normalizedVal As String = trimmedVal

            Select Case normalizedVal.ToUpper()
                Case "TRUE", "ON", "YES"
                    normalizedVal = "1"
                Case "FALSE", "OFF", "NO"
                    normalizedVal = "0"
            End Select

            ' ----------------------------------------------------------------
            ' STEP 3: Numeric comparison
            ' ----------------------------------------------------------------
            Dim numericValue As Double
            If Not Double.TryParse(normalizedVal, numericValue) Then
                ' Last resort: plain string equality against expected value
                Return String.Equals(
                    normalizedVal,
                    rule.ExpectedValue.ToString(),
                    StringComparison.OrdinalIgnoreCase)
            End If

            Select Case rule.Operator_
                Case ComparisonOp.Equal
                    Return numericValue = rule.ExpectedValue
                Case ComparisonOp.LessThanOrEqual
                    Return numericValue <= rule.ExpectedValue
                Case ComparisonOp.GreaterThanOrEqual
                    Return numericValue >= rule.ExpectedValue
                Case Else
                    Return False
            End Select

        Catch ex As Exception
            Logger.LogMessage("ValidateValue error: " & ex.Message, False)
            Return False
        End Try
    End Function

#End Region

#Region "MAIN EXECUTION"

    Sub ExecuteITGC18()

        Dim WordApp As Word.Application = Nothing
        Dim WordDoc As Word.Document = Nothing

        Try
            Logger.LogMessage("ITGC18 SAP Parameter Review Started", True)

            Dim executionMode As String = Home.cmbExecutionMode.SelectedItem?.ToString()
            If executionMode = "Foreground" Then
                MinimizeAllWindowsExceptSAP()
            End If

            WordApp = New Word.Application()
            WordApp.Visible = True
            WordDoc = WordApp.Documents.Add()

            Dim systemName As String = Home.lblSystemId.Text
            Dim controlID As String = Home.cmbControl.SelectedItem.ToString.Split("-"c)(0).Trim()
            Dim controlName As String = Home.lblDescription.Text
            Dim reportMonth As String = Home.txtReportMonth.Text
            Dim folderPath As String = Home.txtFolderPath.Text
            Dim controlITGC As String = Home.cmbControl.SelectedItem?.ToString().Split("-"c)(0).Trim()
            Dim fileName As String = $"{controlITGC} {systemName} Audit Report.docx"
            Dim stepNum As Integer = 1
            Dim waitTime As TimeSpan = TimeSpan.FromSeconds(1)

            With WordDoc.Paragraphs.Last.Range
                .Text = $"{controlID} - {controlName}{vbCrLf}System: {systemName}"
                .Font.Size = 11
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
            If SapGuiAuto Is Nothing Then Throw New Exception("SAP GUI is not running.")

            Dim expectedParams = GetExpectedParameters()
            Dim results As New List(Of ParameterResult)

            session.FindById("wnd[0]").maximize()
            session.FindById("wnd[0]/tbar[0]/okcd").Text = "RZ11"
            Logger.LogMessage("Navigated to RZ11 transaction", True)
            Threading.Thread.Sleep(waitTime)
            Takescreenshot(WordDoc, stepNum)
            stepNum += 1
            session.FindById("wnd[0]").SendVKey(0)

            Dim baseFolder As String = Path.Combine(My.Settings.DownloadDestination, "ITGC REPORT")

            For Each rule In expectedParams

                Dim parameterName As String = rule.Name
                Dim expectedDisplay As String = rule.DisplayText
                Dim currentValue As String = ""
                Dim isCompliant As Boolean = False

                Try
                    session.FindById("wnd[0]/usr/ctxtTPFYSTRUCT-NAME").Text = parameterName
                    session.FindById("wnd[0]").SendVKey(0)
                    Threading.Thread.Sleep(2000)  ' Wait for screen to load

                    ' ===== Read the Current Value via OCR =====
                    currentValue = ReadCurrentValueViaOCR(parameterName)

                    If String.IsNullOrEmpty(currentValue) Then
                        currentValue = "Not Read"
                        isCompliant = False
                    Else
                        isCompliant = ValidateValue(currentValue, rule)
                    End If

                    Logger.LogMessage($"Parameter: {parameterName} | Current: {currentValue} | Expected: {expectedDisplay} | Compliant: {isCompliant}", True)

                    Takescreenshot(WordDoc, stepNum)
                    stepNum += 1

                    With WordDoc.Paragraphs.Last.Range
                        .Text = $"Parameter: {parameterName}  |  Current Value: {currentValue}  |  Expected: {expectedDisplay}  |  Status: {If(isCompliant, "PASS", "FAIL")}"
                        .Font.Size = 10
                        .Font.Bold = True
                        .Font.Color = If(isCompliant, WdColor.wdColorDarkGreen, WdColor.wdColorDarkRed)
                        .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft
                        .InsertParagraphAfter()
                    End With

                    With WordDoc.Paragraphs.Last.Range
                        .Font.Size = 11
                        .Font.Bold = False
                        .Font.Color = WdColor.wdColorAutomatic
                    End With

                    results.Add(New ParameterResult With {
                        .Name = parameterName,
                        .ExpectedDisplay = expectedDisplay,
                        .CurrentValue = currentValue,
                        .IsCompliant = isCompliant
                    })

                    session.findById("wnd[0]/tbar[0]/btn[3]").press
                    Threading.Thread.Sleep(500)

                Catch paramEx As Exception
                    Logger.LogMessage($"Error processing parameter {parameterName}: {paramEx.Message}", False)

                    results.Add(New ParameterResult With {
                        .Name = parameterName,
                        .ExpectedDisplay = expectedDisplay,
                        .CurrentValue = "Error",
                        .IsCompliant = False
                    })

                    Try
                        session.findById("wnd[0]/tbar[0]/btn[3]").press
                        Threading.Thread.Sleep(500)
                    Catch
                        Try
                            session.findById("wnd[0]/tbar[0]/okcd").Text = "RZ11"
                            session.findById("wnd[0]").sendVKey(0)
                            Threading.Thread.Sleep(waitTime)
                        Catch
                        End Try
                    End Try
                End Try

            Next

            ' ================================================================
            ' Validation Summary Table
            ' ================================================================
            Try
                With WordDoc.Paragraphs.Last.Range
                    .Text = ""
                    .ParagraphFormat.SpaceBefore = 12
                    .InsertParagraphAfter()
                End With

                With WordDoc.Paragraphs.Last.Range
                    .Text = "Validation Summary - SAP Profile Parameters"
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

                With WordDoc.Paragraphs.Last.Range
                    .Font.Size = 11
                    .Font.Bold = False
                    .Font.Color = WdColor.wdColorAutomatic
                    .ParagraphFormat.Borders(WdBorderType.wdBorderBottom).LineStyle = WdLineStyle.wdLineStyleNone
                End With

                Dim tbl As Word.Table = WordDoc.Tables.Add(WordDoc.Paragraphs.Last.Range, results.Count + 1, 4)
                tbl.Borders.Enable = 1
                tbl.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle
                tbl.Borders.InsideLineWidth = WdLineWidth.wdLineWidth025pt
                tbl.Borders.InsideColor = WdColor.wdColorGray50
                tbl.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle
                tbl.Borders.OutsideLineWidth = WdLineWidth.wdLineWidth025pt
                tbl.Borders.OutsideColor = WdColor.wdColorGray50

                tbl.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent
                tbl.PreferredWidth = 100
                tbl.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow)

                ' Table Headers
                Dim headers() As String = {"Parameter Name", "Expected Value", "Current Value", "Status"}
                For i As Integer = 0 To headers.Length - 1
                    With tbl.Cell(1, i + 1).Range
                        .Text = headers(i)
                        .Font.Size = 10
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                        .ParagraphFormat.SpaceBefore = 0
                        .ParagraphFormat.SpaceAfter = 0
                    End With
                    tbl.Cell(1, i + 1).Shading.BackgroundPatternColor = WdColor.wdColorGray15
                    tbl.Cell(1, i + 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter
                Next

                ' Table Rows
                Dim rowIdx As Integer = 2
                For Each r In results
                    With tbl.Cell(rowIdx, 1).Range
                        .Text = r.Name
                        .Font.Size = 9
                        .Font.Bold = False
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft
                        .ParagraphFormat.SpaceBefore = 0
                        .ParagraphFormat.SpaceAfter = 0
                    End With

                    With tbl.Cell(rowIdx, 2).Range
                        .Text = r.ExpectedDisplay
                        .Font.Size = 9
                        .Font.Bold = False
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                        .ParagraphFormat.SpaceBefore = 0
                        .ParagraphFormat.SpaceAfter = 0
                    End With

                    With tbl.Cell(rowIdx, 3).Range
                        .Text = r.CurrentValue
                        .Font.Size = 9
                        .Font.Bold = True
                        .Font.Color = WdColor.wdColorBlack
                        .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                        .ParagraphFormat.SpaceBefore = 0
                        .ParagraphFormat.SpaceAfter = 0
                    End With

                    With tbl.Cell(rowIdx, 4).Range
                        If r.IsCompliant Then
                            .Text = "PASS"
                            .Font.Color = WdColor.wdColorDarkGreen
                        Else
                            .Text = "FAIL"
                            .Font.Color = WdColor.wdColorDarkRed
                        End If
                        .Font.Size = 10
                        .Font.Bold = True
                        .ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
                        .ParagraphFormat.SpaceBefore = 0
                        .ParagraphFormat.SpaceAfter = 0
                    End With

                    For c As Integer = 1 To 4
                        tbl.Cell(rowIdx, c).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter
                    Next

                    rowIdx += 1
                Next

                tbl.Rows.HeightRule = WdRowHeightRule.wdRowHeightAtLeast
                tbl.Rows.Height = WordApp.CentimetersToPoints(0.65)

                ' ============================================================
                ' Overall Result Summary
                ' ============================================================
                Dim totalParams As Integer = results.Count
                Dim passedParams As Integer = results.Where(Function(r) r.IsCompliant).Count()
                Dim failedParams As Integer = totalParams - passedParams
                Dim isOverallCompliant As Boolean = (failedParams = 0)

                With WordDoc.Paragraphs.Last.Range
                    .Text = ""
                    .ParagraphFormat.SpaceBefore = 8
                    .InsertParagraphAfter()
                End With

                With WordDoc.Paragraphs.Last.Range
                    .Text = "Overall Result:"
                    .Font.Size = 11
                    .Font.Bold = True
                    .Font.Color = WdColor.wdColorBlack
                    .ParagraphFormat.SpaceBefore = 6
                    .ParagraphFormat.SpaceAfter = 2
                    .InsertParagraphAfter()
                End With

                Dim overallText As String
                If isOverallCompliant Then
                    overallText = $"PASS - All {totalParams} parameters meet the expected criteria. No action required."
                Else
                    overallText = $"FAIL - {failedParams} of {totalParams} parameter(s) do not meet expected criteria. Review and remediation required."
                End If

                With WordDoc.Paragraphs.Last.Range
                    .Text = overallText
                    .Font.Size = 11
                    .Font.Bold = True
                    .Font.Color = If(isOverallCompliant, WdColor.wdColorDarkGreen, WdColor.wdColorDarkRed)
                    .ParagraphFormat.LeftIndent = WordApp.CentimetersToPoints(0.3)
                    .ParagraphFormat.SpaceAfter = 6
                    .InsertParagraphAfter()
                End With

                With WordDoc.Paragraphs.Last.Range
                    .Font.Size = 11
                    .Font.Bold = False
                    .Font.Color = WdColor.wdColorAutomatic
                    .ParagraphFormat.LeftIndent = 0
                End With

            Catch summaryEx As Exception
                Logger.LogMessage("Error adding summary table: " & summaryEx.Message, False)
            End Try

            ' ================================================================
            ' Finalize SAP Session
            ' ================================================================
            session.FindById("wnd[0]/tbar[0]/okcd").Text = "/nex"
            session.FindById("wnd[0]").SendVKey(0)
            DisconnectFromSAP()

            Logger.LogMessage("ITGC18 SAP Parameter Review Completed", True)

            ' ================================================================
            ' Save ITGC Comment
            ' ================================================================
            Dim ReprtOutputPath As String
            If Not String.IsNullOrEmpty(reportMonth) Then
                ReprtOutputPath = baseFolder
            Else
                ReprtOutputPath = String.Empty
            End If

            Dim totalP As Integer = results.Count
            Dim passedP As Integer = results.Where(Function(r) r.IsCompliant).Count()
            Dim failedP As Integer = totalP - passedP
            Dim isOverall As Boolean = (failedP = 0)

            Dim finalComment As String
            If isOverall Then
                finalComment = $"All {totalP} SAP parameters meet expected criteria. No action required."
            Else
                Dim failedList = String.Join(
                    " | ",
                    results.Where(Function(r) Not r.IsCompliant) _
                           .Select(Function(r) $"{r.Name}={r.CurrentValue} (expected {r.ExpectedDisplay})") _
                           .ToArray())
                finalComment = $"Validation FAILED. {failedP} of {totalP} parameters non-compliant: {failedList}"
            End If

            SaveITGCComment(
                controlID:=controlITGC,
                description:=controlName,
                systemName:=systemName,
                isGreen:=isOverall,
                comment:=finalComment,
                baseFolder:=ReprtOutputPath,
                forMonth:=reportMonth)

            Logger.LogMessage($"ITGC comment saved for {controlITGC} – {If(isOverall, "GREEN", "RED")}", True)

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

#End Region

End Module