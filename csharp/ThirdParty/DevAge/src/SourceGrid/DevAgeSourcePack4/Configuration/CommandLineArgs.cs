/*
* Arguments class: application arguments interpreter
*
* Authors:		R. LOPES
* Contributors:	R. LOPES, BillyZKid, Hastarin, E. Marcon (VB version)
* Created:		25 October 2002
* Modified:		29 September 2003
* URL:				http://www.codeproject.com/csharp/command_line.asp
*
* Version:		1.1
* Original NameSpace.ClassName: Mozzarella.Utility.Arguments
*/

using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace DevAge.Configuration
{
	/// <summary>
	/// Description résumée de Arguments.
	/// </summary>
	public class CommandLineArgs : StringDictionary
	{
		// Constructors
		public CommandLineArgs(string Args)
		{
			if (Args == null || Args == "")
				Extract(new string[0]);
			else
			{
				Regex Extractor=new Regex(@"(['""][^""]+['""])\s*|([^\s]+)\s*",RegexOptions.IgnoreCase|RegexOptions.Compiled);
				MatchCollection Matches;
				string[] Parts;

				// Get matches (first string ignored because Environment.CommandLine starts with program filename)
				Matches=Extractor.Matches(Args);
				Parts=new string[Matches.Count-1];
				for(int i=1;i<Matches.Count;i++)
					Parts[i-1]=Matches[i].Value.Trim();

				Extract(Parts);
			}
		}

		public CommandLineArgs(string[] Args)
		{
			Extract(Args);
		}

		// Extract command line parameters and values stored in a string array
		private void Extract(string[] Args)
		{
			Clear();
			Regex Spliter = new Regex(@"^([/-]|--){1}(?<name>\w+)([:=])?(?<value>.+)?$",RegexOptions.IgnoreCase|RegexOptions.Compiled);
			char[] TrimChars = {'"','\''};
			string Parameter = null;
			Match Part;

			// Valid parameters forms:
			// {-,/,--}param{ ,=,:}((",')value(",'))
			// Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
			foreach (string Arg in Args)
			{
				Part = Spliter.Match(Arg);
				if (!Part.Success)
				{
					// Found a value (for the last parameter found (space separator))
					if (Parameter != null)
						this[Parameter] = Arg.Trim(TrimChars);
				}
				else
				{
					// Matched a name, optionally with inline value
					Parameter = Part.Groups["name"].Value;
					Add(Parameter,Part.Groups["value"].Value.Trim(TrimChars));
				}
			}
		}


		public override string ToString()
		{
			string ret = "";
			foreach (string k in Keys)
				ret += k + "='" + this[k] + "'\n";

			return ret;
		}
	}
}
