/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       petrih
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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using EWSoftware.StatusBarText;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.Interfaces.AsynchronousExecution;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonDialogs;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Applink4GL;
using Ict.Petra.Client.MCommon.Applink;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MPartner
{
    /// UserControl for editing Partner Extracts (List/Detail view).
    public partial class TExtractSubscriptionAddWinForm : TFrmPetraEdit
    {
        public const String StrInvalidDataNotCorrected = "Cannot end editing because invalid data has not been corrected!";

        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Clean up any resources being used. </summary>
        protected new PartnerEditTDS FMainDS;
        protected TUCPartnerSubscriptionsLogic FLogic;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private Boolean FClosingForbidden;
        private TProgressDialogForm FFrmPRDLG;
        private Boolean FKeepUpProgressCheck;
        private string FAddPublicationCode;
        private Int32 FExtractID;
        private String FExtractName;
        private Int32 FExtractCount;

        /// <summary>Sets the parameters ExtractId and ExtractNameprocedure SetParameters(ExtractID: Int32; ExtractName: String); Reads and writes FMainDS  This should not be used anymore!</summary>
        public PartnerEditTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }

        public TExtractSubscriptionAddWinForm() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            GetPartnerEditUIConnector();
            FMainDS = new PartnerEditTDS();
            FLogic = new TUCPartnerSubscriptionsLogic();

            /*
             * This Form doesn't care about data change Events (it can always be closed,
             * even if data was entered because it's a Dialogue); if data change Events
             * were enabled, they would interfere with the closing of this Form once the
             * user has entered data and wants to close out of the Form by using the 'x'
             * Button or ALT+F4 key combination!
             */
            DisableDataChangedEvent();
        }

        private void TExtractSubscriptionAddWinForm_Load(System.Object sender, System.EventArgs e)
        {
            TAppLink.GAppLink.RequestFocus(this);
        }

        private void GetPartnerEditUIConnector()
        {
            FPartnerEditUIConnector = TRemote.MPartner.Partner.UIConnectors.PartnerEdit();

            // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
            TEnsureKeepAlive.Register(FPartnerEditUIConnector);
        }

        // cancel all actions from UcoDetails. This NEEDS to be done, or a new Row is added to a temp Partner.

        /// <summary>
        /// Disposes (Closes the screen)
        /// </summary>
        /// <returns>void</returns>
        private void ActionCancel()
        {
            ucoDetails.CancelEditing(true, true);
        }

        // Here should be call to PetraHelp.

        /// <summary>
        /// Show help for the screen.
        /// </summary>
        /// <returns>void</returns>
        private void ActionHelp()
        {
            TCmdMCommon.OpenScreenAboutPetra(this);
            TCmdMCommon.OpenHelp(this);
        }

        /// <summary>
        /// Private Declarations  Checks Subscriptionvalues, Shows a list of partners that already have the subsciprion, adds the subscription to those that don't yet have it.
        /// </summary>
        /// <returns>void</returns>
        private void ActionOK2()
        {
            String ErrorMessages;
            Control FirstErrorControl;
            PSubscriptionRow Row;
            PSubscriptionRow Rowtmp = null;
            PSubscriptionTable Table;
            TDataModeEnum Mode;

            try
            {
                FClosingForbidden = true;
                Table = new PSubscriptionTable();
                Row = Table.NewRowTyped();

                if (!FVerificationResultCollection.contains(ucoDetails))
                {
                    // Data Validation OK
                    // here is checked that data is logically in correct order. Example:
                    // if Subscriptionstatus is 'GIFT' there must be also a Partner that gives the gift
                    if (ucoDetails.CheckCorrectnessOfValues())
                    {
                        // ucoDetails.CancelEditing(true, true);
                        try
                        {
                            Rowtmp = ucoDetails.GetCurrentSubscriptionRow();
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.PublicationCode = Rowtmp.PublicationCode;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.PartnerKey = Rowtmp.PartnerKey;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.PublicationCopies = Rowtmp.PublicationCopies;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.ReasonSubsGivenCode = Rowtmp.ReasonSubsGivenCode;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.ReasonSubsCancelledCode = Row.ReasonSubsCancelledCode;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.ExpiryDate = Rowtmp.ExpiryDate;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.GratisSubscription = Row.GratisSubscription;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.DateNoticeSent = Rowtmp.DateNoticeSent;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.DateCancelled = Rowtmp.DateCancelled;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.StartDate = Rowtmp.StartDate;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.NumberIssuesReceived = Rowtmp.NumberIssuesReceived;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.NumberComplimentary = Row.NumberComplimentary;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.SubscriptionRenewalDate = Rowtmp.SubscriptionRenewalDate;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.SubscriptionStatus = Rowtmp.SubscriptionStatus;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.FirstIssue = Row.FirstIssue;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.LastIssue = Rowtmp.LastIssue;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.GiftFromKey = Row.GiftFromKey;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.DateCreated = Rowtmp.DateCreated;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.CreatedBy = Row.CreatedBy;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.DateModified = Rowtmp.DateModified;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.ModifiedBy = Row.ModifiedBy;
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            Row.ModificationId = Rowtmp.ModificationId;
                        }
                        catch (Exception)
                        {
                        }
                        Table.Rows.Add(Row);
                        FAddPublicationCode = Row.PublicationCode;

                        // Disable UI
                        Mode = TDataModeEnum.dmBrowse;
                        ucoDetails.SetMode(Mode);
                        pnlBtnOKCancelHelpLayout.Enabled = false;
                        stbMain.Panels[stbMain.Panels.IndexOf(stpInfo)].Text = "Please wait...";
                        this.Cursor = Cursors.WaitCursor;
                        FClosingForbidden = true;

                        //
                        // Add Subscriptions (asynchronously!)
                        //
                        FLogic.AddSubscriptions(FExtractID, Table);
                        ShowProgressDialog();
                    }
                }
                else
                {
                    // Data Validation failed
                    FVerificationResultCollection.BuildScreenVerificationResultList(ucoDetails, out ErrorMessages, out FirstErrorControl);

                    // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                    MessageBox.Show(StrInvalidDataNotCorrected + "" + Environment.NewLine + Environment.NewLine + ErrorMessages,
                        "Record contains invalid data!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    FirstErrorControl.Focus();
                    return;
                }
            }
            catch (Exception)
            {
                FClosingForbidden = false;
                pnlBtnOKCancelHelpLayout.Enabled = true;
                this.Cursor = Cursors.Default;
                throw;
            }
        }

        private void AddSubscriptionsFinished(System.Object AException)
        {
            TDataModeEnum Mode;
            DataTable AlreadySubscribedPartnersDT;
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult SubmitChangesResult;
            TExtractPartnersSubscribedWinForm PartnersSubscribingDialog;
            TMyUpdateDelegate2 MyUpdateDelegate;

            object[] Args;

            // Since this procedure is called from a separate (background) Thread, it is
            // necessary to execute this procedure in the Thread of the GUI
            if (btnOK.InvokeRequired)
            {
                // messagebox.show('btnOK.InvokeRequired: yes');
                Args = new object[1];
                try
                {
                    MyUpdateDelegate = new TMyUpdateDelegate2(AddSubscriptionsFinished);
                    Args[0] = AException;
                    btnOK.Invoke((System.Delegate)MyUpdateDelegate, Args);

                    // messagebox.show('Invoke finished!');
                }
                finally
                {
                    Args = new object[0];
                }

                // messagebox.show('Invoke finished!');
            }
            else
            {
                try
                {
                    FFrmPRDLG.Close();
                    FFrmPRDLG.Dispose();

                    // Update UI
                    Mode = TDataModeEnum.dmEdit;
                    ucoDetails.SetMode(Mode);
                    ucoDetails.SetScreenPartsForNewSubscription();
                    ucoDetails.Focus();
                    FClosingForbidden = false;
                    pnlBtnOKCancelHelpLayout.Enabled = true;
                    this.Cursor = Cursors.Default;

                    if (AException == null)
                    {
                        // Retrieve Partners that were already subscribed
                        FLogic.GetSubscriptionAddingResults(
                            out AlreadySubscribedPartnersDT,
                            out VerificationResult,
                            out SubmitChangesResult);

                        if (SubmitChangesResult == TSubmitChangesResult.scrOK)
                        {
                            // Clear any errors that might have been set (these don't matter, for the changes have been cancelled)
                            FVerificationResultCollection.Remove(ucoDetails);

                            if (AlreadySubscribedPartnersDT.Rows.Count > 0)
                            {
                                MessageBox.Show("The Subscription was added to " +
                                    Convert.ToString(
                                        FExtractCount -
                                        AlreadySubscribedPartnersDT.Rows.Count) + " Partners out of " + FExtractCount.ToString() +
                                    " Partners in the Extract." + "\r\n" + "See the following Dialog for the " +
                                    AlreadySubscribedPartnersDT.Rows.Count.ToString() +
                                    " Partners that were already subscribed to this Subscription. The Subscription was not added those Partners!",
                                    "Finished Adding Subscriptions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                PartnersSubscribingDialog = new TExtractPartnersSubscribedWinForm();
                                PartnersSubscribingDialog.ShowPartners(AlreadySubscribedPartnersDT,
                                    FAddPublicationCode,
                                    FLogic.GetDescriptionForPublication(FAddPublicationCode));
                                PartnersSubscribingDialog.ShowDialog();
                                PartnersSubscribingDialog.Dispose();
                            }
                            else
                            {
                                MessageBox.Show(
                                    "The Subscription was added to all " + FExtractCount.ToString() + " Partners in the Extract.",
                                    "Finished Adding Subscriptions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            stbMain.Panels[stbMain.Panels.IndexOf(stpInfo)].Text = "Ready.";
                        }
                        else
                        {
                            MessageBox.Show(Messages.BuildMessageFromVerificationResult(null, VerificationResult));
                            stbMain.Panels[stbMain.Panels.IndexOf(stpInfo)].Text = "Subscription could not be added to Partners!";
                        }
                    }
                    else
                    {
                        stbMain.Panels[stbMain.Panels.IndexOf(stpInfo)].Text = "Subscription could not be added to Partners!";
                        throw (Exception)AException;
                    }
                }
                catch (Exception Exp)
                {
                    // Cannot simply reraise the Exception because it would be caught by
                    // the Exception Handler in AsyncProgressCheckThread. Therefore just
                    // show the Exception in a MessageBox.
                    MessageBox.Show("Exception occured in asynchronous processing:" + "\r\n" + Exp.ToString());
                }
            }
        }

        private void BtnCancel_Click(System.Object sender, System.EventArgs e)
        {
            this.ActionCancel();
        }

        private void BtnHelp_Click(System.Object sender, System.EventArgs e)
        {
            ActionHelp();
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            this.ActionOK2();
        }

        /// <summary>
        /// Initializes the UseControl
        /// </summary>
        /// <returns>void</returns>
        public void InitializeUserControl()
        {
            /// FDataModeEnum : TDataModeEnum;
            double PublicationCost;
            String PublicationCostCurrencyCode;
            String BestPublicationPublicationCode;

            this.Menu = null;
            FLogic.MultiTableDS = FMainDS;
            FLogic.LoadPublications();
            FLogic.LoadDataOnDemand();
            FLogic.CreateTempSubscriptionsTable();
            FLogic.FillTempSubscriptionsTable();
            BestPublicationPublicationCode = "MONTHLY";
            ucoDetails.VerificationResultCollection = FVerificationResultCollection;

            // ucoDetails.StatusBarTextProvider.InstanceStatusBar := sbtUserControl.InstanceStatusBar;
            ucoDetails.StatusBarTextProvider.InstanceStatusBar = this.sbtForm.InstanceStatusBar;
            sbtForm = ucoDetails.StatusBarTextProvider;             /// needed in the case where the TabPage is selected during PartnerEdit form_load procedureMessageBox.Show('TabSetupPartnerAddresses finished');

            // Here is created a temp Partners that will never be saved.
            FLogic.CreateNewSubscriptionRow(12345678, "INDEED");

            // create databindings.
            ucoDetails.PerformDataBinding(FMainDS, BestPublicationPublicationCode);
            ucoDetails.PublicationCode = "INDEED";
            FLogic.PublicationCode = "INDEED";

            // Set the buttons.
            // Set screenparts to default for new Subscription
            ucoDetails.SetScreenPartsForNewSubscription();
            FLogic.DeterminePublicationCost(out PublicationCost, out PublicationCostCurrencyCode);
            ucoDetails.SetupPublicationCost(PublicationCost, PublicationCostCurrencyCode);
            this.lblExtractID.Text = FExtractID.ToString();
            this.lblExtractName.Text = FExtractName;
        }

        /// <summary>
        /// Set the paramters.
        /// </summary>
        /// <returns>void</returns>
        public void SetParameters(Int32 extractID, String extractName, Int32 extractCount)
        {
            // Set the parameters,
            FExtractID = extractID;
            FExtractName = extractName;
            FExtractCount = extractCount;
        }

        private void ShowProgressDialog()
        {
            Thread ProgressCheckThread;

            FFrmPRDLG = new TProgressDialogForm();
            FFrmPRDLG.DialogCaption = "Working...";
            FFrmPRDLG.CancelButtonVisible = false;
            FFrmPRDLG.PercentMinimumLabel = "0%";
            FFrmPRDLG.PercentMaximumLabel = "100%";
            FFrmPRDLG.ProgressInfo = "Preparing...";
            FFrmPRDLG.SubPercentMinimumLabel = "";
            FFrmPRDLG.SubPercentMaximumLabel = "";
            FFrmPRDLG.SubProcessDescription = "";
            FKeepUpProgressCheck = true;
            ProgressCheckThread = new Thread(@AsyncProgressCheckThread);
            ProgressCheckThread.Start();
            FFrmPRDLG.ShowDialog();
        }

        private void AsyncProgressCheckThread()
        {
            IAsynchronousExecutionProgress AsyncExecProgress;
            TAsyncExecProgressState ProgressState;
            Int16 ProgressPercentage;
            String ProgressInformation;

            while (FKeepUpProgressCheck)
            {
                try
                {
                    // Note: If this function is called once every second, approximately 2.7 kb/sec are transferred over the network
                    AsyncExecProgress = FLogic.AsyncExecProgress;

                    if (AsyncExecProgress != null)
                    {
                        AsyncExecProgress.ProgressCombinedInfo(out ProgressState, out ProgressPercentage, out ProgressInformation);

                        switch (ProgressState)
                        {
                            case TAsyncExecProgressState.Aeps_Finished:
                                FKeepUpProgressCheck = false;

                                // MessageBox.Show('Asynchronous Progress finished!');
                                AddSubscriptionsFinished(null);
                                break;

                            case TAsyncExecProgressState.Aeps_Stopping:

                                if (FFrmPRDLG.SubProgressAvailable)
                                {
                                    FFrmPRDLG.SubProgressInfo = "Stopping...";
                                }
                                else
                                {
                                    FFrmPRDLG.ProgressInfo = "Stopping...";
                                }

                                break;

                            case TAsyncExecProgressState.Aeps_Stopped:
                                FKeepUpProgressCheck = false;

                                if (FFrmPRDLG.SubProgressAvailable)
                                {
                                    FFrmPRDLG.SubProgressInfo = "Stopped!";
                                }
                                else
                                {
                                    FFrmPRDLG.ProgressInfo = "Stopped!";
                                }

                                // MessageBox.Show('Asynchronous Progress stopped!');
                                AddSubscriptionsFinished(null);
                                break;

                            default:

                                if (FFrmPRDLG.SubProgressAvailable)
                                {
                                    FFrmPRDLG.PercentFinished = (ProgressPercentage / 2);
                                    FFrmPRDLG.ProgressInfo = "Main Process";
                                    FFrmPRDLG.SubPercentFinished = ProgressPercentage;
                                    FFrmPRDLG.SubProgressInfo = "Sub-Process " + ProgressInformation;
                                }
                                else
                                {
                                    FFrmPRDLG.PercentFinished = ProgressPercentage;
                                    FFrmPRDLG.ProgressInfo = ProgressInformation;
                                }

                                break;
                        }
                    }
                }
                catch (Exception Exp)
                {
                    // MessageBox.Show('Exception caught in asynchronous process!');
                    FKeepUpProgressCheck = false;

                    if (btnOK.InvokeRequired)
                    {
                        AddSubscriptionsFinished(Exp);
                    }
                    else
                    {
                        throw;

                        // MessageBox.Show('Exception occured in asynchonrous processing:' + "\r\n" + Exp.ToString);
                    }
                }

                // Sleep for some time. After that, this function is called again automatically.
                Thread.Sleep(1000);
            }
        }

        private void TExtractSubscriptionAddWinForm_Closing(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!FClosingForbidden)
            {
                if (FPartnerEditUIConnector != null)
                {
                    // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
                    TEnsureKeepAlive.UnRegister(FPartnerEditUIConnector);
                }

                // Need to call the following method in the Base Form to remove this Form from the Open Forms List
                TFrmPetra_Closing(this, null);
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }

    public delegate void TMyUpdateDelegate2(System.Object msg);

    public class ExtractSubscriptionAdd
    {
        #region Called by 4GL
        public static TCommand RunExtractSubscriptionAddScreen(TCommand cmd)
        {
            TCommand ReturnValue;

            System.Int32 ExtractID;
            String ExtractName;
            System.Int32 ExtractCount;
            TExtractSubscriptionAddWinForm frmES;
            frmES = null;             /// just to prevent compiler warning...

            if (cmd.commandName == "run_extract_subscription_add_screen")
            {
                // Set the temp parameters.
                ExtractID = (Int32)cmd.GetIntegerParam(0);
                ExtractName = cmd.GetStringParam(1);
                ExtractCount = (Int32)cmd.GetIntegerParam(2);

                // Create "this" screen.
                frmES = new TExtractSubscriptionAddWinForm();

                // set the actual parameters.
                frmES.SetParameters(ExtractID, ExtractName, ExtractCount);
                frmES.InitializeUserControl();

                // this doesn't return
                frmES.ShowDialog();

                // If everything went well, send information of that back (SUCCESS) to Progress.
            }

            ReturnValue = new TCommand();
            ReturnValue.commandType = TCommandType.ctResult;
            ReturnValue.commandName = "SUCCESS";
            ReturnValue.id = cmd.id;
            TAppLink.GAppLink.SendCommand(ReturnValue, null);

            if (frmES != null)
            {
                if (frmES.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    // 'Unfreeze' 4GL by changing 4GL Hourglass Cursor to normal Cursor
                    TAppLink.GAppLink.UnfreezeAndNormalCursor();
                }

                frmES.Dispose();
            }

            return ReturnValue;
        }

        #endregion
    }
}