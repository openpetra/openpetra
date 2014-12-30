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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MCommon.Gui.Setup
{
    public partial class TFrmLanguageCodeSetup
    {
        private void InitializeManualCode()
        {
            FPetraUtilsObject.DataSaved += HandleDataSaved;
        }

        private void RunOnceOnActivationManual()
        {
            chkDetailCongressLanguage.Enabled = false;
        }

        private void NewRowManual(ref PLanguageRow ARow)
        {
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PLanguage.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PLanguage.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.LanguageCode = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPLanguage();
        }

        void HandleDataSaved(object Sender, TDataSavedEventArgs e)
        {
            if (e.Success)
            {
                TDataCache.TMCommon.RefreshCacheableCommonTable(TCacheableCommonTablesEnum.LanguageCodeList);
            }
        }
    }
}