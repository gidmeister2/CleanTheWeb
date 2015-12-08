<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormDiagnostic
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
        Me.TextBoxMessage = New System.Windows.Forms.TextBox()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBoxMessage
        '
        Me.TextBoxMessage.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxMessage.Multiline = True
        Me.TextBoxMessage.Name = "TextBoxMessage"
        Me.TextBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxMessage.Size = New System.Drawing.Size(781, 354)
        Me.TextBoxMessage.TabIndex = 0
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(807, 183)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 1
        Me.ButtonOK.Text = "OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'FormDiagnostic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(909, 351)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.TextBoxMessage)
        Me.Name = "FormDiagnostic"
        Me.Text = "FormDiagnostic"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBoxMessage As System.Windows.Forms.TextBox
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
End Class
