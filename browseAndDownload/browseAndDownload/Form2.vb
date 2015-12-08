Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Net.Mail
Public Class Form2
    Public selpath As String
    Public fnameid As Integer = 0
    Public concat As Boolean
    Public filecounter As Integer = 0
    Public startphrase As String = ""
    Public endphrase As String = ""
    Public concatname As String = ""
    Private Sub ButtonHelp_Click(sender As Object, e As EventArgs) Handles ButtonHelp.Click

        Dim sb As New StringBuilder("")
        sb.AppendLine("Here you can either paste some URLs that you wish to download, or if the list already appears, you can, if you wish, rearange some, remove some, modify some, and add some links. You can have blank lines between links (URLS).")
        sb.AppendLine("You can have each on its own line, or you can separate them with commas or semicolons.  If they are surrounded by double-quotes, the program will read them correctly anyway.")
        sb.AppendLine("If you used this program earlier and want to redownload a set of URLS that you already put in, you should click on 'Use Last Set'.")
        sb.AppendLine("There are two ways to download the files.  You can click on 'Download Individually', which will download each file separately, or you can click on 'Concatenate', which will merge all the files into one file.")
        sb.AppendLine("If the pages have extra material that you do not want, you can specify a text string to start at, and/or a text string to end at.  For example, if you are downloading a webpage that has a big introduction that you don't need, but you want to get everything after the words 'Favorite Books', you would put that phrase in the 'Start Text' field.")
        sb.AppendLine("You can filter tags too, if you are downloading a webpage.  Some tags (such as 'script') can be dangerous, if they execute code on your PC and you don't fully trust the webpage desigener.  ")
        sb.AppendLine("So you have the choice to get rid of 'dangerous' tags, to get rid of no tags ('keep them'), or get rid of ALL tags with exceptions that you specify.  If you choose the latter choice, then a set of boxes will appear where you can put a checkmark next to the tags you want to keep.")
        sb.Append("There is also a browse button, in case you have a webpage that you want to simplify that is already on your PC (simplify in this context means that you get rid of some of tags in it)")
        MessageBox.Show(sb.ToString)
    End Sub
    Private Function prefix(ByVal part As Integer) As String
        ' if 'part' is less than 2 digits, prefix with zero.  So 8 becomes '08' and 10 becomes '10'
        Dim strpart As String
        strpart = part.ToString
        If strpart.Length = 1 Then
            strpart = "0" & strpart
        End If
        Return strpart
    End Function
    Private Function makeFriendlyNameFromDomain() As String
        ' to make a filename for the file where you concatenate webpages, you can use the domain name that the user specified earlier, and try to make
        ' a partial file name with it.
        Dim urlstr As String = GlobalVariablesClass.pURL.ToLower
        ' start with http://ruthfully.com/pizza.aspx'
        Dim slashpos As Integer = urlstr.IndexOf("//")
        If slashpos > -1 Then
            urlstr = urlstr.Substring(slashpos + 2)
        End If
        ' now have ruthfully.com/pizza.aspx
        Dim lastslashpos As Integer
        lastslashpos = urlstr.LastIndexOf("/")
        If lastslashpos > 0 Then
            urlstr = urlstr.Substring(0, lastslashpos)
        End If
        ' now have ruthfully.com
        Dim lastperiodpos As Integer
        lastperiodpos = urlstr.LastIndexOf(".")
        If lastperiodpos > 0 Then
            urlstr = urlstr.Substring(0, lastperiodpos)
        End If
        ' now have ruthfully
        Dim periodpos As Integer
        periodpos = urlstr.IndexOf(".")
        If periodpos > 0 Then
            urlstr = urlstr.Substring(periodpos + 1)
        End If
        urlstr = urlstr.Replace(".", "_")
        Return urlstr
    End Function

    Private Sub Form2_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub
    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dr As System.Windows.Forms.DialogResult

        Dim today As Date = Now
        Dim year As Integer = today.Year
        Dim month As Integer = today.Month
        Dim day As Integer = today.Day

     
        ' at this point we don't know if the user plans to concatenate the files he downloads, but in case he does, we try to come with a name for the produced file.
        If GlobalVariablesClass.pOperation = GlobalVariablesClass.enumOperation.scan Then
            If GlobalVariablesClass.pURL(1) <> ":" Then
                concatname = "concat" & makeFriendlyNameFromDomain() & year & prefix(month) & prefix(day)
            Else
                concatname = "concat" & year & prefix(month) & prefix(day)
            End If
            ' the filename can have the domain if you are scanning from a website
        Else
            concatname = "concat" & year & prefix(month) & prefix(day)
            ' the filename can't incorporate the domain if you are using a set of urls that you typed in to the textbox
        End If

            Me.CenterToScreen()
            Me.RadioButtonDoNotFilter.Checked = True
            Dim folderb As New FolderBrowserDialog
            folderb.Description = "Where should I save the downloaded files?"
            ' we have opened a common dialog that lets the user browse to the folder where he wants to save the produced files.
            ' if we have saved a path from a previous session, we navigate to it here in the dialog.  The user can of course change it.
            Dim savedPathIn As String = GlobalVariablesClass.uniquesession("savesearchpath.txt")
            If File.Exists(savedPathIn) Then
                Try
                    selpath = File.ReadAllText(savedPathIn)
                    folderb.SelectedPath = selpath
                Catch ex As Exception
                    ' do nothing
                End Try
            End If

            dr = folderb.ShowDialog()
        If dr = DialogResult.Cancel Or dr = DialogResult.Abort Then
            Application.Exit()
            Exit Sub
        End If

        selpath = folderb.SelectedPath
        ' save the path to make a future session more convenient for the users
        File.WriteAllText(savedPathIn, selpath)

        If GlobalVariablesClass.pOperation = GlobalVariablesClass.enumOperation.scan Then
            ' these are the links we downloaded in the previous form
            Me.TextBox1.Text = GlobalVariablesClass.pFoundTheseLinks
        End If
        Me.CheckBoxCompleteEachURL.Checked = GlobalVariablesClass.pCompleteTheURLs ' if true, then a link such as images/pizza.png becomes www.someDomain.com/images/pizza.png
        ' initialize the controls that let the user specify which tags to keep in each downloaded page.
        Me.CheckBoxLineBreaks.Checked = GlobalVariablesClass.pKeepLinebreaks '
        Me.CheckBoxImages.Checked = GlobalVariablesClass.pKeepImages
        Me.CheckBoxHyperlinks.Checked = GlobalVariablesClass.pKeepHyperlinks
        Me.CheckBoxStyles.Checked = GlobalVariablesClass.pKeepStyles
        Me.CheckBoxTables.Checked = GlobalVariablesClass.pKeepTables

        Select Case GlobalVariablesClass.pFilterOutTags
            Case GlobalVariablesClass.enumfilterwhat.DangerousTagsOnly
                Me.RadioButtonDangerousTags.Checked = True
            Case GlobalVariablesClass.enumfilterwhat.keep
                Me.RadioButtonDoNotFilter.Checked = True
            Case GlobalVariablesClass.enumfilterwhat.MostTags
                Me.RadioButtonMostTags.Checked = True
        End Select
        If GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.MostTags Then
            Me.CheckBoxHyperlinks.Visible = True
            Me.CheckBoxImages.Visible = True
            Me.CheckBoxLineBreaks.Visible = True
            Me.CheckBoxStyles.Visible = True
            Me.CheckBoxTables.Visible = True
        Else
            Me.CheckBoxHyperlinks.Visible = False
            Me.CheckBoxImages.Visible = False
            Me.CheckBoxLineBreaks.Visible = False
            Me.CheckBoxStyles.Visible = False
            Me.CheckBoxTables.Visible = False
        End If
        ' the above controls may have been set in a prior session, and if so, for user convenience, we obtain those old settings and use them in 'readKeep':
        readKeep()
        Dim sfile As String = GlobalVariablesClass.uniquesession("startphrase.txt")
        Dim efile As String = GlobalVariablesClass.uniquesession("endphrase.txt")
        If My.Computer.FileSystem.FileExists(sfile) Then
            ' if the user specified these phrases in a previous session, it is convenient to preset the controls with them here.
            Me.TextBoxStartText.Text = My.Computer.FileSystem.ReadAllText(sfile)
            Me.TextBoxEndText.Text = My.Computer.FileSystem.ReadAllText(efile)
        End If
        Me.Activate()
    End Sub

    Public Sub DownloadFileAndSave(ByVal strURL As String, ByRef structErrorCheck As FilterTagStringOperations.structalert, _
                                   ByRef structErrorFilter As FilterTagStringOperations.structalert, ByVal concat As Boolean, _
                                   ByVal islast As Boolean, ByVal ishtmlparam As Boolean, ByVal isfirst As Boolean)
        Dim backslashpos As Integer
        Dim fname As String
        FilterTagsNew.clearAlertStruct(structErrorFilter)
        FilterTagsNew.clearAlertStruct(structErrorCheck)
        Dim strExt As String = String.Empty
        Dim localfile As Boolean
        Dim otherfilename As String
        Dim returnedStrExt As String = String.Empty
        Dim localfilename As String
        Dim responseFromServer As String = ""
        Dim responsefromserverlowercase As String = ""
        Dim isolate As Boolean = False

        ' given a URL, download it to your PC, and then process it, if it is not an image or a pdf.
        Dim ishtml As Boolean
        If concat Then
            ishtml = ishtmlparam
        Else
            ishtml = deduceIfHTMLFromExtension(strURL)
        End If
        If strURL(1) = ":" Then
            localfile = True ' in other words, the url is not a web url, but is a file name on your own local PC, such as C:\politics\politicianbios.txt
        Else
            localfile = False
        End If
        GlobalVariablesClass.pCurrentlyWorkingOnURL = strURL
        fname = MakeFriendlyFileNameFromURL(strURL, returnedStrExt)
        If returnedStrExt.Length > 0 Then
            strExt = returnedStrExt
        elseIf fname.LastIndexOf(".") > -1 Then
            strExt = fname.ToLower.Substring(fname.LastIndexOf(".") + 1)
        Else
            If ishtml Then
                strExt = "htm"
            Else
                strExt = "txt"
            End If
        fname = fname & "." & strExt
        End If
        ' make the filename acceptable to computers
        fname = fname.Replace(":", "_")
        If fname.Contains("\") Then
            backslashpos = fname.LastIndexOf("\")
            fname = fname.Substring(backslashpos + 1)
        End If
        If Not downloadobjectIsLetters(strExt) Then
            ' if the object to be downloaded is not text, or html, but is rather a PDF or an image.
            Try
                Dim wc As New WebClient()

                Dim fullname As String = CombineLocalPathAndFile(selpath, fname)
                fullname = fullname.Replace("?", "_").Replace("=", "_")
                If My.Computer.FileSystem.FileExists(fullname) Then
                    ' if the file exists on your PC, it will be deleted and replaced without any warning to you.
                    My.Computer.FileSystem.DeleteFile(fullname)
                End If
                If localfile Then
                    ' copy the local file to file 'fullname'
                    If My.Computer.FileSystem.FileExists(strURL) Then
                        My.Computer.FileSystem.WriteAllText(fullname, My.Computer.FileSystem.ReadAllText(strURL), False)
                    Else
                        structErrorCheck.errorMessage = strURL & " does not exist on your PC!"
                        structErrorCheck.dangerlevel = FilterTagStringOperations.enumWarning.fatal
                        Exit Sub
                    End If
                Else
                    My.Computer.Network.DownloadFile(strURL, fullname)

                End If

            Catch ex As Exception
                structErrorCheck.errorMessage = fname & ": " & ex.Message
                structErrorCheck.dangerlevel = FilterTagStringOperations.enumWarning.fatal
            End Try
            Exit Sub
        End If

        If localfile Then
            If My.Computer.FileSystem.FileExists(strURL) Then
                responseFromServer = My.Computer.FileSystem.ReadAllText(strURL) ' in this case its not a responseFromServer, but I use the same variable name 
            Else
                structErrorCheck.errorMessage = strURL & " does not exist on your PC!"
                structErrorCheck.dangerlevel = FilterTagStringOperations.enumWarning.fatal
                Exit Sub
            End If
        Else
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
                reader.Close()
                dataStream.Close()
                response.Close()
            Catch ex As Exception
                structErrorCheck.errorMessage = fname & ": " & ex.Message
                structErrorCheck.dangerlevel = FilterTagStringOperations.enumWarning.fatal
                Exit Sub
            End Try
        End If

        If ishtml Then
            Dim prefix As String = ""
            Dim suffix As String = ""
            If Not concat Then
                fname = fname.Replace(":", "_")
                
                localfilename = CombineLocalPathAndFile(selpath, fname)

               
                If Not FilterTagsNew.couldfilter(responseFromServer, structErrorCheck, structErrorFilter, _
                                                 GlobalVariablesClass.pMaxWidth, GlobalVariablesClass.pMaxHeight) Then
                    localfilename = CombineLocalPathAndFile(selpath, GlobalVariablesClass.DangerPrefix & fname)
                ElseIf structErrorCheck.errorcode = FilterTagStringOperations.enumError.unbalanced Then
                    If GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.separatefileForUnbalanced Then
                        localfilename = CombineLocalPathAndFile(selpath, GlobalVariablesClass.UnbalancedPrefix & fname)
                    End If
                End If
                responsefromserverlowercase = responseFromServer.ToLower
                PruneBySearch(responsefromserverlowercase, responseFromServer, True)
                ' we check that all start tags are matched by end tags in a webpage.  Therefore, if we are given a html fragment that isn't an entire page, 
                ' we force a balance.
                If Not responsefromserverlowercase.Contains("<body") Then
                    prefix = "<html><body>"
                End If
                If Not responsefromserverlowercase.Contains("</body") Then
                    suffix = "</body></html>"
                End If
                responseFromServer = prefix & responseFromServer & suffix

                ' the next few lines are not really making a difference at the moment, because asking for a filename for every download file
                ' can annoy the user.
                '  otherfilename = InputBox("If this filename is not satisfactory, please overwrite it here:", "filename check", localfilename)
                otherfilename = localfilename
                If otherfilename.Length > 0 Then
                    File.WriteAllText(otherfilename, responseFromServer)
                Else
                    File.WriteAllText(localfilename, responseFromServer)
                End If

             
            Else
                ' remove any tags that the user doesn't want to have.
                localfilename = ""
                If Not FilterTagsNew.couldfilter(responseFromServer, structErrorCheck, structErrorFilter, _
                                                 GlobalVariablesClass.pMaxWidth, GlobalVariablesClass.pMaxHeight) Then
                    isolate = True
                    localfilename = CombineLocalPathAndFile(selpath, GlobalVariablesClass.DangerPrefix & fname)
                End If
                If structErrorCheck.errorcode = FilterTagStringOperations.enumError.unbalanced Then
                    If GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.separatefileForUnbalanced Then
                        isolate = True
                        localfilename = CombineLocalPathAndFile(selpath, GlobalVariablesClass.UnbalancedPrefix & fname)
                    End If
                End If
                responsefromserverlowercase = responseFromServer.ToLower
                ' get the section between body and /body
                PruneToInsideOfBody(responsefromserverlowercase, responseFromServer)
                ' if the user gave a startphrase and endphrase so that only part of the page will be saved, get that section here
                PruneBySearch(responsefromserverlowercase, responseFromServer, True)

                ' we could be downloading multiple files, and concatenating them, in which case the first file has to start with html and body, assuming we are
                ' constructing a giant web page consisting of all these files.
                If isfirst Or isolate Then
                    prefix = "<html><body>"
                End If
                If islast Or isolate Then
                    suffix = "</body></html>"
                End If
                If isolate Then
                    My.Computer.FileSystem.WriteAllText(localfilename, prefix & responseFromServer & suffix, False)
                Else
                    My.Computer.FileSystem.WriteAllText(concatname, prefix & responseFromServer & suffix, True)
                End If

            End If
        Else ' non html, perhaps text
            responsefromserverlowercase = responseFromServer.ToLower
            If Not concat Then
                localfilename = CombineLocalPathAndFile(selpath, fname)
                MakeSaferName(localfilename)
                PruneBySearch(responsefromserverlowercase, responseFromServer, False) ' (get segment bounded by startphrase and endphrase, if the user entered them)
                File.WriteAllText(localfilename, responseFromServer) ' write locally.
            Else
                PruneBySearch(responsefromserverlowercase, responseFromServer, False)
                My.Computer.FileSystem.WriteAllText(concatname, responseFromServer, True) ' append to the concatenated file
            End If
        End If

    End Sub 'Main
    Sub MakeSaferName(ByRef fname As String)
        ' a name that computers can handle.
        If fname.Length < 3 Then
            Exit Sub
        End If
        fname = fname.Substring(0, 2) & fname.Substring(2).Replace(":", "_")
    End Sub


    Sub SpecialShow(ByVal strError As String)
        Dim fm As New FormMessage
        fm.TextBoxMessage.Text = strError
        fm.ShowDialog()
    End Sub

    Function downloadobjectIsLetters(ByVal ext As String) As Boolean
        Dim textarray() As String = {"txt", "htm", "html", "css", "js", "aspx", "/", "php"}
        If textarray.Contains(ext) Then
            ' all these file types are made of letters, unlike binary files such as images.
            Return True
        ElseIf ext = String.Empty Then
            Return True ' we are making an assumption that if there is no extension, its a file made up of letters (like a webpage or a text file).
        Else
            Return False
        End If
    End Function
    Function downloadobjectIshtml(ByVal ext As String) As Boolean
        ' does the file type have html tags (usually) in it.
        Dim textarray() As String = {"htm", "php", "aspx", "/"} ' removed "html" since search for "htm" finds it
        Dim containsext As Boolean
        containsext = False
        Dim i As Integer
        For i = 0 To textarray.Length - 1
            If ext.StartsWith(textarray(i)) Then
                containsext = True
                Exit For
            End If
        Next
        If containsext Then
            Return True
        Else
            Return False
        End If
    End Function
    Function deduceIfHTMLFromExtension(ByVal urlstr As String) As Boolean
        Dim periodpos As Integer
        Dim slashpos As Integer
        Dim lowercasefname = urlstr.ToLower
        Dim localfile As Boolean
        If urlstr(1) = ":" Then
            localfile = True
        Else
            localfile = False
        End If

        If localfile Then
            slashpos = urlstr.LastIndexOf("\")
        Else
            slashpos = urlstr.LastIndexOf("/")
        End If

        If Not localfile Then
            If slashpos = urlstr.Length - 1 Then
                Return True
            End If
        End If

        periodpos = urlstr.LastIndexOf(".")
        If slashpos > -1 Then
            periodpos = urlstr.IndexOf(".", slashpos + 1)
        End If
        If periodpos > slashpos Then
            Return (downloadobjectIshtml(lowercasefname.Substring(periodpos + 1)))
        End If
        Return True
    End Function
    Private Sub SaveAllUrls()
        ' download every URL that is listed in the textbox, possibly each to its own file, or possibly concatenate them all.
        Dim urlArray As New List(Of String)
        Dim strAllContents As String
        Dim StartPosX As Integer = 0
        Dim urlStr As String = ""
        Dim ishtml As Boolean
        Dim sbWarn As New StringBuilder("")
        Dim sbErr As New StringBuilder("")
        Dim countErrors As Integer = 0
        Dim countWarnings As Integer = 0
        Dim count As Integer = 0
        Dim structErrorCheck As New FilterTagStringOperations.structalert
        Dim structErrorFilter As New FilterTagStringOperations.structalert
        Dim strInErrorText As String = ""
        Dim errorfilename As String
        Dim strFinalMessage As String
        Dim warnmsg As String = ""

        strAllContents = Me.TextBox1.Text.Trim

        Me.ProgressBar1.Minimum = 1
        Me.ProgressBar1.Step = 1

        Dim sbFromx As New StringBuilder(strAllContents)
        Do While getNextURL(sbFromx, StartPosX, urlStr)

            If urlStr.Length < 4 Then
                Continue Do
            End If
            If urlStr.ToUpper.Substring(0, 4) <> "HTTP" And urlStr(1) <> ":" Then
                Continue Do
            End If

            urlArray.Add(urlStr)
        Loop


        ' Filter distinct elements, and convert back into list, getting rid of duplicate urls (this may not work).
        urlArray = urlArray.Distinct().ToList

        Me.ProgressBar1.Maximum = urlArray.Count
        Dim i As Integer = 0
        For i = 0 To urlArray.Count - 1
            urlStr = urlArray(i)
            ishtml = deduceIfHTMLFromExtension(urlStr)

            If ishtml Then
                Exit For
            End If
        Next
        If ishtml Then
            concatname = concatname & ".htm"
        Else
            concatname = concatname & ".txt"
        End If
        concatname = CombineLocalPathAndFile(selpath, concatname)
        If concat Then
            Dim otherfilename As String
            otherfilename = InputBox("If the following filename is unsatisfactory, please replace it.  The results will be written to it on your PC", "filename check", _
                                     concatname)
            If otherfilename.Length > 0 Then
                concatname = otherfilename
            End If
        End If
        If My.Computer.FileSystem.FileExists(concatname) Then
            My.Computer.FileSystem.DeleteFile(concatname)
        End If
        Dim countunbalanced As Integer = 0
        ' download every file listed in the textbox, and possibly concatenate them, if that is what the user asked for.
        For i = 0 To urlArray.Count - 1
            urlStr = urlArray(i)
            Me.ProgressBar1.Value = count + 1
            ' note that the next routine can either write to a single file for every URL, or append to a big existing file.
            DownloadFileAndSave(urlStr, structErrorCheck, structErrorFilter, concat, i = (urlArray.Count - 1), ishtml, i = 0)

            If structErrorCheck.errorMessage.Length > 0 Then
                If structErrorCheck.dangerlevel = FilterTagStringOperations.enumWarning.fatal Then
                    countErrors = countErrors + 1
                    sbErr.AppendLine("Fatal error: " & countErrors.ToString & ") " & urlStr & " " & structErrorCheck.errorMessage)
                ElseIf structErrorFilter.dangerlevel = FilterTagStringOperations.enumWarning.fatal Then
                    countErrors = countErrors + 1
                    sbErr.AppendLine("Fatal error: " & countErrors.ToString & ") " & urlStr & " " & structErrorFilter.errorMessage)
                Else
                    countWarnings = countWarnings + 1
                    sbWarn.AppendLine("Warning: " & countWarnings.ToString & ") " & urlStr & " " & structErrorCheck.errorMessage)
                    If structErrorCheck.errorcode = FilterTagStringOperations.enumError.unbalanced Then
                        countunbalanced = countunbalanced + 1
                    End If
                End If
            ElseIf structErrorFilter.errorMessage.Length > 0 Then
                If structErrorFilter.dangerlevel = FilterTagStringOperations.enumWarning.fatal Then
                    countErrors = countErrors + 1
                    sbErr.AppendLine("Fatal error: " & countErrors.ToString & ") " & urlStr & " " & structErrorFilter.errorMessage)
                Else
                    countWarnings = countWarnings + 1
                    sbWarn.AppendLine("Warning: " & countWarnings.ToString & ") " & urlStr & " " & structErrorFilter.errorMessage)
                    If structErrorCheck.errorcode = FilterTagStringOperations.enumError.unbalanced Then
                        countunbalanced = countunbalanced + 1
                    End If
                End If
            End If

            count = count + 1
        Next

        Dim stradd2 As String = ""
        Dim stradd As String = ""
        If countunbalanced > 0 Then
            If GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.separatefileForUnbalanced Then
                stradd2 = " " & countunbalanced & " files were downloaded separately, since their tags were not balanced.  They have a prefix of " & GlobalVariablesClass.UnbalancedPrefix & "."
            End If
        End If

        If concat Then
            stradd = " Concatenated versions of text/html etc. files were saved in the file: " & concatname & "."
        End If

        errorfilename = CombineLocalPathAndFile(selpath, "errorlog.txt")
        File.Delete(errorfilename)
        strFinalMessage = 0
        If countWarnings = 0 And countErrors = 0 Then
            strFinalMessage = "The program has ended.  Your files are saved in: " & selpath & "." & stradd & " "
        ElseIf countWarnings = 0 And countErrors > 0 Then
            strFinalMessage = "The program has ended.  Your files are saved in: " & selpath & "." & stradd & _
                ".  However, we had some files with fatal syntax errors, and those we saved separately with a  " & GlobalVariablesClass.DangerPrefix & " prefix." & _
                " For more details, see " & errorfilename & "."
        ElseIf countWarnings > 0 And countErrors = 0 Then
            warnmsg = " There was some questionable syntax in " & countWarnings & " files, but they were saved anyway. " & stradd2
            strFinalMessage = "The program has ended.  Your files are saved in: " & selpath & "." & stradd & " " & warnmsg
        ElseIf countWarnings > 0 And countErrors > 0 Then
            warnmsg = " There were " & countErrors & " files with fatal syntax errors, so we saved them with a prefix of " & _
                GlobalVariablesClass.DangerPrefix & ". There was some questionable syntax in " & countWarnings & " files, but they were saved anyway. " & stradd2
            strFinalMessage = "The program has ended.  Your files are saved in: " & selpath & "." & stradd & " " & warnmsg & " For more details, see " & errorfilename & "."
        End If

        If countErrors > 0 Or countWarnings > 0 Then
            My.Computer.FileSystem.WriteAllText(errorfilename, sbErr.ToString & sbWarn.ToString, False)
        End If
 
        MessageBox.Show(strFinalMessage)
        If File.Exists(errorfilename) Then
            strInErrorText = My.Computer.FileSystem.ReadAllText(errorfilename).Trim
            If strInErrorText.Length > 0 Then
                MessageBox.Show("errorfile had this: " & strInErrorText)
            End If
        End If

        Application.Exit()
    End Sub

    Function CombineRemotePathAndFile(ByVal strPath As String, ByVal fname As String) As String
        Dim lastchar As String

        ' for instance http://microsoft.com plus myfile.aspx
        lastchar = strPath.Substring(strPath.Length - 1)
        If lastchar = "/" Then
            Return (strPath & fname)
        Else
            Return (strPath & "/" & fname)
        End If
    End Function
    Function CombineLocalPathAndFile(ByVal strPath As String, ByVal fname As String) As String
        Dim lastchar As String

        ' for instance c:\myfolder plus myfile.aspx
        If GlobalVariablesClass.pOperation = GlobalVariablesClass.enumOperation.scan Then
            If fname.IndexOf(".") < 0 Then
                fname = fname & ".htm"
            End If
        End If
        lastchar = strPath.Substring(strPath.Length - 1)
        If lastchar = "\" Then
            Return (strPath & fname)
        Else
            Return (strPath & "\" & fname)
        End If
    End Function
    Function getNextURL(ByRef sbFrom As StringBuilder, ByRef startPosition As Integer, ByRef strURL As String) As Boolean
        Dim mylen As Integer

        strURL = ""
        ' get the next URL in the textbox.  URL's don't have to be on separate lines, as long as there is white-space (or commas, or semicolons) between them.  
        ' one limitation is the URL's are not allowed to contain blanks - the following code would not work UNLESS you surround the URL with doublequotes.
        mylen = sbFrom.Length
        If startPosition >= mylen Then
            Return (False)
        End If
        Do While myWhite(sbFrom(startPosition))
            startPosition = startPosition + 1
            If startPosition >= mylen Then
                Return (False)
            End If
        Loop
        Do While Not myWhite(sbFrom(startPosition))
            strURL = strURL & sbFrom(startPosition)
            startPosition = startPosition + 1
            If startPosition >= mylen Then
                Return (True)
            End If

        Loop
        Return (True)
    End Function
    Function myWhite(ByVal myChar As Char) As Boolean
        If Char.IsWhiteSpace(myChar) Or myChar = "," Or myChar = ";" Or myChar = Chr(34) Then
            Return (True)
        End If
        Return (False)
    End Function
    Sub readKeep()
        '  "keepwhat.txt" stores a bit array that represents what checkboxes you have checked on a previous session (if there was one).
        Dim bitarr(10) As Byte
        If File.Exists(GlobalVariablesClass.uniquesession("keepwhat.txt")) Then
            bitarr = File.ReadAllBytes(GlobalVariablesClass.uniquesession("keepwhat.txt"))
        Else
            Exit Sub
        End If

        Me.RadioButtonDangerousTags.Checked = False

        Me.RadioButtonDoNotFilter.Checked = False

        Me.RadioButtonMostTags.Checked = False


        Me.CheckBoxCompleteEachURL.Checked = False

        Me.CheckBoxLineBreaks.Checked = False

        Me.CheckBoxStyles.Checked = False

        Me.CheckBoxImages.Checked = False

        Me.CheckBoxTables.Checked = False

        Me.CheckBoxHyperlinks.Checked = False

        Me.CheckBoxCompleteEachURL.Checked = False

        If bitarr(6) = 1 Then

            Me.RadioButtonDangerousTags.Checked = True
            Exit Sub
        ElseIf bitarr(7) = 1 Then
            Me.RadioButtonDoNotFilter.Checked = True
            Exit Sub
        ElseIf bitarr(8) = 1 Then
            Me.RadioButtonMostTags.Checked = True
        End If

        If bitarr(9) = 1 Then
            Me.CheckBoxCompleteEachURL.Checked = True
        End If
        RadioButtonMostTags_CheckedChanged(Nothing, Nothing)
        If bitarr(0) = 1 Then

            Me.CheckBoxCompleteEachURL.Checked = True

        End If
        If bitarr(1) = 1 Then
            Me.CheckBoxLineBreaks.Checked = True

        End If
        If bitarr(2) = 1 Then
            Me.CheckBoxStyles.Checked = True

        End If
        If bitarr(3) = 1 Then
            Me.CheckBoxImages.Checked = True

        End If
        If bitarr(4) = 1 Then
            Me.CheckBoxTables.Checked = True
        End If

        If bitarr(5) = 1 Then
            Me.CheckBoxHyperlinks.Checked = True
        End If



    End Sub
    Sub downloadall()
        My.Computer.FileSystem.WriteAllText("pasteset.txt", Me.TextBox1.Text.Trim, False)
        ' store the set of URLs from the textbox, in case you might want to use them
        ' in some future session (of using the program).   Once you do that, you call 'saveAllUrls', which downloads the URLS.  Note that this routine is called
        ' when the user clicks the CONCATENATE button or when he clicks the DOWNLOAD (individually) button.

        ' store other settings
        Dim bitarr(10) As Byte
        If Me.CheckBoxCompleteEachURL.Checked Then
            GlobalVariablesClass.pCompleteTheURLs = True
            bitarr(0) = 1
        Else
            GlobalVariablesClass.pCompleteTheURLs = False
        End If
        If Me.CheckBoxLineBreaks.Checked Then
            bitarr(1) = 1
            GlobalVariablesClass.pKeepLinebreaks = True
        Else
            GlobalVariablesClass.pKeepLinebreaks = False
        End If
        If Me.CheckBoxStyles.Checked Then
            bitarr(2) = 1
            GlobalVariablesClass.pKeepStyles = True
        Else
            GlobalVariablesClass.pKeepStyles = False
        End If
        If Me.CheckBoxImages.Checked Then
            bitarr(3) = 1
            GlobalVariablesClass.pKeepImages = True
        Else
            GlobalVariablesClass.pKeepImages = False
        End If
        If Me.CheckBoxTables.Checked Then
            bitarr(4) = 1
            GlobalVariablesClass.pKeepTables = True
        Else
            GlobalVariablesClass.pKeepTables = False
        End If

        If Me.CheckBoxHyperlinks.Checked Then
            bitarr(5) = 1
            GlobalVariablesClass.pKeepHyperlinks = True
        Else
            GlobalVariablesClass.pKeepHyperlinks = False
        End If
        startphrase = Me.TextBoxStartText.Text.Trim.ToLower
        endphrase = Me.TextBoxEndText.Text.Trim.ToLower
        Dim sfile As String = GlobalVariablesClass.uniquesession("startphrase.txt")
        Dim efile As String = GlobalVariablesClass.uniquesession("endphrase.txt")
        My.Computer.FileSystem.WriteAllText(sfile, startphrase, False)
        My.Computer.FileSystem.WriteAllText(efile, endphrase, False)
        If Me.RadioButtonDangerousTags.Checked Then
            GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.DangerousTagsOnly
            bitarr(6) = 1
        ElseIf Me.RadioButtonDoNotFilter.Checked Then
            GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.keep
            bitarr(7) = 1
        ElseIf Me.RadioButtonMostTags.Checked Then
            GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.MostTags ' aka ALL BUT
            bitarr(8) = 1
        End If
        If Me.CheckBoxCompleteEachURL.Checked Then
            bitarr(9) = 1
        End If
        File.WriteAllBytes(GlobalVariablesClass.uniquesession("keepwhat.txt"), bitarr)

        SaveAllUrls()
    End Sub
    Private Sub ButtonSaveAllUrls_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSaveAllUrls.Click
        Dim oldcolor As System.Drawing.Color

        ' download urls, each individually to a separate files, but do not concatenate them
        ' disable buttons so that they won't be hit twice by user while the download happens
        ' change the color of the button that was clicked, so the user remembers which button he clicked while the download is happening.
        oldcolor = Me.ButtonSaveAllUrls.BackColor
        Me.ButtonSaveAllUrls.BackColor = Color.Beige
        Me.ButtonSaveAllUrls.Enabled = False
        Me.ButtonConcatenate.Visible = False
        Me.LabelOR.Visible = False
        concat = False
        downloadall()
        Me.ButtonSaveAllUrls.Enabled = True
        Me.ButtonSaveAllUrls.BackColor = oldcolor
        Me.ButtonConcatenate.Visible = True
        Me.LabelOR.Visible = True
    End Sub
    Private Sub ButtonConcatenate_Click(sender As Object, e As EventArgs) Handles ButtonConcatenate.Click
        Dim oldcolor As System.Drawing.Color

        ' download urls and concatenate them to one large file.
        ' disable buttons so that they won't be hit twice by user while the download happens
        ' change the color of the button that was clicked, so the user remembers which button he clicked while the download is happening.
        oldcolor = Me.ButtonConcatenate.BackColor
        Me.ButtonConcatenate.BackColor = Color.Beige
        Me.ButtonConcatenate.Enabled = False
        Me.ButtonSaveAllUrls.Visible = False
        Me.LabelOR.Visible = False
        concat = True
        downloadall()
        ' re-enable buttons, since download has happened
        Me.ButtonConcatenate.Enabled = True
        Me.ButtonConcatenate.BackColor = oldcolor
        Me.ButtonSaveAllUrls.Visible = True
        Me.LabelOR.Visible = True
    End Sub

    Private Sub ButtonUseLastSet_Click(sender As Object, e As EventArgs) Handles ButtonUseLastSet.Click
        Dim strSet As String
        ' if you want to reuse a set of URLs from a previous session, you click this button, and it reads them and puts them in the textbox.
        If My.Computer.FileSystem.FileExists("pasteset.txt") Then
            strSet = My.Computer.FileSystem.ReadAllText("pasteset.txt")
            Me.TextBox1.Text = strSet
        End If
    End Sub

    Private Sub RadioButtonMostTags_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonMostTags.CheckedChanged
        ' RadioButtonMostTags is the radio that says "all but".  This means remove all tags except those that the user specifically chooses to keep.
        ' For instance, the user might choose to only keep line-breaks and paragraphs.
        ' this routine fires when the "all but" button is checked or unchecked by the user.
        If RadioButtonMostTags.Checked Then
            Me.CheckBoxHyperlinks.Visible = True
            Me.CheckBoxImages.Visible = True
            Me.CheckBoxLineBreaks.Visible = True
            Me.CheckBoxStyles.Visible = True
            Me.CheckBoxTables.Visible = True
        Else
            Me.CheckBoxHyperlinks.Visible = False
            Me.CheckBoxImages.Visible = False
            Me.CheckBoxLineBreaks.Visible = False
            Me.CheckBoxStyles.Visible = False
            Me.CheckBoxTables.Visible = False
        End If
    End Sub

   

    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        Dim openFileDialog1 As New OpenFileDialog()
        Dim strFname As String

        ' if you have a file on your local PC that has links written in it, then you would use 'browse' to find it, and then the program downloads the links
        ' browse opens a common dialog used to find folders and files
        openFileDialog1.Filter = "htm files (*.htm)|*.htm;*.html|txt files (*.txt)|*.txt|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 1
        openFileDialog1.RestoreDirectory = False

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            strFname = openFileDialog1.FileName
            Me.TextBox1.Text = strFname & vbCrLf & Me.TextBox1.Text
        End If
    End Sub

    Function MakeFriendlyFileNameFromURL(ByVal urlStr As String, ByRef extension As String) As String
        Dim slashpos As Integer
        Dim lastpos As Integer
        Dim periodpos As Integer
        Dim prelastpos As Integer
        Dim questionmarkpos As Integer
        Dim nsignpos As Integer
        Dim local As Boolean
        ' given a url, make a file name out of it to save the URL contents to.  Also get the extension in a separate variable (htm, aspx, txt).
        '   If there are characters that can't be used in a filename, get rid of them or replace them.  We try to make a unique filename, so we use 
        ' any clue possible, including a querystring, if one exists.

        extension = ""
        If urlStr(1) = ":" Then
            local = True
        Else
            local = False
        End If
        nsignpos = urlStr.IndexOf("#")
        If nsignpos > -1 Then
            urlStr = urlStr.Substring(nsignpos + 1) ' not sure what I'm doing here.
        End If
        If local Then
            slashpos = urlStr.LastIndexOf("\")
        Else
            slashpos = urlStr.LastIndexOf("/")
        End If


        questionmarkpos = urlStr.IndexOf("?")
        ' note that a URL could have a querystring such as http://msoft.com/myfile.aspx?device=mobile
        If questionmarkpos > -1 Then
            Dim firstpart As String
            Dim secondpart As String
            ' don't get rid of the querystring, if it exists, make it safe instead, and append it as part of the filename.
            firstpart = urlStr.Substring(0, questionmarkpos)
            secondpart = urlStr.Substring(questionmarkpos)
            secondpart = secondpart.Replace("?", "_").Replace(".", "_").Replace("=", "_").Replace("#", "_").Replace("~", "_")
            urlStr = firstpart & secondpart
            periodpos = firstpart.LastIndexOf(".")
            If periodpos > -1 Then
                extension = firstpart.Substring(periodpos + 1)
            End If
        Else
            periodpos = urlStr.LastIndexOf(".")
            If periodpos > slashpos Then
                extension = urlStr.Substring(periodpos + 1)
            End If
        End If

        lastpos = urlStr.Length - 1
        If slashpos = lastpos Then
            extension = "htm"
            prelastpos = urlStr.Substring(0, lastpos).LastIndexOf("/")
            If prelastpos >= 0 Then

                Return (urlStr.Substring(prelastpos + 1, lastpos - prelastpos - 1) & ".htm")
            Else
                fnameid = fnameid + 1
                Return ("UnknownName" & fnameid & ".htm")
            End If
        End If
        Return (urlStr.Substring(slashpos + 1))
    End Function


    Sub PruneBySearch(ByRef responsefromserverlowercase As String, ByRef responsefromserver As String, ByVal SearchWithinBody As Boolean)
        ' if the user provides a startphrase and/or an endphrase, use them to retain just part of the downloaded page.
        Dim posStartPhrase As Integer
        Dim posEndPhrase As Integer

        Dim startbodypos As Integer

        startbodypos = responsefromserverlowercase.IndexOf("<body")

        If startbodypos = -1 Then
            startbodypos = 0
        End If

        posStartPhrase = startbodypos
        If startphrase.Length > 0 Then
            posStartPhrase = responsefromserverlowercase.IndexOf(startphrase, posStartPhrase)
            If posStartPhrase >= 0 Then
                responsefromserver = responsefromserver.Substring(posStartPhrase)
                responsefromserverlowercase = responsefromserverlowercase.Substring(posStartPhrase)

            End If
        End If

        If endphrase.Length > 0 Then
         
            posEndPhrase = responsefromserverlowercase.IndexOf(endphrase, 0)
            If posEndPhrase >= 0 Then
                responsefromserver = responsefromserver.Substring(0, posEndPhrase)
                responsefromserverlowercase = responsefromserverlowercase.Substring(0, posEndPhrase)
            End If
        End If

    End Sub
    Sub PruneToInsideOfBody(ByRef responsefromserverlowercase As String, ByRef responsefromserver As String)
        Dim pos As Integer
        Dim pos2 As Integer

        ' get the section of the download webpage that is between the body-tags and retain only that.
        pos = responsefromserverlowercase.IndexOf("</body")
        If pos >= 0 Then
            responseFromServer = responseFromServer.Substring(0, pos)
            responsefromserverlowercase = responsefromserverlowercase.Substring(0, pos)
        End If
        pos = responsefromserverlowercase.IndexOf("<body")
        If pos >= 0 Then
            pos2 = responsefromserverlowercase.IndexOf(">", pos + 5)
            If pos2 >= 0 Then
                responseFromServer = responseFromServer.Substring(pos2 + 1)
                responsefromserverlowercase = responsefromserverlowercase.Substring(pos2 + 1)
            End If

        End If
    End Sub
End Class