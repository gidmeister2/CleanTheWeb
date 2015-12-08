<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form5
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadioButtonYesJustWarn = New System.Windows.Forms.RadioButton()
        Me.RadioButtonNoDontInclude = New System.Windows.Forms.RadioButton()
        Me.RadioButtonNoButSaveSeparate = New System.Windows.Forms.RadioButton()
        Me.ButtonHelp = New System.Windows.Forms.Button()
        Me.ButtonSubmit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(48, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(426, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Should unbalanced tag pairs be allowed (for instance a <p> tag with a missing </p" & _
    "> tag)?"
        '
        'RadioButtonYesJustWarn
        '
        Me.RadioButtonYesJustWarn.AutoSize = True
        Me.RadioButtonYesJustWarn.Location = New System.Drawing.Point(105, 70)
        Me.RadioButtonYesJustWarn.Name = "RadioButtonYesJustWarn"
        Me.RadioButtonYesJustWarn.Size = New System.Drawing.Size(211, 17)
        Me.RadioButtonYesJustWarn.TabIndex = 1
        Me.RadioButtonYesJustWarn.TabStop = True
        Me.RadioButtonYesJustWarn.Text = "Yes, just give a warning, but don't abort"
        Me.RadioButtonYesJustWarn.UseVisualStyleBackColor = True
        '
        'RadioButtonNoDontInclude
        '
        Me.RadioButtonNoDontInclude.AutoSize = True
        Me.RadioButtonNoDontInclude.Location = New System.Drawing.Point(105, 93)
        Me.RadioButtonNoDontInclude.Name = "RadioButtonNoDontInclude"
        Me.RadioButtonNoDontInclude.Size = New System.Drawing.Size(156, 17)
        Me.RadioButtonNoDontInclude.TabIndex = 2
        Me.RadioButtonNoDontInclude.TabStop = True
        Me.RadioButtonNoDontInclude.Text = "No, don't include them at all"
        Me.RadioButtonNoDontInclude.UseVisualStyleBackColor = True
        '
        'RadioButtonNoButSaveSeparate
        '
        Me.RadioButtonNoButSaveSeparate.AutoSize = True
        Me.RadioButtonNoButSaveSeparate.Location = New System.Drawing.Point(105, 116)
        Me.RadioButtonNoButSaveSeparate.Name = "RadioButtonNoButSaveSeparate"
        Me.RadioButtonNoButSaveSeparate.Size = New System.Drawing.Size(192, 17)
        Me.RadioButtonNoButSaveSeparate.TabIndex = 3
        Me.RadioButtonNoButSaveSeparate.TabStop = True
        Me.RadioButtonNoButSaveSeparate.Text = "No, but save them in a separate file"
        Me.RadioButtonNoButSaveSeparate.UseVisualStyleBackColor = True
        '
        'ButtonHelp
        '
        Me.ButtonHelp.Location = New System.Drawing.Point(229, 159)
        Me.ButtonHelp.Name = "ButtonHelp"
        Me.ButtonHelp.Size = New System.Drawing.Size(75, 23)
        Me.ButtonHelp.TabIndex = 25
        Me.ButtonHelp.Text = "Help"
        Me.ButtonHelp.UseVisualStyleBackColor = True
        '
        'ButtonSubmit
        '
        Me.ButtonSubmit.Location = New System.Drawing.Point(105, 159)
        Me.ButtonSubmit.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonSubmit.Name = "ButtonSubmit"
        Me.ButtonSubmit.Size = New System.Drawing.Size(56, 19)
        Me.ButtonSubmit.TabIndex = 24
        Me.ButtonSubmit.Text = "Next >"
        Me.ButtonSubmit.UseVisualStyleBackColor = True
        '
        'Form5
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(499, 203)
        Me.Controls.Add(Me.ButtonHelp)
        Me.Controls.Add(Me.ButtonSubmit)
        Me.Controls.Add(Me.RadioButtonNoButSaveSeparate)
        Me.Controls.Add(Me.RadioButtonNoDontInclude)
        Me.Controls.Add(Me.RadioButtonYesJustWarn)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form5"
        Me.Text = "How strict should tests be?"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RadioButtonYesJustWarn As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonNoDontInclude As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonNoButSaveSeparate As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonHelp As System.Windows.Forms.Button
    Friend WithEvents ButtonSubmit As System.Windows.Forms.Button
End Class
