//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
//
using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using GNU.Gettext;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonForms;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MCommon.Gui
{
    /// <summary>
    /// Field Of Service Screen
    ///
    /// @Comment Currently just a very basic screen, used as a showcase on how to
    ///   make a new, basic Petra.NET screen.
    /// </summary>
    public partial class TFieldOfServiceWinForm : System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetraEdit
    {
        private TFrmPetraEditUtils FTheObject;

        #region Resourcestrings

        private static readonly string StrOpeningCancelledByUser = Catalog.GetString("Opening of Field Of Service screen got cancelled by user.");

        private static readonly string StrOpeningCancelledByUserTitle = Catalog.GetString("Screen opening cancelled");

        #endregion

        /// <summary>Reference to the Logic for the Screen</summary>
        private TFieldOfServiceLogic FLogic;

        /// <summary>Main Parameter for the Screen</summary>
        private Int64 FPartnerKey;

        /// <summary>Main DataSet for the Screen</summary>
        private FieldOfServiceTDS FMainDS;

        /// <summary>
        /// Constructor for the Screen.
        ///
        /// </summary>
        /// <returns>void</returns>
        public TFieldOfServiceWinForm(Form AParentForm) : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblPartnerKey.Text = Catalog.GetString("PartnerKey");
            this.lblPartnerName.Text = Catalog.GetString("Partner Name");
            this.Label3.Text = Catalog.GetString("Partner Name") + ":" +;
            this.Label4.Text = Catalog.GetString("PartnerKey") + ":" +;
            this.Text = Catalog.GetString("Field Of Service");
            #endregion

            // Initialise Screen Logic
            FLogic = new TFieldOfServiceLogic();

            FTheObject = new TFrmPetraEditUtils(AParentForm, this, stbMain);

            // TODO FTheObject.ActionEnablingEvent += ActionEnabledEvent;
            FTheObject.InitActionState();
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner for which the screen should be
        /// opened
        /// </param>
        /// <returns>void</returns>
        public void SetParameters(System.Int64 APartnerKey)
        {
            FPartnerKey = APartnerKey;
        }

        private void TFieldOfServiceWinForm_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!FLogic.GetFieldOfServiceUIConnector(FPartnerKey, out FMainDS))
                {
                    MessageBox.Show(StrOpeningCancelledByUser, StrOpeningCancelledByUserTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FTheObject.Close();
                    Close();
                    return;
                }
            }
            catch (EPartnerNotExistantException)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(
                    "Partner with Partner Key " + FPartnerKey.ToString() + " does not exist.", "Nonexistant Partner!", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // for the modal dialog (called from Progress)
                DialogResult = System.Windows.Forms.DialogResult.Cancel;

                FTheObject.Close();
                this.Close();
                return;
            }
            // on Exp: ESecurityDBTableAccessDeniedException do
            // begin
            // this.Cursor := Cursors.Default;
            //
            // TMessages.MsgSecurityException(Exp, this.GetType);
            //
            // for the modal dialog (called from Progress)
            // DialogResult := System.Windows.Forms.DialogResult.Cancel;
            // to prevent strange error message, that would stop the form from closing
            // FFormActivatedForFirstTime := false;
            // this.Close();
            // return;
            // end;
            // on Exp: ESecurityScreenAccessDeniedException do
            // begin
            // this.Cursor := Cursors.Default;
            //
            // TMessages.MsgSecurityException(Exp, this.GetType);
            //
            // for the modal dialog (called from Progress)
            // DialogResult := System.Windows.Forms.DialogResult.Cancel;
            // to prevent strange error message, that would stop the form from closing
            // FFormActivatedForFirstTime := false;
            // this.Close();
            // return;
            // end;
            catch (Exception Exp)
            {
                this.Cursor = Cursors.Default;
                TLogging.Log(
                    "An error occured while trying to retrieve data for the Field Of Service Screen!" + Environment.NewLine + Exp.ToString(),
                    TLoggingType.ToLogfile);

                // MessageBox.Show('An error occured while trying to retrieve data for the Field Of Service Screen!' + Environment.NewLine + 'For details see the log file.',
                // 'Error in Partner Edit Screen', MessageBoxButtons.OK, MessageBoxIcon.Stop);
                // for the modal dialog (called from Progress)
                DialogResult = System.Windows.Forms.DialogResult.Cancel;

                FTheObject.Close();
                this.Close();

                throw;
            }

            /*
             * From here on we have access to the Server Object and the DataSet is filled
             * with data.
             */
            PerformDataBinding();
            this.Cursor = Cursors.Default;
        }

        private void TFieldOfServiceWinForm_Closing(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!FTheObject.CloseFormCheckRun)
            {
                if (!CanClose())
                {
                    // MessageBox.Show('TFieldOfServiceWinForm.TPartnerEditDSWinForm_Closing: e.Cancel := true');
                    e.Cancel = true;
                }
                else
                {
                    FLogic.UnRegisterUIConnector();

                    // Needs to be set to false because it got set to true in ancestor Form!
                    e.Cancel = false;

                    // Need to call the following method in the Base Form to remove this Form from the Open Forms List
                    FTheObject.TFrmPetra_Closing(this, null);
                }
            }
            else
            {
                FLogic.UnRegisterUIConnector();

                // Needs to be set to false because it got set to true in ancestor Form!
                e.Cancel = false;

                // Need to call the following method in the Base Form to remove this Form from the Open Forms List
                FTheObject.TFrmPetra_Closing(this, null);
            }
        }

        /// <summary>
        /// Binds Controls on the Screen to Data from the DB
        ///
        /// </summary>
        /// <returns>void</returns>
        private void PerformDataBinding()
        {
            // DataBind individual Controls
            lblPartnerKey.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerKeyDBName());
            lblPartnerName.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerShortNameDBName());
        }

        /// <summary>
        /// Determines whether closing the Form is allowed, or not
        /// </summary>
        /// <returns>true if closing is allowed, false if it isn't
        /// </returns>
        public Boolean CanClose()
        {
            // TODO?
            return true;
        }

        /// <summary>
        /// for the interface
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            // TODO
            return false;
        }

        /// <summary>
        /// needed for interface
        /// </summary>
        public void RunOnceOnActivation()
        {
        }

        /// <summary>
        /// needed for interface
        /// </summary>
        public TFrmPetraUtils GetPetraUtilsObject()
        {
            return FTheObject;
        }
    }
}