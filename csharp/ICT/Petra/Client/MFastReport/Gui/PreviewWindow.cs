//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       moray
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
using FastReport.Export;
using FastReport.Utils;
using Ict.Common;
using Ict.Common.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Petra.Client.MFastReport.Gui
{
    /// <summary>
    /// Custom FastReport Preview Window.
    /// </summary>
    public partial class TFrmPreviewWindow : Form
    {
        /// <summary>
        /// The FastReport Preview control itself. Set the report Preview property to this form's PreviewControl before showing the report.
        /// </summary>
        public FastReport.Preview.PreviewControl PreviewControl {
            get; private set;
        }

        /// <summary>
        /// Construct the form, add the FastReport PreviewControl, initialize the button images from FastReport resources,
        /// and populate the Save menu with the export types we want from those built into FastReport.
        /// </summary>
        public TFrmPreviewWindow()
        {
            InitializeComponent();

            this.SuspendLayout();

            this.toolStrip1.Renderer = new ToolStripProfessionalRenderer(new TOpenPetraMenuColours());

            PreviewControl = new FastReport.Preview.PreviewControl();
            PreviewControl.ToolbarVisible = false;
            PreviewControl.Dock = DockStyle.Fill;
            PreviewControl.PageChanged += new System.EventHandler(PreviewControl_PageChanged);
            Controls.Add(PreviewControl);

            tbbPrint.Image = Res.GetImage(88);
            tbbSave.Image = Res.GetImage(2);
            tbbEmail.Image = Res.GetImage(200);
            tbbFind.Image = Res.GetImage(181);
            tbbFirst.Image = Res.GetImage(185);
            tbbPrev.Image = Res.GetImage(186);
            tbbNext.Image = Res.GetImage(187);
            tbbLast.Image = Res.GetImage(188);

            //Standard ExportBase types registered with FastReport:
            //  PDFExport: 'Export,Pdf,File' (type FastReport.Export.Pdf.PDFExport)
            //  RTFExport: 'Export,RichText,File' (type FastReport.Export.RichText.RTFExport)
            //  HTMLExport: 'Export,Html,File' (type FastReport.Export.Html.HTMLExport)
            //  MHTExport: 'Export,Mht,File' (type FastReport.Export.Mht.MHTExport)
            //  XMLExport: 'Export,Xml,File' (type FastReport.Export.Xml.XMLExport)
            //  Excel2007Export: 'Export,Xlsx,File' (type FastReport.Export.OoXML.Excel2007Export)
            //  Word2007Export: 'Export,Docx,File' (type FastReport.Export.OoXML.Word2007Export)
            //  PowerPoint2007Export: 'Export,Pptx,File' (type FastReport.Export.OoXML.PowerPoint2007Export)
            //  ODSExport: 'Export,Ods,File' (type FastReport.Export.Odf.ODSExport)
            //  ODTExport: 'Export,Odt,File' (type FastReport.Export.Odf.ODTExport)
            //  XPSExport: 'Export,Xps,File' (type FastReport.Export.OoXML.XPSExport)
            //  CSVExport: 'Export,Csv,File' (type FastReport.Export.Csv.CSVExport)
            //  DBFExport: 'Export,Dbf,File' (type FastReport.Export.Dbf.DBFExport)
            //  TextExport: 'Export,Text,File' (type FastReport.Export.Text.TextExport)
            //  ImageExport: 'Export,Image,File' (type FastReport.Export.Image.ImageExport)
            //  XAMLExport: 'Export,Xaml,File'(type FastReport.Export.XAML.XAMLExport)
            //  SVGExport: 'Export,Svg,File' (type FastReport.Export.Svg.SVGExport)

            //Standard CloudStorageClient types registered with FastReport
            //  FtpStorageClient: 'Cloud,Ftp,Name' (type FastReport.Cloud.StorageClient.Ftp.FtpStorageClient)
            //  BoxStorageClient: 'Cloud,Box,Name' (type FastReport.Cloud.StorageClient.Box.BoxStorageClient)
            //  DropboxStorageClient: 'Cloud,Dropbox,Name' (type FastReport.Cloud.StorageClient.Dropbox.DropboxStorageClient)
            //  GoogleDriveStorageClient: 'Cloud,GoogleDrive,Name' (type FastReport.Cloud.StorageClient.GoogleDrive.GoogleDriveStorageClient)
            //  SkyDriveStorageClient: 'Cloud,SkyDrive,Name' (type FastReport.Cloud.StorageClient.SkyDrive.SkyDriveStorageClient)

            // Add the types we want to the Save menu
            foreach (var Exporter in new Type[] { typeof(FastReport.Export.Pdf.PDFExport),
                                                  typeof(FastReport.Export.Csv.CSVExport),
                                                  typeof(FastReport.Export.OoXML.Excel2007Export),
                                                  typeof(FastReport.Export.OoXML.Word2007Export),
                                                  typeof(FastReport.Export.Html.HTMLExport),
                                                  typeof(FastReport.Export.Text.TextExport) })
            {
                var ExportObject = RegisteredObjects.FindObject(Exporter);

                if (ExportObject == null)
                {
                    continue;
                }

                var MenuItem = new ToolStripMenuItem(Res.TryGet(ExportObject.Text) + "...");

                if (ExportObject.ImageIndex != -1)
                {
                    MenuItem.Image = Res.GetImage(ExportObject.ImageIndex);
                }

                MenuItem.Click += new EventHandler(SaveItem_Click);
                MenuItem.Tag = ExportObject;
                tbbSave.DropDownItems.Add(MenuItem);
            }

            this.ResumeLayout();
        }

        /// <summary>
        /// Override command key processing to allow Ctrl-M for Email
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.M | Keys.Control))
            {
                tbbEmail_Click(this, new EventArgs());
                return true;
            }

            if (keyData == (Keys.S | Keys.Control))
            {
                tbbSave.ShowDropDown();
                return true;
            }

            if (keyData == (Keys.Escape))
            {
                Close();
                return true;
            }

            return false;
        }

        private void tbbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PreviewControl_PageChanged(object sender, EventArgs e)
        {
            tbtPageNo.Text = PreviewControl.PageNo.ToString();
        }

        private void tbbPrint_Click(object sender, EventArgs e)
        {
            PreviewControl.Print();
        }

        private void TFrmPreviewWindow_Shown(object sender, EventArgs e)
        {
            this.Text = String.Format("{0}: {1}", Catalog.GetString("Report Preview"), PreviewControl.Report.ReportInfo.Name);

            tblOfPages.Text = "of " + PreviewControl.PageCount.ToString();
            PreviewControl.Focus();
        }

        private void tbbFind_Click(object sender, EventArgs e)
        {
            PreviewControl.Find();
        }

        private void tbbFirst_Click(object sender, EventArgs e)
        {
            PreviewControl.First();
        }

        private void tbbPrev_Click(object sender, EventArgs e)
        {
            PreviewControl.Prior();
        }

        private void tbtPageNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                PreviewControl.PageNo = int.Parse(tbtPageNo.Text);
            }
            else if (e.KeyData == (Keys.A | Keys.Control))
            {
                tbtPageNo.SelectAll();
            }
            else if (!(((e.KeyData >= Keys.D0) && (e.KeyData <= Keys.D9)) || ((e.KeyCode >= Keys.NumPad0) && (e.KeyCode <= Keys.NumPad9))
                       || ((e.KeyData == Keys.Back) || (e.KeyData == Keys.Delete) || (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right)
                           || (e.KeyCode == Keys.Home) || (e.KeyCode == Keys.End))))
            {
                // Only allowing digits and editing keys is much more complex than it should be!
                e.SuppressKeyPress = true;
            }
        }

        private void tbbNext_Click(object sender, EventArgs e)
        {
            PreviewControl.Next();
        }

        private void tbbLast_Click(object sender, EventArgs e)
        {
            PreviewControl.Last();
        }

        private void SaveItem_Click(object sender, EventArgs e)
        {
            var ExportObject = ((sender as ToolStripMenuItem).Tag as ObjectInfo).Object;
            ExportBase export = Activator.CreateInstance(ExportObject) as ExportBase;

            export.CurPage = PreviewControl.PageNo;
            export.Export(PreviewControl.Report);
        }

        private void tbbEmail_Click(object sender, EventArgs e)
        {
            try
            {
                using (var EmailDialog = new TFrmEmailDialog(this))
                {
                    EmailDialog.ReportInstance = PreviewControl.Report;
                    EmailDialog.ShowDialog();
                }
            }
            catch
            {
                // Exceptions are handled within TFrmEmailDialog but re-thrown to indicate error condition to the caller.
                // (This looks ugly - is there a better way to do it?)
            }
        }
    }
}