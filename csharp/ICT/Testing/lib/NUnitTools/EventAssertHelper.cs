//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2017 by OM International
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
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

using NUnit.Framework;
using NUnit.Framework.Constraints;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;

namespace Ict.Testing.NUnitTools
{
    /// <summary>
    /// To be used in connection with Class <see cref="TNUnitEventHandlerCheck{T}" /> for
    /// easy asserting of Events.
    /// </summary>
    public static class TNUnitEventAsserter
    {
        /// <summary>
        /// Convenient way of writing an Assert that checks whether an Event got raised, or not.
        /// </summary>
        /// <param name="AEventCapturer"><see cref="TNUnitEventHandlerCheck{T}" /> instance.</param>
        /// <param name="AEventTest">Event test that should be executed.</param>
        /// <param name="ACodeToRun">Code that is to be executed that should/shouldn't raise the Event.</param>
        public static void Assert <TEventArgs>(TNUnitEventHandlerCheck <TEventArgs>AEventCapturer,
            Action <TNUnitEventHandlerCheck <TEventArgs>>AEventTest,
            Action ACodeToRun) where TEventArgs : EventArgs
        {
            AEventCapturer.ResetCheck();

            ACodeToRun();

            AEventTest(AEventCapturer);
        }

        /// <summary>
        /// Use this Method to check whether the Event in question got raised.
        /// </summary>
        /// <returns><see cref="Assert" /> that is used as input for the <see cref="Assert" /> Method.</returns>
        public static Action <TNUnitEventHandlerCheck <TEventArgs>>GotRaised <TEventArgs>() where TEventArgs : EventArgs
        {
            return (TNUnitEventHandlerCheck <TEventArgs>Check) =>
                   {
                       NUnit.Framework.Assert.That(Check.EventGotRaised, Is.True);
                   };
        }

        /// <summary>
        /// Use this Method to check whether the Event in question did not get raised.
        /// </summary>
        /// <returns><see cref="Assert" /> that is used as input for the <see cref="Assert" /> Method.</returns>
        public static Action <TNUnitEventHandlerCheck <TEventArgs>>DidNotGetRaised <TEventArgs>() where TEventArgs : EventArgs
        {
            return (TNUnitEventHandlerCheck <TEventArgs>Check) =>
                   {
                       NUnit.Framework.Assert.That(Check.EventGotRaised, Is.False);
                   };
        }
    }

    /// <summary>
    /// To be used with NUnit to faciltate checking of Events - if they are getting
    /// raised and if they aren't getting raised.
    /// </summary>
    /// <remarks>To be used in connection with Class <see cref="TNUnitEventAsserter" />.</remarks>
    public class TNUnitEventHandlerCheck <TEventArgs>where TEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TNUnitEventHandlerCheck()
        {
            this.ResetCheck();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sender of the Event.
        /// </summary>
        public object Sender
        {
            get;

            private set;
        }

        /// <summary>
        /// EventArgs of the Event.
        /// </summary>
        public TEventArgs EventArgs
        {
            get;

            private set;
        }

        /// <summary>
        /// True if the Event got raised, false if it did not get raised.
        /// </summary>
        public bool EventGotRaised
        {
            get;

            private set;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Use this to re-use the same instance of TNUnitEventHandlerCheck for multiple checks.
        /// </summary>
        public void ResetCheck()
        {
            Sender = null;
            EventArgs = null;
            EventGotRaised = false;
        }

        /// <summary>
        /// Subscriber/consumer of the Event that should be tested.
        /// </summary>
        /// <remarks>Example: MyWidget.Firing += {your instance of TNUnitEventHandlerCheck}.Handler</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Handler(object sender, TEventArgs e)
        {
            EventGotRaised = true;
            Sender = sender;
            EventArgs = e;
        }

        #endregion
    }
}
