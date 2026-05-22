Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class FirebaseUserService

    Private Shared baseUrl As String =
"https://sap-itgc-audit-default-rtdb.asia-southeast1.firebasedatabase.app/Licensed_users/"

    Private Shared client As New HttpClient()

    Shared Sub New()
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
    End Sub

    '========================================
    ' USER MODEL
    '========================================
    Public Class UserModel

        Public Property uid As String
        Public Property username As String
        Public Property fullname As String
        Public Property email As String
        Public Property company As String
        Public Property role As String
        Public Property status As String
        Public Property created_on As String
        Public Property last_login As String

    End Class


    '========================================
    ' GENERATE UID
    '========================================
    Public Shared Async Function GenerateUID() As Task(Of String)

        Dim users = Await GetUsers()

        Dim nextId As Integer = 1

        If users IsNot Nothing AndAlso users.Count > 0 Then

            Dim maxId =
            users.Keys.Select(Function(x) Integer.Parse(x.Substring(1))).Max()

            nextId = maxId + 1

        End If

        Return "C" & nextId.ToString("D6")

    End Function


    '========================================
    ' CREATE USER
    '========================================
    Public Shared Async Function CreateUser(user As UserModel) As Task

        Dim json = JsonConvert.SerializeObject(user)

        Dim content As New StringContent(json, Encoding.UTF8, "application/json")

        Await client.PutAsync(baseUrl & user.uid & ".json", content)

    End Function


    '========================================
    ' READ USERS
    '========================================
    Public Shared Async Function GetUsers() _
    As Task(Of Dictionary(Of String, UserModel))

        Dim response = Await client.GetStringAsync(baseUrl & ".json")

        If response = "null" Then
            Return New Dictionary(Of String, UserModel)
        End If

        Return JsonConvert.DeserializeObject(Of Dictionary(Of String, UserModel))(response)

    End Function


    '========================================
    ' UPDATE USER
    '========================================
    Public Shared Async Function UpdateUser(user As UserModel) As Task

        Dim json = JsonConvert.SerializeObject(user)

        Dim content As New StringContent(json, Encoding.UTF8, "application/json")

        Await client.PutAsync(baseUrl & user.uid & ".json", content)

    End Function


    '========================================
    ' DISABLE USER
    '========================================
    Public Shared Async Function DisableUser(uid As String) As Task

        Dim users = Await GetUsers()

        If Not users.ContainsKey(uid) Then Exit Function

        Dim user = users(uid)

        user.status = "Disabled"

        Await UpdateUser(user)

    End Function


    '========================================
    ' UPDATE LAST LOGIN
    '========================================
    Public Shared Async Function UpdateLastLogin(uid As String) As Task

        Dim users = Await GetUsers()

        If Not users.ContainsKey(uid) Then Exit Function

        Dim user = users(uid)

        user.last_login = DateTime.Now.ToString("yyyy-MM-dd HH:mm")

        Await UpdateUser(user)

    End Function

End Class