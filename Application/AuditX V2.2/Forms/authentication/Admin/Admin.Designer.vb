<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Admin
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Admin))
        Me.PanelHeader = New System.Windows.Forms.Panel()
        Me.pbProfile = New System.Windows.Forms.PictureBox()
        Me.picLogo = New System.Windows.Forms.PictureBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.lblUserHeader = New System.Windows.Forms.Label()
        Me.lblLicenseHeader = New System.Windows.Forms.Label()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.txtWindowsID = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFullName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.lblExpiry = New System.Windows.Forms.Label()
        Me.dtpExpiry = New System.Windows.Forms.DateTimePicker()
        Me.lblPath = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.pbProgressLine = New System.Windows.Forms.ProgressBar()
        Me.flowActions = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.tblMain = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelHeader.SuspendLayout()
        CType(Me.pbProfile, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.flowActions.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelHeader
        '
        Me.PanelHeader.BackColor = System.Drawing.Color.Silver
        Me.PanelHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PanelHeader.Controls.Add(Me.pbProfile)
        Me.PanelHeader.Controls.Add(Me.picLogo)
        Me.PanelHeader.Controls.Add(Me.lblTitle)
        Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
        Me.PanelHeader.Name = "PanelHeader"
        Me.PanelHeader.Padding = New System.Windows.Forms.Padding(12, 12, 12, 12)
        Me.PanelHeader.Size = New System.Drawing.Size(960, 56)
        Me.PanelHeader.TabIndex = 1
        '
        'pbProfile
        '
        Me.pbProfile.BackColor = System.Drawing.Color.Transparent
        Me.pbProfile.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbProfile.Image = CType(resources.GetObject("pbProfile.Image"), System.Drawing.Image)
        Me.pbProfile.Location = New System.Drawing.Point(900, 8)
        Me.pbProfile.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pbProfile.Name = "pbProfile"
        Me.pbProfile.Size = New System.Drawing.Size(58, 49)
        Me.pbProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbProfile.TabIndex = 40
        Me.pbProfile.TabStop = False
        '
        'picLogo
        '
        Me.picLogo.BackColor = System.Drawing.Color.Transparent
        Me.picLogo.Image = Global.AuditX.My.Resources.Resources.AVSA_icon
        Me.picLogo.Location = New System.Drawing.Point(0, 5)
        Me.picLogo.Name = "picLogo"
        Me.picLogo.Size = New System.Drawing.Size(60, 57)
        Me.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picLogo.TabIndex = 8
        Me.picLogo.TabStop = False
        '
        'lblTitle
        '
        Me.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblTitle.AutoSize = True
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(59, 6)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(490, 48)
        Me.lblTitle.TabIndex = 7
        Me.lblTitle.Text = "AuditX Access Management"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.GroupBox1.Controls.Add(Me.btnBrowse)
        Me.GroupBox1.Controls.Add(Me.txtPath)
        Me.GroupBox1.Controls.Add(Me.lblUserHeader)
        Me.GroupBox1.Controls.Add(Me.lblLicenseHeader)
        Me.GroupBox1.Controls.Add(Me.lblUser)
        Me.GroupBox1.Controls.Add(Me.txtWindowsID)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtFullName)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtEmail)
        Me.GroupBox1.Controls.Add(Me.lblExpiry)
        Me.GroupBox1.Controls.Add(Me.dtpExpiry)
        Me.GroupBox1.Controls.Add(Me.lblPath)
        Me.GroupBox1.Controls.Add(Me.lblStatus)
        Me.GroupBox1.Controls.Add(Me.pbProgressLine)
        Me.GroupBox1.Controls.Add(Me.flowActions)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 56)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(960, 470)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        '
        'btnBrowse
        '
        Me.btnBrowse.BackColor = System.Drawing.Color.FromArgb(CType(CType(24, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnBrowse.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnBrowse.ForeColor = System.Drawing.Color.White
        Me.btnBrowse.Location = New System.Drawing.Point(815, 199)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(110, 48)
        Me.btnBrowse.TabIndex = 5
        Me.btnBrowse.Text = "Browse..."
        Me.btnBrowse.UseVisualStyleBackColor = False
        '
        'txtPath
        '
        Me.txtPath.Enabled = False
        Me.txtPath.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtPath.Location = New System.Drawing.Point(504, 253)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(421, 37)
        Me.txtPath.TabIndex = 10
        '
        'lblUserHeader
        '
        Me.lblUserHeader.AutoSize = True
        Me.lblUserHeader.BackColor = System.Drawing.Color.Transparent
        Me.lblUserHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblUserHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.lblUserHeader.Location = New System.Drawing.Point(60, 40)
        Me.lblUserHeader.Name = "lblUserHeader"
        Me.lblUserHeader.Size = New System.Drawing.Size(210, 32)
        Me.lblUserHeader.TabIndex = 11
        Me.lblUserHeader.Text = "User Information"
        '
        'lblLicenseHeader
        '
        Me.lblLicenseHeader.AutoSize = True
        Me.lblLicenseHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblLicenseHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.lblLicenseHeader.Location = New System.Drawing.Point(500, 40)
        Me.lblLicenseHeader.Name = "lblLicenseHeader"
        Me.lblLicenseHeader.Size = New System.Drawing.Size(196, 32)
        Me.lblLicenseHeader.TabIndex = 12
        Me.lblLicenseHeader.Text = "License Settings"
        '
        'lblUser
        '
        Me.lblUser.AutoSize = True
        Me.lblUser.BackColor = System.Drawing.Color.Transparent
        Me.lblUser.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblUser.Location = New System.Drawing.Point(61, 80)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(121, 28)
        Me.lblUser.TabIndex = 0
        Me.lblUser.Text = "Windows ID:"
        '
        'txtWindowsID
        '
        Me.txtWindowsID.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtWindowsID.Location = New System.Drawing.Point(64, 111)
        Me.txtWindowsID.Name = "txtWindowsID"
        Me.txtWindowsID.Size = New System.Drawing.Size(391, 37)
        Me.txtWindowsID.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.Location = New System.Drawing.Point(61, 149)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(104, 28)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Full Name:"
        '
        'txtFullName
        '
        Me.txtFullName.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtFullName.Location = New System.Drawing.Point(66, 180)
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.Size = New System.Drawing.Size(389, 37)
        Me.txtFullName.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label3.Location = New System.Drawing.Point(61, 220)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(138, 28)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Email Address:"
        '
        'txtEmail
        '
        Me.txtEmail.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtEmail.Location = New System.Drawing.Point(64, 253)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(391, 37)
        Me.txtEmail.TabIndex = 3
        '
        'lblExpiry
        '
        Me.lblExpiry.AutoSize = True
        Me.lblExpiry.BackColor = System.Drawing.Color.Transparent
        Me.lblExpiry.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblExpiry.Location = New System.Drawing.Point(501, 80)
        Me.lblExpiry.Name = "lblExpiry"
        Me.lblExpiry.Size = New System.Drawing.Size(115, 28)
        Me.lblExpiry.TabIndex = 2
        Me.lblExpiry.Text = "Expiry Date:"
        '
        'dtpExpiry
        '
        Me.dtpExpiry.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.dtpExpiry.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpExpiry.Location = New System.Drawing.Point(504, 111)
        Me.dtpExpiry.Name = "dtpExpiry"
        Me.dtpExpiry.Size = New System.Drawing.Size(180, 37)
        Me.dtpExpiry.TabIndex = 4
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.BackColor = System.Drawing.Color.Transparent
        Me.lblPath.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblPath.Location = New System.Drawing.Point(499, 222)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(137, 28)
        Me.lblPath.TabIndex = 4
        Me.lblPath.Text = "Save Location:"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblStatus.ForeColor = System.Drawing.Color.MediumSeaGreen
        Me.lblStatus.Location = New System.Drawing.Point(267, 310)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(400, 44)
        Me.lblStatus.TabIndex = 8
        Me.lblStatus.Text = "Ready to generate license..."
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pbProgressLine
        '
        Me.pbProgressLine.Location = New System.Drawing.Point(164, 357)
        Me.pbProgressLine.Name = "pbProgressLine"
        Me.pbProgressLine.Size = New System.Drawing.Size(600, 15)
        Me.pbProgressLine.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbProgressLine.TabIndex = 13
        Me.pbProgressLine.Value = 30
        '
        'flowActions
        '
        Me.flowActions.BackColor = System.Drawing.Color.Transparent
        Me.flowActions.Controls.Add(Me.btnGenerate)
        Me.flowActions.Location = New System.Drawing.Point(327, 377)
        Me.flowActions.Name = "flowActions"
        Me.flowActions.Size = New System.Drawing.Size(310, 66)
        Me.flowActions.TabIndex = 6
        '
        'btnGenerate
        '
        Me.btnGenerate.BackColor = System.Drawing.Color.FromArgb(CType(CType(24, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnGenerate.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnGenerate.ForeColor = System.Drawing.Color.White
        Me.btnGenerate.Location = New System.Drawing.Point(3, 3)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(307, 63)
        Me.btnGenerate.TabIndex = 6
        Me.btnGenerate.Text = "Generate License"
        Me.btnGenerate.UseVisualStyleBackColor = False
        '
        'tblMain
        '
        Me.tblMain.Location = New System.Drawing.Point(0, 0)
        Me.tblMain.Name = "tblMain"
        Me.tblMain.Size = New System.Drawing.Size(200, 100)
        Me.tblMain.TabIndex = 0
        '
        'Admin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 526)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PanelHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Admin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Admin Dashboard"
        Me.PanelHeader.ResumeLayout(False)
        Me.PanelHeader.PerformLayout()
        CType(Me.pbProfile, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.flowActions.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelHeader As Panel
    Friend WithEvents pbProfile As PictureBox
    Friend WithEvents picLogo As PictureBox
    Friend WithEvents lblTitle As Label
    Friend WithEvents tblMain As TableLayoutPanel ' Kept declaration in case referenced in code-behind
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblUser As Label
    Friend WithEvents txtWindowsID As TextBox
    Friend WithEvents lblExpiry As Label
    Friend WithEvents dtpExpiry As DateTimePicker
    Friend WithEvents lblPath As Label
    Friend WithEvents txtPath As TextBox
    Friend WithEvents btnBrowse As Button
    Friend WithEvents flowActions As FlowLayoutPanel
    Friend WithEvents btnGenerate As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents txtFullName As TextBox
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblUserHeader As Label
    Friend WithEvents lblLicenseHeader As Label
    Friend WithEvents pbProgressLine As ProgressBar

End Class