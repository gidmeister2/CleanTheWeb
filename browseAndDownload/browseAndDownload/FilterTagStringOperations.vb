Imports System.Text
Public Class FilterTagStringOperations
    Structure segstruct
        Dim startPosition As Integer
        Dim endPosition As Integer
    End Structure
    Enum enumWarning
        warning = 0
        fatal = 2
    End Enum
    Enum tokenenum
        word = 1
        number = 2
        startOfTag = 3 ' <
        endOfTag = 4 ' >
        unknown = 5
        doubledash = 6 ' --
        singlequote = 7
        doublequote = 8
        slashstar = 9 ' /*
        starslash = 10 ' */
        doubleslash = 11 ' //
        lessthan = 12
        greaterthan = 13
    End Enum
    Enum enumStartEnd
        start = 1
        finish = 2
        unknown = 3
    End Enum
    Enum enumError
        other = 0
        unbalanced = 1
        extratagsprefix = 2
    End Enum
    Structure structalert
        Dim dangerlevel As enumWarning
        Dim errorMessage As String
        Dim errorcode As enumError
    End Structure
    Structure tagstruct
        Dim theword As String
        Dim exclamationpoint As Boolean ' true if this tag starts with an exclamation point (<!doctype...)
        Dim kind As enumStartEnd ' is this tag the start of a pair, or an end of a pair.  note that some tags don't come in pairs, in which case the default is 'start'
        Dim startbracketpos As Integer ' position in the webpage text of the '<' of the tag
        Dim endbracketpos As Integer
        Dim bRemoveContents As Boolean ' should we remove the text between the start tag and the end tag
        Dim bTagIsSafe As Boolean ' should we remove the tag (from the final filtered webpage we give th user
        Dim thestep As Integer ' used to keep track of what we've done to this structure so far
        Dim selfEnding As Boolean ' <br /> for example.  Its  not part of a pair
        Dim index As Integer
        Dim superfluous As Boolean
    End Structure
    Public Shared Function removeWhitespace(ByVal instring As String) As String
        Dim i As Integer
        Dim sbin As New StringBuilder(instring)
        Dim sbout As New StringBuilder("")

        For i = 0 To sbin.Length - 1
            If Not Char.IsWhiteSpace(sbin(i)) Then
                sbout.Append(sbin(i))
            End If
        Next
        Return sbout.ToString
    End Function
    Public Shared Function SafeSubstring(ByVal inString As String, ByVal pos As Integer, ByVal thelen As Integer) As String
        ' get a substring of 'inString', but if the length asked for would make the substring extend beyond the end of 'inString', then reduce that length before trying to get it.
        If pos >= inString.Length Then
            Return ""
        End If
        Dim instrLen As Integer
        instrLen = inString.Length
        Dim allowedlen As Integer
        allowedlen = instrLen - pos
        If allowedlen <= 0 Then
            Return ""
        End If
        If thelen > allowedlen Then
            thelen = allowedlen
        End If
        Return (inString.Substring(pos, thelen))
    End Function
    Public Shared Function removeStyleWhitespace(ByVal instring As String) As String
        Dim len1 As Integer
        Dim len2 As Integer

        ' a style might have white space that we do not need, for instance:
        ' "background-color : brown   ;color: red;   border....
        instring = instring.Trim()

        Do
            len1 = instring.Length
            instring = instring.Replace("  ", " ")
            instring = instring.Replace(" ;", ";")
            instring = instring.Replace("; ", ";")
            instring = instring.Replace(": ", ":")
            len2 = instring.Length
        Loop Until len1 = len2

        Return instring
    End Function
    Public Shared Function cutMiddle(ByVal thestr As String, ByVal numCharsToCut As Integer) As String
        ' shorten a string by cutting out a big chunk in the middle, and putting .... there instead.
        Dim thelen As Integer
        thelen = thestr.Length
        Dim midpoint As Integer
        midpoint = Math.Round((thelen - 1) / 2)
        Dim startCutAt As Integer
        startCutAt = midpoint - numCharsToCut / 2
        RemoveSegment(thestr, startCutAt, startCutAt + numCharsToCut - 1)
        insertInString(thestr, "......", startCutAt)
        Return thestr
    End Function
    Public Shared Sub insertInString(ByRef bigstr As String, ByVal littlestring As String, ByVal pos As Integer)
        ' assume if pos is 3, that we insert right after position 2.   so 'abcdefg' becomes abc....defg
        bigstr = bigstr.Substring(0, pos) & littlestring & bigstr.Substring(pos)
    End Sub
    Public Shared Sub showDiagnosticEntireWebpage(ByVal strAll As String, ByVal startbracketpos As Integer, ByVal endbracketpos As Integer)
        ' this shows a dialog used for diagnostics
        Dim sbAll As New StringBuilder("")
        Dim calledFromWhere As String = "ShowDiagnosticEntireWebPage"
        sbAll.AppendLine("scroll down until you see a line of asterixes with the words '****trouble spot'")
        sbAll.AppendLine("")
        sbAll.AppendLine(GetSegment(strAll, 0, startbracketpos - 1, calledFromWhere))
        sbAll.AppendLine("")
        sbAll.AppendLine("")

        sbAll.AppendLine("***************************START OF TROUBLE SPOT......**************************")
        sbAll.AppendLine(GetSegment(strAll, startbracketpos, endbracketpos, calledFromWhere))
        sbAll.AppendLine("***************************END OF TROUBLE SPOT........**************************")
        sbAll.AppendLine("")
        sbAll.AppendLine("")
        sbAll.AppendLine(strAll.Substring(endbracketpos + 1))
        Dim fd As New FormDiagnostic
        fd.TextBoxMessage.Text = sbAll.ToString
        fd.ShowDialog()
    End Sub
    Public Shared Function SafeSubstring(ByVal inString As String, ByVal pos As Integer) As String
        ' return a substring starting at position pos, but only if pos is not beyond the end of the main string (inString)
        If pos >= inString.Length Then
            Return ""
        End If
        Return (inString.Substring(pos))
    End Function
    Public Shared Function GetSegment(ByRef strAll As String, ByVal startPosition As Integer, ByVal endPosition As Integer, ByVal calledFromWhere As String) As String
        ' get a segment from passed string strAll that starts at a given position and ends at a given position.  'calledFromWhere' is used for diagnostics.
        Dim thelen As Integer
        Dim strReturn As String = ""
        thelen = strAll.Length
        Dim lastpos As Integer
        lastpos = thelen - 1
        If startPosition > lastpos Then
            Return strReturn
        End If
        If endPosition > lastpos Then
            endPosition = lastpos
        End If
        If lastpos = endPosition Then
            strReturn = strAll.Substring(startPosition)
            Return (strReturn)
        End If

        strReturn = strAll.Substring(startPosition, endPosition - startPosition + 1)
        Return strReturn
    End Function
    Public Shared Sub RemoveSegment(ByRef strAll As String, ByVal startPosition As Integer, ByVal endPosition As Integer)
        ' remove a segment from passed string strAll where the segment starts at a given position and ends at a given position.
        Dim thelen As Integer
        thelen = strAll.Length
        Dim lastpos As Integer
        lastpos = thelen - 1
        If startPosition > lastpos Then
            Exit Sub
        End If
        If endPosition > lastpos Then
            endPosition = lastpos
        End If
        If lastpos = endPosition Then
            strAll = strAll.Substring(0, startPosition)
            Exit Sub
        End If

        strAll = strAll.Substring(0, startPosition) & strAll.Substring(endPosition + 1)

    End Sub

    Public Shared Function keepdigits(ByVal token As String) As String
        ' given a string such as '98soup', return the '98', not the 'soup'
        Dim sb As New StringBuilder("")
        Dim i As Integer
        For i = 0 To token.Length - 1
            If Char.IsDigit(token(i)) Then
                sb.Append(token(i))
            Else
                Exit For
            End If
        Next
        Return sb.ToString
    End Function
    Public Shared Sub RemoveAllMarkedSegments(ByRef strAll As String, ByVal removelist As List(Of segstruct))
        ' given a list of startPosition/endposition pairs, go through strAll, removing all text that is in those pairs.  For instance, if strall was "Gratitude' and removelist had 
        ' just one item, with a startpos of 1 and a endpos of 3, then the result sets strAll to 'Gitude' (counting starts at zero).
        Dim sb2 As New StringBuilder("")
        Dim i As Integer
        Dim lastpos As Integer = strAll.Length - 1
        Dim startPosition As Integer
        Dim endPosition As Integer
        Dim previousstartpos As Integer
        Dim previousendpos As Integer
        For i = 0 To removelist.Count - 1
            startPosition = removelist(i).startPosition
            endPosition = removelist(i).endPosition
            If i = 0 Then
                If startPosition > 0 Then
                    sb2.Append(strAll.Substring(0, startPosition))
                End If
            Else
                sb2.Append(strAll.Substring(previousendpos + 1, startPosition - previousendpos - 1))
            End If
            previousendpos = endPosition
            previousstartpos = startPosition
        Next
        If endPosition < lastpos Then
            sb2.Append(strAll.Substring(endPosition + 1, lastpos - endPosition))
        End If
        strAll = sb2.ToString
    End Sub
    Public Shared Function getToken(ByVal strAll As String, ByRef Pos As Integer, ByRef thetoken As String, _
                     ByRef tokentype As tokenenum, ByRef tokenstartsAt As Integer, ByRef encounteredNewline As Boolean, ByRef incommentSlashdouble As Boolean, ByVal intag As Boolean) As Boolean

        ' given the string strAll, and a position 'Pos' in the string", find the next token, which might be something like IMG, or <, or --, etc.
        ' we also decide what type it is and set the tokentype to that.  We also store the position wher the token starts, and we return two flags, one of which says we 
        ' encountered a newline as we proceed letter by letter to find the token, and the other says whether we found a double-slash (used for comments)
        Dim sb As New StringBuilder("")
        Dim firstletter As String
        Dim secondletter As String
        Dim priorletter As String
        thetoken = ""
        tokentype = tokenenum.unknown
        Do
            If Pos >= strAll.Length Then
                Return False
            End If

            If Char.IsWhiteSpace(strAll(Pos)) Then
                If strAll(Pos) = vbCr Or strAll(Pos) = vbLf Then
                    encounteredNewline = True
                    incommentSlashdouble = False
                End If
                Pos = Pos + 1
                Continue Do
            End If
            Exit Do
        Loop
        firstletter = strAll(Pos)
        If Pos < strAll.Length - 1 Then
            secondletter = strAll(Pos + 1)
        Else
            secondletter = ""
        End If
        If Pos > 0 Then
            priorletter = strAll(Pos - 1)
        Else
            priorletter = ""
        End If
        If firstletter = Chr(34) Then
            tokentype = tokenenum.doublequote
        ElseIf firstletter = "'" Then
            tokentype = tokenenum.singlequote
        ElseIf Char.IsLetter(firstletter) Then
            tokentype = tokenenum.word
        ElseIf Char.IsDigit(firstletter) Then
            tokentype = tokenenum.number
        ElseIf firstletter = "<" Then
            If secondletter = "" Or Char.IsWhiteSpace(secondletter) Then
                tokentype = tokenenum.lessthan
            Else
                tokentype = tokenenum.startOfTag
            End If

        ElseIf firstletter = ">" Then
            If Not intag Then
                tokentype = tokenenum.greaterthan
            Else
                tokentype = tokenenum.endOfTag
            End If

        ElseIf firstletter = "-" And secondletter = "-" Then
            tokentype = tokenenum.doubledash
        ElseIf firstletter = "/" And secondletter = "*" Then
            tokentype = tokenenum.slashstar
        ElseIf firstletter = "*" And secondletter = "/" Then
            tokentype = tokenenum.starslash
        ElseIf firstletter = "/" And secondletter = "/" Then
            tokentype = tokenenum.doubleslash
        Else
            tokentype = tokenenum.unknown
        End If
        tokenstartsAt = Pos
        Do
            If Pos >= strAll.Length Then
                Exit Do
            End If
            If Pos > tokenstartsAt Then
                If Pos > 0 Then
                    priorletter = strAll(Pos - 1)
                End If
                If Pos > tokenstartsAt Then
                    If shouldendtoken(strAll(Pos), tokentype, priorletter, tokenstartsAt, Pos) Then
                        Exit Do
                    End If
                End If
            End If

            sb.Append(strAll(Pos))
            Pos = Pos + 1
        Loop
        thetoken = sb.ToString
        Return True
    End Function
  
    Public Shared Sub getnonblanksegment(ByVal savefirstpos As Integer, ByRef segment As String, ByVal strAll As String, ByRef copypos As Integer)
        copypos = savefirstpos
        segment = ""
        ' starting at position copypos, get a sequence of characters until you hit a 'separator' such as a blank space, or a linefeed, or until your run out of characters provided.
        ' also we consider a '>' as a separator here.
        Dim letter As Char
        Do
            letter = strAll(copypos)
            If Char.IsSeparator(letter) Then
                Exit Do
            End If
            If letter = ">" Then
                Exit Do
            End If
            segment = segment & letter
            copypos = copypos + 1
            If copypos = strAll.Length Then
                Exit Do
            End If
        Loop

    End Sub
    Public Shared Function getString(ByVal strAll As String, ByVal copypos As Integer, _
                              ByRef theString As String, ByRef FirstPosOfString As Integer, _
                              ByRef LastPosOfString As Integer, ByRef hadquotes As Boolean, ByRef incommentSlashDouble As Boolean, _
                              ByVal intag As Boolean) As Boolean
        ' get a string, including quotes.
        ' if there are no quotes around it, just get the text.
        Dim thetoken As String = ""
        Dim tokentype As tokenenum = tokenenum.unknown
        Dim tokenstartsAt As Integer = 0
        Dim firsttoken As Boolean
        Dim quotechar As String = ""
        Dim previoustoken = ""
        Dim encounteredCR As Boolean = False
        Dim savefirstpos As Integer
        Dim segment As String = ""

        theString = ""
        firsttoken = True
        savefirstpos = copypos
        Do While getToken(strAll, copypos, thetoken, tokentype, tokenstartsAt, encounteredCR, incommentSlashDouble, intag)
            If firsttoken Then
                If thetoken <> Chr(34) And thetoken <> "'" Then
                    hadquotes = False
                    getnonblanksegment(savefirstpos, segment, strAll, copypos)
                    theString = segment
                    LastPosOfString = copypos - 1
                    FirstPosOfString = copypos - segment.Length
                    Return True

                End If
                hadquotes = True
                quotechar = thetoken
                firsttoken = False
                previoustoken = thetoken
                FirstPosOfString = tokenstartsAt
                Continue Do
            End If
            If thetoken = quotechar And previoustoken <> "\" Then
                LastPosOfString = copypos - 1
                theString = strAll.Substring(FirstPosOfString, LastPosOfString - FirstPosOfString + 1)
                Return True
            End If
            previoustoken = thetoken
        Loop
        Return False
    End Function
    Public Shared Function shouldendtoken(ByVal letter As String, ByVal tokentype As tokenenum, ByVal priorletter As String, ByVal tokenstartsAt As Integer, ByVal currentpos As Integer) As Boolean
        ' this function is called when we already know the type of a token, but we don't know if we've found the last character of the token.  We do some checks of the
        ' letter at the current position to decide whether we've reached the end.
        If Char.IsWhiteSpace(letter) Then
            Return True
        End If
        Select Case tokentype
            Case tokenenum.endOfTag, tokenenum.greaterthan ' >
                Return True
            Case tokenenum.singlequote
                Return True
            Case tokenenum.doublequote
                Return True
            Case tokenenum.number
                If Not Char.IsDigit(letter) And letter <> "+" And letter <> "-" And letter <> "." Then
                    ' this does not cope with 55+ or 5...5 or 6-8
                    ' but its good enough for our purposes
                    Return True
                End If
            Case tokenenum.startOfTag, tokenenum.lessthan ' <
                Return True
            Case tokenenum.unknown
                Return True
            Case tokenenum.doubledash
                If letter <> "-" Then
                    Return True
                End If
                If priorletter = "-" Then
                    Return False
                End If
                Return True
            Case tokenenum.word
                If Not Char.IsLetterOrDigit(letter) And letter <> "_" And letter <> "-" Then
                    Return True
                End If
            Case tokenenum.slashstar '/*
                If currentpos >= tokenstartsAt + 2 Then
                    Return True
                End If

            Case tokenenum.starslash
                If currentpos >= tokenstartsAt + 2 Then
                    Return True
                End If

            Case tokenenum.doubleslash
                If currentpos >= tokenstartsAt + 2 Then
                    Return True
                End If
        End Select

        Return False

    End Function
    Public Shared Sub ParseDimension(ByVal thestring As String, ByRef theNum As Double, ByRef theunits As String)
        Dim i As Integer
        Dim numstr As String = ""
        Dim letter As String

        ' widths might look like "40px", "30em", "70" etc.  Try and get the number and the units separately and return each.  Assume a default of "px"
        If thestring.Length = 0 Then
            theNum = 0
            theunits = "px"
            Exit Sub
        End If
        For i = 0 To thestring.Length - 1
            letter = thestring(i)
            If Char.IsDigit(letter) Or letter = "." Then
                numstr = numstr & letter
            Else
                Exit For
            End If
        Next
        If numstr.Length = 0 Then
            theNum = 0
            theunits = thestring
            Exit Sub
        End If
        If Not Double.TryParse(numstr, theNum) Then
            theNum = 0
            theunits = "px"
            Exit Sub
        End If
        If i < thestring.Length Then
            theunits = thestring.Substring(i).ToLower
        Else
            theunits = "px"
        End If
    End Sub
    Public Shared Function removeQuotes(ByVal thestr As String) As String
        ' assuming the string coming in is surrounded by quotes 'hi there' - remove those quotes
        Dim thelen As Integer = thestr.Length
        If thelen < 2 Then
            Return thestr
        End If
        thestr = thestr.Substring(1, thelen - 2)
        Return thestr
    End Function
    Public Shared Function showExcerpt(ByVal strAll As String, ByVal StartAt As Integer) As String
        Dim thelen As Integer
        thelen = strAll.Substring(StartAt).Length
        Dim desiredlength = 80
        If desiredlength > thelen Then
            desiredlength = thelen
        End If
        Return strAll.Substring(StartAt, desiredlength)
    End Function
    Public Shared Sub InsertSegment(ByRef strAll As String, ByVal newseg As String, ByVal startPosition As Integer)
        ' insert a string at postion startPosition in the big string: strAll
        If startPosition >= strAll.Length Then
            strAll = strAll & newseg
        ElseIf startPosition = 0 Then
            strAll = newseg & strAll
        Else
            strAll = strAll.Substring(0, startPosition) & newseg & strAll.Substring(startPosition)
        End If
    End Sub
    Public Shared Sub ReplaceSegment(ByRef strAll As String, ByVal newseg As String, ByVal startPosition As String, ByVal endPosition As Integer)

        ' given strAll remove whatever is between startPosition and endPosition and put in segment newseg there instead.
        Dim thelen As Integer
        thelen = strAll.Length
        Dim lastpos As Integer
        lastpos = thelen - 1
        If startPosition > lastpos Then
            Exit Sub
        End If
        If endPosition > lastpos Then
            endPosition = lastpos
        End If
        If lastpos = endPosition Then
            strAll = strAll.Substring(0, startPosition) & newseg
            Exit Sub
        End If

        strAll = strAll.Substring(0, startPosition) & newseg & strAll.Substring(endPosition + 1)

    End Sub
    Public Shared Function CompleteTheURL(ByVal smallURl As String) As String
        Dim strbigURL As String = GlobalVariablesClass.pCurrentlyWorkingOnURL
        Dim lowercaseurl As String = smallURl.ToLower
        Dim doubleslashpos As Integer
        Dim strDomain As String
        Dim firstslashpos As Integer
        Dim lastslashpos As Integer

        ' if you find a URL such as images/myfile.jpg, and the domain is www.microsoft.com, make it to www.microsoft.com/images/myfile.jpg
        doubleslashpos = strbigURL.IndexOf("//")
        If doubleslashpos < 0 Then
            Return (smallURl)
        End If
        firstslashpos = strbigURL.IndexOf("/", doubleslashpos + 2)
        lastslashpos = strbigURL.LastIndexOf("/")
        If lowercaseurl.StartsWith("http") Then
            Return smallURl
        End If

        ' http://www.mydomain.com/myfolder/myfile.htm and /horace.htm
        If lowercaseurl.StartsWith("/") Then
            If firstslashpos < 0 Then
                strDomain = strbigURL
            Else
                strDomain = strbigURL.Substring(0, firstslashpos)
            End If

            Return strDomain & smallURl
        End If

        ' http://www.mydomain.com/myfolder/myfile.htm and horace.htm
        If lastslashpos > doubleslashpos + 1 Then
            Return strbigURL.Substring(0, lastslashpos + 1) & smallURl
        Else
            Return strbigURL & "/" & smallURl
        End If
        Return smallURl

    End Function

End Class
