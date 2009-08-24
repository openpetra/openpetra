/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using System.Xml;

namespace Ict.Tools.CodeGeneration
{
    public interface IControlGenerator
    {
        void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode);
        void GenerateDeclaration(IFormWriter writer, TControlDef ctrl);
        void SetControlProperties(IFormWriter writer, TControlDef container);
        void OnChangeDataType(IFormWriter writer, XmlNode curNode);
        void OnChangeDataType(IFormWriter writer, XmlNode curNode, string controlName);
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

        bool AddControlToContainer
        {
            set;
            get;
        }
    }

    public interface IFormWriter
    {
        void CreateCode(TCodeStorage AStorage, string AXamlFilename, string ATemplate);
        void CreateResourceFile(string AResourceFile, string AResourceTemplate);
        bool WriteFile(string ADestinationFile, string ATemplate);
        bool WriteFile(string ADestinationFile);
        void SetControlProperty(string AControlName, string APropertyName, string APropertyValue);
        void SetControlProperty(TControlDef ACtrl, string APropertyName);
        void ApplyDerivedFunctionality(IControlGenerator generator, XmlNode curNode);
        IControlGenerator FindControlGenerator(XmlNode curNode);
        void CallControlFunction(string AControlName, string AFunctionCall);
        void SetEventHandlerToControl(string AControlName, string AEvent, string AEventHandlerType, string AEventHandlingMethod);
        void SetEventHandlerFunction(string AControlName, string AEvent, string AEventImplementation);
        void AddContainer(string AControlName);
        void AddImageToResource(string AControlName, string AImageName, string AImageOrIcon);
        void InitialiseDataSource(XmlNode curNode, string AControlName);


        TCodeStorage CodeStorage
        {
            get;
        }

        ProcessTemplate Template
        {
            get;
        }
    }
}