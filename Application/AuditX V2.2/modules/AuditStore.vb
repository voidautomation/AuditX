Option Strict On
Option Explicit On

Imports System.IO
Imports System.Text

Public Module AuditStore

    Public Class AuditRecord
        Public Property ControlID As String
        Public Property Description As String
        Public Property SystemName As String
        Public Property Status As String
        Public Property CommentText As String
        Public Property TimeStamp As DateTime

        Public Function ToCsv() As String
            Return String.Join(","c, {
                CsvEscape(ControlID),
                CsvEscape(Description),
                CsvEscape(SystemName),
                CsvEscape(Status),
                CsvEscape(CommentText),
                CsvEscape(TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"))
            })
        End Function

        Public Shared Function FromCsv(parts As String()) As AuditRecord
            Return New AuditRecord With {
                .ControlID = CsvUnescape(parts(0)),
                .Description = CsvUnescape(parts(1)),
                .SystemName = CsvUnescape(parts(2)),
                .Status = CsvUnescape(parts(3)),
                .CommentText = CsvUnescape(parts(4)),
                .TimeStamp = DateTime.Parse(CsvUnescape(parts(5)))
            }
        End Function
    End Class

    Public Sub SaveITGCComment(controlID As String,
                               description As String,
                               systemName As String,
                               isGreen As Boolean,
                               comment As String,
                               baseFolder As String,
                               forMonth As String)

        Dim monthFolder As String = GetMonthlyFolderPath(baseFolder, forMonth)
        Dim csvPath As String = Path.Combine(monthFolder, $"ITGC_Audit_Comments_{forMonth}.csv")

        If Not Directory.Exists(monthFolder) Then Directory.CreateDirectory(monthFolder)

        Dim records As List(Of AuditRecord) = LoadCsv(csvPath)

        records.RemoveAll(Function(r) r.ControlID.Equals(controlID, StringComparison.OrdinalIgnoreCase) AndAlso
                                         r.SystemName.Equals(systemName, StringComparison.OrdinalIgnoreCase))

        records.Add(New AuditRecord With {
            .ControlID = controlID,
            .Description = description,
            .SystemName = systemName,
            .Status = If(isGreen, "Green", "Red"),
            .CommentText = comment,
            .TimeStamp = DateTime.Now
        })

        SaveCsv(csvPath, records)
    End Sub

    Public Function LoadMonthlyRecords(baseFolder As String, forMonth As String) As List(Of AuditRecord)
        Dim csvPath As String = Path.Combine(GetMonthlyFolderPath(baseFolder, forMonth), $"ITGC_Audit_Comments_{forMonth}.csv")
        Return LoadCsv(csvPath)
    End Function

    Public Function GetMonthlyFolderPath(baseFolder As String, forMonth As String) As String
        ' Directly use the string as the subfolder name (e.g., "July, 2025")
        Return Path.Combine(baseFolder, forMonth)
    End Function

    Private Function LoadCsv(csvPath As String) As List(Of AuditRecord)
        Dim list As New List(Of AuditRecord)
        If Not File.Exists(csvPath) Then Return list

        Dim allLines = File.ReadAllLines(csvPath, Encoding.UTF8)
        If allLines.Length <= 1 Then Return list

        For i = 1 To allLines.Length - 1
            Dim line = allLines(i).Trim()
            If String.IsNullOrWhiteSpace(line) Then Continue For
            Dim parts = SplitCsv(line)
            If parts.Length = 6 Then list.Add(AuditRecord.FromCsv(parts))
        Next

        Return list
    End Function

    Private Sub SaveCsv(csvPath As String, records As List(Of AuditRecord))
        Dim sb As New StringBuilder()
        sb.AppendLine("ControlID,Description,System,Status,Comment,Timestamp")
        For Each rec In records.OrderBy(Function(r) r.ControlID)
            sb.AppendLine(rec.ToCsv())
        Next
        File.WriteAllText(csvPath, sb.ToString(), Encoding.UTF8)
    End Sub

    Private Function CsvEscape(value As String) As String
        If value Is Nothing Then value = ""
        value = value.Replace("""", """""")
        Return $"""{value}"""
    End Function

    Private Function CsvUnescape(value As String) As String
        If value.StartsWith("""") AndAlso value.EndsWith("""") Then
            value = value.Substring(1, value.Length - 2)
        End If
        Return value.Replace("""""", """")
    End Function

    Private Function SplitCsv(line As String) As String()
        Dim parts As New List(Of String)()
        Dim cur As New StringBuilder()
        Dim inQuotes As Boolean = False

        For i = 0 To line.Length - 1
            Dim ch = line(i)
            If ch = """"c Then
                If inQuotes AndAlso i + 1 < line.Length AndAlso line(i + 1) = """"c Then
                    cur.Append(""""c) : i += 1
                Else
                    inQuotes = Not inQuotes
                End If
            ElseIf ch = ","c AndAlso Not inQuotes Then
                parts.Add(cur.ToString()) : cur.Clear()
            Else
                cur.Append(ch)
            End If
        Next

        parts.Add(cur.ToString())
        Return parts.ToArray()
    End Function

End Module
