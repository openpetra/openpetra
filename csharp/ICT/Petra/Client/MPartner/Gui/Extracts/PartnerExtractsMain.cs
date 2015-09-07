//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb, andreww
//
// Copyright 2004-2012 by OM International
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
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    /// <summary>
    /// the manually written part of TFrmPartnerMain
    /// </summary>
    public class TPartnerExtractsMain
    {
        /// <summary>
        /// open General Extract screen
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void PartnerByGeneralCriteriaExtract(Form AParentForm)
        {
            TFrmPartnerByGeneralCriteria frm = new TFrmPartnerByGeneralCriteria(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// open screen to create Publication Extract
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void PartnerBySubscriptionExtract(Form AParentForm)
        {
            TFrmPartnerBySubscription frm = new TFrmPartnerBySubscription(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// open screen to create "Partner by City" Extract
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void PartnerByCityExtract(Form AParentForm)
        {
            TFrmPartnerByCity frm = new TFrmPartnerByCity(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// open screen to create "Partner by Special Type" Extract
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void PartnerBySpecialTypeExtract(Form AParentForm)
        {
            TFrmPartnerBySpecialType frm = new TFrmPartnerBySpecialType(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// open screen to create "Partner by Relationship" Extract
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void PartnerByRelationshipExtract(Form AParentForm)
        {
            TFrmPartnerByRelationship frm = new TFrmPartnerByRelationship(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        /// <summary>
        /// prompt user for name of a new manual extract, create it and open screen for it
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void PartnerNewManualExtract(Form AParentForm)
        {
            TFrmExtractNamingDialog ExtractNameDialog = new TFrmExtractNamingDialog(AParentForm);
            int ExtractId = 0;
            string ExtractName;
            string ExtractDescription;

            ExtractNameDialog.ShowDialog();

            if (ExtractNameDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
            {
                /* Get values from the Dialog */
                ExtractNameDialog.GetReturnedParameters(out ExtractName, out ExtractDescription);
            }
            else
            {
                // dialog was cancelled, do not continue with extract generation
                return;
            }

            ExtractNameDialog.Dispose();

            // create empty extract with given name and description and store it in db
            if (TRemote.MPartner.Partner.WebConnectors.CreateEmptyExtract(ref ExtractId,
                    ExtractName, ExtractDescription))
            {
                NewExtractCreated(ExtractName, ExtractId, AParentForm);
            }
            else
            {
                MessageBox.Show(Catalog.GetString("Creation of extract failed"),
                    Catalog.GetString("Generate Manual Extract"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }
        }

        /// <summary>
        /// Guide user through process to create extract which contains all family member records (Persons)
        /// of families and persons in a base extract.
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void FamilyMembersExtract(Form AParentForm)
        {
            CreateFamilyMembersExtract(AParentForm, null);
        }

        /// <summary>
        /// Guide user through process to create extract which contains all family member records (Persons)
        /// of families and persons in a base extract.
        /// </summary>
        /// <param name="AParentForm"></param>
        /// <param name="AExtractMasterRow"></param>
        public static void CreateFamilyMembersExtract(Form AParentForm, MExtractMasterRow AExtractMasterRow)
        {
            TFrmExtractFindDialog ExtractFindDialog = new TFrmExtractFindDialog(AParentForm);
            TFrmExtractNamingDialog ExtractNameDialog = new TFrmExtractNamingDialog(AParentForm);
            int BaseExtractId = 0;
            string BaseExtractName;
            string BaseExtractDescription;
            int ExtractId = 0;
            string ExtractName;
            string ExtractDescription;

            if (AExtractMasterRow != null)
            {
                BaseExtractId = AExtractMasterRow.ExtractId;

                // inform user what this extract is about and what will happen
                if (MessageBox.Show(Catalog.GetString("A new Extract will be created that will contain all Family Members (Persons) " +
                            "of the Families that exist in the Extract '" + AExtractMasterRow.ExtractName + "'."),
                        Catalog.GetString("Generate Family Members Extract"),
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                // inform user what this extract is about and what will happen
                if (MessageBox.Show(Catalog.GetString("Please select an existing Extract with the Find Screen that follows.\r\n\r\n" +
                            "A new Extract will be created that will contain all Family Members (Persons) of the Families" +
                            " that exist in the selected Extract."),
                        Catalog.GetString("Generate Family Members Extract"),
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return;
                }

                // let the user select base extract
                ExtractFindDialog.ShowDialog(true);

                // get data for selected base extract
                ExtractFindDialog.GetResult(out BaseExtractId, out BaseExtractName, out BaseExtractDescription);
                ExtractFindDialog.Dispose();
            }

            // only continue if a base extract was selected
            if (BaseExtractId >= 0)
            {
                ExtractNameDialog.ShowDialog();

                if (ExtractNameDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                {
                    /* Get values from the Dialog */
                    ExtractNameDialog.GetReturnedParameters(out ExtractName, out ExtractDescription);
                    ExtractNameDialog.Dispose();
                }
                else
                {
                    // dialog was cancelled, do not continue with extract generation
                    ExtractNameDialog.Dispose();
                    return;
                }

                // create extract with given name and description and store it in db
                if (TRemote.MPartner.Partner.WebConnectors.CreateFamilyMembersExtract(BaseExtractId,
                        ref ExtractId, ExtractName, ExtractDescription))
                {
                    NewExtractCreated(ExtractName, ExtractId, AParentForm);
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("Creation of extract failed"),
                        Catalog.GetString("Generate Family Members Extract"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
            }
        }

        /// <summary>
        /// Guide user through process to create extract which contains all family member records (Persons)
        /// of families and persons in a base extract.
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void FamilyExtractForPersons(Form AParentForm)
        {
            CreateFamilyMembersExtract(AParentForm, null);
        }

        /// <summary>
        /// Guide user through process to create extract which contains all family records of
        /// Persons in a base extract.
        /// </summary>
        /// <param name="AParentForm"></param>
        /// <param name="AExtractMasterRow"></param>
        public static void CreateFamilyExtractForPersons(Form AParentForm, MExtractMasterRow AExtractMasterRow)
        {
            TFrmExtractFindDialog ExtractFindDialog = new TFrmExtractFindDialog(AParentForm);
            TFrmExtractNamingDialog ExtractNameDialog = new TFrmExtractNamingDialog(AParentForm);
            int BaseExtractId = 0;
            string BaseExtractName;
            string BaseExtractDescription;
            int ExtractId = 0;
            string ExtractName;
            string ExtractDescription;

            if (AExtractMasterRow != null)
            {
                BaseExtractId = AExtractMasterRow.ExtractId;

                // inform user what this extract is about and what will happen
                if (MessageBox.Show(Catalog.GetString("A new Extract will be created that will contain all Families of the Persons" +
                            " that exist in the Extract '" + AExtractMasterRow.ExtractName + "'."),
                        Catalog.GetString("Generate Family Extract for Persons"),
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                // inform user what this extract is about and what will happen
                MessageBox.Show(Catalog.GetString("Please select an existing Extract with the Find Screen that follows.\r\n\r\n" +
                        "A new Extract will be created that will contain all Families of the Persons" +
                        " that exist in the selected Extract."),
                    Catalog.GetString("Generate Family Extract for Persons"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // let the user select base extract
                ExtractFindDialog.ShowDialog(true);

                // get data for selected base extract
                ExtractFindDialog.GetResult(out BaseExtractId, out BaseExtractName, out BaseExtractDescription);
                ExtractFindDialog.Dispose();
            }

            // only continue if a base extract was selected
            if (BaseExtractId >= 0)
            {
                ExtractNameDialog.ShowDialog();

                if (ExtractNameDialog.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                {
                    /* Get values from the Dialog */
                    ExtractNameDialog.GetReturnedParameters(out ExtractName, out ExtractDescription);
                    ExtractNameDialog.Dispose();
                }
                else
                {
                    // dialog was cancelled, do not continue with extract generation
                    ExtractNameDialog.Dispose();
                    return;
                }

                // create extract with given name and description and store it in db
                if (TRemote.MPartner.Partner.WebConnectors.CreateFamilyExtractForPersons(BaseExtractId,
                        ref ExtractId, ExtractName, ExtractDescription))
                {
                    NewExtractCreated(ExtractName, ExtractId, AParentForm);
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("Creation of extract failed"),
                        Catalog.GetString("Generate Family Extract for Persons"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
            }
        }

        /// <summary>
        /// open screen to create "Partner by Contact Log" Extract
        /// </summary>
        /// <param name="AParentForm"></param>
        public static void PartnerByContactLogExtract(Form AParentForm)
        {
            TFrmPartnerByContactLog frm = new TFrmPartnerByContactLog(AParentForm);

            frm.CalledFromExtracts = true;
            frm.Show();
        }

        // once an extract has been created, this will refresh extract master screen and open maintainance screen for extract
        private static void NewExtractCreated(string AExtractName, int AExtractId, Form AParentForm)
        {
            // refresh extract master screen if it is open
            TFormsMessage BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcExtractCreated);

            BroadcastMessage.SetMessageDataName(AExtractName);
            TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);

            // now open Screen for new extract so user can add partner records manually
            TFrmExtractMaintain frm = new TFrmExtractMaintain(AParentForm);
            frm.ExtractId = AExtractId;
            frm.ExtractName = AExtractName;
            frm.Show();
        }
    }
}