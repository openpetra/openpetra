--
-- PostgreSQL database dump
--
-- This files contain a data set with a damaged a_display_format_c entry
-- in order to test some routines ...

-- Dumped from database version 9.0.1
-- Dumped by pg_dump version 9.0.1
-- Started on 2011-03-24 11:22:41

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = off;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET escape_string_warning = off;

SET search_path = public, pg_catalog;

--
-- TOC entry 2979 (class 0 OID 308587)
-- Dependencies: 2320
-- Data for Name: a_currency; Type: TABLE DATA; Schema: public; Owner: wolfgangu
--

COPY a_currency (a_currency_code_c, a_currency_name_c, a_currency_symbol_c, p_country_code_c, a_display_format_c, a_in_emu_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c) FROM stdin;
DMG	Damaged Currency	DMG	DM	nonsense	f	\N	\N	\N	\N
\.


-- Completed on 2011-03-24 11:22:42

--
-- PostgreSQL database dump complete
--

