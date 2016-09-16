//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.Threading;

using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Logic
{
    /// <summary>
    /// Contains logic for the exporting of a Partner.
    /// </summary>
    public class TPartnerExportLogic
    {
        /// <summary>
        /// Exports a single Partner.
        /// </summary>
        /// <param name="APartnerKey">Partner Key of the Partner.</param>
        /// <param name="APartnerClass">Class of partner for the specified key</param>
        /// <param name="ASiteKey">SiteKey of the Location.</param>
        /// <param name="ALocationKey">LocationKey of the Location.</param>
        /// <param name="AOldPetraFormat">Set to true if old format should be used.</param>
        public static void ExportSinglePartner(Int64 APartnerKey, String APartnerClass, Int64 ASiteKey, int ALocationKey, Boolean AOldPetraFormat)
        {
            bool Result = false;
            String ExtFormattedDocument;

            string FileName = TImportExportDialogs.GetExportFilename(Catalog.GetString("Save Partners into File"));

            if (FileName.Length > 0)
            {
                if (FileName.EndsWith("ext"))
                {
                    bool ExportFamiliesPersons = false;

                    if (string.Compare(APartnerClass, TPartnerClass.FAMILY.ToString(), true) == 0)
                    {
                        if (MessageBox.Show(
                                Catalog.GetString("Do you want to also export all PERSON records associated with this FAMILY?"),
                                Catalog.GetString("Export Partner"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            ExportFamiliesPersons = true;
                        }
                    }

                    ExtFormattedDocument = TRemote.MPartner.ImportExport.WebConnectors.ExportSinglePartnerExt(APartnerKey,
                        ExportFamiliesPersons,
                        AOldPetraFormat);

                    Result = TImportExportDialogs.ExportTofile(ExtFormattedDocument, FileName, AOldPetraFormat);

                    if (!Result)
                    {
                        MessageBox.Show(Catalog.GetString("Export of Partner failed!"), Catalog.GetString(
                                "Export Partner"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(Catalog.GetString("Export of Partner finished"), Catalog.GetString(
                                "Export Partner"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(Catalog.GetString("Export with this format is not yet supported!"), Catalog.GetString(
                            "Export Partner"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Export Partners from an Extract.
        /// </summary>
        /// <param name="AExtractId">Extract Identifier.</param>
        /// <param name="AOldPetraFormat">Set to true if old format should be used.</param>
        public static Boolean ExportPartnersInExtract(int AExtractId, Boolean AOldPetraFormat)
        {
            Boolean Result = false;

            String FileName = TImportExportDialogs.GetExportFilename(Catalog.GetString("Save Partners into File"));

            if (AExtractId < 0)
            {
                return false;
            }

            if (FileName.Length > 0)
            {
                bool ExportFamiliesPersons = false;
                bool ContainsFamily = TRemote.MPartner.ImportExport.WebConnectors.CheckExtractContainsFamily(AExtractId);

                if (ContainsFamily)
                {
                    if (MessageBox.Show(
                            Catalog.GetString("When exporting a FAMILY record do you want to also export all associated PERSON records?"),
                            Catalog.GetString("Export Partners"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ExportFamiliesPersons = true;
                    }
                }

                if (FileName.EndsWith("ext"))
                {
                    string Doc = string.Empty;

                    // run in thread so we can have Progress Dialog
                    Thread t = new Thread(() => ExportExtractToFile(AExtractId, ExportFamiliesPersons, ref Doc, AOldPetraFormat));

                    using (TProgressDialog dialog = new TProgressDialog(t))
                    {
                        dialog.ShowDialog();
                    }

                    // null if the user cancelled the operation
                    if (Doc == null)
                    {
                        MessageBox.Show(Catalog.GetString("Export cancelled."), Catalog.GetString(
                                "Export Partners"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    Result = TImportExportDialogs.ExportTofile(Doc, FileName,AOldPetraFormat);

                    if (!Result)
                    {
                        MessageBox.Show(Catalog.GetString("Export of Partners in Extract failed!"), Catalog.GetString(
                                "Export Partners"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(Catalog.GetString("Export of Partners in Extract finished"), Catalog.GetString(
                                "Export Partners"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // XmlDocument doc = new XmlDocument();
                    MessageBox.Show(Catalog.GetString("Export with this format is not yet supported!"), Catalog.GetString(
                            "Export Partners"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // doc.LoadXml(TRemote.MPartner.ImportExport.WebConnectors.ExportExtractPartners(GetSelectedDetailRow().ExtractId, false));
                    // Result = TImportExportDialogs.ExportTofile(doc, FileName);
                }
                return Result;
            }
            return false;
        }

        private static void ExportExtractToFile(int AExtractId, bool AExportFamiliesPersons, ref string ADoc, Boolean AOldPetraFormat)
        {
            ADoc = TRemote.MPartner.ImportExport.WebConnectors.ExportExtractPartnersExt(
                AExtractId, AExportFamiliesPersons, AOldPetraFormat);
        }
    }
}