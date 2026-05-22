'Imports System.Net
'Imports System.IO
'Imports Newtonsoft.Json


'Public Class UpdateManager

'    Private Const VersionUrl As String =
'        "https://raw.githubusercontent.com/voidautomation/AuditX/refs/heads/main/version.json"

'    Public Shared Async Sub CheckForUpdate(owner As Form)

'        Try

'            Dim currentVersion As String = Application.ProductVersion

'            Using client As New WebClient()

'                client.Headers.Add("User-Agent", "AuditX")

'                Dim json As String =
'                    Await client.DownloadStringTaskAsync(VersionUrl)

'                Dim versionData As VersionInfo =
'                    JsonConvert.DeserializeObject(Of VersionInfo)(json)

'                If New Version(versionData.latest_version) >
'                   New Version(currentVersion) Then

'                    Dim frm As New UpdateForm(versionData)
'                    frm.ShowDialog(owner) ' Modal on LoginForm

'                End If

'            End Using

'        Catch
'            ' Silent fail
'        End Try

'    End Sub



'    Private Shared Sub LaunchUpdater()

'        Process.Start(
'            Path.Combine(Application.StartupPath, "Updater.exe"))

'        Application.Exit()

'    End Sub

'End Class