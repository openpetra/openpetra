//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, alanp
//
// Copyright 2004-2016 by OM International
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
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using GNU.Gettext;

namespace Ict.Common.IO
{
    /// <summary>
    /// Description of SelectCSVSeparator.
    /// </summary>
    public partial class TDlgSelectCSVSeparator : Form
    {
        /// <summary>american number format: decimal point, comma for thousands separator</summary>
        public static string NUMBERFORMAT_AMERICAN = "American";
        /// <summary>european number format: decimal comma, point for thousands separator</summary>
        public static string NUMBERFORMAT_EUROPEAN = "European";

        /// <summary>The connected UserID</summary>
        private static string FUserID = null;

        private bool FIsActivatedOnce = false;

        private int MAXLINESPARSE = 50;
        private int MAXLINESDISPLAY = 5;
        private int MAXBYTESPARSE = 4000;
        private string FSeparator;
        private bool FFileHasCaption;
        private List <String>FCSVRows = null;

        // Encoding
        private Encoding FCurrentEncoding = null;
        private byte[] FRawBytes = null;
        private string FFileContent = string.Empty;

        /// <summary>
        /// Set the user connection ID
        /// </summary>
        public static string UserID
        {
            set
            {
                FUserID = value;
            }
        }

        /// <summary>
        /// read the separator that the user has selected
        /// </summary>
        public string SelectedSeparator
        {
            get
            {
                return FSeparator;
            }
            set
            {
                FSeparator = value;
                UpdateRadioButtons();
            }
        }

        /// <summary>
        /// read/set the date format that the user has selected
        /// </summary>
        public string DateFormat
        {
            get
            {
                return cmbDateFormat.SelectedItem.ToString();
            }
            set
            {
                //Conversion of some old Petra Values
                if (value.Equals("MDY"))
                {
                    value = "MM/dd/yyyy";
                }
                else
                {
                    if (value.Equals("DMY"))
                    {
                        value = "dd/MM/yyyy";
                    }
                }

                if (!cmbDateFormat.Items.Contains(value))
                {
                    cmbDateFormat.Items.Add(value);
                }

                cmbDateFormat.SelectedItem = value;
            }
        }

        /// <summary>
        /// returns a string constant for the selected number format
        /// </summary>
        public String NumberFormat
        {
            set
            {
                cmbNumberFormat.SelectedIndex = (value == NUMBERFORMAT_AMERICAN ? 0 : 1);
            }
            get
            {
                return cmbNumberFormat.SelectedIndex == 0 ? NUMBERFORMAT_AMERICAN : NUMBERFORMAT_EUROPEAN;
            }
        }

        /// <summary>
        /// Returns the content of the file using the selected text encoding
        /// </summary>
        public string FileContent
        {
            get
            {
                return FFileContent;
            }
        }

        /// <summary>
        /// Returns the selected encoding of the current file
        /// </summary>
        public Encoding CurrentEncoding
        {
            get
            {
                return FCurrentEncoding;
            }
        }

        /// <summary>
        /// constructor
        /// TODO: select if first row contains captions? or use a parameter to avoid or request captions?
        /// </summary>
        public TDlgSelectCSVSeparator(bool AFileHasCaption)
        {
            FFileHasCaption = AFileHasCaption;

            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.rbtComma.Text = Catalog.GetString("Comma");
            this.rbtTabulator.Text = Catalog.GetString("Tabulator");
            this.rbtOther.Text = Catalog.GetString("Other Separator") + ":";
            this.rbtSemicolon.Text = Catalog.GetString("Semicolon");
            this.btnCancel.Text = Catalog.GetString("Cancel");
            this.btnOK.Text = Catalog.GetString("OK");
            this.lblDateFormat.Text = Catalog.GetString("Date format") + ":";
            this.lblNumberFormat.Text = Catalog.GetString("Number format") + ":";
            this.Text = Catalog.GetString("Select CSV Separator");
            #endregion

            FSeparator = TAppSettingsManager.GetValue("CSVSeparator",
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
            System.Globalization.CultureInfo myCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            String regionalDateString = myCulture.DateTimeFormat.ShortDatePattern;

            if (!cmbDateFormat.Items.Contains(regionalDateString))
            {
                cmbDateFormat.Items.Insert(0, regionalDateString);
            }

            cmbDateFormat.SelectedIndex = cmbDateFormat.Items.IndexOf(regionalDateString);

            cmbNumberFormat.SelectedIndex = cmbNumberFormat.Items.IndexOf(myCulture.NumberFormat.NumberDecimalSeparator);

            TTextFileEncoding dataSource = new TTextFileEncoding();
            this.cmbTextEncoding.ValueMember = TTextFileEncoding.ColumnCodeDbName;
            this.cmbTextEncoding.DisplayMember = TTextFileEncoding.ColumnDescriptionDbName;
            this.cmbTextEncoding.DataSource = dataSource.DefaultView;

            UpdateRadioButtons();
        }

        private void Form_Activated(object sender, EventArgs e)
        {
            if (FIsActivatedOnce)
            {
                return;
            }

            FIsActivatedOnce = true;
            RestorePreviousScreenPosition();

            DataView dv = (DataView)cmbTextEncoding.DataSource;

            for (int i = 0; i < dv.Count; i++)
            {
                if (Convert.ToInt32(dv[i].Row[TTextFileEncoding.ColumnCodeDbName]) == FCurrentEncoding.CodePage)
                {
                    cmbTextEncoding.SelectedIndex = i;
                    break;
                }
            }

            cmbTextEncoding.SelectedIndexChanged += CmbTextEncoding_SelectedIndexChanged;
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            SaveScreenPosition();
        }

        private void CmbTextEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            FCurrentEncoding = Encoding.GetEncoding(
                Convert.ToInt32(((DataView)cmbTextEncoding.DataSource)[cmbTextEncoding.SelectedIndex].Row[TTextFileEncoding.ColumnCodeDbName]));
            FFileContent = FCurrentEncoding.GetString(FRawBytes);

            DisplayGrid();
        }

        private void UpdateRadioButtons()
        {
            if (FSeparator == ";")
            {
                rbtSemicolon.Checked = true;
            }
            else if (FSeparator == ",")
            {
                rbtComma.Checked = true;
            }
            else if (FSeparator == "\t")
            {
                rbtTabulator.Checked = true;
            }
            else
            {
                rbtOther.Checked = true;
                txtOtherSeparator.Text = FSeparator;
            }
        }

        /// <summary>
        /// set CSV data directly, not reading from file.
        /// used when pasting from clipboard
        /// </summary>
        public string CSVData
        {
            set
            {
                FFileContent = value;
                FCurrentEncoding = Encoding.Unicode;

                TTextFileEncoding.SetComboBoxProperties(cmbTextEncoding, true, false, FCurrentEncoding);
                DisplayGrid();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AfileName"></param>
        /// <returns></returns>
        public Boolean OpenCsvFile(String AfileName)
        {
            // We don't need to read the whole file for this because we will only display the first 5 lines
            bool hasBOM, isAmbiguousUTF;

            if (TTextFile.AutoDetectTextEncodingAndOpenFile(AfileName, out FFileContent, out FCurrentEncoding,
                    out hasBOM, out isAmbiguousUTF, out FRawBytes) == false)
            {
                return false;
            }

            TTextFileEncoding.SetComboBoxProperties(cmbTextEncoding, hasBOM, isAmbiguousUTF, FCurrentEncoding);
            DisplayGrid();
            return true;
        }

        private void DisplayGrid()
        {
            using (StringReader reader = new StringReader(FFileContent))
            {
                FCSVRows = new List <string>();

                string line = reader.ReadLine();

                while (line != null && FCSVRows.Count < MAXLINESPARSE)
                {
                    FCSVRows.Add(line);
                    line = reader.ReadLine();
                }

                reader.Close();
            }

            RbtCheckedChanged(null, null);
        }

        private void RbtCheckedChanged(object sender, EventArgs e)
        {
            txtOtherSeparator.Enabled = rbtOther.Checked;

            if (rbtComma.Checked)
            {
                FSeparator = ",";
            }
            else if (rbtSemicolon.Checked)
            {
                FSeparator = ";";
            }
            else if (rbtTabulator.Checked)
            {
                FSeparator = "\t";
            }
            else if (rbtOther.Checked)
            {
                FSeparator = txtOtherSeparator.Text;
            }

            if ((FSeparator.Length > 0) && (FCSVRows != null) && (FCSVRows.Count > 0))
            {
                DataTable table = new DataTable();
                string line = FCSVRows[0];
                int counter = 0;

                if (FFileHasCaption)
                {
                    while (line.Length > 0)
                    {
                        string header = StringHelper.GetNextCSV(ref line, FSeparator);

                        if (header.StartsWith("#"))
                        {
                            header = header.Substring(1);
                        }

                        table.Columns.Add(header);
                    }

                    counter++;
                }

                char columnCounter = (char)('A' + table.Columns.Count);

                for (; counter < FCSVRows.Count; counter++)
                {
                    line = FCSVRows[counter];
                    DataRow row = table.NewRow();
                    int countColumns = 0;

                    try
                    {
                        while (line.Length > 0)
                        {
                            if (countColumns + 1 > table.Columns.Count)
                            {
                                table.Columns.Add(columnCounter.ToString());
                                columnCounter++;
                            }

                            // cope with cells containing new line (quoted)
                            row[countColumns] = StringHelper.GetNextCSV(ref line, FCSVRows, ref counter, FSeparator);
                            countColumns++;
                        }

                        table.Rows.Add(row);

                        if (table.Rows.Count == MAXLINESDISPLAY)
                        {
                            break;
                        }
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        // ignore this exception, it can happen because we only parse the first lines, and a cell might spread across several lines
                        break;
                    }
                }

                table.DefaultView.AllowNew = false;
                table.DefaultView.AllowDelete = false;
                table.DefaultView.AllowEdit = false;

                grdPreview.SuspendLayout();
                grdPreview.DataSource = table;
                grdPreview.ResumeLayout();
            }
        }

        #region Save/Restore screen position (we cannot use the FPetraUtilsObject facility in Common.IO)

        private bool GetScreenFilePath(out string APath)
        {
            string localAppDataPath = Path.Combine(
                TAppSettingsManager.GetLocalAppDataPath(),
                "OpenPetraOrg",
                System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName);
            string settingsFileName = String.Format("{0}CSVSeparatorDialog.cfg", FUserID == null ? "" : FUserID + ".");

            try
            {
                if (!Directory.Exists(localAppDataPath))
                {
                    Directory.CreateDirectory(localAppDataPath);
                }

                APath = Path.Combine(localAppDataPath, settingsFileName);
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Exception occurred while creating the folder for a window position file '{0}': {1}", localAppDataPath,
                        ex.Message), TLoggingType.ToLogfile);
                APath = string.Empty;
                return false;
            }

            return true;
        }

        private void SaveScreenPosition()
        {
            // Now start on the window position and size
            string windowProperties;

            if (WindowState == FormWindowState.Normal)
            {
                windowProperties = String.Format("{0};{1};{2};{3};{4}",
                    Left,
                    Top,
                    Width,
                    Height,
                    "Normal");
            }
            else
            {
                windowProperties = String.Format("{0};{1};{2};{3};{4}",
                    RestoreBounds.Left,
                    RestoreBounds.Top,
                    RestoreBounds.Width,
                    RestoreBounds.Height,
                    WindowState.ToString());
            }

            string settingsPath;

            if (GetScreenFilePath(out settingsPath))
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(settingsPath))
                    {
                        sw.WriteLine(windowProperties);
                        sw.Close();
                    }
                }
                catch (Exception ex)
                {
                    TLogging.Log(String.Format("Exception occurred while saving the window position file '{0}': {1}", settingsPath,
                            ex.Message), TLoggingType.ToLogfile);
                }
            }
        }

        private void RestorePreviousScreenPosition()
        {
            string settingsPath;

            if (GetScreenFilePath(out settingsPath))
            {
                try
                {
                    string[] details = null;

                    if (File.Exists(settingsPath))
                    {
                        using (StreamReader sr = new StreamReader(settingsPath))
                        {
                            while (!sr.EndOfStream)
                            {
                                string oneLine = sr.ReadLine();

                                if (oneLine.Length > 0)
                                {
                                    details = oneLine.Split(';');
                                    break;
                                }
                            }

                            sr.Close();
                        }

                        if (details.Length == 5)
                        {
                            // parse the left, top, width and height
                            int l = int.Parse(details[0]);
                            int t = int.Parse(details[1]);
                            int w = int.Parse(details[2]);
                            int h = int.Parse(details[3]);

                            // parse the window state
                            string windowState = details[4];

                            Size size = new Size(w, h);
                            Point location = new Point(l, t);
                            Point locationBottomRight = new Point(l + w, t + h);

                            // Check the location - just in case it is on a screen that is no longer attached
                            foreach (Screen screen in Screen.AllScreens)
                            {
                                // If the whole form is visible on one or more of the available screens we can display it.
                                // Otherwise we let the OS show the form
                                if (screen.Bounds.Contains(locationBottomRight))
                                {
                                    Location = location;
                                    Size = size;
                                    break;
                                }
                            }

                            if ((windowState == "Maximized"))
                            {
                                WindowState = FormWindowState.Maximized;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TLogging.Log(String.Format("Exception occurred while loading the window position file '{0}': {1}",
                            settingsPath,
                            ex.Message), TLoggingType.ToLogfile);
                }
            }
        }

        #endregion
    }
}
