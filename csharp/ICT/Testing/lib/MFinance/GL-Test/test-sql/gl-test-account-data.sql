--
-- PostgreSQL database dump
--

-- Dumped from database version 9.0.1
-- Dumped by pg_dump version 9.0.1
-- Started on 2011-03-31 10:19:27

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
-- TOC entry 2428 (class 1259 OID 571750)
-- Dependencies: 2967 2968 2969 2970 2971 2972 2973 2974 2975 2976 2977 2978 2979 5
-- Name: a_account; Type: TABLE; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE TABLE a_account (
    a_ledger_number_i integer DEFAULT 0 NOT NULL,
    a_account_code_c character varying(16) NOT NULL,
    a_account_type_c character varying(20),
    a_account_code_long_desc_c character varying(160),
    a_account_code_short_desc_c character varying(64),
    a_eng_account_code_short_desc_c character varying(64),
    a_eng_account_code_long_desc_c character varying(160),
    a_debit_credit_indicator_l boolean DEFAULT true,
    a_account_active_flag_l boolean DEFAULT true,
    a_analysis_attribute_flag_l boolean DEFAULT false,
    a_standard_account_flag_l boolean DEFAULT false,
    a_consolidation_account_flag_l boolean DEFAULT false,
    a_intercompany_account_flag_l boolean DEFAULT false,
    a_budget_type_code_c character varying(16),
    a_posting_status_l boolean DEFAULT true,
    a_system_account_flag_l boolean DEFAULT false,
    a_budget_control_flag_l boolean DEFAULT false,
    a_valid_cc_combo_c character varying(16) DEFAULT 'All'::character varying,
    a_foreign_currency_flag_l boolean DEFAULT false,
    a_foreign_currency_code_c character varying(16),
    p_banking_details_key_i integer,
    s_date_created_d date DEFAULT ('now'::text)::date,
    s_created_by_c character varying(20),
    s_date_modified_d date,
    s_modified_by_c character varying(20),
    s_modification_id_c character varying(150)
);


ALTER TABLE public.a_account OWNER TO wolfgangu;

--
-- TOC entry 2996 (class 0 OID 571750)
-- Dependencies: 2428
-- Data for Name: a_account; Type: TABLE DATA; Schema: public; Owner: wolfgangu
--

COPY a_account (a_ledger_number_i, a_account_code_c, a_account_type_c, a_account_code_long_desc_c, a_account_code_short_desc_c, a_eng_account_code_short_desc_c, a_eng_account_code_long_desc_c, a_debit_credit_indicator_l, a_account_active_flag_l, a_analysis_attribute_flag_l, a_standard_account_flag_l, a_consolidation_account_flag_l, a_intercompany_account_flag_l, a_budget_type_code_c, a_posting_status_l, a_system_account_flag_l, a_budget_control_flag_l, a_valid_cc_combo_c, a_foreign_currency_flag_l, a_foreign_currency_code_c, p_banking_details_key_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	6001	Asset	Petty Cash GBP	Petty Cash GBP	Petty Cash GBP	Petty Cash GBP	t	t	f	f	f	f	\N	t	f	f	All	t	GBP	\N	2011-03-31	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000348
43	6002	Asset	Petty Cash JPY	Petty Cash JPY	Petty Cash JPY	Petty Cash JPY	t	t	f	f	f	f	\N	t	f	f	All	t	JPY	\N	2011-03-31	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000349
\.


--
-- TOC entry 2981 (class 2606 OID 571770)
-- Dependencies: 2428 2428 2428
-- Name: a_account_pk; Type: CONSTRAINT; Schema: public; Owner: wolfgangu; Tablespace: 
--

ALTER TABLE ONLY a_account
    ADD CONSTRAINT a_account_pk PRIMARY KEY (a_ledger_number_i, a_account_code_c);


--
-- TOC entry 2982 (class 1259 OID 581243)
-- Dependencies: 2428
-- Name: inx_a_account_fk1_key2; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_account_fk1_key2 ON a_account USING btree (a_ledger_number_i);


--
-- TOC entry 2983 (class 1259 OID 581244)
-- Dependencies: 2428
-- Name: inx_a_account_fk2_key3; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_account_fk2_key3 ON a_account USING btree (a_budget_type_code_c);


--
-- TOC entry 2984 (class 1259 OID 581245)
-- Dependencies: 2428
-- Name: inx_a_account_fk3_key4; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_account_fk3_key4 ON a_account USING btree (a_foreign_currency_code_c);


--
-- TOC entry 2985 (class 1259 OID 581246)
-- Dependencies: 2428
-- Name: inx_a_account_fk4_key5; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_account_fk4_key5 ON a_account USING btree (p_banking_details_key_i);


--
-- TOC entry 2986 (class 1259 OID 581247)
-- Dependencies: 2428
-- Name: inx_a_account_fkcr_key6; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_account_fkcr_key6 ON a_account USING btree (s_created_by_c);


--
-- TOC entry 2987 (class 1259 OID 581248)
-- Dependencies: 2428
-- Name: inx_a_account_fkmd_key7; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_account_fkmd_key7 ON a_account USING btree (s_modified_by_c);


--
-- TOC entry 2988 (class 1259 OID 581242)
-- Dependencies: 2428 2428
-- Name: inx_a_account_pk1; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE UNIQUE INDEX inx_a_account_pk1 ON a_account USING btree (a_ledger_number_i, a_account_code_c);


--
-- TOC entry 2989 (class 1259 OID 581241)
-- Dependencies: 2428 2428
-- Name: inx_a_ep_account_fk3_ref0; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_ep_account_fk3_ref0 ON a_account USING btree (a_ledger_number_i, a_account_code_c);


--
-- TOC entry 2995 (class 2606 OID 576013)
-- Dependencies: 2428 2424
-- Name: a_account_fk1; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_account
    ADD CONSTRAINT a_account_fk1 FOREIGN KEY (a_ledger_number_i) REFERENCES a_ledger(a_ledger_number_i);


--
-- TOC entry 2994 (class 2606 OID 576018)
-- Dependencies: 2428 2427
-- Name: a_account_fk2; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_account
    ADD CONSTRAINT a_account_fk2 FOREIGN KEY (a_budget_type_code_c) REFERENCES a_budget_type(a_budget_type_code_c);


--
-- TOC entry 2993 (class 2606 OID 576023)
-- Dependencies: 2428 2320
-- Name: a_account_fk3; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_account
    ADD CONSTRAINT a_account_fk3 FOREIGN KEY (a_foreign_currency_code_c) REFERENCES a_currency(a_currency_code_c);


--
-- TOC entry 2992 (class 2606 OID 576028)
-- Dependencies: 2428 2376
-- Name: a_account_fk4; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_account
    ADD CONSTRAINT a_account_fk4 FOREIGN KEY (p_banking_details_key_i) REFERENCES p_banking_details(p_banking_details_key_i);


--
-- TOC entry 2991 (class 2606 OID 576033)
-- Dependencies: 2428 2321
-- Name: a_account_fkcr; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_account
    ADD CONSTRAINT a_account_fkcr FOREIGN KEY (s_created_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 2990 (class 2606 OID 576038)
-- Dependencies: 2428 2321
-- Name: a_account_fkmd; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_account
    ADD CONSTRAINT a_account_fkmd FOREIGN KEY (s_modified_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 2999 (class 0 OID 0)
-- Dependencies: 2428
-- Name: a_account; Type: ACL; Schema: public; Owner: wolfgangu
--

REVOKE ALL ON TABLE a_account FROM PUBLIC;
REVOKE ALL ON TABLE a_account FROM wolfgangu;
GRANT ALL ON TABLE a_account TO wolfgangu;
GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE a_account TO petraserver;


-- Completed on 2011-03-31 10:19:27

--
-- PostgreSQL database dump complete
--

