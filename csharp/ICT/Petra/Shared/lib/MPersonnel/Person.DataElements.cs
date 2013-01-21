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

namespace Ict.Petra.Shared.MPersonnel.Person
{
    /// <summary>
    /// Individual Data Items.
    /// </summary>
    /// <remarks>Displayed in the Partner Edit screen, on the 'Personnel Data' Tab Group, 'Indidivual Data' Tab.</remarks>
    public enum TIndividualDataItemEnum
    {
        /// <summary>
        /// The Summary page (initially displayed)
        /// </summary>
        idiSummary,

        /// <summary>Personal Data</summary>
        idiPersonalData,

        /// <summary>Emergency Data</summary>
        idiEmergencyData,

        /// <summary>Passport Details</summary>
        idiPassportDetails,

        /// <summary>Personal Documents</summary>
        idiPersonalDocuments,

        /// <summary>Special Needs</summary>
        idiSpecialNeeds,

        /// <summary>Local Personnel Data</summary>
        idiLocalPersonnelData,

        /// <summary>Professional Areas</summary>
        idiProfessionalAreas,

        /// <summary>Personal Languages</summary>
        idiPersonalLanguages,

        /// <summary>Abilities</summary>
        idiPersonalAbilities,

        /// <summary>Previous Experience</summary>
        idiPreviousExperiences,

        /// <summary>Commitments</summary>
        idiCommitmentPeriods,

        /// <summary>Job Assignments</summary>
        idiJobAssignments,

        /// <summary>Progress Reports (Person Evaluations)</summary>
        idiProgressReports,

        /// <summary>Person Skills</summary>
        idiPersonSkills,

        /// <summary>Applications</summary>
        idiApplications
    }
}