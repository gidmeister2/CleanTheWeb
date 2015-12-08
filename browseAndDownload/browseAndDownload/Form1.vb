Imports System.Text
Imports System.IO
Public Class Form1
    Private Sub ButtonHelp_Click(sender As Object, e As EventArgs) Handles ButtonHelp.Click
        Dim sb As New StringBuilder("")

        sb.AppendLine("Please select (by clicking) any extensions of filetypes that you want to appear, for instance, if you select 'jpg', then you will see a list of any jpeg-images that are referred to from the webpage.")
        sb.AppendLine("You also have the option to view files that have no extension at all.  There is a separate checkbox for that option.")
        sb.AppendLine("When you are finished, click NEXT to proceed.")
        MessageBox.Show(sb.ToString)
    End Sub
    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        Me.Hide()
        Form3.ShowDialog()
        Me.Show()
        ReadExtensions()
    End Sub
    Private Sub SaveExtensions()
        ' the list of extensions that the users checks are saved in a file, and when the user revisits the program, they are pre-checked, in case
        ' he is using the same configuration again.
        Dim bitarray(CheckedListBoxExtensions.Items.Count - 1) As Byte
        Dim i As Integer
        For i = 0 To bitarray.Length - 1
            If CheckedListBoxExtensions.GetItemChecked(i) = True Then
                bitarray(i) = 1
            End If
        Next
        File.WriteAllBytes(GlobalVariablesClass.uniquesession("ExtensionList.txt"), bitarray)
        Dim bitarray2(0) As Byte
        If Me.CheckBoxLackExt.Checked Then
            bitarray2(0) = 1
        End If
        File.WriteAllBytes(GlobalVariablesClass.uniquesession("form1extras.txt"), bitarray2)
    End Sub
    Private Sub ReadExtensions()
        ' read extensions from the extensions file, if one was saved in a prior session.
        If Not File.Exists(GlobalVariablesClass.uniquesession("ExtensionList.txt")) Then
            Exit Sub
        End If
        Dim bitarray(CheckedListBoxExtensions.Items.Count - 1) As Byte

        bitarray = File.ReadAllBytes(GlobalVariablesClass.uniquesession("ExtensionList.txt"))
        Dim i As Integer
        For i = 0 To bitarray.Length - 1
            If bitarray(i) = 1 Then
                CheckedListBoxExtensions.SetItemChecked(i, True)
            Else
                CheckedListBoxExtensions.SetItemChecked(i, False)
            End If
        Next
        If File.Exists(GlobalVariablesClass.uniquesession("form1extras.txt")) Then
            Dim bitarray2(0) As Byte
            bitarray2 = File.ReadAllBytes(GlobalVariablesClass.uniquesession("form1extras.txt"))
            If bitarray2(0) = 1 Then
                Me.CheckBoxLackExt.Checked = True
            End If
        End If
    End Sub
    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        SaveExtensions()
      

        GlobalVariablesClass.pAlsoNoExt = Me.CheckBoxLackExt.Checked ' also search for urls that have no period followed by an extension
        If Me.CheckedListBoxExtensions.CheckedItems.Count = 0 And _
          Not GlobalVariablesClass.pAlsoNoExt Then
            MessageBox.Show("Error: you forgot to select at least one extension.")
            Exit Sub
        End If
        GlobalVariablesClass.pExtensions.Clear()
        For i As Integer = 0 To CheckedListBoxExtensions.CheckedItems.Count - 1
            GlobalVariablesClass.pExtensions.Add(CheckedListBoxExtensions.CheckedItems(i))
        Next

        Me.Hide()
        Form5.ShowDialog()

    End Sub

   
  
   
End Class
