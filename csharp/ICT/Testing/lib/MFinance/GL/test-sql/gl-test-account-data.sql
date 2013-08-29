--
-- PostgreSQL database dump
--


COPY a_account (a_ledger_number_i, a_account_code_c, a_account_type_c, a_account_code_long_desc_c, a_account_code_short_desc_c, a_eng_account_code_short_desc_c, a_eng_account_code_long_desc_c, a_debit_credit_indicator_l, a_account_active_flag_l, a_analysis_attribute_flag_l, a_standard_account_flag_l, a_consolidation_account_flag_l, a_intercompany_account_flag_l, a_budget_type_code_c, a_posting_status_l, a_system_account_flag_l, a_budget_control_flag_l, a_valid_cc_combo_c, a_foreign_currency_flag_l, a_foreign_currency_code_c, p_banking_details_key_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c) FROM stdin;
{ledgernumber}	6001	Asset	Petty Cash GBP	Petty Cash GBP	Petty Cash GBP	Petty Cash GBP	t	t	f	f	f	f	\N	t	f	f	All	t	GBP	\N	2011-03-31	DEMO	\N	\N
{ledgernumber}	6002	Asset	Petty Cash JPY	Petty Cash JPY	Petty Cash JPY	Petty Cash JPY	t	t	f	f	f	f	\N	t	f	f	All	t	JPY	\N	2011-03-31	DEMO	\N	\N
\.

