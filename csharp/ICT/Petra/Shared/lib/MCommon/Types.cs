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
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;

namespace Ict.Petra.Shared.MCommon
{
    /// <summary>
    /// enumeration of values for items that can be associated with Office specific data labels
    /// </summary>
    public enum TOfficeSpecificDataLabelUseEnum
    {
        /// <summary>
        /// partner of class person
        /// </summary>
        Person,

        /// <summary>
        /// partner of class family
        /// </summary>
        Family,

        /// <summary>
        /// partner of class church
        /// </summary>
        Church,

        /// <summary>
        /// partner of class organisation
        /// </summary>
        Organisation,

        /// <summary>
        /// partner of class unit
        /// </summary>
        Unit,

        /// <summary>
        /// partner of class bank
        /// </summary>
        Bank,

        /// <summary>
        /// partner of class venue
        /// </summary>
        Venue,

        /// <summary>
        /// personnel module
        /// </summary>
        Personnel,

        /// <summary>
        /// long term application
        /// </summary>
        LongTermApp,

        /// <summary>
        /// short term application
        /// </summary>
        ShortTermApp
    };

    /// <summary>
    /// collection of static methods that are useful for the types defined in MCommon
    /// </summary>
    public class MCommonTypes
    {
        /// <summary>
        /// get the appropriate enum value for a given partner class
        /// </summary>
        /// <param name="APartnerClass">the partner class</param>
        /// <returns>the appropriate enum value</returns>
        public static TOfficeSpecificDataLabelUseEnum PartnerClassEnumToOfficeSpecificDataLabelUseEnum(TPartnerClass APartnerClass)
        {
            switch (APartnerClass)
            {
                case TPartnerClass.PERSON:
                    return TOfficeSpecificDataLabelUseEnum.Person;

                case TPartnerClass.FAMILY:
                    return TOfficeSpecificDataLabelUseEnum.Family;

                case TPartnerClass.CHURCH:
                    return TOfficeSpecificDataLabelUseEnum.Church;

                case TPartnerClass.ORGANISATION:
                    return TOfficeSpecificDataLabelUseEnum.Organisation;

                case TPartnerClass.BANK:
                    return TOfficeSpecificDataLabelUseEnum.Bank;

                case TPartnerClass.UNIT:
                    return TOfficeSpecificDataLabelUseEnum.Unit;

                case TPartnerClass.VENUE:
                    return TOfficeSpecificDataLabelUseEnum.Venue;
            }

            throw new ArgumentException("Cannot convert the submitted value of TPartnerClass." + APartnerClass.ToString(
                    "G") + " to an TOfficeSpecificDataLabelUseEnum!");
        }

        /// <summary>
        /// this returns the appropriate partner class for a given office specific data label enum value
        /// </summary>
        /// <param name="AOfficeSpecificDataLabelUse">enum value</param>
        /// <returns>partner class for that enum value</returns>
        public static TPartnerClass OfficeSpecificDataLabelUseToPartnerClassEnum(TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            switch (AOfficeSpecificDataLabelUse)
            {
                case TOfficeSpecificDataLabelUseEnum.Person:
                    return TPartnerClass.PERSON;

                case TOfficeSpecificDataLabelUseEnum.Family:
                    return TPartnerClass.FAMILY;

                case TOfficeSpecificDataLabelUseEnum.Church:
                    return TPartnerClass.CHURCH;

                case TOfficeSpecificDataLabelUseEnum.Organisation:
                    return TPartnerClass.ORGANISATION;

                case TOfficeSpecificDataLabelUseEnum.Unit:
                    return TPartnerClass.UNIT;

                case TOfficeSpecificDataLabelUseEnum.Bank:
                    return TPartnerClass.BANK;

                case TOfficeSpecificDataLabelUseEnum.Venue:
                    return TPartnerClass.VENUE;

                default:
                    throw new ArgumentException(
                    "Cannot convert the submitted value of TOfficeSpecificDataLabelUseEnum." + AOfficeSpecificDataLabelUse.ToString(
                        "G") + " to a TPartnerClass!");
            }
        }

        /// <summary>
        /// convert a partner class given as a string to an enum value
        /// </summary>
        /// <param name="APartnerClass">class written as string</param>
        /// <returns>the correct enum for that partner class</returns>
        public static TOfficeSpecificDataLabelUseEnum PartnerClassStringToOfficeSpecificDataLabelUseEnum(String APartnerClass)
        {
            if (APartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
            {
                return TOfficeSpecificDataLabelUseEnum.Person;
            }
            else if (APartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                return TOfficeSpecificDataLabelUseEnum.Family;
            }
            else if (APartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.CHURCH))
            {
                return TOfficeSpecificDataLabelUseEnum.Church;
            }
            else if (APartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.ORGANISATION))
            {
                return TOfficeSpecificDataLabelUseEnum.Organisation;
            }
            else if (APartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.BANK))
            {
                return TOfficeSpecificDataLabelUseEnum.Bank;
            }
            else if (APartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.UNIT))
            {
                return TOfficeSpecificDataLabelUseEnum.Unit;
            }
            else if (APartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.VENUE))
            {
                return TOfficeSpecificDataLabelUseEnum.Venue;
            }

            throw new ArgumentException("Cannot convert the submitted PartnerClass '" + APartnerClass + "' to a TOfficeSpecificDataLabelUseEnum!");
        }
    }

    /// object that will be serialized to the client.
    /// it opens a new channel for each new object.
    /// this is needed for cross domain marshalling.
    [Serializable]
    public class TAsynchronousExecutionProgressRemote : IAsynchronousExecutionProgress
    {
        private IAsynchronousExecutionProgress RemoteObject = null;
        private string FObjectURI;
        /// constructor
        public TAsynchronousExecutionProgressRemote(string AObjectURI)
        {
            FObjectURI = AObjectURI;
        }

        private void InitRemoteObject()
        {
            RemoteObject = (IAsynchronousExecutionProgress)
                           TConnector.TheConnector.GetRemoteObject(FObjectURI,
                typeof(IAsynchronousExecutionProgress));
        }

        /// forward the property
        public TAsyncExecProgressState ProgressState
        {
            get
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ProgressState;
            }
            set
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.ProgressState = value;
            }
        }
        /// forward the property
        public string ProgressInformation
        {
            get
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ProgressInformation;
            }
            set
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.ProgressInformation = value;
            }
        }
        /// forward the property
        public Int16 ProgressPercentage
        {
            get
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ProgressPercentage;
            }
            set
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.ProgressPercentage = value;
            }
        }
        /// forward the property
        public object Result
        {
            get
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.Result;
            }
            set
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.Result = value;
            }
        }
        /// forward the method call
        public void ProgressCombinedInfo(out TAsyncExecProgressState ProgressState, out Int16 ProgressPercentage, out String ProgressInformation)
        {
            if (RemoteObject == null)
            {
                InitRemoteObject();
            }

            RemoteObject.ProgressCombinedInfo(out ProgressState, out ProgressPercentage, out ProgressInformation);
        }

        /// forward the method call
        public void Cancel()
        {
            if (RemoteObject == null)
            {
                InitRemoteObject();
            }

            RemoteObject.Cancel();
        }
    }
}