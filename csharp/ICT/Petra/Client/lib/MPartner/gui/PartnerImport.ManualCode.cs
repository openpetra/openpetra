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
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;

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
            pnlImportRecord.Enabled = false;
            chkSemiAutomatic.Checked = false;
        }

        PartnerImportExportTDS FMainDS = null;
        Int32 FCurrentNumberOfRecord = 0;
        Int32 FTotalNumberOfRecords = 0;
        private void OpenFile(System.Object sender, EventArgs e)
        {
            if (!FPetraUtilsObject.IsEnabled("actStartImport"))
            {
                MessageBox.Show(Catalog.GetString("Please cancel the current import before selecting a different file"));
                return;
            }

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
                    FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportFromPartnerExtract(FileContent, out VerificationResult);
                }

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

            pnlImportRecord.Enabled = true;
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

        private void DisplayCurrentRecord()
        {
            if ((FMainDS == null) || (FCurrentNumberOfRecord < 1))
            {
                return;
            }

            // have we finished importing?
            if (FCurrentNumberOfRecord > FTotalNumberOfRecords)
            {
                txtCurrentRecordStatus.Text =
                    String.Format(Catalog.GetString("{0} Records processed - Import finished"),
                        FTotalNumberOfRecords);
                pnlActions.Enabled = false;
                this.FPetraUtilsObject.EnableAction("actStartImport", true);
                this.FPetraUtilsObject.EnableAction("actCancelImport", false);

                // finish the thread
                FNeedUserFeedback = true;

                grdMatchingRecords.DataSource = null;
                pnlImportRecord.Enabled = false;
                return;
            }

            PPartnerRow CurrentPartner = FMainDS.PPartner[FCurrentNumberOfRecord - 1];

            string PartnerInfo = CurrentPartner.PartnerShortName + Environment.NewLine;
            PartnerInfo += Environment.NewLine;

            if (CurrentPartner.PartnerKey > 0)
            {
                PartnerInfo += CurrentPartner.PartnerKey.ToString() + Environment.NewLine;
            }

            FMainDS.PFamily.DefaultView.RowFilter = String.Format("{0}={1}",
                PFamilyTable.GetPartnerKeyDBName(),
                CurrentPartner.PartnerKey);

            FMainDS.PUnit.DefaultView.RowFilter = String.Format("{0}={1}",
                PUnitTable.GetPartnerKeyDBName(),
                CurrentPartner.PartnerKey);

            FMainDS.POrganisation.DefaultView.RowFilter = String.Format("{0}={1}",
                POrganisationTable.GetPartnerKeyDBName(),
                CurrentPartner.PartnerKey);

            // TODO: filter for other partner classes as well

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

                if (FMainDS.PLocation.DefaultView.Count > 0)
                {
                    PLocationRow LocationRow = (PLocationRow)FMainDS.PLocation.DefaultView[0].Row;

                    PartnerInfo += LocationRow.StreetName + Environment.NewLine;
                    PartnerInfo += LocationRow.Address3 + Environment.NewLine;
                    PartnerInfo += LocationRow.PostalCode + Environment.NewLine;
                    PartnerInfo += LocationRow.City + Environment.NewLine;
                    PartnerInfo += LocationRow.CountryCode + Environment.NewLine;
                    PartnerInfo += PartnerLocationRow.EmailAddress + Environment.NewLine;

                    PartnerInfo += Environment.NewLine;
                }

                txtCurrentRecordStatus.Text =
                    String.Format(Catalog.GetString("Processing record {0} of {1}"),
                        FCurrentNumberOfRecord,
                        FTotalNumberOfRecords);
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

            txtPartnerInfo.Text = PartnerInfo;

            // TODO: several tabs, displaying count of numbers of partners with same name, different cities, etc? support Address change, etc

            // get all partners with same surname in that city. using the first Plocation for the moment.
            // TODO should use getBestAddress?
            grdMatchingRecords.Columns.Clear();

            bool FoundPartnerInDatabase = false;
            bool FoundPossiblePartnersInDatabase = false;

            // try to find an existing partner and set the partner key
            if ((BestLocation != null) && (FMainDS.PPartner[FCurrentNumberOfRecord - 1].PartnerKey < 0))
            {
                PartnerFindTDS result =
                    TRemote.MPartner.Partner.WebConnectors.FindPartners(
                        "",
                        Ict.Petra.Shared.MPartner.Calculations.FormatShortName(CurrentPartner.PartnerShortName, eShortNameFormat.eOnlySurname),
                        BestLocation.City,
                        new StringCollection());

                grdMatchingRecords.AddTextColumn(Catalog.GetString("Class"), result.SearchResult.ColumnPartnerClass, 50);
                grdMatchingRecords.AddTextColumn(Catalog.GetString("Name"), result.SearchResult.ColumnPartnerShortName, 200);
                grdMatchingRecords.AddTextColumn(Catalog.GetString("Address"), result.SearchResult.ColumnStreetName, 200);
                grdMatchingRecords.AddTextColumn(Catalog.GetString("City"), result.SearchResult.ColumnCity, 150);
                result.SearchResult.DefaultView.AllowNew = false;
                grdMatchingRecords.DataSource = new DevAge.ComponentModel.BoundDataView(result.SearchResult.DefaultView);

                if (FThreadAutomaticImport != null)
                {
                    FoundPossiblePartnersInDatabase = result.SearchResult.Rows.Count != 0;

                    // check if the partner to import matches completely one of the search results
                    foreach (PartnerFindTDSSearchResultRow row in result.SearchResult.Rows)
                    {
                        if ((row.StreetName == BestLocation.StreetName)
                            && (row.City == BestLocation.City)
                            && (row.PartnerShortName == CurrentPartner.PartnerShortName))
                        {
                            FMainDS.PPartner[FCurrentNumberOfRecord - 1].PartnerKey = row.PartnerKey;
                            FoundPartnerInDatabase = true;
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
                    // otherwise skip to next partner
                    SkipRecord(null, null);
                }
                else if (!FoundPossiblePartnersInDatabase)
                {
                    // automatically create a new partner, and proceed to next partner
                    // TODO: create PERSON or FAMILY?
                    try
                    {
                        CreateNewPartner(null, null);
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

        private void SkipRecord(Object sender, EventArgs e)
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

            DisplayCurrentRecord();
        }

        private void AddAddresses(ref PartnerEditTDS ANewPartnerDS)
        {
            // TODO check duplicate addresses
            foreach (DataRowView rv in FMainDS.PPartnerLocation.DefaultView)
            {
                PPartnerLocationRow PartnerLocationRow = (PPartnerLocationRow)rv.Row;

                PartnerLocationRow.PartnerKey = ANewPartnerDS.PPartner[0].PartnerKey;
                ANewPartnerDS.PPartnerLocation.ImportRow(PartnerLocationRow);

                if (PartnerLocationRow.LocationKey != 0)
                {
                    FMainDS.PLocation.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                        PLocationTable.GetLocationKeyDBName(),
                        PartnerLocationRow.LocationKey,
                        PLocationTable.GetSiteKeyDBName(),
                        PartnerLocationRow.SiteKey);

                    ANewPartnerDS.PLocation.ImportRow((PLocationRow)FMainDS.PLocation.DefaultView[0].Row);
                }
            }
        }

        private void CreateNewPartner(Object sender, EventArgs e)
        {
            if ((FCurrentNumberOfRecord < 1) || (FCurrentNumberOfRecord > FTotalNumberOfRecords))
            {
                return;
            }

            PartnerEditTDS NewPartnerDS = new PartnerEditTDS();

            NewPartnerDS.PPartner.ImportRow(FMainDS.PPartner[FCurrentNumberOfRecord - 1]);

            Int64 OrigPartnerKey = FMainDS.PPartner[FCurrentNumberOfRecord - 1].PartnerKey;
            Int64 NewPartnerKey = OrigPartnerKey;

            // for UNITs we want to be able to specify the partner key in the import file
            if (OrigPartnerKey < 0)
            {
                NewPartnerKey = TRemote.MPartner.Partner.WebConnectors.NewPartnerKey(-1);
            }

            NewPartnerDS.PPartner[0].PartnerKey = NewPartnerKey;

            if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                NewPartnerDS.PFamily.ImportRow((PFamilyRow)FMainDS.PFamily.DefaultView[0].Row);
                NewPartnerDS.PFamily[0].PartnerKey = NewPartnerKey;
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
            {
                NewPartnerDS.POrganisation.ImportRow((POrganisationRow)FMainDS.POrganisation.DefaultView[0].Row);
                NewPartnerDS.POrganisation[0].PartnerKey = NewPartnerKey;
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                NewPartnerDS.PUnit.ImportRow((PUnitRow)FMainDS.PUnit.DefaultView[0].Row);
                NewPartnerDS.PUnit[0].PartnerKey = NewPartnerKey;

                FMainDS.UmUnitStructure.DefaultView.RowFilter = String.Format("{0}={1}",
                    UmUnitStructureTable.GetChildUnitKeyDBName(),
                    OrigPartnerKey);

                foreach (DataRowView rv in FMainDS.UmUnitStructure.DefaultView)
                {
                    NewPartnerDS.UmUnitStructure.ImportRow((UmUnitStructureRow)rv.Row);
                }

                foreach (UmUnitStructureRow UnitStructureRow in NewPartnerDS.UmUnitStructure.Rows)
                {
                    UnitStructureRow.ChildUnitKey = NewPartnerKey;
                }
            }

            // TODO add special types etc
            FMainDS.PPartnerType.DefaultView.RowFilter = String.Format("{0}={1}",
                PPartnerTypeTable.GetPartnerKeyDBName(),
                OrigPartnerKey);

            foreach (DataRowView rv in FMainDS.PPartnerType.DefaultView)
            {
                NewPartnerDS.PPartnerType.ImportRow((PPartnerTypeRow)rv.Row);
            }

            foreach (PPartnerTypeRow PartnerTypeRow in NewPartnerDS.PPartnerType.Rows)
            {
                PartnerTypeRow.PartnerKey = NewPartnerKey;
            }

            AddAddresses(ref NewPartnerDS);

            TVerificationResultCollection VerificationResult;

            if (!TRemote.MPartner.Partner.WebConnectors.SavePartner(NewPartnerDS, out VerificationResult))
            {
                MessageBox.Show(VerificationResult.BuildVerificationResultString(), Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                // new record has been created, now load the next record
                SkipRecord(null, null);
            }
        }
    }
}