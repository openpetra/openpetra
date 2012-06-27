EPPlus is a .net library that reads and writes Excel 2007/2010 files using the Open Office Xml format (xlsx).

http://epplus.codeplex.com/

licensed under LGPL

current version:
modified version based on Mercurial from 23 June 2012 
http://epplus.codeplex.com/SourceControl/changeset/view/67b2e199f54f



Problems with Mono:

see http://epplus.codeplex.com/workitem/13096        
and https://bugzilla.xamarin.com/show_bug.cgi?id=2527

https://github.com/mono/mono/blob/master/mcs/class/WindowsBase/System.IO.Packaging/PackUriHelper.cs

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



for debugging:

    public class PackUriHelperMonoSafe
    {
        public static Uri GetRelativeUri(Uri sourcePartUri, Uri targetPartUri)
        {
            Uri result = PackUriHelper.GetRelativeUri(sourcePartUri, targetPartUri);
            
            
            StreamWriter sw = new StreamWriter("/tmp/test.log", true);
            
            sw.WriteLine("source " + sourcePartUri.ToString());
            sw.WriteLine("target " + targetPartUri.ToString());
            sw.WriteLine("result " + result.ToString());
            
            if (result.ToString().StartsWith("/"))
            {
                // bug in Mono, see https://bugzilla.xamarin.com/show_bug.cgi?id=2527
                // Mono always returns the sourcePartUri.
                string source = sourcePartUri.ToString();
                string target = targetPartUri.ToString();
    
                int countSame = 0;
    
                while (countSame < target.Length
                       && countSame < source.Length
                       && target[countSame] == source[countSame])
                {
                    countSame++;
                }
    
                // go back to the last separator
                countSame = target.Substring(0, countSame).LastIndexOf("/") + 1;
                string ResultString = target.Substring(countSame);
    
                if (countSame > 0)
                {
                    // how many directories do we need to go up from the working Directory
                    while (countSame < source.Length)
                    {
                        if (source[countSame] == '/')
                        {
                            ResultString = "../" + ResultString;
                        }
    
                        countSame++;
                    }
                }
                
                result = new Uri(ResultString, UriKind.Relative);
                
            sw.WriteLine("result Mono " + result.ToString());
            }
            
            sw.WriteLine();
            sw.Close();
            return result;
        }
    }

2nd Problem:
Microsoft Office 2010 does not like just a number starting with 0, that Mono would produce.

For testing xlsx files without owning Microsoft Office, and for more detailed error messages:
http://www.microsoft.com/en-us/download/details.aspx?id=5124
Open XML SDK 2.0 for Microsoft Office
C:\Program Files (x86)\Open XML SDK\V2.0\tool

Search for CreateRelationship in all cs files in project EPPlus.
add parameter PackagePartForMono.NextRelationshipID to function call, at the end.

add to XmlHelper.cs:
    public class PackagePartForMono
    {
        private static Int32 FNextRelationshipID = 1;
        
        public static string NextRelationshipID
        {
            get
            {
                // Microsoft Office 2010 does not like just a number starting with 0, that Mono would produce
                // see https://github.com/mono/mono/blob/master/mcs/class/WindowsBase/System.IO.Packaging/PackagePart.cs
                // Open XML SDK 2.0 Productivity Tool for Microsoft Office error message: 
                // Cannot open the file: Die ID "0" ist keine gültige XSD-ID.
                string result = "rID" + FNextRelationshipID.ToString();
                FNextRelationshipID++;
                return result;
            }
        }
    }
