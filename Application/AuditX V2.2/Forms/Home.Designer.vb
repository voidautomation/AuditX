<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Home
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Home))
        Dim ChartArea7 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend7 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series7 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea8 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend8 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series8 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PanelHeader = New System.Windows.Forms.Panel()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.pbUserProfile = New System.Windows.Forms.PictureBox()
        Me.lblHeader = New System.Windows.Forms.Label()
        Me.TabControlMain = New System.Windows.Forms.TabControl()
        Me.TabHome = New System.Windows.Forms.TabPage()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblRefresh = New System.Windows.Forms.LinkLabel()
        Me.chartStatus = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.chartFolderStatus = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.lblEndTime = New System.Windows.Forms.Label()
        Me.lblPath = New System.Windows.Forms.Label()
        Me.txtFolderPath = New System.Windows.Forms.TextBox()
        Me.imgFolderPath = New System.Windows.Forms.PictureBox()
        Me.lblStartTime = New System.Windows.Forms.Label()
        Me.lblReportTo = New System.Windows.Forms.Label()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.txtReportMonth = New System.Windows.Forms.TextBox()
        Me.btnGenerateWordReport = New System.Windows.Forms.Button()
        Me.lblReportFrom = New System.Windows.Forms.Label()
        Me.lblReportMonth = New System.Windows.Forms.Label()
        Me.dgvControlStatus = New System.Windows.Forms.DataGridView()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblSystem = New System.Windows.Forms.Label()
        Me.lblSystemId = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.cmbSystem = New System.Windows.Forms.ComboBox()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtClient = New System.Windows.Forms.TextBox()
        Me.CheckBoxClient = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbExecutionType = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbExecutionMode = New System.Windows.Forms.ComboBox()
        Me.lblControl = New System.Windows.Forms.Label()
        Me.cmbControl = New System.Windows.Forms.ComboBox()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.TabAppendix = New System.Windows.Forms.TabPage()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PanelButtons = New System.Windows.Forms.Panel()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnSaveXML = New System.Windows.Forms.Button()
        Me.btnRevert = New System.Windows.Forms.Button()
        Me.btnExportCSV = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.TabSettings = New System.Windows.Forms.TabPage()
        Me.TabLogs = New System.Windows.Forms.TabPage()
        Me.dgvLogs = New System.Windows.Forms.DataGridView()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.cmbLogFiles = New System.Windows.Forms.ComboBox()
        Me.TabInstruction = New System.Windows.Forms.TabPage()
        Me.WebView2 = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.TabHosekeeper = New System.Windows.Forms.TabPage()
        Me.PanelMain = New System.Windows.Forms.Panel()
        Me.PanelBody = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btnNextPage = New System.Windows.Forms.Button()
        Me.lblPageInfo = New System.Windows.Forms.Label()
        Me.btnPreviousPage = New System.Windows.Forms.Button()
        Me.dgvResult = New System.Windows.Forms.DataGridView()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GroupBoxUserType = New System.Windows.Forms.GroupBox()
        Me.chkService = New System.Windows.Forms.CheckBox()
        Me.chkSystem = New System.Windows.Forms.CheckBox()
        Me.chkComm = New System.Windows.Forms.CheckBox()
        Me.chkDialog = New System.Windows.Forms.CheckBox()
        Me.GroupBoxRowNo = New System.Windows.Forms.GroupBox()
        Me.txtPageSize = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBoxExclude = New System.Windows.Forms.GroupBox()
        Me.chkLockedUsers = New System.Windows.Forms.CheckBox()
        Me.btnAnalyze = New System.Windows.Forms.Button()
        Me.GroupBoxUserValidity = New System.Windows.Forms.GroupBox()
        Me.chkInvalidUsers = New System.Windows.Forms.CheckBox()
        Me.chkValidUsers = New System.Windows.Forms.CheckBox()
        Me.dtTargetDate = New System.Windows.Forms.DateTimePicker()
        Me.cmbSystems = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.GroupBoxCriteria = New System.Windows.Forms.GroupBox()
        Me.btnChangeUserGroup = New System.Windows.Forms.Button()
        Me.btnChangeValidity = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnRFCSettings = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnLockUser = New System.Windows.Forms.Button()
        Me.TabAboutUs = New System.Windows.Forms.TabPage()
        Me.lblAboutSoftware = New System.Windows.Forms.Label()
        Me.LabelProductName = New System.Windows.Forms.Label()
        Me.ListViewDiagnostics = New System.Windows.Forms.ListView()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.ButtonCopyDiagnostics = New System.Windows.Forms.Button()
        Me.LabelCopyright = New System.Windows.Forms.Label()
        Me.ButtonExportDiagnostics = New System.Windows.Forms.Button()
        Me.LabelCompanyName = New System.Windows.Forms.Label()
        Me.LabelVersion = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PanelFooter = New System.Windows.Forms.Panel()
        Me.lblNotification = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TimerSidebar = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTips = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.txtcreationFilterDays = New System.Windows.Forms.TextBox()
        Me.grpAction = New System.Windows.Forms.GroupBox()
        Me.PanelHeader.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbUserProfile, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlMain.SuspendLayout()
        Me.TabHome.SuspendLayout()
        CType(Me.chartStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chartFolderStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.imgFolderPath, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvControlStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabAppendix.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.PanelButtons.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TabLogs.SuspendLayout()
        CType(Me.dgvLogs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.TabInstruction.SuspendLayout()
        CType(Me.WebView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabHosekeeper.SuspendLayout()
        Me.PanelMain.SuspendLayout()
        Me.PanelBody.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBoxUserType.SuspendLayout()
        Me.GroupBoxRowNo.SuspendLayout()
        Me.GroupBoxExclude.SuspendLayout()
        Me.GroupBoxUserValidity.SuspendLayout()
        Me.GroupBoxCriteria.SuspendLayout()
        Me.TabAboutUs.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelFooter.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.grpAction.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelHeader
        '
        Me.PanelHeader.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.PanelHeader.Controls.Add(Me.PictureBox4)
        Me.PanelHeader.Controls.Add(Me.pbUserProfile)
        Me.PanelHeader.Controls.Add(Me.lblHeader)
        Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
        Me.PanelHeader.Name = "PanelHeader"
        Me.PanelHeader.Size = New System.Drawing.Size(1631, 78)
        Me.PanelHeader.TabIndex = 0
        '
        'PictureBox4
        '
        Me.PictureBox4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(1296, -10)
        Me.PictureBox4.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(253, 97)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox4.TabIndex = 54
        Me.PictureBox4.TabStop = False
        '
        'pbUserProfile
        '
        Me.pbUserProfile.BackColor = System.Drawing.Color.Transparent
        Me.pbUserProfile.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbUserProfile.Dock = System.Windows.Forms.DockStyle.Right
        Me.pbUserProfile.Image = CType(resources.GetObject("pbUserProfile.Image"), System.Drawing.Image)
        Me.pbUserProfile.Location = New System.Drawing.Point(1539, 0)
        Me.pbUserProfile.Margin = New System.Windows.Forms.Padding(4)
        Me.pbUserProfile.Name = "pbUserProfile"
        Me.pbUserProfile.Size = New System.Drawing.Size(92, 78)
        Me.pbUserProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbUserProfile.TabIndex = 53
        Me.pbUserProfile.TabStop = False
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.BackColor = System.Drawing.Color.Transparent
        Me.lblHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 28.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.ForeColor = System.Drawing.Color.White
        Me.lblHeader.Location = New System.Drawing.Point(3, 4)
        Me.lblHeader.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(198, 64)
        Me.lblHeader.TabIndex = 1
        Me.lblHeader.Text = "AuditX"
        Me.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TabControlMain
        '
        Me.TabControlMain.Controls.Add(Me.TabHome)
        Me.TabControlMain.Controls.Add(Me.TabAppendix)
        Me.TabControlMain.Controls.Add(Me.TabSettings)
        Me.TabControlMain.Controls.Add(Me.TabLogs)
        Me.TabControlMain.Controls.Add(Me.TabInstruction)
        Me.TabControlMain.Controls.Add(Me.TabHosekeeper)
        Me.TabControlMain.Controls.Add(Me.TabAboutUs)
        Me.TabControlMain.Cursor = System.Windows.Forms.Cursors.Hand
        Me.TabControlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControlMain.ItemSize = New System.Drawing.Size(150, 45)
        Me.TabControlMain.Location = New System.Drawing.Point(0, 78)
        Me.TabControlMain.Name = "TabControlMain"
        Me.TabControlMain.SelectedIndex = 0
        Me.TabControlMain.Size = New System.Drawing.Size(1631, 1078)
        Me.TabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControlMain.TabIndex = 1
        '
        'TabHome
        '
        Me.TabHome.BackColor = System.Drawing.Color.White
        Me.TabHome.Controls.Add(Me.Label5)
        Me.TabHome.Controls.Add(Me.lblRefresh)
        Me.TabHome.Controls.Add(Me.chartStatus)
        Me.TabHome.Controls.Add(Me.chartFolderStatus)
        Me.TabHome.Controls.Add(Me.GroupBox2)
        Me.TabHome.Controls.Add(Me.dgvControlStatus)
        Me.TabHome.Controls.Add(Me.GroupBox3)
        Me.TabHome.Controls.Add(Me.GroupBox1)
        Me.TabHome.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabHome.Location = New System.Drawing.Point(4, 49)
        Me.TabHome.Name = "TabHome"
        Me.TabHome.Padding = New System.Windows.Forms.Padding(3)
        Me.TabHome.Size = New System.Drawing.Size(1623, 1025)
        Me.TabHome.TabIndex = 0
        Me.TabHome.Text = "Home"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(11, 21)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(671, 46)
        Me.Label5.TabIndex = 75
        Me.Label5.Text = "ITGC Audit Automation Framework"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblRefresh
        '
        Me.lblRefresh.AutoSize = True
        Me.lblRefresh.Font = New System.Drawing.Font("Calisto MT", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRefresh.LinkColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblRefresh.Location = New System.Drawing.Point(1519, 66)
        Me.lblRefresh.Name = "lblRefresh"
        Me.lblRefresh.Size = New System.Drawing.Size(100, 23)
        Me.lblRefresh.TabIndex = 72
        Me.lblRefresh.TabStop = True
        Me.lblRefresh.Text = "🔄Refresh "
        '
        'chartStatus
        '
        Me.chartStatus.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight
        Me.chartStatus.BorderlineColor = System.Drawing.Color.Transparent
        ChartArea7.BackColor = System.Drawing.Color.Transparent
        ChartArea7.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight
        ChartArea7.Name = "ChartArea1"
        Me.chartStatus.ChartAreas.Add(ChartArea7)
        Legend7.Name = "Legend1"
        Me.chartStatus.Legends.Add(Legend7)
        Me.chartStatus.Location = New System.Drawing.Point(977, 617)
        Me.chartStatus.Name = "chartStatus"
        Series7.ChartArea = "ChartArea1"
        Series7.Legend = "Legend1"
        Series7.Name = "Series1"
        Me.chartStatus.Series.Add(Series7)
        Me.chartStatus.Size = New System.Drawing.Size(642, 262)
        Me.chartStatus.TabIndex = 74
        Me.chartStatus.Text = "Selected Month Control"
        '
        'chartFolderStatus
        '
        ChartArea8.Name = "ChartArea1"
        Me.chartFolderStatus.ChartAreas.Add(ChartArea8)
        Legend8.Name = "Legend1"
        Me.chartFolderStatus.Legends.Add(Legend8)
        Me.chartFolderStatus.Location = New System.Drawing.Point(977, 354)
        Me.chartFolderStatus.Name = "chartFolderStatus"
        Series8.ChartArea = "ChartArea1"
        Series8.Legend = "Legend1"
        Series8.Name = "Series1"
        Me.chartFolderStatus.Series.Add(Series8)
        Me.chartFolderStatus.Size = New System.Drawing.Size(642, 253)
        Me.chartFolderStatus.TabIndex = 73
        Me.chartFolderStatus.Text = "Chart1"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.dtpStart)
        Me.GroupBox2.Controls.Add(Me.lblEndTime)
        Me.GroupBox2.Controls.Add(Me.lblPath)
        Me.GroupBox2.Controls.Add(Me.txtFolderPath)
        Me.GroupBox2.Controls.Add(Me.imgFolderPath)
        Me.GroupBox2.Controls.Add(Me.lblStartTime)
        Me.GroupBox2.Controls.Add(Me.lblReportTo)
        Me.GroupBox2.Controls.Add(Me.dtpEnd)
        Me.GroupBox2.Controls.Add(Me.txtReportMonth)
        Me.GroupBox2.Controls.Add(Me.btnGenerateWordReport)
        Me.GroupBox2.Controls.Add(Me.lblReportFrom)
        Me.GroupBox2.Controls.Add(Me.lblReportMonth)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(12, 607)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(936, 261)
        Me.GroupBox2.TabIndex = 68
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Report Duration"
        '
        'dtpStart
        '
        Me.dtpStart.CalendarFont = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStart.Location = New System.Drawing.Point(56, 80)
        Me.dtpStart.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(180, 30)
        Me.dtpStart.TabIndex = 5
        Me.dtpStart.Value = New Date(2025, 5, 1, 0, 0, 0, 0)
        '
        'lblEndTime
        '
        Me.lblEndTime.AutoSize = True
        Me.lblEndTime.BackColor = System.Drawing.Color.Transparent
        Me.lblEndTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndTime.Location = New System.Drawing.Point(553, 117)
        Me.lblEndTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEndTime.Name = "lblEndTime"
        Me.lblEndTime.Size = New System.Drawing.Size(73, 25)
        Me.lblEndTime.TabIndex = 36
        Me.lblEndTime.Text = "ETIME"
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.BackColor = System.Drawing.Color.Transparent
        Me.lblPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPath.Location = New System.Drawing.Point(53, 163)
        Me.lblPath.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(114, 25)
        Me.lblPath.TabIndex = 3
        Me.lblPath.Text = "Report Path"
        '
        'txtFolderPath
        '
        Me.txtFolderPath.BackColor = System.Drawing.Color.White
        Me.txtFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFolderPath.Enabled = False
        Me.txtFolderPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFolderPath.ForeColor = System.Drawing.Color.Black
        Me.txtFolderPath.HideSelection = False
        Me.txtFolderPath.Location = New System.Drawing.Point(56, 192)
        Me.txtFolderPath.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtFolderPath.Multiline = True
        Me.txtFolderPath.Name = "txtFolderPath"
        Me.txtFolderPath.Size = New System.Drawing.Size(676, 38)
        Me.txtFolderPath.TabIndex = 8
        '
        'imgFolderPath
        '
        Me.imgFolderPath.BackColor = System.Drawing.Color.Transparent
        Me.imgFolderPath.Cursor = System.Windows.Forms.Cursors.Hand
        Me.imgFolderPath.Image = CType(resources.GetObject("imgFolderPath.Image"), System.Drawing.Image)
        Me.imgFolderPath.Location = New System.Drawing.Point(756, 192)
        Me.imgFolderPath.Margin = New System.Windows.Forms.Padding(4)
        Me.imgFolderPath.Name = "imgFolderPath"
        Me.imgFolderPath.Size = New System.Drawing.Size(40, 38)
        Me.imgFolderPath.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgFolderPath.TabIndex = 27
        Me.imgFolderPath.TabStop = False
        '
        'lblStartTime
        '
        Me.lblStartTime.AutoSize = True
        Me.lblStartTime.BackColor = System.Drawing.Color.Transparent
        Me.lblStartTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartTime.Location = New System.Drawing.Point(51, 117)
        Me.lblStartTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStartTime.Name = "lblStartTime"
        Me.lblStartTime.Size = New System.Drawing.Size(74, 25)
        Me.lblStartTime.TabIndex = 34
        Me.lblStartTime.Text = "STIME"
        '
        'lblReportTo
        '
        Me.lblReportTo.AutoSize = True
        Me.lblReportTo.BackColor = System.Drawing.Color.Transparent
        Me.lblReportTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportTo.Location = New System.Drawing.Point(553, 46)
        Me.lblReportTo.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblReportTo.Name = "lblReportTo"
        Me.lblReportTo.Size = New System.Drawing.Size(36, 25)
        Me.lblReportTo.TabIndex = 13
        Me.lblReportTo.Text = "To"
        Me.lblReportTo.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'dtpEnd
        '
        Me.dtpEnd.CalendarFont = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEnd.Cursor = System.Windows.Forms.Cursors.Hand
        Me.dtpEnd.CustomFormat = ""
        Me.dtpEnd.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEnd.Location = New System.Drawing.Point(558, 80)
        Me.dtpEnd.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(174, 30)
        Me.dtpEnd.TabIndex = 6
        Me.dtpEnd.Value = New Date(2025, 6, 30, 0, 0, 0, 0)
        '
        'txtReportMonth
        '
        Me.txtReportMonth.BackColor = System.Drawing.Color.White
        Me.txtReportMonth.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtReportMonth.Enabled = False
        Me.txtReportMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReportMonth.ForeColor = System.Drawing.Color.Black
        Me.txtReportMonth.HideSelection = False
        Me.txtReportMonth.Location = New System.Drawing.Point(242, 80)
        Me.txtReportMonth.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtReportMonth.Multiline = True
        Me.txtReportMonth.Name = "txtReportMonth"
        Me.txtReportMonth.ReadOnly = True
        Me.txtReportMonth.Size = New System.Drawing.Size(310, 37)
        Me.txtReportMonth.TabIndex = 9
        Me.txtReportMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnGenerateWordReport
        '
        Me.btnGenerateWordReport.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnGenerateWordReport.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnGenerateWordReport.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGenerateWordReport.ForeColor = System.Drawing.Color.Black
        Me.btnGenerateWordReport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenerateWordReport.Location = New System.Drawing.Point(756, 60)
        Me.btnGenerateWordReport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnGenerateWordReport.Name = "btnGenerateWordReport"
        Me.btnGenerateWordReport.Size = New System.Drawing.Size(164, 63)
        Me.btnGenerateWordReport.TabIndex = 7
        Me.btnGenerateWordReport.Text = "Generate Report"
        Me.btnGenerateWordReport.UseVisualStyleBackColor = False
        '
        'lblReportFrom
        '
        Me.lblReportFrom.AutoSize = True
        Me.lblReportFrom.BackColor = System.Drawing.Color.Transparent
        Me.lblReportFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportFrom.Location = New System.Drawing.Point(51, 46)
        Me.lblReportFrom.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblReportFrom.Name = "lblReportFrom"
        Me.lblReportFrom.Size = New System.Drawing.Size(57, 25)
        Me.lblReportFrom.TabIndex = 43
        Me.lblReportFrom.Text = "From"
        Me.lblReportFrom.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReportMonth
        '
        Me.lblReportMonth.BackColor = System.Drawing.Color.Transparent
        Me.lblReportMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportMonth.Location = New System.Drawing.Point(246, 43)
        Me.lblReportMonth.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblReportMonth.Name = "lblReportMonth"
        Me.lblReportMonth.Size = New System.Drawing.Size(294, 33)
        Me.lblReportMonth.TabIndex = 4
        Me.lblReportMonth.Text = "Reporting Month"
        Me.lblReportMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvControlStatus
        '
        Me.dgvControlStatus.AllowUserToAddRows = False
        Me.dgvControlStatus.AllowUserToDeleteRows = False
        Me.dgvControlStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvControlStatus.BackgroundColor = System.Drawing.Color.White
        Me.dgvControlStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvControlStatus.Location = New System.Drawing.Point(977, 92)
        Me.dgvControlStatus.Name = "dgvControlStatus"
        Me.dgvControlStatus.RowHeadersWidth = 62
        Me.dgvControlStatus.RowTemplate.Height = 28
        Me.dgvControlStatus.Size = New System.Drawing.Size(642, 250)
        Me.dgvControlStatus.TabIndex = 71
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox3.Controls.Add(Me.lblSystem)
        Me.GroupBox3.Controls.Add(Me.lblSystemId)
        Me.GroupBox3.Controls.Add(Me.txtUsername)
        Me.GroupBox3.Controls.Add(Me.txtPassword)
        Me.GroupBox3.Controls.Add(Me.lblUsername)
        Me.GroupBox3.Controls.Add(Me.cmbSystem)
        Me.GroupBox3.Controls.Add(Me.btnExecute)
        Me.GroupBox3.Controls.Add(Me.lblPassword)
        Me.GroupBox3.Controls.Add(Me.txtClient)
        Me.GroupBox3.Controls.Add(Me.CheckBoxClient)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(12, 348)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(937, 253)
        Me.GroupBox3.TabIndex = 70
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "SAP configuration"
        '
        'lblSystem
        '
        Me.lblSystem.AutoSize = True
        Me.lblSystem.BackColor = System.Drawing.Color.Transparent
        Me.lblSystem.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystem.Location = New System.Drawing.Point(58, 52)
        Me.lblSystem.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSystem.Name = "lblSystem"
        Me.lblSystem.Size = New System.Drawing.Size(184, 25)
        Me.lblSystem.TabIndex = 1
        Me.lblSystem.Text = "Select SAP System"
        '
        'lblSystemId
        '
        Me.lblSystemId.BackColor = System.Drawing.Color.Transparent
        Me.lblSystemId.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystemId.Location = New System.Drawing.Point(618, 52)
        Me.lblSystemId.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSystemId.Name = "lblSystemId"
        Me.lblSystemId.Size = New System.Drawing.Size(110, 23)
        Me.lblSystemId.TabIndex = 22
        Me.lblSystemId.Text = "System ID"
        Me.lblSystemId.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblSystemId.UseCompatibleTextRendering = True
        '
        'txtUsername
        '
        Me.txtUsername.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList
        Me.txtUsername.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtUsername.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsername.Location = New System.Drawing.Point(56, 194)
        Me.txtUsername.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtUsername.Multiline = True
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(325, 40)
        Me.txtUsername.TabIndex = 3
        Me.txtUsername.Text = "ITGCBOT"
        '
        'txtPassword
        '
        Me.txtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(402, 194)
        Me.txtPassword.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.txtPassword.Multiline = True
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(330, 40)
        Me.txtPassword.TabIndex = 4
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.BackColor = System.Drawing.Color.Transparent
        Me.lblUsername.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsername.Location = New System.Drawing.Point(56, 166)
        Me.lblUsername.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(128, 25)
        Me.lblUsername.TabIndex = 17
        Me.lblUsername.Text = "Enter User ID"
        '
        'cmbSystem
        '
        Me.cmbSystem.DropDownHeight = 200
        Me.cmbSystem.DropDownWidth = 300
        Me.cmbSystem.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSystem.FormattingEnabled = True
        Me.cmbSystem.IntegralHeight = False
        Me.cmbSystem.Location = New System.Drawing.Point(58, 80)
        Me.cmbSystem.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.cmbSystem.Name = "cmbSystem"
        Me.cmbSystem.Size = New System.Drawing.Size(674, 33)
        Me.cmbSystem.TabIndex = 2
        '
        'btnExecute
        '
        Me.btnExecute.AutoSize = True
        Me.btnExecute.BackColor = System.Drawing.Color.Honeydew
        Me.btnExecute.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExecute.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExecute.ForeColor = System.Drawing.Color.Black
        Me.btnExecute.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExecute.Location = New System.Drawing.Point(756, 180)
        Me.btnExecute.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(164, 58)
        Me.btnExecute.TabIndex = 8
        Me.btnExecute.Text = "Execute"
        Me.btnExecute.UseVisualStyleBackColor = False
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.BackColor = System.Drawing.Color.Transparent
        Me.lblPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPassword.Location = New System.Drawing.Point(398, 166)
        Me.lblPassword.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(149, 25)
        Me.lblPassword.TabIndex = 18
        Me.lblPassword.Text = "Enter Password"
        '
        'txtClient
        '
        Me.txtClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClient.Location = New System.Drawing.Point(736, 83)
        Me.txtClient.Margin = New System.Windows.Forms.Padding(2)
        Me.txtClient.Name = "txtClient"
        Me.txtClient.Size = New System.Drawing.Size(80, 30)
        Me.txtClient.TabIndex = 39
        '
        'CheckBoxClient
        '
        Me.CheckBoxClient.AutoSize = True
        Me.CheckBoxClient.BackColor = System.Drawing.Color.Transparent
        Me.CheckBoxClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxClient.Location = New System.Drawing.Point(637, 122)
        Me.CheckBoxClient.Margin = New System.Windows.Forms.Padding(4)
        Me.CheckBoxClient.Name = "CheckBoxClient"
        Me.CheckBoxClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBoxClient.Size = New System.Drawing.Size(88, 29)
        Me.CheckBoxClient.TabIndex = 38
        Me.CheckBoxClient.Text = "Client"
        Me.CheckBoxClient.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cmbExecutionType)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.cmbExecutionMode)
        Me.GroupBox1.Controls.Add(Me.lblControl)
        Me.GroupBox1.Controls.Add(Me.cmbControl)
        Me.GroupBox1.Controls.Add(Me.lblDescription)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 92)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(937, 250)
        Me.GroupBox1.TabIndex = 69
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Execution Setting"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(579, 43)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(153, 25)
        Me.Label1.TabIndex = 39
        Me.Label1.Text = "Execution Mode"
        '
        'cmbExecutionType
        '
        Me.cmbExecutionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbExecutionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbExecutionType.FormattingEnabled = True
        Me.cmbExecutionType.Items.AddRange(New Object() {"Individual Control", "All Controls"})
        Me.cmbExecutionType.Location = New System.Drawing.Point(62, 71)
        Me.cmbExecutionType.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbExecutionType.Name = "cmbExecutionType"
        Me.cmbExecutionType.Size = New System.Drawing.Size(266, 33)
        Me.cmbExecutionType.TabIndex = 41
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(59, 43)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(148, 25)
        Me.Label3.TabIndex = 42
        Me.Label3.Text = "Execution Type"
        '
        'cmbExecutionMode
        '
        Me.cmbExecutionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbExecutionMode.DropDownWidth = 76
        Me.cmbExecutionMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbExecutionMode.FormattingEnabled = True
        Me.cmbExecutionMode.Items.AddRange(New Object() {"Foreground", "Background"})
        Me.cmbExecutionMode.Location = New System.Drawing.Point(486, 71)
        Me.cmbExecutionMode.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.cmbExecutionMode.Name = "cmbExecutionMode"
        Me.cmbExecutionMode.Size = New System.Drawing.Size(246, 33)
        Me.cmbExecutionMode.TabIndex = 37
        Me.cmbExecutionMode.Tag = "executionMode"
        '
        'lblControl
        '
        Me.lblControl.AutoSize = True
        Me.lblControl.BackColor = System.Drawing.Color.Transparent
        Me.lblControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblControl.Location = New System.Drawing.Point(58, 132)
        Me.lblControl.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblControl.Name = "lblControl"
        Me.lblControl.Size = New System.Drawing.Size(188, 25)
        Me.lblControl.TabIndex = 2
        Me.lblControl.Text = "Select ITGC Control"
        '
        'cmbControl
        '
        Me.cmbControl.DropDownWidth = 900
        Me.cmbControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbControl.FormattingEnabled = True
        Me.cmbControl.Items.AddRange(New Object() {"ITGC01 - SAPOSS Enabling Audit", "ITGC02 - Client Opening Audit", "ITGC03", "ITGC04", "ITGC05", "ITGC06", "ITGC07", "ITGC08", "ITGC09", "ITGC10", "ITGC11", "ITGC12", "ITGC13", "ITGC14", "ITGC15", "ITGC16", "ITGC17", "ITGC18"})
        Me.cmbControl.Location = New System.Drawing.Point(58, 160)
        Me.cmbControl.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.cmbControl.Name = "cmbControl"
        Me.cmbControl.Size = New System.Drawing.Size(674, 33)
        Me.cmbControl.TabIndex = 1
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.Color.Transparent
        Me.lblDescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.Location = New System.Drawing.Point(53, 197)
        Me.lblDescription.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(144, 20)
        Me.lblDescription.TabIndex = 21
        Me.lblDescription.Text = "Control Description"
        '
        'TabAppendix
        '
        Me.TabAppendix.Controls.Add(Me.DataGridView1)
        Me.TabAppendix.Controls.Add(Me.Panel1)
        Me.TabAppendix.Controls.Add(Me.Panel2)
        Me.TabAppendix.Location = New System.Drawing.Point(4, 49)
        Me.TabAppendix.Name = "TabAppendix"
        Me.TabAppendix.Size = New System.Drawing.Size(1623, 1025)
        Me.TabAppendix.TabIndex = 2
        Me.TabAppendix.Text = "Appendix"
        Me.TabAppendix.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 76)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(1623, 914)
        Me.DataGridView1.TabIndex = 9
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.PanelButtons)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.ForeColor = System.Drawing.Color.Black
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1623, 76)
        Me.Panel1.TabIndex = 8
        '
        'PanelButtons
        '
        Me.PanelButtons.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelButtons.Controls.Add(Me.btnEdit)
        Me.PanelButtons.Controls.Add(Me.btnSaveXML)
        Me.PanelButtons.Controls.Add(Me.btnRevert)
        Me.PanelButtons.Controls.Add(Me.btnExportCSV)
        Me.PanelButtons.Dock = System.Windows.Forms.DockStyle.Right
        Me.PanelButtons.ForeColor = System.Drawing.Color.Black
        Me.PanelButtons.Location = New System.Drawing.Point(792, 0)
        Me.PanelButtons.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelButtons.Name = "PanelButtons"
        Me.PanelButtons.Size = New System.Drawing.Size(831, 76)
        Me.PanelButtons.TabIndex = 1
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnEdit.BackColor = System.Drawing.Color.White
        Me.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnEdit.Font = New System.Drawing.Font("Calibri", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ForeColor = System.Drawing.Color.Black
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnEdit.Location = New System.Drawing.Point(532, 28)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(144, 52)
        Me.btnEdit.TabIndex = 2
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnSaveXML
        '
        Me.btnSaveXML.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnSaveXML.BackColor = System.Drawing.Color.White
        Me.btnSaveXML.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSaveXML.Font = New System.Drawing.Font("Calibri", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveXML.ForeColor = System.Drawing.Color.Black
        Me.btnSaveXML.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnSaveXML.Location = New System.Drawing.Point(382, 28)
        Me.btnSaveXML.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSaveXML.Name = "btnSaveXML"
        Me.btnSaveXML.Size = New System.Drawing.Size(144, 52)
        Me.btnSaveXML.TabIndex = 1
        Me.btnSaveXML.Text = "Save"
        Me.btnSaveXML.UseVisualStyleBackColor = False
        '
        'btnRevert
        '
        Me.btnRevert.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnRevert.BackColor = System.Drawing.Color.White
        Me.btnRevert.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRevert.Font = New System.Drawing.Font("Calibri", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRevert.ForeColor = System.Drawing.Color.Black
        Me.btnRevert.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnRevert.Location = New System.Drawing.Point(232, 28)
        Me.btnRevert.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRevert.Name = "btnRevert"
        Me.btnRevert.Size = New System.Drawing.Size(144, 52)
        Me.btnRevert.TabIndex = 4
        Me.btnRevert.Text = "Revert"
        Me.btnRevert.UseVisualStyleBackColor = False
        '
        'btnExportCSV
        '
        Me.btnExportCSV.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnExportCSV.BackColor = System.Drawing.Color.White
        Me.btnExportCSV.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExportCSV.Font = New System.Drawing.Font("Calibri", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportCSV.ForeColor = System.Drawing.Color.Black
        Me.btnExportCSV.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnExportCSV.Location = New System.Drawing.Point(682, 28)
        Me.btnExportCSV.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportCSV.Name = "btnExportCSV"
        Me.btnExportCSV.Size = New System.Drawing.Size(144, 52)
        Me.btnExportCSV.TabIndex = 6
        Me.btnExportCSV.Text = "Export"
        Me.btnExportCSV.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 22.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(0, 11)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(579, 54)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Appendix"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblStatus)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 990)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1623, 35)
        Me.Panel2.TabIndex = 7
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(3, 4)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(73, 25)
        Me.lblStatus.TabIndex = 7
        Me.lblStatus.Text = "Status"
        '
        'TabSettings
        '
        Me.TabSettings.Location = New System.Drawing.Point(4, 49)
        Me.TabSettings.Name = "TabSettings"
        Me.TabSettings.Size = New System.Drawing.Size(1623, 1025)
        Me.TabSettings.TabIndex = 3
        Me.TabSettings.Text = "Settings"
        Me.TabSettings.UseVisualStyleBackColor = True
        '
        'TabLogs
        '
        Me.TabLogs.Controls.Add(Me.dgvLogs)
        Me.TabLogs.Controls.Add(Me.Panel3)
        Me.TabLogs.Location = New System.Drawing.Point(4, 49)
        Me.TabLogs.Name = "TabLogs"
        Me.TabLogs.Size = New System.Drawing.Size(1623, 1025)
        Me.TabLogs.TabIndex = 4
        Me.TabLogs.Text = "Logs"
        Me.TabLogs.UseVisualStyleBackColor = True
        '
        'dgvLogs
        '
        Me.dgvLogs.BackgroundColor = System.Drawing.Color.White
        Me.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLogs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvLogs.Location = New System.Drawing.Point(0, 94)
        Me.dgvLogs.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dgvLogs.Name = "dgvLogs"
        Me.dgvLogs.RowHeadersWidth = 51
        Me.dgvLogs.RowTemplate.Height = 24
        Me.dgvLogs.Size = New System.Drawing.Size(1623, 931)
        Me.dgvLogs.TabIndex = 3
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel3.Controls.Add(Me.lblFilter)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.txtFilter)
        Me.Panel3.Controls.Add(Me.lblHeading)
        Me.Panel3.Controls.Add(Me.cmbLogFiles)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1623, 94)
        Me.Panel3.TabIndex = 2
        '
        'lblFilter
        '
        Me.lblFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilter.ForeColor = System.Drawing.Color.Black
        Me.lblFilter.Location = New System.Drawing.Point(795, 9)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(371, 35)
        Me.lblFilter.TabIndex = 4
        Me.lblFilter.Text = "🔍 Filter logs by keyword..."
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(322, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(467, 35)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Log File..."
        '
        'txtFilter
        '
        Me.txtFilter.BackColor = System.Drawing.Color.AliceBlue
        Me.txtFilter.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFilter.Location = New System.Drawing.Point(800, 48)
        Me.txtFilter.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(366, 35)
        Me.txtFilter.TabIndex = 1
        '
        'lblHeading
        '
        Me.lblHeading.BackColor = System.Drawing.Color.WhiteSmoke
        Me.lblHeading.Font = New System.Drawing.Font("Microsoft Sans Serif", 22.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeading.ForeColor = System.Drawing.Color.Black
        Me.lblHeading.Location = New System.Drawing.Point(3, 0)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(312, 94)
        Me.lblHeading.TabIndex = 2
        Me.lblHeading.Text = "Log viewer"
        Me.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbLogFiles
        '
        Me.cmbLogFiles.BackColor = System.Drawing.Color.AliceBlue
        Me.cmbLogFiles.DropDownWidth = 650
        Me.cmbLogFiles.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLogFiles.FormattingEnabled = True
        Me.cmbLogFiles.Location = New System.Drawing.Point(322, 48)
        Me.cmbLogFiles.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cmbLogFiles.Name = "cmbLogFiles"
        Me.cmbLogFiles.Size = New System.Drawing.Size(466, 36)
        Me.cmbLogFiles.TabIndex = 0
        '
        'TabInstruction
        '
        Me.TabInstruction.Controls.Add(Me.WebView2)
        Me.TabInstruction.Location = New System.Drawing.Point(4, 49)
        Me.TabInstruction.Name = "TabInstruction"
        Me.TabInstruction.Padding = New System.Windows.Forms.Padding(3)
        Me.TabInstruction.Size = New System.Drawing.Size(1623, 1025)
        Me.TabInstruction.TabIndex = 1
        Me.TabInstruction.Text = "Instruction"
        Me.TabInstruction.UseVisualStyleBackColor = True
        '
        'WebView2
        '
        Me.WebView2.AllowExternalDrop = True
        Me.WebView2.CreationProperties = Nothing
        Me.WebView2.DefaultBackgroundColor = System.Drawing.Color.White
        Me.WebView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebView2.Location = New System.Drawing.Point(3, 3)
        Me.WebView2.Name = "WebView2"
        Me.WebView2.Size = New System.Drawing.Size(1617, 1019)
        Me.WebView2.TabIndex = 0
        Me.WebView2.ZoomFactor = 1.0R
        '
        'TabHosekeeper
        '
        Me.TabHosekeeper.Controls.Add(Me.PanelMain)
        Me.TabHosekeeper.Location = New System.Drawing.Point(4, 49)
        Me.TabHosekeeper.Name = "TabHosekeeper"
        Me.TabHosekeeper.Size = New System.Drawing.Size(1623, 1025)
        Me.TabHosekeeper.TabIndex = 6
        Me.TabHosekeeper.Text = "Housekeeping"
        Me.TabHosekeeper.UseVisualStyleBackColor = True
        '
        'PanelMain
        '
        Me.PanelMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelMain.AutoScroll = True
        Me.PanelMain.Controls.Add(Me.PanelBody)
        Me.PanelMain.Controls.Add(Me.Panel5)
        Me.PanelMain.Location = New System.Drawing.Point(0, 0)
        Me.PanelMain.Name = "PanelMain"
        Me.PanelMain.Size = New System.Drawing.Size(1627, 981)
        Me.PanelMain.TabIndex = 25
        '
        'PanelBody
        '
        Me.PanelBody.AutoScroll = True
        Me.PanelBody.Controls.Add(Me.Panel4)
        Me.PanelBody.Controls.Add(Me.dgvResult)
        Me.PanelBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelBody.Location = New System.Drawing.Point(0, 282)
        Me.PanelBody.Name = "PanelBody"
        Me.PanelBody.Size = New System.Drawing.Size(1627, 699)
        Me.PanelBody.TabIndex = 23
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.White
        Me.Panel4.Controls.Add(Me.btnNextPage)
        Me.Panel4.Controls.Add(Me.lblPageInfo)
        Me.Panel4.Controls.Add(Me.btnPreviousPage)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(0, 661)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1627, 38)
        Me.Panel4.TabIndex = 26
        '
        'btnNextPage
        '
        Me.btnNextPage.AutoSize = True
        Me.btnNextPage.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnNextPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnNextPage.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnNextPage.Location = New System.Drawing.Point(1535, 0)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(92, 38)
        Me.btnNextPage.TabIndex = 17
        Me.btnNextPage.Text = "▶"
        Me.btnNextPage.UseVisualStyleBackColor = False
        '
        'lblPageInfo
        '
        Me.lblPageInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPageInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPageInfo.Location = New System.Drawing.Point(96, 0)
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.Size = New System.Drawing.Size(1433, 38)
        Me.lblPageInfo.TabIndex = 18
        Me.lblPageInfo.Text = "PageInfo"
        Me.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnPreviousPage
        '
        Me.btnPreviousPage.AutoSize = True
        Me.btnPreviousPage.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnPreviousPage.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnPreviousPage.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnPreviousPage.Location = New System.Drawing.Point(0, 0)
        Me.btnPreviousPage.Name = "btnPreviousPage"
        Me.btnPreviousPage.Size = New System.Drawing.Size(90, 38)
        Me.btnPreviousPage.TabIndex = 16
        Me.btnPreviousPage.Text = "◀"
        Me.btnPreviousPage.UseVisualStyleBackColor = False
        '
        'dgvResult
        '
        Me.dgvResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvResult.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvResult.BackgroundColor = System.Drawing.Color.White
        Me.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.Padding = New System.Windows.Forms.Padding(3)
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvResult.DefaultCellStyle = DataGridViewCellStyle7
        Me.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvResult.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvResult.GridColor = System.Drawing.Color.WhiteSmoke
        Me.dgvResult.Location = New System.Drawing.Point(0, 0)
        Me.dgvResult.Name = "dgvResult"
        Me.dgvResult.RowHeadersWidth = 51
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvResult.RowsDefaultCellStyle = DataGridViewCellStyle8
        Me.dgvResult.RowTemplate.Height = 30
        Me.dgvResult.Size = New System.Drawing.Size(1627, 699)
        Me.dgvResult.TabIndex = 2
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel5.Controls.Add(Me.grpAction)
        Me.Panel5.Controls.Add(Me.Label8)
        Me.Panel5.Controls.Add(Me.GroupBox4)
        Me.Panel5.Controls.Add(Me.GroupBoxCriteria)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1627, 282)
        Me.Panel5.TabIndex = 22
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(3, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(362, 52)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Inactivity analysis"
        '
        'GroupBox4
        '
        Me.GroupBox4.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GroupBox4.Controls.Add(Me.GroupBox6)
        Me.GroupBox4.Controls.Add(Me.GroupBoxUserType)
        Me.GroupBox4.Controls.Add(Me.GroupBoxRowNo)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.GroupBoxExclude)
        Me.GroupBox4.Controls.Add(Me.GroupBoxUserValidity)
        Me.GroupBox4.Controls.Add(Me.dtTargetDate)
        Me.GroupBox4.Controls.Add(Me.cmbSystems)
        Me.GroupBox4.Controls.Add(Me.Label7)
        Me.GroupBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.ForeColor = System.Drawing.Color.Black
        Me.GroupBox4.Location = New System.Drawing.Point(8, 55)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(767, 224)
        Me.GroupBox4.TabIndex = 13
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Selection criteria"
        '
        'GroupBoxUserType
        '
        Me.GroupBoxUserType.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GroupBoxUserType.Controls.Add(Me.chkService)
        Me.GroupBoxUserType.Controls.Add(Me.chkSystem)
        Me.GroupBoxUserType.Controls.Add(Me.chkComm)
        Me.GroupBoxUserType.Controls.Add(Me.chkDialog)
        Me.GroupBoxUserType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxUserType.ForeColor = System.Drawing.Color.Black
        Me.GroupBoxUserType.Location = New System.Drawing.Point(20, 88)
        Me.GroupBoxUserType.Name = "GroupBoxUserType"
        Me.GroupBoxUserType.Size = New System.Drawing.Size(301, 121)
        Me.GroupBoxUserType.TabIndex = 12
        Me.GroupBoxUserType.TabStop = False
        Me.GroupBoxUserType.Text = "User type"
        '
        'chkService
        '
        Me.chkService.AutoSize = True
        Me.chkService.Checked = True
        Me.chkService.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkService.Location = New System.Drawing.Point(166, 33)
        Me.chkService.Name = "chkService"
        Me.chkService.Size = New System.Drawing.Size(96, 26)
        Me.chkService.TabIndex = 3
        Me.chkService.Text = "Service"
        Me.chkService.UseVisualStyleBackColor = True
        '
        'chkSystem
        '
        Me.chkSystem.AutoSize = True
        Me.chkSystem.Checked = True
        Me.chkSystem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSystem.Location = New System.Drawing.Point(166, 81)
        Me.chkSystem.Name = "chkSystem"
        Me.chkSystem.Size = New System.Drawing.Size(95, 26)
        Me.chkSystem.TabIndex = 11
        Me.chkSystem.Text = "System"
        Me.chkSystem.UseVisualStyleBackColor = True
        '
        'chkComm
        '
        Me.chkComm.AutoSize = True
        Me.chkComm.Checked = True
        Me.chkComm.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkComm.Location = New System.Drawing.Point(6, 81)
        Me.chkComm.Name = "chkComm"
        Me.chkComm.Size = New System.Drawing.Size(132, 26)
        Me.chkComm.TabIndex = 4
        Me.chkComm.Text = "Background"
        Me.chkComm.UseVisualStyleBackColor = True
        '
        'chkDialog
        '
        Me.chkDialog.AutoSize = True
        Me.chkDialog.Checked = True
        Me.chkDialog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDialog.Location = New System.Drawing.Point(6, 33)
        Me.chkDialog.Name = "chkDialog"
        Me.chkDialog.Size = New System.Drawing.Size(87, 26)
        Me.chkDialog.TabIndex = 2
        Me.chkDialog.Text = "Dialog"
        Me.chkDialog.UseVisualStyleBackColor = True
        '
        'GroupBoxRowNo
        '
        Me.GroupBoxRowNo.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GroupBoxRowNo.Controls.Add(Me.txtPageSize)
        Me.GroupBoxRowNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxRowNo.ForeColor = System.Drawing.Color.Black
        Me.GroupBoxRowNo.Location = New System.Drawing.Point(530, 90)
        Me.GroupBoxRowNo.Name = "GroupBoxRowNo"
        Me.GroupBoxRowNo.Size = New System.Drawing.Size(113, 119)
        Me.GroupBoxRowNo.TabIndex = 22
        Me.GroupBoxRowNo.TabStop = False
        Me.GroupBoxRowNo.Text = "Rows to Display"
        '
        'txtPageSize
        '
        Me.txtPageSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPageSize.Location = New System.Drawing.Point(6, 79)
        Me.txtPageSize.Name = "txtPageSize"
        Me.txtPageSize.Size = New System.Drawing.Size(94, 28)
        Me.txtPageSize.TabIndex = 15
        Me.txtPageSize.Text = "20"
        Me.txtPageSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(16, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(110, 22)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "SAP System"
        '
        'GroupBoxExclude
        '
        Me.GroupBoxExclude.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GroupBoxExclude.Controls.Add(Me.chkLockedUsers)
        Me.GroupBoxExclude.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxExclude.ForeColor = System.Drawing.Color.Black
        Me.GroupBoxExclude.Location = New System.Drawing.Point(530, 24)
        Me.GroupBoxExclude.Name = "GroupBoxExclude"
        Me.GroupBoxExclude.Size = New System.Drawing.Size(231, 59)
        Me.GroupBoxExclude.TabIndex = 13
        Me.GroupBoxExclude.TabStop = False
        Me.GroupBoxExclude.Text = "Exclude"
        '
        'chkLockedUsers
        '
        Me.chkLockedUsers.AutoSize = True
        Me.chkLockedUsers.Location = New System.Drawing.Point(31, 24)
        Me.chkLockedUsers.Name = "chkLockedUsers"
        Me.chkLockedUsers.Size = New System.Drawing.Size(94, 26)
        Me.chkLockedUsers.TabIndex = 1
        Me.chkLockedUsers.Text = "Locked"
        Me.chkLockedUsers.UseVisualStyleBackColor = True
        '
        'btnAnalyze
        '
        Me.btnAnalyze.BackColor = System.Drawing.Color.White
        Me.btnAnalyze.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAnalyze.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAnalyze.ForeColor = System.Drawing.Color.Black
        Me.btnAnalyze.Location = New System.Drawing.Point(8, 31)
        Me.btnAnalyze.Name = "btnAnalyze"
        Me.btnAnalyze.Size = New System.Drawing.Size(149, 56)
        Me.btnAnalyze.TabIndex = 6
        Me.btnAnalyze.Text = "Execute"
        Me.btnAnalyze.UseVisualStyleBackColor = False
        '
        'GroupBoxUserValidity
        '
        Me.GroupBoxUserValidity.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GroupBoxUserValidity.Controls.Add(Me.chkInvalidUsers)
        Me.GroupBoxUserValidity.Controls.Add(Me.chkValidUsers)
        Me.GroupBoxUserValidity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxUserValidity.ForeColor = System.Drawing.Color.Black
        Me.GroupBoxUserValidity.Location = New System.Drawing.Point(352, 88)
        Me.GroupBoxUserValidity.Name = "GroupBoxUserValidity"
        Me.GroupBoxUserValidity.Size = New System.Drawing.Size(172, 121)
        Me.GroupBoxUserValidity.TabIndex = 21
        Me.GroupBoxUserValidity.TabStop = False
        Me.GroupBoxUserValidity.Text = "Validity of users"
        '
        'chkInvalidUsers
        '
        Me.chkInvalidUsers.AutoSize = True
        Me.chkInvalidUsers.Location = New System.Drawing.Point(6, 63)
        Me.chkInvalidUsers.Name = "chkInvalidUsers"
        Me.chkInvalidUsers.Size = New System.Drawing.Size(136, 26)
        Me.chkInvalidUsers.TabIndex = 20
        Me.chkInvalidUsers.Text = "Invalid users"
        Me.chkInvalidUsers.UseVisualStyleBackColor = True
        '
        'chkValidUsers
        '
        Me.chkValidUsers.AutoSize = True
        Me.chkValidUsers.Checked = True
        Me.chkValidUsers.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkValidUsers.Location = New System.Drawing.Point(6, 31)
        Me.chkValidUsers.Name = "chkValidUsers"
        Me.chkValidUsers.Size = New System.Drawing.Size(125, 26)
        Me.chkValidUsers.TabIndex = 19
        Me.chkValidUsers.Text = "Valid users"
        Me.chkValidUsers.UseVisualStyleBackColor = True
        '
        'dtTargetDate
        '
        Me.dtTargetDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtTargetDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtTargetDate.Location = New System.Drawing.Point(356, 49)
        Me.dtTargetDate.Name = "dtTargetDate"
        Me.dtTargetDate.Size = New System.Drawing.Size(168, 30)
        Me.dtTargetDate.TabIndex = 10
        '
        'cmbSystems
        '
        Me.cmbSystems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSystems.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSystems.FormattingEnabled = True
        Me.cmbSystems.Location = New System.Drawing.Point(20, 49)
        Me.cmbSystems.Name = "cmbSystems"
        Me.cmbSystems.Size = New System.Drawing.Size(301, 33)
        Me.cmbSystems.TabIndex = 17
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(354, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(106, 22)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Target Date"
        '
        'GroupBoxCriteria
        '
        Me.GroupBoxCriteria.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GroupBoxCriteria.Controls.Add(Me.btnClear)
        Me.GroupBoxCriteria.Controls.Add(Me.btnAnalyze)
        Me.GroupBoxCriteria.Controls.Add(Me.btnRFCSettings)
        Me.GroupBoxCriteria.Controls.Add(Me.btnExport)
        Me.GroupBoxCriteria.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxCriteria.ForeColor = System.Drawing.Color.Black
        Me.GroupBoxCriteria.Location = New System.Drawing.Point(775, 64)
        Me.GroupBoxCriteria.Name = "GroupBoxCriteria"
        Me.GroupBoxCriteria.Size = New System.Drawing.Size(322, 215)
        Me.GroupBoxCriteria.TabIndex = 15
        Me.GroupBoxCriteria.TabStop = False
        Me.GroupBoxCriteria.Text = "Menu"
        '
        'btnChangeUserGroup
        '
        Me.btnChangeUserGroup.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnChangeUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangeUserGroup.ForeColor = System.Drawing.Color.Black
        Me.btnChangeUserGroup.Location = New System.Drawing.Point(6, 165)
        Me.btnChangeUserGroup.Name = "btnChangeUserGroup"
        Me.btnChangeUserGroup.Size = New System.Drawing.Size(188, 57)
        Me.btnChangeUserGroup.TabIndex = 23
        Me.btnChangeUserGroup.Text = "Change User Group"
        Me.btnChangeUserGroup.UseVisualStyleBackColor = True
        '
        'btnChangeValidity
        '
        Me.btnChangeValidity.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnChangeValidity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangeValidity.ForeColor = System.Drawing.Color.Black
        Me.btnChangeValidity.Location = New System.Drawing.Point(6, 97)
        Me.btnChangeValidity.Name = "btnChangeValidity"
        Me.btnChangeValidity.Size = New System.Drawing.Size(188, 57)
        Me.btnChangeValidity.TabIndex = 22
        Me.btnChangeValidity.Text = "Change Validity"
        Me.btnChangeValidity.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.ForeColor = System.Drawing.Color.Black
        Me.btnClear.Location = New System.Drawing.Point(163, 30)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(149, 57)
        Me.btnClear.TabIndex = 21
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnRFCSettings
        '
        Me.btnRFCSettings.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRFCSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRFCSettings.ForeColor = System.Drawing.Color.Black
        Me.btnRFCSettings.Location = New System.Drawing.Point(8, 118)
        Me.btnRFCSettings.Name = "btnRFCSettings"
        Me.btnRFCSettings.Size = New System.Drawing.Size(149, 57)
        Me.btnRFCSettings.TabIndex = 19
        Me.btnRFCSettings.Text = "RFC Setting"
        Me.btnRFCSettings.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.ForeColor = System.Drawing.Color.Black
        Me.btnExport.Location = New System.Drawing.Point(163, 118)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(149, 57)
        Me.btnExport.TabIndex = 1
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnLockUser
        '
        Me.btnLockUser.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLockUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLockUser.ForeColor = System.Drawing.Color.Black
        Me.btnLockUser.Location = New System.Drawing.Point(6, 27)
        Me.btnLockUser.Name = "btnLockUser"
        Me.btnLockUser.Size = New System.Drawing.Size(188, 57)
        Me.btnLockUser.TabIndex = 20
        Me.btnLockUser.Text = "🔒Lock User"
        Me.btnLockUser.UseVisualStyleBackColor = True
        '
        'TabAboutUs
        '
        Me.TabAboutUs.BackColor = System.Drawing.Color.Black
        Me.TabAboutUs.Controls.Add(Me.lblAboutSoftware)
        Me.TabAboutUs.Controls.Add(Me.LabelProductName)
        Me.TabAboutUs.Controls.Add(Me.ListViewDiagnostics)
        Me.TabAboutUs.Controls.Add(Me.GroupBox5)
        Me.TabAboutUs.Controls.Add(Me.PictureBox1)
        Me.TabAboutUs.Location = New System.Drawing.Point(4, 49)
        Me.TabAboutUs.Name = "TabAboutUs"
        Me.TabAboutUs.Size = New System.Drawing.Size(1623, 1025)
        Me.TabAboutUs.TabIndex = 5
        Me.TabAboutUs.Text = "About"
        '
        'lblAboutSoftware
        '
        Me.lblAboutSoftware.BackColor = System.Drawing.Color.Black
        Me.lblAboutSoftware.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAboutSoftware.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAboutSoftware.ForeColor = System.Drawing.Color.White
        Me.lblAboutSoftware.Location = New System.Drawing.Point(8, 78)
        Me.lblAboutSoftware.Name = "lblAboutSoftware"
        Me.lblAboutSoftware.Size = New System.Drawing.Size(955, 161)
        Me.lblAboutSoftware.TabIndex = 7
        Me.lblAboutSoftware.Text = "lblAboutSoftware"
        '
        'LabelProductName
        '
        Me.LabelProductName.AutoSize = True
        Me.LabelProductName.BackColor = System.Drawing.Color.Black
        Me.LabelProductName.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelProductName.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelProductName.ForeColor = System.Drawing.Color.White
        Me.LabelProductName.Location = New System.Drawing.Point(3, 27)
        Me.LabelProductName.Name = "LabelProductName"
        Me.LabelProductName.Size = New System.Drawing.Size(308, 37)
        Me.LabelProductName.TabIndex = 6
        Me.LabelProductName.Text = "LabelProductName"
        '
        'ListViewDiagnostics
        '
        Me.ListViewDiagnostics.BackColor = System.Drawing.Color.Black
        Me.ListViewDiagnostics.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListViewDiagnostics.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListViewDiagnostics.ForeColor = System.Drawing.Color.White
        Me.ListViewDiagnostics.HideSelection = False
        Me.ListViewDiagnostics.Location = New System.Drawing.Point(8, 242)
        Me.ListViewDiagnostics.Name = "ListViewDiagnostics"
        Me.ListViewDiagnostics.Size = New System.Drawing.Size(955, 532)
        Me.ListViewDiagnostics.TabIndex = 8
        Me.ListViewDiagnostics.UseCompatibleStateImageBehavior = False
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.ButtonCopyDiagnostics)
        Me.GroupBox5.Controls.Add(Me.LabelCopyright)
        Me.GroupBox5.Controls.Add(Me.ButtonExportDiagnostics)
        Me.GroupBox5.Controls.Add(Me.LabelCompanyName)
        Me.GroupBox5.Controls.Add(Me.LabelVersion)
        Me.GroupBox5.Location = New System.Drawing.Point(969, 66)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(624, 129)
        Me.GroupBox5.TabIndex = 7
        Me.GroupBox5.TabStop = False
        '
        'ButtonCopyDiagnostics
        '
        Me.ButtonCopyDiagnostics.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.ButtonCopyDiagnostics.Location = New System.Drawing.Point(435, 21)
        Me.ButtonCopyDiagnostics.Name = "ButtonCopyDiagnostics"
        Me.ButtonCopyDiagnostics.Size = New System.Drawing.Size(89, 40)
        Me.ButtonCopyDiagnostics.TabIndex = 6
        Me.ButtonCopyDiagnostics.Text = "Copy"
        Me.ButtonCopyDiagnostics.UseVisualStyleBackColor = True
        '
        'LabelCopyright
        '
        Me.LabelCopyright.AutoSize = True
        Me.LabelCopyright.BackColor = System.Drawing.Color.Black
        Me.LabelCopyright.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCopyright.ForeColor = System.Drawing.Color.White
        Me.LabelCopyright.Location = New System.Drawing.Point(6, 87)
        Me.LabelCopyright.Name = "LabelCopyright"
        Me.LabelCopyright.Size = New System.Drawing.Size(131, 22)
        Me.LabelCopyright.TabIndex = 3
        Me.LabelCopyright.Text = "LabelCopyright"
        '
        'ButtonExportDiagnostics
        '
        Me.ButtonExportDiagnostics.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportDiagnostics.Location = New System.Drawing.Point(530, 21)
        Me.ButtonExportDiagnostics.Name = "ButtonExportDiagnostics"
        Me.ButtonExportDiagnostics.Size = New System.Drawing.Size(89, 40)
        Me.ButtonExportDiagnostics.TabIndex = 5
        Me.ButtonExportDiagnostics.Text = "Export"
        Me.ButtonExportDiagnostics.UseVisualStyleBackColor = True
        '
        'LabelCompanyName
        '
        Me.LabelCompanyName.AutoSize = True
        Me.LabelCompanyName.BackColor = System.Drawing.Color.Black
        Me.LabelCompanyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCompanyName.ForeColor = System.Drawing.Color.White
        Me.LabelCompanyName.Location = New System.Drawing.Point(6, 54)
        Me.LabelCompanyName.Name = "LabelCompanyName"
        Me.LabelCompanyName.Size = New System.Drawing.Size(177, 22)
        Me.LabelCompanyName.TabIndex = 2
        Me.LabelCompanyName.Text = "LabelCompanyName"
        '
        'LabelVersion
        '
        Me.LabelVersion.AutoSize = True
        Me.LabelVersion.BackColor = System.Drawing.Color.Black
        Me.LabelVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelVersion.ForeColor = System.Drawing.Color.White
        Me.LabelVersion.Location = New System.Drawing.Point(6, 21)
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New System.Drawing.Size(115, 22)
        Me.LabelVersion.TabIndex = 1
        Me.LabelVersion.Text = "LabelVersion"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(969, 181)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(624, 633)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'PanelFooter
        '
        Me.PanelFooter.Controls.Add(Me.lblNotification)
        Me.PanelFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelFooter.Location = New System.Drawing.Point(0, 1119)
        Me.PanelFooter.Name = "PanelFooter"
        Me.PanelFooter.Size = New System.Drawing.Size(1631, 37)
        Me.PanelFooter.TabIndex = 2
        '
        'lblNotification
        '
        Me.lblNotification.BackColor = System.Drawing.Color.Transparent
        Me.lblNotification.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblNotification.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNotification.ForeColor = System.Drawing.Color.Black
        Me.lblNotification.Location = New System.Drawing.Point(0, 0)
        Me.lblNotification.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNotification.Name = "lblNotification"
        Me.lblNotification.Size = New System.Drawing.Size(1631, 37)
        Me.lblNotification.TabIndex = 38
        Me.lblNotification.Text = "Notification"
        '
        'TimerSidebar
        '
        Me.TimerSidebar.Interval = 15
        '
        'GroupBox6
        '
        Me.GroupBox6.BackColor = System.Drawing.Color.WhiteSmoke
        Me.GroupBox6.Controls.Add(Me.txtcreationFilterDays)
        Me.GroupBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.ForeColor = System.Drawing.Color.Black
        Me.GroupBox6.Location = New System.Drawing.Point(649, 90)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(112, 119)
        Me.GroupBox6.TabIndex = 23
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Creation Filter Days"
        '
        'txtcreationFilterDays
        '
        Me.txtcreationFilterDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtcreationFilterDays.Location = New System.Drawing.Point(6, 78)
        Me.txtcreationFilterDays.Name = "txtcreationFilterDays"
        Me.txtcreationFilterDays.Size = New System.Drawing.Size(94, 28)
        Me.txtcreationFilterDays.TabIndex = 15
        Me.txtcreationFilterDays.Text = "15"
        Me.txtcreationFilterDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'grpAction
        '
        Me.grpAction.Controls.Add(Me.btnChangeUserGroup)
        Me.grpAction.Controls.Add(Me.btnLockUser)
        Me.grpAction.Controls.Add(Me.btnChangeValidity)
        Me.grpAction.Location = New System.Drawing.Point(1097, 40)
        Me.grpAction.Name = "grpAction"
        Me.grpAction.Size = New System.Drawing.Size(200, 239)
        Me.grpAction.TabIndex = 22
        Me.grpAction.TabStop = False
        Me.grpAction.Text = "Action"
        '
        'Home
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1631, 1156)
        Me.Controls.Add(Me.PanelFooter)
        Me.Controls.Add(Me.TabControlMain)
        Me.Controls.Add(Me.PanelHeader)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Home"
        Me.Text = "Dashboard"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.PanelHeader.ResumeLayout(False)
        Me.PanelHeader.PerformLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbUserProfile, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlMain.ResumeLayout(False)
        Me.TabHome.ResumeLayout(False)
        Me.TabHome.PerformLayout()
        CType(Me.chartStatus, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chartFolderStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.imgFolderPath, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvControlStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabAppendix.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.PanelButtons.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.TabLogs.ResumeLayout(False)
        CType(Me.dgvLogs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.TabInstruction.ResumeLayout(False)
        CType(Me.WebView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabHosekeeper.ResumeLayout(False)
        Me.PanelMain.ResumeLayout(False)
        Me.PanelBody.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBoxUserType.ResumeLayout(False)
        Me.GroupBoxUserType.PerformLayout()
        Me.GroupBoxRowNo.ResumeLayout(False)
        Me.GroupBoxRowNo.PerformLayout()
        Me.GroupBoxExclude.ResumeLayout(False)
        Me.GroupBoxExclude.PerformLayout()
        Me.GroupBoxUserValidity.ResumeLayout(False)
        Me.GroupBoxUserValidity.PerformLayout()
        Me.GroupBoxCriteria.ResumeLayout(False)
        Me.TabAboutUs.ResumeLayout(False)
        Me.TabAboutUs.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelFooter.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.grpAction.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelHeader As Panel
    Friend WithEvents PictureBox4 As PictureBox
    Friend WithEvents pbUserProfile As PictureBox
    Friend WithEvents lblHeader As Label
    Friend WithEvents TabControlMain As TabControl
    Friend WithEvents TabHome As TabPage
    Friend WithEvents TabInstruction As TabPage
    Friend WithEvents TabAppendix As TabPage
    Friend WithEvents TabSettings As TabPage
    Friend WithEvents TabLogs As TabPage
    Friend WithEvents TabAboutUs As TabPage
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents dtpStart As DateTimePicker
    Friend WithEvents lblEndTime As Label
    Friend WithEvents lblPath As Label
    Friend WithEvents txtFolderPath As TextBox
    Friend WithEvents imgFolderPath As PictureBox
    Friend WithEvents lblStartTime As Label
    Friend WithEvents lblReportTo As Label
    Friend WithEvents dtpEnd As DateTimePicker
    Friend WithEvents txtReportMonth As TextBox
    Friend WithEvents btnGenerateWordReport As Button
    Friend WithEvents lblReportFrom As Label
    Friend WithEvents lblReportMonth As Label
    Friend WithEvents dgvControlStatus As DataGridView
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents lblSystem As Label
    Friend WithEvents lblSystemId As Label
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents lblUsername As Label
    Friend WithEvents cmbSystem As ComboBox
    Friend WithEvents btnExecute As Button
    Friend WithEvents lblPassword As Label
    Friend WithEvents txtClient As TextBox
    Friend WithEvents CheckBoxClient As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbExecutionType As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbExecutionMode As ComboBox
    Friend WithEvents lblControl As Label
    Friend WithEvents cmbControl As ComboBox
    Friend WithEvents lblDescription As Label
    Friend WithEvents WebView2 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents PanelFooter As Panel
    Friend WithEvents lblNotification As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblStatus As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PanelButtons As Panel
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnSaveXML As Button
    Friend WithEvents btnRevert As Button
    Friend WithEvents btnExportCSV As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents dgvLogs As DataGridView
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblFilter As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtFilter As TextBox
    Friend WithEvents lblHeading As Label
    Friend WithEvents cmbLogFiles As ComboBox
    Friend WithEvents TimerSidebar As Timer
    Friend WithEvents ToolTips As ToolTip
    Friend WithEvents chartStatus As DataVisualization.Charting.Chart
    Friend WithEvents chartFolderStatus As DataVisualization.Charting.Chart
    Friend WithEvents Label5 As Label
    Friend WithEvents lblRefresh As LinkLabel
    Friend WithEvents TabHosekeeper As TabPage
    Friend WithEvents Panel4 As Panel
    Friend WithEvents btnNextPage As Button
    Friend WithEvents lblPageInfo As Label
    Friend WithEvents btnPreviousPage As Button
    Friend WithEvents PanelMain As Panel
    Friend WithEvents PanelBody As Panel
    Friend WithEvents dgvResult As DataGridView
    Friend WithEvents Panel5 As Panel
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents GroupBoxUserType As GroupBox
    Friend WithEvents chkService As CheckBox
    Friend WithEvents chkSystem As CheckBox
    Friend WithEvents chkComm As CheckBox
    Friend WithEvents chkDialog As CheckBox
    Friend WithEvents GroupBoxExclude As GroupBox
    Friend WithEvents chkLockedUsers As CheckBox
    Friend WithEvents Label6 As Label
    Friend WithEvents GroupBoxUserValidity As GroupBox
    Friend WithEvents chkInvalidUsers As CheckBox
    Friend WithEvents chkValidUsers As CheckBox
    Friend WithEvents dtTargetDate As DateTimePicker
    Friend WithEvents cmbSystems As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents GroupBoxCriteria As GroupBox
    Friend WithEvents btnClear As Button
    Friend WithEvents GroupBoxRowNo As GroupBox
    Friend WithEvents txtPageSize As TextBox
    Friend WithEvents btnLockUser As Button
    Friend WithEvents btnRFCSettings As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents btnAnalyze As Button
    Friend WithEvents LabelProductName As Label
    Friend WithEvents ListViewDiagnostics As ListView
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents ButtonCopyDiagnostics As Button
    Friend WithEvents LabelCopyright As Label
    Friend WithEvents ButtonExportDiagnostics As Button
    Friend WithEvents LabelCompanyName As Label
    Friend WithEvents LabelVersion As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblAboutSoftware As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents btnChangeUserGroup As Button
    Friend WithEvents btnChangeValidity As Button
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents txtcreationFilterDays As TextBox
    Friend WithEvents grpAction As GroupBox
End Class
