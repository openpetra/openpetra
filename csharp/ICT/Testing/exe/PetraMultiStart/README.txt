PetraMultiStart
===============
PetraMultiStart is an application that starts many Clients on a single machine, running against the same Server, 
and makes these Clients perform some actions, and then have them close after a specified time.

This program is very useful in testing the stability of the PetraServerConsole application and for finding 
concurrency issues that are otherwise very hard to test when the product is not used in a production environment!

-------   -------   
WARNING   WARNING
-------   -------
    As the program potentially launches many Clients, and may do so in a very short time frame, the computer where the
    program is run can be RENDERED UNRESPONSIVE at times, esp. when the PetraServerConsole (and potentially PostgreSQL, 
    too) are run on the same computer! Advice: save any work before you launch PetraMultiStart!!!
-------   -------   
WARNING   WARNING
-------   -------



Parameterisation
----------------
The test cases, the number of Clients the program starts in each test case, and the actions the program asks the 
Clients to do, are set up in an XML file.
* The XML file is specified with the '-testscript' command line argument. Usually this would be the file 'multistart.testing' 
  found in this directory.
  Have a look at the comments at the top of the file for an explanation of the parameterisation through that file.
  
  Note: Some tests have paramters that are passed on to the PetraClient, e.g. PartnerKey, LedgerNumber. Since they need to be
        hard-coded in this file they relate to a specific DB. The ones that are found in this file are at the moment working
        for the ZA DB and will need to be adapted to any other DB!
  
* The test case is specified with the '-testcase' command line argument. Example: 'FullClient_only2'.


Usage
-----
1) Start PetraServerConsole.exe

2) Start PetraMultiStart on the command line from the \delivery\bin folder.
      Example command line:
      PetraMultiStart.exe -C:C:\openpetraorg\trunk\csharp\ICT\Testing\exe\PetraMultiStart\app-sample.config -testscript:C:\openpetraorg\trunk\csharp\ICT\Testing\exe\PetraMultiStart\multistart.testing -testcase:FullClient_only2 -startclientid:0

   Note: As PetraMultiStart starts Clients it asks them to log in as different users. Since the program cannot assume specific users as PetraServerConsole could be connected to any DB,
         the program creates 40 test users with 'demo' user privileges in the DB that PetraServerConsole is connected to when the program is started. 
         In case these users exist already, nothing 'bad' happens and the program continues.