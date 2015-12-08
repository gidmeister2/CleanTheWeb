Imports System.Text
Public Class GlobalVariablesClass
    Public Enum enumhowstrict
        includeunbalanced = 1
        forbidunbalanced = 2
        separatefileForUnbalanced = 3
    End Enum
    Private Shared _url As String
    Private Shared _strictness As enumhowstrict = enumhowstrict.includeunbalanced
    Private Shared _extensions As New List(Of String)
    Private Shared _foundtheselinks As String
    Private Shared _alsoNoExt As Boolean
    Private Shared _stripOutTags As enumfilterwhat = enumfilterwhat.DangerousTagsOnly
    Private Shared _currentlyWorkingOnURL As String
    Private Shared _maxHeight As Integer = 1000
    Private Shared _maxWidth As Integer = 2000
    Private Shared _operation As enumOperation
    Private Shared _verboseErrors As Boolean = True
    Private Shared _completeTheURLs As Boolean = True
    Private Shared _KeepLinebreaks As Boolean = True
    Private Shared _KeepImages As Boolean = True
    Private Shared _KeepHyperlinks As Boolean = True
    Private Shared _KeepStyles As Boolean = True
    Private Shared _localfile As Boolean = False
    Private Shared _KeepTables As Boolean = True
    Private Shared _startlinksearch As String = ""
    Private Shared _endlinksearch As String = ""
    Private Shared _urlprefix As String = ""
    Public Shared UnbalancedPrefix As String = "Unbalanced_"
    Public Shared DangerPrefix As String = "Danger_"
    Public Shared Property pStrictness As enumhowstrict
        Get
            Return _strictness
        End Get
        Set(value As enumhowstrict)
            _strictness = value
        End Set
    End Property
    Public Shared Property pStartLinkSearch As String
        Get
            Return _startlinksearch
        End Get
        Set(value As String)
            _startlinksearch = value
        End Set
    End Property
    Public Shared Property pEndLinkSearch As String
        Get
            Return _endlinksearch
        End Get
        Set(value As String)
            _endlinksearch = value
        End Set
    End Property
    Public Shared Property pKeepHyperlinks As Boolean
        Get
            Return _KeepHyperlinks
        End Get
        Set(value As Boolean)
            _KeepHyperlinks = value
        End Set
    End Property
    Public Shared Property pKeepTables As Boolean
        Get
            Return _KeepTables
        End Get
        Set(value As Boolean)
            _KeepTables = value
        End Set
    End Property
    Public Shared Property pKeepImages As Boolean
        Get
            Return _KeepImages
        End Get
        Set(value As Boolean)
            _KeepImages = value
        End Set
    End Property
    Public Shared Property pVerboseErrors As Boolean
        Get
            Return _verboseErrors
        End Get
        Set(value As Boolean)
            _verboseErrors = value
        End Set
    End Property
    Public Shared Property pKeepLinebreaks As Boolean
        Get
            Return _KeepLinebreaks
        End Get
        Set(value As Boolean)
            _KeepLinebreaks = value
        End Set
    End Property
    Public Shared Property pKeepStyles As Boolean
        Get
            Return _KeepStyles
        End Get
        Set(value As Boolean)
            _KeepStyles = value
        End Set
    End Property
    Public Enum enumfilterwhat
        keep = 1
        DangerousTagsOnly = 2
        MostTags = 3
    End Enum
    Public Enum enumOperation
        paste = 1
        scan = 2
    End Enum
    Public Shared Property pCompleteTheURLs As Boolean
        Get
            Return _completeTheURLs
        End Get
        Set(value As Boolean)
            _completeTheURLs = value
        End Set
    End Property

    Public Shared Property pMaxHeight As Integer
        Get
            Return _maxHeight
        End Get
        Set(value As Integer)
            _maxHeight = value
        End Set
    End Property
    Public Shared Property pMaxWidth As Integer
        Get
            Return _maxWidth
        End Get
        Set(value As Integer)
            _maxWidth = value
        End Set
    End Property
    Public Shared Property pCurrentlyWorkingOnURL As String
        Get
            Return (_currentlyWorkingOnURL)
        End Get
        Set(value As String)
            _currentlyWorkingOnURL = value
        End Set
    End Property
    Public Shared Property pFilterOutTags As enumfilterwhat
        Get
            Return _stripOutTags
        End Get
        Set(value As enumfilterwhat)
            _stripOutTags = value
        End Set
    End Property
    Public Shared Property pOperation As enumOperation
        Get
            Return _operation
        End Get
        Set(value As enumOperation)
            _operation = value
        End Set
    End Property
    Public Shared Property pAlsoNoExt As Boolean
        Get
            Return _alsoNoExt
        End Get
        Set(value As Boolean)
            _alsoNoExt = value
        End Set
    End Property
    Public Shared Property pFoundTheseLinks As String
        Get
            Return _foundtheselinks
        End Get
        Set(value As String)
            _foundtheselinks = value
        End Set
    End Property
    Public Shared Property pURL As String
        Get
            Return _url
        End Get
        Set(value As String)
            _url = value
            _urlprefix = justDigitsChars(_url)
        End Set
    End Property
    Public Shared Function uniquesession(ByVal infilename As String) As String
        Return _urlprefix & infilename
    End Function
    Private Shared Function justDigitsChars(ByVal inString As String) As String
        Dim i As Integer
        Dim sb As New StringBuilder("")
        For i = 0 To inString.Length - 1
            If Char.IsLetterOrDigit(inString(i)) Then
                sb.Append(inString(i))
            End If
        Next
        Return sb.ToString.ToLower
    End Function
    Public Shared Property pExtensions As List(Of String)
        Get
            Return _extensions
        End Get
        Set(value As List(Of String))
            _extensions = value
        End Set
    End Property
End Class
