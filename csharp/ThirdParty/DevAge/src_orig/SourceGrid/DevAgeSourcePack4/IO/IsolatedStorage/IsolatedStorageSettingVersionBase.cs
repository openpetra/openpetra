using System;

namespace DevAge.IO.IsolatedStorage
{
	/// <summary>
	/// Summary description for IsolatedStorageSettingVersionBase.
	/// </summary>
	public abstract class IsolatedStorageSettingVersionBase : IsolatedStorageSettingBase
	{
		private int m_Version;
		private const string c_Check = "BINSETTING";
		public IsolatedStorageSettingVersionBase(int p_Version)
		{
			m_Version = p_Version;
		}
	
		public virtual int Version
		{
			get{return m_Version;}
		}
	
		protected override void OnLoad(System.IO.IsolatedStorage.IsolatedStorageFileStream p_File)
		{
            string l_Check = StreamPersistence.ReadString(p_File, System.Text.Encoding.UTF8);
			if (l_Check!=c_Check)
				throw new DevAge.IO.InvalidDataException();

            int l_CurrentVersion = StreamPersistence.ReadInt32(p_File);
			OnLoad(p_File,l_CurrentVersion);
		}
	
		protected abstract void OnLoad(System.IO.IsolatedStorage.IsolatedStorageFileStream p_File, int p_CurrentVersion);

		protected override void OnSave(System.IO.IsolatedStorage.IsolatedStorageFileStream p_File)
		{
            StreamPersistence.Write(p_File, c_Check, System.Text.Encoding.UTF8);
            StreamPersistence.Write(p_File, Version);
			//custom values
		}
	}
}
