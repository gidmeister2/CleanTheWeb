Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions
Imports System
Imports System.Net
Imports System.Collections.Generic
Imports System.ComponentModel
Public Class Form4

    Private Sub ButtonHelp_Click(sender As Object, e As EventArgs) Handles ButtonHelp.Click
        Dim sb As New stringbuilder("")
        sb.AppendLine("The page that has links may have only a small section of links that you are interested in. ")
        sb.AppendLine("Rather than be presented with every link on the page, you may be able to specify that you only want links between a certain start area and end area. ")
        sb.AppendLine("For instance, you might be only interested in the links in a paragraph about cars that appears in a page about all types of vehicles. ")
        sb.AppendLine("If that is the case, AND if there is a unique string of word(s) before and/or after the paragraph(s), you can specify those here, and ")
        sb.AppendLine("the program will search for links that occur between these two strings.  In the case of cars, there might be a start string such as 'vehicles with wheels'")
        sb.AppendLine("and perhaps and end-string such as 'ad for valvoline motor oil. ")
        sb.AppendLine("the search string is not case sensitive.  You can leave both fields blank, just one of the two fields blank, or fill in both of them.")
        MessageBox.Show(sb.ToString)
    End Sub
    Public Sub getListOfURLS()
        Dim pageHTML As String
        Dim strURL As String
        Dim localfile As Boolean

        strURL = GlobalVariablesClass.pURL

        If strURL(1) = ":" Then
            localfile = True
        Else
            localfile = False
        End If
        If localfile Then
            If My.Computer.FileSystem.FileExists(strURL) Then
                pageHTML = My.Computer.FileSystem.ReadAllText(strURL)
            Else
                MessageBox.Show("Error: the file you entered cannot be found.")
                Exit Sub
            End If
        Else
            pageHTML = GetURLContents(strURL)
        End If


        GlobalVariablesClass.pFoundTheseLinks = doRegex(pageHTML)
    End Sub
    Private Sub DirSearch(ByVal strContents As String, ByRef RegexObj As Regex, ByRef listRegexArg As List(Of String))

        Dim MatchObj As Match

        MatchObj = RegexObj.Match(strContents, 0)

        Dim count As Integer = 0
        While MatchObj.Success

            listRegexArg.Add(MatchObj.Value)

            MatchObj = MatchObj.NextMatch()
            count += 1
            If count > 1000 Then
                MessageBox.Show("too many matches in file ")
                Return
            End If
        End While

    End Sub
    Function combineUrlAndItem(ByVal theURL As String, ByVal stritem As String) As String
        ' combine a URL with a file name, for instance http://www.amazon.com with a filename such as GoneWiththeWind.htm or a partial path such as /images/roberta.jpg
        If stritem.ToLower.StartsWith("http") Then
            Return stritem
        End If
        Dim singleslashpos As Integer
        Dim doubleslashpos As Integer
        Dim firstslashpos As Integer
        Dim prefix As String = ""

        doubleslashpos = theURL.IndexOf("//")

        singleslashpos = theURL.LastIndexOf("/")

        firstslashpos = theURL.IndexOf("/", doubleslashpos + 2)
        If singleslashpos > doubleslashpos + 1 Then
            prefix = theURL.Substring(0, singleslashpos) ' lacks slash at end
        Else
            prefix = theURL
        End If
        If stritem.StartsWith("/") Then
            If firstslashpos > 0 Then
                prefix = theURL.Substring(0, firstslashpos)
            Else
                prefix = theURL
            End If
            Return prefix & stritem
        Else
            Return prefix & "/" & stritem
        End If
    End Function
    Function cleanup(ByVal inString As String) As String
        ' the regular expression search found strings such as <a href="microsoft.com/mobilephones.htm" >, but we want to just get the link (in this case: microsoft.com/mobilephones.htm"
        Dim instrlower As String
        Dim hrefpos As Integer
        instrlower = inString.ToLower
        If instrlower.StartsWith("<a") Or instrlower.StartsWith("<link") Then
            hrefpos = instrlower.IndexOf("href")

        ElseIf instrlower.StartsWith("<script") Or instrlower.StartsWith("<img") Then
            hrefpos = instrlower.IndexOf("src")
        End If
        If hrefpos < 0 Then
            Return ("")
        End If
        Dim startquotepos As Integer
        Dim endquotepos As Integer
        startquotepos = instrlower.IndexOf(Chr(34), hrefpos + 3)
        If startquotepos < 0 Then
            Return ("")
        End If
        endquotepos = instrlower.IndexOf(Chr(34), startquotepos + 1)
        If endquotepos < 0 Then
            Return ("")
        End If
        Return (inString.Substring(startquotepos + 1, endquotepos - startquotepos - 1))
    End Function
    Function Kosher(ByVal inString As String) As Boolean
        Dim periodpos As Integer
        Dim lastSlashPos As Integer
        Dim i As Integer

        ' apply various tests to see if the link is OK.  For instance, 'bookmark' links (that start with a # sign) are not really links to files, and should not be included
        ' also, only files that have extensions that the user specified earlier should be stamped "kosher".
        inString = inString.ToLower.Trim
        lastSlashPos = inString.LastIndexOf("/")


        If inString.Length = 0 Then
            Return False
        End If
        If inString.StartsWith("#") Then
            Return False
        End If
        If GlobalVariablesClass.pAlsoNoExt Then
            periodpos = inString.IndexOf(".")
            If periodpos <= lastSlashPos Then
                Return True
            End If
        End If
        For i = 0 To GlobalVariablesClass.pExtensions.Count - 1
            If inString.EndsWith(GlobalVariablesClass.pExtensions(i)) Then
                Return (True)
            End If
        Next
        Return False
    End Function
    Private Function doRegex(ByVal strContents As String) As String
        Dim RegexObj As Regex
        Dim ErrInfo As String = ""
        Dim posStart As Integer = -1
        Dim posEnd As Integer = -1
        Dim thelen As Integer

        ' use a regular expression to find any link in this webpage.  Links can be ordinary anchor links (<a> tag), but they can also be
        ' to images, javascript scripts, and stylesheets
        If GlobalVariablesClass.pStartLinkSearch.Length > 0 Then
            thelen = strContents.Length
            posStart = strContents.IndexOf(GlobalVariablesClass.pStartLinkSearch, 0, thelen, StringComparison.CurrentCultureIgnoreCase)
            If posStart > -1 Then
                posStart = posStart + GlobalVariablesClass.pStartLinkSearch.Length
                strContents = strContents.Substring(posStart)
            End If
        End If

        If GlobalVariablesClass.pEndLinkSearch.Length > 0 Then
            thelen = strContents.Length

            posEnd = strContents.IndexOf(GlobalVariablesClass.pEndLinkSearch, 0, thelen, StringComparison.CurrentCultureIgnoreCase)
            If posEnd > -1 Then
                strContents = strContents.Substring(0, posEnd)
            End If
        End If

        Dim rOptions As New RegexOptions()
        Dim finalList As List(Of String)
        rOptions = RegexOptions.IgnoreCase

        ' match entire paragraphs.  newline is treated as a blank.
        rOptions = rOptions Or RegexOptions.Singleline

        Dim strsearch As String

        strsearch = "\<a\s+.*?\>|\<script\s+.*?\>|\<link\s+.*?\>|\<img\s+.*?\>"

        RegexObj = New Regex(strsearch, rOptions)
        finalList = New List(Of String)
        RegexMeat(strContents, RegexObj, finalList)
        If finalList.Count = 0 Then
            strsearch = "^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-‌​\.\?\,\'\/\\\+&amp;%\$#_]*)?$"

            RegexObj = New Regex(strsearch, rOptions)
            finalList = New List(Of String)
            RegexMeat(strContents, RegexObj, finalList)
        End If
        Dim AllVals() As String = finalList.ToArray

        Dim strReturn As String = Join(AllVals, vbCrLf)
        Return strReturn

    End Function
    Sub RegexMeat(ByVal strContents As String, ByRef RegexObj As Regex, ByRef finalList As List(Of String))
        Dim listRegEx As New List(Of String)
        Me.DirSearch(strContents, RegexObj, listRegEx)

        Dim anotherList As New List(Of String)
        Dim i As Integer
        Dim stritem As String
        For i = 0 To listRegEx.Count - 1
            stritem = cleanup(listRegEx(i))
            If Kosher(stritem) Then
                stritem = combineUrlAndItem(GlobalVariablesClass.pURL, stritem)
                anotherList.Add(stritem)
            End If
        Next
        ' attempt to get rid of duplicate urls
        anotherList.Sort()

        If anotherList.Count > 0 Then
            finalList.Add(anotherList(0))
        End If
        For i = 1 To anotherList.Count - 1
            If anotherList(i) <> anotherList(i - 1) Then
                finalList.Add(anotherList(i))
            End If
        Next
    End Sub
    Public Function GetURLContents(ByVal strURL) As String
        ' download the contents of the webpage pointed to by the URL to a buffer, and return the buffer
        Dim responseFromServer As String = ""
        Try
            ' Create a request for the URL. 		
            Dim request As WebRequest = WebRequest.Create(strURL)
            ' If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials
            ' Get the response.
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            ' Display the status.
            ' Get the stream containing content returned by the server.
            Dim dataStream As Stream = response.GetResponseStream()
            ' Open the stream using a StreamReader for easy access.
            Dim reader As New StreamReader(dataStream)
            ' Read the content.
            responseFromServer = reader.ReadToEnd()
            ' Display the content.

            reader.Close()
            dataStream.Close()
            response.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Return responseFromServer
    End Function 'Main
    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        Dim strstart As String = ""
        Dim strEnd As String = ""

        strstart = Me.TextBoxStartText.Text.Trim
        strEnd = Me.TextBoxEndText.Text.Trim
        GlobalVariablesClass.pStartLinkSearch = strstart
        GlobalVariablesClass.pEndLinkSearch = strEnd
        ' save the settings in case you want to use them again
        My.Computer.FileSystem.WriteAllText(GlobalVariablesClass.uniquesession("linkstartphrase.txt"), strstart, False)
        My.Computer.FileSystem.WriteAllText(GlobalVariablesClass.uniquesession("linkendphrase.txt"), strEnd, False)
        getListOfURLS()
        Me.Hide()
        Form2.ShowDialog()
    End Sub

    Private Sub Form4_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.CenterToScreen()
        Dim strstart As String = String.Empty

        If My.Computer.FileSystem.FileExists(GlobalVariablesClass.uniquesession("linkstartphrase.txt")) Then
            strstart = My.Computer.FileSystem.ReadAllText(GlobalVariablesClass.uniquesession("linkstartphrase.txt"))
        End If
        If strstart.Length > 0 Then
            Me.TextBoxStartText.Text = strstart
        End If
        Dim strend As String = String.Empty

        If My.Computer.FileSystem.FileExists(GlobalVariablesClass.uniquesession("linkendphrase.txt")) Then
            strend = My.Computer.FileSystem.ReadAllText(GlobalVariablesClass.uniquesession("linkendphrase.txt"))
        End If
        If strend.Length > 0 Then
            Me.TextBoxEndText.Text = strend
        End If
    End Sub
End Class