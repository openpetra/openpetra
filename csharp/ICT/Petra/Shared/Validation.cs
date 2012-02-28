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
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ict.Petra.Shared
{
    /// <summary>
    /// Holds Controls who are involved in Data Validation.
    /// </summary>
    /// <remarks>In situations in which two Controls' values are compared in a
    /// Data Validation situation the <see cref="SecondValidationControl" /> and 
    /// <see cref="SecondValidationControlLabel" /> Properties need to be set to 
    /// the second of the two Controls and its Label, respectively.</remarks>
    public struct TValidationControlsData
    {
        private string FValidationControlLabel;
        private Control FValidationControl;
        private string FSecondValidationControlLabel;
        private Control FSecondValidationControl;
        
        /// <summary>
        /// Label text of the Windows Forms Control on which the validation is run.
        /// </summary>       
        public string ValidationControlLabel
        {
            get
            {
                return FValidationControlLabel;
            }
            
            set
            {
                FValidationControlLabel = value;
            }
        }
        
        /// <summary>
        /// Windows Forms Control on which the validation is run.
        /// </summary>
        public Control ValidationControl
        {
            get
            {
                return FValidationControl;
            }
            
            set
            {
                FValidationControl = value;
            }
        }
        
        /// <summary>
        /// Label text of the Second Windows Forms Control on which the validation is run.
        /// </summary>       
        public string SecondValidationControlLabel
        {
            get
            {
                return FSecondValidationControlLabel;
            }
            
            set
            {
                FSecondValidationControlLabel = value;
            }
        }
        
        /// <summary>
        /// Second Windows Forms Control on which the validation is run.
        /// </summary>
        public Control SecondValidationControl
        {
            get
            {
                return FSecondValidationControl;
            }
            
            set
            {
                FSecondValidationControl = value;
            }
        }       
        
        /// <summary>
        /// Constructor for Data Validation situations in which the value of one Control is validated.
        /// </summary>
        /// <param name="AValidationControl">Windows Forms Control on which the validation is run.</param>
        /// <param name="AValidationControlLabel">Label text of the Windows Forms Control on which the validation is run.</param>
        public TValidationControlsData(Control AValidationControl, string AValidationControlLabel)
        {
            FValidationControl = AValidationControl;
            FValidationControlLabel = AValidationControlLabel;
            FSecondValidationControl = null;
            FSecondValidationControlLabel = String.Empty;
        }
        
        /// <summary>
        /// Constructor for Data Validation situations in which two Controls' values are compared.
        /// </summary>
        /// <param name="AValidationControl">Windows Forms Control on which the validation is run.</param>
        /// <param name="AValidationControlLabel">Label text of the Windows Forms Control on which the validation is run.</param>
        /// <param name="ASecondValidationControl">Second Windows Forms Control on which the validation is run.</param>
        /// <param name="ASecondValidationControlLabel">Label text of the Second Windows Forms Control on which the validation is run.</param>
        public TValidationControlsData(Control AValidationControl, string AValidationControlLabel,
                                      Control ASecondValidationControl, string ASecondValidationControlLabel)
        {
            FValidationControl = AValidationControl;
            FValidationControlLabel = AValidationControlLabel;
            FSecondValidationControl = ASecondValidationControl;
            FSecondValidationControlLabel = ASecondValidationControlLabel;
        }
    }
    
    /// <summary>
    /// Dictionary for storing Controls whose values are getting validated.
    /// </summary>
    /// <remarks>The Keys of this Dictionary are of Type <see cref="DataColumn" /> and the Values 
    /// of this Dictionary are of Type <see cref="TValidationControlsData" />.</remarks>
    public class TValidationControlsDict : Dictionary<DataColumn, TValidationControlsData>
    {
        
    }
}