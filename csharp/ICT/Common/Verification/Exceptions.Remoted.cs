//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using System.Runtime.Serialization;

using Ict.Common.Exceptions;

// This Namespace contains Exceptions that can be passed from the Server to the Client
// via .NET Remoting.
//
// These Exceptions are OpenPetra-specific, but not specific to a certain
// OpenPetra Module (Partner, Finance, etc).
//
// Comment:
// Put remotable Exceptions which are specific to a certain Petra Module
// into shared Petra Module DLLs - eg Ict.Petra.Shared.MPartner, Ict.Petra.Shared.MFinance...

namespace Ict.Common.Verification.Exceptions
{
    #region EVerificationResultsException

    /// <summary>
    /// Can be hrown when TVerificationResultCollections hold one or more severe TVerificationResult item(s).
    /// </summary>
    [Serializable()]
    public class EVerificationResultsException : EOPAppException
    {
        /// <summary><see cref ="TVerificationResultCollection" /> that holds one or more severe TVerificationResult item(s).</summary>
        private TVerificationResultCollection FVerificationResults;


        /// <summary><see cref ="TVerificationResultCollection" /> that holds one or more severe TVerificationResult item(s).</summary>
        public TVerificationResultCollection VerificationResults
        {
            get
            {
                return FVerificationResults;
            }

            set
            {
                FVerificationResults = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EVerificationResultsException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EVerificationResultsException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EVerificationResultsException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message, access richt and Database Table.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AVerificationResults"><see cref ="TVerificationResultCollection" /> that holds one or more severe TVerificationResult item(s).</param>
        public EVerificationResultsException(String AMessage, TVerificationResultCollection AVerificationResults) : base(AMessage)
        {
            FVerificationResults = AVerificationResults;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message, access right and Database Table.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AVerificationResults"><see cref ="TVerificationResultCollection" /> that holds one or more severe TVerificationResult item(s).</param>
        /// /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EVerificationResultsException(String AMessage, TVerificationResultCollection AVerificationResults,
            Exception AInnerException) : base(AMessage, AInnerException)
        {
            FVerificationResults = AVerificationResults;
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EVerificationResultsException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
            FVerificationResults = (TVerificationResultCollection)AInfo.GetValue("VerificationResults", typeof(TVerificationResultCollection));
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            AInfo.AddValue("VerificationResults", FVerificationResults);

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion
}