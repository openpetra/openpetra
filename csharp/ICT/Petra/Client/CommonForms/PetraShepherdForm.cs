/*
 * Created by SharpDevelop.
 * User: Austin
 * Date: 1/13/2011
 * Time: 10:45 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Petra.Client.CommonForms
{
	/// <summary>
	/// Description of PetraShepherdForm.
	/// </summary>
	public partial class TPetraShepherdForm : Form
	{
		object FLogic;
			
		public TPetraShepherdForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			////FLogic=new objecttobecreated;
		}
		
		void BtnFinishClick(object sender, EventArgs e)
		{
			FLogic.HandleActionFinish();
		}
		
		void BtnNextClick(object sender, EventArgs e)
		{
			FLogic.HandleActionNext();
		}
		
		void BtnBackClick(object sender, EventArgs e)
		{
			FLogic.HandleActionBack();
		}
		
		void BtnCancelClick(object sender, EventArgs e)
		{
			FLogic.HandleActionCancel();
		}
		
		void BtnHelpClick(object sender, EventArgs e)
		{
			FLogic.HandleActionHelp();
		}
		
		void TPetraShepherdFormLoad(object sender, EventArgs e)
		{
			////FLogic.dosomething
		}
	}
}
