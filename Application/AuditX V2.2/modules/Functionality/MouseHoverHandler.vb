' **********************************************
' MouseHoverHandler.vb
' Stores original colors, cursors, and sizes
' Text color remains black always
' Supports custom cursor and resizing on hover
' **********************************************

Public Module MouseHoverHandler

    ' Dictionaries to store original state
    Private originalBackColors As New Dictionary(Of Control, Color)
    Private originalForeColors As New Dictionary(Of Control, Color)
    Private originalCursors As New Dictionary(Of Control, Cursor)
    Private originalSizes As New Dictionary(Of Control, Size)
    Private originalLocations As New Dictionary(Of Control, Point)

    ''' <summary>
    ''' Applies hover effect to a control.
    ''' Includes background color, cursor, optional size increase.
    ''' </summary>
    ''' <param name="ctrl">Control to apply hover effect to</param>
    ''' <param name="hoverBack">Background color on hover (default: LightBlue)</param>
    ''' <param name="hoverCursor">Cursor on hover (default: Hand)</param>
    ''' <param name="enlargeSize">Optional size increase (e.g., New Size(10, 5))</param>
    Public Sub ApplyHoverEffect(ctrl As Control,
                                 Optional hoverBack As Color = Nothing,
                                 Optional hoverCursor As Cursor = Nothing,
                                 Optional enlargeSize As Size = Nothing)

        If hoverBack = Nothing Then hoverBack = Color.LightBlue
        If hoverCursor Is Nothing Then hoverCursor = Cursors.Hand

        ' Store original properties once
        If Not originalBackColors.ContainsKey(ctrl) Then originalBackColors(ctrl) = ctrl.BackColor
        If Not originalForeColors.ContainsKey(ctrl) Then originalForeColors(ctrl) = ctrl.ForeColor
        If Not originalCursors.ContainsKey(ctrl) Then originalCursors(ctrl) = ctrl.Cursor
        If Not originalSizes.ContainsKey(ctrl) Then originalSizes(ctrl) = ctrl.Size
        If Not originalLocations.ContainsKey(ctrl) Then originalLocations(ctrl) = ctrl.Location

        ' MouseEnter
        AddHandler ctrl.MouseEnter, Sub(sender As Object, e As EventArgs)
                                        Dim c As Control = CType(sender, Control)
                                        c.BackColor = hoverBack
                                        c.ForeColor = Color.Black
                                        c.Cursor = hoverCursor

                                        ' Resize if requested
                                        If Not enlargeSize.IsEmpty Then
                                            c.Size = New Size(c.Size.Width + enlargeSize.Width,
                                                              c.Size.Height + enlargeSize.Height)

                                            ' Optional: shift location to keep it centered
                                            c.Location = New Point(c.Location.X - (enlargeSize.Width \ 2),
                                                                   c.Location.Y - (enlargeSize.Height \ 2))
                                        End If
                                    End Sub

        ' MouseLeave
        AddHandler ctrl.MouseLeave, Sub(sender As Object, e As EventArgs)
                                        Dim c As Control = CType(sender, Control)

                                        If originalBackColors.ContainsKey(c) Then c.BackColor = originalBackColors(c)
                                        If originalForeColors.ContainsKey(c) Then c.ForeColor = originalForeColors(c)
                                        If originalCursors.ContainsKey(c) Then c.Cursor = originalCursors(c)

                                        ' Reset size and location if enlarged
                                        If originalSizes.ContainsKey(c) Then c.Size = originalSizes(c)
                                        If originalLocations.ContainsKey(c) Then c.Location = originalLocations(c)
                                    End Sub
    End Sub

End Module
