Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class MultiSystemSelectionForm
    Inherits Form

#Region "Fields"

    Private _availableSystems As List(Of String)
    Private _checkBoxes As New List(Of CheckBox)()

    ' Panels
    Private _pnlHeader As Panel
    Private _pnlSelectAll As Panel
    Private _pnlSystemScroll As Panel
    Private _pnlStatusStrip As Panel
    Private _pnlFooter As Panel

    ' Header
    Private _lblTitle As Label
    Private _lblSubTitle As Label
    Private _btnHeaderClose As Button

    ' Select All
    Private _chkSelectAll As CheckBox
    Private _lblSelectAllTitle As Label
    Private _lblSelectedCount As Label

    ' Status
    Private _lblStatus As Label

    ' Footer
    Private _btnOK As Button
    Private _btnCancel As Button

    ' DPI scale factor
    Private _dpiScale As Single = 1.0F

    ' Color palette
    Private ReadOnly _clrPrimary As Color = Color.FromArgb(0, 120, 212)
    Private ReadOnly _clrPrimaryDark As Color = Color.FromArgb(0, 90, 158)
    Private ReadOnly _clrPrimaryLight As Color = Color.FromArgb(235, 245, 255)
    Private ReadOnly _clrSuccess As Color = Color.FromArgb(16, 124, 16)
    Private ReadOnly _clrWarning As Color = Color.FromArgb(197, 90, 0)
    Private ReadOnly _clrBg As Color = Color.FromArgb(245, 245, 245)
    Private ReadOnly _clrCard As Color = Color.White
    Private ReadOnly _clrBorder As Color = Color.FromArgb(210, 210, 210)
    Private ReadOnly _clrTextPrimary As Color = Color.FromArgb(24, 24, 24)
    Private ReadOnly _clrTextSecondary As Color = Color.FromArgb(100, 100, 100)
    Private ReadOnly _clrHover As Color = Color.FromArgb(238, 246, 255)
    Private ReadOnly _clrDisabledBtn As Color = Color.FromArgb(172, 172, 172)

    ' Dragging
    Private _dragging As Boolean = False
    Private _dragStart As Point

    Public Property SelectedSystems As New List(Of String)()

#End Region

#Region "Constructor"

    Public Sub New(availableSystems As List(Of String))
        _availableSystems = availableSystems
        Me.SetStyle(
            ControlStyles.OptimizedDoubleBuffer Or
            ControlStyles.AllPaintingInWmPaint, True)
        Me.AutoScaleMode = AutoScaleMode.Dpi
        InitializeFormControls()
    End Sub

#End Region

#Region "DPI Helper"

    ''' <summary>
    ''' Scales a pixel value by the current DPI factor.
    ''' </summary>
    Private Function S(pixels As Integer) As Integer
        Return CInt(pixels * _dpiScale)
    End Function

    ''' <summary>
    ''' Detects DPI from the current screen and stores scale factor.
    ''' </summary>
    Private Sub DetectDpi()
        Using g As Graphics = Me.CreateGraphics()
            _dpiScale = g.DpiX / 96.0F
        End Using
        ' Clamp to reasonable range
        If _dpiScale < 1.0F Then _dpiScale = 1.0F
        If _dpiScale > 3.0F Then _dpiScale = 3.0F
    End Sub

    ''' <summary>
    ''' Returns a DPI-scaled Font.
    ''' </summary>
    Private Function SF(name As String,
                        size As Single,
                        Optional style As FontStyle = FontStyle.Regular) As Font
        Return New Font(name, size, style,
                        GraphicsUnit.Point)
    End Function

#End Region

#Region "Form Initialization"

    Private Sub InitializeFormControls()
        DetectDpi()

        ' ── Base form ────────────────────────────────────────────
        Me.Text = "Multi System Analysis"
        Me.FormBorderStyle = FormBorderStyle.None
        Me.StartPosition = FormStartPosition.CenterParent
        Me.ShowInTaskbar = False
        Me.BackColor = _clrBg
        Me.MinimumSize = New Size(S(480), S(200))

        ' Fixed widths — scale with DPI
        Dim formW As Integer = S(500)
        Dim padH As Integer = S(16) ' horizontal padding
        Dim padV As Integer = S(12) ' vertical padding

        ' Heights
        Dim headerH As Integer = S(76)
        Dim selectAllH As Integer = S(56)
        Dim cardH As Integer = S(56)
        Dim cardGap As Integer = S(6)
        Dim maxScrollH As Integer = S(260)
        Dim statusH As Integer = S(38)
        Dim footerH As Integer = S(64)

        ' Scroll panel height
        Dim totalCardH As Integer =
            (_availableSystems.Count * (cardH + cardGap)) + S(8)
        Dim scrollH As Integer = Math.Min(totalCardH, maxScrollH)

        Dim totalFormH As Integer =
            headerH + selectAllH + scrollH + statusH + footerH

        Me.Width = formW
        Me.Height = totalFormH

        ' Content area width (inside horizontal padding)
        Dim cW As Integer = formW - (padH * 2)

        ' Build sections top-to-bottom
        Dim yOffset As Integer = 0
        BuildHeader(yOffset, formW, headerH, padH)
        yOffset += headerH

        BuildSelectAll(yOffset, padH, cW, selectAllH)
        yOffset += selectAllH + S(4)

        BuildScrollArea(yOffset, padH, cW, scrollH, cardH, cardGap)
        yOffset += scrollH + S(4)

        BuildStatusStrip(yOffset, formW, statusH)
        yOffset += statusH

        BuildFooter(yOffset, formW, footerH, padH)

        AddHandler Me.Paint, AddressOf OnFormPaint
        UpdateOKButtonState()
    End Sub

#End Region

#Region "Header"

    Private Sub BuildHeader(top As Integer,
                            formW As Integer,
                            headerH As Integer,
                            padH As Integer)
        _pnlHeader = New Panel()
        _pnlHeader.SetBounds(0, top, formW, headerH)
        _pnlHeader.BackColor = _clrPrimary
        AddHandler _pnlHeader.Paint, AddressOf OnHeaderPaint
        AddHandler _pnlHeader.MouseDown, AddressOf OnDragDown
        AddHandler _pnlHeader.MouseMove, AddressOf OnDragMove
        AddHandler _pnlHeader.MouseUp, AddressOf OnDragUp
        Me.Controls.Add(_pnlHeader)

        ' Icon label
        Dim lblIcon As New Label()
        lblIcon.Text = "⚙"
        lblIcon.Font = SF("Segoe UI Symbol", 20.0F)
        lblIcon.ForeColor = Color.White
        lblIcon.BackColor = Color.Transparent
        lblIcon.AutoSize = False
        lblIcon.SetBounds(padH, 0, S(44), headerH)
        lblIcon.TextAlign = ContentAlignment.MiddleCenter
        HookDrag(lblIcon)
        _pnlHeader.Controls.Add(lblIcon)

        ' Title
        _lblTitle = New Label()
        _lblTitle.Text = "Multi System Analysis"
        _lblTitle.Font = SF("Segoe UI", 13.0F, FontStyle.Bold)
        _lblTitle.ForeColor = Color.White
        _lblTitle.BackColor = Color.Transparent
        _lblTitle.AutoSize = False
        _lblTitle.SetBounds(
            padH + S(44) + S(8),
            S(12),
            formW - padH - S(44) - S(8) - S(48),
            S(26))
        _lblTitle.TextAlign = ContentAlignment.MiddleLeft
        HookDrag(_lblTitle)
        _pnlHeader.Controls.Add(_lblTitle)

        ' Subtitle
        _lblSubTitle = New Label()
        _lblSubTitle.Text =
            "Select systems to include in cross-system user analysis"
        _lblSubTitle.Font = SF("Segoe UI", 8.5F)
        _lblSubTitle.ForeColor = Color.FromArgb(195, 225, 255)
        _lblSubTitle.BackColor = Color.Transparent
        _lblSubTitle.AutoSize = False
        _lblSubTitle.SetBounds(
            padH + S(44) + S(8),
            S(40),
            formW - padH - S(44) - S(8) - S(48),
            S(20))
        _lblSubTitle.TextAlign = ContentAlignment.MiddleLeft
        HookDrag(_lblSubTitle)
        _pnlHeader.Controls.Add(_lblSubTitle)

        ' Close (X) button
        _btnHeaderClose = New Button()
        _btnHeaderClose.Text = "✕"
        _btnHeaderClose.Font = SF("Segoe UI", 10.0F, FontStyle.Bold)
        _btnHeaderClose.ForeColor = Color.White
        _btnHeaderClose.BackColor = Color.Transparent
        _btnHeaderClose.FlatStyle = FlatStyle.Flat
        _btnHeaderClose.FlatAppearance.BorderSize = 0
        _btnHeaderClose.FlatAppearance.MouseOverBackColor =
            Color.FromArgb(196, 43, 28)
        _btnHeaderClose.FlatAppearance.MouseDownBackColor =
            Color.FromArgb(160, 30, 18)
        _btnHeaderClose.SetBounds(formW - S(44), 0, S(44), headerH)
        _btnHeaderClose.Cursor = Cursors.Hand
        AddHandler _btnHeaderClose.Click, AddressOf OnCancelClick
        _pnlHeader.Controls.Add(_btnHeaderClose)
        _btnHeaderClose.BringToFront()
    End Sub

    Private Sub OnHeaderPaint(sender As Object, e As PaintEventArgs)
        Dim p As Panel = DirectCast(sender, Panel)
        Using br As New LinearGradientBrush(
            p.ClientRectangle,
            Color.FromArgb(0, 130, 220),
            Color.FromArgb(0, 100, 185),
            LinearGradientMode.Vertical)
            e.Graphics.FillRectangle(br, p.ClientRectangle)
        End Using
    End Sub

    Private Sub HookDrag(ctrl As Control)
        AddHandler ctrl.MouseDown, AddressOf OnDragDown
        AddHandler ctrl.MouseMove, AddressOf OnDragMove
        AddHandler ctrl.MouseUp, AddressOf OnDragUp
    End Sub

#End Region

#Region "Select All Bar"

    Private Sub BuildSelectAll(top As Integer,
                               padH As Integer,
                               cW As Integer,
                               selectAllH As Integer)
        _pnlSelectAll = New Panel()
        _pnlSelectAll.SetBounds(padH, top + S(4), cW, selectAllH)
        _pnlSelectAll.BackColor = _clrPrimaryLight
        _pnlSelectAll.Cursor = Cursors.Hand
        AddHandler _pnlSelectAll.Paint, AddressOf OnSelectAllPaint
        AddHandler _pnlSelectAll.Click, AddressOf OnSelectAllPanelClick
        Me.Controls.Add(_pnlSelectAll)

        ' Checkbox
        _chkSelectAll = New CheckBox()
        _chkSelectAll.Text = String.Empty
        _chkSelectAll.SetBounds(S(12), (selectAllH - S(18)) \ 2, S(18), S(18))
        _chkSelectAll.BackColor = Color.Transparent
        AddHandler _chkSelectAll.CheckedChanged,
            AddressOf OnSelectAllChanged
        _pnlSelectAll.Controls.Add(_chkSelectAll)

        ' "Select All Systems" title
        _lblSelectAllTitle = New Label()
        _lblSelectAllTitle.Text = "Select All Systems"
        _lblSelectAllTitle.Font = SF("Segoe UI", 10.0F, FontStyle.Bold)
        _lblSelectAllTitle.ForeColor = _clrPrimary
        _lblSelectAllTitle.BackColor = Color.Transparent
        _lblSelectAllTitle.AutoSize = False
        _lblSelectAllTitle.SetBounds(
            S(38), S(8),
            cW - S(48), S(22))
        _lblSelectAllTitle.TextAlign = ContentAlignment.MiddleLeft
        AddHandler _lblSelectAllTitle.Click, AddressOf OnSelectAllPanelClick
        _pnlSelectAll.Controls.Add(_lblSelectAllTitle)

        ' Count label below title
        _lblSelectedCount = New Label()
        _lblSelectedCount.Text = "0 of " &
            _availableSystems.Count & " selected"
        _lblSelectedCount.Font = SF("Segoe UI", 8.0F)
        _lblSelectedCount.ForeColor = _clrTextSecondary
        _lblSelectedCount.BackColor = Color.Transparent
        _lblSelectedCount.AutoSize = False
        _lblSelectedCount.SetBounds(
            S(38), S(30),
            cW - S(48), S(16))
        _lblSelectedCount.TextAlign = ContentAlignment.MiddleLeft
        AddHandler _lblSelectedCount.Click, AddressOf OnSelectAllPanelClick
        _pnlSelectAll.Controls.Add(_lblSelectedCount)
    End Sub

    Private Sub OnSelectAllPaint(sender As Object, e As PaintEventArgs)
        Dim p As Panel = DirectCast(sender, Panel)
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        Using pen As New Pen(_clrPrimary, 1)
            e.Graphics.DrawRectangle(pen,
                0, 0, p.Width - 1, p.Height - 1)
        End Using
    End Sub

#End Region

#Region "System Scroll Area"

    Private Sub BuildScrollArea(top As Integer,
                                padH As Integer,
                                cW As Integer,
                                scrollH As Integer,
                                cardH As Integer,
                                cardGap As Integer)
        _pnlSystemScroll = New Panel()
        _pnlSystemScroll.SetBounds(padH, top, cW, scrollH)
        _pnlSystemScroll.AutoScroll = True
        _pnlSystemScroll.BackColor = _clrBg
        Me.Controls.Add(_pnlSystemScroll)

        Dim yPos As Integer = S(4)
        For i As Integer = 0 To _availableSystems.Count - 1
            Dim card As Panel = BuildCard(
                _availableSystems(i), i,
                cW - SystemInformation.VerticalScrollBarWidth - S(2),
                cardH)
            card.Top = yPos
            card.Left = 0
            _pnlSystemScroll.Controls.Add(card)
            yPos += cardH + cardGap
        Next
    End Sub

    Private Function BuildCard(sysName As String,
                               index As Integer,
                               cardW As Integer,
                               cardH As Integer) As Panel
        Dim card As New Panel()
        card.Size = New Size(cardW, cardH)
        card.BackColor = _clrCard
        card.Cursor = Cursors.Hand
        card.Tag = sysName
        AddHandler card.Paint, AddressOf OnCardPaint
        AddHandler card.MouseEnter, AddressOf OnCardEnter
        AddHandler card.MouseLeave, AddressOf OnCardLeave
        AddHandler card.Click, AddressOf OnCardClick

        ' ── Left accent bar (painted in OnCardPaint) ──────────────

        ' ── Index badge ───────────────────────────────────────────
        Dim badgeSize As Integer = S(28)
        Dim badgeLeft As Integer = S(14)
        Dim badgeTop As Integer = (cardH - badgeSize) \ 2

        Dim lblBadge As New Label()
        lblBadge.Text = (index + 1).ToString()
        lblBadge.Font = SF("Segoe UI", 8.5F, FontStyle.Bold)
        lblBadge.ForeColor = Color.White
        lblBadge.BackColor = _clrPrimary
        lblBadge.Size = New Size(badgeSize, badgeSize)
        lblBadge.Location = New Point(badgeLeft, badgeTop)
        lblBadge.TextAlign = ContentAlignment.MiddleCenter
        lblBadge.Tag = sysName
        HookCardEvents(lblBadge)
        card.Controls.Add(lblBadge)

        ' ── System name ───────────────────────────────────────────
        Dim textLeft As Integer = badgeLeft + badgeSize + S(12)
        Dim chkW As Integer = S(22)
        Dim chkRight As Integer = S(14)
        Dim textW As Integer = cardW - textLeft - chkW - chkRight - S(8)

        Dim lblName As New Label()
        lblName.Text = sysName
        lblName.Font = SF("Segoe UI", 10.5F, FontStyle.Bold)
        lblName.ForeColor = _clrTextPrimary
        lblName.BackColor = Color.Transparent
        lblName.AutoSize = False
        lblName.Size = New Size(textW, S(22))
        lblName.Location = New Point(textLeft, S(8))
        lblName.TextAlign = ContentAlignment.MiddleLeft
        lblName.Tag = sysName
        HookCardEvents(lblName)
        card.Controls.Add(lblName)

        ' ── Sub label ─────────────────────────────────────────────
        Dim lblSub As New Label()
        lblSub.Text = "SAP System  ·  RFC Destination"
        lblSub.Font = SF("Segoe UI", 7.5F)
        lblSub.ForeColor = _clrTextSecondary
        lblSub.BackColor = Color.Transparent
        lblSub.AutoSize = False
        lblSub.Size = New Size(textW, S(16))
        lblSub.Location = New Point(textLeft, S(30))
        lblSub.TextAlign = ContentAlignment.MiddleLeft
        lblSub.Tag = sysName
        HookCardEvents(lblSub)
        card.Controls.Add(lblSub)

        ' ── Checkbox ──────────────────────────────────────────────
        Dim chk As New CheckBox()
        chk.Text = String.Empty
        chk.Size = New Size(chkW, chkW)
        chk.Location = New Point(
            cardW - chkW - chkRight,
            (cardH - chkW) \ 2)
        chk.BackColor = Color.Transparent
        chk.Tag = sysName
        AddHandler chk.CheckedChanged, AddressOf OnSystemCheckChanged
        AddHandler chk.MouseEnter, AddressOf OnCardEnter
        AddHandler chk.MouseLeave, AddressOf OnCardLeave
        card.Controls.Add(chk)
        _checkBoxes.Add(chk)

        Return card
    End Function

    Private Sub HookCardEvents(ctrl As Control)
        AddHandler ctrl.MouseEnter, AddressOf OnCardEnter
        AddHandler ctrl.MouseLeave, AddressOf OnCardLeave
        AddHandler ctrl.Click, AddressOf OnCardClick
    End Sub

#End Region

#Region "Card Paint & Hover"

    Private Sub OnCardPaint(sender As Object, e As PaintEventArgs)
        Dim card As Panel = DirectCast(sender, Panel)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Outer border
        Using pen As New Pen(_clrBorder, 1)
            g.DrawRectangle(pen, 0, 0,
                card.Width - 1, card.Height - 1)
        End Using

        ' Left accent bar if checked
        If IsCardChecked(card) Then
            Using br As New SolidBrush(_clrPrimary)
                g.FillRectangle(br, 0, 0, S(4), card.Height)
            End Using
        End If
    End Sub

    Private Function IsCardChecked(card As Panel) As Boolean
        Dim tag As String = If(card.Tag IsNot Nothing,
                               card.Tag.ToString(), String.Empty)
        For Each chk As CheckBox In _checkBoxes
            If chk.Tag IsNot Nothing AndAlso
               chk.Tag.ToString() = tag AndAlso chk.Checked Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub OnCardEnter(sender As Object, e As EventArgs)
        Dim card As Panel = FindParentCard(sender)
        If card Is Nothing Then Return
        card.BackColor = _clrHover
        SetChildBackColor(card, _clrHover)
    End Sub

    Private Sub OnCardLeave(sender As Object, e As EventArgs)
        Dim card As Panel = FindParentCard(sender)
        If card Is Nothing Then Return
        Dim mouse As Point = card.PointToClient(Cursor.Position)
        If Not card.ClientRectangle.Contains(mouse) Then
            card.BackColor = _clrCard
            SetChildBackColor(card, _clrCard)
        End If
    End Sub

    Private Sub SetChildBackColor(card As Panel, clr As Color)
        For Each ctrl As Control In card.Controls
            If Not TypeOf ctrl Is CheckBox Then
                ctrl.BackColor = clr
            End If
        Next
    End Sub

    Private Sub OnCardClick(sender As Object, e As EventArgs)
        Dim card As Panel = FindParentCard(sender)
        If card Is Nothing Then Return
        Dim tag As String = If(card.Tag IsNot Nothing,
                               card.Tag.ToString(), String.Empty)
        For Each chk As CheckBox In _checkBoxes
            If chk.Tag IsNot Nothing AndAlso
               chk.Tag.ToString() = tag Then
                chk.Checked = Not chk.Checked
                Exit For
            End If
        Next
    End Sub

    Private Function FindParentCard(sender As Object) As Panel
        Dim ctrl As Control = TryCast(sender, Control)
        Do While ctrl IsNot Nothing
            If TypeOf ctrl Is Panel AndAlso
               ctrl.Parent Is _pnlSystemScroll Then
                Return DirectCast(ctrl, Panel)
            End If
            ctrl = ctrl.Parent
        Loop
        Return Nothing
    End Function

#End Region

#Region "Status Strip"

    Private Sub BuildStatusStrip(top As Integer,
                                 formW As Integer,
                                 statusH As Integer)
        _pnlStatusStrip = New Panel()
        _pnlStatusStrip.SetBounds(0, top, formW, statusH)
        _pnlStatusStrip.BackColor = Color.FromArgb(240, 240, 240)
        AddHandler _pnlStatusStrip.Paint, AddressOf OnStatusPaint
        Me.Controls.Add(_pnlStatusStrip)

        _lblStatus = New Label()
        _lblStatus.Text = "  Select at least 2 systems to enable analysis"
        _lblStatus.Font = SF("Segoe UI", 8.5F)
        _lblStatus.ForeColor = _clrTextSecondary
        _lblStatus.BackColor = Color.Transparent
        _lblStatus.AutoSize = False
        _lblStatus.SetBounds(S(12), 0, formW - S(24), statusH)
        _lblStatus.TextAlign = ContentAlignment.MiddleLeft
        _pnlStatusStrip.Controls.Add(_lblStatus)
    End Sub

    Private Sub OnStatusPaint(sender As Object, e As PaintEventArgs)
        Dim p As Panel = DirectCast(sender, Panel)
        Using pen As New Pen(_clrBorder, 1)
            e.Graphics.DrawLine(pen, 0, 0, p.Width, 0)
            e.Graphics.DrawLine(pen,
                0, p.Height - 1, p.Width, p.Height - 1)
        End Using
    End Sub

#End Region

#Region "Footer"

    Private Sub BuildFooter(top As Integer,
                            formW As Integer,
                            footerH As Integer,
                            padH As Integer)
        _pnlFooter = New Panel()
        _pnlFooter.SetBounds(0, top, formW, footerH)
        _pnlFooter.BackColor = _clrCard
        AddHandler _pnlFooter.Paint, AddressOf OnFooterPaint
        Me.Controls.Add(_pnlFooter)

        Dim btnH As Integer = S(36)
        Dim btnTop As Integer = (footerH - btnH) \ 2
        Dim cancelW As Integer = S(100)
        Dim okW As Integer = S(180)
        Dim gap As Integer = S(10)
        Dim rightEdge As Integer = formW - padH

        ' Cancel
        _btnCancel = New Button()
        _btnCancel.Text = "Cancel"
        _btnCancel.Font = SF("Segoe UI", 9.5F)
        _btnCancel.ForeColor = _clrTextPrimary
        _btnCancel.BackColor = Color.White
        _btnCancel.FlatStyle = FlatStyle.Flat
        _btnCancel.FlatAppearance.BorderColor = _clrBorder
        _btnCancel.FlatAppearance.BorderSize = 1
        _btnCancel.FlatAppearance.MouseOverBackColor =
            Color.FromArgb(242, 242, 242)
        _btnCancel.FlatAppearance.MouseDownBackColor =
            Color.FromArgb(230, 230, 230)
        _btnCancel.Size = New Size(cancelW, btnH)
        _btnCancel.Location = New Point(rightEdge - cancelW, btnTop)
        _btnCancel.Cursor = Cursors.Hand
        AddHandler _btnCancel.Click, AddressOf OnCancelClick
        _pnlFooter.Controls.Add(_btnCancel)
        Me.CancelButton = _btnCancel

        ' OK / Analyze
        _btnOK = New Button()
        _btnOK.Text = "Select 2+ Systems"
        _btnOK.Font = SF("Segoe UI", 9.5F, FontStyle.Bold)
        _btnOK.ForeColor = Color.White
        _btnOK.BackColor = _clrDisabledBtn
        _btnOK.FlatStyle = FlatStyle.Flat
        _btnOK.FlatAppearance.BorderSize = 0
        _btnOK.FlatAppearance.MouseOverBackColor = _clrPrimaryDark
        _btnOK.FlatAppearance.MouseDownBackColor =
            Color.FromArgb(0, 75, 140)
        _btnOK.Size = New Size(okW, btnH)
        _btnOK.Location = New Point(
            _btnCancel.Left - gap - okW, btnTop)
        _btnOK.Enabled = False
        _btnOK.Cursor = Cursors.Hand
        AddHandler _btnOK.Click, AddressOf OnOKClick
        _pnlFooter.Controls.Add(_btnOK)
        Me.AcceptButton = _btnOK
    End Sub

    Private Sub OnFooterPaint(sender As Object, e As PaintEventArgs)
        Using pen As New Pen(_clrBorder, 1)
            e.Graphics.DrawLine(pen, 0, 0,
                DirectCast(sender, Panel).Width, 0)
        End Using
    End Sub

#End Region

#Region "Form Border"

    Private Sub OnFormPaint(sender As Object, e As PaintEventArgs)
        Using pen As New Pen(Color.FromArgb(180, 180, 180), 1)
            e.Graphics.DrawRectangle(pen,
                0, 0, Me.Width - 1, Me.Height - 1)
        End Using
    End Sub

#End Region

#Region "Drag"

    Private Sub OnDragDown(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            _dragging = True
            _dragStart = e.Location
        End If
    End Sub

    Private Sub OnDragMove(sender As Object, e As MouseEventArgs)
        If _dragging Then
            Me.Location = New Point(
                Me.Location.X + e.Location.X - _dragStart.X,
                Me.Location.Y + e.Location.Y - _dragStart.Y)
        End If
    End Sub

    Private Sub OnDragUp(sender As Object, e As MouseEventArgs)
        _dragging = False
    End Sub

#End Region

#Region "Selection Logic"

    Private Sub OnSelectAllPanelClick(sender As Object, e As EventArgs)
        Dim checkedCount As Integer = CountChecked()
        _chkSelectAll.Checked = (checkedCount < _checkBoxes.Count)
    End Sub

    Private Sub OnSelectAllChanged(sender As Object, e As EventArgs)
        ' Detach card handlers
        For Each chk As CheckBox In _checkBoxes
            RemoveHandler chk.CheckedChanged,
                AddressOf OnSystemCheckChanged
        Next

        Dim state As Boolean = _chkSelectAll.Checked
        For Each chk As CheckBox In _checkBoxes
            chk.Checked = state
        Next

        ' Repaint all cards
        For Each ctrl As Control In _pnlSystemScroll.Controls
            ctrl.Invalidate()
        Next

        ' Re-attach
        For Each chk As CheckBox In _checkBoxes
            AddHandler chk.CheckedChanged,
                AddressOf OnSystemCheckChanged
        Next

        UpdateOKButtonState()
    End Sub

    Private Sub OnSystemCheckChanged(sender As Object, e As EventArgs)
        ' Repaint parent card for accent bar
        Dim chk As CheckBox = TryCast(sender, CheckBox)
        If chk IsNot Nothing Then
            Dim card As Panel = FindParentCard(chk)
            If card IsNot Nothing Then card.Invalidate()
        End If

        SyncSelectAll()
        UpdateOKButtonState()
    End Sub

    Private Sub SyncSelectAll()
        RemoveHandler _chkSelectAll.CheckedChanged,
            AddressOf OnSelectAllChanged

        Dim n As Integer = CountChecked()
        If n = 0 Then
            _chkSelectAll.CheckState = CheckState.Unchecked
        ElseIf n = _checkBoxes.Count Then
            _chkSelectAll.CheckState = CheckState.Checked
        Else
            _chkSelectAll.CheckState = CheckState.Indeterminate
        End If

        AddHandler _chkSelectAll.CheckedChanged,
            AddressOf OnSelectAllChanged
    End Sub

    Private Function CountChecked() As Integer
        Dim n As Integer = 0
        For Each chk As CheckBox In _checkBoxes
            If chk.Checked Then n += 1
        Next
        Return n
    End Function

    Private Sub UpdateOKButtonState()
        Dim n As Integer = CountChecked()

        _lblSelectedCount.Text =
            n & " of " & _availableSystems.Count & " selected"

        Select Case n
            Case 0
                _btnOK.Enabled = False
                _btnOK.BackColor = _clrDisabledBtn
                _btnOK.Text = "Select 2+ Systems"
                _lblStatus.Text =
                    "  Select at least 2 systems to enable analysis"
                _lblStatus.ForeColor = _clrTextSecondary

            Case 1
                _btnOK.Enabled = False
                _btnOK.BackColor = _clrDisabledBtn
                _btnOK.Text = "Select 1 More System"
                _lblStatus.Text =
                    "  Select 1 more system to enable analysis"
                _lblStatus.ForeColor = _clrWarning

            Case Else
                _btnOK.Enabled = True
                _btnOK.BackColor = _clrPrimary
                _btnOK.Text = "Analyze " & n & " Systems"
                _lblStatus.Text =
                    "  " & n & " systems selected — ready to run"
                _lblStatus.ForeColor = _clrSuccess
        End Select
    End Sub

#End Region

#Region "Button Handlers"

    Private Sub OnOKClick(sender As Object, e As EventArgs)
        SelectedSystems.Clear()
        For Each chk As CheckBox In _checkBoxes
            If chk.Checked Then
                SelectedSystems.Add(chk.Tag.ToString())
            End If
        Next

        If SelectedSystems.Count < 2 Then
            MessageBox.Show(
                "Please select at least 2 SAP systems.",
                "Selection Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub OnCancelClick(sender As Object, e As EventArgs)
        SelectedSystems.Clear()
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

#End Region

End Class