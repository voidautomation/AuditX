Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class ColoredProgressBar
    Inherits ProgressBar

    Public Property DisplayText As String = ""

    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or
                 ControlStyles.AllPaintingInWmPaint Or
                 ControlStyles.OptimizedDoubleBuffer, True)
        Me.Maximum = 100
        Me.Minimum = 0
        Me.Value = 0
        Me.Height = 26
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.Clear(Color.White)

        ' Percentage width
        Dim percent As Double = (Value - Minimum) / (Maximum - Minimum)
        Dim fillWidth As Integer = CInt(Me.Width * percent)

        ' Gradient fill (green -> yellow -> red)
        If fillWidth > 0 Then
            Using lg As New LinearGradientBrush(New Rectangle(0, 0, fillWidth, Me.Height),
                                                Color.LimeGreen,
                                                Color.Red,
                                                LinearGradientMode.Horizontal)
                g.FillRectangle(lg, 0, 0, fillWidth, Me.Height)
            End Using
        End If

        ' Border
        Using p As New Pen(Color.Gray)
            g.DrawRectangle(p, 0, 0, Me.Width - 1, Me.Height - 1)
        End Using

        ' Text
        Dim txt As String = If(String.IsNullOrEmpty(DisplayText), $"{Value}%", DisplayText)
        Dim sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
        Dim txtBrush As Brush = If(percent > 0.5, Brushes.White, Brushes.Black)
        g.DrawString(txt, Me.Font, txtBrush, Me.ClientRectangle, sf)
    End Sub
End Class
