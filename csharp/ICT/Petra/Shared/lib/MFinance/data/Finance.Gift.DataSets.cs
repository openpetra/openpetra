/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
namespace Ict.Petra.Shared.MFinance.Gift.Data
{
    using Ict.Common;
    using Ict.Common.Data;
    using System;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Petra.Shared.MFinance.Gift.Data;
    using Ict.Petra.Shared.MFinance.Account.Data;

     /// auto generated
    [Serializable()]
    public class GiftBatchTDS : TTypedDataSet
    {

        private ALedgerTable TableALedger;
        private AGiftBatchTable TableAGiftBatch;
        private AGiftTable TableAGift;
        private GiftBatchTDSAGiftDetailTable TableAGiftDetail;
        private AMotivationDetailTable TableAMotivationDetail;
        private AMotivationGroupTable TableAMotivationGroup;

        /// auto generated
        public GiftBatchTDS() :
                base("GiftBatchTDS")
        {
        }

        /// auto generated for serialization
        public GiftBatchTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public GiftBatchTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public ALedgerTable ALedger
        {
            get
            {
                return this.TableALedger;
            }
        }

        /// auto generated
        public AGiftBatchTable AGiftBatch
        {
            get
            {
                return this.TableAGiftBatch;
            }
        }

        /// auto generated
        public AGiftTable AGift
        {
            get
            {
                return this.TableAGift;
            }
        }

        /// auto generated
        public GiftBatchTDSAGiftDetailTable AGiftDetail
        {
            get
            {
                return this.TableAGiftDetail;
            }
        }

        /// auto generated
        public AMotivationDetailTable AMotivationDetail
        {
            get
            {
                return this.TableAMotivationDetail;
            }
        }

        /// auto generated
        public AMotivationGroupTable AMotivationGroup
        {
            get
            {
                return this.TableAMotivationGroup;
            }
        }

        /// auto generated
        public new virtual GiftBatchTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((GiftBatchTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new ALedgerTable("ALedger"));
            this.Tables.Add(new AGiftBatchTable("AGiftBatch"));
            this.Tables.Add(new AGiftTable("AGift"));
            this.Tables.Add(new GiftBatchTDSAGiftDetailTable("AGiftDetail"));
            this.Tables.Add(new AMotivationDetailTable("AMotivationDetail"));
            this.Tables.Add(new AMotivationGroupTable("AMotivationGroup"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("ALedger") != -1))
            {
                this.Tables.Add(new ALedgerTable("ALedger"));
            }
            if ((ds.Tables.IndexOf("AGiftBatch") != -1))
            {
                this.Tables.Add(new AGiftBatchTable("AGiftBatch"));
            }
            if ((ds.Tables.IndexOf("AGift") != -1))
            {
                this.Tables.Add(new AGiftTable("AGift"));
            }
            if ((ds.Tables.IndexOf("AGiftDetail") != -1))
            {
                this.Tables.Add(new GiftBatchTDSAGiftDetailTable("AGiftDetail"));
            }
            if ((ds.Tables.IndexOf("AMotivationDetail") != -1))
            {
                this.Tables.Add(new AMotivationDetailTable("AMotivationDetail"));
            }
            if ((ds.Tables.IndexOf("AMotivationGroup") != -1))
            {
                this.Tables.Add(new AMotivationGroupTable("AMotivationGroup"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableALedger != null))
            {
                this.TableALedger.InitVars();
            }
            if ((this.TableAGiftBatch != null))
            {
                this.TableAGiftBatch.InitVars();
            }
            if ((this.TableAGift != null))
            {
                this.TableAGift.InitVars();
            }
            if ((this.TableAGiftDetail != null))
            {
                this.TableAGiftDetail.InitVars();
            }
            if ((this.TableAMotivationDetail != null))
            {
                this.TableAMotivationDetail.InitVars();
            }
            if ((this.TableAMotivationGroup != null))
            {
                this.TableAMotivationGroup.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "GiftBatchTDS";
            this.TableALedger = ((ALedgerTable)(this.Tables["ALedger"]));
            this.TableAGiftBatch = ((AGiftBatchTable)(this.Tables["AGiftBatch"]));
            this.TableAGift = ((AGiftTable)(this.Tables["AGift"]));
            this.TableAGiftDetail = ((GiftBatchTDSAGiftDetailTable)(this.Tables["AGiftDetail"]));
            this.TableAMotivationDetail = ((AMotivationDetailTable)(this.Tables["AMotivationDetail"]));
            this.TableAMotivationGroup = ((AMotivationGroupTable)(this.Tables["AMotivationGroup"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {

            if (((this.TableAGiftBatch != null)
                        && (this.TableAGift != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGift1", "AGiftBatch", new string[] {
                                "a_ledger_number_i", "a_batch_number_i"}, "AGift", new string[] {
                                "a_ledger_number_i", "a_batch_number_i"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableAGiftBatch != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGiftBatch1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AGiftBatch", new string[] {
                                "a_ledger_number_i"}));
            }
            if (((this.TableAGift != null)
                        && (this.TableAGiftDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGiftDetail1", "AGift", new string[] {
                                "a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"}, "AGiftDetail", new string[] {
                                "a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"}));
            }
            if (((this.TableAMotivationDetail != null)
                        && (this.TableAGiftDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGiftDetail2", "AMotivationDetail", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"}, "AGiftDetail", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"}));
            }
            if (((this.TableAMotivationGroup != null)
                        && (this.TableAMotivationDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKMotivationDetail1", "AMotivationGroup", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c"}, "AMotivationDetail", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c"}));
            }
            if (((this.TableALedger != null)
                        && (this.TableAMotivationGroup != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKMotivationGroup1", "ALedger", new string[] {
                                "a_ledger_number_i"}, "AMotivationGroup", new string[] {
                                "a_ledger_number_i"}));
            }
        }
    }

    /// The gift recipient information for a gift.  A single gift can be split among more than one recipient.  A gift detail record is created for each recipient.
    [Serializable()]
    public class GiftBatchTDSAGiftDetailTable : AGiftDetailTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 5600;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorKeyId = 29;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorNameId = 30;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientDescriptionId = 31;
        /// used for generic TTypedDataTable functions
        public static short ColumnAccountCodeId = 32;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AGiftDetail", "a_gift_detail",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", "Gift Batch Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "GiftTransactionNumber", "a_gift_transaction_number_i", "Gift Transaction Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "DetailNumber", "a_detail_number_i", "Gift Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "RecipientLedgerNumber", "a_recipient_ledger_number_n", "Recipient Ledger", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(5, "GiftAmount", "a_gift_amount_n", "Gift Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(6, "MotivationGroupCode", "a_motivation_group_code_c", "Motivation Group", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(7, "MotivationDetailCode", "a_motivation_detail_code_c", "Motivation Detail", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(8, "CommentOneType", "a_comment_one_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(9, "GiftCommentOne", "a_gift_comment_one_c", "Comment One", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(10, "ConfidentialGiftFlag", "a_confidential_gift_flag_l", "Confidential Gift", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(11, "TaxDeductable", "a_tax_deductable_l", "Tax Deductable", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(12, "RecipientKey", "p_recipient_key_n", "Recipient", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(13, "ChargeFlag", "a_charge_flag_l", "Charge Fee", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(14, "CostCentreCode", "a_cost_centre_code_c", "Cost Centre Code", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(15, "GiftAmountIntl", "a_gift_amount_intl_n", "International Gift Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(16, "ModifiedDetail", "a_modified_detail_l", "Part of a gift detail modification", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(17, "GiftTransactionAmount", "a_gift_transaction_amount_n", "Transaction Gift Amount", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(18, "IchNumber", "a_ich_number_i", "ICH Process Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(19, "MailingCode", "p_mailing_code_c", "Mailing Code", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(20, "CommentTwoType", "a_comment_two_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(21, "GiftCommentTwo", "a_gift_comment_two_c", "Comment Two", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(22, "CommentThreeType", "a_comment_three_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(23, "GiftCommentThree", "a_gift_comment_three_c", "Comment Three", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(24, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(25, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(26, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(27, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(28, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false),
                    new TTypedColumnInfo(29, "DonorKey", "DonorKey", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(30, "DonorName", "DonorName", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(31, "RecipientDescription", "RecipientDescription", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(32, "AccountCode", "AccountCode", "", OdbcType.Int, -1, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

        /// constructor
        public GiftBatchTDSAGiftDetailTable() :
                base("AGiftDetail")
        {
        }

        /// constructor
        public GiftBatchTDSAGiftDetailTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public GiftBatchTDSAGiftDetailTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnDonorKey;
        ///
        public DataColumn ColumnDonorName;
        ///
        public DataColumn ColumnRecipientDescription;
        ///
        public DataColumn ColumnAccountCode;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_recipient_ledger_number_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_one_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_one_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_confidential_gift_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_deductable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_recipient_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_charge_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_modified_detail_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_ich_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_two_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_two_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_three_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_three_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("DonorKey", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("DonorName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("RecipientDescription", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("AccountCode", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnGiftTransactionNumber = this.Columns["a_gift_transaction_number_i"];
            this.ColumnDetailNumber = this.Columns["a_detail_number_i"];
            this.ColumnRecipientLedgerNumber = this.Columns["a_recipient_ledger_number_n"];
            this.ColumnGiftAmount = this.Columns["a_gift_amount_n"];
            this.ColumnMotivationGroupCode = this.Columns["a_motivation_group_code_c"];
            this.ColumnMotivationDetailCode = this.Columns["a_motivation_detail_code_c"];
            this.ColumnCommentOneType = this.Columns["a_comment_one_type_c"];
            this.ColumnGiftCommentOne = this.Columns["a_gift_comment_one_c"];
            this.ColumnConfidentialGiftFlag = this.Columns["a_confidential_gift_flag_l"];
            this.ColumnTaxDeductable = this.Columns["a_tax_deductable_l"];
            this.ColumnRecipientKey = this.Columns["p_recipient_key_n"];
            this.ColumnChargeFlag = this.Columns["a_charge_flag_l"];
            this.ColumnCostCentreCode = this.Columns["a_cost_centre_code_c"];
            this.ColumnGiftAmountIntl = this.Columns["a_gift_amount_intl_n"];
            this.ColumnModifiedDetail = this.Columns["a_modified_detail_l"];
            this.ColumnGiftTransactionAmount = this.Columns["a_gift_transaction_amount_n"];
            this.ColumnIchNumber = this.Columns["a_ich_number_i"];
            this.ColumnMailingCode = this.Columns["p_mailing_code_c"];
            this.ColumnCommentTwoType = this.Columns["a_comment_two_type_c"];
            this.ColumnGiftCommentTwo = this.Columns["a_gift_comment_two_c"];
            this.ColumnCommentThreeType = this.Columns["a_comment_three_type_c"];
            this.ColumnGiftCommentThree = this.Columns["a_gift_comment_three_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnDonorKey = this.Columns["DonorKey"];
            this.ColumnDonorName = this.Columns["DonorName"];
            this.ColumnRecipientDescription = this.Columns["RecipientDescription"];
            this.ColumnAccountCode = this.Columns["AccountCode"];
            this.PrimaryKey = new System.Data.DataColumn[4] {
                    ColumnLedgerNumber,ColumnBatchNumber,ColumnGiftTransactionNumber,ColumnDetailNumber};
        }

        /// Access a typed row by index
        public new GiftBatchTDSAGiftDetailRow this[int i]
        {
            get
            {
                return ((GiftBatchTDSAGiftDetailRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new GiftBatchTDSAGiftDetailRow NewRowTyped(bool AWithDefaultValues)
        {
            GiftBatchTDSAGiftDetailRow ret = ((GiftBatchTDSAGiftDetailRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new GiftBatchTDSAGiftDetailRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new GiftBatchTDSAGiftDetailRow(builder);
        }

        /// get typed set of changes
        public new GiftBatchTDSAGiftDetailTable GetChangesTyped()
        {
            return ((GiftBatchTDSAGiftDetailTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "AGiftDetail";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_gift_detail";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetDonorKeyDBName()
        {
            return "DonorKey";
        }

        /// get character length for column
        public static short GetDonorKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDonorNameDBName()
        {
            return "DonorName";
        }

        /// get character length for column
        public static short GetDonorNameLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetRecipientDescriptionDBName()
        {
            return "RecipientDescription";
        }

        /// get character length for column
        public static short GetRecipientDescriptionLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAccountCodeDBName()
        {
            return "AccountCode";
        }

        /// get character length for column
        public static short GetAccountCodeLength()
        {
            return -1;
        }

    }

    /// The gift recipient information for a gift.  A single gift can be split among more than one recipient.  A gift detail record is created for each recipient.
    [Serializable()]
    public class GiftBatchTDSAGiftDetailRow : AGiftDetailRow
    {
        private GiftBatchTDSAGiftDetailTable myTable;

        /// Constructor
        public GiftBatchTDSAGiftDetailRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((GiftBatchTDSAGiftDetailTable)(this.Table));
        }

        ///
        public Int64 DonorKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorKey.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Int64)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDonorKey)
                            || (((Int64)(this[this.myTable.ColumnDonorKey])) != value)))
                {
                    this[this.myTable.ColumnDonorKey] = value;
                }
            }
        }

        ///
        public string DonorName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDonorName)
                            || (((string)(this[this.myTable.ColumnDonorName])) != value)))
                {
                    this[this.myTable.ColumnDonorName] = value;
                }
            }
        }

        ///
        public string RecipientDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRecipientDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnRecipientDescription)
                            || (((string)(this[this.myTable.ColumnRecipientDescription])) != value)))
                {
                    this[this.myTable.ColumnRecipientDescription] = value;
                }
            }
        }

        ///
        public string AccountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAccountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAccountCode)
                            || (((string)(this[this.myTable.ColumnAccountCode])) != value)))
                {
                    this[this.myTable.ColumnAccountCode] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftTransactionNumber.Ordinal] = 0;
            this[this.myTable.ColumnDetailNumber.Ordinal] = 0;
            this[this.myTable.ColumnRecipientLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftAmount.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
            this.SetNull(this.myTable.ColumnCommentOneType);
            this.SetNull(this.myTable.ColumnGiftCommentOne);
            this[this.myTable.ColumnConfidentialGiftFlag.Ordinal] = false;
            this[this.myTable.ColumnTaxDeductable.Ordinal] = true;
            this[this.myTable.ColumnRecipientKey.Ordinal] = 0;
            this[this.myTable.ColumnChargeFlag.Ordinal] = true;
            this.SetNull(this.myTable.ColumnCostCentreCode);
            this[this.myTable.ColumnGiftAmountIntl.Ordinal] = 0;
            this[this.myTable.ColumnModifiedDetail.Ordinal] = false;
            this[this.myTable.ColumnGiftTransactionAmount.Ordinal] = 0;
            this[this.myTable.ColumnIchNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMailingCode);
            this.SetNull(this.myTable.ColumnCommentTwoType);
            this.SetNull(this.myTable.ColumnGiftCommentTwo);
            this.SetNull(this.myTable.ColumnCommentThreeType);
            this.SetNull(this.myTable.ColumnGiftCommentThree);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
            this.SetNull(this.myTable.ColumnDonorKey);
            this.SetNull(this.myTable.ColumnDonorName);
            this.SetNull(this.myTable.ColumnRecipientDescription);
            this.SetNull(this.myTable.ColumnAccountCode);
        }

        /// test for NULL value
        public bool IsDonorKeyNull()
        {
            return this.IsNull(this.myTable.ColumnDonorKey);
        }

        /// assign NULL value
        public void SetDonorKeyNull()
        {
            this.SetNull(this.myTable.ColumnDonorKey);
        }

        /// test for NULL value
        public bool IsDonorNameNull()
        {
            return this.IsNull(this.myTable.ColumnDonorName);
        }

        /// assign NULL value
        public void SetDonorNameNull()
        {
            this.SetNull(this.myTable.ColumnDonorName);
        }

        /// test for NULL value
        public bool IsRecipientDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnRecipientDescription);
        }

        /// assign NULL value
        public void SetRecipientDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnRecipientDescription);
        }

        /// test for NULL value
        public bool IsAccountCodeNull()
        {
            return this.IsNull(this.myTable.ColumnAccountCode);
        }

        /// assign NULL value
        public void SetAccountCodeNull()
        {
            this.SetNull(this.myTable.ColumnAccountCode);
        }
    }

     /// auto generated
    [Serializable()]
    public class BankImportTDS : TTypedDataSet
    {

        private BankImportTDSAGiftDetailTable TableAGiftDetail;
        private AEpTransactionTable TableAEpTransaction;
        private AEpMatchTable TableAEpMatch;

        /// auto generated
        public BankImportTDS() :
                base("BankImportTDS")
        {
        }

        /// auto generated for serialization
        public BankImportTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public BankImportTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public BankImportTDSAGiftDetailTable AGiftDetail
        {
            get
            {
                return this.TableAGiftDetail;
            }
        }

        /// auto generated
        public AEpTransactionTable AEpTransaction
        {
            get
            {
                return this.TableAEpTransaction;
            }
        }

        /// auto generated
        public AEpMatchTable AEpMatch
        {
            get
            {
                return this.TableAEpMatch;
            }
        }

        /// auto generated
        public new virtual BankImportTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((BankImportTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new BankImportTDSAGiftDetailTable("AGiftDetail"));
            this.Tables.Add(new AEpTransactionTable("AEpTransaction"));
            this.Tables.Add(new AEpMatchTable("AEpMatch"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("AGiftDetail") != -1))
            {
                this.Tables.Add(new BankImportTDSAGiftDetailTable("AGiftDetail"));
            }
            if ((ds.Tables.IndexOf("AEpTransaction") != -1))
            {
                this.Tables.Add(new AEpTransactionTable("AEpTransaction"));
            }
            if ((ds.Tables.IndexOf("AEpMatch") != -1))
            {
                this.Tables.Add(new AEpMatchTable("AEpMatch"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableAGiftDetail != null))
            {
                this.TableAGiftDetail.InitVars();
            }
            if ((this.TableAEpTransaction != null))
            {
                this.TableAEpTransaction.InitVars();
            }
            if ((this.TableAEpMatch != null))
            {
                this.TableAEpMatch.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "BankImportTDS";
            this.TableAGiftDetail = ((BankImportTDSAGiftDetailTable)(this.Tables["AGiftDetail"]));
            this.TableAEpTransaction = ((AEpTransactionTable)(this.Tables["AEpTransaction"]));
            this.TableAEpMatch = ((AEpMatchTable)(this.Tables["AEpMatch"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {

            if (((this.TableAEpMatch != null)
                        && (this.TableAEpTransaction != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKEpTransaction3", "AEpMatch", new string[] {
                                "a_ep_match_key_i"}, "AEpTransaction", new string[] {
                                "a_ep_match_key_i"}));
            }
        }
    }

    /// The gift recipient information for a gift.  A single gift can be split among more than one recipient.  A gift detail record is created for each recipient.
    [Serializable()]
    public class BankImportTDSAGiftDetailTable : AGiftDetailTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 5601;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorKeyId = 29;
        /// used for generic TTypedDataTable functions
        public static short ColumnBankAccountNumberId = 30;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AGiftDetail", "a_gift_detail",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LedgerNumber", "a_ledger_number_i", "Ledger Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(1, "BatchNumber", "a_batch_number_i", "Gift Batch Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(2, "GiftTransactionNumber", "a_gift_transaction_number_i", "Gift Transaction Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "DetailNumber", "a_detail_number_i", "Gift Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(4, "RecipientLedgerNumber", "a_recipient_ledger_number_n", "Recipient Ledger", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(5, "GiftAmount", "a_gift_amount_n", "Gift Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(6, "MotivationGroupCode", "a_motivation_group_code_c", "Motivation Group", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(7, "MotivationDetailCode", "a_motivation_detail_code_c", "Motivation Detail", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(8, "CommentOneType", "a_comment_one_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(9, "GiftCommentOne", "a_gift_comment_one_c", "Comment One", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(10, "ConfidentialGiftFlag", "a_confidential_gift_flag_l", "Confidential Gift", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(11, "TaxDeductable", "a_tax_deductable_l", "Tax Deductable", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(12, "RecipientKey", "p_recipient_key_n", "Recipient", OdbcType.Decimal, 10, true),
                    new TTypedColumnInfo(13, "ChargeFlag", "a_charge_flag_l", "Charge Fee", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(14, "CostCentreCode", "a_cost_centre_code_c", "Cost Centre Code", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(15, "GiftAmountIntl", "a_gift_amount_intl_n", "International Gift Amount", OdbcType.Decimal, 24, false),
                    new TTypedColumnInfo(16, "ModifiedDetail", "a_modified_detail_l", "Part of a gift detail modification", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(17, "GiftTransactionAmount", "a_gift_transaction_amount_n", "Transaction Gift Amount", OdbcType.Decimal, 24, true),
                    new TTypedColumnInfo(18, "IchNumber", "a_ich_number_i", "ICH Process Number", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(19, "MailingCode", "p_mailing_code_c", "Mailing Code", OdbcType.VarChar, 50, false),
                    new TTypedColumnInfo(20, "CommentTwoType", "a_comment_two_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(21, "GiftCommentTwo", "a_gift_comment_two_c", "Comment Two", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(22, "CommentThreeType", "a_comment_three_type_c", "Comment Type", OdbcType.VarChar, 24, false),
                    new TTypedColumnInfo(23, "GiftCommentThree", "a_gift_comment_three_c", "Comment Three", OdbcType.VarChar, 160, false),
                    new TTypedColumnInfo(24, "DateCreated", "s_date_created_d", "Created Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(25, "CreatedBy", "s_created_by_c", "Created By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(26, "DateModified", "s_date_modified_d", "Modified Date", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(27, "ModifiedBy", "s_modified_by_c", "Modified By", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(28, "ModificationId", "s_modification_id_c", "", OdbcType.VarChar, 150, false),
                    new TTypedColumnInfo(29, "DonorKey", "DonorKey", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(30, "BankAccountNumber", "BankAccountNumber", "", OdbcType.Int, -1, false)
                },
                new int[] {
                    0, 1, 2, 3
                }));
            return true;
        }

        /// constructor
        public BankImportTDSAGiftDetailTable() :
                base("AGiftDetail")
        {
        }

        /// constructor
        public BankImportTDSAGiftDetailTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public BankImportTDSAGiftDetailTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnDonorKey;
        ///
        public DataColumn ColumnBankAccountNumber;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_recipient_ledger_number_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_one_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_one_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_confidential_gift_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_deductable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_recipient_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_charge_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_intl_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_modified_detail_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_ich_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_two_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_two_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_three_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_three_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("DonorKey", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("BankAccountNumber", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnGiftTransactionNumber = this.Columns["a_gift_transaction_number_i"];
            this.ColumnDetailNumber = this.Columns["a_detail_number_i"];
            this.ColumnRecipientLedgerNumber = this.Columns["a_recipient_ledger_number_n"];
            this.ColumnGiftAmount = this.Columns["a_gift_amount_n"];
            this.ColumnMotivationGroupCode = this.Columns["a_motivation_group_code_c"];
            this.ColumnMotivationDetailCode = this.Columns["a_motivation_detail_code_c"];
            this.ColumnCommentOneType = this.Columns["a_comment_one_type_c"];
            this.ColumnGiftCommentOne = this.Columns["a_gift_comment_one_c"];
            this.ColumnConfidentialGiftFlag = this.Columns["a_confidential_gift_flag_l"];
            this.ColumnTaxDeductable = this.Columns["a_tax_deductable_l"];
            this.ColumnRecipientKey = this.Columns["p_recipient_key_n"];
            this.ColumnChargeFlag = this.Columns["a_charge_flag_l"];
            this.ColumnCostCentreCode = this.Columns["a_cost_centre_code_c"];
            this.ColumnGiftAmountIntl = this.Columns["a_gift_amount_intl_n"];
            this.ColumnModifiedDetail = this.Columns["a_modified_detail_l"];
            this.ColumnGiftTransactionAmount = this.Columns["a_gift_transaction_amount_n"];
            this.ColumnIchNumber = this.Columns["a_ich_number_i"];
            this.ColumnMailingCode = this.Columns["p_mailing_code_c"];
            this.ColumnCommentTwoType = this.Columns["a_comment_two_type_c"];
            this.ColumnGiftCommentTwo = this.Columns["a_gift_comment_two_c"];
            this.ColumnCommentThreeType = this.Columns["a_comment_three_type_c"];
            this.ColumnGiftCommentThree = this.Columns["a_gift_comment_three_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnDonorKey = this.Columns["DonorKey"];
            this.ColumnBankAccountNumber = this.Columns["BankAccountNumber"];
            this.PrimaryKey = new System.Data.DataColumn[4] {
                    ColumnLedgerNumber,ColumnBatchNumber,ColumnGiftTransactionNumber,ColumnDetailNumber};
        }

        /// Access a typed row by index
        public new BankImportTDSAGiftDetailRow this[int i]
        {
            get
            {
                return ((BankImportTDSAGiftDetailRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new BankImportTDSAGiftDetailRow NewRowTyped(bool AWithDefaultValues)
        {
            BankImportTDSAGiftDetailRow ret = ((BankImportTDSAGiftDetailRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new BankImportTDSAGiftDetailRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new BankImportTDSAGiftDetailRow(builder);
        }

        /// get typed set of changes
        public new BankImportTDSAGiftDetailTable GetChangesTyped()
        {
            return ((BankImportTDSAGiftDetailTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "AGiftDetail";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_gift_detail";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetDonorKeyDBName()
        {
            return "DonorKey";
        }

        /// get character length for column
        public static short GetDonorKeyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBankAccountNumberDBName()
        {
            return "BankAccountNumber";
        }

        /// get character length for column
        public static short GetBankAccountNumberLength()
        {
            return -1;
        }

    }

    /// The gift recipient information for a gift.  A single gift can be split among more than one recipient.  A gift detail record is created for each recipient.
    [Serializable()]
    public class BankImportTDSAGiftDetailRow : AGiftDetailRow
    {
        private BankImportTDSAGiftDetailTable myTable;

        /// Constructor
        public BankImportTDSAGiftDetailRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((BankImportTDSAGiftDetailTable)(this.Table));
        }

        ///
        public Int64 DonorKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorKey.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Int64)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDonorKey)
                            || (((Int64)(this[this.myTable.ColumnDonorKey])) != value)))
                {
                    this[this.myTable.ColumnDonorKey] = value;
                }
            }
        }

        ///
        public string BankAccountNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBankAccountNumber.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnBankAccountNumber)
                            || (((string)(this[this.myTable.ColumnBankAccountNumber])) != value)))
                {
                    this[this.myTable.ColumnBankAccountNumber] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftTransactionNumber.Ordinal] = 0;
            this[this.myTable.ColumnDetailNumber.Ordinal] = 0;
            this[this.myTable.ColumnRecipientLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftAmount.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
            this.SetNull(this.myTable.ColumnCommentOneType);
            this.SetNull(this.myTable.ColumnGiftCommentOne);
            this[this.myTable.ColumnConfidentialGiftFlag.Ordinal] = false;
            this[this.myTable.ColumnTaxDeductable.Ordinal] = true;
            this[this.myTable.ColumnRecipientKey.Ordinal] = 0;
            this[this.myTable.ColumnChargeFlag.Ordinal] = true;
            this.SetNull(this.myTable.ColumnCostCentreCode);
            this[this.myTable.ColumnGiftAmountIntl.Ordinal] = 0;
            this[this.myTable.ColumnModifiedDetail.Ordinal] = false;
            this[this.myTable.ColumnGiftTransactionAmount.Ordinal] = 0;
            this[this.myTable.ColumnIchNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMailingCode);
            this.SetNull(this.myTable.ColumnCommentTwoType);
            this.SetNull(this.myTable.ColumnGiftCommentTwo);
            this.SetNull(this.myTable.ColumnCommentThreeType);
            this.SetNull(this.myTable.ColumnGiftCommentThree);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
            this.SetNull(this.myTable.ColumnDonorKey);
            this.SetNull(this.myTable.ColumnBankAccountNumber);
        }

        /// test for NULL value
        public bool IsDonorKeyNull()
        {
            return this.IsNull(this.myTable.ColumnDonorKey);
        }

        /// assign NULL value
        public void SetDonorKeyNull()
        {
            this.SetNull(this.myTable.ColumnDonorKey);
        }

        /// test for NULL value
        public bool IsBankAccountNumberNull()
        {
            return this.IsNull(this.myTable.ColumnBankAccountNumber);
        }

        /// assign NULL value
        public void SetBankAccountNumberNull()
        {
            this.SetNull(this.myTable.ColumnBankAccountNumber);
        }
    }
}