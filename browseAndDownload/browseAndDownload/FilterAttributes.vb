Imports System.Text
Public Class FilterAttributes


    Structure AttributeStruct
        ' attribute could be width, height, color, etc
        Dim attribname As String
        Dim attribvalue As String
        Dim startPosition As Integer ' pos' means position
        Dim endPosition As Integer
        Dim PosValueStarts As Integer
        Dim attribIsSafe As Boolean ' onclick would be an unsafe attribute, because it can call a script.
        Dim thestep As Integer
        Dim widthExceeded As Boolean ' we might set a maximum width, and if this is a width attribute, and exceeds the max-width, we would set this flag.
        Dim hasQuote As Boolean ' if the value has quotes around it (width = "5") then this is set to true
        Dim quotechar As String
    End Structure
    Public Shared Function attributeIsSafe(ByVal theAttribute As String, ByVal attribvalue As String) As Boolean
        ' is the attribute safe (it won't run a script on the user's computer)?
        theAttribute = theAttribute.ToLower

        If GlobalVariablesClass.pFilterOutTags = GlobalVariablesClass.enumfilterwhat.keep Then
            ' "keep" means keep all attributes, safe or not.
            Return True
        End If
        If attribvalue.ToLower.Trim.StartsWith("javascript") Then
            ' scripts are unsafe.
            Return False
        End If
        If extend.SafeAttributeArray.Contains(theAttribute) Then
            Return True
        End If
        Return False
    End Function


    Public Shared Function dangerousStyles(ByVal thetoken As String, ByVal tokenvalue As String) As Boolean
        If extend.dangerousStylesArray.Contains(thetoken) Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Shared Function ProcessAttributes(ByRef strAll As String, ByVal onetagstruct As FilterTagStringOperations.tagstruct, ByVal maxwidth As Integer, _
                                       ByVal maxheight As Integer, ByRef errorstruct As FilterTagStringOperations.structalert, _
                                         ByRef listofremove As List(Of FilterTagStringOperations.segstruct)) As Boolean
        ' for a given tag, remove any unsafe attributes, and modify some of the others, for instance, for links, we might add 'target=_blank' so that
        ' when the webpage is downloaded to your PC, and a link clicked in it, that it will open a separate window.  There are other changes we make as well.
        Dim nextToken As String = ""
        Dim nexttokentype As FilterTagStringOperations.tokenenum
        Dim firsttokentype As FilterTagStringOperations.tokenenum
        Dim nextTokenStartsAt As Integer
        Dim firsttoken As String = ""
        Dim attribvaluestring As String = ""
        Dim FirstPosOfString As Integer
        Dim LastPosOfString As Integer
        Dim theUrl As String
        Dim firsttokenStartsAt As Integer
        Dim pos As Integer = 0
        Dim copypos As Integer = 0
        Dim strTagContents As String
        Dim bFirstword As Boolean = True
        Dim theNum As Double
        Dim theUnits As String = ""
        Dim attribstack As New Stack(Of AttributeStruct)
        Dim hadwidthproblem As Boolean = False
        Dim hadheight As Boolean = False
        Dim oneAttrib As New AttributeStruct
        Dim haveImage As Boolean = False
        Dim haveAnchor As Boolean = False
        Dim alreadyHaveStyle As Boolean = False
        Dim bookmark As Boolean = False
        Dim completedURL As String
        Dim encounteredCR As Boolean
        Dim incommentslashdouble As Boolean
        Dim smallURL As String
        Dim lastslashpos As Integer
        Dim strNewWidth As String = ""
        Dim strNewStyle = ""
        Dim newtarget As String = ""

        If onetagstruct.kind = FilterTagStringOperations.enumStartEnd.start Then
            If onetagstruct.theword = "a" Then
                haveAnchor = True
            ElseIf onetagstruct.theword = "img" Then
                haveImage = True
            End If
        End If
        ' onetagstruct has the start and end position of a tag in "strAll", and that tag may have attributes in it.
        strTagContents = FilterTagStringOperations.GetSegment(strAll, onetagstruct.startbracketpos + 1, onetagstruct.endbracketpos - 1, _
                                                              GlobalVariablesClass.pCurrentlyWorkingOnURL & " process Attributes - start " & onetagstruct.theword & " (" & onetagstruct.index & ")").Trim
        ' loop through the words in the brackets, for instance img src='....' align="right"
        Do While FilterTagStringOperations.getToken(strTagContents, pos, firsttoken, firsttokentype, firsttokenStartsAt, encounteredCR, incommentslashdouble, True)
            If firsttoken = "/" Then
                Continue Do
            End If
            If bFirstword Then
                If firsttoken = "!" Then
                    Continue Do
                End If
                bFirstword = False
                Continue Do
            End If
            ' at this point either have the firstword (e.g. 'img') or a word or token after it.
            copypos = pos
            If FilterTagStringOperations.getToken(strTagContents, copypos, nextToken, nexttokentype, nextTokenStartsAt, encounteredCR, incommentslashdouble, True) Then
                If nextToken = "=" Then
                    ClearAttributeStruct(oneAttrib)
                    ' we have an attribute, because we have a token followed by an equal sign.  So not get the string that follows the equal sign.

                    If Not FilterTagStringOperations.getString(strTagContents, copypos, attribvaluestring, FirstPosOfString, LastPosOfString, oneAttrib.hasQuote, incommentslashdouble, True) Then
                        errorstruct.errorMessage = "Error A: you have an unterminated string - an excerpt follows: " & FilterTagStringOperations.SafeSubstring(attribvaluestring, 0, 70)
                        errorstruct.dangerlevel = FilterTagStringOperations.enumWarning.fatal
                        Return False
                    End If

                    ' at this point, we have some-token = some-string, so we put all that information in a structure:
                    With oneAttrib
                        .attribname = firsttoken.ToLower
                        .startPosition = firsttokenStartsAt
                        .PosValueStarts = FirstPosOfString
                        .endPosition = LastPosOfString
                        If oneAttrib.hasQuote Then
                            .quotechar = attribvaluestring(0)
                            ' we store the attribute value without quotes around them
                            .attribvalue = FilterTagStringOperations.removeQuotes(attribvaluestring)
                        Else
                            .quotechar = String.Empty
                            .attribvalue = attribvaluestring
                        End If
                        .attribIsSafe = attributeIsSafe(.attribname, .attribvalue)

                        If .attribIsSafe Then
                            If extend.heightarray.Contains(.attribname) Then
                                hadheight = True
                            End If
                            If extend.widtharray.Contains(.attribname) Then
                                FilterTagStringOperations.ParseDimension(.attribvalue, theNum, theUnits)
                                If theNum > maxwidth Then ' And theUnits = "px" Then
                                    .widthExceeded = True
                                    hadwidthproblem = True
                                End If
                            End If
                        End If
                    End With
                    If oneAttrib.attribname = "style" Then
                        alreadyHaveStyle = True
                    End If
                    If oneAttrib.attribname = "href" Then
                        If oneAttrib.attribvalue.StartsWith("#") Then
                            bookmark = True
                        End If

                    End If
                    attribstack.Push(oneAttrib) ' put the attribute structures on a stack (these attributes are from one tag only)
                    pos = LastPosOfString + 1
                End If
            End If
        Loop

        Do While attribstack.Count > 0
            oneAttrib = attribstack.Pop
            If Not oneAttrib.attribIsSafe Then
                FilterTagStringOperations.RemoveSegment(strTagContents, oneAttrib.startPosition, oneAttrib.endPosition)
            ElseIf oneAttrib.attribname = "style" Then
                processStyle(oneAttrib, strTagContents, maxwidth, haveImage)
            ElseIf oneAttrib.attribname = "href" And Not bookmark And haveAnchor Then
                theUrl = oneAttrib.attribvalue
                If GlobalVariablesClass.pCompleteTheURLs Then
                    ' convert urls such as /images/myimage.jpg to urls such as http://www.msoft.com/images/image.jpg.  The reason for this
                    ' is because links won't work once the web page is on your local PC, unless they refer to the original site.
                    completedURL = FilterTagStringOperations.CompleteTheURL(theUrl)
                    If theUrl <> completedURL Then
                        ' a domain had to be prefixed to the URL
                        If Not oneAttrib.hasQuote Then
                            ' if the attribute value was not surrounded by quotes, we regard this as a minor error, so we provide the quotes
                            oneAttrib.quotechar = Chr(34)
                            LastPosOfString = LastPosOfString + 2
                        End If
                        completedURL = oneAttrib.quotechar & completedURL & oneAttrib.quotechar
                        FilterTagStringOperations.ReplaceSegment(strTagContents, completedURL, oneAttrib.PosValueStarts, oneAttrib.endPosition)
                    Else
                        If Not oneAttrib.hasQuote Then
                            oneAttrib.quotechar = Chr(34)
                            LastPosOfString = LastPosOfString + 2
                            FilterTagStringOperations.ReplaceSegment(strTagContents, oneAttrib.quotechar & theUrl & oneAttrib.quotechar, oneAttrib.PosValueStarts, oneAttrib.endPosition)
                        End If
                    End If
                Else
                    If Not oneAttrib.hasQuote Then
                        oneAttrib.quotechar = Chr(34)
                        LastPosOfString = LastPosOfString + 2
                        FilterTagStringOperations.ReplaceSegment(strTagContents, oneAttrib.quotechar & theUrl & oneAttrib.quotechar, oneAttrib.PosValueStarts, oneAttrib.endPosition)
                        ' put quotes around the attribute value (remember that we took them away earlier)
                    End If
                End If
            ElseIf hadwidthproblem And (extend.widtharray.Contains(oneAttrib.attribname) Or extend.heightarray.Contains(oneAttrib.attribname)) Then
                ' remove the width attribute completely, but may put it back later, with a smaller value.  This only makes sense for applications
                ' where the html output is supposed to fit in a column of a table of some sort.
                FilterTagStringOperations.RemoveSegment(strTagContents, oneAttrib.startPosition, oneAttrib.endPosition)
            ElseIf oneAttrib.attribname = "src" And haveImage Then
                theUrl = oneAttrib.attribvalue
                If GlobalVariablesClass.pCompleteTheURLs Then
                    completedURL = FilterTagStringOperations.CompleteTheURL(theUrl)
                    If theUrl <> completedURL Then
                        ' the URL was incomplete, so we had to prefix a domain
                        If Not oneAttrib.hasQuote Then
                            ' if the attribute value was not surrounded by quotes, we regard this as a minor error, so we provide the quotes
                            oneAttrib.quotechar = Chr(34)
                            LastPosOfString = LastPosOfString + 2
                        End If
                        completedURL = oneAttrib.quotechar & completedURL & oneAttrib.quotechar
                        FilterTagStringOperations.ReplaceSegment(strTagContents, completedURL, oneAttrib.PosValueStarts, oneAttrib.endPosition)
                    Else
                        If Not oneAttrib.hasQuote Then
                            oneAttrib.quotechar = Chr(34)
                            LastPosOfString = LastPosOfString + 2
                            FilterTagStringOperations.ReplaceSegment(strTagContents, oneAttrib.quotechar & theUrl & oneAttrib.quotechar, oneAttrib.PosValueStarts, oneAttrib.endPosition)
                        End If
                    End If
                ElseIf GlobalVariablesClass.pKeepImages Then
                    ' we'll assume all images on the site are in same folder, not in different subfolders.
                    lastslashpos = theUrl.LastIndexOf("/")
                    If lastslashpos > -1 And lastslashpos < theUrl.Length - 1 Then
                        ' we get the filename of the image, without the path.
                        smallURL = theUrl.Substring(lastslashpos + 1)
                        If Not oneAttrib.hasQuote Then

                            oneAttrib.quotechar = Chr(34)
                            LastPosOfString = LastPosOfString + 2 ' add 2, because of quotes
                            FilterTagStringOperations.ReplaceSegment(strTagContents, oneAttrib.quotechar & smallURL & oneAttrib.quotechar, oneAttrib.PosValueStarts, oneAttrib.endPosition)
                        End If
                    Else
                        ' the URL has no path specified
                        smallURL = theUrl
                        If Not oneAttrib.hasQuote Then
                            oneAttrib.quotechar = Chr(34)
                            LastPosOfString = LastPosOfString + 2
                            FilterTagStringOperations.ReplaceSegment(strTagContents, oneAttrib.quotechar & smallURL & oneAttrib.quotechar, oneAttrib.PosValueStarts, oneAttrib.endPosition)
                        End If
                    End If
                End If
            End If
        Loop

        If haveImage Then
            If Not alreadyHaveStyle Then
                strNewStyle = " style='max-width:" & maxwidth & "px;' " ' this is useful for some applications, if you are inserting the downloaded html
                ' into a narrow column in a table 
            End If
        End If

        If haveAnchor And Not bookmark Then
            ' we make all links open a separate window
            newtarget = " target=" & Chr(34) & "_blank" & Chr(34) & " "
        End If

        If hadwidthproblem Then
            strNewWidth = " width=" & Chr(34) & maxwidth & Chr(34) & " "
        End If

        If strTagContents.EndsWith("/") Then
            strTagContents = strTagContents.Substring(0, strTagContents.Length - 1) & strNewStyle & strNewWidth & newtarget & "/"
        Else
            strTagContents = strTagContents & strNewStyle & strNewWidth & newtarget
        End If
        strAll = strAll.Substring(0, onetagstruct.startbracketpos + 1) & strTagContents & strAll.Substring(onetagstruct.endbracketpos)
        Return True
    End Function

    Public Shared Sub processStyle(ByVal poppedStylePair As AttributeStruct, ByRef strAllInTag As String, _
                                   ByVal maxwidth As Integer, ByVal isImage As Boolean)
        Dim thestring As String = poppedStylePair.attribvalue
        Dim oldstring As String

        ' a style has its own name/value pairs that have to be removed if they are unsafe
        oldstring = thestring
        cleanStyle(thestring, maxwidth, isImage)
        If oldstring <> thestring Then
            ' the style was altered by the cleaning process
            With poppedStylePair
                If thestring.Length > 0 Then
                    ' the style wasn't completely eliminated, there were some safe pairs in it.
                    strAllInTag = strAllInTag.Substring(0, .PosValueStarts) _
                    & .quotechar & thestring & .quotechar _
                & strAllInTag.Substring(.endPosition + 1)
                Else
                    ' the style was completely eliminated, maybe because all the attributes within it were dangerous
                    strAllInTag = strAllInTag.Substring(0, .startPosition) _
                                   & strAllInTag.Substring(.endPosition + 1)
                End If
            End With
        End If
    End Sub
    Public Shared Sub ClearAttributeStruct(ByRef attstruct As AttributeStruct)
        With attstruct
            .attribIsSafe = True
            .attribname = String.Empty
            .attribvalue = String.Empty
            .endPosition = -1
            .startPosition = -1
            .thestep = 0
            .widthExceeded = False
            .quotechar = ""
            .PosValueStarts = 0
        End With
    End Sub
    Public Shared Function cleanStyle(ByRef thestring As String, ByVal maxwidth As Integer, ByVal isImage As Boolean) As Boolean
        Dim thetoken As String = ""
        Dim previousToken As String = ""
        Dim tokentype As FilterTagStringOperations.tokenenum = FilterTagStringOperations.tokenenum.unknown
        Dim tokenstartsAt As Integer = 0
        Dim Pos As Integer
        Dim thelen As Integer
        Dim semicolonpos As Integer
        Dim quotechar As String = "'"
        Dim hadquotechar As Boolean = False
        Dim attributevalue As String
        Dim colonpos As Integer
        Dim Pos2 As Integer
        Dim thetoken2 As String = ""
        Dim tokentype2 As FilterTagStringOperations.tokenenum
        Dim theEndpos As Integer
        Dim attribstack As New Stack(Of AttributeStruct)
        Dim oneAttrib As New AttributeStruct
        Dim hadheight As Boolean = False
        Dim hadwidthproblem As Boolean = False
        Dim theNum As Double
        Dim theUnits As String = ""
        Dim encounteredCR As Boolean
        Dim incommentslashdouble As Boolean
        thestring = FilterTagStringOperations.removeStyleWhitespace(thestring.ToLower)

        thelen = thestring.Length
        ' styles look like: "color:brown;width:300px;..."  it may have dangerous pairs which we have to remove, and we also have the 
        ' option of setting a maximum width.  We gather information on each pair into a structure, push it onto a stack and then pop them and 
        ' process each popped one.
        ' in the above example, we would call 'color' an attribute-name, and 'brown' an attribute-value
        Do While True
            previousToken = thetoken
            If Not FilterTagStringOperations.getToken(thestring, Pos, thetoken, tokentype, tokenstartsAt, encounteredCR, incommentslashdouble, True) Then
                Exit Do
            End If
            thetoken = thetoken.ToLower
            Pos2 = Pos
            If Not FilterTagStringOperations.getToken(thestring, Pos2, thetoken2, tokentype2, colonpos, encounteredCR, incommentslashdouble, True) Then
                Exit Do
            End If
            If thetoken2 <> ":" Then
                Continue Do
            End If
            semicolonpos = thestring.IndexOf(";", Pos2)
            attributevalue = "0"

            If semicolonpos >= 0 Then
                attributevalue = thestring.Substring(colonpos + 1, semicolonpos - colonpos - 1)
                theEndpos = semicolonpos ' last position to remove
            Else
                attributevalue = thestring.Substring(colonpos + 1)
                theEndpos = thestring.Length - 1
            End If
            If attributevalue.Length > 0 Then
                ClearAttributeStruct(oneAttrib)
                With oneAttrib
                    .attribname = thetoken.ToLower
                    .startPosition = tokenstartsAt
                    .quotechar = String.Empty
                    .attribvalue = attributevalue
                    .PosValueStarts = tokenstartsAt
                    .endPosition = theEndpos
                    .attribIsSafe = Not dangerousStyles(.attribname, .attribvalue)
                    If .attribIsSafe Then
                        If extend.heightarray.Contains(.attribname) Then
                            hadheight = True
                        End If
                        If extend.widtharray.Contains(.attribname) Then
                            FilterTagStringOperations.ParseDimension(.attribvalue, theNum, theUnits)
                            If theNum > maxwidth Then ' And theUnits = "px" Then
                                .widthExceeded = True
                                hadwidthproblem = True
                            End If
                        End If
                    End If
                End With

                attribstack.Push(oneAttrib)
            End If
            Pos = theEndpos + 1
        Loop
    
        Do While attribstack.Count > 0
            oneAttrib = attribstack.Pop
            If hadwidthproblem And (extend.widtharray.Contains(oneAttrib.attribname) Or extend.heightarray.Contains(oneAttrib.attribname)) Then
                FilterTagStringOperations.RemoveSegment(thestring, oneAttrib.startPosition, oneAttrib.endPosition) ' get rid of width attribute, if it is too large
            ElseIf Not oneAttrib.attribIsSafe Then
                FilterTagStringOperations.RemoveSegment(thestring, oneAttrib.startPosition, oneAttrib.endPosition)
            End If
        Loop

        Dim strNewWidth As String = ""
        Dim semi As String
        If hadwidthproblem Then
            thestring = thestring.Trim
            If thestring.EndsWith(";") Or thestring.Length = 0 Then
                semi = ""
            Else
                semi = ";"
            End If
            strNewWidth = semi & " width:" & maxwidth & "px; "
        End If
        thestring = thestring & strNewWidth
        If Not thestring.ToLower.Contains("max-width") Then
            If isImage Then
                thestring = thestring.Trim
                If thestring.EndsWith(";") Or thestring.Length = 0 Then
                    semi = ""
                Else
                    semi = ";"
                End If
                thestring = thestring & semi & "max-width:" & maxwidth & "px"
            End If
        End If
        Return True
    End Function
    Public Shared Function printAttributeStack(ByVal attstack As Stack(Of AttributeStruct)) As String
        Dim sb As New stringbuilder("")
        For Each ast In attstack
            With ast
                sb.AppendLine(.attribname & "=" & .attribvalue & " " & translatesafe(.attribIsSafe))
            End With
        Next
        Return sb.ToString
    End Function
    Private Shared Function translatesafe(ByVal issafe As Boolean) As String
        If issafe Then
            Return ("safe")
        Else
            Return ("UNSAFE")
        End If
    End Function
End Class
