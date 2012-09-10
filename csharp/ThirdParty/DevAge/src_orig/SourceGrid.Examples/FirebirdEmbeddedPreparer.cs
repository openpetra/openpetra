using System;
using System.IO;
using Castle.Windsor;
using SourceGrid;

namespace WindowsFormsSample
{
	public class FirebirdEmbeddedPreparer 
	{
		public void EnsureFirebirdReady()
		{
			var current = Directory.GetCurrentDirectory();
			var fbDll = "fbembed.dll";
			var fbEmbed = string.Format("{0}/{1}", current, fbDll);
			if (File.Exists(fbEmbed) == true)
				return;
			
			var source = Path.Combine(current, string.Format(@"../../../libs/{0}", fbDll));
			if (File.Exists(source) == false)
				throw new FileNotFoundException(string.Format("File {0} could not be found in location: '{1}'", fbDll, source));
			File.Copy(source, fbEmbed);
			if (File.Exists(fbEmbed) == false)
				throw new SourceGridException(string.Format("Could not copy {0} to {1}", fbDll, source));
		}
	}
	

}
