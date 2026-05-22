Imports System.Data.SQLite
Imports System.IO

''' <summary>
''' Handles all SQLite database operations for SAP System configurations.
''' Uses System.Data.SQLite which fully supports Any CPU on .NET Framework 4.8
''' </summary>
Public Class SAPSystemDatabase
    Implements IDisposable

#Region "Fields & Constructor"

    Private ReadOnly _connectionString As String
    Private ReadOnly _dbPath As String
    Private _disposed As Boolean = False

    ''' <summary>
    ''' Initializes the database helper and ensures schema exists.
    ''' </summary>
    ''' <param name="dbFileName">SQLite database file name (default: sap_systems.db)</param>
    Public Sub New(Optional dbFileName As String = "sap_systems.db")
        ' Store the database in the application base directory
        _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFileName)
        _connectionString = "Data Source=" & _dbPath & ";Version=3;"
        InitializeDatabase()
    End Sub

#End Region

#Region "Database Initialization"

    ''' <summary>
    ''' Creates the database schema if it does not already exist.
    ''' </summary>
    Private Sub InitializeDatabase()
        Try
            Using conn As New SQLiteConnection(_connectionString)
                conn.Open()
                Using cmd As New SQLiteCommand(GetCreateTableScript(), conn)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New InvalidOperationException(
                "Failed to initialize SAP Systems database: " & ex.Message, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Returns the DDL script to create the SapSystems table.
    ''' </summary>
    Private Function GetCreateTableScript() As String
        Dim sb As New System.Text.StringBuilder()
        sb.AppendLine("CREATE TABLE IF NOT EXISTS SapSystems (")
        sb.AppendLine("    SystemName    TEXT NOT NULL PRIMARY KEY COLLATE NOCASE,")
        sb.AppendLine("    AppServerHost TEXT NOT NULL,")
        sb.AppendLine("    SystemNumber  TEXT NOT NULL,")
        sb.AppendLine("    Client        TEXT NOT NULL,")
        sb.AppendLine("    UserName      TEXT NOT NULL,")
        sb.AppendLine("    Password      TEXT NOT NULL,")
        sb.AppendLine("    Language      TEXT NOT NULL,")
        sb.AppendLine("    CreatedAt     TEXT NOT NULL DEFAULT (datetime('now')),")
        sb.AppendLine("    UpdatedAt     TEXT NOT NULL DEFAULT (datetime('now'))")
        sb.AppendLine(");")
        Return sb.ToString()
    End Function

#End Region

#Region "CRUD Operations"

    ''' <summary>
    ''' Retrieves all SAP systems from the database.
    ''' Passwords are returned in their encrypted (stored) form.
    ''' </summary>
    Public Function GetAllSystems() As Dictionary(Of String, SAPSystem)
        Dim result As New Dictionary(Of String, SAPSystem)(StringComparer.OrdinalIgnoreCase)

        Try
            Using conn As New SQLiteConnection(_connectionString)
                conn.Open()
                Dim sql As String =
                    "SELECT SystemName, AppServerHost, SystemNumber, " &
                    "Client, UserName, Password, Language " &
                    "FROM SapSystems ORDER BY SystemName;"

                Using cmd As New SQLiteCommand(sql, conn)
                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim systemName As String = reader.GetString(0)
                            Dim system As New SAPSystem With {
                                .AppServerHost = reader.GetString(1),
                                .SystemNumber = reader.GetString(2),
                                .Client = reader.GetString(3),
                                .User = reader.GetString(4),
                                .Password = reader.GetString(5),
                                .Language = reader.GetString(6)
                            }
                            result(systemName) = system
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New InvalidOperationException(
                "Failed to retrieve SAP systems: " & ex.Message, ex)
        End Try

        Return result
    End Function

    ''' <summary>
    ''' Retrieves a single SAP system by name.
    ''' Returns Nothing if not found.
    ''' Password is returned in encrypted (stored) form.
    ''' </summary>
    Public Function GetSystem(systemName As String) As SAPSystem
        If String.IsNullOrWhiteSpace(systemName) Then Return Nothing

        Try
            Using conn As New SQLiteConnection(_connectionString)
                conn.Open()
                Dim sql As String =
                    "SELECT AppServerHost, SystemNumber, Client, " &
                    "UserName, Password, Language FROM SapSystems " &
                    "WHERE SystemName = @SystemName;"

                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@SystemName", systemName)
                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Return New SAPSystem With {
                                .AppServerHost = reader.GetString(0),
                                .SystemNumber = reader.GetString(1),
                                .Client = reader.GetString(2),
                                .User = reader.GetString(3),
                                .Password = reader.GetString(4),
                                .Language = reader.GetString(5)
                            }
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New InvalidOperationException(
                "Failed to retrieve SAP system '" & systemName & "': " & ex.Message, ex)
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Checks whether a system name already exists in the database.
    ''' </summary>
    Public Function SystemExists(systemName As String) As Boolean
        If String.IsNullOrWhiteSpace(systemName) Then Return False

        Try
            Using conn As New SQLiteConnection(_connectionString)
                conn.Open()
                Using cmd As New SQLiteCommand(
                    "SELECT COUNT(1) FROM SapSystems WHERE SystemName = @SystemName;", conn)
                    cmd.Parameters.AddWithValue("@SystemName", systemName)
                    Dim count As Long = CLng(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            Throw New InvalidOperationException(
                "Failed to check system existence: " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Inserts a new SAP system. Password must already be encrypted.
    ''' </summary>
    Public Sub InsertSystem(systemName As String, system As SAPSystem)
        ValidateSystemArguments(systemName, system)

        Try
            Using conn As New SQLiteConnection(_connectionString)
                conn.Open()
                Dim sql As String =
                    "INSERT INTO SapSystems " &
                    "(SystemName, AppServerHost, SystemNumber, Client, " &
                    " UserName, Password, Language, CreatedAt, UpdatedAt) " &
                    "VALUES " &
                    "(@SystemName, @AppServerHost, @SystemNumber, @Client, " &
                    " @UserName, @Password, @Language, datetime('now'), datetime('now'));"

                Using cmd As New SQLiteCommand(sql, conn)
                    AddSystemParameters(cmd, systemName, system)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As SQLiteException When ex.ResultCode = SQLiteErrorCode.Constraint
            Throw New InvalidOperationException(
                "A system named '" & systemName & "' already exists.", ex)
        Catch ex As Exception
            Throw New InvalidOperationException(
                "Failed to insert SAP system '" & systemName & "': " & ex.Message, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Updates an existing SAP system.
    ''' Handles rename atomically via a transaction.
    ''' Password must already be encrypted.
    ''' </summary>
    Public Sub UpdateSystem(originalName As String,
                            newSystemName As String,
                            system As SAPSystem)
        ValidateSystemArguments(newSystemName, system)

        Try
            Using conn As New SQLiteConnection(_connectionString)
                conn.Open()
                Using transaction As SQLiteTransaction = conn.BeginTransaction()
                    Try
                        Dim isRename As Boolean = Not String.Equals(
                            originalName, newSystemName, StringComparison.OrdinalIgnoreCase)

                        If isRename Then
                            ' Insert under the new name first
                            Dim sqlInsert As String =
                                "INSERT INTO SapSystems " &
                                "(SystemName, AppServerHost, SystemNumber, Client, " &
                                " UserName, Password, Language, CreatedAt, UpdatedAt) " &
                                "VALUES " &
                                "(@SystemName, @AppServerHost, @SystemNumber, @Client, " &
                                " @UserName, @Password, @Language, datetime('now'), datetime('now'));"

                            Using cmdInsert As New SQLiteCommand(sqlInsert, conn, transaction)
                                AddSystemParameters(cmdInsert, newSystemName, system)
                                cmdInsert.ExecuteNonQuery()
                            End Using

                            ' Delete the old record
                            Using cmdDelete As New SQLiteCommand(
                                "DELETE FROM SapSystems WHERE SystemName = @OldName;",
                                conn, transaction)
                                cmdDelete.Parameters.AddWithValue("@OldName", originalName)
                                cmdDelete.ExecuteNonQuery()
                            End Using

                        Else
                            ' Simple in-place update — no rename
                            Dim sqlUpdate As String =
                                "UPDATE SapSystems SET " &
                                "AppServerHost = @AppServerHost, " &
                                "SystemNumber  = @SystemNumber,  " &
                                "Client        = @Client,        " &
                                "UserName      = @UserName,      " &
                                "Password      = @Password,      " &
                                "Language      = @Language,      " &
                                "UpdatedAt     = datetime('now') " &
                                "WHERE SystemName = @SystemName;"

                            Using cmdUpdate As New SQLiteCommand(sqlUpdate, conn, transaction)
                                AddSystemParameters(cmdUpdate, newSystemName, system)
                                cmdUpdate.ExecuteNonQuery()
                            End Using
                        End If

                        transaction.Commit()

                    Catch
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
        Catch ex As Exception
            Throw New InvalidOperationException(
                "Failed to update SAP system '" & originalName & "': " & ex.Message, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Deletes a SAP system by name.
    ''' Returns True if a record was deleted.
    ''' </summary>
    Public Function DeleteSystem(systemName As String) As Boolean
        If String.IsNullOrWhiteSpace(systemName) Then Return False

        Try
            Using conn As New SQLiteConnection(_connectionString)
                conn.Open()
                Using cmd As New SQLiteCommand(
                    "DELETE FROM SapSystems WHERE SystemName = @SystemName;", conn)
                    cmd.Parameters.AddWithValue("@SystemName", systemName)
                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        Catch ex As Exception
            Throw New InvalidOperationException(
                "Failed to delete SAP system '" & systemName & "': " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Returns the total count of SAP systems stored in the database.
    ''' </summary>
    Public Function GetSystemCount() As Integer
        Try
            Using conn As New SQLiteConnection(_connectionString)
                conn.Open()
                Using cmd As New SQLiteCommand(
                    "SELECT COUNT(1) FROM SapSystems;", conn)
                    Return CInt(cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Throw New InvalidOperationException(
                "Failed to count SAP systems: " & ex.Message, ex)
        End Try
    End Function

#End Region

#Region "Private Helpers"

    ''' <summary>
    ''' Adds standard SAP system parameters to a SQLite command.
    ''' </summary>
    Private Sub AddSystemParameters(cmd As SQLiteCommand,
                                systemName As String,
                                system As SAPSystem)
        cmd.Parameters.AddWithValue("@SystemName", systemName)
        cmd.Parameters.AddWithValue("@AppServerHost", system.AppServerHost)

        ' Always store SystemNumber as exactly 2 digits
        cmd.Parameters.AddWithValue(
        "@SystemNumber", SanitizeSystemNumber(system.SystemNumber))

        cmd.Parameters.AddWithValue("@Client", system.Client)
        cmd.Parameters.AddWithValue("@UserName", system.User)
        cmd.Parameters.AddWithValue("@Password", system.Password)
        cmd.Parameters.AddWithValue("@Language", system.Language)
    End Sub

    ''' <summary>
    ''' Validates that system name and system object are not null or empty.
    ''' </summary>
    Private Sub ValidateSystemArguments(systemName As String, system As SAPSystem)
        If String.IsNullOrWhiteSpace(systemName) Then
            Throw New ArgumentException(
                "System name cannot be null or empty.", "systemName")
        End If
        If system Is Nothing Then
            Throw New ArgumentNullException(
                "system", "SAP system object cannot be null.")
        End If
    End Sub

    ''' <summary>
    ''' Ensures SystemNumber is stored as exactly 2 numeric digits.
    ''' "0" → "00", "2" → "02", "00" → "00"
    ''' </summary>
    Private Function SanitizeSystemNumber(sysNum As String) As String
        If String.IsNullOrWhiteSpace(sysNum) Then Return "00"

        ' Strip non-numeric
        Dim sb As New System.Text.StringBuilder()
        For Each c As Char In sysNum.Trim()
            If Char.IsDigit(c) Then sb.Append(c)
        Next

        Dim result As String = sb.ToString()

        Select Case result.Length
            Case 0 : Return "00"
            Case 1 : Return "0" & result
            Case 2 : Return result
            Case Else : Return result.Substring(result.Length - 2, 2)
        End Select
    End Function

#End Region

#Region "IDisposable"

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            _disposed = True
        End If
    End Sub

#End Region

    ''' <summary>
    ''' One-time migration: ensures all stored SystemNumber values
    ''' are exactly 2 digits. Safe to call on every startup.
    ''' </summary>
    Public Sub MigrateSystemNumbers()
        Try
            Using conn As New SQLiteConnection(_connectionString)
                conn.Open()

                ' Read all current system numbers
                Dim toUpdate As New Dictionary(Of String, String)()

                Using cmd As New SQLiteCommand(
                    "SELECT SystemName, SystemNumber FROM SapSystems;", conn)
                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim name As String = reader.GetString(0)
                            Dim num As String = reader.GetString(1)
                            Dim sanitized As String = SanitizeSystemNumber(num)
                            If num <> sanitized Then
                                toUpdate(name) = sanitized
                            End If
                        End While
                    End Using
                End Using

                ' Apply corrections
                If toUpdate.Count > 0 Then
                    Using transaction As SQLiteTransaction =
                        conn.BeginTransaction()
                        Try
                            For Each kvp In toUpdate
                                Using cmd As New SQLiteCommand(
                                    "UPDATE SapSystems " &
                                    "SET SystemNumber = @Num, " &
                                    "    UpdatedAt = datetime('now') " &
                                    "WHERE SystemName = @Name;",
                                    conn, transaction)
                                    cmd.Parameters.AddWithValue(
                                        "@Num", kvp.Value)
                                    cmd.Parameters.AddWithValue(
                                        "@Name", kvp.Key)
                                    cmd.ExecuteNonQuery()
                                End Using
                            Next
                            transaction.Commit()
                            System.Diagnostics.Debug.WriteLine(
                                "MigrateSystemNumbers: fixed " &
                                toUpdate.Count & " record(s).")
                        Catch
                            transaction.Rollback()
                            Throw
                        End Try
                    End Using
                End If
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine(
                "MigrateSystemNumbers failed: " & ex.Message)
        End Try
    End Sub
End Class