The Newtonsoft.Json.Net20.dll has been compiled with the latest compiler built from Mono 2.8 trunk.
This was necessary so that no Microsoft dlls were linked in.

The ext.net dll was built against the Mono compiled Newtonsoft.Json.Net20.dll on Windows with SharpDevelop. 
The mono compiler crashed on Linux when trying to build the ext.net solution.

