//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2014 by OM International
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
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// This class is required because the standard Windows Forms control captures the ESCAPE key before we get a chance to make use of it on the form.
    /// So we override ProcessCmdKey and that is the only difference
    /// </summary>
    public class TUC_PrintPreviewControl : PrintPreviewControl
    {
        private const Int32 WM_KEYDOWN = 0x100;

        /// <summary>
        /// Handle the ProcessCmdKey method.  All we are looking for is the ESCAPE key.  If we find it, we may close the form.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns>True if we close the form, otherwise we simply return the base class return value</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((msg.Msg == WM_KEYDOWN) && (keyData == Keys.Escape))
            {
                if (TUserDefaults.GetBooleanDefault(TUserDefaults.NamedDefaults.USERDEFAULT_ESC_CLOSES_SCREEN, true) == true)
                {
                    // The user wants us to close the form if the ESCAPE key is pressed.

                    // Here we should check to see if the PPV control actually needs the escape key
                    // At present I do not know how to determine this because I have never seen the control in action ...

                    // If the control does not want the escape key we can find the top level form and close it.
                    Control control = this;
                    while (control.Parent != null)
                    {
                        control = control.Parent;
                    }

                    // The control with no parent should be the form
                    if (control is Form)
                    {
                        ((Form)control).Close();
                        return true;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
