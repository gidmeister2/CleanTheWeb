<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
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
        Me.TextBoxLONGurl = New System.Windows.Forms.TextBox()
        Me.ButtonSubmit = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ButtonBrowse = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ButtonHelp = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 18)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(486, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Type in the URL of the web page that contains all the references to the pages you" & _
    " wish to download. "
        '
        'TextBoxLONGurl
        '
        Me.TextBoxLONGurl.Location = New System.Drawing.Point(19, 64)
        Me.TextBoxLONGurl.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBoxLONGurl.Name = "TextBoxLONGurl"
        Me.TextBoxLONGurl.Size = New System.Drawing.Size(693, 20)
        Me.TextBoxLONGurl.TabIndex = 1
        '
        'ButtonSubmit
        '
        Me.ButtonSubmit.Location = New System.Drawing.Point(321, 107)
        Me.ButtonSubmit.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonSubmit.Name = "ButtonSubmit"
        Me.ButtonSubmit.Size = New System.Drawing.Size(56, 19)
        Me.ButtonSubmit.TabIndex = 2
        Me.ButtonSubmit.Text = "Next >"
        Me.ButtonSubmit.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 40)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(268, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "You can also type the location of a file on your own PC." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'ButtonBrowse
        '
        Me.ButtonBrowse.Location = New System.Drawing.Point(728, 62)
        Me.ButtonBrowse.Name = "ButtonBrowse"
        Me.ButtonBrowse.Size = New System.Drawing.Size(75, 23)
        Me.ButtonBrowse.TabIndex = 4
        Me.ButtonBrowse.Text = "Browse"
        Me.ButtonBrowse.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Yellow
        Me.Label3.Location = New System.Drawing.Point(725, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "(local browse:)"
        '
        'ButtonHelp
        '
        Me.ButtonHelp.Location = New System.Drawing.Point(725, 107)
        Me.ButtonHelp.Name = "ButtonHelp"
        Me.ButtonHelp.Size = New System.Drawing.Size(75, 23)
        Me.ButtonHelp.TabIndex = 6
        Me.ButtonHelp.Text = "Help"
        Me.ButtonHelp.UseVisualStyleBackColor = True
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(865, 150)
        Me.Controls.Add(Me.ButtonHelp)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ButtonBrowse)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ButtonSubmit)
        Me.Controls.Add(Me.TextBoxLONGurl)
        Me.Controls.Add(Me.Label1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form3"
        Me.Text = "Get URL of the web page that has the links to the files"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxLONGurl As System.Windows.Forms.TextBox
    Friend WithEvents ButtonSubmit As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonBrowse As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonHelp As System.Windows.Forms.Button
End Class
