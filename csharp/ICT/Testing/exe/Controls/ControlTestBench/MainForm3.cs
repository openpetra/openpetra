//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 Taylor Students
//       christiank
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
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;

namespace ControlTestBench
{
/// <summary>
/// Description of MainForm3.
/// </summary>
public partial class MainForm3 : Form
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public MainForm3()
    {
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent();

        //
        // TODO: Add constructor code after the InitializeComponent() call.
        //
    }

    void TestShepherd(object sender, EventArgs e)
    {
        XmlDocument UINavigation = LoadYAMLTestFile();

        new ControlTestBench.ShepherdTest(UINavigation.FirstChild.NextSibling.FirstChild).Show();
    }

    XmlDocument LoadYAMLTestFile()
    {
        String yamlFile = txtYaml.Text.ToString();

        new TAppSettingsManager("../../csharp/ICT/Testing/exe/Controls/ControlTestBench/ControlTestBench.exe.config");
        TYml2Xml parser = new TYml2Xml(TAppSettingsManager.GetValue("YAMLDemodataPath") + '/' + yamlFile);

        XmlDocument UINavigation = parser.ParseYML2XML();

        return UINavigation;
    }

    void BtnCollapsibleTestClick(object sender, EventArgs e)
    {
        XmlDocument UINavigation = LoadYAMLTestFile();

        TVisualStylesEnum EnumStyle = Helper.GetVisualStylesEnumFromString(cmbVisualStyle.Text.ToString());

        new CollapsibleTest(UINavigation.FirstChild.NextSibling.FirstChild, EnumStyle).Show();
    }

    void HandlerTaskListTest(object sender, EventArgs e)
    {
        XmlDocument UINavigation = LoadYAMLTestFile();

        TVisualStylesEnum EnumStyle = Helper.GetVisualStylesEnumFromString(cmbVisualStyle.Text.ToString());

        new TaskListTest(UINavigation.FirstChild.NextSibling.FirstChild, EnumStyle).Show();
    }

    void BtnTestAllClick(object sender, EventArgs e)
    {
        XmlDocument UINavigation = LoadYAMLTestFile();

        new TestAll(UINavigation.FirstChild.NextSibling.FirstChild).Show();
    }

    void BtnCollapsibleHosterTestClick(object sender, EventArgs e)
    {
        XmlDocument UINavigation = LoadYAMLTestFile();

        TVisualStylesEnum EnumStyle = Helper.GetVisualStylesEnumFromString(cmbVisualStyle.Text.ToString());

        new CollapsiblePanelHosterTest(UINavigation.FirstChild.NextSibling.FirstChild, EnumStyle).Show();
    }
}
}