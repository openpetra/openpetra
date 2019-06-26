CREATE TABLE s_session (
    -- This is the identifier of the session
  s_session_id_c varchar(128) NOT NULL,
    -- This is session is valid till this point in time
  s_valid_until_d datetime NOT NULL,
    -- JSON encoded list of session values.
  s_session_values_c text,
    -- The date the record was created.
  s_date_created_d date,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp,
  CONSTRAINT s_session_pk
    PRIMARY KEY (s_session_id_c)
) ENGINE=InnoDB
CHARACTER SET UTF8;
