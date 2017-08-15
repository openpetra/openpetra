--
-- PostgreSQL database dump: gl-test-year-end.sql
--


COPY a_cost_centre (a_ledger_number_i, a_cost_centre_code_c, a_cost_centre_to_report_to_c, a_cost_centre_name_c, a_posting_cost_centre_flag_l, a_cost_centre_active_flag_l, a_project_status_l, a_project_constraint_date_d, a_project_constraint_amount_n, a_system_cost_centre_flag_l, a_cost_centre_type_c, a_clearing_account_c, a_ret_earnings_account_code_c, a_rollup_style_c) FROM stdin;
{ledgernumber}	4301	{ledgernumber}00S	Test YearEnd 1	t	t	f	\N	0.0000000000	f	Local	8500	9700	Always
{ledgernumber}	4302	{ledgernumber}00S	Test YearEnd 2	t	t	f	\N	0.0000000000	f	Local	8500	9700	Always
{ledgernumber}	4303	{ledgernumber}00S	Test YearEnd 3	t	t	f	\N	0.0000000000	f	Local	8500	9700	Always
\.



