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
using System.Xml;

namespace Ict.Tools.CodeGeneration
{
    public interface IControlGenerator
    {
        void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode);
        void GenerateDeclaration(TFormWriter writer, TControlDef ctrl);
        ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef container);
        void OnChangeDataType(TFormWriter writer, XmlNode curNode);
        void OnChangeDataType(TFormWriter writer, XmlNode curNode, string controlName);
        bool ControlFitsNode(XmlNode curNode);

        /// <summary>
        /// it will not be created if there are no children; eg. toolbar
        /// </summary>
        bool RequiresChildren
        {
            get;
        }

        bool GenerateLabel(TControlDef ctrl);

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

        bool AddControlToContainer
        {
            set;
            get;
        }
    }
}