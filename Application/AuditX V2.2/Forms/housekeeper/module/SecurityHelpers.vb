Imports System.Security.Cryptography
Imports System.Text

Public Module SecurityHelpers
    Private ReadOnly Entropy As Byte() = Encoding.UTF8.GetBytes("HouseKeeperEntropy_v1") ' changeable constant

    ''' <summary>
    ''' Encrypts a plaintext string using DPAPI (CurrentUser). Returns base64 ciphertext.
    ''' </summary>
    Public Function EncryptString(plain As String) As String
        If String.IsNullOrEmpty(plain) Then
            Return String.Empty
        End If

        Dim plainBytes = Encoding.UTF8.GetBytes(plain)
        Dim protectedBytes = ProtectedData.Protect(plainBytes, Entropy, DataProtectionScope.CurrentUser)
        Return Convert.ToBase64String(protectedBytes)
    End Function

    ''' <summary>
    ''' Attempts to decrypt a base64 ciphertext previously created with <see cref="EncryptString"/>.
    ''' If decryption fails (value was plain text or corrupted) the original value is returned.
    ''' </summary>
    Public Function DecryptString(cipherOrPlain As String) As String
        If String.IsNullOrEmpty(cipherOrPlain) Then
            Return String.Empty
        End If

        Try
            Dim protectedBytes = Convert.FromBase64String(cipherOrPlain)
            Dim plainBytes = ProtectedData.Unprotect(protectedBytes, Entropy, DataProtectionScope.CurrentUser)
            Return Encoding.UTF8.GetString(plainBytes)
        Catch
            ' On any failure (not base64, wrong format, different machine/user, etc.)
            ' assume the stored value was plain text and return it unchanged.
            Return cipherOrPlain
        End Try
    End Function
End Module