/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
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
using Ict.Petra.Client.App.Core;
using System.Windows.Forms;

namespace Ict.Petra.Client.App.Gui
{
    /// Enumeration of User Tips for the Partner Module.
    /// The value of each enum member (0, 1, ...) is the position in the String of
    /// the User Default where the settings are stored.
    ///
    /// IMPORTANT: when adding new values to the enumeration, make sure that the
    /// enumeration VALUE IS SPECIFIED AND UNIQUE in the enum! Otherwise wrong
    /// User Tips settings might be read or written!!!
    public enum TMPartnerTips
    {
        /// <summary>todoComment</summary>
        mpatPartnerEditVideoTutorial = 0,

        /// <summary>todoComment</summary>
        mpatNewTabCountersGeneral = 1,

        /// <summary>todoComment</summary>
        mpatNewTabCountersAddresses = 2,

        /// <summary>todoComment</summary>
        mpatNewTabCountersSubscriptions = 3,

        /// <summary>todoComment</summary>
        mpatNewTabCountersNotes = 4,

        /// <summary>todoComment</summary>
        mpatNewCancelAllSubscriptions = 5,

        /// <summary>todoComment</summary>
        mpatNewPromotePartnerStatusChange = 6,

        /// <summary>todoComment</summary>
        mpatNewDeactivatePartner = 7
    };

    class Tips
    {
        public const Char TIPNOTSETVALUE = '-';
        public const Char TIPSETVALUE = '+';
    }

    /// <summary>
    /// The TUserTips class provides functions that read and write the statuses of
    /// 'User Tips'. User Tips are short messages can be shown anywhere in the UI
    /// (eg. using 'Balloon Tips' or 'Tip of the Day'-style Dialogs).
    ///
    /// The status of a certain User Tip (eg. already shown, not yet shown) is stored
    /// in a single character. The character is part of a single UserDefault of the
    /// User. This allows currently 250 User Tips statuses to be stored in a single
    /// User Default (because the s_default_value_c DB field can hold up to 250
    /// characters)
    /// A separate User Default for storing User Tips exists for each Petra Module
    /// (eg. 'MPartner_TipsState' for the Partner Module).
    /// A separate User Tips Enumeration exists for each Petra Module (eg. TMPartnerTips
    /// for the Partner Module). The items there provide the index at which
    /// character position the single character for the User Tip is stored.
    /// </summary>
    public class TUserTips
    {
        /**
         *     The TMPartner class provides access to User Tips for the Partner Module.
         *
         */
        public class TMPartner : object
        {
            private const String TIPSUSERDEFAULT = TUserDefaults.PARTNER_TIPS;

            #region TUserTips.TMPartner

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="ATip"></param>
            /// <param name="ASetViewed"></param>
            /// <returns></returns>
            public static Boolean CheckTipNotViewed(TMPartnerTips ATip, Boolean ASetViewed)
            {
                String TipsUserDefaultValue;

                TipsUserDefaultValue = GetTipsUserDefaultValue();
                return TUserTips.CheckTipNotViewed((byte)(ATip), TIPSUSERDEFAULT, TipsUserDefaultValue, ASetViewed);
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="ATip"></param>
            /// <returns></returns>
            public static Boolean CheckTipNotViewed(TMPartnerTips ATip)
            {
                return CheckTipNotViewed(ATip, false);
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="ATip"></param>
            /// <param name="ASetViewedStatus"></param>
            /// <returns></returns>
            public static Char CheckTipStatus(TMPartnerTips ATip, Char ASetViewedStatus)
            {
                String TipsUserDefaultValue;

                TipsUserDefaultValue = GetTipsUserDefaultValue();
                return TUserTips.CheckTipStatus((byte)(ATip), TIPSUSERDEFAULT, TipsUserDefaultValue, ASetViewedStatus);
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="ATip"></param>
            /// <returns></returns>
            public static Char CheckTipStatus(TMPartnerTips ATip)
            {
                return CheckTipStatus(ATip, (Char)0);
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="ATip"></param>
            /// <returns></returns>
            public static Boolean CheckTipViewed(TMPartnerTips ATip)
            {
                String TipsUserDefaultValue;

                TipsUserDefaultValue = GetTipsUserDefaultValue();
                return TUserTips.CheckTipViewed((byte)(ATip), TipsUserDefaultValue);
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="ATip"></param>
            /// <param name="ASetStatus"></param>
            public static void SetTipStatus(TMPartnerTips ATip, Char ASetStatus)
            {
                String TipsUserDefaultValue;

                TipsUserDefaultValue = GetTipsUserDefaultValue();
                TUserTips.SetTipStatus((Byte)(ATip), TIPSUSERDEFAULT, TipsUserDefaultValue, ASetStatus);
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="ATip"></param>
            public static void SetTipViewed(TMPartnerTips ATip)
            {
                SetTipStatus(ATip, Tips.TIPSETVALUE);
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <param name="ATip"></param>
            public static void SetTipNotViewed(TMPartnerTips ATip)
            {
                SetTipStatus(ATip, Tips.TIPNOTSETVALUE);
            }

            /// <summary>
            /// todoComment
            /// </summary>
            /// <returns></returns>
            public static String GetTipsUserDefaultValue()
            {
                String ReturnValue;

                ReturnValue = TUserTips.GetTipsUserDefaultValueInternal(TIPSUSERDEFAULT);

//        MessageBox.Show("TUserTips.TMPartner.GetTipsUserDefaultValue: " + ReturnValue  + "\r\n" +
//              Convert.ToString(ReturnValue).Length);

                return ReturnValue;
            }

            #endregion
        }

        private static Boolean CheckTipNotViewedInternal(Int16 ATipIndex, ref String ATipsUserDefault, Boolean ASetViewed)
        {
            Boolean ReturnValue;
            char TipValue;

            TipValue = GetTipValueInternal(ATipIndex, ATipsUserDefault);

            if (TipValue == Tips.TIPNOTSETVALUE)
            {
                ReturnValue = true;

                if (ASetViewed)
                {
                    SetTipValueInternal(ATipIndex, ref ATipsUserDefault, Tips.TIPSETVALUE);
                }
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        private static Boolean CheckTipNotViewedInternal(Int16 ATipIndex, ref String ATipsUserDefault)
        {
            return CheckTipNotViewedInternal(ATipIndex, ref ATipsUserDefault, false);
        }

        private static String GetTipsUserDefaultValueInternal(String ATipsUserDefault)
        {
            // MessageBox.Show('TUserTips.GetTipsUserDefaultValueInternal  ATipsUserDefault: ' + ATipsUserDefault);
            return TUserDefaults.GetStringDefault(ATipsUserDefault, new String(Tips.TIPNOTSETVALUE, 250));
        }

        private static Char GetTipValueInternal(Int16 ATipIndex, String ATipsUserDefault, Char ASetViewedStatus)
        {
            Char ReturnValue;

            try
            {
                ReturnValue = Convert.ToChar(ATipsUserDefault.Substring(ATipIndex, 1));
            }
            catch (ArgumentOutOfRangeException)
            {
                ReturnValue = Tips.TIPNOTSETVALUE;
            }
            catch (Exception)
            {
                throw;
            }

            // MessageBox.Show('TUserTips.GetTipValueInternal for Index ' + ATipIndex.ToString + ': ''' + Result.ToString + '''');
            if ((ASetViewedStatus != (char)(0)) && (ASetViewedStatus != ReturnValue))
            {
                // MessageBox.Show('TUserTips.GetTipValueInternal for Index ' + ATipIndex.ToString + ': ''' + Result.ToString + ''': setting Value');
                // Returned Value isn't what the caller expected, so set it to this Value
                SetTipValueInternal(ATipIndex, ref ATipsUserDefault, ASetViewedStatus);
            }

            return ReturnValue;
        }

        private static Char GetTipValueInternal(Int16 ATipIndex, String ATipsUserDefault)
        {
            return GetTipValueInternal(ATipIndex, ATipsUserDefault, (char)(0));
        }

        private static void SetTipValueInternal(Int16 ATipIndex, ref String ATipsUserDefault, Char ATipValue)
        {
            Int16 TipUserDefaultCurrentLen;
            String PadString;

            TipUserDefaultCurrentLen = (short)ATipsUserDefault.Length;

            // MessageBox.Show('TUserTips.SetTipValueInternal for Index ' + ATipIndex.ToString + ': ''' + ATipValue.ToString + '''');
            if (ATipIndex >= TipUserDefaultCurrentLen)
            {
                PadString = new String(Tips.TIPNOTSETVALUE, ATipIndex - TipUserDefaultCurrentLen + 1);

                // MessageBox.Show('TUserTips.SetTipValueInternal: ATipsUserDefault is too short: extending by ' + Convert.ToString(Length(PadString)) + ' characters...');
                ATipsUserDefault = ATipsUserDefault + PadString;

                // MessageBox.Show('TUserTips.SetTipValueInternal: new ATipsUserDefault after extending: '
                // + ATipsUserDefault  + "\r\n" +
                // Convert.ToString(Length(ATipsUserDefault)));
            }

            if (ATipIndex != 0)
            {
                ATipsUserDefault = ATipsUserDefault.Substring(0, ATipIndex) + ATipValue + ATipsUserDefault.Substring(ATipIndex + 1);
            }
            else
            {
                ATipsUserDefault = ATipValue + ATipsUserDefault.Substring(1, ATipsUserDefault.Length - 1);
            }

            // MessageBox.Show('TUserTips.SetTipValueInternal: updated ATipsUserDefault: ' +
            // ATipsUserDefault + "\r\n" +
            // Convert.ToString(Length(ATipsUserDefault)));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATipIndex"></param>
        /// <param name="ATipsUserDefault"></param>
        /// <param name="ATipsUserDefaultValue"></param>
        /// <param name="ASetViewed"></param>
        /// <returns></returns>
        public static Boolean CheckTipNotViewed(Int16 ATipIndex, String ATipsUserDefault, String ATipsUserDefaultValue, Boolean ASetViewed)
        {
            Boolean ReturnValue;

            // MessageBox.Show('TUserTips.CheckTipNotViewed for Index ' +
            // ATipIndex.ToString + '...');
            ReturnValue = CheckTipNotViewedInternal(ATipIndex, ref ATipsUserDefaultValue, ASetViewed);

            // MessageBox.Show('TUserTips.CheckTipNotViewed for Index ' +
            // ATipIndex.ToString + ' = ' + Result.ToString);
            if (ReturnValue && ASetViewed)
            {
                // MessageBox.Show('UserDefault has changed  updating.');
                TUserDefaults.SetDefault(ATipsUserDefault, ATipsUserDefaultValue);
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATipIndex"></param>
        /// <param name="ATipsUserDefault"></param>
        /// <param name="ATipsUserDefaultValue"></param>
        /// <returns></returns>
        public static Boolean CheckTipNotViewed(Int16 ATipIndex, String ATipsUserDefault, String ATipsUserDefaultValue)
        {
            return CheckTipNotViewed(ATipIndex, ATipsUserDefault, ATipsUserDefaultValue, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATipIndex"></param>
        /// <param name="ATipsUserDefault"></param>
        /// <param name="ATipsUserDefaultValue"></param>
        /// <param name="ASetViewedStatus"></param>
        /// <returns></returns>
        public static Char CheckTipStatus(Int16 ATipIndex, String ATipsUserDefault, String ATipsUserDefaultValue, Char ASetViewedStatus)
        {
            Char ReturnValue;

            // MessageBox.Show('TUserTips.CheckTipStatus for Index ' +
            // EnumValue.ToString + '...');
            ReturnValue = GetTipValueInternal(ATipIndex, ATipsUserDefaultValue, ASetViewedStatus);

            // MessageBox.Show('TUserTips.CheckTipStatus for Index ' +
            // EnumValue.ToString + ' = ' + Result.ToString);
            if ((ASetViewedStatus != (char)(0)) && (ASetViewedStatus != ReturnValue))
            {
                // MessageBox.Show('UserDefault has changed  updating.');
                TUserDefaults.SetDefault(ATipsUserDefault, ATipsUserDefaultValue);
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATipIndex"></param>
        /// <param name="ATipsUserDefault"></param>
        /// <param name="ATipsUserDefaultValue"></param>
        /// <returns></returns>
        public static Char CheckTipStatus(Int16 ATipIndex, String ATipsUserDefault, String ATipsUserDefaultValue)
        {
            return CheckTipStatus(ATipIndex, ATipsUserDefault, ATipsUserDefaultValue, (char)(0));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATipIndex"></param>
        /// <param name="ATipsUserDefault"></param>
        /// <returns></returns>
        public static Boolean CheckTipViewed(Int16 ATipIndex, String ATipsUserDefault)
        {
            return !CheckTipNotViewedInternal(ATipIndex, ref ATipsUserDefault, false);

            // MessageBox.Show('TUserTips.CheckTipViewed for Index ' + ATipIndex.ToString + ': ' + Result.ToString);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATipIndex"></param>
        /// <param name="ATipsUserDefault"></param>
        /// <param name="ATipsUserDefaultValue"></param>
        /// <param name="ASetStatus"></param>
        public static void SetTipStatus(Int16 ATipIndex, String ATipsUserDefault, String ATipsUserDefaultValue, Char ASetStatus)
        {
            // MessageBox.Show('TUserTips.SetTipStatus for Index ' +
            // EnumValue.ToString + ' to ''' + Convert.ToString(ASetStatus) + '''');
            SetTipValueInternal(ATipIndex, ref ATipsUserDefaultValue, ASetStatus);

            // MessageBox.Show('UserDefault has changed  updating.');
            TUserDefaults.SetDefault(ATipsUserDefault, ATipsUserDefaultValue);
        }
    }
}