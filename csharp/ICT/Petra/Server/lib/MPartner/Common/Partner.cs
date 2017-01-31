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
using System.Collections;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Common.Verification;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;
using Npgsql;
using System.Data.Odbc;

namespace Ict.Petra.Server.MPartner.Partner
{
    /**
     * Enumeration: return values for functions in TPartnerFamilyIDHandling.
     * Gives information if the operation was successful or failed.
     *****************************************************************************/

    [Serializable()]
    public enum TFamilyIDSuccessEnum
    {
        /// <summary>
        /// operation on family ID was successful
        /// </summary>
        fiSuccess,

        /// <summary>
        /// there was a warning by the operation on family ID
        /// </summary>
        fiWarning,

        /// <summary>
        /// there was an error by the operation on family ID
        /// </summary>
        fiError
    };

    /// <summary>
    /// handle the family ID
    /// this determines the parents of the family (id 0 and 1, and the children)
    /// </summary>
    public class TPartnerFamilyIDHandling : object
    {
        #region TPartnerFamilyIDHandling
        private TFamilyIDSuccessEnum RetrieveAvailableFamilyID(Int64 AFamilyPartnerKey,
            Int64 APersonPartnerKey,
            Int64 AFromFamilyPartnerKey,
            Boolean AListAvailableIDs,
            Int32 APreferredFamilyID,
            out Int32 ABestAvailableFamilyID,
            out string AProblemMessage,
            out ArrayList AAvailableFamilyIDs,
            TDBTransaction AReadTransaction)
        {
            TFamilyIDSuccessEnum ReturnValue;
            PPersonTable PersonDT;
            DataView PersonDV;
            PFamilyTable FamilyDT;
            PPersonRow PersonRow;
            DataRowView PersonRowView;
            int NumRecords;
            int Counter;
            int ArrayLength;
            int StartFamilyID;
            int TempFamilyID;
            Boolean BasicChecksDone;
            Boolean BestFamilyIDFound;
            Boolean FamilyIDExists;
            ArrayList UsedFamilyIDs;

            // initialize return value (will be set to warning or error if a problem occurs)
            ReturnValue = TFamilyIDSuccessEnum.fiSuccess;

            // initialize out Arguments
            ABestAvailableFamilyID = 0;
            AProblemMessage = "";
            AAvailableFamilyIDs = new ArrayList();

            // initialize local variables
            BestFamilyIDFound = false;
            BasicChecksDone = false;
            UsedFamilyIDs = new ArrayList();

            if (APersonPartnerKey != 0)
            {
                // Determine APreferredFamilyID from a Person with a specified family key
                if (APreferredFamilyID == -1)
                {
                    PersonDT = PPersonAccess.LoadByPrimaryKey(APersonPartnerKey, AReadTransaction);

                    if ((PersonDT.Rows.Count != 0) && (PersonDT[0].FamilyKey == AFromFamilyPartnerKey))
                    {
                        APreferredFamilyID = PersonDT[0].FamilyId;
                    }
                    else
                    {
                        AProblemMessage = "Person with Partner Key " + APersonPartnerKey.ToString("0000000000") + " and Family Key " +
                                          AFromFamilyPartnerKey.ToString("0000000000") + " does not exist!";
                        return TFamilyIDSuccessEnum.fiError;
                    }
                }
                else
                {
                    AProblemMessage = "The Preferred Family ID parameter must be set to -1 if APersonPartnerKey is specified!";
                    return TFamilyIDSuccessEnum.fiError;
                }
            }

            FamilyDT = PFamilyAccess.LoadByPrimaryKey(AFamilyPartnerKey, AReadTransaction);

            if (FamilyDT.Rows.Count != 0)
            {
                if (!AListAvailableIDs)
                {
                    // "Basic checks"  these need no upanddown traversal of already used FamilyID's (included only for speedup here)
                    NumRecords = PPersonAccess.CountViaPFamily(AFamilyPartnerKey, AReadTransaction);

                    if (NumRecords == 0)
                    {
                        // No family members found > return FamilyID 0
                        ABestAvailableFamilyID = 0;
                        BestFamilyIDFound = true;

                        if ((APreferredFamilyID != -1) && (APreferredFamilyID != 0))
                        {
                            AProblemMessage = "Preferred FamilyID " + APreferredFamilyID.ToString() +
                                              " cannot be taken. There are no persons in this family yet, so FamilyID 0 must be used!";
                            ReturnValue = TFamilyIDSuccessEnum.fiWarning;
                        }
                    }
                    else
                    {
                        if (APreferredFamilyID != -1)
                        {
                            PersonDT = PPersonAccess.LoadViaPFamily(AFamilyPartnerKey, AReadTransaction);
                            Counter = 0;
                            FamilyIDExists = false;

                            while ((Counter < PersonDT.Rows.Count) && ((!FamilyIDExists)))
                            {
                                if (PersonDT[Counter].FamilyId == APreferredFamilyID)
                                {
                                    FamilyIDExists = true;
                                }

                                Counter = Counter + 1;
                            }

                            if (!FamilyIDExists)
                            {
                                // The preferred FamilyID is available > return this FamilyID
                                BestFamilyIDFound = true;
                                ABestAvailableFamilyID = APreferredFamilyID;
                            }
                        }
                    }

                    BasicChecksDone = true;
                }

                // Available FamilyID not found yet, or a list of available FamilyID's is to be returned in any case
                if (AListAvailableIDs || ((!BestFamilyIDFound)))
                {
                    // Build list of used FamilyID's
                    PersonDT = PPersonAccess.LoadViaPFamily(AFamilyPartnerKey, AReadTransaction);

                    // sort found person records by old omss id, so we need to create a view
                    // TODO still needed? no omss anymore
                    PersonDV = new DataView(PersonDT);
                    PersonDV.Sort = PPersonTable.GetFamilyIdDBName() + " ASC";

                    for (Counter = 0; Counter <= PersonDV.Count - 1; Counter += 1)
                    {
                        PersonRowView = PersonDV[Counter];
                        PersonRow = (PPersonRow)PersonRowView.Row;

                        if ((!PersonRow.IsFamilyIdNull()))
                        {
                            UsedFamilyIDs.Add((object)PersonRow.FamilyId);
                        }
                    }

                    // Available FamilyID not found yet
                    if (!BestFamilyIDFound)
                    {
                        // "Basic checks"  these need no upanddown traversal of already used
                        // FamilyID's (here again, in case they were not done already)
                        if ((!BasicChecksDone))
                        {
                            if (UsedFamilyIDs.Count == 0)
                            {
                                // No family members found > return FamilyID 0
                                ABestAvailableFamilyID = 0;
                                BestFamilyIDFound = true;

                                if ((APreferredFamilyID != -1) && (APreferredFamilyID != 0))
                                {
                                    AProblemMessage = "Preferred FamilyID " + APreferredFamilyID.ToString() +
                                                      " cannot be taken. There are no persons in this family yet, so FamilyID 0 must be used!";
                                    ReturnValue = TFamilyIDSuccessEnum.fiWarning;
                                }
                            }

                            if (APreferredFamilyID != -1)
                            {
                                if (UsedFamilyIDs.BinarySearch((object)APreferredFamilyID) < 0)
                                {
                                    // The preferred FamilyID is available > return this FamilyID
                                    ABestAvailableFamilyID = APreferredFamilyID;
                                    BestFamilyIDFound = true;
                                }
                            }
                        }

                        // Available FamilyID still not found > look for available FamilyID by
                        // checking through the FamilyID's
                        if ((!BestFamilyIDFound))
                        {
                            // Special handling if a Parent FamilyID (0 or 1) are preferred
                            if ((APreferredFamilyID == 0) || (APreferredFamilyID == 1))
                            {
                                if (APreferredFamilyID == 0)
                                {
                                    TempFamilyID = 1;

                                    if (UsedFamilyIDs.BinarySearch((object)TempFamilyID) < 0)
                                    {
                                        // FamilyID 1 is available > return this FamilyID
                                        ABestAvailableFamilyID = 1;
                                        BestFamilyIDFound = true;
                                        AProblemMessage = "Preferred FamilyID 0 cannot be taken, it is already in use. " +
                                                          "Instead, the other Parent FamilyID 1 is used.";
                                        ReturnValue = TFamilyIDSuccessEnum.fiWarning;
                                    }
                                }

                                if (APreferredFamilyID == 1)
                                {
                                    TempFamilyID = 0;

                                    if (UsedFamilyIDs.BinarySearch((object)TempFamilyID) < 0)
                                    {
                                        // FamilyID 0 is available > return this FamilyID
                                        ABestAvailableFamilyID = 0;
                                        BestFamilyIDFound = true;
                                        AProblemMessage = "Preferred FamilyID 1 cannot be taken, it is already in use. " +
                                                          "Instead, the other Parent FamilyID 0 is used.";
                                        ReturnValue = TFamilyIDSuccessEnum.fiWarning;
                                    }
                                }
                            }

                            // We are either not looking for a Parent FamilyID, or a Parent FamilyID is unavailable
                            if ((!BestFamilyIDFound))
                            {
                                if (APreferredFamilyID == -1)
                                {
                                    StartFamilyID = 0;
                                }
                                else
                                {
                                    StartFamilyID = APreferredFamilyID + 1;
                                }

                                // Check FamilyID's upwards
                                for (Counter = StartFamilyID; Counter <= 9; Counter += 1)
                                {
                                    if (UsedFamilyIDs.BinarySearch((object)Counter) < 0)
                                    {
                                        ABestAvailableFamilyID = Counter;
                                        BestFamilyIDFound = true;

                                        if (APreferredFamilyID != -1)
                                        {
                                            AProblemMessage = "Preferred FamilyID " + APreferredFamilyID.ToString() +
                                                              " cannot be taken, it is already in use! Instead, FamilyID " +
                                                              ABestAvailableFamilyID.ToString() + " is used.";
                                            ReturnValue = TFamilyIDSuccessEnum.fiWarning;
                                        }

                                        // leave loop here if best ID was found
                                        if (BestFamilyIDFound)
                                        {
                                            break;
                                        }
                                    }
                                }

                                if (((!BestFamilyIDFound)) && (APreferredFamilyID > 2))
                                {
                                    for (Counter = APreferredFamilyID; Counter >= 2; Counter -= 1)
                                    {
                                        if (UsedFamilyIDs.BinarySearch((object)Counter) < 0)
                                        {
                                            ABestAvailableFamilyID = Counter;
                                            BestFamilyIDFound = true;

                                            if (APreferredFamilyID != -1)
                                            {
                                                AProblemMessage = "Preferred FamilyID " + APreferredFamilyID.ToString() +
                                                                  " cannot be taken, it is already in use! Instead, FamilyID " +
                                                                  APreferredFamilyID.ToString() +
                                                                  " is used.";
                                                ReturnValue = TFamilyIDSuccessEnum.fiWarning;
                                            }

                                            // leave loop here if best ID was found
                                            if (BestFamilyIDFound)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            // Available FamilyID still not found > determine available FamilyID
                            // by finding the highest FamilyID (could be higher than 9), then adding 1
                            if ((!BestFamilyIDFound))
                            {
                                // UsedFamilyIDs is sorted by FamilyID ascending, so the last entry is the highest!
                                ArrayLength = UsedFamilyIDs.Count;
                                ABestAvailableFamilyID = ((int)UsedFamilyIDs[ArrayLength - 1]) + 1;
                                AProblemMessage = "Since all FamilyID's betweeen 2 and 9 are already in use, FamilyID " +
                                                  ABestAvailableFamilyID.ToString() + " is used.";
                                ReturnValue = TFamilyIDSuccessEnum.fiWarning;
                            }
                        }
                    }

                    // Build list of available FamilyID's out of the used FamilyID's  if requested
                    if (AListAvailableIDs)
                    {
                        // object needs to be created
                        if (AAvailableFamilyIDs == null)
                        {
                            AAvailableFamilyIDs = new ArrayList();
                        }

                        for (Counter = 0; Counter <= 9; Counter += 1)
                        {
                            if (UsedFamilyIDs.BinarySearch((object)Counter) < 0)
                            {
                                AAvailableFamilyIDs.Add((object)Counter);
                            }
                        }
                    }
                }
            }
            else
            {
                AProblemMessage = "Family with Partner Key " + AFamilyPartnerKey.ToString("0000000000") + " does not exist!";
                return TFamilyIDSuccessEnum.fiError;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get a family ID for a new person in the family
        /// Simply a shortcut for calling the GetAvailableFamilyID function when just
        /// any new FamilyID is wanted and no previous FamilyID needs to be maintained.
        ///
        /// </summary>
        /// <param name="AFamilyPartnerKey">PartnerKey of the family</param>
        /// <param name="ANewFamilyID">new family ID if function was successful</param>
        /// <param name="AProblemMessage">error text if function was not successful</param>
        /// <returns>returns TFamilyIDSuccessEnum value to show if function was successful
        /// </returns>
        public TFamilyIDSuccessEnum GetNewFamilyID(Int64 AFamilyPartnerKey, out Int32 ANewFamilyID, out String AProblemMessage)
        {
            Int32 PreferredFamilyID = -1;

            // with this call there is no preference for the family ID
            return GetNewFamilyID(AFamilyPartnerKey, PreferredFamilyID, out ANewFamilyID, out AProblemMessage);
        }

        /// <summary>
        /// Get a family ID for a new person in the family
        /// Simply a shortcut for calling the GetAvailableFamilyID function when just
        /// any new FamilyID is wanted and no previous FamilyID needs to be maintained.
        ///
        /// </summary>
        /// <param name="AFamilyPartnerKey">PartnerKey of the family</param>
        /// <param name="APreferredFamilyID">preferred family ID</param>
        /// <param name="ANewFamilyID">new family ID if function was successful</param>
        /// <param name="AProblemMessage">error text if function was not successful</param>
        /// <returns>returns TFamilyIDSuccessEnum value to show if function was successful
        /// </returns>
        public TFamilyIDSuccessEnum GetNewFamilyID(Int64 AFamilyPartnerKey,
            Int32 APreferredFamilyID, out Int32 ANewFamilyID, out String AProblemMessage)
        {
            TFamilyIDSuccessEnum ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            PPersonTable PersonDT;
            DataView PersonDV;
            PPersonRow PersonRow;
            DataRowView PersonRowView;
            int Counter;

            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                ReturnValue = GetAvailableFamilyID(AFamilyPartnerKey,
                    0,
                    0,
                    APreferredFamilyID,
                    out ANewFamilyID,
                    out AProblemMessage,
                    ReadTransaction);

                if (ANewFamilyID == -1)
                {
                    // this case should actually not happen... (extend the problem message)
                    AProblemMessage = "There was a problem determining an available Family ID for Family " +
                                      AFamilyPartnerKey.ToString("0000000000") + "!\r\n" +
                                      " Please report the follwing to your System Administrator:\r\n" + AProblemMessage + "\r\n";

                    // Find highest FamilyID
                    PersonDT = PPersonAccess.LoadViaPFamily(AFamilyPartnerKey, ReadTransaction);

                    if (PersonDT.Rows.Count == 0)
                    {
                        // It is perfectly OK to have no person in a family. In this case we
                        // can return 0 as first FamilyID since this is the first person
                        ANewFamilyID = 0;
                        return ReturnValue;
                    }

                    // sort found person records by old omss id, so we need to create a view
                    // TODO still needed? no omss anymore
                    ANewFamilyID = 0;
                    PersonDV = new DataView(PersonDT);
                    PersonDV.Sort = PPersonTable.GetFamilyIdDBName() + " DESC";

                    for (Counter = 0; Counter <= PersonDV.Count - 1; Counter += 1)
                    {
                        PersonRowView = PersonDV[Counter];
                        PersonRow = (PPersonRow)PersonRowView.Row;

                        if ((!PersonRow.IsFamilyIdNull()))
                        {
                            // take the next highest one
                            ANewFamilyID = PersonRow.FamilyId + 1;
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();

                    if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_TRACE)
                    {
                        Console.WriteLine(this.GetType().FullName + ".GetNewFamilyID: committed own transaction.");
                    }
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Get a family ID for a new person in the family
        /// Simply a shortcut for calling the GetAvailableFamilyID function when just
        /// any new FamilyID is wanted and no previous FamilyID needs to be maintained.
        ///
        /// </summary>
        /// <param name="AFamilyPartnerKey">PartnerKey of the family</param>
        /// <param name="APreferredFamilyID">family ID that is aimed at being preserved</param>
        /// <param name="ANewFamilyID">new family ID if function was successful</param>
        /// <param name="AProblemMessage">error text if function was not successful</param>
        /// <returns>returns TFamilyIDSuccessEnum value to show if function was successful
        /// </returns>
        public TFamilyIDSuccessEnum GetNewFamilyID_FamilyChange(Int64 AFamilyPartnerKey,
            Int32 APreferredFamilyID,
            out Int32 ANewFamilyID,
            out String AProblemMessage)
        {
            TFamilyIDSuccessEnum ReturnValue;

            ReturnValue = GetNewFamilyID(AFamilyPartnerKey, APreferredFamilyID, out ANewFamilyID, out AProblemMessage);

            if (ReturnValue == TFamilyIDSuccessEnum.fiWarning)
            {
                AProblemMessage = String.Format(
                    Catalog.GetString("The Family ID that the Person had in the former Family could not be " +
                        "preserved in the Family that the Person has now joined.{0}{0}" +
                        "The reason is:{0}{0}" +
                        "{1}{0}{0}" +
                        "Former Family ID:  {2}{0}" +
                        "New Family ID:  {3}"),
                    "\r\n",
                    AProblemMessage,
                    APreferredFamilyID,
                    ANewFamilyID);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns an available FamilyID for a given Family according to certain rules
        /// and specified preferences
        ///
        /// The FamilyID is determined by first looking if the Family specified by
        /// AFamilyPartnerKey has any members yet. If it has no Family members yet,
        /// FamilyID 0 is returned (no matter if APreferredFamilyID was not -1
        /// or whether APersonPartnerKey was not 0; in these cases, a note is
        /// returned in AProblemMessage). If the Family already has got members: in case
        /// the preferred FamilyID (APreferredFamilyID) is not -1 or
        /// APersonPartnerKey is not 0 and the FamilyID specified by these is
        /// available in the given Family, this FamilyID is returned.
        /// If the FamilyID specified by these is not available (or APreferredFamilyID
        /// is -1 and APersonPartnerKey is 0), the next higher FamilyID's up to 9 are
        /// checked whether they are available, starting from the APreferredFamilyID + 1
        /// (or from 0 if it is -1). If one of these FamilyID's is available, this FamilyID
        /// is returned. If not, the next lower FamilyID's down to 2 are checked whether
        /// they are available. If one of these FamilyID's is available, this FamilyID is
        /// returned.
        /// If none of these FamilyID's is available, the next available higher FamilyID
        /// after 9 is determined and this + 1 is returned and a note in AProblemMessage.
        ///
        /// Note:
        /// If APreferredFamilyID is 0 or 1 and it is not available, it is
        /// first checked whether the other available FamilyID for parents (0 or 1) is
        /// available before the FamilyID's from 2 up to 9 are checked for availability.
        /// This tries to keep a person that preferres a FamilyID for parents to give it
        /// a FamilyID for parents in the given Family (AFamilyPartnerKey).
        ///
        /// Important:
        /// APersonPartnerKey and AFromFamilyPartnerKey must both be 0 if
        /// APreferredFamilyID is not -1! If the pv_preferred_family_id_i is -1,
        /// APersonPartnerKey and AFromFamilyPartnerKey must both be specified.
        /// In this case the current FamilyID of the p_person record specified by
        /// AFromFamilyPartnerKey and APersonPartnerKey is fetched. This FamilyID is
        /// then taken as APreferredFamilyID. This saves the calling procedure to
        /// determine the preferred FamilyID if it does it not have at hand.
        ///
        /// Further Notes:
        /// Calling this procedure with APreferredFamilyID set to -1 and
        /// APersonPartnerKey set to 0 simply finds the lowest free FamilyID, starting
        /// the search at 0.
        /// Calling this procedure with either APreferredFamilyID set to an FamilyID
        /// or APersonPartnerKey set to a Partner Key finds the closest matching FamilyID
        /// (up or down) of the FamilyID sumbitted.
        /// The procedure returns fiError if a serious error occured, otherwise an available
        /// FamilyID. The caller should always check the return code  - if
        /// it is fiSuccess the FamilyID found is equal to the submitted FamilyID (if none is
        /// submitted, return value is also fiSuccess). If, for any reason, the submitted
        /// FamilyKey is not available, fiWarning is returned plus further explanations
        /// for the user in AProblemMessage.
        /// NB: If the calling procedure wants to change the Family of a Partner and
        /// try to preserve this FamilyID in the new Family, the calling procedure can
        /// pass in the Partner's current FamilyID either with the APreferredFamilyID
        /// parameter or by specifying its APersonPartnerKey and AFromFamilyPartnerKey.
        ///
        /// </summary>
        /// <param name="AFamilyPartnerKey">Family Key of an already existing Family for which
        /// the available FamilyID should be retrieved. Mandatory!</param>
        /// <param name="APersonPartnerKey">Partner Key of the Person for which the available FamilyID
        /// is to be determined. Optional (set to 0 if you don't want to pass in a
        /// Partner Key).</param>
        /// <param name="AFromFamilyPartnerKey">Family Key where the Person is currently in -
        /// taken into consideration only if APartnerKey is not 0!</param>
        /// <param name="APreferredFamilyID">Set to a preferred FamilyID that should be returned,
        /// or to -1 if there is no preferred FamilyID. Must be -1 if APersonPartnerKey
        /// is specified</param>
        /// <param name="ABestAvailableFamilyID">The (best matching) available FamilyID in the
        /// Family specified AFamilyPartnerKey</param>
        /// <param name="AProblemMessage">Contains the error message that can be displayed if
        /// the return value of the function (TFamilyIDSuccessEnum) did not return
        /// a success value</param>
        /// <param name="AReadTransaction">Current TDBTransaction</param>
        /// <returns>returns TFamilyIDSuccessEnum value to show if function was successful
        /// </returns>
        public TFamilyIDSuccessEnum GetAvailableFamilyID(Int64 AFamilyPartnerKey,
            Int64 APersonPartnerKey,
            Int64 AFromFamilyPartnerKey,
            Int32 APreferredFamilyID,
            out Int32 ABestAvailableFamilyID,
            out string AProblemMessage,
            TDBTransaction AReadTransaction)
        {
            ArrayList TempList;

            return RetrieveAvailableFamilyID(AFamilyPartnerKey,
                APersonPartnerKey,
                AFromFamilyPartnerKey,
                false,
                APreferredFamilyID,
                out ABestAvailableFamilyID,
                out AProblemMessage,
                out TempList,
                AReadTransaction);
        }

        /// <summary>
        /// Returns an available FamilyID for a given Family according to certain rules
        /// and specified preferences
        ///
        /// Further Comments: see above overloaded function
        ///
        /// </summary>
        /// <param name="AFamilyPartnerKey">Family Key of an already existing Family for which
        /// the available FamilyID should be retrieved. Mandatory!</param>
        /// <param name="APersonPartnerKey">Partner Key of the Person for which the available FamilyID
        /// is to be determined. Optional (set to 0 if you don't want to pass in a
        /// Partner Key).</param>
        /// <param name="AFromFamilyPartnerKey">Family Key where the Person is currently in -
        /// taken into consideration only if APartnerKey is not 0!</param>
        /// <param name="APreferredFamilyID">Set to a preferred FamilyID that should be returned,
        /// or to -1 if there is no preferred FamilyID. Must be -1 if APersonPartnerKey
        /// is specified</param>
        /// <param name="ABestAvailableFamilyID">The (best matching) available FamilyID in the
        /// Family specified AFamilyPartnerKey</param>
        /// <param name="AProblemMessage">Contains the error message that can be displayed if
        /// the return value of the function (TFamilyIDSuccessEnum) did not return
        /// a success value</param>
        /// <param name="AAvailableFamilyIDs">A list of available FamilyID's</param>
        /// <param name="AReadTransaction">Current TDBTransaction</param>
        /// <returns>returns TFamilyIDSuccessEnum value to show if function was successful
        /// </returns>
        public TFamilyIDSuccessEnum GetAvailableFamilyID(Int64 AFamilyPartnerKey,
            Int64 APersonPartnerKey,
            Int64 AFromFamilyPartnerKey,
            Int32 APreferredFamilyID,
            out Int32 ABestAvailableFamilyID,
            out string AProblemMessage,
            out ArrayList AAvailableFamilyIDs,
            TDBTransaction AReadTransaction)
        {
            return RetrieveAvailableFamilyID(AFamilyPartnerKey,
                APersonPartnerKey,
                AFromFamilyPartnerKey,
                true,
                APreferredFamilyID,
                out ABestAvailableFamilyID,
                out AProblemMessage,
                out AAvailableFamilyIDs,
                AReadTransaction);
        }

        #endregion
    }

    /// <summary>
    /// class TRecentPartnersHandling
    ///
    /// </summary>
    public class TRecentPartnersHandling : object
    {
        #region TRecentPartnersHandling

        /// <summary>
        /// Add Key of a recently used partner to the table that holds the keys of the
        /// recent partners.
        /// </summary>
        /// <remarks>
        /// IMPORTANT: This Method has a built-in recovery mechanism that allows it to
        /// recover from an Exception that can be thrown due to the 'Predicate Locking' implementation
        /// in PostgreSQL. If the Method makes attempts to recover from that, it
        /// ROLLS BACK THE CURRENT DB TRANSACTION so that the next SQL Command can succeed. The Method will make
        /// MAX_SUBMIT_RETRIES attempts. If the Method was called while a DB Transaction was already running
        /// then this Method will have ROLLED BACK that DB Transaction in such a case and the Method
        /// will return false!!! If the caller cares about that (i.e. if it was writing to the DB in that
        /// DB Transaction) then the caller needs to re-do the writing to the DB if this Method returns 'false'
        /// as that DB Transaction was just rolled back!
        /// </remarks>
        /// <param name="APartnerKey">Key of the partner that was recently used</param>
        /// <param name="APartnerClass"></param>
        /// <param name="ANewPartner">Indicate if the partner is a new partner</param>
        /// <param name="ALastPartnerUse">Where is the partner used?</param>
        /// <returns>returns true if handling was successful
        /// </returns>
        public static bool AddRecentlyUsedPartner(Int64 APartnerKey, TPartnerClass APartnerClass,
            bool ANewPartner, TLastPartnerUse ALastPartnerUse)
        {
            const int MAX_SUBMIT_RETRIES = 5;

            Boolean ReturnValue;
            TDBTransaction ReadAndWriteTransaction;
            Boolean NewTransaction = false;
            PPartnerTable PartnerDT;
            PRecentPartnersTable RecentPartnersDT;
            DataRowView RecentPartnersRowView;
            PRecentPartnersRow RecentPartnersRow;
            PPartnerRow PartnerRow;
            DataView RecentPartnersDV;
            string PartnerClassString;
            int Counter;
            int ClassCounter;
            int NumberOfRecentPartners;
            StringCollection FieldList;
            int SubmitRetries = 0;
            bool SubmitSuccessful = false;

            // initialize result
            ReturnValue = true;

            try
            {
                ReadAndWriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum, out NewTransaction);

                if (!ANewPartner)
                {
                    PartnerDT = PPartnerAccess.LoadByPrimaryKey(APartnerKey, ReadAndWriteTransaction);

                    if (PartnerDT.Rows.Count == 0)
                    {
                        // Somehow the partner does not exist, don't add this and return here already ...
                        return false;
                    }
                }

                // At this point the partner is either new or it is not new but in existing in the db
                // Now get the class of the recently used partner.
                PartnerClassString = SharedTypes.PartnerClassEnumToString(APartnerClass);
                RecentPartnersDT = PRecentPartnersAccess.LoadViaSUser(UserInfo.GUserInfo.UserID, ReadAndWriteTransaction);

                // Check if the recently used partner already exists for the current user.
                // Add it if not, otherwise just change the timestamp.
                RecentPartnersRow = (PRecentPartnersRow)RecentPartnersDT.Rows.Find(new System.Object[] { UserInfo.GUserInfo.UserID, APartnerKey });

                if (RecentPartnersRow == null)
                {
                    RecentPartnersRow = RecentPartnersDT.NewRowTyped(true);
                    RecentPartnersRow.UserId = UserInfo.GUserInfo.UserID;
                    RecentPartnersRow.PartnerKey = APartnerKey;
                    RecentPartnersDT.Rows.Add(RecentPartnersRow);
                }

                // Change the Timestamp
                RecentPartnersRow.whenDate = DateTime.Now;
                RecentPartnersRow.whenTime = Conversions.DateTimeToInt32Time(DateTime.Now);
                RecentPartnersDV = new DataView(RecentPartnersDT);
                RecentPartnersDV.Sort = PRecentPartnersTable.GetwhenDateDBName() + " ASC, " + PRecentPartnersTable.GetwhenTimeDBName() + " ASC";
                FieldList = new StringCollection();
                FieldList.Add(PPartnerTable.GetPartnerKeyDBName());
                FieldList.Add(PPartnerTable.GetPartnerClassDBName());
                PartnerDT = PPartnerAccess.LoadViaSUserPRecentPartners(UserInfo.GUserInfo.UserID, FieldList, ReadAndWriteTransaction, null, 0, 0);

                // Get the number of recent partners that the user has set, if not found
                // take 10 as default value.
                NumberOfRecentPartners = TUserDefaults.GetInt16Default(MSysManConstants.USERDEFAULT_NUMBEROFRECENTPARTNERS, 10);
                ClassCounter = 0;

                for (Counter = RecentPartnersDV.Count - 1; Counter >= 0; Counter -= 1)
                {
                    RecentPartnersRowView = RecentPartnersDV[Counter];
                    RecentPartnersRow = (PRecentPartnersRow)RecentPartnersRowView.Row;

                    if (APartnerKey == RecentPartnersRow.PartnerKey)
                    {
                        // we don't need to check the added partner (was done already)
                        ClassCounter = ClassCounter + 1;
                    }
                    else
                    {
                        // For each recent partner we need to find out if the class matches the
                        // class of the added recent partner. Look in the table that we loaded
                        // earlier on.
                        PartnerRow = (PPartnerRow)PartnerDT.Rows.Find(RecentPartnersRow.PartnerKey);

                        if (PartnerRow == null)
                        {
                            // Partner not found. Should not happen so we delete this record to prevent
                            // future problems.
                            RecentPartnersRow.Delete();
                            continue;
                        }

                        // If the recent partners record is of a different class then don't do
                        // anything with this record.
                        if (PartnerRow.PartnerClass != PartnerClassString)
                        {
                            continue;
                        }
                        else
                        {
                            ClassCounter = ClassCounter + 1;
                        }
                    }

                    if (ClassCounter > NumberOfRecentPartners)
                    {
                        // Only keep the number of records of a certain class that the user
                        // wants to keep (find in User Defaults, otherwise use 10)
                        RecentPartnersRow.Delete();
                    }
                }

                while ((SubmitRetries < MAX_SUBMIT_RETRIES)
                       && (!SubmitSuccessful))
                {
                    try
                    {
                        // now submit the changes to the database
                        PRecentPartnersAccess.SubmitChanges(RecentPartnersDT, ReadAndWriteTransaction);

                        SubmitSuccessful = true;
                    }
                    catch (Npgsql.NpgsqlException Exc)
                    {
                        // Check if we ran into the error 'could not serialize access due to read/write dependencies among transactions'
                        // which has error code 40001. This is due to the 'Predicate Locking' implementation in PostgreSQL.
                        // If so, retry, as this should overcome the error condition!
                        // (See http://stackoverflow.com/questions/12837708/predicate-locking-in-postgresql-9-2-1-with-serializable-isolation)
                        // That error has been encountered with the 'PetraMultiStart' test program where many clients may open a Partner Edit
                        // at nearly the same time, but it could be encountered in a 'real office scenario' as well when two users open a
                        // Partner Edit screen at nearly the same time.
                        if (String.Compare(Exc.Code, "40001") == 0)
                        {
//                            TLogging.LogAtLevel(0, "TRecentPartnersHandling.AddRecentlyUsedPartner: We need to retry issuing SubmitChanges as the RDBMS suggested to do that when 'Predicate Locking' failed (PostgreSQL Error Code 40001).");

//                            TLogging.LogAtLevel(0, "TRecentPartnersHandling.AddRecentlyUsedPartner: rolling back DB Transaction to allow further SQL Commands to succeed");
                            DBAccess.GDBAccessObj.RollbackTransaction();
//                            TLogging.LogAtLevel(0, "TRecentPartnersHandling.AddRecentlyUsedPartner: rolled back DB Transaction, now starting a new DB Transaction");

                            // Let the caller of this Method know that the Method has rolled back the DB Transaction that was started outside of this Method.
                            // If the caller cares about that (i.e. if it was writing to the DB in that DB Transaction) then the caller needs to re-do the
                            // writing to the DB if this Method returns 'false' as that DB Transaction was just rolled back!
                            if (!NewTransaction)
                            {
                                ReturnValue = false;
                            }

                            SubmitRetries++;

                            ReadAndWriteTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted, 2);
//                            TLogging.LogAtLevel(0, "TRecentPartnersHandling.AddRecentlyUsedPartner: successfully started a new DB Transaction, now retrying SubmitChanges (Retry attempt number: " + SubmitRetries.ToString() + ")...");

                            // Now let the while statement retry the submitting!
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if ((SubmitSuccessful)
                            && (SubmitRetries > 0))
                        {
                            TLogging.LogAtLevel(0,
                                "TRecentPartnersHandling.AddRecentlyUsedPartner: SubmitChanges was successful after " + SubmitRetries.ToString() +
                                " retry attempts (we recovered from the 'Predicate Locking' error that PostgreSQL issued [PostgreSQL Error Code 40001])!");
                        }
                        else if (SubmitRetries > MAX_SUBMIT_RETRIES)
                        {
                            TLogging.LogAtLevel(0,
                                "TRecentPartnersHandling.AddRecentlyUsedPartner: SubmitChanges was NOT successful after " +
                                SubmitRetries.ToString() +
                                " retry attempts (we FAILED TO RECOVER from the 'Predicate Locking' error that PostgreSQL issued [PostgreSQL Error Code 40001])!");
                        }
                    }
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();

                    TLogging.LogAtLevel(4, "TRecentPartnersHandling.AddRecentlyUsedPartner: committed own transaction.");
                }
            }

            return ReturnValue;
        }

        #endregion
    }

    /// <summary>
    /// class TFamilyHandling
    /// </summary>
    public class TFamilyHandling : object
    {
        #region TFamilyHandling

        /// <summary>
        /// performs database changes to move person from current (old) family to new family record
        /// </summary>
        /// <param name="APersonKey"></param>
        /// <param name="AOldFamilyKey"></param>
        /// <param name="ANewFamilyKey"></param>
        /// <param name="AProblemMessage"></param>
        /// <returns>true if change of family completed successfully</returns>
        public static bool ChangeFamily(Int64 APersonKey, Int64 AOldFamilyKey, Int64 ANewFamilyKey,
            out String AProblemMessage)
        {
            bool Result = true;
            PFamilyTable OldFamilyDT;
            PFamilyTable NewFamilyDT;
            PPersonTable PersonDT;
            PPartnerRelationshipTable RelationshipDT;
            PPartnerRelationshipRow RelationshipRow;
            TPartnerFamilyIDHandling FamilyIDHandling;
            Int32 NewFamilyID;
            string ProblemMessage = string.Empty;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref SubmissionOK,
                delegate
                {
                    try
                    {
                        PersonDT = PPersonAccess.LoadByPrimaryKey(APersonKey, Transaction);

                        FamilyIDHandling = new TPartnerFamilyIDHandling();

                        if (FamilyIDHandling.GetNewFamilyID_FamilyChange(ANewFamilyKey, PersonDT[0].FamilyId, out NewFamilyID,
                                out ProblemMessage) == TFamilyIDSuccessEnum.fiError)
                        {
                            // this should not really happen  but we cannot continue if it does
                            Result = false;
                        }

                        if (Result)
                        {
                            // reset family id and family key for person
                            PersonDT[0].FamilyId = NewFamilyID;
                            PersonDT[0].FamilyKey = ANewFamilyKey;

                            PPersonAccess.SubmitChanges(PersonDT, Transaction);

                            // reset family members flag in old family if this person was the last family member
                            if (PPersonAccess.CountViaPFamily(AOldFamilyKey, Transaction) == 0)
                            {
                                OldFamilyDT = PFamilyAccess.LoadByPrimaryKey(AOldFamilyKey, Transaction);

                                OldFamilyDT[0].FamilyMembers = false;

                                PFamilyAccess.SubmitChanges(OldFamilyDT, Transaction);
                            }

                            // remove relationships between person and old family
                            PPartnerRelationshipAccess.DeleteByPrimaryKey(AOldFamilyKey, "FAMILY", APersonKey, Transaction);

                            // set family members flag for new family as there is now at least one member
                            NewFamilyDT = PFamilyAccess.LoadByPrimaryKey(ANewFamilyKey, Transaction);
                            NewFamilyDT[0].FamilyMembers = true;
                            PFamilyAccess.SubmitChanges(NewFamilyDT, Transaction);

                            // create relationship between person and new family
                            if ((!PPartnerRelationshipAccess.Exists(ANewFamilyKey, "FAMILY", APersonKey, Transaction)))
                            {
                                RelationshipDT = new PPartnerRelationshipTable();
                                RelationshipRow = RelationshipDT.NewRowTyped(true);
                                RelationshipRow.PartnerKey = ANewFamilyKey;
                                RelationshipRow.RelationKey = APersonKey;
                                RelationshipRow.RelationName = "FAMILY";
                                RelationshipRow.Comment = "System Generated";

                                RelationshipDT.Rows.Add(RelationshipRow);

                                PPartnerRelationshipAccess.SubmitChanges(RelationshipDT, Transaction);
                            }
                        }

                        if (Result)
                        {
                            SubmissionOK = true;
                        }
                    }
                    catch (Exception Exc)
                    {
                        TLogging.Log("An Exception occured during a change of a Family:" + Environment.NewLine + Exc.ToString());
                    }
                });

            AProblemMessage = ProblemMessage;

            return Result;
        }

        #endregion
    }
}