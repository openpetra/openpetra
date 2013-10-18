//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using Ict.Common.Controls;
using Ict.Common.IO;

namespace ControlTestBench
{
/// <summary>
/// </summary>
public partial class FilterFindTest : Form
{
    /// <summary>
    /// </summary>
    public FilterFindTest()
    {
        InitializeComponent();
    }

    void TestDefaultConstructor(object sender, EventArgs e)
    {
        TUcoFilterAndFind FilterAndFind = new TUcoFilterAndFind();

        this.Controls.Remove(FUcoFilterAndFind);
        FUcoFilterAndFind = FilterAndFind;
        FUcoFilterAndFind.Dock = System.Windows.Forms.DockStyle.Left;

        this.Controls.Add(FilterAndFind);
    }

    void TestFullConstructor(object sender, EventArgs e)
    {
        TUcoFilterAndFind FilterAndFind;

        List <Panel>FilterControls = new List <Panel>();
        List <Panel>ExtraFilterControls = new List <Panel>();
        List <Panel>FindControls = new List <Panel>();

        bool ShowExtraFilter = rbtTwoFilterPanels.Checked;
        bool ApplyFilterButtonStd = btnApplyFilterButtonStd.Checked;
        bool ApplyFilterButtonExtra = btnApplyFilterButtonExtra.Checked;
        bool KeepFilterTurnedOnButtonStd = btnKeepFilterTurnedOnButtonStd.Checked;
        bool KeepFilterTurnedOnButtonExtra = btnKeepFilterTurnedOnButtonExtra.Checked;
        bool FilterIsAlwaysOnLabelStd = btnFilterIsAlwaysOnLabelStd.Checked;
        bool FilterIsAlwaysOnLabelExtra = btnFilterIsAlwaysOnLabelExtra.Checked;

        TUcoFilterAndFind.FilterContext ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.None;
        TUcoFilterAndFind.FilterContext ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.None;
        TUcoFilterAndFind.FilterContext ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.None;


        // Build Control Lists

        #region Panel Instances

        if (chkCurrencyCodeStd.Checked)
        {
            FilterControls.Add(pnlCurrencyCode);
        }

        if (chkCurrencyCodeExtra.Checked)
        {
            ExtraFilterControls.Add(pnlCurrencyCode);
        }

        if (chkCurrencyCodeFind.Checked)
        {
            FindControls.Add(pnlCurrencyCode);
        }

        if (chkCurrencyNameStd.Checked)
        {
            FilterControls.Add(pnlCurrencyName);
        }

        if (chkCurrencyNameExtra.Checked)
        {
            ExtraFilterControls.Add(pnlCurrencyName);
        }

        if (chkCurrencyNameFind.Checked)
        {
            FindControls.Add(pnlCurrencyName);
        }

        if (chkYearStd.Checked)
        {
            FilterControls.Add(pnlYear);
        }

        if (chkYearExtra.Checked)
        {
            ExtraFilterControls.Add(pnlYear);
        }

        if (chkYearFind.Checked)
        {
            FindControls.Add(pnlYear);
        }

        if (btnApplyFilterButtonStd.Checked)
        {
            if (rbtTwoFilterPanels.Checked)
            {
                if (btnApplyFilterButtonExtra.Checked)
                {
                    ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.StandardAndExtraFilter;
                }
                else
                {
                    ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.StandardFilterOnly;
                }
            }
            else
            {
                ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.StandardFilterOnly;
            }
        }
        else
        {
            if (rbtTwoFilterPanels.Checked)
            {
                if (btnApplyFilterButtonExtra.Checked)
                {
                    ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.ExtraFilterOnly;
                }
            }
        }

        if (btnKeepFilterTurnedOnButtonStd.Checked)
        {
            if (rbtTwoFilterPanels.Checked)
            {
                if (btnKeepFilterTurnedOnButtonExtra.Checked)
                {
                    ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.StandardAndExtraFilter;
                }
                else
                {
                    ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.StandardFilterOnly;
                }
            }
            else
            {
                ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.StandardFilterOnly;
            }
        }
        else
        {
            if (rbtTwoFilterPanels.Checked)
            {
                if (btnKeepFilterTurnedOnButtonExtra.Checked)
                {
                    ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.ExtraFilterOnly;
                }
            }
        }

        if (btnFilterIsAlwaysOnLabelStd.Checked)
        {
            if (rbtTwoFilterPanels.Checked)
            {
                if (btnFilterIsAlwaysOnLabelExtra.Checked)
                {
                    ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.StandardAndExtraFilter;
                }
                else
                {
                    ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.StandardFilterOnly;
                }
            }
            else
            {
                ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.StandardFilterOnly;
            }
        }
        else
        {
            if (rbtTwoFilterPanels.Checked)
            {
                if (btnFilterIsAlwaysOnLabelExtra.Checked)
                {
                    ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.ExtraFilterOnly;
                }
            }
        }

        #endregion


        #region Control Instances

        if (chkDynamicCtrl1Std.Checked)
        {
            FilterControls.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(lblDynCtrl1, txtDynCtrl1));
        }

        if (chkDynamicCtrl1Extra.Checked)
        {
            ExtraFilterControls.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(lblDynCtrl1, txtDynCtrl1));
        }

        if (chkDynamicCtrl1Find.Checked)
        {
            FindControls.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(lblDynCtrl1, txtDynCtrl1));;
        }

        if (chkDynamicCtrl2Std.Checked)
        {
            FilterControls.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(txtLblDynCtrl2.Text, chkDynCtrl2));
        }

        if (chkDynamicCtrl2Extra.Checked)
        {
            ExtraFilterControls.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(txtLblDynCtrl2.Text, chkDynCtrl2));
        }

        if (chkDynamicCtrl2Find.Checked)
        {
            FindControls.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(txtLblDynCtrl2.Text, chkDynCtrl2));
        }

        if (chkDynamicCtrl3Std.Checked)
        {
            FilterControls.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(lblDynCtrl3.Text, cmbDynCtrl3));
        }

        if (chkDynamicCtrl3Extra.Checked)
        {
            ExtraFilterControls.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(lblDynCtrl3.Text, cmbDynCtrl3));
        }

        if (chkDynamicCtrl3Find.Checked)
        {
            FindControls.Add(TUcoFilterAndFind.ArgumentPanelHelper.CreateArgumentPanel(lblDynCtrl3.Text, cmbDynCtrl3));
        }

        #endregion

        FilterAndFind = new TUcoFilterAndFind(FilterControls, ExtraFilterControls, FindControls,
            ApplyFilterButtonContext, ShowKeepFilterTurnedOnButtonContext, ShowFilterIsAlwaysOnLabelContext,
            System.Int32.Parse(txtControlWidth.Text));

        this.Controls.Remove(FUcoFilterAndFind);
        FUcoFilterAndFind = FilterAndFind;
        FUcoFilterAndFind.Dock = System.Windows.Forms.DockStyle.Left;

        this.Controls.Add(FilterAndFind);

        FUcoFilterAndFind.Expanded += delegate(object UcoEventSender, EventArgs UcoEventArgs) {
            MultiEventHandler(UcoEventSender, UcoEventArgs, "Expanded");
        };
        FUcoFilterAndFind.Collapsed += delegate(object UcoEventSender, EventArgs UcoEventArgs) {
            MultiEventHandler(UcoEventSender, UcoEventArgs, "Collapsed");
        };
        FUcoFilterAndFind.FindTabDisplayed += delegate(object UcoEventSender, EventArgs UcoEventArgs) {
            MultiEventHandler(UcoEventSender, UcoEventArgs, "FindTabDisplayed");
        };
        FUcoFilterAndFind.ApplyFilterClicked += delegate(object UcoEventSender, TUcoFilterAndFind.TContextEventExtControlArgs UcoEventArgs) {
            MultiEventHandler(UcoEventSender, UcoEventArgs, "ApplyFilterClicked");
        };
        FUcoFilterAndFind.KeepFilterTurnedOnClicked +=
            delegate(object UcoEventSender, TUcoFilterAndFind.TContextEventExtButtonDepressedArgs UcoEventArgs) {
            MultiEventHandler(UcoEventSender, UcoEventArgs, "KeepFilterTurnedOnClicked");
        };
        FUcoFilterAndFind.FindNextClicked += delegate(object UcoEventSender, TUcoFilterAndFind.TContextEventExtSearchDirectionArgs UcoEventArgs) {
            MultiEventHandler(UcoEventSender, UcoEventArgs, "FindNextClicked");
        };
        FUcoFilterAndFind.ClearArgumentCtrlButtonClicked +=
            delegate(object UcoEventSender, TUcoFilterAndFind.TContextEventExtControlArgs UcoEventArgs) {
            MultiEventHandler(UcoEventSender, UcoEventArgs, "ClearArgumentCtrlButtonClicked");
        };
        FUcoFilterAndFind.TabSwitched += delegate(object UcoEventSender, TUcoFilterAndFind.TContextEventArgs UcoEventArgs) {
            MultiEventHandler(UcoEventSender, UcoEventArgs, "TabSwitched");
        };
        FUcoFilterAndFind.ArgumentCtrlValueChanged +=
            delegate(object UcoEventSender, TUcoFilterAndFind.TContextEventExtControlValueArgs UcoEventArgs) {
            MultiEventHandler(UcoEventSender, UcoEventArgs, "ArgumentCtrlValueChanged");
        };
    }

    void RbtOneFilterPanelCheckedChanged(object sender, System.EventArgs e)
    {
        grpExtraFilterPanel.Enabled = false;
    }

    void RbtTwoFilterPanelsCheckedChanged(object sender, EventArgs e)
    {
        grpExtraFilterPanel.Enabled = true;
    }

    void BtnHideShowAFBtnClick(object sender, EventArgs e)
    {
        if (sender == btnHideShowAFBtnStd)
        {
            switch (FUcoFilterAndFind.ShowApplyFilterButton)
            {
                case TUcoFilterAndFind.FilterContext.None:
                    FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.StandardFilterOnly;

                    break;

                case TUcoFilterAndFind.FilterContext.StandardFilterOnly:
                    FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.None;

                    break;

                case TUcoFilterAndFind.FilterContext.ExtraFilterOnly:
                    FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.StandardAndExtraFilter;
                    break;

                case TUcoFilterAndFind.FilterContext.StandardAndExtraFilter:
                    FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.ExtraFilterOnly;
                    break;

                default:
                    throw new Exception("Invalid value for FilterContext");
            }
        }
        else
        {
            switch (FUcoFilterAndFind.ShowApplyFilterButton)
            {
                case TUcoFilterAndFind.FilterContext.None:
                    FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.ExtraFilterOnly;

                    break;

                case TUcoFilterAndFind.FilterContext.StandardFilterOnly:
                    FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.StandardAndExtraFilter;

                    break;

                case TUcoFilterAndFind.FilterContext.ExtraFilterOnly:
                    FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.None;
                    break;

                case TUcoFilterAndFind.FilterContext.StandardAndExtraFilter:
                    FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.StandardFilterOnly;
                    break;

                default:
                    throw new Exception("Invalid value for FilterContext");
            }
        }
    }

    void BtnHideShowKFTOBtnClick(object sender, EventArgs e)
    {
        if (sender == btnHideShowKFTOBtnStd)
        {
            switch (FUcoFilterAndFind.ShowKeepFilterTurnedOnButton)
            {
                case TUcoFilterAndFind.FilterContext.None:
                    FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.StandardFilterOnly;

                    break;

                case TUcoFilterAndFind.FilterContext.StandardFilterOnly:
                    FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.None;

                    break;

                case TUcoFilterAndFind.FilterContext.ExtraFilterOnly:
                    FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.StandardAndExtraFilter;
                    break;

                case TUcoFilterAndFind.FilterContext.StandardAndExtraFilter:
                    FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.ExtraFilterOnly;
                    break;

                default:
                    throw new Exception("Invalid value for FilterContext");
            }
        }
        else
        {
            switch (FUcoFilterAndFind.ShowKeepFilterTurnedOnButton)
            {
                case TUcoFilterAndFind.FilterContext.None:
                    FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.ExtraFilterOnly;

                    break;

                case TUcoFilterAndFind.FilterContext.StandardFilterOnly:
                    FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.StandardAndExtraFilter;

                    break;

                case TUcoFilterAndFind.FilterContext.ExtraFilterOnly:
                    FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.None;
                    break;

                case TUcoFilterAndFind.FilterContext.StandardAndExtraFilter:
                    FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.StandardFilterOnly;
                    break;

                default:
                    throw new Exception("Invalid value for FilterContext");
            }
        }
    }

    void BtnHideShowFIAOLblClick(object sender, EventArgs e)
    {
        if (sender == btnHideShowFIAOLblStd)
        {
            switch (FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel)
            {
                case TUcoFilterAndFind.FilterContext.None:
                    FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.StandardFilterOnly;

                    break;

                case TUcoFilterAndFind.FilterContext.StandardFilterOnly:
                    FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.None;

                    break;

                case TUcoFilterAndFind.FilterContext.ExtraFilterOnly:
                    FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.StandardAndExtraFilter;
                    break;

                case TUcoFilterAndFind.FilterContext.StandardAndExtraFilter:
                    FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.ExtraFilterOnly;
                    break;

                default:
                    throw new Exception("Invalid value for FilterContext");
            }
        }
        else
        {
            switch (FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel)
            {
                case TUcoFilterAndFind.FilterContext.None:
                    FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.ExtraFilterOnly;

                    break;

                case TUcoFilterAndFind.FilterContext.StandardFilterOnly:
                    FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.StandardAndExtraFilter;

                    break;

                case TUcoFilterAndFind.FilterContext.ExtraFilterOnly:
                    FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.None;
                    break;

                case TUcoFilterAndFind.FilterContext.StandardAndExtraFilter:
                    FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.StandardFilterOnly;
                    break;

                default:
                    throw new Exception("Invalid value for FilterContext");
            }
        }
    }

    void BtnCollapseExpandPanelClick(object sender, EventArgs e)
    {
        if (!FUcoFilterAndFind.IsCollapsed)
        {
            FUcoFilterAndFind.Collapse();
        }
        else
        {
            FUcoFilterAndFind.Expand();
        }
    }

    void Button1Click(object sender, EventArgs e)
    {
        // Attempt to show the Find Tab only if it is there...
        if (FUcoFilterAndFind.IsFindTabShown)
        {
            FUcoFilterAndFind.DisplayFindTab();
        }
    }

    void BtnFocusFirstArgumentControlClick(object sender, System.EventArgs e)
    {
        FUcoFilterAndFind.FocusFirstArgumentControl();
    }

    void MultiEventHandler(object sender, EventArgs e, string AEventName)
    {
        TUcoFilterAndFind.TContextEventArgs ContextArgs = e as TUcoFilterAndFind.TContextEventArgs;

        txtEventsLog.Text += DateTime.Now.ToLongTimeString() + "   Event: '" + AEventName + "'";

        if (ContextArgs == null)
        {
            txtEventsLog.Text += "." + Environment.NewLine;
        }
        else
        {
            if (ContextArgs is TUcoFilterAndFind.TContextEventExtButtonDepressedArgs)
            {
                txtEventsLog.Text += "  (Context: '" + ContextArgs.Context.ToString("G") + "' - Button is " +
                                     (((TUcoFilterAndFind.TContextEventExtButtonDepressedArgs)ContextArgs).ButtonIsDepressed ? "depressed" :
                                      "not depressed") + ")." +
                                     Environment.NewLine;
            }
            else if (ContextArgs is TUcoFilterAndFind.TContextEventExtSearchDirectionArgs)
            {
                txtEventsLog.Text += "  (Context: '" + ContextArgs.Context.ToString("G") + "' - search direction is " +
                                     (((TUcoFilterAndFind.TContextEventExtSearchDirectionArgs)ContextArgs).SearchUpwards ? "upwards" : "downwards") +
                                     ")." +
                                     Environment.NewLine;
            }
            else if (ContextArgs is TUcoFilterAndFind.TContextEventExtControlValueArgs)
            {
                txtEventsLog.Text += "  (Context: '" + ContextArgs.Context.ToString("G") + "' - Control: '" +
                                     ((TUcoFilterAndFind.TContextEventExtControlValueArgs)ContextArgs).AffectedControl.Name + "'. Changed Value: " +
                                     ((TUcoFilterAndFind.TContextEventExtControlValueArgs)ContextArgs).Value.ToString() + " (Type: " +
                                     ((TUcoFilterAndFind.TContextEventExtControlValueArgs)ContextArgs).TypeOfValue.FullName + ")" +
                                     Environment.NewLine;
            }
            else if (ContextArgs is TUcoFilterAndFind.TContextEventExtControlArgs)
            {
                txtEventsLog.Text += "  (Context: '" + ContextArgs.Context.ToString("G") + "' - associated Control: '" +
                                     ((TUcoFilterAndFind.TContextEventExtControlArgs)ContextArgs).AffectedControl.Name + "'." + Environment.NewLine;
                Application.DoEvents();

                if (ContextArgs.Action != null)
                {
                    ContextArgs.Action(((TUcoFilterAndFind.TContextEventExtControlArgs)ContextArgs).AffectedControl);
                }

                if (ContextArgs.ResetAction != null)
                {
                    System.Threading.Thread.Sleep(1000);

                    ContextArgs.ResetAction(((TUcoFilterAndFind.TContextEventExtControlArgs)ContextArgs).AffectedControl);
                }
            }
            else
            {
                txtEventsLog.Text += "  (Context: '" + ContextArgs.Context.ToString("G") + "')." + Environment.NewLine;
            }
        }

        // Scroll to the last line in the TextBox
        txtEventsLog.SelectionStart = txtEventsLog.Text.Length;
        txtEventsLog.SelectionLength = 0;
        txtEventsLog.ScrollToCaret();
    }

    void BtnAllowedToSetFilterToInactiveClick(object sender, EventArgs e)
    {
        if ((FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel == TUcoFilterAndFind.FilterContext.None)
            && (FUcoFilterAndFind.KeepFilterTurnedOnButtonDepressed == TUcoFilterAndFind.FilterContext.None))
        {
            MessageBox.Show("One is allowed to set the Filter to inactive.\r\nShowFilterIsAlwaysOnLabel: " +
                FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel.ToString("G") + "\r\n" +
                "KeepFilterTurnedOnButtonDepressed: " + FUcoFilterAndFind.KeepFilterTurnedOnButtonDepressed.ToString("G"));
        }
        else
        {
            MessageBox.Show("One is NOT ALLOWED to set the Filter to inactive.\r\nShowFilterIsAlwaysOnLabel: " +
                FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel.ToString("G") + "\r\n" +
                "KeepFilterTurnedOnButtonDepressed: " + FUcoFilterAndFind.KeepFilterTurnedOnButtonDepressed.ToString("G"));
        }
    }
}

/// <summary>
/// Small utility class for Clone support for Controls... provides 'Clone' Extension Method to any Control!
/// </summary>
public static class ControlExtensions
{
    /// <summary>
    /// Clones any Control!
    /// </summary>
    /// <param name="controlToClone">Control that should be 'cloned'.</param>
    /// <returns></returns>
    public static T CloneMe <T>(this T controlToClone)
    where T : Control
    {
//            Panel PanelCtrl = null;
//            Control.ControlCollection ChildControls = null;

        PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        T instance = Activator.CreateInstance <T>();

        foreach (PropertyInfo propInfo in controlProperties)
        {
            if (propInfo.CanWrite)
            {
                if (propInfo.Name != "WindowTarget")
                {
                    propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }

//                else
//                {
//                    if (propInfo.Name == "Controls")
//                    {
//                        if (ChildControls == null)
//                        {
//                            ChildControls = new Control.ControlCollection(instance);
//                        }
//                        propInfo.SetValue(instance, ((ICloneable)propInfo).Clone(), null);
////                        ChildControls.Add(((Control)propInfo.GetValue(instance, new object[] { 0 } )));
//                    }
//                }
        }

//
//            if (ChildControls != null)
//            {
//                foreach (Control SingleControl in ChildControls)
//                {
//                    PanelCtrl.Controls.Add(SingleControl.Clone());
//                }
//            }

        return instance;
    }

    /// <summary>
    /// Makes a copy from the object.
    /// Doesn't copy the reference memory, only data.
    /// </summary>
    /// <typeparam name="T">Type of the return object.</typeparam>
    /// <param name="item">Object to be copied.</param>
    /// <returns>Returns the copied object.</returns>
    public static T Clone <T>(this object item)
    {
        if (item != null)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);

            T result = (T)formatter.Deserialize(stream);

            stream.Close();

            return result;
        }
        else
        {
            return default(T);
        }
    }
}
}