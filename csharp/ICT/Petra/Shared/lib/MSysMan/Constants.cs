//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//       timop
//
// Copyright 2004-2010 by OM International
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

namespace Ict.Petra.Shared.MSysMan
{
    /// <summary>
    /// some constants used in the system manager module
    /// </summary>
    public class MSysManConstants
    {
        /// <summary>
        /// ------------------------------------------------------------------------------
        /// Petra General User Default Constants
        /// </summary>
        public const String PETRA_DISPLAYMODULEBACKGRDPICTURE = "DisplayPicture";

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

        /// <summary>string constant</summary>
        public const String USERDEFAULT_NUMBEROFRECENTPARTNERS = "NumberOfRecentPartners";

        /// <summary>todoComment</summary>
        public const String USERDEFAULT_LASTPARTNERMAILROOM = "MailroomLastPerson";

        /// <summary>todoComment</summary>
        public const String USERDEFAULT_LASTPERSONPERSONNEL = "PersonnelLastPerson";

        /// <summary>todoComment</summary>
        public const String USERDEFAULT_LASTUNITPERSONNEL = "PersonnelLastUnit";

        /// <summary>todoComment</summary>
        public const String USERDEFAULT_LASTPERSONCONFERENCE = "ConferenceLastPerson";

        /// <summary>the UI should be translated to this language</summary>
        public const String USERDEFAULT_UILANGUAGE = "UILanguage";

        /// <summary>the dates and numbers should be formatted with this culture</summary>
        public const String USERDEFAULT_UICULTURE = "UICulture";

        /// <summary>key name for language</summary>
        public const String PARTNER_LANGUAGE = "p_language";

        /// <summary>key name for acquisition</summary>
        public const String PARTNER_ACQUISITION = "p_acquisition";

        /// <summary>column name for last and first name combined field in UserList</summary>
        public const String USER_LAST_AND_FIRST_NAME_COLUMNNAME = "LastAndFirstName";

        /// <summary>todoComment</summary>
        public const String CONFERENCE_LASTCONFERENCEWORKEDWITH = "LastConferenceWorkedWith";

        /// <summary>
        /// ------------------------------------------------------------------------------
        /// Reporting User Default Constants
        /// </summary>
        public const String FINANCE_REPORTING_SHOWDIFFFINANCIALYEARSELECTION = "ShowDiffFinancialYearSelection";

        /*------------------------------------------------------------------------------
         *  EMail Constants
         * -------------------------------------------------------------------------------*/

        /// <summary>todoComment</summary>
        public const String EMAIL_USER_LOGIN_NAME = "IUSROPEMAIL";

        /*------------------------------------------------------------------------------
         *      Put other User Default Constants here as well.
         * -------------------------------------------------------------------------------*/
    }
}