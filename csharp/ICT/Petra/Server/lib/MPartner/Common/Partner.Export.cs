//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
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
using System.Data;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;
using Ict.Petra.Server.MHospitality.Data.Access;

namespace Ict.Petra.Server.MPartner.Import
{
    /// export all data of a partner
    public class TExportAllPartnerData
    {
        /// <summary>
        /// Load all the data of a partner into a TDS
        /// </summary>
        public static PartnerImportExportTDS ExportPartner(Int64 APartnerKey, TPartnerClass? APartnerClass = null)
        {
            PartnerImportExportTDS MainDS = new PartnerImportExportTDS();

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

                    // APartnerClass is optional but if it was not provided we need to assign to it now
                    if (APartnerClass == null)
                    {
                        APartnerClass = SharedTypes.PartnerClassStringToEnum(MainDS.PPartner[0].PartnerClass);
                    }

                    if (APartnerClass == TPartnerClass.CHURCH)
                    {
                        PChurchAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                    }
                    else if (APartnerClass == TPartnerClass.FAMILY)
                    {
                        PFamilyAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

                        PPartnerGiftDestinationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                    }
                    else if (APartnerClass == TPartnerClass.PERSON)
                    {
                        PPersonAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

                        PmPersonalDataAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                        PmPassportDetailsAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                        PmDocumentAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                        PmDocumentTypeAccess.LoadAll(MainDS, Transaction);
                        PmPersonQualificationAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                        PmSpecialNeedAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                        PmPastExperienceAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                        PmPersonLanguageAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                        PmPersonAbilityAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                        PmStaffDataAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                        PmJobAssignmentAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                        PmPersonEvaluationAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);

                        PmGeneralApplicationAccess.LoadViaPPersonPartnerKey(MainDS, APartnerKey, Transaction);
                        PtApplicationTypeAccess.LoadAll(MainDS, Transaction);
                        PmShortTermApplicationAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                        PmYearProgramApplicationAccess.LoadViaPPerson(MainDS, APartnerKey, Transaction);
                    }
                    else if (APartnerClass == TPartnerClass.ORGANISATION)
                    {
                        POrganisationAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                    }
                    else if (APartnerClass == TPartnerClass.UNIT)
                    {
                        PUnitAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

                        UmUnitStructureAccess.LoadViaPUnitChildUnitKey(MainDS, APartnerKey, Transaction);
                        UmUnitAbilityAccess.LoadViaPUnit(MainDS, APartnerKey, Transaction);
                        UmUnitLanguageAccess.LoadViaPUnit(MainDS, APartnerKey, Transaction);
                        UmUnitCostAccess.LoadViaPUnit(MainDS, APartnerKey, Transaction);
                        UmJobAccess.LoadViaPUnit(MainDS, APartnerKey, Transaction);
                        UmJobRequirementAccess.LoadViaPUnit(MainDS, APartnerKey, Transaction);
                        UmJobLanguageAccess.LoadViaPUnit(MainDS, APartnerKey, Transaction);
                        UmJobQualificationAccess.LoadViaPUnit(MainDS, APartnerKey, Transaction);
                    }
                    else if (APartnerClass == TPartnerClass.VENUE)
                    {
                        PVenueAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

                        PcBuildingAccess.LoadViaPVenue(MainDS, APartnerKey, Transaction);
                        PcRoomAccess.LoadViaPVenue(MainDS, APartnerKey, Transaction);
                    }

                    PPartnerAttributeAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);

                    PLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                    PPartnerLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);

                    PPartnerCommentAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                    PPartnerTypeAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                    PPartnerInterestAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                    PInterestAccess.LoadAll(MainDS, Transaction);
                });

            return MainDS;
        }
    }
}