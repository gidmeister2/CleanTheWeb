Imports Microsoft.VisualBasic
Imports System.Text
Public Class FilterTagsNew


    Private Shared strtemp As String = ""
    Private Shared globalBreakItCount As Integer = 0

    Public Shared Sub clearAlertStruct(ByRef structError As FilterTagStringOperations.structalert)
        ' errors encountered while parsing the webpages are stored in this structure.  Only one error can be stored in one structure.
        With structError
            .errorMessage = ""
            .dangerlevel = FilterTagStringOperations.enumWarning.warning
            .errorcode = FilterTagStringOperations.enumError.other
        End With
    End Sub
    Public Shared Sub clearTagstruct(ByRef thetagstruct As FilterTagStringOperations.tagstruct)
        ' a tagstruct contains various fields that describe the contents of a tag (<img...>)
        With thetagstruct
            .theword = ""
            .kind = FilterTagStringOperations.enumStartEnd.unknown
            .startbracketpos = 0
            .endbracketpos = 0
            .bTagIsSafe = True
            .bRemoveContents = False
            .thestep = 0
            .exclamationpoint = False
            .selfEnding = False
        End With
    End Sub
    Public Shared Function translatetokenenum(ByVal tokentype As FilterTagStringOperations.tokenenum) As String
        Select Case tokentype
            Case FilterTagStringOperations.tokenenum.endOfTag
                Return "end of tag"
            Case FilterTagStringOperations.tokenenum.number
                Return ("number")
            Case FilterTagStringOperations.tokenenum.doubledash
                Return ("double-dash")
            Case FilterTagStringOperations.tokenenum.startOfTag
                Return "start of tag"
            Case FilterTagStringOperations.tokenenum.unknown
                Return "unknown"
            Case FilterTagStringOperations.tokenenum.word
                Return ("word")
            Case FilterTagStringOperations.tokenenum.lessthan
                Return ("lessThan")
            Case FilterTagStringOperations.tokenenum.greaterthan
                Return ("Greaterthan")
        End Select
        Return ("error")
    End Function
    Public Shared Function MustRemoveAreaBetween(ByVal thetoken As String) As Boolean
        thetoken = thetoken.ToLower
        If extend.removeInsidesArray.Contains(thetoken) Then
            ' we should remove the token, as well as any text between it and its end tag.  For instance if the token is 'script' we remove all between <script> and </script>.
            Return True
        End If
        Return False
    End Function
    Public Shared Function tagIsSafe(ByVal thetoken As String) As Boolean
        thetoken = thetoken.ToLower

        ' there are various reasons a tag might not be safe.  It might be used for (possibly malicious) scripting for instance.  DIV is dangerous, because it can be too wide.

        If extend.safeTagsArray.Contains(thetoken) Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function minimaltagIsSafe(ByVal thetoken As String) As Boolean
        ' this is used when the user chose to filter all tags apart from exceptions that he specifies

        If (GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.MostTags) Then
            If (GlobalVariablesClass.pKeepImages) Then
                If thetoken = "img" Then
                    Return True
                End If
            End If
            If (GlobalVariablesClass.pKeepLinebreaks) Then
                If extend.linebreakTagsSet.Contains(thetoken) Then
                    Return True
                End If
            End If
            If (GlobalVariablesClass.pKeepHyperlinks) Then
                If thetoken = "a" Then
                    Return True
                End If
            End If
            If (GlobalVariablesClass.pKeepStyles) Then
                If thetoken = "head" Then
                    Return True
                End If
            End If
            If GlobalVariablesClass.pKeepTables Then
                If extend.tableTagsArray.Contains(thetoken) Then
                    Return True
                End If
            End If
        End If

        Return False
    End Function


    Public Shared Function PassesIntegrityCheck(ByVal thestack As Stack(Of FilterTagStringOperations.tagstruct), ByRef dangeroustag As String) As Boolean
        ' we check every tag that we found to be sure none is dangerous.  Normally we remove dangerous tags, but if something went wrong with the removal
        ' process, possibly because the html tags in the webpage we examined were not balanced or designed correctly the the webpage designer, then
        ' we can do this last-minute check.

        For Each thetagstruct In thestack
            If Not thetagstruct.bTagIsSafe Then
                dangeroustag = thetagstruct.theword
                Return False
            End If
        Next

        Return True
    End Function


    Public Shared Function filtertags(ByRef strAll As String, ByRef structError As FilterTagStringOperations.structalert, _
                                    ByRef thestack As Stack(Of FilterTagStringOperations.tagstruct)) As Boolean
        Dim Pos As Integer = 0
        Dim thetoken As String = ""
        Dim previousToken As String = ""
        Dim tokentype As FilterTagStringOperations.tokenenum = FilterTagStringOperations.tokenenum.unknown
        Dim tokenstartsAt As Integer = 0
        Dim intag As Boolean = False
        Dim nexttoken As String = ""
        Dim recursecount As Integer = 0
        Dim theString As String = ""
        Dim twotagStruct As FilterTagStringOperations.tagstruct = Nothing
        Dim onetagstruct As FilterTagStringOperations.tagstruct = Nothing
        Dim removelist As New List(Of FilterTagStringOperations.segstruct)

        ' we've already built a stack of the tags in the webpage being parsed.   The top element in the stack is the last tag in the webpage (normally that would be </html>.  We pop
        ' the tags, and if we find one that we've previously marked for removal, we remove it here.  If we find that we should remove a tag pair and everything between the start 
        ' and end of the pair, we remove that as well.  In that particular case we have to be careful though because we can have nested pairs of tags, something like
        ' <p>  <span> </span> </p>
        ' and I just have run into cases when the same type of tag can be nested within itself.  That is why we use 'recursecount' below to make sure we find the matching end-tag
        ' for a start-tag.
        Do While thestack.Count > 0
            onetagstruct = thestack.Pop
            If onetagstruct.bTagIsSafe And Not onetagstruct.bRemoveContents Then
                Continue Do
            End If
            If onetagstruct.bRemoveContents Then
                If onetagstruct.kind = FilterTagStringOperations.enumStartEnd.finish Then
                    recursecount = 1
                    Do While thestack.Count > 0
                        twotagStruct = thestack.Pop
                        If twotagStruct.theword = onetagstruct.theword Then
                            If twotagStruct.kind = FilterTagStringOperations.enumStartEnd.start Then
                                recursecount = recursecount - 1
                            ElseIf twotagStruct.kind = FilterTagStringOperations.enumStartEnd.finish Then
                                recursecount = recursecount + 1
                            End If
                        End If
                        If recursecount = 0 Then

                            Dim segs As New FilterTagStringOperations.segstruct
                            segs.startPosition = twotagStruct.startbracketpos
                            segs.endPosition = onetagstruct.endbracketpos
                            removelist.Insert(0, segs)
                            Exit Do
                        End If
                    Loop
                Else
                    structError.errorMessage = "Error: you have a tag: " & onetagstruct.theword & " at position " & onetagstruct.startbracketpos & " that should be the end of a sandwich but is not."
                    structError.dangerlevel = FilterTagStringOperations.enumWarning.warning
                    Return True ' true is ok, since not fatal
                End If
            ElseIf Not onetagstruct.bTagIsSafe Then
                'xxx   FilterTagStringOperations.RemoveSegment(strAll, onetagstruct.startbracketpos, onetagstruct.endbracketpos)
                Dim segs As New FilterTagStringOperations.segstruct
                segs.startPosition = onetagstruct.startbracketpos
                segs.endPosition = onetagstruct.endbracketpos
                removelist.Insert(0, segs)
            End If
        Loop
        Dim dangeroustag As String = ""

        If Not PassesIntegrityCheck(thestack, dangeroustag) Then
            structError.dangerlevel = FilterTagStringOperations.enumWarning.fatal
            structError.errorMessage = "Error: it was not possible to remove a dangerous tag: " & _
                dangeroustag & " in your text, so it could not be saved." & ". An excerpt is: [" & FilterTagStringOperations.showExcerpt(strAll, tokenstartsAt) & "]"
            Return False
        End If
        ' we create 'removelist' because it is more efficient to remove everything at once, using a stringbuilder, then removing it as we find it.
        FilterTagStringOperations.RemoveAllMarkedSegments(strAll, removelist)
        Return True
    End Function

    Private Shared Function printStack(ByVal thestackdebug As Stack(Of FilterTagStringOperations.tagstruct)) As String
        ' return a string containing each tag in the stack on its own line, with the last tag of the page being the first tag to be shown
        Dim sb As New StringBuilder("")
        For Each ts In thestackdebug
            printTag(sb, ts)
        Next
        Return (sb.ToString)
    End Function
    Private Shared Function printStack2(ByVal thestackdebug As Stack(Of FilterTagStringOperations.tagstruct)) As String
        ' return a string containing each tag in the stack on its own line, with the last tag of the page being the first tag to be shown
        Dim sb As New StringBuilder("")
        For Each ts In thestackdebug
            printTag2(sb, ts)
        Next
        Return (sb.ToString)
    End Function
    Private Shared Function printStack3(ByVal thestackdebug As Stack(Of FilterTagStringOperations.tagstruct)) As String
        ' return a string containing each tag in the stack on its own line, each nested tag indented to show the nesting.  In this case,
        ' the stack is reversed, so the first tag is the first tag of the webpage, which is more intuitive.  But less details are shown
        ' about each tag than in PrintStack1 and PrintStack2
        Dim indent As Integer = 0
        Dim sb As New StringBuilder("")
        Dim count As Integer = 0
        For Each ts In thestackdebug.Reverse
            count = count + 1
            printTag3(sb, ts, indent, count)
        Next
        Return (sb.ToString)
    End Function
    Private Shared Function printStack4(ByVal thestackdebug As Stack(Of FilterTagStringOperations.tagstruct), ByVal strAll As String, ByVal removepairs As Boolean) As String
        ' return a string containing each tag in the stack on its own line, each nested tag indented to show the nesting.  In this case,
        ' the stack is reversed, so the first tag is the first tag of the webpage, which is more intuitive.  But less details are shown
        ' about each tag than in PrintStack1 and PrintStack2
        Dim indent As Integer = 0
        Dim sb As New StringBuilder("")

        Dim previousts As New FilterTagStringOperations.tagstruct
        previousts.theword = "none"
        Dim thelist As New List(Of FilterTagStringOperations.tagstruct)
        ' stack html  /html
        For Each ts2 In thestackdebug.Reverse
            thelist.Add(ts2)
        Next

        Dim stacklen As Integer
        Dim originalstacklen As Integer
        stacklen = thelist.Count
        originalstacklen = stacklen
        If removepairs Then
            Dim countloop As Integer = 0
            Dim i As Integer
       

            Dim changeMade As Boolean
            Dim stoploop As Boolean
            Do
                changeMade = False
                stoploop = True
                i = 0
                Do While i <= stacklen - 2

                    If thelist(i).kind <> FilterTagStringOperations.enumStartEnd.start Or thelist(i).selfEnding Then
                        i = i + 1
                        Continue Do
                    End If
                    For j = i + 1 To stacklen - 1
                        If thelist(j).selfEnding Then
                            Continue For
                        End If
                        If thelist(j).kind <> FilterTagStringOperations.enumStartEnd.finish Then
                            i = j - 1
                            Exit For
                        End If
                        If thelist(i).theword = thelist(j).theword Then
                            thelist.RemoveAt(j)
                            thelist.RemoveAt(i)
                            changeMade = True
                            Exit For
                        End If
                    Next
                    If changeMade Then
                        stacklen = stacklen - 2
                        stoploop = False
                        changeMade = False
                    Else
                        i = i + 1
                    End If
                Loop
               
            Loop Until stoploop
        End If
        Dim count2 As Integer = 0
        Dim count As Integer
        For Each ts In thelist
            count2 = count2 + 1
            count = 1 + ts.index
            printTag4(sb, ts, previousts, indent, count2, strAll, False, count)
            previousts = ts
        Next
        Return (sb.ToString)
    End Function
    Private Shared Sub printTag(ByRef sb As StringBuilder, ByVal ts As FilterTagStringOperations.tagstruct)
        With ts
            Dim prefix As String = ""
            If .kind = FilterTagStringOperations.enumStartEnd.finish Then
                prefix = "/"
            End If
            sb.AppendLine("[" & prefix & .theword & "," & translateIsSafe(.bTagIsSafe) _
                          & "," & translateRemoveAll(.bRemoveContents) & "] (" & .startbracketpos & ":" & .endbracketpos & ")")
        End With
    End Sub
    Private Shared Sub printTag2(ByRef sb As StringBuilder, ByVal ts As FilterTagStringOperations.tagstruct)
        With ts
            Dim prefix As String = ""
            Dim asterix As String = ""
            If .kind = FilterTagStringOperations.enumStartEnd.finish Then
                prefix = "/"
            ElseIf .selfEnding Then
                asterix = "/"
            End If

            sb.Append("[<" & prefix & .theword & asterix & ">," & translateIsSafe2(.bTagIsSafe, .bRemoveContents) _
                          & "]  ")
        End With
    End Sub
    Private Shared Sub printTag4(ByRef sb As StringBuilder, ByVal ts As FilterTagStringOperations.tagstruct, ByVal previousts As FilterTagStringOperations.tagstruct, _
                                 ByRef indent As Integer, ByVal count As Integer, ByVal strAll As String, ByVal includeFragment As Boolean, ByVal tagcount As Integer)
        Dim i As Integer
        Dim prefix As String = ""
        Dim fragment As String
        Dim asterix As String = ""
        Dim sb2 As New StringBuilder("")

        sb2.Append(prefixNum(tagcount, 5))
        sb2.Append(" ")
        Try


            With ts
                Dim suppressindentchange As Boolean = False
                Dim oldindent As Integer = indent
                ' meta (maybe self ending) title - regular
                If FilterTagsNew.SometimesIsSelfEndingForDebug(ts) Then
                    suppressindentchange = True
                End If
                If .selfEnding Then
                    asterix = "/"
                ElseIf .kind = FilterTagStringOperations.enumStartEnd.finish Then
                    prefix = "/"
                    If (Not suppressindentchange) Then
                        indent = indent - 4
                        If indent < 0 Then
                            indent = 0
                            sb2.Append("!!")
                        End If
                    End If
                ElseIf .kind = FilterTagStringOperations.enumStartEnd.start Then
                    If (Not suppressindentchange) Then
                        indent = indent + 4
                    End If
                End If

                If suppressindentchange Then
                    sb2.Append(prefixNum(oldindent, 3))
                    For i = 1 To oldindent
                        sb2.Append(" ")
                    Next
                ElseIf .kind = FilterTagStringOperations.enumStartEnd.start Then
                    sb2.Append(prefixNum(oldindent, 3))
                    For i = 1 To oldindent
                        sb2.Append(" ")
                    Next
                ElseIf .kind = FilterTagStringOperations.enumStartEnd.finish Then
                    sb2.Append(prefixNum(indent, 3))
                    For i = 1 To indent
                        sb2.Append(" ")
                    Next
                End If
                If previousts.theword <> "none" Then
                    fragment = FilterTagStringOperations.SafeSubstring(strAll, previousts.endbracketpos + 1, ts.startbracketpos - previousts.endbracketpos - 1).Trim
                    sb.AppendLine("")
                    If fragment.Length > 0 And includeFragment Then
                        sb.AppendLine("---- start fragment ---")
                        sb.AppendLine(fragment)
                        sb.AppendLine("---- end fragment ---")
                    End If
                End If
                sb.AppendLine(sb2.ToString & "<" & prefix & .theword & asterix & ">")

                'fragment = FilterTagStringOperations.SafeSubstring(strAll, .endbracketpos + 1, 25).Replace(vbLf, " ").Replace(vbCr, "")
                'sb.AppendLine(fragment)

            End With
        Catch ex As Exception
            Dim strFoo As String
            strFoo = ex.Message

        End Try
    End Sub
    Private Shared Sub printTag3(ByRef sb As StringBuilder, ByVal ts As FilterTagStringOperations.tagstruct, ByRef indent As Integer, ByVal count As Integer)
        Dim i As Integer
        Dim prefix As String = ""
        Dim asterix As String = ""
        sb.Append(prefixNum(count, 5))
        sb.Append(" ")
        With ts

            Dim suppressindentchange As Boolean = False
            Dim oldindent As Integer = indent
            ' meta (maybe self ending) title - regular
            If FilterTagsNew.SometimesIsSelfEndingForDebug(ts) Then
                suppressindentchange = True
            End If
            If .selfEnding Then
                asterix = "/"
            ElseIf .kind = FilterTagStringOperations.enumStartEnd.finish Then
                prefix = "/"
                If (Not suppressindentchange) Then
                    indent = indent - 4
                    If indent < 0 Then
                        indent = 0
                        sb.Append("!!")
                    End If
                End If
            ElseIf .kind = FilterTagStringOperations.enumStartEnd.start Then
                If (Not suppressindentchange) Then
                    indent = indent + 4
                End If
            End If

            If suppressindentchange Then
                sb.Append(prefixNum(oldindent, 3))
                For i = 1 To oldindent
                    sb.Append(" ")
                Next
            ElseIf .kind = FilterTagStringOperations.enumStartEnd.start Then
                sb.Append(prefixNum(oldindent, 3))
                For i = 1 To oldindent
                    sb.Append(" ")
                Next
            ElseIf .kind = FilterTagStringOperations.enumStartEnd.finish Then
                sb.Append(prefixNum(indent, 3))
                For i = 1 To indent
                    sb.Append(" ")
                Next
            End If
            sb.AppendLine("<" & prefix & .theword & asterix & ">")
        End With
    End Sub
    Public Shared Function prefixNum(ByVal thenum As Integer, ByVal digits As Integer) As String
        Dim sb As New StringBuilder("")
        Dim i As Integer
        Dim thelen As Integer
        Dim diff As Integer
        Dim strNum As String

        strNum = thenum.ToString

        thelen = strNum.Length

        diff = digits - thelen
        If diff > 0 Then
            For i = 0 To diff - 1
                sb.Append("0")
            Next
            sb.Append(strNum)
        End If
        Return sb.ToString
    End Function
    Private Shared Function translatestartend(ByVal kind As FilterTagStringOperations.enumStartEnd) As String
        Select Case kind
            Case FilterTagStringOperations.enumStartEnd.finish
                Return ("finish")
            Case FilterTagStringOperations.enumStartEnd.start
                Return ("start")
            Case FilterTagStringOperations.enumStartEnd.unknown
                Return ("unknown")
        End Select
        Return ("?")
    End Function

    Private Shared Function translateIsSafe(ByVal issafe As Boolean) As String
        If issafe Then
            Return ("safe")
        Else
            Return ("unsafe")
        End If
    End Function


    Private Shared Function translateIsSafe2(ByVal issafe As Boolean, ByVal removeall As Boolean) As String
        If removeall Then
            Return ("!!!!")
        End If
        If issafe Then
            Return ("")
        Else
            Return ("!")
        End If
    End Function

    Private Shared Function translateRemoveAll(ByVal removeall As Boolean) As String
        If removeall Then
            Return ("must-remove-insides")
        Else
            Return ("no-need-to-remove insides")
        End If
    End Function

    Public Shared Function FormatError(ByVal themsg As String) As String
        Return ("Error in " & GlobalVariablesClass.pCurrentlyWorkingOnURL & ": " & themsg)
    End Function
    Public Shared Function checktags(ByRef strAll As String, ByRef structError As FilterTagStringOperations.structalert, _
                                       ByRef thestack As Stack(Of FilterTagStringOperations.tagstruct),
                                      ByVal omitchecks As Boolean) As Boolean
        ' given the contents of a webpage in 'strAll', find all the tags in it, and push them on a stack as you find them.  If you find a syntax error, report it in 'structError'.
        ' if the 'omitchecks' flag is true, then do not check for syntax errors, just create the stack.
        Dim Pos As Integer = 0
        Dim posBeforetoken As Integer = 0
        Dim thetoken As String = ""
        Dim previousToken As String = ""
        Dim revisetag As New FilterTagStringOperations.tagstruct
        Dim tokentype As FilterTagStringOperations.tokenenum = FilterTagStringOperations.tokenenum.unknown
        Dim previoustokentype As FilterTagStringOperations.tokenenum = FilterTagStringOperations.tokenenum.unknown
        Dim tokenstartsAt As Integer = 0
        Dim intag As Boolean = False
        Dim didapop As Boolean
        Dim posplus As Integer
        Dim copypos As Integer
        Dim nexttoken As String = ""
        Dim nexttokentype As FilterTagStringOperations.tokenenum
        Dim nexttokenStartsAt As Integer
        Dim theString As String = ""
        Dim FirstPosOfString As Integer
        Dim LastPosOfString As Integer
        Dim hadquotes As Boolean
        Dim firstwordwithinTagBrackets As Boolean
        Dim letter As String
        Dim nextletter As String
        Dim onetagstruct As FilterTagStringOperations.tagstruct = Nothing
        Dim proceed As Boolean
        Dim badtag As New FilterTagStringOperations.tagstruct
        Dim poptag As New FilterTagStringOperations.tagstruct
        Dim examinestack As New Stack(Of FilterTagStringOperations.tagstruct)
        Dim looplimit As Integer = 0
        Dim posForDebug As Integer = -88
        Dim incommentdashdash As Boolean = False
        Dim incommentslashAsterix As Boolean = False
        Dim incommentSlashDouble As Boolean = False
        Dim instring As Boolean = False
        Dim inscript As Boolean = False
        Dim saveStart As String = ""
        Dim encounteredCR As Boolean = False
        Dim halflenstrAll As Integer
        halflenstrAll = strAll.Length / 2
        Dim numConsecutiveBackslashes As Integer = 0
        Dim previousConsecutiveBackslashes As Integer = 0
        Dim listongoing As New List(Of FilterTagStringOperations.tagstruct)
        Dim tagforlist As FilterTagStringOperations.tagstruct
        Try
            Do While True
                looplimit = looplimit + 1

                If looplimit > strAll.Length Then
                    structError.errorMessage = FormatError(" this URL causes an infinite loop in our code.  We can't use it ")
                    Return False
                End If

                previousToken = thetoken
                previoustokentype = tokentype
                If Not FilterTagStringOperations.getToken(strAll, Pos, thetoken, tokentype, tokenstartsAt, encounteredCR, incommentSlashDouble, intag) Then
                    Exit Do
                End If

                'If thetoken = "'" Or thetoken = Chr(34) Then
                '    Dim i As Integer
                '    i = 8
                'End If

                posBeforetoken = Pos - thetoken.Length - 1
                previousConsecutiveBackslashes = numConsecutiveBackslashes
                If thetoken = "\" Then
                    numConsecutiveBackslashes = numConsecutiveBackslashes + 1
                Else
                    numConsecutiveBackslashes = 0
                End If
                Select Case tokentype
                    ' ((((((((((((()))))))))))))
                    Case FilterTagStringOperations.tokenenum.singlequote
                        If incommentdashdash Or incommentslashAsterix Or incommentSlashDouble Then
                            ' html can have various types of comments, and so can the javascript inside the html.  
                            ' for instance, <!-- starts one type of comment in html, // starts another (in javascript), and /* starts another (in javascript)
                            ' here we are saying that if we are in a comment, don't treat a quote mark as the beginning of a string (or end of one)
                            Continue Do
                        End If
                        If inscript Or intag Then
                            If instring Then
                                If saveStart = "'" Then
                                    If previousConsecutiveBackslashes Mod 2 = 0 Then
                                        instring = False
                                    End If
                                End If
                            Else
                                instring = True
                                saveStart = "'"
                            End If
                        End If
                        ' ((((((((((((()))))))))))))
                    Case FilterTagStringOperations.tokenenum.doublequote
                        If incommentdashdash Or incommentslashAsterix Or incommentSlashDouble Then
                            Continue Do
                        End If
                        If inscript Or intag Then
                            If instring Then
                                If saveStart = Chr(34) Then
                                    If previousConsecutiveBackslashes Mod 2 = 0 Then
                                        instring = False
                                    End If
                                End If
                            Else
                                instring = True
                                saveStart = Chr(34)
                            End If
                        End If

                        ' ((((((((((((()))))))))))))
                    Case FilterTagStringOperations.tokenenum.startOfTag ' (i.e. '<')
                        ' step 1 - don't have word yet
                        If incommentdashdash Or incommentslashAsterix Or incommentSlashDouble Then
                            Continue Do
                        End If
                        If instring Then
                            Continue Do
                        End If
                        If intag Then
                            Continue Do
                        End If
                        If Pos < strAll.Length Then
                            proceed = False
                            letter = strAll(Pos)
                            If letter = "!" And Pos + 1 < strAll.Length Then
                                nextletter = strAll(Pos + 1)
                                If Char.IsLetter(nextletter) Or nextletter = "-" Then
                                    ' have something like !DOCTYPE
                                    proceed = True
                                End If
                            ElseIf Char.IsLetter(letter) Then
                                proceed = True
                            End If
                            If proceed Then
                                If inscript Then
                                    ' scripts can have angle-bracket characters in them, and we do not want to treat those as starts or ends of tags.
                                    Continue Do
                                End If
                                intag = True

                                firstwordwithinTagBrackets = True
                                onetagstruct = New FilterTagStringOperations.tagstruct
                                clearTagstruct(onetagstruct)
                                With onetagstruct
                                    If letter = "!" Then
                                        .exclamationpoint = True
                                        .selfEnding = True
                                        Pos = Pos + 1 ' to get by the '!'
                                    End If
                                    .kind = FilterTagStringOperations.enumStartEnd.start ' this is a start-tag of a pair, not an end-tag
                                    .startbracketpos = tokenstartsAt ' the position in 'strAll' of the '<' of this tag
                                    .thestep = 1 ' this is used by the routine for checking that the code is working right and that each step is reached in order.
                                End With
                                onetagstruct.index = thestack.Count
                                thestack.Push(onetagstruct)

                            ElseIf strAll(Pos) = "/" Then
                                ' might have </span... for example
                                posplus = Pos + 1
                                Do While posplus < strAll.Length
                                    If Not Char.IsWhiteSpace(strAll(posplus)) Then
                                        Exit Do
                                    End If
                                    posplus = posplus + 1
                                Loop

                                If Char.IsLetter(strAll(posplus)) Then
                                    If inscript Then
                                        Dim lookaheadtoken As String = ""
                                        Dim lookaheadpos As Integer
                                        lookaheadpos = posplus
                                        Dim lookaheadTokenType As FilterTagStringOperations.tokenenum
                                        Dim lookaheadTokenStartsAt As Integer
                                        Dim laCR As Boolean
                                        Dim inCommentLA As Boolean
                                        If FilterTagStringOperations.getToken(strAll, lookaheadpos, lookaheadtoken, lookaheadTokenType, lookaheadTokenStartsAt, laCR, _
                                                                              inCommentLA, intag) Then
                                            If lookaheadtoken <> "script" Then
                                                Continue Do
                                            End If
                                        End If
                                    End If
                                    intag = True
                                    firstwordwithinTagBrackets = True
                                    onetagstruct = New FilterTagStringOperations.tagstruct
                                    clearTagstruct(onetagstruct)
                                    With onetagstruct
                                        .kind = FilterTagStringOperations.enumStartEnd.finish ' this is a end-tag of a pair, not a start-tag
                                        .startbracketpos = tokenstartsAt
                                        .thestep = 1
                                    End With
                                    onetagstruct.index = thestack.Count
                                    thestack.Push(onetagstruct)
                                End If
                            End If
                        End If
                    Case FilterTagStringOperations.tokenenum.starslash
                        If incommentdashdash Or incommentSlashDouble Then
                            Continue Do
                        End If
                        If inscript And Not instring Then
                            incommentslashAsterix = False
                        End If

                    Case FilterTagStringOperations.tokenenum.slashstar
                        If incommentdashdash Or incommentSlashDouble Then
                            Continue Do
                        End If
                        If inscript And Not instring Then
                            incommentslashAsterix = True
                        End If
                    Case FilterTagStringOperations.tokenenum.doubleslash
                        If inscript And Not instring And Not incommentdashdash Then
                            incommentSlashDouble = True
                        End If
                    Case FilterTagStringOperations.tokenenum.endOfTag
                        If incommentslashAsterix Or incommentSlashDouble Then
                            Continue Do
                        End If
                        If incommentSlashDouble Then
                            Continue Do
                        End If
                        If incommentdashdash Then
                            If previoustokentype <> FilterTagStringOperations.tokenenum.doubledash Then
                                ' might have --hi there--> where there-- is considered one token.
                                If previousToken.EndsWith("--") Then
                                    If strAll.Substring(posBeforetoken, 1) <> "-" Then
                                        Continue Do
                                    Else
                                        incommentdashdash = False
                                    End If
                                Else
                                    Continue Do
                                End If

                            Else ' have either --> or -- >
                                If strAll.Substring(posBeforetoken, 1) <> "-" Then
                                    Continue Do
                                Else
                                    incommentdashdash = False
                                End If
                            End If
                        End If
                        If instring Then
                            Continue Do
                        End If
                        If Not intag And inscript Then
                            Continue Do
                        End If
                        If intag Then
                            If thestack.Count = 0 Then
                                structError.errorMessage = FormatError(" 1: something is wrong with your html syntax - there is an improper endOfTag at position " & _
                                    tokenstartsAt & ": " & thetoken & ". An excerpt is: [" & FilterTagStringOperations.showExcerpt(strAll, tokenstartsAt) & "]")
                                Return False
                            End If
                            intag = False
                            revisetag = thestack.Pop
                            If revisetag.theword = "script" Then
                                If revisetag.kind = FilterTagStringOperations.enumStartEnd.start Then
                                    inscript = True
                                ElseIf revisetag.kind = FilterTagStringOperations.enumStartEnd.finish Then
                                    inscript = False
                                End If
                            End If
                            If revisetag.thestep <> 2 Then
                                structError.errorMessage = FormatError(" 2: something is wrong with your html syntax - there is an improper endOfTag at position " & tokenstartsAt & ": " & _
                                    thetoken & ". An excerpt is: [" & FilterTagStringOperations.showExcerpt(strAll, tokenstartsAt) & "]")
                                Return False
                            End If
                            revisetag.thestep = 3
                            revisetag.endbracketpos = tokenstartsAt
                            If strAll(revisetag.endbracketpos - 1) = "/" Then
                                revisetag.selfEnding = True  ' for instance, <br /> i self ending
                            ElseIf extend.SelfEndingArray.Contains(revisetag.theword) Then
                                revisetag.selfEnding = True ' have to do this, because sometimes br-tag lacks ending slash
                            ElseIf revisetag.exclamationpoint Then
                                revisetag.selfEnding = True ' tags that start with exclamation marks do not come in pairs (<!doctype> for example)
                            End If

                            If revisetag.kind = FilterTagStringOperations.enumStartEnd.start Then
                                If Not revisetag.selfEnding Then
                                    listongoing.Insert(0, revisetag) ' we add start tags to the list so that we can check if any end tag we encounter 
                                    ' might have a start tag before it that matches.
                                End If
                            ElseIf revisetag.kind = FilterTagStringOperations.enumStartEnd.finish Then
                                Dim kk As Integer
                                Dim removedone As Boolean
                                removedone = False
                                kk = 0
                                For Each tagforlist In listongoing
                                    If tagforlist.theword = revisetag.theword Then
                                        listongoing.RemoveAt(kk) ' if we find a start tag that matches the end tag that we have just found, then remove start tag from list
                                        removedone = True
                                        Exit For
                                    End If
                                    kk = kk + 1
                                Next
                                If Not removedone And revisetag.theword <> "p" Then
                                    revisetag.superfluous = True ' you have an end tag with no start tag
                                    revisetag.bTagIsSafe = False
                                End If
                            End If
                            thestack.Push(revisetag)
                        End If

                        ' ((((((((((((()))))))))))))
                    Case FilterTagStringOperations.tokenenum.word, FilterTagStringOperations.tokenenum.doubledash
                        ' <img (for example) or <!--
                        If incommentdashdash Or incommentslashAsterix Or incommentSlashDouble Then
                            If tokentype = FilterTagStringOperations.tokenenum.word Or tokentype = FilterTagStringOperations.tokenenum.doubledash Then
                                Continue Do
                            End If
                        End If
                        If instring Then
                            Continue Do
                        End If
                        If intag And Not firstwordwithinTagBrackets Then
                            revisetag = thestack.Peek
                            If revisetag.bTagIsSafe Then
                                'attribute might not be safe, though tag is
                                '<img width="30" ....
                                copypos = Pos
                                ' the next lines would get 'width' in 'nexttoken', and the string "30" in 'thestring'
                                If FilterTagStringOperations.getToken(strAll, copypos, nexttoken, nexttokentype, nexttokenStartsAt, encounteredCR, incommentSlashDouble, intag) Then
                                    If nexttoken = "=" Then
                                        If FilterTagStringOperations.getString(strAll, copypos, theString, FirstPosOfString, LastPosOfString, hadquotes, incommentSlashDouble, intag) Then
                                            Pos = LastPosOfString + 1
                                        Else
                                            structError.errorMessage = "Error B: you have an unterminated string - an excerpt follows: " & FilterTagStringOperations.SafeSubstring(theString, 0, 70)
                                            structError.dangerlevel = FilterTagStringOperations.enumWarning.fatal
                                            Return False
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        If firstwordwithinTagBrackets Then
                            If thestack.Count = 0 Then
                                structError.errorMessage = FormatError(" 7: something is wrong with your html syntax - there is an improper tag at position " & _
                                    tokenstartsAt & ": " & thetoken & ". An excerpt is: [" & FilterTagStringOperations.showExcerpt(strAll, tokenstartsAt) & "]")
                                Return False
                            End If
                            revisetag = thestack.Pop

                            revisetag.theword = thetoken.ToLower ' the tag, e.g. "p", "img", "span", "br", and many more
                            If tokentype = FilterTagStringOperations.tokenenum.doubledash Then
                                incommentdashdash = True
                            End If

                            If MustRemoveAreaBetween(revisetag.theword) Then
                                revisetag.bRemoveContents = True
                            ElseIf revisetag.superfluous Then
                                revisetag.bTagIsSafe = False
                            ElseIf GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.DangerousTagsOnly Then
                                revisetag.bTagIsSafe = tagIsSafe(thetoken)
                            ElseIf GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.MostTags Then
                                revisetag.bTagIsSafe = minimaltagIsSafe(thetoken)
                            ElseIf GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.keep Then
                                revisetag.bTagIsSafe = True
                            End If
                            If revisetag.theword = "style" Then
                                If GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.DangerousTagsOnly Then
                                    If Not revisetag.bTagIsSafe Then
                                        revisetag.bRemoveContents = True
                                    End If
                                ElseIf GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.MostTags Then
                                    If GlobalVariablesClass.pKeepStyles Then
                                        revisetag.bTagIsSafe = True
                                        revisetag.bRemoveContents = False
                                    Else
                                        revisetag.bTagIsSafe = False
                                        revisetag.bRemoveContents = True
                                    End If
                                End If
                            End If

                            If revisetag.thestep <> 1 Then
                                structError.errorMessage = FormatError(" 8: something is wrong with your html syntax - there is an improper tag at position " & _
                                    tokenstartsAt & ": " & thetoken & ". An excerpt is: [" & FilterTagStringOperations.showExcerpt(strAll, tokenstartsAt) & "]")
                                Return False
                            End If
                            revisetag.thestep = 2
                            thestack.Push(revisetag)

                            firstwordwithinTagBrackets = False
                        End If
                End Select
            Loop
            ' diagnostic
            ' Dim strshowGidxxx As String
            ' strshowGidxxx = printStack4(thestack, strAll, True) ' for debugging, put a breakpoint here

            If Not omitchecks Then
                CopyStack(thestack, examinestack)
                ' remove each nested pair.  think of each pair as a sandwich within another sandwich.  If this fails, then we have unbalanced html, and how we handle it depends
                ' on what the user specified should be done.
                If Not removeSandwich(strAll, examinestack, structError, poptag, 0) Then
                    structError.errorcode = FilterTagStringOperations.enumError.unbalanced
                    If GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.forbidunbalanced Then
                        structError.dangerlevel = FilterTagStringOperations.enumWarning.fatal
                        Return False
                    ElseIf GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.includeunbalanced Then
                        structError.dangerlevel = FilterTagStringOperations.enumWarning.warning
                    ElseIf GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.separatefileForUnbalanced Then
                        structError.dangerlevel = FilterTagStringOperations.enumWarning.warning
                    End If
                End If
                ' diagnostic
                'Dim strshowGidxxx2 As String
                'strshowGidxxx2 = printStack3(examinestack)
                If examinestack.Count > 0 Then
                    poptag = examinestack.Pop
                    skipSelfEndingTags(examinestack, poptag, didapop, "")
                    If examinestack.Count > 0 Then
                        structError.errorcode = FilterTagStringOperations.enumError.extratagsprefix
                        structError.dangerlevel = FilterTagStringOperations.enumWarning.fatal
                        structError.errorMessage = FormatError("the last tag in the web page is balanced by another, but there are extra tags" & _
                            " before that, for example " & explainTag(examinestack.Peek) & ".")
                        Return False
                    End If
                End If
            End If
            Return True

        Catch ex As Exception
            Throw New Exception("Checktags: " & ex.Message, ex)
            Return False
        End Try
    End Function
    Public Shared Function couldfilter(ByRef responseFromServer As String, ByRef structErrorCheck As FilterTagStringOperations.structalert, _
                                      ByRef structErrorFilter As FilterTagStringOperations.structalert,
                                       ByVal maxWidth As Integer, ByVal maxHeight As Integer) As Boolean

        Dim theStack As New Stack(Of FilterTagStringOperations.tagstruct)
        Dim structErrorFilter2 As New FilterTagStringOperations.structalert
        Dim structErrorCheck2 As New FilterTagStringOperations.structalert

        responseFromServer = StripOutTags.removeblanklines(responseFromServer)
        clearAlertStruct(structErrorFilter)
        clearAlertStruct(structErrorCheck)
        ' build a stack from the html in the webpage.  If a stack cannot be built due to bad syntax, then return false.
        If Not checktags(responseFromServer, structErrorCheck, theStack, False) Then
            If structErrorCheck.dangerlevel = FilterTagStringOperations.enumWarning.fatal Then
                Return False
            End If
        End If
        ' remove any dangerous tags.
        If Not filtertags(responseFromServer, structErrorFilter, theStack) Then
            If structErrorFilter.dangerlevel = FilterTagStringOperations.enumWarning.fatal Then
                Return False
            End If
        End If
        theStack.Clear()
        ' must rebuild stack, because the deleted tags cause all positions stored in the stack to become inaccurate

        clearAlertStruct(structErrorFilter2)
        clearAlertStruct(structErrorCheck2)
        checktags(responseFromServer, structErrorCheck2, theStack, True)

        ' remove dangerous attibutes (e.g. onmouseover = "javascript:destroyfiles()"
        If Not filterAttributesX(responseFromServer, structErrorFilter2, maxWidth, maxHeight, theStack) Then
            structErrorFilter = structErrorFilter2
            Return False
        End If
        Return True
    End Function
    Public Shared Function filterAttributesX(ByRef strAll As String, ByRef structErrorInAttrib As FilterTagStringOperations.structalert, _
                                    ByVal maxwidth As Integer, ByVal maxheight As Integer, _
                                    ByRef theStack As Stack(Of FilterTagStringOperations.tagstruct)) As Boolean
        Dim listofremove As New List(Of FilterTagStringOperations.segstruct)
        Dim onetagstruct As FilterTagStringOperations.tagstruct = Nothing

        ' go through stack looking for tags that are candidates for having attributes.  When you find them, call 'processAttributes' on each.  Note that stacks have the last tag
        ' at the top, so even we remove attributes, we don't throw off the positions of the next tags to be processed.  'processAttributes' creates a list that specifies what has 
        ' to be removed.
        Do While theStack.Count > 0
            onetagstruct = theStack.Pop
            If onetagstruct.endbracketpos < 1 Then
                Dim errmsg As String
                Dim sb As New StringBuilder("")
                printTag2(sb, onetagstruct)
                errmsg = "Error in filterAttributesX, current file: " & GlobalVariablesClass.pCurrentlyWorkingOnURL & " tag: " & sb.ToString & _
                    FilterTagStringOperations.showExcerpt(strAll, onetagstruct.startbracketpos)
                Dim ex As New Exception(errmsg)
                Throw ex
            End If
            If onetagstruct.kind = FilterTagStringOperations.enumStartEnd.finish Then
                Continue Do
            End If
            If onetagstruct.theword = "--" Then
                Continue Do
            End If
            If Not FilterAttributes.ProcessAttributes(strAll, onetagstruct, maxwidth, maxheight, structErrorInAttrib, listofremove) Then
                Return False
            End If
        Loop
        FilterTagStringOperations.RemoveAllMarkedSegments(strAll, listofremove)

        Return True
    End Function
    Public Shared Sub CopyStack(ByRef originstack As Stack(Of FilterTagStringOperations.tagstruct), ByRef resultstack As Stack(Of FilterTagStringOperations.tagstruct))
        Dim i As Integer
        Dim ts As New FilterTagStringOperations.tagstruct
        For i = originstack.Count - 1 To 0 Step -1
            ts = originstack(i)
            If Not ts.superfluous Then
                resultstack.Push(ts)
            End If
        Next
    End Sub

    Public Shared Function SometimesIsSelfEnding(ByVal thetag As FilterTagStringOperations.tagstruct) As Boolean
        ' some tags do not come in pairs.  Others may or may not come in pairs.  This routine returns true if they always end without a matching end tag, or if they sometimes end without
        ' a matching end tag.
        If thetag.selfEnding Then
            Return True
        End If

        If extend.arrayOfMaybeSelfEnding.Contains(thetag.theword) And thetag.kind = FilterTagStringOperations.enumStartEnd.start Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function SometimesIsSelfEndingForDebug(ByVal thetag As FilterTagStringOperations.tagstruct) As Boolean
        ' somewhat similar to [SometimesIsSelfEnding] but only use this version for printstack3, a routine that prints the stack with indents
        If extend.arrayOfMaybeSelfEnding.Contains(thetag.theword) Then
            Return True
        End If
        Return False
    End Function
    Private Shared Function MakeDummyTag() As FilterTagStringOperations.tagstruct
        Dim newtag As New FilterTagStringOperations.tagstruct
        clearTagstruct(newtag)
        Return newtag
    End Function
    Public Shared Function isSelfEnding(ByVal thetag As FilterTagStringOperations.tagstruct, ByVal exceptionword As String) As Boolean
        If GlobalVariablesClass.pStrictness = GlobalVariablesClass.enumhowstrict.includeunbalanced Then
            ' includeUnbalanced of true means that the user wants us to tolerate bad syntax - up to a point
            If thetag.theword = "p" Then
                Return True ' I noticed sometimes a /p will exist with no p to match it.
            End If
        End If
        If thetag.kind = FilterTagStringOperations.enumStartEnd.finish Then
            ' if this tag is the end-tag of a pair, then obviously it is not self-ending, where we mean by self ending that the tag is not part of a pair
            Return False
        End If

        If thetag.exclamationpoint Then
            Return True ' <!doctype
        End If
        If thetag.selfEnding Then
            Return True ' <br />
        End If

        If SometimesIsSelfEnding(thetag) Then
            ' if we have encountered a tag that sometimes is self ending e.g. 'style' and are not trying to match an end tag such as </style> (exceptionword)
            ' in that case we return true - we say that tag must be self-ending
            If exceptionword <> thetag.theword Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Shared Sub skipSelfEndingTags(ByRef theStack As Stack(Of FilterTagStringOperations.tagstruct), ByRef poptag As FilterTagStringOperations.tagstruct, _
                                         ByRef didapop As Boolean, ByVal exceptionword As String)

        didapop = False
        Do While (isSelfEnding(poptag, exceptionword) And theStack.Count > 0)
            poptag = theStack.Pop
            didapop = True
        Loop

    End Sub

    Public Shared Function removeSandwich(ByRef strAll As String, ByRef thestack As Stack(Of FilterTagStringOperations.tagstruct), _
                                         ByRef structError As FilterTagStringOperations.structalert, ByRef argLastPoppedtag As FilterTagStringOperations.tagstruct, ByVal theLevel As Integer) As Boolean
        Dim poptag As New FilterTagStringOperations.tagstruct
        Dim initialEndtag As FilterTagStringOperations.tagstruct
        Dim Tag2 As FilterTagStringOperations.tagstruct = Nothing
        Dim strShow As String = ""
        Dim LastPoppedTag As FilterTagStringOperations.tagstruct = Nothing
        Dim loopcount As Integer = 0

        ' diagnostic to put in breakpoint if never get out of this nested tag check
        'globalBreakItCount = globalBreakItCount + 1
        'If globalBreakItCount > 3000 Then
        '    Dim i As Integer
        '    i = 6666
        'End If

        ' removeSandwich is a recursive routine to find all tag pairs, whether they occur one after another, or nested in other pairs.  If all tags match, we return true.
        theLevel = theLevel + 1
        If theLevel > 30 Then
            structError.errorMessage = "Error: the nesting level of html tags has exceeded 30"
            Return False
        End If

        If thestack.Count = 0 Then
            Return True
        End If

        poptag = findNextTagThatIsPartOfAPair(thestack, "")
        If poptag.selfEnding Then
            Return True
        End If

        If poptag.kind = FilterTagStringOperations.enumStartEnd.start Then
            structError.errorMessage = "Error 1: we have an unexpected start-tag " & explainTag(poptag) & ". An excerpt: " & _
                    ShowTagOnward(strAll, poptag, 100)
            Return False
        End If
        initialEndtag = poptag
        ' now we have the end-tag of a pair, and must search for the matching start-tag

        If thestack.Count = 0 Then
            structError.errorMessage = "Error 1b: we are missing a start-tag for " & explainTag(initialEndtag) & "."
            Return False
        End If
        Do
            Tag2 = findNextTagThatIsPartOfAPair(thestack, initialEndtag.theword) ' pop the stack until you find either an end-tag of a pair, or a start-tag of a pair

            If Tag2.kind = FilterTagStringOperations.enumStartEnd.start Then
                If Tag2.theword = initialEndtag.theword Then
                    ' our start-tag that we just popped matches the initial-end-tag.  Success....
                    Return True
                ElseIf Not SometimesIsSelfEnding(Tag2) Then
                    structError.errorMessage = "Error 1c: we have the wrong start-tag for " & explainTag(initialEndtag) & ". An excerpt: " & _
                        showTagSandwich(strAll, Tag2, initialEndtag)
                    Return False
                Else
                    ' tag2 is a tag that sometimes lacks an end slash, as well as lacking a end pair tag. 
                    ' so we might have <p></body>
                    If thestack.Count = 0 Then
                        structError.errorMessage = "Error 40: we lack a start-tag for " & explainTag(initialEndtag) & ". An excerpt: " & _
                      showTagSandwich(strAll, Tag2, initialEndtag)
                        Return False
                    End If
                    Continue Do
                End If
            Else
                Exit Do
            End If
        Loop
        If isSelfEnding(Tag2, "") Then
            structError.errorMessage = "Error 1f: we lack a start-tag for " & explainTag(initialEndtag) & ". An excerpt: " & _
                   showTagSandwich(strAll, Tag2, initialEndtag)
            Return False
        End If

        ' we have a nested end tag, so we want to recurse:
        ' </p></div>
        thestack.Push(Tag2)
        Do
            loopcount = loopcount + 1
            If loopcount > 3000 Then
                structError.errorMessage = "Error: the loop in remove-sandwich never ends"
                Return False
            End If
            If removeSandwich(strAll, thestack, structError, LastPoppedTag, theLevel) Then
                ' <p></p></div>
                If thestack.Count = 0 Then
                    structError.errorMessage = "Error 11: we have not found the  start-tag for " & explainTag(initialEndtag) & ". Excerpt: " & _
                    showTagSandwich(strAll, LastPoppedTag, initialEndtag)
                    Return False
                End If

                poptag = findNextTagThatIsPartOfAPair(thestack, initialEndtag.theword)

                If poptag.kind = FilterTagStringOperations.enumStartEnd.start Then
                    If poptag.theword = initialEndtag.theword Then
                        ' <div>     <p></p></div>
                        Return True

                    ElseIf Not poptag.selfEnding Then
                        structError.errorMessage = "Error 12: we have found a non-matching  start-tag for " & explainTag(initialEndtag) & ". Excerpt: " & _
                       showTagSandwich(strAll, poptag, initialEndtag)

                        Return False
                    End If
                End If
                If isSelfEnding(poptag, "") Then
                    structError.errorMessage = "Error 11b: we have not found the  start-tag for " & explainTag(initialEndtag) & ". Excerpt: " & _
                    showTagSandwich(strAll, poptag, initialEndtag)
                    Return False
                End If

                ' stay at same level since have another end tag
                thestack.Push(poptag)
                Continue Do
            Else
                Return False
            End If
        Loop
    End Function


    Public Shared Function findNextTagThatIsPartOfAPair(ByRef theStack As Stack(Of FilterTagStringOperations.tagstruct), ByVal exceptionword As String) As FilterTagStringOperations.tagstruct
        Dim poptag As New FilterTagStringOperations.tagstruct
        Dim DidAPop As Boolean

        poptag = theStack.Pop
        ' the next call keeps popping tags
        skipSelfEndingTags(theStack, poptag, DidAPop, exceptionword) ' tags such as <br /> are not pair of a pair, neither are <meta.../> tags and others.
        Return poptag ' returns the first tag that is not self-ending, or if it can exist in some cases as self-ending, matches 'exceptionword' so its considered part of a pair
    End Function
    Public Shared Sub AddtoStrTemp(ByVal msg As String)
        ' diagnostic routine
        strtemp = strtemp & msg & vbCrLf
    End Sub
    Public Shared Function explainTag(ByVal thetag As FilterTagStringOperations.tagstruct) As String

        Dim strexplain As String
        Dim strPrefix As String = ""
        With thetag
            If thetag.kind = FilterTagStringOperations.enumStartEnd.finish Then
                strPrefix = "/"
            End If
            strexplain = " tag: " & strPrefix & .theword & " at pos: " & .startbracketpos ' & "[" & translatestartend(.kind) & "]"
        End With
        Return strexplain
    End Function

    Public Shared Function showTagSandwich(ByRef strAll As String, ByVal firsttag As FilterTagStringOperations.tagstruct, ByVal endtag As FilterTagStringOperations.tagstruct) As String
        ' show text between two tags (as well as the two tags).  Used for error message explanations.

        Dim strSubstring As String
        strSubstring = FilterTagStringOperations.GetSegment(strAll, firsttag.startbracketpos, endtag.endbracketpos, "ShowTagSandwich")
        Dim thelen As Integer
        thelen = strSubstring.Length
        Dim maxlen As Integer = 400
        If thelen <= maxlen Then
            Return strSubstring
        End If
        Dim diff As Integer
        diff = thelen - maxlen
        strSubstring = FilterTagStringOperations.cutMiddle(strSubstring, diff)
        Return strSubstring
    End Function
    Public Shared Function ShowTagOnward(ByRef strAll As String, ByVal firsttag As FilterTagStringOperations.tagstruct, ByVal wantlength As Integer) As String
        ' show 'wantlength' characters of the webpage, starting at the start-bracket of 'firsttag'.
        Dim lenleft As Integer
        lenleft = strAll.Length - firsttag.startbracketpos
        If wantlength > lenleft Then
            wantlength = lenleft
        End If
        Dim strSubstring As String
        strSubstring = strAll.Substring(firsttag.startbracketpos, wantlength)
        Return strSubstring
    End Function

    Public Shared Function DebugTopOfStack(ByVal prefix As String, ByRef theStack As Stack(Of FilterTagStringOperations.tagstruct)) As String

        Dim thetag As FilterTagStringOperations.tagstruct
        If theStack.Count = 0 Then
            Return (prefix & " empty stack")
        End If
        thetag = theStack.Peek
        Return prefix & "==> " & explainTag(thetag)
    End Function


End Class


