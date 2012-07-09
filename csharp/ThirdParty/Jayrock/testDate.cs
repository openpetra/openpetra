/*
 * Created by SharpDevelop.
 * User: tpokorra
 * Date: 20.02.2012
 * Time: 13:41
 *

simple program for testing JayRock
 
mcs test.cs -r:Jayrock.Json.dll
cp /opt/mono-2.10-git/lib/mono/4.5/System.Core.dll .
same for System.Numerics.dll
mono  --runtime=v4.0 test.exe


 */
using System;
using System.Globalization;
using System.Threading;
using Jayrock.Json;

namespace testDate
{
    /// <summary>
    /// collection of data that is entered on the web form
    /// </summary>
    public class TApplicationFormData
    {
        /// <summary>
        /// email address
        /// </summary>
        public string email;
        /// <summary>
        /// Date of Birth of the person
        /// </summary>
        public DateTime dateofbirth;
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cs-CZ");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");

            Console.WriteLine(new DateTime(2012, 12, 31).ToShortDateString());
Console.WriteLine(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
Console.WriteLine(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern);
Console.WriteLine(Convert.ToDateTime("31.12.2012").ToString());

string json = "{\"RegistrationCountryCode\":\"nb-NO\",\"FirstName\":\"asdf\",\"LastName\":\"asdf\",\"DateOfBirth\":\"27.11.1999\",\"Gender\":\"Male\",\"ParentName\":\"asdf\",\"Email\":\"timo@sample.org\"}";
            try
            {
                 TApplicationFormData test = (TApplicationFormData) Jayrock.Json.Conversion.JsonConvert.Import(typeof(TApplicationFormData),
                    json);
Console.WriteLine(test.dateofbirth.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem parsing JSON object: " + json);
                Console.WriteLine(e.ToString());
            }
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
    }
}
