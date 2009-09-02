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
using System;
using System.Xml;
using System.Collections.Specialized;
using System.Windows.Forms;
using Ict.Tools.CodeGeneration;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Common;

//using Ict.Petra.Client.CommonControls;

namespace Ict.Tools.CodeGeneration.Winforms
{
    public class ReportControls
    {
        public static string GetParameterName(XmlNode curNode)
        {
            if (TYml2Xml.HasAttribute(curNode, "NoParameter") && (TYml2Xml.GetAttribute(curNode, "NoParameter").ToLower() == "true"))
            {
                return null;
            }

            string result = "param_" + curNode.Name;

            if (TYml2Xml.HasAttribute(curNode, "ParameterName"))
            {
                result = TYml2Xml.GetAttribute(curNode, "ParameterName");
            }

            return result;
        }
    }
    public class TcmbAutoPopulatedReportGenerator : TcmbAutoPopulatedGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            string controlName = curNode.Name;

            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            writer.Template.AddToCodelet("READCONTROLS",
                "ACalc.AddParameter(\"" + paramName + "\", this." + controlName + ".GetSelectedString());" +
                Environment.NewLine);

            if (TXMLParser.HasAttribute(curNode, "OnChangeDataType"))
            {
                string datatype = TXMLParser.GetAttribute(curNode, "OnChangeDataType");
                writer.Template.AddToCodelet("SETCONTROLS",
                    StringHelper.UpperCamelCase(controlName, ",", false, false) + "_Initialise(" +
                    "AParameters.Get(\"" + paramName + "\").To" +
                    datatype + "());" +
                    Environment.NewLine);
            }
        }
    }
    public class ComboBoxReportGenerator : ComboBoxGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            string controlName = curNode.Name;

            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            writer.Template.AddToCodelet("READCONTROLS",
                "if (this." + controlName + ".SelectedItem != null) " + Environment.NewLine +
                "{" + Environment.NewLine +
                "  ACalc.AddParameter(\"" + paramName + "\", this." + controlName + ".SelectedItem.ToString()); " + Environment.NewLine +
                "}" + Environment.NewLine +
                "else" + Environment.NewLine +
                "{" + Environment.NewLine +
                "  ACalc.AddParameter(\"" + paramName + "\", \"\");" +
                "}" + Environment.NewLine);
            writer.Template.AddToCodelet("SETCONTROLS",
                controlName + ".SelectedValue = " +
                "AParameters.Get(\"" + paramName + "\").ToString();" +
                Environment.NewLine);
        }
    }

    public class CheckBoxReportGenerator : CheckBoxGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            string controlName = curNode.Name;

            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            writer.Template.AddToCodelet("READCONTROLS",
                "ACalc.AddParameter(\"" + paramName + "\", this." + controlName + ".Checked);" +
                Environment.NewLine);
            writer.Template.AddToCodelet("SETCONTROLS",
                controlName + ".Checked = " +
                "AParameters.Get(\"" + paramName + "\").ToBool();" +
                Environment.NewLine);
        }
    }
#if TODO
    public class TtxtAutoPopulatedButtonLabelReportGenerator : TtxtAutoPopulatedButtonLabelGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            string controlName = curNode.Name;

            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            writer.Template.AddToCodelet("READCONTROLS",
                "ACalc.AddParameter(\"" + paramName + "\", this." + controlName + ".Text);" +
                Environment.NewLine);
            writer.Template.AddToCodelet("SETCONTROLS",
                controlName + ".Text = " +
                "AParameters.Get(\"" + paramName + "\").ToString();" +
                Environment.NewLine);
        }
    }
#endif
    public class TextBoxReportGenerator : TextBoxGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            string controlName = curNode.Name;

            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            writer.Template.AddToCodelet("READCONTROLS",
                "ACalc.AddParameter(\"" + paramName + "\", this." + controlName + ".Text);" +
                Environment.NewLine);
            writer.Template.AddToCodelet("SETCONTROLS",
                controlName + ".Text = " +
                "AParameters.Get(\"" + paramName + "\").ToString();" +
                Environment.NewLine);
        }
    }

    public class TClbVersatileReportGenerator : TClbVersatileGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            string controlName = curNode.Name;

            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            writer.Template.AddToCodelet("READCONTROLS",
                "ACalc.AddParameter(\"" + paramName + "\", this." + controlName + ".GetCheckedStringList());" +
                Environment.NewLine);
            writer.Template.AddToCodelet("SETCONTROLS",
                controlName + ".SetCheckedStringList(" +
                "AParameters.Get(\"" + paramName + "\").ToString());" +
                Environment.NewLine);
        }
    }

    public class DateTimePickerReportGenerator : DateTimePickerGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            string controlName = curNode.Name;

            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            writer.Template.AddToCodelet("READCONTROLS",
                "ACalc.AddParameter(\"" + paramName + "\", this." + controlName + ".Value);" +
                Environment.NewLine);

            string NewCode = "DateTime " + controlName + "Date = AParameters.Get(\"" + paramName + "\").ToDate();" + Environment.NewLine +
                             "if ((" + controlName + "Date < " + controlName + ".MinDate)" + Environment.NewLine +
                             "    || (" + controlName + "Date > " + controlName + ".MaxDate))" + Environment.NewLine +
                             "{" + Environment.NewLine +
                             "    " + controlName + "Date = DateTime.Now;" + Environment.NewLine +
                             "}" + Environment.NewLine +
                             controlName + ".Value = " + controlName + "Date;" + Environment.NewLine;
            writer.Template.AddToCodelet("SETCONTROLS", NewCode);
        }
    }

    public class RadioGroupSimpleReportGenerator : RadioGroupSimpleGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            StringCollection optionalValues =
                TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "OptionalValues"));

            foreach (string rbtValueText in optionalValues)
            {
                string rbtValue = StringHelper.UpperCamelCase(rbtValueText.Replace("'", "").Replace(" ", "_"), false, false);
                string rbtName = "rbt" + rbtValue;
                writer.Template.AddToCodelet("READCONTROLS",
                    "if (" + rbtName + ".Checked) " + Environment.NewLine +
                    "{" + Environment.NewLine +
                    "  ACalc.AddParameter(\"" + paramName + "\", \"" + rbtValue + "\");" + Environment.NewLine +
                    "}" + Environment.NewLine);
                writer.Template.AddToCodelet("SETCONTROLS",
                    rbtName + ".Checked = " +
                    "AParameters.Get(\"" + paramName + "\").ToString() == \"" + rbtValue + "\";" +
                    Environment.NewLine);
            }
        }
    }

    public class RadioGroupComplexReportGenerator : RadioGroupComplexGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            StringCollection Controls =
                TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "Controls"));

            foreach (string controlName in Controls)
            {
                curNode = (XmlNode)writer.CodeStorage.FXmlNodes[controlName];
                TControlDef rbtCtrl = writer.CodeStorage.GetControl(controlName);
                string rbtValue = rbtCtrl.Label;
                rbtValue = StringHelper.UpperCamelCase(rbtValue.Replace("'", "").Replace(" ", "_"), false, false);
                string rbtName = "rbt" + curNode.Name.Substring(3);

                if (curNode.Name.StartsWith("tableLayoutPanel"))
                {
                    // the table layouts of sub controls for each radio button need to be skipped
                    continue;
                }

                writer.Template.AddToCodelet("READCONTROLS",
                    "if (" + rbtName + ".Checked) " + Environment.NewLine +
                    "{" + Environment.NewLine +
                    "  ACalc.AddParameter(\"" + paramName + "\", \"" + rbtValue + "\");" + Environment.NewLine +
                    "}" + Environment.NewLine);
                writer.Template.AddToCodelet("SETCONTROLS",
                    rbtName + ".Checked = " +
                    "AParameters.Get(\"" + paramName + "\").ToString() == \"" + rbtValue + "\";" +
                    Environment.NewLine);
            }
        }
    }

    public class RadioButtonReportGenerator : RadioButtonGenerator
    {
        public override void ApplyDerivedFunctionality(IFormWriter writer, XmlNode curNode)
        {
            // no writing or reading of parameters, should be done in RadioGroup
        }
    }
}