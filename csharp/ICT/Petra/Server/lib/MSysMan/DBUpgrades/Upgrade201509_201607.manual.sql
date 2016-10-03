-- Need to run this manually as db user petraserver, otherwise the SYSADMIN cannot login to run the database upgrade...

--#################### s_login ###########################
--
-- Add 's_login_type_c' column 
--
ALTER TABLE s_login ADD COLUMN s_login_type_c character varying(45);

-- First make sure that there will be no rows that will not match the NOT NULL constraint that we are adding further down...
UPDATE s_login 
    SET s_login_type_c = '';

-- ...then populate s_login_type_c according to text found in s_login_status_c!
UPDATE s_login 
    SET s_login_type_c = 'LOGIN_SUCCESSFUL_SYSADMIN'
    WHERE s_login_status_c ILIKE 'User login - SYSADMIN privileges.%';

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_SUCCESSFUL_SYSADMIN'
    WHERE s_login_status_c ILIKE 'Successful - SYSADMIN%';  -- Older style

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_SUCCESSFUL_SYSADMIN'
    WHERE s_login_status_c ILIKE 'Successful  SYSADMIN%';  -- Older style

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_SUCCESSFUL'
    WHERE s_login_status_c ILIKE 'User Login.%';

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_SUCCESSFUL'
    WHERE s_login_status_c ILIKE 'Successful%';  -- Older style

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_ATTEMPT_PWD_WRONG'
    WHERE s_login_status_c ILIKE 'User supplied wrong password!%';

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_ATTEMPT_PWD_WRONG'
    WHERE s_login_status_c ILIKE 'Invalid Password%';    -- Older style
    
UPDATE s_login 
    SET s_login_type_c = 'LOGIN_ATTEMPT_FOR_LOCKED_USER'
    WHERE s_login_status_c ILIKE 'User attempted to log in, but the user account was locked!%';

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_ATTEMPT_FOR_RETIRED_USER'
    WHERE s_login_status_c ILIKE 'User attempted to log in, but the user is retired!%';

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_ATTEMPT_FOR_RETIRED_USER'
    WHERE s_login_status_c ILIKE 'Retired User%';    -- Older style

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_ATTEMPT_PWD_WRONG_ACCOUNT_GOT_LOCKED'
    WHERE s_login_status_c ILIKE 'User got auto-retired%';    -- Older style

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_ATTEMPT_FOR_NONEXISTING_USER'
    WHERE s_login_status_c ILIKE 'Invalid User%';    -- Older style

UPDATE s_login 
    SET s_login_type_c = 'LOGIN_ATTEMPT_WHEN_SYSTEM_WAS_DISABLED'
    WHERE s_login_status_c ILIKE 'User wanted to log in, but the System was disabled.%';
    
UPDATE s_login 
    SET s_login_type_c = 'LOGIN_ATTEMPT_WHEN_SYSTEM_WAS_DISABLED'
    WHERE s_login_status_c ILIKE 'System disabled%';    -- Older style
    
UPDATE s_login 
    SET s_login_type_c = 'NO_LOGIN'
    WHERE s_login_status_c ILIKE 'No login%';    -- Older style (no entry that corresponds with this will be written by OpenPetra anymore)

ALTER TABLE s_login RENAME COLUMN s_login_time_i TO s_time_i;
ALTER TABLE s_login RENAME COLUMN s_login_date_d TO s_date_i;
ALTER TABLE s_login ADD COLUMN s_login_details_c varchar(500);
ALTER TABLE s_login ADD COLUMN s_login_process_id_ref_i integer;
ALTER TABLE s_login DROP CONSTRAINT s_login_uk;
ALTER TABLE s_login DROP CONSTRAINT s_login_pk, ADD CONSTRAINT s_login_pk PRIMARY KEY (s_login_process_id_r);

--#################### s_user_account_activity ###########################
CREATE TABLE s_user_account_activity (
  s_user_id_c varchar(20) NOT NULL,
  s_activity_date_d date NOT NULL,
  s_activity_time_i integer DEFAULT 0 NOT NULL,
  s_activity_type_c varchar(50) NOT NULL,
  s_activity_details_c varchar(1000),
  s_date_created_d date DEFAULT CURRENT_DATE,
  s_created_by_c varchar(20),
  s_date_modified_d date,
  s_modified_by_c varchar(20),
  s_modification_id_t timestamp,
  CONSTRAINT s_user_account_activity_pk
    PRIMARY KEY (s_user_id_c,s_activity_date_d,s_activity_time_i,s_activity_type_c)
);

ALTER TABLE s_user_account_activity
  ADD CONSTRAINT s_user_account_activity_fkcr
    FOREIGN KEY (s_created_by_c)
    REFERENCES s_user(s_user_id_c);
ALTER TABLE s_user_account_activity
  ADD CONSTRAINT s_user_account_activity_fkmd
    FOREIGN KEY (s_modified_by_c)
    REFERENCES s_user(s_user_id_c);

CREATE UNIQUE INDEX inx_s_user_account_activity_pk0
   ON s_user_account_activity
   (s_user_id_c,s_activity_date_d,s_activity_time_i,s_activity_type_c);
CREATE INDEX inx_er_account_activity_fkcr_key1
   ON s_user_account_activity
   (s_created_by_c);
CREATE INDEX inx_er_account_activity_fkmd_key2
   ON s_user_account_activity
   (s_modified_by_c);
CREATE INDEX s_user_account_activity_date0
   ON s_user_account_activity
   (s_activity_date_d);
CREATE INDEX s_user_account_activity_user1
   ON s_user_account_activity
   (s_user_id_c);
CREATE INDEX s_user_account_activity_type2
   ON s_user_account_activity
   (s_activity_type_c);

--#################### s_user ###########################
ALTER TABLE s_user ADD COLUMN s_pwd_scheme_version_i integer DEFAULT 1;
ALTER TABLE s_user ADD COLUMN s_account_locked_l boolean DEFAULT '0';

--################################################################################################
-- Change name of System Default 'FailedLoginsUntilRetire' to 'FailedLoginsUntilAccountGetsLocked'
--################################################################################################
UPDATE s_system_defaults
  SET s_default_code_c = 'FailedLoginsUntilAccountGetsLocked'
  WHERE s_default_code_c = 'FailedLoginsUntilRetire'

