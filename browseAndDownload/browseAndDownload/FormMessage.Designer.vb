<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMessage
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
        Me.ButtonSubmit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBoxMessage
        '
        Me.TextBoxMessage.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxMessage.Multiline = True
        Me.TextBoxMessage.Name = "TextBoxMessage"
        Me.TextBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxMessage.Size = New System.Drawing.Size(821, 324)
        Me.TextBoxMessage.TabIndex = 0
        '
        'ButtonSubmit
        '
        Me.ButtonSubmit.Location = New System.Drawing.Point(365, 347)
        Me.ButtonSubmit.Name = "ButtonSubmit"
        Me.ButtonSubmit.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSubmit.TabIndex = 1
        Me.ButtonSubmit.Text = "Submit"
        Me.ButtonSubmit.UseVisualStyleBackColor = True
        '
        'FormMessage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(833, 382)
        Me.Controls.Add(Me.ButtonSubmit)
        Me.Controls.Add(Me.TextBoxMessage)
        Me.Name = "FormMessage"
        Me.Text = "FormMessage"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBoxMessage As System.Windows.Forms.TextBox
    Friend WithEvents ButtonSubmit As System.Windows.Forms.Button
End Class
