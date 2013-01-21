http://code.google.com/p/jayrock/
this dll provides JSON functionality to .Net
License: LGPL

We are using version Jayrock 0.9.12915 from Feb 2011

Timotheus Pokorra, January 2011:
I have made a change to jayrock-0.9.12915\src\Jayrock.Json\Json\Conversion\Converters\DateTimeImporter.cs which I have added in this directory.
This is about parsing a date in the current culture.
It seems that Ext.js does not provide a proper date encoding:
http://www.prodromus.com/2011/04/15/extjs-jsonwriter-not-respecting-dateformat-used-with-jsonreader