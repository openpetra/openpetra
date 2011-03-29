--
-- PostgreSQL database dump+
-- Data to test GetBatchInfo in TestGLPeriodicEndMonth
--

-- Dumped from database version 9.0.1
-- Dumped by pg_dump version 9.0.1
-- Started on 2011-03-29 15:48:57

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
-- TOC entry 2483 (class 1259 OID 572439)
-- Dependencies: 2967 2968 2969 2970 2971 2972 2973 2974 2975 2976 2977 2978 5
-- Name: a_batch; Type: TABLE; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE TABLE a_batch (
    a_ledger_number_i integer DEFAULT 0 NOT NULL,
    a_batch_number_i integer DEFAULT 0 NOT NULL,
    a_batch_description_c character varying(160),
    a_batch_control_total_n numeric(24,10) DEFAULT 0,
    a_batch_running_total_n numeric(24,10) DEFAULT 0 NOT NULL,
    a_batch_debit_total_n numeric(24,10) DEFAULT 0 NOT NULL,
    a_batch_credit_total_n numeric(24,10) DEFAULT 0 NOT NULL,
    a_batch_period_i integer DEFAULT 0 NOT NULL,
    a_date_effective_d date DEFAULT ('now'::text)::date NOT NULL,
    a_date_of_entry_d date DEFAULT ('now'::text)::date NOT NULL,
    a_batch_status_c character varying(24) DEFAULT 'Unposted'::character varying,
    a_last_journal_i integer DEFAULT 0 NOT NULL,
    s_date_created_d date DEFAULT ('now'::text)::date,
    s_created_by_c character varying(20),
    s_date_modified_d date,
    s_modified_by_c character varying(20),
    s_modification_id_c character varying(150)
);


ALTER TABLE public.a_batch OWNER TO wolfgangu;

--
-- TOC entry 2990 (class 0 OID 572439)
-- Dependencies: 2483
-- Data for Name: a_batch; Type: TABLE DATA; Schema: public; Owner: wolfgangu
--

COPY a_batch (a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_batch_control_total_n, a_batch_running_total_n, a_batch_debit_total_n, a_batch_credit_total_n, a_batch_period_i, a_date_effective_d, a_date_of_entry_d, a_batch_status_c, a_last_journal_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	3	TestGLPeriodicEndMonth-TESTDATA	0.0000000000	0.0000000000	0.0000000000	0.0000000000	1	2011-03-29	2011-03-29	Unposted	0	2011-03-29	\N	\N	\N	\N
43	0	TestGLPeriodicEndMonth-TESTDATA	0.0000000000	0.0000000000	0.0000000000	0.0000000000	1	2011-03-29	2011-03-29	Posted	0	2011-03-29	\N	\N	\N	\N
43	2	TestGLPeriodicEndMonth-TESTDATA	0.0000000000	0.0000000000	0.0000000000	0.0000000000	1	2011-03-29	2011-03-29	Cancelled	0	2011-03-29	\N	\N	\N	\N
43	1	TestGLPeriodicEndMonth-TESTDATA	0.0000000000	0.0000000000	0.0000000000	0.0000000000	1	2011-03-29	2011-03-29	HasTransactions	0	2011-03-29	\N	\N	\N	\N
\.


--
-- TOC entry 2980 (class 2606 OID 572455)
-- Dependencies: 2483 2483 2483
-- Name: a_batch_pk; Type: CONSTRAINT; Schema: public; Owner: wolfgangu; Tablespace: 
--

ALTER TABLE ONLY a_batch
    ADD CONSTRAINT a_batch_pk PRIMARY KEY (a_ledger_number_i, a_batch_number_i);


--
-- TOC entry 2981 (class 1259 OID 581564)
-- Dependencies: 2483
-- Name: inx_a_batch_fk1_key1; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_batch_fk1_key1 ON a_batch USING btree (a_ledger_number_i);


--
-- TOC entry 2982 (class 1259 OID 581565)
-- Dependencies: 2483 2483
-- Name: inx_a_batch_fk2_key2; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_batch_fk2_key2 ON a_batch USING btree (a_ledger_number_i, a_batch_period_i);


--
-- TOC entry 2983 (class 1259 OID 581566)
-- Dependencies: 2483
-- Name: inx_a_batch_fkcr_key3; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_batch_fkcr_key3 ON a_batch USING btree (s_created_by_c);


--
-- TOC entry 2984 (class 1259 OID 581567)
-- Dependencies: 2483
-- Name: inx_a_batch_fkmd_key4; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_batch_fkmd_key4 ON a_batch USING btree (s_modified_by_c);


--
-- TOC entry 2985 (class 1259 OID 581563)
-- Dependencies: 2483 2483
-- Name: inx_a_batch_pk0; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE UNIQUE INDEX inx_a_batch_pk0 ON a_batch USING btree (a_ledger_number_i, a_batch_number_i);


--
-- TOC entry 2989 (class 2606 OID 577253)
-- Dependencies: 2483 2424
-- Name: a_batch_fk1; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_batch
    ADD CONSTRAINT a_batch_fk1 FOREIGN KEY (a_ledger_number_i) REFERENCES a_ledger(a_ledger_number_i);


--
-- TOC entry 2988 (class 2606 OID 577258)
-- Dependencies: 2483 2439 2483 2439
-- Name: a_batch_fk2; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_batch
    ADD CONSTRAINT a_batch_fk2 FOREIGN KEY (a_ledger_number_i, a_batch_period_i) REFERENCES a_accounting_period(a_ledger_number_i, a_accounting_period_number_i);


--
-- TOC entry 2987 (class 2606 OID 577263)
-- Dependencies: 2321 2483
-- Name: a_batch_fkcr; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_batch
    ADD CONSTRAINT a_batch_fkcr FOREIGN KEY (s_created_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 2986 (class 2606 OID 577268)
-- Dependencies: 2483 2321
-- Name: a_batch_fkmd; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_batch
    ADD CONSTRAINT a_batch_fkmd FOREIGN KEY (s_modified_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 2993 (class 0 OID 0)
-- Dependencies: 2483
-- Name: a_batch; Type: ACL; Schema: public; Owner: wolfgangu
--

REVOKE ALL ON TABLE a_batch FROM PUBLIC;
REVOKE ALL ON TABLE a_batch FROM wolfgangu;
GRANT ALL ON TABLE a_batch TO wolfgangu;
GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE a_batch TO petraserver;


-- Completed on 2011-03-29 15:48:57

--
-- PostgreSQL database dump complete
--

