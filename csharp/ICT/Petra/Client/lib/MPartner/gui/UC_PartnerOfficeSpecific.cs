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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.MCommon;
using System.Globalization;
using Ict.Petra.Shared.MCommon;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// UserControl for editing Office Specific Data for a Partner.
    public class TUC_PartnerOfficeSpecific : System.Windows.Forms.UserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlPartnerDetailsPerson;
        private TbtnCreated btnCreatedOfficeSpecific;
        private TUCOfficeSpecificDataLabels ucoOfficeSpecificDataLabels;
        protected PartnerEditTDS FMainDS;
        protected TDelegateGetPartnerShortName FDelegateGetPartnerShortName;
        public PartnerEditTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }

        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerOfficeSpecific));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerDetailsPerson = new System.Windows.Forms.Panel();
            this.ucoOfficeSpecificDataLabels = new Ict.Petra.Client.MCommon.TUCOfficeSpecificDataLabels();
            this.btnCreatedOfficeSpecific = new Ict.Common.Controls.TbtnCreated();
            this.pnlPartnerDetailsPerson.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsPerson
            //
            this.pnlPartnerDetailsPerson.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPartnerDetailsPerson.AutoScroll = true;
            this.pnlPartnerDetailsPerson.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsPerson.Controls.Add(this.ucoOfficeSpecificDataLabels);
            this.pnlPartnerDetailsPerson.Controls.Add(this.btnCreatedOfficeSpecific);
            this.pnlPartnerDetailsPerson.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsPerson.Name = "pnlPartnerDetailsPerson";
            this.pnlPartnerDetailsPerson.Size = new System.Drawing.Size(634, 530);
            this.pnlPartnerDetailsPerson.TabIndex = 0;

            //
            // ucoOfficeSpecificDataLabels
            //
            this.ucoOfficeSpecificDataLabels.Font = new System.Drawing.Font("Verda" + "na",
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.ucoOfficeSpecificDataLabels.Location = new System.Drawing.Point(32, 16);
            this.ucoOfficeSpecificDataLabels.Name = "ucoOfficeSpecificDataLabels";
            this.ucoOfficeSpecificDataLabels.Size = new System.Drawing.Size(526, 286);
            this.ucoOfficeSpecificDataLabels.TabIndex = 26;

            //
            // btnCreatedOfficeSpecific
            //
            this.btnCreatedOfficeSpecific.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedOfficeSpecific.CreatedBy = null;
            this.btnCreatedOfficeSpecific.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedOfficeSpecific.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedOfficeSpecific.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedOfficeSpecific.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedOfficeSpecific.Image")));
            this.btnCreatedOfficeSpecific.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedOfficeSpecific.ModifiedBy = null;
            this.btnCreatedOfficeSpecific.Name = "btnCreatedOfficeSpecific";
            this.btnCreatedOfficeSpecific.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedOfficeSpecific.TabIndex = 25;
            this.btnCreatedOfficeSpecific.Tag = "dontdisable";

            //
            // TUC_PartnerOfficeSpecific
            //
            this.Controls.Add(this.pnlPartnerDetailsPerson);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerOfficeSpecific";
            this.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsPerson.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public TUC_PartnerOfficeSpecific() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

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
        /// Initialises Delegate Function to retrieve a partner key
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDelegateGetPartnerShortName(TDelegateGetPartnerShortName ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            FDelegateGetPartnerShortName = ADelegateFunction;
        }

        public void InitialiseUserControl()
        {
            // MessageBox.Show('TUC_PartnerOfficeSpecific.InitialiseUserControl called');
            // Make sure that Typed DataTables are already there at Client side
            if (FMainDS.PDataLabelValuePartner == null)
            {
                FMainDS.Tables.Add(new PDataLabelValuePartnerTable());
                FMainDS.InitVars();
            }

            // set the delegate function for retrieving the partner short name
            ucoOfficeSpecificDataLabels.InitializeDelegateGetPartnerShortName(@GetPartnerShortName);

            // pass the StatusBarProvider through to the subUserControl to make the StatusBar Texts work.
            ucoOfficeSpecificDataLabels.StatusBarTextProvider.InstanceStatusBar = stbUCPartnerOfficeSpecific.InstanceStatusBar;
            stbUCPartnerOfficeSpecific = ucoOfficeSpecificDataLabels.StatusBarTextProvider;
            ucoOfficeSpecificDataLabels.PerformDataBinding(FMainDS.PDataLabelValuePartner, FMainDS.PPartner[0].PartnerKey,
                MCommonTypes.PartnerClassStringToOfficeSpecificDataLabelUseEnum(FMainDS.PPartner[0].PartnerClass));
            OnHookupDataChange(new THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum.petpOfficeSpecific));

            btnCreatedOfficeSpecific.UpdateFields(FMainDS.PDataLabelValuePartner);
        }

        public void DataSavedEventFired(Boolean ASuccess)
        {
            ucoOfficeSpecificDataLabels.DataSavedEventFired(ASuccess);
        }

        private Boolean GetPartnerShortName(Int64 APartnerKey,
            out String APartnerShortName,
            out TPartnerClass APartnerClass)
        {
            Boolean ReturnValue = false;

            APartnerShortName = "";
            APartnerClass = TPartnerClass.UNIT;

            if (FDelegateGetPartnerShortName != null)
            {
                try
                {
                    ReturnValue = this.FDelegateGetPartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass);
                }
                catch (Exception)
                {
                    throw new ApplicationException("this.FDelegateGetPartnerShortName could not be called!");
                }
            }
            else
            {
#if DEBUGMODE
                MessageBox.Show("TUC_PartnerOfficeSpecific.GetPartnerShortName: FDelegateGetPartnerShortName not assigned!");
#endif
            }

            return ReturnValue;
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        public void SaveAllChanges()
        {
            IFrmPetraEdit myIPetraEdit;

            System.Windows.Forms.Form frm;
            frm = this.ParentForm;
            myIPetraEdit = (IFrmPetraEdit)frm;

            if (myIPetraEdit != null)
            {
                myIPetraEdit.EnableSaveButton();
            }

            ucoOfficeSpecificDataLabels.SaveAllChanges();
        }
    }
}