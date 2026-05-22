Imports SAP.Middleware.Connector
Imports System.Data

Public Class TableAnalysis
    Private Async Sub btnFetch_Click(sender As Object, e As EventArgs) Handles btnFetch.Click
        btnFetch.Enabled = False
        DataGridView1.DataSource = Nothing
        lblStatus.Text = "Fetching data from SAP..."
        lblStatus.Refresh()
        Dim TableName As String = ComboBoxTable.SelectedItem?.ToString()

        Try
            ' Initialize SAP connection
            Dim sap As New SAPTableHelper("43.242.210.90", "02", "200", "Abhinavk", "Twxred@710", "EN")
            Dim dt As DataTable = New DataTable()


            If TableName = "T000" Then
                dt = Await Task.Run(Function() sap.FetchTableAllFields("T000", 50))
                DataGridView1.DataSource = dt
            ElseIf TableName = "USR02" Then
                ' Example: Fetch USR02 table
                dt = Await Task.Run(Function() sap.FetchTableAllFields("USR02", 50))
                DataGridView1.DataSource = dt
            ElseIf TableName = "E070" Then
                ' Example: Fetch USR02 table
                dt = Await Task.Run(Function() sap.FetchTableAllFields("E070", 150))
                DataGridView1.DataSource = dt
            ElseIf TableName = "RSUSR002" Then
                ' RSUSR002
                Dim dtRSUSR002 As DataTable = Await Task.Run(Function() sap.FetchRSUSR002(100))
                DataGridView1.DataSource = dtRSUSR002
            Else
                MessageBox.Show("Please select a valid table.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                lblStatus.Text = "No table selected."
            End If


            lblStatus.Text = "Data fetched successfully."
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "SAP RFC Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblStatus.Text = "Failed to fetch data."
        Finally
            btnFetch.Enabled = True
        End Try
    End Sub



End Class
