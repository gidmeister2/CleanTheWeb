Public Class extend
    ' the following arrays need to be extended as the html language gets new tags:
    ' "safe" generally means that they can't be used for scripting.
    ' there is also an issue with width/height type attributes - if the user wants to fit all the html in a narrow table, for instance.

    Public Shared SafeAttributeArray() As String = {"abbr", "align", "background", "bgcolor", "border", "cellpadding", _
                       "cellspacing", "char", "charoff", "charset", "checked", "cite", "class", _
                       "classid", "clear", "color", "cols", "colspan", "compact", "content", _
                       "coords", "datetime", "declare", "defer", "disabled", _
                       "enctype", "face", "for", "headers", "height", "href", "hspace", _
                       "label", "longdesc", "marginheight", "marginwidth", "maxlength", _
                        "multiple", "noresize", "noshade", "nowrap", _
                       "readonly", "rel", "rev", "rows", "rowspan", "selected", "shape", "size", "span", _
                       "src", "srcset", "standby", "start", "style", "summary", "tabindex", "text", _
                       "title", "type", "usemap", "valign", "value", "valuetype", "version", _
                       "vlink", "vspace", "width"}

    Public Shared dangerousStylesArray() As String = {"position", "clip"}
    Public Shared heightarray() As String = {"min-height", "height"} ' "max-height",
    Public Shared widtharray() As String = {"min-width", "width"} ' "max-width",


    Public Shared arrayOfMaybeSelfEnding() As String = {"area", "base", "br", "col", "embed", "hr", "img", "input", "keygen", _
                                 "link", "menuitem", "meta", "p", "param", "source", "track", "wbr"} ' p tag can't be <p />, but can be by itself in sloppy html
    ' the above array is of tags that don't need to be in pairs, though some (such as "p") can be.

    Public Shared tableTagsArray() As String = {"table", "tr", "td", "th", "thead", "tfoot", "tbody", "caption"}

    ' the removeInsidesArray is of tags that you not only remove, but you remove everything between the start tag and the end tag.  
    Public Shared removeInsidesArray() As String = {"script", "title"}

    ' the safeTagsArray are (some) tags that are not used for scripting, though in some cases they can have attributes such as 'onclick' that would be used for scripting.
    ' (there is code that will remove those attributes.)
    Public Shared safeTagsArray() As String = {"--", "a", "abbr", "acronym", "article", "aside", "b", "base", "basefont", "bdi", "bdo", _
                                   "big", "blockquote", "br", "canvas", "center", "cite", "col", "colgroup", _
                                   "dd", "del", "div", "details", "dl", "dt", "em", "figcaption", "figure", _
                                   "font", "h1", "hr", "i", "img", "label", "legend", "li", "map", "meter", _
                                   "ol", _
                                   "optgroup", "option", "output", "p", "pre", "q", "rp", "rt", "s", "samp", _
                                   "small", "source", "span", "strike", "strong", "style", "sub", _
                                   "summary", "sup", "table", "tbody", "td", "textarea", "tfoot", "th", "thead", _
                                   "title", "tr", "tt", "u", "ul", "var", "wbr", _
        "altGlyph", _
    "altGlyphDef", _
    "altGlyphItem", _
    "animate", _
    "animateColor", _
    "animateMotion", _
    "animateTransform", _
    "circle", _
    "clipPath", _
    "color-profile", _
    "cursor", _
    "defs", _
    "desc", _
    "ellipse", _
    "feBlend", _
    "feColorMatrix", _
    "feComponentTransfer", _
    "feComposite", _
    "feConvolveMatrix", _
    "feDiffuseLighting", _
    "feDisplacementMap", _
    "feDistantLight", _
    "feFlood", _
    "feFuncA", _
    "feFuncB", _
    "feFuncG", _
    "feFuncR", _
    "feGaussianBlur", _
    "feImage", _
    "feMerge", _
    "feMergeNode", _
    "feMorphology", _
    "feOffset", _
    "fePointLight", _
    "feSpecularLighting", _
    "feSpotLight", _
    "feTile", _
    "feTurbulence", _
    "filter", _
    "font", _
    "font-face", _
    "font-face-format", _
    "font-face-name", _
    "font-face-src", _
    "font-face-uri", _
    "foreignObject", _
    "g", _
    "glyph", _
    "glyphRef", _
    "hkern", _
    "image", _
    "line", _
    "linearGradient", _
    "marker", _
    "mask", _
    "metadata", _
    "missing-glyph", _
    "mpath", _
    "path", _
    "pattern", _
    "polygon", _
    "polyline", _
    "radialGradient", _
    "rect", _
    "script", _
    "set", _
    "stop", _
    "style", _
    "svg", _
    "switch", _
    "symbol", _
    "text", _
    "textPath", _
    "title", _
    "tref", _
    "tspan", "use", "view", "vkern"}


    Public Shared linebreakTagsSet() As String = {"br", "p"}

    Public Shared SelfEndingArray() As String = {"link", "meta", "hr", "input", "embed", "param", "--", "br", "use", "circle", _
 "ellipse", _
 "feGaussianBlur", _
 "line", _
 "path", _
 "polygon", _
 "polyline", _
 "rect", _
 "use"}
End Class

