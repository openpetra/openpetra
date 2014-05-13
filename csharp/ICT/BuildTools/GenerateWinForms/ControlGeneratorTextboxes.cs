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
    /// generator for a date picker
    /// </summary>
    public class DateTimePickerGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public DateTimePickerGenerator()
            : base("dtp", "Ict.Petra.Client.CommonControls.TtxtPetraDate")
        {
            this.FChangeEventName = "DateChanged";
            this.FChangeEventHandlerType = "TPetraDateChangedEventHandler";
            FDefaultWidth = 94;
            FDefaultHeight = 22;
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Date == null";
            }

            if (AFieldTypeDotNet.Contains("Date?"))
            {
                return ctrl.controlName + ".Date";
            }

            return "(" + ctrl.controlName + ".Date.HasValue?" + ctrl.controlName + ".Date.Value:DateTime.MinValue)";
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Date = null;";
            }

            return ctrl.controlName + ".Date = " + AFieldOrNull + ";";
        }

        /// <summary>
        /// how to undo the change of a value of a control
        /// </summary>
        protected override string UndoValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            return ctrl.controlName + ".Date = (DateTime)" + AFieldOrNull + ";";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl, "Description", "\"" + ctrl.Label + "\"");

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a text box
    /// </summary>
    public class TextBoxGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public TextBoxGenerator()
            : base("txt", typeof(TextBox))
        {
            FChangeEventName = "TextChanged";
            FHasReadOnlyProperty = true;
            FDefaultHeight = 22;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if ((TYml2Xml.GetAttribute(curNode, "Format") != String.Empty))
                {
                    return false;
                }

                if (TYml2Xml.GetAttribute(curNode, "ReadOnly").ToLower() == "true")
                {
                    if ((TYml2Xml.GetAttribute(curNode, "Type") != "PartnerKey"))
                    {
                        return true;
                    }
                }

                if ((TYml2Xml.GetAttribute(curNode, "Type") == "PartnerKey")
                    || (TYml2Xml.GetAttribute(curNode, "Type") == "Extract")
                    || (TYml2Xml.GetAttribute(curNode, "Type") == "Occupation")
                    || (TYml2Xml.GetAttribute(curNode, "Type") == "Conference")
                    || (TYml2Xml.GetAttribute(curNode, "Type") == "Event")
                    || (TYml2Xml.GetAttribute(curNode, "Type") == "Hyperlink"))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Text = String.Empty;";
            }

            if (!AFieldTypeDotNet.ToLower().Contains("string"))
            {
                if (ctrl.GetAttribute("Type") == "PartnerKey")
                {
                    // for readonly text box
                    return ctrl.controlName + ".Text = String.Format(\"{0:0000000000}\", " + AFieldOrNull + ");";
                }
                else if (ctrl.GetAttribute("Type") == "ShortTime")
                {
                    // for seconds to short time
                    return ctrl.controlName + ".Text = new Ict.Common.TypeConverter.TShortTimeConverter().ConvertTo(" + AFieldOrNull +
                           ", typeof(string)).ToString();";
                }
                else if (ctrl.GetAttribute("Type") == "LongTime")
                {
                    // for seconds to long time
                    return ctrl.controlName + ".Text = new Ict.Common.TypeConverter.TLongTimeConverter().ConvertTo(" + AFieldOrNull +
                           ", typeof(string)).ToString();";
                }

                return ctrl.controlName + ".Text = " + AFieldOrNull + ".ToString();";
            }

            return ctrl.controlName + ".Text = " + AFieldOrNull + ";";
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Text.Length == 0";
            }

            if (ctrl.GetAttribute("Type") == "ShortTime")
            {
                return "(int)new Ict.Common.TypeConverter.TShortTimeConverter().ConvertTo(" + ctrl.controlName + ".Text, typeof(int))";
            }
            else if (ctrl.GetAttribute("Type") == "LongTime")
            {
                return "(int)new Ict.Common.TypeConverter.TLongTimeConverter().ConvertTo(" + ctrl.controlName + ".Text, typeof(int))";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("int64"))
            {
                return "Convert.ToInt64(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("int"))
            {
                return "Convert.ToInt32(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return "Convert.ToDecimal(" + ctrl.controlName + ".Text)";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "DefaultValue"))
            {
                writer.SetControlProperty(ctrl,
                    "Text",
                    "\"" + TYml2Xml.GetAttribute(ctrl.xmlNode, "DefaultValue") + "\"");
            }

            if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "Multiline")) && (TYml2Xml.GetAttribute(ctrl.xmlNode, "Multiline") == "true"))
            {
                writer.SetControlProperty(ctrl, "Multiline", "true");

                if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "WordWrap")) && (TYml2Xml.GetAttribute(ctrl.xmlNode, "WordWrap") == "false"))
                {
                    writer.SetControlProperty(ctrl, "WordWrap", "false");
                }

                if (TYml2Xml.HasAttribute(ctrl.xmlNode, "ScrollBars"))
                {
                    writer.SetControlProperty(ctrl, "ScrollBars", "ScrollBars." + TYml2Xml.GetAttribute(ctrl.xmlNode, "ScrollBars"));
                }
            }

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "TextAlign"))
            {
                writer.SetControlProperty(ctrl, "TextAlign", "HorizontalAlignment." + TYml2Xml.GetAttribute(ctrl.xmlNode, "TextAlign"));
            }

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "CharacterCasing"))
            {
                writer.SetControlProperty(ctrl, "CharacterCasing", "CharacterCasing." +
                    TYml2Xml.GetAttribute(ctrl.xmlNode, "CharacterCasing"));
            }

            if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "PasswordEntry")) && (TYml2Xml.GetAttribute(ctrl.xmlNode, "PasswordEntry") == "true"))
            {
                writer.SetControlProperty(ctrl, "UseSystemPasswordChar", "true");
            }

            writer.Template.AddToCodelet("ASSIGNFONTATTRIBUTES",
                "this." + ctrl.controlName + ".Font = TAppSettingsManager.GetDefaultBoldFont();" + Environment.NewLine);

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for the control that has an autopopulated text box with a button for a find dialog
    /// </summary>
    public class TTxtAutoPopulatedButtonLabelGenerator : TControlGenerator
    {
        String FButtonLabelType = "";

        /// <summary>constructor</summary>
        public TTxtAutoPopulatedButtonLabelGenerator()
            : base("txt", "Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel")
        {
            this.FChangeEventHandlerType = "TDelegatePartnerChanged";
            FDefaultHeight = 22;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if ((TYml2Xml.GetAttribute(curNode, "Format") != String.Empty))
                {
                    return false;
                }

                if (TYml2Xml.GetAttribute(curNode, "Type") == "PartnerKey")
                {
                    FButtonLabelType = "PartnerKey";

                    if (!(TYml2Xml.HasAttribute(curNode,
                              "ShowLabel") && (TYml2Xml.GetAttribute(curNode, "ShowLabel").ToLower() == "false")))
                    {
                        FDefaultWidth = 370;
                    }
                    else
                    {
                        FDefaultWidth = 80;
                    }

                    FHasReadOnlyProperty = true;

                    return true;
                }
                else if (TYml2Xml.GetAttribute(curNode, "Type") == "Extract")
                {
                    FButtonLabelType = "Extract";
                    return true;
                }
                else if (TYml2Xml.GetAttribute(curNode, "Type") == "Occupation")
                {
                    FButtonLabelType = "OccupationList";
                    return true;
                }
                else if (TYml2Xml.GetAttribute(curNode, "Type") == "Conference")
                {
                    FButtonLabelType = "Conference";

                    if (!(TYml2Xml.HasAttribute(curNode,
                              "ShowLabel") && (TYml2Xml.GetAttribute(curNode, "ShowLabel").ToLower() == "false")))
                    {
                        FDefaultWidth = 370;
                    }
                    else
                    {
                        FDefaultWidth = 80;
                    }

                    FHasReadOnlyProperty = true;

                    return true;
                }
                else if (TYml2Xml.GetAttribute(curNode, "Type") == "Event")
                {
                    FButtonLabelType = "Event";

                    if (!(TYml2Xml.HasAttribute(curNode,
                              "ShowLabel") && (TYml2Xml.GetAttribute(curNode, "ShowLabel").ToLower() == "false")))
                    {
                        FDefaultWidth = 370;
                    }
                    else
                    {
                        FDefaultWidth = 80;
                    }

                    FHasReadOnlyProperty = true;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Text = String.Empty;";
            }

            return ctrl.controlName + ".Text = String.Format(\"{0:0000000000}\", " + AFieldOrNull + ");";
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Text.Length == 0";
            }

            if (AFieldTypeDotNet.ToLower().Contains("int64"))
            {
                return "Convert.ToInt64(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("int"))
            {
                return "Convert.ToInt32(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return "Convert.ToDecimal(" + ctrl.controlName + ".Text)";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            Int32 buttonWidth = 40;
            Int32 textBoxWidth = 80;

            // seems to be hardcoded in csharp\ICT\Petra\Client\CommonControls\Gui\txtAutoPopulatedButtonLabel.Designer.cs
            Int32 controlWidth = 390;

            base.SetControlProperties(writer, ctrl);

            if ((ctrl.HasAttribute("ShowLabel") && (ctrl.GetAttribute("ShowLabel").ToLower() == "false")))
            {
                writer.SetControlProperty(ctrl, "ShowLabel", "false");
                controlWidth = 120;
            }

            // Note: the control defaults to 'ShowLabel' true, so this doesn't need to be set to 'true' in code.

            writer.SetControlProperty(ctrl, "ASpecialSetting", "true");
            writer.SetControlProperty(ctrl, "ButtonTextAlign", "System.Drawing.ContentAlignment.MiddleCenter");
            writer.SetControlProperty(ctrl, "ListTable", "TtxtAutoPopulatedButtonLabel.TListTableEnum." +
                FButtonLabelType);
            writer.SetControlProperty(ctrl, "PartnerClass", "\"" + ctrl.GetAttribute("PartnerClass") + "\"");
            writer.SetControlProperty(ctrl, "MaxLength", "32767");
            writer.SetControlProperty(ctrl, "Tag", "\"CustomDisableAlthoughInvisible\"");
            writer.SetControlProperty(ctrl, "TextBoxWidth", textBoxWidth.ToString());

            if (!(ctrl.HasAttribute("ReadOnly") && (ctrl.GetAttribute("ReadOnly").ToLower() == "true")))
            {
                writer.SetControlProperty(ctrl, "ButtonWidth", buttonWidth.ToString());
                writer.SetControlProperty(ctrl, "ReadOnly", "false");
                writer.SetControlProperty(ctrl,
                    "Font",
                    "new System.Drawing.Font(\"Verdana\", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0)");
                writer.SetControlProperty(ctrl, "ButtonText", "\"Find\"");
            }
            else
            {
                writer.SetControlProperty(ctrl, "ButtonWidth", "0");
                writer.SetControlProperty(ctrl, "BorderStyle", "System.Windows.Forms.BorderStyle.None");
                writer.SetControlProperty(ctrl, "Padding", "new System.Windows.Forms.Padding(0, 4, 0, 0)");
            }

            if (!ctrl.HasAttribute("Width"))
            {
                ctrl.SetAttribute("Width", controlWidth.ToString());
            }

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "DefaultValue"))
            {
                writer.SetControlProperty(ctrl,
                    "Text",
                    "\"" + TYml2Xml.GetAttribute(ctrl.xmlNode, "DefaultValue") + "\"");
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a numeric text box control
    /// </summary>
    public class TTxtNumericTextBoxGenerator : TControlGenerator
    {
        string FControlMode;
        Int16 FDecimalPrecision = 2;
        bool FNullValueAllowed = true;

        /// <summary>constructor</summary>
        public TTxtNumericTextBoxGenerator()
            : base("txt", "Ict.Common.Controls.TTxtNumericTextBox")
        {
            FChangeEventName = "TextChanged";
            FHasReadOnlyProperty = true;
            FDefaultHeight = 22;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            bool ReturnValue = false;
            string NumberFormat;
            string PotentialDecimalPrecision;
            string PotentialNullValue;

//Console.WriteLine("TTxtNumericTextBoxGenerator ControlFitsNode");
            if (base.ControlFitsNode(curNode))
            {
                FDefaultWidth = 80;

                NumberFormat = TYml2Xml.GetAttribute(curNode, "Format");

//Console.WriteLine("TTxtNumericTextBoxGenerator Format: '" + NumberFormat + "'");
                if ((NumberFormat == "Integer")
                    || (NumberFormat == "PercentInteger"))
                {
                    FControlMode = "Integer";

                    ReturnValue = true;
                }

                if (NumberFormat == "LongInteger")
                {
                    FControlMode = "LongInteger";

                    ReturnValue = true;
                }

                if ((NumberFormat == "Decimal")
                    || (NumberFormat == "PercentDecimal")
                    || (NumberFormat.StartsWith("Decimal("))
                    || (NumberFormat.StartsWith("PercentDecimal(")))
                {
                    FControlMode = "Decimal";

                    ReturnValue = true;
                }

                if (ReturnValue)
                {
                    if ((NumberFormat.StartsWith("Decimal("))
                        || (NumberFormat.StartsWith("PercentDecimal(")))
                    {
                        PotentialDecimalPrecision = NumberFormat.Substring(NumberFormat.IndexOf('(') + 1,
                            NumberFormat.IndexOf(')') - NumberFormat.IndexOf('(') - 1);

//Console.WriteLine("TTxtNumericTextBoxGenerator: PotentialDecimalPrecision: " + PotentialDecimalPrecision);
                        if (PotentialDecimalPrecision != String.Empty)
                        {
                            try
                            {
                                FDecimalPrecision = Convert.ToInt16(PotentialDecimalPrecision);
                            }
                            catch (System.FormatException)
                            {
                                throw new ApplicationException(
                                    "TextBox with decimal formatting: The specifier for the decimal precision '" + PotentialDecimalPrecision +
                                    "' is not a number!");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }

                    if (TYml2Xml.HasAttribute(curNode, "NullValueAllowed"))
                    {
                        PotentialNullValue = TYml2Xml.GetAttribute(curNode, "NullValueAllowed");

                        if ((PotentialNullValue == "true")
                            || (PotentialNullValue == "false"))
                        {
                            FNullValueAllowed = Convert.ToBoolean(PotentialNullValue);
                        }
                        else
                        {
                            throw new ApplicationException(
                                "TextBox with number formatting: Value for 'NullValueAllowed' needs to be either 'true' or 'false', but is '" +
                                PotentialNullValue + "'.");
                        }
                    }
                }

                return ReturnValue;
            }

            return false;
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                if (FControlMode == "Decimal")
                {
                    return ctrl.controlName + ".NumberValueDecimal = null;";
                }
                else
                {
                    if (FControlMode == "Integer")
                    {
                        return ctrl.controlName + ".NumberValueInt = null;";
                    }
                    else
                    {
                        return ctrl.controlName + ".NumberValueLongInt = null;";
                    }
                }
            }
            else
            {
                if (AFieldTypeDotNet.ToLower() == "int32")
                {
                    if (AFieldOrNull == null)
                    {
                        return ctrl.controlName + ".NumberValueInt = null;";
                    }

                    return ctrl.controlName + ".NumberValueInt = " + AFieldOrNull + ";";
                }
                else if (AFieldTypeDotNet.ToLower() == "int64")
                {
                    if (AFieldOrNull == null)
                    {
                        return ctrl.controlName + ".NumberValueLongInt = null;";
                    }

                    return ctrl.controlName + ".NumberValueLongInt = " + AFieldOrNull + ";";
                }
                else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
                {
                    if (AFieldOrNull == null)
                    {
                        return ctrl.controlName + ".NumberValueDecimal = null;";
                    }

                    return ctrl.controlName + ".NumberValueDecimal = Convert.ToDecimal(" + AFieldOrNull + ");";
                }
                else
                {
                    TLogging.Log("Warning: unknown type " + AFieldTypeDotNet + " for Textbox.AssignValue for control " + ctrl.controlName);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// how to undo the change of a value of a control
        /// </summary>
        protected override string UndoValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet.ToLower() == "int32")
            {
                return ctrl.controlName + ".NumberValueInt = (Int32)" + AFieldOrNull + ";";
            }
            else if (AFieldTypeDotNet.ToLower() == "int64")
            {
                return ctrl.controlName + ".NumberValueLongInt = (Int64)" + AFieldOrNull + ";";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return ctrl.controlName + ".NumberValueDecimal = Convert.ToDecimal(" + AFieldOrNull + ");";
            }
            else
            {
                TLogging.Log("Warning: unknown type " + AFieldTypeDotNet + " for Textbox.UndoValue for control " + ctrl.controlName);
                return string.Empty;
            }
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                if (FControlMode == "Decimal")
                {
                    return ctrl.controlName + ".NumberValueDecimal == null";
                }
                else
                {
                    if (FControlMode == "Integer")
                    {
                        return ctrl.controlName + ".NumberValueInt == null";
                    }
                    else
                    {
                        return ctrl.controlName + ".NumberValueLongInt == null";
                    }
                }
            }

            if (AFieldTypeDotNet.ToLower() == "int64")
            {
                return "Convert.ToInt64(" + ctrl.controlName + ".NumberValueLongInt)";
            }
            else if (AFieldTypeDotNet.ToLower() == "int32")
            {
                return "Convert.ToInt32(" + ctrl.controlName + ".NumberValueInt)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return "Convert.ToDecimal(" + ctrl.controlName + ".NumberValueDecimal)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return "Convert.ToDecimal(" + ctrl.controlName + ".NumberValueDecimal)";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            string NumberFormat = String.Empty;

            base.SetControlProperties(writer, ctrl);

            if ((ctrl.HasAttribute("ShowLabel") && (ctrl.GetAttribute("ShowLabel").ToLower() == "false")))
            {
                writer.SetControlProperty(ctrl, "ShowLabel", "false");
            }

            // Note: the control defaults to 'ShowLabel' true, so this doesn't need to be set to 'true' in code.

            writer.SetControlProperty(ctrl, "ControlMode", "TTxtNumericTextBox.TNumericTextBoxMode." + FControlMode);
            writer.SetControlProperty(ctrl, "DecimalPlaces", FDecimalPrecision.ToString());
            writer.SetControlProperty(ctrl, "NullValueAllowed", FNullValueAllowed.ToString().ToLower());

            if (ctrl.HasAttribute("Format"))
            {
                NumberFormat = ctrl.GetAttribute("Format");
            }

            if ((NumberFormat.StartsWith("PercentInteger"))
                || (NumberFormat.StartsWith("PercentDecimal")))
            {
                writer.SetControlProperty(ctrl, "ShowPercentSign", "true");
            }

            return writer.FTemplate;
        }
    }


    /// <summary>
    /// generator for a numeric currency text box control
    /// </summary>
    public class TTxtCurrencyTextBoxGenerator : TControlGenerator
    {
        Int16 FDecimalPrecision = 2;
        bool FNullValueAllowed = true;

        /// <summary>constructor</summary>
        public TTxtCurrencyTextBoxGenerator()
            : base("txt", "Ict.Common.Controls.TTxtCurrencyTextBox")
        {
            FChangeEventName = "TextChanged";
            FHasReadOnlyProperty = true;
            FDefaultHeight = 22;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            bool ReturnValue = false;
            string NumberFormat;
            string PotentialNullValue;

//Console.WriteLine("TTxtCurrencyTextBoxGenerator ControlFitsNode");
            if (base.ControlFitsNode(curNode))
            {
                FDefaultWidth = 80;

                NumberFormat = TYml2Xml.GetAttribute(curNode, "Format");

//Console.WriteLine("TTxtCurrencyTextBoxGenerator Format: '" + NumberFormat + "'");
                if ((NumberFormat == "Currency")
                    || (NumberFormat.StartsWith("Currency(")))
                {
                    FDefaultWidth = 150;
                    ReturnValue = true;
                }

                if (ReturnValue)
                {
                    if (TYml2Xml.HasAttribute(curNode, "NullValueAllowed"))
                    {
                        PotentialNullValue = TYml2Xml.GetAttribute(curNode, "NullValueAllowed");

                        if ((PotentialNullValue == "true")
                            || (PotentialNullValue == "false"))
                        {
                            FNullValueAllowed = Convert.ToBoolean(PotentialNullValue);
                        }
                        else
                        {
                            throw new ApplicationException(
                                "TextBox with currency formatting: Value for 'NullValueAllowed' needs to be either 'true' or 'false', but is '" +
                                PotentialNullValue + "'.");
                        }
                    }
                }

                return ReturnValue;
            }

            return false;
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".NumberValueDecimal = null;";
            }
            else
            {
                if (AFieldTypeDotNet.ToLower().Contains("decimal"))
                {
                    if (AFieldOrNull == null)
                    {
                        return ctrl.controlName + ".NumberValueDecimal = null;";
                    }

                    return ctrl.controlName + ".NumberValueDecimal = Convert.ToDecimal(" + AFieldOrNull + ");";
                }
                else
                {
                    TLogging.Log("Warning: unknown type " + AFieldTypeDotNet + " for Textbox.AssignValue for control " + ctrl.controlName);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// how to undo the change of a value of a control
        /// </summary>
        protected override string UndoValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return ctrl.controlName + ".NumberValueDecimal = Convert.ToDecimal(" + AFieldOrNull + ");";
            }
            else
            {
                TLogging.Log("Warning: unknown type " + AFieldTypeDotNet + " for Textbox.UndoValue for control " + ctrl.controlName);
                return string.Empty;
            }
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".NumberValueDecimal == null";
            }

            if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return "Convert.ToDecimal(" + ctrl.controlName + ".NumberValueDecimal)";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if ((ctrl.HasAttribute("ShowLabel") && (ctrl.GetAttribute("ShowLabel").ToLower() == "false")))
            {
                writer.SetControlProperty(ctrl, "ShowLabel", "false");
            }

            writer.SetControlProperty(ctrl, "DecimalPlaces", FDecimalPrecision.ToString());
            writer.SetControlProperty(ctrl, "NullValueAllowed", FNullValueAllowed.ToString().ToLower());
            writer.SetControlProperty(ctrl, "CurrencyCode", "\"###\"");

            return writer.FTemplate;
        }
    }
    
    /// <summary>
    /// generator for a Hyperlink Textbox
    /// </summary>
    public class TTxtLinkTextBoxGenerator : TControlGenerator
    {
        private string FLinkType = "None";
        
        /// <summary>constructor</summary>
        public TTxtLinkTextBoxGenerator()
            : base("txt", "Ict.Common.Controls.TTxtLinkTextBox")
        {
            FChangeEventName = "TextChanged";
            FHasReadOnlyProperty = true;
            FDefaultHeight = 22;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if ((TYml2Xml.GetAttribute(curNode, "Format") != String.Empty))
                {
                    return false;
                }
//Console.WriteLine("TTxtLinkTextBoxGenerator ControlFitsNode");
                if (TYml2Xml.GetAttribute(curNode, "Type") == "Hyperlink")
                {
                    if(TYml2Xml.HasAttribute(curNode, "LinkType"))
                    {
//Console.WriteLine("TTxtLinkTextBoxGenerator: LinkType=" + TYml2Xml.GetAttribute(curNode, "LinkType"));                        
                        string LinkTypeStr = TYml2Xml.GetAttribute(curNode, "LinkType").ToLower();
                        
                        if (LinkTypeStr == "http") 
                        {
                            FLinkType = "Http";    
                        }
                        else if (LinkTypeStr == "ftp") 
                        {
                            FLinkType = "Ftp";    
                        }
                        else if (LinkTypeStr == "email") 
                        {
                            FLinkType = "Email";    
                        }
                        else if (LinkTypeStr == "ftp") 
                        {
                            FLinkType = "Ftp";    
                        }
                        else if (LinkTypeStr == "skype") 
                        {
                            FLinkType = "Skype";    
                        }                        
                    }

                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Text == null";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Text = String.Empty;";
            }
            
            return ctrl.controlName + ".Text = " + AFieldOrNull + ";";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl, "LinkType", "TLinkTypes." + FLinkType);

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a RichTextBox
    /// </summary>
    public class TRichTextBoxGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public TRichTextBoxGenerator()
            : base("rtb", typeof(RichTextBox))        
        {
            FChangeEventName = "TextChanged";
            FHasReadOnlyProperty = true;
            FDefaultHeight = 22;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TYml2Xml.HasAttribute(curNode, "Type"))
                {
                    if ((TYml2Xml.GetAttribute(curNode, "Type") == "Hyperlink"))
                    {
                        return false;
                    }                  
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Text = String.Empty;";
            }

            if (!AFieldTypeDotNet.ToLower().Contains("string"))
            {
                return ctrl.controlName + ".Text = " + AFieldOrNull + ".ToString();";
            }

            return ctrl.controlName + ".Text = " + AFieldOrNull + ";";
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Text.Length == 0";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "DefaultValue"))
            {
                writer.SetControlProperty(ctrl,
                    "Text",
                    "\"" + TYml2Xml.GetAttribute(ctrl.xmlNode, "DefaultValue") + "\"");
            }

            if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "Multiline")) && (TYml2Xml.GetAttribute(ctrl.xmlNode, "Multiline") == "true"))
            {
                writer.SetControlProperty(ctrl, "Multiline", "true");

                if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "WordWrap")) && (TYml2Xml.GetAttribute(ctrl.xmlNode, "WordWrap") == "false"))
                {
                    writer.SetControlProperty(ctrl, "WordWrap", "false");
                }

                if (TYml2Xml.HasAttribute(ctrl.xmlNode, "ScrollBars"))
                {
                    writer.SetControlProperty(ctrl, "ScrollBars", "ScrollBars." + TYml2Xml.GetAttribute(ctrl.xmlNode, "ScrollBars"));
                }
            }

            writer.Template.AddToCodelet("ASSIGNFONTATTRIBUTES",
                "this." + ctrl.controlName + ".Font = TAppSettingsManager.GetDefaultBoldFont();" + Environment.NewLine);

            return writer.FTemplate;
        }
    }
    
    /// <summary>
    /// generator for a RichTextBox With Hyperlinks
    /// </summary>
    public class TRichTextBoxWithHyperlinksGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public TRichTextBoxWithHyperlinksGenerator()
            : base("rtb", "Ict.Common.Controls.TRtbHyperlinks")        
        {
            FChangeEventName = "TextChanged";
            FHasReadOnlyProperty = true;
            FDefaultHeight = 22;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TYml2Xml.HasAttribute(curNode, "Type"))
                {
                    if ((TYml2Xml.GetAttribute(curNode, "Type") == "Hyperlink"))
                    {
                        return true;
                    }                  
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Text = String.Empty;";
            }

            if (!AFieldTypeDotNet.ToLower().Contains("string"))
            {
                return ctrl.controlName + ".Text = " + AFieldOrNull + ".ToString();";
            }

            return ctrl.controlName + ".Text = " + AFieldOrNull + ";";
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Text.Length == 0";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "DefaultValue"))
            {
                writer.SetControlProperty(ctrl,
                    "Text",
                    "\"" + TYml2Xml.GetAttribute(ctrl.xmlNode, "DefaultValue") + "\"");
            }

            if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "Multiline")) && (TYml2Xml.GetAttribute(ctrl.xmlNode, "Multiline") == "true"))
            {
                writer.SetControlProperty(ctrl, "Multiline", "true");

                if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "WordWrap")) && (TYml2Xml.GetAttribute(ctrl.xmlNode, "WordWrap") == "false"))
                {
                    writer.SetControlProperty(ctrl, "WordWrap", "false");
                }

                if (TYml2Xml.HasAttribute(ctrl.xmlNode, "ScrollBars"))
                {
                    writer.SetControlProperty(ctrl, "ScrollBars", "ScrollBars." + TYml2Xml.GetAttribute(ctrl.xmlNode, "ScrollBars"));
                }
            }

            writer.Template.AddToCodelet("ASSIGNFONTATTRIBUTES",
                "this." + ctrl.controlName + ".Font = TAppSettingsManager.GetDefaultBoldFont();" + Environment.NewLine);

            return writer.FTemplate;
        }
    }    
}