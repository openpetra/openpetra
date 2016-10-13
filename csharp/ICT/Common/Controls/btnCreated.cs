//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, berndr, christiank
//
// Copyright 2004-2016 by OM International
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
//
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Ict.Common;
using System.Reflection;
using System.Resources;
using System.Globalization;

namespace Ict.Common.Controls
{
    /// <summary>
    /// The btnCreated Button is a small button which provides information on who and
    /// when something was changed on the hosting form. It is not databound to the
    /// host, so the developer needs to provide the necessary information himself.
    /// </summary>
    public class TbtnCreated : Button
    {
        private readonly string StrUnknown = Catalog.GetString("[Unknown]");

        private const Int32 UNIT_DEFAULT_WIDTH = 14;
        private const Int32 UNIT_DEFAULT_HEIGHT = 16;
        private const String UNIT_DATE_CREATED_COL = "s_date_created_d";
        private const String UNIT_CREATED_BY_COL = "s_created_by_c";
        private const String UNIT_DATE_MODIFIED_COL = "s_date_modified_d";
        private const String UNIT_MODIFIED_BY_COL = "s_modified_by_c";
        private const Int32 WM_MOUSEENTER = 0x123;
        private const Int32 WM_MOUSEHOVER = 0x123;

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolTip tipFields;

        /// <summary>Add strict private fields here</summary>
        private System.DateTime FDateCreated;
        private string FCreatedBy;
        private System.DateTime FDateModified;
        private string FModifiedBy;
        private string FToolTipString;

        #region property getter and setter

        /// <summary>
        /// This property gets or sets the creator.
        ///
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return this.FCreatedBy;
            }

            set
            {
                this.FCreatedBy = value;
                this.FToolTipString = this.BuildMessage();
                this.tipFields.SetToolTip(this, this.FToolTipString);
            }
        }

        /// <summary>
        /// This property gets or sets the creation date.
        ///
        /// </summary>
        public System.DateTime DateCreated
        {
            get
            {
                return this.FDateCreated;
            }

            set
            {
                this.FDateCreated = value;
                this.FToolTipString = this.BuildMessage();
                this.tipFields.SetToolTip(this, this.FToolTipString);
            }
        }

        /// <summary>
        /// This property determines which column should be sorted. This may be esential
        /// for heirs of this class which use more the one column in the combobox. The
        /// default value for this property is therefore NIL.
        ///
        /// </summary>
        public System.DateTime DateModified
        {
            get
            {
                return this.FDateModified;
            }

            set
            {
                this.FDateModified = value;
                this.FToolTipString = this.BuildMessage();
                this.tipFields.SetToolTip(this, this.FToolTipString);
            }
        }

        /// <summary>
        /// This property determines which column should be sorted. This may be esential
        /// for heirs of this class which use more the one column in the combobox. The
        /// default value for this property is therefore NIL.
        ///
        /// </summary>
        public string ModifiedBy
        {
            get
            {
                return this.FModifiedBy;
            }

            set
            {
                this.FModifiedBy = value;
                this.FToolTipString = this.BuildMessage();
                this.tipFields.SetToolTip(this, this.FToolTipString);
            }
        }

        /// <summary>
        /// This property determines which column should be sorted. This may be esential
        /// for heirs of this class which use more the one column in the combobox. The
        /// default value for this property is therefore NIL.
        ///
        /// </summary>
        public new System.Drawing.Size Size
        {
            get
            {
                return base.Size;
            }

            set
            {
                base.Size = value;
            }
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tipFields = new System.Windows.Forms.ToolTip(this.components);

            //
            // tipFields
            //
            this.tipFields.AutoPopDelay = 5000;
            this.tipFields.InitialDelay = 100;
            this.tipFields.ReshowDelay = 0;

            //
            // TbtnCreated
            //
            this.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        }

        #endregion

        #region Creation and disposal

        /// <summary>
        /// Private Declarations }
        /// The constructor of this class
        ///
        /// </summary>
        /// <returns>void</returns>
        public TbtnCreated() : base()
        {
            new TButtonImage();

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.SetDefaults();
            this.Image = TButtonImage.FPersonImage;
        }

        /// <summary>
        /// This function builts a message for the tooltip and the messagebox.
        /// </summary>
        /// <returns>The message string according to the property values.
        /// </returns>
        public string BuildMessage()
        {
            string ReturnValue;
            String mDateCreated;
            String mDateModified;

            //String mFormat;
            mDateCreated = "";
            mDateModified = "";
            ReturnValue = "";

            //mFormat = "dd/MMM/yyyy HH:mm:ss:ff";
            //mFormat = "dd/MMM/yyyy";

            // Get date created string
            if ((this.FDateCreated != System.DateTime.MinValue) && (this.FDateCreated != System.DateTime.MaxValue))
            {
                mDateCreated = StringHelper.DateToLocalizedString(this.FDateCreated);
            }

            // Get date modified string
            if ((this.FDateModified != System.DateTime.MinValue) && (this.FDateModified != System.DateTime.MaxValue))
            {
                mDateModified = StringHelper.DateToLocalizedString(this.FDateModified);
            }

            if ((mDateCreated != "") && (mDateModified != ""))
            {
                // Display all data
                ReturnValue = "Created: " + mDateCreated + " by " + this.FCreatedBy + "\r\nModified: " + mDateModified + " by " + this.FModifiedBy;
            }
            else if ((mDateCreated != "") && (mDateModified == ""))
            {
                // Display only created message
                ReturnValue = "Created: " + mDateCreated + " by " + this.FCreatedBy;
            }

            return ReturnValue;
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// This procedure processes the disposing of this class.
        /// </summary>
        /// <param name="Disposing">true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        /// <returns>void</returns>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (this.components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// Add strict private routines here
        /// </summary>
        /// <returns>void</returns>
        private void SetDefaults()
        {
            System.Drawing.Size mSize;
            mSize = new System.Drawing.Size(UNIT_DEFAULT_WIDTH, UNIT_DEFAULT_HEIGHT);
            this.Size = mSize;
            this.FToolTipString = this.BuildMessage();
            this.Tag = "dontdisable";
            this.Text = "";

            if (DesignMode == false)
            {
                this.tipFields.SetToolTip(this, this.FToolTipString);
            }

            // this.tipFields.ShowAlways;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        }

        #endregion

        #region Control events

        /// <summary>
        /// This procedure processes the click on this control.
        /// </summary>
        /// <param name="e">Event Arguments
        /// </param>
        /// <returns>void</returns>
        protected override void OnClick(System.EventArgs e)
        {
            base.OnClick(e);
            String m_String;
            m_String = "";
            m_String = this.BuildMessage();

            if (m_String != "")
            {
                MessageBox.Show(this.BuildMessage(), Catalog.GetString("Record Created / Modified Information"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// This procedure ensures that the control is displayed in the right size at
        /// the beginning.
        /// </summary>
        /// <returns>void</returns>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.SetDefaults();
        }

        /// <summary>
        /// This procedure ensures that the tooltip is displayed when the mouse enteres
        /// the button.
        /// </summary>
        /// <param name="e">Event Arguments
        /// </param>
        /// <returns>void</returns>
        protected override void OnMouseEnter(System.EventArgs e)
        {
            if ((DesignMode == false) && (this.tipFields.Active == false))
            {
                this.tipFields.SetToolTip(this, this.FToolTipString);
            }

            base.OnMouseEnter(e);
        }

        /// <summary>
        /// This procedure ensures that the tooltip is displayed when the mouse hovers
        /// over the button.
        /// </summary>
        /// <param name="e">Event Arguments
        /// </param>
        /// <returns>void</returns>
        protected override void OnMouseHover(System.EventArgs e)
        {
            if (DesignMode == false)
            {
                this.tipFields.SetToolTip(this, this.FToolTipString);
            }

            base.OnMouseHover(e);
        }

        /// <summary>
        /// This procedure ensures that the height of this control cannot be changed.
        /// </summary>
        /// <param name="e">Event Arguments
        /// </param>
        /// <returns>void</returns>
        protected override void OnResize(System.EventArgs e)
        {
            System.Drawing.Size mSize;
            mSize = new System.Drawing.Size(UNIT_DEFAULT_WIDTH, UNIT_DEFAULT_HEIGHT);
            this.Size = mSize;
        }

        #endregion

        #region Update field values

        /// <summary>
        /// This procedure updates the following fields in the TbtnButton class:
        /// CreatedBy, DateCreated, DateModified, ModifiedBy.
        /// The <see cref="DataRow"/> with row number <paramref name="ARow "/> in the 
        /// <see cref="DataTable"/> passed in with <paramref name="ATable"/> is used for this.
        /// </summary>
        /// <param name="ATable">A table to which this control refers.</param>
        /// <param name="ARow">A row in the table which holds the data.</param>
        public void UpdateFields(DataTable ATable, int ARow)
        {
            if (ATable.Rows.Count > 0)
            {
                // Update the Fields according to data found in the DataRow.
                UpdateFields(ATable.Rows[ARow]);
            }
        }

        /// <summary>
        /// This procedure updates the following fields in the TbtnButton class:
        /// CreatedBy, DateCreated, DateModified, ModifiedBy.
        /// The <see cref="DataRow"/> passed in with <paramref name="ARow"/> is used for this.
        /// </summary>
        /// <param name="ARow">A <see cref="DataRow"/> which holds the data.</param>
        public void UpdateFields(DataRow ARow)
        {
            DateTime DateCreated;
            DateTime DateModified;

            // A null date is transformed to the 1st January 0001
            DateCreated = TSaveConvert.ObjectToDate(ARow[UNIT_DATE_CREATED_COL]);
            DateModified = TSaveConvert.ObjectToDate(ARow[UNIT_DATE_MODIFIED_COL]);

            this.DateCreated = DateCreated;
            CreatedBy = ARow[UNIT_CREATED_BY_COL].ToString() != String.Empty ? 
                System.Convert.ToString(ARow[UNIT_CREATED_BY_COL]) : StrUnknown;

            this.DateModified = DateModified;
            ModifiedBy = ARow[UNIT_MODIFIED_BY_COL].ToString() != String.Empty ?
                System.Convert.ToString(ARow[UNIT_MODIFIED_BY_COL]) : StrUnknown;

            this.tipFields.SetToolTip(this, this.FToolTipString);
        }

        /// <summary>
        /// This procedure updates the following fields in the TbtnButton class:
        /// CreatedBy, DateCreated, DateModified, ModifiedBy.
        /// If the <see cref="DataTable"/> passed in with <paramref name="ATable"/>
        /// holds only one <see cref="DataRow"/> then this is used for this,
        /// otherwise a search is done for the smallest "created" field
        /// and the biggest "modified" field.
        /// </summary>
        /// <param name="ATable">A table to which this control refers.</param>
        public void UpdateFields(DataTable ATable)
        {
            if (ATable.Rows.Count == 1)
            {
                // if we have only one row then just use it
                this.UpdateFields(ATable, 0);
            }
            else
            {
                // Update special:
                this.UpdateFields_SearchDate(ATable);
            }
        }

        /// <summary>
        /// This procedure updates the following fields in the TbtnButton class:
        /// CreatedBy, DateCreated, DateModified, ModifiedBy.
        /// The <see cref="DataRow"/> with row number <paramref name="ARow "/> in the 
        /// <see cref="DataView"/> passed in with <paramref name="AView"/> is used for this.
        /// </summary>
        /// <param name="AView">A <see cref="DataView"/> to which this control refers.</param>
        /// <param name="ARow">A row in the table which holds the data.</param>
        public void UpdateFields(DataView AView, int ARow)
        {
            if (AView.Count > 0)
            {
                UpdateFields(AView[ARow].Row);
            }
        }

        /// <summary>
        /// This procedure updates the following fields in the TbtnButton class:
        /// CreatedBy, DateCreated, DateModified, ModifiedBy.
        /// The <em>first</em> <see cref="DataRow"/> in the <see cref="DataView"/> passed in with 
        /// <paramref name="AView"/> is used for this.
        /// </summary>
        /// <param name="AView">A <see cref="DataView"/> to which this control referes.</param>
        public void UpdateFields(DataView AView)
        {
            this.UpdateFields(AView, 0);
        }

        /// <summary>
        /// Search in each row for the smallest "created" field
        /// and the biggest "modified" field.
        /// Update these values: CreatedBy, DateCreated, DateModified, ModifiedBy.
        /// </summary>
        /// <param name="ADataTable">The data table to retrieve the modified and created values.</param>
        private void UpdateFields_SearchDate(DataTable ADataTable)
        {
            System.DateTime TempCreatedDate = new System.DateTime(System.DateTime.MinValue.Ticks);
            System.DateTime TempModifiedDate = new System.DateTime(System.DateTime.MaxValue.Ticks);
            System.DateTime TempDate = new System.DateTime(2000, 1, 1);
            String TempCreatedBy = StrUnknown;
            String TempModifiedBy = StrUnknown;

            // search for the smallest / biggest field
            for (int Counter = 0; Counter < ADataTable.Rows.Count; ++Counter)
            {
                System.Data.DataRow CurrentRow = ADataTable.Rows[Counter];

                if (Counter == 0)
                {
                    TempCreatedDate = TSaveConvert.ObjectToDate(CurrentRow[UNIT_DATE_CREATED_COL]);
                    TempModifiedDate = TSaveConvert.ObjectToDate(CurrentRow[UNIT_DATE_MODIFIED_COL]);
                    TempCreatedBy = System.Convert.ToString(CurrentRow[UNIT_CREATED_BY_COL]);
                    TempModifiedBy = System.Convert.ToString(CurrentRow[UNIT_MODIFIED_BY_COL]);
                }
                else
                {
                    TempDate = TSaveConvert.ObjectToDate(CurrentRow[UNIT_DATE_CREATED_COL]);

                    if (TempDate.CompareTo(TempCreatedDate) < 0)
                    {
                        TempCreatedDate = TempDate;
                        TempCreatedBy = System.Convert.ToString(CurrentRow[UNIT_CREATED_BY_COL]);
                    }

                    TempDate = TSaveConvert.ObjectToDate(CurrentRow[UNIT_DATE_MODIFIED_COL]);

                    if (TempDate.CompareTo(TempModifiedDate) > 0)
                    {
                        TempModifiedDate = TempDate;
                        TempModifiedBy = System.Convert.ToString(CurrentRow[UNIT_MODIFIED_BY_COL]);
                    }
                }
            }

            // update the control
            DateCreated = TempCreatedDate;
            DateModified = TempModifiedDate;
            CreatedBy = TempCreatedBy;
            ModifiedBy = TempModifiedBy;

            this.tipFields.SetToolTip(this, this.FToolTipString);
        }

        #endregion

        /// <summary>
        /// Provides the image for the TbtnCreated class.
        /// </summary>
        private class TButtonImage
        {
            /// <summary>Field containing the image</summary>
            public static Image FPersonImage;

            /// <summary>
            /// Constructor.
            /// </summary>
            public TButtonImage()
            {
                try
                {
                    ResourceManager resman =
                        new ResourceManager("Ict.Common.Controls.Icons", Assembly.GetExecutingAssembly());

                    FPersonImage = ((Icon)resman.GetObject("DateUserChanged")).ToBitmap();
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Unhandled Exception occured in TbtnCreated.TButtonImage Constructor: " + exp.ToString());
                }
            }
        }
    }

    /// <summary>
    /// Helper Class for the <see cref="TbtnCreated"/> Class.
    /// </summary>
    public static class TbtnCreatedHelper
    {
        /// <summary>
        /// This Method manually adds a <see cref="TbtnCreated"/> for the modified/created information to a
        /// Panel or GroupBox - inside a newly created Panel that gets Docked to the right of the Panel or GroupBox. 
        /// (The WinForms Generator doesn't have a built-in support for the creation of those buttons yet [Bug #1782]).
        /// </summary>
        /// <param name="ABtnCreated">Pass in a freshly created instance of a <see cref="TbtnCreated"/> Control - 
        /// no need to set any Properties on it, all necessary layout-related Properties will be set by this Method! If
        /// no instance gets passed in then it will get created by this Method automatically.</param>
        /// <param name="AContainerControl">A Panel or a GroupBox Control that is to host a newly created Panel that 
        /// will host the instance of the <see cref="TbtnCreated"/> Control that gets passed in with 
        /// <paramref name="ABtnCreated"/>.</param>
        /// <param name="ATabIndexOfNewContainingPanel">TabIndex of the newly created Panel that will host the instance 
        /// of the <see cref="TbtnCreated"/> Control that gets passed in with 
        /// <paramref name="ABtnCreated"/> (default=999, to ensure it comes after all other Controls found in the 
        /// <paramref name="AContainerControl"/>).</param>
        /// <param name="AOuterContainerControl">Optional (outer) container control that contains 
        /// <paramref name="AContainerControl"/> (default = null). If a Control gets passed in with this Argument then
        /// SuspendLayout() and ResumeLayout(true) will get called on it at the appropriate times in this Method. This
        /// can be necessary to make other Controls that are Anchored left, top, and right in 
        /// <paramref name="AContainerControl"/> expand and contract correctly!</param>
        /// <param name="ABtnCreatedName">Name of the <see cref="TbtnCreated"/> Control that gets passed in with 
        /// <paramref name="ABtnCreated"/> (default="btnCreatedModified").</param>
        /// <param name="ACustomYLocation">By default the <see cref="TbtnCreated"/> Control that gets passed in with 
        /// <paramref name="ABtnCreated"/> is positioned 5 pixels from the top of the newly created Panel that gets Docked
        /// to the right of <paramref name="AContainerControl"/> (fits for a <paramref name="AContainerControl"/> that is 
        /// the usual 'pnlDetail' of a List/Detail Form/UserControl. Pass in a different value for positioning it differently
        /// (typically, the value passed should be 0 when <paramref name="AContainerControl"/> is a GroupBox and 7 when
        /// <paramref name="AContainerControl"/> is a Fill-Docked-Panel that hosts a UserControl that forms the Details of 
        /// a List/Detail Form/UserControl.</param>
        /// <param name="AOptionalLayoutFixupCode">Optional custom 'layout fixup code' (default = null). If a Delegate gets 
        /// passed in with this Argument then it will get called at the appropriate time in this Method. This
        /// can be necessary to make other Controls that are Anchored left, top, and right in 
        /// <paramref name="AContainerControl"/> expand and contract correctly!</param>
        public static void AddModifiedCreatedButtonToContainerControl(ref TbtnCreated ABtnCreated, Control AContainerControl,
            int ATabIndexOfNewContainingPanel = 999, Control AOuterContainerControl = null, 
            string ABtnCreatedName = "btnCreatedModified",
            int ACustomYLocation = -1, Action AOptionalLayoutFixupCode = null)
        {
            if (!((AContainerControl is Panel) || (AContainerControl is GroupBox)))
            {
                throw new ArgumentException("AContainerControl must be a Panel Control or a GroupBox Control",
                    "AContainerControl");
            }

            if (ABtnCreated == null)
            {
                ABtnCreated = new TbtnCreated();
            }

            if (AOuterContainerControl != null)
            {
                AOuterContainerControl.SuspendLayout();
            }

            AContainerControl.SuspendLayout();

            //
            // pnlModifiedCreatedButtons
            //
            Panel pnlModifiedCreatedButtons = new Panel();
            pnlModifiedCreatedButtons.SuspendLayout();

            pnlModifiedCreatedButtons.Dock = DockStyle.Right;
            pnlModifiedCreatedButtons.Width = 20;
            pnlModifiedCreatedButtons.TabIndex = ATabIndexOfNewContainingPanel;
            //pnlModifiedCreatedButtons.BackColor = System.Drawing.Color.Green;     // enable this line if you have trouble positioning the Panel...

            //
            // ABtnCreated
            //
            ABtnCreated.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | 
                 System.Windows.Forms.AnchorStyles.Left));
            ABtnCreated.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            ABtnCreated.Left = 1;

            if (ACustomYLocation == -1)
            {
                ABtnCreated.Top = 5;
            }
            else
            {
                ABtnCreated.Top = ACustomYLocation;
            }
            
            ABtnCreated.Size = new System.Drawing.Size(14, 16);
            ABtnCreated.TabIndex = 0;
            ABtnCreated.Name = ABtnCreatedName;
            ABtnCreated.CreatedBy = null;
            ABtnCreated.DateCreated = new System.DateTime((System.Int64)0);
            ABtnCreated.DateModified = new System.DateTime((System.Int64)0);
            ABtnCreated.ModifiedBy = null;


            pnlModifiedCreatedButtons.Controls.Add(ABtnCreated);
            AContainerControl.Controls.Add(pnlModifiedCreatedButtons);
            pnlModifiedCreatedButtons.SendToBack();
            pnlModifiedCreatedButtons.ResumeLayout(false);

            if (AOptionalLayoutFixupCode != null)
            {
                AOptionalLayoutFixupCode();
            }

            AContainerControl.ResumeLayout(true);

            if (AOuterContainerControl != null)
            {
                AOuterContainerControl.ResumeLayout(true);
            }
        }
    }
}