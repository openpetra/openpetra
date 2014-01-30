//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Windows.Forms;
using System.Reflection;

using Ict.Common.Exceptions;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Contains routines that help with Tab Index setting in WinForms.
    ///
    /// </summary>
    public class TTabIndex : System.Object
    {
        #region TTabIndex

        /// <summary>
        /// Exchanges the Tab Index of two Controls. Optionally the Tab Index of
        /// Labels that are associated with the specified Controls can be changed as
        /// well (useful for maintaining keyboard shortcuts).
        ///
        /// @comment Both controls need to reside in the same Container Control.
        ///
        /// </summary>
        /// <param name="AControl1">The first Control that should change Tab Index with the
        /// second Control.</param>
        /// <param name="AControl2">The second Control that should change Tab Index with the
        /// first Control.</param>
        /// <param name="AExchangeAssociatedLabels">Set to true to change the Tab Index of
        /// Labels that are associated with the specified Controls as well
        /// </param>
        /// <returns>void</returns>
        public static void ExchangeTabIndex(Control AControl1, Control AControl2, Boolean AExchangeAssociatedLabels)
        {
            Control Container;
            Int32 Control1TabIndex;
            Int32 Label1TabIndex;
            int Counter;
            String Label1Name;
            String Label2Name;
            Control Label1;
            Control Label2;

            #region Check Arguments

            if ((AControl1.Parent is System.Windows.Forms.Form) || (AControl1.Parent is System.Windows.Forms.Panel)
                || (AControl1.Parent is System.Windows.Forms.GroupBox) || (AControl1.Parent is System.Windows.Forms.UserControl)
                || (AControl1.Parent is System.Windows.Forms.TabControl))
            {
                Container = (Control)AControl1.Parent;
            }
            else
            {
                throw new ArgumentException("AControl1 must be in the ControlCollection of a Form or a Control");
            }

            if ((AControl2.Parent is System.Windows.Forms.Form) || (AControl2.Parent is System.Windows.Forms.Panel)
                || (AControl2.Parent is System.Windows.Forms.GroupBox) || (AControl2.Parent is System.Windows.Forms.UserControl)
                || (AControl2.Parent is System.Windows.Forms.TabControl))
            {
                if (AControl2.Parent != Container)
                {
                    throw new ArgumentException("AControl2 must be in the same ControlCollection than AControl1");
                }
            }
            else
            {
                throw new ArgumentException("AControl2 must be in the ControlCollection of a Form or a Control");
            }

            #endregion

            // Exchange TabIndex of AControl1 and AControl2
            Control1TabIndex = AControl1.TabIndex;
            AControl1.TabIndex = AControl2.TabIndex;
            AControl2.TabIndex = Control1TabIndex;

            // Exchange TabIndexes of associated labels as well  if requested
            if (AExchangeAssociatedLabels)
            {
                Label1Name = "lbl" + AControl1.Name.Substring(3);
                Label1 = null;

                for (Counter = 0; Counter <= Container.Controls.Count - 1; Counter += 1)
                {
                    if (Container.Controls[Counter].Name == Label1Name)
                    {
                        Label1 = Container.Controls[Counter];
                        continue;
                    }
                }

                if (Label1 == null)
                {
                    throw new EOPAppException(
                        "TTabIndex.ExchangeTabIndex: Associated label '" + Label1Name + "' couldn't be found for Control1 ('" + AControl1.Name + "')");
                }

                Label2Name = "lbl" + AControl2.Name.Substring(3);
                Label2 = null;

                for (Counter = 0; Counter <= Container.Controls.Count - 1; Counter += 1)
                {
                    if (Container.Controls[Counter].Name == Label2Name)
                    {
                        Label2 = Container.Controls[Counter];
                        continue;
                    }
                }

                if (Label2 == null)
                {
                    throw new EOPAppException(
                        "TTabIndex.ExchangeTabIndex: Associated label  '" + Label2Name + "' couldn't be found for Control2 ('" + AControl2.Name +
                        "')");
                }

                // Exchange TabIndex of Label1 and Label2
                Label1TabIndex = Label1.TabIndex;
                Label1.TabIndex = Label2.TabIndex;
                Label2.TabIndex = Label1TabIndex;
            }
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AControl1"></param>
        /// <param name="AControl2"></param>
        public static void ExchangeTabIndex(Control AControl1, Control AControl2)
        {
            ExchangeTabIndex(AControl1, AControl2, false);
        }

        #endregion
    }
}