--
-- PostgreSQL database dump
--

-- Dumped from database version 9.0.1
-- Dumped by pg_dump version 9.0.1
-- Started on 2011-03-31 10:20:45

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
-- TOC entry 2434 (class 1259 OID 571807)
-- Dependencies: 2967 2968 2969 2970 2971 2972 2973 2974 5
-- Name: a_cost_centre; Type: TABLE; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE TABLE a_cost_centre (
    a_ledger_number_i integer DEFAULT 0 NOT NULL,
    a_cost_centre_code_c character varying(24) NOT NULL,
    a_cost_centre_to_report_to_c character varying(24),
    a_cost_centre_name_c character varying(64) NOT NULL,
    a_posting_cost_centre_flag_l boolean DEFAULT true NOT NULL,
    a_cost_centre_active_flag_l boolean DEFAULT true,
    a_project_status_l boolean DEFAULT false,
    a_project_constraint_date_d date,
    a_project_constraint_amount_n numeric(24,10) DEFAULT 0,
    a_system_cost_centre_flag_l boolean DEFAULT false,
    a_cost_centre_type_c character varying(16) DEFAULT 'Local'::character varying,
    a_key_focus_area_c character varying(40),
    s_date_created_d date DEFAULT ('now'::text)::date,
    s_created_by_c character varying(20),
    s_date_modified_d date,
    s_modified_by_c character varying(20),
    s_modification_id_c character varying(150)
);


ALTER TABLE public.a_cost_centre OWNER TO wolfgangu;

--
-- TOC entry 2989 (class 0 OID 571807)
-- Dependencies: 2434
-- Data for Name: a_cost_centre; Type: TABLE DATA; Schema: public; Owner: wolfgangu
--

COPY a_cost_centre (a_ledger_number_i, a_cost_centre_code_c, a_cost_centre_to_report_to_c, a_cost_centre_name_c, a_posting_cost_centre_flag_l, a_cost_centre_active_flag_l, a_project_status_l, a_project_constraint_date_d, a_project_constraint_amount_n, a_system_cost_centre_flag_l, a_cost_centre_type_c, a_key_focus_area_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c) FROM stdin;
43	4301	4300S	Test Base 1	t	t	f	\N	0.0000000000	f	Local	\N	2011-03-31	DEMO	\N	\N
43	4302	4300S	Test Base 2	t	t	f	\N	0.0000000000	f	Local	\N	2011-03-31	DEMO	\N	\N
43	4303	4300S	Test Foreign 1	t	t	f	\N	0.0000000000	f	Foreign	\N	2011-03-31	DEMO	\N	\N
43	4304	4300S	Test Foreign 2	t	t	f	\N	0.0000000000	f	Foreign	\N	2011-03-31	DEMO	\N	\N
\.


--
-- TOC entry 2976 (class 2606 OID 571819)
-- Dependencies: 2434 2434 2434
-- Name: a_cost_centre_pk; Type: CONSTRAINT; Schema: public; Owner: wolfgangu; Tablespace: 
--

ALTER TABLE ONLY a_cost_centre
    ADD CONSTRAINT a_cost_centre_pk PRIMARY KEY (a_ledger_number_i, a_cost_centre_code_c);


--
-- TOC entry 2977 (class 1259 OID 581274)
-- Dependencies: 2434
-- Name: inx_a_cost_centre_fk1_key2; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_cost_centre_fk1_key2 ON a_cost_centre USING btree (a_ledger_number_i);


--
-- TOC entry 2978 (class 1259 OID 581275)
-- Dependencies: 2434 2434
-- Name: inx_a_cost_centre_fk2_key3; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_cost_centre_fk2_key3 ON a_cost_centre USING btree (a_ledger_number_i, a_cost_centre_type_c);


--
-- TOC entry 2979 (class 1259 OID 581276)
-- Dependencies: 2434
-- Name: inx_a_cost_centre_fk3_key4; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_cost_centre_fk3_key4 ON a_cost_centre USING btree (a_key_focus_area_c);


--
-- TOC entry 2980 (class 1259 OID 581277)
-- Dependencies: 2434
-- Name: inx_a_cost_centre_fkcr_key5; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_cost_centre_fkcr_key5 ON a_cost_centre USING btree (s_created_by_c);


--
-- TOC entry 2981 (class 1259 OID 581278)
-- Dependencies: 2434
-- Name: inx_a_cost_centre_fkmd_key6; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_cost_centre_fkmd_key6 ON a_cost_centre USING btree (s_modified_by_c);


--
-- TOC entry 2982 (class 1259 OID 581273)
-- Dependencies: 2434 2434
-- Name: inx_a_cost_centre_pk1; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE UNIQUE INDEX inx_a_cost_centre_pk1 ON a_cost_centre USING btree (a_ledger_number_i, a_cost_centre_code_c);


--
-- TOC entry 2983 (class 1259 OID 581272)
-- Dependencies: 2434 2434
-- Name: inx_a_ep_match_fk5_ref0; Type: INDEX; Schema: public; Owner: wolfgangu; Tablespace: 
--

CREATE INDEX inx_a_ep_match_fk5_ref0 ON a_cost_centre USING btree (a_ledger_number_i, a_cost_centre_code_c);


--
-- TOC entry 2988 (class 2606 OID 576133)
-- Dependencies: 2434 2424
-- Name: a_cost_centre_fk1; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_cost_centre
    ADD CONSTRAINT a_cost_centre_fk1 FOREIGN KEY (a_ledger_number_i) REFERENCES a_ledger(a_ledger_number_i);


--
-- TOC entry 2987 (class 2606 OID 576138)
-- Dependencies: 2433 2434 2433 2434
-- Name: a_cost_centre_fk2; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_cost_centre
    ADD CONSTRAINT a_cost_centre_fk2 FOREIGN KEY (a_ledger_number_i, a_cost_centre_type_c) REFERENCES a_cost_centre_types(a_ledger_number_i, a_cost_centre_type_c);


--
-- TOC entry 2986 (class 2606 OID 576143)
-- Dependencies: 2434 2617
-- Name: a_cost_centre_fk3; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_cost_centre
    ADD CONSTRAINT a_cost_centre_fk3 FOREIGN KEY (a_key_focus_area_c) REFERENCES a_key_focus_area(a_key_focus_area_c);


--
-- TOC entry 2985 (class 2606 OID 576148)
-- Dependencies: 2321 2434
-- Name: a_cost_centre_fkcr; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_cost_centre
    ADD CONSTRAINT a_cost_centre_fkcr FOREIGN KEY (s_created_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 2984 (class 2606 OID 576153)
-- Dependencies: 2434 2321
-- Name: a_cost_centre_fkmd; Type: FK CONSTRAINT; Schema: public; Owner: wolfgangu
--

ALTER TABLE ONLY a_cost_centre
    ADD CONSTRAINT a_cost_centre_fkmd FOREIGN KEY (s_modified_by_c) REFERENCES s_user(s_user_id_c);


--
-- TOC entry 2992 (class 0 OID 0)
-- Dependencies: 2434
-- Name: a_cost_centre; Type: ACL; Schema: public; Owner: wolfgangu
--

REVOKE ALL ON TABLE a_cost_centre FROM PUBLIC;
REVOKE ALL ON TABLE a_cost_centre FROM wolfgangu;
GRANT ALL ON TABLE a_cost_centre TO wolfgangu;
GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE a_cost_centre TO petraserver;


-- Completed on 2011-03-31 10:20:46

--
-- PostgreSQL database dump complete
--

