Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Web
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Word


Module ITGC_AUDIT

    ' Declare global SAP objects
    Public SapGuiAuto As Object
    Public SAPApp As Object
    Public SAPCon As Object
    Public session As Object
    Public WordApp As Object
    Public WordDoc As Object


    ' Windows API Declarations for capturing specific window

    <DllImport("user32.dll")>
    Private Function GetDC(hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("gdi32.dll", SetLastError:=True)>
    Private Function StretchBlt(hdcDest As IntPtr,
                           nXDest As Integer,
                           nYDest As Integer,
                           nDestWidth As Integer,
                           nDestHeight As Integer,
                           hdcSrc As IntPtr,
                           nXSrc As Integer,
                           nYSrc As Integer,
                           nSrcWidth As Integer,
                           nSrcHeight As Integer,
                           dwRop As Integer) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Sub keybd_event(
        ByVal bVk As Byte,
        ByVal bScan As Byte,
        ByVal dwFlags As UInteger,
        ByVal dwExtraInfo As UIntPtr)

    End Sub

    ' Constants for ShowWindow
    Public Const SW_MAXIMIZE As Integer = 3
    Private Const VK_SNAPSHOT As Byte = &H2C
    Private Const KEYEVENTF_KEYUP As Integer = &H2



    '**********************************************************************************************************************

    ' ========= USER32 / GDI32 / DWM API =========
    <DllImport("user32.dll", SetLastError:=True)>
    Private Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Function GetWindowRect(hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Function UpdateWindow(hWnd As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Function IsIconic(hWnd As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Function GetWindowDC(hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As Integer
    End Function

    <DllImport("gdi32.dll", SetLastError:=True)>
    Private Function BitBlt(hdcDest As IntPtr, nXDest As Integer, nYDest As Integer,
                            nWidth As Integer, nHeight As Integer,
                            hdcSrc As IntPtr, nXSrc As Integer, nYSrc As Integer,
                            dwRop As UInteger) As Boolean
    End Function



    <DllImport("gdi32.dll")>
    Private Function CreateCompatibleDC(hdc As IntPtr) As IntPtr
    End Function

    <DllImport("gdi32.dll")>
    Private Function CreateCompatibleBitmap(hdc As IntPtr, nWidth As Integer, nHeight As Integer) As IntPtr
    End Function

    <DllImport("gdi32.dll")>
    Private Function SelectObject(hdc As IntPtr, h As IntPtr) As IntPtr
    End Function

    <DllImport("gdi32.dll")>
    Private Function DeleteObject(hObject As IntPtr) As Boolean
    End Function

    <DllImport("gdi32.dll")>
    Private Function DeleteDC(hdc As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Function PrintWindow(hwnd As IntPtr, hdcBlt As IntPtr, nFlags As UInteger) As Boolean
    End Function

    <DllImport("dwmapi.dll")>
    Private Function DwmGetWindowAttribute(hWnd As IntPtr, dwAttribute As Integer,
                                                  ByRef pvAttribute As RECT, cbAttribute As Integer) As Integer
    End Function

    ' ========= Structures & Constants =========
    <StructLayout(LayoutKind.Sequential)>
    Public Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    Private Const SRCCOPY As UInteger = &HCC0020UI
    Private Const SW_RESTORE As Integer = 9
    Private Const PW_RENDERFULLCONTENT As UInteger = &H2UI
    Private Const DWMWA_EXTENDED_FRAME_BOUNDS As Integer = 9

    ' ========= DPI Awareness =========
    <DllImport("user32.dll")>
    Private Function SetProcessDPIAware() As Boolean
    End Function

    <DllImport("shcore.dll")>
    Private Function SetProcessDpiAwareness(value As Integer) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Function SetProcessDpiAwarenessContext(value As IntPtr) As Boolean
    End Function

    Private ReadOnly DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 As New IntPtr(-4)

    Private dpiSet As Boolean = False

    Public Sub EnsureDpiAware()
        If dpiSet Then Exit Sub
        Try
            If Not SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2) Then
                Try : SetProcessDpiAwareness(2) : Catch : End Try
                Try : SetProcessDPIAware() : Catch : End Try
            End If
        Catch
        End Try
        dpiSet = True
    End Sub

    ' ========= Helper: Get Correct Bounds =========
    Private Function GetWindowBoundsPixels(hWnd As IntPtr) As RECT
        Dim r As RECT
        Dim hr As Integer = DwmGetWindowAttribute(hWnd, DWMWA_EXTENDED_FRAME_BOUNDS, r, Marshal.SizeOf(GetType(RECT)))
        If hr <> 0 Then
            GetWindowRect(hWnd, r)
        End If
        Return r
    End Function

    ' ========= Helper: Capture Window in Background =========
    Private Function CaptureWindowBackground(hWnd As IntPtr) As Bitmap
        EnsureDpiAware()

        ' If minimized, restore so PrintWindow works
        If IsIconic(hWnd) Then
            ShowWindow(hWnd, SW_RESTORE)
            UpdateWindow(hWnd)
            Threading.Thread.Sleep(50)
        End If

        Dim r As RECT = GetWindowBoundsPixels(hWnd)
        Dim w As Integer = Math.Max(1, r.Right - r.Left)
        Dim h As Integer = Math.Max(1, r.Bottom - r.Top)

        Dim hdcWin As IntPtr = GetWindowDC(hWnd)
        If hdcWin = IntPtr.Zero Then Return Nothing

        Dim hdcMem As IntPtr = IntPtr.Zero
        Dim hBmp As IntPtr = IntPtr.Zero
        Dim hOld As IntPtr = IntPtr.Zero

        Try
            hdcMem = CreateCompatibleDC(hdcWin)
            If hdcMem = IntPtr.Zero Then Return Nothing

            hBmp = CreateCompatibleBitmap(hdcWin, w, h)
            If hBmp = IntPtr.Zero Then Return Nothing

            hOld = SelectObject(hdcMem, hBmp)

            ' Try PrintWindow with full content
            Dim ok As Boolean = PrintWindow(hWnd, hdcMem, PW_RENDERFULLCONTENT)
            If Not ok Then
                ok = PrintWindow(hWnd, hdcMem, 0UI)
            End If
            If Not ok Then
                BitBlt(hdcMem, 0, 0, w, h, hdcWin, 0, 0, SRCCOPY)
            End If

            Dim managed As Bitmap = Image.FromHbitmap(hBmp)
            Dim finalBmp As New Bitmap(managed.Width, managed.Height, Imaging.PixelFormat.Format32bppArgb)
            Using g As Graphics = Graphics.FromImage(finalBmp)
                g.DrawImageUnscaled(managed, 0, 0)
            End Using
            managed.Dispose()
            Return finalBmp
        Finally
            If hOld <> IntPtr.Zero Then SelectObject(hdcMem, hOld)
            If hBmp <> IntPtr.Zero Then DeleteObject(hBmp)
            If hdcMem <> IntPtr.Zero Then DeleteDC(hdcMem)
            If hdcWin <> IntPtr.Zero Then ReleaseDC(hWnd, hdcWin)
        End Try
    End Function

    '***********************************************************************************************************************

    ' Main execution function

    Sub ExecuteSAPScripts()
        ' Get control ID from ComboBox
        Dim controlID As String = Home.cmbControl.SelectedItem?.ToString().Split("-"c)(0).Trim()
        Dim executionMode As String = Home.cmbExecutionMode.SelectedItem?.ToString()
        Logger.InitializeLog(controlID)
        Try

            If String.IsNullOrEmpty(controlID) Then
                MessageBox.Show("Please select a valid Control ID from the dropdown.", "Control ID Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Home.btnExecute.Enabled = True
                Exit Sub
            End If


            Dim executionType As String = Home.cmbExecutionType.SelectedItem?.ToString()
            Logger.InitializeLog(executionType)

            Try
                If executionType = "All Controls" Then
                    ' Loop through all active controls safely
                    For Each itemObj As Object In Home.cmbControl.Items
                        Try
                            If itemObj Is Nothing Then
                                Continue For
                            End If

                            Dim itemText As String = itemObj.ToString()
                            If String.IsNullOrWhiteSpace(itemText) Then
                                Continue For
                            End If

                            ' Extract control ID (text before hyphen) and normalize
                            Dim ctrlId As String = itemText.Split("-"c)(0).Trim().ToUpper()

                            ' Update the dropdown so UI + dependent fields change (select the original item)
                            Home.cmbControl.SelectedItem = itemObj

                            Logger.LogMessage($"Starting execution for control: {itemText}", True)

                            ' Execute the control using the normalized ID
                            Try
                                ExecuteSingleControl(ctrlId)
                            Catch ex As Exception
                                ' Log but continue with next control
                                Logger.LogMessage($"Execution failed for control {ctrlId}: {ex.Message}", False)
                                Logger.LogException(ex, $"ExecuteSingleControl failed for {ctrlId}")
                            End Try

                        Catch ex As Exception
                            ' Defensive: log and continue
                            Logger.LogMessage($"Skipping control due to error: {ex.Message}", False)
                            Logger.LogException(ex, "Error iterating controls in All Controls loop")
                            Continue For
                        End Try
                    Next

                    ' After all controls executed
                    MessageBox.Show("All controls have been executed successfully.", "Execution Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ' Run only the selected control
                    Dim singleControl As String = Home.cmbControl.SelectedItem?.ToString()
                    singleControl = If(singleControl IsNot Nothing, singleControl.Split("-"c)(0).Trim(), "")
                    If Not String.IsNullOrEmpty(singleControl) Then
                        ExecuteSingleControl(singleControl)
                        Logger.LogMessage($"Execution completed for control: {singleControl}", True)
                    Else
                        MessageBox.Show("Please select a valid Control ID.", "Missing Control", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End If

                Home.lblNotification.Text = ""
            Catch ex As Exception
                Logger.LogMessage($"Execution failed: {ex.Message}", False)
                Logger.LogException(ex, "Error in ExecuteSAPScripts")
            End Try
            Home.btnExecute.Enabled = True
            Home.lblNotification.Text = ""


        Catch ex As Exception
            'MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Logger.LogMessage($"An unexpected error occurred: {ex.Message}", False)
            Logger.LogException(ex, "An unexpected error occurred:")
            DisconnectFromSAP()
            Home.btnExecute.Enabled = True
            Exit Sub
        End Try

    End Sub


    Private Sub ExecuteSingleControl(controlID As String)
        Try

            ' Initialize SAP connection
            If Not AttachToSAP() Then
                MessageBox.Show("Could not attach to SAP. Ensure you have required privileges to SAP and scripting is enabled.", "SAP Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Logger.LogMessage("Failed to attach to SAP.", False)
                Exit Sub
            End If

            ' Check if login was successful
            If Not CheckLoginStatus() Then
                MessageBox.Show("Login failed.", "SAP Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Logger.LogMessage("Login failed.", False)
                DisconnectFromSAP()
                Exit Sub
            End If

            ' Execute based on control
            Select Case controlID.ToUpper()
                Case "ITGC01"
                    ITGC01.ExecuteITGC01()
                Case "ITGC02"
                    ITGC02.ExecuteITGC02()
                Case "ITGC04"
                    ITGC04.ExecuteITGC04()
                Case "ITGC06"
                    ITGC06.ExecuteITGC06()
                Case "ITGC07"
                    ITGC07.ExecuteITGC07()
                Case "ITGC08"
                    ITGC08.ExecuteITGC08()
                Case "ITGC09"
                    ITGC09.ExecuteITGC09()
                Case "ITGC10"
                    ITGC10.ExecuteITGC10()
                Case "ITGC11"
                    ITGC11.ExecuteITGC11()
                Case "ITGC12"
                    ITGC12.ExecuteITGC12()
                Case "ITGC13"
                    ITGC13.ExecuteITGC13()
                Case "ITGC14"
                    ITGC14.ExecuteITGC14()
                Case "ITGC15"
                    ITGC15.ExecuteITGC15()
                Case "ITGC16"
                    ITGC16.ExecuteITGC16()
                Case "ITGC17"
                    ITGC17.ExecuteITGC17()
                Case "ITGC18"
                    ITGC18.ExecuteITGC18()
                Case Else
                    Logger.LogMessage("Unsupported control: " & controlID, False)
            End Select

            ' Disconnect after each control
            DisconnectFromSAP()
            Thread.Sleep(1000) ' Small delay before next
        Catch ex As Exception
            Logger.LogMessage($"Error in control {controlID}: {ex.Message}", False)
            Logger.LogException(ex, "ExecuteSingleControl Failed")
            DisconnectFromSAP()
        End Try
    End Sub


    ' Function to attach to the SAP GUI
    Function AttachToSAP() As Boolean
        Try
            ' Get credentials from form
            Dim controlID As String = Home.cmbControl.SelectedItem?.ToString().Split("-"c)(0).Trim()
            Dim sapSystem As String = Home.cmbSystem.SelectedItem?.ToString()
            Dim userID As String = Home.txtUsername.Text
            Dim password As String = Home.txtPassword.Text
            Dim LoginStatus As String = ""
            Dim client As String = Home.txtClient.Text ' Client Number

            ' check if controlID is valid
            Logger.LogMessage($"Starting execution for control: {controlID}", True)
            If controlID = "ITGC03" Or controlID = "ITGC05" Or controlID = "ITGC19" Then
                MsgBox($"{controlID} is currently not implemented.", MsgBoxStyle.Information, "Not Implemented")
                Logger.LogMessage($"{controlID} is currently not implemented.", False)
                Return False
            End If

            If String.IsNullOrEmpty(userID) OrElse String.IsNullOrEmpty(password) Then
                MessageBox.Show("User ID or Password missing.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Logger.LogMessage("User ID or Password missing.", False)
                Return False
            End If
            Try
                ' Launch SAP Logon

                Try
                    Process.Start(New ProcessStartInfo(My.Settings.SAPLogonPath) With {
    .UseShellExecute = True
})

                Catch ex As Exception
                    'Dim SAPLogonPath As String = "C:\Program Files (x86)\SAP\FrontEnd\SAPgui\saplogon.exe"
                    Dim SAPLogonPath As String = My.Settings.SAPLogonPath
                    Shell(SAPLogonPath, AppWinStyle.NormalFocus)
                End Try


                ' Wait for SAP Logon to open
                System.Threading.Thread.Sleep(3000)
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error")
                Return False
                Exit Function
            End Try


            ' Attempt to attach to SAP GUI up to 3 times
            Dim SapGuiAuto As Object = Nothing
            Dim maxAttempts As Integer = 3
            Dim attempt As Integer = 0

            Do While SapGuiAuto Is Nothing AndAlso attempt < maxAttempts
                Try
                    SapGuiAuto = GetObject("SAPGUI")
                Catch ex As Exception
                    SapGuiAuto = Nothing
                End Try

                If SapGuiAuto Is Nothing Then
                    Threading.Thread.Sleep(3000) ' Wait 3 second before retrying
                    attempt += 1
                End If
            Loop

            If SapGuiAuto Is Nothing Then
                MessageBox.Show("Unable to connect to SAP GUI after 3 attempts.", "SAP GUI Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            '------------------------------------------------------------------------
            SAPApp = SapGuiAuto.GetScriptingEngine
            If SAPApp Is Nothing Then Return False

            ' Open SAP Connection with the passed system name
            SAPCon = SAPApp.OpenConnection(sapSystem)
            If SAPCon Is Nothing Then Return False

            ' Get Active SAP Session
            session = SAPCon.Children(0)
            If session Is Nothing Then Return False

            ' Login
            If Home.cmbExecutionMode.SelectedItem IsNot Nothing AndAlso Home.cmbExecutionMode.SelectedItem.ToString() = "Foreground" Then
                session.findById("wnd[0]").maximize()
            End If

            ' Fill client only if checkbox is checked
            If Home.CheckBoxClient.Checked Then
                session.findById("wnd[0]/usr/txtRSYST-MANDT").Text = client ' Client Number
            End If

            ' Always fill user ID and password
            session.findById("wnd[0]/usr/txtRSYST-BNAME").Text = userID
            session.findById("wnd[0]/usr/pwdRSYST-BCODE").Text = password

            ' Press login button
            session.findById("wnd[0]/tbar[0]/btn[0]").press()

            ' Wait for login response
            System.Threading.Thread.Sleep(3000)

            ' Check login status
            LoginStatus = session.findById("wnd[0]/sbar").Text

            If String.IsNullOrEmpty(LoginStatus) Then
                Return True
            Else
                ' Exit session if login failed
                session.FindById("wnd[0]/tbar[0]/okcd").Text = "/nex"
                session.FindById("wnd[0]").SendVKey(0)
                Return False
            End If



        Catch ex As Exception
            MessageBox.Show("SAP Attachment Failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Logger.LogMessage("SAP Attachment Failed: " & ex.Message, False)
            Logger.LogException(ex, "Failed during SAP attachment")
            Return False
        End Try

    End Function

    Function CheckLoginStatus() As Boolean
        Try
            Dim errormsg As String = session.findById("wnd[0]/sbar").Text
            If errormsg.Contains("Incorrect logon data") Then
                'Console.WriteLine("Login failed: Incorrect credentials.")
                MsgBox("Login failed: Incorrect credentials.")
                Logger.LogMessage("Login failed: Incorrect credentials.", False)
                Return False
            Else
                Logger.LogMessage("Login Successfull.", True)
                Return True
            End If

        Catch ex As Exception
            Logger.LogMessage("Error during login: " & ex.Message, False)
            Return False
        End Try
    End Function

    Sub DisconnectFromSAP()
        Try
            ' Release the session reference but don't close SAP
            If session IsNot Nothing Then
                Marshal.ReleaseComObject(session)
                session = Nothing
            End If

            If SAPCon IsNot Nothing Then
                Marshal.ReleaseComObject(SAPCon)
                SAPCon = Nothing
            End If

            If SAPApp IsNot Nothing Then
                Marshal.ReleaseComObject(SAPApp)
                SAPApp = Nothing
            End If

            If SapGuiAuto IsNot Nothing Then
                Marshal.ReleaseComObject(SapGuiAuto)
                SapGuiAuto = Nothing
            End If

            ' Force garbage collection to clean up COM objects
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Logger.LogMessage("Disconnected from SAP.", True)
        Catch ex As Exception
            'Console.WriteLine("Error during detaching: " & ex.Message)
            MsgBox("Error during detaching: " & ex.Message)
            Logger.LogMessage("Error during detaching: " & ex.Message, False)
            Logger.LogException(ex, "Error during detaching")
        End Try
    End Sub

    Sub MinimizeAllWindowsExceptSAP()
        Dim hwndSAP As IntPtr = FindWindow("SAP_FRONTEND_SESSION", vbNullString)
        If hwndSAP <> IntPtr.Zero Then
            ShowWindow(hwndSAP, SW_MAXIMIZE)
        End If

        ' Minimize all windows via Win + D
        keybd_event(&H5B, 0, 0, UIntPtr.Zero)  ' Press Win
        keybd_event(&H44, 0, 0, UIntPtr.Zero)  ' Press D
        keybd_event(&H44, 0, &H2, UIntPtr.Zero)  ' Release D
        keybd_event(&H5B, 0, &H2, UIntPtr.Zero)  ' Release Win

        ' Restore SAP window
        If hwndSAP <> IntPtr.Zero Then
            ShowWindow(hwndSAP, SW_MAXIMIZE)
        End If
    End Sub


    Public Sub Takescreenshot(WordDoc As Object, stepNum As Integer)

        Dim controlITCGCheck As String = Home.cmbControl.SelectedItem?.ToString().Split("-"c)(0).Trim()

        Dim executionMode As String = Home.cmbExecutionMode.SelectedItem?.ToString()

        Dim selectedStartDate As DateTime = Home.dtpStart.Value
        Dim reportStartDate As String = selectedStartDate.ToString("dd.MM.yyyy")
        Dim selectedEndDate As DateTime = Home.dtpEnd.Value
        Dim reportEndDate As String = selectedEndDate.ToString("dd.MM.yyyy")
        Try
            '------------------------------------------------------------------------------------------------------------------------------
            ' Step description
            Dim hardcodedStepDesc As String = ""

            If controlITCGCheck = "ITGC01" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Navigate to Change Documents and select 'For Users'."
                    Case 3 : hardcodedStepDesc = "Step 3: Input User ID: " & My.Settings.ITGC01_OSSID &
                                       vbCrLf & "Duration: " & reportStartDate & ", " & My.Settings.Report_FTIME &
                                       " To " & reportEndDate & ", " & My.Settings.Report_TTIME
                    Case 4 : hardcodedStepDesc = "Step 4: Navigate to the User Attributes tab and select 'All'."
                    Case 5 : hardcodedStepDesc = "Step 5: Navigate to the Roles/Profiles tab, select 'All', and execute the query."
                    Case 6 : hardcodedStepDesc = "Result: Identified change documents for the specified SAP OSS ID."
                    Case 7 : hardcodedStepDesc = "Result: Continue execution."
                    Case 401 : hardcodedStepDesc = "Result: No change documents identified matching the specified criteria."
                End Select

            ElseIf controlITCGCheck = "ITGC02" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction SCC4."
                    Case 2 : hardcodedStepDesc = "Step 2: Navigate to Utilities and select 'Change Logs'."
                    Case 3 : hardcodedStepDesc = "Step 3: Define the reporting duration and execute the query." &
                                       vbCrLf & "Duration: " & reportStartDate & ", " & My.Settings.Report_FTIME &
                                       " To " & reportEndDate & ", " & My.Settings.Report_TTIME
                    Case 4, 200 : hardcodedStepDesc = "Result: Client opening occurred during the specified reporting period."
                    Case 401 : hardcodedStepDesc = "Result: No client opening occurred during the specified reporting period."
                End Select

            ElseIf controlITCGCheck = "ITGC04" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction SE16."
                    Case 2 : hardcodedStepDesc = "Step 2: Select the CDHDR table."
                    Case 3 : hardcodedStepDesc = "Step 3: Define the reporting duration." &
                                       vbCrLf & "Duration: " & reportStartDate & ", " & My.Settings.Report_FTIME &
                                       " To " & reportEndDate & ", " & My.Settings.Report_TTIME
                    Case 4 : hardcodedStepDesc = $"Step 4: Select the following transaction codes: {My.Settings.ITGC04_TCodeList}."
                    Case 5, 200 : hardcodedStepDesc = "Result: Table entries identified for the specified key."
                    Case 401 : hardcodedStepDesc = "Result: No table entries identified for the specified key."
                End Select

            ElseIf controlITCGCheck = "ITGC06" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Checkpoint 1: Verify entry count within the DEVACCESS table." & vbCrLf &
                                       "Step 1: Execute transaction SE16."
                    Case 2 : hardcodedStepDesc = "Step 2: Select the DEVACCESS table."
                    Case 3 : hardcodedStepDesc = "Step 3: Execute the query to verify assigned developer keys."
                    Case 4, 200 : hardcodedStepDesc = "Result: Table entries identified for the specified key."
                    Case 401 : hardcodedStepDesc = "Result: No table entries identified for the specified key."
                    Case 5 : hardcodedStepDesc = "Checkpoint 2: Evaluate the number of roles possessing S_DEVELOP and change capabilities." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 6 : hardcodedStepDesc = "Step 2: Navigate to the User menu and select 'By Complex Selection Criteria'."
                    Case 7 : hardcodedStepDesc = "Step 3: [Logon Tab] - Set the User Type to 'Dialog'."
                    Case 8 : hardcodedStepDesc = "Step 4: [Authorization Tab] - Input Authorization Object S_DEVELOP with OBJECT TYPE: DEBUG and ACTVT: 02, 03."
                    Case 9 : hardcodedStepDesc = "Result: The following users possess access to Authorization Object S_DEVELOP (OBJECT TYPE: DEBUG; ACTVT: 02, 03)."
                    Case 402 : hardcodedStepDesc = "Result: No users possess access to Authorization Object S_DEVELOP (OBJECT TYPE: DEBUG; ACTVT: 02, 03)."
                    Case 10 : hardcodedStepDesc = "Step 5: Select all applicable users and click 'In Accordance with Selection'."
                    Case 11 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                End Select

            ElseIf controlITCGCheck = "ITGC07" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = $"Checkpoint 1: Verify changes made to SAP* and DDIC user IDs during: {Home.txtReportMonth.Text}." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Select 'Change Document for Users'."
                    Case 3 : hardcodedStepDesc = "Step 3: Select users 'SAP*' and 'DDIC'."
                    Case 4 : hardcodedStepDesc = "Step 4: Define the reporting duration." & vbCrLf &
                                       reportStartDate & ", " & My.Settings.Report_FTIME & " To " & reportEndDate & ", " & My.Settings.Report_TTIME
                    Case 5 : hardcodedStepDesc = "Step 5: Navigate to the User Attributes tab and select 'All'."
                    Case 6 : hardcodedStepDesc = "Step 6: Navigate to the Roles/Profiles tab, select 'All', and execute the query."
                    Case 401 : hardcodedStepDesc = "Result: No change documents identified for SAP* and DDIC users."
                    Case 7 : hardcodedStepDesc = "Result: Change documents identified matching the specified criteria."
                    Case 8 : hardcodedStepDesc = $"Checkpoint 2: Identify Dialog (A) and Service (S) users assigned the SAP_ALL or SAP_NEW profiles during: {Home.txtReportMonth.Text}." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 9 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 10 : hardcodedStepDesc = "Step 3: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 11 : hardcodedStepDesc = "Step 4: [Roles/Profiles Tab] - Select profiles SAP_ALL and SAP_NEW."
                    Case 402 : hardcodedStepDesc = "Result: No Dialog (A) or Service (S) users possess the SAP_ALL or SAP_NEW profiles."
                    Case 12 : hardcodedStepDesc = "Result: The following users possess the SAP_ALL and SAP_NEW profiles."
                End Select

            ElseIf controlITCGCheck = "ITGC08" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = $"Checkpoint 1: Identify Dialog and Service users with access to S_TABU_NAM (ACTVT: 02) and S_TCODE (SM30, SM31)." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 3 : hardcodedStepDesc = "Step 3: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 4 : hardcodedStepDesc = "Step 4: [Authorization Tab] - Input values: Object S_TCODE {TCD: SM30 or SM31}, Object S_TABU_NAM {ACTVT: 02, Table: *}."
                    Case 5 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 401 : hardcodedStepDesc = "Result: No Dialog (A) or Service (S) users possess access to S_TABU_NAM (ACTVT: 02) and S_TCODE (SM30, SM31)."
                    Case 6 : hardcodedStepDesc = $"Checkpoint 2: Identify Dialog and Service users with access to S_TABU_DIS (ACTVT: 02) and S_TCODE (SM30, SM31)." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 7 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 8 : hardcodedStepDesc = "Step 3: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 9 : hardcodedStepDesc = "Step 4: [Authorization Tab] - Input values: Object S_TCODE {TCD: SM30 or SM31}, Object S_TABU_DIS {ACTVT: 02, Table: *}."
                    Case 10 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 402 : hardcodedStepDesc = "Result: No Dialog (A) or Service (S) users possess access to S_TABU_DIS (ACTVT: 02) and S_TCODE (SM30, SM31)."
                End Select

            ElseIf controlITCGCheck = "ITGC09" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = $"Checkpoint 1: Verify users are authorized to execute programs based on job responsibilities." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 3 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC09_ApprovedUsers
                    Case 4 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 5 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input values: Object S_TCODE {TCD: SA38 or SE38}, Object S_PROGRAM {P_ACTION: SUBMIT}, Object S_DEVELOP {ACTVT: 16, Obj Type: PROG, Obj Name: *}."
                    Case 6 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 401 : hardcodedStepDesc = "Result: No Dialog (A) or Service (S) users possess access to execute programs."
                End Select

            ElseIf controlITCGCheck = "ITGC10" Then
                Select Case stepNum
                    Case 400 : hardcodedStepDesc = "User have access to create user ID via direct profile assignmet."
                    Case 1 : hardcodedStepDesc = $"Checkpoint 1: Evaluate the ability to create user IDs." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 3 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC10_ApprovedUserscheck1
                    Case 4 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 5 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input conditions: Object S_TCODE {TCD: SU01 or SU10}, Object S_USER_GRP {Class: *, ACTVT: 02, 05}, Object S_USER_AUT {OBJECT: *, AUTH: *, ACTVT: 01, 02}, Object S_USER_PRO {Profile: *, ACTVT: 22}. Execute the query."
                    Case 401 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 6 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 7 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 8 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 402 : hardcodedStepDesc = "Result: No roles identified for the selected users."

                    Case 9 : hardcodedStepDesc = $"Checkpoint 2: Evaluate the ability to lock and unlock user IDs." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 10 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 11 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC10_ApprovedUserscheck2
                    Case 12 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 13 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input conditions: Object S_TCODE {TCD: SU01 or SU10}, Object S_USER_GRP {Class: *, ACTVT: 02, 05}. Execute the query."
                    Case 403 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 14 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 15 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 16 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 404 : hardcodedStepDesc = "Result: No roles identified for the selected users."

                    Case 17 : hardcodedStepDesc = $"Checkpoint 3: Evaluate the ability to modify roles in the production environment." & vbCrLf &
                                        "Step 1: Execute transaction SUIM."
                    Case 18 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 19 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC10_ApprovedUserscheck3
                    Case 20 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 21 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input conditions: Object S_TCODE {TCD: PFCG}, Object S_USER_AGR {ACT_GROUP: *, ACTVT: 01, 02}. Execute the query."
                    Case 405 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 22 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 23 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 24 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 406 : hardcodedStepDesc = "Result: No roles identified for the selected users."

                    Case 25 : hardcodedStepDesc = $"Checkpoint 4: Evaluate the ability to perform role assignments to users." & vbCrLf &
                                        "Step 1: Execute transaction SUIM."
                    Case 26 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 27 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC10_ApprovedUserscheck4
                    Case 28 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 29 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input conditions: Object S_TCODE {TCD: SU01 or SU10}, Object S_USER_AGR {ACT_GROUP: *, ACTVT: 22}. Execute the query."
                    Case 407 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 30 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 31 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 32 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 408 : hardcodedStepDesc = "Result: No roles identified for the selected users."

                    Case 33 : hardcodedStepDesc = $"Checkpoint 5: Evaluate the ability to assign SAP_ALL and SAP_NEW profiles." & vbCrLf &
                                        "Step 1: Execute transaction SUIM."
                    Case 34 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 35 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC10_ApprovedUserscheck5
                    Case 36 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 37 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input conditions: Object S_TCODE {TCD: SU01 or SU10}, Object S_USER_PRO {Profile: SAP_ALL or SAP_NEW, ACTVT: 22}. Execute the query."
                    Case 409 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 38 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 39 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 40 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 410 : hardcodedStepDesc = "Result: No roles identified for the selected users."
                End Select

            ElseIf controlITCGCheck = "ITGC11" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 3 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC11_ApprovedUsers
                    Case 4 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 5 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input values: Object S_TCODE {TCD: STMS or STMS_TRANSPORT}, Object S_CTS_ADMI {CTS_ADMFCT: IMPA, IMPS}, Object S_TRANSPRT {TType: *, ACTVT: 03}. Execute the query."
                    Case 401 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 6 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 7 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 8 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 402 : hardcodedStepDesc = "Result: No roles identified for the selected users."
                End Select

            ElseIf controlITCGCheck = "ITGC12" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 3 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC12_ApprovedUsers
                    Case 4 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 5 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input values: Object S_TCODE {TCD: SM37}, Object S_BTCH_ADM {BTCADMIN: Y}. Execute the query."
                    Case 401 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 6 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 7 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 8 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 402 : hardcodedStepDesc = "Result: No roles identified for the selected users."
                End Select

            ElseIf controlITCGCheck = "ITGC13" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 3 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC13_ApprovedUsers
                    Case 4 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 5 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input values: Object S_TCODE {TCD: RZ10}, Object S_RZL_ADM {ACTVT: 01, 07}, Object S_TABU_DIS {DICBERCLS: *, ACTVT: 01, 02, 03}, Object S_DATASET {PROGRAM: *, ACTVT: *, FILENAME: *}. Execute the query."
                    Case 401 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 6 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 7 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 8 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 402 : hardcodedStepDesc = "Result: No roles identified for the selected users."
                End Select

            ElseIf controlITCGCheck = "ITGC14" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction SA38."
                    Case 2 : hardcodedStepDesc = "Step 2: Input the program name RSUSR003."
                    Case 3 : hardcodedStepDesc = "Step 3: Execute the program to verify the password status for the default SAP ID."
                    Case 4 : hardcodedStepDesc = "Result: Displayed password status for the default SAP ID."
                End Select

            ElseIf controlITCGCheck = "ITGC15" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Checkpoint 1: Verify SCUA Authorization." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 3 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC13_ApprovedUsers
                    Case 4 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 5 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input values: Object S_TCODE {TCD: SCUA}, Object S_USER_SYS {SUBSYSTEM: *, ACTVT: 78}. Execute the query."
                    Case 401 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 6 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 7 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 8 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 402 : hardcodedStepDesc = "Result: No roles identified for the selected users."

                    Case 9 : hardcodedStepDesc = "Checkpoint 2: Verify SCUM Authorization." & vbCrLf &
                                       "Step 1: Execute transaction SUIM."
                    Case 10 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 11 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC13_ApprovedUsers
                    Case 12 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 13 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input values: Object S_TCODE {TCD: SCUM}, Object S_USER_SYS {SUBSYSTEM: *, ACTVT: 78}. Execute the query."
                    Case 403 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 14 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 15 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 16 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 404 : hardcodedStepDesc = "Result: No roles identified for the selected users."
                End Select

            ElseIf controlITCGCheck = "ITGC16" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction SE16."
                    Case 2 : hardcodedStepDesc = "Step 2: Access the TRDIR table."
                    Case 3 : hardcodedStepDesc = $"Step 3: Input the Program Name: {My.Settings.ITGC16_ProgramName}"
                    Case 4 : hardcodedStepDesc = "Set the Change date range from " & reportStartDate & " To " & reportEndDate
                    Case 5 : hardcodedStepDesc = "Result: The following SAP standard program changes were identified."
                    Case 402 : hardcodedStepDesc = "Result: No changes identified for the specified SAP standard program."
                End Select

            ElseIf controlITCGCheck = "ITGC17" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction SUIM."
                    Case 2 : hardcodedStepDesc = "Step 2: Select 'User by Complex Selection Criteria'."
                    Case 3 : hardcodedStepDesc = $"Step 3: Enter approved users into the Exception tab." & vbCrLf & My.Settings.ITGC13_ApprovedUsers
                    Case 4 : hardcodedStepDesc = "Step 4: [Logon Data Tab] - Set the User Type to Dialog (A) and Service (S)."
                    Case 5 : hardcodedStepDesc = "Step 5: [Authorization Tab] - Input values: Object S_TCODE {TCD: BD87 or WE19}, Object S_IDOCCTRL {ACTVT: 02, 16, EDI_TCD: *}. Execute the query."
                    Case 401 : hardcodedStepDesc = "Result: No users identified with this access."
                    Case 6 : hardcodedStepDesc = "Result: The following users possess the specified access."
                    Case 7 : hardcodedStepDesc = "Step 6: Select all applicable users and click 'In Accordance with Selection'."
                    Case 8 : hardcodedStepDesc = "Result: The following roles are assigned to the selected users."
                    Case 402 : hardcodedStepDesc = "Result: No roles identified for the selected users."
                End Select

            ElseIf controlITCGCheck = "ITGC18" Then
                Select Case stepNum
                    Case 1 : hardcodedStepDesc = "Step 1: Execute transaction RZ11."
                    Case 2 : hardcodedStepDesc = "Step 2: Verify system parameter 'login/min_password_lng'."
                    Case 3 : hardcodedStepDesc = "Step 3: Verify system parameter 'login/password_expiration_time'."
                    Case 4 : hardcodedStepDesc = "Step 4: Verify system parameter 'login/fails_to_user_lock'."
                    Case 5 : hardcodedStepDesc = "Step 5: Verify system parameter 'login/no_automatic_user_sapstar'."
                    Case 6 : hardcodedStepDesc = "Step 6: Verify system parameter 'rdisp/gui_auto_logout'."
                    Case 7 : hardcodedStepDesc = "Step 7: Verify system parameter 'rsau/enable'."
                    Case 8 : hardcodedStepDesc = "Step 8: Verify system parameter 'sapgui/user_scripting'."
                End Select

            End If


            '------------------------------------------------------------------------------------------------------------------------------
            ' Insert description in Word
            Dim sel As Microsoft.Office.Interop.Word.Selection = WordDoc.Application.Selection
            sel.EndKey(Microsoft.Office.Interop.Word.WdUnits.wdStory)
            sel.TypeParagraph()
            sel.Font.Size = 10
            sel.Font.Name = "Arial"
            sel.TypeText(hardcodedStepDesc)
            sel.TypeParagraph()

            '------------------------------------------------------------------------------------------------------------------------------
            ' Take Screenshot
            Dim hwndSAP As IntPtr = FindWindow("SAP_FRONTEND_SESSION", Nothing)
            If hwndSAP = IntPtr.Zero Then
                Logger.LogMessage("SAP window not found for screenshot.", False)
                Exit Sub
            End If

            If executionMode = "Foreground" Then

                ' --- Capture entire screen including taskbar & timestamp ---
                ShowWindow(hwndSAP, SW_MAXIMIZE)
                'Thread.Sleep(1200) ' give time to repaint
                ' Simulate PrintScreen
                My.Computer.Keyboard.SendKeys("{PRTSC}")
                Threading.Thread.Sleep(500)

                ' Paste from clipboard
                Dim img As Image = Clipboard.GetImage()
                If img IsNot Nothing Then
                    Using bmp As New Bitmap(img.Width, img.Height, Imaging.PixelFormat.Format32bppArgb)
                        Using g As Graphics = Graphics.FromImage(bmp)
                            g.DrawImage(img, 0, 0, img.Width, img.Height)
                        End Using
                        Dim tempFile As String = IO.Path.GetTempFileName() & ".png"
                        bmp.Save(tempFile, Imaging.ImageFormat.Png)
                        sel.InlineShapes.AddPicture(tempFile)
                        sel.TypeParagraph()
                    End Using
                End If

            ElseIf executionMode = "Background" Then
                Dim bmp As Bitmap = CaptureWindowBackground(hwndSAP)
                If bmp Is Nothing Then
                    MsgBox("Background screenshot failed.")
                    Exit Sub
                End If

                ' Overlay info
                Using gfx As Graphics = Graphics.FromImage(bmp)
                    Dim timestamp As String = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
                    Dim systemName As String = Home.lblSystemId.Text
                    Dim userId As String = Home.txtUsername.Text
                    Dim overlayText As String = $"Time: {timestamp}" '& vbCrLf &
                    '            $"System: {systemName}" & vbCrLf &
                    '            $"User: {userId}"

                    Using font As New System.Drawing.Font("Segoe UI", 12, FontStyle.Bold)
                        Dim textSize As SizeF = gfx.MeasureString(overlayText, font)

                        ' --- Center horizontally ---
                        Dim x As Single = (bmp.Width - textSize.Width) / 2
                        ' --- Keep near top ---
                        Dim y As Single = 20

                        ' Shadow brush (subtle gray for outline effect)
                        Using shadow As New SolidBrush(Color.FromArgb(0, 0, 0, 0)) ' semi-transparent black
                            Using textBrush As New SolidBrush(Color.Black) ' professional white text
                                ' Draw shadow (slight offset)
                                gfx.DrawString(overlayText, font, shadow, x + 2, y + 2)
                                ' Draw main text
                                gfx.DrawString(overlayText, font, textBrush, x, y)
                            End Using
                        End Using
                    End Using
                End Using


                ' Save & insert
                Dim tempFile As String = IO.Path.GetTempFileName() & ".png"
                bmp.Save(tempFile, Imaging.ImageFormat.Png)
                bmp.Dispose()
                sel.InlineShapes.AddPicture(tempFile)
                sel.TypeParagraph()

            End If

        Catch ex As Exception
            Logger.LogMessage("Error initializing Word application: " & ex.Message, False)
            Logger.LogException(ex, "Error initializing Word application")
            Exit Sub
        End Try
    End Sub
End Module
