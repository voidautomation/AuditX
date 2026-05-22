<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")>
Partial Class LoginForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoginForm))
        Me.lblLoginStatus = New System.Windows.Forms.Label()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.lblEmailError = New System.Windows.Forms.Label()
        Me.UsernameLabel = New System.Windows.Forms.Label()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.PasswordLabel = New System.Windows.Forms.Label()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnUserLogin = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnForgotPassword = New System.Windows.Forms.LinkLabel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
        Me.pbUserLogin = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbUserLogin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblLoginStatus
        '
        Me.lblLoginStatus.AutoSize = True
        Me.lblLoginStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblLoginStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoginStatus.ForeColor = System.Drawing.Color.White
        Me.lblLoginStatus.Location = New System.Drawing.Point(16, 238)
        Me.lblLoginStatus.Name = "lblLoginStatus"
        Me.lblLoginStatus.Size = New System.Drawing.Size(61, 22)
        Me.lblLoginStatus.TabIndex = 15
        Me.lblLoginStatus.Text = "Status"
        '
        'txtEmail
        '
        Me.txtEmail.AutoCompleteCustomSource.AddRange(New String() {""})
        Me.txtEmail.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.txtEmail.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList
        Me.txtEmail.BackColor = System.Drawing.Color.White
        Me.txtEmail.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmail.ForeColor = System.Drawing.Color.Black
        Me.txtEmail.Location = New System.Drawing.Point(18, 89)
        Me.txtEmail.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(346, 35)
        Me.txtEmail.TabIndex = 9
        '
        'lblEmailError
        '
        Me.lblEmailError.AutoSize = True
        Me.lblEmailError.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmailError.ForeColor = System.Drawing.Color.White
        Me.lblEmailError.Location = New System.Drawing.Point(15, 128)
        Me.lblEmailError.Name = "lblEmailError"
        Me.lblEmailError.Size = New System.Drawing.Size(50, 22)
        Me.lblEmailError.TabIndex = 14
        Me.lblEmailError.Text = "Error"
        '
        'UsernameLabel
        '
        Me.UsernameLabel.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UsernameLabel.ForeColor = System.Drawing.Color.Cyan
        Me.UsernameLabel.Location = New System.Drawing.Point(14, 46)
        Me.UsernameLabel.Name = "UsernameLabel"
        Me.UsernameLabel.Size = New System.Drawing.Size(106, 39)
        Me.UsernameLabel.TabIndex = 8
        Me.UsernameLabel.Text = "Email"
        Me.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Cancel
        '
        Me.Cancel.BackColor = System.Drawing.Color.LightCoral
        Me.Cancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel.ForeColor = System.Drawing.Color.White
        Me.Cancel.Location = New System.Drawing.Point(1031, 118)
        Me.Cancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(177, 58)
        Me.Cancel.TabIndex = 13
        Me.Cancel.Text = "&Cancel"
        Me.Cancel.UseVisualStyleBackColor = False
        '
        'PasswordLabel
        '
        Me.PasswordLabel.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PasswordLabel.ForeColor = System.Drawing.Color.Cyan
        Me.PasswordLabel.Location = New System.Drawing.Point(15, 156)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New System.Drawing.Size(116, 39)
        Me.PasswordLabel.TabIndex = 10
        Me.PasswordLabel.Text = "&Password"
        Me.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnLogin
        '
        Me.btnLogin.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLogin.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogin.ForeColor = System.Drawing.Color.White
        Me.btnLogin.Location = New System.Drawing.Point(188, 341)
        Me.btnLogin.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(177, 58)
        Me.btnLogin.TabIndex = 12
        Me.btnLogin.Text = "&Login"
        Me.btnLogin.UseVisualStyleBackColor = False
        '
        'txtPassword
        '
        Me.txtPassword.BackColor = System.Drawing.Color.White
        Me.txtPassword.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.ForeColor = System.Drawing.Color.Black
        Me.txtPassword.Location = New System.Drawing.Point(19, 199)
        Me.txtPassword.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(346, 35)
        Me.txtPassword.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calisto MT", 31.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(226, 74)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(582, 74)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Welcome to AuditX"
        '
        'btnUserLogin
        '
        Me.btnUserLogin.BackColor = System.Drawing.Color.LightSkyBlue
        Me.btnUserLogin.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUserLogin.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUserLogin.ForeColor = System.Drawing.Color.White
        Me.btnUserLogin.Location = New System.Drawing.Point(592, 341)
        Me.btnUserLogin.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnUserLogin.Name = "btnUserLogin"
        Me.btnUserLogin.Size = New System.Drawing.Size(177, 58)
        Me.btnUserLogin.TabIndex = 19
        Me.btnUserLogin.Text = "&Login"
        Me.btnUserLogin.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(42, 28)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(158, 148)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 18
        Me.PictureBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Black
        Me.Label2.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Cyan
        Me.Label2.Location = New System.Drawing.Point(6, -9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(152, 39)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "User Login"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Black
        Me.Label3.Font = New System.Drawing.Font("Calisto MT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Cyan
        Me.Label3.Location = New System.Drawing.Point(15, -9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(152, 39)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Admin Login"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnLogin)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.btnForgotPassword)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtPassword)
        Me.GroupBox1.Controls.Add(Me.lblLoginStatus)
        Me.GroupBox1.Controls.Add(Me.txtEmail)
        Me.GroupBox1.Controls.Add(Me.UsernameLabel)
        Me.GroupBox1.Controls.Add(Me.lblEmailError)
        Me.GroupBox1.Controls.Add(Me.PasswordLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(42, 203)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(391, 439)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Calisto MT", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(15, 311)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(352, 43)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "(Please check your spam folder in case email not recieved in inbox)"
        '
        'btnForgotPassword
        '
        Me.btnForgotPassword.AutoSize = True
        Me.btnForgotPassword.BackColor = System.Drawing.Color.Transparent
        Me.btnForgotPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForgotPassword.LinkColor = System.Drawing.Color.Coral
        Me.btnForgotPassword.Location = New System.Drawing.Point(14, 286)
        Me.btnForgotPassword.Name = "btnForgotPassword"
        Me.btnForgotPassword.Size = New System.Drawing.Size(159, 25)
        Me.btnForgotPassword.TabIndex = 24
        Me.btnForgotPassword.TabStop = True
        Me.btnForgotPassword.Text = "Forgot Password"
        Me.btnForgotPassword.VisitedLinkColor = System.Drawing.Color.Red
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnUserLogin)
        Me.GroupBox2.Controls.Add(Me.pbUserLogin)
        Me.GroupBox2.Controls.Add(Me.LogoPictureBox)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox2.Location = New System.Drawing.Point(439, 203)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(821, 439)
        Me.GroupBox2.TabIndex = 25
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "GroupBox2"
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.Image = CType(resources.GetObject("LogoPictureBox.Image"), System.Drawing.Image)
        Me.LogoPictureBox.Location = New System.Drawing.Point(6, 34)
        Me.LogoPictureBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(390, 387)
        Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.LogoPictureBox.TabIndex = 0
        Me.LogoPictureBox.TabStop = False
        '
        'pbUserLogin
        '
        Me.pbUserLogin.Image = CType(resources.GetObject("pbUserLogin.Image"), System.Drawing.Image)
        Me.pbUserLogin.Location = New System.Drawing.Point(388, 34)
        Me.pbUserLogin.Name = "pbUserLogin"
        Me.pbUserLogin.Size = New System.Drawing.Size(427, 309)
        Me.pbUserLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbUserLogin.TabIndex = 21
        Me.pbUserLogin.TabStop = False
        '
        'LoginForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(1287, 689)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.GroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginForm"
        Me.Opacity = 0.95R
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login to ISAAF"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbUserLogin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblLoginStatus As Label
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents lblEmailError As Label
    Friend WithEvents UsernameLabel As Label
    Friend WithEvents Cancel As Button
    Friend WithEvents PasswordLabel As Label
    Friend WithEvents btnLogin As Button
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnUserLogin As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btnForgotPassword As LinkLabel
    Friend WithEvents Label4 As Label
    Friend WithEvents pbUserLogin As PictureBox
    Friend WithEvents LogoPictureBox As PictureBox
End Class
