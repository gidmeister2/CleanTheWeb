Imports System.Text
Imports System.IO
Public Class Form5

    Private Sub ButtonHelp_Click(sender As Object, e As EventArgs) Handles ButtonHelp.Click
        Dim sb As New StringBuilder("")
        sb.AppendLine("This program downloads web-pages, and removes dangerous tags (that can run scripts on your PC).  It also reduces clutter.")
        sb.Append("However, many webpages are sloppy, and their syntax is flawed.  For instance, tags often should come in pairs:")
        sb.AppendLine(" a Paragraph tag should be matched by an end-Paragraph tag (<p> should be matched by </p>).  Sometimes web creators don't do this.")
        sb.AppendLine("Other times, they don't make sure they nest tags symetrically.  Many browsers are forgiving of these kinds of errors, but you may have an application which is not.")
        sb.AppendLine("So the choices here are: allow these sloppy practices, and download and process the files anyway (but with a warning) or do not allow at all, or compromise by still downloading the files but prefixing their name with 'error_'.")
        sb.AppendLine("In the case of concatenated files, you can create two - one with the OK files, and the other with an error_prefix that contains the not-OK files.")
        MessageBox.Show(sb.ToString)
    End Sub

    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        If RadioButtonNoButSaveSeparate.Checked Then
            GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.separatefileForUnbalanced
        ElseIf RadioButtonNoDontInclude.Checked Then
            GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.forbidunbalanced
        ElseIf RadioButtonYesJustWarn.Checked Then
            GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.includeunbalanced
        End If
        Dim sfile As String = GlobalVariablesClass.uniquesession("strictness.txt")
        Dim bw As BinaryWriter

        Dim s As String = "I am happy"
        'create the file
        Try
            bw = New BinaryWriter(New FileStream(sfile, FileMode.Create))
        Catch ex As IOException
            ' Console.WriteLine(ex.Message + "\n Cannot create file.")
            Exit Sub
        End Try
        'writing into the file
        Try
            bw.Write(Convert.ToInt16(GlobalVariablesClass.pStrictness))

        Catch ex As IOException
            '  Console.WriteLine(ec.Message + "\n Cannot write to file.")
            Exit Sub
        End Try
        bw.Close()

        Me.Hide()
        Form4.ShowDialog()
    End Sub

    Private Sub Form5_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.CenterToScreen()
        Dim sfile As String = GlobalVariablesClass.uniquesession("strictness.txt")
        Dim br As BinaryReader

        If My.Computer.FileSystem.FileExists(sfile) Then

            br = New BinaryReader(New FileStream(sfile, FileMode.Open))

            GlobalVariablesClass.pStrictness = br.ReadInt16()

            br.Close()

        End If
        Select Case GlobalVariablesClass.pStrictness
            Case GlobalVariablesClass.enumhowstrict.forbidunbalanced
                RadioButtonNoDontInclude.Checked = 1
            Case GlobalVariablesClass.enumhowstrict.includeunbalanced
                RadioButtonYesJustWarn.Checked = 1
            Case GlobalVariablesClass.enumhowstrict.separatefileForUnbalanced
                RadioButtonNoButSaveSeparate.Checked = 1
        End Select
    End Sub
End Class