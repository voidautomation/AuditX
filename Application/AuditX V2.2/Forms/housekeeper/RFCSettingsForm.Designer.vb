<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RFCSettingsForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RFCSettingsForm))
        Me.PanelHeader = New System.Windows.Forms.Panel()
        Me.PanelHeaderShine = New System.Windows.Forms.Panel()
        Me.LabelSubtitle = New System.Windows.Forms.Label()
        Me.LabelHeader = New System.Windows.Forms.Label()
        Me.PanelMain = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblReferesh = New System.Windows.Forms.LinkLabel()
        Me.lblStatusSummary = New System.Windows.Forms.Label()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnTestConnection = New System.Windows.Forms.Button()
        Me.btnCancelEdit = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.dgvSystemStatus = New System.Windows.Forms.DataGridView()
        Me.PanelServerSection = New System.Windows.Forms.Panel()
        Me.LabelServerInfo = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.LabelServerDesc = New System.Windows.Forms.Label()
        Me.cmbSystems = New System.Windows.Forms.ComboBox()
        Me.txtSystemName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LabelSystemNameDesc = New System.Windows.Forms.Label()
        Me.PanelConnectionSection = New System.Windows.Forms.Panel()
        Me.LabelConnectionInfo = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.LabelConnectionDesc = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LabelLanguageDesc = New System.Windows.Forms.Label()
        Me.txtAppServerHost = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.LabelPasswordDesc = New System.Windows.Forms.Label()
        Me.txtClient = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LabelUserDesc = New System.Windows.Forms.Label()
        Me.txtSystemNumber = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LabelInstanceDesc = New System.Windows.Forms.Label()
        Me.txtUser = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LabelClientDesc = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LabelHostDesc = New System.Windows.Forms.Label()
        Me.txtLanguage = New System.Windows.Forms.TextBox()
        Me.PanelHeader.SuspendLayout()
        Me.PanelMain.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvSystemStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelServerSection.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelConnectionSection.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelHeader
        '
        Me.PanelHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(185, Byte), Integer))
        Me.PanelHeader.Controls.Add(Me.PanelHeaderShine)
        Me.PanelHeader.Controls.Add(Me.LabelSubtitle)
        Me.PanelHeader.Controls.Add(Me.LabelHeader)
        Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
        Me.PanelHeader.Name = "PanelHeader"
        Me.PanelHeader.Size = New System.Drawing.Size(1560, 100)
        Me.PanelHeader.TabIndex = 0
        '
        'PanelHeaderShine
        '
        Me.PanelHeaderShine.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PanelHeaderShine.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelHeaderShine.Location = New System.Drawing.Point(0, 0)
        Me.PanelHeaderShine.Name = "PanelHeaderShine"
        Me.PanelHeaderShine.Size = New System.Drawing.Size(1560, 2)
        Me.PanelHeaderShine.TabIndex = 1
        '
        'LabelSubtitle
        '
        Me.LabelSubtitle.AutoSize = True
        Me.LabelSubtitle.BackColor = System.Drawing.Color.Transparent
        Me.LabelSubtitle.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.LabelSubtitle.Location = New System.Drawing.Point(25, 63)
        Me.LabelSubtitle.Name = "LabelSubtitle"
        Me.LabelSubtitle.Size = New System.Drawing.Size(465, 28)
        Me.LabelSubtitle.TabIndex = 2
        Me.LabelSubtitle.Text = "Configure and manage SAP RFC connection settings"
        '
        'LabelHeader
        '
        Me.LabelHeader.AutoSize = True
        Me.LabelHeader.BackColor = System.Drawing.Color.Transparent
        Me.LabelHeader.Font = New System.Drawing.Font("Segoe UI", 20.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelHeader.ForeColor = System.Drawing.Color.White
        Me.LabelHeader.Location = New System.Drawing.Point(21, 9)
        Me.LabelHeader.Name = "LabelHeader"
        Me.LabelHeader.Size = New System.Drawing.Size(260, 54)
        Me.LabelHeader.TabIndex = 0
        Me.LabelHeader.Text = "RFC Settings"
        Me.LabelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PanelMain
        '
        Me.PanelMain.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.PanelMain.Controls.Add(Me.GroupBox2)
        Me.PanelMain.Controls.Add(Me.GroupBox1)
        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelMain.Location = New System.Drawing.Point(0, 100)
        Me.PanelMain.Name = "PanelMain"
        Me.PanelMain.Size = New System.Drawing.Size(1560, 920)
        Me.PanelMain.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.White
        Me.GroupBox2.Controls.Add(Me.lblReferesh)
        Me.GroupBox2.Controls.Add(Me.lblStatusSummary)
        Me.GroupBox2.Controls.Add(Me.btnDelete)
        Me.GroupBox2.Controls.Add(Me.btnAdd)
        Me.GroupBox2.Controls.Add(Me.btnTestConnection)
        Me.GroupBox2.Controls.Add(Me.btnCancelEdit)
        Me.GroupBox2.Controls.Add(Me.btnEdit)
        Me.GroupBox2.Controls.Add(Me.btnSave)
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.GroupBox2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.GroupBox2.Location = New System.Drawing.Point(3, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(20)
        Me.GroupBox2.Size = New System.Drawing.Size(1548, 111)
        Me.GroupBox2.TabIndex = 23
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Actions"
        '
        'lblReferesh
        '
        Me.lblReferesh.AutoSize = True
        Me.lblReferesh.BackColor = System.Drawing.Color.Transparent
        Me.lblReferesh.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReferesh.Location = New System.Drawing.Point(1458, 13)
        Me.lblReferesh.Name = "lblReferesh"
        Me.lblReferesh.Size = New System.Drawing.Size(87, 25)
        Me.lblReferesh.TabIndex = 28
        Me.lblReferesh.TabStop = True
        Me.lblReferesh.Text = "Referesh"
        '
        'lblStatusSummary
        '
        Me.lblStatusSummary.AutoEllipsis = True
        Me.lblStatusSummary.AutoSize = True
        Me.lblStatusSummary.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblStatusSummary.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatusSummary.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.lblStatusSummary.Location = New System.Drawing.Point(1346, 47)
        Me.lblStatusSummary.Name = "lblStatusSummary"
        Me.lblStatusSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatusSummary.Size = New System.Drawing.Size(182, 28)
        Me.lblStatusSummary.TabIndex = 24
        Me.lblStatusSummary.Text = "No systems loaded."
        Me.lblStatusSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnDelete
        '
        Me.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDelete.Location = New System.Drawing.Point(578, 34)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(135, 54)
        Me.btnDelete.TabIndex = 26
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAdd.Location = New System.Drawing.Point(12, 34)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(135, 54)
        Me.btnAdd.TabIndex = 25
        Me.btnAdd.Text = "New"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnTestConnection
        '
        Me.btnTestConnection.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnTestConnection.Location = New System.Drawing.Point(294, 34)
        Me.btnTestConnection.Name = "btnTestConnection"
        Me.btnTestConnection.Size = New System.Drawing.Size(135, 54)
        Me.btnTestConnection.TabIndex = 14
        Me.btnTestConnection.Text = "Test"
        Me.btnTestConnection.UseVisualStyleBackColor = True
        '
        'btnCancelEdit
        '
        Me.btnCancelEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnCancelEdit.Location = New System.Drawing.Point(718, 34)
        Me.btnCancelEdit.Name = "btnCancelEdit"
        Me.btnCancelEdit.Size = New System.Drawing.Size(135, 54)
        Me.btnCancelEdit.TabIndex = 23
        Me.btnCancelEdit.Text = "Cancel"
        Me.btnCancelEdit.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnEdit.Location = New System.Drawing.Point(153, 34)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(135, 54)
        Me.btnEdit.TabIndex = 22
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSave.Location = New System.Drawing.Point(436, 34)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(135, 54)
        Me.btnSave.TabIndex = 13
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.White
        Me.GroupBox1.Controls.Add(Me.dgvSystemStatus)
        Me.GroupBox1.Controls.Add(Me.PanelServerSection)
        Me.GroupBox1.Controls.Add(Me.PanelConnectionSection)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.GroupBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.GroupBox1.Location = New System.Drawing.Point(3, 117)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(20)
        Me.GroupBox1.Size = New System.Drawing.Size(1545, 795)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "RFC Configuration"
        '
        'dgvSystemStatus
        '
        Me.dgvSystemStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvSystemStatus.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvSystemStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSystemStatus.Location = New System.Drawing.Point(848, 25)
        Me.dgvSystemStatus.Name = "dgvSystemStatus"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvSystemStatus.RowHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvSystemStatus.RowHeadersWidth = 62
        Me.dgvSystemStatus.RowTemplate.Height = 28
        Me.dgvSystemStatus.Size = New System.Drawing.Size(697, 766)
        Me.dgvSystemStatus.TabIndex = 2
        '
        'PanelServerSection
        '
        Me.PanelServerSection.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.PanelServerSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelServerSection.Controls.Add(Me.LabelServerInfo)
        Me.PanelServerSection.Controls.Add(Me.PictureBox1)
        Me.PanelServerSection.Controls.Add(Me.Label8)
        Me.PanelServerSection.Controls.Add(Me.LabelServerDesc)
        Me.PanelServerSection.Controls.Add(Me.cmbSystems)
        Me.PanelServerSection.Controls.Add(Me.txtSystemName)
        Me.PanelServerSection.Controls.Add(Me.Label1)
        Me.PanelServerSection.Controls.Add(Me.LabelSystemNameDesc)
        Me.PanelServerSection.Location = New System.Drawing.Point(9, 25)
        Me.PanelServerSection.Name = "PanelServerSection"
        Me.PanelServerSection.Size = New System.Drawing.Size(833, 207)
        Me.PanelServerSection.TabIndex = 0
        '
        'LabelServerInfo
        '
        Me.LabelServerInfo.AutoSize = True
        Me.LabelServerInfo.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.LabelServerInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(185, Byte), Integer))
        Me.LabelServerInfo.Location = New System.Drawing.Point(40, 9)
        Me.LabelServerInfo.Name = "LabelServerInfo"
        Me.LabelServerInfo.Size = New System.Drawing.Size(209, 30)
        Me.LabelServerInfo.TabIndex = 23
        Me.LabelServerInfo.Text = "Server Information"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Location = New System.Drawing.Point(10, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(24, 25)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 22
        Me.PictureBox1.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(41, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(106, 28)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "RFC Server"
        '
        'LabelServerDesc
        '
        Me.LabelServerDesc.AutoSize = True
        Me.LabelServerDesc.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.LabelServerDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.LabelServerDesc.Location = New System.Drawing.Point(307, 95)
        Me.LabelServerDesc.Name = "LabelServerDesc"
        Me.LabelServerDesc.Size = New System.Drawing.Size(337, 23)
        Me.LabelServerDesc.TabIndex = 20
        Me.LabelServerDesc.Text = "Select the RFC server from the available list"
        '
        'cmbSystems
        '
        Me.cmbSystems.BackColor = System.Drawing.Color.White
        Me.cmbSystems.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSystems.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.cmbSystems.FormattingEnabled = True
        Me.cmbSystems.Location = New System.Drawing.Point(311, 56)
        Me.cmbSystems.Name = "cmbSystems"
        Me.cmbSystems.Size = New System.Drawing.Size(494, 36)
        Me.cmbSystems.TabIndex = 10
        '
        'txtSystemName
        '
        Me.txtSystemName.BackColor = System.Drawing.Color.White
        Me.txtSystemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSystemName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSystemName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.txtSystemName.Location = New System.Drawing.Point(311, 121)
        Me.txtSystemName.Name = "txtSystemName"
        Me.txtSystemName.Size = New System.Drawing.Size(496, 33)
        Me.txtSystemName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(41, 121)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(131, 28)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "System Name"
        '
        'LabelSystemNameDesc
        '
        Me.LabelSystemNameDesc.AutoSize = True
        Me.LabelSystemNameDesc.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.LabelSystemNameDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.LabelSystemNameDesc.Location = New System.Drawing.Point(307, 157)
        Me.LabelSystemNameDesc.Name = "LabelSystemNameDesc"
        Me.LabelSystemNameDesc.Size = New System.Drawing.Size(402, 23)
        Me.LabelSystemNameDesc.TabIndex = 15
        Me.LabelSystemNameDesc.Text = "Unique name for this RFC connection configuration"
        '
        'PanelConnectionSection
        '
        Me.PanelConnectionSection.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.PanelConnectionSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelConnectionSection.Controls.Add(Me.LabelConnectionInfo)
        Me.PanelConnectionSection.Controls.Add(Me.PictureBox2)
        Me.PanelConnectionSection.Controls.Add(Me.LabelConnectionDesc)
        Me.PanelConnectionSection.Controls.Add(Me.Label7)
        Me.PanelConnectionSection.Controls.Add(Me.LabelLanguageDesc)
        Me.PanelConnectionSection.Controls.Add(Me.txtAppServerHost)
        Me.PanelConnectionSection.Controls.Add(Me.Label6)
        Me.PanelConnectionSection.Controls.Add(Me.LabelPasswordDesc)
        Me.PanelConnectionSection.Controls.Add(Me.txtClient)
        Me.PanelConnectionSection.Controls.Add(Me.Label5)
        Me.PanelConnectionSection.Controls.Add(Me.LabelUserDesc)
        Me.PanelConnectionSection.Controls.Add(Me.txtSystemNumber)
        Me.PanelConnectionSection.Controls.Add(Me.Label4)
        Me.PanelConnectionSection.Controls.Add(Me.LabelInstanceDesc)
        Me.PanelConnectionSection.Controls.Add(Me.txtUser)
        Me.PanelConnectionSection.Controls.Add(Me.Label3)
        Me.PanelConnectionSection.Controls.Add(Me.LabelClientDesc)
        Me.PanelConnectionSection.Controls.Add(Me.txtPassword)
        Me.PanelConnectionSection.Controls.Add(Me.Label2)
        Me.PanelConnectionSection.Controls.Add(Me.LabelHostDesc)
        Me.PanelConnectionSection.Controls.Add(Me.txtLanguage)
        Me.PanelConnectionSection.Location = New System.Drawing.Point(9, 238)
        Me.PanelConnectionSection.Name = "PanelConnectionSection"
        Me.PanelConnectionSection.Size = New System.Drawing.Size(833, 553)
        Me.PanelConnectionSection.TabIndex = 1
        '
        'LabelConnectionInfo
        '
        Me.LabelConnectionInfo.AutoSize = True
        Me.LabelConnectionInfo.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.LabelConnectionInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(185, Byte), Integer))
        Me.LabelConnectionInfo.Location = New System.Drawing.Point(40, 9)
        Me.LabelConnectionInfo.Name = "LabelConnectionInfo"
        Me.LabelConnectionInfo.Size = New System.Drawing.Size(296, 30)
        Me.LabelConnectionInfo.TabIndex = 23
        Me.LabelConnectionInfo.Text = "Connection & Authentication"
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox2.Location = New System.Drawing.Point(10, 8)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(24, 25)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 22
        Me.PictureBox2.TabStop = False
        '
        'LabelConnectionDesc
        '
        Me.LabelConnectionDesc.AutoSize = True
        Me.LabelConnectionDesc.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.LabelConnectionDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.LabelConnectionDesc.Location = New System.Drawing.Point(42, 39)
        Me.LabelConnectionDesc.Name = "LabelConnectionDesc"
        Me.LabelConnectionDesc.Size = New System.Drawing.Size(553, 23)
        Me.LabelConnectionDesc.TabIndex = 21
        Me.LabelConnectionDesc.Text = "Enter the SAP system connection details and authentication credentials"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(37, 403)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 28)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Language"
        '
        'LabelLanguageDesc
        '
        Me.LabelLanguageDesc.AutoSize = True
        Me.LabelLanguageDesc.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.LabelLanguageDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.LabelLanguageDesc.Location = New System.Drawing.Point(305, 434)
        Me.LabelLanguageDesc.Name = "LabelLanguageDesc"
        Me.LabelLanguageDesc.Size = New System.Drawing.Size(410, 23)
        Me.LabelLanguageDesc.TabIndex = 21
        Me.LabelLanguageDesc.Text = "Language code for the RFC connection (e.g., EN, DE)"
        '
        'txtAppServerHost
        '
        Me.txtAppServerHost.BackColor = System.Drawing.Color.White
        Me.txtAppServerHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAppServerHost.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAppServerHost.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.txtAppServerHost.Location = New System.Drawing.Point(309, 84)
        Me.txtAppServerHost.Name = "txtAppServerHost"
        Me.txtAppServerHost.Size = New System.Drawing.Size(496, 33)
        Me.txtAppServerHost.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(37, 327)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(93, 28)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Password"
        '
        'LabelPasswordDesc
        '
        Me.LabelPasswordDesc.AutoSize = True
        Me.LabelPasswordDesc.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.LabelPasswordDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.LabelPasswordDesc.Location = New System.Drawing.Point(305, 358)
        Me.LabelPasswordDesc.Name = "LabelPasswordDesc"
        Me.LabelPasswordDesc.Size = New System.Drawing.Size(364, 23)
        Me.LabelPasswordDesc.TabIndex = 20
        Me.LabelPasswordDesc.Text = "Password for the RFC user (securely encrypted)"
        '
        'txtClient
        '
        Me.txtClient.BackColor = System.Drawing.Color.White
        Me.txtClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtClient.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClient.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.txtClient.Location = New System.Drawing.Point(309, 157)
        Me.txtClient.Name = "txtClient"
        Me.txtClient.Size = New System.Drawing.Size(130, 33)
        Me.txtClient.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(39, 256)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(90, 28)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "RFC User"
        '
        'LabelUserDesc
        '
        Me.LabelUserDesc.AutoSize = True
        Me.LabelUserDesc.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.LabelUserDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.LabelUserDesc.Location = New System.Drawing.Point(303, 287)
        Me.LabelUserDesc.Name = "LabelUserDesc"
        Me.LabelUserDesc.Size = New System.Drawing.Size(376, 23)
        Me.LabelUserDesc.TabIndex = 19
        Me.LabelUserDesc.Text = "Username for RFC authentication to SAP system"
        '
        'txtSystemNumber
        '
        Me.txtSystemNumber.BackColor = System.Drawing.Color.White
        Me.txtSystemNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSystemNumber.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSystemNumber.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.txtSystemNumber.Location = New System.Drawing.Point(309, 203)
        Me.txtSystemNumber.Name = "txtSystemNumber"
        Me.txtSystemNumber.Size = New System.Drawing.Size(130, 33)
        Me.txtSystemNumber.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(37, 205)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(160, 28)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Instance Number"
        '
        'LabelInstanceDesc
        '
        Me.LabelInstanceDesc.AutoSize = True
        Me.LabelInstanceDesc.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.LabelInstanceDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.LabelInstanceDesc.Location = New System.Drawing.Point(445, 210)
        Me.LabelInstanceDesc.Name = "LabelInstanceDesc"
        Me.LabelInstanceDesc.Size = New System.Drawing.Size(354, 23)
        Me.LabelInstanceDesc.TabIndex = 18
        Me.LabelInstanceDesc.Text = "SAP system instance number (typically 00-99)"
        '
        'txtUser
        '
        Me.txtUser.BackColor = System.Drawing.Color.White
        Me.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUser.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUser.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.txtUser.Location = New System.Drawing.Point(309, 251)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(496, 33)
        Me.txtUser.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(37, 163)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 28)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Client"
        '
        'LabelClientDesc
        '
        Me.LabelClientDesc.AutoSize = True
        Me.LabelClientDesc.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.LabelClientDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.LabelClientDesc.Location = New System.Drawing.Point(445, 163)
        Me.LabelClientDesc.Name = "LabelClientDesc"
        Me.LabelClientDesc.Size = New System.Drawing.Size(359, 23)
        Me.LabelClientDesc.TabIndex = 17
        Me.LabelClientDesc.Text = "SAP client number (typically 3 digits, e.g., 100)"
        '
        'txtPassword
        '
        Me.txtPassword.BackColor = System.Drawing.Color.White
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.txtPassword.Location = New System.Drawing.Point(309, 322)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(496, 33)
        Me.txtPassword.TabIndex = 6
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(39, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(155, 28)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "App Server Host"
        '
        'LabelHostDesc
        '
        Me.LabelHostDesc.AutoSize = True
        Me.LabelHostDesc.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.LabelHostDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.LabelHostDesc.Location = New System.Drawing.Point(305, 120)
        Me.LabelHostDesc.Name = "LabelHostDesc"
        Me.LabelHostDesc.Size = New System.Drawing.Size(416, 23)
        Me.LabelHostDesc.TabIndex = 16
        Me.LabelHostDesc.Text = "Hostname or IP address of the SAP application server"
        '
        'txtLanguage
        '
        Me.txtLanguage.BackColor = System.Drawing.Color.White
        Me.txtLanguage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLanguage.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLanguage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.txtLanguage.Location = New System.Drawing.Point(309, 398)
        Me.txtLanguage.Name = "txtLanguage"
        Me.txtLanguage.Size = New System.Drawing.Size(130, 33)
        Me.txtLanguage.TabIndex = 7
        Me.txtLanguage.Text = "EN"
        '
        'RFCSettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1560, 1020)
        Me.Controls.Add(Me.PanelMain)
        Me.Controls.Add(Me.PanelHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1000, 800)
        Me.Name = "RFCSettingsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SAP Connection Manager"
        Me.PanelHeader.ResumeLayout(False)
        Me.PanelHeader.PerformLayout()
        Me.PanelMain.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.dgvSystemStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelServerSection.ResumeLayout(False)
        Me.PanelServerSection.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelConnectionSection.ResumeLayout(False)
        Me.PanelConnectionSection.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelHeader As Panel
    Friend WithEvents PanelHeaderShine As Panel
    Friend WithEvents LabelHeader As Label
    Friend WithEvents LabelSubtitle As Label
    Friend WithEvents PanelMain As Panel
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents txtUser As TextBox
    Friend WithEvents txtSystemNumber As TextBox
    Friend WithEvents txtClient As TextBox
    Friend WithEvents txtAppServerHost As TextBox
    Friend WithEvents cmbSystems As ComboBox
    Friend WithEvents txtSystemName As TextBox
    Friend WithEvents txtLanguage As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnTestConnection As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnCancelEdit As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents LabelSystemNameDesc As Label
    Friend WithEvents LabelHostDesc As Label
    Friend WithEvents LabelClientDesc As Label
    Friend WithEvents LabelInstanceDesc As Label
    Friend WithEvents LabelUserDesc As Label
    Friend WithEvents LabelPasswordDesc As Label
    Friend WithEvents LabelLanguageDesc As Label
    Friend WithEvents LabelServerDesc As Label
    Friend WithEvents LabelConnectionDesc As Label
    Friend WithEvents LabelServerInfo As Label
    Friend WithEvents LabelConnectionInfo As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PanelServerSection As Panel
    Friend WithEvents PanelConnectionSection As Panel
    Friend WithEvents dgvSystemStatus As DataGridView
    Friend WithEvents lblStatusSummary As Label
    Friend WithEvents lblReferesh As LinkLabel
End Class