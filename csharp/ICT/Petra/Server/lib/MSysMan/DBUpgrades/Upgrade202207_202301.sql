-- There is a record for each payment of a membership, or hours served
-- GROUP: partner
CREATE TABLE p_partner_membership_paid (

    -- The is the key to the membership table
  p_membership_code_c varchar(20) NOT NULL,
    -- This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
  p_partner_key_n bigint DEFAULT 0 NOT NULL,
    -- Date of the period that this payment was for
  p_period_date_d date,
    -- Date when this payment was made or the hours were served
  p_date_payment_d date NOT NULL,
    -- This is the amount that was paid.
  p_amount_paid_n numeric(24, 10) DEFAULT 0,
    -- This is the number of hours that were served.
  p_hours_served_n numeric(24, 10) DEFAULT 0,
    -- The date the record was created.
  s_date_created_d date,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT p_partner_membership_paid_pk
    PRIMARY KEY (p_membership_code_c,p_partner_key_n,p_date_payment_d)
) ENGINE=InnoDB
 CHARACTER SET UTF8
;

-- membership of a partner
-- GROUP: partner
CREATE TABLE p_partner_membership (

    -- A sequence key for the memberships.
  p_partner_membership_key_i integer NOT NULL,
    -- The is the key to the membership table
  p_membership_code_c varchar(20) NOT NULL,
    -- This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
  p_partner_key_n bigint DEFAULT 0 NOT NULL,
    -- Date the membership expires
  p_expiry_date_d date,
  p_date_cancelled_d date,
  p_start_date_d date NOT NULL,
  p_membership_status_c varchar(24) DEFAULT 'ACTIVE',
    -- The date the record was created.
  s_date_created_d date,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT p_partner_membership_pk
    PRIMARY KEY (p_partner_membership_key_i),
  CONSTRAINT p_partner_membership_uk
    UNIQUE (p_membership_code_c,p_partner_key_n,p_start_date_d)
) ENGINE=InnoDB
 CHARACTER SET UTF8
;

-- Details of a membership
-- GROUP: partner
CREATE TABLE p_membership (

    -- This is the key to the membership table
  p_membership_code_c varchar(20) NOT NULL,
    -- A short description of the membership
  p_membership_description_c varchar(80),
  p_valid_membership_l boolean DEFAULT 1,
  a_frequency_code_c varchar(24) NOT NULL,
    -- This is the amount charged for each membership period.
  a_membership_fee_n numeric(24, 10) DEFAULT 0,
    -- This is the time required to do service for each membership period.
  a_membership_hours_service_n numeric(24, 10) DEFAULT 0,
    -- The date the record was created.
  s_date_created_d date,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT p_membership_pk
    PRIMARY KEY (p_membership_code_c)
) ENGINE=InnoDB
 CHARACTER SET UTF8
;

CREATE TABLE seq_partner_membership (sequence INTEGER AUTO_INCREMENT, dummy INTEGER, PRIMARY KEY(sequence));

ALTER TABLE p_partner_membership_paid
  ADD CONSTRAINT p_partner_membership_paid_fk1
    FOREIGN KEY (p_membership_code_c)
    REFERENCES p_membership(p_membership_code_c);
ALTER TABLE p_partner_membership_paid
  ADD CONSTRAINT p_partner_membership_paid_fk2
    FOREIGN KEY (p_partner_key_n)
    REFERENCES p_partner(p_partner_key_n);
ALTER TABLE p_partner_membership_paid
  ADD CONSTRAINT p_partner_membership_paid_fkcr
    FOREIGN KEY (s_created_by_c)
    REFERENCES s_user(s_user_id_c);
ALTER TABLE p_partner_membership_paid
  ADD CONSTRAINT p_partner_membership_paid_fkmd
    FOREIGN KEY (s_modified_by_c)
    REFERENCES s_user(s_user_id_c);
ALTER TABLE p_partner_membership
  ADD CONSTRAINT p_partner_membership_fk1
    FOREIGN KEY (p_membership_code_c)
    REFERENCES p_membership(p_membership_code_c);
ALTER TABLE p_partner_membership
  ADD CONSTRAINT p_partner_membership_fk2
    FOREIGN KEY (p_partner_key_n)
    REFERENCES p_partner(p_partner_key_n);
ALTER TABLE p_partner_membership
  ADD CONSTRAINT p_partner_membership_fkcr
    FOREIGN KEY (s_created_by_c)
    REFERENCES s_user(s_user_id_c);
ALTER TABLE p_partner_membership
  ADD CONSTRAINT p_partner_membership_fkmd
    FOREIGN KEY (s_modified_by_c)
    REFERENCES s_user(s_user_id_c);

ALTER TABLE p_membership
  ADD CONSTRAINT p_membership_fk1
    FOREIGN KEY (a_frequency_code_c)
    REFERENCES a_frequency(a_frequency_code_c);
ALTER TABLE p_membership
  ADD CONSTRAINT p_membership_fkcr
    FOREIGN KEY (s_created_by_c)
    REFERENCES s_user(s_user_id_c);
ALTER TABLE p_membership
  ADD CONSTRAINT p_membership_fkmd
    FOREIGN KEY (s_modified_by_c)
    REFERENCES s_user(s_user_id_c);

CREATE UNIQUE INDEX inx_p_partner_membership_paid_pk0
   ON p_partner_membership_paid
   (p_membership_code_c,p_partner_key_n,p_date_payment_d);
CREATE INDEX inx_tner_membership_paid_fk1_key1
   ON p_partner_membership_paid
   (p_membership_code_c);
CREATE INDEX inx_tner_membership_paid_fk2_key2
   ON p_partner_membership_paid
   (p_partner_key_n);
CREATE INDEX inx_ner_membership_paid_fkcr_key3
   ON p_partner_membership_paid
   (s_created_by_c);
CREATE INDEX inx_ner_membership_paid_fkmd_key4
   ON p_partner_membership_paid
   (s_modified_by_c);
CREATE UNIQUE INDEX inx_p_partner_membership_pk0
   ON p_partner_membership
   (p_partner_membership_key_i);
CREATE UNIQUE INDEX inx_p_partner_membership_uk1
   ON p_partner_membership
   (p_membership_code_c,p_partner_key_n,p_start_date_d);
CREATE INDEX inx_p_partner_membership_fk1_key2
   ON p_partner_membership
   (p_membership_code_c);
CREATE INDEX inx_p_partner_membership_fk2_key3
   ON p_partner_membership
   (p_partner_key_n);
CREATE INDEX inx__partner_membership_fkcr_key4
   ON p_partner_membership
   (s_created_by_c);
CREATE INDEX inx__partner_membership_fkmd_key5
   ON p_partner_membership
   (s_modified_by_c);
CREATE INDEX p_expiry_k0
   ON p_partner_membership
   (p_expiry_date_d);

CREATE UNIQUE INDEX inx_p_membership_pk0
   ON p_membership
   (p_membership_code_c);
CREATE INDEX inx_p_membership_fk1_key1
   ON p_membership
   (a_frequency_code_c);
CREATE INDEX inx_p_membership_fkcr_key2
   ON p_membership
   (s_created_by_c);
CREATE INDEX inx_p_membership_fkmd_key3
   ON p_membership
   (s_modified_by_c);


ALTER TABLE a_motivation_detail ADD COLUMN a_sponsorship_l boolean DEFAULT 0 NOT NULL AFTER a_tax_deductible_account_code_c;
ALTER TABLE a_motivation_detail ADD COLUMN a_membership_l boolean DEFAULT 0 NOT NULL AFTER a_sponsorship_l;
ALTER TABLE a_motivation_detail ADD COLUMN a_worker_support_l boolean DEFAULT 0 NOT NULL AFTER a_membership_l;

UPDATE a_motivation_detail SET a_tax_deductible_l = 1;