--
-- PostgreSQL database dump
--

-- Dumped from database version 9.0.1
-- Dumped by pg_dump version 9.0.1
-- Started on 2011-04-11 11:41:23

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = off;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET escape_string_warning = off;

SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 2481 (class 1259 OID 596120)
-- Dependencies: 2979 2980 2981 2982 2983 2984 2985 2986 2987 2988 2989 2990 2991 2992 2993 5
-- Name: a_gift; Type: TABLE; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE TABLE a_gift (
    a_ledger_number_i integer DEFAULT 0 NOT NULL,
    a_batch_number_i integer DEFAULT 0 NOT NULL,
    a_gift_transaction_number_i integer DEFAULT 0 NOT NULL,
    a_gift_status_c character varying(24),
    a_date_entered_d date DEFAULT ('now'::text)::date NOT NULL,
    a_home_admin_charges_flag_l boolean DEFAULT true NOT NULL,
    a_ilt_admin_charges_flag_l boolean DEFAULT true NOT NULL,
    a_receipt_letter_code_c character varying(16),
    a_method_of_giving_code_c character varying(24),
    a_method_of_payment_code_c character varying(16),
    p_donor_key_n numeric(10,0) DEFAULT 0 NOT NULL,
    a_admin_charge_l boolean DEFAULT false,
    a_receipt_number_i integer DEFAULT 0,
    a_last_detail_number_i integer DEFAULT 0 NOT NULL,
    a_reference_c character varying(20),
    a_first_time_gift_l boolean DEFAULT false,
    a_receipt_printed_l boolean DEFAULT false NOT NULL,
    a_restricted_l boolean DEFAULT false,
    p_banking_details_key_i integer DEFAULT 0 NOT NULL,
    s_date_created_d date DEFAULT ('now'::text)::date,
    s_created_by_c character varying(20),
    s_date_modified_d date,
    s_modified_by_c character varying(20),
    s_modification_id_c character varying(150)
);


ALTER TABLE public.a_gift OWNER TO wolfgangu;

--
-- TOC entry 2480 (class 1259 OID 596103)
-- Dependencies: 2967 2968 2969 2970 2971 2972 2973 2974 2975 2976 2977 2978 5
-- Name: a_gift_batch; Type: TABLE; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE TABLE a_gift_batch (
    a_ledger_number_i integer DEFAULT 0 NOT NULL,
    a_batch_number_i integer DEFAULT 0 NOT NULL,
    a_batch_description_c character varying(80),
    s_modification_date_d date DEFAULT ('now'::text)::date,
    a_hash_total_n numeric(24,10) DEFAULT 0,
    a_batch_total_n numeric(24,10) DEFAULT 0,
    a_bank_account_code_c character varying(16) NOT NULL,
    a_last_gift_number_i integer DEFAULT 0,
    a_batch_status_c character varying(16) DEFAULT 'Unposted'::character varying,
    a_batch_period_i integer DEFAULT 0 NOT NULL,
    a_batch_year_i integer NOT NULL,
    a_gl_effective_date_d date DEFAULT ('now'::text)::date NOT NULL,
    a_currency_code_c character varying(16) NOT NULL,
    a_exchange_rate_to_base_n numeric(24,10) DEFAULT 0 NOT NULL,
    a_bank_cost_centre_c character varying(24) NOT NULL,
    a_gift_type_c character varying(16) DEFAULT 'Gift'::character varying NOT NULL,
    a_method_of_payment_code_c character varying(16),
    s_date_created_d date DEFAULT ('now'::text)::date,
    s_created_by_c character varying(20),
    s_date_modified_d date,
    s_modified_by_c character varying(20),
    s_modification_id_c character varying(150)
);


ALTER TABLE public.a_gift_batch OWNER TO wolfgangu;

--
-- TOC entry 2482 (class 1259 OID 596140)
-- Dependencies: 2994 2995 2996 2997 2998 2999 3000 3001 3002 3003 3004 3005 3006 3007 3008 5
-- Name: a_gift_detail; Type: TABLE; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE TABLE a_gift_detail (
    a_ledger_number_i integer DEFAULT 0 NOT NULL,
    a_batch_number_i integer DEFAULT 0 NOT NULL,
    a_gift_transaction_number_i integer DEFAULT 0 NOT NULL,
    a_detail_number_i integer DEFAULT 0 NOT NULL,
    a_recipient_ledger_number_n numeric(10,0) DEFAULT 0 NOT NULL,
    a_gift_amount_n numeric(24,10) DEFAULT 0,
    a_motivation_group_code_c character varying(16) NOT NULL,
    a_motivation_detail_code_c character varying(16) NOT NULL,
    a_comment_one_type_c character varying(24),
    a_gift_comment_one_c character varying(160),
    a_confidential_gift_flag_l boolean DEFAULT false NOT NULL,
    a_tax_deductable_l boolean DEFAULT true,
    p_recipient_key_n numeric(10,0) DEFAULT 0 NOT NULL,
    a_charge_flag_l boolean DEFAULT true,
    a_cost_centre_code_c character varying(24),
    a_gift_amount_intl_n numeric(24,10) DEFAULT 0,
    a_modified_detail_l boolean DEFAULT false,
    a_gift_transaction_amount_n numeric(24,10) DEFAULT 0 NOT NULL,
    a_ich_number_i integer DEFAULT 0 NOT NULL,
    p_mailing_code_c character varying(50),
    a_comment_two_type_c character varying(24),
    a_gift_comment_two_c character varying(160),
    a_comment_three_type_c character varying(24),
    a_gift_comment_three_c character varying(160),
    s_date_created_d date DEFAULT ('now'::text)::date,
    s_created_by_c character varying(20),
    s_date_modified_d date,
    s_modified_by_c character varying(20),
    s_modification_id_c character varying(150)
);


ALTER TABLE public.a_gift_detail OWNER TO wolfgangu;

--
-- TOC entry 3066 (class 0 OID 596120)
-- Dependencies: 2481
-- Data for Name: a_gift; Type: TABLE DATA; Schema: public; Owner: wolfgangu
--

COPY a_gift (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, a_gift_status_c, a_date_entered_d, a_home_admin_charges_flag_l, a_ilt_admin_charges_flag_l, a_receipt_letter_code_c, a_method_of_giving_code_c, a_method_of_payment_code_c, p_donor_key_n, a_admin_charge_l, a_receipt_number_i, a_last_detail_number_i, a_reference_c, a_first_time_gift_l, a_receipt_printed_l, a_restricted_l, p_banking_details_key_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	1	1	\N	2010-01-11	t	t	\N	\N	\N	0	f	0	1	r	f	f	f	0	2011-04-11	DEMO	2011-04-11	DEMO	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000454
43	1	2	\N	2010-01-11	t	t	\N	\N	\N	0	f	0	1	\N	f	f	f	0	2011-04-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000455
\.


--
-- TOC entry 3065 (class 0 OID 596103)
-- Dependencies: 2480
-- Data for Name: a_gift_batch; Type: TABLE DATA; Schema: public; Owner: wolfgangu
--

COPY a_gift_batch (a_ledger_number_i, a_batch_number_i, a_batch_description_c, s_modification_date_d, a_hash_total_n, a_batch_total_n, a_bank_account_code_c, a_last_gift_number_i, a_batch_status_c, a_batch_period_i, a_batch_year_i, a_gl_effective_date_d, a_currency_code_c, a_exchange_rate_to_base_n, a_bank_cost_centre_c, a_gift_type_c, a_method_of_payment_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	1	test-batch	2011-04-11	0.0000000000	30.0000000000	6200	2	Unposted	14	0	2010-01-01	EUR	1.0000000000	4300	Gift	\N	2011-04-11	DEMO	2011-04-11	DEMO	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000453
\.


--
-- TOC entry 3067 (class 0 OID 596140)
-- Dependencies: 2482
-- Data for Name: a_gift_detail; Type: TABLE DATA; Schema: public; Owner: wolfgangu
--

COPY a_gift_detail (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, a_detail_number_i, a_recipient_ledger_number_n, a_gift_amount_n, a_motivation_group_code_c, a_motivation_detail_code_c, a_comment_one_type_c, a_gift_comment_one_c, a_confidential_gift_flag_l, a_tax_deductable_l, p_recipient_key_n, a_charge_flag_l, a_cost_centre_code_c, a_gift_amount_intl_n, a_modified_detail_l, a_gift_transaction_amount_n, a_ich_number_i, p_mailing_code_c, a_comment_two_type_c, a_gift_comment_two_c, a_comment_three_type_c, a_gift_comment_three_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	1	1	1	0	0.0000000000	GIFT	FIELD	Both	\N	f	t	0	t	4300	0.0000000000	f	10.0000000000	0	\N	Both	\N	Both	\N	2011-04-11	DEMO	2011-04-11	DEMO	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000456
43	1	2	1	0	0.0000000000	GIFT	FIELD	Both	\N	f	t	0	t	4300	0.0000000000	f	20.0000000000	0	\N	Both	\N	Both	\N	2011-04-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000457
\.


--
-- TOC entry 3010 (class 2606 OID 596119)
-- Dependencies: 2480 2480 2480
-- Name: a_gift_batch_pk; Type: CONSTRAINT; Schema: public; Owner: wolfgangu; Tablespace: 
--

ALTER TABLE ONLY a_gift_batch
    ADD CONSTRAINT a_gift_batch_pk PRIMARY KEY (a_ledger_number_i, a_batch_number_i);


--
-- TOC entry 3032 (class 2606 OID 596162)
-- Dependencies: 2482 2482 2482 2482 2482
-- Name: a_gift_detail_pk; Type: CONSTRAINT; Schema: public; Owner: wolfgangu; Tablespace: 
--

ALTER TABLE ONLY a_gift_detail
    ADD CONSTRAINT a_gift_detail_pk PRIMARY KEY (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, a_detail_number_i);


--
-- TOC entry 3023 (class 2606 OID 596139)
-- Dependencies: 2481 2481 2481 2481
-- Name: a_gift_pk; Type: CONSTRAINT; Schema: public; Owner: wolfgangu; Tablespace: 
--

ALTER TABLE ONLY a_gift
    ADD CONSTRAINT a_gift_pk PRIMARY KEY (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i);


--
-- TOC entry 3020 (class 1259 OID 605273)
-- Dependencies: 2481 2481
-- Name: a_date_entered0; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX a_date_entered0 ON a_gift USING btree (a_ledger_number_i, a_date_entered_d);


--
-- TOC entry 3021 (class 1259 OID 605274)
-- Dependencies: 2481 2481
-- Name: a_donor1; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX a_donor1 ON a_gift USING btree (p_donor_key_n, a_ledger_number_i);


--
-- TOC entry 3033 (class 1259 OID 605285)
-- Dependencies: 2482 2482 2482
-- Name: a_recipient_ledger_number_i0; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX a_recipient_ledger_number_i0 ON a_gift_detail USING btree (a_ledger_number_i, a_recipient_ledger_number_n, p_recipient_key_n);


--
-- TOC entry 3011 (class 1259 OID 605264)
-- Dependencies: 2480 2480 2480
-- Name: date_effective0; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX date_effective0 ON a_gift_batch USING btree (a_ledger_number_i, a_batch_status_c, a_gl_effective_date_d);


--
-- TOC entry 3012 (class 1259 OID 605258)
-- Dependencies: 2480
-- Name: inx_a_gift_batch_fk1_key1; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_batch_fk1_key1 ON a_gift_batch USING btree (a_ledger_number_i);


--
-- TOC entry 3013 (class 1259 OID 605259)
-- Dependencies: 2480 2480
-- Name: inx_a_gift_batch_fk2_key2; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_batch_fk2_key2 ON a_gift_batch USING btree (a_ledger_number_i, a_bank_account_code_c);


--
-- TOC entry 3014 (class 1259 OID 605260)
-- Dependencies: 2480 2480
-- Name: inx_a_gift_batch_fk3_key3; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_batch_fk3_key3 ON a_gift_batch USING btree (a_ledger_number_i, a_bank_cost_centre_c);


--
-- TOC entry 3015 (class 1259 OID 605261)
-- Dependencies: 2480
-- Name: inx_a_gift_batch_fk4_key4; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_batch_fk4_key4 ON a_gift_batch USING btree (a_currency_code_c);


--
-- TOC entry 3016 (class 1259 OID 605262)
-- Dependencies: 2480
-- Name: inx_a_gift_batch_fkcr_key5; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_batch_fkcr_key5 ON a_gift_batch USING btree (s_created_by_c);


--
-- TOC entry 3017 (class 1259 OID 605263)
-- Dependencies: 2480
-- Name: inx_a_gift_batch_fkmd_key6; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_batch_fkmd_key6 ON a_gift_batch USING btree (s_modified_by_c);


--
-- TOC entry 3018 (class 1259 OID 605257)
-- Dependencies: 2480 2480
-- Name: inx_a_gift_batch_pk0; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE UNIQUE INDEX inx_a_gift_batch_pk0 ON a_gift_batch USING btree (a_ledger_number_i, a_batch_number_i);


--
-- TOC entry 3034 (class 1259 OID 605277)
-- Dependencies: 2482 2482 2482
-- Name: inx_a_gift_detail_fk1_key2; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_detail_fk1_key2 ON a_gift_detail USING btree (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i);


--
-- TOC entry 3035 (class 1259 OID 605278)
-- Dependencies: 2482 2482 2482
-- Name: inx_a_gift_detail_fk2_key3; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_detail_fk2_key3 ON a_gift_detail USING btree (a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c);


--
-- TOC entry 3036 (class 1259 OID 605279)
-- Dependencies: 2482
-- Name: inx_a_gift_detail_fk3_key4; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_detail_fk3_key4 ON a_gift_detail USING btree (p_recipient_key_n);


--
-- TOC entry 3037 (class 1259 OID 605280)
-- Dependencies: 2482
-- Name: inx_a_gift_detail_fk4_key5; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_detail_fk4_key5 ON a_gift_detail USING btree (p_mailing_code_c);


--
-- TOC entry 3038 (class 1259 OID 605281)
-- Dependencies: 2482
-- Name: inx_a_gift_detail_fk5_key6; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_detail_fk5_key6 ON a_gift_detail USING btree (a_recipient_ledger_number_n);


--
-- TOC entry 3039 (class 1259 OID 605282)
-- Dependencies: 2482 2482
-- Name: inx_a_gift_detail_fk6_key7; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_detail_fk6_key7 ON a_gift_detail USING btree (a_ledger_number_i, a_cost_centre_code_c);


--
-- TOC entry 3040 (class 1259 OID 605283)
-- Dependencies: 2482
-- Name: inx_a_gift_detail_fkcr_key8; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_detail_fkcr_key8 ON a_gift_detail USING btree (s_created_by_c);


--
-- TOC entry 3041 (class 1259 OID 605284)
-- Dependencies: 2482
-- Name: inx_a_gift_detail_fkmd_key9; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_detail_fkmd_key9 ON a_gift_detail USING btree (s_modified_by_c);


--
-- TOC entry 3042 (class 1259 OID 605276)
-- Dependencies: 2482 2482 2482 2482
-- Name: inx_a_gift_detail_pk1; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE UNIQUE INDEX inx_a_gift_detail_pk1 ON a_gift_detail USING btree (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, a_detail_number_i);


--
-- TOC entry 3024 (class 1259 OID 605267)
-- Dependencies: 2481 2481
-- Name: inx_a_gift_fk1_key1; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_fk1_key1 ON a_gift USING btree (a_ledger_number_i, a_batch_number_i);


--
-- TOC entry 3025 (class 1259 OID 605268)
-- Dependencies: 2481
-- Name: inx_a_gift_fk2_key2; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_fk2_key2 ON a_gift USING btree (a_method_of_giving_code_c);


--
-- TOC entry 3026 (class 1259 OID 605269)
-- Dependencies: 2481
-- Name: inx_a_gift_fk3_key3; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_fk3_key3 ON a_gift USING btree (a_method_of_payment_code_c);


--
-- TOC entry 3027 (class 1259 OID 605270)
-- Dependencies: 2481
-- Name: inx_a_gift_fk4_key4; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_fk4_key4 ON a_gift USING btree (p_donor_key_n);


--
-- TOC entry 3028 (class 1259 OID 605271)
-- Dependencies: 2481
-- Name: inx_a_gift_fkcr_key5; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_fkcr_key5 ON a_gift USING btree (s_created_by_c);


--
-- TOC entry 3029 (class 1259 OID 605272)
-- Dependencies: 2481
-- Name: inx_a_gift_fkmd_key6; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_gift_fkmd_key6 ON a_gift USING btree (s_modified_by_c);


--
-- TOC entry 3030 (class 1259 OID 605266)
-- Dependencies: 2481 2481 2481
-- Name: inx_a_gift_pk0; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE UNIQUE INDEX inx_a_gift_pk0 ON a_gift USING btree (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i);


--
-- TOC entry 3043 (class 1259 OID 605275)
-- Dependencies: 2482 2482 2482 2482
-- Name: inx_a_processed_fee_fk3_ref0; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_processed_fee_fk3_ref0 ON a_gift_detail USING btree (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, a_detail_number_i);


--
-- TOC entry 3044 (class 1259 OID 605286)
-- Dependencies: 2482 2482
-- Name: p_recipient_key1; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX p_recipient_key1 ON a_gift_detail USING btree (a_ledger_number_i, p_recipient_key_n);


--
-- TOC entry 3019 (class 1259 OID 605265)
-- Dependencies: 2480 2480 2480 2480
-- Name: period_effective1; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX period_effective1 ON a_gift_batch USING btree (a_ledger_number_i, a_batch_status_c, a_batch_year_i, a_batch_period_i);


--
-- TOC entry 3050 (class 2606 OID 600877)
-- Dependencies: 2424 2480
-- Name: a_gift_batch_fk1; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_batch
    ADD CONSTRAINT a_gift_batch_fk1 FOREIGN KEY (a_ledger_number_i) REFERENCES a_ledger(a_ledger_number_i);


--
-- TOC entry 3049 (class 2606 OID 600882)
-- Dependencies: 2480 2480 2428 2428
-- Name: a_gift_batch_fk2; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_batch
    ADD CONSTRAINT a_gift_batch_fk2 FOREIGN KEY (a_ledger_number_i, a_bank_account_code_c) REFERENCES a_account(a_ledger_number_i, a_account_code_c);


--
-- TOC entry 3048 (class 2606 OID 600887)
-- Dependencies: 2480 2434 2434 2480
-- Name: a_gift_batch_fk3; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_batch
    ADD CONSTRAINT a_gift_batch_fk3 FOREIGN KEY (a_ledger_number_i, a_bank_cost_centre_c) REFERENCES a_cost_centre(a_ledger_number_i, a_cost_centre_code_c);


--
-- TOC entry 3047 (class 2606 OID 600892)
-- Dependencies: 2320 2480
-- Name: a_gift_batch_fk4; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_batch
    ADD CONSTRAINT a_gift_batch_fk4 FOREIGN KEY (a_currency_code_c) REFERENCES a_currency(a_currency_code_c);


--
-- TOC entry 3046 (class 2606 OID 600897)
-- Dependencies: 2321 2480
-- Name: a_gift_batch_fkcr; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_batch
    ADD CONSTRAINT a_gift_batch_fkcr FOREIGN KEY (s_created_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 3045 (class 2606 OID 600902)
-- Dependencies: 2480 2321
-- Name: a_gift_batch_fkmd; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_batch
    ADD CONSTRAINT a_gift_batch_fkmd FOREIGN KEY (s_modified_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 3064 (class 2606 OID 600937)
-- Dependencies: 2482 2482 2481 2482 2481 2481 3022
-- Name: a_gift_detail_fk1; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_detail
    ADD CONSTRAINT a_gift_detail_fk1 FOREIGN KEY (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i) REFERENCES a_gift(a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i);


--
-- TOC entry 3063 (class 2606 OID 600942)
-- Dependencies: 2460 2460 2460 2482 2482 2482
-- Name: a_gift_detail_fk2; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_detail
    ADD CONSTRAINT a_gift_detail_fk2 FOREIGN KEY (a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c) REFERENCES a_motivation_detail(a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c);


--
-- TOC entry 3062 (class 2606 OID 600947)
-- Dependencies: 2354 2482
-- Name: a_gift_detail_fk3; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_detail
    ADD CONSTRAINT a_gift_detail_fk3 FOREIGN KEY (p_recipient_key_n) REFERENCES p_partner(p_partner_key_n);


--
-- TOC entry 3061 (class 2606 OID 600952)
-- Dependencies: 2396 2482
-- Name: a_gift_detail_fk4; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_detail
    ADD CONSTRAINT a_gift_detail_fk4 FOREIGN KEY (p_mailing_code_c) REFERENCES p_mailing(p_mailing_code_c);


--
-- TOC entry 3060 (class 2606 OID 600957)
-- Dependencies: 2354 2482
-- Name: a_gift_detail_fk5; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_detail
    ADD CONSTRAINT a_gift_detail_fk5 FOREIGN KEY (a_recipient_ledger_number_n) REFERENCES p_partner(p_partner_key_n);


--
-- TOC entry 3059 (class 2606 OID 600962)
-- Dependencies: 2434 2482 2482 2434
-- Name: a_gift_detail_fk6; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_detail
    ADD CONSTRAINT a_gift_detail_fk6 FOREIGN KEY (a_ledger_number_i, a_cost_centre_code_c) REFERENCES a_cost_centre(a_ledger_number_i, a_cost_centre_code_c);


--
-- TOC entry 3058 (class 2606 OID 600967)
-- Dependencies: 2321 2482
-- Name: a_gift_detail_fkcr; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_detail
    ADD CONSTRAINT a_gift_detail_fkcr FOREIGN KEY (s_created_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 3057 (class 2606 OID 600972)
-- Dependencies: 2321 2482
-- Name: a_gift_detail_fkmd; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift_detail
    ADD CONSTRAINT a_gift_detail_fkmd FOREIGN KEY (s_modified_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 3056 (class 2606 OID 600907)
-- Dependencies: 2481 2480 2481 3009 2480
-- Name: a_gift_fk1; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift
    ADD CONSTRAINT a_gift_fk1 FOREIGN KEY (a_ledger_number_i, a_batch_number_i) REFERENCES a_gift_batch(a_ledger_number_i, a_batch_number_i);


--
-- TOC entry 3055 (class 2606 OID 600912)
-- Dependencies: 2481 2457
-- Name: a_gift_fk2; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift
    ADD CONSTRAINT a_gift_fk2 FOREIGN KEY (a_method_of_giving_code_c) REFERENCES a_method_of_giving(a_method_of_giving_code_c);


--
-- TOC entry 3054 (class 2606 OID 600917)
-- Dependencies: 2458 2481
-- Name: a_gift_fk3; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift
    ADD CONSTRAINT a_gift_fk3 FOREIGN KEY (a_method_of_payment_code_c) REFERENCES a_method_of_payment(a_method_of_payment_code_c);


--
-- TOC entry 3053 (class 2606 OID 600922)
-- Dependencies: 2354 2481
-- Name: a_gift_fk4; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift
    ADD CONSTRAINT a_gift_fk4 FOREIGN KEY (p_donor_key_n) REFERENCES p_partner(p_partner_key_n);


--
-- TOC entry 3052 (class 2606 OID 600927)
-- Dependencies: 2321 2481
-- Name: a_gift_fkcr; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift
    ADD CONSTRAINT a_gift_fkcr FOREIGN KEY (s_created_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 3051 (class 2606 OID 600932)
-- Dependencies: 2321 2481
-- Name: a_gift_fkmd; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_gift
    ADD CONSTRAINT a_gift_fkmd FOREIGN KEY (s_modified_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 3070 (class 0 OID 0)
-- Dependencies: 2481
-- Name: a_gift; Type: ACL; Schema: public; Owner: wolfgangu
--

REVOKE ALL ON TABLE a_gift FROM PUBLIC;
REVOKE ALL ON TABLE a_gift FROM wolfgangu;
GRANT ALL ON TABLE a_gift TO wolfgangu;
GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE a_gift TO petraserver;


--
-- TOC entry 3071 (class 0 OID 0)
-- Dependencies: 2480
-- Name: a_gift_batch; Type: ACL; Schema: public; Owner: wolfgangu
--

REVOKE ALL ON TABLE a_gift_batch FROM PUBLIC;
REVOKE ALL ON TABLE a_gift_batch FROM wolfgangu;
GRANT ALL ON TABLE a_gift_batch TO wolfgangu;
GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE a_gift_batch TO petraserver;


--
-- TOC entry 3072 (class 0 OID 0)
-- Dependencies: 2482
-- Name: a_gift_detail; Type: ACL; Schema: public; Owner: wolfgangu
--

REVOKE ALL ON TABLE a_gift_detail FROM PUBLIC;
REVOKE ALL ON TABLE a_gift_detail FROM wolfgangu;
GRANT ALL ON TABLE a_gift_detail TO wolfgangu;
GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE a_gift_detail TO petraserver;


-- Completed on 2011-04-11 11:41:23

--
-- PostgreSQL database dump complete
--

