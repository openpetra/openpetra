//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
//
// Contains classes for data verifications that are needed both on Server and
// Client side.
//
// @Comment None of the data verifications in here must access the database
//   since the Client doesn't have access to the database!
//
// @Comment NOTES on C# conversion: We dropped the 'Main' sub-namespace and put
//   the 'SetColumnErrorText' function into a new Class 'Data'.
//
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Ict.Common.Verification
{
    #region TResultSeverity

    /// <summary>
    /// a verification error can either be critical or non critical
    /// </summary>
    public enum TResultSeverity
    {
        /// <summary>
        /// the verification failed
        /// </summary>
        Resv_Critical,

        /// <summary>
        /// verification warning
        /// </summary>
        Resv_Noncritical,

        /// <summary>
        /// only a status message ...
        /// </summary>
        Resv_Status,

        /// <summary>
        /// purely information (without a warning connotation)
        /// </summary>
        Resv_Info
    };

    #endregion


    #region IResultInterface

    /// <summary>
    /// Properties that every 'Verification Result' needs to implement.
    /// </summary>
    public interface IResultInterface
    {
        /// <summary>
        /// Context of the Verification Result (where the Verification Result originated from).
        /// </summary>
        object ResultContext
        {
            get;
        }


        /// <summary>
        /// Text of the Verification Result.
        /// </summary>
        String ResultText
        {
            get;
        }


        /// <summary>
        /// Caption of the Verification Result (e.g. for use in MessageBox Titles).
        /// </summary>
        String ResultTextCaption
        {
            get;
        }


        /// <summary>
        /// ResultCode of the Verification Result.
        /// </summary>
        String ResultCode
        {
            get;
        }


        /// <summary>
        /// Severity of the Verification Result.
        /// </summary>
        TResultSeverity ResultSeverity
        {
            get;
        }
    }

    #endregion


    #region TVerificationResult

    /// <summary>
    /// A TVerificationResult object stores information about failed data
    /// verification and is passed (serialised) from the Server to the Client.
    /// It is made to be stored in the TVerificationResultCollection.
    /// </summary>
    [Serializable]
    public class TVerificationResult : IResultInterface
    {
        /// <summary>DB Field or other context that describes where the data verification failed (use '[ODBC ...]' instead to signal a database error (such as a failed call to a stored procedure)</summary>
        protected object FResultContext = String.Empty;

        /// <summary>Verification failure explanation</summary>
        protected String FResultText = String.Empty;

        /// <summary>Verification failure caption</summary>
        protected String FResultTextCaption = String.Empty;

        /// <summary>Error code if verification failure</summary>
        protected String FResultCode = String.Empty;

        /// <summary>Signals whether the verification failure prevented saving of data (critical) or the verification result is only for information purposes (noncritical).</summary>
        protected TResultSeverity FResultSeverity;

        /// <summary>
        /// Is true (or set to true) if the data validation code requests that the validated
        /// Control's value is undone. This is only useful for <see cref="TScreenVerificationResult" />s
        /// but needs to be declared in this Class to make the handling easier.
        /// </summary>
        protected bool FControlValueUndoRequested = false;

        /// <summary>
        /// Is true (or set to true) if the data validation code requests that the validation
        /// ToolTip should not be shown on the validated Control. This is only useful for <see cref="TScreenVerificationResult" />s
        /// but needs to be declared in this Class to make the handling easier.
        /// </summary>
        protected bool FSuppressValidationToolTip = false;

        /// <summary>
        /// Data Validation Run ID.
        /// </summary>
        protected System.Guid FDataValidationRunID;


        /// <summary>
        /// We need this constructor so that inherited Classes can get by not having a default constructor...
        /// </summary>
        protected internal TVerificationResult()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AResultContext">context where this verification happens (e.g. DB field name)</param>
        /// <param name="AErrorCodeInfo">An <see cref="ErrCodeInfo" /> that contains data which is used for populating the Verification Result's Properites.</param>
        /// <param name="ADataValidationRunID">A Data Validation Run ID that this instance should be associated with. Default: a new System.Guid instance.</param>
        public TVerificationResult(object AResultContext, ErrCodeInfo AErrorCodeInfo, System.Guid ADataValidationRunID = new System.Guid())
        {
            FResultContext = AResultContext;
            FResultCode = AErrorCodeInfo.ErrorCode;

            if (AErrorCodeInfo.ErrorMessageText == String.Empty)
            {
                FResultText = AErrorCodeInfo.ShortDescription;
            }
            else
            {
                FResultText = AErrorCodeInfo.ErrorMessageText;
            }

            if (AErrorCodeInfo.ErrorMessageTitle != String.Empty)
            {
                FResultTextCaption = AErrorCodeInfo.ErrorMessageTitle;
            }

            if ((AErrorCodeInfo.Category == ErrCodeCategory.Error)
                || (AErrorCodeInfo.Category == ErrCodeCategory.Validation))
            {
                FResultSeverity = TResultSeverity.Resv_Critical;
            }
            else if (AErrorCodeInfo.Category == ErrCodeCategory.NonCriticalError)
            {
                FResultSeverity = TResultSeverity.Resv_Noncritical;
            }

            FControlValueUndoRequested = AErrorCodeInfo.ControlValueUndoRequested;
            FDataValidationRunID = ADataValidationRunID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AResultContext">context where this verification happens (e.g. DB field name)</param>
        /// <param name="AResultText">Verification failure explanation</param>
        /// <param name="AResultSeverity">is this an error or just a warning</param>
        /// <param name="ADataValidationRunID">A Data Validation Run ID that this instance should be associated with. Default: a new System.Guid instance.</param>
        public TVerificationResult(object AResultContext,
            String AResultText,
            TResultSeverity AResultSeverity,
            System.Guid ADataValidationRunID = new System.Guid())
        {
            FResultContext = AResultContext;
            FResultText = AResultText;
            FResultSeverity = AResultSeverity;
            FDataValidationRunID = ADataValidationRunID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AResultContext">context where this verification happens (e.g. DB field name)</param>
        /// <param name="AResultText">Verification failure explanation</param>
        /// <param name="AResultCode">a result code to identify error messages</param>
        /// <param name="AResultSeverity">is this an error or just a warning</param>
        /// <param name="ADataValidationRunID">A Data Validation Run ID that this instance should be associated with. Default: a new System.Guid instance.</param>
        public TVerificationResult(object AResultContext,
            String AResultText,
            String AResultCode,
            TResultSeverity AResultSeverity,
            System.Guid ADataValidationRunID = new System.Guid())
        {
            FResultContext = AResultContext;
            FResultText = AResultText;
            FResultCode = AResultCode;
            FResultSeverity = AResultSeverity;
            FDataValidationRunID = ADataValidationRunID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AResultContext">context where this verification happens (e.g. DB field name)</param>
        /// <param name="AResultText">Verification failure explanation</param>
        /// <param name="AResultTextCaption">caption for message box</param>
        /// <param name="AResultCode">a result code to identify error messages</param>
        /// <param name="AResultSeverity">is this an error or just a warning</param>
        /// <param name="ADataValidationRunID">A Data Validation Run ID that this instance should be associated with. Default: a new System.Guid instance.</param>
        public TVerificationResult(String AResultContext,
            String AResultText,
            String AResultTextCaption,
            String AResultCode,
            TResultSeverity AResultSeverity,
            System.Guid ADataValidationRunID = new System.Guid())
        {
            FResultContext = AResultContext;
            FResultText = AResultText;
            FResultTextCaption = AResultTextCaption;
            FResultCode = AResultCode;
            FResultSeverity = AResultSeverity;
            FDataValidationRunID = ADataValidationRunID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>'Downgrades' a <see cref="TScreenVerificationResult" /> to a <see cref="TVerificationResult" />.</remarks>
        /// <param name="AScreenVerificationResult">A <see cref="TScreenVerificationResult" />.</param>
        public TVerificationResult(TScreenVerificationResult AScreenVerificationResult)
        {
            FResultContext = AScreenVerificationResult.ResultContext;
            FResultText = AScreenVerificationResult.ResultText;
            FResultTextCaption = AScreenVerificationResult.ResultTextCaption;
            FResultCode = AScreenVerificationResult.ResultCode;
            FResultSeverity = AScreenVerificationResult.ResultSeverity;
            FDataValidationRunID = AScreenVerificationResult.DataValidationRunID;
        }

        /// <summary>
        /// Context of the Verification Result (where the Verification Result originated from).
        /// </summary>
        /// <remarks>This Property cannot be written to in order to avoid accidental overwriting. However,
        /// by calling the Method <see cref="OverrideResultContext" /> the <see cref="ResultContext" />
        /// <em>can</em> be modified.</remarks>
        public object ResultContext
        {
            get
            {
                return FResultContext;
            }
        }

        /// <summary>
        /// Text of the Verification Result.
        /// </summary>
        /// <remarks>This Property cannot be written to in order to avoid accidental overwriting. However,
        /// by calling the Method <see cref="OverrideResultText" /> the <see cref="ResultText" />
        /// <em>can</em> be modified.</remarks>
        public String ResultText
        {
            get
            {
                return FResultText;
            }
        }

        /// <summary>
        /// Caption of the Verification Result (e.g. for use in MessageBox Titles).
        /// </summary>
        /// <remarks>This Property cannot be written to in order to avoid accidental overwriting. However,
        /// by calling the Method <see cref="OverrideResultTextCaption" /> the <see cref="ResultTextCaption" />
        /// <em>can</em> be modified.</remarks>
        public String ResultTextCaption
        {
            get
            {
                return FResultTextCaption;
            }
        }

        /// <summary>
        /// ResultCode of the Verification Result.
        /// </summary>
        public String ResultCode
        {
            get
            {
                return FResultCode;
            }

            set
            {
                FResultCode = value;
            }
        }

        /// <summary>
        /// Severity of the Verification Result.
        /// </summary>
        public TResultSeverity ResultSeverity
        {
            get
            {
                return FResultSeverity;
            }
        }

        /// <summary>
        /// Is true (or set to true) if the data validation code requests that the validated
        /// Control's value is undone.
        /// </summary>
        public bool ControlValueUndoRequested
        {
            get
            {
                return FControlValueUndoRequested;
            }

            set
            {
                FControlValueUndoRequested = value;
            }
        }

        /// <summary>
        /// Is true (or set to true) if the data validation code requests that the validation
        /// ToolTip should not be shown on the validated Control.
        /// </summary>
        public bool SuppressValidationToolTip
        {
            get
            {
                return FSuppressValidationToolTip;
            }

            set
            {
                FSuppressValidationToolTip = value;
            }
        }

        /// <summary>
        /// Data Validation Run ID.
        /// </summary>
        public System.Guid DataValidationRunID
        {
            get
            {
                return FDataValidationRunID;
            }

            set
            {
                FDataValidationRunID = value;
            }
        }

        /// <summary>
        /// Overrides the ResultContext that the <see cref="TVerificationResult" /> was
        /// originally populated with.
        /// </summary>
        /// <param name="ANewResultContext">New ResultText.</param>
        public void OverrideResultContext(object ANewResultContext)
        {
            FResultContext = ANewResultContext;
        }

        /// <summary>
        /// Overrides the ResultText that the <see cref="TVerificationResult" /> was
        /// originally populated with.
        /// </summary>
        /// <param name="ANewResultText">New ResultText.</param>
        public void OverrideResultText(string ANewResultText)
        {
            FResultText = ANewResultText;
        }

        /// <summary>
        /// Overrides the ResultTextCaption that the <see cref="TVerificationResult" /> was
        /// originally populated with.
        /// </summary>
        /// <param name="ANewResultTextCaption">New ResultTextCaption.</param>
        public void OverrideResultTextCaption(string ANewResultTextCaption)
        {
            FResultTextCaption = ANewResultTextCaption;
        }
    }

    #endregion


    #region TScreenVerificationResult

    /// <summary>
    /// A TScreenVerificationResult object stores information about failed data
    /// verification in a Form or UserControl on the Client side.
    /// It is made to be stored in the TVerificationResultCollection.
    /// </summary>
    public class TScreenVerificationResult : TVerificationResult, IResultInterface
    {
        /// <summary>
        /// specify the column where the error verification happened
        /// </summary>
        private DataColumn FResultColumn;

        /// <summary>
        /// specify the control that failed the verification
        /// </summary>
        private Control FResultControl;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AResultContext">context of verification</param>
        /// <param name="AResultColumn">which column failed</param>
        /// <param name="AResultText">description and error message for the user</param>
        /// <param name="AResultCode">error code to identify the error message</param>
        /// <param name="AResultControl">which control is involved</param>
        /// <param name="ADataValidationRunID">A Data Validation Run ID that this instance should be associated with. Default: a new System.Guid instance.</param>
        public TScreenVerificationResult(object AResultContext,
            DataColumn AResultColumn,
            String AResultText,
            String AResultCode,
            Control AResultControl,
            System.Guid ADataValidationRunID = new System.Guid())
        {
            FResultContext = AResultContext;
            FResultColumn = AResultColumn;
            FResultText = AResultText;
            FResultSeverity = TResultSeverity.Resv_Critical;
            FResultCode = AResultCode;
            FResultControl = AResultControl;
            FDataValidationRunID = ADataValidationRunID;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AResultContext">context of verification</param>
        /// <param name="AResultColumn">which column failed</param>
        /// <param name="AResultText">description and error message for the user</param>
        /// <param name="AResultControl">which control is involved</param>
        /// <param name="AResultSeverity">is this serious, or just a warning</param>
        /// <param name="ADataValidationRunID">A Data Validation Run ID that this instance should be associated with. Default: a new System.Guid instance.</param>
        public TScreenVerificationResult(object AResultContext,
            DataColumn AResultColumn,
            String AResultText,
            Control AResultControl,
            TResultSeverity AResultSeverity,
            System.Guid ADataValidationRunID = new System.Guid())
        {
            FResultContext = AResultContext;
            FResultColumn = AResultColumn;
            FResultText = AResultText;
            FResultSeverity = AResultSeverity;
            FResultControl = AResultControl;
            FDataValidationRunID = ADataValidationRunID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AResultContext">context of verification</param>
        /// <param name="AResultColumn">which column failed</param>
        /// <param name="AResultText">description and error message for the user</param>
        /// <param name="AResultCode">error code to identify the error message</param>
        /// <param name="AResultControl">which control is involved</param>
        /// <param name="AResultSeverity">is this serious, or just a warning</param>
        /// <param name="ADataValidationRunID">A Data Validation Run ID that this instance should be associated with. Default: a new System.Guid instance.</param>
        public TScreenVerificationResult(object AResultContext,
            DataColumn AResultColumn,
            String AResultText,
            String AResultCode,
            Control AResultControl,
            TResultSeverity AResultSeverity,
            System.Guid ADataValidationRunID = new System.Guid())
        {
            FResultContext = AResultContext;
            FResultColumn = AResultColumn;
            FResultText = AResultText;
            FResultCode = AResultCode;
            FResultControl = AResultControl;
            FResultSeverity = AResultSeverity;
            FDataValidationRunID = ADataValidationRunID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AResultContext">context of verification</param>
        /// <param name="AResultColumn">which column failed</param>
        /// <param name="AResultText">description and error message for the user</param>
        /// <param name="AResultCaption">caption for error message box</param>
        /// <param name="AResultCode">error code to identify the error message</param>
        /// <param name="AResultControl">which control is involved</param>
        /// <param name="AResultSeverity">is this serious, or just a warning</param>
        /// <param name="ADataValidationRunID">A Data Validation Run ID that this instance should be associated with. Default: a new System.Guid instance.</param>
        public TScreenVerificationResult(object AResultContext,
            DataColumn AResultColumn,
            String AResultText,
            String AResultCaption,
            String AResultCode,
            Control AResultControl,
            TResultSeverity AResultSeverity,
            System.Guid ADataValidationRunID = new System.Guid())
        {
            FResultContext = AResultContext;
            FResultColumn = AResultColumn;
            FResultText = AResultText;
            FResultTextCaption = AResultCaption;
            FResultCode = AResultCode;
            FResultControl = AResultControl;
            FResultSeverity = AResultSeverity;
            FDataValidationRunID = ADataValidationRunID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AVerificationResult"><see cref="TVerificationResult" /> which
        /// contains the basic data to which the <paramref name="AResultColumn" /> and
        /// <paramref name="AResultControl" /> are getting added.</param>
        /// <param name="AResultColumn">which column failed</param>
        /// <param name="AResultControl">which control is involved</param>
        /// <param name="ADataValidationRunID">A Data Validation Run ID that this instance should be associated with. Default: a new System.Guid instance.</param>
        public TScreenVerificationResult(TVerificationResult AVerificationResult,
            DataColumn AResultColumn, Control AResultControl, System.Guid ADataValidationRunID = new System.Guid())
        {
            FResultContext = AVerificationResult.ResultContext;
            FResultColumn = AResultColumn;
            FResultText = AVerificationResult.ResultText;
            FResultTextCaption = AVerificationResult.ResultTextCaption;
            FResultCode = AVerificationResult.ResultCode;
            FResultControl = AResultControl;
            FResultSeverity = AVerificationResult.ResultSeverity;
            FControlValueUndoRequested = AVerificationResult.ControlValueUndoRequested;
            FDataValidationRunID = ADataValidationRunID;
        }

        /// <summary>
        /// the DataColumn of the verification failure
        /// </summary>
        /// <returns></returns>
        public DataColumn ResultColumn
        {
            get
            {
                return FResultColumn;
            }
        }

        /// <summary>
        /// get the Control for the verification failure
        /// </summary>
        /// <returns></returns>
        public Control ResultControl
        {
            get
            {
                return FResultControl;
            }
        }
    }

    #endregion


    #region TVerificationResultCollection

    /// <summary>
    /// A TVerificationResultCollection object stores any number of TVerificationResult objects.
    /// With this typed Collection it is for instance possible to perform
    /// several data verification steps on the Server and pass the results back to
    /// the Client in one object.
    /// </summary>
    /// <remarks>
    /// NOTES on C# conversion:
    /// (1) The 'Item' method overloads have been renamed to 'FindBy' method
    /// overloads (couldn't do that as in Delphi.NET!);
    /// (2) The 'VerificationResultInfo' Indexed Property that we had in .NET has
    /// now become the Default Indexed Property of this Class because C# doesn't
    /// allow named Indexed Properties!
    /// </remarks>
    [Serializable]
    public class TVerificationResultCollection : CollectionBase
    {
        /// <summary>
        /// Control for which the first data validation error is recorded.
        /// </summary>
        /// <remarks>Updated when one of the overloads of Method
        /// <see cref="M:BuildScreenVerificationResultList(out String, out Control, out Object, bool, Type)" /> or
        /// <see cref="M:BuildScreenVerificationResultList(object, out String, out Control, bool)" /> is called,
        /// except if their Argument 'AUpdateFirstErrorControl' is set to false.</remarks>
        Control FFirstErrorControl = null;

        /// <summary>
        /// Should the Focus be set on the FirstErrorControl?
        /// </summary>
        bool FFocusOnFirstErrorControlRequested = false;

        /// <summary>
        /// Data Validation Run ID that this instance is associated with.
        /// </summary>
        System.Guid FCurrentDataValidationRunID;

        /// <summary>
        /// constructor
        /// </summary>
        public TVerificationResultCollection()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TVerificationResultCollection(System.Guid ACurrentDataValidationRunID)
        {
            FCurrentDataValidationRunID = ACurrentDataValidationRunID;
        }

        /// <summary>
        /// access the elements of the verification collection
        /// </summary>
        public IResultInterface this[int index]
        {
            get
            {
                return GetVerificationResult(index);
            }

            set
            {
                SetVerificationResult(index, value);
            }
        }

        /// <summary>
        /// Checks whether there are any <see cref="TVerificationResult" />s  in the collection that denote a
        /// critical error.
        /// </summary>
        /// <remarks>Does not check/count any <see cref="TVerificationResult" /> whose
        /// <see cref="TVerificationResult.ResultSeverity" /> </remarks> is <see cref="TResultSeverity.Resv_Noncritical" />
        /// or <see cref="TResultSeverity.Resv_Info" />.
        public bool HasCriticalErrors
        {
            get
            {
                foreach (TVerificationResult v in List)
                {
                    if (v.ResultSeverity == TResultSeverity.Resv_Critical)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Checks whether there are any <see cref="TVerificationResult" />s in the collection that denote a
        /// critical or non-critical error.
        /// </summary>
        /// <remarks>Does not check/count any <see cref="TVerificationResult" /> whose
        /// <see cref="TVerificationResult.ResultSeverity" /> </remarks> is <see cref="TResultSeverity.Resv_Info" />.
        public bool HasCriticalOrNonCriticalErrors
        {
            get
            {
                foreach (TVerificationResult v in List)
                {
                    if ((v.ResultSeverity == TResultSeverity.Resv_Critical)
                        || (v.ResultSeverity == TResultSeverity.Resv_Noncritical))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Control for which the first data validation error is recorded.
        /// </summary>
        /// <remarks>Updated when one of the overloads of Method
        /// <see cref="M:BuildScreenVerificationResultList(out String, out Control, out Object, bool, Type)" /> or
        /// <see cref="M:BuildScreenVerificationResultList(object, out String, out Control, bool)" /> is called,
        /// except if their Argument 'AUpdateFirstErrorControl' is set to false.</remarks>
        public Control FirstErrorControl
        {
            get
            {
                return FFirstErrorControl;
            }

            set
            {
                FFirstErrorControl = value;
            }
        }

        /// <summary>
        /// Inspect this from outside this Class to inquire whether the Focus should be set on the FirstErrorControl.
        /// </summary>
        public bool FocusOnFirstErrorControlRequested
        {
            get
            {
                return FFocusOnFirstErrorControlRequested;
            }

            set
            {
                FFocusOnFirstErrorControlRequested = value;
            }
        }

        /// <summary>
        /// Data Validation Run ID that this instance is associated with.
        /// </summary>
        public System.Guid CurrentDataValidationRunID
        {
            get
            {
                return FCurrentDataValidationRunID;
            }
        }

        /// <summary>
        /// Adds a new verification object.
        /// </summary>
        /// <param name="value">the verification object to be added (must not be null)</param>
        /// <returns></returns>
        public int Add(IResultInterface value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Adds a new verification object. Should the verification object be null,
        /// nothing happens.
        /// </summary>
        /// <param name="value">the verification object to be added (can be null)</param>
        /// <returns></returns>
        public int AddAndIgnoreNullValue(IResultInterface value)
        {
            int ReturnValue = -1;

            if (value != null)
            {
                ReturnValue = List.Add(value);
            }

            return ReturnValue;
        }

        /// <summary>
        /// merge another verification collection into the current collection
        /// </summary>
        /// <param name="value">collection to be merged</param>
        public void AddCollection(TVerificationResultCollection value)
        {
            if (value != null)
            {
                for (int Counter = 0; Counter <= value.Count - 1; Counter += 1)
                {
                    List.Add(value.GetVerificationResult(Counter));
                }
            }
        }

        /// <summary>
        /// Generates text for a MessageBox showing all verification errors that are held in the
        /// <see cref="TVerificationResultCollection" /> (optionally excluding some if the
        /// <paramref name="ARestrictToTypeWhichRaisesError" /> Argument is not null).
        /// </summary>
        /// <param name="AErrorMessages">String containing a formatted list of error messages that is taken from the
        /// <see cref="TVerificationResultCollection" />.</param>
        /// <param name="AFirstErrorControl">Control which the Focus should be set to as it is the first Control for which a
        /// Verification failure is recorded against.</param>
        /// <param name="AFirstErrorContext"><see cref="TVerificationResult.ResultContext" /> of the first error.</param>
        /// <param name="AUpdateFirstErrorControl">Set to false to not update the <see cref="FirstErrorControl" /> Property
        /// of this Class (defaults to true).</param>
        /// <param name="ARestrictToTypeWhichRaisesError">Restricts the <see cref="TVerificationResult" />s that
        /// are added to the result list to those whose <see cref="TVerificationResult.ResultContext" /> matches
        /// <paramref name="ARestrictToTypeWhichRaisesError"></paramref> (defaults to null).</param>
        /// <param name="AIgnoreWarnings">Set to true if Warnings are to be ignored (defaults to false).</param>
        public void BuildScreenVerificationResultList(out String AErrorMessages, out Control AFirstErrorControl, out object AFirstErrorContext,
            bool AUpdateFirstErrorControl = true, Type ARestrictToTypeWhichRaisesError = null, bool AIgnoreWarnings = false)
        {
            TVerificationResult si;
            TScreenVerificationResult siScr;
            bool IncludeVerificationResult;

            AFirstErrorControl = null;
            AErrorMessages = "";
            AFirstErrorContext = null;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TVerificationResult)(List[Counter]);

                if (ARestrictToTypeWhichRaisesError != null)
                {
                    if ((si.ResultContext.GetType() == ARestrictToTypeWhichRaisesError) || (si.ResultCode == CommonErrorCodes.ERR_DUPLICATE_RECORD))
                    {
                        IncludeVerificationResult = true;
                    }
                    else
                    {
                        IncludeVerificationResult = false;
                    }
                }
                else
                {
                    IncludeVerificationResult = true;
                }

                if (AIgnoreWarnings && (si.ResultSeverity == TResultSeverity.Resv_Noncritical))
                {
                    IncludeVerificationResult = false;
                }

                if (IncludeVerificationResult)
                {
                    AErrorMessages = AErrorMessages + si.ResultText;

                    if (si.ResultCode != String.Empty)
                    {
                        AErrorMessages += "  [" + si.ResultCode + "]";
                    }

                    AErrorMessages += Environment.NewLine + Environment.NewLine;

                    if (si is TScreenVerificationResult)
                    {
                        siScr = (TScreenVerificationResult)(List[Counter]);

                        if (AFirstErrorControl == null)
                        {
                            AFirstErrorControl = siScr.ResultControl;
                            AFirstErrorContext = siScr.ResultContext;

                            if (AUpdateFirstErrorControl)
                            {
                                FFirstErrorControl = AFirstErrorControl;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// generate the text for a message box showing all verification errors of a given context
        /// </summary>
        /// <param name="AResultContext">only show errors of the given context</param>
        /// <param name="AErrorMessages">will have the list of error messages</param>
        /// <param name="AFirstErrorControl">for focusing the first control that caused verification failure</param>
        /// <param name="AUpdateFirstErrorControl" >Set to false to not update the <see cref="FirstErrorControl" /> Property
        /// of this Class (defaults to true).</param>
        public void BuildScreenVerificationResultList(object AResultContext, out String AErrorMessages, out Control AFirstErrorControl,
            bool AUpdateFirstErrorControl = true)
        {
            TScreenVerificationResult si;

            AFirstErrorControl = null;
            AErrorMessages = "";

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultContext == AResultContext)
                {
                    AErrorMessages = AErrorMessages + si.ResultText + Environment.NewLine + Environment.NewLine;

                    if (AFirstErrorControl == null)
                    {
                        AFirstErrorControl = si.ResultControl;

                        if (AUpdateFirstErrorControl)
                        {
                            FFirstErrorControl = AFirstErrorControl;
                        }
                    }
                }
            }
        }

        private static readonly string StrErrorFooter = Catalog.GetString("{0}\r\n    Problem: {2}\r\n    (Severity: {1}, Code={3})");
        private static readonly string StrErrorNoCodeFooter = Catalog.GetString("{0}\r\n    Problem: {2}\r\n    (Severity: {1})");
        private static readonly string StrStatusFooter = Catalog.GetString("{0}\r\n    Status: {2}\r\n");

        /// <summary>
        /// Returns a formatted String that contains information about all
        /// <see cref="TVerificationResult" />s in the <see cref="TVerificationResultCollection" />.
        /// </summary>
        /// <returns>Formatted String that contains information about all
        /// <see cref="TVerificationResult" />s in the <see cref="TVerificationResultCollection" />.</returns>
        public string BuildVerificationResultString()
        {
            TVerificationResult si;
            string ReturnValue = String.Empty;

            for (int i = 0; i <= Count - 1; i += 1)
            {
                si = (TVerificationResult)(List[i]);
                String Status = "Info";
                String Formatter = StrStatusFooter; // For either Resv_Status or Resv_Info, this smaller message format is used.

                switch (si.ResultSeverity)
                {
                    case TResultSeverity.Resv_Critical:
                        Status = "Critical";
                        Formatter = StrErrorFooter;
                        break;

                    case TResultSeverity.Resv_Noncritical:
                        Status = "Non-critical";
                        Formatter = StrErrorFooter;
                        break;
                }

                if ((Formatter == StrErrorFooter) && (si.ResultCode == "")) // If no code was given, don't show the empty placeholder.
                {
                    Formatter = StrErrorNoCodeFooter;
                }

                ReturnValue = ReturnValue +
                              (String.Format(Formatter,
                                   new object[] { si.ResultContext, Status, si.ResultText, si.ResultCode })) +
                              Environment.NewLine + Environment.NewLine;
            }

            return ReturnValue;
        }

        /// <summary>
        /// check if the Verification list contains this value already
        /// </summary>
        /// <param name="value">check list for this value</param>
        /// <returns>true if the value is already there</returns>
        public bool Contains(IResultInterface value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// check if the verification list already contains a message from the given context
        /// </summary>
        /// <param name="AResultContext">check list for this value</param>
        /// <returns>true if the value is already there</returns>
        /// <returns>true if an error from this context is already there</returns>
        public bool Contains(object AResultContext)
        {
            IResultInterface si;
            Boolean Found = false;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TVerificationResult)(List[Counter]);

                if (si.ResultContext == AResultContext)
                {
                    Found = true;
                    break;
                }
            }

            return Found;
        }

        /// <summary>
        /// Checks if there is an error for this data column already.
        /// </summary>
        /// <param name="AResultColumn">The <see cref="System.Data.DataColumn" /> to check for.</param>
        /// <returns>true if such an error already is part of the list</returns>
        public bool Contains(DataColumn AResultColumn)
        {
            TScreenVerificationResult si;
            Boolean Found = false;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultColumn == AResultColumn)
                {
                    Found = true;
                    break;
                }
            }

            return Found;
        }

        /// <summary>
        /// Checks if there is an error for any data column of this DataTable already.
        /// </summary>
        /// <param name="ADataTable">The <see cref="System.Data.DataTable" /> to check for.</param>
        /// <returns>true if such an error already is part of the list</returns>
        public bool Contains(DataTable ADataTable)
        {
            TScreenVerificationResult si;
            Boolean Found = false;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if ((si.ResultColumn != null)
                    && (si.ResultColumn.Table == ADataTable))
                {
                    Found = true;
                    break;
                }
            }

            return Found;
        }

        private void ClassifyExceptions()
        {
            // If collection contains an ExceptionResult, remove all non-exceptions
            TScreenVerificationResult si;
            Boolean Found = false;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultCode == CommonErrorCodes.ERR_DUPLICATE_RECORD)
                {
                    Found = true;
                    break;
                }
            }

            if (!Found)
            {
                return;
            }

            for (int Counter = Count - 1; Counter >= 0; Counter--)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultCode != CommonErrorCodes.ERR_DUPLICATE_RECORD)
                {
                    RemoveAt(Counter);
                }
            }

            Found = false;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultColumn != null)
                {
                    Found = true;
                    break;
                }
            }

            if (!Found)
            {
                return;
            }

            for (int Counter = Count - 1; Counter >= 0; Counter--)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultColumn == null)
                {
                    RemoveAt(Counter);
                }
            }
        }

        /// <summary>
        /// Adds a <see cref="TVerificationResult" /> for a <see cref="System.Data.DataColumn" />
        /// specified with <paramref name="AResultColumn" />, or removes a
        /// <see cref="TVerificationResult" /> that is stored in the collection for the
        /// <see cref="System.Data.DataColumn" /> specified with <paramref name="AResultColumn" />.
        /// If <paramref name="AVerificationResult" /> isn't null, this Method will add it, if
        /// <paramref name="AVerificationResult" /> is null, this Method will remove *all* entries in
        /// the <see cref="TVerificationResultCollection" /> that are recorded for <paramref name="AResultColumn" />.
        /// </summary>
        /// <remarks>
        /// When adding a <see cref="TVerificationResult" />, a check is done if a <see cref="TVerificationResult" />
        /// with exactly the same Property values is already stored. If this is the case, the
        /// <see cref="TVerificationResult" /> is not added a second time.
        /// </remarks>
        /// <param name="AVerificationResult">The <see cref="TVerificationResult" /> to add,
        /// or null if a <see cref="TVerificationResult" /> that is stored in the collection for the
        /// <see cref="System.Data.DataColumn" /> <paramref name="AResultColumn" /> should get removed.</param>
        /// <param name="AResultColumn">The <see cref="System.Data.DataColumn" /> to check for.</param>
        /// <param name="AResultContext">Considered only when <paramref name="AVerificationResult"></paramref> is null:
        /// removal from collection will only happen if <paramref name="AResultContext"></paramref>.ToString() matches an
        /// <see cref="TVerificationResult" />'s ResultContext.ToString() that is stored in the collection for the
        /// <see cref="System.Data.DataColumn" />.  (Default: null.)</param>
        /// <param name="ATreatUserControlAndFormContextsAsIdentical">Set to true to treat a UserControl and the Form
        /// where it is placed on as identical ResultContexts. (Default: false.)</param>
        /// <returns>True if the <see cref="TVerificationResult" /> got added, otherwise false.</returns>
        public bool AddOrRemove(TVerificationResult AVerificationResult, DataColumn AResultColumn, object AResultContext = null,
            bool ATreatUserControlAndFormContextsAsIdentical = false)
        {
            List <TScreenVerificationResult>si = FindAllBy(AResultColumn);
            bool IdenticalVResultFound = false;
            bool ReturnValue = false;

            if (AVerificationResult != null)
            {
                if (si == null)
                {
                    // If CurrentDataValidationRunID was set on this instance, set it on any AVerificationResult we add so it can be associated with a "run" of the Data Validation.
                    if (CurrentDataValidationRunID != new System.Guid())
                    {
                        AVerificationResult.DataValidationRunID = CurrentDataValidationRunID;
                    }

                    this.Add(AVerificationResult);

                    ReturnValue = true;
                }
                else
                {
                    // A TVerificationResult for the same AResultColumn was found: inspect it.
                    foreach (TScreenVerificationResult SingleEntry in si)
                    {
                        if (TVerificationHelper.AreVerificationResultsIdentical(SingleEntry, AVerificationResult, false,
                                ATreatUserControlAndFormContextsAsIdentical))
                        {
                            // The existing TVerificationResult is identical to AVerificationResult:
                            // update the existing TVerificationResult's DataValidationRunID to the current one as it will need to be
                            // treated in further processing as if it was found in this run.
                            SingleEntry.DataValidationRunID = CurrentDataValidationRunID;

                            IdenticalVResultFound = true;

                            break;
                        }
                    }

                    if (!IdenticalVResultFound)
                    {
                        // The existing TVerificationResult is NOT identical to AVerificationResult:
                        // if CurrentDataValidationRunID was set on this instance of TVerificationResultColleciont, set it on any
                        // AVerificationResult we add so it can be associated with a 'run' of the Data Validation.
                        if (CurrentDataValidationRunID != new System.Guid())
                        {
                            AVerificationResult.DataValidationRunID = CurrentDataValidationRunID;
                        }

                        this.Add(AVerificationResult);

                        ReturnValue = true;
                    }
                }
            }
            else if (si != null)
            {
                foreach (TScreenVerificationResult SingleEntry in si)
                {
                    // Only remove a VerificationResult if it hasn't been added in this "run" of Data Validation
                    //  - This allows multiple Data Validations in one run on a single DataColumn as it prevents the last successful
                    //    Data Validation from removing a Data Validation error/warning for the same DataColumn that was enlisted in
                    //    the same "run".
                    if (SingleEntry.DataValidationRunID != this.CurrentDataValidationRunID)
                    {
                        if (AResultContext != null)
                        {
                            if (SingleEntry.ResultContext.ToString() == AResultContext.ToString())
                            {
                                this.Remove(SingleEntry);
                            }
                        }
                        else
                        {
                            this.Remove(SingleEntry);
                        }
                    }
                }
            }

            ClassifyExceptions();

            return ReturnValue;
        }

        /// <summary>
        /// Calls either the <see cref="Add" /> or <see cref="AddOrRemove" /> Method. Which Method is
        /// called is determined by evaluating the Type of <paramref name="AContext" />: if it is
        /// <see cref="System.Windows.Forms.Form" /> then the <see cref="AddOrRemove" /> Method is
        /// called, otherwise the <see cref="Add" /> Method.
        /// </summary>
        /// <param name="AContext">Context that describes where the data verification failed.</param>
        /// <param name="AVerificationResult">An instance of <see cref="TVerificationResult" />
        /// that is to be added/to be added or removed from the <see cref="TVerificationResultCollection" />.</param>
        /// <param name="AValidationColumn">The <see cref="DataRow" /> which holds the the data against which
        /// the validation was run. Only used if the <see cref="AddOrRemove" /> Methods is called.</param>
        /// <param name="ATreatUserControlAndFormContextsAsIdentical">Set to true to treat a UserControl and the Form
        /// where it is placed on as identical ResultContexts. (Default: false.)</param>
        /// <returns>True if the <see cref="TVerificationResult" /> got added, otherwise false.</returns>
        public bool Auto_Add_Or_AddOrRemove(object AContext, TVerificationResult AVerificationResult, DataColumn AValidationColumn,
            bool ATreatUserControlAndFormContextsAsIdentical = false)
        {
            bool ReturnValue = false;

            if ((AContext is System.Windows.Forms.Form)
                || (AContext is System.Windows.Forms.UserControl))
            {
                ReturnValue = this.AddOrRemove(AVerificationResult, AValidationColumn, null, ATreatUserControlAndFormContextsAsIdentical);
            }
            else
            {
                if (AVerificationResult != null)
                {
                    this.Add(AVerificationResult);
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// access result by index
        /// </summary>
        /// <param name="Index">which result should be returned</param>
        /// <returns>the selected result</returns>
        public IResultInterface GetVerificationResult(int Index)
        {
            return (TVerificationResult)List[Index];
        }

        /// <summary>
        /// assign a specified verification result in the list
        /// </summary>
        /// <param name="Index">index to change the verification result</param>
        /// <param name="Value">the new value</param>
        public void SetVerificationResult(int Index, IResultInterface Value)
        {
            List[Index] = Value;
        }

        /// <summary>
        /// find the index of the given value
        /// </summary>
        /// <param name="value">value to look for</param>
        /// <returns>index of the value</returns>
        public int IndexOf(IResultInterface value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// insert a new value at the given position
        /// </summary>
        /// <param name="index">position to insert after</param>
        /// <param name="value">value to add</param>
        public void Insert(int index, IResultInterface value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Find a <see cref="TScreenVerificationResult" /> by ResultColumn.
        /// </summary>
        /// <param name="AResultColumn">ResultColumn to look for.</param>
        /// <returns>The first result for that ResultColumn, or null if no result was found.</returns>
        public TScreenVerificationResult FindBy(DataColumn AResultColumn)
        {
            TScreenVerificationResult ReturnValue;
            TScreenVerificationResult si;

            ReturnValue = null;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultColumn == AResultColumn)
                {
                    ReturnValue = si;
                    break;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Finds all <see cref="TScreenVerificationResult" />s that are stored for a certain ResultColumn.
        /// </summary>
        /// <param name="AResultColumn">ResultColumn to look for.</param>
        /// <returns>An List of <see cref="TScreenVerificationResult" /> that contains all the found
        /// <see cref="TScreenVerificationResult" />s, or null if no result was found.</returns>
        public List <TScreenVerificationResult>FindAllBy(DataColumn AResultColumn)
        {
            List <TScreenVerificationResult>ReturnValue = null;
            TScreenVerificationResult si;

            ReturnValue = null;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultColumn == AResultColumn)
                {
                    if (ReturnValue == null)
                    {
                        ReturnValue = new List <TScreenVerificationResult>();
                    }

                    ReturnValue.Add(si);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Finds all <see cref="TScreenVerificationResult" />s that are stored for any data column of this DataTable already.
        /// </summary>
        /// <param name="ADataTable">The <see cref="System.Data.DataTable" /> to check for.</param>
        /// <returns>An List of <see cref="TScreenVerificationResult" /> that contains all the found
        /// <see cref="TScreenVerificationResult" />s, or null if no result was found.</returns>
        public List <TScreenVerificationResult>FindAllBy(DataTable ADataTable)
        {
            List <TScreenVerificationResult>ReturnValue = null;
            TScreenVerificationResult si;

            ReturnValue = null;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultColumn.Table == ADataTable)
                {
                    if (ReturnValue == null)
                    {
                        ReturnValue = new List <TScreenVerificationResult>();
                    }

                    ReturnValue.Add(si);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Find a <see cref="TScreenVerificationResult" /> by ResultContext
        /// </summary>
        /// <param name="AResultContext">context to look for</param>
        /// <returns>the first result for that context</returns>
        public IResultInterface FindBy(object AResultContext)
        {
            IResultInterface ReturnValue;
            IResultInterface si;

            ReturnValue = null;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (IResultInterface)(List[Counter]);

                if (si.ResultContext == AResultContext)
                {
                    ReturnValue = si;
                    break;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the <see cref="TScreenVerificationResult" /> that is found at the index position.
        /// </summary>
        /// <param name="index">Tndex to identify the <see cref="TScreenVerificationResult" />.</param>
        /// <returns>The <see cref="TScreenVerificationResult" /> at the index position.</returns>
        public IResultInterface FindBy(int index)
        {
            return (IResultInterface)(List[index]);
        }

        /// <summary>
        /// Type checking events
        /// </summary>
        /// <returns>void</returns>
        private new void OnInsert(int index, object value)
        {
            VerifyType(value);
        }

        private new void OnSet(int index, object oldValue, object newValue)
        {
            VerifyType(newValue);
        }

        private new void OnValidate(object value)
        {
            VerifyType(value);
        }

        /// <summary>
        /// remove a result from the list
        /// </summary>
        /// <param name="value">value to delete</param>
        public void Remove(IResultInterface value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// remove a result from the list, specified by the column
        /// </summary>
        /// <param name="AResultColumn">the column identifying the result</param>
        public void Remove(DataColumn AResultColumn)
        {
            TScreenVerificationResult si;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                if (si.ResultColumn == AResultColumn)
                {
                    List.Remove(si);
                    break;
                }
            }
        }

        /// <summary>
        /// remove a result identified by the column name
        /// </summary>
        /// <param name="AResultColumnName">column name</param>
        public void Remove(String AResultColumnName)
        {
            TScreenVerificationResult si;

            try
            {
                for (int Counter = 0; Counter <= Count - 1; Counter += 1)
                {
                    si = (TScreenVerificationResult)(List[Counter]);

                    if (si.ResultColumn.ColumnName == AResultColumnName)
                    {
                        List.Remove(si);
                        break;
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show("Exception occured in TVerificationResultCollection.Remove(AResultColumnName): " +
                    Exp.ToString());
            }
        }

        /// <summary>
        /// remove a result identified by the context
        /// </summary>
        /// <param name="AResultContext">the context</param>
        public void Remove(object AResultContext)
        {
            TScreenVerificationResult si;
            ArrayList siarray = new ArrayList();

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);

                // MessageBox.Show(si.ResultColumn.ToString + ' is found at array count ' + Counter.ToString);
                if (si.ResultContext == AResultContext)
                {
                    // MessageBox.Show('Length(siarray): ' + Convert.ToInt16(Length(siarray)).ToString);
                    siarray.Add(si);

                    // MessageBox.Show('Added ' + si.ResultColumn.ToString + ' at array count ' + Counter.ToString);
                }
            }

            for (int Counter2 = 0; Counter2 <= siarray.Count - 1; Counter2 += 1)
            {
                // MessageBox.Show('List.Remove of ' + siarray[Counter2].ResultColumn.ToString + ' at array count ' + Counter2.ToString);
                List.Remove((TScreenVerificationResult)siarray[Counter2]);
            }
        }

        private void VerifyType(object value)
        {
            if (!(value is IResultInterface))
            {
                throw new ArgumentException("Invalid Type");
            }
        }

        /// <summary>
        /// Downgrades all <see cref="TScreenVerificationResult" /> items in a <see cref="TVerificationResultCollection" />
        /// to <see cref="TVerificationResult" /> items.
        /// </summary>
        /// <param name="AScreenVerificationResults">A <see cref="TVerificationResultCollection" /> holding <em>exclusively</em>
        /// <see cref="TScreenVerificationResult" /> items.</param>
        public static void DowngradeScreenVerificationResults(TVerificationResultCollection AScreenVerificationResults)
        {
            int NumberOfVerificationResults = AScreenVerificationResults.Count;
            int NumberOfDowngradedVerificationResults = 0;

            for (int Counter1 = 0; Counter1 < NumberOfVerificationResults; Counter1++)
            {
                if (AScreenVerificationResults[Counter1] is TScreenVerificationResult)
                {
                    AScreenVerificationResults.Add(new TVerificationResult((TScreenVerificationResult)AScreenVerificationResults[Counter1]));
                    NumberOfDowngradedVerificationResults++;
                }
            }

            for (int Counter2 = 0; Counter2 < NumberOfDowngradedVerificationResults; Counter2++)
            {
                AScreenVerificationResults.RemoveAt(0);
            }
        }

        /// <summary>
        /// Records a new Data Validation Run. All TVerificationResults/TScreenVerificationResults that are created
        /// during this 'run' are associated with this 'run' through a unique System.Guid.
        /// </summary>
        public void RecordNewDataValidationRun()
        {
            FCurrentDataValidationRunID = System.Guid.NewGuid();
        }
    }

    #endregion


    #region TVerificationHelper

    /// <summary>
    /// Helper Methods for dealing with <see cref="TVerificationResult" />s.
    /// </summary>
    public static class TVerificationHelper
    {
        /// <summary>
        /// Checks whether two <see cref="TVerificationResult" />s are completely identical. The comparison
        /// takes all the data they hold into consideration.
        /// </summary>
        /// <param name="AVerificationResult1">First <see cref="TVerificationResult" />.</param>
        /// <param name="AVerificationResult2">Second <see cref="TVerificationResult" />.</param>
        /// <param name="ACompareResultContextsAsStrings">Set to true to compare the ResultContexts not as objects, but
        /// compare what a call of the .ToString() Method on the two object yields. (Default: false.)</param>
        /// <param name="ATreatUserControlAndFormContextsAsIdentical">Set to true to treat a UserControl and the Form
        /// where it is placed on as identical ResultContexts. (Default: false.)</param>
        /// <returns>True if the two <see cref="TVerificationResult" />s are completely identical,
        /// otherwise false.</returns>
        public static bool AreVerificationResultsIdentical(TVerificationResult AVerificationResult1, TVerificationResult AVerificationResult2,
            bool ACompareResultContextsAsStrings = false, bool ATreatUserControlAndFormContextsAsIdentical = false)
        {
            if ((AVerificationResult1 == null)
                || (AVerificationResult2 == null))
            {
                if ((AVerificationResult1 == null)
                    && (AVerificationResult2 == null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (AVerificationResult1.ResultCode != AVerificationResult2.ResultCode)
                {
                    return false;
                }

                if (!ACompareResultContextsAsStrings)
                {
                    if (ATreatUserControlAndFormContextsAsIdentical)
                    {
                        if ((AVerificationResult1.ResultContext is UserControl)
                            || (AVerificationResult2.ResultContext is UserControl))
                        {
                            if ((AVerificationResult1.ResultContext is UserControl)
                                && (AVerificationResult2.ResultContext is Form))
                            {
                                if (((UserControl)AVerificationResult1.ResultContext)
                                    != AVerificationResult2.ResultContext)
                                {
                                    return false;
                                }
                            }
                            else if ((AVerificationResult2.ResultContext is UserControl)
                                     && (AVerificationResult1.ResultContext is Form))
                            {
                                if (((UserControl)AVerificationResult2.ResultContext).ParentForm
                                    != AVerificationResult1.ResultContext)
                                {
                                    return false;
                                }
                            }
                        }
                        else if (AVerificationResult1.ResultContext != AVerificationResult2.ResultContext)
                        {
                            return false;
                        }
                    }
                    else if (AVerificationResult1.ResultContext != AVerificationResult2.ResultContext)
                    {
                        return false;
                    }
                }
                else
                {
                    if (ATreatUserControlAndFormContextsAsIdentical)
                    {
                        if ((AVerificationResult1.ResultContext is UserControl)
                            || (AVerificationResult2.ResultContext is UserControl))
                        {
                            if ((AVerificationResult1.ResultContext is UserControl)
                                && (AVerificationResult2.ResultContext is Form))
                            {
                                if (((UserControl)AVerificationResult1.ResultContext).ParentForm.ToString()
                                    != AVerificationResult2.ResultContext.ToString())
                                {
                                    return false;
                                }
                            }
                            else if ((AVerificationResult2.ResultContext is UserControl)
                                     && (AVerificationResult1.ResultContext is Form))
                            {
                                if (((UserControl)AVerificationResult2.ResultContext).ParentForm.ToString()
                                    != AVerificationResult1.ResultContext.ToString())
                                {
                                    return false;
                                }
                            }
                        }
                        else if (AVerificationResult1.ResultContext.ToString() != AVerificationResult2.ResultContext.ToString())
                        {
                            return false;
                        }
                    }
                    else if (AVerificationResult1.ResultContext.ToString() != AVerificationResult2.ResultContext.ToString())
                    {
                        return false;
                    }
                }

                if (AVerificationResult1.ResultSeverity != AVerificationResult2.ResultSeverity)
                {
                    return false;
                }

                if (AVerificationResult1.ResultText != AVerificationResult2.ResultText)
                {
                    // There may be some specific result codes that can allow the result text to differ so long as everything else is ok.
                    // This will happen when the result text changes with the user's INPUT (examples of this are rare!)
                    // Normally result text is either constant or depends on the current database content which will be constant during a verification run.
                    // But, for example, the result text for a duplicate record contains a hint indicating the entered data that might be wrong.
                    // So if the user makes the same error twice but with two different attempts at making a non-duplicate, we need to accept the result
                    //    as the same but update the old text, replacing it with the latest.
                    // Other examples of this behaviour may be created in the future, and can be OR'd with the ERR_DUPLICATE_RECORD case
                    if (AVerificationResult1.ResultCode == CommonErrorCodes.ERR_DUPLICATE_RECORD)
                    {
                        // ensure that the ResultText is the most recent instance
                        AVerificationResult1.OverrideResultText(AVerificationResult2.ResultText);
                    }
                    else
                    {
                        return false;
                    }
                }

                if (AVerificationResult1.ResultTextCaption != AVerificationResult2.ResultTextCaption)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a string that contains the data of all the <see cref="TVerificationResult" />s in the Collection.
        /// </summary>
        /// <returns>
        /// String that contains the data of all the <see cref="TVerificationResult" />s in the Collection. The
        /// data of the <see cref="TVerificationResult" />s are separated by <see cref="System.Environment.NewLine" />s.
        /// </returns>
        public static string FormatVerificationCollectionItems(TVerificationResultCollection AVerifColl)
        {
            const String PRINTFORMAT = "ResultContext: {0}, ResultText: {1}, ResultTextCaption: {2}, ResultCode {3}, ResultSeverity: {4}.";
            string ReturnValue = String.Empty;

            TVerificationResult si;

            for (int i = 0; i <= AVerifColl.Count - 1; i += 1)
            {
                si = (TVerificationResult)(AVerifColl[i]);

                ReturnValue = ReturnValue + String.Format(PRINTFORMAT,
                    si.ResultContext, si.ResultText, si.ResultTextCaption, si.ResultCode,
                    si.ResultSeverity) + Environment.NewLine;
            }

            if (ReturnValue != String.Empty)
            {
                // Remove trailing Environment.NewLine
                ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - Environment.NewLine.Length);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Calls the <see cref="M:DataRow.SetColumnError(DataColumn, String)" /> Method of a
        /// DataRow's Column to the  <see cref="TVerificationResult.ResultText" /> Property
        /// of the passed in <see cref="TVerificationResult" />.
        /// </summary>
        /// <param name="AEventArgs">An instance of DataColumnChangeEventArgs.</param>
        /// <param name="AVerificationResultEntry"><see cref="TVerificationResult" /> which has
        /// its <see cref="TVerificationResult.ResultText" /> Property set.</param>
        /// <param name="AControlName">Name of the Control to which the <see cref="TVerificationResult" />
        /// is related.</param>
        /// <param name="AResetValue">Set this to true to retain the
        /// <see cref="DataColumnChangeEventArgs.ProposedValue" />.</param>
        public static void SetColumnErrorText(DataColumnChangeEventArgs AEventArgs,
            TVerificationResult AVerificationResultEntry,
            String AControlName,
            Boolean AResetValue)
        {
            object PreviousProposedValue = AEventArgs.ProposedValue;

            AEventArgs.Row.SetColumnError(AEventArgs.Column,
                AVerificationResultEntry.ResultText + "//[[" + AControlName + "]]");

            /*
             * SetColumnError automatically resets the value of the DataColumn.
             * Therefore we need to re-assign it again if we don't want to reset the value.
             */
            if (!AResetValue)
            {
                //            MessageBox.Show("SetColumnErrorText: Before resetting the value: " + AEventArgs.ProposedValue.ToString());
                AEventArgs.ProposedValue = PreviousProposedValue;
                //            MessageBox.Show("SetColumnErrorText: After resetting the value: " + AEventArgs.ProposedValue.ToString());
            }
        }
    }

    #endregion


    #region TVerificationException

    /// <summary>
    /// This exception transports the error message and if the reason was another exception
    /// to the end of the routine. ResultCollection unpacks this data into a
    /// TVerificationResultCollection object, so that the user gets this message on the
    /// "normal" message box.
    /// </summary>
    public class TVerificationException : Exception
    {
        /// <summary>
        /// Constructor with inner exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public TVerificationException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner exception
        /// </summary>
        /// <param name="message"></param>
        public TVerificationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Property to handle (transport) the error code
        /// </summary>
        public string ErrorCode = String.Empty;

        /// <summary>
        /// Property to handle (transport) the error context
        /// </summary>
        public string Context = String.Empty;

        /// <summary>
        /// A Method to transform the exception message(s) into a
        /// TVerificationResultCollection
        /// </summary>
        /// <returns></returns>
        public TVerificationResultCollection ResultCollection()
        {
            TVerificationResultCollection collection =
                new TVerificationResultCollection();
            TVerificationResult avrEntry;

            avrEntry = new TVerificationResult(this.Context,
                this.Message, "",
                this.ErrorCode,
                TResultSeverity.Resv_Critical);
            collection.Add(avrEntry);
            avrEntry = new TVerificationResult(Catalog.GetString("Exception has been thrown"),
                this.ToString(), "",
                this.ErrorCode,
                TResultSeverity.Resv_Critical);
            collection.Add(avrEntry);

            if (this.InnerException != null)
            {
                avrEntry = new TVerificationResult(Catalog.GetString("Inner Exception"),
                    this.InnerException.ToString(),
                    TResultSeverity.Resv_Critical);
                collection.Add(avrEntry);
            }

            return collection;
        }
    }

    #endregion
}