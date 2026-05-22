Option Strict On
Option Explicit On

Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Word

Public Module WordReportGenerator

    ' =========================================================
    '  Helper – write a tick (✔) or cross (✘) into one cell
    ' =========================================================
    Private Sub SetMatrixCell(cell As Word.Cell, status As String)
        Dim rng As Word.Range = cell.Range
        rng.MoveEnd(WdUnits.wdCharacter, -1)

        Select Case status.ToUpper()
            Case "GREEN"
                rng.Text = ChrW(&H2714)
                rng.Font.Color = WdColor.wdColorGreen
                rng.Font.Bold = 1
                rng.Font.Size = 12
                rng.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter

            Case "RED"
                rng.Text = ChrW(&H2718)
                rng.Font.Color = WdColor.wdColorRed
                rng.Font.Bold = 1
                rng.Font.Size = 12
                rng.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter

            Case Else
                rng.Text = "–"
                rng.Font.Color = WdColor.wdColorGray50
                rng.Font.Bold = 0
                rng.Font.Size = 10
                rng.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
        End Select
    End Sub

    ' =========================================================
    '  Main entry point
    ' =========================================================
    Public Sub GenerateMonthlyReport(baseFolder As String,
                                     forMonth As String,
                                     coverImagePath As String,
                                     parentForm As Home)

        If String.IsNullOrWhiteSpace(forMonth) Then
            parentForm.lblNotification.Text = "Invalid month format"
            MessageBox.Show("Please enter a valid month like 'July, 2025'.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim monthlyFolder As String = Path.Combine(baseFolder, forMonth)
        Dim auditFilePath As String = Path.Combine(monthlyFolder,
                                        $"ITGC_Audit_Comments_{forMonth}.csv")
        Dim xmlPath As String = My.Settings.ITGCControl

        If Not File.Exists(auditFilePath) Then
            parentForm.lblNotification.ForeColor = Color.Red
            parentForm.lblNotification.Text = $"No data found for {forMonth}!"
            MessageBox.Show($"No data found for {forMonth}.",
                            "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim records = AuditStore.LoadMonthlyRecords(baseFolder, forMonth)
        If records.Count = 0 Then
            parentForm.lblNotification.ForeColor = Color.Red
            parentForm.lblNotification.Text = "No records found"
            MessageBox.Show($"No data found for {forMonth}.",
                            "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ITGCControlMetadataStore.LoadControlActions(xmlPath)

        Dim wordApp As Application = Nothing
        Dim doc As Document = Nothing
        parentForm.lblNotification.ForeColor = Color.Black
        parentForm.lblNotification.Text = $"Generating Report for {forMonth}..."

        Try
            wordApp = New Application()
            wordApp.Visible = True
            doc = wordApp.Documents.Add()

            ' --------------------------------------------------
            ' 1. Cover Image
            ' --------------------------------------------------
            parentForm.lblNotification.Text = "Inserting Cover Image..."
            If File.Exists(coverImagePath) Then
                Dim imgRange As Word.Range = doc.Paragraphs.Last.Range
                imgRange.ParagraphFormat.Alignment =
                    WdParagraphAlignment.wdAlignParagraphCenter
                doc.InlineShapes.AddPicture(coverImagePath,
                                            LinkToFile:=False,
                                            SaveWithDocument:=True)
                imgRange.InsertParagraphAfter()
            End If

            ' --------------------------------------------------
            ' 2. Report Title
            ' --------------------------------------------------
            parentForm.lblNotification.Text = "Adding Title and Metadata..."
            Dim titleRng As Word.Range = doc.Paragraphs.Last.Range
            titleRng.Text = $"Monthly ITGC Audit Report – {forMonth}" & vbCrLf
            titleRng.Font.Size = 16
            titleRng.Font.Bold = 1
            titleRng.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter
            titleRng.InsertParagraphAfter()

            ' --------------------------------------------------
            ' 3. Document Control / Metadata Table
            ' --------------------------------------------------
            parentForm.lblNotification.Text = "Adding Metadata Table..."
            Dim metaHeader As Word.Range = doc.Paragraphs.Last.Range
            metaHeader.InsertParagraphAfter()
            metaHeader = doc.Paragraphs.Last.Range
            metaHeader.Text = "📑 Document Control Information"
            metaHeader.Font.Size = 14
            metaHeader.Font.Bold = 1
            metaHeader.ParagraphFormat.Alignment =
                WdParagraphAlignment.wdAlignParagraphLeft
            metaHeader.InsertParagraphAfter()

            Dim metaTbl As Word.Table =
                doc.Tables.Add(doc.Bookmarks("\endofdoc").Range, 5, 2)
            metaTbl.Range.ParagraphFormat.SpaceAfter = 6
            metaTbl.Borders.Enable = 0
            metaTbl.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow)
            metaTbl.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent
            metaTbl.PreferredWidth = 100

            Dim metaLabels() As String =
                {"Client", "Prepared By", "Reviewed By", "Approved By"}
            Dim metaValues() As String =
                {"_________________", "_________________",
                 "_________________", "_________________"}

            With metaTbl.Cell(1, 1).Range
                .Text = "Report Generated"
                .Bold = 1
                .Font.Size = 11
            End With
            With metaTbl.Cell(1, 2).Range
                .Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                .Font.Size = 11
            End With

            For i As Integer = 0 To metaLabels.Length - 1
                With metaTbl.Cell(i + 2, 1).Range
                    .Text = metaLabels(i)
                    .Bold = 1
                    .Font.Size = 11
                End With
                With metaTbl.Cell(i + 2, 2).Range
                    .Text = metaValues(i)
                    .Font.Size = 11
                End With
            Next

            metaTbl.Rows.HeightRule = WdRowHeightRule.wdRowHeightAtLeast
            metaTbl.Rows.Height = wordApp.CentimetersToPoints(0.6)

            ' --------------------------------------------------
            ' 4. Summary Dashboard
            ' --------------------------------------------------
            parentForm.lblNotification.Text = "Adding Summary Dashboard..."
            Dim totalControls As Integer = records.Count
            Dim greenCount As Integer =
                records.Where(Function(r) r.Status.Equals("Green",
                              StringComparison.OrdinalIgnoreCase)).Count()
            Dim redCount As Integer =
                records.Where(Function(r) r.Status.Equals("Red",
                              StringComparison.OrdinalIgnoreCase)).Count()

            Dim dashHeader As Word.Range = doc.Paragraphs.Last.Range
            dashHeader.InsertParagraphAfter()
            dashHeader = doc.Paragraphs.Last.Range
            dashHeader.Text = "📊 Control Summary"
            dashHeader.Font.Size = 14
            dashHeader.Font.Bold = 1
            dashHeader.ParagraphFormat.Alignment =
                WdParagraphAlignment.wdAlignParagraphLeft
            dashHeader.InsertParagraphAfter()

            Dim dashTbl As Word.Table =
                doc.Tables.Add(doc.Bookmarks("\endofdoc").Range, 3, 2)
            dashTbl.Range.ParagraphFormat.SpaceAfter = 6
            dashTbl.Borders.Enable = 0
            dashTbl.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow)
            dashTbl.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent
            dashTbl.PreferredWidth = 100

            With dashTbl.Cell(1, 1).Range
                .Text = "Total Controls Checked"
                .Bold = 1
            End With
            dashTbl.Cell(1, 2).Range.Text = totalControls.ToString()

            With dashTbl.Cell(2, 1).Range
                .Text = "Green"
                .Bold = 1
            End With
            With dashTbl.Cell(2, 2).Range
                .Text = greenCount.ToString()
                .Font.Color = WdColor.wdColorGreen
            End With

            With dashTbl.Cell(3, 1).Range
                .Text = "Red"
                .Bold = 1
            End With
            With dashTbl.Cell(3, 2).Range
                .Text = redCount.ToString()
                .Font.Color = WdColor.wdColorRed
            End With

            ' --------------------------------------------------
            ' 5. System-wise Green / Red Count Table
            ' --------------------------------------------------
            parentForm.lblNotification.Text = "Adding System-wise Summary..."

            Dim sysHeaderRng As Word.Range = doc.Paragraphs.Last.Range
            sysHeaderRng.InsertParagraphAfter()
            sysHeaderRng = doc.Paragraphs.Last.Range
            sysHeaderRng.Text = "📊 System-wise Control Summary"
            sysHeaderRng.Font.Size = 14
            sysHeaderRng.Font.Bold = 1
            sysHeaderRng.ParagraphFormat.Alignment =
                WdParagraphAlignment.wdAlignParagraphLeft
            sysHeaderRng.InsertParagraphAfter()

            Dim systemGroups =
                records.GroupBy(Function(r) r.SystemName) _
                       .OrderBy(Function(g) g.Key).ToList()

            Dim sysTbl As Word.Table =
                doc.Tables.Add(doc.Bookmarks("\endofdoc").Range,
                               systemGroups.Count + 1, 4)
            sysTbl.Range.ParagraphFormat.SpaceAfter = 6
            sysTbl.Borders.Enable = 1
            sysTbl.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow)
            sysTbl.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPercent
            sysTbl.PreferredWidth = 100

            For Each pair In New Dictionary(Of Integer, String) From
                    {{1, "System"}, {2, "Total Controls"},
                     {3, "Green"}, {4, "Red"}}
                With sysTbl.Cell(1, pair.Key).Range
                    .Text = pair.Value
                    .Bold = 1
                    .Font.Size = 11
                    .Shading.BackgroundPatternColor = WdColor.wdColorGray25
                End With
            Next

            Dim sysRowIdx As Integer = 2
            For Each sysGroup In systemGroups
                Dim sysName As String = sysGroup.Key
                Dim sysTot As Integer = sysGroup.Count()
                Dim sysGr As Integer =
                    sysGroup.Where(Function(r) r.Status.Equals("Green",
                                   StringComparison.OrdinalIgnoreCase)).Count()
                Dim sysRd As Integer =
                    sysGroup.Where(Function(r) r.Status.Equals("Red",
                                   StringComparison.OrdinalIgnoreCase)).Count()

                With sysTbl.Cell(sysRowIdx, 1).Range
                    .Text = sysName
                    .Font.Size = 10
                    .Font.Bold = 0
                End With
                With sysTbl.Cell(sysRowIdx, 2).Range
                    .Text = sysTot.ToString()
                    .Font.Size = 10
                    .Font.Bold = 0
                End With
                With sysTbl.Cell(sysRowIdx, 3).Range
                    .Text = sysGr.ToString()
                    .Font.Size = 10
                    .Font.Bold = 1
                    .Font.Color = WdColor.wdColorGreen
                End With
                With sysTbl.Cell(sysRowIdx, 4).Range
                    .Text = sysRd.ToString()
                    .Font.Size = 10
                    .Font.Bold = 1
                    .Font.Color = WdColor.wdColorRed
                End With
                sysRowIdx += 1
            Next

            sysTbl.Rows.HeightRule = WdRowHeightRule.wdRowHeightAtLeast
            sysTbl.Rows.Height = wordApp.CentimetersToPoints(0.7)

            ' ==================================================
            ' 6.  ★ ITGC CONTROL MATRIX TABLE ★
            '     Column 1 = Control ID – Description (WIDE)
            '     Columns 2..N = Systems (NARROW, fit to content)
            ' ==================================================
            parentForm.lblNotification.Text = "Building ITGC Control Matrix..."

            Dim matrixTitle As Word.Range = doc.Paragraphs.Last.Range
            matrixTitle.InsertParagraphAfter()
            matrixTitle = doc.Paragraphs.Last.Range
            matrixTitle.Text = "📋 ITGC Control Matrix – Controls vs Systems"
            matrixTitle.Font.Size = 14
            matrixTitle.Font.Bold = 1
            matrixTitle.ParagraphFormat.Alignment =
                WdParagraphAlignment.wdAlignParagraphLeft
            matrixTitle.InsertParagraphAfter()

            ' Legend
            Dim legendRng As Word.Range = doc.Paragraphs.Last.Range
            legendRng.Text =
                ChrW(&H2714) & " = Green (Pass)    " &
                ChrW(&H2718) & " = Red (Fail)    – = Not Tested"
            legendRng.Font.Size = 9
            legendRng.Font.Bold = 0
            legendRng.Font.Color = WdColor.wdColorGray50
            legendRng.ParagraphFormat.Alignment =
                WdParagraphAlignment.wdAlignParagraphLeft
            legendRng.InsertParagraphAfter()

            ' Collect distinct sorted control IDs and system names
            Dim allControlIDs As List(Of String) =
                records.Select(Function(r) r.ControlID) _
                       .Distinct() _
                       .OrderBy(Function(id) id) _
                       .ToList()

            Dim allSystems As List(Of String) =
                records.Select(Function(r) r.SystemName) _
                       .Distinct() _
                       .OrderBy(Function(s) s) _
                       .ToList()

            ' Build lookup: ControlID → Description
            Dim descriptionLookup As New Dictionary(Of String, String)()
            For Each rec In records
                If Not descriptionLookup.ContainsKey(rec.ControlID) Then
                    descriptionLookup(rec.ControlID) = rec.Description
                End If
            Next

            ' Build lookup: (ControlID|SystemName) → status
            Dim statusLookup As New Dictionary(Of String, String)()
            For Each rec In records
                Dim key As String = rec.ControlID & "|" & rec.SystemName
                If Not statusLookup.ContainsKey(key) Then
                    statusLookup(key) = rec.Status
                Else
                    If rec.Status.Equals("Red", StringComparison.OrdinalIgnoreCase) Then
                        statusLookup(key) = "Red"
                    End If
                End If
            Next

            Dim matrixRows As Integer = allControlIDs.Count + 1
            Dim matrixCols As Integer = allSystems.Count + 1
            Dim systemCount As Integer = allSystems.Count

            Dim matrixTbl As Word.Table =
                doc.Tables.Add(doc.Bookmarks("\endofdoc").Range,
                               matrixRows, matrixCols)
            matrixTbl.Borders.Enable = 1
            matrixTbl.Range.ParagraphFormat.SpaceAfter = 0

            ' ── Column Width Strategy ───────────────────────
            ' Each system column gets a small fixed width
            ' Column 1 (Control + Description) gets everything else
            '
            ' Approach:
            '   1. First auto-fit to content so system cols shrink
            '   2. Then manually set system col preferred width
            '   3. Give remaining to column 1
            ' ────────────────────────────────────────────────

            ' Calculate widths
            ' Total usable width ≈ page width minus margins
            Dim pageSetup As Word.PageSetup = doc.PageSetup
            Dim totalWidth As Single =
                pageSetup.PageWidth - pageSetup.LeftMargin - pageSetup.RightMargin

            ' Each system column: ~1.5 cm (enough for ✔/✘ + padding)
            ' Find longest system name to calculate minimum width
            Dim maxSysNameLen As Integer = 0
            For Each sName In allSystems
                If sName.Length > maxSysNameLen Then maxSysNameLen = sName.Length
            Next

            ' Approximate: ~6 points per character at 9pt font + 10pt padding
            Dim sysColWidth As Single =
                CSng(Math.Max(
                    wordApp.CentimetersToPoints(1.5),
                    maxSysNameLen * 5.5F + 10.0F))

            ' Cap system column width so it doesn't get too wide
            Dim maxSysColWidth As Single = wordApp.CentimetersToPoints(3.0)
            If sysColWidth > maxSysColWidth Then
                sysColWidth = maxSysColWidth
            End If

            ' Column 1 width = total minus all system columns
            Dim col1Width As Single =
                totalWidth - (sysColWidth * systemCount)

            ' Ensure column 1 has a reasonable minimum
            Dim minCol1Width As Single = wordApp.CentimetersToPoints(5.0)
            If col1Width < minCol1Width Then
                ' Recalculate: shrink system columns to fit
                sysColWidth = (totalWidth - minCol1Width) / systemCount
                col1Width = minCol1Width
            End If

            ' Disable auto-fit so manual widths are respected
            matrixTbl.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitFixed)
            matrixTbl.PreferredWidthType = WdPreferredWidthType.wdPreferredWidthPoints
            matrixTbl.PreferredWidth = totalWidth

            ' Apply column 1 width (Control + Description)
            matrixTbl.Columns(1).PreferredWidthType =
                WdPreferredWidthType.wdPreferredWidthPoints
            matrixTbl.Columns(1).PreferredWidth = col1Width

            ' Apply system column widths
            For colIdx As Integer = 2 To matrixCols
                matrixTbl.Columns(colIdx).PreferredWidthType =
                    WdPreferredWidthType.wdPreferredWidthPoints
                matrixTbl.Columns(colIdx).PreferredWidth = sysColWidth
            Next

            ' ── Header Row ──────────────────────────────────
            With matrixTbl.Cell(1, 1).Range
                .Text = "ITGC Control – Description"
                .Bold = 1
                .Font.Size = 10
                .Font.Color = WdColor.wdColorBlack
                .ParagraphFormat.Alignment =
                    WdParagraphAlignment.wdAlignParagraphLeft
            End With

            For sysIdx As Integer = 0 To allSystems.Count - 1
                Dim hdrRng As Word.Range = matrixTbl.Cell(1, sysIdx + 2).Range
                hdrRng.MoveEnd(WdUnits.wdCharacter, -1)
                hdrRng.Text = allSystems(sysIdx)
                hdrRng.Bold = 1
                hdrRng.Font.Size = 9
                hdrRng.Font.Color = WdColor.wdColorBlack
                hdrRng.ParagraphFormat.Alignment =
                    WdParagraphAlignment.wdAlignParagraphCenter
            Next

            ' ── Data Rows ────────────────────────────────────
            For ctrlIdx As Integer = 0 To allControlIDs.Count - 1
                Dim controlID As String = allControlIDs(ctrlIdx)
                Dim dataRow As Integer = ctrlIdx + 2

                ' Get description
                Dim ctrlDescription As String = ""
                If descriptionLookup.ContainsKey(controlID) Then
                    ctrlDescription = descriptionLookup(controlID)
                End If

                ' Column 1: Control ID – Description
                Dim idRng As Word.Range = matrixTbl.Cell(dataRow, 1).Range
                idRng.MoveEnd(WdUnits.wdCharacter, -1)
                idRng.Text = $"{controlID} – {ctrlDescription}"
                idRng.Font.Size = 9
                idRng.Font.Color = WdColor.wdColorBlack
                idRng.ParagraphFormat.Alignment =
                    WdParagraphAlignment.wdAlignParagraphLeft

                ' Bold only the Control ID portion
                Dim boldEnd As Integer = controlID.Length
                Dim boldRange As Word.Range = matrixTbl.Cell(dataRow, 1).Range
                boldRange.MoveEnd(WdUnits.wdCharacter, -1)
                boldRange.SetRange(boldRange.Start, boldRange.Start + boldEnd)
                boldRange.Bold = 1

                ' System columns: tick / cross / dash
                For sysIdx As Integer = 0 To allSystems.Count - 1
                    Dim lookupKey As String = controlID & "|" & allSystems(sysIdx)
                    Dim cellStatus As String = "NONE"

                    If statusLookup.ContainsKey(lookupKey) Then
                        cellStatus = statusLookup(lookupKey)
                    End If

                    SetMatrixCell(matrixTbl.Cell(dataRow, sysIdx + 2), cellStatus)
                Next
            Next

            matrixTbl.Rows.HeightRule = WdRowHeightRule.wdRowHeightAtLeast
            matrixTbl.Rows.Height = wordApp.CentimetersToPoints(0.65)

            ' ==================================================
            ' 7. Detailed Control Records Table
            ' ==================================================
            parentForm.lblNotification.Text = "Adding Detailed Control Records..."
            Dim tblHeader As Word.Range = doc.Paragraphs.Last.Range
            tblHeader.InsertParagraphAfter()
            tblHeader = doc.Paragraphs.Last.Range
            tblHeader.Text = "📝 Detailed Control Records"
            tblHeader.Font.Size = 14
            tblHeader.Font.Bold = 1
            tblHeader.Font.Color = WdColor.wdColorBlack
            tblHeader.ParagraphFormat.Alignment =
    WdParagraphAlignment.wdAlignParagraphLeft
            tblHeader.InsertParagraphAfter()

            ' Table now has 5 columns (removed Timestamp)
            Dim tbl As Word.Table =
    doc.Tables.Add(doc.Bookmarks("\endofdoc").Range,
                   records.Count + 1, 5)
            tbl.Range.ParagraphFormat.SpaceAfter = 6

            ' ── Border Setup ─────────────────────────────
            tbl.Borders.Enable = 0

            With tbl.Borders(WdBorderType.wdBorderTop)
                .LineStyle = WdLineStyle.wdLineStyleSingle
                .LineWidth = WdLineWidth.wdLineWidth075pt
                .Color = WdColor.wdColorBlack
            End With

            With tbl.Borders(WdBorderType.wdBorderBottom)
                .LineStyle = WdLineStyle.wdLineStyleSingle
                .LineWidth = WdLineWidth.wdLineWidth075pt
                .Color = WdColor.wdColorBlack
            End With

            With tbl.Borders(WdBorderType.wdBorderHorizontal)
                .LineStyle = WdLineStyle.wdLineStyleSingle
                .LineWidth = WdLineWidth.wdLineWidth050pt
                .Color = WdColor.wdColorBlack
            End With

            ' Header bottom border – loop 1 to 5
            For c As Integer = 1 To 5
                With tbl.Cell(1, c).Borders(WdBorderType.wdBorderBottom)
                    .LineStyle = WdLineStyle.wdLineStyleSingle
                    .LineWidth = WdLineWidth.wdLineWidth150pt
                    .Color = WdColor.wdColorBlack
                End With
            Next

            ' ── Header Row Content ───────────────────────
            ' Removed "Timestamp" from headers
            Dim headers() As String =
    {"Control ID", "Description", "System", "Status", "Comment"}
            For i As Integer = 0 To headers.Length - 1
                With tbl.Cell(1, i + 1).Range
                    .Text = headers(i)
                    .Bold = 1
                    .Font.Size = 12
                    .Font.Color = WdColor.wdColorBlack
                    .Shading.BackgroundPatternColor = WdColor.wdColorGray25
                End With
            Next

            ' ── Data Rows ────────────────────────────────
            For r As Integer = 0 To records.Count - 1
                Dim rec = records(r)
                Dim rowIdx As Integer = r + 2

                tbl.Cell(rowIdx, 1).Range.Text = rec.ControlID
                tbl.Cell(rowIdx, 2).Range.Text = rec.Description
                tbl.Cell(rowIdx, 3).Range.Text = rec.SystemName
                tbl.Cell(rowIdx, 4).Range.Text = rec.Status
                tbl.Cell(rowIdx, 5).Range.Text = rec.CommentText

                ' Set font for all 5 columns
                For c As Integer = 1 To 5
                    tbl.Cell(rowIdx, c).Range.Font.Size = 10
                    tbl.Cell(rowIdx, c).Range.Font.Bold = 0
                    tbl.Cell(rowIdx, c).Range.Font.Color = WdColor.wdColorBlack
                Next

                ' Status column (now column 4) gets color
                If rec.Status.Equals("Green", StringComparison.OrdinalIgnoreCase) Then
                    tbl.Cell(rowIdx, 4).Range.Font.Color = WdColor.wdColorGreen
                    tbl.Cell(rowIdx, 4).Range.Font.Bold = 1
                ElseIf rec.Status.Equals("Red", StringComparison.OrdinalIgnoreCase) Then
                    tbl.Cell(rowIdx, 4).Range.Font.Color = WdColor.wdColorRed
                    tbl.Cell(rowIdx, 4).Range.Font.Bold = 1
                End If
            Next

            tbl.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitContent)

            ' ==================================================
            ' 8. Detailed Summary by Control ID + System
            ' ==================================================
            parentForm.lblNotification.Text = "Adding Summary Section..."
            Dim summaryTitle As Word.Range = doc.Paragraphs.Last.Range
            summaryTitle.InsertParagraphAfter()
            summaryTitle = doc.Paragraphs.Last.Range
            summaryTitle.Text = "📝 Detailed Summary by Control ID and System"
            summaryTitle.Font.Size = 14
            summaryTitle.Font.Bold = 1
            summaryTitle.Font.Color = WdColor.wdColorBlack
            summaryTitle.ParagraphFormat.Alignment =
    WdParagraphAlignment.wdAlignParagraphLeft
            summaryTitle.InsertParagraphAfter()

            Dim grouped =
    records.GroupBy(Function(r) r.ControlID) _
           .OrderBy(Function(g) g.Key)

            For Each controlGroup In grouped
                Dim firstRec = controlGroup.First()
                Dim ctrlLine As Word.Range = doc.Paragraphs.Last.Range
                ctrlLine.Text =
        $"• Control ID: {firstRec.ControlID} – {firstRec.Description}"
                ctrlLine.Font.Size = 11
                ctrlLine.Font.Bold = 0
                ctrlLine.Font.Color = WdColor.wdColorBlack
                ctrlLine.InsertParagraphAfter()

                Dim sysGrps =
        controlGroup.GroupBy(Function(r) r.SystemName) _
                    .OrderBy(Function(g) g.Key)

                For Each systemGroup In sysGrps
                    Dim systemLine As Word.Range = doc.Paragraphs.Last.Range
                    systemLine.Text = $"   • System: {systemGroup.Key}"
                    systemLine.Font.Size = 10
                    systemLine.Font.Color = WdColor.wdColorBlack
                    systemLine.InsertParagraphAfter()
                    parentForm.lblNotification.Text =
            $"Processing {systemGroup.Key} records..."

                    For Each rec In systemGroup
                        Dim actionLine As Word.Range = doc.Paragraphs.Last.Range
                        If rec.Status.Equals("Red",
                   StringComparison.OrdinalIgnoreCase) Then
                            Dim requiredAction As String =
                    ITGCControlMetadataStore.GetControlAction(rec.ControlID)
                            actionLine.Text =
                    $"      → Required Action: {requiredAction}"
                            actionLine.Font.Color = WdColor.wdColorBlack
                        Else
                            actionLine.Text = "      → No Action Required"
                            actionLine.Font.Color = WdColor.wdColorBlack
                        End If
                        actionLine.Font.Size = 10
                        actionLine.InsertParagraphAfter()
                    Next
                Next
            Next

            ' ==================================================
            ' 9. Save
            ' ==================================================
            Dim outPath As String =
                Path.Combine(monthlyFolder,
                    $"ITGC_Monthly_Report_{forMonth.Replace(",", "").Replace(" ", "_")}.docx")
            doc.SaveAs2(DirectCast(outPath, Object))
            parentForm.lblNotification.ForeColor = Color.Green
            parentForm.lblNotification.Text =
                $"Report generated for {forMonth} at {outPath}"
            MessageBox.Show(
                $"Report saved successfully at:{Environment.NewLine}{outPath}",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            parentForm.lblNotification.ForeColor = Color.Red
            parentForm.lblNotification.Text =
                $"Error generating report: {ex.Message}"
            MessageBox.Show("Error generating report: " & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If doc IsNot Nothing Then Marshal.ReleaseComObject(doc)
            If wordApp IsNot Nothing Then Marshal.ReleaseComObject(wordApp)
        End Try

    End Sub

End Module