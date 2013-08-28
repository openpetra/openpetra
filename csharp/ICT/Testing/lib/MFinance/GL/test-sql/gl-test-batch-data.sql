--
-- PostgreSQL database dump+
-- Data to test GetBatchInfo in TestGLPeriodicEndMonth
-- Be careful in case auf automatic test data creation:
--
-- ************* this data are loaded into data base and after the test they are deleted *********

COPY a_batch (a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_batch_control_total_n, a_batch_running_total_n, a_batch_debit_total_n, a_batch_credit_total_n, a_batch_period_i, a_date_effective_d, a_date_of_entry_d, a_batch_status_c, a_last_journal_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_t) FROM stdin;
{ledgernumber}	3	TestGLPeriodicEndMonth-TESTDATA	0.0000000000	0.0000000000	0.0000000000	0.0000000000	1	2011-03-29	2011-03-29	Unposted	0	2011-03-29	\N	\N	\N	\N
{ledgernumber}	0	TestGLPeriodicEndMonth-TESTDATA	0.0000000000	0.0000000000	0.0000000000	0.0000000000	1	2011-03-29	2011-03-29	Posted	0	2011-03-29	\N	\N	\N	\N
{ledgernumber}	2	TestGLPeriodicEndMonth-TESTDATA	0.0000000000	0.0000000000	0.0000000000	0.0000000000	1	2011-03-29	2011-03-29	Cancelled	0	2011-03-29	\N	\N	\N	\N
{ledgernumber}	1	TestGLPeriodicEndMonth-TESTDATA	0.0000000000	0.0000000000	0.0000000000	0.0000000000	1	2011-03-29	2011-03-29	HasTransactions	0	2011-03-29	\N	\N	\N	\N
\.

