<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ctrlGeneral
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.txtEndTime = New System.Windows.Forms.TextBox()
        Me.lblEndTime = New System.Windows.Forms.Label()
        Me.txtStartTime = New System.Windows.Forms.TextBox()
        Me.lblStartTime = New System.Windows.Forms.Label()
        Me.btnGeneralSave = New System.Windows.Forms.Button()
        Me.lblSAPGUIXML = New System.Windows.Forms.Label()
        Me.lblITGCControl = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtITGC17_ApprovedUsers = New System.Windows.Forms.TextBox()
        Me.txtITGC16_ProgramName = New System.Windows.Forms.TextBox()
        Me.txtITGC04_TcodeList = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblSAPGUIApp = New System.Windows.Forms.Label()
        Me.PanelHeader = New System.Windows.Forms.Panel()
        Me.PanelButton = New System.Windows.Forms.Panel()
        Me.btnRevertDefaults = New System.Windows.Forms.Button()
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PanelBody = New System.Windows.Forms.Panel()
        Me.GroupBox12 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtITGC08_ApprovedUsers = New System.Windows.Forms.TextBox()
        Me.PanelFooter = New System.Windows.Forms.Panel()
        Me.lblFooter = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblDownloadDestination = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtITGC01_OssId = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtITGC09_ApprovedUsers = New System.Windows.Forms.TextBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.txtITGC10_ApprovedUserCheck5 = New System.Windows.Forms.TextBox()
        Me.txtITGC10_ApprovedUserCheck1 = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtITGC10_ApprovedUserCheck4 = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtITGC10_ApprovedUserCheck2 = New System.Windows.Forms.TextBox()
        Me.txtITGC10_ApprovedUserCheck3 = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.txtITGC11_ApprovedUsers = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtITGC13_ApprovedUsers = New System.Windows.Forms.TextBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtITGC12_ApprovedUsers = New System.Windows.Forms.TextBox()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.txtITGC15_ApprovedUsers = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.GroupBox11 = New System.Windows.Forms.GroupBox()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.PanelHeader.SuspendLayout()
        Me.PanelButton.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelBody.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.PanelFooter.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtEndTime
        '
        Me.txtEndTime.BackColor = System.Drawing.Color.AliceBlue
        Me.txtEndTime.Font = New System.Drawing.Font("Segoe UI", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndTime.ForeColor = System.Drawing.Color.Black
        Me.txtEndTime.Location = New System.Drawing.Point(573, 272)
        Me.txtEndTime.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtEndTime.Name = "txtEndTime"
        Me.txtEndTime.Size = New System.Drawing.Size(299, 36)
        Me.txtEndTime.TabIndex = 14
        Me.ToolTip1.SetToolTip(Me.txtEndTime, "End Time must be in HH:mm:ss format (e.g., 23:59:59).")
        '
        'lblEndTime
        '
        Me.lblEndTime.AutoSize = True
        Me.lblEndTime.BackColor = System.Drawing.Color.White
        Me.lblEndTime.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndTime.Location = New System.Drawing.Point(116, 278)
        Me.lblEndTime.Name = "lblEndTime"
        Me.lblEndTime.Size = New System.Drawing.Size(177, 25)
        Me.lblEndTime.TabIndex = 13
        Me.lblEndTime.Text = "Report End Time"
        '
        'txtStartTime
        '
        Me.txtStartTime.BackColor = System.Drawing.Color.AliceBlue
        Me.txtStartTime.Font = New System.Drawing.Font("Segoe UI", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartTime.ForeColor = System.Drawing.Color.Black
        Me.txtStartTime.Location = New System.Drawing.Point(573, 212)
        Me.txtStartTime.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtStartTime.Name = "txtStartTime"
        Me.txtStartTime.Size = New System.Drawing.Size(299, 36)
        Me.txtStartTime.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.txtStartTime, "Start Time must be in HH:mm:ss format (e.g., 00:00:00).")
        '
        'lblStartTime
        '
        Me.lblStartTime.AutoSize = True
        Me.lblStartTime.BackColor = System.Drawing.Color.White
        Me.lblStartTime.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartTime.Location = New System.Drawing.Point(116, 218)
        Me.lblStartTime.Name = "lblStartTime"
        Me.lblStartTime.Size = New System.Drawing.Size(183, 25)
        Me.lblStartTime.TabIndex = 11
        Me.lblStartTime.Text = "Report Start Time"
        '
        'btnGeneralSave
        '
        Me.btnGeneralSave.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnGeneralSave.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnGeneralSave.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGeneralSave.ForeColor = System.Drawing.Color.Black
        Me.btnGeneralSave.Location = New System.Drawing.Point(153, 14)
        Me.btnGeneralSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnGeneralSave.Name = "btnGeneralSave"
        Me.btnGeneralSave.Size = New System.Drawing.Size(144, 52)
        Me.btnGeneralSave.TabIndex = 17
        Me.btnGeneralSave.Text = "Save"
        Me.ToolTip1.SetToolTip(Me.btnGeneralSave, "Click to save the setting")
        Me.btnGeneralSave.UseVisualStyleBackColor = False
        '
        'lblSAPGUIXML
        '
        Me.lblSAPGUIXML.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSAPGUIXML.AutoSize = True
        Me.lblSAPGUIXML.BackColor = System.Drawing.Color.White
        Me.lblSAPGUIXML.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblSAPGUIXML.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSAPGUIXML.ForeColor = System.Drawing.SystemColors.Highlight
        Me.lblSAPGUIXML.Location = New System.Drawing.Point(569, 104)
        Me.lblSAPGUIXML.Name = "lblSAPGUIXML"
        Me.lblSAPGUIXML.Size = New System.Drawing.Size(160, 27)
        Me.lblSAPGUIXML.TabIndex = 19
        Me.lblSAPGUIXML.Text = "SAPGUI XML File"
        '
        'lblITGCControl
        '
        Me.lblITGCControl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblITGCControl.AutoSize = True
        Me.lblITGCControl.BackColor = System.Drawing.Color.White
        Me.lblITGCControl.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblITGCControl.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblITGCControl.ForeColor = System.Drawing.SystemColors.Highlight
        Me.lblITGCControl.Location = New System.Drawing.Point(569, 161)
        Me.lblITGCControl.Name = "lblITGCControl"
        Me.lblITGCControl.Size = New System.Drawing.Size(160, 27)
        Me.lblITGCControl.TabIndex = 21
        Me.lblITGCControl.Text = "Control XML File"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.White
        Me.Label2.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(116, 160)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(189, 25)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "ITGC Control File"
        '
        'ToolTip1
        '
        Me.ToolTip1.BackColor = System.Drawing.Color.White
        '
        'txtITGC17_ApprovedUsers
        '
        Me.txtITGC17_ApprovedUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC17_ApprovedUsers.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC17_ApprovedUsers.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC17_ApprovedUsers.ForeColor = System.Drawing.Color.Black
        Me.txtITGC17_ApprovedUsers.Location = New System.Drawing.Point(555, 64)
        Me.txtITGC17_ApprovedUsers.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC17_ApprovedUsers.Name = "txtITGC17_ApprovedUsers"
        Me.txtITGC17_ApprovedUsers.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC17_ApprovedUsers.TabIndex = 75
        Me.ToolTip1.SetToolTip(Me.txtITGC17_ApprovedUsers, "NOTE: Approved Users must be separated with a semicolon (;) only.")
        '
        'txtITGC16_ProgramName
        '
        Me.txtITGC16_ProgramName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC16_ProgramName.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC16_ProgramName.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC16_ProgramName.ForeColor = System.Drawing.Color.Black
        Me.txtITGC16_ProgramName.Location = New System.Drawing.Point(554, 69)
        Me.txtITGC16_ProgramName.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC16_ProgramName.Name = "txtITGC16_ProgramName"
        Me.txtITGC16_ProgramName.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC16_ProgramName.TabIndex = 71
        Me.ToolTip1.SetToolTip(Me.txtITGC16_ProgramName, "NOTE: Must be separated with a semicolon (;) only.")
        '
        'txtITGC04_TcodeList
        '
        Me.txtITGC04_TcodeList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC04_TcodeList.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC04_TcodeList.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC04_TcodeList.ForeColor = System.Drawing.Color.Black
        Me.txtITGC04_TcodeList.Location = New System.Drawing.Point(564, 55)
        Me.txtITGC04_TcodeList.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC04_TcodeList.Name = "txtITGC04_TcodeList"
        Me.txtITGC04_TcodeList.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC04_TcodeList.TabIndex = 34
        Me.ToolTip1.SetToolTip(Me.txtITGC04_TcodeList, "NOTE: Tcodes must be separated with a semicolon (;) only.")
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.White
        Me.Label4.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(116, 102)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(205, 25)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "SAP GUI XML File"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.White
        Me.Label1.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(116, 52)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(172, 25)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "SAP Logon Path"
        '
        'lblSAPGUIApp
        '
        Me.lblSAPGUIApp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSAPGUIApp.AutoSize = True
        Me.lblSAPGUIApp.BackColor = System.Drawing.Color.White
        Me.lblSAPGUIApp.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblSAPGUIApp.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSAPGUIApp.ForeColor = System.Drawing.SystemColors.Highlight
        Me.lblSAPGUIApp.Location = New System.Drawing.Point(569, 54)
        Me.lblSAPGUIApp.Name = "lblSAPGUIApp"
        Me.lblSAPGUIApp.Size = New System.Drawing.Size(142, 27)
        Me.lblSAPGUIApp.TabIndex = 28
        Me.lblSAPGUIApp.Text = "SAPLogonPath"
        '
        'PanelHeader
        '
        Me.PanelHeader.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelHeader.Controls.Add(Me.PanelButton)
        Me.PanelHeader.Controls.Add(Me.lblHeading)
        Me.PanelHeader.Controls.Add(Me.PictureBox1)
        Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
        Me.PanelHeader.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelHeader.Name = "PanelHeader"
        Me.PanelHeader.Size = New System.Drawing.Size(1447, 74)
        Me.PanelHeader.TabIndex = 29
        '
        'PanelButton
        '
        Me.PanelButton.AutoSize = True
        Me.PanelButton.BackColor = System.Drawing.Color.Transparent
        Me.PanelButton.Controls.Add(Me.btnGeneralSave)
        Me.PanelButton.Controls.Add(Me.btnRevertDefaults)
        Me.PanelButton.Dock = System.Windows.Forms.DockStyle.Right
        Me.PanelButton.Location = New System.Drawing.Point(1147, 0)
        Me.PanelButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelButton.Name = "PanelButton"
        Me.PanelButton.Size = New System.Drawing.Size(300, 74)
        Me.PanelButton.TabIndex = 80
        '
        'btnRevertDefaults
        '
        Me.btnRevertDefaults.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnRevertDefaults.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRevertDefaults.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRevertDefaults.ForeColor = System.Drawing.Color.Black
        Me.btnRevertDefaults.Location = New System.Drawing.Point(3, 14)
        Me.btnRevertDefaults.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRevertDefaults.Name = "btnRevertDefaults"
        Me.btnRevertDefaults.Size = New System.Drawing.Size(144, 52)
        Me.btnRevertDefaults.TabIndex = 28
        Me.btnRevertDefaults.Text = "Defaults"
        Me.btnRevertDefaults.UseVisualStyleBackColor = False
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.BackColor = System.Drawing.Color.Transparent
        Me.lblHeading.Font = New System.Drawing.Font("Calisto MT", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeading.ForeColor = System.Drawing.Color.Black
        Me.lblHeading.Location = New System.Drawing.Point(94, 11)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(181, 55)
        Me.lblHeading.TabIndex = 27
        Me.lblHeading.Text = "Settings"
        Me.lblHeading.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = Global.AuditX.My.Resources.Resources.settings__1_
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(101, 74)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 26
        Me.PictureBox1.TabStop = False
        '
        'PanelBody
        '
        Me.PanelBody.AutoScroll = True
        Me.PanelBody.AutoSize = True
        Me.PanelBody.BackColor = System.Drawing.Color.White
        Me.PanelBody.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PanelBody.Controls.Add(Me.GroupBox12)
        Me.PanelBody.Controls.Add(Me.PanelFooter)
        Me.PanelBody.Controls.Add(Me.GroupBox1)
        Me.PanelBody.Controls.Add(Me.GroupBox2)
        Me.PanelBody.Controls.Add(Me.GroupBox3)
        Me.PanelBody.Controls.Add(Me.GroupBox4)
        Me.PanelBody.Controls.Add(Me.GroupBox5)
        Me.PanelBody.Controls.Add(Me.GroupBox6)
        Me.PanelBody.Controls.Add(Me.GroupBox8)
        Me.PanelBody.Controls.Add(Me.GroupBox7)
        Me.PanelBody.Controls.Add(Me.GroupBox9)
        Me.PanelBody.Controls.Add(Me.GroupBox11)
        Me.PanelBody.Controls.Add(Me.GroupBox10)
        Me.PanelBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelBody.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PanelBody.Location = New System.Drawing.Point(0, 74)
        Me.PanelBody.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelBody.Name = "PanelBody"
        Me.PanelBody.Size = New System.Drawing.Size(1447, 2389)
        Me.PanelBody.TabIndex = 30
        '
        'GroupBox12
        '
        Me.GroupBox12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox12.BackColor = System.Drawing.Color.White
        Me.GroupBox12.Controls.Add(Me.Label5)
        Me.GroupBox12.Controls.Add(Me.txtITGC08_ApprovedUsers)
        Me.GroupBox12.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox12.ForeColor = System.Drawing.Color.Black
        Me.GroupBox12.Location = New System.Drawing.Point(18, 678)
        Me.GroupBox12.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox12.Size = New System.Drawing.Size(1113, 135)
        Me.GroupBox12.TabIndex = 80
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "ITGC08 Controls"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.White
        Me.Label5.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(108, 61)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(164, 25)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Approved Users"
        '
        'txtITGC08_ApprovedUsers
        '
        Me.txtITGC08_ApprovedUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC08_ApprovedUsers.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC08_ApprovedUsers.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC08_ApprovedUsers.ForeColor = System.Drawing.Color.Black
        Me.txtITGC08_ApprovedUsers.Location = New System.Drawing.Point(561, 55)
        Me.txtITGC08_ApprovedUsers.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC08_ApprovedUsers.Name = "txtITGC08_ApprovedUsers"
        Me.txtITGC08_ApprovedUsers.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC08_ApprovedUsers.TabIndex = 39
        '
        'PanelFooter
        '
        Me.PanelFooter.BackColor = System.Drawing.Color.White
        Me.PanelFooter.Controls.Add(Me.lblFooter)
        Me.PanelFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelFooter.Location = New System.Drawing.Point(0, 2297)
        Me.PanelFooter.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelFooter.Name = "PanelFooter"
        Me.PanelFooter.Size = New System.Drawing.Size(1421, 100)
        Me.PanelFooter.TabIndex = 37
        '
        'lblFooter
        '
        Me.lblFooter.BackColor = System.Drawing.Color.White
        Me.lblFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblFooter.Font = New System.Drawing.Font("Calisto MT", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFooter.ForeColor = System.Drawing.Color.Black
        Me.lblFooter.Location = New System.Drawing.Point(0, 0)
        Me.lblFooter.Name = "lblFooter"
        Me.lblFooter.Size = New System.Drawing.Size(1421, 100)
        Me.lblFooter.TabIndex = 0
        Me.lblFooter.Text = "Thank You!"
        Me.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.BackColor = System.Drawing.Color.White
        Me.GroupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.GroupBox1.Controls.Add(Me.lblDownloadDestination)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtStartTime)
        Me.GroupBox1.Controls.Add(Me.lblSAPGUIXML)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtEndTime)
        Me.GroupBox1.Controls.Add(Me.lblITGCControl)
        Me.GroupBox1.Controls.Add(Me.lblEndTime)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.lblSAPGUIApp)
        Me.GroupBox1.Controls.Add(Me.lblStartTime)
        Me.GroupBox1.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.Black
        Me.GroupBox1.Location = New System.Drawing.Point(18, 33)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(1113, 387)
        Me.GroupBox1.TabIndex = 78
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Tag = "General Controls"
        Me.GroupBox1.Text = "General Controls"
        '
        'lblDownloadDestination
        '
        Me.lblDownloadDestination.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDownloadDestination.AutoSize = True
        Me.lblDownloadDestination.BackColor = System.Drawing.Color.White
        Me.lblDownloadDestination.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblDownloadDestination.Font = New System.Drawing.Font("Calibri", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDownloadDestination.ForeColor = System.Drawing.SystemColors.Highlight
        Me.lblDownloadDestination.Location = New System.Drawing.Point(569, 334)
        Me.lblDownloadDestination.Name = "lblDownloadDestination"
        Me.lblDownloadDestination.Size = New System.Drawing.Size(233, 27)
        Me.lblDownloadDestination.TabIndex = 30
        Me.lblDownloadDestination.Text = "lblDownloadDestination"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.White
        Me.Label3.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(116, 334)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(227, 25)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Download Destination"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.BackColor = System.Drawing.Color.White
        Me.GroupBox2.Controls.Add(Me.txtITGC01_OssId)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.Black
        Me.GroupBox2.Location = New System.Drawing.Point(18, 428)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox2.Size = New System.Drawing.Size(1113, 109)
        Me.GroupBox2.TabIndex = 79
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "ITGC01 Controls"
        '
        'txtITGC01_OssId
        '
        Me.txtITGC01_OssId.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC01_OssId.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC01_OssId.ForeColor = System.Drawing.Color.Black
        Me.txtITGC01_OssId.Location = New System.Drawing.Point(564, 49)
        Me.txtITGC01_OssId.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtITGC01_OssId.Name = "txtITGC01_OssId"
        Me.txtITGC01_OssId.Size = New System.Drawing.Size(308, 37)
        Me.txtITGC01_OssId.TabIndex = 30
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.White
        Me.Label6.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(110, 55)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(168, 25)
        Me.Label6.TabIndex = 31
        Me.Label6.Text = "Selected OSS ID"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.BackColor = System.Drawing.Color.White
        Me.GroupBox3.Controls.Add(Me.txtITGC04_TcodeList)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Font = New System.Drawing.Font("Calisto MT", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.ForeColor = System.Drawing.Color.Black
        Me.GroupBox3.Location = New System.Drawing.Point(18, 545)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox3.Size = New System.Drawing.Size(1113, 125)
        Me.GroupBox3.TabIndex = 79
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "ITGC4 Controls"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.White
        Me.Label9.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(108, 61)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(306, 25)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "Set Critical Transaction Usages"
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.BackColor = System.Drawing.Color.White
        Me.GroupBox4.Controls.Add(Me.Label10)
        Me.GroupBox4.Controls.Add(Me.txtITGC09_ApprovedUsers)
        Me.GroupBox4.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.ForeColor = System.Drawing.Color.Black
        Me.GroupBox4.Location = New System.Drawing.Point(18, 821)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox4.Size = New System.Drawing.Size(1113, 135)
        Me.GroupBox4.TabIndex = 79
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "ITGC09 Controls"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.White
        Me.Label10.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(108, 61)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(164, 25)
        Me.Label10.TabIndex = 40
        Me.Label10.Text = "Approved Users"
        '
        'txtITGC09_ApprovedUsers
        '
        Me.txtITGC09_ApprovedUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC09_ApprovedUsers.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC09_ApprovedUsers.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC09_ApprovedUsers.ForeColor = System.Drawing.Color.Black
        Me.txtITGC09_ApprovedUsers.Location = New System.Drawing.Point(561, 55)
        Me.txtITGC09_ApprovedUsers.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC09_ApprovedUsers.Name = "txtITGC09_ApprovedUsers"
        Me.txtITGC09_ApprovedUsers.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC09_ApprovedUsers.TabIndex = 39
        '
        'GroupBox5
        '
        Me.GroupBox5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox5.BackColor = System.Drawing.Color.White
        Me.GroupBox5.Controls.Add(Me.txtITGC10_ApprovedUserCheck5)
        Me.GroupBox5.Controls.Add(Me.txtITGC10_ApprovedUserCheck1)
        Me.GroupBox5.Controls.Add(Me.Label18)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.txtITGC10_ApprovedUserCheck4)
        Me.GroupBox5.Controls.Add(Me.Label15)
        Me.GroupBox5.Controls.Add(Me.Label16)
        Me.GroupBox5.Controls.Add(Me.txtITGC10_ApprovedUserCheck2)
        Me.GroupBox5.Controls.Add(Me.txtITGC10_ApprovedUserCheck3)
        Me.GroupBox5.Controls.Add(Me.Label17)
        Me.GroupBox5.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.ForeColor = System.Drawing.Color.Black
        Me.GroupBox5.Location = New System.Drawing.Point(12, 956)
        Me.GroupBox5.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox5.Size = New System.Drawing.Size(1113, 442)
        Me.GroupBox5.TabIndex = 79
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "ITGC10 Controls"
        '
        'txtITGC10_ApprovedUserCheck5
        '
        Me.txtITGC10_ApprovedUserCheck5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC10_ApprovedUserCheck5.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC10_ApprovedUserCheck5.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC10_ApprovedUserCheck5.ForeColor = System.Drawing.Color.Black
        Me.txtITGC10_ApprovedUserCheck5.Location = New System.Drawing.Point(564, 360)
        Me.txtITGC10_ApprovedUserCheck5.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC10_ApprovedUserCheck5.Name = "txtITGC10_ApprovedUserCheck5"
        Me.txtITGC10_ApprovedUserCheck5.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC10_ApprovedUserCheck5.TabIndex = 52
        '
        'txtITGC10_ApprovedUserCheck1
        '
        Me.txtITGC10_ApprovedUserCheck1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC10_ApprovedUserCheck1.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC10_ApprovedUserCheck1.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC10_ApprovedUserCheck1.ForeColor = System.Drawing.Color.Black
        Me.txtITGC10_ApprovedUserCheck1.Location = New System.Drawing.Point(564, 61)
        Me.txtITGC10_ApprovedUserCheck1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC10_ApprovedUserCheck1.Name = "txtITGC10_ApprovedUserCheck1"
        Me.txtITGC10_ApprovedUserCheck1.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC10_ApprovedUserCheck1.TabIndex = 43
        '
        'Label18
        '
        Me.Label18.AllowDrop = True
        Me.Label18.BackColor = System.Drawing.Color.White
        Me.Label18.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(107, 348)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(432, 66)
        Me.Label18.TabIndex = 53
        Me.Label18.Text = "Approved Users to SAP_ALL and SAP_NEW Profile Assignment"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.White
        Me.Label13.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(108, 64)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(339, 25)
        Me.Label13.TabIndex = 44
        Me.Label13.Text = "Approved Users to Create User ID"
        '
        'txtITGC10_ApprovedUserCheck4
        '
        Me.txtITGC10_ApprovedUserCheck4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC10_ApprovedUserCheck4.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC10_ApprovedUserCheck4.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC10_ApprovedUserCheck4.ForeColor = System.Drawing.Color.Black
        Me.txtITGC10_ApprovedUserCheck4.Location = New System.Drawing.Point(564, 275)
        Me.txtITGC10_ApprovedUserCheck4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC10_ApprovedUserCheck4.Name = "txtITGC10_ApprovedUserCheck4"
        Me.txtITGC10_ApprovedUserCheck4.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC10_ApprovedUserCheck4.TabIndex = 50
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.White
        Me.Label15.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(106, 130)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(367, 25)
        Me.Label15.TabIndex = 47
        Me.Label15.Text = "Approved Users to Lock/Unlock user"
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.White
        Me.Label16.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(106, 254)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(349, 62)
        Me.Label16.TabIndex = 51
        Me.Label16.Text = "Approved Users to perform role assignments"
        '
        'txtITGC10_ApprovedUserCheck2
        '
        Me.txtITGC10_ApprovedUserCheck2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC10_ApprovedUserCheck2.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC10_ApprovedUserCheck2.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC10_ApprovedUserCheck2.ForeColor = System.Drawing.Color.Black
        Me.txtITGC10_ApprovedUserCheck2.Location = New System.Drawing.Point(564, 135)
        Me.txtITGC10_ApprovedUserCheck2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC10_ApprovedUserCheck2.Name = "txtITGC10_ApprovedUserCheck2"
        Me.txtITGC10_ApprovedUserCheck2.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC10_ApprovedUserCheck2.TabIndex = 46
        '
        'txtITGC10_ApprovedUserCheck3
        '
        Me.txtITGC10_ApprovedUserCheck3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC10_ApprovedUserCheck3.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC10_ApprovedUserCheck3.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC10_ApprovedUserCheck3.ForeColor = System.Drawing.Color.Black
        Me.txtITGC10_ApprovedUserCheck3.Location = New System.Drawing.Point(564, 201)
        Me.txtITGC10_ApprovedUserCheck3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC10_ApprovedUserCheck3.Name = "txtITGC10_ApprovedUserCheck3"
        Me.txtITGC10_ApprovedUserCheck3.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC10_ApprovedUserCheck3.TabIndex = 48
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.White
        Me.Label17.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(107, 196)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(312, 25)
        Me.Label17.TabIndex = 49
        Me.Label17.Text = "Approved Users to modify roles"
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.BackColor = System.Drawing.Color.White
        Me.GroupBox6.Controls.Add(Me.txtITGC11_ApprovedUsers)
        Me.GroupBox6.Controls.Add(Me.Label20)
        Me.GroupBox6.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.ForeColor = System.Drawing.Color.Black
        Me.GroupBox6.Location = New System.Drawing.Point(18, 1406)
        Me.GroupBox6.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox6.Size = New System.Drawing.Size(1113, 136)
        Me.GroupBox6.TabIndex = 79
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "ITGC11 Controls"
        '
        'txtITGC11_ApprovedUsers
        '
        Me.txtITGC11_ApprovedUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC11_ApprovedUsers.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC11_ApprovedUsers.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC11_ApprovedUsers.ForeColor = System.Drawing.Color.Black
        Me.txtITGC11_ApprovedUsers.Location = New System.Drawing.Point(555, 55)
        Me.txtITGC11_ApprovedUsers.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC11_ApprovedUsers.Name = "txtITGC11_ApprovedUsers"
        Me.txtITGC11_ApprovedUsers.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC11_ApprovedUsers.TabIndex = 55
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.Color.White
        Me.Label20.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(101, 69)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(359, 25)
        Me.Label20.TabIndex = 56
        Me.Label20.Text = "Approved Users to Import Transport"
        '
        'GroupBox8
        '
        Me.GroupBox8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox8.BackColor = System.Drawing.Color.White
        Me.GroupBox8.Controls.Add(Me.Label26)
        Me.GroupBox8.Controls.Add(Me.txtITGC13_ApprovedUsers)
        Me.GroupBox8.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox8.ForeColor = System.Drawing.Color.Black
        Me.GroupBox8.Location = New System.Drawing.Point(18, 1707)
        Me.GroupBox8.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox8.Size = New System.Drawing.Size(1113, 129)
        Me.GroupBox8.TabIndex = 79
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "ITGC13 Controls"
        '
        'Label26
        '
        Me.Label26.BackColor = System.Drawing.Color.White
        Me.Label26.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(108, 54)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(363, 56)
        Me.Label26.TabIndex = 64
        Me.Label26.Text = "Approved Users Ability to change password configuration settings"
        '
        'txtITGC13_ApprovedUsers
        '
        Me.txtITGC13_ApprovedUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC13_ApprovedUsers.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC13_ApprovedUsers.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC13_ApprovedUsers.ForeColor = System.Drawing.Color.Black
        Me.txtITGC13_ApprovedUsers.Location = New System.Drawing.Point(554, 54)
        Me.txtITGC13_ApprovedUsers.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC13_ApprovedUsers.Name = "txtITGC13_ApprovedUsers"
        Me.txtITGC13_ApprovedUsers.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC13_ApprovedUsers.TabIndex = 63
        '
        'GroupBox7
        '
        Me.GroupBox7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox7.BackColor = System.Drawing.Color.White
        Me.GroupBox7.Controls.Add(Me.Label23)
        Me.GroupBox7.Controls.Add(Me.txtITGC12_ApprovedUsers)
        Me.GroupBox7.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox7.ForeColor = System.Drawing.Color.Black
        Me.GroupBox7.Location = New System.Drawing.Point(18, 1554)
        Me.GroupBox7.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox7.Size = New System.Drawing.Size(1113, 146)
        Me.GroupBox7.TabIndex = 79
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "ITGC12 Controls"
        '
        'Label23
        '
        Me.Label23.BackColor = System.Drawing.Color.White
        Me.Label23.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(110, 55)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(320, 55)
        Me.Label23.TabIndex = 60
        Me.Label23.Text = "Approved Users to perform Job Administration"
        '
        'txtITGC12_ApprovedUsers
        '
        Me.txtITGC12_ApprovedUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC12_ApprovedUsers.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC12_ApprovedUsers.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC12_ApprovedUsers.ForeColor = System.Drawing.Color.Black
        Me.txtITGC12_ApprovedUsers.Location = New System.Drawing.Point(554, 70)
        Me.txtITGC12_ApprovedUsers.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC12_ApprovedUsers.Name = "txtITGC12_ApprovedUsers"
        Me.txtITGC12_ApprovedUsers.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC12_ApprovedUsers.TabIndex = 59
        '
        'GroupBox9
        '
        Me.GroupBox9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox9.BackColor = System.Drawing.Color.White
        Me.GroupBox9.Controls.Add(Me.txtITGC15_ApprovedUsers)
        Me.GroupBox9.Controls.Add(Me.Label29)
        Me.GroupBox9.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox9.ForeColor = System.Drawing.Color.Black
        Me.GroupBox9.Location = New System.Drawing.Point(18, 1844)
        Me.GroupBox9.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox9.Size = New System.Drawing.Size(1113, 136)
        Me.GroupBox9.TabIndex = 79
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "ITGC15 Controls"
        '
        'txtITGC15_ApprovedUsers
        '
        Me.txtITGC15_ApprovedUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtITGC15_ApprovedUsers.BackColor = System.Drawing.Color.AliceBlue
        Me.txtITGC15_ApprovedUsers.Font = New System.Drawing.Font("Calibri", 12.0!)
        Me.txtITGC15_ApprovedUsers.ForeColor = System.Drawing.Color.Black
        Me.txtITGC15_ApprovedUsers.Location = New System.Drawing.Point(554, 52)
        Me.txtITGC15_ApprovedUsers.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtITGC15_ApprovedUsers.Name = "txtITGC15_ApprovedUsers"
        Me.txtITGC15_ApprovedUsers.Size = New System.Drawing.Size(378, 37)
        Me.txtITGC15_ApprovedUsers.TabIndex = 67
        '
        'Label29
        '
        Me.Label29.BackColor = System.Drawing.Color.White
        Me.Label29.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(100, 51)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(361, 54)
        Me.Label29.TabIndex = 68
        Me.Label29.Text = "Approved Users to maintain CUA landscape and user distribution "
        '
        'GroupBox11
        '
        Me.GroupBox11.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox11.BackColor = System.Drawing.Color.White
        Me.GroupBox11.Controls.Add(Me.txtITGC17_ApprovedUsers)
        Me.GroupBox11.Controls.Add(Me.Label35)
        Me.GroupBox11.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox11.ForeColor = System.Drawing.Color.Black
        Me.GroupBox11.Location = New System.Drawing.Point(18, 2137)
        Me.GroupBox11.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox11.Size = New System.Drawing.Size(1113, 160)
        Me.GroupBox11.TabIndex = 79
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "ITGC17 Controls"
        '
        'Label35
        '
        Me.Label35.BackColor = System.Drawing.Color.White
        Me.Label35.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.ForeColor = System.Drawing.Color.Black
        Me.Label35.Location = New System.Drawing.Point(102, 56)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(326, 59)
        Me.Label35.TabIndex = 76
        Me.Label35.Text = "Approved Users to Modify IDOCs in Production"
        '
        'GroupBox10
        '
        Me.GroupBox10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox10.BackColor = System.Drawing.Color.White
        Me.GroupBox10.Controls.Add(Me.txtITGC16_ProgramName)
        Me.GroupBox10.Controls.Add(Me.Label32)
        Me.GroupBox10.Font = New System.Drawing.Font("Calisto MT", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox10.ForeColor = System.Drawing.Color.Black
        Me.GroupBox10.Location = New System.Drawing.Point(18, 1991)
        Me.GroupBox10.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox10.Size = New System.Drawing.Size(1113, 139)
        Me.GroupBox10.TabIndex = 79
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "ITGC16 Controls"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.BackColor = System.Drawing.Color.White
        Me.Label32.Font = New System.Drawing.Font("Calisto MT", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(100, 69)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(307, 25)
        Me.Label32.TabIndex = 72
        Me.Label32.Text = "SAP Standard Program Names"
        '
        'ctrlGeneral
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.Controls.Add(Me.PanelBody)
        Me.Controls.Add(Me.PanelHeader)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "ctrlGeneral"
        Me.Size = New System.Drawing.Size(1447, 2463)
        Me.PanelHeader.ResumeLayout(False)
        Me.PanelHeader.PerformLayout()
        Me.PanelButton.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelBody.ResumeLayout(False)
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout()
        Me.PanelFooter.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtEndTime As TextBox
    Friend WithEvents lblEndTime As Label
    Friend WithEvents txtStartTime As TextBox
    Friend WithEvents lblStartTime As Label
    Friend WithEvents btnGeneralSave As Button
    Friend WithEvents lblSAPGUIXML As Label
    Friend WithEvents lblITGCControl As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblSAPGUIApp As Label
    Friend WithEvents PanelHeader As Panel
    Friend WithEvents lblHeading As Label
    Friend WithEvents PanelBody As Panel
    Friend WithEvents txtITGC01_OssId As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtITGC04_TcodeList As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtITGC09_ApprovedUsers As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtITGC10_ApprovedUserCheck2 As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents txtITGC10_ApprovedUserCheck1 As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtITGC10_ApprovedUserCheck5 As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents txtITGC10_ApprovedUserCheck4 As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents txtITGC10_ApprovedUserCheck3 As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents txtITGC11_ApprovedUsers As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents txtITGC12_ApprovedUsers As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents txtITGC13_ApprovedUsers As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents txtITGC15_ApprovedUsers As TextBox
    Friend WithEvents Label29 As Label
    Friend WithEvents txtITGC16_ProgramName As TextBox
    Friend WithEvents Label32 As Label
    Friend WithEvents txtITGC17_ApprovedUsers As TextBox
    Friend WithEvents Label35 As Label
    Friend WithEvents PanelFooter As Panel
    Friend WithEvents lblFooter As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBox11 As GroupBox
    Friend WithEvents GroupBox10 As GroupBox
    Friend WithEvents GroupBox9 As GroupBox
    Friend WithEvents GroupBox8 As GroupBox
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents btnRevertDefaults As Button
    Friend WithEvents PanelButton As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents lblDownloadDestination As Label
    Friend WithEvents GroupBox12 As GroupBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtITGC08_ApprovedUsers As TextBox
End Class
