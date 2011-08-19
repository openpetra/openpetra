//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.IO;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Printing;
using Ict.Common.Verification;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MConference.Applications
{
    /// <summary>for each medical incident</summary>
    public class TMedicalIncident
    {
        /// <summary>identification number</summary>
        public int ID {
            get; set;
        }
        /// <summary>date of incident</summary>
        public DateTime Date {
            get; set;
        }
        /// <summary>medical person doing the examination/treatment</summary>
        public string Examiner {
            get; set;
        }
        /// <summary>pulse</summary>
        public string Pulse {
            get; set;
        }
        /// <summary>blood pressure</summary>
        public string BloodPressure {
            get; set;
        }
        /// <summary>temperature</summary>
        public string Temperature {
            get; set;
        }
        /// <summary>what is the problem</summary>
        public string Diagnosis {
            get; set;
        }
        /// <summary>what has been done or will be done</summary>
        public string Therapy {
            get; set;
        }
        /// <summary>keywords for reports</summary>
        public string Keywords {
            get; set;
        }

        /// <summary>constructor</summary>
        public TMedicalIncident(int AID)
        {
            this.ID = AID;
            this.Date = DateTime.Now;
        }

        /// <summary>constructor</summary>
        public TMedicalIncident(int AID, DateTime ADate, string AExaminer,
            string APulse,
            string ABloodPressure,
            string ATemperature,
            string ADiagnosis,
            string ATherapy,
            string AKeywords)
        {
            this.ID = AID;
            this.Date = ADate;
            this.Examiner = AExaminer;
            this.Pulse = APulse;
            this.BloodPressure = ABloodPressure;
            this.Temperature = ATemperature;
            this.Diagnosis = ADiagnosis;
            this.Therapy = ATherapy;
            this.Keywords = AKeywords;
        }
    }
}