This is from the GNU gettext package.

The homepage of this package is at

http://www.gnu.org/software/gettext/

see also http://www.gnu.org/software/gettext/manual/gettext.html#C_0023 for the description how to use GNU gettext with C#.

To compile the file GNU.Gettext.dll, I have created the AssemblyInfo.cs file in this directory,
which you have to copy to gettext-0.18.1.1\gettext-runtime\intl-csharp, and then in that directory in the mono shell run:

gmcs -out:GNU.Gettext.dll -target:library AssemblyInfo.cs intl.cs