 ------------------------------------------
*** CodeChecker Utility (ICT\BuildTools) ***
 ------------------------------------------

The CodeChecker Utility was written by ChristianK to be able to run arbitrary Regular Expression searches across 
the whole of the C# Code Base that is 
  (a) making up the OpenPetra application, or 
  (b) that is related to it (e.g. data conversion utilities, Unit Tests, etc).

At present CodeChecker has no command line arguments and runs a fixed set of hard-coded Regular Expressions.
The Regular Expressions that are presently coded up are for finding DB Access commands that supply 'null' 
instead of an instance of TDBTransaction (which is highly problematic!). However, any number of Regular Expressions 
could be added for finding different code problems that are identified over time and that should be highlighted with
this utility!
The set of Regular Expressions is coded up in Method 'DeclareRegExpressions'. Some Regular Expressions can lead to
'false positives'; those can be suppressed in Method 'DeclareFalsePositives'.

CodeChecker uses TLogging.Log for any reporting of problems so the output is seen on the Console as well
as in its logfile, CodeChecker.log (in the \log\ folder). That log file gets at present always appended to!

There are plans for returning a result value to the Console (0 if no RegEx matches were found, otherwise the number
of RegEx matches) to allow meaningful integration into the Build Server Jobs for monitoring code issues.


The CodeChecker Utility is at present in its very early stages.

_______________________
ChristianK, August 2014