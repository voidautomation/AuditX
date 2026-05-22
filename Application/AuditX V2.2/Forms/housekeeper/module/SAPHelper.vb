Imports SAP.Middleware.Connector
Imports System.Data

Public Class SAPHelper

    ' ================================================================
    ' Fixed field list
    ' ================================================================
    Private Shared ReadOnly USR02Fields As String() = {
        "BNAME", "ERDAT", "GLTGB", "TRDAT",
        "CLASS", "USTYP", "UFLAG", "PWDCHGDATE"
    }

    ' ================================================================
    ' GetUSR02Data
    ' ================================================================
    Public Function GetUSR02Data(destinationName As String) As DataTable
        Try
            Dim destination As RfcDestination =
                RfcDestinationManager.GetDestination(destinationName)
            Dim repo As RfcRepository = destination.Repository
            Dim dt As DataTable = FetchUSR02(destination, repo)

            System.Diagnostics.Debug.WriteLine(
                "GetUSR02Data [" & destinationName & "] rows=" & dt.Rows.Count)

            Return dt
        Catch ex As Exception
            Throw New ApplicationException(
                "Error retrieving USR02 data from SAP system '" &
                destinationName & "': " & ex.Message, ex)
        End Try
    End Function

    ' ================================================================
    ' FetchUSR02
    ' ================================================================
    Private Function FetchUSR02(destination As RfcDestination,
                                repo As RfcRepository) As DataTable

        Dim rfcFunction As IRfcFunction =
            repo.CreateFunction("RFC_READ_TABLE")

        rfcFunction.SetValue("QUERY_TABLE", "USR02")
        rfcFunction.SetValue("DELIMITER", "|")
        rfcFunction.SetValue("ROWSKIPS", 0)
        rfcFunction.SetValue("ROWCOUNT", 0)

        ' Populate requested fields
        Dim fields As IRfcTable = rfcFunction.GetTable("FIELDS")
        For Each fieldName As String In USR02Fields
            fields.Append()
            fields(fields.Count - 1).SetValue("FIELDNAME", fieldName)
        Next

        rfcFunction.Invoke(destination)

        ' -------------------------------------------------------
        ' Read SAP-returned FIELDS metadata for offset extraction
        ' -------------------------------------------------------
        Dim fieldsOut As IRfcTable = rfcFunction.GetTable("FIELDS")
        Dim fieldMeta As New List(Of Tuple(Of String, Integer, Integer))()

        For i As Integer = 0 To fieldsOut.Count - 1
            fieldsOut.CurrentIndex = i
            Dim fname As String = fieldsOut.GetString("FIELDNAME").Trim()
            Dim offset As Integer = 0
            Dim length As Integer = 0
            Integer.TryParse(fieldsOut.GetString("OFFSET").Trim(), offset)
            Integer.TryParse(fieldsOut.GetString("LENGTH").Trim(), length)
            fieldMeta.Add(New Tuple(Of String, Integer, Integer)(
                fname, offset, length))
        Next

        System.Diagnostics.Debug.WriteLine(
            "FetchUSR02 fieldMeta count=" & fieldMeta.Count)

        Dim rawData As IRfcTable = rfcFunction.GetTable("DATA")

        System.Diagnostics.Debug.WriteLine(
            "FetchUSR02 raw rows from SAP=" & rawData.Count)

        ' -------------------------------------------------------
        ' Build result DataTable
        ' USR02Fields (8 columns) + PWDLGNDATE alias = 9 total
        ' Indices: 0-7 = fields, 8 = alias
        ' -------------------------------------------------------
        Dim dt As New DataTable()
        For Each name As String In USR02Fields
            dt.Columns.Add(name)
        Next
        dt.Columns.Add("PWDLGNDATE") ' alias at index 8

        Dim totalColumns As Integer = USR02Fields.Length + 1 ' = 9

        For Each row As IRfcStructure In rawData
            Dim wa As String = row.GetValue("WA").ToString()

            ' Declare correct size: indices 0 to totalColumns-1
            Dim values(totalColumns - 1) As String

            If fieldMeta.Count = USR02Fields.Length Then
                ' -----------------------------------------------
                ' Offset-based extraction — primary method
                ' -----------------------------------------------
                For i As Integer = 0 To fieldMeta.Count - 1
                    Dim off As Integer = fieldMeta(i).Item2
                    Dim len As Integer = fieldMeta(i).Item3
                    If off < wa.Length Then
                        Dim actualLen As Integer =
                            Math.Min(len, wa.Length - off)
                        values(i) = wa.Substring(off, actualLen).Trim()
                    Else
                        values(i) = String.Empty
                    End If
                Next
            Else
                ' -----------------------------------------------
                ' Fallback: pipe-split
                ' -----------------------------------------------
                Dim parts As String() = wa.Split("|"c)
                For i As Integer = 0 To USR02Fields.Length - 1
                    values(i) = If(i < parts.Length,
                                   parts(i).Trim(), String.Empty)
                Next
            End If

            ' PWDLGNDATE alias = same as PWDCHGDATE (index 7)
            values(8) = values(7)

            ' -----------------------------------------------
            ' FIX: Handle ERDAT = "00000000" or blank
            ' Do NOT drop these rows — substitute a safe past date
            ' so they pass the date filter in BuildResultTable
            ' -----------------------------------------------
            Dim erdatIdx As Integer = Array.IndexOf(USR02Fields, "ERDAT")
            If erdatIdx >= 0 Then
                Dim erdatVal As String = If(values(erdatIdx),
                                            String.Empty).Trim()
                If String.IsNullOrWhiteSpace(erdatVal) OrElse
                   erdatVal = "00000000" OrElse
                   erdatVal.Replace("0", "").Length = 0 Then
                    ' Substitute a safe past date (2 years ago)
                    ' so the row passes all date-based filters
                    values(erdatIdx) = "00:00:00"
                    'Date.Now.AddYears(-10).ToString("yyyyMMdd")
                End If
            End If

            dt.Rows.Add(values.Cast(Of Object)().ToArray())
        Next

        System.Diagnostics.Debug.WriteLine(
            "FetchUSR02 DataTable rows built=" & dt.Rows.Count)

        Return dt
    End Function

    ' ================================================================
    ' GetMultiSystemUSR02Data
    ' ================================================================
    Public Function GetMultiSystemUSR02Data(
        destinationNames As List(Of String)) As Dictionary(Of String, DataTable)

        If destinationNames Is Nothing OrElse
           destinationNames.Count = 0 Then
            Throw New ArgumentException(
                "No systems specified for multi-system analysis.")
        End If

        Dim results As New Dictionary(Of String, DataTable)(
            StringComparer.OrdinalIgnoreCase)

        For Each destName As String In destinationNames
            Try
                Dim dt As DataTable = GetUSR02Data(destName)
                results(destName) = dt
            Catch ex As Exception
                Dim errorDt As New DataTable()
                errorDt.Columns.Add("ERROR")
                errorDt.Rows.Add("Failed: " & ex.Message)
                results(destName) = errorDt
            End Try
        Next

        Return results
    End Function

    ' ================================================================
    ' LockUser
    ' ================================================================
    Public Function LockUser(destinationName As String,
                             userName As String,
                             ByRef outMessage As String) As Boolean
        outMessage = String.Empty
        If String.IsNullOrWhiteSpace(destinationName) OrElse
           String.IsNullOrWhiteSpace(userName) Then
            outMessage = "Destination or username is empty."
            Return False
        End If

        Try
            Dim destination As RfcDestination =
                RfcDestinationManager.GetDestination(destinationName)
            Dim repo As RfcRepository = destination.Repository

            Dim lockFunc As IRfcFunction =
                repo.CreateFunction("BAPI_USER_LOCK")
            lockFunc.SetValue("USERNAME", userName)
            lockFunc.Invoke(destination)

            Dim hasError As Boolean = False
            Try
                Dim retTable As IRfcTable = lockFunc.GetTable("RETURN")
                Dim messages As New List(Of String)()
                For i As Integer = 0 To retTable.Count - 1
                    retTable.CurrentIndex = i
                    Dim typ As String = retTable.GetString("TYPE").Trim()
                    Dim msg As String = retTable.GetString("MESSAGE").Trim()
                    If Not String.IsNullOrWhiteSpace(msg) Then
                        messages.Add(msg)
                    End If
                    If typ.ToUpperInvariant() = "E" OrElse
                       typ.ToUpperInvariant() = "A" Then
                        hasError = True
                    End If
                Next
                outMessage = String.Join("; ", messages)
            Catch
            End Try

            If hasError Then Return False

            Try
                Dim commit As IRfcFunction =
                    repo.CreateFunction("BAPI_TRANSACTION_COMMIT")
                commit.SetValue("WAIT", "X")
                commit.Invoke(destination)
            Catch ex As Exception
                outMessage &= " (Commit failed: " & ex.Message & ")"
            End Try

            If String.IsNullOrWhiteSpace(outMessage) Then
                outMessage = "User locked successfully."
            End If
            Return True

        Catch ex As RfcBaseException
            outMessage = "RFC error: " & ex.Message
            Return False
        Catch ex As Exception
            outMessage = "Error: " & ex.Message
            Return False
        End Try
    End Function

    ' ================================================================
    ' UserGroupExists
    ' ================================================================
    Public Function UserGroupExists(destinationName As String,
                                    groupName As String,
                                    ByRef outMessage As String) As Boolean
        outMessage = String.Empty
        If String.IsNullOrWhiteSpace(destinationName) OrElse
           String.IsNullOrWhiteSpace(groupName) Then
            outMessage = "Destination or group name is empty."
            Return False
        End If

        Dim trimmedGroup As String = groupName.Trim().ToUpperInvariant()
        Dim debugLog As New System.Text.StringBuilder()

        Try
            Dim destination As RfcDestination =
                RfcDestinationManager.GetDestination(destinationName)
            Dim repo As RfcRepository = destination.Repository

            ' Method 1: USGRPT
            Try
                Dim fn1 As IRfcFunction =
                    repo.CreateFunction("RFC_READ_TABLE")
                fn1.SetValue("QUERY_TABLE", "USGRPT")
                fn1.SetValue("DELIMITER", "|")
                fn1.SetValue("ROWCOUNT", 1)
                Dim f1 As IRfcTable = fn1.GetTable("FIELDS")
                f1.Append()
                f1(0).SetValue("FIELDNAME", "CLASS")
                Dim o1 As IRfcTable = fn1.GetTable("OPTIONS")
                o1.Append()
                o1(0).SetValue("TEXT", "CLASS = '" & trimmedGroup & "'")
                fn1.Invoke(destination)
                If fn1.GetTable("DATA").Count > 0 Then Return True
                debugLog.AppendLine("Method 1 (USGRPT): No rows found.")
            Catch ex As Exception
                debugLog.AppendLine("Method 1 failed: " & ex.Message)
            End Try

            ' Method 2: USR02
            Try
                Dim fn2 As IRfcFunction =
                    repo.CreateFunction("RFC_READ_TABLE")
                fn2.SetValue("QUERY_TABLE", "USR02")
                fn2.SetValue("DELIMITER", "|")
                fn2.SetValue("ROWCOUNT", 1)
                Dim f2 As IRfcTable = fn2.GetTable("FIELDS")
                f2.Append()
                f2(0).SetValue("FIELDNAME", "CLASS")
                Dim o2 As IRfcTable = fn2.GetTable("OPTIONS")
                o2.Append()
                o2(0).SetValue("TEXT", "CLASS = '" & trimmedGroup & "'")
                fn2.Invoke(destination)
                If fn2.GetTable("DATA").Count > 0 Then Return True
                debugLog.AppendLine("Method 2 (USR02): No rows found.")
            Catch ex As Exception
                debugLog.AppendLine("Method 2 failed: " & ex.Message)
            End Try

            ' Method 3: USGRP/USERGROUP
            Try
                Dim fn3 As IRfcFunction =
                    repo.CreateFunction("RFC_READ_TABLE")
                fn3.SetValue("QUERY_TABLE", "USGRP")
                fn3.SetValue("DELIMITER", "|")
                fn3.SetValue("ROWCOUNT", 1)
                Dim f3 As IRfcTable = fn3.GetTable("FIELDS")
                f3.Append()
                f3(0).SetValue("FIELDNAME", "USERGROUP")
                Dim o3 As IRfcTable = fn3.GetTable("OPTIONS")
                o3.Append()
                o3(0).SetValue("TEXT", "USERGROUP = '" & trimmedGroup & "'")
                fn3.Invoke(destination)
                If fn3.GetTable("DATA").Count > 0 Then Return True
                debugLog.AppendLine("Method 3 (USGRP/USERGROUP): No rows.")
            Catch ex As Exception
                debugLog.AppendLine("Method 3 failed: " & ex.Message)
            End Try

            ' Method 4: USGRP/CLASS
            Try
                Dim fn4 As IRfcFunction =
                    repo.CreateFunction("RFC_READ_TABLE")
                fn4.SetValue("QUERY_TABLE", "USGRP")
                fn4.SetValue("DELIMITER", "|")
                fn4.SetValue("ROWCOUNT", 1)
                Dim f4 As IRfcTable = fn4.GetTable("FIELDS")
                f4.Append()
                f4(0).SetValue("FIELDNAME", "CLASS")
                Dim o4 As IRfcTable = fn4.GetTable("OPTIONS")
                o4.Append()
                o4(0).SetValue("TEXT", "CLASS = '" & trimmedGroup & "'")
                fn4.Invoke(destination)
                If fn4.GetTable("DATA").Count > 0 Then Return True
                debugLog.AppendLine("Method 4 (USGRP/CLASS): No rows.")
            Catch ex As Exception
                debugLog.AppendLine("Method 4 failed: " & ex.Message)
            End Try

            outMessage = "User group '" & trimmedGroup &
                         "' not found." & Environment.NewLine &
                         debugLog.ToString()
            Return False

        Catch ex As RfcBaseException
            outMessage = "RFC error: " & ex.Message
            Return False
        Catch ex As Exception
            outMessage = "Error: " & ex.Message
            Return False
        End Try
    End Function

    ' ================================================================
    ' ChangeUserGroup
    ' ================================================================
    Public Function ChangeUserGroup(destinationName As String,
                                    userName As String,
                                    newGroup As String,
                                    ByRef outMessage As String) As Boolean
        outMessage = String.Empty
        If String.IsNullOrWhiteSpace(destinationName) OrElse
           String.IsNullOrWhiteSpace(userName) Then
            outMessage = "Destination or username is empty."
            Return False
        End If

        Try
            Dim destination As RfcDestination =
                RfcDestinationManager.GetDestination(destinationName)
            Dim repo As RfcRepository = destination.Repository

            Dim changeFunc As IRfcFunction =
                repo.CreateFunction("BAPI_USER_CHANGE")
            changeFunc.SetValue("USERNAME", userName)

            Dim logonData As IRfcStructure =
                changeFunc.GetStructure("LOGONDATA")
            logonData.SetValue("CLASS",
                               newGroup.Trim().ToUpperInvariant())

            Dim logonDataX As IRfcStructure =
                changeFunc.GetStructure("LOGONDATAX")
            logonDataX.SetValue("CLASS", "X")

            changeFunc.Invoke(destination)

            Dim hasError As Boolean = False
            Try
                Dim retTable As IRfcTable = changeFunc.GetTable("RETURN")
                Dim messages As New List(Of String)()
                For i As Integer = 0 To retTable.Count - 1
                    retTable.CurrentIndex = i
                    Dim typ As String = retTable.GetString("TYPE").Trim()
                    Dim msg As String = retTable.GetString("MESSAGE").Trim()
                    If Not String.IsNullOrWhiteSpace(msg) Then
                        messages.Add(msg)
                    End If
                    If typ.ToUpperInvariant() = "E" OrElse
                       typ.ToUpperInvariant() = "A" Then
                        hasError = True
                    End If
                Next
                outMessage = String.Join("; ", messages)
            Catch
            End Try

            If hasError Then Return False

            Try
                Dim commit As IRfcFunction =
                    repo.CreateFunction("BAPI_TRANSACTION_COMMIT")
                commit.SetValue("WAIT", "X")
                commit.Invoke(destination)
            Catch ex As Exception
                outMessage &= " (Commit failed: " & ex.Message & ")"
            End Try

            If String.IsNullOrWhiteSpace(outMessage) Then
                outMessage = "User group changed to '" & newGroup & "'."
            End If
            Return True

        Catch ex As RfcBaseException
            outMessage = "RFC error: " & ex.Message
            Return False
        Catch ex As Exception
            outMessage = "Error: " & ex.Message
            Return False
        End Try
    End Function

    ' ================================================================
    ' ChangeUserValidity
    ' ================================================================
    Public Function ChangeUserValidity(destinationName As String,
                                       userName As String,
                                       validToDate As String,
                                       ByRef outMessage As String) As Boolean
        outMessage = String.Empty
        If String.IsNullOrWhiteSpace(destinationName) OrElse
           String.IsNullOrWhiteSpace(userName) Then
            outMessage = "Destination or username is empty."
            Return False
        End If

        Dim parsedDate As Date
        If Not Date.TryParseExact(validToDate,
                                   "dd.MM.yyyy",
                                   Globalization.CultureInfo.InvariantCulture,
                                   Globalization.DateTimeStyles.None,
                                   parsedDate) Then
            outMessage = "Invalid date format. Expected dd.MM.yyyy, got: " &
                         validToDate
            Return False
        End If

        Try
            Dim destination As RfcDestination =
                RfcDestinationManager.GetDestination(destinationName)
            Dim repo As RfcRepository = destination.Repository

            Dim changeFunc As IRfcFunction =
                repo.CreateFunction("BAPI_USER_CHANGE")
            changeFunc.SetValue("USERNAME", userName)

            Dim logonData As IRfcStructure =
                changeFunc.GetStructure("LOGONDATA")
            logonData.SetValue("GLTGB", parsedDate.ToString("yyyyMMdd"))

            Dim logonDataX As IRfcStructure =
                changeFunc.GetStructure("LOGONDATAX")
            logonDataX.SetValue("GLTGB", "X")

            changeFunc.Invoke(destination)

            Dim hasError As Boolean = False
            Try
                Dim retTable As IRfcTable = changeFunc.GetTable("RETURN")
                Dim messages As New List(Of String)()
                For i As Integer = 0 To retTable.Count - 1
                    retTable.CurrentIndex = i
                    Dim typ As String = retTable.GetString("TYPE").Trim()
                    Dim msg As String = retTable.GetString("MESSAGE").Trim()
                    If Not String.IsNullOrWhiteSpace(msg) Then
                        messages.Add(msg)
                    End If
                    If typ.ToUpperInvariant() = "E" OrElse
                       typ.ToUpperInvariant() = "A" Then
                        hasError = True
                    End If
                Next
                outMessage = String.Join("; ", messages)
            Catch
            End Try

            If hasError Then Return False

            Try
                Dim commit As IRfcFunction =
                    repo.CreateFunction("BAPI_TRANSACTION_COMMIT")
                commit.SetValue("WAIT", "X")
                commit.Invoke(destination)
            Catch ex As Exception
                outMessage &= " (Commit failed: " & ex.Message & ")"
            End Try

            If String.IsNullOrWhiteSpace(outMessage) Then
                outMessage = "Validity date changed to '" &
                             validToDate & "'."
            End If
            Return True

        Catch ex As RfcBaseException
            outMessage = "RFC error: " & ex.Message
            Return False
        Catch ex As Exception
            outMessage = "Error: " & ex.Message
            Return False
        End Try
    End Function

End Class