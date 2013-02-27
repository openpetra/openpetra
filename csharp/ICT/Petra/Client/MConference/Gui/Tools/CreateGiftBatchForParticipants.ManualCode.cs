//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;

namespace Ict.Petra.Client.MConference.Gui.Tools
{
    public partial class TFrmCreateGiftBatchForParticipants
    {
        private void ExportGiftBatch(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();

            sd.AddExtension = true;
            sd.DefaultExt = ".csv";
            sd.Filter = "Gift batches (*.csv)|*.csv";

            if (sd.ShowDialog() == DialogResult.OK)
            {
                string content = TRemote.MConference.WebConnectors.CreateGiftTransactions(
                    txtPasteData.Text,
                    TAppSettingsManager.GetInt64("ConferenceTool.UnknownPartnerKey"),
                    TAppSettingsManager.GetValue("ConferenceTool.UnknownPartnerName"),
                    TAppSettingsManager.GetInt64("ConferenceTool.DefaultPartnerLedger"),
                    TAppSettingsManager.GetBoolean("ConferenceTool.ValidatePartnerKeys", true),
                    TAppSettingsManager.GetValue("ConferenceTool.TemplateApplication"),
                    TAppSettingsManager.GetValue("ConferenceTool.TemplateManualApplication"),
                    TAppSettingsManager.GetValue("ConferenceTool.TemplateConferenceFee"),
                    TAppSettingsManager.GetValue("ConferenceTool.TemplateDonation"));
                using (StreamWriter sw = new StreamWriter(sd.FileName))
                {
                    sw.Write(content);
                    sw.Close();

                    MessageBox.Show(String.Format(Catalog.GetString("The file has been written to {0}."), sd.FileName), Catalog.GetString(
                            "Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}