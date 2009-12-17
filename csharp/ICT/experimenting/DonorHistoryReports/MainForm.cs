/* auto generated with nant generateWinforms from MainForm.yaml and template windowBrowsePrint
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
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
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.DonorHistoryReports
{

  /// auto generated: Get Partner Keys of Donors
  public partial class TFrmMainForm: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;
    private DonorHistoryTDS FMainDS;

    /// constructor
    public TFrmMainForm(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblStartDate.Text = Catalog.GetString("Start Date:");
      this.lblEndDate.Text = Catalog.GetString("End Date:");
      this.lblMinimumAmount.Text = Catalog.GetString("Minimum Amount:");
      this.lblMaximumAmount.Text = Catalog.GetString("Maximum Amount:");
      this.lblMinimumCount.Text = Catalog.GetString("Minimum Count:");
      this.lblMaximumCount.Text = Catalog.GetString("Maximum Count:");
      this.chkProjects.Text = Catalog.GetString("Projects");
      this.chkSupport.Text = Catalog.GetString("Support");
      this.chkFamily.Text = Catalog.GetString("Family");
      this.chkChurch.Text = Catalog.GetString("Church");
      this.chkOrganisation.Text = Catalog.GetString("Organisation");
      this.tblTotalNumberPages.Text = Catalog.GetString("Total Number Pages");
      this.tbbPrevPage.Text = Catalog.GetString("Prev Page");
      this.tbbNextPage.Text = Catalog.GetString("Next Page");
      this.tbbPrintCurrentPage.Text = Catalog.GetString("Print Current Page");
      this.tbbPrint.Text = Catalog.GetString("Print");
      this.tbbGenerateLetters.Text = Catalog.GetString("&Generate Letters");
      this.tbbExportCSV.Text = Catalog.GetString("&Export CSV");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Get Partner Keys of Donors");
      #endregion

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);
      FMainDS = new DonorHistoryTDS();
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
    }

    private void TFrmPetra_Activated(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Activated(sender, e);
    }

    private void TFrmPetra_Load(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Load(sender, e);
    }

    private void TFrmPetra_Closing(object sender, CancelEventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Closing(sender, e);
    }

    private void Form_KeyDown(object sender, KeyEventArgs e)
    {
        FPetraUtilsObject.Form_KeyDown(sender, e);
    }

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

    }

    private void SelectRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.Gift.PrimaryKey)
            {
                string value1 = FMainDS.Gift.Rows[ARowNumberInTable][myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).mDataView[Counter][myColumn.Ordinal].ToString();
                if (value1 != value2)
                {
                    found = false;
                }
            }
            if (found)
            {
                RowNumberGrid = Counter + 1;
            }
        }
        grdDetails.Selection.ResetSelection(false);
        grdDetails.Selection.SelectRow(RowNumberGrid, true);
        // scroll to the row
        grdDetails.ShowCell(new SourceGrid.Position(RowNumberGrid, 0), true);

        FocusedRowChanged(this, new SourceGrid.RowEventArgs(RowNumberGrid));
    }

    /// return the selected row index
    private Int32 GetRowIndex(DonorHistoryTDSGiftRow row)
    {
        if (row == null)
        {
            return 1;
        }

        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.Gift.PrimaryKey)
            {
                string value1 = row[myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).mDataView.Table.Rows[Counter][myColumn.Ordinal].ToString();
                if (value1 != value2)
                {
                    found = false;
                }
            }
            if (found)
            {
                return Counter + 1;
            }
        }

        return -1;
    }

    /// return the selected row
    private DonorHistoryTDSGiftRow GetSelectedRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (DonorHistoryTDSGiftRow)SelectedGridRow[0].Row;
        }

        return null;
    }

    private void ShowData(DonorHistoryTDSGiftRow ARow)
    {
        if (ARow != null)
        {
            Int32 Id = GetRowIndex(ARow);
            ppvLetters.StartPage = Id - 1;
            RefreshPagePosition();
        }
    }

    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        // display the details of the currently selected row
        ShowData(GetSelectedRow());
    }

#region Implement interface functions

    /// auto generated
    public void RunOnceOnActivation()
    {
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
    }

    /// auto generated
    public void HookupAllInContainer(Control container)
    {
        FPetraUtilsObject.HookupAllInContainer(container);
    }

    /// auto generated
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// auto generated
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actGenerateLetters")
        {
            tbbGenerateLetters.Enabled = e.Enabled;
        }
        if (e.ActionName == "actExportCSV")
        {
            tbbExportCSV.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        mniHelpPetraHelp.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion

#region Print Preview and Printing
    void RefreshPagePosition()
    {
        tblTotalNumberPages.Text = String.Format(Catalog.GetString("of {0}"), FNumberOfPages);
        ttxCurrentPage.Text = (this.ppvLetters.StartPage + 1).ToString();
    }

    void PrevPageClick(object sender, EventArgs e)
    {
        if (this.ppvLetters.StartPage > 0)
        {
            this.ppvLetters.StartPage = this.ppvLetters.StartPage - 1;
            RefreshPagePosition();
        }
    }

    void NextPageClick(object sender, EventArgs e)
    {
        if (this.ppvLetters.StartPage + 1 < FNumberOfPages)
        {
            this.ppvLetters.StartPage = this.ppvLetters.StartPage + 1;
            RefreshPagePosition();
        }
    }

    void CurrentPageTextChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 NewCurrentPage = Convert.ToInt32(ttxCurrentPage.Text);

            if ((NewCurrentPage > 0) && (NewCurrentPage <= FNumberOfPages) && this.ppvLetters.StartPage != NewCurrentPage - 1)
            {
                this.ppvLetters.StartPage = NewCurrentPage - 1;
                SelectRowByDataTableIndex(NewCurrentPage - 1);
            }
        }
        catch (Exception)
        {
        }
    }

    void PrintCurrentPage(object sender, EventArgs e)
    {
        PrintDialog dlg = new PrintDialog();

        dlg.Document = FGfxPrinter.Document;
        dlg.AllowCurrentPage = true;
        dlg.AllowSomePages = true;
        dlg.PrinterSettings.PrintRange = PrintRange.SomePages;
        dlg.PrinterSettings.FromPage = GetRowIndex(GetSelectedRow());
        dlg.PrinterSettings.ToPage = dlg.PrinterSettings.FromPage;

        if (dlg.ShowDialog() == DialogResult.OK)
        {
            dlg.Document.Print();
        }
    }

    void PrintAllPages(object sender, System.EventArgs e)
    {
        PrintDialog dlg = new PrintDialog();

        dlg.Document = FGfxPrinter.Document;
        dlg.AllowCurrentPage = true;
        dlg.AllowSomePages = true;

        if (dlg.ShowDialog() == DialogResult.OK)
        {
            dlg.Document.Print();
        }
    }

#endregion
  }
}
