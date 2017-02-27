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
using System.Collections.Specialized;
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
        /// <summary>American number format: decimal point, comma for thousands separator</summary>
        public static string NUMBERFORMAT_AMERICAN = "American";
        /// <summary>European number format: decimal comma, point for thousands separator</summary>
        public static string NUMBERFORMAT_EUROPEAN = "European";

        /// <summary>american date format: month first **when the date is ambiguous**
        /// This does not alter the ability to parse yyyy-MM-dd or dd-MMM-yy successfully</summary>
        public static string DATEFORMAT_MONTH_FIRST = "MDY";
        /// <summary>european date format: day first **when the date is ambiguous**
        /// This does not alter the ability to parse yyyy-MM-dd or MMM-dd-yy successfully</summary>
        public static string DATEFORMAT_DAY_FIRST = "DMY";

        /// <summary>The connected UserID</summary>
        private static string FUserID = null;

        private bool FIsActivatedOnce = false;
        private string FSeparator;
        private bool FFileHasCaption;
        private bool FDateMayBeInteger = false;
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
                DateFormatChanged();
                NumberFormatChanged();
            }
        }

        /// <summary>
        /// read/set the date format that the user has selected
        /// </summary>
        public string DateFormat
        {
            get
            {
                return cmbDateFormat.SelectedIndex == 0 ? DATEFORMAT_MONTH_FIRST : DATEFORMAT_DAY_FIRST;
            }
            set
            {
                // Note: Old Petra used MDY and DMY and it suits us to define our contsants the same way
                cmbDateFormat.SelectedIndex = value.StartsWith("M") ? 0 : 1;
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
        /// Set to true if dates may llok like 6 digit integers.  Default is false.
        /// </summary>
        public bool DateMayBeInteger
        {
            set
            {
                FDateMayBeInteger = value;
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
            this.rbtTabulator.Text = Catalog.GetString("Tab");
            this.rbtOther.Text = Catalog.GetString("Other Separator") + ":";
            this.rbtSemicolon.Text = Catalog.GetString("Semicolon");
            this.btnCancel.Text = Catalog.GetString("Cancel");
            this.btnOK.Text = Catalog.GetString("OK");
            this.lblNumberFormat.Text = Catalog.GetString("Number format") + ":";
            this.lblNumberFormatHint.Text = Catalog.GetString("Number format Hint") + ":";
            this.lblDateFormat.Text = Catalog.GetString("Ambiguous dates") + ":";
            this.lblDateFormatHint.Text = Catalog.GetString("Date format Hint") + ":";
            this.lblTextEncoding.Text = Catalog.GetString("Text encoding") + ":";
            this.lblTextEncodingHint.Text = Catalog.GetString("Hint: Where a choice of encodings exists, a file exported from OpenPetra or Excel");
            this.Text = Catalog.GetString("Select CSV Separator");
            #endregion

            FSeparator = TAppSettingsManager.GetValue("CSVSeparator",
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);

            System.Globalization.CultureInfo myCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            //if (!cmbDateFormat.Items.Contains(regionalDateString))
            //{
            //    cmbDateFormat.Items.Insert(0, regionalDateString);
            //}

            //cmbDateFormat.SelectedIndex = cmbDateFormat.Items.IndexOf(regionalDateString);

            DateFormat = myCulture.DateTimeFormat.ShortDatePattern;
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
            cmbDateFormat.SelectedIndexChanged += cmbDateFormat_SelectedIndexChanged;
            cmbNumberFormat.SelectedIndexChanged += cmbNumberFormat_SelectedIndexChanged;
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

        private void cmbNumberFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumberFormatChanged();
        }

        private void NumberFormatChanged()
        {
            if (cmbNumberFormat.SelectedIndex < 0)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            StringReader sr = new StringReader(FFileContent);
            int rowNumber = 0;
            List <Tuple <string, int>>floatCandidates = new List <Tuple <string, int>>();

            string line = sr.ReadLine();

            while (line != null && rowNumber < 100)
            {
                line = line.Trim();

                if ((line.Length == 0) || line.StartsWith("#") || line.StartsWith("/*"))
                {
                    line = sr.ReadLine();
                    continue;
                }

                rowNumber++;

                // Parse the line
                StringCollection columns = StringHelper.GetCSVList(line, SelectedSeparator);
                bool ? isDotDecimal;

                foreach (string column in columns)
                {
                    if (StringHelper.LooksLikeFloat(column, out isDotDecimal) && isDotDecimal.HasValue)
                    {
                        // The Tuple int matches the ComboBox selected index
                        floatCandidates.Add(new Tuple <string, int>(column, isDotDecimal.Value ? 0 : 1));
                    }
                }

                line = sr.ReadLine();
            }

            // Did we find any float candidates?
            if (floatCandidates.Count > 0)
            {
                int selectedIndex = cmbNumberFormat.SelectedIndex;
                string msg = Catalog.GetString("Warning: ");
                int itemCount = 0;

                foreach (Tuple <string, int>candidate in floatCandidates)
                {
                    if (candidate.Item2 != selectedIndex)
                    {
                        if (itemCount > 0)
                        {
                            msg += ",  ";
                        }

                        msg += string.Format("'{0}'", candidate.Item1);
                        itemCount++;

                        if (itemCount > 5)
                        {
                            break;
                        }
                    }
                }

                if (itemCount > 0)
                {
                    msg = string.Format(Catalog.GetPluralString("{0} looks like a number", "{0} look like numbers", itemCount), msg);
                    msg += Catalog.GetString(" but the selected Number Format does not match.");
                    lblNumberFormatHint.Text = msg;
                    lblNumberFormatHint.BackColor = Color.Yellow;
                    this.Cursor = Cursors.Default;
                    return;
                }
            }

            lblNumberFormatHint.Text = string.Empty;
            lblNumberFormatHint.BackColor = DefaultBackColor;
            this.Cursor = Cursors.Default;
        }

        private void cmbDateFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateFormatChanged();
        }

        private void DateFormatChanged()
        {
            if (cmbDateFormat.SelectedIndex < 0)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            StringReader sr = new StringReader(FFileContent);
            int rowNumber = 0;
            int numDatesFound = 0;
            List <Tuple <string, DateTime>>monthFirstOnlyCandidates = new List <Tuple <string, DateTime>>();
            List <Tuple <string, DateTime>>dayFirstOnlyCandidates = new List <Tuple <string, DateTime>>();
            string badFormatSuffix = Catalog.GetString(" but the selected Date Format does not match.");

            DateTime minMonthFirstDate, maxMonthFirstDate, minDayFirstDate, maxDayFirstDate;
            minMonthFirstDate = minDayFirstDate = DateTime.MaxValue;
            maxMonthFirstDate = maxDayFirstDate = DateTime.MinValue;

            string line = sr.ReadLine();

            while (line != null && rowNumber < 100)
            {
                line = line.Trim();

                if ((line.Length == 0) || line.StartsWith("#") || line.StartsWith("/*"))
                {
                    line = sr.ReadLine();
                    continue;
                }

                rowNumber++;

                // Parse the line
                StringCollection columns = StringHelper.GetCSVList(line, SelectedSeparator);
                DateTime monthFirstDate, dayFirstDate;

                foreach (string column in columns)
                {
                    if (StringHelper.LooksLikeAmbiguousShortDate(column, FDateMayBeInteger, out monthFirstDate, out dayFirstDate))
                    {
                        // We have a date column...
                        numDatesFound++;

                        // Note the information if the Date can only be parsed one way (i.e. ignoring dates like 1/2/16 which parse both ways)
                        if ((monthFirstDate > DateTime.MinValue) && (dayFirstDate == DateTime.MinValue))
                        {
                            monthFirstOnlyCandidates.Add(new Tuple <string, DateTime>(column, monthFirstDate));
                        }

                        if ((dayFirstDate > DateTime.MinValue) && (monthFirstDate == DateTime.MinValue))
                        {
                            dayFirstOnlyCandidates.Add(new Tuple <string, DateTime>(column, dayFirstDate));
                        }

                        // Now work out the min and max dates that we discovered
                        if (monthFirstDate > maxMonthFirstDate)
                        {
                            maxMonthFirstDate = monthFirstDate;
                        }

                        if ((monthFirstDate > DateTime.MinValue) && (monthFirstDate < minMonthFirstDate))
                        {
                            minMonthFirstDate = monthFirstDate;
                        }

                        if (dayFirstDate > maxDayFirstDate)
                        {
                            maxDayFirstDate = dayFirstDate;
                        }

                        if ((dayFirstDate > DateTime.MinValue) && (dayFirstDate < minDayFirstDate))
                        {
                            minDayFirstDate = dayFirstDate;
                        }
                    }
                }

                line = sr.ReadLine();
            }

            // What did we find?
            string badMonthFirstCandidateString = string.Empty;
            string badDayFirstCandidateString = string.Empty;
            List <string>distinctItems = new List <string>();

            // Build a string containing any bad date formats
            if (DateFormat.StartsWith("M"))
            {
                // Current option starts with Month
                int samples = 0;

                foreach (Tuple <string, DateTime>candidate in dayFirstOnlyCandidates)
                {
                    // We add this to our message string if it is not a repeat of a date we already have
                    if (!distinctItems.Contains(candidate.Item1))
                    {
                        distinctItems.Add(candidate.Item1);

                        // This only works as d/M/y or y/M/d
                        if (badMonthFirstCandidateString.Length > 0)
                        {
                            badMonthFirstCandidateString += ",  ";
                        }

                        badMonthFirstCandidateString += string.Format("'{0}'", candidate.Item1);
                        samples++;

                        if (samples > 5)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                // Current option starts with Day or Year
                // Note that this is why it is important in OP that importing dates that start with y use en-GB Culture as a basis.
                // Year parsing uses day-first parsing as a basis
                int samples = 0;

                foreach (Tuple <string, DateTime>candidate in monthFirstOnlyCandidates)
                {
                    // We add this to our message string if it is not a repeat of a date we already have
                    if (!distinctItems.Contains(candidate.Item1))
                    {
                        distinctItems.Add(candidate.Item1);

                        // This only works as M/d/y
                        if (badDayFirstCandidateString.Length > 0)
                        {
                            badDayFirstCandidateString += ",  ";
                        }

                        badDayFirstCandidateString += string.Format("'{0}'", candidate.Item1);
                        samples++;

                        if (samples > 5)
                        {
                            break;
                        }
                    }
                }
            }

            // Work out what the Hint/Warning is going to be (if anything)
            bool showHighlight = false;

            if (badMonthFirstCandidateString.Length > 0)
            {
                lblDateFormatHint.Text = string.Format(Catalog.GetPluralString(
                        "Warning: {0} looks like a date{1}", "Warning: {0} look like dates{1}", distinctItems.Count),
                    badMonthFirstCandidateString, badFormatSuffix);
                showHighlight = true;
            }
            else if (badDayFirstCandidateString.Length > 0)
            {
                lblDateFormatHint.Text = string.Format(Catalog.GetPluralString(
                        "Warning: {0} looks like a date{1}", "Warning: {0} look like dates{1}", distinctItems.Count),
                    badDayFirstCandidateString, badFormatSuffix);
                showHighlight = true;
            }
            else if (DateFormat.StartsWith("M") && (minMonthFirstDate > DateTime.MinValue) && (maxMonthFirstDate > DateTime.MinValue))
            {
                lblDateFormatHint.Text = string.Format(Catalog.GetString(
                        "Hint: The content appears to contain dates ranging from {0:D} to {1:D}"),
                    minMonthFirstDate, maxMonthFirstDate);
            }
            else if ((minDayFirstDate > DateTime.MinValue) && (maxDayFirstDate > DateTime.MinValue))
            {
                lblDateFormatHint.Text = string.Format(Catalog.GetString(
                        "Hint: The content appears to contain dates ranging from {0:D} to {1:D}"),
                    minDayFirstDate, maxDayFirstDate);
            }
            else
            {
                lblDateFormatHint.Text = string.Empty;
            }

            if (showHighlight)
            {
                lblDateFormatHint.BackColor = Color.Yellow;
            }
            else
            {
                lblDateFormatHint.BackColor = DefaultBackColor;
            }

            this.Cursor = Cursors.Default;
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

                while (line != null)
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

            RefreshGrid();
        }

        private void RefreshGrid()
        {
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
                }

                table.DefaultView.AllowNew = false;
                table.DefaultView.AllowDelete = false;
                table.DefaultView.AllowEdit = false;

                grdPreview.SuspendLayout();
                grdPreview.DataSource = table;

                foreach (DataGridViewColumn column in grdPreview.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                grdPreview.ResumeLayout();

                if (FIsActivatedOnce)
                {
                    NumberFormatChanged();
                    DateFormatChanged();
                }
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
