//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//       ChristianK
//       PeterS
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
using System.Text;
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using System.Collections.Generic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmPartnerImport
    {
        /// <summary>The Proxy System.Object of the Serverside UIConnector for finding matching partners</summary>
        private IPartnerUIConnectorsPartnerFind FPartnerFindObject;

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
            FPartnerFindObject = TRemote.MPartner.Partner.UIConnectors.PartnerFind();

            chkSemiAutomatic.Checked = false;
            chkSemiAutomatic.Width = 126;
            FPetraUtilsObject.SetStatusBarText(chkSemiAutomatic, Catalog.GetString(
                    "Selecting ‘Automatic Import’ will import all remaining partners in the file without stopping again (unless a decision is needed)."));

            grdMatchingRecords.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMatchingRecordSelChange);
        }

        PartnerImportExportTDS FMainDS = null;
        PartnerImportExportTDS FMainDSBackup = new PartnerImportExportTDS();
        PPartnerRow FCurrentPartner;
        PPersonRow FCurrentPerson;
        String FMatchingPartnersExplanation;
        List <String>FCSVColumns = new List <String>();
        Int32 FCurrentNumberOfRecord = 0;
        Int32 FTotalNumberOfRecords = 0;
        Int64 FoundPartnerMatchingKey = -1;
        Int64 ExistingPartnerKey = -1;
        int UserSelLocationKey = -1;
        PartnerFindTDSSearchResultRow UserSelectedRow;
        List <Int64>FImportedUnits = new List <Int64>();
        Calculations.TOverallContactSettings FPartnersOverallContactSettings;
        TImportFileFormat FFileFormat = TImportFileFormat.unknown;
        string FFileName = string.Empty;
        string FFileContent = string.Empty;
        string FImportIDHeaderText = string.Empty;

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

        private void CheckAndAddToCSVColumnList(XmlDocument ADoc, String AHeaderText)
        {
            if (ADoc.NameTable.Get(AHeaderText) == AHeaderText)
            {
                FCSVColumns.Add(AHeaderText);
            }
        }

        private void FillCSVColumnList(XmlDocument ADoc)
        {
            FCSVColumns = new List <String>();
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PARTNERCLASS);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PARTNERKEY);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_FAMILYPARTNERKEY);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PERSONPARTNERKEY);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_FAMILYNAME);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_MARITALSTATUS);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_STREET);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_ADDRESS1);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_ADDRESS3);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_POSTCODE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_CITY);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_COUNTY);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_COUNTRYCODE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_AQUISITION);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_GENDER);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_LANGUAGE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_OMERFIELD);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_FIRSTNAME);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_EMAIL);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PHONE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_MOBILEPHONE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_TITLE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_DATEOFBIRTH);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_SPECIALTYPES);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_VEGETARIAN);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_MEDICALNEEDS);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_DIETARYNEEDS);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_OTHERNEEDS);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_EVENTKEY);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_ARRIVALDATE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_ARRIVALTIME);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_DEPARTUREDATE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_DEPARTURETIME);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_EVENTROLE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_APPDATE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_APPSTATUS);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_APPTYPE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PREVIOUSATTENDANCE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_CHARGEDFIELD);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_APPCOMMENTS);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_NOTES);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_NOTESFAMILY);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_CONTACTCODE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_CONTACTDATE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_CONTACTTIME);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_CONTACTOR);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_CONTACTNOTES);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_CONTACTATTR);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_CONTACTDETAIL);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PASSPORTNUMBER);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PASSPORTNAME);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PASSPORTTYPE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFBIRTH);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PASSPORTNATIONALITY);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFISSUE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PASSPORTCOUNTRYOFISSUE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFISSUE);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFEXPIRATION);
            CheckAndAddToCSVColumnList(ADoc, MPartnerConstants.PARTNERIMPORT_RECORDIMPORTED);
        }

        /// <summary>
        /// Display File open dialog box and read file into memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile(System.Object sender, EventArgs e)
        {
            if (!btnStartImport.Enabled && btnCancelImport.Enabled)
            {
                MessageBox.Show(Catalog.GetString("Please stop the current import before selecting a different file"));
                return;
            }

            txtPartnerInfo.Text = "";
            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter =
                Catalog.GetString(
                    "All supported formats|*.csv;*.ext|Partner Extract (*.ext)|*.ext|Partner List (*.csv)|*.csv");
            // For now: don't show yml file format any longer --> needs to be determined if/how helpful this is for future use
            // "All supported formats|*.yml;*.csv;*.ext|Text file (*.yml)|*.yml|Partner Extract (*.ext)|*.ext|Partner List (*.csv)|*.csv");
            DialogOpen.FilterIndex = 1;

            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = Catalog.GetString("Select the file for importing partners");

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    TVerificationResultCollection VerificationResult = null;

                    FFileName = DialogOpen.FileName;

                    switch (Path.GetExtension(FFileName).ToLower())
                    {
                        case ".ext":
                            FFileFormat = TImportFileFormat.ext;
                            break;

                        case ".csv":
                            FFileFormat = TImportFileFormat.csv;
                            break;

                        case ".yml":
                            FFileFormat = TImportFileFormat.yml;
                            break;

                        default:
                            FFileFormat = TImportFileFormat.unknown;
                            break;
                    }

                    AddStatus(String.Format("Reading {0}\r\n", Path.GetFileName(FFileName)));

                    if (FFileFormat == TImportFileFormat.yml)
                    {
                        TYml2Xml yml = new TYml2Xml(FFileName);
                        AddStatus(Catalog.GetString("Parsing file. Please wait...\r\n"));
                        FFileContent = TXMLParser.XmlToString(yml.ParseYML2XML());

                        LoadDataSet(ref VerificationResult);
                    }

                    // provisionally enable "Automatic Import"
                    chkSemiAutomatic.Enabled = true;

                    if (FFileFormat == TImportFileFormat.csv)
                    {
                        btnUseSelectedFamily.Show();
                        btnFindOtherPartner.Show();
                        chkReplaceAddress.Show();

                        // disable "Automatic Import" for .csv files
                        chkSemiAutomatic.Checked = false;
                        chkSemiAutomatic.Enabled = false;

                        // select separator, make sure there is a header line with the column captions/names
                        TDlgSelectCSVSeparator dlgSeparator = new TDlgSelectCSVSeparator(true);
                        dlgSeparator.StartPosition = FormStartPosition.CenterParent;
                        LoadUserDefaults(dlgSeparator);

                        Boolean fileCanOpen = dlgSeparator.OpenCsvFile(FFileName);

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
                            try
                            {
                                SaveUserDefaults(dlgSeparator);
                                FSelectedSeparator = dlgSeparator.SelectedSeparator;
                                XmlDocument doc = TCsv2Xml.ParseCSV2Xml(FFileName, FSelectedSeparator, Encoding.Default);
                                FFileContent = TXMLParser.XmlToString(doc);
                                GetImportIDHeaderText(doc);
                                FillCSVColumnList(doc);

                                LoadDataSet(ref VerificationResult);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, Catalog.GetString("Import Partners"));
                                AddStatus("\r\n" + ex.Message + "\r\n\r\n" +
                                    String.Format("Import of file {0} cancelled!", Path.GetFileName(FFileName)) + "\r\n");

                                return;
                            }
                        }
                        else
                        {
                            AddStatus(String.Format("\r\nImport of file {0} cancelled!\r\n", Path.GetFileName(FFileName)));

                            // We always save the defaults even if cancelled because the user usually wants to see them again
                            SaveUserDefaults(dlgSeparator);

                            return;
                        }
                    }
                    else if (FFileFormat == TImportFileFormat.ext)
                    {
                        btnUseSelectedFamily.Hide();
                        btnFindOtherPartner.Hide();
                        chkReplaceAddress.Checked = false;
                        chkReplaceAddress.Hide();

                        // Be sure the encoding imports accented characters correctly by getting the file encoding
                        Encoding fileEncoding;
                        bool hasBOM, isAmbiguous;
                        byte[] rawBytes;

                        if (TTextFile.AutoDetectTextEncodingAndOpenFile(FFileName, null, out FFileContent,
                                out fileEncoding, out hasBOM, out isAmbiguous, out rawBytes) == false)
                        {
                            AddStatus(Catalog.GetString("Failed to open the file, or the file was empty.\r\n"));
                            return;
                        }

                        int lineCount = 1;
                        int pos = -1;

                        do
                        {
                            pos = FFileContent.IndexOf('\n', ++pos);

                            if (pos >= 0)
                            {
                                lineCount++;
                            }
                        } while (pos >= 0);

                        AddStatus(String.Format("{0} lines.\r\n", lineCount));
                        AddStatus(Catalog.GetString("Parsing file. Please wait...\r\n"));

                        LoadDataSet(ref VerificationResult);
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

                    txtFilename.Text = FFileName;

                    grpStepTwo.Enabled = true;
                    btnStartImport.Enabled = true;

                    // Determine the 'Primary E-Mail Address' of all Partners that are contained in the import file
                    FPartnersOverallContactSettings = Calculations.DeterminePrimaryOrWithinOrgSettings(FMainDS.PPartnerAttribute,
                        Calculations.TOverallContSettingKind.ocskPrimaryEmailAddress);

                    AddStatus(String.Format(Catalog.GetString("File read OK ({0} partners) - press Start to import.\r\n"),
                            FMainDS.PPartner.Rows.Count));
                }
                finally
                {
                    if (FMainDS == null)
                    {
                        FMainDSBackup = new PartnerImportExportTDS();
                    }
                    else
                    {
                        FMainDSBackup.Merge(FMainDS.Copy());
                    }

                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void LoadDataSet(ref TVerificationResultCollection AVerificationResult, Int64 AOldPartnerKey = -1, Int64 ANewPartnerKey = -1)
        {
            if (FFileFormat == TImportFileFormat.yml)
            {
                if ((AOldPartnerKey != -1) && (ANewPartnerKey != -1))
                {
                    // update partner key to create a new partner with a new key
                    FFileContent = FFileContent.Replace(AOldPartnerKey.ToString("0000000000"), ANewPartnerKey.ToString("0000000000"));
                    FFileContent = FFileContent.Replace(AOldPartnerKey.ToString(), ANewPartnerKey.ToString());
                }

                FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportPartnersFromYml(FFileContent, out AVerificationResult);
            }

            if (FFileFormat == TImportFileFormat.csv)
            {
                if ((AOldPartnerKey != -1) && (ANewPartnerKey != -1))
                {
                    // update partner key to create a new partner with a new key
                    FFileContent = FFileContent.Replace(AOldPartnerKey.ToString("0000000000"), ANewPartnerKey.ToString("0000000000"));
                    FFileContent = FFileContent.Replace(AOldPartnerKey.ToString(), ANewPartnerKey.ToString());
                }

                FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportFromCSVFile(FFileContent, out AVerificationResult);
            }
            else if (FFileFormat == TImportFileFormat.ext)
            {
                if ((AOldPartnerKey != -1) && (ANewPartnerKey != -1))
                {
                    // update partner key to create a new partner with a new key
                    FFileContent = FFileContent.Replace(AOldPartnerKey.ToString("0000000000"), ANewPartnerKey.ToString("0000000000"));
                    FFileContent = FFileContent.Replace(AOldPartnerKey.ToString(), ANewPartnerKey.ToString());
                }

                string[] FileContentArray = FFileContent.Split(new char[] { '\n' });
                FMainDS = TRemote.MPartner.ImportExport.WebConnectors.ImportFromPartnerExtract(FileContentArray, out AVerificationResult);
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
            DataRowView[] UserSelectedRecord = grdMatchingRecords.SelectedDataRowsAsDataRowView;
            //btnUseSelectedAddress.Enabled = false;
            btnUseSelectedPerson.Enabled = false;
            btnCreateNewPartner.Enabled = true;

            if (UserSelectedRecord.GetLength(0) > 0)
            {
                //btnUseSelectedAddress.Enabled = true;
                UserSelectedRow = (PartnerFindTDSSearchResultRow)UserSelectedRecord[0].Row;
                UserSelLocationKey = UserSelectedRow.LocationKey;

                if (FFileFormat == TImportFileFormat.csv)
                {
                    if ((UserSelectedRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
                        && (FCurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON))
                    {
                        btnUseSelectedFamily.Enabled = true;
                    }
                    else
                    {
                        btnUseSelectedFamily.Enabled = false;
                    }

                    // if (UserSelectedRow.PartnerKey == FoundPartnerMatchingKey)
                    if (UserSelectedRow.PartnerClass == FCurrentPartner.PartnerClass)
                    {
                        btnUseSelectedPerson.Enabled = true;
                    }
                }
                else if (FFileFormat == TImportFileFormat.ext)
                {
                    btnUseSelectedPerson.Enabled = true;
                    btnCreateNewPartner.Enabled = false;
                }
            }
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

        private void UseSelectedPerson(Object sender, EventArgs e)
        {
            AddStatus("<Update existing Partner>" + Environment.NewLine);
            //ExistingPartnerKey = FoundPartnerMatchingKey; //TODOWBxxx
            ExistingPartnerKey = UserSelectedRow.PartnerKey;

            if (UserSelectedRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                // I need to get the Person linked to this Partner, and overwrite its FamilyKey.
                FMainDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}", PPersonTable.GetPartnerKeyDBName(), FCurrentPartner.PartnerKey);

                PPersonRow PersonRow = (PPersonRow)FMainDS.PPerson.DefaultView[0].Row; // I'm assuming this succeeds, because if it doesn't, something bad has happened!
                PersonRow.FamilyKey = UserSelectedRow.FamilyKey;
            }

            CreateOrUpdatePartner(FCurrentPartner, true);
        }

        private void FindOtherPartner(Object sender, EventArgs e)
        {
            System.Int64 PartnerKey = 0;
            string PartnerShortName;
            TPartnerClass? PartnerClass;
            String RestrictToPartnerClass = "";
            TLocationPK ResultLocationPK;

            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
            if (TCommonScreensForwarding.OpenPartnerFindScreen != null)
            {
                // delegate IS defined
                try
                {
                    if (FCurrentPartner != null)
                    {
                        RestrictToPartnerClass = FCurrentPartner.PartnerClass.ToString();

                        if (FCurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                        {
                            // for PERSON records we also allow FAMILY as the user can add the person to a different family
                            RestrictToPartnerClass += "," + MPartnerConstants.PARTNERCLASS_FAMILY;
                        }
                    }

                    TCommonScreensForwarding.OpenPartnerFindScreen.Invoke
                        (RestrictToPartnerClass,
                        out PartnerKey,
                        out PartnerShortName,
                        out PartnerClass,
                        out ResultLocationPK,
                        this);

                    if (PartnerKey != -1)
                    {
                        DataTable result = new DataTable();
                        PartnerFindTDSSearchCriteriaTable CriteriaTable = new PartnerFindTDSSearchCriteriaTable();
                        PartnerFindTDSSearchCriteriaRow CriteriaRow;
                        int TotalRecords;
                        short TotalPages;
                        short PageSize = 50;

                        // if partner key is given then search for exactly that Partner
                        CriteriaTable.Rows.Clear();
                        CriteriaRow = CriteriaTable.NewRowTyped();
                        CriteriaRow.ExactPartnerKeyMatch = true;
                        CriteriaRow.PartnerKey = PartnerKey;
                        CriteriaRow.PartnerClass = (PartnerClass.HasValue ? PartnerClass.ToString() : "*");
                        CriteriaRow.LocationKey = ResultLocationPK.LocationKey.ToString();
                        CriteriaTable.Rows.Add(CriteriaRow);

                        FPartnerFindObject.PerformSearch(CriteriaTable, true);
                        FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                        result = FPartnerFindObject.FilterResultByBestAddress();

                        //if (TotalRecords > 0)
                        {
                            ((DevAge.ComponentModel.BoundDataView)grdMatchingRecords.DataSource).DataTable.Merge(result);
                        }

                        // Refresh DataGrid to show the added partner record
                        grdMatchingRecords.Refresh();
                    }
                }
                catch (Exception exp)
                {
                    throw new EOPAppException("Exception occured while calling PartnerFindScreen Delegate!", exp);
                }
                // end try
            }
        }

        private void StartImport(Object sender, EventArgs e)
        {
            if (FMainDS == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a text file containing the partners first"),
                    Catalog.GetString("Need a file to import"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (FFileFormat == TImportFileFormat.csv)
            {
                // Get the Finish options by displaying the dialog.  If the user Cancels we can just return.
                if (GetFinishOptions(out FCreateCSVFile, out FIncludeFamiliesInCSV, out FCsvOutFilePath,
                        out FCreateExtract, out FExtractName, out FExtractDescription, out FIncludeFamiliesInExtract) == false)
                {
                    // User clicked Cancel
                    return;
                }
            }

            btnStartImport.Enabled = false;
            btnCancelImport.Enabled = true;

            FCurrentNumberOfRecord = 1;
            FTotalNumberOfRecords = FMainDS.PPartner.Count;

            grpStepThree.Enabled = true;

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
            // replace the final '.' with a '-'
            int Index = txtFilename.Text.LastIndexOf('.');
            string Filename = txtFilename.Text;

            if (Index != -1)
            {
                Filename = Filename.Remove(Index, 1).Insert(Index, "-");
            }

            String LogFilePath = Filename + "-import.log";

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

            btnStartImport.Enabled = true;
            btnCancelImport.Enabled = false;

            btnSkip.Enabled = false;
            btnUseSelectedPerson.Enabled = false;
            btnCreateNewPartner.Enabled = false;
            //btnUseSelectedAddress.Enabled = false;
            btnUseSelectedFamily.Enabled = false;
            btnFindOtherPartner.Enabled = false;

            grpStepThree.Enabled = false;

            grdMatchingRecords.Columns.Clear();
            grdMatchingRecords.Enabled = false;
            txtHint.Text = string.Empty;
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

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (FCurrentNumberOfRecord <= FTotalNumberOfRecords)
                {
                    do
                    {
                        FCurrentPartner = FMainDS.PPartner[FCurrentNumberOfRecord - 1];

                        if ((FCurrentPartner != null)
                            && (FCurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON))
                        {
                            // I need to get the Person linked to this Partner and set it as the current person
                            FMainDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}",
                                PPersonTable.GetPartnerKeyDBName(), FCurrentPartner.PartnerKey);
                            FCurrentPerson = (PPersonRow)FMainDS.PPerson.DefaultView[0].Row;
                        }
                        else
                        {
                            FCurrentPerson = null;
                        }

                        if (FImportedUnits.Contains(FCurrentPartner.PartnerKey))
                        {
                            AddStatus(String.Format(Catalog.GetString("Unit [{0}] already imported.\r\n"), FCurrentPartner.PartnerKey));
                            FCurrentNumberOfRecord++;
                        }
                    } while (FImportedUnits.Contains(FCurrentPartner.PartnerKey) && (FCurrentNumberOfRecord <= FTotalNumberOfRecords));
                }

                // have we finished importing?
                if (FCurrentNumberOfRecord > FTotalNumberOfRecords)
                {
                    AddStatus(String.Format(Catalog.GetPluralString(
                                "{0} record processed - Import finished.\r\n", "{0} records processed - Import finished.\r\n", FTotalNumberOfRecords,
                                true),
                            FTotalNumberOfRecords));
                    SaveLogFile();
                    DoFinishOptions(false);
                    SetControlsIdle();

                    FMainDS.Clear();
                    txtFilename.Text = string.Empty;
                    grpStepTwo.Enabled = false;

                    // finish the thread
                    FNeedUserFeedback = true;

                    grdMatchingRecords.DataSource = null;
                    txtHint.Text = String.Empty;
                    return;
                }

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

                    AddStatus(String.Format(Catalog.GetString("\r\nProcessing record {0} of {1}:\r\n"), FCurrentNumberOfRecord, FTotalNumberOfRecords));
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
                grdMatchingRecords.Enabled = true;
                //btnUseSelectedAddress.Enabled = false;
                btnUseSelectedPerson.Enabled = false;
                btnCreateNewPartner.Enabled = true;
                btnUseSelectedFamily.Enabled = false;

                if (FFileFormat != TImportFileFormat.csv)
                {
                    btnFindOtherPartner.Enabled = false;
                }
                else
                {
                    btnFindOtherPartner.Enabled = true;

                    if ((FCurrentPartner == null) || (FCurrentPartner.PartnerKey > 0))
                    {
                        // if there is a partner key in the import file then we currently won't allow the user to update a different partner
                        btnFindOtherPartner.Enabled = false;
                    }
                }

                bool FoundPartnerInDatabase = false;
                bool FoundPossiblePartnersInDatabase = false;

                // retrieve data set with matching partner records
                PartnerFindTDS result = FindMatchingPartners(BestLocation, out FMatchingPartnersExplanation);
                txtHint.Text = FMatchingPartnersExplanation;

                if (result.SearchResult.DefaultView.Count > 0)
                {
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("Class"), result.SearchResult.ColumnPartnerClass);
                    grdMatchingRecords.AddPartnerKeyColumn(Catalog.GetString("Partner Key"), result.SearchResult.ColumnPartnerKey);
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("Partner Name"), result.SearchResult.ColumnPartnerShortName);
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("Address"), result.SearchResult.ColumnStreetName);
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("City"), result.SearchResult.ColumnCity);
                    grdMatchingRecords.AddTextColumn(Catalog.GetString("Post Code"), result.SearchResult.ColumnPostalCode);
                    grdMatchingRecords.SelectionMode = SourceGrid.GridSelectionMode.Row;

                    result.SearchResult.DefaultView.AllowNew = false;

                    //
                    // For any partner class OTHER THAN Person, I only want to see matching records of the same class.
                    if (FCurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        // For Person we allow to show Person and Family (as people could add a person to a different family)
                        result.SearchResult.DefaultView.RowFilter = String.Format("{0} = '{1}' OR {2} = '{3}'",
                            PartnerFindTDSSearchResultTable.GetPartnerClassDBName(),
                            MPartnerConstants.PARTNERCLASS_PERSON,
                            PartnerFindTDSSearchResultTable.GetPartnerClassDBName(),
                            MPartnerConstants.PARTNERCLASS_FAMILY);
                    }
                    else
                    {
                        result.SearchResult.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                            PartnerFindTDSSearchResultTable.GetPartnerClassDBName(),
                            FCurrentPartner.PartnerClass);
                    }

                    grdMatchingRecords.DataSource = new DevAge.ComponentModel.BoundDataView(result.SearchResult.DefaultView);

                    grdMatchingRecords.AutoResizeGrid();

                    if (FFileFormat == TImportFileFormat.ext)
                    {
                        btnUseSelectedPerson.Enabled = true;
                        btnFindOtherPartner.Enabled = false;
                    }
                }
                else
                {
                    btnCreateNewPartner.Enabled = true;
                    btnCreateNewPartner.Focus();
                }

                FoundPartnerMatchingKey = -1;
                FoundPossiblePartnersInDatabase = result.SearchResult.Rows.Count != 0;

                // Check if the partner to import matches completely one of the search results
                foreach (PartnerFindTDSSearchResultRow row in result.SearchResult.Rows)
                {
                    // first check if address and name match (in case there may not be a partner key in import file)
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

                    // now check if key (and class) of partner to import exists in db
                    if ((row.PartnerClass == FCurrentPartner.PartnerClass)
                        && (row.PartnerKey == FCurrentPartner.PartnerKey))
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

                if (FThreadAutomaticImport != null)
                {
                    if (FoundPartnerInDatabase)
                    {
                        UseSelectedPerson(this, null);
                    }
                    else if (!FoundPossiblePartnersInDatabase)
                    {
                        // Automatically create a new partner, and proceed to next partner
                        // TODO: create PERSON or FAMILY?
                        try
                        {
                            AddStatus("<Automatic import>" + Environment.NewLine);
                            CreateNewPartner(this, null);
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Return a string that can be used for SQL search with special characters (e.g. Umlaute) replaced by wildcards
        /// </summary>
        /// <param name="AOriginalString"></param>
        /// <returns>returns string that replaces special characters (e.g. Umlaute) with wildcards for use in SQL search</returns>
        private String AssembleMatchString(String AOriginalString)
        {
            String MatchString = AOriginalString;

            MatchString = MatchString.Replace("ö", "%");
            MatchString = MatchString.Replace("Ö", "%");
            MatchString = MatchString.Replace("ü", "%");
            MatchString = MatchString.Replace("Ü", "%");
            MatchString = MatchString.Replace("ä", "%");
            MatchString = MatchString.Replace("Ä", "%");
            MatchString = MatchString.Replace("ß", "%");

            MatchString = MatchString.Replace("oe", "%");
            MatchString = MatchString.Replace("ue", "%");
            MatchString = MatchString.Replace("ae", "%");
            MatchString = MatchString.Replace("ss", "%");

            MatchString = MatchString.Replace("â", "_");
            MatchString = MatchString.Replace("é", "_");
            MatchString = MatchString.Replace("è", "_");
            MatchString = MatchString.Replace("ë", "_");
            MatchString = MatchString.Replace("ê", "_");
            MatchString = MatchString.Replace("ï", "_");
            MatchString = MatchString.Replace("ô", "_");

            return MatchString;
        }

        /// <summary>
        /// Return a string that can be used for SQL search for matching street names
        /// </summary>
        /// <param name="AOriginalString"></param>
        /// <returns>returns a string that can be used for SQL search for matching street names</returns>
        private String AssembleMatchStringStreet(String AOriginalString)
        {
            String MatchString = AssembleMatchString(AOriginalString);

            if (Regex.IsMatch(MatchString, "-strasse", RegexOptions.IgnoreCase))
            {
                return Regex.Replace(MatchString, "-strasse", "%str%", RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(MatchString, "-straße", RegexOptions.IgnoreCase))
            {
                return Regex.Replace(MatchString, "-straße", "%str%", RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(MatchString, " strasse", RegexOptions.IgnoreCase))
            {
                return Regex.Replace(MatchString, " strasse", "%str%", RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(MatchString, " straße", RegexOptions.IgnoreCase))
            {
                return Regex.Replace(MatchString, " straße", "%str%", RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(MatchString, "strasse", RegexOptions.IgnoreCase))
            {
                return Regex.Replace(MatchString, " strasse", "%str%", RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(MatchString, "straße", RegexOptions.IgnoreCase))
            {
                return Regex.Replace(MatchString, "straße", "%str%", RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(MatchString, "-str.", RegexOptions.IgnoreCase))
            {
                return Regex.Replace(MatchString, "-str.", "%str%", RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(MatchString, " str.", RegexOptions.IgnoreCase))
            {
                return Regex.Replace(MatchString, " str.", "%str%", RegexOptions.IgnoreCase);
            }

            if (Regex.IsMatch(MatchString, "str.", RegexOptions.IgnoreCase))
            {
                return Regex.Replace(MatchString, "str.", "%str%", RegexOptions.IgnoreCase);
            }

            return MatchString;
        }

        /// <summary>
        /// Find results for possible matching partners for records given in csv file
        /// </summary>
        /// <param name="ACurrentPartner"></param>
        /// <param name="ABestLocation"></param>
        /// <param name="AMatchingPartnersExplanation"></param>
        /// <returns>returns data table with results</returns>
        private DataTable PerformSearchForMatchingPartners(PPartnerRow ACurrentPartner,
            PLocationRow ABestLocation,
            out String AMatchingPartnersExplanation)
        {
            DataTable result = new DataTable();
            DataTable result2 = new DataTable();
            PartnerFindTDSSearchCriteriaTable CriteriaTable = new PartnerFindTDSSearchCriteriaTable();
            PartnerFindTDSSearchCriteriaRow CriteriaRow;
            int TotalRecords;
            short TotalPages;
            short PageSize = 50;

            // Match options: BEGINS, ENDS, CONTAINS, EXACT

            // initialize explanation
            AMatchingPartnersExplanation = "";

            // if there is a person partner key given then search for that record
            if (ACurrentPartner.PartnerKey > 0)
            {
                // if partner key is given then search for exactly that Partner
                CriteriaTable.Rows.Clear();
                CriteriaRow = CriteriaTable.NewRowTyped();
                CriteriaRow.ExactPartnerKeyMatch = true;
                CriteriaRow.PartnerKey = ACurrentPartner.PartnerKey;
                CriteriaRow.PartnerClass = "*";
                CriteriaTable.Rows.Add(CriteriaRow);

                FPartnerFindObject.PerformSearch(CriteriaTable, true);
                FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                result = FPartnerFindObject.FilterResultByBestAddress();

                if (TotalRecords > 0)
                {
                    // we found a partner with the given key
                    AMatchingPartnersExplanation = Catalog.GetString("Partner already found with given Partner Key");
                    return result;
                }
            }

            // find matching partners if a family key is given
            if ((ACurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                && (FCurrentPerson != null)
                && (FCurrentPerson.FamilyKey > 0))
            {
                // if we are importing a person then check if we can find somebody with family key, DOB and name
                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_DATEOFBIRTH)
                    && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FAMILYNAME)
                    && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FIRSTNAME)
                    && !FCurrentPerson.IsDateOfBirthNull())
                {
                    CriteriaTable.Rows.Clear();
                    CriteriaRow = CriteriaTable.NewRowTyped();
                    CriteriaRow.PartnerClass = ACurrentPartner.PartnerClass;
                    CriteriaRow.PartnerName = Calculations.FormatShortName(AssembleMatchString(
                            ACurrentPartner.PartnerShortName), eShortNameFormat.eJustRemoveTitle);
                    CriteriaRow.PartnerNameMatch = "BEGINS";
                    CriteriaRow.DateOfBirth = FCurrentPerson.DateOfBirth.Value;
                    CriteriaRow.FamilyKey = FCurrentPerson.FamilyKey;
                    CriteriaTable.Rows.Add(CriteriaRow);

                    FPartnerFindObject.PerformSearch(CriteriaTable, true);
                    FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                    result = FPartnerFindObject.FilterResultByBestAddress();

                    if (TotalRecords > 0)
                    {
                        // we found a partner with the given key
                        AMatchingPartnersExplanation = Catalog.GetString("Person found in Family with given Family Partner Key. " +
                            "The person found matches name and date of birth.");
                        return result;
                    }
                }

                // next omit DOB, but check for family key and name
                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FAMILYNAME)
                    && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FIRSTNAME))
                {
                    CriteriaTable.Rows.Clear();
                    CriteriaRow = CriteriaTable.NewRowTyped();
                    CriteriaRow.PartnerClass = ACurrentPartner.PartnerClass;
                    CriteriaRow.PartnerName = Calculations.FormatShortName(AssembleMatchString(
                            ACurrentPartner.PartnerShortName), eShortNameFormat.eJustRemoveTitle);
                    CriteriaRow.PartnerNameMatch = "BEGINS";
                    CriteriaRow.FamilyKey = FCurrentPerson.FamilyKey;
                    CriteriaTable.Rows.Add(CriteriaRow);

                    FPartnerFindObject.PerformSearch(CriteriaTable, true);
                    FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                    result = FPartnerFindObject.FilterResultByBestAddress();

                    if (TotalRecords > 0)
                    {
                        // we found a partner with the given key
                        AMatchingPartnersExplanation = Catalog.GetString("Person found in Family with given Family Partner Key. " +
                            "The person found matches the given name.");
                        return result;
                    }
                }

                // next omit DOB and name, but check for family key (in both Family and Person record)
                CriteriaTable.Rows.Clear();
                CriteriaRow = CriteriaTable.NewRowTyped();
                CriteriaRow.PartnerClass = MPartnerConstants.PARTNERCLASS_PERSON;
                CriteriaRow.FamilyKey = FCurrentPerson.FamilyKey;
                CriteriaTable.Rows.Add(CriteriaRow);

                FPartnerFindObject.PerformSearch(CriteriaTable, true);
                FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                result = FPartnerFindObject.FilterResultByBestAddress();

                CriteriaTable.Rows.Clear();
                CriteriaRow = CriteriaTable.NewRowTyped();
                CriteriaRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
                CriteriaRow.ExactPartnerKeyMatch = true;
                CriteriaRow.PartnerKey = FCurrentPerson.FamilyKey;
                CriteriaTable.Rows.Add(CriteriaRow);

                FPartnerFindObject.PerformSearch(CriteriaTable, true);
                result2 = FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);

                // now combine the two results
                result.Merge(result2);
                TotalRecords = result.Rows.Count;

                if (TotalRecords > 0)
                {
                    // we found a family with given key and persons with that family key
                    AMatchingPartnersExplanation = Catalog.GetString("List shows Family and Person Records of the Family with the given " +
                        "Partner Key. Please make your selection..");
                    return result;
                }
            }

            // find matching partners independent of family key
            if (ACurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                // if we are importing a person then check if we can find somebody with DOB and name
                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_DATEOFBIRTH)
                    && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FAMILYNAME)
                    && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FIRSTNAME)
                    && (FCurrentPerson != null)
                    && !FCurrentPerson.IsDateOfBirthNull())
                {
                    CriteriaTable.Rows.Clear();
                    CriteriaRow = CriteriaTable.NewRowTyped();
                    CriteriaRow.PartnerClass = ACurrentPartner.PartnerClass;
                    CriteriaRow.PartnerName = Calculations.FormatShortName(AssembleMatchString(
                            ACurrentPartner.PartnerShortName), eShortNameFormat.eJustRemoveTitle);
                    CriteriaRow.PartnerNameMatch = "BEGINS";
                    CriteriaRow.DateOfBirth = FCurrentPerson.DateOfBirth.Value;
                    CriteriaTable.Rows.Add(CriteriaRow);

                    FPartnerFindObject.PerformSearch(CriteriaTable, true);
                    FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                    result = FPartnerFindObject.FilterResultByBestAddress();

                    if (TotalRecords > 0)
                    {
                        // we found a partner with the given key
                        AMatchingPartnersExplanation = Catalog.GetString("Person found with given name and date of birth.");
                        return result;
                    }
                }
            }

            /* If  nobody could be found yet then the import program checks for name and address matches */
            /* This is done for any type of partner */
            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FAMILYNAME)
                && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FIRSTNAME)
                && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_POSTCODE)
                && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CITY)
                && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_ADDRESS1)
                && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_STREET)
                && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_ADDRESS3))
            {
                CriteriaTable.Rows.Clear();
                CriteriaRow = CriteriaTable.NewRowTyped();

                // For any partner class OTHER THAN Person, I only want to see matching records of the same class. For Persons we can include Family records.
                // !!! It is important to set the PartnerClass Search Criteria if we don't only search for PartnerKey
                if (ACurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                {
                    CriteriaRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY + "," + MPartnerConstants.PARTNERCLASS_PERSON;
                }
                else
                {
                    CriteriaRow.PartnerClass = ACurrentPartner.PartnerClass;
                }

                CriteriaRow.PartnerName = Calculations.FormatShortName(AssembleMatchString(
                        ACurrentPartner.PartnerShortName), eShortNameFormat.eJustRemoveTitle);
                CriteriaRow.PartnerNameMatch = "BEGINS";
                CriteriaRow.PostCode = ABestLocation.PostalCode;
                CriteriaRow.City = AssembleMatchString(ABestLocation.City);
                CriteriaRow.Address1 = AssembleMatchString(ABestLocation.Locality);
                CriteriaRow.Address2 = AssembleMatchString(ABestLocation.StreetName);
                CriteriaRow.Address3 = AssembleMatchString(ABestLocation.Address3);
                CriteriaTable.Rows.Add(CriteriaRow);

                FPartnerFindObject.PerformSearch(CriteriaTable, true);
                FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                result = FPartnerFindObject.FilterResultByBestAddress();

                if (TotalRecords > 0)
                {
                    // we found a partner with the given key
                    AMatchingPartnersExplanation = Catalog.GetString("Partners found with given name and address.");
                    return result;
                }
            }

            /* If  nobody could be found yet then the import program checks for name only */
            /* This is done for any type of partner */
            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FAMILYNAME)
                && FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FIRSTNAME))
            {
                CriteriaTable.Rows.Clear();
                CriteriaRow = CriteriaTable.NewRowTyped();

                // For any partner class OTHER THAN Person, I only want to see matching records of the same class. For Persons we can include Family records.
                // !!! It is important to set the PartnerClass Search Criteria if we don't only search for PartnerKey
                if (ACurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                {
                    CriteriaRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY + "," + MPartnerConstants.PARTNERCLASS_PERSON;
                }
                else
                {
                    CriteriaRow.PartnerClass = ACurrentPartner.PartnerClass;
                }

                CriteriaRow.PartnerName = Calculations.FormatShortName(AssembleMatchString(
                        ACurrentPartner.PartnerShortName), eShortNameFormat.eJustRemoveTitle);
                CriteriaRow.PartnerNameMatch = "BEGINS";
                CriteriaTable.Rows.Add(CriteriaRow);

                FPartnerFindObject.PerformSearch(CriteriaTable, true);
                FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                result = FPartnerFindObject.FilterResultByBestAddress();

                if (TotalRecords > 0)
                {
                    // we found a partner with the given key
                    AMatchingPartnersExplanation = Catalog.GetString("Partners found with given name.");
                    return result;
                }

                /* If the first name is just an initial (in which case one of the characters is a "." */
                /* then check for first names that start with the characters of the imported initial */
                String FirstName = Calculations.FormatShortName(ACurrentPartner.PartnerShortName, eShortNameFormat.eOnlyFirstname);

                if (FirstName.EndsWith("."))
                {
                    CriteriaTable.Rows.Clear();
                    CriteriaRow = CriteriaTable.NewRowTyped();

                    // For any partner class OTHER THAN Person, I only want to see matching records of the same class. For Persons we can include Family records.
                    // !!! It is important to set the PartnerClass Search Criteria if we don't only search for PartnerKey
                    if (ACurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        CriteriaRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY + "," + MPartnerConstants.PARTNERCLASS_PERSON;
                    }
                    else
                    {
                        CriteriaRow.PartnerClass = ACurrentPartner.PartnerClass;
                    }

                    CriteriaRow.PartnerName = Calculations.FormatShortName(AssembleMatchString(
                            ACurrentPartner.PartnerShortName), eShortNameFormat.eOnlySurnameFirstNameInitial);
                    CriteriaRow.PartnerName = CriteriaRow.PartnerName.TrimEnd('.');
                    CriteriaRow.PartnerNameMatch = "BEGINS";
                    CriteriaTable.Rows.Add(CriteriaRow);

                    FPartnerFindObject.PerformSearch(CriteriaTable, true);
                    FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                    result = FPartnerFindObject.FilterResultByBestAddress();

                    if (TotalRecords > 0)
                    {
                        // we found a partner with the given key
                        AMatchingPartnersExplanation = Catalog.GetString(
                            "Partners found with given family name and first name starting with same initials.");
                        return result;
                    }
                }

                /* If nothing found yet then check if there is an abbreviated first name (with dot) in the database. */
                /* This would find "J. Jansen" in the database if "Jan Jansen" is in the import file */
                CriteriaTable.Rows.Clear();
                CriteriaRow = CriteriaTable.NewRowTyped();

                // For any partner class OTHER THAN Person, I only want to see matching records of the same class. For Persons we can include Family records.
                // !!! It is important to set the PartnerClass Search Criteria if we don't only search for PartnerKey
                if (ACurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                {
                    CriteriaRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY + "," + MPartnerConstants.PARTNERCLASS_PERSON;
                }
                else
                {
                    CriteriaRow.PartnerClass = ACurrentPartner.PartnerClass;
                }

                CriteriaRow.PartnerName = Calculations.FormatShortName(AssembleMatchString(
                        ACurrentPartner.PartnerShortName), eShortNameFormat.eOnlySurnameFirstNameInitial);
                CriteriaRow.PartnerName = CriteriaRow.PartnerName.TrimEnd('.');
                CriteriaRow.PartnerName += ".";
                CriteriaRow.PartnerNameMatch = "BEGINS";
                CriteriaTable.Rows.Add(CriteriaRow);

                FPartnerFindObject.PerformSearch(CriteriaTable, true);
                FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                result = FPartnerFindObject.FilterResultByBestAddress();

                if (TotalRecords > 0)
                {
                    // we found a partner with initial of imported first name
                    AMatchingPartnersExplanation = Catalog.GetString("Partners found with given initial of the imported first name.");
                    return result;
                }

                /* If nothing found yet then check if there is an abbreviated first name (no dot) in the database. */
                /* This would find "J Jansen" in the database if "Jan Jansen" is in the import file */
                CriteriaTable.Rows.Clear();
                CriteriaRow = CriteriaTable.NewRowTyped();

                // For any partner class OTHER THAN Person, I only want to see matching records of the same class. For Persons we can include Family records.
                // !!! It is important to set the PartnerClass Search Criteria if we don't only search for PartnerKey
                if (ACurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                {
                    CriteriaRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY + "," + MPartnerConstants.PARTNERCLASS_PERSON;
                }
                else
                {
                    CriteriaRow.PartnerClass = ACurrentPartner.PartnerClass;
                }

                CriteriaRow.PartnerName = Calculations.FormatShortName(AssembleMatchString(
                        ACurrentPartner.PartnerShortName), eShortNameFormat.eOnlySurnameFirstNameInitial);
                CriteriaRow.PartnerName = CriteriaRow.PartnerName.TrimEnd('.');
                CriteriaRow.PartnerName += ",";
                CriteriaRow.PartnerNameMatch = "BEGINS";
                CriteriaTable.Rows.Add(CriteriaRow);

                FPartnerFindObject.PerformSearch(CriteriaTable, true);
                FPartnerFindObject.GetDataPagedResult(0, PageSize, out TotalRecords, out TotalPages);
                result = FPartnerFindObject.FilterResultByBestAddress();

                if (TotalRecords > 0)
                {
                    // we found a partner with initial of imported first name
                    AMatchingPartnersExplanation = Catalog.GetString("Partners found with given initial of the imported first name.");
                    return result;
                }
            }

            return result;
        }

        private PartnerFindTDS FindMatchingPartners(PLocationRow BestLocation, out String MatchingPartnersExplanation)
        {
            PartnerFindTDS result = new PartnerFindTDS();

            MatchingPartnersExplanation = "";

            if (FFileFormat == TImportFileFormat.csv)
            {
                result.SearchResult.Merge(PerformSearchForMatchingPartners(FCurrentPartner, BestLocation, out MatchingPartnersExplanation));
            }
            else if (FFileFormat == TImportFileFormat.ext)
            {
                // Try to find an existing partner (either with partner key or location data) and set the partner key
                // Or if the address is found, the location record can be shared.
                if ((FCurrentPartner.PartnerKey > 0)
                    || (BestLocation != null))
                {
                    if (FCurrentPartner.PartnerKey > 0)
                    {
                        // if partner key is given then search for exactly that Partner
                        result = TRemote.MPartner.Partner.WebConnectors.FindPartners(FCurrentPartner.PartnerKey, true);
                    }
                }

                if (result.SearchResult.DefaultView.Count > 0)
                {
                    MatchingPartnersExplanation = "Partner already found with given Partner Key. Select \"Update Partner\" to import.";
                }
                else
                {
                    MatchingPartnersExplanation = "Partner not found. Select \"Create Partner\" to import this partner.";
                }
            }

            return result;
        }

        private void CancelImport(Object sender, EventArgs e)
        {
            // todo: cleanly stop the thread during automatic import?
            if (FThreadAutomaticImport != null)
            {
                FNeedUserFeedback = true;
                return;
            }

            DoFinishOptions(true);

            // restore dataset so import can be restarted from scratch
            FMainDS.Clear();
            FMainDS.Merge(FMainDSBackup.Copy());

            // TODO: store partner keys of imported partners
            btnStartImport.Enabled = true;

            SetControlsIdle();

            AddStatus(Catalog.GetString("Import stopped.") + "\r\n\r\n");
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
                    //rv.Row.AcceptChanges(); // This removes the RowState: Added //TODOWBxxx?
                }
                else
                {
                    rv.Row[AKeyDbName] = ANewPartnerKey;
                }

                ADestTable.ImportRow(rv.Row);
            }
        }

        private void AddPartnerForCSVPartnerUpdate(Int64 AImportPartnerKey,
            Int64 AExistingPartnerKey,
            ref PartnerImportExportTDS AImportPartnerDS,
            PartnerImportExportTDS AExistingPartnerDS)
        {
            Boolean UpdateName = false;

            AImportPartnerDS.PPartner.DefaultView.RowFilter = String.Format("{0}={1}",
                PPartnerTable.GetPartnerKeyDBName(), AImportPartnerKey);

            PPartnerRow ImportedPartnerRow = (PPartnerRow)AImportPartnerDS.PPartner.DefaultView[0].Row;
            String ImportedPartnerClass = ImportedPartnerRow.PartnerClass;

            AExistingPartnerDS.PPartner.DefaultView.RowFilter = String.Format("{0}={1}",
                PPartnerTable.GetPartnerKeyDBName(), AExistingPartnerKey);

            PPartnerRow ExistingPartnerRow = (PPartnerRow)AExistingPartnerDS.PPartner.DefaultView[0].Row;

            // At this point we can't remove and add any partner row, so we need ot make sure we update the existing one
            // (as other parts of the code rely on the order of records in the Parnter Table)
            // Therefore we copy the imported values to a temporary row, then update the imported row with values
            // from the existing (db) one. Then we need to call AcceptChanges so we can start fresh. And finally we
            // move the values of the temporary row into the imported one so they get saved to the db.
            PPartnerRow ImportedPartnerRowCopy = AImportPartnerDS.PPartner.NewRowTyped(false);
            ImportedPartnerRowCopy.ItemArray = (object[])ImportedPartnerRow.ItemArray.Clone();

            if (ImportedPartnerRow.PartnerShortName != ExistingPartnerRow.PartnerShortName)
            {
                if (MessageBox.Show(string.Format(Catalog.GetString("Do you want to update the name '{0}' to '{1}'?"),
                            ExistingPartnerRow.PartnerShortName, ImportedPartnerRow.PartnerShortName),
                        Catalog.GetString("Update Name"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    UpdateName = true;
                }
            }

            ImportedPartnerRow.ItemArray = (object[])ExistingPartnerRow.ItemArray.Clone();
            ImportedPartnerRow.AcceptChanges();

            if ((FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FAMILYNAME)
                 || FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FIRSTNAME)
                 || FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_TITLE))
                && UpdateName)
            {
                ImportedPartnerRow.PartnerShortName = ImportedPartnerRowCopy.PartnerShortName;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_AQUISITION))
            {
                ImportedPartnerRow.AcquisitionCode = ImportedPartnerRowCopy.AcquisitionCode;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE))
            {
                ImportedPartnerRow.AddresseeTypeCode = ImportedPartnerRowCopy.AddresseeTypeCode;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_LANGUAGE))
            {
                ImportedPartnerRow.LanguageCode = ImportedPartnerRowCopy.LanguageCode;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_NOTES))
            {
                ImportedPartnerRow.Comment = ImportedPartnerRowCopy.Comment;
            }

            if (ImportedPartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                AImportPartnerDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}",
                    PPersonTable.GetPartnerKeyDBName(), AImportPartnerKey);

                PPersonRow ImportedPersonRow = (PPersonRow)AImportPartnerDS.PPerson.DefaultView[0].Row;

                AExistingPartnerDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}",
                    PPersonTable.GetPartnerKeyDBName(), AExistingPartnerKey);

                PPersonRow ExistingPersonRow = (PPersonRow)AExistingPartnerDS.PPerson.DefaultView[0].Row;

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FAMILYNAME)
                    && UpdateName)
                {
                    ExistingPersonRow.FamilyName = ImportedPersonRow.FamilyName;
                }

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_MARITALSTATUS))
                {
                    ExistingPersonRow.MaritalStatus = ImportedPersonRow.MaritalStatus;
                }

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_GENDER))
                {
                    ExistingPersonRow.Gender = ImportedPersonRow.Gender;
                }

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FIRSTNAME)
                    && UpdateName)
                {
                    ExistingPersonRow.FirstName = ImportedPersonRow.FirstName;
                }

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_TITLE)
                    && UpdateName)
                {
                    ExistingPersonRow.Title = ImportedPersonRow.Title;
                }

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_DATEOFBIRTH))
                {
                    ExistingPersonRow.DateOfBirth = ImportedPersonRow.DateOfBirth;
                }

                ExistingPersonRow.PartnerKey = AImportPartnerKey;

                // make sure we update FCurrentPerson if row is changed
                if (FCurrentPerson == ImportedPersonRow)
                {
                    FCurrentPerson = ExistingPersonRow;
                }

                AImportPartnerDS.PPerson.Rows.Remove(ImportedPersonRow);
                AImportPartnerDS.PPerson.ImportRow(ExistingPersonRow);
            }

            if (ImportedPartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                AImportPartnerDS.PFamily.DefaultView.RowFilter = String.Format("{0}={1}",
                    PFamilyTable.GetPartnerKeyDBName(), AImportPartnerKey);

                PFamilyRow ImportedFamilyRow = (PFamilyRow)AImportPartnerDS.PFamily.DefaultView[0].Row;

                AExistingPartnerDS.PFamily.DefaultView.RowFilter = String.Format("{0}={1}",
                    PFamilyTable.GetPartnerKeyDBName(), AExistingPartnerKey);

                PFamilyRow ExistingFamilyRow = (PFamilyRow)AExistingPartnerDS.PFamily.DefaultView[0].Row;

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FAMILYNAME)
                    && UpdateName)
                {
                    ExistingFamilyRow.FamilyName = ImportedFamilyRow.FamilyName;
                }

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_MARITALSTATUS))
                {
                    ExistingFamilyRow.MaritalStatus = ImportedFamilyRow.MaritalStatus;
                }

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_FIRSTNAME)
                    && UpdateName)
                {
                    ExistingFamilyRow.FirstName = ImportedFamilyRow.FirstName;
                }

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_TITLE)
                    && UpdateName)
                {
                    ExistingFamilyRow.Title = ImportedFamilyRow.Title;
                }

                if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_NOTESFAMILY))
                {
                    ImportedPartnerRow.Comment = ImportedPartnerRowCopy.Comment;
                }

                ExistingFamilyRow.PartnerKey = AImportPartnerKey;

                AImportPartnerDS.PFamily.Rows.Remove(ImportedFamilyRow);
                AImportPartnerDS.PFamily.ImportRow(ExistingFamilyRow);
            }

            //TODO: cover other partner types?
        }

        private void AddAddressForCSVPartnerUpdate(Int64 AImportPartnerKey,
            Int64 AExistingPartnerKey,
            ref PartnerImportExportTDS AImportPartnerDS,
            PartnerImportExportTDS AExistingPartnerDS)
        {
            //TODOWBxxx
        }

        private void AddShortTermAppForCSVPartnerUpdate(Int64 AImportPartnerKey,
            Int64 AExistingPartnerKey,
            ref PartnerImportExportTDS AImportPartnerDS,
            PartnerImportExportTDS AExistingPartnerDS)
        {
            // Import short term application record for an already existing person: use retrieved data from db, update it with import data and exchange it with the one to import
            AImportPartnerDS.PmShortTermApplication.DefaultView.RowFilter = String.Format("{0}={1}",
                PmShortTermApplicationTable.GetPartnerKeyDBName(), AImportPartnerKey);

            if (!FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_EVENTKEY)
                || (AImportPartnerDS.PmShortTermApplication.DefaultView.Count == 0))
            {
                return;
            }

            // for a csv file there is just one short term application row per person
            PmShortTermApplicationRow ImportedShortTermRow = (PmShortTermApplicationRow)AImportPartnerDS.PmShortTermApplication.DefaultView[0].Row;

            // now we need to find the general application that belongs to the short term application
            AImportPartnerDS.PmGeneralApplication.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                PmGeneralApplicationTable.GetPartnerKeyDBName(), ImportedShortTermRow.PartnerKey,
                PmGeneralApplicationTable.GetApplicationKeyDBName(), ImportedShortTermRow.ApplicationKey,
                PmGeneralApplicationTable.GetRegistrationOfficeDBName(), ImportedShortTermRow.RegistrationOffice);

            PmGeneralApplicationRow ImportedGenAppRow = (PmGeneralApplicationRow)AImportPartnerDS.PmGeneralApplication.DefaultView[0].Row;


            AExistingPartnerDS.PmShortTermApplication.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3}",
                PmShortTermApplicationTable.GetPartnerKeyDBName(), AExistingPartnerKey,
                PmShortTermApplicationTable.GetStConfirmedOptionDBName(), ImportedShortTermRow.StConfirmedOption);

            if (AExistingPartnerDS.PmShortTermApplication.DefaultView.Count == 0)
            {
                return;
            }

            PmShortTermApplicationRow ExistingShortTermRow = (PmShortTermApplicationRow)AExistingPartnerDS.PmShortTermApplication.DefaultView[0].Row;

            // now we need to find the general application that belongs to the short term application
            AExistingPartnerDS.PmGeneralApplication.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                PmGeneralApplicationTable.GetPartnerKeyDBName(), ExistingShortTermRow.PartnerKey,
                PmGeneralApplicationTable.GetApplicationKeyDBName(), ExistingShortTermRow.ApplicationKey,
                PmGeneralApplicationTable.GetRegistrationOfficeDBName(), ExistingShortTermRow.RegistrationOffice);

            PmGeneralApplicationRow ExistingGenAppRow = (PmGeneralApplicationRow)AExistingPartnerDS.PmGeneralApplication.DefaultView[0].Row;

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_ARRIVALDATE))
            {
                ExistingShortTermRow.Arrival = ImportedShortTermRow.Arrival;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_ARRIVALTIME))
            {
                ExistingShortTermRow.ArrivalHour = ImportedShortTermRow.ArrivalHour;
                ExistingShortTermRow.ArrivalMinute = ImportedShortTermRow.ArrivalMinute;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_DEPARTUREDATE))
            {
                ExistingShortTermRow.Departure = ImportedShortTermRow.Departure;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_DEPARTURETIME))
            {
                ExistingShortTermRow.DepartureHour = ImportedShortTermRow.DepartureHour;
                ExistingShortTermRow.DepartureMinute = ImportedShortTermRow.DepartureMinute;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_EVENTROLE))
            {
                ExistingShortTermRow.StCongressCode = ImportedShortTermRow.StCongressCode;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_APPDATE))
            {
                ExistingShortTermRow.StAppDate = ImportedShortTermRow.StAppDate;
                ExistingGenAppRow.GenAppDate = ImportedGenAppRow.GenAppDate;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_APPSTATUS))
            {
                ExistingGenAppRow.GenApplicationStatus = ImportedGenAppRow.GenApplicationStatus;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_APPTYPE))
            {
                ExistingGenAppRow.AppTypeName = ImportedGenAppRow.AppTypeName;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CHARGEDFIELD))
            {
                ExistingShortTermRow.StFieldCharged = ImportedShortTermRow.StFieldCharged;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_APPCOMMENTS))
            {
                ExistingGenAppRow.Comment = ImportedGenAppRow.Comment;
            }

            ExistingGenAppRow.PartnerKey = AImportPartnerKey;
            ExistingShortTermRow.PartnerKey = AImportPartnerKey;

            AImportPartnerDS.PmShortTermApplication.Rows.Remove(ImportedShortTermRow);
            AImportPartnerDS.PmGeneralApplication.Rows.Remove(ImportedGenAppRow);

            AImportPartnerDS.PmGeneralApplication.ImportRow(ExistingGenAppRow);
            AImportPartnerDS.PmShortTermApplication.ImportRow(ExistingShortTermRow);
        }

        private void AddSpecialTypeForCSVPartnerUpdate(Int64 AImportPartnerKey,
            Int64 AExistingPartnerKey,
            ref PartnerImportExportTDS AImportPartnerDS,
            PartnerImportExportTDS AExistingPartnerDS)
        {
            // Import special type record for an already existing partner: use retrieved data from db, update it with import data and exchange it with the one to import
            AImportPartnerDS.PPartnerType.DefaultView.RowFilter = String.Format("{0}={1}",
                PPartnerTypeTable.GetPartnerKeyDBName(), AImportPartnerKey);

            foreach (DataRowView rv in AImportPartnerDS.PPartnerType.DefaultView)
            {
                PPartnerTypeRow ImportedRow = (PPartnerTypeRow)rv.Row;

                AExistingPartnerDS.PPartnerType.DefaultView.RowFilter = String.Format("{0}={1} AND {2}='{3}'",
                    PPartnerTypeTable.GetPartnerKeyDBName(), AExistingPartnerKey,
                    PPartnerTypeTable.GetTypeCodeDBName(), ImportedRow.TypeCode);

                if (AExistingPartnerDS.PPartnerType.DefaultView.Count > 0)
                {
                    PPartnerTypeRow ExistingRow = (PPartnerTypeRow)AExistingPartnerDS.PPartnerType.DefaultView[0].Row;

                    ExistingRow.PartnerKey = AImportPartnerKey;

                    AImportPartnerDS.PPartnerType.Rows.Remove(ImportedRow);
                    AImportPartnerDS.PPartnerType.ImportRow(ExistingRow);
                }
            }
        }

        private void AddSubscriptionForCSVPartnerUpdate(Int64 AImportPartnerKey,
            Int64 AExistingPartnerKey,
            ref PartnerImportExportTDS AImportPartnerDS,
            PartnerImportExportTDS AExistingPartnerDS)
        {
            //TODOWBxxx
        }

        private void AddContactForCSVPartnerUpdate(Int64 AImportPartnerKey,
            Int64 AExistingPartnerKey,
            ref PartnerImportExportTDS AImportPartnerDS,
            PartnerImportExportTDS AExistingPartnerDS)
        {
            //TODOWBxxx
        }

        private void AddPassportForCSVPartnerUpdate(Int64 AImportPartnerKey,
            Int64 AExistingPartnerKey,
            ref PartnerImportExportTDS AImportPartnerDS,
            PartnerImportExportTDS AExistingPartnerDS)
        {
            // Import passport record for an already existing person: use retrieved data from db, update it with import data and exchange it with the one to import
            AImportPartnerDS.PmPassportDetails.DefaultView.RowFilter = String.Format("{0}={1}",
                PmPassportDetailsTable.GetPartnerKeyDBName(), AImportPartnerKey);

            if (!FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTNUMBER)
                || (AImportPartnerDS.PmPassportDetails.DefaultView.Count == 0))
            {
                return;
            }

            PmPassportDetailsRow ImportedRow = (PmPassportDetailsRow)AImportPartnerDS.PmPassportDetails.DefaultView[0].Row;

            AExistingPartnerDS.PmPassportDetails.DefaultView.RowFilter = String.Format("{0}={1} AND {2}='{3}'",
                PmPassportDetailsTable.GetPartnerKeyDBName(), AExistingPartnerKey,
                PmPassportDetailsTable.GetPassportNumberDBName(), ImportedRow.PassportNumber);

            if (AExistingPartnerDS.PmPassportDetails.DefaultView.Count == 0)
            {
                return;
            }

            PmPassportDetailsRow ExistingRow = (PmPassportDetailsRow)AExistingPartnerDS.PmPassportDetails.DefaultView[0].Row;

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTNAME))
            {
                ExistingRow.FullPassportName = ImportedRow.FullPassportName;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTTYPE))
            {
                ExistingRow.PassportDetailsType = ImportedRow.PassportDetailsType;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFBIRTH))
            {
                ExistingRow.PlaceOfBirth = ImportedRow.PlaceOfBirth;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTNATIONALITY))
            {
                ExistingRow.PassportNationalityCode = ImportedRow.PassportNationalityCode;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFISSUE))
            {
                ExistingRow.PlaceOfIssue = ImportedRow.PlaceOfIssue;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTCOUNTRYOFISSUE))
            {
                ExistingRow.CountryOfIssue = ImportedRow.CountryOfIssue;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFISSUE))
            {
                ExistingRow.DateOfIssue = ImportedRow.DateOfIssue;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFEXPIRATION))
            {
                ExistingRow.DateOfExpiration = ImportedRow.DateOfExpiration;
            }

            ExistingRow.PartnerKey = AImportPartnerKey;

            AImportPartnerDS.PmPassportDetails.Rows.Remove(ImportedRow);
            AImportPartnerDS.PmPassportDetails.ImportRow(ExistingRow);
        }

        private void AddSpecialNeedsForCSVPartnerUpdate(Int64 AImportPartnerKey,
            Int64 AExistingPartnerKey,
            ref PartnerImportExportTDS AImportPartnerDS,
            PartnerImportExportTDS AExistingPartnerDS)
        {
            // Import special needs record for an already existing person: use retrieved data from db, update it with import data and exchange it with the one to import

            AImportPartnerDS.PmSpecialNeed.DefaultView.RowFilter = String.Format("{0}={1}",
                PmSpecialNeedTable.GetPartnerKeyDBName(), AImportPartnerKey);

            if (AImportPartnerDS.PmSpecialNeed.DefaultView.Count == 0)
            {
                return;
            }

            PmSpecialNeedRow ImportedRow = (PmSpecialNeedRow)AImportPartnerDS.PmSpecialNeed.DefaultView[0].Row;

            AExistingPartnerDS.PmSpecialNeed.DefaultView.RowFilter = String.Format("{0}={1}",
                PmSpecialNeedTable.GetPartnerKeyDBName(), AExistingPartnerKey);

            if (AExistingPartnerDS.PmSpecialNeed.DefaultView.Count == 0)
            {
                return;
            }

            PmSpecialNeedRow ExistingRow = (PmSpecialNeedRow)AExistingPartnerDS.PmSpecialNeed.DefaultView[0].Row;

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_MEDICALNEEDS)
                && (ImportedRow.MedicalComment.Trim() != "")
                && !ExistingRow.MedicalComment.Contains(ImportedRow.MedicalComment))
            {
                if (ExistingRow.MedicalComment != "")
                {
                    ExistingRow.MedicalComment += " - ";
                }

                ExistingRow.MedicalComment += ImportedRow.MedicalComment;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_DIETARYNEEDS)
                && (ImportedRow.DietaryComment.Trim() != "")
                && !ExistingRow.DietaryComment.Contains(ImportedRow.DietaryComment))
            {
                if (ExistingRow.DietaryComment != "")
                {
                    ExistingRow.DietaryComment += " - ";
                }

                ExistingRow.DietaryComment += ImportedRow.DietaryComment;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_OTHERNEEDS)
                && (ImportedRow.OtherSpecialNeed.Trim() != "")
                && !ExistingRow.OtherSpecialNeed.Contains(ImportedRow.OtherSpecialNeed))
            {
                if (ExistingRow.OtherSpecialNeed != "")
                {
                    ExistingRow.OtherSpecialNeed += " - ";
                }

                ExistingRow.OtherSpecialNeed += ImportedRow.OtherSpecialNeed;
            }

            if (FCSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_VEGETARIAN))
            {
                ExistingRow.VegetarianFlag = ImportedRow.VegetarianFlag;
            }

            ExistingRow.PartnerKey = AImportPartnerKey;

            AImportPartnerDS.PmSpecialNeed.Rows.Remove(ImportedRow);
            AImportPartnerDS.PmSpecialNeed.ImportRow(ExistingRow);
        }

        private void ImportRecordsByPartnerKey(DataTable ADestTable,
            DataTable ASourceTable,
            String AKeyDbName,
            Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey)
        {
            ImportRecordsByPartnerKey(ADestTable, ASourceTable, AKeyDbName, AOrigPartnerKey, ANewPartnerKey, ExistingPartnerKey > 0);
        }

        private void AddAbility(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonAbility, FMainDS.PmPersonAbility,
                PmPersonAbilityTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddApplication(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmGeneralApplication, FMainDS.PmGeneralApplication,
                PmGeneralApplicationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);

            ImportRecordsByPartnerKey(ANewPartnerDS.PmShortTermApplication, FMainDS.PmShortTermApplication,
                PmShortTermApplicationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);

            ImportRecordsByPartnerKey(ANewPartnerDS.PmYearProgramApplication, FMainDS.PmYearProgramApplication,
                PmYearProgramApplicationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddAddresses(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            FMainDS.PPartnerLocation.DefaultView.RowFilter = String.Format("{0}={1}",
                PPartnerLocationTable.GetPartnerKeyDBName(), AOrigPartnerKey);

            foreach (DataRowView rv in FMainDS.PPartnerLocation.DefaultView)
            {
                PPartnerLocationRow PartnerLocationRow = (PPartnerLocationRow)rv.Row;
                bool importingAlready = false;
                bool IgnoreImportLocationForCsv = false;

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
                        // Check address already being imported, comparing StreetName, City, PostalCode etc.
                        // If I'm already importing it, I'll ignore this row.
                        // (The address may still be already in the database.)

                        if ((FFileFormat == TImportFileFormat.csv)
                            && chkReplaceAddress.Checked)
                        {
                            String CurrentAddress = UserSelectedRow.Locality + " - " + UserSelectedRow.StreetName + " - " +
                                                    UserSelectedRow.Address3 + " - " + UserSelectedRow.City + " - " +
                                                    UserSelectedRow.PostalCode + " - " + UserSelectedRow.County + " - " +
                                                    UserSelectedRow.CountryCode;
                            String NewAddress = NewLocation.Locality + " - " + NewLocation.StreetName + " - " +
                                                NewLocation.Address3 + " - " + NewLocation.City + " - " +
                                                NewLocation.PostalCode + " - " + NewLocation.County + " - " +
                                                NewLocation.CountryCode;

                            if (CurrentAddress != NewAddress)
                            {
                                String AddressMessage;
                                AddressMessage = Catalog.GetString("Do you really want to replace the current address:") + "\r\n";
                                AddressMessage += CurrentAddress + "\r\n\r\n";
                                AddressMessage += Catalog.GetString("with this NEW address:") + "\r\n";
                                AddressMessage += NewAddress + "\r\n";

                                if (MessageBox.Show(AddressMessage, Catalog.GetString("Replace Address"), MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    IgnoreImportLocationForCsv = true;
                                }
                            }
                        }

                        foreach (DataRowView plrv in ANewPartnerDS.PLocation.DefaultView)
                        {
                            PLocationRow ExistingLocation = (PLocationRow)plrv.Row;

                            if (
                                (ExistingLocation.StreetName == NewLocation.StreetName)
                                && (ExistingLocation.Locality == NewLocation.Locality)
                                && (ExistingLocation.Address3 == NewLocation.Address3)
                                && (ExistingLocation.County == NewLocation.County)
                                && (ExistingLocation.City == NewLocation.City)
                                && (ExistingLocation.CountryCode == NewLocation.CountryCode)
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
                        }

                        //TODOWBxxx: is it ok that I took this out of the if statement above?
                        // if location already exists: make sure we point the partner location record to the existing location
                        if (IgnoreImportLocationForCsv)
                        {
                            PartnerLocationRow.SiteKey = UserSelectedRow.SiteKey;
                            PartnerLocationRow.LocationKey = UserSelectedRow.LocationKey;
                        }
                        else
                        {
                            PartnerLocationRow.SiteKey = NewLocation.SiteKey;
                            PartnerLocationRow.LocationKey = NewLocation.LocationKey;
                        }

                        ANewPartnerDS.PPartnerLocation.ImportRow(PartnerLocationRow);
                        // Set the PartnerKey for the new Row (TODOWBxxx: why does this have to be set after ImportRow?)
                        int NewRow = ANewPartnerDS.PPartnerLocation.Rows.Count - 1;
                        ANewPartnerDS.PPartnerLocation[NewRow].PartnerKey = ANewPartnerKey;
                    }
                }
            }
        }

        private void AddCommentSeq(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerComment, FMainDS.PPartnerComment,
                PPartnerCommentTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddStaffData(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmStaffData, FMainDS.PmStaffData,
                PmStaffDataTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddJobAssignment(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
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
                PmJobAssignmentTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddPmPersonLanguage(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonLanguage, FMainDS.PmPersonLanguage,
                PmPersonLanguageTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddPreviousExperience(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPastExperience, FMainDS.PmPastExperience,
                PmPastExperienceTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddPassport(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPassportDetails, FMainDS.PmPassportDetails,
                PmPassportDetailsTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddPersonalData(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonalData, FMainDS.PmPersonalData,
                PmPersonalDataTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddPersonalDocument(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmDocument, FMainDS.PmDocument,
                PmDocumentTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddProfessionalData(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonQualification, FMainDS.PmPersonQualification,
                PmPersonQualificationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddPersonalEvaluation(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonEvaluation, FMainDS.PmPersonEvaluation,
                PmPersonEvaluationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddSpecialNeeds(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmSpecialNeed, FMainDS.PmSpecialNeed,
                PmSpecialNeedTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddPartnerType(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerType, FMainDS.PPartnerType,
                PPartnerTypeTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddPartnerAttribute(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerAttribute, FMainDS.PPartnerAttribute,
                PPartnerAttributeTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddInterest(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerInterest, FMainDS.PPartnerInterest,
                PPartnerInterestTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

/*
 *      private void AddVision(Int64 OrigPartnerKey, Int64 NewPartnerKey, ref PartnerImportExportTDS NewPartnerDS)
 *      {
 *          ImportRecordsByPartnerKey(NewPartnerDS.PmPersonVision, FMainDS.PmPersonVision,
 *              PmPersonVisionTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey);
 *      }
 */
        private void AddGiftDestination(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerGiftDestination, FMainDS.PPartnerGiftDestination,
                PPartnerGiftDestinationTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddUnitstructure(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
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
                Boolean PartnerExistsInDB = TServerLookup.TMPartner.VerifyPartner(ParentKey);

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

        private void AddBankingDetails(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
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

        private void AddBuilding(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PcBuilding, FMainDS.PcBuilding,
                PcBuildingTable.GetVenueKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddRoom(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PcRoom, FMainDS.PcRoom,
                PcRoomTable.GetVenueKeyDBName(), AOrigPartnerKey, ANewPartnerKey);
        }

        private void AddSkill(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PmPersonSkill, FMainDS.PmPersonSkill,
                PmPersonSkillTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
        }

        private void AddSubscriptions(Int64 AOrigPartnerKey,
            Int64 ANewPartnerKey,
            ref PartnerImportExportTDS ANewPartnerDS,
            bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PSubscription, FMainDS.PSubscription,
                PSubscriptionTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);

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

        private void AddContacts(Int64 AOrigPartnerKey, Int64 ANewPartnerKey, ref PartnerImportExportTDS ANewPartnerDS, bool AUpdateExistingRecord)
        {
            ImportRecordsByPartnerKey(ANewPartnerDS.PPartnerContact, FMainDS.PPartnerContact,
                PPartnerContactTable.GetPartnerKeyDBName(), AOrigPartnerKey, ANewPartnerKey, AUpdateExistingRecord);
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
            PartnerImportExportTDSOutputDataRow outputDataRow = null;
            bool gotOutputRow = false;

            FMainDS.OutputData.DefaultView.RowFilter = string.Format("{0}={1}",
                APartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON ?
                PartnerImportExportTDSOutputDataTable.GetOutputPersonPartnerKeyDBName() :
                PartnerImportExportTDSOutputDataTable.GetOutputFamilyPartnerKeyDBName(),
                APartnerRow.PartnerKey);

            if (FMainDS.OutputData.DefaultView.Count > 0)
            {
                outputDataRow = (PartnerImportExportTDSOutputDataRow)FMainDS.OutputData.DefaultView[0].Row;
                gotOutputRow = true;
            }

            Int64 OrigPartnerKey = APartnerRow.PartnerKey;
            Int64 NewPartnerKey = OrigPartnerKey;
            bool UpdateExistingRecord = false;

            // If the import file had a negative PartnerKey, I need to create a new one here,
            // and use it on all the dependent tables.

            if ((FFileFormat == TImportFileFormat.csv)
                && (ExistingPartnerKey > 0)
                && (OrigPartnerKey < 0))
            {
                // If csv import file did not have a partner key and we want to update an existing partner
                // then we need to retrieve existing data for that partner in order to determine which records
                // have to be added/updated.
                PartnerImportExportTDS ExistingPartnerTDS = TRemote.MPartner.ImportExport.WebConnectors.ReadPartnerDataForCSV(ExistingPartnerKey,
                    FCSVColumns);
                AddPartnerForCSVPartnerUpdate(OrigPartnerKey, ExistingPartnerKey, ref FMainDS, ExistingPartnerTDS);
                AddAddressForCSVPartnerUpdate(OrigPartnerKey, ExistingPartnerKey, ref FMainDS, ExistingPartnerTDS);
                AddShortTermAppForCSVPartnerUpdate(OrigPartnerKey, ExistingPartnerKey, ref FMainDS, ExistingPartnerTDS);
                AddSpecialTypeForCSVPartnerUpdate(OrigPartnerKey, ExistingPartnerKey, ref FMainDS, ExistingPartnerTDS);
                AddSubscriptionForCSVPartnerUpdate(OrigPartnerKey, ExistingPartnerKey, ref FMainDS, ExistingPartnerTDS);
                AddContactForCSVPartnerUpdate(OrigPartnerKey, ExistingPartnerKey, ref FMainDS, ExistingPartnerTDS);
                AddPassportForCSVPartnerUpdate(OrigPartnerKey, ExistingPartnerKey, ref FMainDS, ExistingPartnerTDS);
                AddSpecialNeedsForCSVPartnerUpdate(OrigPartnerKey, ExistingPartnerKey, ref FMainDS, ExistingPartnerTDS);
            }

            if (ExistingPartnerKey > 0)  // This ExistingPartnerKey has been set by the UseSelectedPerson button.
            {
                NewPartnerKey = ExistingPartnerKey;
                UpdateExistingRecord = true;
                ExistingPartnerKey = -1; // Don't use this next time!
            }
            else
            {
                // if original partner key isn't included or a different partner already exists with the same key
                if ((OrigPartnerKey < 0) || TServerLookup.TMPartner.VerifyPartner(OrigPartnerKey))
                {
                    NewPartnerKey = TRemote.MPartner.Partner.WebConnectors.NewPartnerKey(-1);
                }
            }

            NewPartnerDS.PPartner.ImportRow(APartnerRow);

            if (UpdateExistingRecord)
            {
                NewPartnerDS.PPartner[0].PartnerKey = UserSelectedRow.PartnerKey;
                NewPartnerDS.PPartner[0].DateCreated = UserSelectedRow.DateCreated;
                NewPartnerDS.PPartner[0].CreatedBy = UserSelectedRow.CreatedBy;
                NewPartnerDS.PPartner[0].ModificationId = UserSelectedRow.ModificationId;

// if I leave in the following line then any data for PPartner records does not get into db
                //NewPartnerDS.PPartner[0].AcceptChanges(); // This should reset the RowState, allowing me to Update rather than Add //TODOWBxxx???
            }
            else
            {
                NewPartnerDS.PPartner[0].PartnerKey = NewPartnerKey;
            }

            if (gotOutputRow)
            {
                if (UpdateExistingRecord)
                {
                    outputDataRow.ImportStatus = "U";       // Updated
                }
                else
                {
                    outputDataRow.ImportStatus = "A";       // Added
                }
            }

            if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_CHURCH)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PChurch, FMainDS.PChurch,
                    PChurchTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
                NewPartnerDS.PChurch[0].ChurchName = APartnerRow.PartnerShortName;
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

                if (gotOutputRow)
                {
                    PFamilyRow newFamily = (PFamilyRow)NewPartnerDS.PFamily.Rows[0];
                    outputDataRow.PartnerShortName = Calculations.DeterminePartnerShortName(newFamily.FamilyName,
                        newFamily.Title,
                        newFamily.FirstName);
                    outputDataRow.OutputFamilyPartnerKey = newFamily.PartnerKey;
                }
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PPerson, FMainDS.PPerson,
                    PPersonTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
                NewPartnerDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}", PPersonTable.GetPartnerKeyDBName(), NewPartnerKey);
                PPersonRow newPerson = (PPersonRow)NewPartnerDS.PPerson.DefaultView[0].Row;

                if (gotOutputRow)
                {
                    outputDataRow.PartnerShortName = Calculations.DeterminePartnerShortName(newPerson.FamilyName,
                        newPerson.Title,
                        newPerson.FirstName);
                    outputDataRow.OutputPersonPartnerKey = newPerson.PartnerKey;
                }

                // If there's an associated PFamily record that I've not imported yet, I could try to do that now,
                // but it's problematic because it might end up getting imported twice.
                // Anyway, I should not come to here because the family should have been imported first.
                Int64 RelatedFamilyKey = newPerson.FamilyKey;

                if (RelatedFamilyKey < 0) // There's a related family that's not been imported
                {
                    AddStatus("Import Problem: PPerson record with no related PFamily.");

                    if (gotOutputRow)
                    {
                        outputDataRow.ImportStatus = "E";       // Error
                    }

                    return 0;
                }
                else if (gotOutputRow)
                {
                    outputDataRow.OutputFamilyPartnerKey = RelatedFamilyKey;
                }
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.POrganisation, FMainDS.POrganisation,
                    POrganisationTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
                NewPartnerDS.POrganisation[0].OrganisationName = APartnerRow.PartnerShortName;
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PUnit, FMainDS.PUnit,
                    PUnitTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
                NewPartnerDS.PUnit[0].UnitName = APartnerRow.PartnerShortName;

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
                NewPartnerDS.PVenue[0].VenueName = APartnerRow.PartnerShortName;
            }
            else if (NewPartnerDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
            {
                ImportRecordsByPartnerKey(NewPartnerDS.PBank, FMainDS.PBank,
                    PBankTable.GetPartnerKeyDBName(), OrigPartnerKey, NewPartnerKey, UpdateExistingRecord);
                NewPartnerDS.PBank[0].BranchName = APartnerRow.PartnerShortName;
            }

            // Add special types etc
            AddAbility(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddApplication(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddAddresses(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddCommentSeq(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddStaffData(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddJobAssignment(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddPmPersonLanguage(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddPreviousExperience(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddPassport(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddPersonalDocument(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddPersonalData(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddProfessionalData(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddPersonalEvaluation(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddSkill(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddSpecialNeeds(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddPartnerType(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);

            foreach (PPartnerTypeRow PartnerTypeRow in NewPartnerDS.PPartnerType.Rows)
            {
                PartnerTypeRow.PartnerKey = NewPartnerKey;
            }

            AddPartnerAttribute(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddInterest(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
//          AddVision(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddGiftDestination(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);

            AddUnitstructure(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddBuilding(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddRoom(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddSubscriptions(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);
            AddContacts(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);

            AddBankingDetails(OrigPartnerKey, NewPartnerKey, ref NewPartnerDS, UpdateExistingRecord);

            TVerificationResultCollection VerificationResult;
            bool CommitRes = TRemote.MPartner.ImportExport.WebConnectors.CommitChanges(NewPartnerDS,
                FFileFormat,
                chkReplaceAddress.Checked,
                0,
                UserSelLocationKey,
                out VerificationResult);
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

                if (ResultString.Length > 0)
                {
                    MessageBox.Show(ResultString, Catalog.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (gotOutputRow)
                {
                    // Set the import status to Error
                    outputDataRow.ImportStatus = "E";
                }
            }

            // new record has been created, now load the next record
            if (StepAfterImport)
            {
                NextRecord();
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

            CreateNewPartner(false);
        }

        private void UseSelectedAddress(Object sender, EventArgs e)
        {
            AddStatus("<Create Partner with shared address>" + Environment.NewLine);

            CreateNewPartner(true);
        }

        private void CreateNewPartner(bool AUseSelectedAddress = false)
        {
            string ShortName = null;
            TPartnerClass PartnerClass;
            bool Merged;
            bool UserCanAccessPartner;

            // if a partner with this partner key already exists
            if ((FCurrentPartner.PartnerKey >= 0)
                && TServerLookup.TMPartner.VerifyPartner(
                    FCurrentPartner.PartnerKey, out ShortName, out PartnerClass, out Merged, out UserCanAccessPartner))
            {
                if (MessageBox.Show(string.Format(Catalog.GetString("A partner with this partner key already exists in the database.{0}{0}" +
                                "Partner Key: {1}{0}Partner Name: {2}{0}Partner Class: {3}{0}{0}" +
                                "Do you want to continue creating this new partner with a new partner key?"),
                            "\r\n", FCurrentPartner.PartnerKey.ToString("0000000000"), ShortName, PartnerClass),
                        Catalog.GetString("Create Partner"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    AddStatus(Catalog.GetString("Create new partner cancelled.") + "\r\n\r\n");
                    return;
                }
                else
                {
                    // reload data from scratch using new partner key
                    TVerificationResultCollection VerificationResult = new TVerificationResultCollection();
                    Int64 NewPartnerKey = TRemote.MPartner.Partner.WebConnectors.NewPartnerKey(-1);
                    LoadDataSet(ref VerificationResult, FCurrentPartner.PartnerKey, NewPartnerKey);
                    FCurrentPartner = FMainDS.PPartner[FCurrentNumberOfRecord - 1];

                    if ((FCurrentPartner != null)
                        && (FCurrentPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON))
                    {
                        // I need to get the Person linked to this Partner and set it as the current person
                        FMainDS.PPerson.DefaultView.RowFilter = String.Format("{0}={1}",
                            PPersonTable.GetPartnerKeyDBName(), FCurrentPartner.PartnerKey);
                        FCurrentPerson = (PPersonRow)FMainDS.PPerson.DefaultView[0].Row;
                    }
                    else
                    {
                        FCurrentPerson = null;
                    }
                }
            }

            if (AUseSelectedAddress)
            {
                // This expression uses the first location key in the imported data, which matches the method used
                // to create the data grid from which the address was selected.
                // Only this one address will be shared - if there are also other addresses,
                // these will be created. (And checks on CommitChanges may catch them.) But in most cases it's probably useful.
                ((PPartnerLocationRow)FMainDS.PPartnerLocation.DefaultView[0].Row).LocationKey = UserSelLocationKey;
            }

            CreateOrUpdatePartner(FCurrentPartner, true);
        }

        #region Output Options

        private void GetImportIDHeaderText(XmlDocument ADoc)
        {
            XmlNode node = ADoc.SelectSingleNode("//Element");

            if (node != null)
            {
                if (node.Attributes["ImportID"] != null)
                {
                    FImportIDHeaderText = "ImportID";
                }
                else if (node.Attributes["EnrolmentID"] != null)
                {
                    // British spelling
                    FImportIDHeaderText = "EnrolmentID";
                }
                else if (node.Attributes["EnrollmentID"] != null)
                {
                    // American spelling
                    FImportIDHeaderText = "EnrollmentID";
                }
            }
        }

        private bool FCreateCSVFile, FIncludeFamiliesInCSV, FCreateExtract, FIncludeFamiliesInExtract;
        private string FCsvOutFilePath, FExtractName, FExtractDescription;
        private string FSelectedSeparator = ",";

        private bool GetFinishOptions(out bool ACreateCSVFile, out bool AIncludeFamiliesInCSV, out string ACsvOutFilePath,
            out bool ACreateExtract, out string AExtractName, out string AExtractDescription, out bool AIncludeFamiliesInExtract)
        {
            ACreateCSVFile = AIncludeFamiliesInCSV = ACreateExtract = AIncludeFamiliesInExtract = false;
            ACsvOutFilePath = AExtractName = AExtractDescription = string.Empty;

            // Show the Finish Options dialog
            TFrmPartnerImportFinishOptionsDialog dialog = new TFrmPartnerImportFinishOptionsDialog(this);

            // CSV File
            string outputCSVPath = Path.Combine(
                Path.GetDirectoryName(Path.GetFullPath(txtFilename.Text)),
                Path.GetFileNameWithoutExtension(Path.GetFullPath(txtFilename.Text)) + "-out.csv");

            //Extracts
            string suggestedName = Path.GetFileNameWithoutExtension(txtFilename.Text);
            string suggestedDescription = "Imported on " + DateTime.Today.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString();

            dialog.SetParameters(suggestedName, suggestedDescription, outputCSVPath);
            dialog.StartPosition = FormStartPosition.CenterParent;

            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return false;
            }

            // Get the dialog settings?
            dialog.GetResults(out ACreateCSVFile, out AIncludeFamiliesInCSV, out ACsvOutFilePath,
                out ACreateExtract, out AExtractName, out AExtractDescription, out AIncludeFamiliesInExtract);

            return true;
        }

        private void DoFinishOptions(bool AStopped)
        {
            if (string.Compare(Path.GetExtension(txtFilename.Text), ".csv", true) != 0)
            {
                // We only have options when the file format was CSV
                return;
            }

            if ((FCreateCSVFile == false) && (FCreateExtract == false))
            {
                // Nothing to do
                string msgNothing = AStopped ? Catalog.GetString("The Import was stopped.") : Catalog.GetString("The Import was completed.");
                msgNothing += Catalog.GetString("  Please review the messages to see which rows were successfully imported.");
                MessageBox.Show(msgNothing, Catalog.GetString("Import Partners"), MessageBoxButtons.OK);
                return;
            }

            string msgTitle = Catalog.GetString("Import Options");
            string msg = Catalog.GetString("The selected Finish Actions have been completed.");

            if (FCreateCSVFile)
            {
                int numLines, numImports;
                CreateOutputCSVFile(FCsvOutFilePath, FIncludeFamiliesInCSV, out numLines, out numImports);
                msg += string.Format(Catalog.GetString(
                        "{0}  - An output CSV file containing {1} entries was created at{0}      '{2}'{0}    {3} entries were successfully imported"),
                    Environment.NewLine, numLines, FCsvOutFilePath, numImports);
            }

            if (FCreateExtract)
            {
                int keyCount;

                if (CreateOutputExtract(FExtractName, FExtractDescription, FIncludeFamiliesInExtract, out keyCount))
                {
                    msg += string.Format(Catalog.GetString("{0}  - An extract was created containing {1} keys"), Environment.NewLine, keyCount);
                }
                else
                {
                    msg += string.Format(Catalog.GetString("{0}  - The server failed to create the extract"), Environment.NewLine);
                }
            }

            MessageBox.Show(msg, msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Call this method to check if the extract name already exists
        /// </summary>
        /// <param name="AExtractName"></param>
        /// <returns>True of the name does not exist</returns>
        public bool ValidateExtractName(string AExtractName)
        {
            return TRemote.MPartner.Partner.WebConnectors.ExtractExists(AExtractName) == false;
        }

        private void CreateOutputCSVFile(string APath, bool AIncludeFamiliesOfPersons, out int ALinesWrittenCount, out int ARowsImportedCount)
        {
            using (StreamWriter sw = new StreamWriter(APath))
            {
                bool gotImportIDs = FImportIDHeaderText.Length > 0;
                string header = string.Empty;

                if (gotImportIDs)
                {
                    header += string.Format("\"{0}\"{1}", FImportIDHeaderText, FSelectedSeparator);
                }

                header += string.Format("\"{0}\"{1}\"{2}\"{1}\"{3}\"{1}\"PartnerShortName\"{1}\"ImportStatus\"{1}\"{4}\"",
                    MPartnerConstants.PARTNERIMPORT_PARTNERCLASS,
                    FSelectedSeparator,
                    MPartnerConstants.PARTNERIMPORT_FAMILYPARTNERKEY,
                    MPartnerConstants.PARTNERIMPORT_PERSONPARTNERKEY,
                    MPartnerConstants.PARTNERIMPORT_RECORDIMPORTED);
                sw.WriteLine(header);

                ALinesWrittenCount = 0;
                ARowsImportedCount = 0;

                for (int i = 0; i < FMainDS.OutputData.Rows.Count; i++)
                {
                    PartnerImportExportTDSOutputDataRow row = (PartnerImportExportTDSOutputDataRow)FMainDS.OutputData.Rows[i];

                    if ((row.IsFromFile == false) && (AIncludeFamiliesOfPersons == false))
                    {
                        // The row was not part of the original file, so will be a new FAMILY for a PERSON
                        continue;
                    }

                    string outText = string.Empty;

                    if (gotImportIDs)
                    {
                        outText += string.Format("\"{0}\"{1}", row.ImportID, FSelectedSeparator);
                    }

                    string familyKey = string.Empty;
                    string personKey = string.Empty;
                    string wasImported = string.Empty;

                    if ((row.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY) || (row.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON))
                    {
                        if (row.OutputFamilyPartnerKey > 0)
                        {
                            familyKey = row.OutputFamilyPartnerKey.ToString("0000000000");
                        }
                    }

                    if (row.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        if (row.OutputPersonPartnerKey > 0)
                        {
                            personKey = row.OutputPersonPartnerKey.ToString("0000000000");
                        }
                    }

                    if ((row.ImportStatus == "N") || (row.ImportStatus == "E"))
                    {
                        // The row was skipped, the import was stopped, or there was an error
                        wasImported = "no";
                    }
                    else
                    {
                        wasImported = "yes";
                        ARowsImportedCount++;
                    }

                    outText += string.Format("\"{0}\"{1}\"{2}\"{1}\"{3}\"{1}\"{4}\"{1}\"{5}\"{1}\"{6}\"",
                        row.PartnerClass,
                        FSelectedSeparator,
                        familyKey,
                        personKey,
                        row.PartnerShortName,
                        row.ImportStatus,
                        wasImported);

                    sw.WriteLine(outText);
                    ALinesWrittenCount++;
                }

                sw.Close();
            }
        }

        private bool CreateOutputExtract(string AExtractName, string AExtractDescription, bool AIncludeFamiliesOfPersons, out int AKeyCount)
        {
            AKeyCount = 0;

            int extractID = -1;
            int keyCountInExtract = -1;
            List <long>ignoredKeysList = null;

            DataTable table = new DataTable();
            table.Columns.Add("Key", typeof(System.Int64));

            int badRows = 0;

            for (int i = 0; i < FMainDS.OutputData.Rows.Count; i++)
            {
                PartnerImportExportTDSOutputDataRow outputDataRow = (PartnerImportExportTDSOutputDataRow)FMainDS.OutputData.Rows[i];

                if ((outputDataRow.IsFromFile == false) && (AIncludeFamiliesOfPersons == false))
                {
                    continue;
                }

                if ((outputDataRow.ImportStatus == "N")
                    || (outputDataRow.ImportStatus == "E")
                    || ((outputDataRow.OutputFamilyPartnerKey <= 0) && (outputDataRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY))
                    || ((outputDataRow.OutputPersonPartnerKey <= 0) && (outputDataRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)))
                {
                    badRows++;
                }
                else
                {
                    DataRow dr = table.NewRow();

                    if (outputDataRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
                    {
                        dr[0] = outputDataRow.OutputFamilyPartnerKey;
                    }
                    else
                    {
                        dr[0] = outputDataRow.OutputPersonPartnerKey;
                    }

                    table.Rows.Add(dr);
                }
            }

            if (badRows > 0)
            {
                string msg = string.Format(Catalog.GetPluralString(
                        "{0} row could not be imported.", "{0} rows could not be imported.", badRows), badRows);
                msg += Catalog.GetString(" Do you want to create an extract that only contains the successfully imported rows?");

                if (MessageBox.Show(msg, Catalog.GetString(""), MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return false;
                }
            }

            if (TRemote.MPartner.Partner.WebConnectors.CreateNewExtractFromPartnerKeys(ref extractID, AExtractName, AExtractDescription,
                    table, 0, false, false, false, out keyCountInExtract, out ignoredKeysList))
            {
                AKeyCount = keyCountInExtract;
                return true;
            }

            return false;
        }

        private void LoadUserDefaults(TDlgSelectCSVSeparator ADialog)
        {
            // Start with separator and number format
            CultureInfo myCulture = CultureInfo.CurrentCulture;;

            string defaultImpOptions = myCulture.TextInfo.ListSeparator + TDlgSelectCSVSeparator.NUMBERFORMAT_EUROPEAN;

            if (myCulture.TextInfo.CultureName.EndsWith("-US"))
            {
                defaultImpOptions = myCulture.TextInfo.ListSeparator + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN;
            }

            string impOptions = TUserDefaults.GetStringDefault("Imp Options", defaultImpOptions);

            if (impOptions.Length > 0)
            {
                ADialog.SelectedSeparator = impOptions.Substring(0, 1);
            }

            if (impOptions.Length > 1)
            {
                ADialog.NumberFormat = impOptions.Substring(1);
            }

            // Now do date format
            string dateFormat = TUserDefaults.GetStringDefault("Imp Date", "MDY");

            // mdy and dmy have been the old default settings in Petra 2.x
            if (dateFormat.ToLower() == "mdy")
            {
                dateFormat = "MM/dd/yyyy";
            }

            if (dateFormat.ToLower() == "dmy")
            {
                dateFormat = "dd/MM/yyyy";
            }

            ADialog.DateFormat = dateFormat;
        }

        private void SaveUserDefaults(TDlgSelectCSVSeparator ADialog)
        {
            string impOptions = ADialog.SelectedSeparator;

            impOptions += ADialog.NumberFormat;
            TUserDefaults.SetDefault("Imp Options", impOptions);
            TUserDefaults.SetDefault("Imp Date", ADialog.DateFormat);
            TUserDefaults.SaveChangedUserDefaults();
        }

        #endregion
    }
}
