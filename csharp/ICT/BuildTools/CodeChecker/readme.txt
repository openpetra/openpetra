 ----------------------------------------------------
*** Ict.Tools.CodeChecker Utility (ICT\BuildTools) ***
 ----------------------------------------------------

The Ict.Tools.CodeChecker Utility was written by ChristianK to be able to run arbitrary Regular Expression searches across 
the whole of the C# Code Base that is ...
  (a) making up the OpenPetra application, and
  (b) that is related to it (e.g. data conversion utilities, Unit Tests, etc).

Ict.Tools.CodeChecker has no command line arguments and runs a fixed set of hard-coded Regular Expressions.
The Regular Expressions that are presently coded up are for finding DB Access commands that supply 'null' 
instead of an instance of TDBTransaction (which is highly problematic!). However, any number of Regular Expressions 
could be added for finding different code problems that are identified over time and that should be highlighted with
this utility!
The set of Regular Expressions is coded up in Method 'DeclareRegExpressions'. Some Regular Expressions can lead to
'false positives'; those can be suppressed in Method 'DeclareFalsePositives'.

Ict.Tools.CodeChecker uses TLogging.Log for any reporting of problems so the output is seen on the Console as well
as in its logfile, CodeChecker.log (in the \log\ folder). That log file gets at present always appended to!

Ict.Tools.CodeChecker.exe returns a result value to the Console (-1 if Exceptions were encountered, 0 if no RegEx matches were 
found, otherwise the number of RegEx matches). That allows for meaningful integration into the Build Server Jobs for monitoring 
code issues!  The example batch file 'RunCodeChecker.bat' illustrates the evaluation of the result values 
(found in %ERRORLEVEL%).


_______________________
ChristianK, August 2014