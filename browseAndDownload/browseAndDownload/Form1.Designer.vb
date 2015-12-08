<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.LabelWhichExtensions = New System.Windows.Forms.Label()
        Me.ButtonSubmit = New System.Windows.Forms.Button()
        Me.LabelInfo = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.CheckBoxLackExt = New System.Windows.Forms.CheckBox()
        Me.CheckedListBoxExtensions = New System.Windows.Forms.CheckedListBox()
        Me.ButtonHelp = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LabelWhichExtensions
        '
        Me.LabelWhichExtensions.AutoSize = True
        Me.LabelWhichExtensions.Location = New System.Drawing.Point(59, 116)
        Me.LabelWhichExtensions.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelWhichExtensions.Name = "LabelWhichExtensions"
        Me.LabelWhichExtensions.Size = New System.Drawing.Size(221, 13)
        Me.LabelWhichExtensions.TabIndex = 4
        Me.LabelWhichExtensions.Text = "Which Extensions do you want to search for?"
        '
        'ButtonSubmit
        '
        Me.ButtonSubmit.Location = New System.Drawing.Point(224, 225)
        Me.ButtonSubmit.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonSubmit.Name = "ButtonSubmit"
        Me.ButtonSubmit.Size = New System.Drawing.Size(56, 19)
        Me.ButtonSubmit.TabIndex = 5
        Me.ButtonSubmit.Text = "Next >"
        Me.ButtonSubmit.UseVisualStyleBackColor = True
        '
        'LabelInfo
        '
        Me.LabelInfo.AutoSize = True
        Me.LabelInfo.ForeColor = System.Drawing.Color.Maroon
        Me.LabelInfo.Location = New System.Drawing.Point(84, 22)
        Me.LabelInfo.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelInfo.Name = "LabelInfo"
        Me.LabelInfo.Size = New System.Drawing.Size(0, 13)
        Me.LabelInfo.TabIndex = 6
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.Coral
        Me.TextBox1.Location = New System.Drawing.Point(34, 22)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(440, 71)
        Me.TextBox1.TabIndex = 7
        Me.TextBox1.Text = resources.GetString("TextBox1.Text")
        '
        'CheckBoxLackExt
        '
        Me.CheckBoxLackExt.AutoSize = True
        Me.CheckBoxLackExt.Location = New System.Drawing.Point(188, 179)
        Me.CheckBoxLackExt.Margin = New System.Windows.Forms.Padding(2)
        Me.CheckBoxLackExt.Name = "CheckBoxLackExt"
        Me.CheckBoxLackExt.Size = New System.Drawing.Size(174, 17)
        Me.CheckBoxLackExt.TabIndex = 8
        Me.CheckBoxLackExt.Text = "Also files that lack an extension"
        Me.CheckBoxLackExt.UseVisualStyleBackColor = True
        '
        'CheckedListBoxExtensions
        '
        Me.CheckedListBoxExtensions.FormattingEnabled = True
        Me.CheckedListBoxExtensions.Items.AddRange(New Object() {"pdf", "js", "css", "jpg", "png", "gif", "txt", "htm", "html", "aspx", "/"})
        Me.CheckedListBoxExtensions.Location = New System.Drawing.Point(62, 145)
        Me.CheckedListBoxExtensions.Margin = New System.Windows.Forms.Padding(2)
        Me.CheckedListBoxExtensions.Name = "CheckedListBoxExtensions"
        Me.CheckedListBoxExtensions.Size = New System.Drawing.Size(101, 169)
        Me.CheckedListBoxExtensions.TabIndex = 9
        '
        'ButtonHelp
        '
        Me.ButtonHelp.Location = New System.Drawing.Point(340, 223)
        Me.ButtonHelp.Name = "ButtonHelp"
        Me.ButtonHelp.Size = New System.Drawing.Size(75, 23)
        Me.ButtonHelp.TabIndex = 10
        Me.ButtonHelp.Text = "Help"
        Me.ButtonHelp.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(512, 348)
        Me.Controls.Add(Me.ButtonHelp)
        Me.Controls.Add(Me.CheckedListBoxExtensions)
        Me.Controls.Add(Me.CheckBoxLackExt)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.LabelInfo)
        Me.Controls.Add(Me.ButtonSubmit)
        Me.Controls.Add(Me.LabelWhichExtensions)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "Obtain URLS"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelWhichExtensions As System.Windows.Forms.Label
    Friend WithEvents ButtonSubmit As System.Windows.Forms.Button
    Friend WithEvents LabelInfo As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxLackExt As System.Windows.Forms.CheckBox
    Friend WithEvents CheckedListBoxExtensions As System.Windows.Forms.CheckedListBox
    Friend WithEvents ButtonHelp As System.Windows.Forms.Button

End Class
