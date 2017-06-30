Npgsql is a .Net data provider for Postgresql. 
http://www.npgsql.org/doc/index.html

current version: Npgsql 3.2.4.1

I have downloaded the stable version from nuget: https://www.nuget.org/packages/Npgsql/
and extracted the files Npgsql.dll and Npgsql.xml from the nupkg file, from path lib/net451

Mono.Security.dll is a managed dll, from Mono, which is required by Npgsql on Windows

System.Threading.Tasks.Extensions.dll is required by the nuget package, and is itself extracted from a nupkg file:
https://www.nuget.org/packages/System.Threading.Tasks.Extensions/
https://www.nuget.org/api/v2/package/System.Threading.Tasks.Extensions/4.3.0
