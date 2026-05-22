Module EnsureFolderPath
    '----------------------------------------------Ensure Folder Path Exists And Save--------------------------------------------------------
    Sub EnsureFolderPathExistsAndSave(folderPath As String, fileName As String, WordDoc As Object)
        Dim folderParts() As String
        Dim i As Integer
        Dim currentPath As String = ""
        ' Split the path by the backslash delimiter
        folderParts = folderPath.Split("\"c)
        If folderParts.Length > 0 Then
            currentPath = folderParts(0) & "\"
        End If
        ' Create each part of the folder path if it doesn't exist
        For i = 1 To folderParts.Length - 1
            currentPath &= folderParts(i) & "\"
            If Not IO.Directory.Exists(currentPath) Then
                Try
                    IO.Directory.CreateDirectory(currentPath)
                    Logger.LogMessage("Creating directory: " & currentPath, True)
                Catch ex As Exception
                    MsgBox("Error creating directory: " & currentPath & vbCrLf & "Error: " & ex.Message, vbCritical)
                    Logger.LogMessage("Error creating directory: " & currentPath & vbCrLf & "Error: " & ex.Message, False)
                    Exit Sub
                End Try
            End If
        Next

        ' Save the Word document to the specified path
        Dim fullFilePath As String = IO.Path.Combine(currentPath, fileName)
        Try
            WordDoc.SaveAs2(fullFilePath)
            ' MsgBox("File saved successfully at: " & fullFilePath, vbInformation)
            Home.lblNotification.ForeColor = Color.Green
            Home.lblNotification.Text = "File saved successfully at: " & fullFilePath

            Logger.LogMessage("File saved successfully at: " & fullFilePath, True)
        Catch ex As Exception
            MsgBox("Error saving file: " & ex.Message & vbCrLf & "Full File Path: " & fullFilePath, vbCritical)
            Logger.LogMessage("Error saving file: " & ex.Message & vbCrLf & "Full File Path: " & fullFilePath, False)
            Logger.LogException(ex, "Error saving file")
        End Try
    End Sub

End Module
