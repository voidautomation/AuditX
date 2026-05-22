Imports System.Windows.Forms
Imports System.Drawing

Public Module ResponsiveModule

#Region "Private Classes"

    Private Class ControlSnapshot
        Public OriginalBounds As Rectangle
        Public OriginalFontSize As Single
        Public OriginalFontName As String
        Public OriginalFontStyle As FontStyle
        Public ParentOriginalSize As Size
    End Class

    Private Class FormSnapshot
        Public OriginalClientSize As Size
        Public Snapshots As New Dictionary(Of Control, ControlSnapshot)
        Public IsReady As Boolean = False
        Public BaseScaleX As Single = 1.0F
        Public BaseScaleY As Single = 1.0F
    End Class

#End Region

#Region "Fields & Settings"

    Private _formStore As New Dictionary(Of Form, FormSnapshot)

    ' Public Settings
    Public ScaleFonts As Boolean = True
    Public MinFontSize As Single = 7.0F
    Public MaxFontSize As Single = 72.0F
    Public MaintainAspectRatio As Boolean = False
    Public ScalePadding As Boolean = False ' Usually better OFF to avoid overlap

#End Region

#Region "Main API"

    ''' <summary>
    ''' Call this in Form_Load - Makes form fully responsive
    ''' </summary>
    Public Sub MakeResponsive(frm As Form,
                              Optional minWidth As Integer = 0,
                              Optional minHeight As Integer = 0)
        If frm Is Nothing Then Return
        If _formStore.ContainsKey(frm) Then Return

        ' Set minimum size to prevent crushing
        If minWidth > 0 OrElse minHeight > 0 Then
            frm.MinimumSize = New Size(
                If(minWidth > 0, minWidth, frm.Width),
                If(minHeight > 0, minHeight, frm.Height)
            )
        ElseIf frm.MinimumSize = Size.Empty OrElse
               (frm.MinimumSize.Width = 0 AndAlso frm.MinimumSize.Height = 0) Then
            ' Auto set minimum to half the original size
            frm.MinimumSize = New Size(
                CInt(frm.Width * 0.5),
                CInt(frm.Height * 0.5)
            )
        End If

        Dim snap As New FormSnapshot()
        snap.OriginalClientSize = frm.ClientSize

        ' Take snapshot of all controls
        TakeSnapshot(frm, frm.Controls, snap)

        snap.IsReady = True
        _formStore(frm) = snap

        ' Hook events
        AddHandler frm.Resize, AddressOf OnFormResize
        AddHandler frm.ResizeEnd, AddressOf OnFormResizeEnd
        AddHandler frm.FormClosed, AddressOf OnFormClosed

    End Sub

    ''' <summary>
    ''' Call this after dynamically adding/removing controls
    ''' </summary>
    Public Sub ResetResponsive(frm As Form)
        If frm Is Nothing Then Return

        ' Remove old handlers
        If _formStore.ContainsKey(frm) Then
            _formStore.Remove(frm)
            RemoveHandler frm.Resize, AddressOf OnFormResize
            RemoveHandler frm.ResizeEnd, AddressOf OnFormResizeEnd
            RemoveHandler frm.FormClosed, AddressOf OnFormClosed
        End If

        ' Re-apply
        MakeResponsive(frm)
    End Sub

    ''' <summary>
    ''' Exclude a control from responsive scaling
    ''' </summary>
    Public Sub Exclude(frm As Form, ctrl As Control)
        If Not _formStore.ContainsKey(frm) Then Return
        Dim snap = _formStore(frm)
        If snap.Snapshots.ContainsKey(ctrl) Then
            snap.Snapshots.Remove(ctrl)
        End If
    End Sub

    ''' <summary>
    ''' Get current scale of a form
    ''' </summary>
    Public Function GetScale(frm As Form) As SizeF
        If Not _formStore.ContainsKey(frm) Then Return New SizeF(1, 1)
        Dim snap = _formStore(frm)
        Return New SizeF(
            CSng(frm.ClientSize.Width) / snap.OriginalClientSize.Width,
            CSng(frm.ClientSize.Height) / snap.OriginalClientSize.Height
        )
    End Function

#End Region

#Region "Snapshot Logic"

    Private Sub TakeSnapshot(frm As Form,
                             controls As Control.ControlCollection,
                             snap As FormSnapshot)
        For Each ctrl As Control In controls

            If snap.Snapshots.ContainsKey(ctrl) Then Continue For

            Dim cs As New ControlSnapshot()
            cs.OriginalBounds = ctrl.Bounds
            cs.OriginalFontSize = ctrl.Font.Size
            cs.OriginalFontName = ctrl.Font.FontFamily.Name
            cs.OriginalFontStyle = ctrl.Font.Style

            ' Record parent size for relative scaling
            If ctrl.Parent IsNot Nothing Then
                cs.ParentOriginalSize = ctrl.Parent.ClientSize
            Else
                cs.ParentOriginalSize = frm.ClientSize
            End If

            snap.Snapshots(ctrl) = cs

            ' Record DataGridView column widths
            If TypeOf ctrl Is DataGridView Then
                Dim dgv = DirectCast(ctrl, DataGridView)
                Dim colWidths As New Dictionary(Of String, Integer)
                For Each col As DataGridViewColumn In dgv.Columns
                    colWidths(col.Name) = col.Width
                Next
                dgv.Tag = colWidths
            End If

            ' Record ListView column widths
            If TypeOf ctrl Is ListView Then
                Dim lv = DirectCast(ctrl, ListView)
                Dim colWidths As New List(Of Integer)
                For Each col As ColumnHeader In lv.Columns
                    colWidths.Add(col.Width)
                Next
                lv.Tag = colWidths
            End If

            ' Recurse
            If ctrl.HasChildren Then
                TakeSnapshot(frm, ctrl.Controls, snap)
            End If
        Next
    End Sub

#End Region

#Region "Resize Logic"

    Private Sub ApplyResize(frm As Form)
        If Not _formStore.ContainsKey(frm) Then Return
        Dim snap = _formStore(frm)
        If Not snap.IsReady Then Return
        If frm.WindowState = FormWindowState.Minimized Then Return

        ' Global scale factors
        Dim gScaleX As Single = CSng(frm.ClientSize.Width) / CSng(snap.OriginalClientSize.Width)
        Dim gScaleY As Single = CSng(frm.ClientSize.Height) / CSng(snap.OriginalClientSize.Height)

        If MaintainAspectRatio Then
            Dim uniformScale As Single = Math.Min(gScaleX, gScaleY)
            gScaleX = uniformScale
            gScaleY = uniformScale
        End If

        frm.SuspendLayout()

        Try
            For Each kvp In snap.Snapshots
                Dim ctrl = kvp.Key
                Dim cs = kvp.Value

                If ctrl Is Nothing OrElse ctrl.IsDisposed Then Continue For
                If ctrl.Parent Is Nothing Then Continue For

                ' Calculate parent-relative scale
                Dim pScaleX As Single = gScaleX
                Dim pScaleY As Single = gScaleY

                ' If control lives inside a container (not the form itself)
                ' use the container's own scale relative to its original size
                If ctrl.Parent IsNot frm AndAlso
                   cs.ParentOriginalSize.Width > 0 AndAlso
                   cs.ParentOriginalSize.Height > 0 Then

                    pScaleX = CSng(ctrl.Parent.Width) / CSng(cs.ParentOriginalSize.Width)
                    pScaleY = CSng(ctrl.Parent.Height) / CSng(cs.ParentOriginalSize.Height)
                End If

                ' New position & size
                Dim newX As Integer = CInt(cs.OriginalBounds.X * pScaleX)
                Dim newY As Integer = CInt(cs.OriginalBounds.Y * pScaleY)
                Dim newW As Integer = Math.Max(1, CInt(cs.OriginalBounds.Width * pScaleX))
                Dim newH As Integer = Math.Max(1, CInt(cs.OriginalBounds.Height * pScaleY))

                ctrl.SetBounds(newX, newY, newW, newH)

                ' Scale font
                If ScaleFonts Then
                    Dim fontScale As Single = Math.Min(gScaleX, gScaleY)
                    Dim newSize As Single = cs.OriginalFontSize * fontScale
                    newSize = Math.Max(MinFontSize, Math.Min(MaxFontSize, newSize))

                    If Math.Abs(ctrl.Font.Size - newSize) > 0.05F Then
                        Try
                            Dim newFont As New Font(cs.OriginalFontName, newSize, cs.OriginalFontStyle)
                            ctrl.Font = newFont
                        Catch
                            ' ignore font error
                        End Try
                    End If
                End If

                ' Handle special controls
                HandleSpecial(ctrl, pScaleX)

            Next
        Finally
            frm.ResumeLayout(True)
        End Try
    End Sub

    ''' <summary>
    ''' Handle DataGridView, ListView columns etc.
    ''' </summary>
    Private Sub HandleSpecial(ctrl As Control, scaleX As Single)

        ' DataGridView
        If TypeOf ctrl Is DataGridView Then
            Dim dgv = DirectCast(ctrl, DataGridView)
            If dgv.Tag IsNot Nothing AndAlso TypeOf dgv.Tag Is Dictionary(Of String, Integer) Then
                Dim origWidths = DirectCast(dgv.Tag, Dictionary(Of String, Integer))
                For Each col As DataGridViewColumn In dgv.Columns
                    If origWidths.ContainsKey(col.Name) Then
                        col.Width = Math.Max(10, CInt(origWidths(col.Name) * scaleX))
                    End If
                Next
            End If
        End If

        ' ListView
        If TypeOf ctrl Is ListView Then
            Dim lv = DirectCast(ctrl, ListView)
            If lv.Tag IsNot Nothing AndAlso TypeOf lv.Tag Is List(Of Integer) Then
                Dim origWidths = DirectCast(lv.Tag, List(Of Integer))
                For i As Integer = 0 To Math.Min(lv.Columns.Count - 1, origWidths.Count - 1)
                    lv.Columns(i).Width = Math.Max(10, CInt(origWidths(i) * scaleX))
                Next
            End If
        End If

    End Sub

#End Region

#Region "Event Handlers"

    Private Sub OnFormResize(sender As Object, e As EventArgs)
        Dim frm = TryCast(sender, Form)
        If frm Is Nothing Then Return
        ApplyResize(frm)
    End Sub

    Private Sub OnFormResizeEnd(sender As Object, e As EventArgs)
        Dim frm = TryCast(sender, Form)
        If frm Is Nothing Then Return
        ApplyResize(frm) ' Final pass after resize ends
    End Sub

    Private Sub OnFormClosed(sender As Object, e As FormClosedEventArgs)
        Dim frm = TryCast(sender, Form)
        If frm Is Nothing Then Return

        RemoveHandler frm.Resize, AddressOf OnFormResize
        RemoveHandler frm.ResizeEnd, AddressOf OnFormResizeEnd
        RemoveHandler frm.FormClosed, AddressOf OnFormClosed

        _formStore.Remove(frm)
    End Sub

#End Region

End Module