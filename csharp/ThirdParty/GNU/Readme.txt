This is from the GNU gettext package.

The homepage of this package is at

http://www.gnu.org/software/gettext/

see also http://www.gnu.org/software/gettext/manual/gettext.html#C_0023 for the description how to use GNU gettext with C#.

To compile the file GNU.Gettext.dll, I have created the AssemblyInfo.cs file in this directory,
and a modified copy of gettext-0.18.1.3\gettext-runtime\intl-csharp\intl.cs, so just run in the mono shell:

gmcs -out:GNU.Gettext.dll -target:library AssemblyInfo.cs intl.cs