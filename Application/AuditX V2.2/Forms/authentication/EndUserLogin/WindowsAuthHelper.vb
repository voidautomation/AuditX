Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Security.Principal
Imports System.ComponentModel

Public Class WindowsAuthHelper

#Region "User Details Model"

    Public Class WindowsUserDetails
        Public Property FullIdentity As String
        Public Property Domain As String
        Public Property UserName As String
        Public Property Sid As String
    End Class

#End Region

#Region "Win32 Structures"

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Private Structure CREDUI_INFO
        Public cbSize As Integer
        Public hwndParent As IntPtr
        Public pszMessageText As String
        Public pszCaptionText As String
        Public hbmBanner As IntPtr
    End Structure

#End Region

#Region "Constants"

    Private Const LOGON32_LOGON_INTERACTIVE As Integer = 2
    Private Const LOGON32_PROVIDER_DEFAULT As Integer = 0

    Private Const CREDUI_FLAGS_GENERIC_CREDENTIALS As Integer = &H40000
    Private Const CREDUI_FLAGS_ALWAYS_SHOW_UI As Integer = &H80

#End Region

#Region "Win32 API"

    <DllImport("credui.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function CredUIPromptForCredentialsW(
        ByRef creditUR As CREDUI_INFO,
        targetName As String,
        reserved1 As IntPtr,
        iError As Integer,
        userName As StringBuilder,
        maxUserName As Integer,
        password As StringBuilder,
        maxPassword As Integer,
        ByRef pfSave As Boolean,
        flags As Integer) As Integer
    End Function

    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function LogonUser(
        lpszUsername As String,
        lpszDomain As String,
        lpszPassword As String,
        dwLogonType As Integer,
        dwLogonProvider As Integer,
        ByRef phToken As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function CloseHandle(hObject As IntPtr) As Boolean
    End Function

#End Region

#Region "Authentication"

    Public Shared Function Authenticate(
        ByRef errorMessage As String,
        ByRef userInfo As WindowsUserDetails
    ) As Boolean

        errorMessage = ""
        userInfo = Nothing

        Dim info As New CREDUI_INFO()
        info.cbSize = Marshal.SizeOf(info)
        info.pszCaptionText = "AuditX Secure Login"
        info.pszMessageText = "Authenticate using your Windows credentials"

        Dim userName As New StringBuilder(256)
        Dim password As New StringBuilder(256)
        Dim save As Boolean = False

        ' Auto-fill current user
        userName.Append(WindowsIdentity.GetCurrent().Name)

        Dim result As Integer = CredUIPromptForCredentialsW(
            info,
            Environment.MachineName,
            IntPtr.Zero,
            0,
            userName,
            userName.Capacity,
            password,
            password.Capacity,
            save,
            CREDUI_FLAGS_GENERIC_CREDENTIALS Or CREDUI_FLAGS_ALWAYS_SHOW_UI
        )

        If result <> 0 Then
            errorMessage = "Login cancelled."
            Return False
        End If

        Dim fullUser As String = userName.ToString()
        Dim domain As String = ""
        Dim user As String = ""

        If fullUser.Contains("\") Then
            Dim parts = fullUser.Split("\"c)
            domain = parts(0)
            user = parts(1)
        Else
            domain = Environment.MachineName
            user = fullUser
        End If

        Dim token As IntPtr = IntPtr.Zero

        Dim isValid As Boolean = LogonUser(
            user,
            domain,
            password.ToString(),
            LOGON32_LOGON_INTERACTIVE,
            LOGON32_PROVIDER_DEFAULT,
            token
        )

        If Not isValid Then
            Dim err = Marshal.GetLastWin32Error()
            errorMessage = New Win32Exception(err).Message
            Return False
        End If

        ' Build user details from token
        Dim identity As New WindowsIdentity(token)

        userInfo = New WindowsUserDetails With {
            .FullIdentity = identity.Name,
            .Domain = domain,
            .UserName = user,
            .Sid = identity.User.Value
        }

        CloseHandle(token)

        Return True

    End Function

#End Region

End Class
