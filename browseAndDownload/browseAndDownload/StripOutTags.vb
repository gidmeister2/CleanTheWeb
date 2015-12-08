Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions
Public Class StripOutTags

    Public Shared Function StripTagsRegex(source As String) As String
        ' remove all html tags using a regular expression
        Return Regex.Replace(source, "<.*?>", String.Empty)
    End Function

    ''' <summary>
    ''' Compiled regular expression for performance.
    ''' </summary>
    Shared _htmlRegex As New Regex("<.*?>", RegexOptions.Compiled)

    ''' <summary>
    ''' Remove HTML from string with compiled Regex.
    ''' </summary>
    Public Shared Function StripTagsRegexCompiled(source As String) As String
        ' another way to remove tags
        Return _htmlRegex.Replace(source, String.Empty)
    End Function
    Public Shared Function removeblanklines(source As String) As String
        Dim strLines() As String
        Dim strLines2() As String
        Dim strResult As String
        Dim i As Integer
        strLines = source.Split(vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
        ReDim strLines2(strLines.Length)
        Dim j As Integer = 0
        For i = 0 To strLines.Length - 1
            strLines(i) = strLines(i).Trim
            If strLines(i).Length > 0 Then
                strLines2(j) = strLines(i)
                j = j + 1
            End If
        Next

        If j = 0 Then
            strResult = ""
        Else
            ReDim Preserve strLines2(j - 1)
            strResult = String.Join(vbCrLf.ToString, strLines2)
            ' above joins the non-blank lines again with a CarriageReturn-LineFeed between each
        End If

        Return strResult
    End Function
    ''' <summary>
    ''' Remove HTML tags from string using char array.
    ''' </summary>
    Public Shared Function StripTagsCharArray(source As String) As String

        ' yet another way to remove tags
        Dim array As Char() = New Char(source.Length - 1) {}
        Dim arrayIndex As Integer = 0
        Dim inside As Boolean = False

        For i As Integer = 0 To source.Length - 1
            Dim [let] As Char = source(i)
            If [let] = "<"c Then
                inside = True
                Continue For
            End If
            If [let] = ">"c Then
                inside = False
                Continue For
            End If
            If Not inside Then
                array(arrayIndex) = [let]
                arrayIndex += 1
            End If
        Next
        Return New String(array, 0, arrayIndex)
    End Function
End Class
