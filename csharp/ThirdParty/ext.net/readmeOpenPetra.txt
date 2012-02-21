Currently used version: ext.net community 1.2

see also http://examples.ext.net/#/Getting_Started/Introduction/README/ about what is required in the web.config file!

The Newtonsoft.Json.Net40.dll is from http://json.codeplex.com/, Json.NET 4.0 Release 5, Dec 10 2011

Copy Newtonsoft.Json.Net40.dll to Ext.Net\Build\ReferenceAssemblies\Json.NET.
In SharpDevelop, open the ext.net solution file, and from project Ext.Net, remove the reference to Newtonsoft.Json.dll, and instead add a reference to Newtonsoft.Json.Net40.dll.
Disable signing the Ext.net and utilities project, otherwise the compiler does complain about the unsigned Json dll.
Upgrade the ext.net and the utilities project to full 4.0 target framework.
Remove all references, and add them again specifically with version 4.0.