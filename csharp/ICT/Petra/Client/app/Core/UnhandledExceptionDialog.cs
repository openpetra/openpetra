/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Resources;
using System.Net.Sockets;
using System.Runtime.Remoting;
using Ict.Common;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Dialog for displaying information to the user that a severe error occured in
    /// the PetraClient application.
    ///
    /// Mimics Windows' own 'application crash window', but allows
    ///   (1) different texts to be displayed (eg. depending on the situation, the
    ///       error, etc.)
    ///   (2) different 'OK' button text (if it is a fatal error, the Text is changed
    ///       to 'Close Petra')
    ///   (3) different Icons to be displayed (eg. depending on the situation, the
    ///       error, etc.)
    ///   (4) opening a Error Detail screen that shows the Exception text and allows
    ///       copying of that to the Clipboard
    ///   (5) e-mailing the Exception text to the Petra Team using the 'Bugreport'
    ///       feature (only enabled if Progress 4GL didn't crash)
    ///
    /// @Comment This Form is intended to be called from Unit
    ///   Ict.Petra.Client.App.Core.ExceptionHandling.
    /// </summary>
    public class TUnhandledExceptionForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TUnhandledExceptionForm() : base()
        {
            // TODO TUnhandledExceptionForm
        }
    }
}