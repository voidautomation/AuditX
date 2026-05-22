Imports System.ComponentModel
Imports System.Linq
Imports System.Threading.Tasks
Imports SAP.Middleware.Connector

Public Class RFCSettingsForm

#Region "Fields"

    ''' <summary>In-memory cache of all loaded SAP systems with decrypted passwords.</summary>
    Private _systems As Dictionary(Of String, SAPSystem)

    ''' <summary>Database access layer.</summary>
    Private _db As SAPSystemDatabase

    ''' <summary>Tracks whether the form is currently in Add or Edit mode.</summary>
    Private _isEditing As Boolean = False

    ''' <summary>Holds the original system name before an edit or rename operation.</summary>
    Private _originalName As String = String.Empty

    ''' <summary>Background cancellation for status refresh.</summary>
    Private _statusCts As System.Threading.CancellationTokenSource = Nothing

#End Region

#Region "Form Load & Close"

    Private Sub RFCSettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Make form responsive
        MakeResponsive(Me)
        ITGC_AUDIT.EnsureDpiAware()
        Try
            _db = New SAPSystemDatabase("sap_systems.db")
            InitializeStatusGrid()
            LoadSystems()
            'Header Gradient
            GradientHelper.ApplyGradient(PanelHeader,
                                             ColorTranslator.FromHtml("#1F2A44"),
                                             ColorTranslator.FromHtml("#E3F2FD"),
                                         Drawing2D.LinearGradientMode.Horizontal)
        Catch ex As Exception
            MessageBox.Show(
                "Failed to initialize database:" & Environment.NewLine & ex.Message,
                "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            SetAllControlsEnabled(False)
        End Try
    End Sub

    Private Sub RFCSettingsForm_FormClosing(sender As Object,
                                            e As FormClosingEventArgs) Handles MyBase.FormClosing
        CancelStatusRefresh()

        If _db IsNot Nothing Then
            _db.Dispose()
            _db = Nothing
        End If
    End Sub

#End Region

#Region "Status DataGridView Setup"

    ''' <summary>
    ''' Initializes the dgvSystemStatus DataGridView columns and appearance.
    ''' </summary>
    Private Sub InitializeStatusGrid()
        dgvSystemStatus.Columns.Clear()
        dgvSystemStatus.AutoGenerateColumns = False
        dgvSystemStatus.ReadOnly = True
        dgvSystemStatus.AllowUserToAddRows = False
        dgvSystemStatus.AllowUserToDeleteRows = False
        dgvSystemStatus.RowHeadersVisible = False
        dgvSystemStatus.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvSystemStatus.MultiSelect = False
        dgvSystemStatus.BackgroundColor = System.Drawing.Color.White
        dgvSystemStatus.BorderStyle = BorderStyle.None
        dgvSystemStatus.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgvSystemStatus.GridColor = System.Drawing.Color.FromArgb(220, 220, 220)
        dgvSystemStatus.Font = New System.Drawing.Font("Segoe UI", 9)

        ' System Name
        Dim colName As New DataGridViewTextBoxColumn()
        colName.Name = "colSystemName"
        colName.HeaderText = "System Name"
        colName.DataPropertyName = "SystemName"
        colName.Width = 140
        colName.ReadOnly = True

        ' App Server Host
        Dim colHost As New DataGridViewTextBoxColumn()
        colHost.Name = "colHost"
        colHost.HeaderText = "App Server Host"
        colHost.DataPropertyName = "AppServerHost"
        colHost.Width = 180
        colHost.ReadOnly = True

        ' Client
        Dim colClient As New DataGridViewTextBoxColumn()
        colClient.Name = "colClient"
        colClient.HeaderText = "Client"
        colClient.DataPropertyName = "Client"
        colClient.Width = 70
        colClient.ReadOnly = True

        ' System Number
        Dim colSysNum As New DataGridViewTextBoxColumn()
        colSysNum.Name = "colSysNum"
        colSysNum.HeaderText = "Sys No."
        colSysNum.DataPropertyName = "SystemNumber"
        colSysNum.Width = 60
        colSysNum.ReadOnly = True

        ' User
        Dim colUser As New DataGridViewTextBoxColumn()
        colUser.Name = "colUser"
        colUser.HeaderText = "RFC User"
        colUser.DataPropertyName = "User"
        colUser.Width = 110
        colUser.ReadOnly = True

        ' Language
        Dim colLang As New DataGridViewTextBoxColumn()
        colLang.Name = "colLang"
        colLang.HeaderText = "Lang"
        colLang.DataPropertyName = "Language"
        colLang.Width = 55
        colLang.ReadOnly = True

        ' Status
        Dim colStatus As New DataGridViewTextBoxColumn()
        colStatus.Name = "colStatus"
        colStatus.HeaderText = "Status"
        colStatus.DataPropertyName = "Status"
        colStatus.Width = 90
        colStatus.ReadOnly = True

        ' Last Checked
        Dim colChecked As New DataGridViewTextBoxColumn()
        colChecked.Name = "colLastChecked"
        colChecked.HeaderText = "Last Checked"
        colChecked.DataPropertyName = "LastChecked"
        colChecked.Width = 140
        colChecked.ReadOnly = True
        colChecked.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

        dgvSystemStatus.Columns.AddRange(New DataGridViewColumn() {
            colName, colHost, colClient, colSysNum,
            colUser, colLang, colStatus, colChecked})

        ' Style the column headers
        dgvSystemStatus.ColumnHeadersDefaultCellStyle.BackColor =
            System.Drawing.Color.FromArgb(45, 45, 48)
        dgvSystemStatus.ColumnHeadersDefaultCellStyle.ForeColor =
            System.Drawing.Color.White
        dgvSystemStatus.ColumnHeadersDefaultCellStyle.Font =
            New System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold)
        dgvSystemStatus.ColumnHeadersHeightSizeMode =
            DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        dgvSystemStatus.ColumnHeadersHeight = 32
        dgvSystemStatus.EnableHeadersVisualStyles = False
    End Sub

#End Region

#Region "Load & Refresh"

    ''' <summary>
    ''' Loads all systems from the SQLite database into the in-memory cache
    ''' and refreshes the UI combo box and status grid.
    ''' </summary>
    Private Sub LoadSystems()
        If _db Is Nothing Then Return

        Try
            Dim rawSystems As Dictionary(Of String, SAPSystem) = _db.GetAllSystems()
            _systems = New Dictionary(Of String, SAPSystem)(StringComparer.OrdinalIgnoreCase)

            For Each kvp In rawSystems
                _systems(kvp.Key) = New SAPSystem With {
                    .AppServerHost = kvp.Value.AppServerHost,
                    .SystemNumber = kvp.Value.SystemNumber,
                    .Client = kvp.Value.Client,
                    .User = kvp.Value.User,
                    .Password = SecurityHelpers.DecryptString(kvp.Value.Password),
                    .Language = kvp.Value.Language
                }
            Next

            RefreshComboBox(String.Empty)
            ResetToReadOnlyMode()

            ' Cancel any running status check before repopulating
            CancelStatusRefresh()

            ' Unbind and rebuild the status grid
            PopulateStatusGrid()

            ' Start fresh background status check
            StartStatusRefresh()

        Catch ex As Exception
            MessageBox.Show(
                "Error loading SAP systems from database:" &
                Environment.NewLine & ex.Message,
                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Repopulates the combo box from the in-memory systems dictionary.
    ''' </summary>
    Private Sub RefreshComboBox(preferredSelection As String)
        cmbSystems.Items.Clear()

        If _systems Is Nothing OrElse _systems.Count = 0 Then
            ClearFields()
            Return
        End If

        For Each key As String In _systems.Keys
            cmbSystems.Items.Add(key)
        Next

        Dim idx As Integer = -1
        If Not String.IsNullOrEmpty(preferredSelection) Then
            idx = cmbSystems.FindStringExact(preferredSelection)
        End If
        cmbSystems.SelectedIndex = If(idx >= 0, idx, 0)
    End Sub

#End Region

#Region "Status Grid Population"

    ''' <summary>
    ''' Holds one row of data for the system status grid.
    ''' Uses Public Sub New() and Properties for BindingList compatibility.
    ''' </summary>
    Public Class SystemStatusRow
        Public Property SystemName As String = String.Empty
        Public Property AppServerHost As String = String.Empty
        Public Property Client As String = String.Empty
        Public Property SystemNumber As String = String.Empty
        Public Property User As String = String.Empty
        Public Property Language As String = String.Empty
        Public Property Status As String = String.Empty
        Public Property LastChecked As String = String.Empty
    End Class

    ''' <summary>
    ''' Builds the status grid rows from the in-memory systems dictionary.
    ''' All rows start with Status = "Checking..." until background ping completes.
    ''' CRITICAL: Always sets DataSource = Nothing before clearing or repopulating.
    ''' </summary>
    Private Sub PopulateStatusGrid()
        ' CRITICAL: Unbind BEFORE any row operations
        dgvSystemStatus.DataSource = Nothing

        If _systems Is Nothing OrElse _systems.Count = 0 Then
            lblStatusSummary.Text = "No systems configured."
            Return
        End If

        Dim bindingList As New BindingList(Of SystemStatusRow)()

        For Each kvp In _systems
            bindingList.Add(New SystemStatusRow With {
                .SystemName = kvp.Key,
                .AppServerHost = kvp.Value.AppServerHost,
                .Client = kvp.Value.Client,
                .SystemNumber = kvp.Value.SystemNumber,
                .User = kvp.Value.User,
                .Language = kvp.Value.Language,
                .Status = "Checking...",
                .LastChecked = ChrW(8212).ToString()
            })
        Next

        ' Rebind
        dgvSystemStatus.DataSource = bindingList

        lblStatusSummary.Text = "Checking " & bindingList.Count & " system(s)..."
        ApplyRowStyles()
    End Sub

    ''' <summary>
    ''' Updates the Status and LastChecked columns for a specific system row.
    ''' Modifies the BindingList items directly — the grid auto-refreshes.
    ''' Must be called on the UI thread.
    ''' </summary>
    Private Sub UpdateSystemStatusRow(systemName As String,
                                      isActive As Boolean,
                                      checkedAt As DateTime)
        Dim bindingList As BindingList(Of SystemStatusRow) = Nothing

        Try
            bindingList = TryCast(dgvSystemStatus.DataSource,
                                  BindingList(Of SystemStatusRow))
        Catch
            Return
        End Try

        If bindingList Is Nothing Then Return

        For i As Integer = 0 To bindingList.Count - 1
            If String.Equals(bindingList(i).SystemName, systemName,
                             StringComparison.OrdinalIgnoreCase) Then

                ' Update data
                bindingList(i).Status = If(isActive, "Active", "Inactive")
                bindingList(i).LastChecked = checkedAt.ToString("dd.MM.yyyy HH:mm:ss")

                ' Update visual style on the grid row
                If i < dgvSystemStatus.Rows.Count Then
                    Dim row As DataGridViewRow = dgvSystemStatus.Rows(i)

                    Dim backColor As System.Drawing.Color
                    Dim foreColor As System.Drawing.Color

                    If isActive Then
                        backColor = System.Drawing.Color.FromArgb(232, 255, 232)
                        foreColor = System.Drawing.Color.FromArgb(0, 120, 0)
                    Else
                        backColor = System.Drawing.Color.FromArgb(255, 232, 232)
                        foreColor = System.Drawing.Color.FromArgb(180, 0, 0)
                    End If

                    row.DefaultCellStyle.BackColor = backColor
                    row.DefaultCellStyle.ForeColor = foreColor

                    row.Cells("colStatus").Style.Font =
                        New System.Drawing.Font("Segoe UI", 9,
                                               System.Drawing.FontStyle.Bold)
                    row.Cells("colStatus").Style.ForeColor = foreColor

                    row.Cells("colStatus").Value = bindingList(i).Status
                    row.Cells("colLastChecked").Value = bindingList(i).LastChecked
                End If

                Exit For
            End If
        Next

        UpdateStatusSummary()
    End Sub

    ''' <summary>
    ''' Applies alternating row styles to rows that have not been coloured by status check.
    ''' </summary>
    Private Sub ApplyRowStyles()
        Dim colorWhite As System.Drawing.Color = System.Drawing.Color.White
        Dim colorAlt As System.Drawing.Color = System.Drawing.Color.FromArgb(248, 248, 252)

        For i As Integer = 0 To dgvSystemStatus.Rows.Count - 1
            Dim row As DataGridViewRow = dgvSystemStatus.Rows(i)
            Dim currentBack As System.Drawing.Color = row.DefaultCellStyle.BackColor

            ' Only apply alternating style if the row has not been coloured by status check
            If currentBack = colorWhite OrElse
               currentBack = colorAlt OrElse
               currentBack = System.Drawing.Color.Empty Then
                row.DefaultCellStyle.BackColor = If(i Mod 2 = 0, colorWhite, colorAlt)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Updates the summary label below the status grid.
    ''' </summary>
    Private Sub UpdateStatusSummary()
        Dim total As Integer = 0
        Dim active As Integer = 0
        Dim inactive As Integer = 0
        Dim checking As Integer = 0

        For Each row As DataGridViewRow In dgvSystemStatus.Rows
            total += 1
            Dim statusVal As String = If(row.Cells("colStatus").Value IsNot Nothing,
                                         row.Cells("colStatus").Value.ToString(),
                                         String.Empty)
            If statusVal = "Active" Then
                active += 1
            ElseIf statusVal = "Inactive" Then
                inactive += 1
            Else
                checking += 1
            End If
        Next

        If checking > 0 Then
            lblStatusSummary.Text = "Checking... (" & checking & " remaining)"
        Else
            lblStatusSummary.Text =
                "Total: " & total &
                "   |   Active: " & active &
                "   |   Inactive: " & inactive
        End If
    End Sub

#End Region

#Region "Background Status Check"

    ''' <summary>
    ''' Cancels any in-progress status refresh.
    ''' </summary>
    Private Sub CancelStatusRefresh()
        If _statusCts IsNot Nothing Then
            Try
                _statusCts.Cancel()
                _statusCts.Dispose()
            Catch
            End Try
            _statusCts = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Starts a background task that pings each configured SAP system
    ''' and updates the status grid row by row as results come in.
    ''' </summary>
    Private Async Sub StartStatusRefresh()
        CancelStatusRefresh()
        _statusCts = New System.Threading.CancellationTokenSource()
        Dim token As System.Threading.CancellationToken = _statusCts.Token

        If _systems Is Nothing OrElse _systems.Count = 0 Then Return

        ' Snapshot for background access
        Dim snapshot As New List(Of KeyValuePair(Of String, SAPSystem))(_systems)


        lblReferesh.Enabled = False

        Try
            For Each kvp In snapshot
                If token.IsCancellationRequested Then Exit For

                Dim sysName As String = kvp.Key
                Dim system As SAPSystem = kvp.Value
                Dim checkedAt As DateTime = DateTime.Now

                Dim isActive As Boolean = Await Task.Run(
                    Function() As Boolean
                        Return PingSystem(sysName, system)
                    End Function, token)

                If token.IsCancellationRequested Then Exit For

                If Not IsDisposed AndAlso Not token.IsCancellationRequested Then
                    UpdateSystemStatusRow(sysName, isActive, checkedAt)
                End If
            Next

        Catch ex As OperationCanceledException
            ' Normal when form closes or reloads

        Catch ex As Exception
            If Not IsDisposed Then
                System.Diagnostics.Debug.WriteLine(
                    "Status refresh error: " & ex.Message)
            End If

        Finally
            If Not IsDisposed Then
                lblReferesh.Enabled = True
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Attempts to ping a single SAP system using a temporary destination config.
    ''' Returns True if the ping succeeds.
    ''' Called from a background thread.
    ''' </summary>
    Private Function PingSystem(systemName As String,
                                system As SAPSystem) As Boolean
        Dim tempDestName As String = "STATUS_" & Guid.NewGuid().ToString("N")
        Dim tempCfg As SingleDestinationConfig = Nothing
        Dim originalRegistered As IDestinationConfiguration = Nothing
        Dim didUnregister As Boolean = False

        Try
            Dim parms As New RfcConfigParameters()
            parms.Add(RfcConfigParameters.Name, tempDestName)
            parms.Add(RfcConfigParameters.AppServerHost, system.AppServerHost)
            parms.Add(RfcConfigParameters.SystemNumber, system.SystemNumber)
            parms.Add(RfcConfigParameters.Client, system.Client)
            parms.Add(RfcConfigParameters.User, system.User)
            parms.Add(RfcConfigParameters.Password, system.Password)
            parms.Add(RfcConfigParameters.Language, system.Language)

            ' Temporarily take over NCo config if needed
            If RfcDestinationManager.IsDestinationConfigurationRegistered() Then
                Try
                    originalRegistered = SAPConnection.RegisteredConfig
                    If originalRegistered IsNot Nothing Then
                        RfcDestinationManager.UnregisterDestinationConfiguration(
                            originalRegistered)
                        didUnregister = True
                    End If
                Catch
                End Try
            End If

            tempCfg = New SingleDestinationConfig(tempDestName, parms)
            RfcDestinationManager.RegisterDestinationConfiguration(tempCfg)

            Dim dest As RfcDestination =
                RfcDestinationManager.GetDestination(tempDestName)
            dest.Ping()
            Return True

        Catch
            Return False

        Finally
            If tempCfg IsNot Nothing Then
                Try
                    RfcDestinationManager.UnregisterDestinationConfiguration(tempCfg)
                Catch
                End Try
            End If

            If didUnregister AndAlso originalRegistered IsNot Nothing Then
                Try
                    RfcDestinationManager.RegisterDestinationConfiguration(
                        originalRegistered)
                Catch
                End Try
            End If
        End Try
    End Function



#End Region

#Region "UI State Management"

    Private Sub ResetToReadOnlyMode()
        SetFieldsReadOnly(True)
        btnSave.Enabled = False
        btnSave.Visible = False
        btnCancelEdit.Visible = False
        btnDelete.Visible = False
        btnTestConnection.Enabled = True
        btnCancelEdit.Enabled = False
        btnAdd.Enabled = True
        btnEdit.Enabled = (cmbSystems.Items.Count > 0)
        btnDelete.Enabled = False
        cmbSystems.Enabled = True
        _isEditing = False
        _originalName = String.Empty
    End Sub

    Private Sub SetEditMode(enabled As Boolean)
        _isEditing = enabled
        SetFieldsReadOnly(Not enabled)
        btnSave.Visible = enabled
        btnSave.Enabled = enabled
        btnCancelEdit.Visible = enabled
        btnCancelEdit.Enabled = enabled
        btnCancelEdit.BackColor = If(enabled, Color.LightCoral, System.Drawing.Color.FromArgb(200, 200, 200))
        btnAdd.Enabled = Not enabled
        btnEdit.Enabled = Not enabled

        btnDelete.Visible = enabled
        btnDelete.Enabled = enabled AndAlso (cmbSystems.Items.Count > 0)
        btnTestConnection.Enabled = True
        cmbSystems.Enabled = Not enabled
    End Sub

    Private Sub SetFieldsReadOnly(isReadOnly As Boolean)
        txtSystemName.ReadOnly = isReadOnly
        txtAppServerHost.ReadOnly = isReadOnly
        txtSystemNumber.ReadOnly = isReadOnly
        txtClient.ReadOnly = isReadOnly
        txtUser.ReadOnly = isReadOnly
        txtPassword.ReadOnly = isReadOnly
        txtLanguage.ReadOnly = isReadOnly
    End Sub

    Private Sub SetAllControlsEnabled(enabled As Boolean)
        btnAdd.Enabled = enabled
        btnEdit.Enabled = enabled
        btnDelete.Enabled = enabled
        btnSave.Enabled = enabled
        btnCancelEdit.Enabled = enabled
        btnTestConnection.Enabled = enabled
        cmbSystems.Enabled = enabled
        lblReferesh.Enabled = enabled
    End Sub

    Private Sub ClearFields()
        txtSystemName.Text = String.Empty
        txtAppServerHost.Text = String.Empty
        txtSystemNumber.Text = String.Empty
        txtClient.Text = String.Empty
        txtUser.Text = String.Empty
        txtPassword.Text = String.Empty
        txtLanguage.Text = String.Empty
    End Sub

    Private Sub PopulateFields(systemName As String, system As SAPSystem)
        txtSystemName.Text = systemName
        txtAppServerHost.Text = system.AppServerHost
        txtSystemNumber.Text = system.SystemNumber
        txtClient.Text = system.Client
        txtUser.Text = system.User
        txtPassword.Text = system.Password
        txtLanguage.Text = system.Language
    End Sub

#End Region

#Region "Validation"

    Private Function ValidateAllFields() As Boolean
        Dim missing As New List(Of String)()

        If String.IsNullOrWhiteSpace(txtSystemName.Text) Then missing.Add("System Name")
        If String.IsNullOrWhiteSpace(txtAppServerHost.Text) Then missing.Add("App Server Host")
        If String.IsNullOrWhiteSpace(txtSystemNumber.Text) Then missing.Add("System Number")
        If String.IsNullOrWhiteSpace(txtClient.Text) Then missing.Add("Client")
        If String.IsNullOrWhiteSpace(txtUser.Text) Then missing.Add("User")
        If String.IsNullOrWhiteSpace(txtPassword.Text) Then missing.Add("Password")
        If String.IsNullOrWhiteSpace(txtLanguage.Text) Then missing.Add("Language")

        If missing.Count > 0 Then
            Dim lines As New System.Text.StringBuilder()
            lines.AppendLine("Please fill in the following required fields:")
            For Each field As String In missing
                lines.AppendLine("  " & Chr(149) & " " & field)
            Next
            MessageBox.Show(lines.ToString(), "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

#End Region

#Region "ComboBox Event"

    Private Sub cmbSystems_SelectedIndexChanged(sender As Object,
                                                e As EventArgs) Handles cmbSystems.SelectedIndexChanged
        If cmbSystems.SelectedItem Is Nothing Then
            ClearFields()
            Return
        End If

        Dim key As String = cmbSystems.SelectedItem.ToString()
        If _systems IsNot Nothing AndAlso _systems.ContainsKey(key) Then
            PopulateFields(key, _systems(key))
        End If
    End Sub

#End Region

#Region "Button Event Handlers"

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If _isEditing Then
            Dim answer As DialogResult = MessageBox.Show(
                "Discard unsaved changes and add a new system?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If answer = DialogResult.No Then Return
        End If

        ClearFields()
        _originalName = String.Empty
        SetEditMode(True)
        txtSystemName.Focus()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If cmbSystems.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a system to edit.", "Edit",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If _isEditing Then
            Dim answer As DialogResult = MessageBox.Show(
                "Discard unsaved changes?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If answer = DialogResult.No Then Return
        End If

        _originalName = cmbSystems.SelectedItem.ToString()
        SetEditMode(True)
        txtSystemName.Focus()
    End Sub

    Private Sub btnCancelEdit_Click(sender As Object,
                                    e As EventArgs) Handles btnCancelEdit.Click
        LoadSystems()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If _db Is Nothing Then
            MessageBox.Show("Database is not initialized. Please restart the application.",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Not ValidateAllFields() Then Return

        Dim newName As String = txtSystemName.Text.Trim()

        Dim isNewRecord As Boolean = String.IsNullOrEmpty(_originalName)
        Dim isRename As Boolean = Not isNewRecord AndAlso
                                   Not String.Equals(_originalName, newName,
                                                     StringComparison.OrdinalIgnoreCase)

        Try
            If (isNewRecord OrElse isRename) AndAlso _db.SystemExists(newName) Then
                MessageBox.Show(
                    "A system named '" & newName &
                    "' already exists. Please choose a different name.",
                    "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(
                "Error checking system name:" & Environment.NewLine & ex.Message,
                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim systemToSave As New SAPSystem With {
            .AppServerHost = txtAppServerHost.Text.Trim(),
            .SystemNumber = txtSystemNumber.Text.Trim(),
            .Client = txtClient.Text.Trim(),
            .User = txtUser.Text.Trim(),
            .Password = SecurityHelpers.EncryptString(txtPassword.Text.Trim()),
            .Language = txtLanguage.Text.Trim()
        }

        Try
            If isNewRecord Then
                _db.InsertSystem(newName, systemToSave)
            Else
                _db.UpdateSystem(_originalName, newName, systemToSave)
            End If
        Catch ex As Exception
            MessageBox.Show(
                "Error saving system to database:" & Environment.NewLine & ex.Message,
                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        LoadSystems()

        Dim idx As Integer = cmbSystems.FindStringExact(newName)
        If idx >= 0 Then cmbSystems.SelectedIndex = idx

        MessageBox.Show("System saved successfully.", "Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If _db Is Nothing Then
            MessageBox.Show("Database is not initialized. Please restart the application.",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If cmbSystems.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a system to delete.", "Delete",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim key As String = cmbSystems.SelectedItem.ToString()

        Dim confirm As DialogResult = MessageBox.Show(
            "Are you sure you want to delete '" & key & "'?",
            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirm = DialogResult.Yes Then
            Try
                Dim deleted As Boolean = _db.DeleteSystem(key)
                If deleted Then
                    LoadSystems()
                    MessageBox.Show("System deleted successfully.", "Delete",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("System was not found in the database.", "Delete",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show(
                    "Error deleting system:" & Environment.NewLine & ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region

#Region "RFC Test Connection"

    ''' <summary>
    ''' Temporary IDestinationConfiguration used only during connection test.
    ''' </summary>
    Private Class SingleDestinationConfig
        Implements IDestinationConfiguration

        Private ReadOnly _name As String
        Private ReadOnly _params As RfcConfigParameters

        Public Sub New(name As String, parms As RfcConfigParameters)
            _name = name
            _params = parms
        End Sub

        Public Function GetParameters(destinationName As String) As RfcConfigParameters _
            Implements IDestinationConfiguration.GetParameters
            If String.Equals(destinationName, _name, StringComparison.Ordinal) Then
                Return _params
            End If
            Return Nothing
        End Function

        Public Function ChangeEventsSupported() As Boolean _
            Implements IDestinationConfiguration.ChangeEventsSupported
            Return False
        End Function

        Public Event ConfigurationChanged As RfcDestinationManager.ConfigurationChangeHandler _
            Implements IDestinationConfiguration.ConfigurationChanged
    End Class

    Private Function TestConnectionBackground(
            parms As RfcConfigParameters,
            destName As String,
            originalRegistered As IDestinationConfiguration) As Tuple(Of Boolean, String)

        Dim cfg As SingleDestinationConfig = Nothing
        Dim unregisteredOriginal As Boolean = False

        Try
            If RfcDestinationManager.IsDestinationConfigurationRegistered() Then
                If originalRegistered IsNot Nothing Then
                    Try
                        RfcDestinationManager.UnregisterDestinationConfiguration(
                            originalRegistered)
                        Try
                            SAPConnection.RegisteredConfig = Nothing
                        Catch
                        End Try
                        unregisteredOriginal = True
                    Catch ex As Exception
                        Return New Tuple(Of Boolean, String)(
                            False,
                            "Unable to temporarily unregister the SAP configuration: " &
                            ex.Message)
                    End Try
                Else
                    Return New Tuple(Of Boolean, String)(
                        False,
                        "A destination configuration is already registered. " &
                        "Please restart the application and try again.")
                End If
            End If

            cfg = New SingleDestinationConfig(destName, parms)
            RfcDestinationManager.RegisterDestinationConfiguration(cfg)

            Dim dest As RfcDestination = RfcDestinationManager.GetDestination(destName)
            dest.Ping()

            Return New Tuple(Of Boolean, String)(True, "Connection successful.")

        Catch ex As RfcBaseException
            Return New Tuple(Of Boolean, String)(
                False, "RFC connection failed: " & ex.Message)
        Catch ex As Exception
            Return New Tuple(Of Boolean, String)(
                False, "Connection failed: " & ex.Message)
        Finally
            If cfg IsNot Nothing Then
                Try
                    RfcDestinationManager.UnregisterDestinationConfiguration(cfg)
                Catch
                End Try
            End If

            If unregisteredOriginal AndAlso originalRegistered IsNot Nothing Then
                Try
                    RfcDestinationManager.RegisterDestinationConfiguration(originalRegistered)
                    Try
                        SAPConnection.RegisteredConfig = originalRegistered
                    Catch
                    End Try
                Catch
                End Try
            End If
        End Try
    End Function

    Private Async Sub btnTestConnection_Click(sender As Object,
                                             e As EventArgs) Handles btnTestConnection.Click
        If Not ValidateAllFields() Then Return

        ' Snapshot states
        Dim prevTestConn As Boolean = btnTestConnection.Enabled
        Dim prevSave As Boolean = btnSave.Enabled
        Dim prevAdd As Boolean = btnAdd.Enabled
        Dim prevEdit As Boolean = btnEdit.Enabled
        Dim prevDelete As Boolean = btnDelete.Enabled
        Dim prevCmb As Boolean = cmbSystems.Enabled
        Dim prevRefresh As Boolean = lblReferesh.Enabled

        btnTestConnection.Enabled = False
        btnSave.Enabled = False
        btnAdd.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        cmbSystems.Enabled = False
        lblReferesh.Enabled = False

        Dim loading As New LoadingForm("Testing SAP RFC connection...")
        loading.Show(Me)
        loading.Refresh()

        Dim destName As String = "TEMP_" & Guid.NewGuid().ToString("N")
        Dim parms As New RfcConfigParameters()
        parms.Add(RfcConfigParameters.Name, destName)
        parms.Add(RfcConfigParameters.AppServerHost, txtAppServerHost.Text.Trim())
        parms.Add(RfcConfigParameters.SystemNumber, txtSystemNumber.Text.Trim())
        parms.Add(RfcConfigParameters.Client, txtClient.Text.Trim())
        parms.Add(RfcConfigParameters.User, txtUser.Text.Trim())
        parms.Add(RfcConfigParameters.Password, txtPassword.Text.Trim())
        parms.Add(RfcConfigParameters.Language, txtLanguage.Text.Trim())

        Dim originalRegistered As IDestinationConfiguration = Nothing
        If RfcDestinationManager.IsDestinationConfigurationRegistered() Then
            Try
                originalRegistered = SAPConnection.RegisteredConfig
            Catch
                originalRegistered = Nothing
            End Try
        End If

        Dim result As Tuple(Of Boolean, String) = Nothing

        Try
            result = Await Task.Run(
                Function() As Tuple(Of Boolean, String)
                    Return TestConnectionBackground(parms, destName, originalRegistered)
                End Function)
        Catch ex As Exception
            result = New Tuple(Of Boolean, String)(
                False, "Unexpected error: " & ex.Message)
        Finally
            Try
                If loading IsNot Nothing AndAlso Not loading.IsDisposed Then
                    If loading.InvokeRequired Then
                        loading.Invoke(New Action(Sub() loading.Close()))
                    Else
                        loading.Close()
                    End If
                    loading.Dispose()
                End If
            Catch
            End Try

            btnTestConnection.Enabled = prevTestConn
            btnSave.Enabled = prevSave
            btnAdd.Enabled = prevAdd
            btnEdit.Enabled = prevEdit
            btnDelete.Enabled = prevDelete
            cmbSystems.Enabled = prevCmb
            lblReferesh.Enabled = prevRefresh
        End Try

        If result IsNot Nothing Then
            If result.Item1 Then
                MessageBox.Show(result.Item2, "SAP RFC Test",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(result.Item2, "SAP RFC Test",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("Connection test did not return a result.", "SAP RFC Test",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub



    Private Sub lblReferesh_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblReferesh.LinkClicked

        Dim bindingList As BindingList(Of SystemStatusRow) = Nothing
        Try
            bindingList = TryCast(dgvSystemStatus.DataSource,
                                  BindingList(Of SystemStatusRow))
        Catch
        End Try

        If bindingList IsNot Nothing Then
            For Each item In bindingList
                item.Status = "Checking..."
                item.LastChecked = ChrW(8212).ToString()
            Next
        End If

        ' Reset visual styles on all rows
        For Each row As DataGridViewRow In dgvSystemStatus.Rows
            row.DefaultCellStyle.BackColor = System.Drawing.Color.White
            row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black
            row.DefaultCellStyle.Font =
                New System.Drawing.Font("Segoe UI", 9)
            row.Cells("colStatus").Style.Font =
                New System.Drawing.Font("Segoe UI", 9,
                                       System.Drawing.FontStyle.Regular)
            row.Cells("colStatus").Style.ForeColor = System.Drawing.Color.Black
        Next

        lblStatusSummary.Text = "Refreshing..."
        StartStatusRefresh()

    End Sub

#End Region

End Class

'==============================================================================
' Supporting Types
'==============================================================================

Public Class SAPSystem
    Public Property AppServerHost As String
    Public Property SystemNumber As String
    Public Property Client As String
    Public Property User As String
    Public Property Password As String
    Public Property Language As String
End Class

Public Class LoadingForm
    Inherits System.Windows.Forms.Form

    Private ReadOnly _lblMessage As System.Windows.Forms.Label
    Private ReadOnly _prg As System.Windows.Forms.ProgressBar

    Public Sub New(message As String)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.ControlBox = False
        Me.ShowInTaskbar = False
        Me.Width = 350
        Me.Height = 150
        Me.Text = "Please wait..."

        _lblMessage = New System.Windows.Forms.Label()
        _lblMessage.AutoSize = False
        _lblMessage.Width = 300
        _lblMessage.Height = 30
        _lblMessage.Left = 16
        _lblMessage.Top = 10
        _lblMessage.Text = message
        _lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        _prg = New System.Windows.Forms.ProgressBar()
        _prg.Style = ProgressBarStyle.Marquee
        _prg.MarqueeAnimationSpeed = 30
        _prg.Width = 300
        _prg.Height = 22
        _prg.Left = 16
        _prg.Top = 46

        Me.Controls.Add(_lblMessage)
        Me.Controls.Add(_prg)
    End Sub
End Class