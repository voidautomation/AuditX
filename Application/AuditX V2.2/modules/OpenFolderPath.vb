Imports System.IO

Module OpenFolderPath
    '----------------------------------------------Folder Path Open--------------------------------------------------------
    Public Sub OpenFolder(path As String)
        If Directory.Exists(path) Then
            ' Open the folder directly
            Process.Start("explorer.exe", """" & path & """")
        ElseIf File.Exists(path) Then
            ' Extract directory from file path and open
            Dim folderPath As String = path
            If Directory.Exists(folderPath) Then
                Process.Start("explorer.exe", """" & folderPath & """")
            Else
                MsgBox("Folder not found for file path.", MsgBoxStyle.Exclamation, "Error")
                Logger.LogMessage("Folder not found for file path.", False)
            End If
        Else
            MsgBox("Path not found: " & path, MsgBoxStyle.Exclamation, "Error")
            Logger.LogMessage(path & " not found: ", False)
        End If
    End Sub
End Module
