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
using Ict.Common.Exceptions;

namespace Ict.Common
{
    /// <summary>
    /// Contains Helper Methods for working with information that is associated
    /// with Error Codes.
    /// </summary>
    public class ErrorCodes
    {
        #region HELPER METHODS (*No need to change these* when adding/modifying/deleting error codes!)

        /// <summary>
        /// Returns an <see cref="ErrCodeInfo" /> object for a specified error code.
        /// </summary>
        /// <param name="AErrorCode">Error Code.</param>
        /// <returns>An <see cref="ErrCodeInfo" /> object which holds information about
        /// the specified error code, or null, if the error code was not found.</returns>
        public static ErrCodeInfo GetErrorInfo(string AErrorCode)
        {
            ErrCodeInfo ReturnValue = null;

            ReturnValue = ErrorCodeInventory.RetrieveErrCodeInfo(AErrorCode);

            if (ReturnValue == null)
            {
                throw new EErrorCodeNotRegisteredException(String.Format("Error Code '{0}' could not be found in any of the registered Types!",
                        AErrorCode) + Environment.NewLine +
                    "Registered Types: " + ErrorCodeInventory.ListRegisteredTypes() + ".");
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns an <see cref="ErrCodeInfo" /> object for a specified error code.
        /// </summary>
        /// <param name="AErrorCode">Error Code.</param>
        /// <param name="AErrorMessagePlaceholderTexts">Array whose strings are placed into placeholders
        /// which are found in the ErrorCode's ErrorMessageText (optional Argument!)</param>.
        /// <returns>An <see cref="ErrCodeInfo" /> object which holds information about
        /// the specified error code, or null, if the error code was not found.</returns>
        public static ErrCodeInfo GetErrorInfo(string AErrorCode, string[] AErrorMessagePlaceholderTexts)
        {
            ErrCodeInfo ReturnValue = GetErrorInfo(AErrorCode);
            string ErrorMessageText;

            if (ReturnValue.ErrorMessageText != String.Empty)
            {
                ErrorMessageText = ReturnValue.ErrorMessageText;
            }
            else
            {
                if ((AErrorMessagePlaceholderTexts != null)
                    && (AErrorMessagePlaceholderTexts.Length > 0))
                {
                    ErrorMessageText = AErrorMessagePlaceholderTexts[0];
                    AErrorMessagePlaceholderTexts = null;
                }
                else
                {
                    throw new ArgumentException("The error code's ErrorMessageText is an empty string, therefore this overload can't be used. " +
                        "Use the overload that has the Argument 'AErrorMessageText' instead, or define the error code's ErrorMessageText");
                }
            }

            return GetErrorInfo(AErrorCode, ErrorMessageText, AErrorMessagePlaceholderTexts);
        }

        /// <summary>
        /// Returns an <see cref="ErrCodeInfo" /> object for a specified error code.
        /// </summary>
        /// <param name="AErrorCode">Error Code.</param>
        /// <param name="AErrorMessageText">Set this to <see cref="String.Empty" /> to use the
        /// <see cref="ErrCodeInfo.ShortDescription" /> of the <see cref="ErrCodeInfo" />, set it to any other string and
        /// this will be displayed instead.</param>
        /// <param name="AErrorMessagePlaceholderTexts">Array whose strings are placed into placeholders which are found in
        /// <paramref name="AErrorMessageText" /> (optional Argument!)</param>.
        /// <param name="AErrorTitlePlaceholderTexts">Array whose strings are placed into placeholders which are found in
        /// the ErrorCode's ErrorTitle (optional Argument!)</param>.
        /// /// <returns>An <see cref="ErrCodeInfo" /> object which holds information about
        /// the specified error code, or null, if the error code was not found.</returns>
        public static ErrCodeInfo GetErrorInfo(string AErrorCode, string AErrorMessageText,
            string[] AErrorMessagePlaceholderTexts = null, string[] AErrorTitlePlaceholderTexts = null)
        {
            ErrCodeInfo FoundErrInfo;
            ErrCodeInfo ReturnValue = null;
            string ErrorMessageText;
            string ErrorTitleText = String.Empty;

            if (AErrorMessageText == null)
            {
                throw new ArgumentException("Argument 'AErrorMessageText' must not be null");
            }

            FoundErrInfo = GetErrorInfo(AErrorCode);

            if (AErrorMessageText == String.Empty)
            {
                if (FoundErrInfo.ErrorMessageText == String.Empty)
                {
                    throw new ArgumentException(
                        "Argument 'AErrorMessageText' must not be an empty string if the error code's ErrorMessageText is an empty string, too");
                }
                else
                {
                    ErrorMessageText = FoundErrInfo.ErrorMessageText;
                }
            }
            else
            {
                ErrorMessageText = AErrorMessageText;
            }

            if ((FoundErrInfo != null))
            {
                if ((AErrorMessagePlaceholderTexts != null)
                    && (AErrorMessagePlaceholderTexts.Length != 0))
                {
                    ErrorMessageText = String.Format(ErrorMessageText, AErrorMessagePlaceholderTexts);
                }

                ErrorTitleText = FoundErrInfo.ErrorMessageTitle;

                if ((AErrorTitlePlaceholderTexts != null)
                    && (AErrorTitlePlaceholderTexts.Length != 0))
                {
                    ErrorTitleText = String.Format(ErrorTitleText, AErrorTitlePlaceholderTexts);
                }
            }

            ReturnValue = new ErrCodeInfo(FoundErrInfo.ErrorCode, FoundErrInfo.ErrorCodeConstantClass,
                FoundErrInfo.ErrorCodeConstantName, FoundErrInfo.ShortDescription,
                FoundErrInfo.FullDescription, ErrorMessageText, ErrorTitleText,
                FoundErrInfo.Category, FoundErrInfo.HelpID);

            return ReturnValue;
        }

        /// <summary>
        /// Returns the error text (ShortDescription) for a specified error code.
        /// </summary>
        /// <param name="AErrorCode">Error Code.</param>
        /// <returns>Error text (ShortDescription) for a specified error code, or
        /// <see cref="System.String.Empty" /> if the error code was not found.</returns>
        public static string GetErrorText(string AErrorCode)
        {
            ErrCodeInfo EInfo = GetErrorInfo(AErrorCode);

            if (EInfo != null)
            {
                return EInfo.ShortDescription;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Returns the error description (FullDescription) for a specified error code.
        /// </summary>
        /// <param name="AErrorCode">Error Code.</param>
        /// <returns>Error description (FullDescription) for a specified error code, or
        /// <see cref="System.String.Empty" /> if the error code was not found.</returns>
        public static string GetErrorDescription(string AErrorCode)
        {
            ErrCodeInfo EInfo = GetErrorInfo(AErrorCode);

            if (EInfo != null)
            {
                return EInfo.FullDescription;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Returns the error category for a specified error code.
        /// </summary>
        /// <param name="AErrorCode">Error Code.</param>
        /// <returns>Error category for a specified error code.</returns>
        /// <exception cref="NullReferenceException">Thrown if the specified error code wasn't found.</exception>
        public ErrCodeCategory GetErrorCategory(string AErrorCode)
        {
            return GetErrorInfo(AErrorCode).Category;
        }

        /// <summary>
        /// Returns the error Help ID for a specified error code.
        /// </summary>
        /// <param name="AErrorCode">Error Code.</param>
        /// <returns>Error Help ID for a specified error code.</returns>
        public static string GetErrorHelpID(string AErrorCode)
        {
            ErrCodeInfo EInfo = GetErrorInfo(AErrorCode);

            if (EInfo != null)
            {
                return EInfo.HelpID;
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion
    }

    /// <summary>
    /// Error Code Category.
    /// </summary>
    public enum ErrCodeCategory
    {
        /// <summary>Error Code for validation errors.</summary>
        Validation,

        /// <summary>Error Code for non-critical general errors.</summary>
        NonCriticalError,

        /// <summary>Error Code for general errors.</summary>
        Error
    }

    /// <summary>
    /// Holds information about a specific error code.
    /// </summary>
    public class ErrCodeInfo
    {
        string FErrorCode = String.Empty;
        string FErrorCodeConstantClass = String.Empty;
        string FErrorCodeConstantName = String.Empty;
        string FShortDescription = String.Empty;
        string FFullDescription = String.Empty;
        string FErrorMessageText = String.Empty;
        string FErrorMessageTitle = String.Empty;
        ErrCodeCategory FCategory;
        string FHelpID = String.Empty;
        bool FControlValueUndoRequested = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="AErrorCode">Error code.</param>
        /// <param name="AErrorCodeConstantClass">Class that holds the definition of the constant for the error code.</param>
        /// <param name="AErrorCodeConstantName">Name of the constant for the error code.</param>
        /// <param name="AShortDescription">Short description of the error code.</param>
        /// <param name="AFullDescription">Full description of the error code.</param>
        /// <param name="AErrorMessageText">String that will be displayed in MessageBoxes, etc. instead of the value of <paramref name="AShortDescription" />.</param>
        /// <param name="AErrorMessageTitle">String that will be displayed as Title of MessageBoxes, etc..</param>
        /// <param name="ACategory">Category of the error code.</param>
        /// <param name="AHelpID">Help ID of the error code.</param>
        /// <param name="AControlValueUndoRequested">Set this to true if the Error Code requests that the validated
        /// Control's value is undone.</param>
        public ErrCodeInfo(string AErrorCode,
            string AErrorCodeConstantClass,
            string AErrorCodeConstantName,
            string AShortDescription,
            string AFullDescription,
            string AErrorMessageText,
            string AErrorMessageTitle,
            ErrCodeCategory ACategory,
            string AHelpID,
            bool AControlValueUndoRequested = false)
        {
            FErrorCode = AErrorCode;
            FErrorCodeConstantClass = AErrorCodeConstantClass;
            FErrorCodeConstantName = AErrorCodeConstantName;
            FShortDescription = AShortDescription;
            FFullDescription = AFullDescription;
            FErrorMessageText = AErrorMessageText;
            FErrorMessageTitle = AErrorMessageTitle;
            FCategory = ACategory;
            FHelpID = AHelpID;
            FControlValueUndoRequested = AControlValueUndoRequested;
        }

        /// <summary>
        /// Error code.
        /// </summary>
        public string ErrorCode
        {
            get
            {
                return FErrorCode;
            }
        }


        /// <summary>
        /// Name of the Class which holds the Constant that defines the error code.
        /// </summary>
        public string ErrorCodeConstantClass
        {
            get
            {
                return FErrorCodeConstantClass;
            }
        }

        /// <summary>
        /// Name of the Constant that defines the error code.
        /// </summary>
        public string ErrorCodeConstantName
        {
            get
            {
                return FErrorCodeConstantName;
            }
        }

        /// <summary>
        /// Short description of the error code.
        /// </summary>
        /// <remarks>This string will be used when a Ict.Common.Verification.TVerificationResult is created with an <see cref="ErrCodeInfo" /> Argument.</remarks>
        public string ShortDescription
        {
            get
            {
                return Catalog.GetString(FShortDescription);
            }
        }

        /// <summary>
        /// Full description of the error code.
        /// </summary>
        public string FullDescription
        {
            get
            {
                return Catalog.GetString(FFullDescription);
            }
        }

        /// <summary>
        /// Error message which should be displayed in a MessageBox, etc. for the error code. Can contain {0}, etc.
        /// </summary>
        /// <remarks>This string will override <see cref="ShortDescription" /> when a Ict.Common.Verification.TVerificationResult" is created with an <see cref="ErrCodeInfo" /> Argument.
        /// (<see cref="ShortDescription" /> would otherwise be used in MessageBoxes, etc.)</remarks>
        public virtual string ErrorMessageText
        {
            get
            {
                return FErrorMessageText;
            }

            set
            {
                FErrorMessageText = value;
            }
        }

        /// <summary>
        /// Title of an error message which should be displayed in a MessageBox, etc. for the error code.
        /// </summary>
        public virtual string ErrorMessageTitle
        {
            get
            {
                return FErrorMessageTitle;
            }

            set
            {
                FErrorMessageTitle = value;
            }
        }

        /// <summary>
        /// Category of the error code.
        /// </summary>
        public ErrCodeCategory Category
        {
            get
            {
                return FCategory;
            }
        }

        /// <summary>
        /// Help ID of the error code.
        /// </summary>
        public string HelpID
        {
            get
            {
                return FHelpID;
            }
        }

        /// <summary>
        /// Is true (or set to true) if the Error Code requests that the validated
        /// Control's value is undone.
        /// </summary>
        public bool ControlValueUndoRequested
        {
            get
            {
                return FControlValueUndoRequested;
            }
        }
    }


    /// <summary>
    /// Allows to add information to a constant for a specific error code.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ErrCodeAttribute : System.Attribute
    {
        string FShortDescription = String.Empty;
        string FFullDescription = String.Empty;
        string FErrorMessageText = String.Empty;
        string FErrorMessageTitle = String.Empty;
        ErrCodeCategory FCategory;
        string FHelpID = String.Empty;
        bool FControlValueUndoRequested = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="AShortDescription">Short description of the error code.</param>
        public ErrCodeAttribute(string AShortDescription)
        {
            FShortDescription = AShortDescription;
        }

        /// <summary>
        /// Category of the error code.
        /// </summary>
        public virtual ErrCodeCategory Category
        {
            get
            {
                return FCategory;
            }

            set
            {
                FCategory = value;
            }
        }

        /// <summary>
        /// Short description of the error code.
        /// </summary>
        public virtual string ShortDescription
        {
            get
            {
                return FShortDescription;
            }
        }

        /// <summary>
        /// Full description of the error code.
        /// </summary>
        public virtual string FullDescription
        {
            get
            {
                return FFullDescription;
            }

            set
            {
                FFullDescription = value;
            }
        }

        /// <summary>
        /// Error message which should be displayed in a MessageBox, etc. for the error code. Can contain {0}, etc.
        /// </summary>
        /// <remarks>This will override <see cref="ShortDescription" />, which would otherwise be used in MessageBoxes, etc.</remarks>
        public virtual string ErrorMessageText
        {
            get
            {
                return FErrorMessageText;
            }

            set
            {
                FErrorMessageText = value;
            }
        }

        /// <summary>
        /// Title of an error message which should be displayed in a MessageBox, etc. for the error code.
        /// </summary>
        public virtual string ErrorMessageTitle
        {
            get
            {
                return FErrorMessageTitle;
            }

            set
            {
                FErrorMessageTitle = value;
            }
        }

        /// <summary>
        /// Help ID of the error code.
        /// </summary>
        public virtual string HelpID
        {
            get
            {
                return FHelpID;
            }

            set
            {
                FHelpID = value;
            }
        }

        /// <summary>
        /// Is true (or set to true) if the Error Code requests that the validated
        /// Control's value is undone.
        /// </summary>
        public virtual bool ControlValueUndoRequested
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
    }


    /// <summary>
    /// Thrown if an attempt is made to inquire an Error Code that doesn't exist in Dictionary <see cref="ErrorCodeInventory.ErrorCodeCatalogue" />.
    /// </summary>
    public class EErrorCodeNotRegisteredException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EErrorCodeNotRegisteredException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EErrorCodeNotRegisteredException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EErrorCodeNotRegisteredException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }
}