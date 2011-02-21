// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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

using Ict.Common;
using Ict.Common.Data;
using System;
using System.Data;
using System.Data.Odbc;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Shared.MFinance.Gift.Data
{
     /// auto generated
    [Serializable()]
    public class GiftBatchTDS : TTypedDataSet
    {

        private ALedgerTable TableALedger;
        private AGiftBatchTable TableAGiftBatch;
        private AGiftTable TableAGift;
        private GiftBatchTDSAGiftDetailTable TableAGiftDetail;
        private AMotivationGroupTable TableAMotivationGroup;
        private AMotivationDetailTable TableAMotivationDetail;

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
        public AMotivationGroupTable AMotivationGroup
        {
            get
            {
                return this.TableAMotivationGroup;
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
            this.Tables.Add(new AMotivationGroupTable("AMotivationGroup"));
            this.Tables.Add(new AMotivationDetailTable("AMotivationDetail"));
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
            if ((ds.Tables.IndexOf("AMotivationGroup") != -1))
            {
                this.Tables.Add(new AMotivationGroupTable("AMotivationGroup"));
            }
            if ((ds.Tables.IndexOf("AMotivationDetail") != -1))
            {
                this.Tables.Add(new AMotivationDetailTable("AMotivationDetail"));
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
            if ((this.TableAMotivationGroup != null))
            {
                this.TableAMotivationGroup.InitVars();
            }
            if ((this.TableAMotivationDetail != null))
            {
                this.TableAMotivationDetail.InitVars();
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
            this.TableAMotivationGroup = ((AMotivationGroupTable)(this.Tables["AMotivationGroup"]));
            this.TableAMotivationDetail = ((AMotivationDetailTable)(this.Tables["AMotivationDetail"]));
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
        public new static short TableId = 165;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorKeyId = 29;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorNameId = 30;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorClassId = 31;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateEnteredId = 32;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientDescriptionId = 33;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientFieldId = 34;
        /// used for generic TTypedDataTable functions
        public static short ColumnReceiptNumberId = 35;
        /// used for generic TTypedDataTable functions
        public static short ColumnReceiptPrintedId = 36;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfGivingCodeId = 37;
        /// used for generic TTypedDataTable functions
        public static short ColumnMethodOfPaymentCodeId = 38;
        /// used for generic TTypedDataTable functions
        public static short ColumnAccountCodeId = 39;

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
        public DataColumn ColumnDonorClass;
        ///
        public DataColumn ColumnDateEntered;
        ///
        public DataColumn ColumnRecipientDescription;
        ///
        public DataColumn ColumnRecipientField;
        ///
        public DataColumn ColumnReceiptNumber;
        ///
        public DataColumn ColumnReceiptPrinted;
        ///
        public DataColumn ColumnMethodOfGivingCode;
        ///
        public DataColumn ColumnMethodOfPaymentCode;
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
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_one_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_one_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_confidential_gift_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_deductable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_recipient_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_charge_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_intl_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("a_modified_detail_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_amount_n", typeof(Decimal)));
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
            this.Columns.Add(new System.Data.DataColumn("DonorName", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("DonorClass", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("DateEntered", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("RecipientDescription", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("RecipientField", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("ReceiptNumber", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("ReceiptPrinted", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("MethodOfGivingCode", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("MethodOfPaymentCode", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("AccountCode", typeof(String)));
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
            this.ColumnDonorClass = this.Columns["DonorClass"];
            this.ColumnDateEntered = this.Columns["DateEntered"];
            this.ColumnRecipientDescription = this.Columns["RecipientDescription"];
            this.ColumnRecipientField = this.Columns["RecipientField"];
            this.ColumnReceiptNumber = this.Columns["ReceiptNumber"];
            this.ColumnReceiptPrinted = this.Columns["ReceiptPrinted"];
            this.ColumnMethodOfGivingCode = this.Columns["MethodOfGivingCode"];
            this.ColumnMethodOfPaymentCode = this.Columns["MethodOfPaymentCode"];
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
        public static string GetDonorClassDBName()
        {
            return "DonorClass";
        }

        /// get character length for column
        public static short GetDonorClassLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateEnteredDBName()
        {
            return "DateEntered";
        }

        /// get character length for column
        public static short GetDateEnteredLength()
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
        public static string GetRecipientFieldDBName()
        {
            return "RecipientField";
        }

        /// get character length for column
        public static short GetRecipientFieldLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReceiptNumberDBName()
        {
            return "ReceiptNumber";
        }

        /// get character length for column
        public static short GetReceiptNumberLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetReceiptPrintedDBName()
        {
            return "ReceiptPrinted";
        }

        /// get character length for column
        public static short GetReceiptPrintedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfGivingCodeDBName()
        {
            return "MethodOfGivingCode";
        }

        /// get character length for column
        public static short GetMethodOfGivingCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMethodOfPaymentCodeDBName()
        {
            return "MethodOfPaymentCode";
        }

        /// get character length for column
        public static short GetMethodOfPaymentCodeLength()
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
        public String DonorName
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
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDonorName)
                            || (((String)(this[this.myTable.ColumnDonorName])) != value)))
                {
                    this[this.myTable.ColumnDonorName] = value;
                }
            }
        }

        ///
        public String DonorClass
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorClass.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDonorClass)
                            || (((String)(this[this.myTable.ColumnDonorClass])) != value)))
                {
                    this[this.myTable.ColumnDonorClass] = value;
                }
            }
        }

        ///
        public DateTime DateEntered
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateEntered.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return ((DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateEntered)
                            || (((DateTime)(this[this.myTable.ColumnDateEntered])) != value)))
                {
                    this[this.myTable.ColumnDateEntered] = value;
                }
            }
        }

        ///
        public String RecipientDescription
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
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnRecipientDescription)
                            || (((String)(this[this.myTable.ColumnRecipientDescription])) != value)))
                {
                    this[this.myTable.ColumnRecipientDescription] = value;
                }
            }
        }

        ///
        public Int64 RecipientField
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRecipientField.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRecipientField)
                            || (((Int64)(this[this.myTable.ColumnRecipientField])) != value)))
                {
                    this[this.myTable.ColumnRecipientField] = value;
                }
            }
        }

        ///
        public Int32 ReceiptNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReceiptNumber.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Int32)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnReceiptNumber)
                            || (((Int32)(this[this.myTable.ColumnReceiptNumber])) != value)))
                {
                    this[this.myTable.ColumnReceiptNumber] = value;
                }
            }
        }

        ///
        public Boolean ReceiptPrinted
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReceiptPrinted.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Boolean)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnReceiptPrinted)
                            || (((Boolean)(this[this.myTable.ColumnReceiptPrinted])) != value)))
                {
                    this[this.myTable.ColumnReceiptPrinted] = value;
                }
            }
        }

        ///
        public String MethodOfGivingCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfGivingCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfGivingCode)
                            || (((String)(this[this.myTable.ColumnMethodOfGivingCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfGivingCode] = value;
                }
            }
        }

        ///
        public String MethodOfPaymentCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfPaymentCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnMethodOfPaymentCode)
                            || (((String)(this[this.myTable.ColumnMethodOfPaymentCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfPaymentCode] = value;
                }
            }
        }

        ///
        public String AccountCode
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
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAccountCode)
                            || (((String)(this[this.myTable.ColumnAccountCode])) != value)))
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
            this.SetNull(this.myTable.ColumnDonorClass);
            this.SetNull(this.myTable.ColumnDateEntered);
            this.SetNull(this.myTable.ColumnRecipientDescription);
            this.SetNull(this.myTable.ColumnRecipientField);
            this.SetNull(this.myTable.ColumnReceiptNumber);
            this.SetNull(this.myTable.ColumnReceiptPrinted);
            this.SetNull(this.myTable.ColumnMethodOfGivingCode);
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
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
        public bool IsDonorClassNull()
        {
            return this.IsNull(this.myTable.ColumnDonorClass);
        }

        /// assign NULL value
        public void SetDonorClassNull()
        {
            this.SetNull(this.myTable.ColumnDonorClass);
        }

        /// test for NULL value
        public bool IsDateEnteredNull()
        {
            return this.IsNull(this.myTable.ColumnDateEntered);
        }

        /// assign NULL value
        public void SetDateEnteredNull()
        {
            this.SetNull(this.myTable.ColumnDateEntered);
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
        public bool IsRecipientFieldNull()
        {
            return this.IsNull(this.myTable.ColumnRecipientField);
        }

        /// assign NULL value
        public void SetRecipientFieldNull()
        {
            this.SetNull(this.myTable.ColumnRecipientField);
        }

        /// test for NULL value
        public bool IsReceiptNumberNull()
        {
            return this.IsNull(this.myTable.ColumnReceiptNumber);
        }

        /// assign NULL value
        public void SetReceiptNumberNull()
        {
            this.SetNull(this.myTable.ColumnReceiptNumber);
        }

        /// test for NULL value
        public bool IsReceiptPrintedNull()
        {
            return this.IsNull(this.myTable.ColumnReceiptPrinted);
        }

        /// assign NULL value
        public void SetReceiptPrintedNull()
        {
            this.SetNull(this.myTable.ColumnReceiptPrinted);
        }

        /// test for NULL value
        public bool IsMethodOfGivingCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfGivingCode);
        }

        /// assign NULL value
        public void SetMethodOfGivingCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfGivingCode);
        }

        /// test for NULL value
        public bool IsMethodOfPaymentCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMethodOfPaymentCode);
        }

        /// assign NULL value
        public void SetMethodOfPaymentCodeNull()
        {
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
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
        private BankImportTDSPBankingDetailsTable TablePBankingDetails;
        private AEpStatementTable TableAEpStatement;
        private BankImportTDSAEpTransactionTable TableAEpTransaction;
        private BankImportTDSAEpMatchTable TableAEpMatch;
        private ACostCentreTable TableACostCentre;
        private AMotivationDetailTable TableAMotivationDetail;

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
        public BankImportTDSPBankingDetailsTable PBankingDetails
        {
            get
            {
                return this.TablePBankingDetails;
            }
        }

        /// auto generated
        public AEpStatementTable AEpStatement
        {
            get
            {
                return this.TableAEpStatement;
            }
        }

        /// auto generated
        public BankImportTDSAEpTransactionTable AEpTransaction
        {
            get
            {
                return this.TableAEpTransaction;
            }
        }

        /// auto generated
        public BankImportTDSAEpMatchTable AEpMatch
        {
            get
            {
                return this.TableAEpMatch;
            }
        }

        /// auto generated
        public ACostCentreTable ACostCentre
        {
            get
            {
                return this.TableACostCentre;
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
        public new virtual BankImportTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((BankImportTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new BankImportTDSAGiftDetailTable("AGiftDetail"));
            this.Tables.Add(new BankImportTDSPBankingDetailsTable("PBankingDetails"));
            this.Tables.Add(new AEpStatementTable("AEpStatement"));
            this.Tables.Add(new BankImportTDSAEpTransactionTable("AEpTransaction"));
            this.Tables.Add(new BankImportTDSAEpMatchTable("AEpMatch"));
            this.Tables.Add(new ACostCentreTable("ACostCentre"));
            this.Tables.Add(new AMotivationDetailTable("AMotivationDetail"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("AGiftDetail") != -1))
            {
                this.Tables.Add(new BankImportTDSAGiftDetailTable("AGiftDetail"));
            }
            if ((ds.Tables.IndexOf("PBankingDetails") != -1))
            {
                this.Tables.Add(new BankImportTDSPBankingDetailsTable("PBankingDetails"));
            }
            if ((ds.Tables.IndexOf("AEpStatement") != -1))
            {
                this.Tables.Add(new AEpStatementTable("AEpStatement"));
            }
            if ((ds.Tables.IndexOf("AEpTransaction") != -1))
            {
                this.Tables.Add(new BankImportTDSAEpTransactionTable("AEpTransaction"));
            }
            if ((ds.Tables.IndexOf("AEpMatch") != -1))
            {
                this.Tables.Add(new BankImportTDSAEpMatchTable("AEpMatch"));
            }
            if ((ds.Tables.IndexOf("ACostCentre") != -1))
            {
                this.Tables.Add(new ACostCentreTable("ACostCentre"));
            }
            if ((ds.Tables.IndexOf("AMotivationDetail") != -1))
            {
                this.Tables.Add(new AMotivationDetailTable("AMotivationDetail"));
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
            if ((this.TablePBankingDetails != null))
            {
                this.TablePBankingDetails.InitVars();
            }
            if ((this.TableAEpStatement != null))
            {
                this.TableAEpStatement.InitVars();
            }
            if ((this.TableAEpTransaction != null))
            {
                this.TableAEpTransaction.InitVars();
            }
            if ((this.TableAEpMatch != null))
            {
                this.TableAEpMatch.InitVars();
            }
            if ((this.TableACostCentre != null))
            {
                this.TableACostCentre.InitVars();
            }
            if ((this.TableAMotivationDetail != null))
            {
                this.TableAMotivationDetail.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "BankImportTDS";
            this.TableAGiftDetail = ((BankImportTDSAGiftDetailTable)(this.Tables["AGiftDetail"]));
            this.TablePBankingDetails = ((BankImportTDSPBankingDetailsTable)(this.Tables["PBankingDetails"]));
            this.TableAEpStatement = ((AEpStatementTable)(this.Tables["AEpStatement"]));
            this.TableAEpTransaction = ((BankImportTDSAEpTransactionTable)(this.Tables["AEpTransaction"]));
            this.TableAEpMatch = ((BankImportTDSAEpMatchTable)(this.Tables["AEpMatch"]));
            this.TableACostCentre = ((ACostCentreTable)(this.Tables["ACostCentre"]));
            this.TableAMotivationDetail = ((AMotivationDetailTable)(this.Tables["AMotivationDetail"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TableAMotivationDetail != null)
                        && (this.TableAEpMatch != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKEpMatch1", "AMotivationDetail", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"}, "AEpMatch", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableAEpMatch != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKEpMatch5", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "AEpMatch", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
            if (((this.TablePBankingDetails != null)
                        && (this.TableAEpStatement != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKEpStatement1", "PBankingDetails", new string[] {
                                "p_banking_details_key_i"}, "AEpStatement", new string[] {
                                "a_bank_account_key_i"}));
            }
            if (((this.TableAEpStatement != null)
                        && (this.TableAEpTransaction != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKEpTransaction1", "AEpStatement", new string[] {
                                "a_statement_key_i"}, "AEpTransaction", new string[] {
                                "a_statement_key_i"}));
            }
            if (((this.TableAEpMatch != null)
                        && (this.TableAEpTransaction != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKEpTransaction2", "AEpMatch", new string[] {
                                "a_ep_match_key_i"}, "AEpTransaction", new string[] {
                                "a_ep_match_key_i"}));
            }
            if (((this.TableAMotivationDetail != null)
                        && (this.TableAGiftDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGiftDetail2", "AMotivationDetail", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"}, "AGiftDetail", new string[] {
                                "a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableAGiftDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKGiftDetail6", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "AGiftDetail", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
            if (((this.TableACostCentre != null)
                        && (this.TableAMotivationDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKMotivationDetail3", "ACostCentre", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}, "AMotivationDetail", new string[] {
                                "a_ledger_number_i", "a_cost_centre_code_c"}));
            }
        }
    }

    /// The gift recipient information for a gift.  A single gift can be split among more than one recipient.  A gift detail record is created for each recipient.
    [Serializable()]
    public class BankImportTDSAGiftDetailTable : AGiftDetailTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 165;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorKeyId = 29;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorShortNameId = 30;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientDescriptionId = 31;
        /// used for generic TTypedDataTable functions
        public static short ColumnAlreadyMatchedId = 32;
        /// used for generic TTypedDataTable functions
        public static short ColumnBatchStatusId = 33;

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
        public DataColumn ColumnDonorShortName;
        ///
        public DataColumn ColumnRecipientDescription;
        ///
        public DataColumn ColumnAlreadyMatched;
        ///
        public DataColumn ColumnBatchStatus;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_recipient_ledger_number_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_one_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_one_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_confidential_gift_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_deductable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_recipient_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_charge_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_amount_intl_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("a_modified_detail_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_amount_n", typeof(Decimal)));
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
            this.Columns.Add(new System.Data.DataColumn("DonorShortName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("RecipientDescription", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("AlreadyMatched", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("BatchStatus", typeof(string)));
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
            this.ColumnDonorShortName = this.Columns["DonorShortName"];
            this.ColumnRecipientDescription = this.Columns["RecipientDescription"];
            this.ColumnAlreadyMatched = this.Columns["AlreadyMatched"];
            this.ColumnBatchStatus = this.Columns["BatchStatus"];
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
        public static string GetDonorShortNameDBName()
        {
            return "DonorShortName";
        }

        /// get character length for column
        public static short GetDonorShortNameLength()
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
        public static string GetAlreadyMatchedDBName()
        {
            return "AlreadyMatched";
        }

        /// get character length for column
        public static short GetAlreadyMatchedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBatchStatusDBName()
        {
            return "BatchStatus";
        }

        /// get character length for column
        public static short GetBatchStatusLength()
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
        public string DonorShortName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorShortName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDonorShortName)
                            || (((string)(this[this.myTable.ColumnDonorShortName])) != value)))
                {
                    this[this.myTable.ColumnDonorShortName] = value;
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
        public Boolean AlreadyMatched
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAlreadyMatched.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Boolean)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnAlreadyMatched)
                            || (((Boolean)(this[this.myTable.ColumnAlreadyMatched])) != value)))
                {
                    this[this.myTable.ColumnAlreadyMatched] = value;
                }
            }
        }

        ///
        public string BatchStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBatchStatus.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBatchStatus)
                            || (((string)(this[this.myTable.ColumnBatchStatus])) != value)))
                {
                    this[this.myTable.ColumnBatchStatus] = value;
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
            this.SetNull(this.myTable.ColumnDonorShortName);
            this.SetNull(this.myTable.ColumnRecipientDescription);
            this.SetNull(this.myTable.ColumnAlreadyMatched);
            this.SetNull(this.myTable.ColumnBatchStatus);
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
        public bool IsDonorShortNameNull()
        {
            return this.IsNull(this.myTable.ColumnDonorShortName);
        }

        /// assign NULL value
        public void SetDonorShortNameNull()
        {
            this.SetNull(this.myTable.ColumnDonorShortName);
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
        public bool IsAlreadyMatchedNull()
        {
            return this.IsNull(this.myTable.ColumnAlreadyMatched);
        }

        /// assign NULL value
        public void SetAlreadyMatchedNull()
        {
            this.SetNull(this.myTable.ColumnAlreadyMatched);
        }

        /// test for NULL value
        public bool IsBatchStatusNull()
        {
            return this.IsNull(this.myTable.ColumnBatchStatus);
        }

        /// assign NULL value
        public void SetBatchStatusNull()
        {
            this.SetNull(this.myTable.ColumnBatchStatus);
        }
    }

    /// Any bank details for a partner can be stored in this table
    [Serializable()]
    public class BankImportTDSPBankingDetailsTable : PBankingDetailsTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 59;
        /// used for generic TTypedDataTable functions
        public static short ColumnPartnerKeyId = 19;

        /// constructor
        public BankImportTDSPBankingDetailsTable() :
                base("PBankingDetails")
        {
        }

        /// constructor
        public BankImportTDSPBankingDetailsTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public BankImportTDSPBankingDetailsTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnPartnerKey;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_banking_details_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_banking_type_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_account_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_title_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_first_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_middle_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_last_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_bank_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_bank_account_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_iban_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_security_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_valid_from_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_expiry_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("PartnerKey", typeof(Int64)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnBankingDetailsKey = this.Columns["p_banking_details_key_i"];
            this.ColumnBankingType = this.Columns["p_banking_type_i"];
            this.ColumnAccountName = this.Columns["p_account_name_c"];
            this.ColumnTitle = this.Columns["p_title_c"];
            this.ColumnFirstName = this.Columns["p_first_name_c"];
            this.ColumnMiddleName = this.Columns["p_middle_name_c"];
            this.ColumnLastName = this.Columns["p_last_name_c"];
            this.ColumnBankKey = this.Columns["p_bank_key_n"];
            this.ColumnBankAccountNumber = this.Columns["p_bank_account_number_c"];
            this.ColumnIban = this.Columns["p_iban_c"];
            this.ColumnSecurityCode = this.Columns["p_security_code_c"];
            this.ColumnValidFromDate = this.Columns["p_valid_from_date_d"];
            this.ColumnExpiryDate = this.Columns["p_expiry_date_d"];
            this.ColumnComment = this.Columns["p_comment_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnPartnerKey = this.Columns["PartnerKey"];
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnBankingDetailsKey};
        }

        /// Access a typed row by index
        public new BankImportTDSPBankingDetailsRow this[int i]
        {
            get
            {
                return ((BankImportTDSPBankingDetailsRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new BankImportTDSPBankingDetailsRow NewRowTyped(bool AWithDefaultValues)
        {
            BankImportTDSPBankingDetailsRow ret = ((BankImportTDSPBankingDetailsRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new BankImportTDSPBankingDetailsRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new BankImportTDSPBankingDetailsRow(builder);
        }

        /// get typed set of changes
        public new BankImportTDSPBankingDetailsTable GetChangesTyped()
        {
            return ((BankImportTDSPBankingDetailsTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "PBankingDetails";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "p_banking_details";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "PartnerKey";
        }

        /// get character length for column
        public static short GetPartnerKeyLength()
        {
            return -1;
        }

    }

    /// Any bank details for a partner can be stored in this table
    [Serializable()]
    public class BankImportTDSPBankingDetailsRow : PBankingDetailsRow
    {
        private BankImportTDSPBankingDetailsTable myTable;

        /// Constructor
        public BankImportTDSPBankingDetailsRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((BankImportTDSPBankingDetailsTable)(this.Table));
        }

        ///
        public Int64 PartnerKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPartnerKey)
                            || (((Int64)(this[this.myTable.ColumnPartnerKey])) != value)))
                {
                    this[this.myTable.ColumnPartnerKey] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
        {
            this.SetNull(this.myTable.ColumnBankingDetailsKey);
            this.SetNull(this.myTable.ColumnBankingType);
            this.SetNull(this.myTable.ColumnAccountName);
            this.SetNull(this.myTable.ColumnTitle);
            this.SetNull(this.myTable.ColumnFirstName);
            this.SetNull(this.myTable.ColumnMiddleName);
            this.SetNull(this.myTable.ColumnLastName);
            this.SetNull(this.myTable.ColumnBankKey);
            this.SetNull(this.myTable.ColumnBankAccountNumber);
            this.SetNull(this.myTable.ColumnIban);
            this.SetNull(this.myTable.ColumnSecurityCode);
            this.SetNull(this.myTable.ColumnValidFromDate);
            this.SetNull(this.myTable.ColumnExpiryDate);
            this.SetNull(this.myTable.ColumnComment);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
            this.SetNull(this.myTable.ColumnPartnerKey);
        }

        /// test for NULL value
        public bool IsPartnerKeyNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerKey);
        }

        /// assign NULL value
        public void SetPartnerKeyNull()
        {
            this.SetNull(this.myTable.ColumnPartnerKey);
        }
    }

    /// the transactions from the recently imported bank statements; they should help to identify the other party of the transaction (donor, etc) and the purpose of the transaction
    [Serializable()]
    public class BankImportTDSAEpTransactionTable : AEpTransactionTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 66;
        /// used for generic TTypedDataTable functions
        public static short ColumnMatchActionId = 24;

        /// constructor
        public BankImportTDSAEpTransactionTable() :
                base("AEpTransaction")
        {
        }

        /// constructor
        public BankImportTDSAEpTransactionTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public BankImportTDSAEpTransactionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnMatchAction;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_statement_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_order_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_number_on_paper_statement_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_match_text_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_account_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_title_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_first_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_middle_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_last_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_branch_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_bic_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_bank_account_number_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_iban_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_transaction_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_transaction_amount_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("a_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_date_effective_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_ep_match_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("MatchAction", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnStatementKey = this.Columns["a_statement_key_i"];
            this.ColumnOrder = this.Columns["a_order_i"];
            this.ColumnDetailKey = this.Columns["a_detail_key_i"];
            this.ColumnNumberOnPaperStatement = this.Columns["a_number_on_paper_statement_i"];
            this.ColumnMatchText = this.Columns["a_match_text_c"];
            this.ColumnAccountName = this.Columns["a_account_name_c"];
            this.ColumnTitle = this.Columns["a_title_c"];
            this.ColumnFirstName = this.Columns["a_first_name_c"];
            this.ColumnMiddleName = this.Columns["a_middle_name_c"];
            this.ColumnLastName = this.Columns["a_last_name_c"];
            this.ColumnBranchCode = this.Columns["p_branch_code_c"];
            this.ColumnBic = this.Columns["p_bic_c"];
            this.ColumnBankAccountNumber = this.Columns["a_bank_account_number_c"];
            this.ColumnIban = this.Columns["a_iban_c"];
            this.ColumnTransactionTypeCode = this.Columns["a_transaction_type_code_c"];
            this.ColumnTransactionAmount = this.Columns["a_transaction_amount_n"];
            this.ColumnDescription = this.Columns["a_description_c"];
            this.ColumnDateEffective = this.Columns["a_date_effective_d"];
            this.ColumnEpMatchKey = this.Columns["a_ep_match_key_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnMatchAction = this.Columns["MatchAction"];
            this.PrimaryKey = new System.Data.DataColumn[3] {
                    ColumnStatementKey,ColumnOrder,ColumnDetailKey};
        }

        /// Access a typed row by index
        public new BankImportTDSAEpTransactionRow this[int i]
        {
            get
            {
                return ((BankImportTDSAEpTransactionRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new BankImportTDSAEpTransactionRow NewRowTyped(bool AWithDefaultValues)
        {
            BankImportTDSAEpTransactionRow ret = ((BankImportTDSAEpTransactionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new BankImportTDSAEpTransactionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new BankImportTDSAEpTransactionRow(builder);
        }

        /// get typed set of changes
        public new BankImportTDSAEpTransactionTable GetChangesTyped()
        {
            return ((BankImportTDSAEpTransactionTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "AEpTransaction";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_ep_transaction";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetMatchActionDBName()
        {
            return "MatchAction";
        }

        /// get character length for column
        public static short GetMatchActionLength()
        {
            return -1;
        }

    }

    /// the transactions from the recently imported bank statements; they should help to identify the other party of the transaction (donor, etc) and the purpose of the transaction
    [Serializable()]
    public class BankImportTDSAEpTransactionRow : AEpTransactionRow
    {
        private BankImportTDSAEpTransactionTable myTable;

        /// Constructor
        public BankImportTDSAEpTransactionRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((BankImportTDSAEpTransactionTable)(this.Table));
        }

        ///
        public string MatchAction
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMatchAction.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMatchAction)
                            || (((string)(this[this.myTable.ColumnMatchAction])) != value)))
                {
                    this[this.myTable.ColumnMatchAction] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
        {
            this.SetNull(this.myTable.ColumnStatementKey);
            this.SetNull(this.myTable.ColumnOrder);
            this[this.myTable.ColumnDetailKey.Ordinal] = -1;
            this[this.myTable.ColumnNumberOnPaperStatement.Ordinal] = -1;
            this.SetNull(this.myTable.ColumnMatchText);
            this.SetNull(this.myTable.ColumnAccountName);
            this.SetNull(this.myTable.ColumnTitle);
            this.SetNull(this.myTable.ColumnFirstName);
            this.SetNull(this.myTable.ColumnMiddleName);
            this.SetNull(this.myTable.ColumnLastName);
            this.SetNull(this.myTable.ColumnBranchCode);
            this.SetNull(this.myTable.ColumnBic);
            this.SetNull(this.myTable.ColumnBankAccountNumber);
            this.SetNull(this.myTable.ColumnIban);
            this.SetNull(this.myTable.ColumnTransactionTypeCode);
            this[this.myTable.ColumnTransactionAmount.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnDescription);
            this[this.myTable.ColumnDateEffective.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnEpMatchKey);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
            this.SetNull(this.myTable.ColumnMatchAction);
        }

        /// test for NULL value
        public bool IsMatchActionNull()
        {
            return this.IsNull(this.myTable.ColumnMatchAction);
        }

        /// assign NULL value
        public void SetMatchActionNull()
        {
            this.SetNull(this.myTable.ColumnMatchAction);
        }
    }

    /// the matches that can be used to identify recurring gift or GL transactions
    [Serializable()]
    public class BankImportTDSAEpMatchTable : AEpMatchTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 65;
        /// used for generic TTypedDataTable functions
        public static short ColumnCostCentreNameId = 41;

        /// constructor
        public BankImportTDSAEpMatchTable() :
                base("AEpMatch")
        {
        }

        /// constructor
        public BankImportTDSAEpMatchTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public BankImportTDSAEpMatchTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnCostCentreName;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ep_match_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_match_text_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_action_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_recent_match_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_recipient_ledger_number_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_one_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_one_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_confidential_gift_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_deductable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_recipient_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_charge_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_cost_centre_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_two_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_two_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_comment_three_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_comment_three_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_amount_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("a_home_admin_charges_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_ilt_admin_charges_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_receipt_letter_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_giving_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_donor_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_admin_charge_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_narrative_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_donor_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_recipient_short_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_account_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_key_ministry_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("CostCentreName", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnEpMatchKey = this.Columns["a_ep_match_key_i"];
            this.ColumnMatchText = this.Columns["a_match_text_c"];
            this.ColumnDetail = this.Columns["a_detail_i"];
            this.ColumnAction = this.Columns["a_action_c"];
            this.ColumnRecentMatch = this.Columns["a_recent_match_d"];
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnRecipientLedgerNumber = this.Columns["a_recipient_ledger_number_n"];
            this.ColumnMotivationGroupCode = this.Columns["a_motivation_group_code_c"];
            this.ColumnMotivationDetailCode = this.Columns["a_motivation_detail_code_c"];
            this.ColumnCommentOneType = this.Columns["a_comment_one_type_c"];
            this.ColumnGiftCommentOne = this.Columns["a_gift_comment_one_c"];
            this.ColumnConfidentialGiftFlag = this.Columns["a_confidential_gift_flag_l"];
            this.ColumnTaxDeductable = this.Columns["a_tax_deductable_l"];
            this.ColumnRecipientKey = this.Columns["p_recipient_key_n"];
            this.ColumnChargeFlag = this.Columns["a_charge_flag_l"];
            this.ColumnCostCentreCode = this.Columns["a_cost_centre_code_c"];
            this.ColumnMailingCode = this.Columns["p_mailing_code_c"];
            this.ColumnCommentTwoType = this.Columns["a_comment_two_type_c"];
            this.ColumnGiftCommentTwo = this.Columns["a_gift_comment_two_c"];
            this.ColumnCommentThreeType = this.Columns["a_comment_three_type_c"];
            this.ColumnGiftCommentThree = this.Columns["a_gift_comment_three_c"];
            this.ColumnGiftTransactionAmount = this.Columns["a_gift_transaction_amount_n"];
            this.ColumnHomeAdminChargesFlag = this.Columns["a_home_admin_charges_flag_l"];
            this.ColumnIltAdminChargesFlag = this.Columns["a_ilt_admin_charges_flag_l"];
            this.ColumnReceiptLetterCode = this.Columns["a_receipt_letter_code_c"];
            this.ColumnMethodOfGivingCode = this.Columns["a_method_of_giving_code_c"];
            this.ColumnMethodOfPaymentCode = this.Columns["a_method_of_payment_code_c"];
            this.ColumnDonorKey = this.Columns["p_donor_key_n"];
            this.ColumnAdminCharge = this.Columns["a_admin_charge_l"];
            this.ColumnNarrative = this.Columns["a_narrative_c"];
            this.ColumnReference = this.Columns["a_reference_c"];
            this.ColumnDonorShortName = this.Columns["p_donor_short_name_c"];
            this.ColumnRecipientShortName = this.Columns["p_recipient_short_name_c"];
            this.ColumnRestricted = this.Columns["a_restricted_l"];
            this.ColumnAccountCode = this.Columns["a_account_code_c"];
            this.ColumnKeyMinistryKey = this.Columns["a_key_ministry_key_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnCostCentreName = this.Columns["CostCentreName"];
            this.PrimaryKey = new System.Data.DataColumn[1] {
                    ColumnEpMatchKey};
        }

        /// Access a typed row by index
        public new BankImportTDSAEpMatchRow this[int i]
        {
            get
            {
                return ((BankImportTDSAEpMatchRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new BankImportTDSAEpMatchRow NewRowTyped(bool AWithDefaultValues)
        {
            BankImportTDSAEpMatchRow ret = ((BankImportTDSAEpMatchRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new BankImportTDSAEpMatchRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new BankImportTDSAEpMatchRow(builder);
        }

        /// get typed set of changes
        public new BankImportTDSAEpMatchTable GetChangesTyped()
        {
            return ((BankImportTDSAEpMatchTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "AEpMatch";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_ep_match";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCostCentreNameDBName()
        {
            return "CostCentreName";
        }

        /// get character length for column
        public static short GetCostCentreNameLength()
        {
            return -1;
        }

    }

    /// the matches that can be used to identify recurring gift or GL transactions
    [Serializable()]
    public class BankImportTDSAEpMatchRow : AEpMatchRow
    {
        private BankImportTDSAEpMatchTable myTable;

        /// Constructor
        public BankImportTDSAEpMatchRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((BankImportTDSAEpMatchTable)(this.Table));
        }

        ///
        public string CostCentreName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCostCentreName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCostCentreName)
                            || (((string)(this[this.myTable.ColumnCostCentreName])) != value)))
                {
                    this[this.myTable.ColumnCostCentreName] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
        {
            this.SetNull(this.myTable.ColumnEpMatchKey);
            this.SetNull(this.myTable.ColumnMatchText);
            this[this.myTable.ColumnDetail.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAction);
            this[this.myTable.ColumnRecentMatch.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnRecipientLedgerNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
            this.SetNull(this.myTable.ColumnCommentOneType);
            this.SetNull(this.myTable.ColumnGiftCommentOne);
            this[this.myTable.ColumnConfidentialGiftFlag.Ordinal] = false;
            this[this.myTable.ColumnTaxDeductable.Ordinal] = true;
            this[this.myTable.ColumnRecipientKey.Ordinal] = 0;
            this[this.myTable.ColumnChargeFlag.Ordinal] = true;
            this.SetNull(this.myTable.ColumnCostCentreCode);
            this.SetNull(this.myTable.ColumnMailingCode);
            this.SetNull(this.myTable.ColumnCommentTwoType);
            this.SetNull(this.myTable.ColumnGiftCommentTwo);
            this.SetNull(this.myTable.ColumnCommentThreeType);
            this.SetNull(this.myTable.ColumnGiftCommentThree);
            this[this.myTable.ColumnGiftTransactionAmount.Ordinal] = 0;
            this[this.myTable.ColumnHomeAdminChargesFlag.Ordinal] = true;
            this[this.myTable.ColumnIltAdminChargesFlag.Ordinal] = true;
            this.SetNull(this.myTable.ColumnReceiptLetterCode);
            this.SetNull(this.myTable.ColumnMethodOfGivingCode);
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
            this[this.myTable.ColumnDonorKey.Ordinal] = 0;
            this[this.myTable.ColumnAdminCharge.Ordinal] = false;
            this.SetNull(this.myTable.ColumnNarrative);
            this.SetNull(this.myTable.ColumnReference);
            this.SetNull(this.myTable.ColumnDonorShortName);
            this.SetNull(this.myTable.ColumnRecipientShortName);
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this.SetNull(this.myTable.ColumnAccountCode);
            this.SetNull(this.myTable.ColumnKeyMinistryKey);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
            this.SetNull(this.myTable.ColumnCostCentreName);
        }

        /// test for NULL value
        public bool IsCostCentreNameNull()
        {
            return this.IsNull(this.myTable.ColumnCostCentreName);
        }

        /// assign NULL value
        public void SetCostCentreNameNull()
        {
            this.SetNull(this.myTable.ColumnCostCentreName);
        }
    }

     /// auto generated
    [Serializable()]
    public class NewDonorTDS : TTypedDataSet
    {

        private NewDonorTDSAGiftTable TableAGift;
        private BestAddressTDSLocationTable TableBestAddress;

        /// auto generated
        public NewDonorTDS() :
                base("NewDonorTDS")
        {
        }

        /// auto generated for serialization
        public NewDonorTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public NewDonorTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public NewDonorTDSAGiftTable AGift
        {
            get
            {
                return this.TableAGift;
            }
        }

        /// auto generated
        public BestAddressTDSLocationTable BestAddress
        {
            get
            {
                return this.TableBestAddress;
            }
        }

        /// auto generated
        public new virtual NewDonorTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((NewDonorTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new NewDonorTDSAGiftTable("AGift"));
            this.Tables.Add(new BestAddressTDSLocationTable("BestAddress"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("AGift") != -1))
            {
                this.Tables.Add(new NewDonorTDSAGiftTable("AGift"));
            }
            if ((ds.Tables.IndexOf("BestAddress") != -1))
            {
                this.Tables.Add(new BestAddressTDSLocationTable("BestAddress"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableAGift != null))
            {
                this.TableAGift.InitVars();
            }
            if ((this.TableBestAddress != null))
            {
                this.TableBestAddress.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "NewDonorTDS";
            this.TableAGift = ((NewDonorTDSAGiftTable)(this.Tables["AGift"]));
            this.TableBestAddress = ((BestAddressTDSLocationTable)(this.Tables["BestAddress"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
        }
    }

    /// Information on the donor's giving. Points to the gift detail records.
    [Serializable()]
    public class NewDonorTDSAGiftTable : AGiftTable
    {
        /// TableId for Ict.Common.Data generic functions
        public new static short TableId = 164;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorShortNameId = 24;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientDescriptionId = 25;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateOfSubscriptionStartId = 26;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateOfFirstGiftId = 27;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupCodeId = 28;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailCodeId = 29;

        /// constructor
        public NewDonorTDSAGiftTable() :
                base("AGift")
        {
        }

        /// constructor
        public NewDonorTDSAGiftTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public NewDonorTDSAGiftTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnDonorShortName;
        ///
        public DataColumn ColumnRecipientDescription;
        ///
        public DataColumn ColumnDateOfSubscriptionStart;
        ///
        public DataColumn ColumnDateOfFirstGift;
        ///
        public DataColumn ColumnMotivationGroupCode;
        ///
        public DataColumn ColumnMotivationDetailCode;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_batch_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_transaction_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_gift_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_date_entered_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_home_admin_charges_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_ilt_admin_charges_flag_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_receipt_letter_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_giving_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_method_of_payment_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_donor_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_admin_charge_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_receipt_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_last_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_first_time_gift_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_receipt_printed_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_banking_details_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("DonorShortName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("RecipientDescription", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("DateOfSubscriptionStart", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("DateOfFirstGift", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("MotivationGroupCode", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("MotivationDetailCode", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnBatchNumber = this.Columns["a_batch_number_i"];
            this.ColumnGiftTransactionNumber = this.Columns["a_gift_transaction_number_i"];
            this.ColumnGiftStatus = this.Columns["a_gift_status_c"];
            this.ColumnDateEntered = this.Columns["a_date_entered_d"];
            this.ColumnHomeAdminChargesFlag = this.Columns["a_home_admin_charges_flag_l"];
            this.ColumnIltAdminChargesFlag = this.Columns["a_ilt_admin_charges_flag_l"];
            this.ColumnReceiptLetterCode = this.Columns["a_receipt_letter_code_c"];
            this.ColumnMethodOfGivingCode = this.Columns["a_method_of_giving_code_c"];
            this.ColumnMethodOfPaymentCode = this.Columns["a_method_of_payment_code_c"];
            this.ColumnDonorKey = this.Columns["p_donor_key_n"];
            this.ColumnAdminCharge = this.Columns["a_admin_charge_l"];
            this.ColumnReceiptNumber = this.Columns["a_receipt_number_i"];
            this.ColumnLastDetailNumber = this.Columns["a_last_detail_number_i"];
            this.ColumnReference = this.Columns["a_reference_c"];
            this.ColumnFirstTimeGift = this.Columns["a_first_time_gift_l"];
            this.ColumnReceiptPrinted = this.Columns["a_receipt_printed_l"];
            this.ColumnRestricted = this.Columns["a_restricted_l"];
            this.ColumnBankingDetailsKey = this.Columns["p_banking_details_key_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.ColumnDonorShortName = this.Columns["DonorShortName"];
            this.ColumnRecipientDescription = this.Columns["RecipientDescription"];
            this.ColumnDateOfSubscriptionStart = this.Columns["DateOfSubscriptionStart"];
            this.ColumnDateOfFirstGift = this.Columns["DateOfFirstGift"];
            this.ColumnMotivationGroupCode = this.Columns["MotivationGroupCode"];
            this.ColumnMotivationDetailCode = this.Columns["MotivationDetailCode"];
            this.PrimaryKey = new System.Data.DataColumn[3] {
                    ColumnLedgerNumber,ColumnBatchNumber,ColumnGiftTransactionNumber};
        }

        /// Access a typed row by index
        public new NewDonorTDSAGiftRow this[int i]
        {
            get
            {
                return ((NewDonorTDSAGiftRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public new NewDonorTDSAGiftRow NewRowTyped(bool AWithDefaultValues)
        {
            NewDonorTDSAGiftRow ret = ((NewDonorTDSAGiftRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public new NewDonorTDSAGiftRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new NewDonorTDSAGiftRow(builder);
        }

        /// get typed set of changes
        public new NewDonorTDSAGiftTable GetChangesTyped()
        {
            return ((NewDonorTDSAGiftTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static new string GetTableName()
        {
            return "AGift";
        }

        /// return the name of the table as it is used in the database
        public static new string GetTableDBName()
        {
            return "a_gift";
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetDonorShortNameDBName()
        {
            return "DonorShortName";
        }

        /// get character length for column
        public static short GetDonorShortNameLength()
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
        public static string GetDateOfSubscriptionStartDBName()
        {
            return "DateOfSubscriptionStart";
        }

        /// get character length for column
        public static short GetDateOfSubscriptionStartLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateOfFirstGiftDBName()
        {
            return "DateOfFirstGift";
        }

        /// get character length for column
        public static short GetDateOfFirstGiftLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationGroupCodeDBName()
        {
            return "MotivationGroupCode";
        }

        /// get character length for column
        public static short GetMotivationGroupCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationDetailCodeDBName()
        {
            return "MotivationDetailCode";
        }

        /// get character length for column
        public static short GetMotivationDetailCodeLength()
        {
            return -1;
        }

    }

    /// Information on the donor's giving. Points to the gift detail records.
    [Serializable()]
    public class NewDonorTDSAGiftRow : AGiftRow
    {
        private NewDonorTDSAGiftTable myTable;

        /// Constructor
        public NewDonorTDSAGiftRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((NewDonorTDSAGiftTable)(this.Table));
        }

        ///
        public string DonorShortName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorShortName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDonorShortName)
                            || (((string)(this[this.myTable.ColumnDonorShortName])) != value)))
                {
                    this[this.myTable.ColumnDonorShortName] = value;
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
        public DateTime DateOfSubscriptionStart
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateOfSubscriptionStart.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return ((DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateOfSubscriptionStart)
                            || (((DateTime)(this[this.myTable.ColumnDateOfSubscriptionStart])) != value)))
                {
                    this[this.myTable.ColumnDateOfSubscriptionStart] = value;
                }
            }
        }

        ///
        public DateTime DateOfFirstGift
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateOfFirstGift.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return ((DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateOfFirstGift)
                            || (((DateTime)(this[this.myTable.ColumnDateOfFirstGift])) != value)))
                {
                    this[this.myTable.ColumnDateOfFirstGift] = value;
                }
            }
        }

        ///
        public string MotivationGroupCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationGroupCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMotivationGroupCode)
                            || (((string)(this[this.myTable.ColumnMotivationGroupCode])) != value)))
                {
                    this[this.myTable.ColumnMotivationGroupCode] = value;
                }
            }
        }

        ///
        public string MotivationDetailCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationDetailCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMotivationDetailCode)
                            || (((string)(this[this.myTable.ColumnMotivationDetailCode])) != value)))
                {
                    this[this.myTable.ColumnMotivationDetailCode] = value;
                }
            }
        }

        /// set default values
        public override void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this[this.myTable.ColumnBatchNumber.Ordinal] = 0;
            this[this.myTable.ColumnGiftTransactionNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnGiftStatus);
            this[this.myTable.ColumnDateEntered.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnHomeAdminChargesFlag.Ordinal] = true;
            this[this.myTable.ColumnIltAdminChargesFlag.Ordinal] = true;
            this.SetNull(this.myTable.ColumnReceiptLetterCode);
            this.SetNull(this.myTable.ColumnMethodOfGivingCode);
            this.SetNull(this.myTable.ColumnMethodOfPaymentCode);
            this[this.myTable.ColumnDonorKey.Ordinal] = 0;
            this[this.myTable.ColumnAdminCharge.Ordinal] = false;
            this[this.myTable.ColumnReceiptNumber.Ordinal] = 0;
            this[this.myTable.ColumnLastDetailNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnReference);
            this[this.myTable.ColumnFirstTimeGift.Ordinal] = false;
            this[this.myTable.ColumnReceiptPrinted.Ordinal] = false;
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this[this.myTable.ColumnBankingDetailsKey.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
            this.SetNull(this.myTable.ColumnDonorShortName);
            this.SetNull(this.myTable.ColumnRecipientDescription);
            this.SetNull(this.myTable.ColumnDateOfSubscriptionStart);
            this.SetNull(this.myTable.ColumnDateOfFirstGift);
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
        }

        /// test for NULL value
        public bool IsDonorShortNameNull()
        {
            return this.IsNull(this.myTable.ColumnDonorShortName);
        }

        /// assign NULL value
        public void SetDonorShortNameNull()
        {
            this.SetNull(this.myTable.ColumnDonorShortName);
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
        public bool IsDateOfSubscriptionStartNull()
        {
            return this.IsNull(this.myTable.ColumnDateOfSubscriptionStart);
        }

        /// assign NULL value
        public void SetDateOfSubscriptionStartNull()
        {
            this.SetNull(this.myTable.ColumnDateOfSubscriptionStart);
        }

        /// test for NULL value
        public bool IsDateOfFirstGiftNull()
        {
            return this.IsNull(this.myTable.ColumnDateOfFirstGift);
        }

        /// assign NULL value
        public void SetDateOfFirstGiftNull()
        {
            this.SetNull(this.myTable.ColumnDateOfFirstGift);
        }

        /// test for NULL value
        public bool IsMotivationGroupCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationGroupCode);
        }

        /// assign NULL value
        public void SetMotivationGroupCodeNull()
        {
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
        }

        /// test for NULL value
        public bool IsMotivationDetailCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationDetailCode);
        }

        /// assign NULL value
        public void SetMotivationDetailCodeNull()
        {
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
        }
    }

     /// auto generated
    [Serializable()]
    public class DonorHistoryTDS : TTypedDataSet
    {

        private DonorHistoryTDSGiftTable TableGift;
        private DonorHistoryTDSDonorTable TableDonor;

        /// auto generated
        public DonorHistoryTDS() :
                base("DonorHistoryTDS")
        {
        }

        /// auto generated for serialization
        public DonorHistoryTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public DonorHistoryTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public DonorHistoryTDSGiftTable Gift
        {
            get
            {
                return this.TableGift;
            }
        }

        /// auto generated
        public DonorHistoryTDSDonorTable Donor
        {
            get
            {
                return this.TableDonor;
            }
        }

        /// auto generated
        public new virtual DonorHistoryTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((DonorHistoryTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new DonorHistoryTDSGiftTable("Gift"));
            this.Tables.Add(new DonorHistoryTDSDonorTable("Donor"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("Gift") != -1))
            {
                this.Tables.Add(new DonorHistoryTDSGiftTable("Gift"));
            }
            if ((ds.Tables.IndexOf("Donor") != -1))
            {
                this.Tables.Add(new DonorHistoryTDSDonorTable("Donor"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableGift != null))
            {
                this.TableGift.InitVars();
            }
            if ((this.TableDonor != null))
            {
                this.TableDonor.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "DonorHistoryTDS";
            this.TableGift = ((DonorHistoryTDSGiftTable)(this.Tables["Gift"]));
            this.TableDonor = ((DonorHistoryTDSDonorTable)(this.Tables["Donor"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {
        }
    }

    ///
    [Serializable()]
    public class DonorHistoryTDSGiftTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5606;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorShortNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnRecipientDescriptionId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnDateOfGiftId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftAmountId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationGroupCodeId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnMotivationDetailCodeId = 6;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "Gift", "DonorHistoryTDSGift",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "DonorKey", "DonorKey", "", OdbcType.Decimal, -1, false),
                    new TTypedColumnInfo(1, "DonorShortName", "DonorShortName", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(2, "RecipientDescription", "RecipientDescription", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(3, "DateOfGift", "DateOfGift", "", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "GiftAmount", "GiftAmount", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(5, "MotivationGroupCode", "MotivationGroupCode", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(6, "MotivationDetailCode", "MotivationDetailCode", "", OdbcType.VarChar, -1, false)
                },
                new int[] {
                }));
            return true;
        }

        /// constructor
        public DonorHistoryTDSGiftTable() :
                base("Gift")
        {
        }

        /// constructor
        public DonorHistoryTDSGiftTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public DonorHistoryTDSGiftTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnDonorKey;
        ///
        public DataColumn ColumnDonorShortName;
        ///
        public DataColumn ColumnRecipientDescription;
        ///
        public DataColumn ColumnDateOfGift;
        ///
        public DataColumn ColumnGiftAmount;
        ///
        public DataColumn ColumnMotivationGroupCode;
        ///
        public DataColumn ColumnMotivationDetailCode;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("DonorKey", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("DonorShortName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("RecipientDescription", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("DateOfGift", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("GiftAmount", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("MotivationGroupCode", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("MotivationDetailCode", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnDonorKey = this.Columns["DonorKey"];
            this.ColumnDonorShortName = this.Columns["DonorShortName"];
            this.ColumnRecipientDescription = this.Columns["RecipientDescription"];
            this.ColumnDateOfGift = this.Columns["DateOfGift"];
            this.ColumnGiftAmount = this.Columns["GiftAmount"];
            this.ColumnMotivationGroupCode = this.Columns["MotivationGroupCode"];
            this.ColumnMotivationDetailCode = this.Columns["MotivationDetailCode"];
        }

        /// Access a typed row by index
        public DonorHistoryTDSGiftRow this[int i]
        {
            get
            {
                return ((DonorHistoryTDSGiftRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public DonorHistoryTDSGiftRow NewRowTyped(bool AWithDefaultValues)
        {
            DonorHistoryTDSGiftRow ret = ((DonorHistoryTDSGiftRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public DonorHistoryTDSGiftRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new DonorHistoryTDSGiftRow(builder);
        }

        /// get typed set of changes
        public DonorHistoryTDSGiftTable GetChangesTyped()
        {
            return ((DonorHistoryTDSGiftTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "Gift";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "DonorHistoryTDSGift";
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
        public static string GetDonorShortNameDBName()
        {
            return "DonorShortName";
        }

        /// get character length for column
        public static short GetDonorShortNameLength()
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
        public static string GetDateOfGiftDBName()
        {
            return "DateOfGift";
        }

        /// get character length for column
        public static short GetDateOfGiftLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftAmountDBName()
        {
            return "GiftAmount";
        }

        /// get character length for column
        public static short GetGiftAmountLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationGroupCodeDBName()
        {
            return "MotivationGroupCode";
        }

        /// get character length for column
        public static short GetMotivationGroupCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetMotivationDetailCodeDBName()
        {
            return "MotivationDetailCode";
        }

        /// get character length for column
        public static short GetMotivationDetailCodeLength()
        {
            return -1;
        }

    }

    ///
    [Serializable()]
    public class DonorHistoryTDSGiftRow : System.Data.DataRow
    {
        private DonorHistoryTDSGiftTable myTable;

        /// Constructor
        public DonorHistoryTDSGiftRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((DonorHistoryTDSGiftTable)(this.Table));
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
        public string DonorShortName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorShortName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDonorShortName)
                            || (((string)(this[this.myTable.ColumnDonorShortName])) != value)))
                {
                    this[this.myTable.ColumnDonorShortName] = value;
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
        public DateTime DateOfGift
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateOfGift.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return ((DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateOfGift)
                            || (((DateTime)(this[this.myTable.ColumnDateOfGift])) != value)))
                {
                    this[this.myTable.ColumnDateOfGift] = value;
                }
            }
        }

        ///
        public Decimal GiftAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftAmount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Decimal)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftAmount)
                            || (((Decimal)(this[this.myTable.ColumnGiftAmount])) != value)))
                {
                    this[this.myTable.ColumnGiftAmount] = value;
                }
            }
        }

        ///
        public string MotivationGroupCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationGroupCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMotivationGroupCode)
                            || (((string)(this[this.myTable.ColumnMotivationGroupCode])) != value)))
                {
                    this[this.myTable.ColumnMotivationGroupCode] = value;
                }
            }
        }

        ///
        public string MotivationDetailCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationDetailCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMotivationDetailCode)
                            || (((string)(this[this.myTable.ColumnMotivationDetailCode])) != value)))
                {
                    this[this.myTable.ColumnMotivationDetailCode] = value;
                }
            }
        }

        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnDonorKey);
            this.SetNull(this.myTable.ColumnDonorShortName);
            this.SetNull(this.myTable.ColumnRecipientDescription);
            this.SetNull(this.myTable.ColumnDateOfGift);
            this.SetNull(this.myTable.ColumnGiftAmount);
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
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
        public bool IsDonorShortNameNull()
        {
            return this.IsNull(this.myTable.ColumnDonorShortName);
        }

        /// assign NULL value
        public void SetDonorShortNameNull()
        {
            this.SetNull(this.myTable.ColumnDonorShortName);
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
        public bool IsDateOfGiftNull()
        {
            return this.IsNull(this.myTable.ColumnDateOfGift);
        }

        /// assign NULL value
        public void SetDateOfGiftNull()
        {
            this.SetNull(this.myTable.ColumnDateOfGift);
        }

        /// test for NULL value
        public bool IsGiftAmountNull()
        {
            return this.IsNull(this.myTable.ColumnGiftAmount);
        }

        /// assign NULL value
        public void SetGiftAmountNull()
        {
            this.SetNull(this.myTable.ColumnGiftAmount);
        }

        /// test for NULL value
        public bool IsMotivationGroupCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationGroupCode);
        }

        /// assign NULL value
        public void SetMotivationGroupCodeNull()
        {
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
        }

        /// test for NULL value
        public bool IsMotivationDetailCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationDetailCode);
        }

        /// assign NULL value
        public void SetMotivationDetailCodeNull()
        {
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
        }
    }

    ///
    [Serializable()]
    public class DonorHistoryTDSDonorTable : TTypedDataTable
    {
        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 5607;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorKeyId = 0;
        /// used for generic TTypedDataTable functions
        public static short ColumnDonorShortNameId = 1;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftTotalCountId = 2;
        /// used for generic TTypedDataTable functions
        public static short ColumnGiftTotalAmountId = 3;
        /// used for generic TTypedDataTable functions
        public static short ColumnEmailId = 4;
        /// used for generic TTypedDataTable functions
        public static short ColumnValidAddressId = 5;
        /// used for generic TTypedDataTable functions
        public static short ColumnLocalityId = 6;
        /// used for generic TTypedDataTable functions
        public static short ColumnStreetNameId = 7;
        /// used for generic TTypedDataTable functions
        public static short ColumnBuilding1Id = 8;
        /// used for generic TTypedDataTable functions
        public static short ColumnBuilding2Id = 9;
        /// used for generic TTypedDataTable functions
        public static short ColumnAddress3Id = 10;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountryCodeId = 11;
        /// used for generic TTypedDataTable functions
        public static short ColumnCountryNameId = 12;
        /// used for generic TTypedDataTable functions
        public static short ColumnPostalCodeId = 13;
        /// used for generic TTypedDataTable functions
        public static short ColumnCityId = 14;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "Donor", "DonorHistoryTDSDonor",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "DonorKey", "DonorKey", "", OdbcType.Decimal, -1, false),
                    new TTypedColumnInfo(1, "DonorShortName", "DonorShortName", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(2, "GiftTotalCount", "GiftTotalCount", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "GiftTotalAmount", "GiftTotalAmount", "", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(4, "Email", "Email", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(5, "ValidAddress", "ValidAddress", "", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(6, "Locality", "Locality", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(7, "StreetName", "StreetName", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(8, "Building1", "Building1", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(9, "Building2", "Building2", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(10, "Address3", "Address3", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(11, "CountryCode", "CountryCode", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(12, "CountryName", "CountryName", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(13, "PostalCode", "PostalCode", "", OdbcType.VarChar, -1, false),
                    new TTypedColumnInfo(14, "City", "City", "", OdbcType.VarChar, -1, false)
                },
                new int[] {
                }));
            return true;
        }

        /// constructor
        public DonorHistoryTDSDonorTable() :
                base("Donor")
        {
        }

        /// constructor
        public DonorHistoryTDSDonorTable(string ATablename) :
                base(ATablename)
        {
        }

        /// constructor for serialization
        public DonorHistoryTDSDonorTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        ///
        public DataColumn ColumnDonorKey;
        ///
        public DataColumn ColumnDonorShortName;
        ///
        public DataColumn ColumnGiftTotalCount;
        ///
        public DataColumn ColumnGiftTotalAmount;
        ///
        public DataColumn ColumnEmail;
        ///
        public DataColumn ColumnValidAddress;
        ///
        public DataColumn ColumnLocality;
        ///
        public DataColumn ColumnStreetName;
        ///
        public DataColumn ColumnBuilding1;
        ///
        public DataColumn ColumnBuilding2;
        ///
        public DataColumn ColumnAddress3;
        ///
        public DataColumn ColumnCountryCode;
        ///
        public DataColumn ColumnCountryName;
        ///
        public DataColumn ColumnPostalCode;
        ///
        public DataColumn ColumnCity;

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("DonorKey", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("DonorShortName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("GiftTotalCount", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("GiftTotalAmount", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("Email", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("ValidAddress", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("Locality", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("StreetName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Building1", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Building2", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("Address3", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("CountryCode", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("CountryName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("PostalCode", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("City", typeof(string)));
        }

        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnDonorKey = this.Columns["DonorKey"];
            this.ColumnDonorShortName = this.Columns["DonorShortName"];
            this.ColumnGiftTotalCount = this.Columns["GiftTotalCount"];
            this.ColumnGiftTotalAmount = this.Columns["GiftTotalAmount"];
            this.ColumnEmail = this.Columns["Email"];
            this.ColumnValidAddress = this.Columns["ValidAddress"];
            this.ColumnLocality = this.Columns["Locality"];
            this.ColumnStreetName = this.Columns["StreetName"];
            this.ColumnBuilding1 = this.Columns["Building1"];
            this.ColumnBuilding2 = this.Columns["Building2"];
            this.ColumnAddress3 = this.Columns["Address3"];
            this.ColumnCountryCode = this.Columns["CountryCode"];
            this.ColumnCountryName = this.Columns["CountryName"];
            this.ColumnPostalCode = this.Columns["PostalCode"];
            this.ColumnCity = this.Columns["City"];
        }

        /// Access a typed row by index
        public DonorHistoryTDSDonorRow this[int i]
        {
            get
            {
                return ((DonorHistoryTDSDonorRow)(this.Rows[i]));
            }
        }

        /// create a new typed row
        public DonorHistoryTDSDonorRow NewRowTyped(bool AWithDefaultValues)
        {
            DonorHistoryTDSDonorRow ret = ((DonorHistoryTDSDonorRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }

        /// create a new typed row, always with default values
        public DonorHistoryTDSDonorRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new DonorHistoryTDSDonorRow(builder);
        }

        /// get typed set of changes
        public DonorHistoryTDSDonorTable GetChangesTyped()
        {
            return ((DonorHistoryTDSDonorTable)(base.GetChangesTypedInternal()));
        }

        /// return the CamelCase name of the table
        public static string GetTableName()
        {
            return "Donor";
        }

        /// return the name of the table as it is used in the database
        public static string GetTableDBName()
        {
            return "DonorHistoryTDSDonor";
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
        public static string GetDonorShortNameDBName()
        {
            return "DonorShortName";
        }

        /// get character length for column
        public static short GetDonorShortNameLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftTotalCountDBName()
        {
            return "GiftTotalCount";
        }

        /// get character length for column
        public static short GetGiftTotalCountLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetGiftTotalAmountDBName()
        {
            return "GiftTotalAmount";
        }

        /// get character length for column
        public static short GetGiftTotalAmountLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetEmailDBName()
        {
            return "Email";
        }

        /// get character length for column
        public static short GetEmailLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetValidAddressDBName()
        {
            return "ValidAddress";
        }

        /// get character length for column
        public static short GetValidAddressLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetLocalityDBName()
        {
            return "Locality";
        }

        /// get character length for column
        public static short GetLocalityLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetStreetNameDBName()
        {
            return "StreetName";
        }

        /// get character length for column
        public static short GetStreetNameLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBuilding1DBName()
        {
            return "Building1";
        }

        /// get character length for column
        public static short GetBuilding1Length()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetBuilding2DBName()
        {
            return "Building2";
        }

        /// get character length for column
        public static short GetBuilding2Length()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddress3DBName()
        {
            return "Address3";
        }

        /// get character length for column
        public static short GetAddress3Length()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCountryCodeDBName()
        {
            return "CountryCode";
        }

        /// get character length for column
        public static short GetCountryCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCountryNameDBName()
        {
            return "CountryName";
        }

        /// get character length for column
        public static short GetCountryNameLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetPostalCodeDBName()
        {
            return "PostalCode";
        }

        /// get character length for column
        public static short GetPostalCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCityDBName()
        {
            return "City";
        }

        /// get character length for column
        public static short GetCityLength()
        {
            return -1;
        }

    }

    ///
    [Serializable()]
    public class DonorHistoryTDSDonorRow : System.Data.DataRow
    {
        private DonorHistoryTDSDonorTable myTable;

        /// Constructor
        public DonorHistoryTDSDonorRow(System.Data.DataRowBuilder rb) :
                base(rb)
        {
            this.myTable = ((DonorHistoryTDSDonorTable)(this.Table));
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
        public string DonorShortName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDonorShortName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDonorShortName)
                            || (((string)(this[this.myTable.ColumnDonorShortName])) != value)))
                {
                    this[this.myTable.ColumnDonorShortName] = value;
                }
            }
        }

        ///
        public Int32 GiftTotalCount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftTotalCount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Int32)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftTotalCount)
                            || (((Int32)(this[this.myTable.ColumnGiftTotalCount])) != value)))
                {
                    this[this.myTable.ColumnGiftTotalCount] = value;
                }
            }
        }

        ///
        public Decimal GiftTotalAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftTotalAmount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Decimal)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnGiftTotalAmount)
                            || (((Decimal)(this[this.myTable.ColumnGiftTotalAmount])) != value)))
                {
                    this[this.myTable.ColumnGiftTotalAmount] = value;
                }
            }
        }

        ///
        public string Email
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnEmail.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnEmail)
                            || (((string)(this[this.myTable.ColumnEmail])) != value)))
                {
                    this[this.myTable.ColumnEmail] = value;
                }
            }
        }

        ///
        public bool ValidAddress
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnValidAddress.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnValidAddress)
                            || (((bool)(this[this.myTable.ColumnValidAddress])) != value)))
                {
                    this[this.myTable.ColumnValidAddress] = value;
                }
            }
        }

        ///
        public string Locality
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocality.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocality)
                            || (((string)(this[this.myTable.ColumnLocality])) != value)))
                {
                    this[this.myTable.ColumnLocality] = value;
                }
            }
        }

        ///
        public string StreetName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStreetName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnStreetName)
                            || (((string)(this[this.myTable.ColumnStreetName])) != value)))
                {
                    this[this.myTable.ColumnStreetName] = value;
                }
            }
        }

        ///
        public string Building1
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBuilding1.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBuilding1)
                            || (((string)(this[this.myTable.ColumnBuilding1])) != value)))
                {
                    this[this.myTable.ColumnBuilding1] = value;
                }
            }
        }

        ///
        public string Building2
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBuilding2.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBuilding2)
                            || (((string)(this[this.myTable.ColumnBuilding2])) != value)))
                {
                    this[this.myTable.ColumnBuilding2] = value;
                }
            }
        }

        ///
        public string Address3
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddress3.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddress3)
                            || (((string)(this[this.myTable.ColumnAddress3])) != value)))
                {
                    this[this.myTable.ColumnAddress3] = value;
                }
            }
        }

        ///
        public string CountryCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountryCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCountryCode)
                            || (((string)(this[this.myTable.ColumnCountryCode])) != value)))
                {
                    this[this.myTable.ColumnCountryCode] = value;
                }
            }
        }

        ///
        public string CountryName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountryName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCountryName)
                            || (((string)(this[this.myTable.ColumnCountryName])) != value)))
                {
                    this[this.myTable.ColumnCountryName] = value;
                }
            }
        }

        ///
        public string PostalCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPostalCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPostalCode)
                            || (((string)(this[this.myTable.ColumnPostalCode])) != value)))
                {
                    this[this.myTable.ColumnPostalCode] = value;
                }
            }
        }

        ///
        public string City
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCity.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCity)
                            || (((string)(this[this.myTable.ColumnCity])) != value)))
                {
                    this[this.myTable.ColumnCity] = value;
                }
            }
        }

        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnDonorKey);
            this.SetNull(this.myTable.ColumnDonorShortName);
            this.SetNull(this.myTable.ColumnGiftTotalCount);
            this.SetNull(this.myTable.ColumnGiftTotalAmount);
            this.SetNull(this.myTable.ColumnEmail);
            this.SetNull(this.myTable.ColumnValidAddress);
            this.SetNull(this.myTable.ColumnLocality);
            this.SetNull(this.myTable.ColumnStreetName);
            this.SetNull(this.myTable.ColumnBuilding1);
            this.SetNull(this.myTable.ColumnBuilding2);
            this.SetNull(this.myTable.ColumnAddress3);
            this.SetNull(this.myTable.ColumnCountryCode);
            this.SetNull(this.myTable.ColumnCountryName);
            this.SetNull(this.myTable.ColumnPostalCode);
            this.SetNull(this.myTable.ColumnCity);
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
        public bool IsDonorShortNameNull()
        {
            return this.IsNull(this.myTable.ColumnDonorShortName);
        }

        /// assign NULL value
        public void SetDonorShortNameNull()
        {
            this.SetNull(this.myTable.ColumnDonorShortName);
        }

        /// test for NULL value
        public bool IsGiftTotalCountNull()
        {
            return this.IsNull(this.myTable.ColumnGiftTotalCount);
        }

        /// assign NULL value
        public void SetGiftTotalCountNull()
        {
            this.SetNull(this.myTable.ColumnGiftTotalCount);
        }

        /// test for NULL value
        public bool IsGiftTotalAmountNull()
        {
            return this.IsNull(this.myTable.ColumnGiftTotalAmount);
        }

        /// assign NULL value
        public void SetGiftTotalAmountNull()
        {
            this.SetNull(this.myTable.ColumnGiftTotalAmount);
        }

        /// test for NULL value
        public bool IsEmailNull()
        {
            return this.IsNull(this.myTable.ColumnEmail);
        }

        /// assign NULL value
        public void SetEmailNull()
        {
            this.SetNull(this.myTable.ColumnEmail);
        }

        /// test for NULL value
        public bool IsValidAddressNull()
        {
            return this.IsNull(this.myTable.ColumnValidAddress);
        }

        /// assign NULL value
        public void SetValidAddressNull()
        {
            this.SetNull(this.myTable.ColumnValidAddress);
        }

        /// test for NULL value
        public bool IsLocalityNull()
        {
            return this.IsNull(this.myTable.ColumnLocality);
        }

        /// assign NULL value
        public void SetLocalityNull()
        {
            this.SetNull(this.myTable.ColumnLocality);
        }

        /// test for NULL value
        public bool IsStreetNameNull()
        {
            return this.IsNull(this.myTable.ColumnStreetName);
        }

        /// assign NULL value
        public void SetStreetNameNull()
        {
            this.SetNull(this.myTable.ColumnStreetName);
        }

        /// test for NULL value
        public bool IsBuilding1Null()
        {
            return this.IsNull(this.myTable.ColumnBuilding1);
        }

        /// assign NULL value
        public void SetBuilding1Null()
        {
            this.SetNull(this.myTable.ColumnBuilding1);
        }

        /// test for NULL value
        public bool IsBuilding2Null()
        {
            return this.IsNull(this.myTable.ColumnBuilding2);
        }

        /// assign NULL value
        public void SetBuilding2Null()
        {
            this.SetNull(this.myTable.ColumnBuilding2);
        }

        /// test for NULL value
        public bool IsAddress3Null()
        {
            return this.IsNull(this.myTable.ColumnAddress3);
        }

        /// assign NULL value
        public void SetAddress3Null()
        {
            this.SetNull(this.myTable.ColumnAddress3);
        }

        /// test for NULL value
        public bool IsCountryCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCountryCode);
        }

        /// assign NULL value
        public void SetCountryCodeNull()
        {
            this.SetNull(this.myTable.ColumnCountryCode);
        }

        /// test for NULL value
        public bool IsCountryNameNull()
        {
            return this.IsNull(this.myTable.ColumnCountryName);
        }

        /// assign NULL value
        public void SetCountryNameNull()
        {
            this.SetNull(this.myTable.ColumnCountryName);
        }

        /// test for NULL value
        public bool IsPostalCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPostalCode);
        }

        /// assign NULL value
        public void SetPostalCodeNull()
        {
            this.SetNull(this.myTable.ColumnPostalCode);
        }

        /// test for NULL value
        public bool IsCityNull()
        {
            return this.IsNull(this.myTable.ColumnCity);
        }

        /// assign NULL value
        public void SetCityNull()
        {
            this.SetNull(this.myTable.ColumnCity);
        }
    }
}
