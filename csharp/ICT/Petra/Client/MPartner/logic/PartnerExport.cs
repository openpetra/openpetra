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
using System.Collections.Specialized;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MPartner;

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
        /// <param name="ASiteKey">SiteKey of the Location.</param>
        /// <param name="ALocationKey">LocationKey of the Location.</param>
        public static void ExportSinglePartner(Int64 APartnerKey, Int64 ASiteKey, int ALocationKey)
        {
            bool Result = false;
            StringCollection ASpecificBuildingInfo = null;
            String ExtFormattedDocument;
            
            string FileName = TImportExportDialogs.GetExportFilename(Catalog.GetString("Save Partners into File"));

            if (FileName.Length > 0)
            {
                if (FileName.EndsWith("ext"))
                {
                    ExtFormattedDocument = TRemote.MPartner.ImportExport.WebConnectors.GetExtFileHeader();

                    ExtFormattedDocument += TRemote.MPartner.ImportExport.WebConnectors.ExportPartnerExt(
                        APartnerKey, ASiteKey, ALocationKey, false, ASpecificBuildingInfo);

                    ExtFormattedDocument += TRemote.MPartner.ImportExport.WebConnectors.GetExtFileFooter();

                    Result = TImportExportDialogs.ExportTofile(ExtFormattedDocument, FileName);

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
    }
}