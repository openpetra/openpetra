//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2015 by OM International
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
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

using SourceGrid;
using SourceGrid.Cells.Editors;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// The class that handles standardised Form printing in OP
    /// </summary>
    public class TStandardFormPrint
    {
        /// <summary>
        /// Applications to handle printing
        /// </summary>
        public enum TPrintUsing
        {
            /// <summary>Microsoft Word</summary>
            Word,
            /// <summary>Microsoft Excel</summary>
            Excel
        };

        /// <summary>
        /// Print the data that is shown in a grid
        /// </summary>
        /// <param name="APrintApplication">The print application to use - either Word or Excel</param>
        /// <param name="APreviewOnly">True if preview, False to print without preview</param>
        /// <param name="AModule">The module that is making the call</param>
        /// <param name="ATitleText">Title for the page</param>
        /// <param name="AGrid">A grid displaying data</param>
        /// <param name="AGridColumnOrder">Zero-based grid column number order for column titles</param>
        /// <param name="ATableColumnOrder">Zero-based table column order that matches the grid columns</param>
        /// <param name="ABaseFilter">A filter that is always applied to the grid even when it is apparently showing all rows</param>
        public static void PrintGrid(TPrintUsing APrintApplication, bool APreviewOnly, TModule AModule, string ATitleText, TSgrdDataGrid AGrid,
            int[] AGridColumnOrder, int[] ATableColumnOrder, string ABaseFilter = "")
        {
            //TFrmSelectPrintFields TFrmSelectPFields = new TFrmSelectPrintFields();
            TFormDataKeyDescriptionList recordList = new TFormDataKeyDescriptionList();

            List <TFormData>formDataList = new List <TFormData>();
            string msgTitle = Catalog.GetString("Print");

            int numColumns = Math.Min(AGridColumnOrder.GetLength(0), ATableColumnOrder.GetLength(0));

            // Title of the document
            recordList.Title = ATitleText;

            // First column: key by default (or diffrent when sort order was changed by user)
            recordList.KeyTitle = AGrid.Columns[AGridColumnOrder[0]].HeaderCell.ToString();

            // Second column: description by default (or diffrent when sort order was changed by user)
            recordList.DescriptionTitle = numColumns > 1 ? AGrid.Columns[AGridColumnOrder[1]].HeaderCell.ToString() : string.Empty;

            // Other columns
            recordList.Field3Title = numColumns > 2 ? AGrid.Columns[AGridColumnOrder[2]].HeaderCell.ToString() : string.Empty;
            recordList.Field4Title = numColumns > 3 ? AGrid.Columns[AGridColumnOrder[3]].HeaderCell.ToString() : string.Empty;
            recordList.Field5Title = numColumns > 4 ? AGrid.Columns[AGridColumnOrder[4]].HeaderCell.ToString() : string.Empty;
            recordList.Field6Title = numColumns > 5 ? AGrid.Columns[AGridColumnOrder[5]].HeaderCell.ToString() : string.Empty;
            recordList.Field7Title = numColumns > 6 ? AGrid.Columns[AGridColumnOrder[6]].HeaderCell.ToString() : string.Empty;
            recordList.Field8Title = numColumns > 7 ? AGrid.Columns[AGridColumnOrder[7]].HeaderCell.ToString() : string.Empty;
            recordList.Field9Title = numColumns > 8 ? AGrid.Columns[AGridColumnOrder[8]].HeaderCell.ToString() : string.Empty;
            recordList.Field10Title = numColumns > 9 ? AGrid.Columns[AGridColumnOrder[9]].HeaderCell.ToString() : string.Empty;

            // Look at each data row and set values for key, description and other columns and add to the Form Data list
            DataView dv = ((DevAge.ComponentModel.BoundDataView)AGrid.DataSource).DataView;

            for (int i = 0; i < dv.Count; i++)
            {
                TFormDataKeyDescription record = new TFormDataKeyDescription();
                DataRowView drv = dv[i];

                record.Key = GetPrintableText(AGrid, AGridColumnOrder[0], drv.Row[ATableColumnOrder[0]]);
                record.Description = numColumns > 1 ? GetPrintableText(AGrid, AGridColumnOrder[1], drv.Row[ATableColumnOrder[1]]) : string.Empty;
                record.Field3 = numColumns > 2 ? GetPrintableText(AGrid, AGridColumnOrder[2], drv.Row[ATableColumnOrder[2]]) : string.Empty;
                record.Field4 = numColumns > 3 ? GetPrintableText(AGrid, AGridColumnOrder[3], drv.Row[ATableColumnOrder[3]]) : string.Empty;
                record.Field5 = numColumns > 4 ? GetPrintableText(AGrid, AGridColumnOrder[4], drv.Row[ATableColumnOrder[4]]) : string.Empty;
                record.Field6 = numColumns > 5 ? GetPrintableText(AGrid, AGridColumnOrder[5], drv.Row[ATableColumnOrder[5]]) : string.Empty;
                record.Field7 = numColumns > 6 ? GetPrintableText(AGrid, AGridColumnOrder[6], drv.Row[ATableColumnOrder[6]]) : string.Empty;
                record.Field8 = numColumns > 7 ? GetPrintableText(AGrid, AGridColumnOrder[7], drv.Row[ATableColumnOrder[7]]) : string.Empty;
                record.Field9 = numColumns > 8 ? GetPrintableText(AGrid, AGridColumnOrder[8], drv.Row[ATableColumnOrder[8]]) : string.Empty;
                record.Field10 = numColumns > 9 ? GetPrintableText(AGrid, AGridColumnOrder[9], drv.Row[ATableColumnOrder[9]]) : string.Empty;
                recordList.Add(record);
            }

            formDataList.Add(recordList);

            // Work out the template file name to use.
            string formName;
            //Chooses the template depending on the number of columns
            formName = "OM Print Grid " + numColumns + " ";

            formName += (APrintApplication == TPrintUsing.Excel ? "X" : "W");

            if (numColumns <= 4)
            {
                formName += "P";        // Portrait
            }
            else
            {
                formName += "L";        // Landscape
            }

            // Get the template from the db or file system
            string templatePath = GetTemplatePath(AModule, APrintApplication, formName);

            // Tell the user if we didn't find it - and quit
            if (templatePath.Length == 0)
            {
                MessageBox.Show(Catalog.GetString("Could not find the template to use for printing."), msgTitle);
                return;
            }
#if USING_TEMPLATER
            string targetDir = TTemplaterAccess.GetFormLetterBaseDirectory(TModule.mPartner);
            TTemplaterAccess.AppendUserAndDateInfo(ref targetDir, Path.GetFileNameWithoutExtension(templatePath));

            // Set up the last few data items
            recordList.PrintedBy = UserInfo.GUserInfo.UserID;
            recordList.Date = StringHelper.DateToLocalizedString(DateTime.Now, true, true);
            recordList.Filename = Path.Combine(targetDir, ATitleText + Path.GetExtension(templatePath));

            // And the sub-title
            int nGridRowsData = AGrid.Rows.Count - 1;
            int nFullViewRowsData = new DataView(dv.Table, ABaseFilter, "", DataViewRowState.CurrentRows).Count;

            if (nGridRowsData == nFullViewRowsData)
            {
                recordList.SubTitle = String.Format(
                    Catalog.GetString("The table below shows all {0} rows of data in the data table."), nGridRowsData);
            }
            else
            {
                recordList.SubTitle = String.Format(
                    Catalog.GetString("The table below shows a filtered selection of {0} out of {1} rows of data in the complete data table."),
                    nGridRowsData, nFullViewRowsData);
            }

            // Send to Templater to print!
            try
            {
                bool allDocumentsOpened;
                bool printOnCompletion;
                string outputPath = TTemplaterAccess.PrintTemplaterDocument(TModule.mPartner,
                    formDataList,
                    templatePath,
                    false,
                    APreviewOnly,
                    !APreviewOnly,
                    out allDocumentsOpened,
                    out printOnCompletion,
                    targetDir,
                    ATitleText);

                if (printOnCompletion)
                {
                    string[] files = null;

                    if (Directory.Exists(outputPath))
                    {
                        files = Directory.GetFiles(outputPath);
                    }

                    if ((files == null) || (files.Length != 1))
                    {
                        // Where has it gone??
                        string msg = string.Format(Catalog.GetString(
                                "Unexpectedly failed to find the document to print in the folder{0}'{1}'"),
                            Environment.NewLine, outputPath);
                        MessageBox.Show(msg, Catalog.GetString("Print Error"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    string printerName;

                    // request print information from user
                    using (PrintDialog pd = new PrintDialog())
                    {
                        if (pd.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        // Remember this for use lower down
                        printerName = String.Format("\"{0}\"", pd.PrinterSettings.PrinterName);
                    }

                    TTemplaterAccess.RunPrintJob(printerName, files[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Catalog.GetString("An error occurred while trying to print the document.  The error message was: ") + ex.Message,
                    msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
#endif
        }

        private static string GetPrintableText(TSgrdDataGrid AGrid, int AGridColumnId, object ADataColumn)
        {
            string ReturnValue;

            EditorBase editor = AGrid.Columns[AGridColumnId].DataCell.Editor;

            if (editor != null)
            {
                System.ComponentModel.TypeConverter tc = editor.TypeConverter;

                if (tc is TSgrdDataGrid.PartnerKeyConverter
                    || tc is TypeConverter.TDecimalConverter
                    || tc is TypeConverter.TCurrencyConverter
                    || tc is TypeConverter.TDateConverter
                    || tc is TypeConverter.TShortTimeConverter
                    || tc is TypeConverter.TLongTimeConverter)
                {
                    ReturnValue = (string)tc.ConvertTo(ADataColumn, typeof(System.String));
                }
                else
                {
                    // Including Boolean - in a grid that is a checkbox but we want simple text
                    ReturnValue = ADataColumn.ToString();
                }
            }
            else
            {
                // Simple cell with no special editor
                ReturnValue = ADataColumn.ToString();
            }

            return ReturnValue;
        }

        private static string GetTemplatePath(TModule AModule, TPrintUsing APrintApplication, string AFormName)
        {
            string ReturnValue = string.Empty;
            string msgTitle = Catalog.GetString("Print");

#if USING_TEMPLATER
            PFormTable formTable = null;

            switch (AModule)
            {
                case TModule.mPartner:
                    formTable = TRemote.MCommon.FormTemplates.WebConnectors.DownloadPartnerFormTemplate(AFormName, "99");
                    break;

                case TModule.mPersonnel:
                    formTable = TRemote.MCommon.FormTemplates.WebConnectors.DownloadPersonnelFormTemplate(AFormName, "99");
                    break;

                case TModule.mFinance:
                    formTable = TRemote.MCommon.FormTemplates.WebConnectors.DownloadFinanceFormTemplate("PRINTGRID", AFormName, "99");
                    break;

                default:
                    break;
            }

            if ((formTable != null) && (formTable.Rows.Count == 1))
            {
                // We got something from the DB
                PFormRow row = (PFormRow)(formTable.Rows[0]);
                string base64Text = row.TemplateDocument;

                // Work out the default file editing location for this template
                string uniqueFileName = row.FormCode + "_" + row.FormName + "_" + row.FormLanguage;
                string extension = row.TemplateFileExtension.Length == 0 ? APrintApplication ==
                                   TPrintUsing.Excel ? "xlsx" : "docx" : row.TemplateFileExtension;
                string templateFileName = TFileHelper.GetDefaultTemporaryTemplatePath(uniqueFileName, extension);

                if (File.Exists(templateFileName))
                {
                    // delete the existing file
                    // If this fails it must already be open in the editor
                    Boolean showInUseMessage = false;
                    try
                    {
                        File.Delete(templateFileName);
                        System.Threading.Thread.Sleep(500);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        showInUseMessage = true;
                    }
                    catch (IOException)
                    {
                        showInUseMessage = true;
                    }

                    if (showInUseMessage)
                    {
                        string msg = Catalog.GetString("Cannot access the template file.  Maybe you already have it open in an editor?  ");
                        msg += Catalog.GetString("The file is: ");
                        msg += templateFileName;
                        msg += Environment.NewLine + Environment.NewLine;
                        msg += Catalog.GetString("Do you want to continue? Click Yes to try again or No to cancel.");

                        if (MessageBox.Show(msg, msgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                        {
                            return ReturnValue;
                        }
                    }
                }

                // Write the file
                string failMessage;

                if (TFileHelper.WriteBinaryFileConvertedFromBase64String(base64Text, templateFileName, out failMessage))
                {
                    // Succeeded in writing the file locally, so now we can open it ...
                    return templateFileName;
                }
                else
                {
                    MessageBox.Show(failMessage, msgTitle);
                    return ReturnValue;
                }
            }

            // So we did not get anything from the database.  Lets try the file system
            string tryPath = Path.Combine(TTemplaterAccess.GetFormLetterTemplateBaseDirectory(TModule.mPartner),
                AFormName + (APrintApplication == TPrintUsing.Excel ? ".xlsx" : ".docx"));

            if (File.Exists(tryPath))
            {
                // Use this one then
                return tryPath;
            }
#endif
            // Did not find anything
            return ReturnValue;
        }
    }
}
