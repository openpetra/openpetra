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
using System.Runtime.Serialization;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Shared.MPartner
{
    #region EPartnerNotExistantException

    /// <summary>
    /// Throw this Exception when a Business Object is asked to load data for a Partner that does either not exist or is deleted, not active, etc.  whatever is applicable to the situation.
    /// </summary>
    [Serializable()]
    public class EPartnerNotExistantException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EPartnerNotExistantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public EPartnerNotExistantException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EPartnerNotExistantException() : base()
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EPartnerLocationKeyNotExistantException

    /// <summary>
    /// Can be thrown to signalise that a PPartnerLocation Key does not exist in the DB
    /// </summary>
    [Serializable()]
    public class EPartnerLocationNotExistantException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EPartnerLocationNotExistantException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public EPartnerLocationNotExistantException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EPartnerLocationNotExistantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EPartnerFamilyIDException

    /// <summary>
    /// Throw this exception to signalize that there is a problem in finding a family ID
    /// </summary>
    [Serializable()]
    public class EPartnerFamilyIDException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EPartnerFamilyIDException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public EPartnerFamilyIDException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public EPartnerFamilyIDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion
}