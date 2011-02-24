// auto generated with nant generateWinforms from UnhandledExceptionDetailsDialog.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.App.Core
{

  /// auto generated: Error Details - OpenPetra
  public partial class TFrmUnhandledExceptionDetailsDialog: System.Windows.Forms.Form
  {
    /// constructor
    public TFrmUnhandledExceptionDetailsDialog(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.btnCopyToClipboard.Text = Catalog.GetString("Copy To Clipboard");
      this.btnOK.Text = Catalog.GetString("OK");
      this.Text = Catalog.GetString("Error Details - OpenPetra");
      #endregion

      this.txtErrorDetails.Font = TAppSettingsManager.GetDefaultBoldFont();

    }

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position
    }

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actCopyToClipboard")
        {
            btnCopyToClipboard.Enabled = e.Enabled;
        }
        if (e.ActionName == "actOK")
        {
            btnOK.Enabled = e.Enabled;
        }
    }

#endregion
  }
}
