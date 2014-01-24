//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using Ict.Petra.Shared;

/* Copied from Petra 2.3 */

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// Implement this Interface in any base Form whose derived Forms
    /// should be able to receive 'Forms Messages'.
    /// </summary>
    /// <description>
    /// Implement <see cref="IFormsMessagingInterface"></see> as a
    /// 'virtual' Method of the base Form so that each derived Form can choose
    /// whether it wants to be able to receive 'Forms Messages' (by overriding
    /// the <see cref="ProcessFormsMessage"> Method</see>.
    /// </description>
    public interface IFormsMessagingInterface
    {
        /// <summary>
        /// This Method will be called by
        /// <see cref="Ict.Petra.Client.CommonForms.TFormsList.BroadcastFormMessage"></see>
        /// in each Form that is derived from a base Form that implements the
        /// <see cref="IFormsMessagingInterface"></see> and that is enlisted in the
        /// <see cref="Ict.Petra.Client.CommonForms.TFormsList.GFormsList"></see>.
        /// </summary>
        /// <param name="AFormsMessage">The 'Forms Message' that is being broadcast
        /// by some code that calls
        /// <see cref="Ict.Petra.Client.CommonForms.TFormsList.BroadcastFormMessage"></see>.</param>
        /// <returns>Returns True if the Form reacted on the specific Forms Message,
        /// otherwise false.</returns>
        bool ProcessFormsMessage(TFormsMessage AFormsMessage);
    }

    /// <summary>
    /// Base Interface for any specific Interfaces that are to be implemented
    /// in 'data-only Classes' (such as <see cref="TFormsMessage.FormsMessagePartner"></see>).
    /// </summary>
    public interface IFormsMessageClassInterface
    {
    }

    /// <summary>
    /// Interface for a 'data-only Class' that holds Partner Data (such as
    /// <see cref="TFormsMessage.FormsMessagePartner"></see>).
    /// </summary>
    public interface IFormsMessagePartnerInterface : IFormsMessageClassInterface
    {
        /// <summary>PartnerKey of the Partner in the 'Forms Message'.</summary>
        Int64 PartnerKey
        {
            get;
        }

        /// <summary>Partner Class of the Partner in the 'Forms Message'.</summary>
        TPartnerClass PartnerClass
        {
            get;
        }

        /// <summary>ShortName of the Partner in the 'Forms Message'.</summary>
        string ShortName
        {
            get;
        }

        /// <summary>Status of the Partner in the 'Forms Message'.</summary>
        string PartnerStatus
        {
            get;
        }
    }

    /// <summary>
    /// Specifies the MessageClass of a <see cref="TFormsMessage"></see>.
    /// </summary>
    /// <description>
    /// When a Form of the application chooses to listen to such messages, it can evaluate
    /// the <see cref="TFormsMessage.MessageClass"></see> to determine whether it is interested in that
    /// particular class of <see cref="TFormsMessage"></see>.</description>
    public enum TFormsMessageClassEnum
    {
        /// <summary>Newly created Partner that just got saved to the DB.</summary>
        mcNewPartnerSaved,

        /// <summary>Edited (already existing) Partner that just got saved to the DB.</summary>
        mcExistingPartnerSaved,

        /// <summary>Existing Partner that just got deleted.</summary>
        mcPartnerDeleted,

        /// <summary>Family Members of an already existing Partner of Partner Class FAMILY
        /// just got changed in the DB.</summary>
        mcFamilyMembersChanged
    }

    /// <summary>
    /// Instances of <see cref="TFormsMessage"></see> can be sent to all open Forms of the application
    /// who can choose to listen to such messages. Sending is done by calling
    /// <see cref="Ict.Petra.Client.CommonForms.TFormsList.BroadcastFormMessage"></see>.
    /// </summary>
    /// <description>
    /// When a Form of the application chooses to listen to such messages, it can evaluate
    /// the <see cref="MessageClass"></see> of the <see cref="TFormsMessage"></see>
    /// and the Typed Data they contain by getting a specific 'MessageObject' (a data-only Class)
    /// by reading the <see cref="MessageObject"></see> Property.
    /// </description>
    public class TFormsMessage
    {
        TFormsMessageClassEnum FMessageClass;
        IFormsMessageClassInterface FMessageObject;
        string FMessageContext = "";

        /// <summary>
        /// Constructor without a 'MessageContext'
        /// </summary>
        /// <param name="AMessageClass">Specifies the 'class' of the new instance of <see cref="TFormsMessage"></see>.</param>
        public TFormsMessage(TFormsMessageClassEnum AMessageClass)
        {
            FMessageClass = AMessageClass;
        }

        /// <summary>
        /// Constructor with a 'MessageContext'
        /// </summary>
        /// <param name="AMessageClass">Specifies the 'class' of the new instance of <see cref="TFormsMessage"></see>.</param>
        /// <param name="AMessageContext">An arbitrary string that allows 'Listeners' of Form Messages
        /// to find out if the 'Forms Message' they receive is exclusively for them.</param>
        public TFormsMessage(TFormsMessageClassEnum AMessageClass, string AMessageContext)
        {
            FMessageClass = AMessageClass;
            FMessageContext = AMessageContext;
        }

        /// <summary>
        /// Message class of the 'Forms Message'.
        /// </summary>
        /// <description>Message classes are used to distinguish between
        /// Messages that have different purposes.</description>
        public TFormsMessageClassEnum MessageClass
        {
            get
            {
                return FMessageClass;
            }
        }

        /// <summary>
        /// The Message Object that got created by calling one of the
        /// 'SetMessageData...' Methods.
        /// </summary>
        public IFormsMessageClassInterface MessageObject
        {
            get
            {
                if (FMessageObject != null)
                {
                    return FMessageObject;
                }
                else
                {
                    throw new ApplicationException("A MessageObject exists only after calling one of the 'SetMessageData...' Methods");
                }
            }
        }

        /// <summary>
        /// An arbitrary string that the instantiator of this Class can specify.
        /// </summary>
        /// <description>
        /// <para>There can be several 'Listeners' for 'Forms Messages'.
        /// These can than find out if the 'Forms Message' they receive is exclusively for
        /// them by inspecting the CallerContext of the received 'Form Message' and checking
        /// it against the original CallerContext passed in here.</para>
        /// <para>Note: GUIDs (Globally Unique IDentifier) are good CallerContexts because
        /// they are unique. Note: A GUID can be created by calling
        /// <see cref="System.Guid.NewGuid()"></see>.<see cref="Object.ToString()"></see>.</para>
        /// </description>
        public string MessageContext
        {
            get
            {
                return FMessageContext;
            }
        }

        /// <summary>
        /// Allows setting of Partner Data for 'Form Messages' of MessageClass
        /// <see cref="TFormsMessageClassEnum.mcNewPartnerSaved"></see>,
        /// <see cref="TFormsMessageClassEnum.mcExistingPartnerSaved"></see> and
        /// <see cref="TFormsMessageClassEnum.mcFamilyMembersChanged"></see>.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner in the 'Forms Message'.</param>
        /// <param name="APartnerClass">Partner Class of the Partner in the 'Forms Message'.</param>
        /// <param name="AShortName">ShortName of the Partner in the 'Forms Message'.</param>
        /// <param name="APartnerStatus">Status of the Partner in the 'Forms Message'.</param>
        public void SetMessageDataPartner(Int64 APartnerKey,
            TPartnerClass APartnerClass, string AShortName, string APartnerStatus)
        {
            switch (FMessageClass)
            {
                case TFormsMessageClassEnum.mcNewPartnerSaved:
                case TFormsMessageClassEnum.mcExistingPartnerSaved:
                case TFormsMessageClassEnum.mcFamilyMembersChanged:
                case TFormsMessageClassEnum.mcPartnerDeleted:

                    FMessageObject = new FormsMessagePartner(APartnerKey,
                    APartnerClass, AShortName, APartnerStatus);
                    break;

                default:
                    throw new ApplicationException(
                    "Method 'SetMessageDataPartner' must not be called for MessageClass '" +
                    Enum.GetName(typeof(TFormsMessageClassEnum), FMessageClass) + "'");
            }
        }

        /// <summary>
        /// A 'data-only' class for Partner Data for 'Form Messages' of MessageClass
        /// <see cref="TFormsMessageClassEnum.mcNewPartnerSaved"></see>,
        /// <see cref="TFormsMessageClassEnum.mcExistingPartnerSaved"></see> and
        /// <see cref="TFormsMessageClassEnum.mcFamilyMembersChanged"></see>.
        /// </summary>
        public class FormsMessagePartner : IFormsMessagePartnerInterface
        {
            Int64 FPartnerKey;
            TPartnerClass FPartnerClass;
            string FPartnerStatus;
            string FShortName;

            /// <summary>
            /// Constructor that initializes internal fields which can be
            /// read out by using the Properties of this Class.
            /// </summary>
            /// <param name="APartnerKey">PartnerKey of the Partner in the 'Forms Message'.</param>
            /// <param name="APartnerClass">Partner Class of the Partner in the 'Forms Message'.</param>
            /// <param name="AShortName">ShortName of the Partner in the 'Forms Message'.</param>
            /// <param name="APartnerStatus">Status of the Partner in the 'Forms Message'.</param>
            public FormsMessagePartner(Int64 APartnerKey,
                TPartnerClass APartnerClass,
                string AShortName, string APartnerStatus)
            {
                FPartnerKey = APartnerKey;
                FPartnerClass = APartnerClass;
                FShortName = AShortName;
                FPartnerStatus = APartnerStatus;
            }

            /// <summary>PartnerKey of the Partner in the 'Forms Message'.</summary>
            public Int64 PartnerKey
            {
                get
                {
                    return FPartnerKey;
                }
            }

            /// <summary>Partner Class of the Partner in the 'Forms Message'.</summary>
            public TPartnerClass PartnerClass
            {
                get
                {
                    return FPartnerClass;
                }
            }

            /// <summary>ShortName of the Partner in the 'Forms Message'.</summary>
            public string ShortName
            {
                get
                {
                    return FShortName;
                }
            }

            /// <summary>Status of the Partner in the 'Forms Message'.</summary>
            public string PartnerStatus
            {
                get
                {
                    return FPartnerStatus;
                }
            }
        }
    }
}