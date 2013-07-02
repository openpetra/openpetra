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
            List<Panel> FilterControls = new List<Panel>();
            List<Panel> ExtraFilterControls = new List<Panel>();
            List<Panel> FindControls = new List<Panel>();
            
            bool ShowExtraFilter = rbtTwoFilterPanels.Checked;
            bool ApplyFilterButtonStd = btnApplyFilterButtonStd.Checked;
            bool ApplyFilterButtonExtra = btnApplyFilterButtonExtra.Checked;
            bool KeepFilterTurnedOnButtonStd = btnKeepFilterTurnedOnButtonStd.Checked;
            bool KeepFilterTurnedOnButtonExtra = btnKeepFilterTurnedOnButtonExtra.Checked;
            bool FilterIsAlwaysOnLabelStd = btnFilterIsAlwaysOnLabelStd.Checked;
            bool FilterIsAlwaysOnLabelExtra = btnFilterIsAlwaysOnLabelExtra.Checked;
            
            TUcoFilterAndFind.FilterContext ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.fcNone;
            TUcoFilterAndFind.FilterContext ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.fcNone;
            TUcoFilterAndFind.FilterContext ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.fcNone;
        



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
                        ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter;
                    }
                    else
                    {
                        ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                    }
                }    
                else
                {
                    ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                }
            }
            else
            {
                if (rbtTwoFilterPanels.Checked) 
                {                
                    if (btnApplyFilterButtonExtra.Checked) 
                    {
                        ApplyFilterButtonContext = TUcoFilterAndFind.FilterContext.fcExtraFilterOnly;
                    }
                }
            }
            
            if (btnKeepFilterTurnedOnButtonStd.Checked) 
            {
                if (rbtTwoFilterPanels.Checked) 
                {
                    if (btnKeepFilterTurnedOnButtonExtra.Checked) 
                    {
                        ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter;
                    }
                    else
                    {
                        ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                    }
                }    
                else
                {
                    ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                }
            }
            else
            {
                if (rbtTwoFilterPanels.Checked) 
                {                
                    if (btnKeepFilterTurnedOnButtonExtra.Checked) 
                    {
                        ShowKeepFilterTurnedOnButtonContext = TUcoFilterAndFind.FilterContext.fcExtraFilterOnly;
                    }
                }
            }
            
            if (btnFilterIsAlwaysOnLabelStd.Checked) 
            {
                if (rbtTwoFilterPanels.Checked) 
                {
                    if (btnFilterIsAlwaysOnLabelExtra.Checked) 
                    {
                        ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter;
                    }
                    else
                    {
                        ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                    }
                }    
                else
                {
                    ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                }
            }
            else
            {
                if (rbtTwoFilterPanels.Checked) 
                {                
                    if (btnFilterIsAlwaysOnLabelExtra.Checked) 
                    {
                        ShowFilterIsAlwaysOnLabelContext = TUcoFilterAndFind.FilterContext.fcExtraFilterOnly;
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
                    case TUcoFilterAndFind.FilterContext.fcNone:                
                        FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardFilterOnly:
                        FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.fcNone;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcExtraFilterOnly:
                        FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter;
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter:
                        FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.fcExtraFilterOnly;
                        break;
                        
                    default:
                        throw new Exception("Invalid value for FilterContext");
                }                
            }    
            else
            {
                switch (FUcoFilterAndFind.ShowApplyFilterButton) 
                {
                    case TUcoFilterAndFind.FilterContext.fcNone:                
                        FUcoFilterAndFind.ShowApplyFilterButton= TUcoFilterAndFind.FilterContext.fcExtraFilterOnly;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardFilterOnly:
                        FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcExtraFilterOnly:
                        FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.fcNone;
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter:
                        FUcoFilterAndFind.ShowApplyFilterButton = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
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
                    case TUcoFilterAndFind.FilterContext.fcNone:                
                        FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardFilterOnly:
                        FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.fcNone;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcExtraFilterOnly:
                        FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter;
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter:
                        FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.fcExtraFilterOnly;
                        break;
                        
                    default:
                        throw new Exception("Invalid value for FilterContext");
                }                
            }    
            else
            {
                switch (FUcoFilterAndFind.ShowKeepFilterTurnedOnButton) 
                {
                    case TUcoFilterAndFind.FilterContext.fcNone:                
                        FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.fcExtraFilterOnly;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardFilterOnly:
                        FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcExtraFilterOnly:
                        FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.fcNone;
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter:
                        FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
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
                    case TUcoFilterAndFind.FilterContext.fcNone:                
                        FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardFilterOnly:
                        FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.fcNone;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcExtraFilterOnly:
                        FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter;
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter:
                        FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.fcExtraFilterOnly;
                        break;
                        
                    default:
                        throw new Exception("Invalid value for FilterContext");
                }                
            }    
            else
            {
                switch (FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel) 
                {
                    case TUcoFilterAndFind.FilterContext.fcNone:                
                        FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.fcExtraFilterOnly;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardFilterOnly:
                        FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter;
                        
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcExtraFilterOnly:
                        FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.fcNone;
                        break;
                        
                    case TUcoFilterAndFind.FilterContext.fcStandardAndExtraFilter:
                        FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = TUcoFilterAndFind.FilterContext.fcStandardFilterOnly;
                        break;
                        
                    default:
                        throw new Exception("Invalid value for FilterContext");
                }                
            }            
        }
    
        void BtnCollapseExpandPanelClick(object sender, EventArgs e)
        {
            if (!FUcoFilterAndFind.Collapsed) 
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
            if (FUcoFilterAndFind.FindTabShown) 
            {
                FUcoFilterAndFind.DisplayFindTab();
            }        
        }
        
        void BtnFocusFirstArgumentControlClick(object sender, System.EventArgs e)
        {
            FUcoFilterAndFind.FocusFirstArgumentControl();
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
        public static T CloneMe<T>(this T controlToClone) 
            where T : Control
        {
//            Panel PanelCtrl = null;
//            Control.ControlCollection ChildControls = null;
            
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    
            T instance = Activator.CreateInstance<T>();
    
            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if(propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                    
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
        public static T Clone<T>(this object item)
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
                return default(T);
        }    
        
    }   
}