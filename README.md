# CleanTheWeb
Download any set of  pages/pdf-files etc from a website and remove any risky tags, scripts, etc.  One application I use this program for: the result can be sent to an e-reader such as a  Kindle
For example, you might see a webpage with a set of links to PDF articles.  You type in the URL of the webpage into this program, and it will download the desired files (you see a screen where you can remove some of them, if you wish).  Or you can download the articles that interest you off a political website, concatenate them into one large file, and send that to an e-reader such as a Kindle.
The program also filters away tags that you specify - so you can say "just retain line breaks and paragraphs" for example.
There are various improvements I can think of to this program.  For instance, you can download images, but currently you cannot preserve the directory structure of the website, which might have images in several folders and subfolders.  Also, you see a list of links in a webpage - from which you can choose some or all to download - but it would be nice if also had some text with each link saying what the link refers to: perhaps http://politico.com/justice : too many veterans are homeless.  Finally, the program could be faster, if possible - there are many string operations which slow it down, and not all can be sped up with stringbuilder objects (concatenation of two strings for example).
