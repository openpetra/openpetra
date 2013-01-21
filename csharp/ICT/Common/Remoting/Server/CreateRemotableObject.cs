//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ict.Common;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// create a object that can be remoted to a client
    /// </summary>
    public class TCreateRemotableObject
    {
        /// <summary>
        /// create a object that can be remoted to a client
        /// </summary>
        public static object CreateRemotableObject(Type AInterfaceToImplement, Type ARemotableObject, ICrossDomainService ObjectToRemote)
        {
            // need to calculate the URI for this object and pass it to the new namespace object
            string ObjectURI = TConfigurableMBRObject.BuildRandomURI(ObjectToRemote.GetType().ToString());

            // we need to add the service in the main domain
            DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

            try
            {
                // create the object
                return Activator.CreateInstance(ARemotableObject, new object[] { ObjectURI });
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.ToString());
                throw;
            }
        }
    }
}