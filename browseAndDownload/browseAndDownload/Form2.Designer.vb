<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ButtonSaveAllUrls = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.LabelOR = New System.Windows.Forms.Label()
        Me.ButtonConcatenate = New System.Windows.Forms.Button()
        Me.ButtonUseLastSet = New System.Windows.Forms.Button()
        Me.TextBoxStartText = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBoxEndText = New System.Windows.Forms.TextBox()
        Me.LabelEndText = New System.Windows.Forms.Label()
        Me.RadioButtonDoNotFilter = New System.Windows.Forms.RadioButton()
        Me.RadioButtonDangerousTags = New System.Windows.Forms.RadioButton()
        Me.RadioButtonMostTags = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CheckBoxTables = New System.Windows.Forms.CheckBox()
        Me.CheckBoxStyles = New System.Windows.Forms.CheckBox()
        Me.CheckBoxHyperlinks = New System.Windows.Forms.CheckBox()
        Me.CheckBoxImages = New System.Windows.Forms.CheckBox()
        Me.CheckBoxLineBreaks = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CheckBoxCompleteEachURL = New System.Windows.Forms.CheckBox()
        Me.ButtonHelp = New System.Windows.Forms.Button()
        Me.ButtonBrowse = New System.Windows.Forms.Button()
        Me.ProgressBarMini = New System.Windows.Forms.ProgressBar()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(255, 23)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(269, 20)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Paste the URLs in the textbox below:"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(22, 56)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(727, 310)
        Me.TextBox1.TabIndex = 4
        '
        'ButtonSaveAllUrls
        '
        Me.ButtonSaveAllUrls.Location = New System.Drawing.Point(789, 410)
        Me.ButtonSaveAllUrls.Name = "ButtonSaveAllUrls"
        Me.ButtonSaveAllUrls.Size = New System.Drawing.Size(152, 23)
        Me.ButtonSaveAllUrls.TabIndex = 3
        Me.ButtonSaveAllUrls.Text = "Download Individually"
        Me.ButtonSaveAllUrls.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.BackColor = System.Drawing.Color.GreenYellow
        Me.ProgressBar1.Location = New System.Drawing.Point(22, 371)
        Me.ProgressBar1.Margin = New System.Windows.Forms.Padding(2)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(726, 19)
        Me.ProgressBar1.TabIndex = 6
        '
        'LabelOR
        '
        Me.LabelOR.AutoSize = True
        Me.LabelOR.Location = New System.Drawing.Point(738, 418)
        Me.LabelOR.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelOR.Name = "LabelOR"
        Me.LabelOR.Size = New System.Drawing.Size(23, 13)
        Me.LabelOR.TabIndex = 8
        Me.LabelOR.Text = "OR"
        '
        'ButtonConcatenate
        '
        Me.ButtonConcatenate.Location = New System.Drawing.Point(612, 412)
        Me.ButtonConcatenate.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonConcatenate.Name = "ButtonConcatenate"
        Me.ButtonConcatenate.Size = New System.Drawing.Size(92, 19)
        Me.ButtonConcatenate.TabIndex = 13
        Me.ButtonConcatenate.Text = "Concatenate"
        Me.ButtonConcatenate.UseVisualStyleBackColor = True
        '
        'ButtonUseLastSet
        '
        Me.ButtonUseLastSet.Location = New System.Drawing.Point(38, 20)
        Me.ButtonUseLastSet.Name = "ButtonUseLastSet"
        Me.ButtonUseLastSet.Size = New System.Drawing.Size(82, 23)
        Me.ButtonUseLastSet.TabIndex = 14
        Me.ButtonUseLastSet.Text = "Use Last Set"
        Me.ButtonUseLastSet.UseVisualStyleBackColor = True
        '
        'TextBoxStartText
        '
        Me.TextBoxStartText.Location = New System.Drawing.Point(9, 48)
        Me.TextBoxStartText.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBoxStartText.Name = "TextBoxStartText"
        Me.TextBoxStartText.Size = New System.Drawing.Size(158, 20)
        Me.TextBoxStartText.TabIndex = 14
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 33)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Start Text"
        '
        'TextBoxEndText
        '
        Me.TextBoxEndText.Location = New System.Drawing.Point(9, 98)
        Me.TextBoxEndText.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBoxEndText.Name = "TextBoxEndText"
        Me.TextBoxEndText.Size = New System.Drawing.Size(158, 20)
        Me.TextBoxEndText.TabIndex = 16
        '
        'LabelEndText
        '
        Me.LabelEndText.AutoSize = True
        Me.LabelEndText.Location = New System.Drawing.Point(17, 83)
        Me.LabelEndText.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelEndText.Name = "LabelEndText"
        Me.LabelEndText.Size = New System.Drawing.Size(50, 13)
        Me.LabelEndText.TabIndex = 17
        Me.LabelEndText.Text = "End Text"
        '
        'RadioButtonDoNotFilter
        '
        Me.RadioButtonDoNotFilter.AutoSize = True
        Me.RadioButtonDoNotFilter.Location = New System.Drawing.Point(47, 158)
        Me.RadioButtonDoNotFilter.Name = "RadioButtonDoNotFilter"
        Me.RadioButtonDoNotFilter.Size = New System.Drawing.Size(110, 17)
        Me.RadioButtonDoNotFilter.TabIndex = 18
        Me.RadioButtonDoNotFilter.TabStop = True
        Me.RadioButtonDoNotFilter.Text = "None (keep them)"
        Me.RadioButtonDoNotFilter.UseVisualStyleBackColor = True
        '
        'RadioButtonDangerousTags
        '
        Me.RadioButtonDangerousTags.AutoSize = True
        Me.RadioButtonDangerousTags.Location = New System.Drawing.Point(46, 181)
        Me.RadioButtonDangerousTags.Name = "RadioButtonDangerousTags"
        Me.RadioButtonDangerousTags.Size = New System.Drawing.Size(98, 17)
        Me.RadioButtonDangerousTags.TabIndex = 19
        Me.RadioButtonDangerousTags.TabStop = True
        Me.RadioButtonDangerousTags.Text = "dangerous tags"
        Me.RadioButtonDangerousTags.UseVisualStyleBackColor = True
        '
        'RadioButtonMostTags
        '
        Me.RadioButtonMostTags.AutoSize = True
        Me.RadioButtonMostTags.Location = New System.Drawing.Point(47, 216)
        Me.RadioButtonMostTags.Name = "RadioButtonMostTags"
        Me.RadioButtonMostTags.Size = New System.Drawing.Size(57, 17)
        Me.RadioButtonMostTags.TabIndex = 20
        Me.RadioButtonMostTags.TabStop = True
        Me.RadioButtonMostTags.Text = "All but:"
        Me.RadioButtonMostTags.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBoxTables)
        Me.GroupBox1.Controls.Add(Me.CheckBoxStyles)
        Me.GroupBox1.Controls.Add(Me.CheckBoxHyperlinks)
        Me.GroupBox1.Controls.Add(Me.CheckBoxImages)
        Me.GroupBox1.Controls.Add(Me.CheckBoxLineBreaks)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.RadioButtonMostTags)
        Me.GroupBox1.Controls.Add(Me.RadioButtonDangerousTags)
        Me.GroupBox1.Controls.Add(Me.RadioButtonDoNotFilter)
        Me.GroupBox1.Controls.Add(Me.LabelEndText)
        Me.GroupBox1.Controls.Add(Me.TextBoxEndText)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.TextBoxStartText)
        Me.GroupBox1.Location = New System.Drawing.Point(774, 10)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Size = New System.Drawing.Size(214, 329)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Concatenation controls"
        '
        'CheckBoxTables
        '
        Me.CheckBoxTables.AutoSize = True
        Me.CheckBoxTables.Location = New System.Drawing.Point(36, 285)
        Me.CheckBoxTables.Name = "CheckBoxTables"
        Me.CheckBoxTables.Size = New System.Drawing.Size(54, 17)
        Me.CheckBoxTables.TabIndex = 25
        Me.CheckBoxTables.Text = "tables"
        Me.CheckBoxTables.UseVisualStyleBackColor = True
        '
        'CheckBoxStyles
        '
        Me.CheckBoxStyles.AutoSize = True
        Me.CheckBoxStyles.Location = New System.Drawing.Point(36, 239)
        Me.CheckBoxStyles.Name = "CheckBoxStyles"
        Me.CheckBoxStyles.Size = New System.Drawing.Size(54, 17)
        Me.CheckBoxStyles.TabIndex = 24
        Me.CheckBoxStyles.Text = "Styles"
        Me.CheckBoxStyles.UseVisualStyleBackColor = True
        '
        'CheckBoxHyperlinks
        '
        Me.CheckBoxHyperlinks.AutoSize = True
        Me.CheckBoxHyperlinks.Location = New System.Drawing.Point(96, 262)
        Me.CheckBoxHyperlinks.Name = "CheckBoxHyperlinks"
        Me.CheckBoxHyperlinks.Size = New System.Drawing.Size(75, 17)
        Me.CheckBoxHyperlinks.TabIndex = 23
        Me.CheckBoxHyperlinks.Text = "Hyperlinks"
        Me.CheckBoxHyperlinks.UseVisualStyleBackColor = True
        '
        'CheckBoxImages
        '
        Me.CheckBoxImages.AutoSize = True
        Me.CheckBoxImages.Location = New System.Drawing.Point(36, 262)
        Me.CheckBoxImages.Name = "CheckBoxImages"
        Me.CheckBoxImages.Size = New System.Drawing.Size(59, 17)
        Me.CheckBoxImages.TabIndex = 22
        Me.CheckBoxImages.Text = "images"
        Me.CheckBoxImages.UseVisualStyleBackColor = True
        '
        'CheckBoxLineBreaks
        '
        Me.CheckBoxLineBreaks.AutoSize = True
        Me.CheckBoxLineBreaks.Location = New System.Drawing.Point(96, 239)
        Me.CheckBoxLineBreaks.Name = "CheckBoxLineBreaks"
        Me.CheckBoxLineBreaks.Size = New System.Drawing.Size(82, 17)
        Me.CheckBoxLineBreaks.TabIndex = 21
        Me.CheckBoxLineBreaks.Text = "Line Breaks"
        Me.CheckBoxLineBreaks.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 131)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Filter which tags?"
        '
        'CheckBoxCompleteEachURL
        '
        Me.CheckBoxCompleteEachURL.AutoSize = True
        Me.CheckBoxCompleteEachURL.Location = New System.Drawing.Point(791, 362)
        Me.CheckBoxCompleteEachURL.Name = "CheckBoxCompleteEachURL"
        Me.CheckBoxCompleteEachURL.Size = New System.Drawing.Size(123, 17)
        Me.CheckBoxCompleteEachURL.TabIndex = 15
        Me.CheckBoxCompleteEachURL.Text = "Complete Each URL"
        Me.CheckBoxCompleteEachURL.UseVisualStyleBackColor = True
        '
        'ButtonHelp
        '
        Me.ButtonHelp.Location = New System.Drawing.Point(484, 412)
        Me.ButtonHelp.Name = "ButtonHelp"
        Me.ButtonHelp.Size = New System.Drawing.Size(75, 19)
        Me.ButtonHelp.TabIndex = 16
        Me.ButtonHelp.Text = "HELP"
        Me.ButtonHelp.UseVisualStyleBackColor = True
        '
        'ButtonBrowse
        '
        Me.ButtonBrowse.Location = New System.Drawing.Point(612, 20)
        Me.ButtonBrowse.Name = "ButtonBrowse"
        Me.ButtonBrowse.Size = New System.Drawing.Size(75, 23)
        Me.ButtonBrowse.TabIndex = 17
        Me.ButtonBrowse.Text = "Browse"
        Me.ButtonBrowse.UseVisualStyleBackColor = True
        '
        'ProgressBarMini
        '
        Me.ProgressBarMini.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ProgressBarMini.Location = New System.Drawing.Point(22, 407)
        Me.ProgressBarMini.Name = "ProgressBarMini"
        Me.ProgressBarMini.Size = New System.Drawing.Size(129, 23)
        Me.ProgressBarMini.TabIndex = 18
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1047, 442)
        Me.Controls.Add(Me.ProgressBarMini)
        Me.Controls.Add(Me.ButtonBrowse)
        Me.Controls.Add(Me.ButtonHelp)
        Me.Controls.Add(Me.CheckBoxCompleteEachURL)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ButtonUseLastSet)
        Me.Controls.Add(Me.LabelOR)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonConcatenate)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ButtonSaveAllUrls)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form2"
        Me.Text = "Choose URLs to download"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonSaveAllUrls As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents LabelOR As System.Windows.Forms.Label
    Friend WithEvents ButtonConcatenate As System.Windows.Forms.Button
    Friend WithEvents ButtonUseLastSet As System.Windows.Forms.Button
    Friend WithEvents TextBoxStartText As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBoxEndText As System.Windows.Forms.TextBox
    Friend WithEvents LabelEndText As System.Windows.Forms.Label
    Friend WithEvents RadioButtonDoNotFilter As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonDangerousTags As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonMostTags As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxCompleteEachURL As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxHyperlinks As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxImages As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxLineBreaks As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxStyles As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonHelp As System.Windows.Forms.Button
    Friend WithEvents ButtonBrowse As System.Windows.Forms.Button
    Friend WithEvents CheckBoxTables As System.Windows.Forms.CheckBox
    Friend WithEvents ProgressBarMini As System.Windows.Forms.ProgressBar
End Class
