//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2020 by OM International
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
using System.Data.Odbc;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MSponsorship.Data;
using Ict.Petra.Server.MSponsorship.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSponsorship.WebConnectors
{

    /// <summary>
    /// webconnector for the sponsorship module
    /// </summary>
    public class TSponsorshipWebConnector
    {
        private const string TYPE_SPONSOREDCHILD = "SPONSOREDCHILD";

        /// <summary>
        /// find children using filters
        /// </summary>
        [RequireModulePermission("OR(SPONSORVIEW,SPONSORADMIN)")]
        public static SponsorshipFindTDSSearchResultTable FindChildren(
            string AFirstName,
            string AFamilyName,
            string APartnerStatus,
            string ASponsorshipStatus,
            string ASponsorAdmin)
        {
            TDBTransaction t = new TDBTransaction();
            TDataBase db = DBAccess.Connect("FindChildren");
            string sql = "SELECT p.p_partner_short_name_c, p.p_status_code_c, p.p_partner_key_n, p.p_user_id_c, " +
                "f.p_first_name_c, f.p_family_name_c, t.p_type_code_c " +
                "FROM PUB_p_partner p, PUB_p_family f, PUB_p_partner_type t " +
                "WHERE p.p_partner_key_n = f.p_partner_key_n " +
                "AND p.p_partner_key_n = t.p_partner_key_n";

            int CountParameters = 0;
            int Pos = 0;
            CountParameters += (AFirstName != String.Empty ? 1: 0);
            CountParameters += (ASponsorshipStatus != String.Empty ? 1: 0);
            CountParameters += (AFamilyName != String.Empty ? 1: 0);
            CountParameters += (ASponsorAdmin != String.Empty ? 1: 0);
            OdbcParameter[] parameters = new OdbcParameter[CountParameters];

            if (ASponsorshipStatus != String.Empty)
            {
                sql += " AND t.p_type_code_c = ?";
                parameters[Pos] = new OdbcParameter("ASponsorshipStatus", OdbcType.VarChar);
                parameters[Pos].Value = ASponsorshipStatus;
                Pos++;
            } else {
                sql += " AND t.p_type_code_c IN ('CHILDREN_HOME','HOME_BASED','BORDING_SCHOOL','PREVIOUS_CHILD')";
            }

            if (AFirstName != String.Empty)
            {
                sql += " AND f.p_first_name_c LIKE ?";
                parameters[Pos] = new OdbcParameter("FirstName", OdbcType.VarChar);
                parameters[Pos].Value = AFirstName;
                Pos++;
            }

            if (AFamilyName != String.Empty)
            {
                sql += " AND f.p_last_name_c LIKE ?";
                parameters[Pos] = new OdbcParameter("AFamilyName", OdbcType.VarChar);
                parameters[Pos].Value = AFamilyName;
                Pos++;
            }

            if (ASponsorAdmin != String.Empty)
            {
                sql += " AND '' LIKE ?";
                parameters[Pos] = new OdbcParameter("ASponsorAdmin", OdbcType.VarChar);
                parameters[Pos].Value = ASponsorAdmin;
                Pos++;
            }
            SponsorshipFindTDSSearchResultTable result = new SponsorshipFindTDSSearchResultTable();

            db.ReadTransaction(ref t,
                delegate
                {
                    db.SelectDT(result, sql, t, parameters);
                });

            db.CloseDBConnection();

            return result;
        }

        /// <summary>
        /// get a partner key for a new partner
        /// </summary>
        /// <param name="AFieldPartnerKey">can be -1, then the default site key is used</param>
        private static Int64 NewPartnerKey(Int64 AFieldPartnerKey = -1)
        {
            Int64 NewPartnerKey = TNewPartnerKey.GetNewPartnerKey(AFieldPartnerKey);

            TNewPartnerKey.SubmitNewPartnerKey(NewPartnerKey - NewPartnerKey % 1000000, NewPartnerKey, ref NewPartnerKey);
            return NewPartnerKey;
        }

        /// <summary>
        /// return the dataset for a new child
        /// </summary>
        [RequireModulePermission("SPONSORADMIN")]
        private static SponsorshipTDS CreateNewChild()
        {
            Int64 SiteKey = DomainManager.GSiteKey;
            SponsorshipTDS MainDS = new SponsorshipTDS();
            Int64 PartnerKey = NewPartnerKey();
            DateTime CreationDate = DateTime.Today;
            string CreationUserID = UserInfo.GetUserInfo().UserID;

	    // Create DataRow for Partner using the default values for all DataColumns
	    // and then modify some.
	    PPartnerRow PartnerRow = MainDS.PPartner.NewRowTyped(true);
	    PartnerRow.PartnerKey = PartnerKey;
	    PartnerRow.DateCreated = CreationDate;
	    PartnerRow.CreatedBy = CreationUserID;
	    PartnerRow.PartnerClass = SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY);
	    PartnerRow.StatusCode = SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE);
	    PartnerRow.UserId = CreationUserID;
            MainDS.PPartner.Rows.Add(PartnerRow);

            PFamilyRow FamilyRow = MainDS.PFamily.NewRowTyped(true);
            FamilyRow.PartnerKey = PartnerKey;
            FamilyRow.DateCreated = CreationDate;
            FamilyRow.CreatedBy = CreationUserID;
            MainDS.PFamily.Rows.Add(FamilyRow);

            PPartnerTypeRow PartnerTypeRow = MainDS.PPartnerType.NewRowTyped(true);
            PartnerTypeRow.PartnerKey = PartnerKey;
            PartnerTypeRow.TypeCode = TYPE_SPONSOREDCHILD;
            MainDS.PPartnerType.Rows.Add(PartnerTypeRow);

            return MainDS;
        }

        /// returns the number of the recurring gift batch
        private static Int32 GetRecurringGiftBatchForSponsorship(Int32 ALedgerNumber, bool ACreateBatch, TDBTransaction ATransaction = null)
        {
            const string BATCHNAME_SPONSORSHIP = "SPONSORSHIP";
            string sql = "SELECT MAX(a_batch_number_i) FROM a_recurring_gift_batch WHERE a_ledger_number_i = " + ALedgerNumber.ToString() +
                " AND a_batch_description_c LIKE '" + BATCHNAME_SPONSORSHIP + "'";
            object result = ATransaction.DataBaseObj.ExecuteScalar(sql, ATransaction);

            // if no batch exists, the result will be System.DBNull
            if (result is Int32)
            {
                return Convert.ToInt32(result);
            }

            if (ACreateBatch)
            {
                TDataBase db = null;

                if (ATransaction == null)
                {
                    db = DBAccess.Connect("GetRecurringGiftBatchForSponsorship");
                }
                else
                {
                    db = ATransaction.DataBaseObj;
                }

                SponsorshipTDS MainDS = new SponsorshipTDS();
                ARecurringGiftBatchRow b = MainDS.ARecurringGiftBatch.NewRowTyped(true);
                b.BatchDescription = BATCHNAME_SPONSORSHIP;
                MainDS.ARecurringGiftBatch.Rows.Add(b);
                SponsorshipTDSAccess.SubmitChanges(MainDS, db);

                if (ATransaction == null)
                {
                    db.CloseDBConnection();
                }

                return MainDS.ARecurringGiftBatch[0].BatchNumber;
            }

            return -1;
        }

        /// <summary>
        /// return the existing data of a child
        /// </summary>
        [RequireModulePermission("OR(SPONSORVIEW,SPONSORADMIN)")]
        public static SponsorshipTDS GetChildDetails(Int64 APartnerKey,
            Int32 ALedgerNumber,
            out string ASponsorshipStatus)
        {
            SponsorshipTDS MainDS = new SponsorshipTDS();

            TDBTransaction Transaction = new TDBTransaction();

            DBAccess.ReadTransaction( ref Transaction,
                delegate
                {
                    PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                    PFamilyAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                    PPartnerTypeAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                    PPartnerCommentAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);

                    int SponsorshipBatchNumber = GetRecurringGiftBatchForSponsorship(ALedgerNumber, true, Transaction);

                    if (SponsorshipBatchNumber > -1)
                    {
                        ARecurringGiftAccess.LoadViaARecurringGiftBatch(MainDS, ALedgerNumber, SponsorshipBatchNumber, Transaction);
                        ARecurringGiftDetailAccess.LoadViaARecurringGiftBatch(MainDS, ALedgerNumber, SponsorshipBatchNumber, Transaction);

                        foreach (SponsorshipTDSARecurringGiftDetailRow gd in MainDS.ARecurringGiftDetail.Rows)
                        {
                            // drop all recurring gift details, that are not related to this child (RecipientKey)
                            if (gd.RecipientKey != APartnerKey)
                            {
                                MainDS.ARecurringGiftDetail.Rows.Remove(gd);
                            }
                            else
                            {
                                // set the donor key from the appropriate recurring gift
                                MainDS.ARecurringGift.DefaultView.RowFilter = String.Format("{0} = {1}",
                                    ARecurringGiftTable.GetGiftTransactionNumberDBName(),
                                    gd.GiftTransactionNumber);

                                // there should be only one row
                                foreach (DataRowView drv in MainDS.ARecurringGift.DefaultView)
                                {
                                    ARecurringGiftRow recurrGiftRow = (ARecurringGiftRow)drv.Row;
                                    gd.DonorKey = recurrGiftRow.DonorKey;
                                }
                            }
                        }

                        // drop all unrelated gift rows, that don't have a detail for this child
                        foreach (ARecurringGiftRow gr in MainDS.ARecurringGift.Rows)
                        {
                            MainDS.ARecurringGiftDetail.DefaultView.RowFilter = String.Format("{0} = {1}",
                                ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                                gr.GiftTransactionNumber);

                            if (MainDS.ARecurringGiftDetail.DefaultView.Count == 0)
                            {
                                MainDS.ARecurringGift.Rows.Remove(gr);
                            }
                        }
                    }

                });

            bool isSponsoredChild = false;
            ASponsorshipStatus = "[N/A]";

            foreach (PPartnerTypeRow type in MainDS.PPartnerType.Rows)
            {
                if (type.TypeCode == "CHILDREN_HOME" || type.TypeCode == "HOME_BASED" || type.TypeCode == "BORDING_SCHOOL" || type.TypeCode == "PREVIOUS_CHILD")
                {
                    isSponsoredChild = true;
                }
            	ASponsorshipStatus = type.TypeCode;
            }

            if (!isSponsoredChild)
            {
                return new SponsorshipTDS();
            }

            return MainDS;
        }

        /// <summary>
        /// store the currently edited child
        /// </summary>
        [RequireModulePermission("SPONSORADMIN")]
        public static bool MaintainChild(
            string ASponsorshipStatus,
            string AFirstName,
            string AFamilyName,
            DateTime? ADateOfBirth,
            string APhoto,
            bool AUploadPhoto,
            string AGender,
            string AUserId,
            Int64 APartnerKey,
            Int32 ALedgerNumber,
            out TVerificationResultCollection AVerificationResult)
        {

            SponsorshipTDS CurrentEdit;
            AVerificationResult = new TVerificationResultCollection();

            if (APartnerKey == -1)
            {
                // no partner key given, so we make a new entry
                CurrentEdit = CreateNewChild();
                if (CurrentEdit.PFamily.Count > 0) { CurrentEdit.PFamily[0].PartnerKey = CurrentEdit.PPartner[0].PartnerKey; }
            }
            else
            {
                // else we try to get a entry based on the partner key
                string dummy = "0";
                CurrentEdit = GetChildDetails(APartnerKey, ALedgerNumber, out dummy);
            }


            // we only save pictures if there is a value in the request
            if (AUploadPhoto)
            {
                CurrentEdit.PFamily[0].Photo = APhoto;
            }
            else
            {
                CurrentEdit.PFamily[0].FirstName = AFirstName;
                CurrentEdit.PFamily[0].FamilyName = AFamilyName;
                CurrentEdit.PFamily[0].DateOfBirth = ADateOfBirth;
                CurrentEdit.PFamily[0].Gender = AGender;
                CurrentEdit.PPartner[0].UserId = AUserId;

                // only on a actuall change, else skip this
                if (ASponsorshipStatus != CurrentEdit.PPartnerType[0].TypeCode)
                {
                    foreach (PPartnerTypeRow OldTypeRow in CurrentEdit.PPartnerType.Rows)
                    {
                        OldTypeRow.Delete();
                    }

                    PPartnerTypeRow NewTypeRow = CurrentEdit.PPartnerType.NewRowTyped(true);

                    NewTypeRow.TypeCode = ASponsorshipStatus;
                    NewTypeRow.PartnerKey = CurrentEdit.PPartner[0].PartnerKey;

                    CurrentEdit.PPartnerType.Rows.Add(NewTypeRow);
                }

            }

            CurrentEdit.PPartner[0].PartnerShortName =
                    Calculations.DeterminePartnerShortName(
                        CurrentEdit.PFamily[0].FamilyName,
                        CurrentEdit.PFamily[0].Title,
                        CurrentEdit.PFamily[0].FirstName);

            // TODO update or insert recurring gifts 

            try
            {
                SponsorshipTDSAccess.SubmitChanges(CurrentEdit);
                return true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                AVerificationResult.Add(new TVerificationResult("error", e.Message, TResultSeverity.Resv_Critical));
                return false;
            }
        }

        /// maintain comments about the child
        [RequireModulePermission("SPONSORADMIN")]
        public static bool MaintainChildComments(
            string AComment,
            string ACommentType,
            Int32 AIndex,
            Int64 APartnerKey,
            out TVerificationResultCollection AVerificationResult)
        {

            SponsorshipTDS CurrentEdit;
            AVerificationResult = new TVerificationResultCollection();

            if (APartnerKey == -1)
            {
              AVerificationResult.Add(new TVerificationResult("error", "no parnter key", TResultSeverity.Resv_Critical));
              return false;
            }

            string dummy = "";
            CurrentEdit = GetChildDetails(APartnerKey, out dummy);

            PPartnerCommentRow EditCommentRow = null;

            // since we get a index from the user (and i don't know how to request one of them)
            // we loop over all comments, edit it if we found it, and if it's null we add a new one
            foreach (PPartnerCommentRow CommentRow in CurrentEdit.PPartnerComment.Rows)
            {
                if (CommentRow.Index == AIndex)
                {
                    EditCommentRow = CommentRow;
                    break;
                }
            }

            // edit
            if (EditCommentRow != null)
            {
                EditCommentRow.Comment = AComment;
            }
            else
            {
                PPartnerCommentRow NewCommentRow = CurrentEdit.PPartnerComment.NewRowTyped(true);
                NewCommentRow.PartnerKey = APartnerKey;
                NewCommentRow.Comment = AComment;
                NewCommentRow.CommentType = ACommentType;
                NewCommentRow.Index = AIndex;
                NewCommentRow.Sequence = 0; // nobody cares about this value but it must be set
                CurrentEdit.PPartnerComment.Rows.Add( NewCommentRow );
            }

            try
            {
                SponsorshipTDSAccess.SubmitChanges(CurrentEdit);
                return true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                AVerificationResult.Add(new TVerificationResult("error", e.Message, TResultSeverity.Resv_Critical));
                return false;
            }
        }


    }
}
