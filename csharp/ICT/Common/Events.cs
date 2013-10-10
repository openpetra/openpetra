//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Windows.Forms;

namespace Ict.Common
{
    /// <summary>
    /// todoComment
    /// </summary>
    public enum TScreenPartEnum
    {
        /// <summary>
        /// todoComment
        /// </summary>
        spAll,

        /// <summary>
        /// todoComment
        /// </summary>
        spCounters
    };

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TEnableDisableScreenPartsEventHandler(System.Object Sender, TEnableDisableEventArgs e);

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TRecalculateScreenPartsEventHandler(System.Object Sender, TRecalculateScreenPartsEventArgs e);

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TShowTabEventHandler(System.Object Sender, TShowTabEventArgs e);

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TTabPageEventHandler(System.Object Sender, TTabPageEventArgs e);

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TValueChangedHandler(Control Sender);

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TDataSavingStartHandler(System.Object Sender, System.EventArgs e);

    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TDataSavedHandler(System.Object Sender, TDataSavedEventArgs e);

    /// <summary>
    /// todoComment
    /// </summary>
    public class TEnableDisableEventArgs : System.EventArgs
    {
        private Boolean FEnable;

        /// <summary>
        /// todoComment
        /// </summary>
        public Boolean Enable
        {
            get
            {
                return FEnable;
            }

            set
            {
                FEnable = value;
            }
        }


        #region TEnableDisableEventArgs

        /// <summary>
        /// todoComment
        /// </summary>
        public TEnableDisableEventArgs()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AEnable"></param>
        public TEnableDisableEventArgs(Boolean AEnable)
        {
            FEnable = AEnable;
        }

        #endregion
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TShowTabEventArgs : System.EventArgs
    {
        private String FTabName;
        private String FShowNextToTabName;
        private Boolean FShow;

        /// <summary>
        /// todoComment
        /// </summary>
        public String TabName
        {
            get
            {
                return FTabName;
            }

            set
            {
                FTabName = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public Boolean Show
        {
            get
            {
                return FShow;
            }

            set
            {
                FShow = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public String ShowNextToTabName
        {
            get
            {
                return FShowNextToTabName;
            }

            set
            {
                FShowNextToTabName = value;
            }
        }


        #region TShowTabEventArgs

        /// <summary>
        /// todoComment
        /// </summary>
        public TShowTabEventArgs()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATabName">todoComment</param>
        /// <param name="AShow">todoComment</param>
        /// <param name="AShowNextToTabName">todoComment</param>
        public TShowTabEventArgs(String ATabName, Boolean AShow, String AShowNextToTabName)
        {
            FTabName = ATabName;
            FShow = AShow;
            FShowNextToTabName = AShowNextToTabName;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATabName">todoComment</param>
        /// <param name="AShow">todoComment</param>
        public TShowTabEventArgs(String ATabName, Boolean AShow) : this(ATabName, AShow, "")
        {
        }

        #endregion
    }

    /// <summary>
    /// Event Arguments for the TTabPage Event
    /// </summary>
    public class TTabPageEventArgs : System.EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ATabPage">TabPage which this Event is about.</param>
        /// <param name="AEvent">Event which is happening for the TabPage.</param>
        public TTabPageEventArgs(TabPage ATabPage, string AEvent)
        {
            Tab = ATabPage;
            UserControlOnTabPage = null;
            Event = AEvent;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ATabPage">TabPage which this Event is about.</param>
        /// <param name="AUserControlOnTabPage">UserControl which makes up the content of the TabPage (if any).</param>
        /// <param name="AEvent">Event which is happening for the TabPage.</param>
        public TTabPageEventArgs(TabPage ATabPage, UserControl AUserControlOnTabPage, string AEvent)
        {
            Tab = ATabPage;
            UserControlOnTabPage = AUserControlOnTabPage;
            Event = AEvent;
        }

        /// <summary>
        /// TabPage which this Event is about.
        /// </summary>
        public TabPage Tab;

        /// <summary>
        /// UserControl which makes up the content of the TabPage (if any).
        /// </summary>
        public UserControl UserControlOnTabPage;

        /// <summary>
        /// Event which is happening for the TabPage.
        /// </summary>
        public string Event;
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TRecalculateScreenPartsEventArgs : System.EventArgs
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public TScreenPartEnum ScreenPart;
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TDataSavedEventArgs : System.EventArgs
    {
        private Boolean FSuccess;

        /// <summary>
        /// todoComment
        /// </summary>
        public Boolean Success
        {
            get
            {
                return FSuccess;
            }

            set
            {
                FSuccess = value;
            }
        }


        #region TDataSavedEventArgs

        /// <summary>
        /// todoComment
        /// </summary>
        public TDataSavedEventArgs()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASuccess"></param>
        public TDataSavedEventArgs(Boolean ASuccess)
        {
            FSuccess = ASuccess;
        }

        #endregion
    }
}