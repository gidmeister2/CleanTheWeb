Imports System.Text
Public Class Form0

    Private Sub ButtonHelp_Click(sender As Object, e As EventArgs) Handles ButtonHelp.Click
        Dim sb As New StringBuilder("")
        sb.AppendLine("Click 'Scan' if you want to view a list of links from a web-page some or all of which that you intend to download.")
        sb.AppendLine("Click 'Paste' if instead, you want to paste in (or type) one or more links that you intend to download.")
        sb.AppendLine("When you are finished, click NEXT to proceed.")
        MessageBox.Show(sb.ToString)
    End Sub

    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click

        If Me.RadioButtonPaste.Checked Then
            GlobalVariablesClass.pOperation = GlobalVariablesClass.enumOperation.paste
        ElseIf Me.RadioButtonScan.Checked Then
            GlobalVariablesClass.pOperation = GlobalVariablesClass.enumOperation.scan
        Else
            MessageBox.Show("Error: you forgot to choose to either scan or paste.")
            Exit Sub
        End If
      
        If GlobalVariablesClass.pOperation = GlobalVariablesClass.enumOperation.paste Then
            Me.Hide() ' I'm not sure that hiding a form is a good idea, but I do this for all the forms.  The program might never end if a hidden form is open.
            Form2.ShowDialog()
        ElseIf GlobalVariablesClass.pOperation = GlobalVariablesClass.enumOperation.scan Then
            Me.Hide()
            Form1.ShowDialog()
        End If
    End Sub

    Private Sub Form0_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub

    Private Sub Form0_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()


    End Sub
End Class