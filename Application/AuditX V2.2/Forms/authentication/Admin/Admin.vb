Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Word
Imports Newtonsoft.Json



Public Class Admin

    Private profilePopup As Panel
    Private lblPopupName As Label
    Private lblPopupEmail As Label
    Private lblPopupCompany As Label

    Public Property UserName As String
    Public Property UserEmail As String
    Public Property UserCompany As String
    Private manageUser As Boolean = False

    Private firebaseURL As String =
            "https://sap-itgc-audit-default-rtdb.asia-southeast1.firebasedatabase.app/licensed_users/"

    Private encryptionKey As String = "InfinityVoid_Enterprise_Key_2026"

    ' --- Windows API Interop to change Progress Bar colors ---
    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=False)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

    Private Const PBM_SETSTATE As Integer = 1040
    Private Const PBST_NORMAL As Integer = 1 ' Green
    Private Const PBST_ERROR As Integer = 2  ' Red
    Private Const PBST_PAUSED As Integer = 3 ' Yellow
    ' --- Placeholder Text Constants ---
    Private Const PH_WIN_ID As String = "Enter Windows ID"
    Private Const PH_NAME As String = "Enter Full Name"
    Private Const PH_EMAIL As String = "Enter Email Address"
    Private Const PH_PATH As String = "Select Folder"
#Region "Form Load"

    Private Sub Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        CreateProfilePopup()
        RegisterGlobalClickHandler(Me)
        'Header Gradient
        GradientHelper.ApplyGradient(PanelHeader,
                                         ColorTranslator.FromHtml("#1F2A44"),
                                         ColorTranslator.FromHtml("#E3F2FD"),
                                         Drawing2D.LinearGradientMode.Horizontal)

        'Body Gradient
        GradientHelper.ApplyGradient(GroupBox1,
                                         ColorTranslator.FromHtml("#FFFFFF"),
                                         ColorTranslator.FromHtml("#F5F7FA"),
                                         Drawing2D.LinearGradientMode.Horizontal)


        ' 2. Initialize all Placeholders on Load (as you requested!)
        SetPlaceholder(txtWindowsID, PH_WIN_ID)
        SetPlaceholder(txtFullName, PH_NAME)
        SetPlaceholder(txtEmail, PH_EMAIL)
        SetPlaceholder(txtPath, PH_PATH)
        ' Set the maximum value of the progress bar to 100%
        pbProgressLine.Maximum = 100
        ' Optional UX improvement: Prevent the user from picking a past date in the calendar UI
        dtpExpiry.MinDate = DateTime.Now.AddDays(1)
        ' Call the update method to set the initial state (0%)
        UpdateProgress()
    End Sub

#End Region

    ' Consolidated closing/logout logic into FormClosing.
    Private Sub Admin_Closing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim msg As String = "Choose an action:" & vbCrLf +
                                "Yes = Logout and return to Login" & vbCrLf +
                                "No  = Exit application" & vbCrLf +
                                "Cancel = Stay in application"
        Dim result As DialogResult = MessageBox.Show(msg,
                                                         "Confirm",
                                                         MessageBoxButtons.YesNoCancel,
                                                         MessageBoxIcon.Question,
                                                         MessageBoxDefaultButton.Button1)

        Select Case result
            Case DialogResult.Cancel
                e.Cancel = True
            Case DialogResult.Yes
                ' Logout: show LoginForm (if present) and allow the form to close
                For Each frm As Form In System.Windows.Forms.Application.OpenForms
                    If TypeOf frm Is LoginForm Then
                        frm.Show()
                        frm.Activate()
                        Exit For
                    End If
                Next
                    ' allow close to proceed
            Case DialogResult.No
                ' Exit application: allow close, do not show LoginForm
            Case Else
                e.Cancel = True
        End Select
    End Sub

    ' ==========================================
    ' REAL-TIME VALIDATION & PROGRESS UPDATES
    ' ==========================================
    Private Sub Input_Changed(sender As Object, e As EventArgs) Handles _
        txtWindowsID.TextChanged, txtFullName.TextChanged, txtEmail.TextChanged, txtPath.TextChanged, dtpExpiry.ValueChanged

        UpdateProgress()
    End Sub
    ' The main logic to validate fields, calculate progress, and update UI
    Private Sub UpdateProgress()
        Dim validCount As Integer = 0
        Dim totalFields As Integer = 5
        Dim statusMessage As String = ""

        ' 1. Validate Windows ID
        If Not String.IsNullOrWhiteSpace(txtWindowsID.Text) AndAlso txtWindowsID.Text <> PH_WIN_ID Then
            validCount += 1
            txtWindowsID.BackColor = Color.Honeydew ' Soft Green
        Else
            txtWindowsID.BackColor = If(txtWindowsID.Text = PH_WIN_ID, Color.White, Color.MistyRose)
            If statusMessage = "" Then statusMessage = "Please enter a Windows ID."
        End If

        ' 2. Validate Full Name
        If Not String.IsNullOrWhiteSpace(txtFullName.Text) AndAlso txtFullName.Text <> PH_NAME Then
            validCount += 1
            txtFullName.BackColor = Color.Honeydew
        Else
            txtFullName.BackColor = If(txtFullName.Text = PH_NAME, Color.White, Color.MistyRose)
            If statusMessage = "" Then statusMessage = "Please enter a Full Name."
        End If

        ' 3. Validate Email
        Dim emailPattern As String = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
        If txtEmail.Text <> PH_EMAIL AndAlso Regex.IsMatch(txtEmail.Text, emailPattern) Then
            validCount += 1
            txtEmail.BackColor = Color.Honeydew
        Else
            txtEmail.BackColor = If(txtEmail.Text = PH_EMAIL, Color.White, Color.MistyRose)
            If txtEmail.Text <> PH_EMAIL AndAlso statusMessage = "" Then
                statusMessage = "Please enter a valid Email Address."
            ElseIf statusMessage = "" Then
                statusMessage = "Please enter an Email Address."
            End If
        End If

        ' 4. Validate Expiry Date
        If dtpExpiry.Value.Date > DateTime.Now.Date Then
            validCount += 1
        Else
            If statusMessage = "" Then statusMessage = "Expiry date must be in the future."
        End If

        ' 5. Validate Path
        If Not String.IsNullOrWhiteSpace(txtPath.Text) AndAlso txtPath.Text <> PH_PATH Then
            validCount += 1
            txtPath.BackColor = Color.Honeydew
        Else
            txtPath.BackColor = If(txtPath.Text = PH_PATH, Color.White, Color.MistyRose)
            If statusMessage = "" Then statusMessage = "Please select a save location."
        End If

        ' --- UPDATE UI (Progress Bar & Status Label) ---
        Dim progressPercentage As Integer = CInt((validCount / totalFields) * 100)
        pbProgressLine.Value = progressPercentage

        If validCount = totalFields Then
            ' 100% - Green
            SendMessage(pbProgressLine.Handle, PBM_SETSTATE, PBST_NORMAL, 0)
            lblStatus.Text = "Status: All Set! Ready to generate license."
            lblStatus.ForeColor = Color.MediumSeaGreen
            btnGenerate.Enabled = True
        ElseIf validCount >= 3 Then
            ' 60% to 80% - Yellow
            SendMessage(pbProgressLine.Handle, PBM_SETSTATE, PBST_PAUSED, 0)
            lblStatus.Text = $"In progress... {statusMessage}"
            lblStatus.ForeColor = Color.DarkOrange
            btnGenerate.Enabled = False
        Else
            ' 0% to 40% - Red
            SendMessage(pbProgressLine.Handle, PBM_SETSTATE, PBST_ERROR, 0)
            If validCount = 0 Then
                lblStatus.Text = "Awaiting user input..."
                lblStatus.ForeColor = Color.Gray
            Else
                lblStatus.Text = $"Getting started... {statusMessage}"
                lblStatus.ForeColor = Color.Crimson
            End If
            btnGenerate.Enabled = False
        End If
    End Sub

    ' ==========================================
    ' PLACEHOLDER LOGIC (Enter / Leave Events)
    ' ==========================================
    Private Sub SetPlaceholder(txt As TextBox, placeholder As String)
        If String.IsNullOrWhiteSpace(txt.Text) OrElse txt.Text = placeholder Then
            txt.Text = placeholder
            txt.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub RemovePlaceholder(txt As TextBox, placeholder As String)
        If txt.Text = placeholder Then
            txt.Text = ""
            txt.ForeColor = Color.Black
        End If
    End Sub

    Private Sub txtWindowsID_Enter(sender As Object, e As EventArgs) Handles txtWindowsID.Enter
        RemovePlaceholder(txtWindowsID, PH_WIN_ID)
    End Sub
    Private Sub txtWindowsID_Leave(sender As Object, e As EventArgs) Handles txtWindowsID.Leave
        SetPlaceholder(txtWindowsID, PH_WIN_ID)
        UpdateProgress() ' Force re-validation when leaving
    End Sub

    Private Sub txtFullName_Enter(sender As Object, e As EventArgs) Handles txtFullName.Enter
        RemovePlaceholder(txtFullName, PH_NAME)
    End Sub
    Private Sub txtFullName_Leave(sender As Object, e As EventArgs) Handles txtFullName.Leave
        SetPlaceholder(txtFullName, PH_NAME)
        UpdateProgress()
    End Sub

    Private Sub txtEmail_Enter(sender As Object, e As EventArgs) Handles txtEmail.Enter
        RemovePlaceholder(txtEmail, PH_EMAIL)
    End Sub
    Private Sub txtEmail_Leave(sender As Object, e As EventArgs) Handles txtEmail.Leave
        SetPlaceholder(txtEmail, PH_EMAIL)
        UpdateProgress()
    End Sub

#Region "Generate CID"

    Private Function GenerateCID() As String

        Dim rnd As New Random()

        Dim number As Integer = rnd.Next(100000, 999999)

        Return "C0" & number.ToString()

    End Function

#End Region


#Region "Get Windows ID"

    Private Function GetWindowsID() As String
        Return Environment.UserDomainName & "\" & Environment.UserName
    End Function

#End Region


#Region "Browse Path"

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click

        Using fbd As New FolderBrowserDialog()
            fbd.Description = "Select a folder to save the license file"
            If fbd.ShowDialog() = DialogResult.OK Then
                txtPath.Text = fbd.SelectedPath
                txtPath.ForeColor = Color.Black ' Ensure it's not grey placeholder text
                UpdateProgress()
            End If
        End Using

    End Sub

#End Region


#Region "Create User"

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

        Try
            Dim userName = txtWindowsID.Text.Trim()
            Dim fullName = If(txtFullName IsNot Nothing, txtFullName.Text.Trim(), String.Empty)
            Dim email = If(txtEmail IsNot Nothing, txtEmail.Text.Trim(), String.Empty)


            If userName = "" Then
                MessageBox.Show("Enter Windows Username", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If fullName = "" Then
                MessageBox.Show("Enter Full Name", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If email = "" OrElse Not email.Contains("@") OrElse Not email.Contains(".") Then
                MessageBox.Show("Enter a valid Email address", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If dtpExpiry.Value <= DateTime.Today Then
                MessageBox.Show("Expiry date must be in future.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            Dim cid As String = GenerateCID()

            Dim user = New Dictionary(Of String, Object)

            user("cid") = cid
            user("windowsId") = txtWindowsID.Text
            user("fullName") = txtFullName.Text
            user("email") = txtEmail.Text
            user("expiry") = dtpExpiry.Value.ToString("yyyy-MM-dd")
            user("created") = DateTime.Now.ToString("yyyy-MM-dd")

            Dim json As String = JsonConvert.SerializeObject(user)

            Dim request As WebRequest = WebRequest.Create(firebaseURL & cid & ".json")
            request.Method = "PUT"

            Dim bytes = Encoding.UTF8.GetBytes(json)

            request.ContentLength = bytes.Length

            Dim stream = request.GetRequestStream()
            stream.Write(bytes, 0, bytes.Length)
            stream.Close()

            GenerateLicense(cid)

            MessageBox.Show("User created successfully." & vbCrLf & "License stored at: " & txtPath.Text)

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

#End Region


#Region "Generate License"

    Private Sub GenerateLicense(cid As String)

        Dim windowsID = txtWindowsID.Text
        Dim fullname = txtFullName.Text
        Dim email = txtEmail.Text

        Dim issueDate = DateTime.Now
        Dim expiryDate = dtpExpiry.Value

        Dim raw = cid &
                      windowsID &
                      expiryDate.ToString("yyyyMMdd") &
                      fullname &
                      email &
                      encryptionKey

        Dim signature = ComputeHash(raw)

        Dim data = String.Join("|", {
                cid,
                windowsID,
                fullname,
                email,
                issueDate.ToString("yyyyMMdd"),
                expiryDate.ToString("yyyyMMdd"),
                signature
            })

        Dim encrypted = Encrypt(data)

        Dim path As String = txtPath.Text & "\License.cert"

        File.WriteAllText(path, encrypted)

    End Sub

#End Region

#Region "Encryption"

    Private Function Encrypt(clearText As String) As String

        Dim aes As Aes = Aes.Create()

        Dim pdb As New Rfc2898DeriveBytes(encryptionKey,
                                              Encoding.UTF8.GetBytes("AuditX_Salt"))

        aes.Key = pdb.GetBytes(32)
        aes.IV = pdb.GetBytes(16)

        Using ms As New MemoryStream()

            Using cs As New CryptoStream(ms,
                                             aes.CreateEncryptor(),
                                             CryptoStreamMode.Write)

                Using sw As New StreamWriter(cs)
                    sw.Write(clearText)
                End Using

            End Using

            Return Convert.ToBase64String(ms.ToArray())

        End Using

    End Function

#End Region


#Region "Hash"

    Private Function ComputeHash(text As String) As String

        Using sha As SHA256 = SHA256.Create()

            Dim bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text))

            Return Convert.ToBase64String(bytes)

        End Using

    End Function

#End Region


#Region "Profile Popup"
    Private Sub CreateProfilePopup()

        profilePopup = New Panel()
        profilePopup.Size = New System.Drawing.Size(350, 350)
        profilePopup.BackColor = Color.White
        profilePopup.Visible = False
        profilePopup.BorderStyle = BorderStyle.None
        profilePopup.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        ' Position relative to pbProfile; ensure pbProfile exists on form
        profilePopup.Location = New System.Drawing.Point(pbProfile.Right - 350, pbProfile.Bottom + 8)

        ' ===== Rounded Card =====
        Dim path As New System.Drawing.Drawing2D.GraphicsPath()
        path.AddArc(0, 0, 20, 20, 180, 90)
        path.AddArc(profilePopup.Width - 20, 0, 20, 20, 270, 90)
        path.AddArc(profilePopup.Width - 20, profilePopup.Height - 20, 20, 20, 0, 90)
        path.AddArc(0, profilePopup.Height - 20, 20, 20, 90, 90)
        path.CloseFigure()
        profilePopup.Region = New System.Drawing.Region(path)

        ' ===== Header Background =====
        Dim headerPanel As New Panel()
        headerPanel.BackColor = Color.FromArgb(210, 220, 235)
        headerPanel.Dock = DockStyle.Fill

        ' ===== Profile Image =====
        Dim profileImage As New PictureBox()
        profileImage.Size = New System.Drawing.Size(80, 80)
        profileImage.SizeMode = PictureBoxSizeMode.Zoom
        profileImage.Location = New System.Drawing.Point((profilePopup.Width - 80) \ 2, 20)

        Try
            profileImage.Image = pbProfile.Image
        Catch
            profileImage.BackColor = Color.LightGray
        End Try

        ' Make circular
        Dim imgPath As New System.Drawing.Drawing2D.GraphicsPath()
        imgPath.AddEllipse(0, 0, profileImage.Width, profileImage.Height)
        profileImage.Region = New System.Drawing.Region(imgPath)


        ' ===== Name Label =====
        lblPopupName = New Label()
        lblPopupName.Text = UserName
        lblPopupName.Font = New System.Drawing.Font("Segoe UI", 12.0F, System.Drawing.FontStyle.Bold)
        lblPopupName.TextAlign = ContentAlignment.MiddleCenter
        lblPopupName.AutoSize = False
        lblPopupName.Size = New System.Drawing.Size(profilePopup.Width, 50)
        lblPopupName.Location = New System.Drawing.Point(0, 110)

        ' ===== Email Label =====
        lblPopupEmail = New Label()
        lblPopupEmail.Text = UserEmail
        lblPopupEmail.Font = New System.Drawing.Font("Segoe UI", 10.0F, System.Drawing.FontStyle.Regular)
        lblPopupEmail.ForeColor = Color.FromArgb(80, 80, 80)
        lblPopupEmail.TextAlign = ContentAlignment.MiddleCenter
        lblPopupEmail.AutoSize = False
        lblPopupEmail.Size = New System.Drawing.Size(profilePopup.Width - 20, 60)
        lblPopupEmail.Location = New System.Drawing.Point(10, lblPopupName.Bottom + 5)

        ' ==============================
        ' SEPARATOR (NOW VISIBLE)
        ' ==============================
        Dim separator As New Panel()
        separator.BackColor = Color.FromArgb(0, 0, 0)
        separator.Size = New System.Drawing.Size(profilePopup.Width - 40, 1)
        separator.Location = New System.Drawing.Point(20, lblPopupEmail.Bottom + 15)
        headerPanel.Controls.Add(separator)

        ' ===== Company Label =====
        lblPopupCompany = New Label()
        lblPopupCompany.Text = UserCompany
        lblPopupCompany.Font = New System.Drawing.Font("Segoe UI", 10.0F, System.Drawing.FontStyle.Regular)
        lblPopupCompany.ForeColor = Color.FromArgb(80, 80, 80)
        lblPopupCompany.TextAlign = ContentAlignment.MiddleCenter
        lblPopupCompany.AutoSize = False
        lblPopupCompany.Size = New System.Drawing.Size(profilePopup.Width, 30)
        lblPopupCompany.Location = New System.Drawing.Point(10, separator.Bottom + 10)


        headerPanel.Controls.Add(profileImage)
        headerPanel.Controls.Add(lblPopupName)
        headerPanel.Controls.Add(lblPopupEmail)
        headerPanel.Controls.Add(lblPopupCompany)

        profilePopup.Controls.Add(headerPanel)

        Me.Controls.Add(profilePopup)

    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)

        If profilePopup IsNot Nothing AndAlso profilePopup.Visible Then

            Dim cursorPos = Me.PointToClient(Cursor.Position)

            If Not profilePopup.Bounds.Contains(cursorPos) AndAlso
               Not pbProfile.Bounds.Contains(cursorPos) Then

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
        If profilePopup.Bounds.Contains(Me.PointToClient(Cursor.Position)) Then
            Exit Sub
        End If

        ' If clicking profile picture → do nothing (it will open)
        If clickedControl Is pbProfile Then
            Exit Sub
        End If

        ' Otherwise hide popup
        profilePopup.Visible = False

    End Sub

    Private Sub pbProfile_Click(sender As Object, e As EventArgs) Handles pbProfile.Click
        profilePopup.Visible = Not profilePopup.Visible
        profilePopup.BringToFront()

    End Sub

#End Region



End Class