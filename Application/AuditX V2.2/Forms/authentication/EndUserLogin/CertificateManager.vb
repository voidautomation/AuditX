Imports System.IO
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports Newtonsoft.Json

Public Class CertificateManager

    Private Shared encryptionKey As String = "InfinityVoid_Enterprise_Key_2026"

    Private Shared firebaseURL As String =
        "https://sap-itgc-audit-default-rtdb.asia-southeast1.firebasedatabase.app/licensed_users/"

    Public Shared Function ValidateCertificate(currentUser As String,
                                            ByRef message As String,
                                            ByRef licenseData As Dictionary(Of String, String)) As Boolean

        message = ""
        licenseData = Nothing

        Dim certPath As String = Path.Combine(Application.StartupPath, "license.cert")

        If Not File.Exists(certPath) Then
            message = "License certificate not found."
            Return False
        End If

        Try

            Dim encrypted As String = File.ReadAllText(certPath)
            Dim decrypted As String = Decrypt(encrypted)

            Dim parts = decrypted.Split("|"c)

            If parts.Length < 7 Then
                message = "Corrupted certificate."
                Return False
            End If

            Dim cid = parts(0)
            Dim windowsID = parts(1)
            Dim fullname = parts(2)
            Dim email = parts(3)
            Dim issue = parts(4)
            Dim expiryLocal = parts(5) ' ignored
            Dim signature = parts(6)

            ' Validate signature
            Dim raw = cid &
                  windowsID &
                  expiryLocal &
                  fullname &
                  email &
                  encryptionKey

            Dim expectedSignature = ComputeHash(raw)

            If expectedSignature <> signature Then
                message = "Certificate tampered."
                Return False
            End If

            ' Validate WindowsID
            Dim currentWindowsID =
            Environment.UserDomainName & "\" & Environment.UserName

            If windowsID <> currentWindowsID Then
                message = "License not issued for this machine."
                Return False
            End If

            ' 🔹 Firebase validation
            Dim firebaseUser = GetFirebaseUser(cid)

            If firebaseUser Is Nothing Then
                message = "License not found on server."
                Return False
            End If

            Dim expiryFirebase As DateTime =
            Date.Parse(firebaseUser("expiry").ToString())

            If Date.Today > expiryFirebase Then
                message = "License expired."
                Return False
            End If

            licenseData = New Dictionary(Of String, String)

            licenseData("cid") = cid
            licenseData("fullname") = firebaseUser("fullName").ToString()
            licenseData("email") = firebaseUser("email").ToString()
            licenseData("issue") = firebaseUser("created").ToString()
            licenseData("expiry") = firebaseUser("expiry").ToString()

            Return True

        Catch

            message = "Invalid license."
            Return False

        End Try

    End Function

    Private Shared Function GetFirebaseUser(cid As String) As Dictionary(Of String, Object)

        Try

            Dim request As WebRequest =
            WebRequest.Create(firebaseURL & cid & ".json")

            request.Method = "GET"

            Dim response = request.GetResponse()

            Dim reader As New StreamReader(response.GetResponseStream())

            Dim json = reader.ReadToEnd()

            If json = "null" Then Return Nothing

            Return JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(json)

        Catch
            Return Nothing
        End Try

    End Function

#Region "Firebase Check"

    Private Shared Function FirebaseUserExists(cid As String) As Boolean

        Try

            Dim request As WebRequest =
                WebRequest.Create(firebaseURL & cid & ".json")

            request.Method = "GET"

            Dim response = request.GetResponse()

            Dim reader As New StreamReader(response.GetResponseStream())

            Dim json = reader.ReadToEnd()

            If json = "null" Then Return False

            Return True

        Catch

            Return False

        End Try

    End Function

#End Region

#Region "Decrypt"

    Private Shared Function Decrypt(cipherText As String) As String

        Dim aes As Aes = Aes.Create()

        Dim pdb As New Rfc2898DeriveBytes(encryptionKey,
                                          Encoding.UTF8.GetBytes("AuditX_Salt"))

        aes.Key = pdb.GetBytes(32)
        aes.IV = pdb.GetBytes(16)

        Using ms As New MemoryStream(Convert.FromBase64String(cipherText))

            Using cs As New CryptoStream(ms,
                                         aes.CreateDecryptor(),
                                         CryptoStreamMode.Read)

                Using sr As New StreamReader(cs)

                    Return sr.ReadToEnd()

                End Using

            End Using

        End Using

    End Function

#End Region

#Region "Hash"

    Private Shared Function ComputeHash(text As String) As String

        Using sha As SHA256 = SHA256.Create()

            Dim bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text))

            Return Convert.ToBase64String(bytes)

        End Using

    End Function

#End Region

End Class