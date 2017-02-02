ALTER TABLE a_recurring_trans_anal_attrib
  DROP CONSTRAINT a_recurring_trans_anal_attr_fk3;
ALTER TABLE a_recurring_trans_anal_attrib
  ADD CONSTRAINT a_recurring_trans_anal_attr_fk3
    FOREIGN KEY (a_ledger_number_i,a_account_code_c,a_analysis_type_code_c)
    REFERENCES a_analysis_attribute(a_ledger_number_i,a_account_code_c,a_analysis_type_code_c);
    
ALTER TABLE a_trans_anal_attrib
  DROP CONSTRAINT a_trans_anal_attrib_fk3;
ALTER TABLE a_trans_anal_attrib
  ADD CONSTRAINT a_trans_anal_attrib_fk3
    FOREIGN KEY (a_ledger_number_i,a_account_code_c,a_analysis_type_code_c)
    REFERENCES a_analysis_attribute(a_ledger_number_i,a_account_code_c,a_analysis_type_code_c);

ALTER TABLE a_ap_anal_attrib
  DROP CONSTRAINT a_ap_anal_attrib_fk2;
ALTER TABLE a_ap_anal_attrib
  ADD CONSTRAINT a_ap_anal_attrib_fk2
    FOREIGN KEY (a_ledger_number_i,a_account_code_c,a_analysis_type_code_c)
    REFERENCES a_analysis_attribute(a_ledger_number_i,a_account_code_c,a_analysis_type_code_c);

ALTER TABLE pm_special_need 
	ALTER COLUMN pm_medical_comment_c varchar(5000),
	ALTER COLUMN pm_dietary_comment_c varchar(5000),
	ALTER COLUMN pm_other_special_need_c varchar(5000);

ALTER TABLE p_partner_reminder
	ALTER COLUMN p_comment_c varchar(5000);

