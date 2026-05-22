Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks

Public Class FirebaseAuthService

    Private Shared ReadOnly apiKey As String = "AIzaSyCZItL-5QgAIn_BQ5GZcyneaPGZJi6xlFw"
    Private Shared ReadOnly firebaseUrl As String =
        "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" & apiKey

    Public Shared Async Function SendPasswordResetAsync(userEmail As String) As Task(Of String)

        Using client As New HttpClient()

            Dim jsonBody As String =
                "{""requestType"":""PASSWORD_RESET"",""email"":""" & userEmail & """}"

            Dim content As New StringContent(jsonBody, Encoding.UTF8, "application/json")

            Dim response = Await client.PostAsync(firebaseUrl, content)
            Dim responseString = Await response.Content.ReadAsStringAsync()

            If response.IsSuccessStatusCode Then
                Return "Password reset email sent successfully." & vbCrLf & "Please check your spam folder in case email is not recieved in Inbox"
            Else
                Return "Error: " & responseString
            End If

        End Using

    End Function

End Class