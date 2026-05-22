' File: FileUtils.vb
Imports System.IO

Module FileUtils
    ''' <summary>
    ''' Returns a unique file name by appending (1), (2), etc., before the extension if needed.
    ''' </summary>
    ''' <param name="basePath">The folder where the file should be saved</param>
    ''' <param name="fileNameWithExt">File name with extension (e.g., "Report.docx")</param>
    ''' <returns>Full file path with unique name</returns>
    Public Function GetUniqueFileName(basePath As String, fileNameWithExt As String) As String
        Dim fullPath As String = Path.Combine(basePath, fileNameWithExt)
        If Not File.Exists(fullPath) Then Return fullPath

        Dim nameWithoutExt As String = Path.GetFileNameWithoutExtension(fileNameWithExt)
        Dim ext As String = Path.GetExtension(fileNameWithExt)
        Dim counter As Integer = 1

        Do
            Dim newFileName As String = $"{nameWithoutExt} ({counter}){ext}"
            fullPath = Path.Combine(basePath, newFileName)
            counter += 1
        Loop While File.Exists(fullPath)

        Return fullPath
    End Function
End Module
