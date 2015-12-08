<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form4
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
        Me.LabelEndText = New System.Windows.Forms.Label()
        Me.TextBoxEndText = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBoxStartText = New System.Windows.Forms.TextBox()
        Me.ButtonHelp = New System.Windows.Forms.Button()
        Me.ButtonSubmit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LabelEndText
        '
        Me.LabelEndText.AutoSize = True
        Me.LabelEndText.Location = New System.Drawing.Point(112, 89)
        Me.LabelEndText.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelEndText.Name = "LabelEndText"
        Me.LabelEndText.Size = New System.Drawing.Size(50, 13)
        Me.LabelEndText.TabIndex = 21
        Me.LabelEndText.Text = "End Text"
        '
        'TextBoxEndText
        '
        Me.TextBoxEndText.Location = New System.Drawing.Point(104, 104)
        Me.TextBoxEndText.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBoxEndText.Name = "TextBoxEndText"
        Me.TextBoxEndText.Size = New System.Drawing.Size(158, 20)
        Me.TextBoxEndText.TabIndex = 20
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(109, 39)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Start Text"
        '
        'TextBoxStartText
        '
        Me.TextBoxStartText.Location = New System.Drawing.Point(104, 54)
        Me.TextBoxStartText.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBoxStartText.Name = "TextBoxStartText"
        Me.TextBoxStartText.Size = New System.Drawing.Size(158, 20)
        Me.TextBoxStartText.TabIndex = 18
        '
        'ButtonHelp
        '
        Me.ButtonHelp.Location = New System.Drawing.Point(205, 174)
        Me.ButtonHelp.Name = "ButtonHelp"
        Me.ButtonHelp.Size = New System.Drawing.Size(75, 23)
        Me.ButtonHelp.TabIndex = 23
        Me.ButtonHelp.Text = "Help"
        Me.ButtonHelp.UseVisualStyleBackColor = True
        '
        'ButtonSubmit
        '
        Me.ButtonSubmit.Location = New System.Drawing.Point(81, 174)
        Me.ButtonSubmit.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonSubmit.Name = "ButtonSubmit"
        Me.ButtonSubmit.Size = New System.Drawing.Size(56, 19)
        Me.ButtonSubmit.TabIndex = 22
        Me.ButtonSubmit.Text = "Next >"
        Me.ButtonSubmit.UseVisualStyleBackColor = True
        '
        'Form4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(360, 219)
        Me.Controls.Add(Me.ButtonHelp)
        Me.Controls.Add(Me.ButtonSubmit)
        Me.Controls.Add(Me.LabelEndText)
        Me.Controls.Add(Me.TextBoxEndText)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBoxStartText)
        Me.Name = "Form4"
        Me.Text = "Choose optional segment of page with links"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelEndText As System.Windows.Forms.Label
    Friend WithEvents TextBoxEndText As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBoxStartText As System.Windows.Forms.TextBox
    Friend WithEvents ButtonHelp As System.Windows.Forms.Button
    Friend WithEvents ButtonSubmit As System.Windows.Forms.Button
End Class
