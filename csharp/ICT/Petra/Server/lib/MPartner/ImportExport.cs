/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Common;

namespace Ict.Petra.Server.MPartner.ImportExport.WebConnectors
{
    /// <summary>
    /// import and export partner data
    /// </summary>
    public class TImportExportWebConnector
    {
        private static void ParsePartners(ref PartnerEditTDS AMainDS, XmlNode ACurNode)
        {
            while (ACurNode != null)
            {
                if (ACurNode.Name == "PartnerGroup")
                {
                    ParsePartners(ref AMainDS, ACurNode.FirstChild);
                }
                else if (ACurNode.Name == "Partner")
                {
                    PPartnerRow newPartner = AMainDS.PPartner.NewRowTyped();

                    // get a new partner key
                    newPartner.PartnerKey = TNewPartnerKey.GetNewPartnerKey(Convert.ToInt64(TYml2Xml.GetAttributeRecursive(ACurNode, "SiteKey")));

                    if (TYml2Xml.GetAttributeRecursive(ACurNode, "class") == MPartnerConstants.PARTNERCLASS_FAMILY)
                    {
                        PFamilyRow newFamily = AMainDS.PFamily.NewRowTyped();
                        newFamily.PartnerKey = newPartner.PartnerKey;
                        newFamily.FamilyName = TYml2Xml.GetAttributeRecursive(ACurNode, "LastName");
                        newFamily.FirstName = TYml2Xml.GetAttribute(ACurNode, "FirstName");

                        newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
                        newPartner.PartnerShortName = newFamily.FamilyName + ", " + newFamily.FirstName;
                    }

                    if (TYml2Xml.GetAttributeRecursive(ACurNode, "class") == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        // TODO
                    }
                    else if (TYml2Xml.GetAttributeRecursive(ACurNode, "class") == MPartnerConstants.PARTNERCLASS_ORGANISATION)
                    {
                        // TODO
                    }
                    else
                    {
                        // TODO AVerificationResult add failing problem: unknown partner class
                    }
                }

                ACurNode = ACurNode.NextSibling;
            }
        }

        /// <summary>
        /// imports partner data from file
        /// </summary>
        /// <returns></returns>
        public static bool ImportPartners(string AXmlPartnerData, out TVerificationResultCollection AVerificationResult)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(AXmlPartnerData);

            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            // import partner groups
            // advantage: can inherit some common attributes, eg. partner class, etc

            ParsePartners(ref MainDS, root);

            TVerificationResultCollection VerificationResult;
            PartnerEditTDS InspectDS = MainDS.GetChangesTyped(true);

            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            AVerificationResult = null;

            if (InspectDS != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    if ((InspectDS.PPartner != null) && !PPartnerAccess.SubmitChanges(InspectDS.PPartner, SubmitChangesTransaction,
                            out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                    else if ((InspectDS.PFamily != null) && !PFamilyAccess.SubmitChanges(InspectDS.PFamily, SubmitChangesTransaction,
                                 out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                    else if ((InspectDS.PPerson != null) || PPersonAccess.SubmitChanges(InspectDS.PPerson, SubmitChangesTransaction,
                                 out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                    else
                    {
                        SubmissionResult = TSubmitChangesResult.scrOK;
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log("after submitchanges: exception " + e.Message);

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
            }

            return SubmissionResult == TSubmitChangesResult.scrOK;
        }
    }
}