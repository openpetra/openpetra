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
namespace Ict.Petra.Shared.MFinance.AP.Data
{
    using Ict.Common;
    using Ict.Common.Data;
    using System;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Petra.Shared.MFinance.Account.Data;

     /// auto generated
    [Serializable()]
    public class AccountsPayableTDS : TTypedDataSet
    {

        private AApSupplierTable TableAApSupplier;
        private AApDocumentTable TableAApDocument;
        private AApDocumentDetailTable TableAApDocumentDetail;
        private AApDocumentPaymentTable TableAApDocumentPayment;
        private AApPaymentTable TableAApPayment;
        private AApAnalAttribTable TableAApAnalAttrib;

        /// auto generated
        public AccountsPayableTDS() :
                base("AccountsPayableTDS")
        {
        }

        /// auto generated for serialization
        public AccountsPayableTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }

        /// auto generated
        public AccountsPayableTDS(string ADatasetName) :
                base(ADatasetName)
        {
        }

        /// auto generated
        public AApSupplierTable AApSupplier
        {
            get
            {
                return this.TableAApSupplier;
            }
        }

        /// auto generated
        public AApDocumentTable AApDocument
        {
            get
            {
                return this.TableAApDocument;
            }
        }

        /// auto generated
        public AApDocumentDetailTable AApDocumentDetail
        {
            get
            {
                return this.TableAApDocumentDetail;
            }
        }

        /// auto generated
        public AApDocumentPaymentTable AApDocumentPayment
        {
            get
            {
                return this.TableAApDocumentPayment;
            }
        }

        /// auto generated
        public AApPaymentTable AApPayment
        {
            get
            {
                return this.TableAApPayment;
            }
        }

        /// auto generated
        public AApAnalAttribTable AApAnalAttrib
        {
            get
            {
                return this.TableAApAnalAttrib;
            }
        }

        /// auto generated
        public new virtual AccountsPayableTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((AccountsPayableTDS)(base.GetChangesTyped(removeEmptyTables)));
        }

        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new AApSupplierTable("AApSupplier"));
            this.Tables.Add(new AApDocumentTable("AApDocument"));
            this.Tables.Add(new AApDocumentDetailTable("AApDocumentDetail"));
            this.Tables.Add(new AApDocumentPaymentTable("AApDocumentPayment"));
            this.Tables.Add(new AApPaymentTable("AApPayment"));
            this.Tables.Add(new AApAnalAttribTable("AApAnalAttrib"));
        }

        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("AApSupplier") != -1))
            {
                this.Tables.Add(new AApSupplierTable("AApSupplier"));
            }
            if ((ds.Tables.IndexOf("AApDocument") != -1))
            {
                this.Tables.Add(new AApDocumentTable("AApDocument"));
            }
            if ((ds.Tables.IndexOf("AApDocumentDetail") != -1))
            {
                this.Tables.Add(new AApDocumentDetailTable("AApDocumentDetail"));
            }
            if ((ds.Tables.IndexOf("AApDocumentPayment") != -1))
            {
                this.Tables.Add(new AApDocumentPaymentTable("AApDocumentPayment"));
            }
            if ((ds.Tables.IndexOf("AApPayment") != -1))
            {
                this.Tables.Add(new AApPaymentTable("AApPayment"));
            }
            if ((ds.Tables.IndexOf("AApAnalAttrib") != -1))
            {
                this.Tables.Add(new AApAnalAttribTable("AApAnalAttrib"));
            }
        }

        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableAApSupplier != null))
            {
                this.TableAApSupplier.InitVars();
            }
            if ((this.TableAApDocument != null))
            {
                this.TableAApDocument.InitVars();
            }
            if ((this.TableAApDocumentDetail != null))
            {
                this.TableAApDocumentDetail.InitVars();
            }
            if ((this.TableAApDocumentPayment != null))
            {
                this.TableAApDocumentPayment.InitVars();
            }
            if ((this.TableAApPayment != null))
            {
                this.TableAApPayment.InitVars();
            }
            if ((this.TableAApAnalAttrib != null))
            {
                this.TableAApAnalAttrib.InitVars();
            }
        }

        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "AccountsPayableTDS";
            this.TableAApSupplier = ((AApSupplierTable)(this.Tables["AApSupplier"]));
            this.TableAApDocument = ((AApDocumentTable)(this.Tables["AApDocument"]));
            this.TableAApDocumentDetail = ((AApDocumentDetailTable)(this.Tables["AApDocumentDetail"]));
            this.TableAApDocumentPayment = ((AApDocumentPaymentTable)(this.Tables["AApDocumentPayment"]));
            this.TableAApPayment = ((AApPaymentTable)(this.Tables["AApPayment"]));
            this.TableAApAnalAttrib = ((AApAnalAttribTable)(this.Tables["AApAnalAttrib"]));
        }

        /// auto generated
        protected override void InitConstraints()
        {

            if (((this.TableAApDocumentDetail != null)
                        && (this.TableAApAnalAttrib != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApAnalAttrib1", "AApDocumentDetail", new string[] {
                                "a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"}, "AApAnalAttrib", new string[] {
                                "a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"}));
            }
            if (((this.TableAApSupplier != null)
                        && (this.TableAApDocument != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApDocument2", "AApSupplier", new string[] {
                                "p_partner_key_n"}, "AApDocument", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TableAApDocument != null)
                        && (this.TableAApDocumentDetail != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApDocumentDetail2", "AApDocument", new string[] {
                                "a_ledger_number_i", "a_ap_number_i"}, "AApDocumentDetail", new string[] {
                                "a_ledger_number_i", "a_ap_number_i"}));
            }
            if (((this.TableAApDocument != null)
                        && (this.TableAApDocumentPayment != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApDocumentPayment2", "AApDocument", new string[] {
                                "a_ledger_number_i", "a_ap_number_i"}, "AApDocumentPayment", new string[] {
                                "a_ledger_number_i", "a_ap_number_i"}));
            }
            if (((this.TableAApPayment != null)
                        && (this.TableAApDocumentPayment != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKApDocumentPayment3", "AApPayment", new string[] {
                                "a_ledger_number_i", "a_payment_number_i"}, "AApDocumentPayment", new string[] {
                                "a_ledger_number_i", "a_payment_number_i"}));
            }

        }
    }
}