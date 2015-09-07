//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Reflection;
using Ict.Common;
using Ict.Common.Exceptions;

namespace Ict.Common
{
    /// <summary>
    /// Inventory functions for the OpenPetra Error Codes.
    /// </summary>
    public static class ErrorCodeInventory
    {
        /// <summary>Catalogue of all OpenPetra Error Codes.</summary>
        /// <description>Automatically created from all the public Constants of the
        /// <see cref="Ict.Common.CommonErrorCodes" /> Class and the
        /// 'Ict.Petra.Shared.PetraErrorCodes' Class when Method
        /// <see cref="BuildErrorCodeInventory" /> of this Class called!</description>
        public static Dictionary <string, ErrCodeInfo>ErrorCodeCatalogue = new Dictionary <string, ErrCodeInfo>();

        /// <summary>
        /// Contains all Types which are registered to contain Error Code constants.
        /// </summary>
        /// <remarks>IMPORTANT: Only Error Code Constants of Types which are registered can be used with
        /// Method <see cref="M:ErrorCodes.GetErrorInfo(string)" />!</remarks>
        public static List <Type>RegisteredTypes = new List <Type>();

        /// <summary>
        /// Internal lookup for Types that have been parsed for Error Codes.
        /// </summary>
        private static Dictionary <string, Type>CataloguedTypes = new Dictionary <string, Type>();

        static ErrorCodeInventory()
        {
            ErrorCodeInventory.RegisteredTypes.Add(new Ict.Common.CommonErrorCodes().GetType());
        }

        /// <summary>
        /// Retrieves the <see cref="ErrCodeInfo" /> for a given Error Code.
        /// </summary>
        /// <param name="AErrorCode">Error Code.</param>
        /// <returns><see cref="ErrCodeInfo" /> for the Error Code specified in <paramref name="AErrorCode" /> or
        /// null if the Error Code isn't found in any of the registered Types or in the calling Class.</returns>
        public static ErrCodeInfo RetrieveErrCodeInfo(string AErrorCode)
        {
            ErrCodeInfo ReturnValue = null;
            Type CheckedType = null;
            int Counter2 = 0;

            if (ErrorCodeCatalogue.ContainsKey(AErrorCode))
            {
                ReturnValue = (ErrCodeInfo)ErrorCodeInventory.ErrorCodeCatalogue[AErrorCode];
            }
            else
            {
                // Error Code wasn't found -> need to check whether it was registered and included in the Error Code Inventory!
                for (int Counter = 0; Counter < RegisteredTypes.Count; Counter++)
                {
                    CheckedType = RegisteredTypes[Counter];

                    if (!AreTypesErrorCodesCatalogued(CheckedType.Name))
                    {
                        BuildErrorCodeInventory(CheckedType);
                    }

                    if (ErrorCodeCatalogue.ContainsKey(AErrorCode))
                    {
                        ReturnValue = (ErrCodeInfo)ErrorCodeInventory.ErrorCodeCatalogue[AErrorCode];
                        break;
                    }
                }

                if (ReturnValue == null)
                {
                    // 'On-the-fly' adding to the 'registered types' of the Type that called this Method.
                    // This is needed for the case where an Error Code is defined in any Type other than the pre-registered Types.

                    do
                    {
                        Counter2++;
                        MethodBase method = new System.Diagnostics.StackTrace(false).GetFrame(Counter2).GetMethod();

                        if (method.DeclaringType.Name.StartsWith("<"))
                        {
                            // Note by AP: 17.08.2015
                            // This indicates that we were called by an anonymous delegate inside a method
                            //  - the Name property will be something like <>c as opposed to a sensible name
                            // So we need to use the DeclaringType property twice
                            // This is what happens in our TestErrorCodes Ict.Testing.lib.Common test
                            // This seems to be a new feature that has come with a recent .NET update (?) or Visual Studio 2015
                            //   because we never needed to do this before and tests always passed.
                            CheckedType = method.DeclaringType.DeclaringType;
                        }
                        else
                        {
                            // Just a normal type
                            CheckedType = method.DeclaringType;
                        }
                    } while (CheckedType.FullName == "Ict.Common.ErrorCodes");

                    if (!AreTypesErrorCodesCatalogued(CheckedType.Name))
                    {
                        BuildErrorCodeInventory(CheckedType);

                        // Recursive self-calling!
                        ReturnValue = RetrieveErrCodeInfo(AErrorCode);
                    }
                }
            }

            return ReturnValue;
        }

        private static bool AreTypesErrorCodesCatalogued(string AType)
        {
            if (ErrorCodeInventory.CataloguedTypes.ContainsKey(AType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Lists the Types that are registered to contain Error Codes.
        /// </summary>
        /// <returns>Types that are registered to contain Error Codes, sepearated by semicolons.</returns>
        public static string ListRegisteredTypes()
        {
            string ReturnValue = String.Empty;

            for (int Counter = 0; Counter < RegisteredTypes.Count; Counter++)
            {
                ReturnValue += RegisteredTypes[Counter].FullName + "; ";
            }

            ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 2);

            return ReturnValue;
        }

        /// <summary>
        /// Populates the Dictionary <see cref="ErrorCodeCatalogue" /> from
        /// all public Constants of the Type that is passed in.
        /// </summary>
        /// <param name="AErrorCodesType">Type that contains public error Constants.</param>
        public static void BuildErrorCodeInventory(Type AErrorCodesType)
        {
            object[] ErrCodeAttributes;
            ErrCodeInfo ErrCodeDetails = null;
            ErrCodeCategory ErrCodeCat;
            string ErrCodeValue;

//TLogging.Log("BuildErrorCodeInventory: AErrorCodesType Type: " + AErrorCodesType.Name);

            FieldInfo[] ErrcodesFields = AErrorCodesType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            try
            {
                for (int FieldCounter = 0; FieldCounter < ErrcodesFields.Length; FieldCounter++)
                {
                    FieldInfo FieldInf = ErrcodesFields[FieldCounter];
                    ErrCodeDetails = null;

                    // We are cataloging only public Constants!
                    if ((!((FieldInf.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.InitOnly))
                        && ((FieldInf.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public))
                    {
                        // Constants' identifier (name) must start with "ERR_"!
                        if (FieldInf.Name.StartsWith("ERR_"))
                        {
                            FieldInfo pi = AErrorCodesType.GetField(FieldInf.Name);
                            ErrCodeAttributes = pi.GetCustomAttributes(typeof(ErrCodeAttribute), false);

                            ErrCodeValue = FieldInf.GetValue(FieldInf).ToString();

                            if (ErrCodeValue.EndsWith("V"))
                            {
                                ErrCodeCat = ErrCodeCategory.Validation;
                            }
                            else if (ErrCodeValue.EndsWith("N"))
                            {
                                ErrCodeCat = ErrCodeCategory.NonCriticalError;
                            }
                            else
                            {
                                ErrCodeCat = ErrCodeCategory.Error;
                            }

                            if (ErrCodeAttributes.Length > 0)
                            {
                                foreach (ErrCodeAttribute Attr in ErrCodeAttributes)
                                {
                                    ErrCodeDetails = new ErrCodeInfo(ErrCodeValue, AErrorCodesType.FullName, FieldInf.Name,
                                        Attr.ShortDescription, Attr.FullDescription,
                                        Attr.ErrorMessageText, Attr.ErrorMessageTitle, ErrCodeCat, Attr.HelpID, Attr.ControlValueUndoRequested);
                                }
                            }
                            else
                            {
                                ErrCodeDetails = new ErrCodeInfo(ErrCodeValue, AErrorCodesType.FullName, FieldInf.Name,
                                    String.Empty, String.Empty,
                                    String.Empty, String.Empty, ErrCodeCat, String.Empty);
                            }

                            try
                            {
                                ErrorCodeCatalogue.Add(ErrCodeValue, ErrCodeDetails);
                            }
                            catch (ArgumentException)
                            {
                                ErrCodeInfo DefiningErrCodeInfo = ErrorCodes.GetErrorInfo(ErrCodeValue);

                                throw new EDuplicateErrorCodeException(
                                    String.Format(
                                        "An attempt to add Error Code with value '{0}' through constant '{1}' failed, as there is already an Error Code with that value: it is defined through constant '{2}'.",
                                        ErrCodeValue, AErrorCodesType.FullName + "." + FieldInf.Name,
                                        DefiningErrCodeInfo.ErrorCodeConstantClass + "." + DefiningErrCodeInfo.ErrorCodeConstantName));
                            }
                        }
                    }
                }
            }
            finally
            {
                CataloguedTypes.Add(AErrorCodesType.Name, AErrorCodesType);
//TLogging.Log("BuildErrorCodeInventory: Added " + AErrorCodesType.Name + " to the CataloguedTypes.");
//            System.Windows.Forms.MessageBox.Show("ErrorCodesInventory has " + ErrorCodeCatalogue.Count.ToString() + " Error Codes.");
            }
        }
    }


    /// <summary>
    /// Thrown if an attempt is made to add a Error Code with a value that already exists in Dictionary <see cref="ErrorCodeInventory.ErrorCodeCatalogue" />.
    /// </summary>
    public class EDuplicateErrorCodeException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDuplicateErrorCodeException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDuplicateErrorCodeException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDuplicateErrorCodeException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }
}