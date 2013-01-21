//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Windows.Forms;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Logic for the Field Of Service Screen
    ///
    /// </summary>
    public class TFieldOfServiceLogic : System.Object
    {
        #region Resourcestrings

        private static readonly string StrPetraServerTooBusy = Catalog.GetString(
            "The OpenPetra Server is currently too busy to open the Partner Edit screen. Please wait a few seconds " +
            "and press 'Retry' then to retry, or 'Cancel' to abort.");
        private static readonly string StrPetraServerTooBusyTitle = Catalog.GetString("OpenPetra Server Too Busy");

        #endregion

        /// <summary>Main DataSet for the Screen</summary>
        private FieldOfServiceTDS FMainDS = new FieldOfServiceTDS();

        /// <summary>Reference to the screen's UIConnector (serverside Business Object)</summary>
        private IPartnerUIConnectorsFieldOfService FUIConnector;

        /// <summary>
        /// Instantiates the Screen's UIConnector and retrieves data.
        ///
        /// </summary>
        /// <param name="APartnerKey">Partner Key of the Partner</param>
        /// <param name="AMainDS">Typed DataSet containing the data from the DB</param>
        /// <returns>true if successful, otherwise false
        /// </returns>
        public Boolean GetFieldOfServiceUIConnector(Int64 APartnerKey, out FieldOfServiceTDS AMainDS)
        {
            System.Windows.Forms.DialogResult ServerBusyDialogResult;
            Boolean ServerCallSuccessful = false;

            do
            {
                try
                {
                    FUIConnector = TRemote.MCommon.UIConnectors.FieldOfService(APartnerKey);

                    ServerCallSuccessful = true;
                }
                catch (EDBTransactionBusyException)
                {
                    ServerBusyDialogResult = MessageBox.Show(StrPetraServerTooBusy,
                        StrPetraServerTooBusyTitle,
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);

                    if (ServerBusyDialogResult == System.Windows.Forms.DialogResult.Retry)
                    {
                        // retry will happen because of the repeat block
                    }
                    else
                    {
                        // break out of repeat block; this function will return false because of that.
                        break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            } while (!(ServerCallSuccessful));

            if (ServerCallSuccessful)
            {
                // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
                TEnsureKeepAlive.Register(FUIConnector);
            }

            AMainDS = FMainDS;
            return ServerCallSuccessful;
        }

        /// <summary>
        /// Frees the UIConnector so it can be GC'ed on the server side.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void UnRegisterUIConnector()
        {
            if (FUIConnector != null)
            {
                // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                TEnsureKeepAlive.UnRegister(FUIConnector);
            }
        }
    }
}