EPPlus is a .net library that reads and writes Excel 2007/2010 files using the Open Office Xml format (xlsx).

http://epplus.codeplex.com/

licensed under LGPL

current version:
modified version based on Mercurial from 23 June 2012 
http://epplus.codeplex.com/SourceControl/changeset/view/67b2e199f54f



Problems with Mono:

see http://epplus.codeplex.com/workitem/13096        
and https://bugzilla.xamarin.com/show_bug.cgi?id=2527


Solution:

in EPPlus\ExcelPackage.cs, replace all 
_package.Close();
with:
_package.Flush();

in all files of the solution, replace 
PackUriHelper.GetRelativeUri(
with:
PackUriHelperMonoSafe.GetRelativeUri(

and add to EPPlus\XmlHelper.cs:

    public class PackUriHelperMonoSafe
    {
        public static Uri GetRelativeUri(Uri sourcePartUri, Uri targetPartUri)
        {
            Uri result = PackUriHelper.GetRelativeUri(sourcePartUri, targetPartUri);
            if (result.ToString() == sourcePartUri.ToString())
            {
                // bug in Mono, see https://bugzilla.xamarin.com/show_bug.cgi?id=2527
                result = PackUriHelper.GetRelativeUri(targetPartUri, sourcePartUri);
            }
            
            return result;
        }
    }

Make sure not to replace PackUriHelper in XmlHelper as well, that will result in a stack overflow, infinite recursive call...