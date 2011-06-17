//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TFrmMaintainLanguageCulture
    {
        private void InitializeManualCode()
        {
            DataTable CultureTable = new DataTable();

            CultureTable.Columns.Add("Value", typeof(string));
            CultureTable.Columns.Add("Display", typeof(string));

            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (!ci.IsNeutralCulture)
                {
                    DataRow row = CultureTable.NewRow();
                    row[0] = ci.Name;
                    row[1] = ci.EnglishName;
                    CultureTable.Rows.Add(row);
                }
            }

            CultureTable.DefaultView.Sort = "Display";

            cmbCulture.DataSource = CultureTable.DefaultView;
            cmbCulture.DisplayMember = "Display";
            cmbCulture.ValueMember = "Value";

            // TODO: load languages from names of language sub directories
            cmbLanguage.SetDataSourceStringList("en-GB,es-ES,da-DK,de-DE");

            string LanguageCode;
            string CultureCode;
            TRemote.MSysMan.Maintenance.WebConnectors.GetLanguageAndCulture(out LanguageCode, out CultureCode);

            cmbCulture.SetSelectedString(CultureCode);
            cmbLanguage.SetSelectedString(LanguageCode);
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            string LanguageCode = cmbLanguage.GetSelectedString();
            string CultureCode = cmbCulture.GetSelectedString();

            // send to server
            TRemote.MSysMan.Maintenance.WebConnectors.SetLanguageAndCulture(LanguageCode, CultureCode);

            // set local settings for client
            Catalog.Init(LanguageCode, CultureCode);

            // TODO: can we reload the main window with the new language?
            MessageBox.Show(Catalog.GetString("Please restart the OpenPetra client to see the new language"),
                Catalog.GetString("Restart the client"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Close();
        }

        /// <summary>
        /// to be called when the client has logged in
        /// </summary>
        public static void InitLanguageAndCulture()
        {
            TRemote.MSysMan.Maintenance.WebConnectors.LoadLanguageAndCultureFromUserDefaults();

            string LanguageCode;
            string CultureCode;
            TRemote.MSysMan.Maintenance.WebConnectors.GetLanguageAndCulture(out LanguageCode, out CultureCode);

            // set local settings for client
            Catalog.Init(LanguageCode, CultureCode);
        }
    }
}