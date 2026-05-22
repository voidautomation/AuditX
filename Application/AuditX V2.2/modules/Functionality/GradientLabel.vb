Imports System.Drawing.Drawing2D

Module GradientHelper
    Public Sub ApplyGradientText(label As Label, color1 As Color, color2 As Color)
        ' Remove any existing handler to prevent multiple subscriptions
        RemoveHandler label.Paint, AddressOf GradientPaintHandler

        ' Store colors in label's Tag or other fields
        label.Tag = New Tuple(Of Color, Color)(color1, color2)

        ' Attach paint handler
        AddHandler label.Paint, AddressOf GradientPaintHandler

        ' Trigger repaint
        label.Invalidate()
    End Sub

    Private Sub GradientPaintHandler(sender As Object, e As PaintEventArgs)
        Dim label = CType(sender, Label)
        Dim colors = CType(label.Tag, Tuple(Of Color, Color))

        e.Graphics.Clear(label.BackColor)
        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        Dim rect As New Rectangle(0, 0, label.Width, label.Height)

        Using brush As New LinearGradientBrush(rect, colors.Item1, colors.Item2, LinearGradientMode.Horizontal)
            Using sf As New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near}
                e.Graphics.DrawString(label.Text, label.Font, brush, rect, sf)
            End Using
        End Using
    End Sub

    'Apply gradient to any control
    Public Sub ApplyGradient(control As Control,
                                    color1 As Color,
                                    color2 As Color,
                                    Optional mode As LinearGradientMode = LinearGradientMode.Vertical)

        AddHandler control.Paint,
            Sub(sender As Object, e As PaintEventArgs)

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

                Dim rect As Rectangle = control.ClientRectangle

                Using brush As New LinearGradientBrush(rect, color1, color2, mode)
                    e.Graphics.FillRectangle(brush, rect)
                End Using

            End Sub

    End Sub



    Public Sub StyleButton(btn As Button)

        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.BackColor = ColorTranslator.FromHtml("#2E86DE")
        btn.ForeColor = Color.White
        btn.Font = New Font("Segoe UI Semibold", 10)
        btn.Cursor = Cursors.Hand

    End Sub

    Public Sub StyleTextbox(txt As TextBox)

        txt.BorderStyle = BorderStyle.FixedSingle
        txt.Font = New Font("Segoe UI", 10)
        txt.BackColor = Color.White

    End Sub

End Module