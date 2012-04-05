COPY a_fees_receivable (a_ledger_number_i, a_fee_code_c, a_charge_option_c, a_charge_percentage_n, a_charge_amount_n, a_cost_centre_code_c, a_account_code_c, a_fee_description_c, a_dr_account_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	HO_ADMIN	Percentage	7.00	0.0000000000	4300	3400	Home Office Admin Due	4900	2012-02-24	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000005
43	HO_ADMIN2	Fixed	0.00	10.0000000000	4300	3400	Home Office Admin Due2	4900	2012-02-24	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000005
\.

INSERT INTO a_motivation_detail_fee(a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c, a_fee_code_c) VALUES (43, 'GIFT', 'FIELD', 'HO_ADMIN');
INSERT INTO a_motivation_detail_fee(a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c, a_fee_code_c) VALUES (43, 'GIFT', 'SUPPORT', 'HO_ADMIN');
