sqlite3.dll has been downloaded from sqlite.org


Version 3.8.11.1: https://www.sqlite.org/2015/sqlite-dll-win32-x86-3081101.zip



Mono.Data.Sqlite is from Mono 4.0.4 Linux


Version for .net 4.0 would not work on Windows. 
see also http://bugzilla.xamarin.com/show_bug.cgi?id=2148

Unhandled Exception: System.TypeLoadException: Inheritance security rules
violated by type: 'Mono.Data.Sqlite.SqliteConnectionHandle'. Derived types must
either match the security accessibility of the base type or be less accessible.



other problem: cannot open on Windows?

http://bugzilla.xamarin.com/show_bug.cgi?id=152


see also http://system.data.sqlite.org/index.html/doc/trunk/www/downloads.wiki

The problem is that those dlls do not seem to work on Linux, I can't get them to work (November 2011).