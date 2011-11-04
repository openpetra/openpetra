//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

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
        PPartnerRow CurrentPartner;
        Int32 FCurrentNumberOfRecord = 0;
        Int32 FTotalNumberOfRecords = 0;
        Int64 FoundPartnerMatchingKey = -1;
        Int64 ExistingPartnerKey = -1;
        int UserSelLocationKey = -1;
        PartnerFindTDSSearchResultRow UserSelectedRow;

        private void AddStatus(String NewStuff)
        {
            if (txtPartnerInfo.InvokeRequired)
            {
                txtPartnerInfo.Invoke(new MethodInvoker(delegate { AddStatus(NewStuff); }));
                return;
            }
            txtPartnerInfo.AppendText(NewStuff);
            txtPartnerInfo.SelectionStart = txtPartnerInfo.Text.Length;
            txtPartnerInfo.ScrollToCaret();
        }

        private String FormatVerificationResult(String Title, TVerificationResultCollection Results)
        {
            String VerifyContext = "";
            String Res = Title;
            if (Res != "")
                Res += Environment.NewLine;

            foreach (TVerificationResult verif in Results)
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
                    FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportPartnersFromYml(TXMLParser.XmlToString(
                            yml.ParseYML2XML()), out VerificationResult);
                }

                if (Path.GetExtension(DialogOpen.FileName) == ".csv")
                {
                    // select separator, make sure there is a header line with the column captions/names
                    TDlgSelectCSVSeparator dlgSeparator = new TDlgSelectCSVSeparator(true);
                    dlgSeparator.CSVFileName = DialogOpen.FileName;

                    if (dlgSeparator.ShowDialog() == DialogResult.OK)
                    {
                        XmlDocument doc = TCsv2Xml.ParseCSV2Xml(DialogOpen.FileName, dlgSeparator.SelectedSeparator);
                        FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportFromCSVFile(TXMLParser.XmlToString(doc), out VerificationResult);
                    }
                }
                else if (Path.GetExtension(DialogOpen.FileName) == ".ext")
                {
                    StreamReader sr = new StreamReader(DialogOpen.FileName, true);
                    string[] FileContent = sr.ReadToEnd().Replace("\r", "").Split(new char[] { '\n' });
                    sr.Close();
                    AddStatus (String.Format("{0} lines.\r\n", FileContent.Length));
                    FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportFromPartnerExtract(FileContent, out VerificationResult);
                }
                AddStatus(FormatVerificationResult("Imported file verification: ", VerificationResult));

                if ((VerificationResult != null) && VerificationResult.HasCriticalError())
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

//              TRemote.MPartner.ImportExport.WebConnectors.CommitChanges (FMainDS,  out VerificationResult);
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
                if ((UserSelectedRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY) && (CurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON))
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
            FMainDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}", PPersonTable.GetPartnerKeyDBName(), CurrentPartner.PartnerKey);

            PPersonRow PersonRow = (PPersonRow) FMainDS.PPerson.DefaultView[0].Row; // I'm assuming this succeeds, because if it doesn't, something bad has happened!
            PersonRow.FamilyKey = UserSelectedRow.PartnerKey;
            CreateOrUpdatePartner(CurrentPartner);
        }

        private void UseSelectedAddress(Object sender, EventArgs e)
        {
            AddStatus("<Create Partner with shared address>"+Environment.NewLine);

            // This expression uses the first location key in the imported data,
            // which matches the method used to create the data grid from which the
            // address was selected. 
            // Only this one address will be shared - if there are also other addresses,
            // these will be created. (And checks on CommitChanges may catch them.) But in most cases it's probably useful.

            ((PPartnerLocationRow) FMainDS.PPartnerLocation.DefaultView[0].Row).LocationKey = UserSelLocationKey;
            CreateOrUpdatePartner(CurrentPartner);
        }

        private void UseSelectedPerson(Object sender, EventArgs e)
        {
            AddStatus("<Update existing Partner>" + Environment.NewLine);
            ExistingPartnerKey = FoundPartnerMatchingKey;

            if (UserSelectedRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                // I need to get the Person linked to this Partner, and overwrite its FamilyKey.
                FMainDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}", PPersonTable.GetPartnerKeyDBName(), CurrentPartner.PartnerKey);

                PPersonRow PersonRow = (PPersonRow)FMainDS.PPerson.DefaultView[0].Row; // I'm assuming this succeeds, because if it doesn't, something bad has happened!
                PersonRow.FamilyKey = UserSelectedRow.FamilyKey;
            }
            CreateOrUpdatePartner(CurrentPartner);
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

            SkipImportedPartners();

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

        private void SaveLogFile ()
            
        {
            String LogFilePath = txtFilename.Text.Replace('.','-') + "-import.log";
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

            pnlActions.Enabled = false;
            this.FPetraUtilsObject.EnableAction("actStartImport", true);
            this.FPetraUtilsObject.EnableAction("actCancelImport", false);
        }

        private void DisplayCurrentRecord()
        {
            if ((FMainDS == null) || (FCurrentNumberOfRecord < 1))
            {
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

            CurrentPartner = FMainDS.PPartner[FCurrentNumberOfRecord - 1];

            string PartnerInfo = String.Format ("[{0}] {1} ",
                CurrentPartner.PartnerClass,
                CurrentPartner.PartnerShortName);

            if (CurrentPartner.PartnerKey > 0)
            {
                PartnerInfo += CurrentPartner.PartnerKey.ToString();
            }

            PartnerInfo += Environment.NewLine;

            FMainDS.PPartnerLocation.DefaultView.RowFilter = String.Format("{0}={1}",
                PPartnerLocationTable.GetPartnerKeyDBName(),
                CurrentPartner.PartnerKey);

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
                    if (PartnerLocationRow.EmailAddress != "")
                    {
                        PartnerInfo += PartnerLocationRow.EmailAddress + Environment.NewLine;
                    }

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

            AddStatus (PartnerInfo);

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
            if ((BestLocation != null) && (CurrentPartner.PartnerKey < 0))
            {
                PartnerFindTDS result =
                    TRemote.MPartner.Partner.WebConnectors.FindPartners(
                        "",
                        Ict.Petra.Shared.MPartner.Calculations.FormatShortName(CurrentPartner.PartnerShortName, eShortNameFormat.eOnlySurname),
                        BestLocation.City,
                        new StringCollection());

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
                    if (CurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        result.SearchResult.DefaultView.RowFilter = String.Empty;
                    }
                    else
                    {
                        result.SearchResult.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                            PartnerFindTDSSearchResultTable.GetPartnerClassDBName(),
                            CurrentPartner.PartnerClass);
                    }
                    grdMatchingRecords.DataSource = new DevAge.ComponentModel.BoundDataView(result.SearchResult.DefaultView);
                    txtHint.Text = "Create a new partner, or select an existing match for addtional options.";
                }
                else
                {
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
                        && (row.PartnerClass == CurrentPartner.PartnerClass)
                        && (row.PartnerShortName == CurrentPartner.PartnerShortName))
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
                        CreateOrUpdatePartner(CurrentPartner);
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

        /// <summary>
        /// check for hash values etc to see if the partner has been imported already.
        /// modify FCurrentNumberOfRecord to move to next partner that should be imported
        /// </summary>
        private void SkipImportedPartners()
        {
            // TODO check for import settings, which partners to skip etc
            // TODO: CurrentNumberOfRecord and TotalRecords different?

            // TODO modify FCurrentNumberOfRecord
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

                SkipImportedPartners();
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
            AddStatus("<Skip Partner>"+Environment.NewLine);
            AddStatus("_______________________________________________" + Environment.NewLine);
            NextRecord();
        }

        private void ImportRecordsByPartnerKey(DataTable DestTable, DataTable SourceTable, String KeyDbName, Int64 OrigPartnerKey, Int64 NewPartnerKey, bool UpdateExistingRecord)
        {
            SourceTable.DefaultView.RowFilter = String.Format("{0}={1}", KeyDbName, OrigPartnerKey);
            foreach (DataRowView rv in SourceTable.DefaultView)
            {
                if (UpdateExistingRecord)
                {
                    rv.Row[KeyDbName] = NewPartnerKey;
                    rv.Row.AcceptChanges(); // This removes the RowState: Added
                }

                rv.Row[KeyDbName] = NewPartnerKey;
                DestTable.ImportRow(rv.Row);
            }
       }

        private void ImportRecordsByPartnerKey(DataTable DestTable, DataTable SourceTable, String KeyDbName, Int64 OrigPartnerKey, Int64 NewPartnerKey)
        {
            ImportRecordsByPartnerKey(DestTable, SourceTable, KeyDbName, OrigPartnerKey, NewPartnerKey, false);
        }

        private void AddAbility(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
             ImportRecordsByPartnerKey(NewPartnerDS.PmPersonAbility, FMainDS.PmPersonAbility,
                PmPersonAbilityTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddApplication(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmGeneralApplication, FMainDS.PmGeneralApplication,
                PmGeneralApplicationTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);

            ImportRecordsByPartnerKey(NewPartnerDS.PmShortTermApplication, FMainDS.PmShortTermApplication,
                PmShortTermApplicationTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);

            ImportRecordsByPartnerKey(NewPartnerDS.PmYearProgramApplication, FMainDS.PmYearProgramApplication,
                PmYearProgramApplicationTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddAddresses(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS)
        {
            FMainDS.PPartnerLocation.DefaultView.RowFilter = String.Format("{0}={1}",
                PPartnerLocationTable.GetPartnerKeyDBName(), OrigPartnerKey);

            foreach (DataRowView rv in FMainDS.PPartnerLocation.DefaultView)
            {
                PPartnerLocationRow PartnerLocationRow = (PPartnerLocationRow)rv.Row;
                bool importingAlready = false;

                if (PartnerLocationRow.LocationKey != 0)
                {
                    FMainDS.PLocation.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                        PLocationTable.GetLocationKeyDBName(),
                        PartnerLocationRow.LocationKey,
                        PLocationTable.GetSiteKeyDBName(),
                        PartnerLocationRow.SiteKey);

                    if ((FMainDS.PLocation.DefaultView.Count == 0) && (PartnerLocationRow.LocationKey > 0))
                    {
                        PartnerLocationRow.PartnerKey = NewPartnerKey;
                        ANewPartnerDS.PPartnerLocation.ImportRow(PartnerLocationRow); // If this PartnerLocation has a real database key, import it anyway!
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
                                int NewRow = ANewPartnerDS.PPartnerLocation.Rows.Count -1;
                                ANewPartnerDS.PPartnerLocation[NewRow].PartnerKey = NewPartnerKey; ;
                            }
                        }
                    }
                }
            }
        }

        private void AddCommentSeq(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PPartnerComment, FMainDS.PPartnerComment,
                PPartnerCommentTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddStaffData(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmStaffData, FMainDS.PmStaffData,
                PmStaffDataTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddPmPersonLanguage(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmPersonLanguage, FMainDS.PmPersonLanguage,
                PmPersonLanguageTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddPreviousExperience(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmPastExperience, FMainDS.PmPastExperience,
                PmPastExperienceTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddPassport(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmPassportDetails, FMainDS.PmPassportDetails,
                PmPassportDetailsTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddPersonalData(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmPersonalData, FMainDS.PmPersonalData,
                PmPersonalDataTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddPersonalDocument(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmDocument, FMainDS.PmDocument,
                PmDocumentTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddProfessionalData(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmPersonQualification, FMainDS.PmPersonQualification,
                PmPersonQualificationTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddPersonalEvaluation(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmPersonEvaluation, FMainDS.PmPersonEvaluation,
                PmPersonEvaluationTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddSpecialNeeds(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmSpecialNeed, FMainDS.PmSpecialNeed,
                PmSpecialNeedTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddPartnerType(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PPartnerType, FMainDS.PPartnerType,
                PPartnerTypeTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddInterest(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PPartnerInterest, FMainDS.PPartnerInterest,
                PPartnerInterestTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddVision(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PmPersonVision, FMainDS.PmPersonVision,
                PmPersonVisionTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddUnitstructure(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.UmUnitStructure, FMainDS.UmUnitStructure,
                UmUnitStructureTable.GetChildUnitKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddBuilding(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PcBuilding, FMainDS.PcBuilding,
                PcBuildingTable.GetVenueKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddRoom(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PcRoom, FMainDS.PcRoom,
                PcRoomTable.GetVenueKeyDBName(), OrigPartnerKey, NewPartnerKey);
        }

        private void AddSubscriptions(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
        {
            ImportRecordsByPartnerKey(NewPartnerDS.PSubscription, FMainDS.PSubscription,
                PSubscriptionTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);

            // I'll also import any related Publication rows.
            NewPartnerDS.PSubscription.DefaultView.RowFilter = String.Format("{0} = {1}", PSubscriptionTable.GetPartnerKeyDBName(), NewPartnerKey);
            foreach (DataRowView rv in NewPartnerDS.PSubscription.DefaultView)
            {
                PSubscriptionRow Row = (PSubscriptionRow)rv.Row;
                FMainDS.PPublication.DefaultView.RowFilter = String.Format("{0}='{1}'", PPublicationTable.GetPublicationCodeDBName(),Row.PublicationCode);
                foreach (DataRowView Pubrv in FMainDS.PPublication.DefaultView)
                {
                    NewPartnerDS.PPublication.ImportRow(Pubrv.Row);
                }
            }
        }

        /// <summary>
        /// Copy this Partner, and all the data linked to it, from the large DataSet into a new one,
        /// and send it back to the server for committing. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Int64 CreateOrUpdatePartner(PPartnerRow PartnerRow)
        {
            if ((FCurrentNumberOfRecord < 1) || (FCurrentNumberOfRecord > FTotalNumberOfRecords))
            {
                return 0;
            }

            PartnerImportExportTDS NewPartnerDS = new PartnerImportExportTDS();

            NewPartnerDS.PPartner.ImportRow(PartnerRow);
            Int64 OrigPartnerKey = PartnerRow.PartnerKey;
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
                ImportRecordsByPartnerKey(NewPartnerDS.PChurch, FMainDS.PChurch, PChurchTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
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
                ImportRecordsByPartnerKey(NewPartnerDS.PFamily, FMainDS.PFamily, PFamilyTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PPerson, FMainDS.PPerson, PPersonTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
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
                ImportRecordsByPartnerKey(NewPartnerDS.POrganisation, FMainDS.POrganisation, POrganisationTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PUnit, FMainDS.PUnit, PUnitTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
                ImportRecordsByPartnerKey(NewPartnerDS.UmUnitStructure, FMainDS.UmUnitStructure, UmUnitStructureTable.GetChildUnitKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
                foreach (UmUnitStructureRow UnitStructureRow in NewPartnerDS.UmUnitStructure.Rows)
                {
                    UnitStructureRow.ChildUnitKey = NewPartnerKey;
                }
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_VENUE)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PVenue, FMainDS.PVenue, PVenueTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PBank, FMainDS.PBank, PBankTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
            }

            // Add special types etc
            AddAbility(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddApplication(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddAddresses(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddCommentSeq(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddStaffData(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
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

            AddInterest(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddVision(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);

            AddUnitstructure(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddBuilding(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddRoom(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);
            AddSubscriptions(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS);


            TVerificationResultCollection VerificationResult;
            bool CommitRes = TRemote.MPartner.ImportExport.WebConnectors.CommitChanges(NewPartnerDS, out VerificationResult);
            AddStatus(FormatVerificationResult("Save Partner: ", VerificationResult));

            if (!CommitRes)
            {
                String ResultString = "";
                foreach (TVerificationResult row in VerificationResult)
                {
                    if (row.ResultSeverity == TResultSeverity.Resv_Critical)
                        ResultString += row.ResultContext.ToString() + Environment.NewLine  + "    " + row.ResultText + Environment.NewLine;
                }

                MessageBox.Show(ResultString, Catalog.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // new record has been created, now load the next record
                NextRecord();
            }
            return NewPartnerKey;
        }

        private void CreateNewPartner(Object sender, EventArgs e)
        {
            AddStatus("<Create New Partner>\r\n");
            CreateOrUpdatePartner(CurrentPartner);
        }
    }
}