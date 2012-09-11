using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	[Flags]
	public enum RestoreFlags
	{
		None = 0,
		WindowState = 1,
		Size = 2,
		Location = 4,
		Minimized = 8
	}

	[Flags]
	public enum SaveFlags
	{
		None = 0,
		/// <summary>
		/// Indicates to save all the childs form of a MDI window with the state of the active child window if this window is maximized. This flag allow to reopen all the child form maximized if you close the form MDI parent with a maximized mdi child.
		/// </summary>
		ActiveMDIMaximized = 1
	}

	/// <summary>
	/// A class that can be used to save into the isolated storage the position and the state of a Windows Forms control.
	/// </summary>
	public class PositionRecorderIsolatedStorage : DevAge.IO.IsolatedStorage.IsolatedStorageSettingVersionBase
	{
		public PositionRecorderIsolatedStorage() : base(1)
		{
		}

		private Point m_Location;
		private Size m_Size;
		private FormWindowState m_WindowState;

		private RestoreFlags m_RestoreFlags = RestoreFlags.Location | RestoreFlags.Size | RestoreFlags.WindowState;

		public RestoreFlags RestoreFlags
		{
			get{return m_RestoreFlags;}
			set{m_RestoreFlags = value;}
		}

		private SaveFlags m_SaveFlags = SaveFlags.ActiveMDIMaximized;

		public SaveFlags SaveFlags
		{
			get{return m_SaveFlags;}
			set{m_SaveFlags = value;}
		}

		public Point Location
		{
			get{return m_Location;}
			set{m_Location = value;}
		}
		public Size Size
		{
			get{return m_Size;}
			set{m_Size = value;}
		}
		public FormWindowState WindowState
		{
			get{return m_WindowState;}
			set{m_WindowState = value;}
		}

		protected override void OnLoad(System.IO.IsolatedStorage.IsolatedStorageFileStream p_File, int p_CurrentVersion)
		{
			if (p_CurrentVersion==Version)
			{
				int x,y,w,h,state;

                x = IO.StreamPersistence.ReadInt32(p_File);
                y = IO.StreamPersistence.ReadInt32(p_File);
                w = IO.StreamPersistence.ReadInt32(p_File);
                h = IO.StreamPersistence.ReadInt32(p_File);
                state = IO.StreamPersistence.ReadInt32(p_File);

				m_Location.X = x;
				m_Location.Y = y;
				m_Size.Width = w;
				m_Size.Height = h;
				m_WindowState = (FormWindowState)state;
			}
			else
				throw new DevAge.IO.InvalidDataException();
		}
	
		protected override void OnSave(System.IO.IsolatedStorage.IsolatedStorageFileStream p_File)
		{
			base.OnSave (p_File);

            IO.StreamPersistence.Write(p_File, m_Location.X);
            IO.StreamPersistence.Write(p_File, m_Location.Y);
            IO.StreamPersistence.Write(p_File, m_Size.Width);
            IO.StreamPersistence.Write(p_File, m_Size.Height);
            IO.StreamPersistence.Write(p_File, (int)m_WindowState);
		}
	
		protected override void OnCreate()
		{
			m_Location = new Point(int.MinValue, int.MinValue);
			m_Size = new Size(int.MinValue, int.MinValue);
			m_WindowState = (FormWindowState)(-1);
		}

		public virtual void Save(Control p_Control)
		{
			if (p_Control is System.Windows.Forms.Form)
			{
				System.Windows.Forms.Form l_Form = ((System.Windows.Forms.Form)p_Control);
				
				// .NET Bug
				//se richiesto controllo se il form è MDIChild. In questo caso vado a vedere quale è il form attivo e como stato uso quello del form attivo.
				// questo perchè se ad esempio ho 2 form entrambi maximized, .net metto normal il form non attivo e quindi durante il close salvo lo stato come normal
				// anche se ad esempio cambio finestra con ctrl+tab il form si rimetta maximized. Secondo me è un baco.
				// Da notare che questo codice funziona solo se chiamato prima di chiudere i form di un MDI. Nel caso del FormPosition richiamo quindi questo codice nel Closing
				if ( (m_SaveFlags & SaveFlags.ActiveMDIMaximized) == SaveFlags.ActiveMDIMaximized &&
					l_Form.IsMdiChild )
				{
					if (l_Form.MdiParent.ActiveMdiChild != null && 
						l_Form.MdiParent.ActiveMdiChild.WindowState == FormWindowState.Maximized)
						WindowState = FormWindowState.Maximized;
					else
						WindowState = l_Form.WindowState;
				}
				else
					WindowState = l_Form.WindowState;
			}

			if (WindowState == FormWindowState.Minimized)
			{
			}
			else
			{
				Location = p_Control.Location;
				Size = p_Control.Size;
			}

			Save();
		}

		public virtual void Load(Control p_Control)
		{
			Load();

			if ( (RestoreFlags & RestoreFlags.WindowState) == RestoreFlags.WindowState)
			{
				if (
					p_Control is System.Windows.Forms.Form &&
					WindowState != (FormWindowState)(-1) &&
						(
							WindowState!=FormWindowState.Minimized || 
							(RestoreFlags & RestoreFlags.Minimized) == RestoreFlags.Minimized
						)
					)
					((System.Windows.Forms.Form)p_Control).WindowState = WindowState;
			}

			if ( p_Control is System.Windows.Forms.Form && 
				((System.Windows.Forms.Form)p_Control).WindowState == FormWindowState.Minimized )
			{
				//se la form è minimizzata non carico le impostazioni altrimenti risulterebbero sbagliate
			}
			else
			{
				if ( (RestoreFlags & RestoreFlags.Location) == RestoreFlags.Location &&
					Location.X != int.MinValue)
					p_Control.Location = Location;

				if ( (RestoreFlags & RestoreFlags.Size) == RestoreFlags.Size &&
					Size.Width != int.MinValue)
					p_Control.Size = Size;
			}
		}
	}
}
