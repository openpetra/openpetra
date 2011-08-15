Currently used version: ext.net community 1.1

The Newtonsoft.Json.Net20.dll has been compiled with the latest compiler built from Mono 2.8 trunk.
This was necessary so that no Microsoft dlls were linked in.

The ext.net dll was built against the Mono compiled Newtonsoft.Json.Net20.dll on Windows with SharpDevelop.
The mono compiler crashed on Linux when trying to build the ext.net solution.

Copy Newtonsoft.Json.Net20.dll to Ext.Net\Build\ReferenceAssemblies\Json.NET.
In SharpDevelop, open the ext.net solution file, and from project Ext.Net, remove the reference to Newtonsoft.Json.dll, and instead add a reference to Newtonsoft.Json.Net20.dll.
Disable signing the Ext.net project, otherwise the compiler does complain about the unsigned Json dll.