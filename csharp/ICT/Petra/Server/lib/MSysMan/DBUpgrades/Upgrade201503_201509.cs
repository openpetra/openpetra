//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;

namespace Ict.Petra.Server.MSysMan.DBUpgrades
{
    /// <summary>
    /// Upgrade the database
    /// </summary>
    public static partial class TDBUpgrade
    {
        /// Upgrade to version 2015-09
        public static bool UpgradeDatabase201503_201509()
        {
            // there are various changes to the database structure

            TDBTransaction SubmitChangesTransaction = null;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref SubmitChangesTransaction,
                ref SubmissionResult,
                delegate
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery("ALTER TABLE a_ep_match ALTER COLUMN a_receipt_letter_code_c TYPE VARCHAR(20)", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery("ALTER TABLE p_address_layout_code ADD COLUMN p_deletable_l boolean DEFAULT '1' NOT NULL", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"
CREATE TABLE p_address_block (
  p_country_code_c varchar(8) NOT NULL,
  p_address_layout_code_c varchar(32) DEFAULT 'SmlLabel' NOT NULL,
    -- The complete set of address lines, including replaceable parameters
  p_address_block_text_c varchar(512) NOT NULL,
    -- The date the record was created.
  s_date_created_d date DEFAULT CURRENT_DATE,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp,
  CONSTRAINT p_address_block_pk
    PRIMARY KEY (p_country_code_c,p_address_layout_code_c)
)", SubmitChangesTransaction);

                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"
CREATE TABLE p_address_block_element (

    -- This Code is used to identify the address element.
  p_address_element_code_c varchar(48) NOT NULL,
  p_address_element_description_c varchar(160),
    -- System flag indicates the element is a print directive and not a data placeholder
  p_is_directive_l boolean NOT NULL,
    -- The date the record was created.
  s_date_created_d date DEFAULT CURRENT_DATE,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp,
  CONSTRAINT p_address_block_element_pk
    PRIMARY KEY (p_address_element_code_c)
)", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"
CREATE TABLE p_form (

    -- The code which defines the type of form described in the table
  p_form_code_c varchar(20) NOT NULL,
    -- The name of the form being created for the form code.
  p_form_name_c varchar(20) NOT NULL,
    -- The language that this form is written in.  Use 99 if the form can be used for unspecified languages.
  p_form_language_c varchar(20) NOT NULL,
    -- Description of the form
  p_form_description_c varchar(100),
    -- If there are several types of form then it can be specified here.  Eg an annual receipt and an individual receipt.
  p_form_type_code_c varchar(24) NOT NULL,
    -- The address layout code that defines the address block content.
  p_address_layout_code_c varchar(16),
    -- The formality level to use if the template contains greetings or salutations. 1=Informal, 6=Very formal
  p_formality_level_i integer DEFAULT 1 NOT NULL,
    -- Is the template available in the database.
  p_template_available_l boolean DEFAULT '0' NOT NULL,
    -- The binary template file encoded as Base64 text
  p_template_document_c text,
    -- The file type associated with the template.
  p_template_file_extension_c varchar(16),
    -- Date the template was uploaded to the database
  p_template_upload_date_d date,
    -- Time the template was uploaded to the database
  p_template_upload_time_i integer,
  p_template_uploaded_by_user_id_c varchar(16),
    -- The minimum amount that is acceptable on a receipt
  p_minimum_amount_n numeric(24, 10) DEFAULT 0 NOT NULL,
    -- Allows the exclusion of certain records from a report
  p_options_c varchar(64),
    -- The date the record was created.
  s_date_created_d date DEFAULT CURRENT_DATE,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp,
  CONSTRAINT p_form_pk
    PRIMARY KEY (p_form_code_c,p_form_name_c,p_form_language_c)
)", SubmitChangesTransaction);
                    
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_ledger DROP COLUMN a_branch_processing_l", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_account_hierarchy_detail ALTER COLUMN a_report_order_i SET NOT NULL", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_cost_centre DROP CONSTRAINT a_cost_centre_fk3", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_cost_centre DROP COLUMN a_key_focus_area_c", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_cost_centre ADD COLUMN a_clearing_account_c varchar(24) DEFAULT '8500'", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_cost_centre ADD COLUMN a_ret_earnings_account_code_c varchar(24) DEFAULT '9700'", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_cost_centre ADD COLUMN a_rollup_style_c varchar(24) DEFAULT 'Always'", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_analysis_type ADD COLUMN a_ledger_number_i integer DEFAULT 0 NOT NULL", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"
ALTER TABLE a_analysis_type
  ADD CONSTRAINT a_analysis_type_fk1
    FOREIGN KEY (a_ledger_number_i)
    REFERENCES a_ledger(a_ledger_number_i)
)", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_analysis_attribute DROP CONSTRAINT a_analysis_attribute_fk2", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"
ALTER TABLE a_analysis_attribute
  ADD CONSTRAINT a_analysis_attribute_fk2
    FOREIGN KEY (a_ledger_number_i,a_analysis_type_code_c)
    REFERENCES a_analysis_type(a_ledger_number_i,a_analysis_type_code_c)
)", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_form ADD COLUMN a_form_file_name_c varchar(2000)", SubmitChangesTransaction);
                    
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_freeform_analysis DROP CONSTRAINT a_freeform_analysis_fk2", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"
ALTER TABLE a_freeform_analysis
  ADD CONSTRAINT a_freeform_analysis_fk2
    FOREIGN KEY (a_ledger_number_i,a_analysis_type_code_c)
    REFERENCES a_analysis_type(a_ledger_number_i,a_analysis_type_code_c)
)", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_recurring_transaction ALTER COLUMN a_reference_c SET NOT NULL", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE _transaction ALTER COLUMN a_reference_c SET NOT NULL", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_gift ADD COLUMN a_link_to_previous_gift_l boolean DEFAULT '0' NOT NULL", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_gift ADD COLUMN a_print_receipt_l boolean DEFAULT '1' NOT NULL", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"ALTER TABLE a_gift_detail ADD COLUMN a_fixed_gift_destination_l boolean DEFAULT '0'", SubmitChangesTransaction);
                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"
CREATE TABLE a_revaluation (

    -- The revaluation journal belongs to this ledger.
  a_ledger_number_i integer DEFAULT 0 NOT NULL,
    -- identifes which batch the revaluation journal belongs to.
  a_batch_number_i integer DEFAULT 0 NOT NULL,
    -- Identifies the revaluation journal within a batch (usually 1)
  a_journal_number_i integer NOT NULL,
    -- This defines which revaluation currency the rate applies to
  a_revaluation_currency_c varchar(16) NOT NULL,
    -- The rate of exchange from the revaluation currency (in a_revaluation_currency_c) to the ledger base currency.
  a_exchange_rate_to_base_n numeric(24, 10) DEFAULT 0 NOT NULL,
    -- The date the record was created.
  s_date_created_d date DEFAULT CURRENT_DATE,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp,
  CONSTRAINT a_revaluation_pk
    PRIMARY KEY (a_ledger_number_i,a_batch_number_i,a_journal_number_i)
)
", SubmitChangesTransaction);

                    DBAccess.GDBAccessObj.ExecuteNonQuery(@"DROP TABLE a_key_focus_area", SubmitChangesTransaction);
                    SubmissionResult = TSubmitChangesResult.scrOK;
                });
            return true;
        }
    }
}