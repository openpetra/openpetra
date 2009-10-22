-- the matches that can be used to identify recurring gift or GL transactions
-- GROUP: account
CREATE TABLE a_ep_match (

    -- this is a sequence to easily identify which transaction has been matched and how
  a_ep_match_key_i integer,
    -- this is a separated list of all the recurring details of a_ep_transaction (ie. name, bank account, sort code, IBAN, amount, description)
  a_match_text_c varchar(100),
    -- the match can be applied to split gifts as well
  a_detail_i integer DEFAULT 0,
    -- What to do with this match: gift, GL, or discard
  a_action_c varchar(40) NOT NULL,
    -- The date when this match was recently applied; useful for purging old entries
  a_recent_match_d date DEFAULT CURRENT_DATE NOT NULL,
    -- The four digit ledger number of the gift.
  a_ledger_number_i integer DEFAULT 0 NOT NULL,
    -- The partner key of the commitment field (the unit) of the recipient of the gift.  This is not the ledger number but rather the partner key of the unit associated with the ledger.
  a_recipient_ledger_number_n numeric(10, 0) DEFAULT 0 NOT NULL,
    -- This defines a motivation group.
  a_motivation_group_code_c varchar(16) NOT NULL,
    -- This defines the motivation detail within a motivation group.
  a_motivation_detail_code_c varchar(16) NOT NULL,
    -- Used to decide whose reports will see this comment
  a_comment_one_type_c varchar(24),
    -- This is a long description and is 80 characters long.
  a_gift_comment_one_c varchar(160),
    -- Defines whether the donor wishes the recipient to know who gave the gift
  a_confidential_gift_flag_l boolean DEFAULT '0' NOT NULL,
    -- Whether this gift is tax deductable
  a_tax_deductable_l boolean DEFAULT '1',
    -- The partner key of the recipient of the gift.
  p_recipient_key_n numeric(10, 0) DEFAULT 0 NOT NULL,
    -- To determine whether an admin fee on the transaction should be overwritten if it normally has a charge associated with it. Used for both local and ilt transaction.
  a_charge_flag_l boolean DEFAULT '1',
    -- This identifies which cost centre an account is applied to. A cost centre can be a partner.
  a_cost_centre_code_c varchar(24),
    -- Mailing Code of the mailing that the gift was a response to.
  p_mailing_code_c varchar(50),
    -- Used to decide whose reports will see this comment
  a_comment_two_type_c varchar(24),
    -- This is a long description and is 80 characters long.
  a_gift_comment_two_c varchar(160),
    -- Used to decide whose reports will see this comment
  a_comment_three_type_c varchar(24),
    -- This is a long description and is 80 characters long.
  a_gift_comment_three_c varchar(160),
    -- This is a number of currency units in the entered Currency
  a_gift_transaction_amount_n numeric(24, 10) DEFAULT 0 NOT NULL,
    -- Used to get a yes no response from the user
  a_home_admin_charges_flag_l boolean DEFAULT '1' NOT NULL,
    -- Used to get a yes no response from the user
  a_ilt_admin_charges_flag_l boolean DEFAULT '1' NOT NULL,
  a_receipt_letter_code_c varchar(16),
    -- Defines how a gift is given.
  a_method_of_giving_code_c varchar(24),
    -- This is how the partner paid. Eg cash, Cheque etc
  a_method_of_payment_code_c varchar(16),
    -- This is the partner key of the donor.
  p_donor_key_n numeric(10, 0) DEFAULT 0 NOT NULL,
    -- NOT USED AT ALL
  a_admin_charge_l boolean DEFAULT '0',
    -- Reference number/code for the transaction
  a_reference_c varchar(20),
    -- Indicates whether or not the gift has restricted access. If it does then the access will be controlled by s_group_gift
  a_restricted_l boolean DEFAULT '0',
    -- The date the record was created.
  s_date_created_d date DEFAULT CURRENT_DATE,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_c varchar(150),
  CONSTRAINT a_ep_match_pk
    PRIMARY KEY (a_ep_match_key_i),
  CONSTRAINT a_ep_match_uk
    UNIQUE (a_match_text_c,a_detail_i)
);

-- CREATE SEQUENCE "seq_match_number"
--   INCREMENT BY 1
--  MINVALUE 0
--   MAXVALUE 999999999
--   START WITH 0;

-- workaround for sqlite which does not support sequences natively
CREATE TABLE seq_match_number
(
    sequence INTEGER PRIMARY KEY AUTOINCREMENT, 
    dummy INTEGER
);

INSERT INTO seq_match_number VALUES(NULL, -1);

CREATE TABLE seq_modification1
(
    sequence INTEGER PRIMARY KEY AUTOINCREMENT, 
    dummy INTEGER
);
INSERT INTO seq_modification1 VALUES(NULL, -1);

CREATE TABLE seq_modification2
(
    sequence INTEGER PRIMARY KEY AUTOINCREMENT, 
    dummy INTEGER
);
INSERT INTO seq_modification2 VALUES(NULL, -1);
