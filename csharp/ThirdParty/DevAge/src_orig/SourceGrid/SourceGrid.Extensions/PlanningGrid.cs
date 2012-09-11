using SourceGrid.Cells.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Planning
{
	/// <summary>
	/// Summary description for PlanningGrid.
	/// </summary>
	public class PlanningGrid : System.Windows.Forms.UserControl
	{
		private Grid grid;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PlanningGrid()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			grid.Selection.FocusStyle = FocusStyle.None;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grid = new Grid();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.AutoStretchColumnsToFitWidth = false;
			this.grid.AutoStretchRowsToFitHeight = false;
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grid.Location = new System.Drawing.Point(0, 0);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(360, 340);
			this.grid.SpecialKeys = GridSpecialKeys.Default;
			this.grid.TabIndex = 0;
			// 
			// PlanningGrid
			// 
			this.Controls.Add(this.grid);
			this.Name = "PlanningGrid";
			this.Size = new System.Drawing.Size(360, 340);
			this.ResumeLayout(false);

		}
		#endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            grid.Controller.AddController(new AppointmentController(this));
        }

		private DateTime m_DateTimeStart;
		private DateTime m_DateTimeEnd;
		private int m_MinAppointmentLength;

		private AppointmentCollection m_Appointments = new AppointmentCollection();

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AppointmentCollection Appointments
		{
			get{return m_Appointments;}
		}
		
		public DateTime DateTimeStart
		{
			get {return m_DateTimeStart;}
		}
		public DateTime DateTimeEnd
		{
			get {return m_DateTimeEnd;}
		}
		public int MinAppointmentLength
		{
			get {return m_MinAppointmentLength;}
		}

        /// <summary>
        /// The grid used internally to display the planning (note that you usually don't need to access directly this class).
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Grid Grid
        {
            get { return grid; }
        }

		private const int c_RowsHeader = 2;
		private const int c_ColumnsHeader = 2;

        /// <summary>
        /// Load the grid using the parameters specified
        /// </summary>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <param name="minAppointmentLength"></param>
		public void LoadPlanning(DateTime dateTimeStart, DateTime dateTimeEnd, int minAppointmentLength)
		{
            m_DateTimeStart = dateTimeStart;
            m_DateTimeEnd = dateTimeEnd;
            m_MinAppointmentLength = minAppointmentLength;

            if (dateTimeStart >= dateTimeEnd)
				throw new ApplicationException("Invalid Planning Range");
            if (dateTimeStart.TimeOfDay >= dateTimeEnd.TimeOfDay)
				throw new ApplicationException("Invalid Plannnin Range");
            if (dateTimeStart.TimeOfDay.Minutes != 0 ||
                dateTimeEnd.TimeOfDay.Minutes != 0)
				throw new ApplicationException("Invalid Start or End hours must be with 0 minutes");
            if (minAppointmentLength <= 0 || minAppointmentLength > 60)
				throw new ApplicationException("Invalid Minimum Appointment Length");
            if (60 % minAppointmentLength != 0)
				throw new ApplicationException("Invalid Minimum Appointment Length must be multiple of 60");

            TimeSpan dayInterval = dateTimeEnd - dateTimeStart;
            TimeSpan timeInterval = dateTimeEnd.TimeOfDay - dateTimeStart.TimeOfDay;
            int partsForHour = 60 / minAppointmentLength;

			if (dayInterval.TotalDays > 30)
				throw new ApplicationException("Range too big");
            if (timeInterval.TotalMinutes < minAppointmentLength)
				throw new ApplicationException("Invalid Minimum Appointment Length for current Planning Range");

			//Redim Grid
			grid.Redim((int)( (timeInterval.TotalHours + 1) * partsForHour + c_RowsHeader), 
				(int)(dayInterval.TotalDays + 1 + c_ColumnsHeader));

			//Load Header
			grid[0, 0] = new Header00(null);
			grid[0, 0].RowSpan = 2;
			grid[0, 0].ColumnSpan = 2;
			//create day caption
            DateTime captionDate = dateTimeStart;
			for (int c = c_ColumnsHeader; c < grid.ColumnsCount; c++)
			{
                grid[0, c] = new HeaderDay1(captionDate.ToShortDateString());
                grid[1, c] = new HeaderDay2(captionDate.ToString("dddd"));

                captionDate = captionDate.AddDays(1);
			}

			//create hour caption
            int hours = dateTimeStart.Hour;
			for (int r = c_RowsHeader; r < grid.RowsCount; r += partsForHour)
			{
                grid[r, 0] = new HeaderHour1(hours);
				grid[r, 0].RowSpan = partsForHour;

				int minutes = 0;
				for (int rs = r; rs < (r + partsForHour); rs++)
				{
                    grid[rs, 1] = new HeaderHour2(minutes);
                    minutes += minAppointmentLength;
				}
                hours++;
			}

			grid.FixedColumns = c_ColumnsHeader;
			grid.FixedRows = c_RowsHeader;

            //Fix the width of the first 2 columns
            grid.Columns[0].Width = 40;
            grid.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.None;
            grid.Columns[1].Width = 40;
            grid.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.None;

			grid.AutoStretchColumnsToFitWidth = true;
			grid.AutoStretchRowsToFitHeight = true;
            grid.AutoSizeCells();


			//Create Appointment Cells
			//Days
			for (int c = c_ColumnsHeader; c < grid.ColumnsCount; c++)
			{
                DateTime currentTime = dateTimeStart.AddDays(c - c_ColumnsHeader);
				int indexAppointment = -1;
				Cells.Cell appointmentCell = null;
				//Hours
				for (int r = c_RowsHeader; r < grid.RowsCount; r += partsForHour)
				{
					//Minutes
					for (int rs = r; rs < (r + partsForHour); rs++)
					{
						
						bool l_bFound = false;
						//Appointments
						for (int i = 0; i < m_Appointments.Count; i++)
						{
                            if (m_Appointments[i].ContainsDateTime(currentTime))
							{
								l_bFound = true;

								if (indexAppointment != i)
								{
									appointmentCell = new CellAppointment(m_Appointments[i]);
									appointmentCell.View = m_Appointments[i].View;
									if (m_Appointments[i].Controller != null)
										appointmentCell.AddController(m_Appointments[i].Controller);
									grid[rs,c] = appointmentCell;
									indexAppointment = i;
								}
								else
								{
									grid[rs,c] = null;
									appointmentCell.RowSpan++;
								}

								break;
							}
						}
						if (l_bFound)
						{
						}
						else
						{
                            grid[rs, c] = new CellEmpty(currentTime, currentTime.AddMinutes(minAppointmentLength));
							indexAppointment = -1;
							appointmentCell = null;
						}

                        currentTime = currentTime.AddMinutes(minAppointmentLength);
					}
				}
			}

		}

		public void UnLoadPlanning()
		{
			grid.Redim(0,0);
		}

        public event AppointmentEventHandler AppointmentClick;
        protected virtual void OnAppointmentClick(AppointmentEventArgs e)
        {
            if (AppointmentClick != null)
                AppointmentClick(this, e);
        }
        public event AppointmentEventHandler AppointmentDoubleClick;
        protected virtual void OnAppointmentDoubleClick(AppointmentEventArgs e)
        {
            if (AppointmentDoubleClick != null)
                AppointmentDoubleClick(this, e);
        }

        public delegate void AppointmentEventHandler(object sender, AppointmentEventArgs e);

        class AppointmentController : ControllerBase
        {
            public AppointmentController(PlanningGrid planningGrid)
            {
                mPlanningGrid = planningGrid;
            }

            private PlanningGrid mPlanningGrid;

            public override void OnClick(CellContext sender, EventArgs e)
            {
                base.OnClick(sender, e);

                if (sender.Cell is CellAppointment)
                {
                    CellAppointment cell = (CellAppointment)sender.Cell;
                    mPlanningGrid.OnAppointmentClick(new AppointmentEventArgs(cell.Appointment.DateTimeStart, cell.Appointment.DateTimeEnd, cell.Appointment));
                }
                else if (sender.Cell is CellEmpty)
                {
                    CellEmpty cell = (CellEmpty)sender.Cell;
                    mPlanningGrid.OnAppointmentClick(new AppointmentEventArgs(cell.Start, cell.End, null));
                }
            }

            public override void OnDoubleClick(CellContext sender, EventArgs e)
            {
                base.OnDoubleClick(sender, e);

                if (sender.Cell is CellAppointment)
                {
                    CellAppointment cell = (CellAppointment)sender.Cell;
                    mPlanningGrid.OnAppointmentDoubleClick(new AppointmentEventArgs(cell.Appointment.DateTimeStart, cell.Appointment.DateTimeEnd, cell.Appointment));
                }
                else if (sender.Cell is CellEmpty)
                {
                    CellEmpty cell = (CellEmpty)sender.Cell;
                    mPlanningGrid.OnAppointmentDoubleClick(new AppointmentEventArgs(cell.Start, cell.End, null));
                }
            }
        }
	}

	public class CellAppointment : Cells.Cell
	{
		public CellAppointment(IAppointment appointment):base(appointment.Title)
		{
            m_Appointment = appointment;

		}

        private IAppointment m_Appointment;
        public IAppointment Appointment
        {
            get { return m_Appointment; }
        }
	}
	public class CellEmpty : Cells.Cell
	{
		public CellEmpty(DateTime start, DateTime end):base(null)
		{
            m_Start = start;
            m_End = end;
		}

        private DateTime m_Start;
        public DateTime Start
        {
            get { return m_Start; }
        }

        private DateTime m_End;
        public DateTime End
        {
            get { return m_End; }
        }

	}
	public class Header00 : Cells.Header
	{
		public Header00(object val):base(val)
		{
		}
	}
	public class HeaderDay1 : Cells.Header
	{
		public HeaderDay1(object val):base(val)
		{
		}
	}
	public class HeaderDay2 : Cells.Header
	{
		public HeaderDay2(object val):base(val)
		{
		}
	}
	public class HeaderHour1 : Cells.RowHeader
	{
		public HeaderHour1(object val):base(val)
		{
		}
	}
	public class HeaderHour2 : Cells.RowHeader
	{
		public HeaderHour2(object val):base(val)
		{
		}
	}

    public class AppointmentEventArgs : EventArgs
    {
        public AppointmentEventArgs(DateTime start, DateTime end, IAppointment appointment)
        {
            m_DateTimeEnd = end;
            m_DateTimeStart = start;
            m_Appointment = appointment;
        }

        private DateTime m_DateTimeStart;
        public DateTime DateTimeStart
        {
            get { return m_DateTimeStart; }
        }

        private DateTime m_DateTimeEnd;
        public DateTime DateTimeEnd
        {
            get { return m_DateTimeEnd; }
        }

        private IAppointment m_Appointment;
        public IAppointment Appointment
        {
            get { return m_Appointment; }
        }
    }

	public interface IAppointment
	{
		string Title
		{
			get;
		}

		Cells.Views.IView View
		{
			get;
		}

		DateTime DateTimeStart
		{
			get;
		}
        DateTime DateTimeEnd
        {
            get;
        }

		bool ContainsDateTime(DateTime p_DateTime);

		Cells.Controllers.IController Controller
		{
			get;
			set;
		}
	}

	public class AppointmentBase : IAppointment
	{
        public AppointmentBase(string title, DateTime dateTimeStart, DateTime dateTimeEnd)
		{
            m_Title = title;
            m_DateTimeEnd = dateTimeEnd;
            m_DateTimeStart = dateTimeStart;

			m_View = new Cells.Views.Cell();
            m_View.Border = DevAge.Drawing.RectangleBorder.RectangleBlack1Width;
		}

		public AppointmentBase():this("", DateTime.Now, DateTime.Now)
		{
		}

		private DateTime m_DateTimeStart;

		public DateTime DateTimeStart
		{
			get{return m_DateTimeStart;}
			set{m_DateTimeStart = value;}
		}

		private DateTime m_DateTimeEnd;

		public DateTime DateTimeEnd
		{
			get{return m_DateTimeEnd;}
			set{m_DateTimeEnd = value;}
		}

		private string m_Title;

		public virtual string Title
		{
			get{return m_Title;}
			set{m_Title = value;}
		}

		private Cells.Views.IView m_View;

		[Browsable(false)]
		public virtual Cells.Views.IView View
		{
			get{return m_View;}
			set{m_View = value;}
		}

		public bool ContainsDateTime(DateTime p_DateTime)
		{
			return (m_DateTimeStart <= p_DateTime && m_DateTimeEnd > p_DateTime);
		}

		private Cells.Controllers.IController mController;
		[Browsable(false)]
		public Cells.Controllers.IController Controller
		{
			get{return mController;}
			set{mController = value;}
		}
	}

	/// <summary>
	/// A collection of elements of type IAppointment
	/// </summary>
	public class AppointmentCollection : List<IAppointment>
	{
	}
}