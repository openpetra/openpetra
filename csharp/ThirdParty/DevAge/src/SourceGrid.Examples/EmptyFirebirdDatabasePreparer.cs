using System;
using System.IO;
using SourceGrid;

namespace WindowsFormsSample
{
	/// <summary>
	/// Prepares FireBird databse file.
	/// Copies empty DB to correct location, etc.
	/// </summary>
	public class EmptyFirebirdDatabasePreparer
	{
		string copyFile = "EMPTY.FDB";
		
		public bool ExistsFile()
		{
			var dest = GetDestFileName();
			return File.Exists(dest);
		}
		
		private string GetDestFileName()
		{
			var current = Directory.GetCurrentDirectory();
			var destFile = "frmSample60.fdb";
			return string.Format("{0}/{1}", current, destFile);
		}
		
		public void Copy()
		{
			var dest = GetDestFileName();
			var source = GetSourceFileName();
			File.Copy(source, dest);
			if (File.Exists(dest) == false)
				throw new SourceGridException(string.Format("Could not copy {0} to {1}", copyFile, source));
		}

		private string GetSourceFileName()
		{
			var current = Directory.GetCurrentDirectory();
			var source = Path.Combine(current, string.Format("../../Data/{0}", copyFile));
			if (File.Exists(source) == false)
				throw new FileNotFoundException(string.Format("File {0} could not be found in location: '{1}'", copyFile, source));
			return source;
		}
	}
}
