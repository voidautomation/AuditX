<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TableAnalysis
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TableAnalysis))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btnFetch = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.PanelBottom = New System.Windows.Forms.Panel()
        Me.ComboBoxTable = New System.Windows.Forms.ComboBox()
        Me.PanelHeader = New System.Windows.Forms.Panel()
        Me.PanelBody = New System.Windows.Forms.Panel()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelBottom.SuspendLayout()
        Me.PanelHeader.SuspendLayout()
        Me.PanelBody.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.Snow
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 51
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(1149, 625)
        Me.DataGridView1.TabIndex = 0
        '
        'btnFetch
        '
        Me.btnFetch.Location = New System.Drawing.Point(516, 26)
        Me.btnFetch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnFetch.Name = "btnFetch"
        Me.btnFetch.Size = New System.Drawing.Size(134, 41)
        Me.btnFetch.TabIndex = 1
        Me.btnFetch.Text = "Fetch Data"
        Me.btnFetch.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(10, 14)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(71, 20)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "lblStatus"
        '
        'PanelBottom
        '
        Me.PanelBottom.Controls.Add(Me.lblStatus)
        Me.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelBottom.Location = New System.Drawing.Point(0, 715)
        Me.PanelBottom.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelBottom.Name = "PanelBottom"
        Me.PanelBottom.Size = New System.Drawing.Size(1149, 45)
        Me.PanelBottom.TabIndex = 3
        '
        'ComboBoxTable
        '
        Me.ComboBoxTable.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxTable.FormattingEnabled = True
        Me.ComboBoxTable.Items.AddRange(New Object() {"USR02", "T000", "E070", "RSUSR002"})
        Me.ComboBoxTable.Location = New System.Drawing.Point(14, 26)
        Me.ComboBoxTable.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ComboBoxTable.Name = "ComboBoxTable"
        Me.ComboBoxTable.Size = New System.Drawing.Size(496, 37)
        Me.ComboBoxTable.TabIndex = 2
        '
        'PanelHeader
        '
        Me.PanelHeader.Controls.Add(Me.ComboBoxTable)
        Me.PanelHeader.Controls.Add(Me.btnFetch)
        Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
        Me.PanelHeader.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelHeader.Name = "PanelHeader"
        Me.PanelHeader.Size = New System.Drawing.Size(1149, 90)
        Me.PanelHeader.TabIndex = 4
        '
        'PanelBody
        '
        Me.PanelBody.Controls.Add(Me.DataGridView1)
        Me.PanelBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelBody.Location = New System.Drawing.Point(0, 90)
        Me.PanelBody.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelBody.Name = "PanelBody"
        Me.PanelBody.Size = New System.Drawing.Size(1149, 625)
        Me.PanelBody.TabIndex = 5
        '
        'TableAnalysis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1149, 760)
        Me.Controls.Add(Me.PanelBody)
        Me.Controls.Add(Me.PanelHeader)
        Me.Controls.Add(Me.PanelBottom)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "TableAnalysis"
        Me.Text = "Home"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelBottom.ResumeLayout(False)
        Me.PanelBottom.PerformLayout()
        Me.PanelHeader.ResumeLayout(False)
        Me.PanelBody.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents btnFetch As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents PanelBottom As Panel
    Friend WithEvents ComboBoxTable As ComboBox
    Friend WithEvents PanelHeader As Panel
    Friend WithEvents PanelBody As Panel
End Class
