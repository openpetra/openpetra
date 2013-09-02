-- updates for Postgresql database for OpenPetra
-- 0.2.25 at August 29, 2013
ALTER TABLE a_transaction ALTER COLUMN a_narrative_c varchar(500);

ALTER TABLE a_motivation_detail ADD COLUMN a_report_column_c varchar(40);
UPDATE a_motivation_detail SET a_report_column_c = 'Field' where a_motivation_detail_code_c <> 'SUPPORT' AND a_motivation_detail_code_c <> 'PERSONAL';
UPDATE a_motivation_detail SET a_report_column_c = 'Worker' where a_motivation_detail_code_c = 'SUPPORT' OR a_motivation_detail_code_c = 'PERSONAL';

ALTER TABLE pc_room_alloc ALTER COLUMN ph_gender_c DROP NOT NULL;
ALTER TABLE pm_job_assignment DROP COLUMN pm_leaving_code_c;
ALTER TABLE pm_job_assignment DROP COLUMN pm_leaving_code_updated_date_d;

ALTER TABLE pm_past_experience ALTER COLUMN pm_start_date_d DROP NOT NULL;
ALTER TABLE pm_past_experience ALTER COLUMN pm_end_date_d DROP NOT NULL;

ALTER TABLE a_ap_payment ADD COLUMN  a_currency_code_c varchar(16) NOT NULL;
UPDATE a_ap_payment SET a_currency_code_c = 'EUR';

DROP TABLE pt_leaving_code;

ALTER TABLE p_postcode_region RENAME TO p_postcode_region_range;

CREATE TABLE p_postcode_region (

    -- Name of a postcode region
  p_region_c varchar(64) NOT NULL,
    -- This describes the region.
  p_description_c varchar(100),
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
  CONSTRAINT p_postcode_region_pk
    PRIMARY KEY (p_region_c)
);
