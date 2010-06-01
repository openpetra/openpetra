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
using System.Windows.Forms;
using System.Collections.Specialized;
using Mono.Unix;
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
        }

        XmlNode FCurrentPartnerNode = null;
        Int32 FCurrentNumberOfRecord = 0;
        Int32 FTotalNumberOfRecords = 0;
        private void OpenFile(System.Object sender, EventArgs e)
        {
            if (!FPetraUtilsObject.IsEnabled("actStartImport"))
            {
                MessageBox.Show(Catalog.GetString("Please cancel the current import before selecting a different file"));
                return;
            }

            string filename;

            XmlDocument doc = TImportExportDialogs.ImportWithDialog(Catalog.GetString("Select the file for importing partners"), out filename);

            if (doc != null)
            {
                txtFilename.Text = filename;

                XmlNode root = doc.FirstChild.NextSibling;
                FCurrentPartnerNode = root.FirstChild;
            }
        }

        private void StartImport(Object sender, EventArgs e)
        {
            if (FCurrentPartnerNode == null)
            {
                OpenFile(null, null);
            }

            if (FCurrentPartnerNode == null)
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
            FTotalNumberOfRecords = FCurrentPartnerNode.ParentNode.ChildNodes.Count;

            FCurrentPartnerNode = SkipImportedPartners(FCurrentPartnerNode);

            pnlImportRecord.Enabled = true;

            DisplayCurrentRecord();
        }

        private void DisplayCurrentRecord()
        {
            if (FCurrentPartnerNode == null)
            {
                // have we finished importing?
                if (FCurrentNumberOfRecord > 0)
                {
                    txtCurrentRecordStatus.Text =
                        String.Format(Catalog.GetString("{0} Records processed - Import finished"),
                            FTotalNumberOfRecords);
                    pnlActions.Enabled = false;
                    this.FPetraUtilsObject.EnableAction("actStartImport", true);
                    this.FPetraUtilsObject.EnableAction("actCancelImport", false);
                }

                grdParsedValues.DataSource = null;
                grdMatchingRecords.DataSource = null;
                pnlImportRecord.Enabled = false;
                return;
            }

            DataTable ValuePairs = new DataTable();

            ValuePairs.Columns.Add(new DataColumn("Attribute", typeof(string)));
            ValuePairs.Columns.Add(new DataColumn("Value", typeof(string)));

            if (FCurrentPartnerNode != null)
            {
                foreach (XmlAttribute attr in FCurrentPartnerNode.Attributes)
                {
                    DataRow valuePair = ValuePairs.NewRow();
                    valuePair["Attribute"] = attr.Name;
                    valuePair["Value"] = attr.Value;
                    ValuePairs.Rows.Add(valuePair);
                }

                txtCurrentRecordStatus.Text =
                    String.Format(Catalog.GetString("Processing record {0} of {1}"),
                        FCurrentNumberOfRecord,
                        FTotalNumberOfRecords);
            }

            grdParsedValues.Columns.Clear();
            grdParsedValues.AddTextColumn(Catalog.GetString("Attribute"), ValuePairs.Columns["Attribute"], 150);
            grdParsedValues.AddTextColumn(Catalog.GetString("Value"), ValuePairs.Columns["Value"], 150);
            ValuePairs.DefaultView.AllowNew = false;
            grdParsedValues.DataSource = new DevAge.ComponentModel.BoundDataView(ValuePairs.DefaultView);

            // get all partners with same surname in that city
            PartnerFindTDS result = TRemote.MPartner.Partner.WebConnectors.FindPartners("",
                TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_FAMILYNAME),
                TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_CITY),
                new StringCollection());

            grdMatchingRecords.Columns.Clear();
            grdMatchingRecords.AddTextColumn(Catalog.GetString("Class"), result.SearchResult.ColumnPartnerClass, 50);
            grdMatchingRecords.AddTextColumn(Catalog.GetString("Name"), result.SearchResult.ColumnPartnerShortName, 200);
            grdMatchingRecords.AddTextColumn(Catalog.GetString("Address"), result.SearchResult.ColumnStreetName, 200);
            grdMatchingRecords.AddTextColumn(Catalog.GetString("City"), result.SearchResult.ColumnCity, 150);
            result.SearchResult.DefaultView.AllowNew = false;
            grdMatchingRecords.DataSource = new DevAge.ComponentModel.BoundDataView(result.SearchResult.DefaultView);
        }

        private void CancelImport(Object sender, EventArgs e)
        {
            FCurrentPartnerNode = null;
            this.FPetraUtilsObject.EnableAction("actStartImport", true);
            this.FPetraUtilsObject.EnableAction("actCancelImport", false);
        }

        private XmlNode SkipImportedPartners(XmlNode ACurrentNode)
        {
            // TODO check for import settings, which partners to skip etc
            // TODO: CurrentNumberOfRecord and TotalRecords different?

            return ACurrentNode;
        }

        private void SkipRecord(Object sender, EventArgs e)
        {
            if (FCurrentPartnerNode != null)
            {
                FCurrentPartnerNode = FCurrentPartnerNode.NextSibling;

                FCurrentNumberOfRecord++;

                FCurrentPartnerNode = SkipImportedPartners(FCurrentPartnerNode);
            }

            DisplayCurrentRecord();
        }

        private void CreateNewFamily(Object sender, EventArgs e)
        {
            if (FCurrentPartnerNode == null)
            {
                return;
            }

            if (TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_PARTNERKEY).Length > 0)
            {
                // it would not make any sense to create a partner if there is already a partner key
                return;
            }

            PartnerEditTDS MainDS = TPartnerImportLogic.CreateNewFamily(FCurrentPartnerNode);

            TVerificationResultCollection VerificationResult;

            if (!TRemote.MPartner.Partner.WebConnectors.SavePartner(MainDS, out VerificationResult))
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