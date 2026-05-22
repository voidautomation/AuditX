Imports System.ComponentModel
Imports System.DirectoryServices.AccountManagement
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows
Imports Microsoft.Office.Interop.Excel
Imports Newtonsoft.Json

Public Class LoginForm


    ' TODO: Insert code to perform custom authentication using the provided username and password 
    ' (See https://go.microsoft.com/fwlink/?LinkId=35339).  
    ' The custom principal can then be attached to the current thread's principal as follows: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
    ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
    ' such as the username, display name, etc. Private Const FirebaseApiKey As String = "YOUR_FIREBASE_WEB_API_KEY"

    ' Ensure DPI awareness runs before UI is shown
    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Call DPI setup early
            ITGC_AUDIT.EnsureDpiAware()
            'UpdateManager.CheckForUpdate(Me)

            ReloadForm()
            AddHandler txtEmail.TextChanged, AddressOf ValidateEmailLive
        Catch ex As Exception
            ' optional logging
            Logger.LogMessage("Error during startup DPI setup: " & ex.Message, False)
        End Try
    End Sub


    Function ReloadForm() As Boolean
        Try
            ' Clear existing data and reset UI elements
            txtEmail.Text = ""
            txtPassword.Text = ""
            lblLoginStatus.Text = ""
            lblEmailError.Text = ""
            ' Re-apply hover effects in case they were lost
            ApplyHoverEffect(txtPassword, Color.LightYellow, Cursors.IBeam)
            ApplyHoverEffect(txtEmail, Color.LightYellow, Cursors.IBeam)
            ' Re-hook live validation
            RemoveHandler txtEmail.TextChanged, AddressOf ValidateEmailLive
            AddHandler txtEmail.TextChanged, AddressOf ValidateEmailLive
            Return True
        Catch ex As Exception
            Logger.LogMessage("Error reloading LoginForm: " & ex.Message, False)
            Return False
        End Try
    End Function

    ' ✅ Email format validation
    Private Sub ValidateEmailLive(sender As Object, e As EventArgs)
        Dim email As String = txtEmail.Text.Trim()
        Dim emailPattern As String = "^[^@\s]+@[^@\s]+\.[^@\s]+$"

        If String.IsNullOrWhiteSpace(email) Then
            lblEmailError.Text = ""   ' Hide error if empty
        ElseIf Not Regex.IsMatch(email, emailPattern) Then
            lblEmailError.Text = "Invalid email format"
            lblEmailError.ForeColor = Color.Coral
        Else
            lblEmailError.Text = ""   ' Clear error if valid
        End If
    End Sub


    Private Const FirebaseApiKey As String = "AIzaSyCZItL-5QgAIn_BQ5GZcyneaPGZJi6xlFw"

    Private Async Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim email As String = txtEmail.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        If String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("Please enter both email and password.")
            Return
        End If
        lblLoginStatus.ForeColor = Color.White
        lblLoginStatus.Text = "Authenticating... Please wait."
        Try
            Dim client As New WebClient()
            client.Headers(HttpRequestHeader.ContentType) = "application/json"

            Dim requestBody As New Dictionary(Of String, Object) From {
            {"email", email},
            {"password", password},
            {"returnSecureToken", True}
        }

            Dim jsonData As String = JsonConvert.SerializeObject(requestBody)
            Dim response As String = Await client.UploadStringTaskAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={FirebaseApiKey}",
            "POST",
            jsonData
        )

            Dim result = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(response)

            If result.ContainsKey("idToken") Then
                Dim idToken As String = result("idToken").ToString()
                Dim uid As String = result("localId").ToString() ' Firebase UID

                ' Fetch user data from Realtime Database
                Dim userUrl As String = $"https://sap-itgc-audit-default-rtdb.asia-southeast1.firebasedatabase.app/users/{uid}.json?auth={idToken}"
                Dim userResponse As String = Await client.DownloadStringTaskAsync(userUrl)

                If Not String.IsNullOrWhiteSpace(userResponse) AndAlso userResponse <> "null" Then
                    Dim userData = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(userResponse)

                    Dim userName As String = If(userData.ContainsKey("name"), userData("name").ToString(), "N/A")
                    Dim company As String = If(userData.ContainsKey("company"), userData("company").ToString(), "N/A")

                    MessageBox.Show("Hello, " & userName & "_________" & vbCrLf & "License to: " & company, "Login successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Pseudocode:
                    ' 1. Ensure Home form is initialized properly before showing.
                    ' 2. If Home form has a Load event or initialization logic, verify it is being called.
                    ' 3. After setting properties, call Show() and optionally Activate().
                    ' 4. Optionally, use ShowDialog() if modal behavior is needed.
                    ' 5. Ensure Me.Hide() does not interfere with Home form loading.
                    lblLoginStatus.Text = "Please wait... Loading Dashboard."

                    Me.ReloadForm()

                    Admin.UserName = userName
                    Admin.UserEmail = email
                    Admin.UserCompany = company
                    Admin.Show()
                    Me.Hide()

                End If
            Else
                MessageBox.Show("Login failed.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As WebException
            Dim errorMessage As String = "Login failed. Please check your internet connection or credentials."
            lblLoginStatus.ForeColor = Color.Red
            lblLoginStatus.Text = "Login Failed"
            ' Handle cases where response is available
            If ex.Response IsNot Nothing Then
                Try
                    Using reader As New IO.StreamReader(ex.Response.GetResponseStream())
                        Dim resp As String = reader.ReadToEnd()

                        If Not String.IsNullOrEmpty(resp) Then
                            Dim errorObj = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(resp)

                            If errorObj.ContainsKey("error") Then
                                Dim err = CType(errorObj("error"), Newtonsoft.Json.Linq.JObject)
                                Dim message As String = err("message").ToString()

                                Select Case message
                                    Case "EMAIL_NOT_FOUND"
                                        errorMessage = "The email address is not registered."
                                    Case "INVALID_EMAIL"
                                        errorMessage = "The email address is not registered."
                                    Case "INVALID_PASSWORD"
                                        errorMessage = "The password is incorrect."
                                    Case "INVALID_LOGIN_CREDENTIALS"
                                        errorMessage = "The password is incorrect."
                                    Case "USER_DISABLED"
                                        errorMessage = "This account has been disabled."
                                    Case Else
                                        errorMessage = "Authentication error: " & message
                                End Select
                            End If
                        End If
                    End Using
                Catch parseEx As Exception
                    errorMessage = "Unexpected error while processing response: " & parseEx.Message
                End Try
            Else
                ' No response at all (network issue, TLS problem, proxy/firewall)
                errorMessage = "No response from authentication server. Please check your internet or firewall settings."
            End If

            MessageBox.Show(errorMessage, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub OnEnterKeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress, txtEmail.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            btnLogin.PerformClick()
            e.Handled = True
        End If
    End Sub


    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Dim Result As String = MsgBox("Are you sure you want to exit the application?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Confirm Exit")

        If Result = vbYes Then
            Me.Close()

        End If
    End Sub

    Private Sub btnUserLogin_Click(sender As Object, e As EventArgs) Handles btnUserLogin.Click
        Dim errorMsg As String = ""
        Dim userInfo As WindowsAuthHelper.WindowsUserDetails = Nothing

        If WindowsAuthHelper.Authenticate(errorMsg, userInfo) Then

            Dim certMessage As String = ""
            Dim licenseData As Dictionary(Of String, String) = Nothing

            If Not CertificateManager.ValidateCertificate(userInfo.UserName,
                                                      certMessage,
                                                      licenseData) Then

                MessageBox.Show(certMessage, "License Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            MessageBox.Show("Login Successful!",
                        "Access Granted",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)


            Home.UserFullName = licenseData("fullname")
            Home.UserEmail = licenseData("email")
            Home.LicenseIssueDate = licenseData("issue")
            Home.LicenseExpiryDate = licenseData("expiry")

            Home.Show()
            ' Hide the login UI (keep it hidden so Application.Run keeps the message loop)
            Me.Hide()

        Else

            MessageBox.Show(errorMsg, "Authentication Failed")

        End If


    End Sub

    Private Async Sub btnForgotPassword_Click(sender As Object, e As EventArgs) Handles btnForgotPassword.Click


        Dim email As String = txtEmail.Text.Trim()

        If email = "" Then
            MessageBox.Show("Please enter email in the text box...", "Invalid Input                                                                ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim result = Await FirebaseAuthService.SendPasswordResetAsync(email)
        MessageBox.Show(result)
    End Sub


    Private Sub btnForgotPassword_MouseEnter(sender As Object, e As EventArgs) _
    Handles btnForgotPassword.MouseEnter

        Dim lnk As LinkLabel = DirectCast(sender, LinkLabel)
        lnk.LinkColor = Color.Orange
        lnk.Cursor = Cursors.Hand

    End Sub


    Private Sub btnForgotPassword_MouseLeave(sender As Object, e As EventArgs) _
    Handles btnForgotPassword.MouseLeave

        Dim lnk As LinkLabel = DirectCast(sender, LinkLabel)
        lnk.LinkColor = Color.Coral
        lnk.Cursor = Cursors.Default

    End Sub

End Class
