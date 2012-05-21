//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Xml;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.Winforms
{
    /// <summary>
    /// Providers are not added to any control; they don't have a name, size of position
    /// </summary>
    public class ProviderGenerator : TControlGenerator
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="APrefix"></param>
        /// <param name="AType"></param>
        public ProviderGenerator(string APrefix, System.Type AType)
            : base(APrefix, AType)
        {
            FLocation = false;
            FGenerateLabel = false;
            FAddControlToContainer = false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            // don't call base, because it should not have size, location, or name
            writer.Template.AddToCodelet("CONTROLINITIALISATION",
                "//" + Environment.NewLine + "// " + ctrl.controlName + Environment.NewLine + "//" + Environment.NewLine);

            return writer.FTemplate;
        }
    }

#if TODO
    /// <summary>
    /// generator for a text box in the statusbar
    /// </summary>
    public class StatusBarTextGenerator : ProviderGenerator
    {
        /// <summary>constructor</summary>
        public StatusBarTextGenerator()
            : base("sbt", typeof(EWSoftware.StatusBarText.StatusBarTextProvider))
        {
        }
    }
#endif
}