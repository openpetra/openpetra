// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template windowBrowsePrint
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SourceGrid;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
{#IFDEF DATASETTYPE}
    private {#DATASETTYPE} FMainDS;
{#ENDIF DATASETTYPE}
    
    /// constructor
    public {#CLASSNAME}(Form AParentForm) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      {#CATALOGI18N}
      #endregion

      {#ASSIGNFONTATTRIBUTES}
      
      FPetraUtilsObject = new {#UTILOBJECTCLASS}(AParentForm, this, stbMain);
{#IFDEF DATASETTYPE}
      FMainDS = new {#DATASETTYPE}();
{#ENDIF DATASETTYPE}
      {#INITUSERCONTROLS}
      {#INITMANUALCODE}
{#IFDEF ACTIONENABLING}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}

      {#INITACTIONSTATE}
    }

    #region Show Method overrides

    /// <summary>
    /// Override of Form.Show(IWin32Window owner) Method. Caters for singleton Forms.
    /// </summary>
    /// <param name="owner">Any object that implements <see cref="IWin32Window " /> and represents the top-level window that will own this Form. </param>    
    public new void Show(IWin32Window owner)
    {
        Form OpenScreen = TFormsList.GFormsList[this.GetType().FullName];
        bool OpenSelf = true;

        if ((OpenScreen != null)
            && (OpenScreen.Modal != true))            
        {
            if (TFormsList.GSingletonForms.Contains(this.GetType().Name)) 
            {
//                MessageBox.Show("Activating singleton screen of Type '" + this.GetType().FullName + "'.");
                                   
                OpenSelf = false;
                this.Visible = false;   // needed as this.Close() would otherwise bring this Form to the foreground and OpenScreen.BringToFront() would not help...
                this.Close();
                
                OpenScreen.BringToFront();
            }            
        }
        
        if (OpenSelf) 
        {
            if (owner != null) 
            {
                base.Show(owner);    
            }
            else
            {
                base.Show();
            }            
        }        
    }

    /// <summary>
    /// Override of Form.Show() Method. Caters for singleton Forms.
    /// </summary>        
    public new void Show()
    {
        this.Show(null);
    }

    #endregion

    {#EVENTHANDLERSIMPLEMENTATION}

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
            foreach (DataColumn myColumn in FMainDS.{#MASTERTABLE}.PrimaryKey)
            {
                string value1 = FMainDS.{#MASTERTABLE}.Rows[ARowNumberInTable][myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][myColumn.Ordinal].ToString();
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
        grdDetails.ShowCell(RowNumberGrid);

        FocusedRowChanged(this, new SourceGrid.RowEventArgs(RowNumberGrid));
    }

    /// return the selected row index
    private Int32 GetRowIndex({#MASTERTABLETYPE}Row row)
    {
        if (row == null)
        {
            return 1;
        }

        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.{#MASTERTABLE}.PrimaryKey)
            {
                string value1 = row[myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView.Table.Rows[Counter][myColumn.Ordinal].ToString();
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
    private {#MASTERTABLETYPE}Row GetSelectedRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return ({#MASTERTABLETYPE}Row)SelectedGridRow[0].Row;
        }

        return null;
    }

    private void ShowData({#MASTERTABLETYPE}Row ARow)
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
        {#RUNONCEINTERFACEIMPLEMENTATION}
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
        {#HOOKUPINTERFACEIMPLEMENTATION}
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
        {#ACTIONENABLING}
        {#ACTIONENABLINGDISABLEMISSINGFUNCS}
    }

    {#ACTIONHANDLERS}

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