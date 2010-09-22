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
using System.IO;
using System.Threading;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Printing;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.MPartner.Logic;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TFrmMainForm
    {
        private void InitializeManualCode()
        {
            try
            {
                TGetData.InitDBConnection();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, Catalog.GetString("Error connecting to Petra 2.x"));
            }
        }

        Thread FRunCheckThread;
        bool FCancel = false;

        private delegate void ThreadFinishedDelegate();
        private void ThreadFinished()
        {
            FPetraUtilsObject.EnableAction("actCancel", false);
            FPetraUtilsObject.EnableAction("actRunCheck", true);
        }

        private void ThreadStarted()
        {
            FPetraUtilsObject.EnableAction("actRunCheck", false);
        }

        private void DoCheck()
        {
            Invoke(new ThreadFinishedDelegate(@ThreadStarted));

            PPartnerTable Partners = TGetData.GetPartnersToCheck();

            foreach (PPartnerRow Partner in Partners.Rows)
            {
                if (FCancel)
                {
                    Invoke(new ThreadFinishedDelegate(@ThreadFinished));
                    return;
                }

                stbMain.ShowMessage(Partner.PartnerShortName);

                PPartnerTable DuplicatePartners = TGetData.CheckDuplicates(Partner);

                if (DuplicatePartners.Rows.Count > 0)
                {
                    PLocationTable Location;
                    TGetData.GetBestAddress(Partner.PartnerKey, out Location);
                    TLogging.Log(Partner.PartnerKey.ToString() + ";" + Partner.PartnerShortName + ";" +
                        Partner.PartnerClass + ";" +
                        Partner.StatusCode + ";" +
                        Location[0].StreetName + ";" + Location[0].PostalCode + ";" + Location[0].City);

                    foreach (PPartnerRow DuplicatePartner in DuplicatePartners.Rows)
                    {
                        TGetData.GetBestAddress(DuplicatePartner.PartnerKey, out Location);

                        TLogging.Log("    Duplicate: " + DuplicatePartner.PartnerKey.ToString() + ";" + DuplicatePartner.PartnerShortName + ";" +
                            DuplicatePartner.PartnerClass + ";" +
                            DuplicatePartner.StatusCode + ";" +
                            Location[0].StreetName + ";" + Location[0].PostalCode + ";" + Location[0].City);
                    }
                }
            }

            Invoke(new ThreadFinishedDelegate(@ThreadFinished));

            MessageBox.Show("finished");
        }

        private void RunCheck(object sender, EventArgs e)
        {
            FCancel = false;
            FPetraUtilsObject.EnableAction("actCancel", true);
            FRunCheckThread = new Thread(new ThreadStart(DoCheck));
            FRunCheckThread.Start();
        }

        private void Cancel(object sender, EventArgs e)
        {
            FCancel = true;
        }
    }
}