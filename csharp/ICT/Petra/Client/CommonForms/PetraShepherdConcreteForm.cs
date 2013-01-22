//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       ChristianK, tomn, pauln
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Petra.Client.CommonForms.Logic;
using System.Xml;

using Ict.Common; //this is also a test

namespace Ict.Petra.Client.CommonForms
{
    ///<summary>Imlements TPetraShepherdForm (and therefore becomes a WinForm)
    /// and handles the GUI behaviour of a Shepherd. Utilises
    /// TPetraShepherdFormLogic for the base Shepherd Logic.</summary>
    public class TPetraShepherdConcreteForm : TPetraShepherdForm, IPetraShepherdConcreteFormInterface, Ict.Petra.Client.CommonForms.IFrmPetraEdit
    {
        /// <summary>Helper object for Edit screens.</summary>
        protected TFrmPetraEditUtils FPetraUtilsObject;

        /// <summary>Holds the DataSet that contains most data that is used on the screen.</summary>
        protected DataSet FMainDS;

        ///<summary>Name of the YAML file that contains the definition of the Shepherd Pages and the Shepherd overall</summary>
        protected string FYamlFile = String.Empty;

        /// <summary>Name of the Shepherd that will be imported. It has to be a global variable because it has to bounce from </summary>
        protected string ShepherdTitle = string.Empty;

        ///<summary>Instance of base Shepherd Logic.</summary>
        protected TPetraShepherdFormLogic FLogic;
        ///<summary>Instance of helper Class for navigation purposes</summary>
        private TShepherdNavigationHelper FShepherdNavigationHelper;

        ///<summary>Constructor</summary>
        public TPetraShepherdConcreteForm(Form AParentForm)
        {
            TLogging.Log("Entering TPetraShepherdConcreteForm Constructor...");

            FPetraUtilsObject = new TFrmPetraEditUtils(AParentForm, this, stbMain);

            TLogging.Log("TPetraShepherdConcreteForm Constructor ran.");
        }

        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnFinishClick(object sender, EventArgs e)
        {
            FLogic.HandleActionFinish();
        }

        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnNextClick(object sender, EventArgs e)
        {
            FLogic.HandleActionNext();
        }

        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnBackClick(object sender, EventArgs e)
        {
            FLogic.HandleActionBack();
        }

        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnCancelClick(object sender, EventArgs e)
        {
            FLogic.HandleActionCancel();
        }

        /// <summary>
        /// Overwrites virtual function in PetraShepherdForm.cs,
        /// calls function in PetraShepherdConcreteForm.cs in logic namespace to handle action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void BtnHelpClick(object sender, EventArgs e)
        {
            FLogic.HandleActionHelp();
        }

        /// <summary>
        /// Gets called when the Form is shown but before it's painted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, EventArgs e)
        {
            TLogging.Log("Entering TPetraShepherdConcreteForm (Base) Form_Load...");

            FShepherdNavigationHelper = new TShepherdNavigationHelper(FLogic.ShepherdPages, pnlNavigation);

            this.Text = ShepherdTitle;

            ShowCurrentPage();

            TLogging.Log("TPetraShepherdConcreteForm (Base) Form_Load ran.");
        }

        ///<summary>Update navigation buttons and navigation panel</summary>
        public void UpdateNavigation()
        {
            TLogging.Log("Updating Navigation Buttons.");

            if (FLogic.CurrentPage.IsFirstPage)
            {
                this.btnBack.Enabled = false;
            }
            else
            {
                this.btnBack.Enabled = true;
            }

            if (FLogic.CurrentPage.IsLastPage)
            {
                this.btnNext.Enabled = false;
                this.btnFinish.Enabled = true;
            }
            else
            {
                this.btnNext.Enabled = true;
                this.btnFinish.Enabled = false;
            }

            this.lblPageProgress.Text = "Page: " + FLogic.GetCurrentPageNumber() + "/" + FLogic.EnumeratePages();
            this.prbPageProgress.Value = ((int)FLogic.GetProgressBarPercentage());

            this.lblHeading1.Text = FLogic.CurrentPage.Title;
            this.lblHeading2.Text = FLogic.CurrentPage.Note;

            pnlCollapsibleNavigation.Text = Catalog.GetString("Shepherd Pages");
            pnlCollapsibleNavigation.TaskListNode = FLogic.CreateTaskList();
            pnlCollapsibleNavigation.Show();

            TLogging.Log("Added a node to the task list.");

            pnlCollapsibleNavigation.RealiseTaskListNow();

            TLogging.Log("UpdateNavigation");
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        /// <param name="AString"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void UpdateShepherdFormProperties(string AString, int width, int height)
        {
            TLogging.Log("UpdateShepherdFormProperties in commonForms--PetraShepherdConcreteFormGui");
            TLogging.Log("Size before UpdateShepherdFormProperties: " + pnlContent.Width + ", height: " + pnlContent.Width);
            TLogging.Log("Resizing the shepherd to the following: width: " + width + ", height: " + height);
            Size FormSize = new Size(width, height);
            this.Size = FormSize;
            ShepherdTitle = AString;

            TLogging.Log("Size AFTER UpdateShepherdFormProperties: width: " + pnlContent.Width + ", height: " + pnlContent.Width);
        }

        ///<summary>Displays the 'current' Shepherd Page and updates the navigation buttons and Navigation Panel</summary>
        public void ShowCurrentPage()
        {
            UserControl PageUserControl;
            IPetraEditUserControl PageUserEditControl;

            TLogging.Log("ShowCurrentPage");

            TLogging.Log("In ShowCurrentPage() method, at line 165, the current page is set to: " + FLogic.CurrentPage.ID);

            PageUserControl = FLogic.CurrentPage.UserControlInstance;


            #region Set up the UserControl

            // TODO: Ensure that the following initialisations are done *only the first time* a particular UserControl is shown
            //       (e.g. by adding a bool Field and Property such as e.g. 'UserControlInstanceInitialised' to the
            //       TPetraShepherdPage Class and checking/setting it here.

            // Set layout-related Control properties
            PageUserControl.Location = new Point(0, 0);
            PageUserControl.Dock = DockStyle.Fill;
            PageUserControl.AutoSize = true;
            PageUserControl.AutoSizeMode = AutoSizeMode.GrowOnly;

            PageUserEditControl = (IPetraEditUserControl)PageUserControl;
            PageUserEditControl.PetraUtilsObject = FPetraUtilsObject;
            PageUserEditControl.InitUserControl();

            #endregion


            // Add the UserControl to the Content Panel, ensuring no other UserControl is left there.
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(PageUserControl);


            UpdateNavigation();
        }

        ///<summary>Closes the Shepherd without any further ado and without saving</summary>
        public void CancelShepherd()
        {
            TLogging.Log("CancelShepherd");
        }

        /// <summary>
        /// Determines the changes in the screen's dataset and submits them to the
        /// Server.
        /// </summary>
        /// <param name="AInspectDS">The screen's dataset
        /// </param>
        /// <returns>True if saving of data succeeded, otherwise false.</returns>
        private Boolean SaveChanges(ref DataSet AInspectDS)
        {
            // TODO
            return false;
        }

        #region Implement interface functions
        /// TODO
        public void RunOnceOnActivation()
        {
        }

        /// <summary>
        /// Adds event handlers for the appropiate onChange event to call a central procedure
        /// </summary>
        public void HookupAllControls()
        {
        }

        /// TODO
        public void HookupAllInContainer(Control container)
        {
            FPetraUtilsObject.HookupAllInContainer(container);
        }

        /// TODO
        public bool CanClose()
        {
            return FPetraUtilsObject.CanClose();
        }

        /// TODO
        public TFrmPetraUtils GetPetraUtilsObject()
        {
            return (TFrmPetraUtils)FPetraUtilsObject;
        }

        /// <summary>
        /// Calls the internal SaveChanges Method.
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            bool ReturnValue;

            ReturnValue = SaveChanges(ref FMainDS);

            return ReturnValue;
        }

        #endregion
    }
}