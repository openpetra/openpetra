//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;

using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Client.MPartner.Logic
{
    /// <summary>
    /// The object class that is used to handle our built-in standard FormData sorting.
    /// Like all FormData sort objects it implements our IFormDataSort standard interface
    /// </summary>
    public class TStandardFormDataSort : IFormDataSort
    {
        #region Available Sort methods as an enumeration

        /// <summary>
        /// Enumeration of the available standard FormData sort options
        /// </summary>
        public enum TSortEnum
        {
            /// <summary>No sorting</summary>
            NoSort,

            /// <summary>Sort by Last Name then First Name</summary>
            LastNameFirstName,

            /// <summary>Sort by First Name then Last Name</summary>
            FirstNameLastName,

            /// <summary>Sort by Postal Code.  Spaces are removed so as to work with UK Post Codes.  CH- prefix is removed</summary>
            PostalCode,

            /// <summary>Sort by Country Code then Postal Code</summary>
            CountryCodePostalCode,

            /// <summary>Sort by Postcode Region as defined by the Postcode Region table</summary>
            PostCodeRegion,

            /// <summary>Sort by County</summary>
            County,

            /// <summary>Sort by Country Name</summary>
            CountryName
        };

        private TSortEnum FFormDataSortMethod = TSortEnum.NoSort;

        #endregion

        #region Standard IFormDataSort method implementation

        /// <summary>
        /// Standard interface method to Initialize the class.  There is one parameter - the Sort method enumeration
        /// </summary>
        /// <param name="AParamList">A single parameter - the sort method enumeration.  Must be one of <see cref="TSortEnum"/> enumerations</param>
        /// <exception cref="System.ArgumentNullException">Thrown if no sort enumeration is specified</exception>
        /// <exception cref="System.ArgumentException">Thrown if there is more than one argument passed in</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the sort enumeration is invalid</exception>
        /// <returns>Always returns True if no exception is raised</returns>
        public bool Initialize(object[] AParamList)
        {
            if (AParamList == null)
            {
                throw new ArgumentNullException("");
            }

            if (AParamList.Length != 1)
            {
                throw new ArgumentException("");
            }

            try
            {
                FFormDataSortMethod = (TSortEnum)AParamList[0];
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException("");
            }

            return true;
        }

        /// <summary>
        /// Standard <cref name="IFormDataSort "></cref>interface method to run the Sort algorithm
        /// </summary>
        /// <param name="AFormDataList">A list of FormData elements to be sorted.  The list must contain <cref name="TFormDataPartner "></cref>elements</param>
        /// <returns>True</returns>
        public bool Sort(List <TFormData>AFormDataList)
        {
            switch (FFormDataSortMethod)
            {
                case TSortEnum.LastNameFirstName:
                    AFormDataList.Sort(CompareUsingLastNameFirstName);
                    break;

                case TSortEnum.FirstNameLastName:
                    AFormDataList.Sort(CompareUsingFirstNameLastName);
                    break;

                case TSortEnum.PostalCode:
                    AFormDataList.Sort(CompareUsingPostalCode);
                    break;

                case TSortEnum.PostCodeRegion:
                    AFormDataList.Sort(CompareUsingPostCodeRegion);
                    break;

                case TSortEnum.CountryCodePostalCode:
                    AFormDataList.Sort(CompareUsingCountryCodePostCode);
                    break;

                case TSortEnum.County:
                    AFormDataList.Sort(CompareUsingCounty);
                    break;

                case TSortEnum.CountryName:
                    AFormDataList.Sort(CompareUsingCountryName);
                    break;

                default:    // NoSort
                    break;
            }

            return true;
        }

        #endregion

        #region Static Sort-method delegates

        private static int CompareUsingLastNameFirstName(TFormData A, TFormData B)
        {
            TFormDataPartner P1 = A as TFormDataPartner;
            TFormDataPartner P2 = B as TFormDataPartner;

            if ((P1 == null) || (P2 == null))
            {
                throw new ArgumentException("Cannot sort the form data because it is not related to Partner details.");
            }

            int byLastName = CompareInternal(P1.LastName, P2.LastName);

            if (byLastName != 0)
            {
                return byLastName;
            }

            return CompareInternal(P1.FirstName, P2.FirstName);
        }

        private static int CompareUsingFirstNameLastName(TFormData A, TFormData B)
        {
            TFormDataPartner P1 = A as TFormDataPartner;
            TFormDataPartner P2 = B as TFormDataPartner;

            if ((P1 == null) || (P2 == null))
            {
                throw new ArgumentException("Cannot sort the form data because it is not related to Partner details.");
            }

            int byFirstName = CompareInternal(P1.FirstName, P2.FirstName);

            if (byFirstName != 0)
            {
                return byFirstName;
            }

            return CompareInternal(P1.LastName, P2.LastName);
        }

        private static int CompareUsingPostalCode(TFormData A, TFormData B)
        {
            TFormDataPartner P1 = A as TFormDataPartner;
            TFormDataPartner P2 = B as TFormDataPartner;

            if ((P1 == null) || (P2 == null))
            {
                throw new ArgumentException("Cannot sort the form data because it is not related to Partner details.");
            }

            string code1 = P1.PostalCode;
            string code2 = P2.PostalCode;

            if (code1 != null)
            {
                // Remove any spaces in the code (eg GB codes can have a space)
                code1 = code1.Replace(" ", "");

                // Special cases:
                // Switzerland uses CH- prefix sometimes
                if (code1.StartsWith("CH-"))
                {
                    code1 = code1.Substring(3);
                }
            }

            if (code2 != null)
            {
                // Same for code2
                code2 = code2.Replace(" ", "");

                if (code2.StartsWith("CH-"))
                {
                    code2 = code2.Substring(3);
                }
            }

            return CompareInternal(code1, code2);
        }

        private static int CompareUsingPostCodeRegion(TFormData A, TFormData B)
        {
            TFormDataPartner P1 = A as TFormDataPartner;
            TFormDataPartner P2 = B as TFormDataPartner;

            if ((P1 == null) || (P2 == null))
            {
                throw new ArgumentException("Cannot sort the form data because it is not related to Partner details.");
            }

            // At the moment TFormDataPartner does not have a PostcodeRegion property
            throw new NotImplementedException();
        }

        private static int CompareUsingCounty(TFormData A, TFormData B)
        {
            TFormDataPartner P1 = A as TFormDataPartner;
            TFormDataPartner P2 = B as TFormDataPartner;

            if ((P1 == null) || (P2 == null))
            {
                throw new ArgumentException("Cannot sort the form data because it is not related to Partner details.");
            }

            return CompareInternal(P1.County, P2.County);
        }

        private static int CompareUsingCountryName(TFormData A, TFormData B)
        {
            TFormDataPartner P1 = A as TFormDataPartner;
            TFormDataPartner P2 = B as TFormDataPartner;

            if ((P1 == null) || (P2 == null))
            {
                throw new ArgumentException("Cannot sort the form data because it is not related to Partner details.");
            }

            return CompareInternal(P1.CountryName, P2.CountryName);
        }

        private static int CompareUsingCountryCodePostCode(TFormData A, TFormData B)
        {
            TFormDataPartner P1 = A as TFormDataPartner;
            TFormDataPartner P2 = B as TFormDataPartner;

            if ((P1 == null) || (P2 == null))
            {
                throw new ArgumentException("Cannot sort the form data because it is not related to Partner details.");
            }

            int byCountryCode = CompareInternal(P1.CountryCode, P2.CountryCode);

            if (byCountryCode != 0)
            {
                return byCountryCode;
            }

            return CompareUsingPostalCode(A, B);
        }

        private static int CompareInternal(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're equal.
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y is greater.
                    return -1;
                }
            }
            else
            {
                // So x is not null...
                if (y == null)
                {
                    // ...and y is null, x is greater.
                    return 1;
                }
                else
                {
                    // ...and y is not null, use the standard case-insensitive string compare
                    // If the strings look like numbers then they are compared numerically
                    Int32 intX, intY;

                    if (Int32.TryParse(x, out intX) && Int32.TryParse(y, out intY))
                    {
                        if (intX == intY)
                        {
                            return 0;
                        }
                        else if (intX < intY)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }

                    return String.Compare(x, y, true);
                }
            }
        }

        #endregion
    }
}