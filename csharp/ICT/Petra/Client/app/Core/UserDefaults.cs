//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2012 by OM International
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
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Loads and saves all user-customisable defaults.
    /// The TUserDefaults class gives access to all user-customisable defaults.
    ///
    /// @Comment The User Defaults are stored in the Database (s_user_defaults table)
    ///   on the Server.
    /// </summary>
    public class TUserDefaults
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public class NamedDefaults
        {
            /// <summary>todoComment</summary>
            public const String WINDOW_POSITION_PREFIX = "WINDOW_POS_";

            /// <summary>todoComment</summary>
            public const String WINDOW_POSITION_AND_SIZE_PREFIX = "WINDOW_POS_AND_SIZE_";

            /// <summary>Colour of the background of all SourceGrid DataGrid instances.</summary>
            public const String COLOUR_GRID_BACKGROUND = "COLOUR_GRID_BACKGROUND";

            /// <summary>Colour of the background of every cell and row of all SourceGrid DataGrid instances.</summary>
            public const String COLOUR_GRID_CELLBACKGROUND = "COLOUR_GRID_CELLBACKGROUND";

            /// <summary>Alternating background colour of all SourceGrid DataGrid instances.</summary>
            public const String COLOUR_GRID_ALTERNATE = "COLOUR_GRID_ALTERNATE";

            /// <summary>Colour of the Selection of all SourceGrid DataGrid instances.</summary>
            public const String COLOUR_GRID_SELECTION = "COLOUR_GRID_SELECTION";

            /// <summary>Colour of the Grid Lines of all SourceGrid DataGrid instances.</summary>
            public const String COLOUR_GRID_GRIDLINES = "COLOUR_GRID_GRIDLINES";
            
            #region TUserDefaults.TNamedDefaults

            /// <summary>
            /// Updates and immediately saves the UserDefault for the 'Last Partner Used'.
            /// </summary>
            /// <param name="APartnerKey">Partner Key of the last used Partner</param>
            /// <param name="ALastPartnerUse">Specifies which 'Last Partner Used' setting should be
            /// updated
            /// </param>
            /// <returns>void</returns>
            public static void SetLastPartnerWorkedWith(Int64 APartnerKey, TLastPartnerUse ALastPartnerUse)
            {
                String LastPartnerDefaultCode;

                /*
                 * Store the fact that this Partner is the 'Last Partner' that was worked with
                 */

                // set the given partner key as the last used partner for the given module
                LastPartnerDefaultCode = "";

                switch (ALastPartnerUse)
                {
                    case TLastPartnerUse.lpuMailroomPartner:
                        LastPartnerDefaultCode = TUserDefaults.USERDEFAULT_LASTPARTNERMAILROOM;
                        break;

                    case TLastPartnerUse.lpuPersonnelPerson:
                        LastPartnerDefaultCode = TUserDefaults.USERDEFAULT_LASTPERSONPERSONNEL;
                        break;

                    case TLastPartnerUse.lpuPersonnelUnit:
                        LastPartnerDefaultCode = TUserDefaults.USERDEFAULT_LASTUNITPERSONNEL;
                        break;

                    case TLastPartnerUse.lpuConferencePerson:
                        LastPartnerDefaultCode = TUserDefaults.USERDEFAULT_LASTPERSONCONFERENCE;
                        break;
                }

                TUserDefaults.SetDefault(LastPartnerDefaultCode, (object)APartnerKey);

                // This needs to be saved instantaneously because Progress 4GL will read it
                // (eg. when when 'Partner' > 'Work with Last Partner' is chosen in Partner
                // Module main screen [mailroom.w])!
                TUserDefaults.SaveChangedUserDefault(LastPartnerDefaultCode);
            }

            #endregion
        }

        private static SUserDefaultsTable UUserDefaultsDataTable;
        private static DataView UUserDefaults;

        /// <summary>
        /// todoComment
        /// </summary>
        public class TInternal
        {
            #region TInternal

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="AKey"></param>
            /// <returns></returns>
            public static bool HasUserDefault(String AKey)
            {
                return UUserDefaults.Find(AKey) != -1;
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="AKey"></param>
            /// <param name="ADefaultValue"></param>
            /// <returns></returns>
            public static String GetUserDefault(String AKey, String ADefaultValue)
            {
                String ReturnValue;
                Int32 FoundInRow;

                ReturnValue = "";
                FoundInRow = UUserDefaults.Find(AKey);

                if (FoundInRow != -1)
                {
                    // User default found
                    ReturnValue = (UUserDefaults[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()]).ToString();
                }
                else
                {
                    // User default not found, return default value
                    ReturnValue = ADefaultValue;
                }

                return ReturnValue;
            }

            /// <summary>
            /// overload
            /// </summary>
            /// <param name="AKey"></param>
            /// <returns></returns>
            public static String GetUserDefault(String AKey)
            {
                return GetUserDefault(AKey, "");
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="AKey"></param>
            /// <param name="AValue"></param>
            public static void SetUserDefault(String AKey, String AValue)
            {
                Int32 FoundInRow;
                DataRowView Tmp;

                FoundInRow = UUserDefaults.Find(AKey);

                if (FoundInRow != -1)
                {
                    // User default found
                    if (AValue != UUserDefaults[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()].ToString())
                    {
                        // Update only if the value is actually different
                        UUserDefaults[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()] = AValue;
                    }
                }
                else
                {
                    // User default not found, add it to the user defaults table
                    Tmp = UUserDefaults.AddNew();
                    Tmp[SUserDefaultsTable.GetUserIdDBName()] = Ict.Petra.Shared.UserInfo.GUserInfo.UserID;
                    Tmp[SUserDefaultsTable.GetDefaultCodeDBName()] = AKey;
                    Tmp[SUserDefaultsTable.GetDefaultValueDBName()] = AValue;
                    Tmp.EndEdit();
                }
            }

            #endregion
        }


        /// <summary>
        /// ------------------------------------------------------------------------------
        /// Petra General User Default Constants
        /// </summary>
        public const String PETRA_DISPLAYMODULEBACKGRDPICTURE = "DisplayPicture";

        /// <summary>todoComment</summary>
        public const String MAINMENU_VIEWOPTIONS_VIEWTASKS = "viewoptions_viewtasks";

        /// <summary>todoComment</summary>
        public const String MAINMENU_VIEWOPTIONS_TILESIZE = "viewoptions_tilesize";

        /// <summary>todoComment</summary>
        public const String MAINMENU_VIEWOPTIONS_SINGLECLICKEXECUTION = "viewoptions_singleclickexecution";

        /*------------------------------------------------------------------------------
         *  Partner User Default Constants
         * -------------------------------------------------------------------------------*/

        /// <summary>todoComment</summary>
        public const String PARTNER_FINDOPTIONS_CRITERIAFIELDSLEFT = "findopt_criteriafieldsleft";

        /// <summary>todoComment</summary>
        public const String PARTNER_FINDOPTIONS_CRITERIAFIELDSRIGHT = "findopt_criteriafieldsright";

        /// <summary>todoComment</summary>
        public const String PARTNER_FINDOPTIONS_NUMBEROFRESULTROWS = "findopt_numberofresultrows";

        /// <summary>todoComment</summary>
        public const String PARTNER_FINDOPTIONS_SHOWMATCHBUTTONS = "findopt_showmatchbuttons";

        /// <summary>todoComment</summary>
        public const String PARTNER_FINDOPTIONS_FIELDSDEFAULTMATCHVALUES = "findopt_fieldsdefaultmatchvalues";

        /// <summary>todoComment</summary>
        public const String PARTNER_FINDOPTIONS_EXACTPARTNERKEYMATCHSEARCH = "findopt_exactpartnerkeymatchsearch";

        /// <summary>todoComment</summary>
        public const String PARTNER_FIND_SPLITPOS_FORM = "partnfindscr_splitpos_form";

        /// <summary>todoComment</summary>
        public const String PARTNER_FIND_SPLITPOS_CRITERIA = "partnfindscr_splitpos_criteria";

        /// <summary>todoComment</summary>
        public const String PARTNER_FIND_SPLITPOS_FINDBYDETAILS = "partnfindscr_splitpos_findbydetails";

        /// <summary>todoComment</summary>
        public const String PARTNER_FIND_SPLITPOS_FINDBYDETAILS_CRIT = "partnfindscr_splitpos_findbydetails_crit";

        /// <summary>todoComment</summary>
        public const String PARTNER_FIND_PARTNERDETAILS_OPEN = "partnfindscr_partnerdetails_open";

        /// <summary>todoComment</summary>
        public const String PARTNER_FIND_PARTNERTASKS_OPEN = "partnfindscr_partnertasks_open";

        /// <summary>todoComment</summary>
        public const String PARTNER_FIND_CRIT_FINDCURRADDRONLY = "Find Curr Addr Only";

        /// <summary>todoComment</summary>
        public const String PARTNER_FIND_CRIT_PARTNERSTATUS = "Find Partner Status";

        /// <summary>todoComment</summary>
        public const String PARTNER_EDIT_UPPERPARTCOLLAPSED = "partneredit_upperpartcollapsed";

        /// <summary>todoComment</summary>
        public const String PARTNER_EDIT_MATCHSETTINGS_LEFT = "partneredit_matchsettings_left";

        /// <summary>todoComment</summary>
        public const String PARTNER_EDIT_MATCHSETTINGS_RIGHT = "partneredit_matchsettings_right";

        /// <summary>todoComment</summary>
        public const String PARTNER_ACQUISITIONCODE = "p_acquisition";

        /// <summary>todoComment</summary>
        public const String PARTNER_LANGUAGECODE = "p_language";

        /// <summary>todoComment</summary>
        public const String PARTNER_TIPS = "MPartner_TipsState";

        /// <summary>Name of the last created extract </summary>
        public const String PARTNER_EXTRAC_LAST_EXTRACT_NAME = "Extract";

        /// <summary>
        /// ------------------------------------------------------------------------------
        /// Reporting User Default Constants
        /// </summary>
        public const String FINANCE_REPORTING_SHOWDIFFFINANCIALYEARSELECTION = "ShowDiffFinancialYearSelection";

        /// <summary>which plugin to use for importing bank statements</summary>
        public const String FINANCE_BANKIMPORT_PLUGIN = "BankImportPlugin";

        /// <summary>which bank account to use</summary>
        public const String FINANCE_BANKIMPORT_BANKACCOUNT = "BankImportBankAccount";

        // Put other User Default Constants here as well.

        /// <summary>todoComment</summary>
        public const String USERDEFAULT_LASTPARTNERMAILROOM = "MailroomLastPerson";

        /// <summary>todoComment</summary>
        public const String USERDEFAULT_LASTPERSONPERSONNEL = "PersonnelLastPerson";

        /// <summary>todoComment</summary>
        public const String USERDEFAULT_LASTUNITPERSONNEL = "PersonnelLastUnit";

        /// <summary>todoComment</summary>
        public const String USERDEFAULT_LASTPERSONCONFERENCE = "ConferenceLastPerson";


        /*------------------------------------------------------------------------------
         *  Finance User Default Constants
         * -------------------------------------------------------------------------------*/

        /// <summary>todoComment</summary>
        public const String FINANCE_DEFAULT_LEDGERNUMBER = "a_default_ledger_number_i";

        /*------------------------------------------------------------------------------
         *  Conference Default Constants
         * -------------------------------------------------------------------------------*/

        /// <summary>todoComment</summary>
        public const String CONFERENCE_LASTCONFERENCEWORKEDWITH = "LastConferenceWorkedWith";


        /// <summary>
        /// initialise static variables
        /// </summary>
        public static void InitUserDefaults()
        {
            TRemote.MSysMan.Maintenance.UserDefaults.WebConnectors.GetUserDefaults(Ict.Petra.Shared.UserInfo.GUserInfo.UserID,
                out UUserDefaultsDataTable);
            UUserDefaults = new DataView(UUserDefaultsDataTable);
            UUserDefaults.Sort = SUserDefaultsTable.GetDefaultCodeDBName();
        }

        /// <summary>
        /// Saves one specific changed User Default.
        ///
        /// </summary>
        /// <param name="AKey">The Key of the User Default that should get saved.</param>
        /// <returns>true if successful, false if not.
        /// </returns>
        public static Boolean SaveChangedUserDefault(String AKey)
        {
            Boolean ReturnValue;
            SUserDefaultsRow SubmittedUserDefaultRow;
            SUserDefaultsTable DesiredUserDefaultsDataTable;
            Int32 FoundInRow;
            TVerificationResultCollection VerificationResult;

            ReturnValue = false;
            FoundInRow = UUserDefaults.Find(AKey);

            if (FoundInRow != -1)
            {
                // User default found
                // MessageBox.Show('Found changed User Default with RowState ''' + Enum(UUserDefaults.Item[FoundInRow].Row.RowState).ToString("G") + '''');
                if ((UUserDefaults[FoundInRow].Row.RowState == DataRowState.Modified)
                    || (UUserDefaults[FoundInRow].Row.RowState == DataRowState.Added))
                {
                    DesiredUserDefaultsDataTable = (SUserDefaultsTable)UUserDefaultsDataTable.Clone();
                    DesiredUserDefaultsDataTable.InitVars();
                    SubmittedUserDefaultRow = DesiredUserDefaultsDataTable.NewRowTyped(false);
                    SubmittedUserDefaultRow.ItemArray = UUserDefaults[FoundInRow].Row.ItemArray;
                    DesiredUserDefaultsDataTable.Rows.Add(SubmittedUserDefaultRow);

                    if (UUserDefaults[FoundInRow].Row.RowState == DataRowState.Modified)
                    {
                        // Mark row as no longer beeing new
                        SubmittedUserDefaultRow.AcceptChanges();

                        // Mark row as beeing changed
                        SubmittedUserDefaultRow.DefaultValue = SubmittedUserDefaultRow.DefaultValue + ' ';
                        SubmittedUserDefaultRow.AcceptChanges();
                        SubmittedUserDefaultRow.DefaultValue = SubmittedUserDefaultRow.DefaultValue.Substring(0,
                            SubmittedUserDefaultRow.DefaultValue.Length - 1);
                    }

                    // MessageBox.Show('Saving single User Default ''' + DesiredUserDefaultsDataTable.Rows[0].Item['s_default_code_c'].ToString + '''');
                    if (TRemote.MSysMan.Maintenance.UserDefaults.WebConnectors.SaveUserDefaults(Ict.Petra.Shared.UserInfo.GUserInfo.UserID,
                            ref DesiredUserDefaultsDataTable,
                            out VerificationResult))
                    {
                        // Copy over ModificationId that was changed on the Server side!
                        UUserDefaults[FoundInRow].Row.ItemArray = DesiredUserDefaultsDataTable.Rows.Find(new Object[] { UUserDefaults[FoundInRow][0],
                                                                                                                        UUserDefaults[FoundInRow][1] })
                                                                  .ItemArray;

                        // Mark this User Default as saved (unchanged)
                        UUserDefaults[FoundInRow].Row.AcceptChanges();
                        ReturnValue = true;
                    }
                    else
                    {
                        MessageBox.Show(Messages.BuildMessageFromVerificationResult("Saving of User Defaults failed!" + Environment.NewLine +
                                "Reasons:", VerificationResult));
                        ReturnValue = false;
                    }
                }
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Saves all changed User Defaults.
        ///
        /// </summary>
        /// <returns>true if successful, false if not.
        /// </returns>
        public static Boolean SaveChangedUserDefaults()
        {
            Boolean ReturnValue;
            SUserDefaultsTable UserDefaultsDataTableChanges;
            TVerificationResultCollection VerificationResult;

            UserDefaultsDataTableChanges = UUserDefaultsDataTable.GetChangesTyped();

            // MessageBox.Show('Changed/added User Defaults: ' + UserDefaultsDataTableChanges.Rows.Count.ToString);
            if (TRemote.MSysMan.Maintenance.UserDefaults.WebConnectors.SaveUserDefaults(Ict.Petra.Shared.UserInfo.GUserInfo.UserID,
                    ref UserDefaultsDataTableChanges,
                    out VerificationResult))
            {
                // TODO 1 oChristianK cUserDefaults / ModificationID : Copy the ModificationID into the Client's DataTable so that the PetraClient's ModificationID's of UserDefaults are the same than the ones of the PetraServer.
                UUserDefaultsDataTable.AcceptChanges();
                ReturnValue = true;
            }
            else
            {
                MessageBox.Show(Messages.BuildMessageFromVerificationResult("Saving of User Defaults failed!" + Environment.NewLine + "Reasons:",
                        VerificationResult));
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Refreshes a UserDefault in the local Cache that has been updated on the
        /// Server side or in the DB.
        ///
        /// @comment This is needed for the case where several PetraClient instances of
        /// the same Petra User are running and they are updating the same UserDefault.
        /// On detecting such a 'collision', the PetraServer queues a ClientTask to
        /// refresh the specific UserDefault in the local Cache, and the
        /// TClientTaskInstance.Execute background thread then invokes this function.
        ///
        /// </summary>
        /// <param name="AChangedUserDefaultCode">UserDefault Code to update</param>
        /// <param name="AChangedUserDefaultValue">Changed UserDefault Value</param>
        /// <param name="AChangedUserDefaultModId">ModificationID of the changed UserDefault
        /// DataRow
        /// </param>
        /// <returns>void</returns>
        public static void RefreshCachedUserDefault(String AChangedUserDefaultCode, String AChangedUserDefaultValue, String AChangedUserDefaultModId)
        {
            // TLogging.Log('Refreshing DefaultCode ''' + AChangedUserDefaultCode + ''' with Value: ''' + AChangedUserDefaultValue + '''');
            // Split String into String Array
            String[] ChangedUserDefaultCodes = AChangedUserDefaultCode.Split(new Char[] { RemotingConstants.GCLIENTTASKPARAMETER_SEPARATOR[0] });
            String[] ChangedUserDefaultValues = AChangedUserDefaultValue.Split(new Char[] { RemotingConstants.GCLIENTTASKPARAMETER_SEPARATOR[0] });
            String[] ChangedUserDefaultModIds = AChangedUserDefaultModId.Split(new Char[] { RemotingConstants.GCLIENTTASKPARAMETER_SEPARATOR[0] });

            for (Int16 Counter = 0; Counter <= ChangedUserDefaultCodes.Length - 1; Counter += 1)
            {
                // TLogging.Log('Refreshing UserDefault ''' + ChangedUserDefaultCodes[Counter] + ''' with value ''' + ChangedUserDefaultValues[Counter] + ''' (ModificationID: ''' + ChangedUserDefaultModIds[Counter] + '''');
                Int32 FoundInRow = UUserDefaults.Find(ChangedUserDefaultCodes[Counter]);

                if (FoundInRow != -1)
                {
                    // User default found
                    // TLogging.Log('Existing UserDefault ''' +
                    // UUserDefaults.Item[FoundInRow].Item[SUserDefaultsTable.GetDefaultCodeDBName].ToString + ''' with value ''' +
                    // UUserDefaults.Item[FoundInRow].Item[SUserDefaultsTable.GetDefaultValueDBName].ToString + ''' (ModificationID: ''' +
                    // UUserDefaults.Item[FoundInRow].Item[SUserDefaultsTable.GetModificationIDDBName].ToString + '''');
                    if (ChangedUserDefaultValues[Counter] != UUserDefaults[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()].ToString())
                    {
                        // Update only if the value is actually different
                        UUserDefaults[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()] = ChangedUserDefaultValues[Counter];
                    }

                    UUserDefaults[FoundInRow][SUserDefaultsTable.GetModificationIdDBName()] = Convert.ToDateTime(ChangedUserDefaultModIds[Counter]);

                    // Mark this refreshed UserDefault as unchanged
                    UUserDefaults[FoundInRow].Row.AcceptChanges();
                }
                else
                {
                    // User default not found, add it to the user defaults table
                    // TLogging.Log('UserDefault doesn''t exist yet > creating new one');
                    DataRowView Tmp = UUserDefaults.AddNew();
                    Tmp[SUserDefaultsTable.GetUserIdDBName()] = Ict.Petra.Shared.UserInfo.GUserInfo.UserID;
                    Tmp[SUserDefaultsTable.GetDefaultCodeDBName()] = ChangedUserDefaultCodes[Counter];
                    Tmp[SUserDefaultsTable.GetDefaultValueDBName()] = ChangedUserDefaultValues[Counter];
                    Tmp[SUserDefaultsTable.GetModificationIdDBName()] = ChangedUserDefaultModIds[Counter];
                    Tmp.EndEdit();

                    // Mark this refreshed UserDefault as unchanged
                    Tmp.Row.AcceptChanges();

                    // TLogging.Log('UserDefault: new Row added, RowState: ' + Enum(Tmp.Row.RowState).ToString("G"));
                }
            }
        }

        /// <summary>
        /// Reload the User Defaults.
        /// Causes TUserDefaults to reload the cached User Defaults Table
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void ReloadCachedUserDefaults()
        {
            MessageBox.Show("in ReloadCachedUserDefaults");
            SUserDefaultsTable TempUserDefaultsDataTable;
            DataSet UserDefaultsDS;

            // TODO 1 : ReaderWriterLock
            // reload user defaults from server
            TRemote.MSysMan.Maintenance.UserDefaults.WebConnectors.GetUserDefaults(Ict.Petra.Shared.UserInfo.GUserInfo.UserID,
                out TempUserDefaultsDataTable);

            // merge the current table with the one requested from the server so that client changes are not lost
            UserDefaultsDS = new DataSet();
            UserDefaultsDS.Tables.Add(UUserDefaultsDataTable);
            UserDefaultsDS.Merge((DataTable)TempUserDefaultsDataTable, true, MissingSchemaAction.Ignore);
            UUserDefaultsDataTable = (SUserDefaultsTable)UserDefaultsDS.Tables[SUserDefaultsTable.GetTableName()];
            UUserDefaults = new DataView(UUserDefaultsDataTable);
            UUserDefaults.Sort = SUserDefaultsTable.GetDefaultCodeDBName();
            UserDefaultsDS.Tables.Remove(UUserDefaultsDataTable);

            // remove table again (otherwise problems when called a second time)
        }

        /// <summary>
        /// Reload the User Defaults if called on the server.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void ReloadCachedUserDefaultsOnServer()
        {
            TRemote.MSysMan.Maintenance.UserDefaults.WebConnectors.ReloadUserDefaults(Ict.Petra.Shared.UserInfo.GUserInfo.UserID,
                out UUserDefaultsDataTable);
            UUserDefaults = new DataView(UUserDefaultsDataTable);
            UUserDefaults.Sort = SUserDefaultsTable.GetDefaultCodeDBName();
        }

        #region UserDefault Get and Set functions

        /// <summary>
        /// Find out if a user default exists already.
        /// This is required where the default should be calculated otherwise
        /// (e.g. FINANCE_REPORTING_SHOWDIFFFINANCIALYEARSELECTION)
        /// </summary>
        /// <returns>true if a default with the given key already exists
        /// </returns>
        public static bool HasDefault(String AKey)
        {
            return TInternal.HasUserDefault(AKey);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static bool GetBooleanDefault(String AKey, bool ADefault)
        {
            bool ReturnValue;
            TVariant value;

            try
            {
                value = new TVariant(TInternal.GetUserDefault(AKey, ADefault.ToString()));
                ReturnValue = value.ToBool();
            }
            catch (Exception)
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns></returns>
        public static bool GetBooleanDefault(String AKey)
        {
            return GetBooleanDefault(AKey, true);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static System.Char GetCharDefault(String AKey, System.Char ADefault)
        {
            return Convert.ToChar(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns></returns>
        public static System.Char GetCharDefault(String AKey)
        {
            return GetCharDefault(AKey, ' ');
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static double GetDoubleDefault(String AKey, double ADefault)
        {
            return Convert.ToDouble(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns></returns>
        public static double GetDoubleDefault(String AKey)
        {
            return GetDoubleDefault(AKey, 0.0);
        }

        /// <summary>
        /// The following set of functions serve as shortcuts to get User Defaults of a
        /// specific type.
        ///
        /// </summary>
        /// <param name="AKey">The Key of the User Default that should get retrieved.</param>
        /// <param name="ADefault">The value that should be returned in case the Key is not (yet)
        /// in the User Defaults.
        /// </param>
        /// <returns>void</returns>
        public static System.Int16 GetInt16Default(String AKey, System.Int16 ADefault)
        {
            return Convert.ToInt16(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns></returns>
        public static System.Int16 GetInt16Default(String AKey)
        {
            return GetInt16Default(AKey, 0);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static System.Int32 GetInt32Default(String AKey, System.Int32 ADefault)
        {
            return Convert.ToInt32(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns></returns>
        public static System.Int32 GetInt32Default(String AKey)
        {
            return GetInt32Default(AKey, 0);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static System.Int64 GetInt64Default(String AKey, System.Int64 ADefault)
        {
            return Convert.ToInt64(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns></returns>
        public static System.Int64 GetInt64Default(String AKey)
        {
            return GetInt64Default(AKey, 0);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static String GetStringDefault(String AKey, String ADefault)
        {
            return TInternal.GetUserDefault(AKey, ADefault);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns></returns>
        public static String GetStringDefault(String AKey)
        {
            return GetStringDefault(AKey, "");
        }

        /// <summary>
        /// Sets a User Default.
        ///
        /// </summary>
        /// <param name="AKey">The Key of the User Default that should get saved.</param>
        /// <param name="AValue">The value that should be stored. Note: This can be anything on
        /// which ToString can be called.
        /// </param>
        /// <returns>void</returns>
        public static void SetDefault(String AKey, object AValue)
        {
            TInternal.SetUserDefault(AKey, AValue.ToString());
        }

        #endregion
    }
}