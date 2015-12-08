<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form0
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
        Me.ButtonHelp = New System.Windows.Forms.Button()
        Me.ButtonSubmit = New System.Windows.Forms.Button()
        Me.RadioButtonScan = New System.Windows.Forms.RadioButton()
        Me.RadioButtonPaste = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ButtonHelp
        '
        Me.ButtonHelp.Location = New System.Drawing.Point(408, 47)
        Me.ButtonHelp.Name = "ButtonHelp"
        Me.ButtonHelp.Size = New System.Drawing.Size(75, 23)
        Me.ButtonHelp.TabIndex = 15
        Me.ButtonHelp.Text = "Help"
        Me.ButtonHelp.UseVisualStyleBackColor = True
        '
        'ButtonSubmit
        '
        Me.ButtonSubmit.Location = New System.Drawing.Point(331, 50)
        Me.ButtonSubmit.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonSubmit.Name = "ButtonSubmit"
        Me.ButtonSubmit.Size = New System.Drawing.Size(56, 19)
        Me.ButtonSubmit.TabIndex = 14
        Me.ButtonSubmit.Text = "Next >"
        Me.ButtonSubmit.UseVisualStyleBackColor = True
        '
        'RadioButtonScan
        '
        Me.RadioButtonScan.AutoSize = True
        Me.RadioButtonScan.Location = New System.Drawing.Point(247, 50)
        Me.RadioButtonScan.Margin = New System.Windows.Forms.Padding(2)
        Me.RadioButtonScan.Name = "RadioButtonScan"
        Me.RadioButtonScan.Size = New System.Drawing.Size(50, 17)
        Me.RadioButtonScan.TabIndex = 13
        Me.RadioButtonScan.TabStop = True
        Me.RadioButtonScan.Text = "Scan"
        Me.RadioButtonScan.UseVisualStyleBackColor = True
        '
        'RadioButtonPaste
        '
        Me.RadioButtonPaste.AutoSize = True
        Me.RadioButtonPaste.Location = New System.Drawing.Point(186, 50)
        Me.RadioButtonPaste.Margin = New System.Windows.Forms.Padding(2)
        Me.RadioButtonPaste.Name = "RadioButtonPaste"
        Me.RadioButtonPaste.Size = New System.Drawing.Size(52, 17)
        Me.RadioButtonPaste.TabIndex = 12
        Me.RadioButtonPaste.TabStop = True
        Me.RadioButtonPaste.Text = "Paste"
        Me.RadioButtonPaste.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(93, 25)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(394, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Do you wish to paste URLs in a textbox, or do you want to scan a page for URLs?"
        '
        'Form0
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(637, 92)
        Me.Controls.Add(Me.ButtonHelp)
        Me.Controls.Add(Me.ButtonSubmit)
        Me.Controls.Add(Me.RadioButtonScan)
        Me.Controls.Add(Me.RadioButtonPaste)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form0"
        Me.Text = "Chooaw Paste or Scan"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonHelp As System.Windows.Forms.Button
    Friend WithEvents ButtonSubmit As System.Windows.Forms.Button
    Friend WithEvents RadioButtonScan As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonPaste As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
