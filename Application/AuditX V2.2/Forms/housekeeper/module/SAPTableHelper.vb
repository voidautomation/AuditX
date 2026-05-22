Imports SAP.Middleware.Connector
Imports System.Data

Public Class SAPTableHelper
    Private _destination As RfcDestination

    ' Field label dictionary (add tables/fields as needed)
    Private FieldLabelMap As New Dictionary(Of String, Dictionary(Of String, String)) From {
        {"USR02", New Dictionary(Of String, String) From {
            {"BNAME", "User Name"},
            {"GLTGV", "Valid From"},
            {"GLTGB", "Valid To"},
            {"USTYP", "User Type"},
            {"CLASS", "User Class"}
        }},
        {"T000", New Dictionary(Of String, String) From {
            {"MANDT", "Client"},
            {"MTEXT", "Name"},
            {"ORT01", "City"},
            {"CHANGEUSER", "Changed By"},
            {"CHANGEDATE", "Changed On"}
        }},
        {"E070", New Dictionary(Of String, String) From {
            {"TRKORR", "Request/Task"},
            {"TRFUNCTION", "Type"},
            {"TRSTATUS", "Status"},
            {"AS4USER", "Owner"},
            {"AS4DATE", "Date"}
        }}
    }

    Public Sub New(appServer As String, sysNo As String, client As String, user As String, password As String, lang As String)
        Dim parms As New RfcConfigParameters()
        parms.Add(RfcConfigParameters.Name, "SAP_CONN")
        parms.Add(RfcConfigParameters.AppServerHost, appServer)
        parms.Add(RfcConfigParameters.SystemNumber, sysNo)
        parms.Add(RfcConfigParameters.Client, client)
        parms.Add(RfcConfigParameters.User, user)
        parms.Add(RfcConfigParameters.Password, password)
        parms.Add(RfcConfigParameters.Language, lang)

        _destination = RfcDestinationManager.GetDestination(parms)
    End Sub

    ' Fetch all fields automatically
    Public Function FetchTableAllFields(tableName As String, Optional rowCount As Integer = 1000) As DataTable
        Dim repo As RfcRepository = _destination.Repository
        Dim rfcFunction As IRfcFunction = repo.CreateFunction("RFC_READ_TABLE")

        rfcFunction.SetValue("QUERY_TABLE", tableName)
        rfcFunction.SetValue("DELIMITER", "|")
        rfcFunction.SetValue("ROWCOUNT", rowCount)

        ' Fetch all fields dynamically
        Dim fieldsTable As IRfcTable = rfcFunction.GetTable("FIELDS")

        ' Append dummy to get all fields (SAP RFC_READ_TABLE allows empty FIELDS table to return all?)
        ' To be safe, fetch top 50 fields from dictionary if exists
        Dim labelMap As Dictionary(Of String, String) = Nothing
        FieldLabelMap.TryGetValue(tableName, labelMap)

        If labelMap IsNot Nothing Then
            For Each fld In labelMap.Keys
                fieldsTable.Append()
                fieldsTable.SetValue("FIELDNAME", fld)
            Next
        End If

        rfcFunction.Invoke(_destination)
        Dim resultTable As IRfcTable = rfcFunction.GetTable("DATA")

        ' Build DataTable with labels if available
        Dim dt As New DataTable()
        Dim columnNames As New List(Of String)

        If labelMap IsNot Nothing Then
            ' Use labels
            For Each fld In labelMap.Keys
                dt.Columns.Add(labelMap(fld))
                columnNames.Add(fld)
            Next
        Else
            ' Use raw SAP field names from FIELDS
            For i As Integer = 0 To fieldsTable.Count - 1
                Dim fldName As String = fieldsTable(i).GetString("FIELDNAME")
                dt.Columns.Add(fldName)
                columnNames.Add(fldName)
            Next
        End If

        ' Fill rows
        For i As Integer = 0 To resultTable.Count - 1
            Dim values = resultTable(i).GetString("WA").Split("|"c)
            ' Convert SAP dates
            For j As Integer = 0 To values.Length - 1
                If values(j).Length = 8 AndAlso IsNumeric(values(j)) Then
                    Dim tempDate As Date
                    If Date.TryParseExact(values(j), "yyyyMMdd", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, tempDate) Then
                        values(j) = tempDate.ToShortDateString()
                    End If
                End If
            Next
            dt.Rows.Add(values)
        Next

        Return dt
    End Function

    Public Function FetchRSUSR002(Optional rowCount As Integer = 1000) As DataTable
        ' Fetch user + roles info
        Dim users As DataTable = FetchTableAllFields("USR02", rowCount)
        Dim roles As DataTable = FetchTableAllFields("AGR_USERS", rowCount)

        ' Merge info: User -> Role
        Dim dt As New DataTable()
        dt.Columns.Add("User Name")
        dt.Columns.Add("User Type")
        dt.Columns.Add("Role Name")

        For Each uRow As DataRow In users.Rows
            Dim userName As String = uRow("User Name").ToString()
            Dim userType As String = uRow("User Type").ToString()
            Dim userRoles = roles.AsEnumerable().Where(Function(r) r("BNAME").ToString() = userName)
            For Each rRow In userRoles
                dt.Rows.Add(userName, userType, rRow("AGR_NAME"))
            Next
        Next

        Return dt
    End Function
End Class
