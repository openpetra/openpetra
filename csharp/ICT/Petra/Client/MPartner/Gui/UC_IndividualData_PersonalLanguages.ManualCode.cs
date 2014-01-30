//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;
using Ict.Petra.Shared.MPersonnel.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_IndividualData_PersonalLanguages
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private PLanguageTable FLanguageCodeDT;
        private PtLanguageLevelTable FLanguageLevelDT;

        #region Properties

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        #endregion

        #region Events

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #endregion

        /// <summary>
        /// todoComment
        /// </summary>
        public void SpecialInitUserControl(IndividualDataTDS AMainDS)
        {
            FMainDS = AMainDS;

            LoadDataOnDemand();

            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Language", FMainDS.PmPersonLanguage.ColumnLanguageDescription);
            grdDetails.AddTextColumn("Language Level", FMainDS.PmPersonLanguage.ColumnLanguageLevel);
            grdDetails.AddTextColumn("Years Of Experience", FMainDS.PmPersonLanguage.ColumnYearsOfExperience);
            grdDetails.AddDateColumn("as of", FMainDS.PmPersonLanguage.ColumnYearsOfExperienceAsOf);

            FLanguageCodeDT = (PLanguageTable)TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.LanguageCodeList);

            // make sure action can be taken when data is saved successfully
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
        }

        /// <summary>
        /// Gets the data from all controls on this UserControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls2()
        {
            // Get data out of the Controls only if there is at least one row of data (Note: Column Headers count as one row)
            if (grdDetails.Rows.Count > 1)
            {
                GetDataFromControls();
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecord(System.Object sender, EventArgs e)
        {
            if (this.CreateNewPmPersonLanguage())
            {
                cmbLanguageCode.Focus();
            }
        }

        private void NewRowManual(ref IndividualDataTDSPmPersonLanguageRow ARow)
        {
            string newName;
            Int32 countNewDetail = 0;

            ARow.PartnerKey = FMainDS.PPerson[0].PartnerKey;
            newName = FLanguageCodeDT[0].LanguageCode;

            if (FMainDS.PmPersonLanguage.Rows.Find(new object[] { ARow.PartnerKey, newName }) != null)
            {
                while (FMainDS.PmPersonLanguage.Rows.Find(new object[] { ARow.PartnerKey, newName }) != null)
                {
                    countNewDetail++;
                    newName = FLanguageCodeDT[countNewDetail].LanguageCode;
                }
            }

            ARow.LanguageCode = newName;
        }

        private void GetDetailDataFromControlsManual(IndividualDataTDSPmPersonLanguageRow ARow)
        {
            // update language description in grid from language code used in details part
            ARow.LanguageDescription = cmbLanguageCode.GetSelectedDescription();
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(PmPersonLanguageRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                DoRecalculateScreenParts();
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        private void ShowDetailsManual(PmPersonLanguageRow ARow)
        {
            // In theory, the next Method call could be done in Methods NewRowManual; however, NewRowManual runs before
            // the Row is actually added and this would result in the Count to be one too less, so we do the Method call here, short
            // of a non-existing 'AfterNewRowManual' Method....
            DoRecalculateScreenParts();
        }

        private void ShowLanguageLevelExplanation(System.Object sender, EventArgs e)
        {
            PtLanguageLevelRow LangLevelDR;

            if (FLanguageLevelDT == null)
            {
                FLanguageLevelDT = (PtLanguageLevelTable)TDataCache.TMPersonnel.GetCacheablePersonnelTable(
                    TCacheablePersonTablesEnum.LanguageLevelList);
            }

            LangLevelDR = (PtLanguageLevelRow)FLanguageLevelDT.Rows.Find(new object[] { Convert.ToInt32(cmbLanguageLevel.cmbCombobox.SelectedValue) });

            if (LangLevelDR != null)
            {
                MessageBox.Show(LangLevelDR.LanguageLevelDescr.Trim() + ":" + Environment.NewLine + LangLevelDR.LanguageComment,
                    Catalog.GetString("Language Level Explanation"));
            }
            else
            {
                MessageBox.Show(String.Format(Catalog.GetString("There is no explanation available for Language Level {0}."), cmbLanguageLevel.Text));
            }
        }

        /// <summary>
        /// Loads Person Language Data from Petra Server into FMainDS, if not already loaded.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PmPersonLanguage == null)
                {
                    FMainDS.Tables.Add(new PmPersonLanguageTable());
                    FMainDS.InitVars();
                }

                if (TClientSettings.DelayedDataLoading
                    && (FMainDS.PmPersonLanguage.Rows.Count == 0))
                {
                    FMainDS.Merge(FPartnerEditUIConnector.GetDataPersonnelIndividualData(TIndividualDataItemEnum.idiPersonalLanguages));

                    // Make DataRows unchanged
                    if (FMainDS.PmPersonLanguage.Rows.Count > 0)
                    {
                        if (FMainDS.PmPersonLanguage.Rows[0].RowState != DataRowState.Added)
                        {
                            FMainDS.PmPersonLanguage.AcceptChanges();
                        }
                    }
                }

                if (FMainDS.PmPersonLanguage.Rows.Count != 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }

            return ReturnValue;
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            if (!FPetraUtilsObject.HasChanges)
            {
                // only set to read only if saving of data was successful
                SetPrimaryKeyReadOnly(true);
            }
        }
        
        private void ValidateDataDetailsManual(PmPersonLanguageRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPersonnelValidation_Personnel.ValidatePersonalLanguageManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }
    }
}