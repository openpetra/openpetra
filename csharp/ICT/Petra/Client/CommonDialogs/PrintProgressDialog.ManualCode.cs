//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

using Ict.Common;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// This class is used to send multiple documents into a specified printer queue asynchronously.
    /// It displayes a dialog window which shows progress and allows the insertion of documents
    /// into the printer queue to be paused or even aborted.
    /// </summary>
    public partial class TFrmPrintProgressDialog
    {
        /// <summary>
        /// An object that holds the arguments passed to the background thread
        /// </summary>
        private class BackgroundWorkerArgument
        {
            /// <summary>
            /// The name of the printer. Enclose in quotes if it includes a space.
            /// </summary>
            public string PrinterName
            {
                get; private set;
            }

            /// <summary>
            /// The full path to the folder containing the files to print
            /// </summary>
            public string FolderPath
            {
                get; private set;
            }

            /// <summary>
            /// The files to print
            /// </summary>
            public string[] Files
            {
                get; private set;
            }

            /// <summary>
            /// Simple constructor to set all the arguments
            /// </summary>
            /// <param name="APrinterName">The name of the printer. Enclose in quotes if it includes a space.</param>
            /// <param name="AFolderPath">The full path to the folder containing the files to print</param>
            /// <param name="AFiles">The files to print</param>
            public BackgroundWorkerArgument(string APrinterName, string AFolderPath, string[] AFiles)
            {
                PrinterName = APrinterName;
                FolderPath = AFolderPath;
                Files = AFiles;
            }
        }

        private ProgressBar FProgressBar = new ProgressBar();

        private IPrintProgress FCallerForm = null;
        private bool FIsPaused = false;
        private object FPauseObject = new object();

        private BackgroundWorker FBackgroundWorker = new BackgroundWorker();

        private void InitializeManualCode()
        {
            // Position the progress bar and add it to the form controls
            FProgressBar.Location = new Point(lblStatus.Left, 50);
            FProgressBar.Size = new Size(this.ClientSize.Width - (2 * lblStatus.Left), 20);
            FProgressBar.Minimum = 0;
            FProgressBar.Maximum = 100;
            pnlProgress.Controls.Add(FProgressBar);

            // Set these dialog properties
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.ShowInTaskbar = true;

            // Position the buttons
            btnPauseOrResume.Left = lblStatus.Left;
            btnAbortPrint.Left = this.ClientSize.Width - btnAbortPrint.Width - btnPauseOrResume.Left;
            btnPauseOrResume.Text = Catalog.GetString("Pause Printing");

            // Add our event handlers
            this.Activated += TFrmPrintProgressDialog_Activated;

            FBackgroundWorker.DoWork += FBackgroundWorker_DoWork;
            FBackgroundWorker.ProgressChanged += FBackgroundWorker_ProgressChanged;
            FBackgroundWorker.RunWorkerCompleted += FBackgroundWorker_RunWorkerCompleted;
            FBackgroundWorker.WorkerReportsProgress = true;
            FBackgroundWorker.WorkerSupportsCancellation = true;

            // Properties for the status label
            lblStatus.AutoSize = false;
            lblStatus.Width = this.ClientSize.Width - lblStatus.Left;
        }

        private void TFrmPrintProgressDialog_Activated(object sender, EventArgs e)
        {
            // If this causes an exception make sure that you called the Show method for this dialog with the Owner parameter set to the caller form
            this.Location = new Point(this.Owner.Left + 50, this.Owner.Top + 100);
        }

        #region Button Click handlers

        private void btnPauseOrResume_Click(object sender, EventArgs e)
        {
            if (FIsPaused)
            {
                // Release the lock
                System.Threading.Monitor.Exit(FPauseObject);
                FIsPaused = false;
                lblStatus.Text = Catalog.GetString("Status:");
            }
            else
            {
                // Try and obtain a lock on the print queuing code.
                // If we succeed we will pause printing
                System.Threading.Monitor.TryEnter(FPauseObject, 5000, ref FIsPaused);

                if (FIsPaused)
                {
                    lblStatus.Text = Catalog.GetString("Status: Printing has been paused");
                }
            }

            btnPauseOrResume.Text = FIsPaused ? Catalog.GetString("Resume Printing") : Catalog.GetString("Pause Printing");
            btnAbortPrint.Enabled = !FIsPaused;
        }

        private void btnAbortPrint_Click(object sender, EventArgs e)
        {
            if (FBackgroundWorker.IsBusy)
            {
                FBackgroundWorker.CancelAsync();
            }

            lblStatus.Text = Catalog.GetString("Status: Aborting ... please wait ...");
        }

        #endregion

        #region Background Worker Events

        private void FBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Tell the progress dialog
            FProgressBar.Value = e.ProgressPercentage;

            if (e.UserState != null)
            {
                string status = e.UserState.ToString();

                if (!status.StartsWith(Catalog.GetString("Status: ")))
                {
                    status = Catalog.GetString("Status: ") + status;
                }

                lblStatus.Text = status;
                lblStatus.Refresh();
            }

            FProgressBar.Refresh();
        }

        private void FBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorkerArgument args = (BackgroundWorkerArgument)e.Argument;
            int countFiles = args.Files.GetLength(0);
            int countDone = 0;

            // Get the print delay from client config.  The default is 2000 ms.
            // A value can be specified for a specific printer or as a general value
            int printDelay = TAppSettingsManager.GetInt32(string.Format("{0}.PrintQueueDelay", args.PrinterName), 0);

            if (printDelay <= 0)
            {
                // There was no printer-specific delay, so check the blanket value.  Default is 2000 ms.
                printDelay = TAppSettingsManager.GetInt32("PrintQueueDelay", 2000);
            }

            foreach (String FileName in args.Files)
            {
                if (FBackgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    FBackgroundWorker.ReportProgress(0, Catalog.GetString("Printing aborted"));
                    return;
                }

                // Get a lock on this (empty) piece of code.  If it is already locked we will pause here until the lock is released (above)
                lock (FPauseObject)
                {
                }

                // The lock is released now (or never held)
                TTemplaterAccess.RunPrintJob(args.PrinterName, Path.Combine(args.FolderPath, FileName));
                countDone++;
                FBackgroundWorker.ReportProgress(
                    (countDone * 100) / countFiles,
                    string.Format(Catalog.GetString("Printing {0} of {1}"), countDone, countFiles));

                // This sleep is important for two reasons...
                // 1. We need to allow some time for the pause button click event to obtain the lock before we print the next document
                // 2. The RunPrintJob method waits until the process that places the document into the print queue completes BUT ...
                //    Some print applications like Word or PDFCreator need a little more time to close down and close open dialogs.
                //    If we do not allow enough time something like PDFCreator may merge two documents into one folder or Word may leave a dialog showing
                //    for the current document.
                //    Two seconds is enough.  Less may be possible.  But 2 secs is probably ok unless the printer is VERY fast and can print documents
                //    faster than we put them into the print queue.
                System.Threading.Thread.Sleep(printDelay);
            }

            FBackgroundWorker.ReportProgress(100, Catalog.GetString("Printing complete"));
        }

        private void FBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Just tell the caller.  It is up to the caller to close this dialog.
            if (!((Form)FCallerForm).IsDisposed)
            {
                FCallerForm.AsyncPrintingCompleted(e);
            }
        }

        #endregion

        /// <summary>
        /// The main method call to print an array of files in a specified folder
        /// </summary>
        /// <param name="ACaller">The calling screen.  This must implement the IPrintProgress interface</param>
        /// <param name="APrinterName">Name of the printer to print to</param>
        /// <param name="AFolderPath">Full path to the folder containing the files</param>
        /// <param name="AFiles">The files to print</param>
        public void StartPrintingAsync(IPrintProgress ACaller, string APrinterName, string AFolderPath, string[] AFiles)
        {
            FCallerForm = ACaller;
            BackgroundWorkerArgument args = new BackgroundWorkerArgument(APrinterName, AFolderPath, AFiles);
            FBackgroundWorker.RunWorkerAsync(args);
        }

        /// <summary>
        /// The main method call to print all files in a specified folder.  The method displays a Printer Dialog
        /// </summary>
        /// <param name="ACaller">The calling screen.  This must implement the IPrintProgress interface</param>
        /// <param name="AFolderPath">Full path to the folder containing the files</param>
        public void StartPrintingAsync(IPrintProgress ACaller, string AFolderPath)
        {
            FCallerForm = ACaller;

            string printerName = null;

            using (PrintDialog pd = new PrintDialog())
            {
                if (pd.ShowDialog() != DialogResult.OK)
                {
                    RunWorkerCompletedEventArgs completedArgs = new RunWorkerCompletedEventArgs(null, null, true);
                    ACaller.AsyncPrintingCompleted(completedArgs);
                    return;
                }

                // Remember this for use lower down
                printerName = String.Format("\"{0}\"", pd.PrinterSettings.PrinterName);
            }

            // Get all the files in the specified folder
            string[] allFiles = Directory.GetFiles(AFolderPath);

            // Create a sorted list of the files (they will be named in sequence)
            SortedList <string, string>sortedFiles = new SortedList <string, string>();

            foreach (string fullPath in allFiles)
            {
                string fileName = Path.GetFileName(fullPath);
                sortedFiles.Add(fileName.Substring(0, 4), fileName);
            }

            // Now create a simple list from the sorted list
            List <string>fileList = new List <string>();

            foreach (KeyValuePair <string, string>kvp in sortedFiles)
            {
                fileList.Add(kvp.Value);
            }

            // And call our async method
            BackgroundWorkerArgument args = new BackgroundWorkerArgument(printerName, AFolderPath, fileList.ToArray());
            FBackgroundWorker.RunWorkerAsync(args);
        }
    }

    /// <summary>
    /// Interface for the callbacks from this class
    /// </summary>
    public interface IPrintProgress
    {
        /// <summary>
        /// called when the printing has finished
        /// </summary>
        /// <param name="e"></param>
        void AsyncPrintingCompleted(RunWorkerCompletedEventArgs e);
    }
}