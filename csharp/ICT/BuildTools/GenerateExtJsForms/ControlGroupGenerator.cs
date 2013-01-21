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
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.ExtJs
{
    /// <summary>
    /// base class for generators that contain controls
    /// </summary>
    public class GroupBoxBaseGenerator : TControlGenerator
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="prefix"></param>
        public GroupBoxBaseGenerator(string prefix)
            : base(prefix, "none")
        {
            FGenerateLabel = false;

            if (base.FPrefix == "rng")
            {
                FGenerateLabel = true;
            }
        }

        /// <summary>
        /// this is currently only overloaded by RadioGroupGenerator
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="curNode"></param>
        /// <returns></returns>
        public virtual StringCollection FindContainedControls(TFormWriter writer, XmlNode curNode)
        {
            return new StringCollection();
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate snippetRowDefinition = writer.FTemplate.GetSnippet(FControlDefinitionSnippetName);

            ((TExtJsFormsWriter)writer).AddResourceString(snippetRowDefinition, "LABEL", ctrl, ctrl.Label);

            StringCollection Controls = FindContainedControls(writer, ctrl.xmlNode);

            snippetRowDefinition.AddToCodelet("ITEMS", "");

            if (Controls.Count > 0)
            {
                // used for radiogroupbox
                foreach (string ChildControlName in Controls)
                {
                    TControlDef childCtrl = FCodeStorage.FindOrCreateControl(ChildControlName, ctrl.controlName);
                    IControlGenerator ctrlGen = writer.FindControlGenerator(childCtrl);
                    ProcessTemplate ctrlSnippet = ctrlGen.SetControlProperties(writer, childCtrl);

                    ctrlSnippet.SetCodelet("COLUMNWIDTH", "");

                    ctrlSnippet.SetCodelet("ITEMNAME", ctrl.controlName);
                    ctrlSnippet.SetCodelet("ITEMID", childCtrl.controlName);

                    if (ctrl.GetAttribute("hideLabel") == "true")
                    {
                        ctrlSnippet.SetCodelet("HIDELABEL", "true");
                    }
                    else if (ChildControlName == Controls[0])
                    {
                        ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "LABEL", ctrl, ctrl.Label);
                    }

                    snippetRowDefinition.InsertSnippet("ITEMS", ctrlSnippet, ",");
                }
            }
            else
            {
                // used for GroupBox, and Composite
                TExtJsFormsWriter.InsertControl(ctrl, snippetRowDefinition, "ITEMS", "HiddenValues", writer);
                TExtJsFormsWriter.InsertControl(ctrl, snippetRowDefinition, "ITEMS", "Controls", writer);
            }

            return snippetRowDefinition;
        }
    }
}