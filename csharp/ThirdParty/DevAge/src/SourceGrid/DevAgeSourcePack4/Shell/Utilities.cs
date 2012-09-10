using System;

namespace DevAge.Shell
{
	/// <summary>
	/// Shell utilities
	/// </summary>
	public class Utilities
	{
		public static void OpenFile(string p_File)
		{
			ExecCommand(p_File);
		}

		public static void ExecCommand(string p_Command)
		{
			System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo(p_Command);
			p.UseShellExecute = true;
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.StartInfo = p;
			process.Start();
		}
	}
}
