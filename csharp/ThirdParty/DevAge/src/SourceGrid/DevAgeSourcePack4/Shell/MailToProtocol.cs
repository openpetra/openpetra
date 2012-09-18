using System;

namespace DevAge.Shell
{
	public class MailToProtocol
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="p_To">null if not used</param>
		/// <param name="p_Cc">null if not used</param>
		/// <param name="p_Bcc">null if not used</param>
		/// <param name="p_Subject">null if not used</param>
		/// <param name="p_Body">null if not used</param>
		/// <returns></returns>
		public static string FormatMailToCommand(string[] p_To, string[] p_Cc, string[] p_Bcc, string p_Subject, string p_Body)
		{
			string l_To = FormatEMailAddress(p_To);
			string l_CC = FormatEMailAddress(p_Cc);
			string l_Bcc = FormatEMailAddress(p_Bcc);
				
			string l_Command = "mailto:";
			if (l_To!=null)
				l_Command+=l_To;

			System.Collections.ArrayList l_Parameters = new System.Collections.ArrayList();

			if (l_CC!=null)
				l_Parameters.Add("CC="+l_CC);
			if (l_Bcc!=null)
				l_Parameters.Add("BCC="+l_Bcc);
			if (p_Subject!=null)
				l_Parameters.Add("subject="+p_Subject);
			if (p_Body!=null)
				l_Parameters.Add("body="+p_Body);

			if (l_Parameters.Count>0)
			{
				string[] l_tmp = new string[l_Parameters.Count];
				l_Parameters.CopyTo(l_tmp,0);
				l_Command+="?";
				l_Command+=string.Join("&",l_tmp);
			}

			return l_Command;
		}

		public static void Exec(string[] p_To)
		{
			Exec(p_To,null);
		}
		public static void Exec(string[] p_To, string p_Subject)
		{
			Exec(p_To,null,null,p_Subject,null);
		}
		public static void Exec(string[] p_To, string[] p_Cc, string[] p_Bcc, string p_Subject, string p_Body)
		{
			//mailto:mtscf@microsoft.com?CC=davide@davide.it&BCC=pippo@pippo.com&subject=Feedback&body=The InetSDK Site Is Superlative
	
			try
			{
				Utilities.ExecCommand(FormatMailToCommand(p_To,p_Cc,p_Bcc,p_Subject,p_Body));
			}
			catch(Exception err)
			{
				throw new ApplicationException("Failed to execute mailto protocol.", err);
			}
		}

		public static string FormatEMailAddress(string[] p_EMails)
		{
			if (p_EMails==null || p_EMails.Length <= 0)
				return null;
			else
				return string.Join(";",p_EMails);
		}
	}
}
