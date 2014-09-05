@ECHO OFF

Ict.Tools.CodeChecker.exe

IF "%ERRORLEVEL%" GEQ "1" (goto MATCHESFOUND)
IF "%ERRORLEVEL%" == "0"  (goto SUCCESS)
IF "%ERRORLEVEL%" LSS "0" (goto ERRORS)

:ERRORS
ECHO CodeChecker encountered %ERRORLEVEL% errors!
GOTO ENDOFBATCH

:SUCCESS
ECHO No Matches = Success!
GOTO ENDOFBATCH

:MATCHESFOUND
ECHO %ERRORLEVEL% Matches found!

:ENDOFBATCH