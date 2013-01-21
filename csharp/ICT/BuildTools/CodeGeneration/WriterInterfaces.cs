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
using System.Xml;
using System.Collections.Specialized;
using Ict.Common.IO;

namespace Ict.Tools.CodeGeneration
{
    /// <summary>
    /// define the methods and properties of a control generator
    /// </summary>
    public interface IControlGenerator
    {
        /// <summary>for implementing some functions specific to the control</summary>
        void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control);
        /// <summary>generate all code for the control</summary>
        void GenerateControl(TFormWriter writer, TControlDef ctrl);
        /// <summary>generate the code of the contained controls</summary>
        void ProcessChildren(TFormWriter writer, TControlDef ctrl);
        /// <summary>add children to the control</summary>
        void AddChildren(TFormWriter writer, TControlDef ctrl);
        /// <summary>write the code for the designer file where the control is declared</summary>
        void GenerateDeclaration(TFormWriter writer, TControlDef ctrl);
        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef container);
        /// <summary>write code for on change event</summary>
        void OnChangeDataType(TFormWriter writer, XmlNode curNode);
        /// <summary>write code for on change event</summary>
        void OnChangeDataType(TFormWriter writer, XmlNode curNode, string controlName);
        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        bool ControlFitsNode(XmlNode curNode);

        /// <summary>
        /// it will not be created if there are no children; eg. toolbar
        /// </summary>
        bool RequiresChildren
        {
            get;
        }

        /// <summary>
        /// the label is assembled
        /// </summary>
        bool GenerateLabel(TControlDef ctrl);

        /// <summary>
        /// type of the control
        /// </summary>
        string ControlType
        {
            set;
            get;
        }

        /// <summary>
        /// the name of the snippet in the template for Readcontrols and setcontrols, in captial letters
        /// </summary>
        string TemplateSnippetName
        {
            set;
            get;
        }

        /// <summary>
        /// should the control be added to the parent container
        /// </summary>
        bool AddControlToContainer
        {
            set;
            get;
        }
    }
}