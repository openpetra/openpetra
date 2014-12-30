//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//       ChristianK
//
// Copyright 2004-2014 by OM International
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
using System.Xml;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using System.Collections.Generic;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmPartnerImport
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public bool SaveChanges()
        {
            // TODO
            return false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void FileSave(System.Object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void InitializeManualCode()
        {
            chkSemiAutomatic.Checked = false;
            grdMatchingRecords.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMatchingRecordSelChange);
        }

        PartnerImportExportTDS FMainDS = null;
        PPartnerRow FCurrentPartner;
        Int32 FCurrentNumberOfRecord = 0;
        Int32 FTotalNumberOfRecords = 0;
        Int64 FoundPartnerMatchingKey = -1;
        Int64 ExistingPartnerKey = -1;
        int UserSelLocationKey = -1;
        PartnerFindTDSSearchResultRow UserSelectedRow;
        List <Int64>FImportedUnits = new List <Int64>();
        Calculations.TOverallContactSettings FPartnersOverallContactSettings;

        private void AddStatus(String ANewStuff)
        {
            if (txtPartnerInfo.InvokeRequired)
            {
                txtPartnerInfo.Invoke(new MethodInvoker(delegate { AddStatus(ANewStuff); }));
                return;
            }

            txtPartnerInfo.AppendText(ANewStuff);
            txtPartnerInfo.SelectionStart = txtPartnerInfo.Text.Length;
            txtPartnerInfo.ScrollToCaret();
        }

        private String FormatVerificationResult(String ATitle, TVerificationResultCollection AResults)
        {
            String VerifyContext = "";
            String Res = ATitle;

            if (Res != "")
            {
                Res += Environment.NewLine;
            }

            foreach (TVerificationResult verif in AResults)
            {
                if (verif.ResultContext.ToString() != VerifyContext)
                {
                    Res += (verif.ResultContext.ToString() + ":" + Environment.NewLine);
                    VerifyContext = verif.ResultContext.ToString();
                }

                Res += ("    " + verif.ResultText.ToString() + Environment.NewLine);
            }

            Res += ("_______________________________________________" + Environment.NewLine);
            return Res;
        }

        /// <summary>
        /// Display File open dialog box and read file into memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile(System.Object sender, EventArgs e)
        {
            if (!FPetraUtilsObject.IsEnabled("actStartImport"))
            {
                MessageBox.Show(Catalog.GetString("Please cancel the current import before selecting a different file"));
                return;
            }

            txtPartnerInfo.Text = "";
            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter =
                Catalog.GetString(
                    "All supported formats|*.yml;*.csv;*.ext|Text file (*.yml)|*.yml|Partner Extract (*.ext)|*.ext|Partner List (*.csv)|.csv");
            DialogOpen.FilterIndex = 1;

            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = Catalog.GetString("Select the file for importing partners");

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                txtFilename.Text = DialogOpen.FileName;

                TVerificationResultCollection VerificationResult = null;

                AddStatus(String.Format("Reading {0}\r\n", Path.GetFileName(DialogOpen.FileName)));

                if (Path.GetExtension(DialogOpen.FileName) == ".yml")
                {
                    TYml2Xml yml = new TYml2Xml(DialogOpen.FileName);
                    AddStatus(Catalog.GetString("Parsing file. Please wait...\r\n"));
                    FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportPartnersFromYml(TXMLParser.XmlToString(
                            yml.ParseYML2XML()), out VerificationResult);
                }

                if (Path.GetExtension(DialogOpen.FileName) == ".csv")
                {
                    // select separator, make sure there is a header line with the column captions/names
                    TDlgSelectCSVSeparator dlgSeparator = new TDlgSelectCSVSeparator(true);
                    Boolean fileCanOpen = dlgSeparator.OpenCsvFile(DialogOpen.FileName);

                    if (!fileCanOpen)
                    {
                        MessageBox.Show(Catalog.GetString("Unable to open file."),
                            Catalog.GetString("Import Partners"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        return;
                    }

                    if (dlgSeparator.ShowDialog() == DialogResult.OK)
                    {
                        XmlDocument doc = TCsv2Xml.ParseCSV2Xml(DialogOpen.FileName, dlgSeparator.SelectedSeparator);
                        FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportFromCSVFile(TXMLParser.XmlToString(doc), out VerificationResult);
                    }
                    else
                    {
                        AddStatus(String.Format("\r\nImport of file {0} cancelled!\r\n", Path.GetFileName(DialogOpen.FileName)));

                        return;
                    }
                }
                else if (Path.GetExtension(DialogOpen.FileName) == ".ext")
                {
                    StreamReader sr = new StreamReader(DialogOpen.FileName, true);
                    string[] FileContent = sr.ReadToEnd().Replace("\r", "").Split(new char[] { '\n' });
                    sr.Close();
                    AddStatus(String.Format("{0} lines.\r\n", FileContent.Length));
                    AddStatus(Catalog.GetString("Parsing file. Please wait...\r\n"));
                    FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportFromPartnerExtract(FileContent, out VerificationResult);
                }

                AddStatus(FormatVerificationResult("Imported file verification: ", VerificationResult));

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(VerificationResult))
                {
                    string ErrorMessages = String.Empty;

                    foreach (TVerificationResult verif in VerificationResult)
                    {
                        ErrorMessages += "[" + verif.ResultContext + "] " +
                                         verif.ResultTextCaption + ": " +
                                         verif.ResultText + Environment.NewLine;
                    }

                    MessageBox.Show(ErrorMessages, Catalog.GetString("Import of partners failed!"));

                    FMainDS = null;

                    return;
                }

                // Determine the 'Primary E-Mail Address' of all Partners that are contained in the import file
                FPartnersOverallContactSettings = Calculations.DeterminePrimaryOrWithinOrgSettings(FMainDS.PPartnerAttribute,
                    Calculations.TOverallContSettingKind.ocskPrimaryEmailAddress);

                AddStatus(String.Format(Catalog.GetString("File read OK ({0} partners) - press Start to import.\r\n"), FMainDS.PPartner.Rows.Count));
            }
        }

        private Thread FThreadAutomaticImport = null;
        bool FNeedUserFeedback = false;

        private void ThreadAutomaticImport()
        {
            while (!FNeedUserFeedback)
            {
                DisplayCurrentRecord();

                if (FNeedUserFeedback)
                {
                    break;
                }
                else
                {
                    FCurrentNumberOfRecord++;
                }
            }

            FThreadAutomaticImport = null;
        }

        private void OnMatchingRecordSelChange(Object sender, MouseEventArgs e)
        {
            String Msg = "";

            DataRowView[] UserSelectedRecord = grdMatchingRecords.SelectedDataRowsAsDataRowView;
            btnUseSelectedAddress.Enabled = false;
            btnUseSelectedPerson.Enabled = false;
            btnCreateNewPartner.Enabled = true;

            if (UserSelectedRecord.GetLength(0) > 0)
            {
                btnUseSelectedAddress.Enabled = true;
                Msg = "Use this existing partner's address";
                UserSelectedRow = (PartnerFindTDSSearchResultRow)UserSelectedRecord[0].Row;
                UserSelLocationKey = UserSelectedRow.LocationKey;

                if ((UserSelectedRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
                    && (FCurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON))
                {
                    Msg += ", add Person to this Family";
                    btnUseSelectedFamily.Enabled = true;
                }
                else
                {
                    btnUseSelectedFamily.Enabled = false;
                }

                if (UserSelectedRow.PartnerKey == FoundPartnerMatchingKey)
                {
                    btnUseSelectedPerson.Enabled = true;
                    btnUseSelectedPerson.Focus();
                    btnCreateNewPartner.Enabled = false;
                    Msg += ", or update the existing partner";
                }
            }

            txtHint.Text = Msg;
        }

        private void UseSelectedFamily(Object sender, EventArgs e)
        {
            AddStatus("<Add PERSON to existing FAMILY>" + Environment.NewLine);

            // I need to get the Person linked to this Partner, and overwrite its FamilyKey.
            FMainDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}", PPersonTable.GetPartnerKeyDBName(), FCurrentPartner.PartnerKey);

            PPersonRow PersonRow = (PPersonRow)FMainDS.PPerson.DefaultView[0].Row;  // I'm assuming this succeeds, because if it doesn't, something bad has happened!
            PersonRow.FamilyKey = UserSelectedRow.PartnerKey;
            CreateOrUpdatePartner(FCurrentPartner, true);
        }

        private void UseSelectedAddress(Object sender, EventArgs e)
        {
            AddStatus("<Create Partner with shared address>" + Environment.NewLine);

            // This expression uses the first location key in the imported data,
            // which matches the method used to create the data grid from which the
            // address was selected.
            // Only this one address will be shared - if there are also other addresses,
            // these will be created. (And checks on CommitChanges may catch them.) But in most cases it's probably useful.

            ((PPartnerLocationRow)FMainDS.PPartnerLocation.DefaultView[0].Row).LocationKey = UserSelLocationKey;
            CreateOrUpdatePartner(FCurrentPartner, true);
        }

        private void UseSelectedPerson(Object sender, EventArgs e)
        {
            AddStatus("<Update existing Partner>" + Environment.NewLine);
            ExistingPartnerKey = FoundPartnerMatchingKey;

            if (UserSelectedRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                // I need to get the Person linked to this Partner, and overwrite its FamilyKey.
                FMainDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}", PPersonTable.GetPartnerKeyDBName(), FCurrentPartner.PartnerKey);

                PPersonRow PersonRow = (PPersonRow)FMainDS.PPerson.DefaultView[0].Row; // I'm assuming this succeeds, because if it doesn't, something bad has happened!
                PersonRow.FamilyKey = UserSelectedRow.FamilyKey;
            }

            CreateOrUpdatePartner(FCurrentPartner, true);
        }

        private void StartImport(Object sender, EventArgs e)
        {
            if (FMainDS == null)
            {
                OpenFile(null, null);
            }

            if (FMainDS == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a text file containing the partners first"),
                    Catalog.GetString("Need a file to import"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            this.FPetraUtilsObject.EnableAction("actStartImport", false);
            this.FPetraUtilsObject.EnableAction("actCancelImport", true);

            FCurrentNumberOfRecord = 1;
            FTotalNumberOfRecords = FMainDS.PPartner.Count;

            pnlActions.Enabled = true;

            if (chkSemiAutomatic.Checked)
            {
                StartThread();
            }
            else
            {
                DisplayCurrentRecord();
            }
        }

        private void StartThread()
        {
            FNeedUserFeedback = false;
            // TODO: disable all buttons apart from cancel button?
            FThreadAutomaticImport = new Thread(new ThreadStart(ThreadAutomaticImport));
            FThreadAutomaticImport.Start();
        }

        private void SaveLogFile()
        {
            String LogFilePath = txtFilename.Text.Replace('.', '-') + "-import.log";

            AddStatus("Saving Import log to " + LogFilePath + "\r\n");

            StreamWriter Stream = new StreamWriter(LogFilePath);
            Stream.Write(txtPartnerInfo.Text);
            Stream.Close();
        }

        private void SetControlsIdle()
        {
            if (pnlActions.InvokeRequired)
            {
                pnlActions.Invoke(new MethodInvoker(delegate { SetControlsIdle(); }));
                return;
            }

            foreach (Control ActionControl in pnlActions.Controls)
            {
                if (ActionControl.Name != "chkSemiAutomatic")
                {
                    ActionControl.Enabled = false;
                }
            }

            this.FPetraUtilsObject.EnableAction("actStartImport", true);
            this.FPetraUtilsObject.EnableAction("actCancelImport", false);
        }

        private void DisplayCurrentRecord()
        {
            string PrimaryEmailAddress;

            if ((FMainDS == null) || (FCurrentNumberOfRecord < 1))
            {
                return;
            }

            if (grdMatchingRecords.InvokeRequired) // must be called from UI thread.
            {
                grdMatchingRecords.Invoke(new MethodInvoker(delegate { DisplayCurrentRecord(); }));
                return;
            }

            // have we finished importing?
            if (FCurrentNumberOfRecord > FTotalNumberOfRecords)
            {
                AddStatus(String.Format(Catalog.GetString("{0} Records processed - Import finished.\r\n"), FTotalNumberOfRecords));
                SaveLogFile();
                SetControlsIdle();

                // finish the thread
                FNeedUserFeedback = true;

                grdMatchingRecords.DataSource = null;
                txtHint.Text = String.Empty;
                return;
            }

            do
            {
                FCurrentPartner = FMainDS.PPartner[FCurrentNumberOfRecord - 1];

                if (FImportedUnits.Contains(FCurrentPartner.PartnerKey))
                {
                    AddStatus(String.Format(Catalog.GetString("Unit [{0}] already imported.\r\n"), FCurrentPartner.PartnerKey));
                    FCurrentNumberOfRecord++;
                }
            } while (FImportedUnits.Contains(FCurrentPartner.PartnerKey));

            string PartnerInfo = String.Format("[{0}] {1} ",
                FCurrentPartner.PartnerClass,
                FCurrentPartner.PartnerShortName);

            if (FCurrentPartner.PartnerKey > 0)
            {
                PartnerInfo += FCurrentPartner.PartnerKey.ToString();
            }

            PartnerInfo += Environment.NewLine;

            FMainDS.PPartnerLocation.DefaultView.RowFilter = String.Format("{0}={1}",
                PPartnerLocationTable.GetPartnerKeyDBName(),
                FCurrentPartner.PartnerKey);

            foreach (DataRowView rv in FMainDS.PPartnerLocation.DefaultView)
            {
                PPartnerLocationRow PartnerLocationRow = (PPartnerLocationRow)rv.Row;

                FMainDS.PLocation.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                    PLocationTable.GetLocationKeyDBName(),
                    PartnerLocationRow.LocationKey,
                    PLocationTable.GetSiteKeyDBName(),
                    PartnerLocationRow.SiteKey);

                foreach (DataRowView LocationRowView in FMainDS.PLocation.DefaultView)
                {
                    PLocationRow LocationRow = (PLocationRow)LocationRowView.Row;
                    PartnerInfo += Calculations.DetermineLocationString(LocationRow, Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);
                    PartnerInfo += Environment.NewLine;


                    PartnerInfo += Environment.NewLine;
                }

                AddStatus(String.Format(Catalog.GetString("Processing record {0} of {1}:\r\n"), FCurrentNumberOfRecord, FTotalNumberOfRecords));
            }

            TLocationPK BestLocationPK = Ict.Petra.Shared.MPartner.Calculations.DetermineBestAddress(FMainDS.PPartnerLocation);
            FMainDS.PLocation.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                PLocationTable.GetLocationKeyDBName(),
                BestLocationPK.LocationKey,
                PLocationTable.GetSiteKeyDBName(),
                BestLocationPK.SiteKey);

            PLocationRow BestLocation = null;

            if (FMainDS.PLocation.DefaultView.Count > 0)
            {
                BestLocation = (PLocationRow)FMainDS.PLocation.DefaultView[0].Row;
            }

            // Display 'Primary E-Mail Address' when available
            if (FPartnersOverallContactSettings != null)
            {
                PrimaryEmailAddress = FPartnersOverallContactSettings.GetPartnersPrimaryEmailAddress(FCurrentPartner.PartnerKey);

                if (PrimaryEmailAddress != null)
                {
                    PartnerInfo += PrimaryEmailAddress + Environment.NewLine;
                }
            }

            AddStatus(PartnerInfo);

            // TODO: several tabs, displaying count of numbers of partners with same name, different cities, etc? support Address change, etc

            // get all partners with same surname in that city. using the first Plocation for the moment.
            // TODO: should use getBestAddress?
            grdMatchingRecords.Columns.Clear();
            btnUseSelectedAddress.Enabled = false;
            btnUseSelectedPerson.Enabled = false;
            btnCreateNewPartner.Enabled = true;
            btnUseSelectedFamily.Enabled = false;


            bool FoundPartnerInDatabase = false;
            bool FoundPossiblePartnersInDatabase = false;

            // Try to find an existing partner and set the partner key
            // Or if the address is found, the location record can be shared.
            if ((BestLocation != null) && (FCurrentPartner.PartnerKey < 0))
            {
                PartnerFindTDS result =
                    TRemote.MPartner.Partner.WebConnectors.FindPartners(
                        "",
                        Ict.Petra.Shared.MPartner.Calculations.FormatShortName(FCurrentPartner.PartnerShortName, eShortNameFormat.eOnlySurname),
                        BestLocation.City,
                        string.Empty);

                if (result.SearchResult.DefaultView.Count > 0)
                {
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("Class"), result.SearchResult.ColumnPartnerClass, 70);
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("Name"), result.SearchResult.ColumnPartnerShortName, 150);
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("Address"), result.SearchResult.ColumnStreetName, 200);
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("City"), result.SearchResult.ColumnCity, 100);
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("Post Code"), result.SearchResult.ColumnPostalCode, 74);
                    grdMatchingRecords.SelectionMode = SourceGrid.GridSelectionMode.Row;

                    result.SearchResult.DefaultView.AllowNew = false;

                    //
                    // For any partner class OTHER THAN Person, I only want to see matching records of the same class.
                    if (FCurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        result.SearchResult.DefaultView.RowFilter = String.Empty;
                    }
                    else
                    {
                        result.SearchResult.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                            PartnerFindTDSSearchResultTable.GetPartnerClassDBName(),
                            FCurrentPartner.PartnerClass);
                    }

                    grdMatchingRecords.DataSource = new DevAge.ComponentModel.BoundDataView(result.SearchResult.DefaultView);
                    txtHint.Text = "Create a new partner, or select an existing match for addtional options.";
                }
                else
                {
                    btnCreateNewPartner.Focus();
                    txtHint.Text = "Select \"Create Partner\" to import this partner.";
                }

                FoundPartnerMatchingKey = -1;
                FoundPossiblePartnersInDatabase = result.SearchResult.Rows.Count != 0;

                // Check if the partner to import matches completely one of the search results
                foreach (PartnerFindTDSSearchResultRow row in result.SearchResult.Rows)
                {
                    if ((row.StreetName == BestLocation.StreetName)
                        && (row.City == BestLocation.City)
                        && (row.PostalCode == BestLocation.PostalCode)
                        && (row.PartnerClass == FCurrentPartner.PartnerClass)
                        && (row.PartnerShortName == FCurrentPartner.PartnerShortName))
                    {
                        FoundPartnerInDatabase = true;
                        FoundPartnerMatchingKey = row.PartnerKey;
                        break;
                    }
                }

                if (FoundPartnerInDatabase)
                {
                    // Now I want to pre-select this item...
                    int MatchRow;

                    for (MatchRow = 0; MatchRow < result.SearchResult.DefaultView.Count; MatchRow++)
                    {
                        PartnerFindTDSSearchResultRow Row = (PartnerFindTDSSearchResultRow)result.SearchResult.DefaultView[MatchRow].Row;

                        if (Row.PartnerKey == FoundPartnerMatchingKey)
                        {
                            grdMatchingRecords.SelectRowInGrid(MatchRow + 1);
                            OnMatchingRecordSelChange(null, null);
                            break;
                        }
                    }
                }
            }

            if (FThreadAutomaticImport != null)
            {
                if (FoundPartnerInDatabase)
                {
                    // TODO: if any data is different, wait for user interaction, or update data automatically?
                    // otherwise go to next partner
                    NextRecord();
                }
                else if (!FoundPossiblePartnersInDatabase)
                {
                    // Automatically create a new partner, and proceed to next partner
                    // TODO: create PERSON or FAMILY?
                    try
                    {
                        AddStatus("<Automatic import>" + Environment.NewLine);
                        CreateOrUpdatePartner(FCurrentPartner, true);
                    }
                    catch (Exception e)
                    {
                        TLogging.Log(e.Message);
                        TLogging.Log(e.StackTrace);
                        // TODO cleaner message box
                        MessageBox.Show(e.Message);
                        FNeedUserFeedback = true;
                    }
                }
                else
                {
                    FNeedUserFeedback = true;
                }
            }
            else // if "auto" is not selected, I'll stop here and wait for user input.
            {
                btnCreateNewPartner.Enabled = true;
                btnSkip.Enabled = true;
            }
        }

        private void CancelImport(Object sender, EventArgs e)
        {
            // todo: cleanly stop the thread during automatic import?
            if (FThreadAutomaticImport != null)
            {
                FNeedUserFeedback = true;
                return;
            }

            FMainDS = null;
            // TODO: store partner keys of imported partners
            this.FPetraUtilsObject.EnableAction("actStartImport", true);
            this.FPetraUtilsObject.EnableAction("actCancelImport", false);
        }

        private void NextRecord()
        {
            if (FThreadAutomaticImport != null)
            {
                // skipping is done centrally in the thread during automatic import
                return;
            }

            if (FMainDS != null)
            {
                FCurrentNumberOfRecord++;
            }

            if (chkSemiAutomatic.Checked)
            {
                StartThread();
            }
            else
            {
                DisplayCurrentRecord();
            }
        }

        private void SkipRecord(Object Sender, EventArgs e)
        {
            AddStatus("<Skip Partner>" + Environment.NewLine);
            AddStatus("_______________________________________________" + Environment.NewLine);
            NextRecord();
        }

        private void ImportRecordsByPartnerKey(DataTable ADestTable,
            DataTable ASourceTable,
            String AKeyDbName,
            Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            bool AUpdateExistingRecord)
        {
            ASourceTable.DefaultView.RowFilter = String.Format("{0}={1}", AKeyDbName, AOrigPartnerKey);

            foreach (DataRowView rv in ASourceTable.DefaultView)
            {
                if (AUpdateExistingRecord)
                {
                    rv.Row[AKeyDbName] = ANewPartnerKey;
                    rv.Row.AcceptChanges(); // This removes the RowState: Added
                }

                rv.Row[AKeyDbName] = ANewPartnerKey;
                ADestTable.ImportRow(rv.Row);
            }
        }

        private void ImportRecordsByPartnerKey(DataTable ADestTable,
            DataTable ASourceTable,
            String AKeyDbName,
            Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey)
        {
            ImportRecordsByPartnerKey(ADestTable, ASourceTable, AKeyDbName, AOrigPartnerKey, ANewPartnerKey, false);
        }

        private void AddAbility(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonAbility, FMainDS.PmPersonAbility,
                PmPersonAbilityTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddApplication(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmGeneralApplication, FMainDS.PmGeneralApplication,
                PmGeneralApplicationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);

            ImportRecordsByPartnerKey(ANewPartnerDS.PmShortTermApplication, FMainDS.PmShortTermApplication,
                PmShortTermApplicationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);

            ImportRecordsByPartnerKey(ANewPartnerDS.PmYearProgramApplication, FMainDS.PmYearProgramApplication,
                PmYearProgramApplicationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddAddresses(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            FMainDS.PPartnerLocation.DefaultView.RowFilter = String.Format("{0}={1}",
                PPartnerLocationTable.GetPartnerKeyDBName(), AOrigPartnerKey);

            foreach (DataRowView rv in FMainDS.PPartnerLocation.DefaultView)
            {
                PPartnerLocationRow PartnerLocationRow = (PPartnerLocationRow)rv.Row;
                bool importingAlready = false;

                FMainDS.PLocation.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                    PLocationTable.GetLocationKeyDBName(),
                    PartnerLocationRow.LocationKey,
                    PLocationTable.GetSiteKeyDBName(),
                    PartnerLocationRow.SiteKey);

                if ((FMainDS.PLocation.DefaultView.Count == 0) && (PartnerLocationRow.LocationKey >= 0))
                {
                    PartnerLocationRow.PartnerKey = ANewPartnerKey;

                    // If this PartnerLocation has a real database key or points to location 0, import it anyway!
                    ANewPartnerDS.PPartnerLocation.ImportRow(PartnerLocationRow);
                }
                else
                {
                    foreach (DataRowView NewLocationRv in FMainDS.PLocation.DefaultView)
                    {
                        PLocationRow NewLocation = (PLocationRow)NewLocationRv.Row;
                        // Check address already being imported, comparing StreetName and PostalCode
                        // If I'm already importing it, I'll ignore this row.
                        // (The address may still be already in the database.)

                        foreach (DataRowView plrv in ANewPartnerDS.PLocation.DefaultView)
                        {
                            PLocationRow ExistingLocation = (PLocationRow)plrv.Row;

                            if (
                                (ExistingLocation.Locality == NewLocation.Locality)
                                && (ExistingLocation.StreetName == NewLocation.StreetName)
                                && (ExistingLocation.PostalCode == NewLocation.PostalCode)
                                )
                            {
                                importingAlready = true;
                                break;
                            }
                        }

                        if (!importingAlready) // This row is not already on my list
                        {
                            ANewPartnerDS.PLocation.ImportRow(NewLocation);
                            ANewPartnerDS.PPartnerLocation.ImportRow(PartnerLocationRow);
                            // Set the PartnerKey for the new Row
                            int NewRow = ANewPartnerDS.PPartnerLocation.Rows.Count - 1;
                            ANewPartnerDS.PPartnerLocation[NewRow].PartnerKey = ANewPartnerKey;;
                        }
                    }
                }
            }
        }

        private void AddCommentSeq(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerComment, FMainDS.PPartnerComment,
                PPartnerCommentTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddStaffData(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmStaffData, FMainDS.PmStaffData,
                PmStaffDataTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddJobAssignment(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            PmJobAssignmentRow JobAssignmentRow;

            // add all jobs that exist for any job assignments for this partner
            FMainDS.PmJobAssignment.DefaultView.RowFilter = String.Format("{0}={1}", PmJobAssignmentTable.GetPartnerKeyDBName(), AOrigPartnerKey);

            foreach (DataRowView rv in FMainDS.PmJobAssignment.DefaultView)
            {
                JobAssignmentRow = (PmJobAssignmentRow)rv.Row;

                // find the job that exists for the current job assignment for this partner
                FMainDS.UmJob.DefaultView.RowFilter = String.Format("{0}={1} AND {2}='{3}' AND {4}='{5}' AND {6}={7}",
                    UmJobTable.GetUnitKeyDBName(), JobAssignmentRow.UnitKey,
                    UmJobTable.GetPositionNameDBName(), JobAssignmentRow.PositionName,
                    UmJobTable.GetPositionScopeDBName(), JobAssignmentRow.PositionScope,
                    UmJobTable.GetJobKeyDBName(), JobAssignmentRow.JobKey);

                foreach (DataRowView rv2 in FMainDS.UmJob.DefaultView)
                {
                    ANewPartnerDS.UmJob.ImportRow(rv2.Row);
                }
            }

            ImportRecordsByPartnerKey(ANewPartnerDS.PmJobAssignment, FMainDS.PmJobAssignment,
                PmJobAssignmentTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddPmPersonLanguage(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonLanguage, FMainDS.PmPersonLanguage,
                PmPersonLanguageTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddPreviousExperience(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPastExperience, FMainDS.PmPastExperience,
                PmPastExperienceTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddPassport(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPassportDetails, FMainDS.PmPassportDetails,
                PmPassportDetailsTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddPersonalData(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonalData, FMainDS.PmPersonalData,
                PmPersonalDataTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddPersonalDocument(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmDocument, FMainDS.PmDocument,
                PmDocumentTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddProfessionalData(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonQualification, FMainDS.PmPersonQualification,
                PmPersonQualificationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddPersonalEvaluation(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonEvaluation, FMainDS.PmPersonEvaluation,
                PmPersonEvaluationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddSpecialNeeds(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmSpecialNeed, FMainDS.PmSpecialNeed,
                PmSpecialNeedTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddPartnerType(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerType, FMainDS.PPartnerType,
                PPartnerTypeTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddPartnerAttribute(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerAttribute, FMainDS.PPartnerAttribute,
                PPartnerAttributeTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddInterest(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerInterest, FMainDS.PPartnerInterest,
                PPartnerInterestTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

/*
 *      private void AddVision(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
 *      {
 *          ImportRecordsByPartnerKey(NewPartnerDS.PmPersonVision, FMainDS.PmPersonVision,
 *              PmPersonVisionTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
 *      }
 */
        private void AddGiftDestination(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerGiftDestination, FMainDS.PPartnerGiftDestination,
                PPartnerGiftDestinationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddUnitstructure(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            bool recordAlreadyExists = false;

            FMainDS.UmUnitStructure.DefaultView.RowFilter = String.Format("{0}={1}",
                UmUnitStructureTable.GetChildUnitKeyDBName(), AOrigPartnerKey);

            if (FMainDS.UmUnitStructure.DefaultView.Count > 0)
            {
                UmUnitStructureRow unitStructureRow = (UmUnitStructureRow)FMainDS.UmUnitStructure.DefaultView[0].Row;

                // only import row if it does not exist yet in the new partner DS
                if (null != ANewPartnerDS.UmUnitStructure.Rows.Find(new object[] { unitStructureRow.ParentUnitKey, unitStructureRow.ChildUnitKey }))
                {
                    recordAlreadyExists = true;
                }
            }

            if (!recordAlreadyExists)
            {
                ImportRecordsByPartnerKey(ANewPartnerDS.UmUnitStructure, FMainDS.UmUnitStructure,
                    UmUnitStructureTable.GetChildUnitKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
            }

            //
            // I need to import, or have imported, the unit that's the parent of this unit
            // otherwise the UmUnitStructure record will not save.
            ANewPartnerDS.UmUnitStructure.DefaultView.Sort = UmUnitStructureTable.GetChildUnitKeyDBName();
            Int32 RowIdx = ANewPartnerDS.UmUnitStructure.DefaultView.Find(AOrigPartnerKey);

            if (RowIdx < 0) // If I can't find the record I've just added, that's pretty bad!
            {
                return;
            }

            UmUnitStructureRow NewRow = (UmUnitStructureRow)ANewPartnerDS.UmUnitStructure.DefaultView[RowIdx].Row;
            Int64 ParentKey = NewRow.ParentUnitKey;

            if (!FImportedUnits.Contains(ParentKey))
            {
                String PartnerShortName;
                TPartnerClass PartnerClass;
                Boolean IsMergedPartner;
                Boolean UserCanAccessPartner;

                Boolean PartnerExistsInDB = TServerLookup.TMPartner.VerifyPartner(ParentKey,
                    out PartnerShortName,
                    out PartnerClass,
                    out IsMergedPartner,
                    out UserCanAccessPartner);

                if (!PartnerExistsInDB) // If this partner is not already in the database
                {
                    FMainDS.PPartner.DefaultView.RowFilter = String.Format("{0}={1}",
                        PPartnerTable.GetPartnerKeyDBName(), ParentKey);

                    //
                    // If there is no parent I can still import this UNIT,
                    // but I'll set the parent to root, and modify the description
                    //
                    if (FMainDS.PPartner.DefaultView.Count == 0)
                    {
                        NewRow.ParentUnitKey = 1000000;
                        ANewPartnerDS.PUnit.DefaultView.Sort = PUnitTable.GetPartnerKeyDBName();
                        RowIdx = ANewPartnerDS.PUnit.DefaultView.Find(AOrigPartnerKey);
                        PUnitRow UnitRow = (PUnitRow)ANewPartnerDS.PUnit.DefaultView[RowIdx].Row;
                        UnitRow.Description += String.Format("(Prev. parent {0})", ParentKey);
                    }
                    else
                    {
                        PPartnerRow ParentRow = (PPartnerRow)FMainDS.PPartner.DefaultView[0].Row;
                        AddStatus("<Import parent Unit>" + Environment.NewLine);
                        CreateOrUpdatePartner(ParentRow, false); // This is recursive!
                    }
                }
            }
        }

        private void AddBankingDetails(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            FMainDS.PPartnerBankingDetails.DefaultView.Sort = PPartnerBankingDetailsTable.GetPartnerKeyDBName();
            int indexPartnerBankingDetails = FMainDS.PPartnerBankingDetails.DefaultView.Find(AOrigPartnerKey);

            if (indexPartnerBankingDetails != -1)
            {
                PPartnerBankingDetailsRow partnerdetailsRow =
                    (PPartnerBankingDetailsRow)FMainDS.PPartnerBankingDetails.DefaultView[indexPartnerBankingDetails].Row;
                partnerdetailsRow.PartnerKey = ANewPartnerKey;
                ANewPartnerDS.PPartnerBankingDetails.ImportRow(partnerdetailsRow);

                // need to copy the associated PBankingDetails as well
                FMainDS.PBankingDetails.DefaultView.Sort = PBankingDetailsTable.GetBankingDetailsKeyDBName();
                PBankingDetailsRow OrigBankingDetailsRow =
                    (PBankingDetailsRow)FMainDS.PBankingDetails.DefaultView[
                        FMainDS.PBankingDetails.DefaultView.Find(partnerdetailsRow.BankingDetailsKey)].Row;

                PBankRow bankRow = (PBankRow)FMainDS.PBank.Rows.Find(OrigBankingDetailsRow.BankKey);

                // create the PBank record as well, if it does not exist yet
                OrigBankingDetailsRow.BankKey = TRemote.MPartner.Partner.WebConnectors.GetBankBySortCode(bankRow.BranchCode);

                ANewPartnerDS.PBankingDetails.ImportRow(OrigBankingDetailsRow);
            }
        }

        private void AddBuilding(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PcBuilding, FMainDS.PcBuilding,
                PcBuildingTable.GetVenueKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddRoom(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PcRoom, FMainDS.PcRoom,
                PcRoomTable.GetVenueKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddSubscriptions(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PSubscription, FMainDS.PSubscription,
                PSubscriptionTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);

            // I'll also import any related Publication rows.
            ANewPartnerDS.PSubscription.DefaultView.RowFilter = String.Format("{0} = {1}", PSubscriptionTable.GetPartnerKeyDBName(), ANewPartnerKey);

            foreach (DataRowView rv in ANewPartnerDS.PSubscription.DefaultView)
            {
                PSubscriptionRow Row = (PSubscriptionRow)rv.Row;
                FMainDS.PPublication.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    PPublicationTable.GetPublicationCodeDBName(), Row.PublicationCode);

                foreach (DataRowView Pubrv in FMainDS.PPublication.DefaultView)
                {
                    ANewPartnerDS.PPublication.ImportRow(Pubrv.Row);
                }
            }
        }

        private void AddContacts(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerContact, FMainDS.PPartnerContact,
                PPartnerContactTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        /// <summary>
        /// Copy this Partner, and all the data linked to it, from the large DataSet into a new one,
        /// and send it back to the server for committing.
        ///
        /// NOTE: May be called recursively to add parent records before adding children.
        /// </summary>
        /// <param name="APartnerRow">Row to import</param>
        /// <param name="StepAfterImport">Go on to next record afterwards. (Usually true)</param>
        /// <returns>Partner key of imported record (although no-one cares)</returns>
        private Int64 CreateOrUpdatePartner(PPartnerRow APartnerRow, Boolean StepAfterImport)
        {
            if ((FCurrentNumberOfRecord < 1) || (FCurrentNumberOfRecord > FTotalNumberOfRecords))
            {
                return 0;
            }

            if (FImportedUnits.Contains(APartnerRow.PartnerKey))
            {
                return APartnerRow.PartnerKey;
            }

            PartnerImportExportTDS NewPartnerDS = new PartnerImportExportTDS();

            NewPartnerDS.PPartner.ImportRow(APartnerRow);
            Int64 OrigPartnerKey = APartnerRow.PartnerKey;
            Int64 NewPartnerKey = OrigPartnerKey;
            bool UpdateExistingRecord = false;

            // If the import file had a negative PartnerKey, I need to create a new one here,
            // and use it on all the dependent tables.

            if (OrigPartnerKey < 0)
            {
                if (ExistingPartnerKey > 0)  // This ExistingPartnerKey has been set by the UseSelectedPerson button.
                {
                    NewPartnerKey = ExistingPartnerKey;
                    UpdateExistingRecord = true;
                    ExistingPartnerKey = -1; // Don't use this next time!
                }
                else
                {
                    NewPartnerKey = TRemote.MPartner.Partner.WebConnectors.NewPartnerKey(-1);
                }
            }

            if (UpdateExistingRecord)
            {
                NewPartnerDS.PPartner[0].PartnerKey = UserSelectedRow.PartnerKey;
                NewPartnerDS.PPartner[0].DateCreated = UserSelectedRow.DateCreated;
                NewPartnerDS.PPartner[0].CreatedBy = UserSelectedRow.CreatedBy;
                NewPartnerDS.PPartner[0].ModificationId = UserSelectedRow.ModificationId;

                NewPartnerDS.PPartner[0].AcceptChanges(); // This should reset the RowState, allowing me to Update rather than Add
            }

            NewPartnerDS.PPartner[0].PartnerKey = NewPartnerKey;

            if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_CHURCH)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PChurch, FMainDS.PChurch,
                    PChurchTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                //
                // Before I change the PartnerKey on this PFamily, I need to update any PPerson records that are linked to it,
                // so that the links will still work...
                FMainDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}", PartnerEditTDSPPersonTable.GetFamilyKeyDBName(), OrigPartnerKey);

                foreach (DataRowView rv in FMainDS.PPerson.DefaultView)
                {
                    PPersonRow RelatedPerson = (PPersonRow)rv.Row;
                    RelatedPerson.FamilyKey = NewPartnerKey;
                }

                ImportRecordsByPartnerKey(NewPartnerDS.PFamily, FMainDS.PFamily,
                    PFamilyTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PPerson, FMainDS.PPerson,
                    PPersonTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
                NewPartnerDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}", PPersonTable.GetPartnerKeyDBName(), NewPartnerKey);
                Int64 RelatedFamilyKey = ((PPersonRow)NewPartnerDS.PPerson.DefaultView[0].Row).FamilyKey;

                // If there's an associated PFamily record that I've not imported yet, I could try to do that now,
                // but it's problematic because it might end up getting imported twice.
                // Anyway, I should not come to here because the family should have been imported first.
                if (RelatedFamilyKey < 0) // There's a related family that's not been imported
                {
                    AddStatus("Import Problem: PPerson record with no related PFamily.");
                    return 0;
                }
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.POrganisation, FMainDS.POrganisation,
                    POrganisationTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PUnit, FMainDS.PUnit,
                    PUnitTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);

/*
 *  // I'm doing this later, in AddUnitstructure
 *              ImportRecordsByPartnerKey(NewPartnerDS.UmUnitStructure, FMainDS.UmUnitStructure,
 *                  UmUnitStructureTable.GetChildUnitKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
 */
                FImportedUnits.Add(NewPartnerKey);

/*
 * // I don't need this at all...
 *              foreach (UmUnitStructureRow UnitStructureRow in NewPartnerDS.UmUnitStructure.Rows)
 *              {
 *                  UnitStructureRow.ChildUnitKey = NewPartnerKey;
 *              }
 */
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_VENUE)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PVenue, FMainDS.PVenue,
                    PVenueTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PBank, FMainDS.PBank,
                    PBankTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
            }

            // Add special types etc
            AddAbility(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddApplication(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddAddresses(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddCommentSeq(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddStaffData(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddJobAssignment(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddPmPersonLanguage(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddPreviousExperience(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddPassport(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddPersonalDocument(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddPersonalData(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddProfessionalData(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddPersonalEvaluation(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddSpecialNeeds(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddPartnerType(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);

            foreach (PPartnerTypeRow PartnerTypeRow in NewPartnerDS.PPartnerType.Rows)
            {
                PartnerTypeRow.PartnerKey = NewPartnerKey;
            }

            AddPartnerAttribute(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddInterest(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
//          AddVision(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddGiftDestination(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);

            AddUnitstructure(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddBuilding(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddRoom(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddSubscriptions(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddContacts(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);

            AddBankingDetails(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);


            TVerificationResultCollection VerificationResult;
            bool CommitRes = TRemote.MPartner.ImportExport.WebConnectors.CommitChanges(NewPartnerDS, out VerificationResult);
            AddStatus(FormatVerificationResult("Save Partner: ", VerificationResult));

            if (!CommitRes)
            {
                String ResultString = "";

                foreach (TVerificationResult row in VerificationResult)
                {
                    if (row.ResultSeverity == TResultSeverity.Resv_Critical)
                    {
                        ResultString += row.ResultContext.ToString() + Environment.NewLine + "    " + row.ResultText + Environment.NewLine;
                    }
                }

                MessageBox.Show(ResultString, Catalog.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // new record has been created, now load the next record
                if (StepAfterImport)
                {
                    NextRecord();
                }
            }

            return NewPartnerKey;
        }

        private void CreateNewPartner(Object sender, EventArgs e)
        {
            if ((FCurrentNumberOfRecord < 1) || (FCurrentNumberOfRecord > FTotalNumberOfRecords))
            {
                MessageBox.Show(Catalog.GetString("Select 'Start Import' from toolbar."));
                return;
            }

            AddStatus("<Create New Partner>\r\n");
            CreateOrUpdatePartner(FCurrentPartner, true);
        }
    }
}