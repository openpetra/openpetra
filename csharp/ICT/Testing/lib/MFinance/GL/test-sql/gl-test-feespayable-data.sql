COPY a_fees_payable (a_ledger_number_i, a_fee_code_c, a_charge_option_c, a_charge_percentage_n, a_charge_amount_n, a_cost_centre_code_c, a_account_code_c, a_fee_description_c, a_dr_account_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	ICT	Minimum	10.00	10.0000000000	4300	3400	International Admin	4900	2012-02-29	DEMO	2012-02-29	DEMO	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000001824
43	GIF	Fixed	0.00	12.0000000000	4300	3400	Global Impact Fund	4900	2012-02-24	DEMO	2012-02-29	DEMO	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000001826
43	ICT2	Maximum	10.00	15.0000000000	4300	3400	International Admin2	4900	2012-02-29	DEMO	2012-02-29	DEMO	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000001827
43	GIF2	Percentage	1.00	0.0000000000	4300	3400	Global Impact Fund2	4900	2012-02-29	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000001828
\.

INSERT INTO a_motivation_detail_fee(a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c, a_fee_code_c) VALUES (43, 'GIFT', 'FIELD', 'ICT');
INSERT INTO a_motivation_detail_fee(a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c, a_fee_code_c) VALUES (43, 'GIFT', 'FIELD', 'GIF');
INSERT INTO a_motivation_detail_fee(a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c, a_fee_code_c) VALUES (43, 'GIFT', 'SUPPORT', 'ICT');
INSERT INTO a_motivation_detail_fee(a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c, a_fee_code_c) VALUES (43, 'GIFT', 'SUPPORT', 'GIF');