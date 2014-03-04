//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2014 by OM International
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// This helper class is used by all the classes auto-generated from templates.
    /// It deals with single record deletion and contains the code to display the references check results to the user.
    /// The user has several options for what to do if references are found.
    /// The class only has one method and no member variables.
    /// 
    /// The class is NOT used when the user selects multiple records for deletion, because that does not result in additional user options.
    /// </summary>
    public class TCascadingReferenceCountHandler
    {
        /// <summary>
        /// The main method to run a reference check on a single item of data
        /// </summary>
        /// <param name="APetraUtilsObject">The calling forms PetraUtils Object</param>
        /// <param name="AVerificationResults">The results collection to check</param>
        /// <param name="ALimitedCount">Will be true if the reference count call was a limited check</param>
        /// <returns>A message box result.  Yes implies doing a new unlimited count, Undefined implies there are no references so deletion can proceed.
        /// Any other value implies that references exist</returns>
        public TFrmExtendedMessageBox.TResult HandleReferences(TFrmPetraEditUtils APetraUtilsObject, TVerificationResultCollection AVerificationResults, bool ALimitedCount)
        {
            Form MyForm = APetraUtilsObject.GetForm();

            // There were reference(s)
            TRowReferenceInfo info = (TRowReferenceInfo)AVerificationResults[0].ResultContext;
            bool bIncomplete = info.CascadingCountEndedEarly;

            // Build up a message string
            string msgContent = Messages.BuildMessageFromVerificationResult(
                    MCommonResourcestrings.StrRecordCannotBeDeleted +
                    Environment.NewLine +
                    Catalog.GetPluralString(MCommonResourcestrings.StrReasonColon, MCommonResourcestrings.StrReasonsColon, AVerificationResults.Count),
                    AVerificationResults);

            TFrmExtendedMessageBox.TButtons buttons = TFrmExtendedMessageBox.TButtons.embbOK;
            TFrmExtendedMessageBox.TDefaultButton defButton = TFrmExtendedMessageBox.TDefaultButton.embdDefButton1;
            if (bIncomplete && ALimitedCount)
            {
                msgContent += String.Format(MCommonResourcestrings.StrCountTerminatedEarly1,
                    Environment.NewLine,
                    APetraUtilsObject.MaxReferenceCountOnDelete);
                msgContent += MCommonResourcestrings.StrCountTerminatedEarly2;
                msgContent += MCommonResourcestrings.StrCountTerminatedEarly3;
                msgContent += String.Format(MCommonResourcestrings.StrCountTerminatedEarly4,
                    Environment.NewLine,
                    MyForm.Text);
                buttons = TFrmExtendedMessageBox.TButtons.embbYesNo;
                defButton = TFrmExtendedMessageBox.TDefaultButton.embdDefButton2;
            }
            else
            {
                if (bIncomplete)
                {
                    // We should never get an incomplete count on an unlimited check
                }

                msgContent += String.Format(MCommonResourcestrings.StrCountTerminatedEarlyOK, Environment.NewLine);
            }

            // Show an Extended Message Box and return the value
            TFrmExtendedMessageBox extendedMsgBox = new TFrmExtendedMessageBox(MyForm);
            
            return extendedMsgBox.ShowDialog(
                msgContent,
                MCommonResourcestrings.StrRecordDeletionTitle,
                String.Empty,
                buttons,
                defButton,
                TFrmExtendedMessageBox.TIcon.embiInformation);
        }
    }
}
