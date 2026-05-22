Imports System.Data.Entity.Core.Common.EntitySql
Imports System.Drawing.Text
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Xml
Imports Microsoft.Office.Interop
Imports SAP.Middleware.Connector


Public Class Home

    '==============================
    ' HOME SELECTION Variables
    '==============================
    Private profilePopup As Panel
    Private lblPopupName As Label
    Private lblPopupEmail As Label
    Private lblPopupWindowsID As Label
    Private lblPopupExpiry As Label

    ' === Dictionary to hold system names and their corresponding system IDs ===
    Private systemIdMap As New Dictionary(Of String, String)
    Private helpProvider As New ErrorProvider()
    ' === Sidebar and animation control variables ===
    'Private monitor As SystemMonitor
    Private monitor As New SystemMonitor()
    Private isCollapsed As Boolean = True
    Private isExpanded As Boolean = False
    Private expandedWidth As Integer = 285
    Private collapsedWidth As Integer = 0
    Private sidebarSpeed As Integer = 10 ' Change this for faster/slower animation
    Private isAnimating As Boolean = False
    Private animationStep As Integer = 0
    Private expandedHeight As Integer = 500 ' You can dynamically fetch Me.Height or desired value
    Private collapsedSize As New Size(0, 0)
    Private targetSize As Size
    Private animationSpeed As Integer = 30

    ' Add these fields at the top of the class
    Private controlDefinitions As New Dictionary(Of String, String)
    Private systemDefinitions As New Dictionary(Of String, String)

    Public Property UserCompany As String
    Public Property UserName As String
    Public Property UserFullName As String
    Public Property UserEmail As String
    Public Property LicenseIssueDate As String
    Public Property LicenseExpiryDate As String

#Region "Appendix variables"
    '==============================
    ' APPENDIX SELECTION Variables
    '==============================
    'Dim xmlFilePath As String = Application.StartupPath & "\ITGCControls.xml"
    'Dim xmlFilePath As String = Application.StartupPath & My.Settings.ITGCControl
    ' Safer and cleaner
    Dim xmlFilePath As String = Path.Combine(Application.StartupPath, My.Settings.ITGCControl)
    Dim dataTable As New DataTable()
    Dim controlActions As New List(Of String)
    Dim isEditing As Boolean = False
    Dim unsavedChanges As Boolean = False

    ' UI Elements
    Dim WithEvents txtNoteBox As New RichTextBox()
    Dim lblNoteTitle As New Label()
    Dim WithEvents btnEditNote As New Button()

    ' Notification status
    Private notifyTimer As New Timer()
    Private notifyOpacity As Double = 1.0
    Private notifyIsFading As Boolean = False
#End Region


#Region "Logs variables"
    Private logFolderPath As String = "C:\Void\Logs"
#End Region
    Public Sub New()
        InitializeComponent()
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
    End Sub

    Private currentControl As UserControl = Nothing

    Private Sub LoadControl(control As UserControl, Optional senderButton As Button = Nothing)
        If currentControl IsNot Nothing Then
            TabSettings.Controls.Remove(currentControl)
            currentControl.Dispose()
        End If
        currentControl = control
        control.Dock = DockStyle.Fill
        TabSettings.Controls.Add(control)
    End Sub


    'Flags to prevent reloading
    Private homeLoaded As Boolean = False
    Private appendixLoaded As Boolean = False
    Private logsLoaded As Boolean = False
    Private settingsLoaded As Boolean = False
    Private instructionLoaded As Boolean = False
    Private housekeepingLoaded As Boolean = False
    Private aboutLoaded As Boolean = False

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ITGC_AUDIT.EnsureDpiAware()
        CreateProfilePopup()
        RegisterGlobalClickHandler(Me)
        'Optional: load Home tab content if it is default
        If TabControlMain.SelectedTab Is TabHome Then
            LoadHomeTab()
        End If

        'Header Gradient
        GradientHelper.ApplyGradient(PanelHeader,
                                         ColorTranslator.FromHtml("#1F2A44"),
                                         ColorTranslator.FromHtml("#E3F2FD"),
                                     Drawing2D.LinearGradientMode.Horizontal)
        'Body Gradient

        'Footer Gradient
        GradientHelper.ApplyGradient(PanelFooter,
                                     ColorTranslator.FromHtml("#F5F6FA"),
                                     Color.White,
                                     Drawing2D.LinearGradientMode.Vertical)

    End Sub

    '______________________________________User Profile Click ______________________________________

    Private Sub CreateProfilePopup()

        profilePopup = New Panel()
        profilePopup.Size = New Size(350, 350)
        profilePopup.BackColor = Color.White
        profilePopup.Visible = False
        profilePopup.BorderStyle = BorderStyle.None
        profilePopup.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        profilePopup.Location = New Point(pbUserProfile.Right - 350, pbUserProfile.Bottom + 8)

        ' ===== Rounded Card =====
        Dim path As New Drawing2D.GraphicsPath()
        path.AddArc(0, 0, 20, 20, 180, 90)
        path.AddArc(profilePopup.Width - 20, 0, 20, 20, 270, 90)
        path.AddArc(profilePopup.Width - 20, profilePopup.Height - 20, 20, 20, 0, 90)
        path.AddArc(0, profilePopup.Height - 20, 20, 20, 90, 90)
        path.CloseFigure()
        profilePopup.Region = New Region(path)

        ' ===== Header Background =====
        Dim headerPanel As New Panel()
        headerPanel.BackColor = Color.FromArgb(210, 220, 235)
        headerPanel.Dock = DockStyle.Fill

        ' ===== Profile Image =====
        Dim profileImage As New PictureBox()
        profileImage.Size = New Size(80, 80)
        profileImage.SizeMode = PictureBoxSizeMode.Zoom
        profileImage.Location = New Point((profilePopup.Width - 80) \ 2, 20)

        Try
            profileImage.Image = pbUserProfile.Image
        Catch
            profileImage.BackColor = Color.LightGray
        End Try

        ' Make circular
        Dim imgPath As New Drawing2D.GraphicsPath()
        imgPath.AddEllipse(0, 0, profileImage.Width, profileImage.Height)
        profileImage.Region = New Region(imgPath)

        ' ===== Name Label =====
        lblPopupName = New Label()
        lblPopupName.Text = UserFullName
        lblPopupName.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblPopupName.TextAlign = ContentAlignment.MiddleCenter
        lblPopupName.AutoSize = False
        lblPopupName.Size = New Size(profilePopup.Width, 50)
        lblPopupName.Location = New Point(0, 110)

        ' ===== Email Label =====
        lblPopupEmail = New Label()
        lblPopupEmail.Text = UserEmail
        lblPopupEmail.Font = New Font("Segoe UI", 10)
        lblPopupEmail.ForeColor = Color.FromArgb(80, 80, 80)
        lblPopupEmail.TextAlign = ContentAlignment.MiddleCenter
        lblPopupEmail.AutoSize = False
        lblPopupEmail.Size = New Size(profilePopup.Width - 20, 60)
        lblPopupEmail.Location = New Point(10, lblPopupName.Bottom + 5)


        ' ==============================
        ' SEPARATOR (NOW VISIBLE)
        ' ==============================
        Dim separator As New Panel()
        separator.BackColor = Color.FromArgb(0, 0, 0)
        separator.Size = New Size(profilePopup.Width - 40, 1)
        separator.Location = New Point(20, lblPopupEmail.Bottom + 15)

        ' ===== Windows ID Label =====
        lblPopupWindowsID = New Label()
        ' lblPopupWindowsID.Text = "Current Date: " & DateTime.Now.ToString("dd MMM, yyyy")
        lblPopupWindowsID.Text = "Windows ID: " & Environment.UserName
        lblPopupWindowsID.Font = New Font("Segoe UI", 10)
        lblPopupWindowsID.ForeColor = Color.FromArgb(80, 80, 80)
        lblPopupWindowsID.TextAlign = ContentAlignment.MiddleLeft
        lblPopupWindowsID.AutoSize = False
        lblPopupWindowsID.Size = New Size(profilePopup.Width, 30)
        lblPopupWindowsID.Location = New Point(10, separator.Bottom + 10)

        ' ===== Expiry date Label =====
        Dim expiry As DateTime

        If Not DateTime.TryParse(LicenseExpiryDate, expiry) Then

            Try
                expiry = DateTime.ParseExact(LicenseExpiryDate, "yyyyMMdd", Nothing)
            Catch
                expiry = DateTime.Now
            End Try

        End If


        lblPopupExpiry = New Label()
        lblPopupExpiry.Text = "Valid till: " & expiry.ToString("dd MMM, yyyy")
        lblPopupExpiry.Font = New Font("Segoe UI", 10)
        lblPopupExpiry.ForeColor = Color.FromArgb(80, 80, 80)
        lblPopupExpiry.TextAlign = ContentAlignment.MiddleLeft
        lblPopupExpiry.AutoSize = False
        lblPopupExpiry.Size = New Size(profilePopup.Width, 30)
        lblPopupExpiry.Location = New Point(10, lblPopupWindowsID.Bottom + 5)


        headerPanel.Controls.Add(profileImage)
        headerPanel.Controls.Add(lblPopupName)
        headerPanel.Controls.Add(lblPopupEmail)
        headerPanel.Controls.Add(separator)
        headerPanel.Controls.Add(lblPopupWindowsID)
        headerPanel.Controls.Add(lblPopupExpiry)

        profilePopup.Controls.Add(headerPanel)

        Me.Controls.Add(profilePopup)

    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)

        If profilePopup IsNot Nothing AndAlso profilePopup.Visible Then

            ' Fix: qualify Cursor.Position with the class name to avoid accessing a shared member through an instance.
            Dim cursorPos = Me.PointToClient(System.Windows.Forms.Cursor.Position)

            If Not profilePopup.Bounds.Contains(cursorPos) AndAlso
           Not pbUserProfile.Bounds.Contains(cursorPos) Then

                profilePopup.Visible = False

            End If
        End If
    End Sub

    Private Sub RegisterGlobalClickHandler(parent As Control)

        For Each ctrl As Control In parent.Controls
            AddHandler ctrl.MouseDown, AddressOf GlobalMouseDown

            If ctrl.HasChildren Then
                RegisterGlobalClickHandler(ctrl)
            End If
        Next

    End Sub

    Private Sub GlobalMouseDown(sender As Object, e As MouseEventArgs)

        If profilePopup Is Nothing OrElse Not profilePopup.Visible Then Exit Sub

        Dim clickedControl As Control = CType(sender, Control)

        ' If clicking inside popup → do nothing
        If profilePopup.Bounds.Contains(Me.PointToClient(System.Windows.Forms.Cursor.Position)) Then
            Exit Sub
        End If

        ' If clicking profile picture → do nothing (it will open)
        If clickedControl Is pbUserProfile Then
            Exit Sub
        End If

        ' Otherwise hide popup
        profilePopup.Visible = False

    End Sub
    Private Sub pbUserProfile_Click(sender As Object, e As EventArgs) Handles pbUserProfile.Click
        ' profilePopup.Visible = Not profilePopup.Visible
        profilePopup.Visible = Not profilePopup.Visible
        profilePopup.BringToFront()

    End Sub

    ' === ReloadForm initializes all UI elements and configurations ===
    Private Sub Home_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim result As DialogResult = MessageBox.Show(
        "Are you sure you want to exit?",
        "Confirm Exit",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question)

        If result = DialogResult.No Then
            e.Cancel = True ' Prevent the form from closing
        End If

        CleanupAndReleaseResources()
    End Sub

    Private Sub Home_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Timer1.Stop()
        Timer1.Dispose()
        TimerSidebar.Stop()
        TimerSidebar.Dispose()
        If LoginForm IsNot Nothing Then
            LoginForm.Dispose()
        End If
        Application.Exit() ' Ensure the entire application exits when the main form is closed
    End Sub



    '==============================
    ' TAB SELECTION EVENT
    '==============================
    Private Async Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControlMain.SelectedIndexChanged

        If TabControlMain.SelectedTab Is TabHome Then

            LoadHomeTab()

        ElseIf TabControlMain.SelectedTab Is TabLogs Then

            LoadLogsTab()

        ElseIf TabControlMain.SelectedTab Is TabAppendix Then

            Await LoadAppendixTab()

        ElseIf TabControlMain.SelectedTab Is TabSettings Then

            Await LoadSettingsTab()

        ElseIf TabControlMain.SelectedTab Is TabInstruction Then

            Await LoadInstructionTab()

        ElseIf TabControlMain.SelectedTab Is TabHosekeeper Then

            LoadHousekeepingTab()

        ElseIf TabControlMain.SelectedTab Is TabAboutUs Then

            Await LoadAboutTab()

        End If

    End Sub

#Region "Home Tab"
    '==============================
    ' TAB HOME
    '==============================
    Private Sub LoadHomeTab()
        ReloadForm()
        If homeLoaded Then Return

        homeLoaded = True

        SetFocusBubble(txtUsername, "Please type your user ID.")
        SetFocusBubble(txtPassword, "Please type your password.")
        ' Reload and initialize the form


    End Sub

    Public Sub ReloadForm()
        ' Clear dropdowns and reset labels
        lblStartTime.Text = ""
        lblEndTime.Text = ""
        cmbSystem.Text = ""
        cmbControl.Text = ""
        txtFolderPath.Text = ""
        txtPassword.Text = ""
        txtUsername.Text = "ITGCBOT"
        lblNotification.Text = ""
        txtClient.Visible = False
        lblSystemId.Text = ""
        CheckBoxClient.Checked = False
        cmbExecutionMode.SelectedIndex = 0
        cmbExecutionType.SelectedIndex = 0
        lblDescription.Text = ""
        chartStatus.Series.Clear()
        chartFolderStatus.Series.Clear()
        LoadCsvAuditDataWithControl()
        SetMonthDates(DateTime.Now)

        ' Ensure report month and folder path are computed immediately after setting dates
        ValidateAndUpdateReportMonth()

        ' Initialize system monitor and timer
        monitor = New SystemMonitor()
        Timer1.Interval = 1000
        Timer1.Start()

        ' Load XML system list and control definitions
        Dim xmlPath As String = My.Settings.SAPGUIXML
        LoadSystemsIntoComboBox(xmlPath)

        Dim xmlControlPath As String = My.Settings.ITGCControl
        LoadControlsIntoComboBox(xmlControlPath)

        ' Apply UI effects & hover styles to controls
        ' ApplyGradientText(lblHeader, Color.Crimson, Color.DeepSkyBlue)
        ApplyHoverEffect(btnExecute, Color.DarkSeaGreen)
        ApplyHoverEffect(btnGenerateWordReport, Color.Orange)
        ApplyHoverEffect(txtPassword, Color.LightYellow, Cursors.IBeam)
        ApplyHoverEffect(txtUsername, Color.LightYellow, Cursors.IBeam)

        ' Adjust sidebar dimensions relative to form size
        expandedHeight = Me.ClientSize.Height
        expandedWidth = Math.Min(300, Me.ClientSize.Width \ 3)
        collapsedSize = New Size(0, 0)

        'Load saved values from settings
        lblStartTime.Text = My.Settings.Report_FTIME
        lblEndTime.Text = My.Settings.Report_TTIME
    End Sub

    '=== Set date pickers to the start And end of the selected month ===
    Private Sub SetMonthDates(baseDate As DateTime)
        ' First day of month
        dtpStart.Value = New DateTime(baseDate.Year, baseDate.Month, 1)

        ' Last day of month
        dtpEnd.Value = New DateTime(
        baseDate.Year,
        baseDate.Month,
        DateTime.DaysInMonth(baseDate.Year, baseDate.Month)
    )
    End Sub


    ' === Load system definitions from XML and populate combo box ===
    Private Sub LoadSystemsIntoComboBox(xmlFilePath As String)
        cmbSystem.Items.Clear()
        Try
            Using reader As New StreamReader(xmlFilePath)
                Dim xmlDoc As New XmlDocument()
                xmlDoc.Load(reader)

                Dim serviceNodes As XmlNodeList = xmlDoc.SelectNodes("//Service")

                For Each node As XmlNode In serviceNodes
                    If node.Attributes("name") IsNot Nothing AndAlso node.Attributes("systemid") IsNot Nothing Then
                        Dim systemName As String = node.Attributes("name").Value
                        Dim systemId As String = node.Attributes("systemid").Value

                        ' Add system name to ComboBox
                        cmbSystem.Items.Add(systemName)

                        ' Store the mapping
                        systemIdMap(systemName) = systemId
                    End If
                Next
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading systems: " & ex.Message, "XML Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CmbSystem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSystem.SelectedIndexChanged

        Dim selectedSystem As String = cmbSystem.SelectedItem.ToString()

        If systemIdMap.ContainsKey(selectedSystem) Then
            lblSystemId.Text = systemIdMap(selectedSystem)
        Else
            lblSystemId.Text = "System ID not found"
        End If
    End Sub
    '=========================Check box to show client==========================
    Private Sub CheckBoxClient_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxClient.CheckedChanged
        If CheckBoxClient.Checked Then
            txtClient.Enabled = True
            txtClient.Visible = True
        Else
            txtClient.Enabled = False
            txtClient.Visible = False
        End If
    End Sub

    ' === Load control definitions from XML and populate combo box ===
    Private Sub LoadControlsIntoComboBox(xmlControlPath As String)
        cmbControl.Items.Clear()
        controlDefinitions.Clear()
        Try
            If Not File.Exists(xmlControlPath) Then
                MessageBox.Show("XML file not found at: " & xmlControlPath)
                Return
            End If

            Dim doc As New XmlDocument()
            doc.Load(xmlControlPath)
            Dim rows = doc.SelectNodes("//ITGCControls/Row")

            For Each row As XmlNode In rows
                Dim ref = row.SelectSingleNode("ControlRef")?.InnerText
                Dim def = row.SelectSingleNode("ControlDefinition")?.InnerText

                If Not String.IsNullOrWhiteSpace(ref) AndAlso Not String.IsNullOrWhiteSpace(def) Then
                    cmbControl.Items.Add(ref & " - " & def)
                    controlDefinitions(ref) = def
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("Error reading XML: " & ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub cmbControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbControl.SelectedIndexChanged
        ' Only try to get the description if something is actually selected
        If cmbControl.SelectedItem IsNot Nothing Then
            Dim selectedControlRef As String = cmbControl.SelectedItem.ToString()
            selectedControlRef = selectedControlRef.Split("-"c)(0).Trim()

            If controlDefinitions.ContainsKey(selectedControlRef) Then
                lblDescription.Text = controlDefinitions(selectedControlRef)
                lblDescription.ForeColor = Color.Black
            Else
                lblDescription.Text = "Control not found."
                lblDescription.ForeColor = Color.Red
            End If
        Else
            lblDescription.Text = ""
        End If

        ' Always run this, even if the selection was cleared
        UpdateFolderPath()
    End Sub

    Private Sub UpdateFolderPath()
        Dim reportMonth As String = txtReportMonth.Text

        ' Use SelectedItem if present, otherwise fallback to the ComboBox text (handles programmatic changes)
        Dim rawControl As String = If(cmbControl.SelectedItem?.ToString(), cmbControl.Text)
        Dim control As String = Nothing

        If Not String.IsNullOrEmpty(rawControl) Then
            control = rawControl.Split("-"c)(0).Trim()
        End If

        ' Only update if both values are provided
        'If Not String.IsNullOrEmpty(reportMonth) AndAlso Not String.IsNullOrEmpty(control) Then
        '    Dim downloadsPath As String = Path.Combine(
        '    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        '    Path.Combine("Downloads", "ITGC REPORT"),
        '    reportMonth,
        '    control
        ')
        If Not String.IsNullOrEmpty(reportMonth) AndAlso Not String.IsNullOrEmpty(control) Then
            Dim downloadsPath As String = Path.Combine(My.Settings.DownloadDestination, "ITGC REPORT", reportMonth, control)



            txtFolderPath.Text = downloadsPath
        Else
            txtFolderPath.Text = String.Empty
        End If
    End Sub

    '---------------------------------------------
    ' Event handler when Execution Type changes
    '---------------------------------------------
    ' Class-level variable to hold the original list of items
    Private originalControlList As New List(Of String)
    Private Sub cmbExecutionType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbExecutionType.SelectedIndexChanged
        If cmbExecutionType.SelectedItem Is Nothing Then Exit Sub

        If cmbExecutionType.SelectedItem.ToString() = "All Controls" Then
            ' Disable manual selection
            cmbControl.Enabled = False
            ' Save the current items (only if not already saved)
            If originalControlList.Count = 0 AndAlso cmbControl.Items.Count > 0 Then
                For Each item As Object In cmbControl.Items
                    originalControlList.Add(item.ToString())
                Next
            End If

            ' Clear and add active controls
            cmbControl.Items.Clear()
            cmbControl.Items.AddRange(New String() {
                "ITGC01 - SAPOSS Enabling Audit",
                "ITGC02 - Client ‘Opening’ Audit",
                "ITGC04 - Critical Transaction Usages in Production (CDHDR Report)",
                "ITGC06 - Developer Key/Access in Production",
                "ITGC07 - List of Users Having SAP_ALL and SAP_NEW Access for User type A(Dialog) and S(Service)",
                "ITGC08 - List of Users having Table Maintenance Access in Production",
                "ITGC09 - Users are authorized to execute programs based on their job responsibilities.",
                "ITGC10 - Ability to alter security configuration settings",
                "ITGC11 - Ability to import transports to production environment",
                "ITGC12 - Ability to perform job administration functions (Maintenance) – SM37 with Admin Access",
                "ITGC13 - Ability to change password configuration settings – RZ10/RZ11 Change access",
                "ITGC14 - Password Status for the below default SAP ID's",
                "ITGC16 - SAP Standard Program changes",
                "ITGC17 - Authorizations to Modify IDOCs in Production",
                "ITGC18 - SAP parameters Review"
            })


            ' Select first item by default
            If cmbControl.Items.Count > 0 Then
                cmbControl.SelectedIndex = 0
            End If
        Else
            ' Enable manual selection again
            cmbControl.Enabled = True
            ' Restore the original items (previous data)
            If originalControlList.Count > 0 Then
                cmbControl.Items.Clear()
                cmbControl.Items.AddRange(originalControlList.ToArray())
            End If
        End If

    End Sub
    '----------------------Resize Label-----------------------
    Private Sub ResizeLabelHeight(ByVal lbl As Label)
        Dim textSize As SizeF
        Using g As Graphics = lbl.CreateGraphics()
            textSize = g.MeasureString(lbl.Text, lbl.Font, lbl.Width)
        End Using

        ' Set the height based on the measured size
        lbl.Height = CInt(Math.Ceiling(textSize.Height))
    End Sub

    ' ---------- UI HELPERS ----------
    Private Sub InitProgressBar(pb As ProgressBar)
        pb.Minimum = 0
        pb.Maximum = 100
        pb.Style = ProgressBarStyle.Continuous
    End Sub

    Private Sub UpdateProgressBar(pb As ColoredProgressBar, value As Integer, text As String)
        pb.Value = Math.Min(value, 100)
        pb.DisplayText = text
        pb.Invalidate()
    End Sub

    ' === Validate inputs and execute SAP audit script ===
    Private Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click

        Dim reportMonth As String = txtReportMonth.Text.Trim()
        Dim selectedControl As String = cmbControl.Text.Trim()
        Dim selectedSystem As String = cmbSystem.Text.Trim()
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        lblNotification.Text = "Validating inputs..."
        ' Validate inputs
        If reportMonth = "Invalid date" OrElse String.IsNullOrWhiteSpace(reportMonth) Then
            MessageBox.Show("Please select a valid reporting duration.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblNotification.Text = "Invalid reporting duration."
            Return
        ElseIf String.IsNullOrWhiteSpace(selectedControl) Then
            MessageBox.Show("Please select a control.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblNotification.Text = "Control not selected."
            Return
        ElseIf String.IsNullOrWhiteSpace(selectedSystem) Then
            MessageBox.Show("Please select a system.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblNotification.Text = "System not selected."
            Return

        ElseIf String.IsNullOrWhiteSpace(username) Then
            MessageBox.Show("Username cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblNotification.Text = "Username cannot be empty."
            Return
        ElseIf String.IsNullOrWhiteSpace(password) Then
            MessageBox.Show("Password cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblNotification.Text = "Password cannot be empty."
            Return
        ElseIf cmbExecutionType.SelectedItem Is Nothing Then
            MessageBox.Show("Please select an execution type.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblNotification.Text = "Execution type not selected."
            Return
        ElseIf cmbExecutionMode.SelectedItem Is Nothing Then
            MessageBox.Show("Please select an execution mode.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblNotification.Text = "Execution mode not selected."
            Return
        ElseIf CheckBoxClient.Checked Then  ' If client checkbox is checked, validate client input
            If String.IsNullOrEmpty(txtClient.Text) Then
                MessageBox.Show("Client number is required.", "Missing Client", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Logger.LogMessage("Client number is required.", False)
                Return
            End If

        End If


        btnExecute.Enabled = False ' Prevent double-clicking

        Try
            ' Run the audit script
            ITGC_AUDIT.ExecuteSAPScripts()
            lblNotification.Text = "Execution completed successfully."
        Catch ex As Exception
            Logger.LogMessage("Execution failed: " & ex.Message, False)
            MessageBox.Show("An error occurred during execution: " & ex.Message, "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnExecute.Enabled = True ' Re-enable the button when done
        End Try

    End Sub
    ' === Update report month when date pickers change ===
    Private Sub dtpEnd_ValueChanged(sender As Object, e As EventArgs) Handles dtpEnd.ValueChanged
        ValidateAndUpdateReportMonth()

    End Sub

    Private Sub dtpStart_ValueChanged(sender As Object, e As EventArgs) Handles dtpStart.ValueChanged
        ValidateAndUpdateReportMonth()
    End Sub
    ' === Set report month based on date range or flag invalid ===
    Private Sub ValidateAndUpdateReportMonth()
        Dim reportStartDate As DateTime = dtpStart.Value
        Dim reportEndDate As DateTime = dtpEnd.Value

        If reportEndDate >= reportStartDate Then
            Dim startMonth As String = reportStartDate.ToString("MMMM")
            Dim endMonth As String = reportEndDate.ToString("MMMM")
            Dim startYear As String = reportStartDate.ToString("yyyy")
            Dim endYear As String = reportEndDate.ToString("yyyy")

            If startYear = endYear AndAlso startMonth = endMonth Then
                ' Single month
                txtReportMonth.Text = $"{startMonth}, {startYear}"
                LoadChartData()
            ElseIf startYear = endYear Then
                ' Multiple months, same year
                txtReportMonth.Text = $"{startMonth}-{endMonth} {startYear}"
                LoadChartData()
            Else
                ' Spans multiple years
                txtReportMonth.Text = $"{startMonth} {startYear} - {endMonth} {endYear}"
                LoadChartData()
            End If

            txtReportMonth.BackColor = SystemColors.Window
            txtReportMonth.ForeColor = SystemColors.ControlText
            UpdateFolderPath()
        Else
            txtReportMonth.Text = "Invalid date"
            txtReportMonth.BackColor = Color.LightCoral
            txtReportMonth.ForeColor = Color.Black
        End If


    End Sub
    ' === Open ITGC folder path in File Explorer ===
    Private Sub imgFolderPath_Click(sender As Object, e As EventArgs) Handles imgFolderPath.Click
        Dim folderPath = txtFolderPath.Text
        OpenFolder(folderPath)
    End Sub

    Private Sub imgPath_MouseEnter(sender As Object, e As EventArgs) Handles imgFolderPath.MouseEnter
        imgFolderPath.Width = 50
        imgFolderPath.Height = 48
    End Sub

    Private Sub imgPath_MouseLeave(sender As Object, e As EventArgs) Handles imgFolderPath.MouseLeave
        imgFolderPath.Width = 40
        imgFolderPath.Height = 38
    End Sub

    ''============ Menu Button ============================================================================
    'Private Sub ButtonToggle_Click(sender As Object, e As EventArgs) Handles ButtonToggle.Click
    '    If isAnimating Then Return
    '    isAnimating = True
    '    ButtonToggle.Enabled = False
    '    If Not isExpanded Then
    '        targetSize = New Size(expandedWidth, expandedHeight)
    '        ButtonToggle.Text = "❌ Close"
    '    Else
    '        targetSize = New Size(1, 1)
    '        ButtonToggle.Text = "☰ Menu"
    '    End If
    '    TimerSidebar.Interval = 10
    '    TimerSidebar.Start()
    'End Sub


    'Private Sub TimerSidebar_Tick(sender As Object, e As EventArgs) Handles TimerSidebar.Tick
    '    Dim doneWidth As Boolean = False
    '    Dim doneHeight As Boolean = False

    '    If PanelSidebar.Width <> targetSize.Width Then
    '        Dim stepW = Math.Max(1, Math.Abs(targetSize.Width - PanelSidebar.Width) \ 2)
    '        PanelSidebar.Width += Math.Sign(targetSize.Width - PanelSidebar.Width) * stepW
    '    Else
    '        doneWidth = True
    '    End If

    '    If PanelSidebar.Height <> targetSize.Height Then
    '        Dim stepH = Math.Max(1, Math.Abs(targetSize.Height - PanelSidebar.Height) \ 2)
    '        PanelSidebar.Height += Math.Sign(targetSize.Height - PanelSidebar.Height) * stepH
    '    Else
    '        doneHeight = True
    '    End If

    '    If doneWidth AndAlso doneHeight Then
    '        TimerSidebar.Stop()
    '        isAnimating = False
    '        isExpanded = Not isExpanded
    '        ButtonToggle.Enabled = True

    '    End If
    'End Sub

    'Private Sub PanelSidebar_Paint(sender As Object, e As PaintEventArgs) Handles PanelSidebar.Paint
    '    If isAnimating Then Exit Sub

    '    Using pen As New Pen(Color.Gray, 2)
    '        e.Graphics.DrawRectangle(pen, 0, 0, PanelSidebar.Width - 1, PanelSidebar.Height - 1)
    '    End Using
    'End Sub


    '============ Generate Report Button ============================================================================
    Private Async Sub btnGenerateWordReport_Click(sender As Object, e As EventArgs) Handles btnGenerateWordReport.Click
        Try
            Dim downloadsPath As String = Path.Combine(My.Settings.DownloadDestination, "ITGC REPORT")

            Dim forMonth As String = txtReportMonth.Text
            Dim baseFolder As String = downloadsPath

            ' Set image paths (adjust to your actual files)
            Dim coverImagePath As String = Path.Combine(Application.StartupPath, "cover.png")


            Await Task.Run(Sub()
                               WordReportGenerator.GenerateMonthlyReport(baseFolder, forMonth, coverImagePath, Me)
                           End Sub)


        Catch ex As Exception
            MsgBox("Failed to generate monthly report: " & ex.Message)
            lblNotification.Text = "Failed to generate report. Check logs for details."
        End Try
    End Sub


    Private bubbleTip As New ToolTip With {
  .IsBalloon = True,
  .ToolTipIcon = ToolTipIcon.Info,
  .ToolTipTitle = "Info"
}
    Private Sub SetFocusBubble(txt As TextBox, message As String)
        AddHandler txt.Enter, Sub(sender, e)
                                  ' Position slightly above the textbox
                                  Dim x As Integer = txt.Right
                                  Dim y As Integer = txt.Top

                                  ' Temporary show to force correct placement
                                  bubbleTip.Show("", txt.Parent, x, y, 1)
                                  bubbleTip.Show(message, txt.Parent, x, y, 5000)
                              End Sub

        AddHandler txt.Leave, Sub(sender, e)
                                  bubbleTip.Hide(txt)
                              End Sub
    End Sub

    '================= CSV DATA LOADING AND PROCESSING ==================
    ' LoadCsvAuditData("C:\Users\Abhinav\Downloads\ITGC REPORT\December, 2025")

    Private Sub LoadCsvAuditData(folderPath As String)

        Dim totalControls As Integer = 0
        Dim greenCount As Integer = 0
        Dim redCount As Integer = 0
        Dim hasData As Boolean = False

        ' Reset chart
        chartStatus.Series.Clear()

        ' ❌ Folder not found
        If Not Directory.Exists(folderPath) Then
            ShowNoData("No data for selected month")
            Exit Sub
        End If

        Dim csvFiles = Directory.GetFiles(folderPath, "*.csv")

        ' ❌ No CSV files
        If csvFiles.Length = 0 Then
            ShowNoData("No data for selected month")
            Exit Sub
        End If

        ' ✅ Process CSV files
        For Each file In csvFiles
            Using sr As New StreamReader(file)

                ' Skip header
                If Not sr.EndOfStream Then sr.ReadLine()

                While Not sr.EndOfStream
                    Dim line = sr.ReadLine()

                    If String.IsNullOrWhiteSpace(line) Then Continue While

                    Dim cols = line.Split(","c)
                    If cols.Length < 4 Then Continue While

                    Dim status As String = cols(3).Replace("""", "").Trim().ToUpper()

                    totalControls += 1
                    hasData = True

                    If status = "GREEN" Then
                        greenCount += 1
                    ElseIf status = "RED" Then
                        redCount += 1
                    End If
                End While
            End Using
        Next

        ' ❌ CSV exists but no usable data
        If Not hasData OrElse totalControls = 0 Then
            ShowNoData("No data for selected month")
            Exit Sub
        End If

        ' ✅ Load chart normally
        LoadPieChart(greenCount, redCount)

    End Sub

    Private Sub ShowNoData(message As String)

        chartStatus.Series.Clear()

        Dim series = chartStatus.Series.Add(message)
        chartStatus.Titles.Clear()
        chartStatus.Titles.Add("Current ITGC Control Status")

    End Sub
    ' ---------------- PIE CHART ----------------
    Private Sub LoadPieChart(green As Integer, red As Integer)

        chartStatus.Series.Clear()
        chartStatus.Titles.Clear()

        chartStatus.Titles.Add("Current ITGC Control Status")


        Dim series = chartStatus.Series.Add("Status")
        series.ChartType = DataVisualization.Charting.SeriesChartType.Pie
        series.BackGradientStyle = DataVisualization.Charting.GradientStyle.TopBottom
        series.Points.AddXY("Green", green)
        series.Points.AddXY("Red", red)
        series.IsValueShownAsLabel = True
        series.Points(0).Color = System.Drawing.Color.LightGreen
        series.Points(1).Color = System.Drawing.Color.LightCoral
        series.Label = "#PERCENT{P0}"
        series.LegendText = "#VALX"
        series.LegendText += " (#VALY)"

    End Sub
    ' ---------------- RESET UI ----------------
    Public Class ControlAuditResult
        Public Property ControlName As String
        Public Property GreenCount As Integer
        Public Property RedCount As Integer
    End Class
    Private Sub LoadChartData()
        Dim reportMonth As String = txtReportMonth.Text
        Dim csvPath As String = Path.Combine(My.Settings.DownloadDestination,
                    Path.Combine("ITGC REPORT"),
                    reportMonth
                )
        LoadCsvAuditData(csvPath)


    End Sub
    Private Sub ButtonTableAnalysis_Click(sender As Object, e As EventArgs)

        Dim csvPath As String = Path.Combine(My.Settings.DownloadDestination,
        "ITGC REPORT")

        LoadCsvAuditDataWithControl()

    End Sub

    Private Sub LoadCsvAuditDataWithControl()

        chartFolderStatus.Series.Clear()

        Dim baseFolderPath As String = Path.Combine(My.Settings.DownloadDestination,
        "ITGC REPORT")



        If Not Directory.Exists(baseFolderPath) Then
            MessageBox.Show("ITGC REPORT folder not found.")
            Exit Sub
        End If

        ' Dictionary to store control-wise results
        Dim controlMap As New Dictionary(Of String, ControlAuditResult)

        ' 🔥 READ ALL CSV FILES RECURSIVELY
        Dim csvFiles = Directory.GetFiles(
        baseFolderPath,
        "*.csv",
        SearchOption.AllDirectories
    )

        If csvFiles.Length = 0 Then
            MessageBox.Show("No CSV files found.")
            Exit Sub
        End If

        For Each file In csvFiles

            ' ✅ CONTROL NAME = PARENT FOLDER NAME
            Dim controlName As String = New DirectoryInfo(Path.GetDirectoryName(file)).Name

            If Not controlMap.ContainsKey(controlName) Then
                controlMap(controlName) = New ControlAuditResult With {
                .ControlName = controlName
            }
            End If

            Using sr As New StreamReader(file)

                ' Skip header
                If Not sr.EndOfStream Then sr.ReadLine()

                While Not sr.EndOfStream
                    Dim line = sr.ReadLine()
                    If String.IsNullOrWhiteSpace(line) Then Continue While

                    Dim cols = line.Split(","c)
                    If cols.Length < 4 Then Continue While

                    Dim status As String =
                    cols(3).Replace("""", "").Trim().ToUpper()

                    If status = "GREEN" Then
                        controlMap(controlName).GreenCount += 1
                    ElseIf status = "RED" Then
                        controlMap(controlName).RedCount += 1
                    End If
                End While
            End Using
        Next

        ' Load UI
        PopulateControlList(controlMap.Values.ToList())
        LoadControlWiseChart(controlMap.Values.ToList())

    End Sub
    Private Sub PopulateControlList(results As List(Of ControlAuditResult))

        dgvControlStatus.Columns.Clear()
        dgvControlStatus.Rows.Clear()

        ' Add columns
        Dim colControl As New DataGridViewTextBoxColumn()
        colControl.Name = "ControlName"
        colControl.HeaderText = "Control Name"
        colControl.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill   ' 🔥 FULL WIDTH
        colControl.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        colControl.MinimumWidth = 150

        Dim colGreen As New DataGridViewTextBoxColumn()
        colGreen.Name = "Green"
        colGreen.HeaderText = "✅"
        colGreen.Width = 50

        Dim colRed As New DataGridViewTextBoxColumn()
        colRed.Name = "Red"
        colRed.HeaderText = "❎"
        colRed.Width = 50

        Dim colTotal As New DataGridViewTextBoxColumn()
        colTotal.Name = "Total"
        colTotal.HeaderText = "T"
        colTotal.Width = 50

        dgvControlStatus.Columns.AddRange(
        colControl,
        colGreen,
        colRed,
        colTotal
    )

        ' ---- ROW SETTINGS (🔥 IMPORTANT) ----
        dgvControlStatus.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvControlStatus.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        ' Add rows
        For Each r In results
            dgvControlStatus.Rows.Add(
            r.ControlName,
            r.GreenCount,
            r.RedCount,
            r.GreenCount + r.RedCount
        )
        Next

        ' Alignment

        dgvControlStatus.Columns("Green").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvControlStatus.Columns("Red").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvControlStatus.Columns("Total").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


        ' Coloring (optional)
        dgvControlStatus.Columns("Green").DefaultCellStyle.BackColor = Color.LightGreen
        dgvControlStatus.Columns("Red").DefaultCellStyle.BackColor = Color.LightCoral

        ' General grid settings
        dgvControlStatus.ReadOnly = True
        dgvControlStatus.AllowUserToAddRows = False
        dgvControlStatus.RowHeadersVisible = False

    End Sub
    Private Sub LoadControlWiseChart(results As List(Of ControlAuditResult))

        chartFolderStatus.Series.Clear()
        chartFolderStatus.Titles.Clear()

        chartFolderStatus.Titles.Add("Overall ITGC Status")

        Dim greenSeries = chartFolderStatus.Series.Add("Green")
        Dim redSeries = chartFolderStatus.Series.Add("Red")

        greenSeries.ChartType = DataVisualization.Charting.SeriesChartType.RangeColumn
        greenSeries.BackGradientStyle = DataVisualization.Charting.GradientStyle.TopBottom
        greenSeries.Label = "#VALY"

        redSeries.ChartType = DataVisualization.Charting.SeriesChartType.RangeColumn
        redSeries.BackGradientStyle = DataVisualization.Charting.GradientStyle.TopBottom
        redSeries.Label = "#VALY"
        redSeries.IsValueShownAsLabel = True

        greenSeries.Color = Color.LightGreen
        redSeries.Color = Color.LightCoral

        For Each r In results
            greenSeries.Points.AddXY(r.ControlName, r.GreenCount)
            redSeries.Points.AddXY(r.ControlName, r.RedCount)
        Next

        chartFolderStatus.ChartAreas(0).AxisX.Interval = 1
        chartFolderStatus.ChartAreas(0).AxisX.LabelStyle.Angle = -45

    End Sub
    Private Sub lblRefresh_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblRefresh.LinkClicked
        Dim csvPath As String = Path.Combine(My.Settings.DownloadDestination,
       "ITGC REPORT")

        LoadCsvAuditDataWithControl()
    End Sub


#End Region

#Region "Instruction Tab"
    '==============================
    ' TAB INSTRUCTION
    '==============================
    Private Async Function LoadInstructionTab() As Task

        If instructionLoaded Then Return

        instructionLoaded = True

        Await WebView2.EnsureCoreWebView2Async()

        WebView2.Source = New Uri("https://voidautomation.github.io/AuditX_Documentation_V2.1/")

        WebView2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = False
        WebView2.CoreWebView2.Settings.AreDevToolsEnabled = False

    End Function

#End Region

#Region "Appendix Tab"
    '==============================
    ' TAB Appendix
    '==============================
    Private Async Function LoadAppendixTab() As Task

        If appendixLoaded Then Return

        appendixLoaded = True
        Await Task.Delay(100) ' Allow UI to update
        dataTable.Columns.Add("Control Ref.")
        dataTable.Columns.Add("Control Definition")
        dataTable.Columns.Add("Description")
        dataTable.Columns.Add("Risk")
        DataGridView1.DataSource = dataTable
        ApplyHoverEffect(btnEdit, Color.Salmon)
        ApplyHoverEffect(btnSaveXML, Color.LightGreen)
        ApplyHoverEffect(btnRevert, Color.Orange)
        ApplyHoverEffect(btnExportCSV, Color.LightSkyBlue)

        SetupDataGridView()
        StyleNoteBox()

        btnSaveXML.Visible = False
        btnRevert.Visible = False

        Me.KeyPreview = True

        Me.Controls.Add(txtNoteBox)
        Me.Controls.Add(lblNoteTitle)
        Me.Controls.Add(btnEditNote)

        lblStatus.Visible = False
        lblStatus.AutoSize = True

        If IO.File.Exists(xmlFilePath) Then LoadFromXML()

    End Function

    Private Sub SetupDataGridView()
        With DataGridView1
            .Font = New Font("Calibri", 10)
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.Font = New Font("Calibri", 10, FontStyle.Bold)
            .DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .DefaultCellStyle.WrapMode = DataGridViewTriState.True
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
            .Dock = DockStyle.Fill
            If .Columns.Count > 0 Then .Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        End With
    End Sub

    Private Sub StyleNoteBox()
        txtNoteBox.Visible = False
        txtNoteBox.ReadOnly = True
        txtNoteBox.Font = New Font("Segoe UI", 10)
        txtNoteBox.BorderStyle = BorderStyle.FixedSingle
        txtNoteBox.ScrollBars = RichTextBoxScrollBars.Vertical
        txtNoteBox.ContextMenuStrip = New ContextMenuStrip()
        txtNoteBox.ContextMenuStrip.Items.Add("Undo").Name = "undo"
        AddHandler txtNoteBox.ContextMenuStrip.ItemClicked, AddressOf NoteContextMenu_Click

        lblNoteTitle.Visible = False
        lblNoteTitle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblNoteTitle.BackColor = Color.Lavender

        btnEditNote.Visible = False
        btnEditNote.Text = "✏"
        btnEditNote.Font = New Font("Segoe UI", 9)
        btnEditNote.Size = New Size(40, 35)
        btnEditNote.Cursor = Cursors.Hand
    End Sub

    Private Sub NoteContextMenu_Click(sender As Object, e As ToolStripItemClickedEventArgs)
        If e.ClickedItem.Name = "undo" AndAlso txtNoteBox.CanUndo Then
            txtNoteBox.Undo()
            unsavedChanges = True
            UpdateStatus("Undo applied", "info")
        End If
    End Sub

    Private Sub LoadFromXML()
        dataTable.Clear()
        controlActions.Clear()

        Dim doc As XDocument = XDocument.Load(xmlFilePath)
        For Each row In doc.Descendants("Row")
            dataTable.Rows.Add(
            row.Element("ControlRef")?.Value,
            row.Element("ControlDefinition")?.Value,
            row.Element("Description")?.Value,
            row.Element("Risk")?.Value
        )
            controlActions.Add(row.Element("ControlAction")?.Value)
        Next

        ' 🔧 Force DataGridView to recalculate row heights
        DataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)

        UpdateStatus("Data Loaded Successfully", "success")
    End Sub

    Private Sub SaveToXML()
        If MessageBox.Show("Save changes to XML?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return

        Try
            ' Build current XML in memory
            Dim currentDoc As New XDocument(
            New XElement("ITGCControls",
                From i In Enumerable.Range(0, dataTable.Rows.Count)
                Let row = dataTable.Rows(i)
                Where Not row.IsNull(0)
                Select New XElement("Row",
                    New XElement("ControlRef", row("Control Ref.").ToString()),
                    New XElement("ControlDefinition", row("Control Definition")),
                    New XElement("Description", row("Description")),
                    New XElement("Risk", row("Risk")),
                    New XElement("ControlAction", controlActions(i))
                )
            )
        )

            ' Check if file exists and compare content
            If File.Exists(xmlFilePath) Then
                Dim existingDoc As XDocument = XDocument.Load(xmlFilePath)

                ' Compare both XML contents
                If XNode.DeepEquals(existingDoc, currentDoc) Then

                    MessageBox.Show("No changes detected.", "Save skipped", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    UpdateStatus("No changes detected.", "info")
                    Return
                End If
            End If

            ' Create backup folder if it doesn't exist
            Dim backupFolder As String = Path.Combine(Application.StartupPath, "Backup")
            If Not Directory.Exists(backupFolder) Then
                Directory.CreateDirectory(backupFolder)
            End If

            ' Backup the existing file
            If File.Exists(xmlFilePath) Then
                Dim backupFileName = $"ITGCControls_backup_{DateTime.Now:dd.MM.yyyy, HH_mm_ss}.xml"
                Dim backupFullPath = Path.Combine(backupFolder, backupFileName)
                File.Copy(xmlFilePath, backupFullPath, True)
            End If

            ' Save new XML
            currentDoc.Save(xmlFilePath)

            MessageBox.Show("Data saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            unsavedChanges = False
            UpdateStatus("Saved ✓", "success")
            'Home.ReloadForm()
        Catch ex As Exception
            MessageBox.Show("Error saving data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            UpdateStatus("Save failed!", "error")
        End Try
    End Sub

    Private Sub SetEditMode(enabled As Boolean)
        isEditing = enabled
        DataGridView1.ReadOnly = Not enabled
        DataGridView1.AllowUserToAddRows = enabled
        DataGridView1.AllowUserToDeleteRows = enabled
        btnEdit.Text = If(enabled, "Cancel Edit", "Edit")
        btnSaveXML.Visible = enabled
        btnRevert.Visible = enabled
        unsavedChanges = False
        UpdateStatus(
    If(enabled,
       "Edit mode enabled. Double-click a field to modify.",
       "Edit mode disabled."),
    If(enabled, "error", "info")
)


    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If isEditing AndAlso unsavedChanges Then
            If MessageBox.Show("Discard unsaved changes?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
        End If
        SetEditMode(Not isEditing)
    End Sub

    Private Sub btnSaveXML_Click(sender As Object, e As EventArgs) Handles btnSaveXML.Click
        If isEditing Then SaveToXML() Else MessageBox.Show("Please enter edit mode first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub btnRevert_Click(sender As Object, e As EventArgs) Handles btnRevert.Click
        If MessageBox.Show("Discard all changes?", "Confirm Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            LoadFromXML()
            SetEditMode(False)
        End If
    End Sub

    Private Sub btnExportCSV_Click(sender As Object, e As EventArgs) Handles btnExportCSV.Click
        If dataTable.Rows.Count = 0 Then MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Return
        Using sfd As New SaveFileDialog() With {.Filter = "CSV files (*.csv)|*.csv", .FileName = "ITGC_Controls.csv"}
            If sfd.ShowDialog() = DialogResult.OK Then
                Using sw As New StreamWriter(sfd.FileName, False, System.Text.Encoding.UTF8)
                    sw.WriteLine(String.Join(",", dataTable.Columns.Cast(Of DataColumn)().
                    Select(Function(c) $"""{c.ColumnName}""")))
                    For Each row As DataRow In dataTable.Rows
                        sw.WriteLine(String.Join(",", row.ItemArray.Select(Function(f) $"""{f?.ToString().Replace("""", """""")}""")))
                    Next
                End Using
                MessageBox.Show("Export successful!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    Private Sub DataGridView1_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.ColumnIndex = 0 AndAlso e.RowIndex >= 0 Then
            e.PaintBackground(e.CellBounds, True)

            Dim cellValue As String = If(DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value?.ToString(), "").Trim()
            Dim isBlank As Boolean = String.IsNullOrWhiteSpace(cellValue)

            ' Choose icon based on value
            Dim iconText As String = If(isBlank, "➕", "📝")
            Dim iconFont As New Font("Segoe UI Emoji", 10)
            Dim iconSize As Size = TextRenderer.MeasureText(iconText, iconFont)

            Dim iconX = e.CellBounds.Left + 4
            Dim iconY = e.CellBounds.Top + (e.CellBounds.Height - iconSize.Height) \ 2
            TextRenderer.DrawText(e.Graphics, iconText, iconFont, New Point(iconX, iconY), Color.Black)

            ' Draw Control Ref. text after icon
            Dim textFont As Font = e.CellStyle.Font
            Dim textX = iconX + iconSize.Width + 4
            Dim textY = e.CellBounds.Top + (e.CellBounds.Height - textFont.Height) \ 2
            TextRenderer.DrawText(e.Graphics, cellValue, textFont, New Point(textX, textY), e.CellStyle.ForeColor)

            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Return

        If e.ColumnIndex = 0 Then
            Dim controlRef = DataGridView1.Rows(e.RowIndex).Cells(0).Value?.ToString().Trim()

            ' 🔒 Don't show if blank
            If String.IsNullOrWhiteSpace(controlRef) Then
                txtNoteBox.Visible = False
                lblNoteTitle.Visible = False
                btnEditNote.Visible = False
                Return
            End If

            If txtNoteBox.Visible AndAlso txtNoteBox.Tag = e.RowIndex Then
                ' Hide existing
                txtNoteBox.Visible = False
                lblNoteTitle.Visible = False
                btnEditNote.Visible = False
                txtNoteBox.ReadOnly = True
                txtNoteBox.Tag = Nothing
                btnEditNote.Text = "✏"
            Else
                ' Load note content
                txtNoteBox.Text = controlActions(e.RowIndex)
                txtNoteBox.Tag = e.RowIndex
                lblNoteTitle.Text = "Note - " & controlRef
                txtNoteBox.BackColor = Color.GhostWhite

                ' ✏️ Size calculation (responsive to form size)
                Dim margin As Integer = 30
                Dim maxWidth As Integer = Me.ClientSize.Width - margin * 2
                Dim maxHeight As Integer = Me.ClientSize.Height - margin * 2
                Dim noteWidth As Integer = Math.Min(600, maxWidth)
                Dim noteHeight As Integer = Math.Min(250, maxHeight)

                ' 📐 Center within the form
                Dim centerX As Integer = (Me.ClientSize.Width - noteWidth) \ 2
                Dim centerY As Integer = (Me.ClientSize.Height - noteHeight) \ 2

                ' 🧭 Apply sizes and positions
                txtNoteBox.SetBounds(centerX, centerY, noteWidth, noteHeight)
                lblNoteTitle.SetBounds(centerX, centerY - 35, noteWidth - 45, 30)
                btnEditNote.SetBounds(centerX + noteWidth - 40, centerY - 35, 40, 30)

                ' 👁 Visibility & State
                txtNoteBox.ReadOnly = True
                btnEditNote.Text = "✏"
                txtNoteBox.Visible = True
                lblNoteTitle.Visible = True
                btnEditNote.Visible = True

                txtNoteBox.BringToFront()
                lblNoteTitle.BringToFront()
                btnEditNote.BringToFront()
            End If

        ElseIf txtNoteBox.Visible Then
            ' Save changes if note was visible and clicked elsewhere
            If txtNoteBox.Tag IsNot Nothing Then
                Dim index = CInt(txtNoteBox.Tag)
                controlActions(index) = txtNoteBox.Text
            End If

            txtNoteBox.Visible = False
            lblNoteTitle.Visible = False
            btnEditNote.Visible = False
            txtNoteBox.ReadOnly = True
            txtNoteBox.Tag = Nothing
            btnEditNote.Text = "✏"
        End If
    End Sub

    Private Sub btnEditNote_Click(sender As Object, e As EventArgs) Handles btnEditNote.Click
        If txtNoteBox.ReadOnly Then
            txtNoteBox.ReadOnly = False
            btnEditNote.Text = "💾"
            txtNoteBox.BackColor = Color.White
        Else
            If MessageBox.Show("Save changes to this note?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim idx = CInt(txtNoteBox.Tag)
                controlActions(idx) = txtNoteBox.Text
                txtNoteBox.ReadOnly = True
                btnEditNote.Text = "✏"
                txtNoteBox.BackColor = Color.GhostWhite
                unsavedChanges = True
                UpdateStatus("Note updated (unsaved)", "warning")
            End If
        End If
        If Not txtNoteBox.ReadOnly Then txtNoteBox.Focus()
    End Sub

    Private Sub Appendix_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.S AndAlso isEditing Then
            btnSaveXML.PerformClick()
            e.SuppressKeyPress = True
        End If
        If e.Control AndAlso e.KeyCode = Keys.Z AndAlso Not txtNoteBox.ReadOnly Then
            txtNoteBox.Undo()
            unsavedChanges = True
            UpdateStatus("Note undone", "info")
        End If
    End Sub

    ' 🔥 Final Status Notification System 🔥

    Private Sub UpdateStatus(message As String, Optional type As String = "info")
        notifyTimer.Stop()
        RemoveHandler notifyTimer.Tick, AddressOf StartFadeOut
        RemoveHandler notifyTimer.Tick, AddressOf FadeStep

        notifyIsFading = False
        notifyOpacity = 1.0
        lblStatus.Text = message
        lblStatus.Visible = True
        lblStatus.ForeColor = Color.Black
        lblStatus.BorderStyle = BorderStyle.None
        lblStatus.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        lblStatus.AutoSize = True
        lblStatus.BringToFront()
        lblStatus.BackColor = GetStatusColor(type)
        lblStatus.Padding = New Padding(0)
        lblStatus.BorderStyle = BorderStyle.FixedSingle
        lblStatus.BringToFront()
        lblStatus.Anchor = AnchorStyles.Left Or AnchorStyles.Bottom

        notifyTimer.Interval = 2000 ' wait 2 seconds before fade
        AddHandler notifyTimer.Tick, AddressOf StartFadeOut
        notifyTimer.Start()
    End Sub

    Private Function GetStatusColor(type As String) As Color
        Select Case type.ToLower()
            Case "success"
                Return Color.FromArgb(198, 239, 206)  ' Light green
            Case "error"
                Return Color.FromArgb(255, 205, 210)  ' Light red
            Case "warning"
                Return Color.FromArgb(255, 249, 196)  ' Light yellow
            Case Else
                Return Color.FromArgb(201, 228, 255)  ' Light blue (info/default)
        End Select
    End Function

    Private Sub StartFadeOut(sender As Object, e As EventArgs)
        notifyTimer.Stop()
        RemoveHandler notifyTimer.Tick, AddressOf StartFadeOut

        notifyIsFading = True
        notifyTimer.Interval = 100
        AddHandler notifyTimer.Tick, AddressOf FadeStep
        notifyTimer.Start()
    End Sub

    Private Sub FadeStep(sender As Object, e As EventArgs)
        If notifyOpacity <= 0 Then
            notifyTimer.Stop()
            RemoveHandler notifyTimer.Tick, AddressOf FadeStep
            lblStatus.Visible = False
            notifyIsFading = False
        Else
            notifyOpacity -= 0.05
            lblStatus.BackColor = Color.FromArgb(CInt(255 * notifyOpacity), lblStatus.BackColor)
            lblStatus.ForeColor = Color.FromArgb(CInt(255 * notifyOpacity), lblStatus.ForeColor)
        End If
    End Sub

    Private Sub btnEditNote_MouseEnter(sender As Object, e As EventArgs) Handles btnEditNote.MouseEnter
        btnEditNote.BackColor = Color.LightBlue  ' or any highlight color

    End Sub

    Private Sub btnEditNote_MouseLeave(sender As Object, e As EventArgs) Handles btnEditNote.MouseLeave
        btnEditNote.BackColor = SystemColors.Control  ' default button color
    End Sub



#End Region

#Region "Settings Tab"
    '==============================
    ' TAB SETTINGS
    '==============================

    Private Async Function LoadSettingsTab() As Task

        If settingsLoaded Then Return

        settingsLoaded = True
        LoadControl(New ctrlGeneral())

        Await Task.CompletedTask


    End Function
#End Region

#Region "Log Tab"
    '==============================
    ' TAB LOGS
    '==============================

    ' Small helper class for ComboBox binding
    Private Class LogFileItem
        Public Property Display As String
        Public Property Value As String
        Public Overrides Function ToString() As String
            Return Display
        End Function
    End Class

    Private Sub LoadLogsTab()

        If logsLoaded Then Return

        logsLoaded = True
        ' ====== Setup ComboBox ======
        cmbLogFiles.DropDownStyle = ComboBoxStyle.DropDownList

        ' ====== Setup Filter TextBox ======
        txtFilter.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        txtFilter.Visible = False
        lblFilter.Visible = False
        'ApplyGradientText(lblHeading, Color.Crimson, Color.DeepSkyBlue)
        ' ====== Setup DataGridView ======
        dgvLogs.Columns.Clear()
        dgvLogs.ReadOnly = True
        dgvLogs.AllowUserToAddRows = False
        dgvLogs.AllowUserToDeleteRows = False
        dgvLogs.AllowUserToResizeColumns = False
        dgvLogs.AllowUserToResizeRows = False
        dgvLogs.AllowUserToOrderColumns = False
        dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvLogs.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvLogs.RowTemplate.Height = 50
        dgvLogs.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        dgvLogs.AllowUserToAddRows = False
        dgvLogs.ReadOnly = True
        dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvLogs.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue

        dgvLogs.Columns.Add("Timestamp", "📅 Timestamp")
        dgvLogs.Columns.Add("User", "👤 User")
        dgvLogs.Columns.Add("Status", "✅ Status")
        dgvLogs.Columns.Add("Message", "📝 Message")

        dgvLogs.Columns("Timestamp").Width = 150
        dgvLogs.Columns("User").Width = 120
        dgvLogs.Columns("Status").Width = 100
        dgvLogs.Columns("Message").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

        ' ====== Load available log files ======
        If Directory.Exists(logFolderPath) Then
            Dim logFiles = Directory.GetFiles(logFolderPath, "*.txt").
                OrderByDescending(Function(f) File.GetLastWriteTime(f)) ' newest first

            cmbLogFiles.Items.Clear()
            For Each filePath In logFiles
                cmbLogFiles.Items.Add(New LogFileItem With {
                    .Display = Path.GetFileName(filePath),
                    .Value = filePath
                })
            Next
        End If
    End Sub


    Private Sub cmbLogFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLogFiles.SelectedIndexChanged
        If cmbLogFiles.SelectedItem Is Nothing Then
            txtFilter.Visible = False
            lblFilter.Visible = False
            Exit Sub
        End If

        Dim selected As LogFileItem = CType(cmbLogFiles.SelectedItem, LogFileItem)
        Dim filePath As String = selected.Value
        If Not File.Exists(filePath) Then Exit Sub

        dgvLogs.Rows.Clear()
        txtFilter.Visible = True
        lblFilter.Visible = True
        Dim lines = File.ReadAllLines(filePath)
        Dim currentTimestamp As String = ""
        Dim currentUser As String = ""
        Dim currentStatus As String = ""
        Dim currentMessage As New StringBuilder()

        For Each line In lines
            ' Detect new log entry (first 19 chars look like datetime)
            If line.Length > 19 AndAlso DateTime.TryParse(line.Substring(0, 19), Nothing) Then
                ' Save previous entry
                If currentTimestamp <> "" Then
                    AddLogRow(currentTimestamp, currentUser, currentStatus, currentMessage.ToString().Trim())
                End If

                currentMessage.Clear()
                currentTimestamp = line.Substring(0, 19)

                ' Extract brackets info
                Dim firstBracket As Integer = line.IndexOf("["c)
                Dim secondBracket As Integer = line.IndexOf("]", firstBracket + 1)
                Dim thirdBracket As Integer = line.IndexOf("[", secondBracket + 1)
                Dim fourthBracket As Integer = line.IndexOf("]", thirdBracket + 1)

                If firstBracket > -1 AndAlso secondBracket > -1 AndAlso thirdBracket > -1 AndAlso fourthBracket > -1 Then
                    currentUser = line.Substring(firstBracket + 1, secondBracket - firstBracket - 1).Trim()
                    currentStatus = line.Substring(thirdBracket + 1, fourthBracket - thirdBracket - 1).Trim()
                    currentMessage.Append(line.Substring(fourthBracket + 1).Trim())
                Else
                    currentUser = ""
                    currentStatus = ""
                    currentMessage.Append(line.Trim())
                End If
            Else
                ' continuation (stack trace, multiline msg)
                currentMessage.AppendLine(line.Trim())
            End If
        Next

        ' Save last entry
        If currentTimestamp <> "" Then
            AddLogRow(currentTimestamp, currentUser, currentStatus, currentMessage.ToString().Trim())
        End If
    End Sub

    ''' <summary>
    ''' Adds a row with status-based coloring
    ''' </summary>
    Private Sub AddLogRow(timestamp As String, user As String, status As String, message As String)
        Dim rowIndex As Integer = dgvLogs.Rows.Add(timestamp, user, status, message)
        Dim row As DataGridViewRow = dgvLogs.Rows(rowIndex)

        Select Case status.ToUpper()
            Case "SUCCESS"
                row.DefaultCellStyle.BackColor = Color.Honeydew
                row.DefaultCellStyle.ForeColor = Color.DarkGreen
            Case "ERROR"
                row.DefaultCellStyle.BackColor = Color.MistyRose
                row.DefaultCellStyle.ForeColor = Color.DarkRed
            Case "EXCEPTION"
                row.DefaultCellStyle.BackColor = Color.LemonChiffon
                row.DefaultCellStyle.ForeColor = Color.DarkOrange
            Case Else
                row.DefaultCellStyle.BackColor = Color.White
                row.DefaultCellStyle.ForeColor = Color.Black
        End Select
    End Sub

    ''' <summary>
    ''' Filter logs dynamically
    ''' </summary>
    Private Sub txtFilter_TextChanged(sender As Object, e As EventArgs) Handles txtFilter.TextChanged
        Dim filterText As String = txtFilter.Text.Trim().ToLower()

        For Each row As DataGridViewRow In dgvLogs.Rows
            If row.IsNewRow Then Continue For
            Dim visible As Boolean = False
            For Each cell As DataGridViewCell In row.Cells
                If cell.Value IsNot Nothing AndAlso cell.Value.ToString().ToLower().Contains(filterText) Then
                    visible = True
                    Exit For
                End If
            Next
            row.Visible = visible
        Next
    End Sub

    Private Sub dgvLogs_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLogs.CellDoubleClick
        If e.RowIndex < 0 Then Return

        Dim row As DataGridViewRow = dgvLogs.Rows(e.RowIndex)
        Dim details As New StringBuilder()

        details.AppendLine($"📅 Timestamp : {row.Cells("Timestamp").Value}")
        details.AppendLine($"👤 User      : {row.Cells("User").Value}")
        details.AppendLine($"✅ Status    : {row.Cells("Status").Value}")
        details.AppendLine()
        details.AppendLine("📝 Message:")
        details.AppendLine(New String("─"c, 40))
        details.AppendLine(row.Cells("Message").Value?.ToString())

        MessageBox.Show(details.ToString(), "Log Entry Detail",
                    MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#End Region

#Region "About Tab"

    '==============================
    ' Constants
    '==============================
    Private Const DiagnosticPropertyColumnWidth As Integer = 350
    Private Const DiagnosticPadWidth As Integer = 25
    Private Const DiagnosticSeparatorLength As Integer = 40
    Private Const BytesPerMegabyte As Double = 1024 * 1024

    '==============================
    ' TAB Load About
    '==============================
    Private Async Function LoadAboutTab() As Task

        If aboutLoaded Then Return
        aboutLoaded = True

        Try
            ' 1. Resolve application title (fallback to assembly name if title is blank)
            Dim appTitle As String = If(
                String.IsNullOrWhiteSpace(My.Application.Info.Title),
                Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName),
                My.Application.Info.Title)

            ' 2. Bind standard application information to UI labels
            LabelProductName.Text = My.Application.Info.ProductName
            LabelVersion.Text = $"Version: {My.Application.Info.Version}"
            LabelCopyright.Text = $"{appTitle} {My.Application.Info.Copyright}"
            LabelCompanyName.Text = $"Developer: {My.Application.Info.CompanyName}"
            lblAboutSoftware.Text =
                "AuditX is a custom VB.NET desktop application built to automate and standardize " &
                "SAP ITGC (Information Technology General Controls) auditing." &
                Environment.NewLine & Environment.NewLine &
                "It programmatically interfaces with SAP to independently execute compliance checks" &
                "—such as verifying privileged access, developer keys, and system security parameters. " &
                "By automating manual navigation, capturing screenshot evidence directly into Word documents, " &
                "and extracting audit data into Excel, AuditX transforms a time-consuming manual process " &
                "into a fast, reliable, and fully documented workflow."

            ' 3. Prepare the ListView structure on the UI thread
            InitializeListView()

            ' 4. Offload heavy diagnostic data collection to a background thread
            Dim diagnosticData As Dictionary(Of String, String) =
                Await Task.Run(Function() GetDiagnosticData())

            ' 5. Populate the ListView with gathered diagnostic data (UI thread)
            PopulateDiagnosticsListView(diagnosticData)

        Catch ex As Exception
            MessageBox.Show(
                $"An error occurred while loading application details:{Environment.NewLine}{ex.Message}",
                "Load Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
        End Try
    End Function

    '==============================
    ' UI Helpers
    '==============================

    ''' <summary>
    ''' Configures the ListViewDiagnostics control columns and appearance.
    ''' Only needs to run once; guarded by aboutLoaded flag upstream.
    ''' </summary>
    Private Sub InitializeListView()
        With ListViewDiagnostics
            .View = View.Details
            .GridLines = False
            .FullRowSelect = True
            .HeaderStyle = ColumnHeaderStyle.Nonclickable
            .Columns.Clear()

            ' Fixed width for the property name column
            .Columns.Add("Property", DiagnosticPropertyColumnWidth)

            ' Width -2 auto-fills remaining space, removing the phantom third column
            .Columns.Add("Value", -2)
        End With
    End Sub

    ''' <summary>
    ''' Clears and repopulates the diagnostics ListView from the provided dictionary.
    ''' </summary>
    Private Sub PopulateDiagnosticsListView(data As Dictionary(Of String, String))
        ListViewDiagnostics.BeginUpdate() ' Suspend UI redraws for performance
        Try
            ListViewDiagnostics.Items.Clear()
            For Each entry In data
                Dim lvItem As New ListViewItem(entry.Key)
                lvItem.SubItems.Add(entry.Value)
                ListViewDiagnostics.Items.Add(lvItem)
            Next
        Finally
            ListViewDiagnostics.EndUpdate() ' Resume UI redraws
        End Try
    End Sub

    '==============================
    ' Data Collection
    '==============================

    ''' <summary>
    ''' Collects application, runtime, hardware, and health diagnostic data.
    ''' Designed to run safely on a background thread.
    ''' </summary>
    Private Function GetDiagnosticData() As Dictionary(Of String, String)
        Dim data As New Dictionary(Of String, String)()

        Try
            ' --- Application Details ---
            If Not String.IsNullOrWhiteSpace(My.Application.Info.Description) Then
                data.Add("Description", My.Application.Info.Description)
            End If

            Dim asm As Assembly = Assembly.GetExecutingAssembly()
            Dim buildDate As DateTime = File.GetLastWriteTime(asm.Location)
            data.Add("Build Date", buildDate.ToString("yyyy-MM-dd HH:mm:ss"))

            ' --- Runtime & Hardware ---
            data.Add("Framework", RuntimeInformation.FrameworkDescription)
            data.Add("CLR Version", Environment.Version.ToString())
            data.Add("SAP .NET Connector Version", "3.1.7.0")
            data.Add("Machine Name", Environment.MachineName)
            data.Add("OS Version", Environment.OSVersion.ToString())
            data.Add("Architecture", If(Environment.Is64BitOperatingSystem, "64-bit OS", "32-bit OS"))
            data.Add("Processor Count", Environment.ProcessorCount.ToString()) ' Added: useful for support

            ' --- Performance Metrics ---
            ' Disposed properly via Using block
            Using currentProcess As Process = Process.GetCurrentProcess()
                Dim uptime As TimeSpan = DateTime.Now - currentProcess.StartTime
                Dim memoryUsedMB As Double = currentProcess.WorkingSet64 / BytesPerMegabyte

                data.Add("Application Uptime",
                          $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s")
                data.Add("Memory Usage", $"{Math.Round(memoryUsedMB, 2)} MB")
            End Using

            ' --- Display Information ---
            ' Screen info is static and safe to read from a background thread
            Dim primaryScreen As Screen = Screen.PrimaryScreen
            If primaryScreen IsNot Nothing Then
                data.Add("Primary Resolution",
                          $"{primaryScreen.Bounds.Width} x {primaryScreen.Bounds.Height}")
            End If

            ' --- Health Checks ---
            data.Add("App Directory Access",
                      If(Directory.Exists(Environment.CurrentDirectory), "OK", "Access Denied"))

            ' Placeholder: add network or database checks here
            ' data.Add("Database Status", CheckDatabaseConnection())

        Catch ex As Exception
            ' Partial data is still useful; log what failed instead of discarding everything
            data.Add("Diagnostic Warning", "Some data could not be loaded due to permissions.")
            data.Add("Last Error", ex.Message)
        End Try

        Return data
    End Function

    '==============================
    ' Shared Report Builder
    '==============================

    ''' <summary>
    ''' Builds a formatted diagnostic report string from the current ListView contents.
    ''' Shared by both Copy and Export actions to eliminate duplication.
    ''' </summary>
    Private Function BuildDiagnosticReport() As String
        Dim sb As New StringBuilder()
        Dim separator As String = New String("-"c, DiagnosticSeparatorLength)

        sb.AppendLine($"--- {My.Application.Info.ProductName} Diagnostics ---")
        sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
        sb.AppendLine(separator)

        For Each item As ListViewItem In ListViewDiagnostics.Items
            sb.AppendLine($"{item.Text.PadRight(DiagnosticPadWidth)} : {item.SubItems(1).Text}")
        Next

        Return sb.ToString()
    End Function

    '==============================
    ' Export & Copy Events
    '==============================

    Private Sub ButtonCopyDiagnostics_Click(sender As Object, e As EventArgs) _
        Handles ButtonCopyDiagnostics.Click

        If ListViewDiagnostics.Items.Count = 0 Then Return

        Clipboard.SetText(BuildDiagnosticReport())

        MessageBox.Show(
            "Diagnostic information copied to clipboard!",
            "Copied",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)
    End Sub

    Private Sub ButtonExportDiagnostics_Click(sender As Object, e As EventArgs) _
        Handles ButtonExportDiagnostics.Click

        If ListViewDiagnostics.Items.Count = 0 Then Return

        Using sfd As New SaveFileDialog()
            sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            sfd.Title = "Save Diagnostic Data"
            sfd.FileName =
                $"{My.Application.Info.ProductName.Replace(" ", "")}" &
                $"_Diagnostics_{DateTime.Now:yyyyMMdd}.txt"

            If sfd.ShowDialog() <> DialogResult.OK Then Return

            Try
                File.WriteAllText(sfd.FileName, BuildDiagnosticReport())
                MessageBox.Show(
                    "Diagnostics successfully saved to file.",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show(
                    $"Failed to save file:{Environment.NewLine}{ex.Message}",
                    "Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

#End Region

#Region "Housekeeper"

    Shared Sub New()
        Try
            Dim appFolder As String = AppDomain.CurrentDomain.BaseDirectory
            Dim requiredDlls As String() = {"sapnco.dll", "sapnco_utils.dll"}
            Dim missingDlls As New List(Of String)()

            For Each dll As String In requiredDlls
                If Not File.Exists(Path.Combine(appFolder, dll)) Then
                    missingDlls.Add(dll)
                End If
            Next

            If missingDlls.Count > 0 Then
                Dim sb As New System.Text.StringBuilder()
                sb.AppendLine("The following required SAP NCo DLLs are missing:")
                For Each d As String In missingDlls
                    sb.AppendLine("  " & d)
                Next
                sb.AppendLine()
                sb.AppendLine("Please copy them into:")
                sb.AppendLine(appFolder)
                MessageBox.Show(sb.ToString(), "Missing SAP DLLs",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim currentPath As String = Environment.GetEnvironmentVariable("PATH")
            If Not String.IsNullOrEmpty(currentPath) AndAlso
               Not currentPath.Contains(appFolder) Then
                Environment.SetEnvironmentVariable(
                    "PATH", appFolder & ";" & currentPath)
            End If

            AddHandler AppDomain.CurrentDomain.AssemblyResolve,
                Function(sender As Object,
                         args As ResolveEventArgs) As System.Reflection.Assembly
                    Dim asmName As String =
                        New System.Reflection.AssemblyName(
                            args.Name).Name.ToLowerInvariant()
                    Dim asmPath As String =
                        Path.Combine(appFolder, asmName & ".dll")
                    If File.Exists(asmPath) Then
                        Return System.Reflection.Assembly.LoadFrom(asmPath)
                    End If
                    Return Nothing
                End Function

        Catch ex As Exception
            MessageBox.Show(
                "Failed to initialize SAP NCo: " & ex.Message,
                "SAP Initialization Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private _fullResult As DataTable
    Private _pageSize As Integer
    Private _currentPage As Integer = 1
    Private _totalPages As Integer = 1
    Private _sapCfg As SAPConnection = Nothing
    Private _rbtnConnectionIndicator As RadioButton = Nothing
    Private _connToolTip As ToolTip = Nothing

    Private Sub LoadHousekeepingTab()
        CleanupAndReleaseResources()

        If Not CheckSapNcoDependencies() Then
            cmbSystems.Enabled = False
            btnAnalyze.Enabled = False
            btnRFCSettings.Enabled = True
            Return
        End If

        Try
            _sapCfg = New SAPConnection()
        Catch ex As Exception
            MessageBox.Show(
                "Failed to load SAP system configurations:" &
                Environment.NewLine & ex.Message,
                "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cmbSystems.Enabled = False
            btnAnalyze.Enabled = False
            btnRFCSettings.Enabled = True
            Return
        End Try

        Try
            If Not RfcDestinationManager.IsDestinationConfigurationRegistered() Then
                RfcDestinationManager.RegisterDestinationConfiguration(_sapCfg)
                SAPConnection.RegisteredConfig = _sapCfg
            Else
                If Not Object.ReferenceEquals(
                    SAPConnection.RegisteredConfig, _sapCfg) Then
                    Try
                        RfcDestinationManager.RegisterDestinationConfiguration(_sapCfg)
                        SAPConnection.RegisteredConfig = _sapCfg
                    Catch regEx As Exception
                        System.Diagnostics.Debug.WriteLine(
                            "SAPConnection re-register skipped: " & regEx.Message)
                    End Try
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(
                "Error initializing SAP connection:" &
                Environment.NewLine & ex.Message,
                "SAP Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            cmbSystems.Enabled = False
            btnAnalyze.Enabled = False
        End Try

        Try
            cmbSystems.Items.Clear()
            If cmbSystems.Enabled Then
                Dim systems As List(Of String) = SAPConnection.GetAvailableSystems()
                If systems.Count > 0 Then
                    cmbSystems.Items.AddRange(systems.ToArray())
                    If systems.Count >= 2 Then
                        cmbSystems.Items.Insert(0, "── Multi System Analysis ──")
                        cmbSystems.SelectedIndex = 1
                    Else
                        cmbSystems.SelectedIndex = 0
                    End If
                Else
                    MessageBox.Show(
                        "No SAP systems are configured." &
                        Environment.NewLine &
                        "Please add a system via RFC Settings.",
                        "No Systems Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                    cmbSystems.Enabled = False
                    btnAnalyze.Enabled = False
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(
                "Unable to load SAP systems:" &
                Environment.NewLine & ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmbSystems.Enabled = False
            btnAnalyze.Enabled = False
        End Try

        lblPageInfo.Text = String.Empty
        btnPreviousPage.Enabled = False
        btnNextPage.Enabled = False
        btnLockUser.Enabled = False
        grpAction.Visible = False
        btnChangeValidity.Enabled = False
        btnChangeUserGroup.Enabled = False
        btnClear.Enabled = False
        btnExport.Enabled = False
    End Sub

    Private Function CheckSapNcoDependencies() As Boolean
        Dim appFolder As String = AppDomain.CurrentDomain.BaseDirectory
        Dim requiredFiles As String() = {"sapnco.dll", "sapnco_utils.dll"}
        Dim missing As New List(Of String)()

        For Each f As String In requiredFiles
            If Not File.Exists(Path.Combine(appFolder, f)) Then
                missing.Add(f)
            End If
        Next

        If missing.Count > 0 Then
            Dim sb As New System.Text.StringBuilder()
            sb.AppendLine("Missing SAP NCo files:")
            For Each f As String In missing
                sb.AppendLine("  " & f)
            Next
            sb.AppendLine()
            sb.AppendLine("Please copy them into:")
            sb.AppendLine(appFolder)
            MessageBox.Show(sb.ToString(), "Missing Files",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        Return True
    End Function

    ' ===========================
    ' Analyze Button
    ' ===========================

    Private Async Sub btnAnalyze_Click(sender As Object,
                                       e As EventArgs) Handles btnAnalyze.Click
        Dim targetDate As Date = dtTargetDate.Value
        Dim selectedSystem As String = If(
            cmbSystems.SelectedItem IsNot Nothing,
            cmbSystems.SelectedItem.ToString(), String.Empty)

        If String.IsNullOrEmpty(selectedSystem) Then
            MessageBox.Show("Please select a SAP system.",
                            "Validation", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Return
        End If

        If selectedSystem.Contains("Multi System Analysis") Then
            RunMultiSystemAnalysis(targetDate)
            Return
        End If

        ' FIX: Do not snapshot action buttons — let UpdatePage set them
        Dim prevAnalyze As Boolean = btnAnalyze.Enabled
        Dim prevRFC As Boolean = btnRFCSettings.Enabled
        Dim prevCmb As Boolean = cmbSystems.Enabled

        btnAnalyze.Enabled = False
        btnRFCSettings.Enabled = False
        cmbSystems.Enabled = False
        btnLockUser.Enabled = False
        grpAction.Visible = False

        btnChangeValidity.Enabled = False
        btnChangeUserGroup.Enabled = False
        btnClear.Enabled = False
        btnExport.Enabled = False

        Dim loading As LoadingForm = Nothing
        Try
            loading = New LoadingForm("Running analysis...") With {
                .StartPosition = FormStartPosition.CenterScreen
            }
            loading.Show(Me)
            loading.Refresh()

            ' Step 1: Ping
            Dim connected As Boolean = Await Task.Run(
                Function() As Boolean
                    Try
                        Dim dest As RfcDestination =
                            RfcDestinationManager.GetDestination(selectedSystem)
                        dest.Ping()
                        Return True
                    Catch
                        Return False
                    End Try
                End Function)

            If Not connected Then
                MessageBox.Show(
                    "Unable to connect to SAP RFC destination '" &
                    selectedSystem & "'." & Environment.NewLine &
                    "Please check your RFC settings.",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Step 2: Fetch data
            Dim rawDt As DataTable = Nothing
            Try
                rawDt = Await Task.Run(
                    Function() As DataTable
                        Dim helper As New SAPHelper()
                        Return helper.GetUSR02Data(selectedSystem)
                    End Function)
            Catch ex As Exception
                MessageBox.Show(
                    "Error fetching data from SAP:" &
                    Environment.NewLine & ex.Message,
                    "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            ' Step 3: Snapshot checkbox state
            Dim inclDialog As Boolean = chkDialog.Checked
            Dim inclService As Boolean = chkService.Checked
            Dim inclComm As Boolean = chkComm.Checked
            Dim inclSystem As Boolean = chkSystem.Checked
            Dim exclLocked As Boolean = chkLockedUsers.Checked
            Dim inclValid As Boolean = chkValidUsers.Checked
            Dim inclInvalid As Boolean = chkInvalidUsers.Checked

            ' FIX: Warn if no user type selected
            If Not inclDialog AndAlso Not inclService AndAlso
               Not inclComm AndAlso Not inclSystem Then
                MessageBox.Show(
                    "Please select at least one user type (Dialog, Service, Comm, System).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Step 4: Build result
            Dim builtResult As DataTable = Await Task.Run(
                Function() As DataTable
                    Return BuildResultTable(
                        rawDt, targetDate,
                        inclDialog, inclService, inclComm, inclSystem,
                        exclLocked, inclValid, inclInvalid)
                End Function)

            ' Step 5: Pagination
            Dim parsedPageSize As Integer
            If Not Integer.TryParse(txtPageSize.Text, parsedPageSize) OrElse
               parsedPageSize <= 0 Then
                parsedPageSize = 20
                txtPageSize.Text = "20"
            End If

            _pageSize = parsedPageSize
            _fullResult = builtResult
            _currentPage = 1
            _totalPages = If(
                _fullResult IsNot Nothing AndAlso _fullResult.Rows.Count > 0,
                CInt(Math.Ceiling(_fullResult.Rows.Count / CDbl(_pageSize))),
                1)

            UpdatePage()

        Catch ex As Exception
            MessageBox.Show(
                "Error during analysis:" & Environment.NewLine & ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

            ' FIX: Restore only navigation controls — UpdatePage handles data buttons
            btnAnalyze.Enabled = prevAnalyze
            btnRFCSettings.Enabled = prevRFC
            cmbSystems.Enabled = prevCmb
        End Try
    End Sub

    ' ===========================
    ' Multi System Analysis
    ' ===========================

    Private Async Sub RunMultiSystemAnalysis(targetDate As Date)
        Dim allSystems As List(Of String) = SAPConnection.GetAvailableSystems()
        If allSystems.Count < 2 Then
            MessageBox.Show(
                "At least 2 SAP systems are required for multi-system analysis.",
                "Multi System Analysis",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectionForm As New MultiSystemSelectionForm(allSystems)
        If selectionForm.ShowDialog(Me) <> DialogResult.OK Then
            selectionForm.Dispose()
            Return
        End If

        Dim selectedSystems As List(Of String) = selectionForm.SelectedSystems
        selectionForm.Dispose()

        If selectedSystems.Count < 2 Then
            MessageBox.Show("Please select at least 2 systems.",
                            "Multi System Analysis",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim prevAnalyze As Boolean = btnAnalyze.Enabled
        Dim prevRFC As Boolean = btnRFCSettings.Enabled
        Dim prevCmb As Boolean = cmbSystems.Enabled

        btnAnalyze.Enabled = False
        btnRFCSettings.Enabled = False
        cmbSystems.Enabled = False
        btnLockUser.Enabled = False
        grpAction.Visible = False

        btnChangeValidity.Enabled = False
        btnChangeUserGroup.Enabled = False
        btnClear.Enabled = False
        btnExport.Enabled = False

        Dim loading As LoadingForm = Nothing
        Dim continueAnalysis As Boolean = True ' FIX: flag instead of Return in Try

        Try
            loading = New LoadingForm("Running multi-system analysis...") With {
                .StartPosition = FormStartPosition.CenterScreen
            }
            loading.Show(Me)
            loading.Refresh()

            ' Step 1: Ping all selected systems
            Dim failedPings As New List(Of String)()
            For Each sysName As String In selectedSystems
                Dim name As String = sysName
                Dim pingOk As Boolean = Await Task.Run(
                    Function() As Boolean
                        Try
                            Dim dest As RfcDestination =
                                RfcDestinationManager.GetDestination(name)
                            dest.Ping()
                            Return True
                        Catch
                            Return False
                        End Try
                    End Function)
                If Not pingOk Then failedPings.Add(sysName)
            Next

            If failedPings.Count > 0 Then
                Dim msg As String =
                    "Unable to connect to the following system(s):" &
                    Environment.NewLine &
                    String.Join(", ", failedPings) &
                    Environment.NewLine & Environment.NewLine &
                    "Do you want to continue with the remaining systems?"

                ' FIX: Use flag instead of Return inside Try
                If MessageBox.Show(msg, "Connection Warning",
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Warning) <> DialogResult.Yes Then
                    continueAnalysis = False
                Else
                    For Each f As String In failedPings
                        selectedSystems.Remove(f)
                    Next
                    If selectedSystems.Count < 2 Then
                        MessageBox.Show(
                            "Less than 2 systems are reachable. Cannot proceed.",
                            "Multi System Analysis",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                        continueAnalysis = False
                    End If
                End If
            End If

            If Not continueAnalysis Then Return

            ' Step 2: Fetch data
            Dim allSystemData As Dictionary(Of String, DataTable) =
                Await Task.Run(
                    Function() As Dictionary(Of String, DataTable)
                        Dim helper As New SAPHelper()
                        Return helper.GetMultiSystemUSR02Data(selectedSystems)
                    End Function)

            ' Step 3: Snapshot checkboxes
            Dim inclDialog As Boolean = chkDialog.Checked
            Dim inclService As Boolean = chkService.Checked
            Dim inclComm As Boolean = chkComm.Checked
            Dim inclSystem As Boolean = chkSystem.Checked
            Dim exclLocked As Boolean = chkLockedUsers.Checked
            Dim inclValid As Boolean = chkValidUsers.Checked
            Dim inclInvalid As Boolean = chkInvalidUsers.Checked

            ' Step 4: Build merged cross-system result table
            ' FIX: Pass creationFilterDays to builder
            Dim creationFilterDays As Integer = 15
            If Not Integer.TryParse(txtcreationFilterDays.Text.Trim(),
                         creationFilterDays) OrElse
   creationFilterDays < 0 Then
                creationFilterDays = 15
            End If

            Dim builtResult As DataTable = Await Task.Run(
    Function() As DataTable
        Return BuildMultiSystemResultTable(
            allSystemData, selectedSystems, targetDate,
            inclDialog, inclService, inclComm, inclSystem,
            exclLocked, inclValid, inclInvalid,
            creationFilterDays)
    End Function)

            ' Step 5: Pagination
            Dim parsedPageSize As Integer
            If Not Integer.TryParse(txtPageSize.Text, parsedPageSize) OrElse
               parsedPageSize <= 0 Then
                parsedPageSize = 20
                txtPageSize.Text = "20"
            End If

            _pageSize = parsedPageSize
            _fullResult = builtResult
            _currentPage = 1
            _totalPages = If(
                _fullResult IsNot Nothing AndAlso _fullResult.Rows.Count > 0,
                CInt(Math.Ceiling(_fullResult.Rows.Count / CDbl(_pageSize))),
                1)

            UpdatePage()

        Catch ex As Exception
            MessageBox.Show(
                "Error during multi-system analysis:" &
                Environment.NewLine & ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

            ' FIX: Only restore nav controls
            btnAnalyze.Enabled = prevAnalyze
            btnRFCSettings.Enabled = prevRFC
            cmbSystems.Enabled = prevCmb
        End Try
    End Sub

    ' ===========================
    ' Result Table Builder
    ' ===========================

    ''' <summary>
    ''' Builds the filtered result DataTable from raw USR02 data.
    ''' FIX: Handles ERDAT = "00000000", blank USTYP, and removed
    '''      the 15-day creation filter that was silently dropping users.
    ''' </summary>
    Private Function BuildResultTable(
        rawDt As DataTable,
        targetDate As Date,
        inclDialog As Boolean,
        inclService As Boolean,
        inclComm As Boolean,
        inclSystem As Boolean,
        exclLocked As Boolean,
        inclValid As Boolean,
        inclInvalid As Boolean) As DataTable

        Dim res As New DataTable()
        res.Columns.Add("Username")
        res.Columns.Add("Created On")
        res.Columns.Add("Valid To")
        res.Columns.Add("Last Login")
        res.Columns.Add("User Group")
        res.Columns.Add("User Type")
        res.Columns.Add("Locked Status")
        res.Columns.Add("Last Password Change")
        res.Columns.Add("Status")

        If rawDt Is Nothing OrElse rawDt.Rows.Count = 0 Then Return res

        ' Build allowed user types
        ' FIX: Include empty/space USTYP as Dialog type
        ' SAP sometimes stores Dialog users with USTYP = " " or ""
        Dim allowedTypes As New HashSet(Of String)(
        StringComparer.OrdinalIgnoreCase)
        If inclDialog Then
            allowedTypes.Add("A")
            allowedTypes.Add(" ")  ' SAP default/blank = Dialog
            allowedTypes.Add("")   ' empty string
        End If
        If inclService Then allowedTypes.Add("S")
        If inclComm Then allowedTypes.Add("C")
        If inclSystem Then allowedTypes.Add("B")

        For Each row As DataRow In rawDt.Rows
            Try
                Dim username As String = row("BNAME").ToString().Trim()

                ' FIX: Skip truly empty usernames only
                If String.IsNullOrWhiteSpace(username) Then Continue For

                Dim createdStr As String = row("ERDAT").ToString().Trim()
                Dim validToStr As String = row("GLTGB").ToString().Trim()
                Dim lastLoginStr As String = row("TRDAT").ToString().Trim()
                Dim userGroup As String = row("CLASS").ToString().Trim()
                Dim userType As String = row("USTYP").ToString().Trim()
                Dim uflag As String = row("UFLAG").ToString().Trim()

                Dim pwdChange As String = String.Empty
                If rawDt.Columns.Contains("PWDLGNDATE") Then
                    pwdChange = row("PWDLGNDATE").ToString().Trim()
                ElseIf rawDt.Columns.Contains("PWDCHGDATE") Then
                    pwdChange = row("PWDCHGDATE").ToString().Trim()
                End If

                ' -----------------------------------------------
                ' FIX: Parse ERDAT — treat "00000000" and blank
                ' as "unknown creation date" — do NOT skip the row
                ' Use Date.MinValue as sentinel for unknown
                ' -----------------------------------------------
                Dim createdDate As Date
                Dim createdDisplay As String
                Dim isUnknownCreated As Boolean = False

                If String.IsNullOrWhiteSpace(createdStr) OrElse
               createdStr = "00000000" OrElse
               createdStr.Replace("0", "").Trim().Length = 0 Then
                    ' Unknown creation date — do not skip
                    isUnknownCreated = True
                    createdDate = Date.MinValue
                    createdDisplay = "Unknown"
                ElseIf Date.TryParseExact(createdStr, "yyyyMMdd",
                Globalization.CultureInfo.InvariantCulture,
                Globalization.DateTimeStyles.None, createdDate) Then
                    createdDisplay = createdDate.ToString("dd.MM.yyyy")
                Else
                    ' Unparseable date — do not skip, treat as unknown
                    isUnknownCreated = True
                    createdDate = Date.MinValue
                    createdDisplay = "Unknown"
                End If

                ' -----------------------------------------------
                ' FIX: Only apply 15-day filter for KNOWN creation
                ' dates. Users with unknown ERDAT are never skipped
                ' by this filter.
                ' -----------------------------------------------
                Dim creationFilterDays As Integer = 15 ' default fallback

                If Not Integer.TryParse(txtcreationFilterDays.Text.Trim(), creationFilterDays) OrElse
   creationFilterDays < 0 Then
                    creationFilterDays = 15 ' reset to default if invalid or negative
                    txtcreationFilterDays.Text = "15"
                End If
                If Not isUnknownCreated Then
                    If (Date.Now - createdDate).Days <= creationFilterDays Then
                        Continue For
                    End If
                End If

                ' -----------------------------------------------
                ' FIX: User type filter
                ' If no type selected → warn caller, skip all
                ' If allowedTypes has entries → filter strictly
                ' Blank USTYP treated as Dialog when Dialog checked
                ' -----------------------------------------------
                If allowedTypes.Count > 0 Then
                    If Not allowedTypes.Contains(userType) Then
                        Continue For
                    End If
                End If

                ' Parse lock flag
                ' FIX: Check multiple UFLAG bits for completeness
                ' Bit 64  = Locked by admin
                ' Bit 128 = Locked after failed logins
                ' Bit 32  = Locked by CUA
                Dim uflagValue As Integer
                If Not Integer.TryParse(uflag, uflagValue) Then
                    uflagValue = 0
                End If
                Dim isLocked As Boolean =
                (uflagValue And 64) <> 0 OrElse   ' Admin lock
                (uflagValue And 128) <> 0 OrElse  ' Too many failed logins
                (uflagValue And 32) <> 0           ' CUA lock

                If exclLocked AndAlso isLocked Then Continue For

                ' Determine locked status label
                Dim lockedStatus As String
                If (uflagValue And 64) <> 0 Then
                    lockedStatus = "Locked (Admin)"
                ElseIf (uflagValue And 128) <> 0 Then
                    lockedStatus = "Locked (Login Errors)"
                ElseIf (uflagValue And 32) <> 0 Then
                    lockedStatus = "Locked (CUA)"
                ElseIf uflagValue = 0 Then
                    lockedStatus = "Unlocked"
                Else
                    lockedStatus = "Locked"
                End If

                ' Parse last login
                Dim lastLoginDate As Date
                Dim lastLoginDisplay As String
                If Date.TryParseExact(lastLoginStr, "yyyyMMdd",
                Globalization.CultureInfo.InvariantCulture,
                Globalization.DateTimeStyles.None, lastLoginDate) Then
                    lastLoginDisplay = lastLoginDate.ToString("dd.MM.yyyy")
                Else
                    lastLoginDate = Date.MinValue
                    lastLoginDisplay = "Never"
                End If

                ' Parse valid-to date
                Dim validToDate As Date
                Dim isValidUser As Boolean = False
                Dim validToDisplay As String = "No Expiry"

                If String.IsNullOrWhiteSpace(validToStr) OrElse
               validToStr = "00000000" OrElse
               validToStr.Replace("0", "").Trim().Length = 0 Then
                    ' No expiry date set = valid indefinitely
                    isValidUser = True
                    validToDisplay = "No Expiry"
                ElseIf Date.TryParseExact(validToStr, "yyyyMMdd",
                Globalization.CultureInfo.InvariantCulture,
                Globalization.DateTimeStyles.None, validToDate) Then
                    isValidUser = (validToDate >= Date.Now.Date)
                    validToDisplay = validToDate.ToString("dd.MM.yyyy")
                Else
                    isValidUser = True
                    validToDisplay = "Unknown"
                End If

                ' Apply valid/invalid filter
                If inclValid AndAlso Not inclInvalid Then
                    If Not isValidUser Then Continue For
                ElseIf inclInvalid AndAlso Not inclValid Then
                    If isValidUser Then Continue For
                ElseIf Not inclValid AndAlso Not inclInvalid Then
                    Continue For ' Nothing selected = show nothing
                End If
                ' If both checked = show all

                ' Parse password change date
                Dim pwdChangeFormatted As String = "Never"
                If Not String.IsNullOrWhiteSpace(pwdChange) AndAlso
               pwdChange <> "00000000" Then
                    Dim pwdChangeDate As Date
                    If Date.TryParseExact(pwdChange, "yyyyMMdd",
                    Globalization.CultureInfo.InvariantCulture,
                    Globalization.DateTimeStyles.None, pwdChangeDate) Then
                        pwdChangeFormatted = pwdChangeDate.ToString("dd.MM.yyyy")
                    End If
                End If

                ' Determine active/inactive status
                ' FIX: Never-logged-in users are always Inactive
                Dim status As String
                If lastLoginDate = Date.MinValue Then
                    status = "Inactive (Never Logged In)"
                ElseIf lastLoginDate >= targetDate Then
                    status = "Active"
                Else
                    status = "Inactive"
                End If

                ' FIX: Resolve user type display label
                Dim userTypeDisplay As String
                Select Case userType.ToUpperInvariant()
                    Case "A" : userTypeDisplay = "A (Dialog)"
                    Case "S" : userTypeDisplay = "S (Service)"
                    Case "C" : userTypeDisplay = "C (Comm)"
                    Case "B" : userTypeDisplay = "B (System)"
                    Case "L" : userTypeDisplay = "L (Reference)"
                    Case Else
                        userTypeDisplay = If(String.IsNullOrWhiteSpace(userType),
                                         "A (Dialog)", userType)
                End Select

                res.Rows.Add(
                username,
                createdDisplay,
                validToDisplay,
                lastLoginDisplay,
                userGroup,
                userTypeDisplay,
                lockedStatus,
                pwdChangeFormatted,
                status)

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(
                "BuildResultTable row error: " & ex.Message)
            End Try
        Next

        Return res
    End Function

    ' ================================================================
    ' BuildMultiSystemResultTable — same ERDAT and USTYP fixes applied
    ' ================================================================
    ''' <summary>
    ''' Builds a cross-system merged result table.
    ''' FIX: Each system's data is read independently using its own
    '''      DataRow — no column name conflicts between systems.
    '''      A user appears if they pass filters in AT LEAST ONE system.
    ''' </summary>
    Private Function BuildMultiSystemResultTable(
    allSystemData As Dictionary(Of String, DataTable),
    selectedSystems As List(Of String),
    targetDate As Date,
    inclDialog As Boolean,
    inclService As Boolean,
    inclComm As Boolean,
    inclSystem As Boolean,
    exclLocked As Boolean,
    inclValid As Boolean,
    inclInvalid As Boolean,
    creationFilterDays As Integer) As DataTable

        ' -------------------------------------------------------
        ' Build result table schema
        ' -------------------------------------------------------
        Dim res As New DataTable()
        res.Columns.Add("Username")
        res.Columns.Add("User Type")
        res.Columns.Add("User Group")
        res.Columns.Add("Created On")
        res.Columns.Add("Valid To")
        res.Columns.Add("Locked Status")
        res.Columns.Add("Last Password Change")

        For Each sysName As String In selectedSystems
            res.Columns.Add("Last Login (" & sysName & ")")
            res.Columns.Add("Status (" & sysName & ")")
        Next

        res.Columns.Add("Cross-System Status")
        res.Columns.Add("Systems Active In")

        ' -------------------------------------------------------
        ' Allowed user types — blank/space = Dialog
        ' -------------------------------------------------------
        Dim allowedTypes As New HashSet(Of String)(
        StringComparer.OrdinalIgnoreCase)
        If inclDialog Then
            allowedTypes.Add("A")
            allowedTypes.Add(" ")
            allowedTypes.Add("")
        End If
        If inclService Then allowedTypes.Add("S")
        If inclComm Then allowedTypes.Add("C")
        If inclSystem Then allowedTypes.Add("B")

        ' -------------------------------------------------------
        ' FIX: Collect all unique usernames across all systems
        ' Store as: userSystemRows(username)(systemName) = DataRow
        ' Each DataRow belongs to its OWN system's DataTable
        ' — no column name conflicts
        ' -------------------------------------------------------
        Dim userSystemRows As New Dictionary(Of String,
        Dictionary(Of String, DataRow))(StringComparer.OrdinalIgnoreCase)

        For Each sysName As String In selectedSystems
            If Not allSystemData.ContainsKey(sysName) Then Continue For
            Dim sysDt As DataTable = allSystemData(sysName)

            ' Skip systems that returned an error
            If sysDt Is Nothing OrElse
           sysDt.Columns.Contains("ERROR") Then Continue For

            ' Verify required columns exist in this system's table
            Dim requiredCols As String() = {
            "BNAME", "ERDAT", "GLTGB", "TRDAT",
            "CLASS", "USTYP", "UFLAG", "PWDCHGDATE"
        }
            Dim missingCol As Boolean = False
            For Each col As String In requiredCols
                If Not sysDt.Columns.Contains(col) Then
                    System.Diagnostics.Debug.WriteLine(
                    "System '" & sysName &
                    "' missing column: " & col)
                    missingCol = True
                    Exit For
                End If
            Next
            If missingCol Then Continue For

            For Each row As DataRow In sysDt.Rows
                Try
                    Dim username As String =
                    row("BNAME").ToString().Trim()
                    If String.IsNullOrWhiteSpace(username) Then
                        Continue For
                    End If

                    If Not userSystemRows.ContainsKey(username) Then
                        userSystemRows(username) =
                        New Dictionary(Of String, DataRow)(
                            StringComparer.OrdinalIgnoreCase)
                    End If

                    ' FIX: Each system stores its OWN row reference
                    ' No overwriting — each system slot is independent
                    If Not userSystemRows(username).ContainsKey(sysName) Then
                        userSystemRows(username)(sysName) = row
                    End If

                Catch ex As Exception
                    System.Diagnostics.Debug.WriteLine(
                    "Row collect error [" & sysName &
                    "]: " & ex.Message)
                    Continue For
                End Try
            Next
        Next

        System.Diagnostics.Debug.WriteLine(
        "BuildMultiSystem: unique users found = " &
        userSystemRows.Count)

        ' -------------------------------------------------------
        ' Process each unique user
        ' -------------------------------------------------------
        For Each kvp In userSystemRows
            Try
                Dim username As String = kvp.Key
                Dim systemRows As Dictionary(Of String, DataRow) = kvp.Value

                ' ---------------------------------------------------
                ' FIX: Read metadata independently from EACH system
                ' Use the system with the most recent login for
                ' base metadata — but read THAT system's own row
                ' directly to avoid any cross-system field conflicts
                ' ---------------------------------------------------
                Dim bestSysName As String = String.Empty
                Dim bestLoginStr As String = "00000000"

                For Each sn As String In selectedSystems
                    If systemRows.ContainsKey(sn) Then
                        Dim loginCandidate As String =
                        systemRows(sn)("TRDAT").ToString().Trim()
                        If String.Compare(loginCandidate,
                                      bestLoginStr,
                                      StringComparison.Ordinal) > 0 Then
                            bestLoginStr = loginCandidate
                            bestSysName = sn
                        End If
                    End If
                Next

                ' Fallback: use first available system
                If String.IsNullOrEmpty(bestSysName) Then
                    For Each sn As String In selectedSystems
                        If systemRows.ContainsKey(sn) Then
                            bestSysName = sn
                            Exit For
                        End If
                    Next
                End If

                If String.IsNullOrEmpty(bestSysName) Then Continue For

                ' Read metadata from best system's OWN DataRow
                Dim metaRow As DataRow = systemRows(bestSysName)

                ' ---------------------------------------------------
                ' Safely read each field from metaRow
                ' ---------------------------------------------------
                Dim createdStr As String = SafeGetField(metaRow, "ERDAT")
                Dim validToStr As String = SafeGetField(metaRow, "GLTGB")
                Dim userGroup As String = SafeGetField(metaRow, "CLASS")
                Dim userType As String = SafeGetField(metaRow, "USTYP")
                Dim uflag As String = SafeGetField(metaRow, "UFLAG")
                Dim pwdChange As String = SafeGetField(metaRow, "PWDLGNDATE")
                If String.IsNullOrWhiteSpace(pwdChange) Then
                    pwdChange = SafeGetField(metaRow, "PWDCHGDATE")
                End If

                ' ---------------------------------------------------
                ' ERDAT — handle zeros/blank
                ' ---------------------------------------------------
                Dim createdDate As Date
                Dim createdDisplay As String
                Dim isUnknownCreated As Boolean = False

                If String.IsNullOrWhiteSpace(createdStr) OrElse
               createdStr = "00000000" OrElse
               createdStr.Replace("0", "").Trim().Length = 0 Then
                    isUnknownCreated = True
                    createdDate = Date.MinValue
                    createdDisplay = "Unknown"
                ElseIf Date.TryParseExact(createdStr, "yyyyMMdd",
                Globalization.CultureInfo.InvariantCulture,
                Globalization.DateTimeStyles.None,
                createdDate) Then
                    createdDisplay = createdDate.ToString("dd.MM.yyyy")
                Else
                    isUnknownCreated = True
                    createdDate = Date.MinValue
                    createdDisplay = "Unknown"
                End If

                ' Skip recently created users (known dates only)
                If Not isUnknownCreated Then
                    If (Date.Now - createdDate).Days <=
                   creationFilterDays Then Continue For
                End If

                ' ---------------------------------------------------
                ' User type filter
                ' ---------------------------------------------------
                If allowedTypes.Count > 0 Then
                    If Not allowedTypes.Contains(userType) Then
                        Continue For
                    End If
                End If

                ' ---------------------------------------------------
                ' Lock flag — bits 32, 64, 128
                ' ---------------------------------------------------
                Dim uflagValue As Integer
                If Not Integer.TryParse(uflag, uflagValue) Then
                    uflagValue = 0
                End If
                Dim isLocked As Boolean =
                (uflagValue And 64) <> 0 OrElse
                (uflagValue And 128) <> 0 OrElse
                (uflagValue And 32) <> 0

                If exclLocked AndAlso isLocked Then Continue For

                Dim lockedStatus As String
                If (uflagValue And 64) <> 0 Then
                    lockedStatus = "Locked (Admin)"
                ElseIf (uflagValue And 128) <> 0 Then
                    lockedStatus = "Locked (Login Errors)"
                ElseIf (uflagValue And 32) <> 0 Then
                    lockedStatus = "Locked (CUA)"
                ElseIf uflagValue = 0 Then
                    lockedStatus = "Unlocked"
                Else
                    lockedStatus = "Locked"
                End If

                ' ---------------------------------------------------
                ' Valid-to date
                ' ---------------------------------------------------
                Dim validToDate As Date
                Dim isValidUser As Boolean = False
                Dim validToDisplay As String = "No Expiry"

                If String.IsNullOrWhiteSpace(validToStr) OrElse
               validToStr = "00000000" OrElse
               validToStr.Replace("0", "").Trim().Length = 0 Then
                    isValidUser = True
                    validToDisplay = "No Expiry"
                ElseIf Date.TryParseExact(validToStr, "yyyyMMdd",
                Globalization.CultureInfo.InvariantCulture,
                Globalization.DateTimeStyles.None,
                validToDate) Then
                    isValidUser = (validToDate >= Date.Now.Date)
                    validToDisplay = validToDate.ToString("dd.MM.yyyy")
                Else
                    isValidUser = True
                    validToDisplay = "Unknown"
                End If

                ' Valid/invalid filter
                If inclValid AndAlso Not inclInvalid Then
                    If Not isValidUser Then Continue For
                ElseIf inclInvalid AndAlso Not inclValid Then
                    If isValidUser Then Continue For
                ElseIf Not inclValid AndAlso Not inclInvalid Then
                    Continue For
                End If

                ' ---------------------------------------------------
                ' Password change date
                ' ---------------------------------------------------
                Dim pwdChangeFormatted As String = "Never"
                If Not String.IsNullOrWhiteSpace(pwdChange) AndAlso
               pwdChange <> "00000000" Then
                    Dim pwdChangeDate As Date
                    If Date.TryParseExact(pwdChange, "yyyyMMdd",
                    Globalization.CultureInfo.InvariantCulture,
                    Globalization.DateTimeStyles.None,
                    pwdChangeDate) Then
                        pwdChangeFormatted =
                        pwdChangeDate.ToString("dd.MM.yyyy")
                    End If
                End If

                ' User type display label
                Dim userTypeDisplay As String
                Select Case userType.ToUpperInvariant()
                    Case "A" : userTypeDisplay = "A (Dialog)"
                    Case "S" : userTypeDisplay = "S (Service)"
                    Case "C" : userTypeDisplay = "C (Comm)"
                    Case "B" : userTypeDisplay = "B (System)"
                    Case "L" : userTypeDisplay = "L (Reference)"
                    Case Else
                        userTypeDisplay = If(
                        String.IsNullOrWhiteSpace(userType),
                        "A (Dialog)", userType)
                End Select

                ' ---------------------------------------------------
                ' Build row values
                ' ---------------------------------------------------
                Dim rowValues As New List(Of Object)()
                rowValues.Add(username)
                rowValues.Add(userTypeDisplay)
                rowValues.Add(userGroup)
                rowValues.Add(createdDisplay)
                rowValues.Add(validToDisplay)
                rowValues.Add(lockedStatus)
                rowValues.Add(pwdChangeFormatted)

                ' ---------------------------------------------------
                ' FIX: Per-system login data — read EACH system's
                ' OWN DataRow independently — zero column conflicts
                ' ---------------------------------------------------
                Dim activeCount As Integer = 0
                Dim activeSystemNames As New List(Of String)()

                For Each sysName As String In selectedSystems
                    If systemRows.ContainsKey(sysName) Then
                        ' Read login date from THIS system's own row
                        Dim sysRow As DataRow = systemRows(sysName)
                        Dim loginStr As String =
                        SafeGetField(sysRow, "TRDAT")

                        Dim loginDate As Date
                        Dim loginDisplay As String = "Never"
                        Dim sysStatus As String = "Inactive"

                        If Not String.IsNullOrWhiteSpace(loginStr) AndAlso
                       loginStr <> "00000000" AndAlso
                       Date.TryParseExact(loginStr, "yyyyMMdd",
                           Globalization.CultureInfo.InvariantCulture,
                           Globalization.DateTimeStyles.None,
                           loginDate) Then
                            loginDisplay = loginDate.ToString("dd.MM.yyyy")
                            If loginDate >= targetDate Then
                                sysStatus = "Active"
                                activeCount += 1
                                activeSystemNames.Add(sysName)
                            End If
                        End If

                        rowValues.Add(loginDisplay)
                        rowValues.Add(sysStatus)
                    Else
                        ' User does not exist in this system
                        rowValues.Add("N/A")
                        rowValues.Add("Not In System")
                    End If
                Next

                ' Cross-system status
                Dim crossStatus As String
                Dim activeInList As String

                If activeCount > 0 Then
                    crossStatus = "Active"
                    activeInList = String.Join(", ", activeSystemNames)
                Else
                    crossStatus = "Inactive In All Systems"
                    activeInList = "None"
                End If

                rowValues.Add(crossStatus)
                rowValues.Add(activeInList)

                res.Rows.Add(rowValues.ToArray())

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(
                "BuildMultiSystem row error [" &
                kvp.Key & "]: " & ex.Message)
                Continue For
            End Try
        Next

        System.Diagnostics.Debug.WriteLine(
        "BuildMultiSystem: result rows = " & res.Rows.Count)

        Return res
    End Function

    ' -------------------------------------------------------
    ' FIX: Helper to safely read a field from a DataRow
    ' Returns String.Empty if column missing or value is DBNull
    ' Prevents cross-system column name exceptions
    ' -------------------------------------------------------
    Private Function SafeGetField(row As DataRow,
                               columnName As String) As String
        Try
            If row Is Nothing Then Return String.Empty
            If Not row.Table.Columns.Contains(columnName) Then
                Return String.Empty
            End If
            Dim val As Object = row(columnName)
            If val Is Nothing OrElse
           IsDBNull(val) Then Return String.Empty
            Return val.ToString().Trim()
        Catch
            Return String.Empty
        End Try
    End Function
    ' ===========================
    ' Pagination
    ' ===========================

    Private Sub UpdatePage()
        If _fullResult Is Nothing OrElse _fullResult.Rows.Count = 0 Then
            dgvResult.DataSource = Nothing
            lblPageInfo.Text = "No results found."
            btnPreviousPage.Enabled = False
            btnNextPage.Enabled = False
            btnLockUser.Enabled = False
            grpAction.Visible = False

            grpAction.Visible = False
            btnChangeValidity.Enabled = False
            btnChangeUserGroup.Enabled = False
            btnClear.Enabled = False
            btnExport.Enabled = False
            Return
        End If

        Dim startRow As Integer = (_currentPage - 1) * _pageSize
        Dim pagedTable As DataTable = _fullResult.Clone()

        For i As Integer = startRow To Math.Min(
            startRow + _pageSize - 1, _fullResult.Rows.Count - 1)
            pagedTable.ImportRow(_fullResult.Rows(i))
        Next

        dgvResult.DataSource = pagedTable
        lblPageInfo.Text = "Page " & _currentPage & " of " & _totalPages &
                           "  (" & _fullResult.Rows.Count & " total records)"

        btnPreviousPage.Enabled = (_currentPage > 1)
        btnNextPage.Enabled = (_currentPage < _totalPages)

        Dim hasRows As Boolean = (dgvResult.Rows.Count > 0)
        Dim hasSelected As Boolean = (dgvResult.SelectedRows.Count > 0 OrElse
                                      dgvResult.SelectedCells.Count > 0)

        ' FIX: Disable Lock in multi-system mode
        Dim isMultiSystem As Boolean =
            cmbSystems.SelectedItem IsNot Nothing AndAlso
            cmbSystems.SelectedItem.ToString().Contains("Multi System Analysis")

        btnLockUser.Enabled = hasRows AndAlso hasSelected AndAlso Not isMultiSystem
        btnChangeValidity.Enabled = hasRows AndAlso hasSelected
        btnChangeUserGroup.Enabled = hasRows AndAlso hasSelected
        btnClear.Enabled = hasRows
        grpAction.Visible = True
        btnExport.Enabled = hasRows
    End Sub

    Private Sub dgvResult_SelectionChanged(sender As Object,
                                           e As EventArgs) Handles dgvResult.SelectionChanged
        Dim hasRows As Boolean = (dgvResult.Rows.Count > 0)
        Dim hasSelected As Boolean = (dgvResult.SelectedRows.Count > 0 OrElse
                                      dgvResult.SelectedCells.Count > 0)

        Dim isMultiSystem As Boolean =
            cmbSystems.SelectedItem IsNot Nothing AndAlso
            cmbSystems.SelectedItem.ToString().Contains("Multi System Analysis")

        btnLockUser.Enabled = hasRows AndAlso hasSelected AndAlso Not isMultiSystem
        btnChangeValidity.Enabled = hasRows AndAlso hasSelected
        btnChangeUserGroup.Enabled = hasRows AndAlso hasSelected
        btnClear.Enabled = hasRows
        btnExport.Enabled = hasRows
        grpAction.Visible = True
    End Sub

    Private Sub btnNextPage_Click(sender As Object,
                                  e As EventArgs) Handles btnNextPage.Click
        If _currentPage < _totalPages Then
            _currentPage += 1
            UpdatePage()
        End If
    End Sub

    Private Sub btnPreviousPage_Click(sender As Object,
                                      e As EventArgs) Handles btnPreviousPage.Click
        If _currentPage > 1 Then
            _currentPage -= 1
            UpdatePage()
        End If
    End Sub

    ' ===========================
    ' Export — FIX: Always export _fullResult
    ' ===========================

    Private Sub btnExport_Click(sender As Object,
                                e As EventArgs) Handles btnExport.Click
        If _fullResult Is Nothing OrElse _fullResult.Rows.Count = 0 Then
            MessageBox.Show("No data to export.", "Export",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Using sfd As New SaveFileDialog()
            sfd.Title = "Save Report"
            sfd.Filter = "Excel File (*.xlsx)|*.xlsx|CSV File (*.csv)|*.csv"
            sfd.FileName = "User Inactivity Report_" &
                           DateTime.Now.ToString("dd.MM.yyyy_HH-mm")

            If sfd.ShowDialog() = DialogResult.OK Then
                Dim ext As String =
                    Path.GetExtension(sfd.FileName).ToLowerInvariant()

                If ext = ".csv" Then
                    ExportToCSV(sfd.FileName)
                Else
                    ExportToExcel(sfd.FileName)
                End If

                MessageBox.Show("Export successful!", "Export",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub

    ' FIX: Export _fullResult (all pages) not just current page
    Private Sub ExportToCSV(filePath As String)
        Using writer As New IO.StreamWriter(
            filePath, False, System.Text.Encoding.UTF8)

            ' Header
            Dim headers As New System.Text.StringBuilder()
            For Each col As DataColumn In _fullResult.Columns
                If headers.Length > 0 Then headers.Append(",")
                headers.Append("""" & col.ColumnName & """")
            Next
            writer.WriteLine(headers.ToString())

            ' Data rows from _fullResult
            For Each row As DataRow In _fullResult.Rows
                Dim cells As New System.Text.StringBuilder()
                For Each item As Object In row.ItemArray
                    If cells.Length > 0 Then cells.Append(",")
                    Dim val As String = If(item IsNot Nothing,
                                           item.ToString().Replace("""", "'"),
                                           String.Empty)
                    cells.Append("""" & val & """")
                Next
                writer.WriteLine(cells.ToString())
            Next
        End Using
    End Sub

    Private Sub ExportToExcel(filePath As String)
        Dim excelApp As New Microsoft.Office.Interop.Excel.Application()
        Dim workbook As Microsoft.Office.Interop.Excel.Workbook = Nothing
        Dim worksheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing

        Try
            workbook = excelApp.Workbooks.Add()
            worksheet = DirectCast(workbook.Sheets(1),
                                   Microsoft.Office.Interop.Excel.Worksheet)

            ' Header
            For i As Integer = 1 To _fullResult.Columns.Count
                worksheet.Cells(1, i).value =
                    _fullResult.Columns(i - 1).ColumnName
            Next

            ' Data rows
            For i As Integer = 0 To _fullResult.Rows.Count - 1
                For j As Integer = 0 To _fullResult.Columns.Count - 1
                    worksheet.Cells(i + 2, j + 1).value =
                        _fullResult.Rows(i)(j)
                Next
            Next

            workbook.SaveAs(filePath)

        Finally
            If workbook IsNot Nothing Then workbook.Close(False)
            excelApp.Quit()

            If worksheet IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet)
            End If
            If workbook IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook)
            End If
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp)

            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    ' ===========================
    ' RFC Settings Button
    ' ===========================

    Private Sub btnRFCSettings_Click(sender As Object,
                                     e As EventArgs) Handles btnRFCSettings.Click
        Dim settingsForm As New RFCSettingsForm()
        settingsForm.ShowDialog()
        settingsForm.Dispose()

        Try
            If SAPConnection.RegisteredConfig IsNot Nothing Then
                Try
                    RfcDestinationManager.UnregisterDestinationConfiguration(
                        SAPConnection.RegisteredConfig)
                Catch
                End Try
                SAPConnection.RegisteredConfig = Nothing
            End If
        Catch
        End Try

        Try
            SAPConnection.RefreshCache()
        Catch ex As Exception
            MessageBox.Show(
                "Failed to refresh SAP system configurations:" &
                Environment.NewLine & ex.Message,
                "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

        _sapCfg = New SAPConnection()

        Try
            If Not RfcDestinationManager.IsDestinationConfigurationRegistered() Then
                RfcDestinationManager.RegisterDestinationConfiguration(_sapCfg)
                SAPConnection.RegisteredConfig = _sapCfg
            End If
        Catch
        End Try

        Try
            cmbSystems.Items.Clear()
            If CheckSapNcoDependencies() Then
                Dim systems As List(Of String) = SAPConnection.GetAvailableSystems()
                If systems.Count > 0 Then
                    cmbSystems.Items.AddRange(systems.ToArray())
                    If systems.Count >= 2 Then
                        cmbSystems.Items.Insert(0, "── Multi System Analysis ──")
                        cmbSystems.SelectedIndex = 1
                    Else
                        cmbSystems.SelectedIndex = 0
                    End If
                    cmbSystems.Enabled = True
                    btnAnalyze.Enabled = True
                Else
                    cmbSystems.Enabled = False
                    btnAnalyze.Enabled = False
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(
                "Unable to reload SAP systems:" &
                Environment.NewLine & ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    ' ===========================
    ' Grid Selection Helper — FIX: Use HashSet for O(1) dedup
    ' ===========================

    Private Function GetSelectedUsernames() As List(Of String)
        Dim seen As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Dim usernames As New List(Of String)()

        Dim usernameColIndex As Integer = -1
        For Each col As DataGridViewColumn In dgvResult.Columns
            If col.HeaderText.Equals("Username",
               StringComparison.OrdinalIgnoreCase) Then
                usernameColIndex = col.Index
                Exit For
            End If
        Next
        If usernameColIndex = -1 Then usernameColIndex = 0

        Dim rowIndexes As New HashSet(Of Integer)()

        If dgvResult.SelectedRows.Count > 0 Then
            For Each r As DataGridViewRow In dgvResult.SelectedRows
                rowIndexes.Add(r.Index)
            Next
        ElseIf dgvResult.SelectedCells.Count > 0 Then
            For Each cell As DataGridViewCell In dgvResult.SelectedCells
                rowIndexes.Add(cell.RowIndex)
            Next
        End If

        For Each idx As Integer In rowIndexes
            Dim r As DataGridViewRow = dgvResult.Rows(idx)
            If r.IsNewRow Then Continue For
            Dim uname As String = If(
                r.Cells(usernameColIndex).Value IsNot Nothing,
                r.Cells(usernameColIndex).Value.ToString().Trim(),
                String.Empty)
            If Not String.IsNullOrWhiteSpace(uname) AndAlso seen.Add(uname) Then
                usernames.Add(uname)
            End If
        Next

        Return usernames
    End Function

    ' ===========================
    ' Lock User — FIX: Remove duplicate username collection
    ' ===========================

    Private Sub btnLockUser_Click(sender As Object,
                                  e As EventArgs) Handles btnLockUser.Click
        Dim selectedUsernames As List(Of String) = GetSelectedUsernames()

        If selectedUsernames.Count = 0 Then
            MessageBox.Show("Please select one or more user rows to lock.",
                            "Lock User", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedSystem As String = If(
            cmbSystems.SelectedItem IsNot Nothing,
            cmbSystems.SelectedItem.ToString(), String.Empty)

        If String.IsNullOrEmpty(selectedSystem) Then
            MessageBox.Show("Please select a SAP system first.",
                            "Lock User", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Return
        End If

        If selectedSystem.Contains("Multi System Analysis") Then
            MessageBox.Show(
                "Lock User is not supported in Multi System Analysis mode." &
                Environment.NewLine &
                "Please select a specific SAP system and run analysis first.",
                "Lock User", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim confirmList As String
        If selectedUsernames.Count <= 10 Then
            confirmList = String.Join(", ", selectedUsernames)
        Else
            confirmList = String.Join(", ",
                          selectedUsernames.GetRange(0, 10)) &
                          " ... (+" & (selectedUsernames.Count - 10) & " more)"
        End If

        Dim confirm As DialogResult = MessageBox.Show(
            "Are you sure you want to lock the following user(s) on '" &
            selectedSystem & "':" & Environment.NewLine & confirmList,
            "Confirm Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirm <> DialogResult.Yes Then Return

        Try
            Dim sapHelper As New SAPHelper()
            Dim successes As New List(Of String)()
            Dim failures As New List(Of String)()

            For Each userName As String In selectedUsernames
                Dim message As String = String.Empty
                Dim ok As Boolean =
                    sapHelper.LockUser(selectedSystem, userName, message)
                If ok Then
                    successes.Add(userName)
                Else
                    failures.Add(userName & ": " & message)
                End If
            Next

            Dim summary As New System.Text.StringBuilder()
            If successes.Count > 0 Then
                summary.AppendLine("Locked: " & String.Join(", ", successes))
            End If
            If failures.Count > 0 Then
                summary.AppendLine()
                summary.AppendLine("Failed:")
                For Each f As String In failures
                    summary.AppendLine("  " & f)
                Next
            End If

            Dim icon As MessageBoxIcon = If(failures.Count = 0,
                MessageBoxIcon.Information, MessageBoxIcon.Warning)
            MessageBox.Show(summary.ToString().Trim(),
                            "Lock Users Result", MessageBoxButtons.OK, icon)

            btnAnalyze_Click(Nothing, EventArgs.Empty)

        Catch ex As Exception
            MessageBox.Show(
                "Error while locking user(s):" & Environment.NewLine & ex.Message,
                "Lock User", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===========================
    ' Clear Button
    ' ===========================

    Private Sub btnClear_Click(sender As Object,
                               e As EventArgs) Handles btnClear.Click
        If dgvResult.Rows.Count = 0 Then Return

        If MessageBox.Show("Clear displayed results?", "Clear Results",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        _fullResult = Nothing
        dgvResult.DataSource = Nothing
        lblPageInfo.Text = String.Empty
        _currentPage = 1
        _totalPages = 1

        btnPreviousPage.Enabled = False
        btnNextPage.Enabled = False
        btnLockUser.Enabled = False
        grpAction.Visible = False
        btnChangeValidity.Enabled = False
        btnChangeUserGroup.Enabled = False
        btnClear.Enabled = False
        btnExport.Enabled = False
    End Sub

    ' ===========================
    ' Cleanup & Resource Release
    ' FIX: Remove Rows.Clear() after DataSource = Nothing
    ' ===========================

    Private Sub CleanupAndReleaseResources()
        Try
            _fullResult = Nothing
            If dgvResult IsNot Nothing Then
                dgvResult.DataSource = Nothing
                ' FIX: Do NOT call dgvResult.Rows.Clear() after DataSource = Nothing
                ' It throws InvalidOperationException on bound grids
            End If

            lblPageInfo.Text = String.Empty
            _currentPage = 1
            _totalPages = 1
            _pageSize = 0
            btnPreviousPage.Enabled = False
            btnNextPage.Enabled = False
            btnLockUser.Enabled = False
            grpAction.Visible = False
            btnChangeValidity.Enabled = False
            btnChangeUserGroup.Enabled = False
            btnClear.Enabled = False
            btnExport.Enabled = False
        Catch
        End Try

        Try
            If SAPConnection.RegisteredConfig IsNot Nothing Then
                Try
                    RfcDestinationManager.UnregisterDestinationConfiguration(
                        SAPConnection.RegisteredConfig)
                Catch
                End Try
                SAPConnection.RegisteredConfig = Nothing
            End If
        Catch
        End Try

        _sapCfg = Nothing

        Try
            If _rbtnConnectionIndicator IsNot Nothing Then
                If _connToolTip IsNot Nothing Then
                    _connToolTip.RemoveAll()
                    _connToolTip.Dispose()
                    _connToolTip = Nothing
                End If
                If Controls.Contains(_rbtnConnectionIndicator) Then
                    Controls.Remove(_rbtnConnectionIndicator)
                End If
                _rbtnConnectionIndicator.Dispose()
                _rbtnConnectionIndicator = Nothing
            End If
        Catch
        End Try

        Try
            GC.Collect()
            GC.WaitForPendingFinalizers()
        Catch
        End Try
    End Sub

    ' ===========================
    ' Change Validity — FIX: Remove duplicate username collection
    ' ===========================

    Private Sub btnChangeValidity_Click(sender As Object,
                                        e As EventArgs) Handles btnChangeValidity.Click
        ' FIX: Use helper exclusively — removed duplicate manual collection
        Dim selectedUsernames As List(Of String) = GetSelectedUsernames()

        If selectedUsernames.Count = 0 Then
            MessageBox.Show(
                "Please select one or more user rows to change validity.",
                "Change Validity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedSystem As String = If(
            cmbSystems.SelectedItem IsNot Nothing,
            cmbSystems.SelectedItem.ToString(), String.Empty)

        If String.IsNullOrEmpty(selectedSystem) Then
            MessageBox.Show("Please select a SAP system first.",
                            "Change Validity", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Return
        End If

        Dim targetDate As Date = dtTargetDate.Value
        Dim validToStr As String = targetDate.ToString("dd.MM.yyyy")

        Dim confirmList As String
        If selectedUsernames.Count <= 10 Then
            confirmList = String.Join(", ", selectedUsernames)
        Else
            confirmList = String.Join(", ",
                          selectedUsernames.GetRange(0, 10)) &
                          " ... (+" & (selectedUsernames.Count - 10) & " more)"
        End If

        Dim confirm As DialogResult = MessageBox.Show(
            "Are you sure you want to set validity end date to '" &
            validToStr & "' for the following user(s) on '" &
            selectedSystem & "':" & Environment.NewLine & confirmList,
            "Confirm Change Validity",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirm <> DialogResult.Yes Then Return

        Try
            Dim sapHelper As New SAPHelper()
            Dim successes As New List(Of String)()
            Dim failures As New List(Of String)()

            For Each userName As String In selectedUsernames
                Dim message As String = String.Empty
                Dim ok As Boolean = sapHelper.ChangeUserValidity(
                    selectedSystem, userName, validToStr, message)
                If ok Then
                    successes.Add(userName)
                Else
                    failures.Add(userName & ": " & message)
                End If
            Next

            Dim summary As New System.Text.StringBuilder()
            If successes.Count > 0 Then
                summary.AppendLine("Validity updated: " &
                                   String.Join(", ", successes))
            End If
            If failures.Count > 0 Then
                summary.AppendLine()
                summary.AppendLine("Failed:")
                For Each f As String In failures
                    summary.AppendLine("  " & f)
                Next
            End If

            Dim icon As MessageBoxIcon = If(failures.Count = 0,
                MessageBoxIcon.Information, MessageBoxIcon.Warning)
            MessageBox.Show(summary.ToString().Trim(),
                            "Change Validity Result", MessageBoxButtons.OK, icon)

            btnAnalyze_Click(Nothing, EventArgs.Empty)

        Catch ex As Exception
            MessageBox.Show(
                "Error while changing validity:" & Environment.NewLine & ex.Message,
                "Change Validity", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===========================
    ' Change User Group — FIX: Remove duplicate username collection
    ' ===========================

    Private Sub btnChangeUserGroup_Click(sender As Object,
                                         e As EventArgs) Handles btnChangeUserGroup.Click
        ' FIX: Use helper exclusively — removed duplicate manual collection
        Dim selectedUsernames As List(Of String) = GetSelectedUsernames()

        If selectedUsernames.Count = 0 Then
            MessageBox.Show(
                "Please select one or more user rows to change user group.",
                "Change User Group", MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedSystem As String = If(
            cmbSystems.SelectedItem IsNot Nothing,
            cmbSystems.SelectedItem.ToString(), String.Empty)

        If String.IsNullOrEmpty(selectedSystem) Then
            MessageBox.Show("Please select a SAP system first.",
                            "Change User Group", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Return
        End If

        Dim targetGroup As String = "INACTIVE"

        Try
            Dim sapHelper As New SAPHelper()
            Dim groupCheckMessage As String = String.Empty
            Dim groupExists As Boolean = sapHelper.UserGroupExists(
                selectedSystem, targetGroup, groupCheckMessage)

            If Not groupExists Then
                Dim createMsg As New System.Text.StringBuilder()
                createMsg.AppendLine("The user group '" & targetGroup &
                                     "' does not exist in system '" &
                                     selectedSystem & "'.")
                createMsg.AppendLine()
                createMsg.AppendLine("Please create it via SAP transaction SUGR.")

                If Not String.IsNullOrWhiteSpace(groupCheckMessage) AndAlso
                   Not groupCheckMessage.ToUpperInvariant().Contains(
                       "IS NOT SUPPORTED") AndAlso
                   Not groupCheckMessage.ToUpperInvariant().Contains(
                       "SELECTION CRITERION") Then
                    createMsg.AppendLine()
                    createMsg.AppendLine("Detail: " & groupCheckMessage)
                End If

                MessageBox.Show(createMsg.ToString(),
                                "User Group Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

        Catch ex As Exception
            MessageBox.Show(
                "Error validating user group '" & targetGroup & "':" &
                Environment.NewLine & ex.Message,
                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim confirmList As String
        If selectedUsernames.Count <= 10 Then
            confirmList = String.Join(", ", selectedUsernames)
        Else
            confirmList = String.Join(", ",
                          selectedUsernames.GetRange(0, 10)) &
                          " ... (+" & (selectedUsernames.Count - 10) & " more)"
        End If

        Dim confirm As DialogResult = MessageBox.Show(
            "Are you sure you want to change the user group to '" &
            targetGroup & "' for the following user(s) on '" &
            selectedSystem & "':" & Environment.NewLine & confirmList,
            "Confirm Change User Group",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirm <> DialogResult.Yes Then Return

        Try
            Dim sapHelper As New SAPHelper()
            Dim successes As New List(Of String)()
            Dim failures As New List(Of String)()

            For Each userName As String In selectedUsernames
                Dim message As String = String.Empty
                Dim ok As Boolean = sapHelper.ChangeUserGroup(
                    selectedSystem, userName, targetGroup, message)
                If ok Then
                    successes.Add(userName)
                Else
                    failures.Add(userName & ": " & message)
                End If
            Next

            Dim summary As New System.Text.StringBuilder()
            If successes.Count > 0 Then
                summary.AppendLine("User group changed to '" & targetGroup &
                                   "': " & String.Join(", ", successes))
            End If
            If failures.Count > 0 Then
                summary.AppendLine()
                summary.AppendLine("Failed:")
                For Each f As String In failures
                    summary.AppendLine("  " & f)
                Next
            End If

            Dim icon As MessageBoxIcon = If(failures.Count = 0,
                MessageBoxIcon.Information, MessageBoxIcon.Warning)
            MessageBox.Show(summary.ToString().Trim(),
                            "Change User Group Result",
                            MessageBoxButtons.OK, icon)

            btnAnalyze_Click(Nothing, EventArgs.Empty)

        Catch ex As Exception
            MessageBox.Show(
                "Error while changing user group:" &
                Environment.NewLine & ex.Message,
                "Change User Group", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#End Region
End Class