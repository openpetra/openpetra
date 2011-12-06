//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
        public TVerificationResult(object AResultContext, ErrCodeInfo AErrorCodeInfo)
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
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AResultContext">context where this verification happens (e.g. DB field name)</param>
        /// <param name="AResultText">Verification failure explanation</param>
        /// <param name="AResultSeverity">is this an error or just a warning</param>
        public TVerificationResult(object AResultContext, String AResultText, TResultSeverity AResultSeverity)
        {
            FResultContext = AResultContext;
            FResultText = AResultText;
            FResultSeverity = AResultSeverity;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AResultContext">context where this verification happens (e.g. DB field name)</param>
        /// <param name="AResultText">Verification failure explanation</param>
        /// <param name="AResultCode">a result code to identify error messages</param>
        /// <param name="AResultSeverity">is this an error or just a warning</param>
        public TVerificationResult(object AResultContext, String AResultText, String AResultCode, TResultSeverity AResultSeverity)
        {
            FResultContext = AResultContext;
            FResultText = AResultText;
            FResultCode = AResultCode;
            FResultSeverity = AResultSeverity;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AResultContext">context where this verification happens (e.g. DB field name)</param>
        /// <param name="AResultText">Verification failure explanation</param>
        /// <param name="AResultTextCaption">caption for message box</param>
        /// <param name="AResultCode">a result code to identify error messages</param>
        /// <param name="AResultSeverity">is this an error or just a warning</param>
        public TVerificationResult(String AResultContext,
            String AResultText,
            String AResultTextCaption,
            String AResultCode,
            TResultSeverity AResultSeverity)
        {
            FResultContext = AResultContext;
            FResultText = AResultText;
            FResultTextCaption = AResultTextCaption;
            FResultCode = AResultCode;
            FResultSeverity = AResultSeverity;
        }

        /// <summary>
        /// Context of the Verification Result (where the Verification Result originated from).
        /// </summary>
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
        public TScreenVerificationResult(object AResultContext,
            DataColumn AResultColumn,
            String AResultText,
            String AResultCode,
            Control AResultControl)
        {
            FResultContext = AResultContext;
            FResultColumn = AResultColumn;
            FResultText = AResultText;
            FResultSeverity = TResultSeverity.Resv_Critical;
            FResultCode = AResultCode;
            FResultControl = AResultControl;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AResultContext">context of verification</param>
        /// <param name="AResultColumn">which column failed</param>
        /// <param name="AResultText">description and error message for the user</param>
        /// <param name="AResultControl">which control is involved</param>
        /// <param name="AResultSeverity">is this serious, or just a warning</param>
        public TScreenVerificationResult(object AResultContext,
            DataColumn AResultColumn,
            String AResultText,
            Control AResultControl,
            TResultSeverity AResultSeverity)
        {
            FResultContext = AResultContext;
            FResultColumn = AResultColumn;
            FResultText = AResultText;
            FResultSeverity = AResultSeverity;
            FResultControl = AResultControl;
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
        public TScreenVerificationResult(object AResultContext,
            DataColumn AResultColumn,
            String AResultText,
            String AResultCode,
            Control AResultControl,
            TResultSeverity AResultSeverity)
        {
            FResultContext = AResultContext;
            FResultColumn = AResultColumn;
            FResultText = AResultText;
            FResultCode = AResultCode;
            FResultControl = AResultControl;
            FResultSeverity = AResultSeverity;
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
        public TScreenVerificationResult(object AResultContext,
            DataColumn AResultColumn,
            String AResultText,
            String AResultCaption,
            String AResultCode,
            Control AResultControl,
            TResultSeverity AResultSeverity)
        {
            FResultContext = AResultContext;
            FResultColumn = AResultColumn;
            FResultText = AResultText;
            FResultTextCaption = AResultCaption;
            FResultCode = AResultCode;
            FResultControl = AResultControl;
            FResultSeverity = AResultSeverity;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AVerificationResult"><see cref="TVerificationResult" /> which
        /// contains the basic data to which the <paramref name="AResultColumn" /> and
        /// <paramref name="AResultControl" /> are getting added.</param>
        /// <param name="AResultColumn">which column failed</param>
        /// <param name="AResultControl">which control is involved</param>
        public TScreenVerificationResult(TVerificationResult AVerificationResult,
            DataColumn AResultColumn, Control AResultControl)
        {
            FResultContext = AVerificationResult.ResultContext;
            FResultColumn = AResultColumn;
            FResultText = AVerificationResult.ResultText;
            FResultTextCaption = AVerificationResult.ResultTextCaption;
            FResultCode = AVerificationResult.ResultCode;
            FResultControl = AResultControl;
            FResultSeverity = AVerificationResult.ResultSeverity;
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
        #region Resourcestrings

        private static readonly string StrMessageFooter = Catalog.GetString("  Context: {0}; Severity: {1}.\r\n    Problem: {2}\r\n    Code: {3}");

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TVerificationResultCollection()
        {
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
        /// add a new verification object
        /// </summary>
        /// <param name="value">the verification object to be added</param>
        /// <returns></returns>
        public int Add(IResultInterface value)
        {
            return List.Add(value);
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
        /// generate the text for a message box showing all verification errors
        /// </summary>
        /// <param name="AErrorMessages">will have the list of error messages</param>
        /// <param name="AFirstErrorControl">for focusing the first control that caused verification failure</param>
        /// <param name="AFirstErrorContext">context of the first error</param>
        public void BuildScreenVerificationResultList(out String AErrorMessages, out Control AFirstErrorControl, out object AFirstErrorContext)
        {
            TScreenVerificationResult si;

            AFirstErrorControl = null;
            AErrorMessages = "";
            AFirstErrorContext = null;

            for (int Counter = 0; Counter <= Count - 1; Counter += 1)
            {
                si = (TScreenVerificationResult)(List[Counter]);
                AErrorMessages = AErrorMessages + si.ResultText;

                if (si.ResultCode != String.Empty)
                {
                    AErrorMessages += "  [" + si.ResultCode + "]";
                }

                AErrorMessages += Environment.NewLine + Environment.NewLine;

                if (AFirstErrorControl == null)
                {
                    AFirstErrorControl = si.ResultControl;
                    AFirstErrorContext = si.ResultContext;
                }
            }
        }

        /// <summary>
        /// generate the text for a message box showing all verification errors of a given context
        /// </summary>
        /// <param name="AResultContext">only show errors of the given context</param>
        /// <param name="AErrorMessages">will have the list of error messages</param>
        /// <param name="AFirstErrorControl">for focusing the first control that caused verification failure</param>
        public void BuildScreenVerificationResultList(object AResultContext, out String AErrorMessages, out Control AFirstErrorControl)
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
                    }
                }
            }
        }

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

                ReturnValue = ReturnValue +
                              (String.Format(StrMessageFooter,
                                   new object[] { si.ResultContext, si.ResultSeverity, si.ResultText, si.ResultCode })) +
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
        /// <returns>void</returns>
        public void AddOrRemove(TVerificationResult AVerificationResult, DataColumn AResultColumn)
        {
            List <TScreenVerificationResult>si = FindAllBy(AResultColumn);
            bool IdenticalVResultFound = false;

            if (AVerificationResult != null)
            {
                if (si == null)
                {
                    this.Add(AVerificationResult);
                }
                else
                {
                    foreach (TScreenVerificationResult SingleEntry in si)
                    {
                        if (TVerificationHelper.AreVerificationResultsIdentical(SingleEntry, AVerificationResult))
                        {
                            IdenticalVResultFound = true;
                            break;
                        }
                    }

                    if (!IdenticalVResultFound)
                    {
                        this.Add(AVerificationResult);
                    }
                }
            }
            else if (si != null)
            {
                foreach (TScreenVerificationResult SingleEntry in si)
                {
                    this.Remove(SingleEntry);
                }
            }
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
        /// <returns>True if the two <see cref="TVerificationResult" />s are completely identical,
        /// otherwise false.</returns>
        public static bool AreVerificationResultsIdentical(TVerificationResult AVerificationResult1, TVerificationResult AVerificationResult2)
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

                if (AVerificationResult1.ResultContext != AVerificationResult2.ResultContext)
                {
                    return false;
                }

                if (AVerificationResult1.ResultSeverity != AVerificationResult2.ResultSeverity)
                {
                    return false;
                }

                if (AVerificationResult1.ResultText != AVerificationResult2.ResultText)
                {
                    return false;
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
        /// Calls the <see cref="DataRow.SetColumnError(DataColumn, String)" /> Method of a
        /// DataRow's Column to the  <see cref="TVerificationResult.ResultText" /> Property
        /// of the passed in <see cref="TVerificationResult" />.
        /// </summary>
        /// <param name="AEventArgs">An instance of DataColumnChangeEventArgs.</param>
        /// <param name="AVerificationResultEntry"><see cref="TVerificationResult" /> which has
        /// its <see cref="TVerificationResult.ResultText" /> Property set.</param>
        /// <param name="AControlName">Name of the Control to which the <see cref="TVerificationResult" />
        /// is related.</param>
        /// <param name="AResetValue">Set this to true to retain the
        /// <see cref="DataColumnChangeEventArgs.ProposedValue " />.</param>
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