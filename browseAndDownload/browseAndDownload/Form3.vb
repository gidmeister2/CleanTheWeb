Imports System
Imports System.Net
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Text.RegularExpressions
Public Class Form3

    Private Sub Form3_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub
    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        Dim strUrl As String = String.Empty

        If My.Computer.FileSystem.FileExists("saveurl.txt") Then
            strUrl = My.Computer.FileSystem.ReadAllText("saveurl.txt")
            ' The user's most recently used url goes here, since this is the first form he sees.  In other words, if the last time he used the program
            ' he visited Microsoft.com, that is currently in 'saveurl.txt'
        End If
        If strUrl.Length > 0 Then
            Me.TextBoxLONGurl.Text = strUrl
        End If
    End Sub
 

    Private Function GetFileContents(FullPath As String, ByRef ErrInfo As String) As String
        Dim strContents As String = ""
        ErrInfo = ""
        Dim objReader As StreamReader

        Try

            objReader = New StreamReader(FullPath)

            strContents = objReader.ReadToEnd()
            objReader.Close()
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return (strContents)
    End Function

   

    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click

        ' the user enters a URL (or perhaps browses for a local file on his own PC instead).  So we get the URL here, (or the filename)
        ' and then call 'doRegex' to find just the list of links in it, which we in turn save in a global variable "GlobalVariablesClass.pFoundTheseLinks"
        ' we also save the user's original URL to a file, to make it more convenient the next time he uses this program - the URL-field on this form will be
        ' pre-filled in.  He can always replace it by another, of course.
        Dim strURL As String
        strURL = Me.TextBoxLONGurl.Text.Trim
        If strURL.Length = 0 Then
            MessageBox.Show("Error: you did not enter a URL")
            Exit Sub
        End If
        Dim localfile As Boolean
        If strURL(1) = ":" Then
            localfile = True
        Else
            localfile = False
            If Not strURL.ToLower.StartsWith("http") Then
                strURL = "http://" & strURL
            End If
        End If
        GlobalVariablesClass.pURL = strURL
        My.Computer.FileSystem.WriteAllText(GlobalVariablesClass.uniquesession("saveurl.txt"), strURL, False)
        My.Computer.FileSystem.WriteAllText("saveurl.txt", strURL, False) ' most recently used url goes here, since this is the first form

        Me.Hide()

    End Sub
 
 

    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        Dim openFileDialog1 As New OpenFileDialog()

        ' The user can decide to browse for a local file on his pc that has a list of URLs in it.  It might be part of a webpage he created himself for instance.
        ' the links in that file will be found and listed so he can download them off the web.
        openFileDialog1.Filter = "htm files (*.htm)|*.htm;*.html|txt files (*.txt)|*.txt|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 1
        openFileDialog1.RestoreDirectory = False

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Me.TextBoxLONGurl.Text = openFileDialog1.FileName
        End If
    End Sub

    Private Sub ButtonHelp_Click(sender As Object, e As EventArgs) Handles ButtonHelp.Click
        Dim sb As New StringBuilder("")
        sb.AppendLine("Here you put in the URL of the webpage that has the links in it.  (example: http://www.nypost.com).  ")
        sb.AppendLine("An alternative is if you have a webpage that you have put on your local PC. In that case, click the Browse button to locate it and it will fill in the text field.")
        MessageBox.Show(sb.ToString)
    End Sub
End Class