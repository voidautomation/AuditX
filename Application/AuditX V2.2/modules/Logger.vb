Imports System.IO

Module Logger

    Private logFolderPath As String = "C:\Void\Logs"
    Private logFilePath As String = ""
    Private currentUser As String = Environment.UserName
    Private controlID As String = ""

    ''' <summary>
    ''' Initializes the log file path with control ID and current user
    ''' </summary>
    ''' <param name="ctrlID">Control ID like ITGC01</param>
    Public Sub InitializeLog(ctrlID As String)
        Try
            controlID = ctrlID
            CreateFolderPath(logFolderPath)

            Dim logFileName As String = $"SAPExecutionLog - {currentUser} {DateTime.Now:yyyy-MM-dd}.txt"
            logFilePath = Path.Combine(logFolderPath, logFileName)
            LogMessage("Log initialized for Control: " & controlID)
        Catch ex As Exception
            MsgBox("Error initializing log: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ''' <summary>
    ''' Writes a message to the log file with timestamp
    ''' </summary>
    ''' <param name="message">The log message</param>
    ''' <param name="success">True for success, False for error</param>
    Public Sub LogMessage(message As String, Optional success As Boolean = True)
        Try
            If String.IsNullOrEmpty(logFilePath) Then
                ' Default fallback if InitializeLog wasn't called
                CreateFolderPath(logFolderPath)
                logFilePath = Path.Combine(logFolderPath, $"SAPExecutionLog - {currentUser} {DateTime.Now:yyyy-MM-dd}.txt")
            End If

            Using sw As StreamWriter = File.AppendText(logFilePath)
                Dim status As String = If(success, "SUCCESS", "ERROR")
                sw.WriteLine($"{DateTime.Now:G} [{currentUser}] - [{status}] {message}")
                Home.lblNotification.Text = message
            End Using
        Catch ex As Exception
            MsgBox("Failed to write to log: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ''' <summary>
    ''' Logs an exception with full details (message, source, stacktrace)
    ''' </summary>
    ''' <param name="ex">The exception object</param>
    ''' <param name="customMessage">Optional extra message for context</param>
    Public Sub LogException(ex As Exception, Optional customMessage As String = "")
        Try
            If String.IsNullOrEmpty(logFilePath) Then
                ' Default fallback if InitializeLog wasn't called
                CreateFolderPath(logFolderPath)
                logFilePath = Path.Combine(logFolderPath, $"SAPExecutionLog - {currentUser} {DateTime.Now:yyyy-MM-dd}.txt")

            End If

            Using sw As StreamWriter = File.AppendText(logFilePath)
                sw.WriteLine($"{DateTime.Now:G} [{currentUser}] - [EXCEPTION] {customMessage}")
                sw.WriteLine($"   Message   : {ex.Message}")
                sw.WriteLine($"   Source    : {ex.Source}")
                sw.WriteLine($"   StackTrace: {ex.StackTrace}")
                sw.WriteLine(New String("-"c, 80))
            End Using
        Catch innerEx As Exception
            MsgBox("Failed to write exception to log: " & innerEx.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ''' <summary>
    ''' Opens the log folder (C:\Void\Logs) in File Explorer
    ''' </summary>
    Public Sub OpenLogFolder()
        Try
            If Directory.Exists(logFolderPath) Then
                Process.Start("explorer.exe", """" & logFolderPath & """")
            Else
                MsgBox("Log folder not found: " & logFolderPath, MsgBoxStyle.Exclamation, "Folder Not Found")
            End If
        Catch ex As Exception
            MsgBox("Error opening log folder: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ''' <summary>
    ''' Ensures the full folder path exists by creating missing directories
    ''' </summary>
    ''' <param name="folderPath">Full folder path to create</param>
    Private Sub CreateFolderPath(folderPath As String)
        Try
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If
        Catch ex As Exception
            LogMessage("Error creating folder: " & folderPath & " - " & ex.Message, False)
            MsgBox("Failed to create folder: " & folderPath & vbCrLf & "Error: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

End Module
