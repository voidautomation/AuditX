Imports System.Data.SQLite
Imports System.IO
Imports SAP.Middleware.Connector

Public Class SAPConnection
    Implements IDestinationConfiguration

#Region "Fields & Shared State"

    Public Shared Property RegisteredConfig As IDestinationConfiguration = Nothing
    Private Shared _cachedSystems As Dictionary(Of String, SAPSystem) = Nothing
    Private Shared ReadOnly _cacheLock As New Object()
    Private Shared _db As SAPSystemDatabase = Nothing

#End Region

#Region "Constructor"

    Public Sub New()
        EnsureDatabaseInitialized()
        Try
            _db.MigrateSystemNumbers()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine(
                "SystemNumber migration warning: " & ex.Message)
        End Try
        RefreshCache()
    End Sub

#End Region

#Region "Database Initialization"

    Private Shared Sub EnsureDatabaseInitialized()
        SyncLock _cacheLock
            If _db Is Nothing Then
                Try
                    _db = New SAPSystemDatabase("sap_systems.db")
                Catch ex As Exception
                    Throw New InvalidOperationException(
                        "Failed to initialize SAP systems database: " &
                        ex.Message, ex)
                End Try
            End If
        End SyncLock
    End Sub

#End Region

#Region "Cache Management"

    Public Shared Sub RefreshCache()
        EnsureDatabaseInitialized()

        SyncLock _cacheLock
            Try
                Dim rawSystems As Dictionary(Of String, SAPSystem) =
                    _db.GetAllSystems()
                Dim decrypted As New Dictionary(Of String, SAPSystem)(
                    StringComparer.OrdinalIgnoreCase)

                For Each kvp In rawSystems
                    decrypted(kvp.Key) = New SAPSystem With {
                        .AppServerHost = kvp.Value.AppServerHost,
                        .SystemNumber = kvp.Value.SystemNumber,
                        .Client = kvp.Value.Client,
                        .User = kvp.Value.User,
                        .Password = SecurityHelpers.DecryptString(kvp.Value.Password),
                        .Language = kvp.Value.Language
                    }
                Next

                _cachedSystems = decrypted

            Catch ex As Exception
                ' FIX: Keep old cache but log — do NOT throw when fallback exists
                If _cachedSystems Is Nothing Then
                    _cachedSystems = New Dictionary(Of String, SAPSystem)(
                        StringComparer.OrdinalIgnoreCase)
                End If
                System.Diagnostics.Debug.WriteLine(
                    "RefreshCache failed, using existing cache: " & ex.Message)
            End Try
        End SyncLock
    End Sub

    Private Shared Function GetCachedSystems() As Dictionary(Of String, SAPSystem)
        SyncLock _cacheLock
            If _cachedSystems Is Nothing Then
                RefreshCache()
            End If
            Return New Dictionary(Of String, SAPSystem)(
                _cachedSystems, StringComparer.OrdinalIgnoreCase)
        End SyncLock
    End Function

#End Region

#Region "Public API"

    Public Shared Function GetAvailableSystems() As List(Of String)
        Try
            Dim systems = GetCachedSystems()
            Dim names As New List(Of String)(systems.Keys)
            names.Sort()
            Return names
        Catch ex As Exception
            Throw New InvalidOperationException(
                "Failed to retrieve available SAP systems: " & ex.Message, ex)
        End Try
    End Function

    Public Shared Function HasSystems() As Boolean
        Try
            Return GetCachedSystems().Count > 0
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' FIX: Safely dispose _db even if Dispose throws
    ''' </summary>
    Public Shared Sub Shutdown()
        SyncLock _cacheLock
            Try
                If _db IsNot Nothing Then
                    _db.Dispose()
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(
                    "SAPConnection.Shutdown dispose error: " & ex.Message)
            Finally
                _db = Nothing          ' FIX: Always set Nothing in Finally
                _cachedSystems = Nothing
            End Try
        End SyncLock
    End Sub

#End Region

#Region "IDestinationConfiguration Implementation"

    ''' <summary>
    ''' FIX: Added required field validation before building parameters
    ''' </summary>
    Public Function GetParameters(destinationName As String) As RfcConfigParameters _
        Implements IDestinationConfiguration.GetParameters

        Try
            Dim systems = GetCachedSystems()

            If systems Is Nothing OrElse
               Not systems.ContainsKey(destinationName) Then
                Return Nothing
            End If

            Dim sys As SAPSystem = systems(destinationName)

            ' FIX: Validate required fields before use
            If String.IsNullOrWhiteSpace(sys.AppServerHost) Then
                System.Diagnostics.Debug.WriteLine(
                    "GetParameters: AppServerHost is empty for " & destinationName)
                Return Nothing
            End If
            If String.IsNullOrWhiteSpace(sys.User) Then
                System.Diagnostics.Debug.WriteLine(
                    "GetParameters: User is empty for " & destinationName)
                Return Nothing
            End If
            If String.IsNullOrWhiteSpace(sys.Client) Then
                System.Diagnostics.Debug.WriteLine(
                    "GetParameters: Client is empty for " & destinationName)
                Return Nothing
            End If

            ' Sanitize SystemNumber to exactly 2 digits
            Dim sysNum As String = If(
                sys.SystemNumber IsNot Nothing,
                sys.SystemNumber.Trim(), "00")

            Dim numericOnly As New System.Text.StringBuilder()
            For Each c As Char In sysNum
                If Char.IsDigit(c) Then numericOnly.Append(c)
            Next
            sysNum = numericOnly.ToString()

            Select Case sysNum.Length
                Case 0 : sysNum = "00"
                Case 1 : sysNum = "0" & sysNum
                Case Is > 2 : sysNum = sysNum.Substring(sysNum.Length - 2, 2)
            End Select

            Dim parms As New RfcConfigParameters()
            parms.Add(RfcConfigParameters.Name, destinationName)
            parms.Add(RfcConfigParameters.AppServerHost, sys.AppServerHost)
            parms.Add(RfcConfigParameters.SystemNumber, sysNum)
            parms.Add(RfcConfigParameters.Client, sys.Client)
            parms.Add(RfcConfigParameters.User, sys.User)
            parms.Add(RfcConfigParameters.Password,
                      If(sys.Password, String.Empty))
            parms.Add(RfcConfigParameters.Language,
                      If(sys.Language, "EN"))

            Return parms

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine(
                "SAPConnection.GetParameters error for '" &
                destinationName & "': " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function ChangeEventsSupported() As Boolean _
        Implements IDestinationConfiguration.ChangeEventsSupported
        Return False
    End Function

    Public Event ConfigurationChanged As RfcDestinationManager.ConfigurationChangeHandler _
        Implements IDestinationConfiguration.ConfigurationChanged

#End Region

End Class