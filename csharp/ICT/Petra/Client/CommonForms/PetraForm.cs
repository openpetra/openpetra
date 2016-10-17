//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank, alanP
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
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Resources;
using System.Threading;
using System.Reflection;
using System.IO;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.Security;
using Ict.Common.Exceptions;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>todoComment</summary>
    public delegate void ActionEventHandler(object sender, ActionEventArgs e);

    /// <summary>
    /// this class provides some useful methods for a Petra form
    /// </summary>
    public class TFrmPetraUtils
    {
        #region Resourcestrings

        /// <summary>This Resourcestring needs to be public becaused it is used elsewhere as well.</summary>
        public static readonly string StrFormCaptionPrefixNew = Catalog.GetString("NEW: ");

        /// <summary>This Resourcestring needs to be public becaused it is used elsewhere as well.</summary>
        public static readonly string StrFormCaptionPrefixReadonly = Catalog.GetString("READ-ONLY: ");

        #endregion

        /// <summary>
        /// Returns a list of screen security restrictions. (Available restrictions are defined by Constants found in Class
        /// <see cref="TSecurityChecks"/>.)
        /// </summary>
        /// <returns>List of screen security restrictions.</returns>
        public delegate List <string>TScreenSecurity();

        /// <summary>
        /// This sets a limit on the cascading reference count when checking for GUI deletion of a row.
        /// </summary>
        private const int FMaxReferenceCountOnDelete = 1000;

        /// <summary>
        /// will set the help text for each control, when it gets the focus
        /// </summary>
        private TExtStatusBarHelp FStatusBar;

        /// <summary>
        /// This is a reference to the WinForm that contains this Petra object
        /// this object implements the IFrmPetra interface
        /// </summary>
        protected IFrmPetra FTheForm;

        /// <summary>What context (e.g. OpenPetra Module) the screen is for (for security purposes).</summary>
        protected String FSecurityScreenContext;

        /// <summary>
        /// The Form's recently set Security Permissions.
        /// </summary>
        protected List <string>FFormsSecurityPermissions = null;

        /// <summary>
        /// ToolTip instance for the Form.
        /// </summary>
        protected System.Windows.Forms.ToolTip FtipForm;

        /// <summary>
        /// ToolTip instance which is used to show Data Validation messages.
        /// </summary>
        protected ToolTip FValidationToolTip;

        /// <summary>
        /// Dictionary that contains Controls on whose data Data Validation should be run.
        /// </summary>
        protected TValidationControlsDict FValidationControlsDict = new TValidationControlsDict();

        /// <summary>
        /// points to the same object as FTheForm, but already casted to a WinForm
        /// </summary>
        protected System.Windows.Forms.Form FWinForm;

        private Form FCallerForm;

        private string FFormTitle = String.Empty;

        /// This helper class handles the extensions to TFrmPetraUtils that deal with opening and closing windows
        /// and loading and saving window sizes, positions and states
        private TFrmPetraWindowExtensions FWindowExtensions;

        /// Tells whether the Form is activated for the first time (after loading the Form) or not
        protected Boolean FFormActivatedForFirstTime;

        /// Set this to true to prevent the automatic hookup of change Events of all Controls on the Form
        protected Boolean FNoAutoHookupOfAllControls;

        /// This holds a reference to ALL controls on the screen  even if they are buried in GroupBoxes, Panels, or TabPages
        protected ArrayList FAllControls;

        /// This holds a reference to ALL controls on the screen that have child controls, even if they are buried in GroupBoxes, Panels, or TabPages
        protected ArrayList FControlsWithChildren;

        /// Used for keeping track of data verification errors
        protected TVerificationResultCollection FVerificationResultCollection;

        /// <summary>Whether the Form's Shown Event already occured. ATTENTION: See comment on
        /// <see cref="FormHasBeenShown" /> Property for important implementation details!</summary>
        protected Boolean FFormHasBeenShown = false;

        /// <summary>Name of Button Controls that will get disabled when the screen is in read-only mode.</summary>
        private List <string>FReadOnlyModeButtons = new List <string>();

        private Color? FStatusBarBackColorBeforeReadonlyMode = null;

        /// <summary>See comment on Property <see cref="SecurityReadOnly"/>!</summary>
        protected bool FSecurityReadOnly = false;

        /// <summary>See comment on Property <see cref="SecurityReportingAllowed"/>!</summary>
        protected bool FSecurityReportExecutionAllowed = true;

        /// <summary>See comment on Property <see cref="SecurityEditingAndSavingPermissionRequired"/>!</summary>
        protected string FSecurityEditingAndSavingPermissionRequired;

        /// Used for keeping track of data verification errors
        public TVerificationResultCollection VerificationResultCollection
        {
            get
            {
                return FVerificationResultCollection;
            }
            set
            {
                FVerificationResultCollection = value;
            }
        }

        /// <summary>
        /// Whether the Form's Shown Event has already occured. ATTENTION: This Property's value is
        /// NOT AUTOMATICALLY SET once a Forms Shown Event has run!!! - A Form that wants to have
        /// that Property's value reflect the fact that a Form has been shown needs to
        /// hook up its .Shown Event to this Classes' <see cref="OnFormShown" /> Method!!!
        /// </summary>
        public Boolean FormHasBeenShown
        {
            get
            {
                return FFormHasBeenShown;
            }
        }

        /// <summary>
        /// Gets the maximum number of references to find before abandoning the count when deleting a record.
        /// </summary>
        public int MaxReferenceCountOnDelete
        {
            get
            {
                return FMaxReferenceCountOnDelete;
            }
        }

        /// <summary>
        /// Sets the Forms' Title.
        /// </summary>
        /// <remarks>Utilised by <see cref="TFrmPetraEditUtils.SetScreenCaption"/>.</remarks>
        public string FormTitle
        {
            get
            {
                return FFormTitle;
            }

            set
            {
                FFormTitle = value;
            }
        }

        /// <summary>What context (e.g. OpenPetra Module) the screen is for (for security purposes).</summary>
        public string SecurityScreenContext
        {
            get
            {
                return FSecurityScreenContext;
            }

            set
            {
                FSecurityScreenContext = value;
            }
        }

        /// <summary>
        /// Set to false to disallow editing (and saving where applicable) of data. The Methods <see cref="SetScreenCaption"/> and
        /// <see cref="ApplySecurity"/> read this Property and prefix the Form's Title with
        /// 'READ-ONLY: ' and disable the 'OK' or 'Apply' button if these are present on the Form in case this
        /// Property returns 'false.
        /// </summary>
        /// <remarks>
        /// 1) TFrmPetraEditUtils overrides this Property (with 'override') and adds to the functionality of the Setter!
        /// 2) Screens can inquire this Property at certain Events/stages to implement custom logic, i.e. to
        /// disable other buttons.
        /// <para>
        /// <em>IMPORTANT:</em>The provisions here only serve for *client-side convenience* and *don't prevent*
        /// saving of data for real, i.e. on the server side!!!</para></remarks>
        public virtual bool SecurityReadOnly
        {
            get
            {
                return FSecurityReadOnly;
            }

            set
            {
                FSecurityReadOnly = value;
            }
        }

        /// <summary>
        /// The Method <see cref="ApplySecurity"/> sets this Property and denies the opening of a screen straightaway
        /// if the <see cref="SecurityScreenContext"/> doesn't match what the 'SecurityPermissions' passed into it
        /// dictates.
        /// </summary>
        /// <remarks>
        /// <para><em>IMPORTANT:</em>The provisions here only serve for *client-side convenience* and *don't prevent*
        /// reporting of data for real, i.e. on the server side!!!</para>
        /// <para>'Reporting' in this context is seen as a loose concept, i.e. not seen as restricted to Reports!</para>
        /// </remarks>
        public virtual bool SecurityReportingAllowed
        {
            get
            {
                return FSecurityReportExecutionAllowed;
            }

            set
            {
                FSecurityReportExecutionAllowed = value;
            }
        }

        /// <summary>
        /// The name of the permission that gives a user the right to edit (and save where applicable) data in this Form.
        /// </summary>
        public string SecurityEditingAndSavingPermissionRequired
        {
            get
            {
                return FSecurityEditingAndSavingPermissionRequired;
            }

            set
            {
                FSecurityEditingAndSavingPermissionRequired = value;
            }
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ACallerForm">the form that has opened this window; needed for focusing when this window is closed later</param>
        /// <param name="ATheForm"></param>
        /// <param name="AStatusBar"></param>
        /// <param name="ASecurityScreenContext"></param>
        public TFrmPetraUtils(Form ACallerForm, IFrmPetra ATheForm, TExtStatusBarHelp AStatusBar,
            string ASecurityScreenContext = "")
        {
            FFormActivatedForFirstTime = true;
            FVerificationResultCollection = new TVerificationResultCollection();

            FTheForm = ATheForm;
            FWinForm = (Form)ATheForm;
            FSecurityScreenContext = ASecurityScreenContext;
            FStatusBar = AStatusBar;
            FCallerForm = ACallerForm;

            if (ACallerForm != null)
            {
                TFormsList.GFormsList.NotifyWindowOpened(ACallerForm.Handle, FWinForm.Handle);
            }

            FWindowExtensions = new TFrmPetraWindowExtensions(FWinForm, FCallerForm);

            //
            // Initialise the Data Validation ToolTip
            //
            FValidationToolTip = new System.Windows.Forms.ToolTip();
            FValidationToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            FValidationToolTip.ToolTipTitle = Catalog.GetString("Incorrect Data");
            FValidationToolTip.UseAnimation = true;
            FValidationToolTip.UseFading = true;

            // WriteToStatusBar(Catalog.GetString("Ready."));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TFrmPetra_Activated(System.Object sender, System.EventArgs e)
        {
            if (FFormActivatedForFirstTime == true)
            {
                // prevent this happening again
                FFormActivatedForFirstTime = false;

                // do any low level initialisation
                LocalRunOnceOnActivation();

                // virtual method, overriden in child forms
                RunOnceOnActivation();
            }
        }

        /// <summary>
        /// just call this function to clean up when closing the form
        /// </summary>
        public void Close()
        {
            // to prevent strange error message, that would stop the form from closing
            FFormActivatedForFirstTime = false;
        }

        /** used to allow subforms to initialise
         * Also, now that the form is shown we can set the splitter bar distances
         */
        public void LocalRunOnceOnActivation()
        {
            if (!FNoAutoHookupOfAllControls)
            {
                HookupAllControls();
            }

            // Are we saving/restoring the window position?  This option is stored in User Defaults.
            if (TUserDefaults.IsInitialised)
            {
                if (TUserDefaults.GetBooleanDefault(TUserDefaults.NamedDefaults.USERDEFAULT_SAVE_WINDOW_POS_AND_SIZE, true))
                {
                    // (Note: Nant tests do not have a caller so we need to allow for this possibility)
                    if ((FWinForm.Name == "TFrmMainWindowNew") || (FWinForm.Name == "TFrmPartnerEdit")
                        || (FWinForm.Name == "TPartnerFindScreen")
                        || ((FCallerForm != null) && (FCallerForm.Name == "TFrmMainWindowNew")))
                    {
                        // Either we are loading the main window or we have been opened by the main window
                        // Now that the window has been activated we are ok to restore things like splitter distances
                        RestoreAdditionalWindowPositionProperties();
                    }
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RunOnceOnActivation()
        {
            if (FTheForm != null)
            {
                FTheForm.RunOnceOnActivation();
            }
        }

        /** used to iterate through the controls on the form
         */
        public void EnumerateControls(Control c)
        {
            foreach (Control ctrl in c.Controls)
            {
                // exclude TPetraUserControl objects that should not be hooked up
                if ((ctrl is TPetraUserControl)
                    && (!((TPetraUserControl)ctrl).CanBeHookedUpForValueChangedEvent))
                {
                    continue;
                }

                // recurse into children;
                // but special case for UpDownBase/NumericUpDown, because we don't want the child controls of that
                if ((ctrl.HasChildren == true) && !(ctrl is UpDownBase) && !(ctrl is TClbVersatile))
                {
                    EnumerateControls(ctrl);

                    if ((ctrl is Panel)
                        || (ctrl is GroupBox)
                        || (ctrl is UserControl))
                    {
                        FControlsWithChildren.Add(ctrl);
                    }
                }
                else
                {
                    FAllControls.Add(ctrl);
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public virtual void HookupAllControls()
        {
            Control IteratedControl;

            FAllControls = new ArrayList();
            FControlsWithChildren = new ArrayList();

            EnumerateControls(FWinForm); //this adds all controls on form to ArrayList

            // this is on an international version of Windows, so we want no bold fonts
            // because the letters are difficult to read
            bool changeFonts = TAppSettingsManager.ChangeFontForLocalisation();

            for (int Counter1 = 0; Counter1 < FAllControls.Count; Counter1++)
            {
                IteratedControl = (Control)FAllControls[Counter1];

                if (changeFonts)
                {
                    if (TAppSettingsManager.ReplaceFont(IteratedControl.Font))
                    {
                        // remove bold and replace with regular
                        IteratedControl.Font = new System.Drawing.Font(IteratedControl.Font.Name,
                            IteratedControl.Font.Size,
                            System.Drawing.FontStyle.Regular);
                    }
                }
            }

            // Hook up Global Application Help handler to to the Form itself
            FWinForm.HelpRequested += new HelpEventHandler(GlobalApplicationHelpEventHandler);

            // Hook up Global Application Help handler to all Controls that have child controls
            for (int Counter2 = 0; Counter2 < FControlsWithChildren.Count; Counter2++)
            {
                ((Control)FControlsWithChildren[Counter2]).HelpRequested += new HelpEventHandler(GlobalApplicationHelpEventHandler);
            }
        }

        /// <summary>
        /// Global application help handler. Every 'F1' key-press that happens in a context where it is hooked up
        /// gets passed to this handler.
        /// </summary>
        /// <param name="ASender">A Form or a Control.</param>
        /// <param name="AHelpEvent">Ignored.</param>
        void GlobalApplicationHelpEventHandler(object ASender, HelpEventArgs AHelpEvent)
        {
            Form HelpContextForm;
            Control HelpContextControl;

            if (ASender is Form)
            {
                HelpContextForm = (Form)ASender;
                HelpContextControl = null;
            }
            else
            {
                HelpContextForm = FWinForm;
                HelpContextControl = (Control)ASender;
            }

            try
            {
                if (!Ict.Common.HelpLauncher.ShowHelp(HelpContextForm, HelpContextControl))
                {
                    WriteToStatusBar(Catalog.GetString("Sorry, there is no help available for the context."));
                }
            }
            catch (EHelpLauncherException Exp)
            {
                MessageBox.Show(Exp.Message, "Error Launching Application Help", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="container"></param>
        public virtual void HookupAllInContainer(Control container)
        {
            // not implemented here
        }

        /// Set this to true to prevent the automatic hookup of change Events of all Controls on the Form
        public Boolean NoAutoHookupOfAllControls
        {
            set
            {
                FNoAutoHookupOfAllControls = value;
            }
        }

        #region Enable and Disable Action

        /// <summary>
        /// todoComment
        /// </summary>
        public event ActionEventHandler ActionEnablingEvent;

        private SortedList <string, bool>FActionStates = new SortedList <string, bool>();

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AActionName"></param>
        /// <param name="enable"></param>
        public void EnableAction(string AActionName, bool enable)
        {
            // store action enabled
            if (FActionStates.ContainsKey(AActionName))
            {
                FActionStates[AActionName] = enable;
            }
            else
            {
                FActionStates.Add(AActionName, enable);
            }

            //trigger event
            if (ActionEnablingEvent != null)
            {
                ActionEnablingEvent(this, new ActionEventArgs(AActionName, enable));
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AActionName"></param>
        /// <returns></returns>
        public bool IsEnabled(string AActionName)
        {
            if (FActionStates.ContainsKey(AActionName))
            {
                return FActionStates[AActionName];
            }

            return true;
        }

        /// <summary>todoComment</summary>
        public bool FormActivatedForFirstTime
        {
            get
            {
                return FFormActivatedForFirstTime;
            }
            set
            {
                FFormActivatedForFirstTime = value;
            }
        }

        /// <summary>
        /// ToolTip instance which is used to show Data Validation messages.
        /// </summary>
        public ToolTip ValidationToolTip
        {
            get
            {
                return FValidationToolTip;
            }
        }

        /// <summary>
        /// Sets the Validation ToolTip severity. This affects the icon and title of the Validation ToolTip.
        /// </summary>
        public TResultSeverity ValidationToolTipSeverity
        {
            set
            {
                switch (value)
                {
                    case TResultSeverity.Resv_Critical:
                        FValidationToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
                        FValidationToolTip.ToolTipTitle = Catalog.GetString("Incorrect Data");

                        break;

                    case TResultSeverity.Resv_Noncritical:
                        FValidationToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
                        FValidationToolTip.ToolTipTitle = Catalog.GetString("Warning");

                        break;

                    case TResultSeverity.Resv_Info:
                        FValidationToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                        FValidationToolTip.ToolTipTitle = Catalog.GetString("Information");

                        break;

                    case TResultSeverity.Resv_Status:
                        FValidationToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.None;
                        FValidationToolTip.ToolTipTitle = Catalog.GetString("Note");

                        break;
                }
            }
        }

        /// <summary>
        /// Dictionary that contains Controls on whose data Data Validation should be run.
        /// </summary>
        public TValidationControlsDict ValidationControlsDict
        {
            get
            {
                return FValidationControlsDict;
            }
            set
            {
                FValidationControlsDict = value;
            }
        }

        /// useful for initialising actions, eg based on permissions
        virtual public void InitActionState()
        {
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Form_KeyDown(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (FWindowExtensions.AContainerControlWantsEscape(sender)
                    || (TUserDefaults.GetBooleanDefault(TUserDefaults.NamedDefaults.USERDEFAULT_ESC_CLOSES_SCREEN, true) == false)
                    || (this.FWinForm.Name == "TFrmMainWindowNew"))
                {
                    // Either the user default is NOT to use the ESC key to close screens
                    // or there is a control on the form that needs to handle the escape key,
                    // or we are the main window
                    //  so we do nothing and the keyDown message will get passed to the control - which will do something if it needs to
                }
                else
                {
                    // No control wants the escape key so we are free to handle the KeyDown message at the Form level
                    e.Handled = true;
                    ExecuteAction(eActionId.eClose);
                }
            }
            else if (e.KeyCode == Keys.F1)
            {
                e.Handled = true;
                ExecuteAction(eActionId.eHelp);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Mni_Click(System.Object sender, System.EventArgs e)
        {
            if (!(sender is ToolStripItem))
            {
                return;
            }

            ExecuteAction((TItemTag)((ToolStripItem)sender).Tag);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATag"></param>
        public void ExecuteAction(TItemTag ATag)
        {
            ExecuteAction(ATag.Id);
        }

        private Assembly CommonDialogsAssembly = null;

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="id"></param>
        public void ExecuteAction(eActionId id)
        {
            if (CommonDialogsAssembly == null)
            {
                CommonDialogsAssembly = Assembly.LoadFrom(
                    TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + "Ict.Petra.Client.CommonDialogs.dll");
            }

            switch (id)
            {
                case eActionId.eClose:
                    this.Close();
                    FWinForm.Close();
                    break;

                case eActionId.eHelpAbout:
                    System.Type aboutDialogType = CommonDialogsAssembly.GetType("Ict.Petra.Client.CommonDialogs.TFrmAboutDialog");

                    using (Form aboutDialog = (Form)Activator.CreateInstance(aboutDialogType, new object[] { this.FWinForm, String.Empty }))
                    {
                        aboutDialog.ShowDialog();
                    }

                    break;

                case eActionId.eKeyboardShortcuts:
                    System.Type shortcutsDialogType = CommonDialogsAssembly.GetType("Ict.Petra.Client.CommonDialogs.TFrmKeyboardShortcutsDialog");
                    Type ActiveFormType = Form.ActiveForm.GetType();

                    using (Form shortcutsDialog = (Form)Activator.CreateInstance(shortcutsDialogType, new object[] { this.FWinForm, String.Empty }))
                    {
                        //
                        // For some Forms (and Tabs on these Forms) we show a specific Shortcut Tab
                        //

                        // Partner Edit Form
                        if (ActiveFormType.FullName == "Ict.Petra.Client.MPartner.Gui.TFrmPartnerEdit")
                        {
                            // Inquire currently selected Tab
                            object CurrentlySelectedTabPageNameObj = ActiveFormType.GetProperty("CurrentlySelectedTabPageName").GetValue(
                                Form.ActiveForm,
                                null);

                            // If it is the Contact Details Tab: show Shortcut Tab that is specific to the Contact Details Tab
                            if (CurrentlySelectedTabPageNameObj.ToString() == "petpContactDetails")
                            {
                                PropertyInfo InitiallySelectedTabProperty = shortcutsDialog.GetType().GetProperty("InitiallySelectedTab",
                                    BindingFlags.Public | BindingFlags.Instance);

                                if ((null != InitiallySelectedTabProperty) && InitiallySelectedTabProperty.CanWrite)
                                {
                                    InitiallySelectedTabProperty.SetValue(shortcutsDialog, "PartnerEditContactDetailsTab", null);
                                }
                            }
                        }

                        if (ActiveFormType.FullName == "Ict.Petra.Client.App.PetraClient.TFrmMainWindowNew")
                        {
                            PropertyInfo InitiallySelectedTabProperty = shortcutsDialog.GetType().GetProperty("InitiallySelectedTab",
                                BindingFlags.Public | BindingFlags.Instance);

                            if ((null != InitiallySelectedTabProperty) && InitiallySelectedTabProperty.CanWrite)
                            {
                                InitiallySelectedTabProperty.SetValue(shortcutsDialog, "tpgMainMenu", null);
                            }
                        }

                        shortcutsDialog.ShowDialog();
                    }
                    break;

                case eActionId.eHelp:
                {
                    throw new NotImplementedException();

                    // TODO Launch OpenPetra Help once it becomes available
                }

                case eActionId.eBugReport:
                {
                    throw new NotImplementedException();

                    // TODO Launch Bug Report Form (feature Bug #4956) once it becomes available
                }

                case eActionId.eHelpDevelopmentTeam:
                    throw new NotImplementedException();
#if TODO
                    using (DevelopmentTeamDialog teamDialog = new DevelopmentTeamDialog())
                    {
                        teamDialog.ShowDialog();
                    }

                    break;
#endif
            }
        }

        /// <summary>
        /// add the form to the forms list and set its window position/size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TFrmPetra_Load(System.Object sender, System.EventArgs e)
        {
            TFormsList.GFormsList.Add(FWinForm);

            FWindowExtensions.TForm_Load();
        }

        /**
         * Event Handler that is invoked when the Form is about to close - no matter
         * how the closing was invoked (by calling Form.Close, a Close button, the
         * x Button of a Form, etc).
         *
         * @param sender Event sender
         * @param e EventArgs that allow cancelling of the closing
         *
         */
        public virtual void TFrmPetra_Closing(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CanClose() || !FTheForm.CanClose())
            {
                // MessageBox.Show('TFrmPetra.TFrmPetra_Closing: e.Cancel := true');
                e.Cancel = true;
            }
            else
            {
                // MessageBox.Show('TFrmPetra.TFrmPetra_Closing: GFormsList.Remove(Self as Form)');
                TFormsList.GFormsList.NotifyWindowClose(this.FWinForm.Handle);
                TFormsList.GFormsList.Remove(FWinForm);

                FWindowExtensions.TForm_Closing();
                FStatusBar = null; // Throwing away my reference to the status bar may allow the form to be disposed,
                                   // and may avoid an exception in WriteToStatusBar, below.
            }
        }

        /// <summary>
        /// Hook up this Event to a Forms' Shown Event to allow the <see cref="FormHasBeenShown" />
        /// Property to reflect that.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        public void OnFormShown(System.Object sender, System.EventArgs e)
        {
            FFormHasBeenShown = true;
        }

        #endregion

        #region Helper Functions

        /**
         * This function can be used to write to the StatusBar.
         *
         * TLogging can use it as a callback procedure, so it does not need to know
         * about the StatusBar itself
         *
         * @param s the text to be displayed in the StatusBar
         *
         */
        delegate void WriteCallback (String s);

        /// <summary>
        /// (Thread safe)
        /// </summary>
        /// <param name="s"></param>
        public void WriteToStatusBar(String s)
        {
            if ((FStatusBar == null) || (FStatusBar.IsDisposed) || (!FStatusBar.IsHandleCreated))
            {
                return;
            }

            try // despite the efforts above, it's still possible that the FStatusBar object will be invalid by the time it's actually used.
            {   // so if it gives any problem, I'm just ignoring it.
                if (FStatusBar.InvokeRequired)
                {
                    FStatusBar.Invoke(new WriteCallback(WriteToStatusBar), new object[] { s });
                }
                else
                {
                    FStatusBar.ShowMessage(s);
                }
            }
            catch (Exception)
            {
            }
        }

        /**
         * This function tells the caller whether the window can be closed.
         * It can be used to find out if something is still edited, for example.
         *
         * @return true if window can be closed
         *
         */
        virtual public bool CanClose()
        {
            return true;
        }

        /// <summary>
        /// Method that restores splitter bar positions.  If you have a tabbed control that hosts a split container
        /// you need to call this method when the tab page changes.
        /// </summary>
        public void RestoreAdditionalWindowPositionProperties()
        {
            FWindowExtensions.RestoreAdditionalWindowPositionProperties();
        }

        /// <summary>
        /// Loads the file that stores windows sizes/positions for the active user
        /// </summary>
        public void LoadWindowPositionsFromFile()
        {
            FWindowExtensions.LoadWindowPositionsFromFile();
        }

        #endregion

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="AHelpText"></param>
        public void SetStatusBarText(Control AControl, string AHelpText)
        {
            if (FStatusBar != null)
            {
                FStatusBar.SetHelpText(AControl, AHelpText);
            }
        }

        const Int16 MAX_COMBOBOX_HISTORY = 30;

        /// <summary>
        /// add new value of combobox to the user defaults, or move existing value to the front;
        /// limits the number of values to MAX_COMBOBOX_HISTORY
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        public void AddComboBoxHistory(System.Object Sender, TAcceptNewEntryEventArgs e)
        {
            string keyName = "CmbHistory" + ((Control)Sender).Name;
            StringCollection values = StringHelper.StrSplit(TUserDefaults.GetStringDefault(keyName, ""), ",");

            values.Remove(e.ItemString);
            values.Insert(0, e.ItemString);

            while (values.Count > MAX_COMBOBOX_HISTORY)
            {
                values.RemoveAt(values.Count - 1);
            }

            TUserDefaults.SetDefault(keyName, StringHelper.StrMerge(values, ','));
        }

        /// <summary>
        /// load the history of a combobox for auto completion from the user defaults
        /// </summary>
        /// <param name="AComboBox"></param>
        public void LoadComboBoxHistory(TCmbAutoComplete AComboBox)
        {
            AComboBox.SetDataSourceStringList(TUserDefaults.GetStringDefault("CmbHistory" + AComboBox.Name, ""));
        }

        /// <summary>
        /// get the form that has opened this form
        /// </summary>
        /// <returns></returns>
        public Form GetCallerForm()
        {
            return FCallerForm;
        }

        /// <summary>
        /// get the current form
        /// </summary>
        /// <returns></returns>
        public Form GetForm()
        {
            return FWinForm;
        }

        /// <summary>
        /// Sets the tooltip for a Control.
        /// </summary>
        public void SetToolTip(Control AControl, string AToolTipText)
        {
            if (FtipForm == null)
            {
                FtipForm = new ToolTip();
            }

            FtipForm.SetToolTip(AControl, AToolTipText);
        }

        /// <summary>
        /// Method that momentarily removes the focus from the active control, so ensuring that the OnLeave event is fired.
        /// The method is used before saving data.
        /// </summary>
        public void ForceOnLeaveForActiveControl()
        {
            // Find the currently active control
            Form theForm = GetForm();
            Control CurrentActiveControl;
            ContainerControl ParentControl = theForm;

            do
            {
                CurrentActiveControl = ParentControl.ActiveControl;
                ParentControl = CurrentActiveControl as ContainerControl;
            } while (ParentControl != null);

            // Momentarily remove focus from active control. This ensures OnLeave event is fired for control.
            theForm.ActiveControl = null;
            theForm.ActiveControl = CurrentActiveControl;
        }

        private FormWindowState FFormWindowState = FormWindowState.Normal;

        /// <summary>
        /// A method to refresh a specific control after it has been resized as a result of a Maximize or a Restore.
        /// This seems to be necessary on the Personnel control in Partner-Edit
        /// </summary>
        /// <param name="AControl">The control to refresh - usually pnlDetailGrid</param>
        public void RefreshSpecificControlOnWindowMaxOrRestore(Control AControl)
        {
            if (FWinForm == null)
            {
                return;
            }

            FormWindowState curWindowState = FWinForm.WindowState;

            switch (curWindowState)
            {
                case FormWindowState.Maximized:
                    AControl.Refresh();
                    break;

                case FormWindowState.Normal:

                    if (FFormWindowState == FormWindowState.Maximized /*|| !FDoneFirstResize */)
                    {
                        AControl.Refresh();
                    }

                    break;
            }

            FFormWindowState = curWindowState;
        }

        /// <summary>
        /// Sets a Form's Title.
        /// </summary>
        /// <param name="ACaptionPostFix">Any string specified here gets added to the Form's Title (default = "").</param>
        /// <remarks>TFrmPetraEditUtils re-implements this Method (with 'new') and adds to the functionality here
        /// but calls this Method with base, too!
        /// </remarks>
        public void SetScreenCaption(string ACaptionPostFix = "")
        {
            String CaptionPrefix = "";

            if (SecurityReadOnly)
            {
                if (!FWinForm.Text.StartsWith(StrFormCaptionPrefixReadonly))
                {
                    CaptionPrefix = StrFormCaptionPrefixReadonly;
                }
            }

            FWinForm.Text = CaptionPrefix + (FormTitle != String.Empty ? FormTitle : FWinForm.Text) + ACaptionPostFix;
        }

        /// <summary>
        /// Strips off a 'NEW: ' or 'READ-ONLY: ' prefix from the screen caption
        /// </summary>
        public void RemoveStdPrefixesFromScreenCaption(bool AReadOnlyPrefix = false)
        {
            if (!AReadOnlyPrefix)
            {
                if (FWinForm.Text.StartsWith(StrFormCaptionPrefixNew))
                {
                    FWinForm.Text = FWinForm.Text.Substring(TFrmPetraEditUtils.StrFormCaptionPrefixNew.Length);
                }
            }

            if (FWinForm.Text.StartsWith(StrFormCaptionPrefixReadonly))
            {
                FWinForm.Text = FWinForm.Text.Substring(TFrmPetraEditUtils.StrFormCaptionPrefixReadonly.Length);
            }
        }

        #region Security

        /// <summary>
        /// Sets up OpenPetra Security to restrict functionality as requested.
        /// </summary>
        /// <remarks>
        /// 1) TFrmPetraEditUtils re-implements this Method (with 'new') and adds to the functionality here
        /// but calls this Method with base, too!
        /// 2) When this Method gets called from a UserControl it affects not only the UserControl but the
        /// Form as a whole!!!</remarks>
        public void ApplySecurity(List <string>ASecurityPermissions = null, TScreenSecurity AGetScreenSecurity = null,
            bool AEnableDisabledButtons = true)
        {
            List <string>SecurityPermissions;
            string PermissionRequiredToOvercomeAccessDenied = String.Empty;

            if ((ASecurityPermissions == null)
                && (FFormsSecurityPermissions == null))
            {
                throw new ArgumentNullException("ASecurityPermissions",
                    "ASecurityPermissions must not be null if they haven't been set up once by the Form");
            }
            else if (ASecurityPermissions == null)
            {
                ASecurityPermissions = FFormsSecurityPermissions;
            }

            FFormsSecurityPermissions = ASecurityPermissions;

            if (AGetScreenSecurity == null)
            {
                switch (SecurityScreenContext)
                {
                    case "Partner":
                    case "Personnel":
                    case "Finance":
                    case "Conference":
                    case "FinDev":
                    case "SysMan":
                        SecurityScreenContext = "M" + SecurityScreenContext;
                        break;
                }

                SecurityPermissions = SecurityEvaluateStandardPermissions(ASecurityPermissions,
                    out PermissionRequiredToOvercomeAccessDenied);
            }
            else
            {
                SecurityPermissions = AGetScreenSecurity();

                if (SecurityPermissions.Count == 0)
                {
                    SecurityPermissions = SecurityEvaluateStandardPermissions(ASecurityPermissions,
                        out PermissionRequiredToOvercomeAccessDenied);
                }
            }

            // Note: The SecurityReadOnly Property is virtual and the inheriting TFrmPetraEditUtils Class
            // extends what the setter of that Property does!
            SecurityReadOnly = SecurityPermissions.Contains(TSecurityChecks.SECURITYRESTRICTION_READONLY);

            if (SecurityReadOnly)
            {
                // Prefix screen's Title with 'READ-ONLY: '
                SetScreenCaption();

                // Alter the colour of the Status Bar, too, to make it more obvious that the screen is in read-only mode
                // ... but first store the current BackColor so it can optionally be restored at a later point in time
                // (see 'else branch' below).
                if (!FStatusBarBackColorBeforeReadonlyMode.HasValue)
                {
                    FStatusBarBackColorBeforeReadonlyMode = FStatusBar.BackColor;
                }

                FStatusBar.BackColor = System.Drawing.Color.LightGray;

                //
                // Find 'OK' and 'Apply' Button Controls and disable them if they exist in the Form
                // to prevent the user from first trying to use those buttons only to find out that the changes that (s)he
                // made can't be saved later.
                //
                CallEnableDisableReadOnlyModeButtons(false);
            }
            else
            {
                if (FStatusBarBackColorBeforeReadonlyMode.HasValue)
                {
                    RemoveStdPrefixesFromScreenCaption(true);
                    FStatusBar.BackColor = FStatusBarBackColorBeforeReadonlyMode.Value;

                    if (AEnableDisabledButtons)
                    {
                        CallEnableDisableReadOnlyModeButtons(true);
                    }
                }
            }

            SecurityReportingAllowed = !SecurityPermissions.Contains(TSecurityChecks.SECURITYRESTRICTION_FINANCEREPORTINGDENIED);

            if (!SecurityReportingAllowed)
            {
                throw new ESecurityAccessDeniedException(
                    String.Format("You are not allowed to use this function because you don't have the necessary permission! " +
                        "({0} permission would be required.)", PermissionRequiredToOvercomeAccessDenied), FCallerForm.GetType().Name);
            }
        }

        /// <summary>
        /// Finds the 'OK' and 'Apply' Button Controls and disables/enables them if they exist in the Form.
        /// Disabling is done to prevent the user from first trying to use those buttons only to find out that the changes that (s)he
        /// made can't be saved later (these Buttons must be named according to the default button naming scheme for this to work!).
        /// </summary>
        /// <param name="AEnable">Set to false to disable these Buttons, set to true to enable them
        /// (default = false).</param>
        public void CallEnableDisableReadOnlyModeButtons(bool AEnable = false)
        {
            EnableDisableReadOnlyModeButtons(AEnable, ref FReadOnlyModeButtons, delegate
                {
                    FReadOnlyModeButtons.Add("btnOK");
                    FReadOnlyModeButtons.Add("btnApply");
                });
        }

        /// <summary>
        /// Finds the passed Button Controls and enables/disables them if they exist in the Form.
        /// The disabling is to prevent the user from first trying to use those buttons only to find out
        /// that the changes that (s)he made can't be saved later.
        /// </summary>
        /// <param name="AEnable">Set to false to disable these Buttons, set to true to enable them
        /// (default = false).</param>
        /// <param name="AReadOnlyModeButtons">List of names of Buttons that should be enabled/disabled in the Form.</param>
        /// <param name="AButtonsAddingCodeWhenDisabling">Program code that will get run once to add the appropriate
        /// Buttons to the AReadOnlyModeButtons List.</param>
        protected void EnableDisableReadOnlyModeButtons(bool AEnable, ref List <string>AReadOnlyModeButtons,
            Action AButtonsAddingCodeWhenDisabling)
        {
            Control[] BtnCtrls;

            if (!AEnable)
            {
                if (AButtonsAddingCodeWhenDisabling == null)
                {
                    throw new ArgumentNullException("AButtonsAddingCodeWhenDisabling",
                        "AButtonsAddingCodeWhenDisabling must not be null if AEnable is false");
                }

                if (AReadOnlyModeButtons.Count == 0)
                {
                    // Execute code that should be run once to add the appropriate Buttons to the AReadOnlyModeButtons List.
                    AButtonsAddingCodeWhenDisabling();
                }
            }

            foreach (var EnableDisableControl in AReadOnlyModeButtons)
            {
                BtnCtrls = FWinForm.Controls.Find(EnableDisableControl, true);

                if ((BtnCtrls != null)
                    && (BtnCtrls.Length > 0))
                {
                    BtnCtrls[0].Enabled = AEnable;
                }
            }
        }

        /// <summary>
        /// Call this Method if the screen has been shown in 'read-only Mode' but the *appearance changes*
        /// that go with that should be revoked.
        /// <para>It enables any Buttons that were disabled because of this Mode, removes the
        /// 'READ-ONLY' prefix from the screen's Title Bar and sets the BackColor of the StatusBar
        /// to what it normally is.</para>
        /// </summary>
        /// <remarks>Calling this Method will <em>not set</em> the Form's 'SecurityReadOnly' Property to false!</remarks>
        protected void SecurityUndoReadOnlyModeAppearanceChanges()
        {
            CallEnableDisableReadOnlyModeButtons(true);

            RemoveStdPrefixesFromScreenCaption(true);

            FStatusBar.BackColor = FStatusBarBackColorBeforeReadonlyMode.Value;
        }

        private List <string>SecurityEvaluateStandardPermissions(List <string>ASecurityPermissions,
            out string APermissionRequiredToOvercomeAccessDenied)
        {
            List <string>ReturnValue = new List <string>();

            if (ASecurityPermissions == null)
            {
                throw new ArgumentNullException("ASecurityPermissions", "ASecurityPermissions must not be null");
            }

            APermissionRequiredToOvercomeAccessDenied = String.Empty;

            if (ASecurityPermissions.Contains(TSecurityChecks.SECURITYPERMISSION_EDITING_AND_SAVING_OF_SETUP_DATA))
            {
                SecurityEditingAndSavingPermissionRequired =
                    TSecurityChecks.GetModulePermissionForSavingOfSetupScreenData(SecurityScreenContext);

                if (SecurityEditingAndSavingPermissionRequired.StartsWith("### ERROR ###"))
                {
                    MessageBox.Show("Developer forgot to specify Module in the YAML file with 'ModuleForSecurity' Element!");
                }

                if (!UserInfo.GUserInfo.IsInModule(SecurityEditingAndSavingPermissionRequired))
                {
                    APermissionRequiredToOvercomeAccessDenied = SecurityEditingAndSavingPermissionRequired;

                    ReturnValue.Add(TSecurityChecks.SECURITYRESTRICTION_READONLY);
                }
            }

            if ((SecurityScreenContext == "MFinanceReporting")
                && (ASecurityPermissions.Contains(TSecurityChecks.SECURITYPERMISSION_FINANCEREPORTING)))
            {
                APermissionRequiredToOvercomeAccessDenied = SharedConstants.PETRAMODULE_FINANCERPT;

                if (!UserInfo.GUserInfo.IsInModule(APermissionRequiredToOvercomeAccessDenied))
                {
                    ReturnValue.Add(TSecurityChecks.SECURITYRESTRICTION_FINANCEREPORTINGDENIED);
                }
                else
                {
                    APermissionRequiredToOvercomeAccessDenied = String.Empty;
                }
            }

            return ReturnValue;
        }

        #endregion
    }

    /// <summary>todoComment</summary>
    public enum eActionId
    {
        /// <summary>todoComment</summary>
        eHelp,

        /// <summary>todoComment</summary>
        eClose,

        /// <summary>todoComment</summary>
        eHelpDevelopmentTeam,

        /// <summary>todoComment</summary>
        eHelpAbout,

        /// <summary>todoComment</summary>
        eBugReport,

        /// <summary>todoComment</summary>
        eKeyboardShortcuts
    };

    /// <summary>
    /// this is a helper class to identify menu items and toolbar buttons
    /// </summary>
    public class TItemTag
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AId">identifier for the type of menu item</param>
        public TItemTag(eActionId AId)
        {
            id = AId;
        }

        private eActionId id;

        /// <summary>todoComment</summary>
        public eActionId Id
        {
            get
            {
                return id;
            }
        }
    }

    /// <summary>todoComment</summary>
    public class PetraForm
    {
        /// <summary>todoComment</summary>
        public const Int32 AUTOSCALEBASESIZEWIDTHFOR96DPI = 6;
    }

    /// <summary>todoComment</summary>
    public interface IFrmPetra
    {
        /// <summary>todoComment</summary>
        void RunOnceOnActivation();

        /// <summary>todoComment</summary>
        bool CanClose();

        /// <summary>todoComment</summary>
        TFrmPetraUtils GetPetraUtilsObject();
    }
}