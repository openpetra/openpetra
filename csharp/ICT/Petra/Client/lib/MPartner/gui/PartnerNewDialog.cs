/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;
using SourceGrid;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MPartner;
using Ict.Common;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Partner New Dialog. Called from Partner Edit screen.
    /// </summary>
    public class TPartnerNewDialogWinForm : System.Windows.Forms.Form, IFrmPetra
    {
        /// <summary>todoComment</summary>
        public const String StrCantCreateNewPartner = "New Partner can't be created because there are " + "no Installed Sites available!" + "\r\n" +
                                                      "Please set up at least one Installed Site in the " + "System Manager Module!";

        /// <summary>todoComment</summary>
        public const String StrFamilyNeedsToBeSelected = "A Family needs to be selected when a new Par" +
                                                         "tner of Partner Class 'PERSON' should be created!";

        /// <summary>todoComment</summary>
        public const String StrFamilyNeedsToBeSelectedTitle = "Family Needed!";

        /// <summary>todoComment</summary>
        public const String StrAPartnerKeyExists1 = "A Partner with Partner Key ";

        /// <summary>todoComment</summary>
        public const String StrAPartnerKeyExists2 = " already exists." + "\r\n" + "Please choose a different Partner Key!";

        /// <summary>todoComment</summary>
        public const String StrAPartnerKeyExistsTitle = "Partner Key already in use";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblSitesAvailable;
        private System.Windows.Forms.Label lblPartnerKey;
        private System.Windows.Forms.Label lblPartnerClass;
        private System.Windows.Forms.Label Label1;
        private TTxtPartnerKeyTextBox txtPartnerKey;
        private TCmbAutoPopulated cmbAcquisitionCode;
        private TSgrdDataGrid grdInstalledSites;
        private TCmbAutoPopulated cmbPartnerClass;
        private System.Windows.Forms.CheckBox chkPrivatePartner;
        private TtxtAutoPopulatedButtonLabel txtFamilyPartnerBox;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlBtnOKCancelHelpLayout;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnHelp;

        private TPartnerNewDialogScreenLogic FLogic;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private Int64 FPartnerKey;
        private Int64 FSiteKey;
        private String FPartnerClass;
        private String FAcquisitionCode;
        private Int64 FFamilyPartnerKey;
        private Int32 FFamilyLocationKey;
        private Int64 FFamilySiteKey;
        private Boolean FPrivatePartner;
        private DataTable FInstalledSitesDT;
        private Boolean FDataGridRowEnteredRepeatedly;
        private Boolean FFamilyPartnerSelectionSetup;
        private Boolean FFormSetupFinished = false;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TPartnerNewDialogWinForm));
            this.lblSitesAvailable = new System.Windows.Forms.Label();
            this.lblPartnerKey = new System.Windows.Forms.Label();
            this.lblPartnerClass = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.grdInstalledSites = new Ict.Common.Controls.TSgrdDataGrid();
            this.cmbPartnerClass = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.chkPrivatePartner = new System.Windows.Forms.CheckBox();
            this.txtPartnerKey = new Ict.Common.Controls.TTxtPartnerKeyTextBox();
            this.txtFamilyPartnerBox = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.pnlBtnOKCancelHelpLayout = new System.Windows.Forms.Panel();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            this.SuspendLayout();

            //
            // btnOK
            //
            this.btnOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(396, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 20);
            this.btnOK.TabIndex = 997;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(478, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 20);
            this.btnCancel.TabIndex = 998;
            this.btnCancel.Text = "&Cancel";


            //
            // btnHelp
            //
            this.btnHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(8, 10);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 20);
            this.btnHelp.TabIndex = 996;
            this.btnHelp.Text = "&Help";
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnCancel);
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnOK);
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnHelp);
            this.pnlBtnOKCancelHelpLayout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 290);
            this.pnlBtnOKCancelHelpLayout.Name = "pnlBtnOKCancelHelpLayout";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(560, 34);
            this.pnlBtnOKCancelHelpLayout.TabIndex = 996;

            //
            // lblSitesAvailable
            //
            this.lblSitesAvailable.Location = new System.Drawing.Point(12, 16);
            this.lblSitesAvailable.Name = "lblSitesAvailable";
            this.lblSitesAvailable.Size = new System.Drawing.Size(100, 16);
            this.lblSitesAvailable.TabIndex = 0;
            this.lblSitesAvailable.Text = "S&ites Available:";
            this.lblSitesAvailable.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblPartnerKey
            //
            this.lblPartnerKey.Location = new System.Drawing.Point(18, 198);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.Size = new System.Drawing.Size(94, 23);
            this.lblPartnerKey.TabIndex = 2;
            this.lblPartnerKey.Text = "Partner &Key:";
            this.lblPartnerKey.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblPartnerClass
            //
            this.lblPartnerClass.Location = new System.Drawing.Point(22, 222);
            this.lblPartnerClass.Name = "lblPartnerClass";
            this.lblPartnerClass.Size = new System.Drawing.Size(90, 23);
            this.lblPartnerClass.TabIndex = 4;
            this.lblPartnerClass.Text = "Partner C&lass:";
            this.lblPartnerClass.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // Label1
            //
            this.Label1.Location = new System.Drawing.Point(-8, 244);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(120, 20);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "&Acquisition Code:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // cmbAcquisitionCode
            //
            this.cmbAcquisitionCode.ComboBoxWidth = 95;
            this.cmbAcquisitionCode.ListTable = TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(118, 242);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbAcquisitionCode.SelectedItem")));
            this.cmbAcquisitionCode.SelectedValue = null;
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(384, 22);
            this.cmbAcquisitionCode.TabIndex = 8;

            //
            // grdInstalledSites
            //
            this.grdInstalledSites.AlternatingBackgroundColour = System.Drawing.Color.FromArgb(230, 230, 230);
            this.grdInstalledSites.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdInstalledSites.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdInstalledSites.DeleteQuestionMessage = "You have chosen to delete" +
                                                           " this record.'#13#10#13#10'Do you really want to delete it?";
            this.grdInstalledSites.FixedRows = 1;
            this.grdInstalledSites.Location = new System.Drawing.Point(118, 14);
            this.grdInstalledSites.MinimumHeight = 19;
            this.grdInstalledSites.Name = "grdInstalledSites";
            this.grdInstalledSites.Size = new System.Drawing.Size(436, 176);
            this.grdInstalledSites.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));
            this.grdInstalledSites.TabIndex = 1;
            this.grdInstalledSites.TabStop = true;

            //
            // cmbPartnerClass
            //
            this.cmbPartnerClass.ComboBoxWidth = 130;
            this.cmbPartnerClass.ListTable = TCmbAutoPopulated.TListTableEnum.PartnerClassList;
            this.cmbPartnerClass.Location = new System.Drawing.Point(118, 219);
            this.cmbPartnerClass.Name = "cmbPartnerClass";
            this.cmbPartnerClass.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbPartnerClass.SelectedItem")));
            this.cmbPartnerClass.SelectedValue = null;
            this.cmbPartnerClass.Size = new System.Drawing.Size(132, 22);
            this.cmbPartnerClass.TabIndex = 5;
            this.cmbPartnerClass.SelectedValueChanged += new TSelectedValueChangedEventHandler(this.CmbPartnerClass_SelectedValueChanged);

            //
            // chkPrivatePartner
            //
            this.chkPrivatePartner.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPrivatePartner.Location = new System.Drawing.Point(10, 262);
            this.chkPrivatePartner.Name = "chkPrivatePartner";
            this.chkPrivatePartner.Size = new System.Drawing.Size(122, 24);
            this.chkPrivatePartner.TabIndex = 9;
            this.chkPrivatePartner.Text = "&Private Partner:";
            this.chkPrivatePartner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPrivatePartner.Visible = false;

            //
            // txtPartnerKey
            //
            this.txtPartnerKey.BackColor = System.Drawing.SystemColors.Control;
            this.txtPartnerKey.DelegateFallbackLabel = true;
            this.txtPartnerKey.Font = new System.Drawing.Font("Courier New", 9.25f, System.Drawing.FontStyle.Bold);
            this.txtPartnerKey.LabelText = "Partner Key";
            this.txtPartnerKey.Location = new System.Drawing.Point(118, 196);
            this.txtPartnerKey.MaxLength = 10;
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.PartnerKey = (Int64)0;
            this.txtPartnerKey.ReadOnly = false;
            this.txtPartnerKey.Size = new System.Drawing.Size(92, 22);
            this.txtPartnerKey.TabIndex = 3;
            this.txtPartnerKey.TextBoxReadOnly = false;
            this.txtPartnerKey.TextBoxWidth = 88;

            //
            // txtFamilyPartnerBox
            //
            this.txtFamilyPartnerBox.ASpecialSetting = true;
            this.txtFamilyPartnerBox.ButtonText = "&Family...";
            this.txtFamilyPartnerBox.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtFamilyPartnerBox.ButtonWidth = 70;
            this.txtFamilyPartnerBox.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtFamilyPartnerBox.Location = new System.Drawing.Point(256, 218);
            this.txtFamilyPartnerBox.MaxLength = 32767;
            this.txtFamilyPartnerBox.Name = "txtFamilyPartnerBox";
            this.txtFamilyPartnerBox.PartnerClass = "FAMILY";
            this.txtFamilyPartnerBox.ReadOnly = true;
            this.txtFamilyPartnerBox.Size = new System.Drawing.Size(294, 23);
            this.txtFamilyPartnerBox.TabIndex = 6;
            this.txtFamilyPartnerBox.TextBoxWidth = 80;
            this.txtFamilyPartnerBox.Visible = false;
            this.txtFamilyPartnerBox.PartnerFound += new TDelegatePartnerFound(this.TxtFamilyPartnerBox_PartnerFound);

            //
            // TPartnerNewDialogWinForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(560, 346);
            this.Controls.Add(this.txtFamilyPartnerBox);
            this.Controls.Add(this.txtPartnerKey);
            this.Controls.Add(this.grdInstalledSites);
            this.Controls.Add(this.cmbAcquisitionCode);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblPartnerClass);
            this.Controls.Add(this.lblPartnerKey);
            this.Controls.Add(this.lblSitesAvailable);
            this.Controls.Add(this.cmbPartnerClass);
            this.Controls.Add(this.chkPrivatePartner);
            this.Controls.Add(this.pnlBtnOKCancelHelpLayout);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerNewDialogWinForm";
            this.Text = "New Partner";
            this.Load += new System.EventHandler(this.TPartnerNewDialogWinForm_Load);
            this.Controls.SetChildIndex(this.chkPrivatePartner, 0);
            this.Controls.SetChildIndex(this.cmbPartnerClass, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.Controls.SetChildIndex(this.lblSitesAvailable, 0);
            this.Controls.SetChildIndex(this.lblPartnerKey, 0);
            this.Controls.SetChildIndex(this.lblPartnerClass, 0);
            this.Controls.SetChildIndex(this.Label1, 0);
            this.Controls.SetChildIndex(this.cmbAcquisitionCode, 0);
            this.Controls.SetChildIndex(this.grdInstalledSites, 0);
            this.Controls.SetChildIndex(this.txtPartnerKey, 0);
            this.Controls.SetChildIndex(this.txtFamilyPartnerBox, 0);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private void DataGrid_FocusRowEntered(System.Object ASender, RowEventArgs AEventArgs)
        {
            Int64 NewSiteKey;

            NewSiteKey = FLogic.DetermineCurrentSitePartnerKey(grdInstalledSites);

            // different SiteKey? If yes, retrieve the next available Partner Key for the Site
            if ((NewSiteKey != FSiteKey) || (!FDataGridRowEnteredRepeatedly))
            {
                FPartnerKey = FPartnerEditUIConnector.GetPartnerKeyForNewPartner(NewSiteKey);
                txtPartnerKey.PartnerKey = FPartnerKey;
                FSiteKey = NewSiteKey;
                FDataGridRowEnteredRepeatedly = true;
            }
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// </summary>
        /// <returns>void</returns>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="AFamilyPartnerKey"></param>
        /// <param name="AFamilyLocationKey"></param>
        /// <param name="AFamilySiteKey"></param>
        /// <returns></returns>
        public Boolean GetReturnedParameters(out String APartnerClass,
            out System.Int64 ASiteKey,
            out System.Int64 APartnerKey,
            out String AAcquisitionCode,
            out Boolean APrivatePartner,
            out Int64 AFamilyPartnerKey,
            out Int32 AFamilyLocationKey,
            out Int64 AFamilySiteKey)
        {
            Boolean ReturnValue;

            APartnerClass = "";
            AAcquisitionCode = "";
            ASiteKey = -1;
            APartnerKey = -1;
            APrivatePartner = false;
            AFamilyPartnerKey = -1;
            AFamilyLocationKey = -1;
            AFamilySiteKey = -1;

            if (FFormSetupFinished)
            {
                APartnerClass = cmbPartnerClass.SelectedItem.ToString();
                AAcquisitionCode = cmbAcquisitionCode.SelectedItem.ToString();
                ASiteKey = FSiteKey;
                APartnerKey = FPartnerKey;
                APrivatePartner = chkPrivatePartner.Checked;
                AFamilyPartnerKey = FFamilyPartnerKey;
                AFamilyLocationKey = FFamilyLocationKey;
                AFamilySiteKey = FFamilySiteKey;
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TPartnerNewDialogWinForm(IntPtr AParentFormHandle) : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FPetraUtilsObject = new TFrmPetraUtils(AParentFormHandle, this);
            this.FPetraUtilsObject.SetStatusBarText(this.btnOK, "Accept data and continue");
            this.FPetraUtilsObject.SetStatusBarText(this.btnCancel, "Cancel data entry and close");
            this.FPetraUtilsObject.SetStatusBarText(this.btnHelp, "Help");
            this.FPetraUtilsObject.SetStatusBarText(this.cmbAcquisitionCode, "Please select an A" + "cquisition Code");
            this.FPetraUtilsObject.SetStatusBarText(this.grdInstalledSites, "Please select a Sit" + 'e');
            this.FPetraUtilsObject.SetStatusBarText(this.cmbPartnerClass, "Please select a Partn" + "er Class");
            this.FPetraUtilsObject.SetStatusBarText(this.txtPartnerKey, "Please enter a Partner " + "Key or Accept the default Partner Key");
            this.FPetraUtilsObject.SetStatusBarText(this.txtFamilyPartnerBox, "Please select a Fam" + "ily that the Person should belong to");

            FFormSetupFinished = false;
        }

        private TFrmPetraUtils FPetraUtilsObject;

        private void TxtFamilyPartnerBox_PartnerFound(Int64 APartnerKey, int ALocationKey)
        {
            FFamilyPartnerKey = APartnerKey;
            FFamilyLocationKey = ALocationKey;

            // todo: return the sitekey of the family; at the moment only the FIXED_SITE_KEY is used for families
            FFamilySiteKey = SharedConstants.FIXED_SITE_KEY;
        }

        private void CmbPartnerClass_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
            if (FFormSetupFinished)
            {
                if (cmbPartnerClass.SelectedValue.ToString() == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
                    ShowFamilyPartnerSelection(true);
                }
                else
                {
                    ShowFamilyPartnerSelection(false);
                }
            }
        }

        private void ShowFamilyPartnerSelection(Boolean AShow)
        {
            if (AShow)
            {
                if (!FFamilyPartnerSelectionSetup)
                {
                    if (FFamilyPartnerKey != -1)
                    {
                        txtFamilyPartnerBox.Text = FFamilyPartnerKey.ToString();
                    }

                    FFamilyPartnerSelectionSetup = true;
                }
            }

            txtFamilyPartnerBox.Visible = AShow;
        }

        private void BtnHelp_Click(System.Object sender, System.EventArgs e)
        {
            // TODO TCommonMenuCommands.PetraHelp.AboutAndHelp(this);
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            Int64 NewPartnerKey;
            String NewPartnerClass;

            NewPartnerClass = cmbPartnerClass.SelectedItem.ToString();

            if (NewPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
            {
                FFamilyPartnerKey = Convert.ToInt32(txtFamilyPartnerBox.Text);

                if (FFamilyPartnerKey == 0)
                {
                    MessageBox.Show(StrFamilyNeedsToBeSelected, StrFamilyNeedsToBeSelectedTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtFamilyPartnerBox.Focus();
                    return;
                }
            }

            NewPartnerKey = txtPartnerKey.PartnerKey;

            if (FPartnerEditUIConnector.SubmitPartnerKeyForNewPartner(FSiteKey, FPartnerKey, ref NewPartnerKey))
            {
                FPartnerKey = NewPartnerKey;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(StrAPartnerKeyExists1 + txtPartnerKey.PartnerKey.ToString() + StrAPartnerKeyExists2, StrAPartnerKeyExistsTitle);
                txtPartnerKey.Focus();
            }
        }

        private void InitialiseUI()
        {
            if (FPartnerClass != "")
            {
                cmbPartnerClass.SelectedItem = FPartnerClass;
            }
            else
            {
                // Default value: FAMILY
                cmbPartnerClass.SelectedItem = "FAMILY";
            }

            if (FAcquisitionCode != "")
            {
                cmbAcquisitionCode.SelectedItem = FAcquisitionCode;
            }
            else
            {
                // Default value: UserDefault PARTNER_ACQUISITIONCODE
                cmbAcquisitionCode.SelectedItem = TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_ACQUISITIONCODE, "MAILROOM");
            }

            // Default value: false (from SetParameters default)
            chkPrivatePartner.Checked = FPrivatePartner;

            if (FPartnerKey != -1)
            {
                txtPartnerKey.PartnerKey = FPartnerKey;

                // suppress setting of PartnerKey through DataGrid_FocusRowEntered...
                FDataGridRowEnteredRepeatedly = true;
            }
        }

        private void TPartnerNewDialogWinForm_Load(System.Object sender, System.EventArgs e)
        {
            FLogic = new TPartnerNewDialogScreenLogic();
            cmbPartnerClass.InitialiseUserControl();
            cmbAcquisitionCode.InitialiseUserControl();

            // Hide invalid Acquisition Codes
            this.cmbAcquisitionCode.Filter = PAcquisitionTable.GetValidAcquisitionDBName() + " <> 0";

            // Setup Data
            FInstalledSitesDT = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.InstalledSitesList);

            if (FInstalledSitesDT.Rows.Count != 0)
            {
                FLogic.CreateColumns(grdInstalledSites, FInstalledSitesDT);

                // DataBindingrelated stuff
                SetupDataGridDataBinding();

                // Setup screen with default values
                InitialiseUI();

                if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
                {
                    ShowFamilyPartnerSelection(true);
                }

                // Make the Grid respond on updown keys
                cmbPartnerClass.Focus();
                grdInstalledSites.Focus();

                FFormSetupFinished = true;
            }
            else
            {
                MessageBox.Show(StrCantCreateNewPartner, CommonResourcestrings.StrErrorNoInstalledSites);
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
                return;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="AFamilyPartnerKey"></param>
        /// <param name="AFamilyLocationKey"></param>
        /// <param name="AFamilySiteKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 AFamilyPartnerKey,
            Int32 AFamilyLocationKey,
            Int64 AFamilySiteKey)
        {
            FPartnerEditUIConnector = APartnerEditUIConnector;
            FPartnerKey = APartnerKey;
            FSiteKey = ASiteKey;
            FPartnerClass = APartnerClass;
            FAcquisitionCode = AAcquisitionCode;
            FPrivatePartner = APrivatePartner;
            FFamilyPartnerKey = AFamilyPartnerKey;
            FFamilyLocationKey = AFamilyLocationKey;
            FFamilySiteKey = AFamilySiteKey;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="AFamilyPartnerKey"></param>
        /// <param name="AFamilyLocationKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 AFamilyPartnerKey,
            Int32 AFamilyLocationKey)
        {
            SetParameters(APartnerEditUIConnector,
                APartnerClass,
                ASiteKey,
                APartnerKey,
                AAcquisitionCode,
                APrivatePartner,
                AFamilyPartnerKey,
                AFamilyLocationKey,
                -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        /// <param name="AFamilyPartnerKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode,
            Boolean APrivatePartner,
            Int64 AFamilyPartnerKey)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, APartnerKey, AAcquisitionCode, APrivatePartner, AFamilyPartnerKey, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        /// <param name="APrivatePartner"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode,
            Boolean APrivatePartner)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, APartnerKey, AAcquisitionCode, APrivatePartner, -1, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AAcquisitionCode"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey,
            String AAcquisitionCode)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, APartnerKey, AAcquisitionCode, false, -1, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="APartnerKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerClass,
            System.Int64 ASiteKey,
            System.Int64 APartnerKey)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, APartnerKey, "", false, -1, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASiteKey"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector, String APartnerClass, System.Int64 ASiteKey)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, ASiteKey, -1, "", false, -1, -1, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="APartnerEditUIConnector"></param>
        /// <param name="APartnerClass"></param>
        public void SetParameters(IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector, String APartnerClass)
        {
            SetParameters(APartnerEditUIConnector, APartnerClass, -1, -1, "", false, -1, -1, -1);
        }

        #region Setup SourceDataGrid
        private void SetupDataGridDataBinding()
        {
            DataView InstalledSitesDV;
            Int32 BestSiteRowNumber;

            InstalledSitesDV = FInstalledSitesDT.DefaultView;
            InstalledSitesDV.AllowNew = false;
            InstalledSitesDV.AllowDelete = false;
            InstalledSitesDV.AllowEdit = false;

            // InstalledSitesDV.Sort := 'p_partner_short_name_c ASC';   for testing purposes only
            // DataBind the DataGrid
            grdInstalledSites.DataSource = new DevAge.ComponentModel.BoundDataView(InstalledSitesDV);

            // Hook up event that fires when a different Row is selected
            grdInstalledSites.Selection.FocusRowEntered += new RowEventHandler(this.DataGrid_FocusRowEntered);

            // Determine the Row that contains 'Best Address' > this Row should be initially selected
            FLogic.DetermineInitiallySelectedSite((grdInstalledSites.DataSource as DevAge.ComponentModel.BoundDataView).mDataView,
                FSiteKey,
                out BestSiteRowNumber,
                out FSiteKey);

            // MessageBox.Show('BestSiteRowNumber: ' + BestSiteRowNumber.ToString +
            // #13#10'BestSiteKey: ' + FSiteKey.ToString);

            // Select Row that contains 'Best Site'
            grdInstalledSites.Selection.SelectRow(BestSiteRowNumber, true);
        }

        #endregion

        /// <summary>
        /// implemented for IFrmPetraEdit
        /// </summary>
        /// <returns></returns>
        public bool CanClose()
        {
            return FPetraUtilsObject.CanClose();
        }

        /// <summary>
        /// implemented for IFrmPetra
        /// </summary>
        public void RunOnceOnActivation()
        {
        }
    }
}