--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = off;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET escape_string_warning = off;

SET search_path = public, pg_catalog;

--
-- Name: s_login_s_login_process_id_r_seq; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('s_login_s_login_process_id_r_seq', 212, true);


--
-- Name: seq_application; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_application', 0, false);


--
-- Name: seq_ar_invoice; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_ar_invoice', 0, false);


--
-- Name: seq_bank_details; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_bank_details', 0, false);


--
-- Name: seq_booking; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_booking', 0, false);


--
-- Name: seq_budget; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_budget', 1, false);


--
-- Name: seq_contact; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_contact', 0, false);


--
-- Name: seq_data_label; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_data_label', 0, false);


--
-- Name: seq_document; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_document', 0, false);


--
-- Name: seq_extract_number; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_extract_number', 0, false);


--
-- Name: seq_file_info; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_file_info', 0, false);


--
-- Name: seq_form_letter_insert; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_form_letter_insert', 0, false);


--
-- Name: seq_foundation_proposal; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_foundation_proposal', 0, false);


--
-- Name: seq_general_ledger_master; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_general_ledger_master', 2, true);


--
-- Name: seq_job; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_job', 0, false);


--
-- Name: seq_job_assignment; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_job_assignment', 0, false);


--
-- Name: seq_location_number; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_location_number', 35, true);


--
-- Name: seq_match_number; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_match_number', 0, false);


--
-- Name: seq_modification1; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_modification1', 3049, true);


--
-- Name: seq_modification2; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_modification2', 0, true);


--
-- Name: seq_past_experience; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_past_experience', 0, false);


--
-- Name: seq_pe_evaluation_number; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_pe_evaluation_number', 0, false);


--
-- Name: seq_person_skill; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_person_skill', 0, false);


--
-- Name: seq_proposal_detail; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_proposal_detail', 0, false);


--
-- Name: seq_report_number; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_report_number', 0, false);


--
-- Name: seq_room_alloc; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_room_alloc', 0, false);


--
-- Name: seq_staff_data; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_staff_data', 0, false);


--
-- Name: seq_statement_number; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_statement_number', 5, true);


--
-- Name: seq_workflow; Type: SEQUENCE SET; Schema: public; Owner: Matthias Hobohm
--

SELECT pg_catalog.setval('seq_workflow', 0, false);


--
-- Data for Name: s_user; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_user (s_user_id_c, s_first_name_c, s_last_name_c, s_password_hash_c, s_password_salt_c, s_password_needs_change_l, s_failed_logins_i, s_retired_l, s_last_login_time_i, s_last_login_date_d, s_language_code_c, s_can_modify_l, s_record_delete_l, s_acquisition_code_c, a_default_ledger_number_i, s_failed_login_time_i, s_failed_login_date_d, p_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
SYSADMIN	\N	\N	E6DE03F83015BE85D6B65215205EDB9EAACF91FE	TAWlTUZY/R0ZfsBi0/oE/ytWSWepsG3oNtFLlopSbeE=	t	0	f	0	\N	99	t	f	\N	0	0	\N	\N	2011-01-11	\N	\N	\N	\N
DEMO	\N	\N	AD4511A8ECED944E690A5623B3D38B8AC0A247F7	fh6ZXHnhBkOFRUZ2MLSSUwE3+Vi6vih7WdycPFk8D0Q=	t	0	f	0	\N	99	t	f	\N	0	0	\N	\N	2011-01-11	\N	\N	\N	\N
\.


--
-- Data for Name: s_file; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_file (s_file_name_c, s_file_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
	\N	2011-01-11	\N	\N	\N	\N
\.


--
-- Data for Name: a_budget_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_budget_type (a_budget_type_code_c, a_budget_type_description_c, a_budget_process_to_call_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
Adhoc	Adhoc		\N	\N	\N	\N	\N
Inf. n	Inflate after n periods		\N	\N	\N	\N	\N
Inf.Base	Inflate off base		\N	\N	\N	\N	\N
Same	Same each period		\N	\N	\N	\N	\N
Split	Split total		\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_international_postal_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_international_postal_type (p_internat_postal_type_code_c, p_description_c, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
#LOCAL	Local	f	\N	\N	\N	\N	\N
88	Unallocated	f	\N	\N	\N	\N	\N
99	Not known	f	\N	\N	\N	\N	\N
AFR	Africa	f	\N	\N	\N	\N	\N
AUS	Australasia & Pacific Islands	f	\N	\N	\N	\N	\N
EA	East Asia	f	\N	\N	\N	\N	\N
EC	European Union	f	\N	\N	\N	\N	\N
EUR	Europe (non European Union)	f	\N	\N	\N	\N	\N
LAM	Latin America	f	\N	\N	\N	\N	\N
ME	Middle East	f	\N	\N	\N	\N	\N
NAMC	North America & Caribbean	f	\N	\N	\N	\N	\N
SWACA	South, West and Central Asia	f	\N	\N	\N	\N	\N
Z1	Zone 1	f	\N	\N	\N	\N	\N
Z2	Zone 2	f	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_country; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_country (p_country_code_c, p_country_name_c, p_nationality_name_c, p_undercover_l, p_internat_telephone_code_i, p_internat_postal_type_code_c, p_internat_access_code_c, p_time_zone_minimum_n, p_time_zone_maximum_n, p_deletable_l, p_address_order_i, p_country_name_local_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
99	BAD COUNTRY CODE	BAD COUNTRY CODE	\N	0	\N	\N	0.00	0.00	t	0	BAD COUNTRY CODE	\N	\N	\N	\N	\N
AD	ANDORRA	ANDORRA	\N	376	EUR	00	1.00	1.00	f	1	ANDORRA	\N	\N	\N	\N	\N
AE	UNITED ARAB EMIRATES	UNITED ARAB EMIRATES	\N	971	ME	00	4.00	4.00	f	0	UNITED ARAB EMIRATES	\N	\N	\N	\N	\N
AF	AFGHANISTAN	AFGHANISTAN	\N	93	SWACA	00	4.50	4.50	f	0	AFGHANISTAN	\N	\N	\N	\N	\N
AG	ANTIGUA AND BARBUDA	ANTIGUA AND BARBUDA	\N	1268	NAMC	011	-4.00	-4.00	f	0	ANTIGUA AND BARBUDA	\N	\N	\N	\N	\N
AI	ANGUILLA	ANGUILLA	\N	1264	NAMC	011	-4.00	-4.00	f	0	ANGUILLA	\N	\N	\N	\N	\N
AL	ALBANIA	ALBANIA	\N	355	EUR	00	1.00	1.00	f	1	ALBANIA	\N	\N	\N	\N	\N
AM	ARMENIA	ARMENIA	\N	374	EUR	00	4.00	4.00	f	0	ARMENIA	\N	\N	\N	\N	\N
AN	NETHERLANDS ANTILLES	NETHERLANDS ANTILLES	\N	599	EA	00	-4.00	-4.00	f	0	NETHERLANDS ANTILLES	\N	\N	\N	\N	\N
AO	ANGOLA	ANGOLA	\N	244	AFR	00	1.00	1.00	f	0	ANGOLA	\N	\N	\N	\N	\N
AQ	ANTARCTICA	ANTARCTICA	\N	672	88	XX	4.50	10.00	f	0	ANTARCTICA	\N	\N	\N	\N	\N
AR	ARGENTINA	ARGENTINA	\N	54	LAM	00	-3.00	-3.00	f	0	ARGENTINA	\N	\N	\N	\N	\N
AS	AMERICAN SAMOA	AMERICAN SAMOA	\N	684	AUS	00	-11.00	-11.00	f	0	AMERICAN SAMOA	\N	\N	\N	\N	\N
AT	AUSTRIA	AUSTRIA	\N	43	EC	00	1.00	1.00	f	1	AUSTRIA	\N	\N	\N	\N	\N
AU	AUSTRALIA	AUSTRALIA	\N	61	AUS	00	8.00	10.00	f	0	AUSTRALIA	\N	\N	\N	\N	\N
AW	ARUBA	ARUBA	\N	297	NAMC	00	-4.00	-4.00	f	0	ARUBA	\N	\N	\N	\N	\N
AZ	AZERBAIJAN	AZERBAIJAN	\N	994	SWACA	8~10	5.00	5.00	f	0	AZERBAIJAN	\N	\N	\N	\N	\N
BA	BOSNIA-HERCEGOVINA	BOSNIA-HERCEGOVINA	\N	387	EUR	00	1.00	1.00	f	0	BOSNIA-HERCEGOVINA	\N	\N	\N	\N	\N
BB	BARBADOS	BARBADOS	\N	1246	NAMC	011	-4.00	-4.00	f	0	BARBADOS	\N	\N	\N	\N	\N
BD	BANGLADESH	BANGLADESH	\N	880	SWACA	00	6.00	6.00	f	0	BANGLADESH	\N	\N	\N	\N	\N
BE	BELGIUM	BELGIUM	\N	32	EC	00	1.00	1.00	f	1	BELGIUM	\N	\N	\N	\N	\N
BF	BURKINA FASO	BURKINA FASO	\N	226	AFR	00	0.00	0.00	f	0	BURKINA FASO	\N	\N	\N	\N	\N
BG	BULGARIA	BULGARIA	\N	359	EUR	00	2.00	2.00	f	1	BULGARIA	\N	\N	\N	\N	\N
BH	BAHRAIN	BAHRAIN	\N	973	ME	0	3.00	3.00	f	0	BAHRAIN	\N	\N	\N	\N	\N
BI	BURUNDI	BURUNDI	\N	257	AFR	90	2.00	2.00	f	0	BURUNDI	\N	\N	\N	\N	\N
BJ	BENIN	BENIN	\N	229	AFR	00	1.00	1.00	f	0	BENIN	\N	\N	\N	\N	\N
BM	BERMUDA	BERMUDA	\N	1441	NAMC	011	-4.00	-4.00	f	0	BERMUDA	\N	\N	\N	\N	\N
BN	BRUNEI DARUSSALAM	BRUNEI DARUSSALAM	\N	673	EA	00	8.00	8.00	f	0	BRUNEI DARUSSALAM	\N	\N	\N	\N	\N
BO	BOLIVIA	BOLIVIA	\N	591	LAM	00	-4.00	-4.00	f	0	BOLIVIA	\N	\N	\N	\N	\N
BR	BRAZIL	BRAZIL	\N	55	LAM	00	-3.00	-3.00	f	0	BRAZIL	\N	\N	\N	\N	\N
BS	BAHAMAS	BAHAMAS	\N	1242	NAMC	011	-5.00	-5.00	f	0	BAHAMAS	\N	\N	\N	\N	\N
BT	BHUTAN	BHUTAN	\N	975	SWACA	00	6.00	6.00	f	0	BHUTAN	\N	\N	\N	\N	\N
BW	BOTSWANA	BOTSWANA	\N	267	AFR	00	2.00	2.00	f	0	BOTSWANA	\N	\N	\N	\N	\N
BY	BELARUS	BELARUS	\N	375	EUR	8~10	2.00	2.00	f	0	BELARUS	\N	\N	\N	\N	\N
BZ	BELIZE	BELIZE	\N	501	LAM	00	-6.00	-6.00	f	0	BELIZE	\N	\N	\N	\N	\N
CA	CANADA	CANADA	\N	1	NAMC	011	-9.00	-3.00	f	0	CANADA	\N	\N	\N	\N	\N
CD	CONGO, THE DEMOCRATIC REPUBLIC OF THE	CONGO, THE DEMOCRATIC REPUBLIC OF THE	\N	243	AFR	00	1.00	1.00	f	0	CONGO, THE DEMOCRATIC REPUBLIC OF THE	\N	\N	\N	\N	\N
CF	CENTRAL AFRICAN REPUBLIC	CENTRAL AFRICAN REPUBLIC	\N	236	AFR	19	1.00	1.00	f	0	CENTRAL AFRICAN REPUBLIC	\N	\N	\N	\N	\N
CG	CONGO	CONGO	\N	242	AFR	00	1.00	1.00	f	0	CONGO	\N	\N	\N	\N	\N
CH	SWITZERLAND	SWITZERLAND	\N	41	EUR	00	1.00	1.00	f	1	SWITZERLAND	\N	\N	\N	\N	\N
CI	COTE D'IVOIRE	COTE D'IVOIRE	\N	225	AFR	00	0.00	0.00	f	0	COTE D'IVOIRE	\N	\N	\N	\N	\N
CK	COOK ISLANDS	COOK ISLANDS	\N	682	AUS	00	-10.00	-10.00	f	0	COOK ISLANDS	\N	\N	\N	\N	\N
CL	CHILE	CHILE	\N	56	LAM	00	-4.00	-4.00	f	0	CHILE	\N	\N	\N	\N	\N
CM	CAMEROON	CAMEROON	\N	237	AFR	00	1.00	1.00	f	0	CAMEROON	\N	\N	\N	\N	\N
CN	CHINA	CHINA	\N	86	EA	00	8.00	8.00	f	0	CHINA	\N	\N	\N	\N	\N
CO	COLOMBIA	COLOMBIA	\N	57	LAM	90	-5.00	-5.00	f	0	COLOMBIA	\N	\N	\N	\N	\N
CR	COSTA RICA	COSTA RICA	\N	506	NAMC	00	-6.00	-6.00	f	0	COSTA RICA	\N	\N	\N	\N	\N
CU	CUBA	CUBA	\N	53	NAMC	119	-5.00	-5.00	f	0	CUBA	\N	\N	\N	\N	\N
CV	CAPE VERDE	CAPE VERDE	\N	238	EUR	0	-1.00	-1.00	f	1	CAPE VERDE	\N	\N	\N	\N	\N
CY	CYPRUS	CYPRUS	\N	357	EC	00	2.00	2.00	f	1	CYPRUS	\N	\N	\N	\N	\N
CZ	CZECH REPUBLIC	CZECH REPUBLIC	\N	420	EUR	00	1.00	1.00	f	1	CZECH REPUBLIC	\N	\N	\N	\N	\N
DE	GERMANY	GERMANY	\N	49	EC	00	1.00	1.00	f	1	GERMANY	\N	\N	\N	\N	\N
DJ	DJIBOUTI	DJIBOUTI	\N	253	AFR	00	3.00	3.00	f	0	DJIBOUTI	\N	\N	\N	\N	\N
DK	DENMARK	DENMARK	\N	45	EC	00	1.00	1.00	f	1	DENMARK	\N	\N	\N	\N	\N
DM	DOMINICA	DOMINICA	\N	1767	NAMC	011	-4.00	-4.00	f	0	DOMINICA	\N	\N	\N	\N	\N
DO	DOMINICAN REPUBLIC	DOMINICAN REPUBLIC	\N	1809	NAMC	011	-4.00	-4.00	f	0	DOMINICAN REPUBLIC	\N	\N	\N	\N	\N
DU	DUBAI	DUBAI	\N	0	ME	00	0.00	0.00	f	0	DUBAI	\N	\N	\N	\N	\N
DZ	ALGERIA	ALGERIA	\N	213	AFR	00~	1.00	1.00	f	0	ALGERIA	\N	\N	\N	\N	\N
EC	ECUADOR	ECUADOR	\N	593	LAM	00	-5.00	-5.00	f	0	ECUADOR	\N	\N	\N	\N	\N
EE	ESTONIA	ESTONIA	\N	372	EUR	8~00	2.00	2.00	f	0	ESTONIA	\N	\N	\N	\N	\N
EG	EGYPT	EGYPT	\N	20	AFR	00	2.00	2.00	f	0	EGYPT	\N	\N	\N	\N	\N
ER	ERITREA	ERITREA	\N	291	AFR	00	3.00	3.00	f	0	ERITREA	\N	\N	\N	\N	\N
ES	SPAIN	SPAIN	\N	34	EC	00	1.00	1.00	f	1	SPAIN	\N	\N	\N	\N	\N
ET	ETHIOPIA	ETHIOPIA	\N	251	AFR	00	3.00	3.00	f	0	ETHIOPIA	\N	\N	\N	\N	\N
FI	FINLAND	FINLAND	\N	358	EC	00	2.00	2.00	f	1	FINLAND	\N	\N	\N	\N	\N
FJ	FIJI	FIJI	\N	679	AUS	05	12.00	12.00	f	0	FIJI	\N	\N	\N	\N	\N
FK	FALKLAND ISLANDS (MALVINAS)	FALKLAND ISLANDS (MALVINAS)	\N	500	LAM	0	-4.00	-4.00	f	0	FALKLAND ISLANDS (MALVINAS)	\N	\N	\N	\N	\N
FM	MICRONESIA (FEDERATE STATES OF)	MICRONESIA (FEDERATE STATES OF)	\N	691	AUS	011	10.00	11.00	f	0	MICRONESIA (FEDERATE STATES OF)	\N	\N	\N	\N	\N
FO	FAROE ISLANDS	FAROE ISLANDS	\N	298	EC	009	0.00	0.00	f	1	FAROE ISLANDS	\N	\N	\N	\N	\N
FR	FRANCE	FRANCE	\N	33	EC	00	1.00	1.00	f	1	FRANCE	\N	\N	\N	\N	\N
GA	GABON	GABON	\N	241	AFR	00	1.00	1.00	f	0	GABON	\N	\N	\N	\N	\N
GB	UNITED KINGDOM	UNITED KINGDOM	\N	44	EC	00	0.00	0.00	f	0	UNITED KINGDOM	\N	\N	\N	\N	\N
GD	GRENADA	GRENADA	\N	1473	NAMC	011	-4.00	-4.00	f	0	GRENADA	\N	\N	\N	\N	\N
GE	GEORGIA	GEORGIA	\N	995	EUR	8~10	4.00	4.00	f	0	GEORGIA	\N	\N	\N	\N	\N
GF	FRENCH GUIANA	FRENCH GUIANA	\N	594	LAM	00	-3.00	-3.00	f	0	FRENCH GUIANA	\N	\N	\N	\N	\N
GH	GHANA	GHANA	\N	233	AFR	00	0.00	0.00	f	0	GHANA	\N	\N	\N	\N	\N
GI	GIBRALTAR	GIBRALTAR	\N	350	EUR	00	1.00	1.00	f	0	GIBRALTAR	\N	\N	\N	\N	\N
GL	GREENLAND	GREENLAND	\N	299	EUR	009	-4.00	-1.00	f	0	GREENLAND	\N	\N	\N	\N	\N
GM	GAMBIA	GAMBIA	\N	220	AFR	00	0.00	0.00	f	0	GAMBIA	\N	\N	\N	\N	\N
GN	GUINEA	GUINEA	\N	224	LAM	00	0.00	0.00	f	0	GUINEA	\N	\N	\N	\N	\N
GP	GUADELOUPE	GUADELOUPE	\N	590	NAMC	00	-4.00	-4.00	f	0	GUADELOUPE	\N	\N	\N	\N	\N
GQ	EQUATORIAL GUINEA	EQUATORIAL GUINEA	\N	240	LAM	00	1.00	1.00	f	0	EQUATORIAL GUINEA	\N	\N	\N	\N	\N
GR	GREECE	GREECE	\N	30	EC	00	2.00	2.00	f	1	GREECE	\N	\N	\N	\N	\N
GT	GUATEMALA	GUATEMALA	\N	502	LAM	00	-6.00	-6.00	f	0	GUATEMALA	\N	\N	\N	\N	\N
GW	GUINEA-BISSAU	GUINEA-BISSAU	\N	245	AFR	00	0.00	0.00	f	0	GUINEA-BISSAU	\N	\N	\N	\N	\N
GY	GUYANA	GUYANA	\N	592	NAMC	001	-4.00	-4.00	f	0	GUYANA	\N	\N	\N	\N	\N
GZ	GUERNSEY (GB)	GUERNSEY (GB)	\N	44	EC	00	0.00	0.00	f	0	GUERNSEY (GB)	\N	\N	\N	\N	\N
HK	HONG KONG	HONG KONG	\N	852	EA	001	8.00	8.00	f	0	HONG KONG	\N	\N	\N	\N	\N
HN	HONDURAS	HONDURAS	\N	504	LAM	00	-6.00	-6.00	f	0	HONDURAS	\N	\N	\N	\N	\N
HR	CROATIA	CROATIA	\N	385	EUR	00	1.00	1.00	f	1	Hrvatska	\N	\N	\N	\N	\N
HT	HAITI	HAITI	\N	509	NAMC	00	-5.00	-5.00	f	0	HAITI	\N	\N	\N	\N	\N
HU	HUNGARY	HUNGARY	\N	36	EUR	00	1.00	1.00	f	1	HUNGARY	\N	\N	\N	\N	\N
ID	INDONESIA	INDONESIA	\N	62	EA	00	7.00	8.00	f	0	INDONESIA	\N	\N	\N	\N	\N
IE	IRELAND	IRELAND	\N	353	EC	00	0.00	0.00	f	0	IRELAND	\N	\N	\N	\N	\N
IL	ISRAEL	ISRAEL	\N	972	ME	00	2.00	2.00	f	0	ISRAEL	\N	\N	\N	\N	\N
IN	INDIA	INDIA	\N	91	SWACA	00	5.50	5.50	f	0	INDIA	\N	\N	\N	\N	\N
IQ	IRAQ	IRAQ	\N	964	ME	00	3.00	3.00	f	0	IRAQ	\N	\N	\N	\N	\N
IR	IRAN (ISLAMIC REPUBLIC OF)	IRAN (ISLAMIC REPUBLIC OF)	\N	98	ME	00	3.50	3.50	f	0	IRAN (ISLAMIC REPUBLIC OF)	\N	\N	\N	\N	\N
IS	ICELAND	ICELAND	\N	354	EUR	00	0.00	0.00	f	1	ICELAND	\N	\N	\N	\N	\N
IT	ITALY	ITALY	\N	39	EC	00	1.00	1.00	f	1	ITALY	\N	\N	\N	\N	\N
JM	JAMAICA	JAMAICA	\N	1876	NAMC	011	-5.00	-5.00	f	0	JAMAICA	\N	\N	\N	\N	\N
JO	JORDAN	JORDAN	\N	962	ME	00	2.00	2.00	f	0	JORDAN	\N	\N	\N	\N	\N
JP	JAPAN	JAPAN	\N	81	EA	001	9.00	9.00	f	0	JAPAN	\N	\N	\N	\N	\N
JY	JERSEY (GB)	JERSEY (GB)	\N	44	EC	00	0.00	0.00	f	0	JERSEY (GB)	\N	\N	\N	\N	\N
KE	KENYA	KENYA	\N	254	AFR	000	3.00	3.00	f	0	KENYA	\N	\N	\N	\N	\N
KG	KYRGYZSTAN	KYRGYZSTAN	\N	996	SWACA	8~10	6.00	6.00	f	0	KYRGYZSTAN	\N	\N	\N	\N	\N
KH	CAMBODIA	CAMBODIA	\N	855	EA	00	7.00	7.00	f	0	CAMBODIA	\N	\N	\N	\N	\N
KI	KIRIBATI	KIRIBATI	\N	686	AUS	00	12.00	12.00	f	0	KIRIBATI	\N	\N	\N	\N	\N
KM	COMOROS	COMOROS	\N	269	EA	10	3.00	3.00	f	0	COMOROS	\N	\N	\N	\N	\N
KN	SAINT KITTS AND NEVIS	SAINT KITTS AND NEVIS	\N	1869	NAMC	011	-4.00	-4.00	f	0	SAINT KITTS AND NEVIS	\N	\N	\N	\N	\N
KP	KOREA, NORTH (PEOPLE'S REPUBLIC)	KOREA, NORTH (PEOPLE'S REPUBLIC)	\N	850	EA	00	9.00	9.00	f	0	KOREA, NORTH (PEOPLE'S REPUBLIC)	\N	\N	\N	\N	\N
KR	KOREA, SOUTH (REPUBLIC OF)	KOREA, SOUTH (REPUBLIC OF)	\N	82	EA	001	9.00	9.00	f	0	KOREA, SOUTH (REPUBLIC OF)	\N	\N	\N	\N	\N
KW	KUWAIT	KUWAIT	\N	965	ME	00	3.00	3.00	f	0	KUWAIT	\N	\N	\N	\N	\N
KY	CAYMAN ISLANDS	CAYMAN ISLANDS	\N	1345	NAMC	011	-5.00	-5.00	f	0	CAYMAN ISLANDS	\N	\N	\N	\N	\N
KZ	KAZAKHSTAN	KAZAKHSTAN	\N	7	SWACA	8~10	5.00	6.00	f	0	KAZAKHSTAN	\N	\N	\N	\N	\N
LA	LAO PEOPLE'S DEMOCRATIC REPUBLIC	LAO PEOPLE'S DEMOCRATIC REPUBLIC	\N	856	EA	14	7.00	7.00	f	0	LAO PEOPLE'S DEMOCRATIC REPUBLIC	\N	\N	\N	\N	\N
LB	LEBANON	LEBANON	\N	961	ME	00	2.00	2.00	f	0	LEBANON	\N	\N	\N	\N	\N
LC	SAINT LUCIA	SAINT LUCIA	\N	1758	NAMC	011	-4.00	-4.00	f	0	SAINT LUCIA	\N	\N	\N	\N	\N
LI	LIECHTENSTEIN	LIECHTENSTEIN	\N	423	EUR	00	1.00	1.00	f	1	LIECHTENSTEIN	\N	\N	\N	\N	\N
LK	SRI LANKA	SRI LANKA	\N	94	SWACA	00	6.00	6.00	f	0	SRI LANKA	\N	\N	\N	\N	\N
LR	LIBERIA	LIBERIA	\N	231	AFR	00	0.00	0.00	f	0	LIBERIA	\N	\N	\N	\N	\N
LS	LESOTHO	LESOTHO	\N	266	AFR	00	2.00	2.00	f	0	LESOTHO	\N	\N	\N	\N	\N
LT	LITHUANIA	LITHUANIA	\N	370	EUR	8~10	2.00	2.00	f	0	LITHUANIA	\N	\N	\N	\N	\N
LU	LUXEMBOURG	LUXEMBOURG	\N	352	EC	00	1.00	1.00	f	1	LUXEMBOURG	\N	\N	\N	\N	\N
LV	LATVIA	LATVIA	\N	371	EUR	00	2.00	2.00	f	0	LATVIA	\N	\N	\N	\N	\N
LY	LIBYAN ARAB JAMAHIRIYA	LIBYAN ARAB JAMAHIRIYA	\N	218	AFR	00	0.00	0.00	f	0	LIBYAN ARAB JAMAHIRIYA	\N	\N	\N	\N	\N
MA	MOROCCO	MOROCCO	\N	212	AFR	00~	0.00	0.00	f	0	MOROCCO	\N	\N	\N	\N	\N
MC	MONACO	MONACO	\N	377	EUR	00	1.00	1.00	f	1	MONACO	\N	\N	\N	\N	\N
MD	MOLDOVA (REPUBLIC OF)	MOLDOVA (REPUBLIC OF)	\N	373	EUR	8~10	2.00	2.00	f	0	MOLDOVA (REPUBLIC OF)	\N	\N	\N	\N	\N
ME	MONTENEGRO (REPUBLIC OF)	MONTENEGRO (REPUBLIC OF)	\N	382	EUR	00	2.00	2.00	f	1	MONTENEGRO (REPUBLIC OF)	\N	\N	\N	\N	\N
MG	MADAGASCAR	MADAGASCAR	\N	261	AFR	00	3.00	3.00	f	0	MADAGASCAR	\N	\N	\N	\N	\N
MH	MARSHALL ISLANDS	MARSHALL ISLANDS	\N	692	AUS	011	12.00	12.00	f	0	MARSHALL ISLANDS	\N	\N	\N	\N	\N
MK	MACEDONIA, FORMER YUGOSLAVIA	MACEDONIA, FORMER YUGOSLAVIA	\N	389	EUR	00	1.00	1.00	f	1	MACEDONIA, FORMER YUGOSLAVIA	\N	\N	\N	\N	\N
ML	MALI	MALI	\N	223	SWACA	00	0.00	0.00	f	0	MALI	\N	\N	\N	\N	\N
MM	MYANMAR (BURMA)	MYANMAR (BURMA)	\N	95	EA	0	6.50	6.50	f	0	MYANMAR (BURMA)	\N	\N	\N	\N	\N
MN	MONGOLIA	MONGOLIA	\N	976	EA	00	8.00	8.00	f	0	MONGOLIA	\N	\N	\N	\N	\N
MO	MACAU	MACAU	\N	853	EA	00	8.00	8.00	f	0	MACAU	\N	\N	\N	\N	\N
MP	NORTHERN MARIANA ISLANDS	NORTHERN MARIANA ISLANDS	\N	1670	EA	011	10.00	10.00	f	0	NORTHERN MARIANA ISLANDS	\N	\N	\N	\N	\N
MQ	MARTINIQUE	MARTINIQUE	\N	963	NAMC	00	-4.00	-4.00	f	0	MARTINIQUE	\N	\N	\N	\N	\N
MR	MAURITANIA	MAURITANIA	\N	222	AFR	00	0.00	0.00	f	0	MAURITANIA	\N	\N	\N	\N	\N
MS	MONTSERRAT	MONTSERRAT	\N	1664	NAMC	011	-4.00	-4.00	f	0	MONTSERRAT	\N	\N	\N	\N	\N
MT	MALTA	MALTA	\N	356	EUR	00	1.00	1.00	f	1	MALTA	\N	\N	\N	\N	\N
MU	MAURITIUS	MAURITIUS	\N	230	AFR	00	4.00	4.00	f	0	MAURITIUS	\N	\N	\N	\N	\N
MV	MALDIVES	MALDIVES	\N	960	EA	00	5.00	5.00	f	0	MALDIVES	\N	\N	\N	\N	\N
MW	MALAWI	MALAWI	\N	265	AFR	101	2.00	2.00	f	0	MALAWI	\N	\N	\N	\N	\N
MX	MEXICO	MEXICO	\N	52	LAM	00	-7.00	-6.00	f	0	MEXICO	\N	\N	\N	\N	\N
MY	MALAYSIA	MALAYSIA	\N	60	EA	00	8.00	8.00	f	0	MALAYSIA	\N	\N	\N	\N	\N
MZ	MOZAMBIQUE	MOZAMBIQUE	\N	258	AFR	00	2.00	2.00	f	0	MOZAMBIQUE	\N	\N	\N	\N	\N
NA	NAMIBIA	NAMIBIA	\N	264	AFR	09	2.00	2.00	f	0	NAMIBIA	\N	\N	\N	\N	\N
NC	NEW CALEDONIA	NEW CALEDONIA	\N	687	AUS	00	11.00	11.00	f	0	NEW CALEDONIA	\N	\N	\N	\N	\N
NE	NIGER	NIGER	\N	227	AFR	00	1.00	1.00	f	0	NIGER	\N	\N	\N	\N	\N
NF	NORFOLK ISLAND	NORFOLK ISLAND	\N	672	AUS	00	11.50	11.50	f	0	NORFOLK ISLAND	\N	\N	\N	\N	\N
NG	NIGERIA	NIGERIA	\N	234	AFR	009	1.00	1.00	f	0	NIGERIA	\N	\N	\N	\N	\N
NI	NICARAGUA	NICARAGUA	\N	505	LAM	00	-6.00	-6.00	f	0	NICARAGUA	\N	\N	\N	\N	\N
NL	NETHERLANDS	NETHERLANDS	\N	31	EC	00	1.00	1.00	f	1	NETHERLANDS	\N	\N	\N	\N	\N
NO	NORWAY	NORWAY	\N	47	EUR	00	1.00	1.00	f	1	NORWAY	\N	\N	\N	\N	\N
NP	NEPAL	NEPAL	\N	977	SWACA	00	5.75	5.75	f	0	NEPAL	\N	\N	\N	\N	\N
NR	NAURU	NAURU	\N	674	EA	00	12.00	12.00	f	0	NAURU	\N	\N	\N	\N	\N
NU	NIUE	NIUE	\N	683	AUS	00	-11.00	-11.00	f	0	NIUE	\N	\N	\N	\N	\N
NZ	NEW ZEALAND	NEW ZEALAND	\N	64	AUS	00	12.00	12.00	f	0	NEW ZEALAND	\N	\N	\N	\N	\N
OM	OMAN	OMAN	\N	968	ME	00	4.00	4.00	f	0	OMAN	\N	\N	\N	\N	\N
PA	PANAMA	PANAMA	\N	507	LAM	0	-5.00	-5.00	f	0	PANAMA	\N	\N	\N	\N	\N
PE	PERU	PERU	\N	51	LAM	00	-5.00	-5.00	f	0	PERU	\N	\N	\N	\N	\N
PF	FRENCH POLYNESIA	FRENCH POLYNESIA	\N	689	AUS	00	-10.00	-10.00	f	0	FRENCH POLYNESIA	\N	\N	\N	\N	\N
PG	PAPUA NEW GUINEA	PAPUA NEW GUINEA	\N	675	EA	05	10.00	10.00	f	0	PAPUA NEW GUINEA	\N	\N	\N	\N	\N
PH	PHILIPPINES	PHILIPPINES	\N	63	EA	00	8.00	8.00	f	0	PHILIPPINES	\N	\N	\N	\N	\N
PK	PAKISTAN	PAKISTAN	\N	92	SWACA	00	5.00	5.00	f	0	PAKISTAN	\N	\N	\N	\N	\N
PL	POLAND	POLAND	\N	48	EUR	0~0	1.00	1.00	f	1	POLAND	\N	\N	\N	\N	\N
PR	PUERTO RICO	PUERTO RICO	\N	1787	NAMC	1	-4.00	-4.00	f	0	PUERTO RICO	\N	\N	\N	\N	\N
PT	PORTUGAL	PORTUGAL	\N	351	EC	00	0.00	0.00	f	1	PORTUGAL	\N	\N	\N	\N	\N
PW	PALAU	PALAU	\N	680	AUS	011	9.00	9.00	f	0	PALAU	\N	\N	\N	\N	\N
PY	PARAGUAY	PARAGUAY	\N	595	LAM	00	-4.00	-4.00	f	0	PARAGUAY	\N	\N	\N	\N	\N
QA	QATAR	QATAR	\N	974	ME	0	3.00	3.00	f	0	QATAR	\N	\N	\N	\N	\N
RE	REUNION	REUNION	\N	262	LAM	00	4.00	4.00	f	0	REUNION	\N	\N	\N	\N	\N
RO	ROMANIA	ROMANIA	\N	40	EUR	00	2.00	2.00	f	1	ROMANIA	\N	\N	\N	\N	\N
RS	SERBIA (REPUBLIC OF)	SERBIA (REPUBLIC OF)	\N	381	EUR	00	2.00	2.00	f	1	SERBIA (REPUBLIC OF)	\N	\N	\N	\N	\N
RU	RUSSIAN FEDERATION	RUSSIAN FEDERATION	\N	7	EUR	8~10	2.00	12.00	f	0	RUSSIAN FEDERATION	\N	\N	\N	\N	\N
RW	RWANDA	RWANDA	\N	250	AFR	00	2.00	2.00	f	0	RWANDA	\N	\N	\N	\N	\N
SA	SAUDI ARABIA	SAUDI ARABIA	\N	966	ME	00	3.00	3.00	f	0	SAUDI ARABIA	\N	\N	\N	\N	\N
SB	SOLOMON ISLANDS	SOLOMON ISLANDS	\N	677	AUS	00	11.00	11.00	f	0	SOLOMON ISLANDS	\N	\N	\N	\N	\N
SC	SEYCHELLES	SEYCHELLES	\N	248	EA	00	4.00	4.00	f	0	SEYCHELLES	\N	\N	\N	\N	\N
SD	SUDAN	SUDAN	\N	249	AFR	00	2.00	2.00	f	0	SUDAN	\N	\N	\N	\N	\N
SE	SWEDEN	SWEDEN	\N	46	EC	00	1.00	1.00	f	1	SWEDEN	\N	\N	\N	\N	\N
SG	SINGAPORE	SINGAPORE	\N	65	EA	001	8.00	8.00	f	0	SINGAPORE	\N	\N	\N	\N	\N
SI	SLOVENIA	SLOVENIA	\N	386	EUR	00	1.00	1.00	f	1	SLOVENIA	\N	\N	\N	\N	\N
SK	SLOVAKIA (SLOVAK REPUBLIC)	SLOVAKIA (SLOVAK REPUBLIC)	\N	421	EUR	00	1.00	1.00	f	1	SLOVAKIA (SLOVAK REPUBLIC)	\N	\N	\N	\N	\N
SL	SIERRA LEONE	SIERRA LEONE	\N	232	AFR	00	0.00	0.00	f	0	SIERRA LEONE	\N	\N	\N	\N	\N
SM	SAN MARINO	SAN MARINO	\N	378	EUR	00	1.00	1.00	f	1	SAN MARINO	\N	\N	\N	\N	\N
SN	SENEGAL	SENEGAL	\N	221	AFR	00	0.00	0.00	f	0	SENEGAL	\N	\N	\N	\N	\N
SO	SOMALIA	SOMALIA	\N	252	AFR	19	3.00	3.00	f	0	SOMALIA	\N	\N	\N	\N	\N
SR	SURINAM(E)	SURINAM(E)	\N	597	EA	00	-3.00	-3.00	f	0	SURINAM(E)	\N	\N	\N	\N	\N
ST	SAO TOME AND PRINCIPE	SAO TOME AND PRINCIPE	\N	239	AUS	00	0.00	0.00	f	0	SAO TOME AND PRINCIPE	\N	\N	\N	\N	\N
SV	EL SALVADOR	EL SALVADOR	\N	503	LAM	0	-6.00	-6.00	f	0	EL SALVADOR	\N	\N	\N	\N	\N
SY	SYRIAN ARAB REPUBLIC	SYRIAN ARAB REPUBLIC	\N	963	ME	XX	2.00	2.00	f	0	SYRIAN ARAB REPUBLIC	\N	\N	\N	\N	\N
SZ	SWAZILAND	SWAZILAND	\N	268	AFR	00	2.00	2.00	f	0	SWAZILAND	\N	\N	\N	\N	\N
TC	TURKS AND CAICOS ISLANDS	TURKS AND CAICOS ISLANDS	\N	1649	NAMC	011	-5.00	-5.00	f	0	TURKS AND CAICOS ISLANDS	\N	\N	\N	\N	\N
TD	CHAD	CHAD	\N	235	AFR	15	1.00	1.00	f	0	CHAD	\N	\N	\N	\N	\N
TG	TOGO	TOGO	\N	228	AUS	00	0.00	0.00	f	0	TOGO	\N	\N	\N	\N	\N
TH	THAILAND	THAILAND	\N	66	EA	001	7.00	7.00	f	0	THAILAND	\N	\N	\N	\N	\N
TJ	TAJIKSTAN	TAJIKSTAN	\N	992	SWACA	8~10	5.00	5.00	f	0	TAJIKSTAN	\N	\N	\N	\N	\N
TM	TURKMENISTAN	TURKMENISTAN	\N	993	SWACA	8~10	5.00	5.00	f	0	TURKMENISTAN	\N	\N	\N	\N	\N
TN	TUNISIA	TUNISIA	\N	216	AFR	00	1.00	1.00	f	0	TUNISIA	\N	\N	\N	\N	\N
TO	TONGA	TONGA	\N	676	AUS	00	13.00	13.00	f	0	TONGA	\N	\N	\N	\N	\N
TR	TURKEY	TURKEY	\N	90	EUR	00	2.00	2.00	f	1	TURKEY	\N	\N	\N	\N	\N
TT	TRINIDAD AND TOBAGO	TRINIDAD AND TOBAGO	\N	1868	NAMC	011	-4.00	-4.00	f	0	TRINIDAD AND TOBAGO	\N	\N	\N	\N	\N
TV	TUVALU	TUVALU	\N	688	AUS	00	12.00	12.00	f	0	TUVALU	\N	\N	\N	\N	\N
TW	TAIWAN	TAIWAN	\N	886	EA	002	8.00	8.00	f	0	TAIWAN	\N	\N	\N	\N	\N
TZ	TANZANIA (UNITED REPUBLIC OF)	TANZANIA (UNITED REPUBLIC OF)	\N	255	AFR	00	3.00	3.00	f	0	TANZANIA (UNITED REPUBLIC OF)	\N	\N	\N	\N	\N
UA	UKRAINE	UKRAINE	\N	380	EUR	8~10	2.00	2.00	f	0	UKRAINE	\N	\N	\N	\N	\N
UG	UGANDA	UGANDA	\N	256	AFR	00	3.00	3.00	f	0	UGANDA	\N	\N	\N	\N	\N
US	UNITED STATES OF AMERICA	UNITED STATES OF AMERICA	\N	1	NAMC	011	-8.00	-5.00	f	2	UNITED STATES OF AMERICA	\N	\N	\N	\N	\N
UY	URUGUAY	URUGUAY	\N	598	LAM	00	-3.00	-3.00	f	0	URUGUAY	\N	\N	\N	\N	\N
UZ	UZBEKISTAN	UZBEKISTAN	\N	998	SWACA	8~10	5.00	6.00	f	0	UZBEKISTAN	\N	\N	\N	\N	\N
VA	VATICAN CITY STATE (HOLY SEE)	VATICAN CITY STATE (HOLY SEE)	\N	39	EUR	00	1.00	1.00	f	1	VATICAN CITY STATE (HOLY SEE)	\N	\N	\N	\N	\N
VC	SAINT VINCENT AND THE GRENADINES	SAINT VINCENT AND THE GRENADINES	\N	1784	NAMC	011	-4.00	-4.00	f	0	SAINT VINCENT AND THE GRENADINES	\N	\N	\N	\N	\N
VE	VENEZUELA	VENEZUELA	\N	58	LAM	00	-4.00	-4.00	f	0	VENEZUELA	\N	\N	\N	\N	\N
VG	VIRGIN ISLANDS (BRITISH)	VIRGIN ISLANDS (BRITISH)	\N	1284	AUS	011	-4.00	-4.00	f	0	VIRGIN ISLANDS (BRITISH)	\N	\N	\N	\N	\N
VI	VIRGIN ISLANDS (US)	VIRGIN ISLANDS (US)	\N	1340	AUS	011	-4.00	-4.00	f	0	VIRGIN ISLANDS (US)	\N	\N	\N	\N	\N
VN	VIETNAM	VIETNAM	\N	84	EA	00	7.00	7.00	f	0	VIETNAM	\N	\N	\N	\N	\N
VU	VANUATU	VANUATU	\N	678	AUS	00	11.00	11.00	f	0	VANUATU	\N	\N	\N	\N	\N
WS	SAMOA	SAMOA	\N	685	AUS	0	-11.00	-11.00	f	0	SAMOA	\N	\N	\N	\N	\N
YE	YEMEN	YEMEN	\N	967	ME	00	3.00	3.00	f	0	YEMEN	\N	\N	\N	\N	\N
YT	MAYOTTE	MAYOTTE	\N	269	EA	00	3.00	3.00	f	0	MAYOTTE	\N	\N	\N	\N	\N
ZA	SOUTH AFRICA	SOUTH AFRICA	\N	27	AFR	09	2.00	2.00	f	0	SOUTH AFRICA	\N	\N	\N	\N	\N
ZM	ZAMBIA	ZAMBIA	\N	260	AFR	00	2.00	2.00	f	0	ZAMBIA	\N	\N	\N	\N	\N
ZW	ZIMBABWE	ZIMBABWE	\N	263	NAMC	00	2.00	2.00	f	0	ZIMBABWE	\N	\N	\N	\N	\N
\.


--
-- Data for Name: a_currency; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_currency (a_currency_code_c, a_currency_name_c, a_currency_symbol_c, p_country_code_c, a_display_format_c, a_in_emu_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
ALL	Albanian Lek	ALL	AL	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
ARS	Argentenian Peso	ARS	AR	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
ATS	Austrian Schilling	ATS	AT	->>>,>>>,>>>,>>9.99	t	\N	\N	\N	\N	\N
AUD	Australian Dollar	AUD	AU	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
BAM	Convertible Marka	BAM	BA	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
BDT	Bangladeshi Taka	BDT	BD	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
BEF	Belgian Franc	BEF	BE	->>,>>>,>>>,>>>,>>9	t	\N	\N	\N	\N	\N
BGL	Bulgarian Lev	BGL	BG	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
BRL	Brazilian Real	BRL	BR	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
CAD	Canadian Dollar	CA$	CA	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
CHF	Swiss Franc	CHF	CH	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
CLP	Chilean Peso	CLP	CL	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
CNY	Chinese Yuan Renminbi	CNY	CN	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
CRC	Costa Rican Colon	CRC	CR	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
CYP	Cyprus Pound	CYP	CY	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
CZK	Czech Koruna	CZK	CZ	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
DEM	Deutsche Mark	DM 	DE	->>>,>>>,>>>,>>9.99	t	\N	\N	\N	\N	\N
DKK	Danish Krone	DKK	DK	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
DZD	Algerian Dinar	DZD	DZ	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
EEK	Estonian Kroon	EEK	EE	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
EGP	Egyptian Pound	EGP	EG	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
ESP	Spanish Peseta	ESP	ES	->>,>>>,>>>,>>>,>>9	t	\N	\N	\N	\N	\N
EUR	European Monetary Unit	EUR	99	->>>,>>>,>>>,>>9.99	t	\N	\N	\N	\N	\N
FIM	Finnish Markka	FIM	FI	->>>,>>>,>>>,>>9.99	t	\N	\N	\N	\N	\N
FRF	French Franc	FFR	FR	->>>,>>>,>>>,>>9.99	t	\N	\N	\N	\N	\N
GBP	Pound Sterling	GBP	GB	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
GRD	Greek Drachma	GRD	GR	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
GTQ	Guatemalan Quetzal	GTQ	GT	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
HKD	Kong Kong Dollar	HK$	HK	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
HRK	Croatian Kuna	HRK	HR	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
HUF	Hungarian Forint	HUF	HU	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
IDR	Indonesian Rupiah	IDR	ID	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
IEP	Irish Punt	IEP	IE	->>>,>>>,>>>,>>9.99	t	\N	\N	\N	\N	\N
ILS	Israeli New Shekel	ILS	IL	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
INR	Indian Rupee	INR	IN	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
ITL	Italian Lira	ITL	IT	->>,>>>,>>>,>>>,>>9	t	\N	\N	\N	\N	\N
JOD	Jordanian Dinar	JOD	JO	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
JPY	Japanese Yen	Y  	JP	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
KES	Kenyan Shilling	KES	KE	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
KRW	South Korean Won	KRW	KR	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
KWD	Kuwaiti Dinar	KWD	KW	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
KZT	Kazakhstan Tenge	KZT	KZ	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
LBP	Lebanese Pound	LBP	LB	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
LUF	Luxembourg Franc	LUF	LU	->>,>>>,>>>,>>>,>>9	t	\N	\N	\N	\N	\N
MAD	Moroccan Dirham	MAD	MA	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
MDL	Moldovan Leu	MDL	MD	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
MMK	Myanmar Kyat	MMK	MM	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
MNT	Mongolian Tugrik	MNT	MN	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
MTL	Maltese Lira	MTL	MT	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
MXP	Mexican Nuevo Peso	MXN	MX	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
MYR	Malaysian Ringgit	MYR	MY	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
MZM	Mozambique Metical	MZM	MZ	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
MZN	New Mozambican Metical	MZN	MZ	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
NLG	Dutch Guilder	NLG	NL	->>>,>>>,>>>,>>9.99	t	\N	\N	\N	\N	\N
NOK	Norwegian Krone	NOK	NO	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
NPR	Nepalese Rupee	NPR	NP	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
NZD	New Zealand Dollar	NZ$	NZ	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
PEN	Peruvian Neuvo Sol	PES	PE	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
PGK	Papua New Guinea Kina	PGK	PG	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
PHP	Philippine Peso	PHP	PH	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
PKR	Pakistani Rupee	PKR	PK	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
PLZ	Polish Zloty	PLZ	PL	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
PTE	Portuguese Escudo	PTE	PT	->>>>,>>>,>>>,>>9.9	t	\N	\N	\N	\N	\N
PYG	Paraguayan Guarani	PYG	PY	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
ROL	Romanian Leu	ROL	RO	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
RON	Romanian New Lei	RON	RO	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
RUR	Russian Ruble	RUR	RU	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
SCR	Seychelles Rupee	SCR	SC	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
SDD	Sudanese Dinar	SDD	SD	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
SEK	Swedish Krona	SEK	SE	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
SGD	Singapore Dollar	SG$	SG	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
SKK	Slovakian Koruna	SKK	SK	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
SYP	Syrian Pound	SYP	SY	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
THB	Thai Baht	THB	TH	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
TND	Tunisian Dinar	TND	TN	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
TRL	Turkish Lira	TRL	TR	->>,>>>,>>>,>>>,>>9	f	\N	\N	\N	\N	\N
TWD	Taiwan Dollar	TW$	TW	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
UAH	Ukrainian Hryvna	UAH	UA	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
UAK	Ukraine Karbovanet	UAK	UA	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
USD	United States Dollar	US$	US	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
UYP	Uruguayan Peso	UYU	UY	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
YTL	New Turkish Lira	YTL	TR	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
ZAR	South African Rand	ZAR	ZA	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
ZWD	Zimbabwe Dollar	ZW$	ZW	->>>,>>>,>>>,>>9.99	f	\N	\N	\N	\N	\N
\.


--
-- Data for Name: a_tax_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_tax_type (a_tax_type_code_c, a_tax_type_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ledger; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ledger (a_ledger_number_i, a_ledger_name_c, a_ledger_status_l, a_last_batch_number_i, a_last_recurring_batch_number_i, a_last_gift_number_i, a_last_ap_inv_number_i, a_last_header_r_number_i, a_last_po_number_i, a_last_so_number_i, a_max_gift_aid_amount_n, a_min_gift_aid_amount_n, a_number_of_gifts_to_display_i, a_tax_type_code_c, a_ilt_gl_account_code_c, a_profit_loss_gl_account_code_c, a_current_accounting_period_i, a_number_of_accounting_periods_i, a_country_code_c, a_base_currency_c, a_transaction_account_flag_l, a_year_end_flag_l, a_forex_gains_losses_account_c, a_system_interface_flag_l, a_suspense_account_flag_l, a_bank_accounts_flag_l, a_delete_ledger_flag_l, a_new_financial_year_flag_l, a_recalculate_gl_master_flag_l, a_installation_id_c, a_budget_control_flag_l, a_budget_data_retention_i, a_cost_of_sales_gl_account_c, a_creditor_gl_account_code_c, a_current_financial_year_i, a_current_period_i, a_date_cr_dr_balances_d, a_debtor_gl_account_code_c, a_fa_depreciation_gl_account_c, a_fa_gl_account_code_c, a_fa_pl_on_sale_gl_account_c, a_fa_prov_for_depn_gl_account_c, a_ilt_account_flag_l, a_last_ap_dn_number_i, a_last_po_ret_number_i, a_last_so_del_number_i, a_last_so_ret_number_i, a_last_special_gift_number_i, a_number_fwd_posting_periods_i, a_periods_per_financial_year_i, a_discount_allowed_pct_n, a_discount_received_pct_n, a_po_accrual_gl_account_code_c, a_provisional_year_end_flag_l, a_purchase_gl_account_code_c, a_ret_earnings_gl_account_c, a_sales_gl_account_code_c, a_so_accrual_gl_account_code_c, a_stock_accrual_gl_account_c, a_stock_adj_gl_account_code_c, a_stock_gl_account_code_c, a_tax_excl_incl_l, a_tax_excl_incl_indicator_l, a_tax_input_gl_account_code_c, a_tax_input_gl_cc_code_c, a_tax_output_gl_account_code_c, a_terms_of_payment_code_c, a_last_po_rec_number_i, a_tax_gl_account_number_i, a_actuals_data_retention_i, p_partner_key_n, a_calendar_mode_l, a_year_end_process_status_i, a_last_header_p_number_i, a_ilt_processing_centre_l, a_last_gift_batch_number_i, a_intl_currency_c, a_last_rec_gift_batch_number_i, a_gift_data_retention_i, a_recalculate_all_periods_l, a_last_ich_number_i, a_branch_processing_l, a_consolidation_ledger_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	\N	t	0	0	0	0	0	0	0	0.0000000000	0.0000000000	0	\N	\N		0	12	DE	EUR	f	f	5003	f	f	f	f	f	f	\N	f	0	\N		0	1	\N	\N		\N		\N	f	0	0	0	0	0	8	0	0.00	0.00	\N	f	\N		\N		\N		\N	t	f	\N		\N		0	0	2	43000000	t	0	0	f	0	USD	0	11	f	0	f	f	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: p_acquisition; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_acquisition (p_acquisition_code_c, p_acquisition_description_c, p_valid_acquisition_l, p_deletable_l, p_recruiting_effort_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
ACCOUNTS	Entered in by the Accounts Department	t	f	f	\N	\N	\N	\N	\N
APL	Normal recruit applying through our organisation	t	f	t	\N	\N	\N	\N	\N
BOOKING	This partner has booked with us for a term of service.	t	f	f	\N	\N	\N	\N	\N
CHURCH	Church of Worker	t	f	f	\N	\N	\N	\N	\N
INQUIRY	This partner is new to the system and requests info.	t	f	f	\N	\N	\N	\N	\N
LOAD	Loaded in a patch	t	f	f	\N	\N	\N	\N	\N
MAILROOM	Entered by the Mailroom Department	t	f	f	\N	\N	\N	\N	\N
STAFF	Present Worker applying through this office	t	f	t	\N	\N	\N	\N	\N
PARTNER	Partner sought out our organisation without solicitation	t	f	f	\N	\N	\N	\N	\N
PERS-OFF	Partner entered by Personnel staff	t	f	f	\N	\N	\N	\N	\N
POSTCARD	A postal response card was received	t	f	f	\N	\N	\N	\N	\N
RECRUITS	Recruiting department entered this partner	t	f	f	\N	\N	\N	\N	\N
REPLY	Partner sent in a reply form from a publication.	t	f	f	\N	\N	\N	\N	\N
SETUP	Petra System Setup	t	f	f	\N	\N	\N	\N	\N
SUPPORTR	Supporter of Worker	t	f	f	\N	\N	\N	\N	\N
UNKNOWN	Acquisition not known	f	f	f	\N	\N	\N	\N	\N
UNT-LOAD	Uploaded for annual events/congresses	t	f	f	\N	\N	\N	\N	\N
IMPORT	Imported Data	t	t	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000b9b
\.


--
-- Data for Name: p_addressee_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_addressee_type (p_addressee_type_code_c, p_description_c, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
1-FEMALE	Single Female	f	\N	\N	\N	\N	\N
1-MALE	Single Male	f	\N	\N	\N	\N	\N
CHURCH	Church	f	\N	\N	\N	\N	\N
COUPLE	Couple (no children)	f	\N	\N	\N	\N	\N
DEFAULT	Default addressee type	f	\N	\N	\N	\N	\N
FAMILY	Family	f	\N	\N	\N	\N	\N
ORGANISA	Organisation	f	\N	\N	\N	\N	\N
VENUE	Venue	f	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_first_contact; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_first_contact (p_first_contact_code_c, p_first_contact_descr_c, p_active_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_language; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_language (p_language_code_c, p_language_description_c, p_congress_language_l, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
99	Other language	f	f	\N	\N	\N	\N	\N
AF	Afrikaans	f	f	\N	\N	\N	\N	\N
AR	Arabic	t	f	\N	\N	\N	\N	\N
BG	Bulgarian	f	f	\N	\N	\N	\N	\N
BN	Bengali	f	f	\N	\N	\N	\N	\N
CAN	Cantonese	f	f	\N	\N	\N	\N	\N
CRE	Creole	f	f	\N	\N	\N	\N	\N
CS	Czech	t	f	\N	\N	\N	\N	\N
CY	Welsh	f	f	\N	\N	\N	\N	\N
DA	Danish	t	f	\N	\N	\N	\N	\N
DAR	Dari	f	f	\N	\N	\N	\N	\N
DE	German	t	f	\N	\N	\N	\N	\N
EL	Greek	f	f	\N	\N	\N	\N	\N
EN	English	t	f	\N	\N	\N	\N	\N
ES	Spanish	t	f	\N	\N	\N	\N	\N
FA	Persian, Farsi	f	f	\N	\N	\N	\N	\N
FI	Finnish	t	f	\N	\N	\N	\N	\N
FO	Faroese	f	f	\N	\N	\N	\N	\N
FR	French	t	f	\N	\N	\N	\N	\N
GA	Irish	f	f	\N	\N	\N	\N	\N
GD	Gaelic	f	f	\N	\N	\N	\N	\N
HI	Hindi	f	f	\N	\N	\N	\N	\N
HR	Croatian	f	f	\N	\N	\N	\N	\N
HU	Hungarian	t	f	\N	\N	\N	\N	\N
IN	Indonesian	f	f	\N	\N	\N	\N	\N
IT	Italian	t	f	\N	\N	\N	\N	\N
IW	Hebrew	f	f	\N	\N	\N	\N	\N
JA	Japanese	f	f	\N	\N	\N	\N	\N
KM	Khmer	f	f	2009-03-16	\N	\N	\N	\N
KO	Korean	t	f	\N	\N	\N	\N	\N
MAN	Mandarin	f	f	\N	\N	\N	\N	\N
MS	Malay	f	f	\N	\N	\N	\N	\N
NE	Nepali	f	f	\N	\N	\N	\N	\N
NL	Dutch, Flemish	t	f	\N	\N	\N	\N	\N
NO	Norwegian	t	f	\N	\N	\N	\N	\N
PA	Punjabi	f	f	\N	\N	\N	\N	\N
PL	Polish	f	f	\N	\N	\N	\N	\N
PT	Portuguese	f	f	\N	\N	\N	\N	\N
RO	Romanian	f	f	\N	\N	\N	\N	\N
RU	Russian	f	f	\N	\N	\N	\N	\N
SK	Slovak	f	f	\N	\N	\N	\N	\N
SR	Serbian	f	f	\N	\N	\N	\N	\N
SV	Swedish	t	f	\N	\N	\N	\N	\N
TG	Tajik	f	f	\N	\N	\N	\N	\N
TH	Thai	f	f	\N	\N	\N	\N	\N
TL	Tagalog (Philipines)	f	f	\N	\N	\N	\N	\N
TO	Tonga	f	f	\N	\N	\N	\N	\N
TR	Turkish	f	f	\N	\N	\N	\N	\N
UG	Uighur	f	f	\N	\N	\N	\N	\N
UK	Ukrainian	t	f	\N	\N	\N	\N	\N
UR	Urdu	f	f	\N	\N	\N	\N	\N
UZ	Uzbek	f	f	\N	\N	\N	\N	\N
VI	Vietnamese	f	f	\N	\N	\N	\N	\N
YI	Yiddish	f	f	\N	\N	\N	\N	\N
YU	Yugoslavian	f	f	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_partner_classes; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_classes (p_partner_class_c, p_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
BANK	\N	\N	\N	\N	\N	\N
CHURCH	\N	\N	\N	\N	\N	\N
FAMILY	\N	\N	\N	\N	\N	\N
ORGANISATION	\N	\N	\N	\N	\N	\N
PERSON	\N	\N	\N	\N	\N	\N
UNIT	\N	\N	\N	\N	\N	\N
VENUE	\N	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_partner_status; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_status (p_status_code_c, p_partner_status_description_c, p_partner_is_active_l, p_include_partner_on_report_l, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
ACTIVE	Active Partner	t	t	f	\N	\N	\N	\N	\N
DIED	Partner has died	t	f	f	\N	\N	\N	\N	\N
INACTIVE	Inactive partner	t	f	f	\N	\N	\N	\N	\N
MERGED	Partner record has been merged into another	t	f	f	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_partner; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner (p_partner_key_n, p_partner_class_c, p_addressee_type_code_c, p_partner_short_name_c, p_partner_short_name_loc_c, p_printed_name_c, p_language_code_c, p_key_information_c, p_comment_c, p_acquisition_code_c, p_status_code_c, p_status_change_d, p_status_change_reason_c, p_deleted_partner_l, p_finance_comment_c, p_receipt_letter_frequency_c, p_receipt_each_gift_l, p_email_gift_statement_l, p_anonymous_donor_l, p_no_solicitations_l, p_child_indicator_l, p_restricted_i, p_user_id_c, p_group_id_c, p_previous_name_c, p_first_contact_code_c, p_first_contact_freeform_c, p_intranet_id_c, p_timezone_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
0	\N	DEFAULT	INVALID PARTNER	\N	\N	99	\N	\N	\N	\N	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	\N	\N	\N	\N
43000000	UNIT	DEFAULT	Germany	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	\N	\N	\N	\N
1000000	UNIT	DEFAULT	The Organisation	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	\N	\N	\N	\N
4000000	UNIT	DEFAULT	International Clearing House	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	\N	\N	\N	\N
95000000	UNIT	DEFAULT	Global Impact Fund	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	\N	\N	\N	\N
35000000	UNIT	DEFAULT	Switzerland	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	\N	\N	\N	\N
73000000	UNIT	DEFAULT	Kenya	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	\N	\N	\N	\N
43005001	FAMILY	FAMILY	Lloyd, Dalton	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000b9c
43005002	FAMILY	FAMILY	Hart, Julian	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000ba3
43005003	FAMILY	FAMILY	Pope, Dahlia	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000baa
43005004	FAMILY	FAMILY	Harvey, Ulric	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bb1
43005005	FAMILY	FAMILY	Ingram, Julian	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bb7
43005006	FAMILY	FAMILY	Parrish, Robert	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bbd
43005007	FAMILY	FAMILY	Oneal, Otto	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bc3
43005008	ORGANISATION	DEFAULT	IT Hardwarestore	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bc9
67000000	UNIT	DEFAULT	Austria	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bcd
1800501	UNIT	DEFAULT	Feed The Hungry	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd2
1800502	UNIT	DEFAULT	Free The Addicted	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd6
1800503	UNIT	DEFAULT	Heal The Injured	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bda
68000000	UNIT	DEFAULT	Australia	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bde
1800504	UNIT	DEFAULT	Save The Forest	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be2
1800505	UNIT	DEFAULT	Rescue the Dolphins	\N	\N	99	\N	\N	\N	ACTIVE	2011-01-11	\N	f	\N	\N	t	f	f	f	f	0	\N	\N	\N	\N	\N	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be6
\.


--
-- Data for Name: p_bank; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_bank (p_partner_key_n, p_branch_name_c, p_contact_partner_key_n, p_branch_code_c, p_bic_c, p_ep_format_file_c, p_credit_card_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_banking_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_banking_type (p_id_i, p_type_c, p_description_c, p_check_procedure, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_banking_details; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_banking_details (p_banking_details_key_i, p_banking_type_i, p_account_name_c, p_title_c, p_first_name_c, p_middle_name_c, p_last_name_c, p_bank_key_n, p_bank_account_number_c, p_iban_c, p_security_code_c, p_valid_from_date_d, p_expiry_date_d, p_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_account; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_account (a_ledger_number_i, a_account_code_c, a_account_type_c, a_account_code_long_desc_c, a_account_code_short_desc_c, a_eng_account_code_short_desc_c, a_eng_account_code_long_desc_c, a_debit_credit_indicator_l, a_account_active_flag_l, a_analysis_attribute_flag_l, a_standard_account_flag_l, a_consolidation_account_flag_l, a_intercompany_account_flag_l, a_budget_type_code_c, a_posting_status_l, a_system_account_flag_l, a_budget_control_flag_l, a_valid_cc_combo_c, a_foreign_currency_flag_l, a_foreign_currency_code_c, p_banking_details_key_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	0100	Income	Support Gifts: Local	Support Gifts: Local	Support Gifts: Local	Support Gifts: Local	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0100S	Income	Support Gifts: local	Support Gifts: local	Support Gifts: local	Support Gifts: local	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0200	Income	Fund Gifts: Local	Fund Gifts: Local	Fund Gifts: Local	Fund Gifts: Local	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0200S	Income	Fund Gifts: Local	Fund Gifts: Local	Fund Gifts: Local	Fund Gifts: Local	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0210	Income	Subscriptions and Fees	Subscriptions and Fees	Subscriptions and Fees	Subscriptions and Fees	f	t	f	t	t	f	Same	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0300	Income	Undesignated Gifts	Undesignated Gifts	Undesignated Gifts	Undesignated Gifts	f	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0300S	Income	Undesignated Gifts	Undesignated Gifts	Undesignated Gifts	Undesignated Gifts	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0400	Income	Project Gifts: Local	Project Gifts: Local	Project Gifts: Local	Project Gifts: Local	f	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0400S	Income	Project Gifts: Local (Total)	Project Gifts: Local	Project Gifts: Local	Project Gifts: Local (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0900	Income	Event Income for Others	Event Income for Others	Event Income for Others	Event Income for Others	f	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0900S	Income	Event Income for Others	Event Income for Others	Event Income for Others	Event Income for Others	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0910	Income	Event Supplements for Others	Event Supplements for Others	Event Supplements for Others	Event Supplements for Others	f	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0910S	Income	Event Supplements for Others (Total)	Event Supplements for Others	Event Supplements for Others	Event Supplements for Others (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0980	Income	Event Supplements	Event Supplements	Event Supplements	Event Supplements	f	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	0980S	Income	Event Supplements (Total)	Event Supplements	Event Supplements	Event Supplements (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1000	Income	Local Event Income	Local Event Income	Local Event Income	Local Event Income	f	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1000S	Income	Local Event Income (Total)	Local Event Income	Local Event Income	Local Event Income (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1010	Income	Local Event Supplements	Local Event Supplements	Local Event Supplements	Local Event Supplements	f	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1010S	Income	Local Event Supplements (Total)	Local Event Supplements	Local Event Supplements	Local Event Supplements (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1100	Income	Support Gifts: Foreign (ie via other funds)	Support Gifts: Foreign	Support Gifts, Foreign	Support Gifts: Foreign (ie via other funds)	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1100S	Income	Support Gifts: Foreign	Support Gifts: Foreign	Support Gifts: Foreign	Support Gifts: Foreign	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1200	Income	Fund Gifts: Foreign	Fund Gifts: Foreign	Fund Gifts: Foreign	Fund Gifts: Foreign	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1200S	Income	Fund Gifts: Foreign	Fund Gifts: Foreign	Fund Gifts: Foreign	Fund Gifts: Foreign	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1400	Income	Project Gifts: Foreign	Project Gifts: Foreign	Project Gifts: Foreign	Project Gifts: Foreign	f	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1400S	Income	Project Gifts: Foreign (Total)	Project Gifts: Foreign	Project Gifts: Foreign	Project Gifts: Foreign (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1900	Income	Foreign Income: Unidentified	Foreign Income: Unidentified	Foreign Income: Unidentified	Foreign Income: Unidentified	f	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	1900S	Income	Foreign Income: Unidentified	Foreign Income: Unidentified	Foreign Income: Unidentified	Foreign Income: Unidentified	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	2100	Income	Literature Sales external	Literature Sales external	Literature Sales external	Literature Sales external	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	2100S	Income	Literature Sales external	Literature Sales external	Literature Sales external	Literature Sales external	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	2200	Income	Literature Sales internal	Literature Sales internal	Literature Sales internal	Literature Sales internal	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	2200S	Income	Literature Sales internal (Total)	Literature Sales internal	Literature Sales internal	Literature Sales internal (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3100	Income	Interest Earned	Interest Earned	Interest Earned	Interest Earned	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3100S	Income	Interest (Total)	Interest	Interest	Interest (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3200	Income	Central Services	Central Services	Central Services	Central Services	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3200S	Income	Central Services (Total)	Central Services	Central Services	Central Services (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3300	Income	Grants from Other Funds	Grants from Other Funds	Grants from Other Funds	Grants from Other Funds	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3300S	Income	Grants from Other Funds (Total)	Grants from Other Funds	Grants from Other Funds	Grants from Other Funds (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3400	Income	Admin Grant Income	Admin Grant Income	Admin Grant Income	Admin Grant Income	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3400S	Income	Admin Grant Income (Total)	Admin Grant Income	Admin Grant Income	Admin Grant Income (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3700	Income	Other Income	Other Income	Other Income	Other Income	f	t	f	t	t	f	Same	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3700S	Income	Other Income (Total)	Other Income	Other Income	Other Income (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3710	Income	Registration Fees	Registration Fees	Registration Fees	Registration Fees	f	t	f	t	t	f	Same	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3720	Income	Sale of Fixed Assets	Sale of Fixed Assets	Sale of Fixed Assets	Sale of Fixed Assets	f	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3730	Income	Gifts in Kind	Gifts in Kind	Gifts in Kind	Gifts in Kind	f	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	3740	Income	Other Sales	Other Sales	Other Sales	Other Sales	f	t	f	t	t	f	Same	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4100	Expense	Ministry	Ministry: General	Ministry: General	Ministry	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4100S	Expense	Ministry (Total)	Ministry	Ministry	Ministry (Total)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4110	Expense	Literature	Literature: General	Literature: General	Literature	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4110S	Expense	Literature (Total)	Literature	Literature	Literature (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4111	Expense	Literature For Sale	Literature For Sale	Literature For Sale	Literature For Sale	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4112	Expense	Literature for free Distribution	Literature for free Distribution	Literature for free Distribution	Literature for free Distribution	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4113	Expense	Freight Inward (lit.)	Freight Inward (lit.)	Freight Inward (lit.)	Freight Inward (lit.)	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4114	Expense	Carriage Out (lit.)	Carriage Out (lit.)	Carriage Out (lit.)	Carriage Out (lit.)	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4120	Expense	Relief of Need	Relief of Need	Relief of Need	Relief of Need	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4120S	Expense	Relief of Need (Total)	Relief of Need	Relief of Need	Relief of Need (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4130S	Expense	Project Expenses (Total)	Project Expenses	Project Expenses	Project Expenses (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4140	Expense	AV (for outreach)	AV (for outreach)	AV (for outreach)	AV (for outreach)	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4140S	Expense	AV (for outreach) (Total)	AV (for outreach)	AV (for outreach)	AV (for outreach) (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4180	Expense	Home Event Expenses	Home Event Expenses	Home Event Expenses	Home Event Expenses	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4180S	Expense	Home Event Expenses (Total)	Home Event Expenses	Home Event Expenses	Home Event Expenses (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4200	Expense	Administration	Administration: General	Administration: General	Administration	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4200S	Expense	Administration (Total)	Administration	Administration	Administration (Total)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4202	Expense	Business Hospitality/Meals	Business Hospitality/Meals	Business Hospitality/Meals	Business Hospitality/Meals	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4203	Expense	Subscriptions	Subscriptions Payable	Subscriptions (Payable)	Subscriptions	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4210	Expense	Office Rent & Utilities	Office Rent & Utilities, General	Office Rent & Utilities, General	Office Rent & Utilities	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4210S	Expense	Office Rent & Utilities (Total)	Office Rent & Utilities	Office Rent & Utilities	Office Rent & Utilities (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4211	Expense	Rent	Rent	Rent	Rent	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4212	Expense	Gas	Gas	Gas	Gas	t	t	f	t	t	f	Same	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4213	Expense	Electricity	Electricity	Electricity	Electricity	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4214	Expense	Heating Oil	Heating Oil	Heating Oil	Heating Oil	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4215	Expense	Water & Sewage	Water & Sewage	Water & Sewage	Water & Sewage	t	t	f	t	t	f	Same	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4216	Expense	Insurance	Insurance	Insurance	Insurance	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4220	Expense	COMMUNICATION	Communication, General	Communication, General	COMMUNICATION	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4220S	Expense	Communication (Total)	Communication	Communication	Communication (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4221	Expense	Telephone	Telephone	Telephone	Telephone	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4222	Expense	Fax	Fax	Fax	Fax	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4223	Expense	E-mail	E-mail	E-mail	E-mail	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4224	Expense	Postage	Postage	Postage	Postage	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4225	Expense	Prayer Letter postage	Prayer Letter postage	Prayer Letter postage	Prayer Letter postage	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4230	Expense	OFFICE SUPPLIES	Office Supplies, General	Office Supplies, General	OFFICE SUPPLIES	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4230S	Expense	Office Supplies (Total)	Office Supplies	Office Supplies	Office Supplies (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4231	Expense	Stationery	Stationery	Stationery	Stationery	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4232	Expense	Computer Expense	Computer Expense	Computer Expense	Computer Expense	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4233	Expense	Printer/copier supplies	Printer/copier supplies	Printer/copier supplies	Printer/copier supplies	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4234	Expense	Photocopying	Photocopying	Photocopying	Photocopying	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4240	Expense	EQUIP/MAINT & REPAIRS	Equip. Maint. & Repairs, General	Equip. Maint. & Repairs, General	EQUIP/MAINT & REPAIRS	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4240S	Expense	Equipment, Maintenance & Repairs (Total)	Equipment, Maint. & Repairs	Equipment, Maint. & Repairs	Equipment, Maintenance & Repairs (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4241	Expense	Service Contracts	Service Contracts	Service Contracts	Service Contracts	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4242	Expense	Equipment Leasing	Equipment Leasing	Equipment Leasing	Equipment Leasing	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4250	Expense	BUILDING REPAIRS AND MAINTENANCE	Building Repairs & Maint	Building Repairs & Maint	BUILDING REPAIRS AND MAINTENANCE	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4250S	Expense	Building Repairs & Maintenance (Total)	Building Repairs & Maint.	Building Repairs & Maint.	Building Repairs & Maintenance (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4260	Expense	PROFESSIONAL FEES	Professional Fees, General	Professional Fees, General	PROFESSIONAL FEES	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4260S	Expense	Professional Fees (Total)	Professional Fees	Professional Fees	Professional Fees (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4261	Expense	Legal Fees	Legal Fees	Legal Fees	Legal Fees	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4262	Expense	Consultancy	Consultancy	Consultancy	Consultancy	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4263	Expense	Audit Fees	Audit Fees	Audit Fees	Audit Fees	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4280	Expense	HOME Event ADMIN.	Home Event Admin.	Home Event Admin.	HOME Event ADMIN.	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4280S	Expense	Home Event Administration (Total)	Home Event Admin.	Home Event Admin.	Home Event Administration (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	43	\N	\N	\N	\N	\N	t	t	f	f	f	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4300	Expense	PERSONNEL	Personnel, General	Personnel, General	PERSONNEL	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4300S	Expense	PERSONNEL	Personnel	Personnel	PERSONNEL	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4310S	Expense	Salaries/Allowances & Payroll Taxes (Total)	Salaries/Allow. & Payroll Taxes	Salaries/Allow. & Payroll Taxes	Salaries/Allowances & Payroll Taxes (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4330	Expense	PENSION/INSURANCE	Pension/Insurance, General	Pension/Insurance, General	PENSION/INSURANCE	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4330S	Expense	Pension/Insurance (Total)	Pension/Insurance	Pension/Insurance	Pension/Insurance (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4331	Expense	Pension	Pension	Pension	Pension	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4332	Expense	Insurance	Insurance	Insurance	Insurance	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4340	Expense	HOUSING	Housing	Housing	HOUSING	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4340S	Expense	Housing (Total)	Housing	Housing	Housing (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4350	Expense	TEAM LIVING	Team Living	Team Living	TEAM LIVING	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4350S	Expense	Team Living (Total)	Team Living	Team Living	Team Living (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4360	Expense	PERSONAL TRAVEL	Personal Travel	Personal Travel	PERSONAL TRAVEL	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4360S	Expense	Personal Travel (Total)	Personal Travel	Personal Travel	Personal Travel (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4370	Expense	MEDICAL EXPENSES	Medical Expenses	Medical Expenses	MEDICAL EXPENSES	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4370S	Expense	Medical Expenses (Total)	Medical Expenses	Medical Expenses	Medical Expenses (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4380	Expense	HOME Event PERSONNEL	Home Event Personnel	Home Event Personnel	HOME Event PERSONNEL	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4380S	Expense	Home Event Personnel (Total)	Home Event Personnel	Home Event Personnel	Home Event Personnel (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4390	Expense	FURTHER EDUCATION/TRAINING	Further Education/Training	Further Education/Training	FURTHER EDUCATION/TRAINING	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4390S	Expense	Further Education/Training (Total)	Further Education/Training	Further Education/Training	Further Education/Training (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4400	Expense	Business Travel	Business Travel, General	Business Travel, General	Business Travel	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4400S	Expense	BUSINESS TRAVEL	Business Travel	Business Travel	BUSINESS TRAVEL	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4410	Expense	PUBLIC TRANSPORT	Public Transport	Public Transport	PUBLIC TRANSPORT	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4410S	Expense	Public Transport (Total)	Public Transport	Public Transport	Public Transport (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4420	Expense	VEHICLE	Vehicle, General	Vehicle, General	VEHICLE	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4420S	Expense	Vehicle (Total)	Vehicle	Vehicle	Vehicle (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4421	Expense	Vehicle Maintenance	Vehicle Maintenance	Vehicle Maintenance	Vehicle Maintenance	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4422	Expense	Vehicle	Vehicle Insurance/Tax	Vehicle Insurance/Tax	Vehicle	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4423	Expense	Vehicle Fuel	Vehicle Fuel	Vehicle Fuel	Vehicle Fuel	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4430	Expense	AIR	Air	Air	AIR	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4430S	Expense	Air (Total)	Air	Air	Air (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4480	Expense	HOME Event TRAVEL	Home Event Travel	Home Event Travel	HOME Event TRAVEL	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4480S	Expense	Home Event Travel (Total)	Home Event Travel	Home Event Travel	Home Event Travel (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4500	Expense	Public Relations	Public Relations, General	Public Relations, General	Public Relations	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4500S	Expense	PUBLIC RELATIONS	Public Relations	Public Relations	PUBLIC RELATIONS	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4510	Expense	ADVERTSIING	Advertising	Advertising	ADVERTSIING	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4510S	Expense	Advertising (Total)	Advertising	Advertising	Advertising (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4520	Expense	NEWSLETTERS	Newsletters	Newsletters	NEWSLETTERS	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4520S	Expense	Newsletters (Total)	Newsletters	Newsletters	Newsletters (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4530	Expense	BROCHURES	Brochures	Brochures	BROCHURES	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4530S	Expense	Brochures (Total)	Brochures	Brochures	Brochures (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4550	Expense	AV PRODUCTIONS	AV Productions, General	AV Productions, General	AV PRODUCTIONS	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4550S	Expense	AV Productions (Total)	AV Productions	AV Productions	AV Productions (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4551	Expense	Videos	Videos	Videos	Videos	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4552	Expense	Slide Presentations	Slide Presentations	Slide Presentations	Slide Presentations	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4553	Expense	Casettes	Casettes	Casettes	Casettes	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4600	Expense	Cost of Sales	Cost of Sales, General	Cost of Sales, General	Cost of Sales	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4600S	Expense	Cost of Sales	Cost of Sales	Cost of Sales	Cost of Sales	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4800	Expense	Grants to Other Funds	Grants to Other Funds	Grants to Other Funds	Grants to Other Funds	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4800S	Expense	Grants to Other Funds (Total)	Grants to Other Funds	Grants to Other Funds	Grants to Other Funds (Total)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4900	Expense	Admin Grant Expense	Admin Grant Expense	Admin Grant Expense	Admin Grant Expense	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	4900S	Expense	Admin Grant Expense (Total)	Admin Grant Expense	Admin Grant Expense	Admin Grant Expense (Total)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5000	Expense	Financial and Other	Financial and Other, General	Financial and Other, General	Financial and Other	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5000S	Expense	Financial  and Other (Total)	Financial  and Other	Financial  and Other	Financial  and Other (Total)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5003	Expense	Currency Revaluation	Currency Revaluation	Currency Revaluation	Currency Revaluation	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5010	Expense	EQUIPMENT ACQUIRED	Equipment Acquired, General	Equipment Acquired, General	EQUIPMENT ACQUIRED	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5010S	Expense	Equipment Acquired (Total)	Equipment Acquired	Equipment Acquired	Equipment Acquired (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5011	Expense	Business Equi[ment	Business Equipment	Business Equipment	Business Equipment	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5012	Expense	Vehicle	Vehicle	Vehicle	Vehicle	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5013	Expense	Computer Equipment	Computer Equipment	Computer Equipment	Computer Exuipment	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5014	Expense	Audio Visial Equipment	Audio Visial Equipment	Audio Visial Equipment	Audio Visial Equipment	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5020	Expense	DEPRECIATION	Depreciation	Depreciation	DEPRECIATION	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5020S	Expense	Depreciation (Total)	Depreciation	Depreciation	Depreciation (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5030	Expense	INTEREST & BANK CHARGES	Interest & Bank Charges	Interest & Bank Charges	INTEREST & BANK CHARGES	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5030S	Expense	Interest & Bank Charges (Total)	Interest & Bank Charges	Interest & Bank Charges	Interest & Bank Charges (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5040	Expense	Bad and Doubtful Debts	Bad and Doubtful Debts, General	Bad and Doubtful Debts, General	Bad and Doubtful Debts	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5040S	Expense	Bad and Doubtful Debts (Total)	Bad and Doubtful Debts	Bad and Doubtful Debts	Bad and Doubtful Debts (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5041	Expense	Bad Debts	Bad Debts	Bad Debts	Bad Debts	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5042	Expense	Doubtful Debts	Doubtful Debts	Doubtful Debts	Doubtful Debts	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5050	Expense	PRIOR YEAR ADJUSTMENTS	Prior Year Adjustments	Prior Year Adjustments	PRIOR YEAR ADJUSTMENTS	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5050S	Expense	Prior Year Adjustments (Total)	Prior Year Adjustments	Prior Year Adjustments	Prior Year Adjustments (Total)	t	t	f	t	t	f	\N	f	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5100	Expense	Conference Expenses	Conference Expenses	Conference Expenses	Conference Expenses	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5100S	Expense	Conference Expenses	Conference Expenses	Conference Expenses	Conference Expenses	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5200	Expense	Event Fees to Other Funds	Event Fees to Other Funds	Event Fees to Other Funds	Event Fees to Other Funds	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5200S	Expense	Event Fees to Other Funds	Event Fees to Other Funds	Event Fees to Other Funds	Event Fees to Other Funds	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5400	Expense	Foreign Expenses: Unidentified	Foreign Expenses: Unidentified	Foreign Expenses: Unidentified	Foreign Expenses: Unidentified	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5400S	Expense	Foreign Expenses: Unidentified	Foreign Expenses: Unidentified	Foreign Expenses: Unidentified	Foreign Expenses: Unidentified	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5500	Expense	Recharges to Other Funds	Recharges to Other Funds	Recharges to Other Funds	Recharges to Other Funds	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5500S	Expense	Recharges to Other Funds (Total)	Recharges to Other Funds	Recharges to Other Funds	Recharges to Other Funds (Total)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5501	Expense	DIRECT TRANSFERS	Direct Transfers	Direct Transfers	DIRECT TRANSFERS	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5600	Expense	ICH Settlement Transfers	ICH Settlement Transfers	ICH Settlement Transfers	ICH Settlement Transfers	t	t	f	t	t	f	Adhoc	t	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5600S	Expense	ICH Settlement (Total)	ICH Settlement	ICH Settlement	ICH Settlement (Total)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	5601	Expense	ICH Settlement	ICH Settlement	ICH Settlement	ICH Settlement	t	t	f	t	t	f	Adhoc	t	f	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6000	Asset	Petty Cash	Petty Cash	Petty Cash	Petty Cash	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6000S	Asset	Petty Cash Accounts (Total)	Petty Cash Accounts (Total)	Petty Cash Accounts (Total)	Petty Cash Accounts (Total)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6200	Asset	Bank Accounts Operational	Bank Accounts Operational	Bank Accounts Operational	Bank Accounts Operational	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6200S	Asset	Bank Accounts(Total Operational)	Bank Accounts(Total Operational)	Bank Accounts(Total Operational)	Bank Accounts(Total Operational)	t	t	f	t	f	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6400	Asset	Bank Accounts Deposit	Bank Accounts Deposit	Bank Accounts Deposit	Bank Accounts Deposit	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6400S	Asset	Bank Accounts (Total Deposit)	Bank Accounts (Total Deposit)	Bank Accounts (Total Deposit)	Bank Accounts (Total Deposit)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6500	Asset	Investments, General	Investments	Investments	Investments, General	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6500S	Asset	INVESTMENTS	Investments	Investments	INVESTMENTS	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6600	Liability	Provision for Doubtful Debts	Provision for Doubtful Debts	Provision for Doubtful Debts	Provision for Doubtful Debts	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6600S	Asset	Provision for Dbtful Debts (Total)	Provision for Dbtful Debts	Provision for Dbtful Debts	Provision for Dbtful Debts (Total)	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6700	Asset	Prepaid Expense	Prepaid Expense	Prepaid Expense	Prepaid Expense	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6700S	Asset	Prepaid Expenses	Prepaid Expenses	Prepaid Expenses	Prepaid Expenses	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6800	Asset	Accounts Receivable within one year	Accounts Receivable	Accounts Receivable	Accounts Receivable within one year	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6800S	Asset	Accounts Receivable within one year	Accounts Receivable	Accounts Receivable	Accounts Receivable within one year	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6900	Asset	Accounts Receivable from other Funds within one year	Accounts Receivable Funds	Accounts Receivable Funds	Accounts Receivable from other Funds within one year	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	6900S	Asset	Accounts Receivable from other Funds within one year	Accounts Receivable Funds	Accounts Receivable Funds	Accounts Receivable from other Funds within one year	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7000	Asset	Long Term Loans Receivable	Long Term Loans Receivable	Long Term Loans Receivable	Long Term Loans Receivable	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7000S	Asset	Long Term Loans Receivable	Long Term Loans Receivable	Long Term Loans Receivable	Long Term Loans Receivable	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7010	Asset	Deposits (landlords etc)	Deposits (landlords etc)	Deposits (landlords etc)	Deposits (landlords etc)	t	t	f	t	t	f	Adhoc	t	f	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7100	Asset	Long Term Loans Receivable from other Funds	Long Term Loans Receivable Funds	Long Term Loans Receivable Funds	Long Term Loans Receivable from other Funds	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7100S	Asset	Long Term Loans Receivable from other Funds	Long Term Loans Receivable Funds	Long Term Loans Receivable Funds	Long Term Loans Receivable from other Funds	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7200	Asset	Stock Inventory	Stock Inventory	Stock Inventory	Stock Inventory	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7200S	Asset	Stock Inventory	Stock Inventory	Stock Inventory	Stock Inventory	t	t	f	t	t	f	Adhoc	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7300	Asset	Equipment	Equipment, General	Equipment, General	Equipment	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7300S	Asset	Equipment	Equipment	Equipment	Equipment	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7311	Asset	Business Equipment	Business Equipment	Business Equipment	Business Equipment	t	t	f	t	t	f	Adhoc	t	f	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7312	Asset	Vehicles	Vehicles	Vehicles	Vehicles	t	t	f	t	t	f	Adhoc	t	f	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7313	Asset	Computer Equipment	Computer Equipment	Computer Equipment	Computer Equipment	t	t	f	t	t	f	Adhoc	t	f	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7314	Asset	Audio Visual Equipment	Audio Visual Equipment	Audio Visual Equipment	Audio Visual Equipment	t	t	f	t	t	f	Adhoc	t	f	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7400	Asset	Land & Buildings	Land & Buildings	Land & Buildings	Land & Buildings	t	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7400S	Asset	Land & Buildings	Land & Buildings	Land & Buildings	Land & Buildings	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7500	Liability	Accumulated Depreciation Equip	Accumulated Depreciation Equip	Accumulated Depreciation Equip	Accumulated Depreciation Equip	f	t	f	t	t	f	\N	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7500S	Asset	Accum. Depreciation: Equipment	Accum. Depreciation: Equipment	Accum. Depreciation: Equipment	Accum. Depreciation: Equipment	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7600	Liability	Land and Buildings	Land and Buildings	Land and Buildings	Land and Buildings	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	7600S	Asset	Accum. Depreciation:  Land & Buildings	Accum. Depr.:  Land & Buildings	Accum. Depr.:  Land & Buildings	Accum. Depreciation:  Land & Buildings	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	8100	Liability	Stewardship Clearing	Stewardship Clearing	Stewardship Clearing	Stewardship Clearing	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	8100S	Liability	Stewardship Clearing	Stewardship Clearing	Stewardship Clearing	Stewardship Clearing	f	t	f	t	t	f	Adhoc	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	8200	Liability	Suspense Account	Suspense Account	Suspense Account	Suspense Account	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	8200S	Liability	Suspense Accounts (Total)	Suspense Accounts	Suspense Accounts	Suspense Accounts (Total)	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	8500	Liability	International Clearing House	International Clearing House	International Clearing House	International Clearing House	f	t	f	t	t	f	\N	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	8500S	Liability	International Clearing House	International Clearing House	International Clearing House	International Clearing House	f	t	f	t	t	f	\N	f	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9000	Liability	Deferred Income	Deferred income	Deferred income	Defrerred Income	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9000S	Liability	Deferred Income (Total)	Deferred income	Deferred income	Deferred Income(Total)	f	t	f	t	t	f	Adhoc	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9100	Liability	Accounts Payable within one year	Accounts Payable	Accounts Payable	Accounts Payable within one year	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9100S	Liability	Accounts Payable within one year	Accounts Payable	Accounts Payable	Accounts Payable within one year	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9200	Liability	Accounts Payable to other Funds within one year	Accounts Payable Funds	Accounts Payable Funds	Accounts Payable to other Funds within one year	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9200S	Liability	Accounts Payable to other Funds within one year	Accounts Payable Funds	Accounts Payable Funds	Accounts Payable to other Funds within one year	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9300	Liability	Long Term Loans Payable	Long Term Loans Payable	Long Term Loans Payable	Long Term Loans Payable	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9300S	Liability	Long Term Loans Payable	Long Term Loans Payable	Long Term Loans Payable	Long Term Loans Payable	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9400	Liability	Long Term Loans Payable to other Funds	Long Term Loans Payable Funds	Long Term Loans Payable Funds	Long Term Loans Payable to other Funds	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9400S	Liability	Long Term Loans Payable to other Funds	Long Term Loans Payable Funds	Long Term Loans Payable Funds	Long Term Loans Payable to other Funds	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9500	Liability	Provisions	Provisions	Provisions	Provisions	f	t	f	t	t	f	\N	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9500S	Liability	Provisions	Provisions	Provisions	Provisions	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9700	Equity	Balance Brought Forward 1st January	Brought Forward 1st January	Brought Forward 1st January	Balance Brought Forward 1st January	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9700S	Equity	Balance Brought Forward 1st January	Brought Forward 1st January	Brought Forward 1st January	Balance Brought Forward 1st January	f	t	f	t	t	f	Adhoc	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9800	Liability	Internal Transfer	Internal Transfer	Internal Transfer	Internal Transfer	f	t	f	t	t	f	Adhoc	t	t	f	Local	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	9800S	Liability	Internal Transfer	Internal Transfer	Internal Transfer	Internal Transfer	f	t	f	t	t	f	Adhoc	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	ASSETS	Asset	Total Assets	Total Assets	Total Assets	Total Assets	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	BAL SHT	Asset	Balance Sheet	Balance Sheet	Balance Sheet	Balance Sheet	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	CASH	Asset	Bank	Cash & Bank	Cash & Bank	Bank	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	CRS	Liability	Creditors:Due within one year	Creditors:Due within one year	Creditors:Due within one year	Creditors:Due within one year	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	CRS CTRL	Liability	Creditor's Control	Creditor's Control	Creditor's Control	Creditor's Control	f	t	f	t	f	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	CRS LNG	Liability	Creditors:Due in more than one year	Creditors:Due in more than 1 year	Creditors:Due in more than 1 year	Creditors:Due in more than one year	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	DRS	Asset	Debtors:Due within one year	Debtors:Due within one year	Debtors:Due within one year	Debtors:Due within one year	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	DRS LNG	Asset	Debtors:Due in more than one year	Debtors:Due in more than 1 year	Debtors:Due in more than 1 year	Debtors:Due in more than one year	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	EXP	Expense	Total Expenditure	Total Expenditure	Total Expenditure	Total Expenditure	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	FA	Asset	Fixed Assets	Fixed Assets	Fixed Assets	Fixed Assets	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	GIFT	Income	Gift Income	Gift Income	Gift Income	Gift Income	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	ILT	Liability	Inter Ledger Transfer Total	Inter Ledger Transfer Total	Inter Ledger Transfer Total	Inter Ledger Transfer Total	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	INC	Income	Total Income	Total Income	Total Income	Total Income	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	LIABS	Liability	Total Liabilities	Total Liabilities	Total Liabilities	Total Liabilities	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	LIT SALE	Income	Total Literature Sales	Literature Sales	Literature Sales	Total Literature Sales	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	OTHEXP	Expense	Other Expenditure	Other Expenditure	Other Expenditure	Other Expenditure	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	OTHINC	Income	Other Income	Other Income	Other Income	Other Income	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	PL	Income	Surplus or Deficit	Surplus or Deficit	Surplus or Deficit	Surplus or Deficit	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	RET EARN	Equity	Equity	Equity	Equity	Equity	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	STC INC	Income	Short Term Event Income	Short Term Event Income	Short Term Event Income	Short Term Event Income	f	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
43	STOCK	Asset	Stock	Stock	Stock	Stock	t	t	f	t	t	f	\N	f	t	f	All	f	\N	\N	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_account_hierarchy; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_account_hierarchy (a_ledger_number_i, a_account_hierarchy_code_c, a_root_account_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	STANDARD	43	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_account_hierarchy_detail; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_account_hierarchy_detail (a_ledger_number_i, a_account_hierarchy_code_c, a_reporting_account_code_c, a_account_code_to_report_to_c, a_report_order_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	STANDARD	0100	0100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	0100S	GIFT	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	0200	0200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	0200S	GIFT	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	0210	0200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	0300	0300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	0300S	GIFT	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	0400	0400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	0400S	GIFT	60	2009-06-15	\N	\N	\N	\N
43	STANDARD	0900	0900S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	0900S	STC INC	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	0910	0910S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	0910S	0900S	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	0980	0980S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	0980S	0900S	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	1000	1000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	1000S	STC INC	35	2009-06-15	\N	\N	\N	\N
43	STANDARD	1010	1010S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	1010S	1000S	35	2009-06-15	\N	\N	\N	\N
43	STANDARD	1100	1100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	1100S	GIFT	40	2009-06-15	\N	\N	\N	\N
43	STANDARD	1200	1200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	1200S	GIFT	50	2009-06-15	\N	\N	\N	\N
43	STANDARD	1400	1400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	1400S	GIFT	80	2009-06-15	\N	\N	\N	\N
43	STANDARD	1900	1900S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	1900S	GIFT	70	2009-06-15	\N	\N	\N	\N
43	STANDARD	2100	2100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	2100S	LIT SALE	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	2200	2200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	2200S	LIT SALE	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	3100	3100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	3100S	OTHINC	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	3200	3200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	3200S	OTHINC	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	3300	3300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	3300S	OTHINC	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	3400	3400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	3400S	OTHINC	40	2009-06-15	\N	\N	\N	\N
43	STANDARD	3700	3700S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	3700S	OTHINC	50	2009-06-15	\N	\N	\N	\N
43	STANDARD	3710	3700S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	3720	3700S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	3730	3700S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	3740	3700S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4100	4100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4100S	EXP	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	4110	4110S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4110S	4100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4111	4110S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4112	4110S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4113	4110S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4114	4110S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4120	4120S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4120S	4100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4130S	4100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4140	4140S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4140S	4100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4180	4180S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4180S	4100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4200	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4200S	EXP	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	4202	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4203	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4210	4210S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4210S	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4211	4210S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4212	4210S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4213	4210S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4214	4210S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4215	4210S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4216	4210S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4220	4220S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4220S	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4221	4220S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4222	4220S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4223	4220S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4224	4220S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4225	4220S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4230	4230S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4230S	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4231	4230S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4232	4230S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4233	4230S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4234	4230S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4240	4240S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4240S	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4241	4240S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4242	4240S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4250	4250S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4250S	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4260	4260S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4260S	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4261	4260S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4262	4260S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4263	4260S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4280	4280S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4280S	4200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4300	4300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4300S	EXP	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	4310S	4300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4330	4330S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4330S	4300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4331	4330S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4332	4330S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4340	4340S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4340S	4300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4350	4350S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4350S	4300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4360	4360S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4360S	4300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4370	4370S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4370S	4300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4380	4380S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4380S	4300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4390	4390S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4390S	4300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4400	4400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4400S	EXP	40	2009-06-15	\N	\N	\N	\N
43	STANDARD	4410	4410S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4410S	4400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4420	4420S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4420S	4400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4421	4420S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4422	4420S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4423	4420S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4430	4430S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4430S	4400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4480	4480S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4480S	4400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4500	4500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4500S	EXP	50	2009-06-15	\N	\N	\N	\N
43	STANDARD	4510	4510S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4510S	4500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4520	4520S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4520S	4500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4530	4530S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4530S	4500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4550	4550S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4550S	4500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4551	4550S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4552	4550S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4553	4550S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4600	4600S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4600S	EXP	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4800	4800S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4800S	OTHEXP	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	4900	4900S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	4900S	OTHEXP	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	5000	5000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5000S	OTHEXP	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	5003	5000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5010	5010S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5010S	5000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5011	5010S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5012	5010S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5013	5010S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5014	5010S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5020	5020S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5020S	5000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5030	5030S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5030S	5000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5040	5040S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5040S	5000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5041	5040S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5042	5040S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5050	5050S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5050S	5000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5100	5100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5100S	OTHEXP	40	2009-06-15	\N	\N	\N	\N
43	STANDARD	5200	5200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5200S	OTHEXP	50	2009-06-15	\N	\N	\N	\N
43	STANDARD	5400	5400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5400S	OTHEXP	70	2009-06-15	\N	\N	\N	\N
43	STANDARD	5500	5500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5500S	OTHEXP	80	2009-06-15	\N	\N	\N	\N
43	STANDARD	5501	5500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5600	5600S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	5600S	OTHEXP	90	2009-06-15	\N	\N	\N	\N
43	STANDARD	5601	5600S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	6000	6000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	6000S	CASH	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	6200	6200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	6200S	CASH	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	6400	6400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	6400S	CASH	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	6500	6500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	6500S	ASSETS	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	6600	6600S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	6600S	DRS	50	2009-06-15	\N	\N	\N	\N
43	STANDARD	6700	6700S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	6700S	DRS	40	2009-06-15	\N	\N	\N	\N
43	STANDARD	6800	6800S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	6800S	DRS	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	6900	6900S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	6900S	DRS	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	7000	7000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7000S	DRS LNG	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	7010	7000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7100	7100S	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	7100S	DRS LNG	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	7200	7200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7200S	STOCK	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7300	7300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7300S	FA	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	7311	7300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7312	7300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7313	7300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7314	7300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7400	7400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7400S	FA	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	7500	7500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7500S	FA	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	7600	7600S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	7600S	FA	40	2009-06-15	\N	\N	\N	\N
43	STANDARD	8100	8100S	70	2009-06-15	\N	\N	\N	\N
43	STANDARD	8100S	CRS	70	2009-06-15	\N	\N	\N	\N
43	STANDARD	8200	8200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	8200S	LIABS	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	8500	8500S	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	8500S	CRS	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	9000	9000S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	9000S	LIABS	40	2009-06-15	\N	\N	\N	\N
43	STANDARD	9100	9100S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	9100S	CRS	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	9200	9200S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	9200S	CRS	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	9300	9300S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	9300S	CRS LNG	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	9400	9400S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	9400S	CRS LNG	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	9500	9500S	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	9500S	CRS LNG	50	2009-06-15	\N	\N	\N	\N
43	STANDARD	9700	9700S	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	9700S	RET EARN	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	9800	9800S	60	2009-06-15	\N	\N	\N	\N
43	STANDARD	9800S	LIABS	60	2009-06-15	\N	\N	\N	\N
43	STANDARD	ASSETS	BAL SHT	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	BAL SHT	43	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	CASH	ASSETS	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	CRS	LIABS	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	CRS CTRL	CRS	0	2009-06-15	\N	\N	\N	\N
43	STANDARD	CRS LNG	LIABS	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	DRS	ASSETS	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	DRS LNG	ASSETS	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	EXP	PL	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	FA	ASSETS	60	2009-06-15	\N	\N	\N	\N
43	STANDARD	GIFT	INC	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	ILT	LIABS	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	INC	PL	10	2009-06-15	\N	\N	\N	\N
43	STANDARD	LIABS	BAL SHT	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	LIT SALE	INC	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	OTHEXP	EXP	60	2009-06-15	\N	\N	\N	\N
43	STANDARD	OTHINC	INC	40	2009-06-15	\N	\N	\N	\N
43	STANDARD	PL	RET EARN	20	2009-06-15	\N	\N	\N	\N
43	STANDARD	RET EARN	BAL SHT	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	STC INC	INC	30	2009-06-15	\N	\N	\N	\N
43	STANDARD	STOCK	ASSETS	50	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_account_property_code; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_account_property_code (a_property_code_c, a_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
Bank Account	\N	2009-06-15	\N	\N	\N	\N
CARRYFORWARDCC	Used to specify that a particular equity account should be used to carry forward the specified Cost Centre. The property value associated with it includes both the Cost Centre and also some text (SAMECC or STANDARDCC) to indicate whether the equity should be carried forward into the same CC or rolled up to the standard CC. (eg. 10TEAPIC,SAMECC)	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_account_property; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_account_property (a_ledger_number_i, a_account_code_c, a_property_code_c, a_property_value_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	6200	Bank Account	true	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_accounting_period; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_accounting_period (a_ledger_number_i, a_accounting_period_number_i, a_accounting_period_desc_c, a_period_start_date_d, a_period_end_date_d, a_effective_date_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	1	January	2010-01-01	2010-01-31	2010-06-15	2010-06-15	\N	\N	\N	\N
43	2	February	2010-02-01	2010-02-28	2010-06-15	2010-06-15	\N	\N	\N	\N
43	3	March	2010-03-01	2010-03-31	2010-06-15	2010-06-15	\N	\N	\N	\N
43	4	April	2010-04-01	2010-04-30	2010-06-15	2010-06-15	\N	\N	\N	\N
43	5	May	2010-05-01	2010-05-31	2010-06-15	2010-06-15	\N	\N	\N	\N
43	6	June	2010-06-01	2010-06-30	2010-06-15	2010-06-15	\N	\N	\N	\N
43	7	July	2010-07-01	2010-07-31	2010-06-15	2010-06-15	\N	\N	\N	\N
43	8	August	2010-08-01	2010-08-31	2010-06-15	2010-06-15	\N	\N	\N	\N
43	9	September	2010-09-01	2010-09-30	2010-06-15	2010-06-15	\N	\N	\N	\N
43	10	October	2010-10-01	2010-10-31	2010-06-15	2010-06-15	\N	\N	\N	\N
43	11	November	2010-11-01	2010-11-30	2010-06-15	2010-06-15	\N	\N	\N	\N
43	12	December	2010-12-01	2010-12-31	2010-06-15	2010-06-15	\N	\N	\N	\N
43	13	January	2011-01-01	2011-01-31	2010-06-15	2010-06-15	\N	\N	\N	\N
43	14	February	2011-02-01	2011-02-28	2010-06-15	2010-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_accounting_system_parameter; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_accounting_system_parameter (a_number_of_accounting_periods_i, a_actuals_data_retention_i, a_budget_data_retention_i, a_number_fwd_posting_periods_i, a_ledger_number_i, a_recipient_gift_statement_txt_c, a_recipient_gift_statement_tx2_c, a_donor_gift_statement_txt_c, a_donor_gift_statement_tx2_c, a_hosa_statement_txt_c, a_hosa_statement_tx2_c, a_hosa_statement_tx3_c, a_hosa_statement_tx4_c, a_donor_receipt_txt_c, a_stewardship_report_txt_c, a_stewardship_report_tx2_c, a_donor_yearly_receipt_txt_c, a_gift_data_retention_i, a_deceased_address_text_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
20	11	2	2	43													8		2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_analysis_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_analysis_type (a_analysis_type_code_c, a_analysis_type_description_c, a_analysis_mode_l, a_analysis_store_c, a_analysis_element_c, a_system_analysis_type_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_cost_centre_types; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_cost_centre_types (a_ledger_number_i, a_cost_centre_type_c, a_cc_description_c, a_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	Local	\N	f	\N	\N	\N	\N	\N
43	Foreign	\N	f	\N	\N	\N	\N	\N
\.


--
-- Data for Name: a_key_focus_area; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_key_focus_area (a_key_focus_area_c, a_key_focus_area_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_cost_centre; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_cost_centre (a_ledger_number_i, a_cost_centre_code_c, a_cost_centre_to_report_to_c, a_cost_centre_name_c, a_posting_cost_centre_flag_l, a_cost_centre_active_flag_l, a_project_status_l, a_project_constraint_date_d, a_project_constraint_amount_n, a_system_cost_centre_flag_l, a_cost_centre_type_c, a_key_focus_area_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	4300	4300S	Germany, General	t	t	f	\N	0.0000000000	t	Local	\N	2009-06-03	\N	\N	\N	\N
43	4300S	[43]	Germany	f	t	f	\N	0.0000000000	t	Local	\N	2009-06-03	\N	\N	\N	\N
43	0400	ILT	International Clearing House	t	t	f	\N	0.0000000000	t	Foreign	\N	2009-06-03	\N	\N	\N	\N
43	3500	ILT	Switzerland	t	t	f	\N	0.0000000000	t	Foreign	\N	2009-06-03	\N	\N	\N	\N
43	7300	ILT	Kenya	t	t	f	\N	0.0000000000	t	Foreign	\N	2009-06-03	\N	\N	\N	\N
43	9500	ILT	Global Impact Fund	t	t	f	\N	0.0000000000	t	Foreign	\N	2009-06-03	\N	\N	\N	\N
43	ILT	[43]	Inter Ledger Transfer Total	f	t	f	\N	0.0000000000	t	Local	\N	2009-06-03	\N	\N	\N	\N
43	[43]	\N	[Germany]	f	t	f	\N	0.0000000000	t	Local	\N	2009-06-03	\N	\N	\N	\N
\.


--
-- Data for Name: a_analysis_attribute; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_analysis_attribute (a_ledger_number_i, a_analysis_type_code_c, a_account_code_c, a_cost_centre_code_c, a_active_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_analysis_store_table; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_analysis_store_table (a_store_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ap_supplier; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ap_supplier (p_partner_key_n, a_preferred_screen_display_i, a_default_bank_account_c, a_payment_type_c, a_currency_code_c, a_default_ap_account_c, a_default_credit_terms_i, a_default_discount_percentage_n, a_default_discount_days_i, a_supplier_type_c, a_default_exp_account_c, a_default_cost_centre_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ap_document; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ap_document (a_ledger_number_i, a_ap_number_i, p_partner_key_n, a_credit_note_flag_l, a_document_code_c, a_reference_c, a_date_issued_d, a_date_entered_d, a_credit_terms_i, a_total_amount_n, a_exchange_rate_to_base_n, a_discount_percentage_n, a_discount_days_i, a_ap_account_c, a_last_detail_number_i, a_document_status_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ap_document_detail; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ap_document_detail (a_ledger_number_i, a_ap_number_i, a_detail_number_i, a_detail_approved_l, a_cost_centre_code_c, a_account_code_c, a_item_ref_c, a_narrative_c, a_amount_n, a_approval_date_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_freeform_analysis; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_freeform_analysis (a_analysis_type_code_c, a_analysis_value_c, a_ledger_number_i, a_active_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ap_anal_attrib; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ap_anal_attrib (a_ledger_number_i, a_ap_number_i, a_detail_number_i, a_analysis_type_code_c, a_account_code_c, a_analysis_attribute_value_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ap_payment; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ap_payment (a_ledger_number_i, a_payment_number_i, a_amount_n, a_exchange_rate_to_base_n, a_payment_date_d, s_user_id_c, a_method_of_payment_c, a_reference_c, a_bank_account_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ap_document_payment; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ap_document_payment (a_ledger_number_i, a_ap_number_i, a_payment_number_i, a_amount_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ar_category; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_category (a_ar_category_code_c, a_ar_description_c, a_ar_local_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ar_article; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_article (a_ar_article_code_c, a_ar_category_code_c, a_tax_type_code_c, a_ar_specific_article_l, a_ar_description_c, a_ar_local_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ar_article_price; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_article_price (a_ar_article_code_c, a_ar_date_valid_from_d, a_ar_amount_n, a_currency_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_type_category; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_type_category (p_code_c, p_description_c, p_unassignable_flag_l, p_unassignable_date_d, p_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_type (p_type_code_c, p_type_description_c, p_category_code_c, p_valid_type_l, p_type_deletable_l, p_type_motivation_group_c, p_type_motivation_detail_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
PARENT	Parent of a Worker	\N	t	f	\N	\N	\N	\N	\N	\N	\N
PASTOR	The Pastor of a Church	\N	t	f	\N	\N	\N	\N	\N	\N	\N
POTENT	Potential Applicant	\N	t	f	\N	\N	\N	\N	\N	\N	\N
RE-JOINER	An Ex-Worker rejoining us	\N	t	f	\N	\N	\N	\N	\N	\N	\N
REFERENCE	A Reference for another partner	\N	t	f	\N	\N	\N	\N	\N	\N	\N
SCH-BIBLE	Bible School/College	\N	t	f	\N	\N	\N	\N	\N	\N	\N
SCHOOL	A School	\N	t	f	\N	\N	\N	\N	\N	\N	\N
SUPPLIER	Supplier of Goods or Services (used by AP)	\N	t	f	\N	\N	\N	\N	\N	\N	\N
TRUST	A Trustee	\N	t	f	\N	\N	\N	\N	\N	\N	\N
VENDOR	Supplier of Goods or services to us	\N	t	f	\N	\N	\N	\N	\N	\N	\N
VOLUNTEER	Short term or untrained worker or helper.	\N	t	f	\N	\N	\N	\N	\N	\N	\N
A/C	Area Coordinator	\N	t	f	\N	\N	\N	\N	\N	\N	\N
ACF	Area Communications Facilitator	\N	t	f	\N	\N	\N	\N	\N	\N	\N
AFDF	Area Financial Development Facilitator	\N	t	f	\N	\N	\N	\N	\N	\N	\N
AFO	Area Finance Officer	\N	t	f	\N	\N	\N	\N	\N	\N	\N
AITA	Area Information Technology Administrator	\N	t	f	\N	\N	\N	\N	\N	\N	\N
APO	Area Personnel Officer	\N	t	f	\N	\N	\N	\N	\N	\N	\N
APPLIED	An Applicant	\N	t	f	\N	\N	\N	\N	\N	\N	\N
APPLIED-NN	A non-national Applicant	\N	t	f	\N	\N	\N	\N	\N	\N	\N
ASSOC	Associate Member of our organisation	\N	t	f	\N	\N	\N	\N	\N	\N	\N
ATO	Area Training Officer	\N	t	f	\N	\N	\N	\N	\N	\N	\N
BANK	Bank	\N	t	f	\N	\N	\N	\N	\N	\N	\N
BOARD	Board member	\N	t	f	\N	\N	\N	\N	\N	\N	\N
BOOKSHOP	Bookshop or place selling books	\N	t	f	\N	\N	\N	\N	\N	\N	\N
C/L	Country Leader	\N	t	f	\N	\N	\N	\N	\N	\N	\N
TBDXYZ	TBDXYZ	\N	t	f	\N	\N	\N	\N	\N	\N	\N
CANCEL-APL	An Applicant who cancelled	\N	t	f	\N	\N	\N	\N	\N	\N	\N
CHARITY	Charity (non profit organization)	\N	t	f	\N	\N	\N	\N	\N	\N	\N
CHURCH	Church	\N	t	f	\N	\N	\N	\N	\N	\N	\N
CHURCH-STAFF	Church of a Worker	\N	t	f	\N	\N	\N	\N	\N	\N	\N
CORP	Company, Corporation or Business	\N	t	f	\N	\N	\N	\N	\N	\N	\N
COSTCENTRE	Partner is linked to a cost centre	\N	t	f	\N	\N	\N	\N	\N	\N	\N
CREDITOR	Creditor in the Petra Accounts Subsystem	\N	t	f	\N	\N	\N	\N	\N	\N	\N
DEBTOR	Debtor in the Petra Accounts Subsystem	\N	t	f	\N	\N	\N	\N	\N	\N	\N
ESTATE	Estate of a deceased Partner	\N	t	f	\N	\N	\N	\N	\N	\N	\N
EX-STAFF	Formerly a long-term member of us	\N	t	f	\N	\N	\N	\N	\N	\N	\N
EX-STAFF-YP	Formerly a year programme member of us	\N	t	f	\N	\N	\N	\N	\N	\N	\N
EX-STAFF-NN	A non-national, previously a long-termer with us	\N	t	f	\N	\N	\N	\N	\N	\N	\N
EX-STAFF-ST	Attended a short term campaign	\N	t	f	\N	\N	\N	\N	\N	\N	\N
EX-POTENT	Potential applicant but did not apply	\N	t	f	\N	\N	\N	\N	\N	\N	\N
EX-VOLUNT	Previously a volunteer worker or helper	\N	t	f	\N	\N	\N	\N	\N	\N	\N
F/L	Field Leader	\N	t	f	\N	\N	\N	\N	\N	\N	\N
FIELD	A Field within our organisation	\N	t	f	\N	\N	\N	\N	\N	\N	\N
FOUNDATION	A Foundation	\N	t	f	\N	\N	\N	\N	\N	\N	\N
FUND	Fund	\N	t	f	\N	\N	\N	\N	\N	\N	\N
GROUP	Group of people	\N	t	f	\N	\N	\N	\N	\N	\N	\N
HOST	This partner hosts/mc's meetings, conventions, etc.	\N	t	f	\N	\N	\N	\N	\N	\N	\N
LEDGER	General Ledger Number	\N	t	f	\N	\N	\N	\N	\N	\N	\N
NEAR-RELAT	Nearest Relative (of a Worker)	\N	t	f	\N	\N	\N	\N	\N	\N	\N
STAFF-KID	Child of a worker	\N	t	f	\N	\N	\N	\N	\N	\N	\N
OFFICE	One of our offices, teams or similar	\N	t	f	\N	\N	\N	\N	\N	\N	\N
SUPPORTER	Supports our work	\N	t	f	\N	\N	\N	\N	\N	\N	\N
STAFF	Member of one of our Fields or Offices	\N	t	f	\N	\N	\N	\N	\N	\N	\N
STAFF-YP	Worker on the year program	\N	t	f	\N	\N	\N	\N	\N	\N	\N
STAFF-ST	Worker on shortterm program	\N	t	f	\N	\N	\N	\N	\N	\N	\N
STAFF-GS	Worker on Technical Service	\N	t	f	\N	\N	\N	\N	\N	\N	\N
STAFF-LT	Long Termer	\N	t	f	\N	\N	\N	\N	\N	\N	\N
STAFF-NN	A Non National Worker	\N	t	f	\N	\N	\N	\N	\N	\N	\N
STAFF-PROV	Provisional Worker	\N	t	f	\N	\N	\N	\N	\N	\N	\N
ORG	Unclassified Organisation	\N	t	f	\N	\N	\N	\N	\N	\N	\N
\.


--
-- Data for Name: a_ar_discount; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_discount (a_ar_discount_code_c, a_ar_date_valid_from_d, a_ar_adhoc_l, a_active_l, a_ar_discount_percentage_n, a_ar_discount_absolute_n, a_ar_absolute_amount_n, a_currency_code_c, a_ar_number_of_items_i, a_ar_minimum_number_of_items_i, a_ar_number_of_nights_i, a_ar_minimum_number_of_nights_i, a_ar_whole_room_l, a_ar_children_l, a_ar_early_booking_days_i, a_ar_early_payment_days_i, a_ar_article_code_c, p_partner_type_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ar_default_discount; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_default_discount (a_ar_category_code_c, a_ar_discount_code_c, a_ar_discount_date_valid_from_d, a_ar_date_valid_from_d, a_ar_date_valid_to_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ar_discount_per_category; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_discount_per_category (a_ar_category_code_c, a_ar_discount_code_c, a_ar_date_valid_from_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_tax_table; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_tax_table (a_ledger_number_i, a_tax_type_code_c, a_tax_rate_code_c, a_tax_valid_from_d, a_tax_rate_description_c, a_tax_rate_n, a_active_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ar_invoice; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_invoice (a_ledger_number_i, a_key_i, a_status_c, p_partner_key_n, a_date_effective_d, a_offer_i, a_taxing_c, a_special_tax_type_code_c, a_special_tax_rate_code_c, a_special_tax_valid_from_d, a_total_amount_n, a_currency_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ar_invoice_detail; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_invoice_detail (a_ledger_number_i, a_invoice_key_i, a_detail_number_i, a_ar_article_code_c, a_ar_reference_c, a_ar_number_of_item_i, a_ar_article_price_d, a_calculated_amount_n, a_currency_code_c, a_tax_type_code_c, a_tax_rate_code_c, a_tax_valid_from_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ar_invoice_detail_discount; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_invoice_detail_discount (a_ledger_number_i, a_invoice_key_i, a_detail_number_i, a_ar_discount_code_c, a_ar_discount_date_valid_from_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ar_invoice_discount; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ar_invoice_discount (a_ledger_number_i, a_invoice_key_i, a_ar_discount_code_c, a_ar_discount_date_valid_from_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_batch; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_batch (a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_batch_control_total_n, a_batch_running_total_n, a_batch_debit_total_n, a_batch_credit_total_n, a_batch_period_i, a_date_effective_d, a_date_of_entry_d, a_batch_status_c, a_last_journal_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_budget_revision; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_budget_revision (a_ledger_number_i, a_year_i, a_revision_i, a_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_budget; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_budget (a_budget_sequence_i, a_ledger_number_i, a_year_i, a_revision_i, a_cost_centre_code_c, a_account_code_c, a_budget_type_code_c, a_budget_status_l, a_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_budget_period; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_budget_period (a_budget_sequence_i, a_period_number_i, a_budget_base_n, a_budget_last_year_n, a_budget_this_year_n, a_budget_next_year_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_corporate_exchange_rate; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_corporate_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_rate_of_exchange_n, a_date_effective_from_d, a_time_effective_from_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_crdt_note_invoice_link; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_crdt_note_invoice_link (a_ledger_number_i, a_credit_note_number_i, a_invoice_number_i, a_applied_date_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_currency_language; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_currency_language (a_currency_code_c, p_language_code_c, a_unit_label_singular_c, a_unit_label_plural_c, a_special_code_c, a_decimal_options_c, a_decimal_label_singular_c, a_decimal_label_plural_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_daily_exchange_rate; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_daily_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_rate_of_exchange_n, a_date_effective_from_d, a_time_effective_from_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_email_destination; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_email_destination (a_file_code_c, a_conditional_value_c, p_partner_key_n, p_email_address_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_motivation_group; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_motivation_group (a_ledger_number_i, a_motivation_group_code_c, a_motivation_group_description_c, a_group_status_l, a_motivation_group_desc_local_c, a_restricted_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	GIFT	Standard Gifts	t	Standard Gifts	f	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_motivation_detail; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_motivation_detail (a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c, a_motivation_detail_audience_c, a_motivation_detail_desc_c, a_account_code_c, a_cost_centre_code_c, a_motivation_status_l, a_mailing_cost_n, a_bulk_rate_flag_l, a_next_response_status_c, a_activate_partner_flag_l, a_number_sent_i, a_number_of_responses_i, a_target_number_of_responses_i, a_target_amount_n, a_amount_received_n, p_recipient_key_n, a_autopopdesc_l, a_receipt_l, a_tax_deductable_l, a_motivation_detail_desc_local_c, a_short_code_c, a_restricted_l, a_export_to_intranet_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	GIFT	FIELD	\N	Field Gift	0200	4300	t	0.0000000000	f	\N	f	0	0	0	0.0000000000	0.0000000000	0	f	t	t	Field Gift	\N	f	t	2009-06-15	\N	\N	\N	\N
43	GIFT	KEYMIN	\N	Key Ministry Gift	0400	4300	t	0.0000000000	f	\N	f	0	0	0	0.0000000000	0.0000000000	0	f	t	t	Key Ministry Gift	\N	f	t	2009-06-15	\N	\N	\N	\N
43	GIFT	PERSONAL	\N	Gift for personal use	0100	4300	t	0.0000000000	f	\N	f	0	0	0	0.0000000000	0.0000000000	0	f	t	t	Gift for personal use	\N	f	t	2009-06-15	\N	\N	\N	\N
43	GIFT	SUPPORT	\N	Support Gift	0100	4300	t	0.0000000000	f	\N	f	0	0	0	0.0000000000	0.0000000000	0	f	t	t	Support Gift	\N	f	t	2009-06-15	\N	\N	\N	\N
43	GIFT	UNDESIG	\N	Undesignated Gift	0300	4300	t	0.0000000000	f	\N	f	0	0	0	0.0000000000	0.0000000000	0	f	t	t	Undesignated Gift	\N	f	t	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_ep_account; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ep_account (a_banking_details_key_i, a_ledger_number_i, a_account_code_c, a_importfile_path_c, a_exportfile_path_c, a_plugin_filename_c, a_plugin_parameters_c, a_confidential_gift_flag_l, a_tax_deductable_l, a_motivation_group_code_c, a_motivation_detail_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ep_payment; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ep_payment (a_ledger_number_i, a_payment_number_i, a_amount_n, s_user_id_c, a_reference_c, a_bank_account_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ep_document_payment; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ep_document_payment (a_ledger_number_i, a_ap_number_i, a_payment_number_i, a_amount_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_method_of_giving; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_method_of_giving (a_method_of_giving_code_c, a_method_of_giving_desc_c, a_trust_flag_l, a_tax_rebate_flag_l, a_recurring_method_flag_l, a_active_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_method_of_payment; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_method_of_payment (a_method_of_payment_code_c, a_method_of_payment_desc_c, a_method_of_payment_type_c, a_process_to_call_c, a_special_method_of_pmt_l, a_active_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_mailing; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_mailing (p_mailing_code_c, p_mailing_description_c, p_mailing_date_d, a_motivation_group_code_c, a_motivation_detail_code_c, p_mailing_cost_n, p_mailing_attributed_amount_n, p_viewable_l, p_viewable_until_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: u_unit_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY u_unit_type (u_unit_type_code_c, u_unit_type_name_c, p_type_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
A	Area	f	\N	\N	\N	\N	\N
C	Country	f	\N	\N	\N	\N	\N
F	Field	f	\N	\N	\N	\N	\N
CONF	Conference	f	\N	\N	\N	\N	\N
XYZ	To be discussed	f	\N	\N	\N	\N	\N
KEY-MIN	Key Ministry	f	\N	\N	\N	\N	\N
O	Other	f	\N	\N	\N	\N	\N
R	Root	f	\N	\N	\N	\N	\N
D	Special Fund	f	\N	\N	\N	\N	\N
T	Team	f	\N	\N	\N	\N	\N
W	Working Group	f	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_unit; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_unit (p_partner_key_n, p_unit_name_c, p_description_c, u_unit_type_code_c, um_minimum_i, um_maximum_i, um_present_i, um_part_timers_i, p_xyz_tbd_code_c, p_xyz_tbd_cost_n, p_xyz_tbd_cost_currency_code_c, p_country_code_c, p_primary_office_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43000000	Germany	\N	F	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	\N	\N	\N	\N
1000000	The Organisation	\N	R	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	\N	\N	\N	\N
4000000	International Clearing House	\N	D	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	\N	\N	\N	\N
95000000	Global Impact Fund	\N	D	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	\N	\N	\N	\N
35000000	Switzerland	\N	F	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	\N	\N	\N	\N
73000000	Kenya	\N	F	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	\N	\N	\N	\N
67000000	Austria	\N	F	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd0
1800501	Feed The Hungry	\N	KEY-MIN	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd4
1800502	Free The Addicted	\N	KEY-MIN	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd8
1800503	Heal The Injured	\N	KEY-MIN	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bdc
68000000	Australia	\N	F	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be0
1800504	Save The Forest	\N	KEY-MIN	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be4
1800505	Rescue the Dolphins	\N	KEY-MIN	0	0	0	0	\N	0.0000000000	\N	99	0	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be8
\.


--
-- Data for Name: a_ep_match; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ep_match (a_ep_match_key_i, a_match_text_c, a_detail_i, a_action_c, a_recent_match_d, a_ledger_number_i, a_recipient_ledger_number_n, a_motivation_group_code_c, a_motivation_detail_code_c, a_comment_one_type_c, a_gift_comment_one_c, a_confidential_gift_flag_l, a_tax_deductable_l, p_recipient_key_n, a_charge_flag_l, a_cost_centre_code_c, p_mailing_code_c, a_comment_two_type_c, a_gift_comment_two_c, a_comment_three_type_c, a_gift_comment_three_c, a_gift_transaction_amount_n, a_home_admin_charges_flag_l, a_ilt_admin_charges_flag_l, a_receipt_letter_code_c, a_method_of_giving_code_c, a_method_of_payment_code_c, p_donor_key_n, a_admin_charge_l, a_narrative_c, a_reference_c, p_donor_short_name_c, p_recipient_short_name_c, a_restricted_l, a_account_code_c, a_key_ministry_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ep_statement; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ep_statement (a_statement_key_i, a_bank_account_key_i, a_ledger_number_i, a_bank_account_code_c, a_date_d, a_id_from_bank_c, a_filename_c, a_end_balance_n, a_currency_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ep_transaction; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ep_transaction (a_statement_key_i, a_order_i, a_detail_key_i, a_number_on_paper_statement_i, a_match_text_c, a_account_name_c, a_title_c, a_first_name_c, a_middle_name_c, a_last_name_c, p_branch_code_c, p_bic_c, a_bank_account_number_c, a_iban_c, a_transaction_type_code_c, a_transaction_amount_n, a_description_c, a_date_effective_d, a_ep_match_key_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_fees_payable; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_fees_payable (a_ledger_number_i, a_fee_code_c, a_charge_option_c, a_charge_percentage_n, a_charge_amount_n, a_cost_centre_code_c, a_account_code_c, a_fee_description_c, a_dr_account_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_fees_receivable; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_fees_receivable (a_ledger_number_i, a_fee_code_c, a_charge_option_c, a_charge_percentage_n, a_charge_amount_n, a_cost_centre_code_c, a_account_code_c, a_fee_description_c, a_dr_account_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_form; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_form (a_form_code_c, a_form_name_c, a_form_description_c, a_form_type_code_c, a_number_of_details_i, a_print_in_bold_l, a_lines_on_page_i, a_minimum_amount_n, a_options_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_form_element_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_form_element_type (a_form_code_c, a_form_element_type_code_c, a_form_element_type_desc_c, a_default_length_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_form_element; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_form_element (a_form_code_c, a_form_name_c, a_form_sequence_i, a_form_element_type_code_c, a_column_i, a_row_i, a_length_i, a_skip_i, a_when_print_c, a_literal_text_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_frequency; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_frequency (a_frequency_code_c, a_frequency_description_c, a_number_of_years_i, a_number_of_months_i, a_number_of_days_i, a_number_of_hours_i, a_number_of_minutes_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
Annual	Every Year	1	0	0	0	0	\N	\N	\N	\N	\N
Bi-Monthly	Every Two Months	0	2	0	0	0	\N	\N	\N	\N	\N
Daily	Every Day	0	0	1	0	0	\N	\N	\N	\N	\N
Monthly	Every Month	0	1	0	0	0	\N	\N	\N	\N	\N
Quarterly	Every Three Months	0	3	0	0	0	\N	\N	\N	\N	\N
Semi-Annual	Every Six Months	0	6	0	0	0	\N	\N	\N	\N	\N
Weekly	Every Week	0	0	7	0	0	\N	\N	\N	\N	\N
\.


--
-- Data for Name: a_general_ledger_master; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_general_ledger_master (a_glm_sequence_i, a_ledger_number_i, a_year_i, a_account_code_c, a_cost_centre_code_c, a_ytd_actual_base_n, a_closing_period_actual_base_n, a_start_balance_base_n, a_ytd_actual_intl_n, a_closing_period_actual_intl_n, a_start_balance_intl_n, a_ytd_actual_foreign_n, a_start_balance_foreign_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_general_ledger_master_period; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_general_ledger_master_period (a_glm_sequence_i, a_period_number_i, a_actual_base_n, a_budget_base_n, a_actual_intl_n, a_budget_intl_n, a_actual_foreign_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_gift_batch; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_gift_batch (a_ledger_number_i, a_batch_number_i, a_batch_description_c, s_modification_date_d, a_hash_total_n, a_batch_total_n, a_bank_account_code_c, a_last_gift_number_i, a_batch_status_c, a_batch_period_i, a_batch_year_i, a_gl_effective_date_d, a_currency_code_c, a_exchange_rate_to_base_n, a_bank_cost_centre_c, a_gift_type_c, a_method_of_payment_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_gift; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_gift (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, a_gift_status_c, a_date_entered_d, a_home_admin_charges_flag_l, a_ilt_admin_charges_flag_l, a_receipt_letter_code_c, a_method_of_giving_code_c, a_method_of_payment_code_c, p_donor_key_n, a_admin_charge_l, a_receipt_number_i, a_last_detail_number_i, a_reference_c, a_first_time_gift_l, a_receipt_printed_l, a_restricted_l, p_banking_details_key_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_gift_detail; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_gift_detail (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, a_detail_number_i, a_recipient_ledger_number_n, a_gift_amount_n, a_motivation_group_code_c, a_motivation_detail_code_c, a_comment_one_type_c, a_gift_comment_one_c, a_confidential_gift_flag_l, a_tax_deductable_l, p_recipient_key_n, a_charge_flag_l, a_cost_centre_code_c, a_gift_amount_intl_n, a_modified_detail_l, a_gift_transaction_amount_n, a_ich_number_i, p_mailing_code_c, a_comment_two_type_c, a_gift_comment_two_c, a_comment_three_type_c, a_gift_comment_three_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ich_stewardship; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ich_stewardship (a_ledger_number_i, a_period_number_i, a_ich_number_i, a_cost_centre_code_c, a_date_processed_d, a_income_amount_n, a_expense_amount_n, a_direct_xfer_amount_n, a_income_amount_intl_n, a_expense_amount_intl_n, a_direct_xfer_amount_intl_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_sub_system; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_sub_system (a_sub_system_code_c, a_sub_system_name_c, a_setup_sub_system_process_c, a_sub_system_to_call_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
AP	Accounts Payable			\N	\N	\N	\N	\N
GL	General Ledger			\N	\N	\N	\N	\N
GR	Gift Receipting			\N	\N	\N	\N	\N
\.


--
-- Data for Name: a_transaction_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_transaction_type (a_ledger_number_i, a_sub_system_code_c, a_transaction_type_code_c, a_debit_account_code_c, a_credit_account_code_c, a_last_journal_i, a_last_recurring_journal_i, a_transaction_type_description_c, a_balancing_account_code_c, a_special_transaction_type_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	AP	INV	BAL SHT	CRS CTRL	0	0	Input Creditor's Invoice		t	2009-06-15	\N	\N	\N	\N
43	GL	ALLOC	BAL SHT	BAL SHT	0	0	Allocation Journal		t	2009-06-15	\N	\N	\N	\N
43	GL	REALLOC	BAL SHT	BAL SHT	0	0	Reallocation Journal		t	2009-06-15	\N	\N	\N	\N
43	GL	REVAL	5003	5003	0	0	Foreign Exchange Revaluation		t	2009-06-15	\N	\N	\N	\N
43	GL	STD	BAL SHT	BAL SHT	0	0	Standard Journal		f	2009-06-15	\N	\N	\N	\N
43	GR	GR	CASH	GIFT	0	0	Gift Receipting		t	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_journal; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_journal_debit_total_n, a_journal_credit_total_n, a_journal_period_i, a_date_effective_d, a_transaction_type_code_c, a_last_transaction_number_i, a_sub_system_code_c, a_journal_status_c, a_transaction_currency_c, a_exchange_rate_to_base_n, a_exchange_rate_time_i, a_date_of_entry_d, a_reversed_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_ledger_init_flag; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_ledger_init_flag (a_ledger_number_i, a_init_option_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_motivation_detail_fee; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_motivation_detail_fee (a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c, a_fee_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_prev_year_corp_ex_rate; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_prev_year_corp_ex_rate (a_from_currency_code_c, a_to_currency_code_c, a_rate_of_exchange_n, a_date_effective_from_d, a_time_effective_from_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_previous_year_batch; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_previous_year_batch (a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_batch_control_total_n, a_batch_running_total_n, a_batch_debit_total_n, a_batch_credit_total_n, a_batch_period_i, a_batch_year_i, a_date_effective_d, a_date_of_entry_d, a_batch_status_c, a_last_journal_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_previous_year_journal; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_previous_year_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_journal_debit_total_n, a_journal_credit_total_n, a_journal_period_i, a_date_effective_d, a_transaction_type_code_c, a_last_transaction_number_i, a_sub_system_code_c, a_journal_status_c, a_transaction_currency_c, a_base_currency_c, a_exchange_rate_to_base_n, a_exchange_rate_time_i, a_reversed_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_previous_year_transaction; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_previous_year_transaction (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_transaction_number_i, a_account_code_c, a_primary_account_code_c, a_cost_centre_code_c, a_primary_cost_centre_code_c, a_transaction_date_d, a_transaction_amount_n, a_amount_in_base_currency_n, a_analysis_indicator_l, a_reconciled_status_l, a_narrative_c, a_debit_credit_indicator_l, a_transaction_status_l, a_header_number_i, a_detail_number_i, a_sub_type_c, a_to_ilt_flag_l, a_source_flag_l, a_reference_c, a_source_reference_c, a_system_generated_l, a_amount_in_intl_currency_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_prev_year_trans_anal_attrib; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_prev_year_trans_anal_attrib (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_transaction_number_i, a_account_code_c, a_cost_centre_code_c, a_analysis_type_code_c, a_analysis_attribute_value_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_processed_fee; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_processed_fee (a_ledger_number_i, a_fee_code_c, a_cost_centre_code_c, a_period_number_i, a_periodic_amount_n, a_batch_number_i, a_gift_transaction_number_i, a_detail_number_i, a_processed_date_d, s_timestamp_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_recurring_batch; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_recurring_batch (a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_batch_control_total_n, a_batch_status_c, a_batch_running_total_n, a_batch_debit_total_n, a_batch_credit_total_n, a_batch_period_i, a_date_effective_d, s_user_id_c, a_date_of_entry_d, a_frequency_code_c, a_date_batch_last_run_d, a_last_journal_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_recurring_gift_batch; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_recurring_gift_batch (a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_hash_total_n, a_batch_total_n, a_bank_account_code_c, a_last_gift_number_i, a_currency_code_c, a_bank_cost_centre_c, a_gift_type_c, a_method_of_payment_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_recurring_gift; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_recurring_gift (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, a_receipt_letter_code_c, a_method_of_giving_code_c, a_method_of_payment_code_c, p_donor_key_n, a_last_detail_number_i, a_reference_c, p_banking_details_key_i, a_charge_status_c, a_last_debit_d, a_debit_day_i, a_active_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_recurring_gift_detail; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_recurring_gift_detail (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, a_detail_number_i, a_recipient_ledger_number_n, a_gift_amount_n, a_motivation_group_code_c, a_motivation_detail_code_c, a_comment_one_type_c, a_gift_comment_one_c, a_confidential_gift_flag_l, a_tax_deductable_l, p_recipient_key_n, a_charge_flag_l, p_mailing_code_c, a_comment_two_type_c, a_gift_comment_two_c, a_comment_three_type_c, a_gift_comment_three_c, a_start_donations_d, a_end_donations_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_recurring_journal; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_recurring_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_journal_status_c, a_journal_debit_total_n, a_journal_credit_total_n, a_journal_period_i, a_date_effective_d, a_transaction_type_code_c, a_method_of_payment_code_c, a_last_transaction_number_i, a_sub_system_code_c, a_exchange_rate_to_base_n, a_transaction_currency_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_recurring_transaction; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_recurring_transaction (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_transaction_number_i, a_account_code_c, a_cost_centre_code_c, a_transaction_date_d, a_transaction_currency_c, a_transaction_amount_n, a_base_currency_c, a_exchange_rate_to_base_n, a_amount_in_base_currency_n, a_analysis_indicator_l, a_method_of_payment_code_c, a_period_number_i, a_reconciled_flag_l, a_sub_system_code_c, a_transaction_type_code_c, a_narrative_c, a_reference_c, a_date_of_entry_d, s_user_id_c, a_debit_credit_indicator_l, a_transaction_status_l, a_header_number_i, a_detail_number_i, a_sub_type_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_recurring_trans_anal_attrib; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_recurring_trans_anal_attrib (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_transaction_number_i, a_account_code_c, a_cost_centre_code_c, a_analysis_type_code_c, a_analysis_attribute_value_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_special_trans_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_special_trans_type (a_sub_system_code_c, a_transaction_type_code_c, a_transaction_type_description_c, a_spec_trans_process_to_call_c, a_spec_trans_undo_process_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_suspense_account; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_suspense_account (a_ledger_number_i, a_suspense_account_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_system_interface; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_system_interface (a_ledger_number_i, a_sub_system_code_c, a_set_up_complete_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43	AP	t	2009-06-15	\N	\N	\N	\N
43	GL	t	2009-06-15	\N	\N	\N	\N
43	GR	t	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: a_this_year_old_batch; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_this_year_old_batch (a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_batch_control_total_n, a_batch_running_total_n, a_batch_debit_total_n, a_batch_credit_total_n, a_batch_period_i, a_date_effective_d, a_date_of_entry_d, a_batch_status_c, a_last_journal_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_this_year_old_journal; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_this_year_old_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_journal_debit_total_n, a_journal_credit_total_n, a_journal_period_i, a_date_effective_d, a_transaction_type_code_c, a_last_transaction_number_i, a_sub_system_code_c, a_journal_status_c, a_transaction_currency_c, a_base_currency_c, a_exchange_rate_to_base_n, a_exchange_rate_time_i, a_reversed_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_this_year_old_transaction; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_this_year_old_transaction (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_transaction_number_i, a_account_code_c, a_primary_account_code_c, a_cost_centre_code_c, a_primary_cost_centre_code_c, a_transaction_date_d, a_transaction_amount_n, a_amount_in_base_currency_n, a_analysis_indicator_l, a_reconciled_status_l, a_narrative_c, a_debit_credit_indicator_l, a_transaction_status_l, a_header_number_i, a_detail_number_i, a_sub_type_c, a_to_ilt_flag_l, a_source_flag_l, a_reference_c, a_source_reference_c, a_system_generated_l, a_amount_in_intl_currency_n, a_ich_number_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_thisyearold_trans_anal_attrib; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_thisyearold_trans_anal_attrib (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_transaction_number_i, a_account_code_c, a_cost_centre_code_c, a_analysis_type_code_c, a_analysis_attribute_value_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_transaction; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_transaction (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_transaction_number_i, a_account_code_c, a_primary_account_code_c, a_cost_centre_code_c, a_primary_cost_centre_code_c, a_transaction_date_d, a_transaction_amount_n, a_amount_in_base_currency_n, a_analysis_indicator_l, a_reconciled_status_l, a_narrative_c, a_debit_credit_indicator_l, a_transaction_status_l, a_header_number_i, a_detail_number_i, a_sub_type_c, a_to_ilt_flag_l, a_source_flag_l, a_reference_c, a_source_reference_c, a_system_generated_l, a_amount_in_intl_currency_n, a_ich_number_i, a_key_ministry_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_trans_anal_attrib; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_trans_anal_attrib (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_transaction_number_i, a_account_code_c, a_cost_centre_code_c, a_analysis_type_code_c, a_analysis_attribute_value_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: a_valid_ledger_number; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY a_valid_ledger_number (p_partner_key_n, a_ledger_number_i, a_ilt_processing_centre_n, a_cost_centre_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
4000000	43	4000000	0400	2009-06-15	\N	\N	\N	\N
35000000	43	4000000	3500	2009-06-15	\N	\N	\N	\N
73000000	43	4000000	7300	2009-06-15	\N	\N	\N	\N
95000000	43	4000000	9500	2009-06-15	\N	\N	\N	\N
\.


--
-- Data for Name: m_extract_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY m_extract_type (m_code_c, m_function_c, m_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: m_extract_master; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY m_extract_master (m_extract_id_i, m_extract_name_c, m_extract_desc_c, m_last_ref_d, m_deletable_l, m_frozen_l, m_key_count_i, m_public_l, m_manual_mod_l, m_manual_mod_d, m_manual_mod_by_c, m_extract_type_code_c, m_template_l, m_restricted_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_location; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_location (p_site_key_n, p_location_key_i, p_building_1_c, p_building_2_c, p_street_name_c, p_locality_c, p_suburb_c, p_city_c, p_county_c, p_postal_code_c, p_country_code_c, p_address_3_c, p_geo_latitude_n, p_geo_longitude_n, p_geo_km_x_i, p_geo_km_y_i, p_geo_accuracy_i, p_restricted_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
0	0	\N	\N	No valid address on file	\N	\N	\N	\N	\N	99	\N	0.000000	0.000000	0	0	-1	f	1998-01-01	SYSADMIN	\N	\N	\N
43000000	0	\N	\N	No valid address on file	\N	\N	\N	\N	\N	99	\N	\N	\N	\N	\N	-1	f	2011-01-11	\N	\N	\N	\N
0	27	\N	\N	2047 Proin Rd.	\N	\N	Ponce	\N	CM0 8RI	DE	\N	\N	\N	\N	\N	-1	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000b9f
0	28	\N	\N	279-8779 Nec Rd.	\N	\N	Valdez	\N	K1B 0GO	DE	\N	\N	\N	\N	\N	-1	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000ba6
0	29	\N	\N	Ap #279-4130 Nunc St.	\N	\N	Huntington Beach	\N	2987PU	DE	\N	\N	\N	\N	\N	-1	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bad
0	30	\N	\N	788-4853 Eu Street	\N	\N	Clairton	\N	6814KU	DE	\N	\N	\N	\N	\N	-1	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bb3
0	31	\N	\N	8487 Rutrum. Rd.	\N	\N	Columbus	\N	NR8 4BT	DE	\N	\N	\N	\N	\N	-1	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bb9
0	32	\N	\N	Ap #975-7000 Congue Av.	\N	\N	Alexandria	\N	9709AE	DE	\N	\N	\N	\N	\N	-1	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bbf
0	33	\N	\N	Ap #975-7000 Congue Av.	\N	\N	Alexandria	\N	9709AE	DE	\N	\N	\N	\N	\N	-1	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bc5
0	34	\N	\N	P.O. Box 102, 8566 Justo. Road	\N	\N	Akron	\N	5656CN	DE	\N	\N	\N	\N	\N	-1	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bca
0	35	\N	\N	Hauptstrasse 1	\N	\N	Wien	\N	192034	AT	\N	\N	\N	\N	\N	-1	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bce
\.


--
-- Data for Name: m_extract; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY m_extract (m_extract_id_i, p_partner_key_n, p_site_key_n, p_location_key_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: m_extract_parameter; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY m_extract_parameter (m_extract_id_i, m_parameter_code_c, m_value_index_i, m_parameter_value_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_process; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_process (p_process_code_c, p_process_descr_c, p_process_partner_classes_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_action; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_action (p_process_code_c, p_action_code_c, p_action_descr_c, p_active_l, p_system_action_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_address_element; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_address_element (p_address_element_code_c, p_address_element_description_c, p_address_element_field_name_c, p_address_element_text_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_address_layout_code; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_address_layout_code (p_code_c, p_description_c, p_display_index_i, p_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_address_layout; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_address_layout (p_country_code_c, p_address_layout_code_c, p_address_line_number_i, p_address_line_code_c, p_address_prompt_c, p_locked_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_address_line; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_address_line (p_address_line_code_c, p_address_element_position_i, p_address_element_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_addressee_title_override; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_addressee_title_override (p_language_code_c, p_title_c, p_title_override_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_banking_details_usage_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_banking_details_usage_type (p_type_c, p_type_description_c, pc_unassignable_flag_l, pc_unassignable_date_d, pc_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_banking_details; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_banking_details (p_partner_key_n, p_banking_details_key_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_banking_details_usage; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_banking_details_usage (p_partner_key_n, p_banking_details_key_i, p_type_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_business; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_business (p_business_code_c, p_business_description_c, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
AFF	Agriculture, forestry and fishing	f	\N	\N	\N	\N	\N
BANKING	Banking, financial services	f	\N	\N	\N	\N	\N
COMMS	Communications - telephone, fax, email	f	\N	\N	\N	\N	\N
COMPUTER	Computer Manufacturer	f	\N	\N	\N	\N	\N
CONSTRUC	Construction	f	\N	\N	\N	\N	\N
EDUCATN	Schools, Colleges etc	f	\N	\N	\N	\N	\N
ENGINEER	Engineering - metal goods, vehicles, electronics etc	f	\N	\N	\N	\N	\N
GOV	Government (Local or National)	f	\N	\N	\N	\N	\N
HEALTH	Doctors, hospitals	f	\N	\N	\N	\N	\N
HOTEL	Hotels, guest houses etc	f	\N	\N	\N	\N	\N
INSURANC	Insurance industry	f	\N	\N	\N	\N	\N
MANUFACT	Other manufacturing indutries	f	\N	\N	\N	\N	\N
MINE	Mining, chemicals, metals and minerals	f	\N	\N	\N	\N	\N
MISSION	Missionary Organisation	f	\N	\N	\N	\N	\N
OTHER	Anything not otherwise represented	f	\N	\N	\N	\N	\N
P-CHURCH	Para-Church Organisation	f	\N	\N	\N	\N	\N
PRINTER	Printer	f	\N	\N	\N	\N	\N
RETAIL	Shops and stores	f	\N	\N	\N	\N	\N
SHIPPER	Post Office/Overnight/2-4 Class Shipping	f	\N	\N	\N	\N	\N
SUPPLIER	Office/Business/Miscellaneous supplies	f	\N	\N	\N	\N	\N
TRANSPRT	Transport operators, Distribution	f	\N	\N	\N	\N	\N
UNKNOWN	Unknown	f	\N	\N	\N	\N	\N
UTILITY	Energy and Water Supply	f	\N	\N	\N	\N	\N
WHITEGDS	Electrical Consumer products	f	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_denomination; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_denomination (p_denomination_code_c, p_denomination_name_c, p_valid_denomination_l, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_church; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_church (p_partner_key_n, p_church_name_c, p_approximate_size_i, p_denomination_code_c, p_accomodation_l, p_prayer_group_l, p_map_on_file_l, p_accomodation_type_c, p_accomodation_size_i, p_contact_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_contact_attribute; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_contact_attribute (p_contact_attribute_code_c, p_contact_attribute_descr_c, p_active_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_contact_attribute_detail; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_contact_attribute_detail (p_contact_attribute_code_c, p_contact_attr_detail_code_c, p_contact_attr_detail_descr_c, p_active_l, p_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_customised_greeting; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_customised_greeting (p_partner_key_n, s_user_id_c, p_customised_greeting_text_c, p_customised_closing_text_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_data_label_lookup_category; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_data_label_lookup_category (p_category_code_c, p_category_desc_c, p_extendable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_data_label; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_data_label (p_key_i, p_text_c, p_group_c, p_description_c, p_data_type_c, p_char_length_i, p_num_decimal_places_i, p_currency_code_c, p_lookup_category_code_c, p_entry_mandatory_l, p_displayed_l, p_not_displayed_from_d, p_editable_l, p_not_editable_from_d, p_restricted_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_data_label_lookup; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_data_label_lookup (p_category_code_c, p_value_code_c, p_value_desc_c, p_active_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_data_label_use; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_data_label_use (p_data_label_key_i, p_use_c, p_idx1_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_marital_status; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_marital_status (pt_code_c, pt_description_c, pt_assignable_flag_l, pt_assignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
C	Child (under 18 years, with family)	t	\N	f	\N	\N	\N	\N	\N
D	Divorced	t	\N	f	\N	\N	\N	\N	\N
E	Engaged to be married	t	\N	f	\N	\N	\N	\N	\N
H	Serious Relationship	t	\N	f	\N	\N	\N	\N	\N
M	Married	t	\N	f	\N	\N	\N	\N	\N
N	Never married (single)	t	\N	f	\N	\N	\N	\N	\N
R	Remarried	t	\N	f	\N	\N	\N	\N	\N
S	Separated	t	\N	f	\N	\N	\N	\N	\N
U	Unknown	t	\N	f	\N	\N	\N	\N	\N
W	Widowed	t	\N	f	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_family; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_family (p_partner_key_n, p_family_members_l, p_title_c, p_first_name_c, p_family_name_c, p_different_surnames_l, p_field_key_n, p_marital_status_c, p_marital_status_since_d, p_marital_status_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43005001	f		Dalton	Lloyd	f	\N	U	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000ba1
43005002	f		Julian	Hart	f	\N	U	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000ba8
43005003	f		Dahlia	Pope	f	\N	U	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000baf
43005004	f		Ulric	Harvey	f	\N	U	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bb5
43005005	f		Julian	Ingram	f	\N	U	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bbb
43005006	f		Robert	Parrish	f	\N	U	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bc1
43005007	f		Otto	Oneal	f	\N	U	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bc7
\.


--
-- Data for Name: p_occupation; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_occupation (p_occupation_code_c, p_occupation_description_c, p_valid_occupation_l, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_person; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_person (p_partner_key_n, p_title_c, p_first_name_c, p_prefered_name_c, p_middle_name_1_c, p_middle_name_2_c, p_middle_name_3_c, p_family_name_c, p_decorations_c, p_date_of_birth_d, p_gender_c, p_marital_status_c, p_occupation_code_c, p_believer_since_year_i, p_believer_since_comment_c, p_family_key_n, p_family_id_i, p_field_key_n, p_academic_title_c, p_marital_status_since_d, p_marital_status_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_applicant_status; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_applicant_status (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_application_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_application_type (pt_app_type_name_c, pt_app_type_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_app_form_type_c, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_contact; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_contact (pt_contact_name_c, pt_contact_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_general_application; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_general_application (p_partner_key_n, pm_application_key_i, pm_registration_office_n, pm_gen_app_date_d, pt_app_type_name_c, pm_old_link_c, pm_gen_app_poss_srv_unit_key_n, pm_gen_app_delete_flag_l, pm_gen_applicant_type_c, pm_gen_application_status_c, pm_closed_l, pm_closed_by_c, pm_date_closed_d, pm_gen_application_on_hold_l, pm_gen_application_hold_reason_c, pm_gen_cancelled_app_l, pm_gen_app_cancel_reason_c, pm_gen_app_cancelled_d, pm_gen_app_srv_fld_accept_l, pm_gen_app_recvg_fld_accept_d, pm_gen_app_send_fld_accept_l, pm_gen_app_send_fld_accept_d, pm_gen_contact1_c, pm_gen_contact2_c, pm_gen_app_update_d, pm_gen_year_program_c, pm_comment_c, pm_gen_app_currency_code_c, pm_placement_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_data_label_value_application; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_data_label_value_application (p_partner_key_n, pm_application_key_i, pm_registration_office_n, p_data_label_key_i, p_value_char_c, p_value_num_n, p_value_currency_n, p_value_int_i, p_value_bool_l, p_value_date_d, p_value_time_i, p_value_partner_key_n, p_value_lookup_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_data_label_value_partner; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_data_label_value_partner (p_partner_key_n, p_data_label_key_i, p_value_char_c, p_value_num_n, p_value_currency_n, p_value_int_i, p_value_bool_l, p_value_date_d, p_value_time_i, p_value_partner_key_n, p_value_lookup_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_email; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_email (p_email_address_c, p_description_c, p_valid_l, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_volume; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_volume (s_name_c, s_parent_volume_name_c, s_path_c, s_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_file_info; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_file_info (p_key_n, p_partner_key_n, s_volume_name_c, p_path_c, p_file_name_c, p_name_c, p_file_type_c, p_comment_c, p_restricted_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_form_letter_body; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_form_letter_body (p_body_name_c, p_body_text_c, p_description_c, p_physical_file_c, p_owner_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_form_letter_design; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_form_letter_design (p_design_name_c, p_description_c, p_address_layout_code_c, p_formality_level_i, p_body_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_form_letter_insert; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_form_letter_insert (p_sequence_i, p_partner_key_n, m_extract_id_i, p_body_name_c, p_insert_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_formality; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_formality (p_language_code_c, p_country_code_c, p_addressee_type_code_c, p_formality_level_i, p_salutation_text_c, p_title_c, p_complimentary_closing_text_c, p_personal_pronoun_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_organisation; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_organisation (p_partner_key_n, p_organisation_name_c, p_business_code_c, p_religious_l, p_contact_partner_key_n, p_foundation_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43005008	IT Hardwarestore	UNKNOWN	f	0	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bcc
\.


--
-- Data for Name: p_proposal_submission_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_proposal_submission_type (p_submission_type_code_c, p_submission_type_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_foundation; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_foundation (p_partner_key_n, p_owner_1_key_n, p_owner_2_key_n, p_key_contact_name_c, p_key_contact_title_c, p_key_contact_email_c, p_key_contact_phone_c, p_contact_partner_n, p_special_requirements_c, p_proposal_formatting_c, p_proposal_submission_type_c, p_special_instructions_c, p_review_frequency_c, p_submit_frequency_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_foundation_deadline; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_foundation_deadline (p_foundation_partner_key_n, p_foundation_deadline_key_i, p_deadline_month_i, p_deadline_day_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_foundation_proposal_status; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_foundation_proposal_status (p_status_code_c, p_status_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_foundation_proposal; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_foundation_proposal (p_foundation_partner_key_n, p_foundation_proposal_key_i, p_proposal_status_c, p_proposal_notes_c, p_submitted_date_d, p_amount_requested_n, p_amount_approved_n, p_amount_granted_n, p_date_granted_d, p_partner_submitted_by_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_foundation_proposal_detail; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_foundation_proposal_detail (p_foundation_partner_key_n, p_foundation_proposal_key_i, p_proposal_detail_id_i, p_key_ministry_key_n, p_project_ledger_number_i, p_project_motivation_group_c, p_project_motivation_detail_c, p_area_partner_key_n, p_field_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_interest_category; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_interest_category (p_category_c, p_description_c, p_level_descriptions_c, p_level_range_low_i, p_level_range_high_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_interest; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_interest (p_interest_c, p_category_c, p_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_form; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_form (s_form_name_c, s_form_description_c, s_default_font_c, s_default_font_size_i, s_default_lpi_i, s_default_cpi_i, s_form_length_n, s_form_width_n, s_form_orientation_c, s_unit_of_measure_l, s_top_margin_n, s_bottom_margin_n, s_left_margin_n, s_right_margin_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_label; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_label (p_code_c, s_form_name_c, p_gap_lines_i, p_height_i, p_width_i, p_gap_columns_i, p_labels_across_i, p_labels_down_i, p_description_c, p_start_column_i, p_start_line_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_location_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_location_type (p_code_c, p_description_c, p_deletable_l, p_assignable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
BUSINESS	Business / Office	f	t	\N	\N	\N	\N	\N
CHURCH	Church	f	t	\N	\N	\N	\N	\N
COLL-UNI	College or University	f	t	\N	\N	\N	\N	\N
FIELD	Field	f	t	\N	\N	\N	\N	\N
HOLIDAY	Holiday	f	t	\N	\N	\N	\N	\N
HOME	Home address	f	t	\N	\N	\N	\N	\N
MOBILE	Mobile	f	t	\N	\N	\N	\N	\N
OTHER	Other	f	t	\N	\N	\N	\N	\N
PASTOR	Pastor / Minister	f	t	\N	\N	\N	\N	\N
SECRETARY	Secretary (Esp Churches)	f	t	\N	\N	\N	\N	\N
TEMPORARY	Temporary address	f	t	\N	\N	\N	\N	\N
TREASURER	Treasurer (Esp Churches)	f	t	\N	\N	\N	\N	\N
UNKNOWN	Unknown	f	t	2009-03-16	\N	\N	\N	\N
\.


--
-- Data for Name: p_merge_form; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_merge_form (p_merge_form_name_c, p_merge_form_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_merge_field; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_merge_field (p_merge_form_name_c, p_merge_field_name_c, p_merge_field_position_i, p_merge_type_c, p_merge_parameters_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_method_of_contact; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_method_of_contact (p_method_of_contact_code_c, p_description_c, p_contact_type_c, p_valid_method_l, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_module; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_module (s_module_id_c, s_module_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
CONFERENCE	Conference Module	2002-05-14	SYSADMIN	\N	\N	\N
DEVADMIN	Development Administrative Access	2009-03-16	SYSADMIN	\N	\N	\N
DEVUSER	Development User	2000-08-11	SYSADMIN	\N	\N	\N
FIN-EX-RATE	Finance Exchange Rates	2000-08-11	SYSADMIN	\N	\N	\N
FINANCE-1	Finance - Basic User	1998-01-01	SYSADMIN	1999-05-14	SYSADMIN	\N
FINANCE-2	Finance - Intermediate User	1998-01-01	SYSADMIN	1999-05-14	SYSADMIN	\N
FINANCE-3	Finance - Advanced User	1998-01-01	SYSADMIN	1999-05-14	SYSADMIN	\N
PERSONNEL	Personnel Module	1998-01-01	SYSADMIN	1999-05-14	SYSADMIN	\N
PTNRADMIN	Partner Administrative Access	1998-01-01	SYSADMIN	1999-05-14	SYSADMIN	\N
PTNRUSER	Partner User Level Access	1998-01-01	SYSADMIN	1999-05-14	SYSADMIN	\N
SYSMAN	System manager	1998-01-01	SYSADMIN	1999-05-14	SYSADMIN	\N
LEDGER0043	LEDGER0043	2011-01-11	\N	\N	\N	\N
\.


--
-- Data for Name: p_partner_contact; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_contact (p_contact_id_i, p_partner_key_n, s_contact_date_d, s_contact_time_i, p_contact_code_c, p_contactor_c, p_contact_message_id_c, p_contact_comment_c, s_module_id_c, s_user_id_c, p_mailing_code_c, p_restricted_l, p_contact_location_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_reminder_category; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_reminder_category (p_code_c, p_description_c, p_unassignable_flag_l, p_unassignable_date_d, p_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_reminder; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_reminder (p_partner_key_n, p_contact_id_i, p_reminder_id_i, s_user_id_c, p_category_code_c, p_action_type_c, p_reminder_reason_c, p_comment_c, p_event_date_d, p_first_reminder_date_d, p_reminder_frequency_i, p_last_reminder_sent_d, p_next_reminder_date_d, p_reminder_active_l, p_email_address_c, p_restricted_l, s_module_id_c, s_user_restriction_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group (s_group_id_c, s_unit_key_n, s_group_name_c, s_can_modify_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_action; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_action (p_partner_key_n, s_group_unit_key_n, p_action_number_i, p_process_code_c, p_action_code_c, p_action_freeform_c, p_perform_by_date_d, p_action_complete_l, p_action_complete_date_d, p_user_to_perform_action_c, p_group_to_perform_action_c, p_user_that_performed_action_c, p_contact_id_i, p_reminder_id_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_attribute_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_attribute_type (p_code_c, p_description_c, p_is_contact_detail_l, p_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_attribute; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_attribute (p_partner_key_n, p_code_c, p_sequence_i, p_value_c, p_comment_c, p_valid_from_d, p_valid_to_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_comment; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_comment (p_partner_key_n, p_index_i, p_sequence_i, p_comment_c, p_comment_type_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_contact_attribute; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_contact_attribute (p_contact_id_i, p_contact_attribute_code_c, p_contact_attr_detail_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_contact_file; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_contact_file (p_file_info_key_n, p_partner_key_n, p_contact_id_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_commitment_status; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_commitment_status (pm_code_c, pm_desc_c, pm_explanation_c, pm_intranet_access_l, pm_display_idx1_i, pm_unassignable_flag_l, pm_unassignable_date_d, pm_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_staff_data; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_staff_data (p_site_key_n, pm_key_n, p_partner_key_n, pm_status_code_c, pm_start_of_commitment_d, pm_start_date_approx_l, pm_end_of_commitment_d, pm_office_recruited_by_n, pm_home_office_n, pm_receiving_field_n, pm_receiving_field_office_n, pm_staff_data_comments_c, pm_job_title_c, pm_office_phone_ext_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_field_of_service; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_field_of_service (p_key_i, p_partner_key_n, p_field_key_n, p_date_effective_d, p_date_expires_d, p_active_l, p_default_gift_destination_l, p_partner_class_c, p_commitment_site_key_n, p_commitment_key_n, p_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_file; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_file (p_file_info_key_n, p_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_graphic; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_graphic (p_partner_key_n, p_file_info_key_n, p_graphic_label_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_interest; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_interest (p_partner_key_n, p_interest_number_i, p_field_key_n, p_country_c, p_interest_c, p_level_i, p_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_ledger; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_ledger (p_partner_key_n, p_last_partner_id_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43000000	5008	2011-01-11	\N	2011-01-11	DEMO	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bc8
\.


--
-- Data for Name: p_partner_location; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_location (p_partner_key_n, p_site_key_n, p_location_key_i, p_date_effective_d, p_date_good_until_d, p_location_type_c, p_send_mail_l, p_fax_number_c, p_telex_i, p_telephone_number_c, p_extension_i, p_email_address_c, p_location_detail_comment_c, p_fax_extension_i, p_mobile_number_c, p_alternate_telephone_c, p_url_c, p_restricted_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43005001	0	27	2011-01-11	\N	HOME	t	\N	0		0	Nullam.ut.nisi@elementumpurusaccumsan.com	\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000ba0
43005002	0	28	2011-01-11	\N	HOME	t	\N	0		0	dignissim.pharetra@sapienCrasdolor.org	\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000ba7
43005003	0	29	2011-01-11	\N	HOME	t	\N	0		0	mauris.blandit.mattis@ac.ca	\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bae
43005004	0	30	2011-01-11	\N	HOME	t	\N	0		0	posuere.cubilia@diameu.ca	\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bb4
43005005	0	31	2011-01-11	\N	HOME	t	\N	0		0	aliquet.Phasellus@Proinnon.com	\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bba
43005006	0	32	2011-01-11	\N	HOME	t	\N	0		0	a.scelerisque.sed@ornare.edu	\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bc0
43005007	0	33	2011-01-11	\N	HOME	t	\N	0		0	Vivamus@variusNam.com	\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bc6
43005008	0	34	2011-01-11	\N	HOME	t	\N	0		0	diam@liberoduinec.com	\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bcb
67000000	0	35	2011-01-11	\N	HOME	t	\N	0		0		\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bcf
1800501	0	0	2011-01-11	\N	HOME	f	\N	0		0		\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd3
1800502	0	0	2011-01-11	\N	HOME	f	\N	0		0		\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd7
1800503	0	0	2011-01-11	\N	HOME	f	\N	0		0		\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bdb
68000000	0	0	2011-01-11	\N	HOME	f	\N	0		0		\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bdf
1800504	0	0	2011-01-11	\N	HOME	f	\N	0		0		\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be3
1800505	0	0	2011-01-11	\N	HOME	f	\N	0		0		\N	0		\N	\N	f	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be7
43000000	43000000	0	\N	\N	\N	f	\N	0	\N	0	\N	\N	0	\N	\N	\N	f	2011-01-11	\N	\N	\N	\N
4000000	0	0	\N	\N	\N	f	\N	0	\N	0	\N	\N	0	\N	\N	\N	f	2011-01-11	\N	\N	\N	\N
95000000	0	0	\N	\N	\N	f	\N	0	\N	0	\N	\N	0	\N	\N	\N	f	2011-01-11	\N	\N	\N	\N
35000000	0	0	\N	\N	\N	f	\N	0	\N	0	\N	\N	0	\N	\N	\N	f	2011-01-11	\N	\N	\N	\N
73000000	0	0	\N	\N	\N	f	\N	0	\N	0	\N	\N	0	\N	\N	\N	f	2011-01-11	\N	\N	\N	\N
\.


--
-- Data for Name: p_partner_merge; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_merge (p_merge_from_n, p_merge_to_n, s_merged_by_c, s_merge_date_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_relation_category; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_relation_category (p_code_c, p_description_c, p_unassignable_flag_l, p_unassignable_date_d, p_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_relation; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_relation (p_relation_name_c, p_relation_description_c, p_relation_category_c, p_deletable_l, p_reciprocal_description_c, p_valid_relation_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_relationship; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_relationship (p_partner_key_n, p_relation_name_c, p_relation_key_n, p_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_set; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_set (p_partner_set_id_c, p_unit_key_n, p_partner_set_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_set_partner; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_set_partner (p_partner_set_id_c, p_partner_set_unit_key_n, p_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_short_code; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_short_code (p_partner_key_n, p_partner_short_code_c, p_field_key_n, p_recipient_flag_l, p_donor_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_state; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_state (p_process_code_c, p_state_code_c, p_state_descr_c, p_active_l, p_system_state_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_state; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_state (p_partner_key_n, p_state_index_i, p_process_code_c, p_state_code_c, p_state_freeform_c, p_state_start_date_d, p_state_end_date_d, p_state_complete_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_partner_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_partner_type (p_partner_key_n, p_type_code_c, p_valid_from_d, p_valid_until_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
43000000	LEDGER	\N	\N	2011-01-11	\N	\N	\N	\N
4000000	LEDGER	\N	\N	2011-01-11	\N	\N	\N	\N
95000000	LEDGER	\N	\N	2011-01-11	\N	\N	\N	\N
35000000	LEDGER	\N	\N	2011-01-11	\N	\N	\N	\N
73000000	LEDGER	\N	\N	2011-01-11	\N	\N	\N	\N
43005001	VOLUNTEER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000b9d
43005001	SUPPORTER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000b9e
43005002	VOLUNTEER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000ba4
43005002	SUPPORTER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000ba5
43005003	VOLUNTEER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bab
43005003	SUPPORTER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bac
43005004	SUPPORTER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bb2
43005005	SUPPORTER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bb8
43005006	SUPPORTER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bbe
43005007	SUPPORTER	\N	\N	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bc4
\.


--
-- Data for Name: p_postcode_range; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_postcode_range (p_range_c, p_from_c, p_to_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_postcode_region; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_postcode_region (p_region_c, p_range_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_publication; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_publication (p_publication_code_c, p_number_of_issues_i, p_number_of_reminders_i, p_publication_description_c, p_valid_publication_l, a_frequency_code_c, p_publication_label_code_c, p_publication_language_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
ANNUALREPORT	0	0	We send this annual report to all supporters	t	Annual	\N	\N	\N	\N	\N	\N	\N
NEWSUPDATES	0	0	Send out updates via letter or email	t	Quarterly	\N	\N	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_publication_cost; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_publication_cost (p_publication_code_c, p_date_effective_d, p_publication_cost_n, p_postage_cost_n, p_currency_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_reason_subscription_cancelled; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_reason_subscription_cancelled (p_code_c, p_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
BAD-ADDR	The mailing was returned due to partner no longer being at that address	\N	\N	\N	\N	\N
COMPLETE	The subscription has ended	\N	\N	\N	\N	\N
DIED	The Partner has died	\N	\N	\N	\N	\N
DISCONT	Publication no longer produced	\N	\N	\N	\N	\N
OFFICE	The Office has discontinued the subscription	\N	\N	\N	\N	\N
OTHER	Unknown	\N	\N	\N	\N	\N
REQUEST	Partner requested to discontinue subscription.	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_reason_subscription_given; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_reason_subscription_given (p_code_c, p_description_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
DONATION	Partner donated money to us.	\N	\N	\N	\N	\N
FREE	Free Subscription	\N	\N	\N	\N	\N
PAID	Subscription fee paid	\N	\N	\N	\N	\N
\.


--
-- Data for Name: p_recent_partners; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_recent_partners (p_when_d, p_when_t, s_user_id_c, p_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_reports; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_reports (p_report_name_c, p_report_description_c, p_report_program_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_subscription; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_subscription (p_publication_code_c, p_partner_key_n, p_publication_copies_i, p_reason_subs_given_code_c, p_reason_subs_cancelled_code_c, p_expiry_date_d, p_provisional_expiry_date_d, p_gratis_subscription_l, p_date_notice_sent_d, p_date_cancelled_d, p_start_date_d, p_number_issues_received_i, p_number_complimentary_i, p_subscription_renewal_date_d, p_subscription_status_c, p_first_issue_d, p_last_issue_d, p_gift_from_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_tax; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_tax (p_partner_key_n, p_tax_type_c, p_tax_ref_c, p_valid_from_d, p_valid_until_d, p_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_title; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_title (p_title_c, p_default_addressee_type_code_c, p_common_title_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: p_venue; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY p_venue (p_partner_key_n, p_venue_name_c, p_venue_code_c, a_currency_code_c, p_contact_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_conference; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_conference (pc_conference_key_n, pc_xyz_tbd_prefix_c, pc_start_d, pc_end_d, a_currency_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_attendee; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_attendee (pc_conference_key_n, p_partner_key_n, pc_home_office_key_n, pc_xyz_tbd_type_c, pc_actual_arr_d, pc_actual_dep_d, pc_badge_print_d, pc_details_print_d, pc_comments_c, pc_discovery_group_c, pc_work_group_c, pc_registered_d, pc_arrival_group_c, pc_departure_group_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_building; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_building (p_venue_key_n, pc_building_code_c, pc_building_desc_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_conference_cost; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_conference_cost (pc_conference_key_n, pc_option_days_i, pc_charge_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_conference_option_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_conference_option_type (pc_option_type_code_c, pc_option_type_description_c, pc_option_type_comment_c, pc_unassignable_flag_l, pc_unassignable_date_d, pc_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_conference_option; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_conference_option (pc_conference_key_n, pc_option_type_code_c, pc_option_set_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_conference_venue; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_conference_venue (pc_conference_key_n, p_venue_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_cost_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_cost_type (pc_cost_type_code_c, pc_cost_type_description_c, pc_unassignable_flag_l, pc_unassignable_date_d, pc_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_discount_criteria; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_discount_criteria (pc_discount_criteria_code_c, pc_discount_criteria_desc_c, pc_unassignable_flag_l, pc_unassignable_date_d, pc_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_discount; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_discount (pc_conference_key_n, pc_discount_criteria_code_c, pc_cost_type_code_c, pc_validity_c, pc_up_to_age_i, pc_percentage_l, pc_discount_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_early_late; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_early_late (pc_conference_key_n, pc_applicable_d, pc_type_l, pc_amount_percent_l, pc_amount_n, pc_percent_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_extra_cost; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_extra_cost (pc_conference_key_n, p_partner_key_n, pc_extra_cost_key_i, pc_cost_type_code_c, pc_cost_amount_n, pc_comment_c, pc_authorising_field_n, pc_authorising_person_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_group; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_group (pc_conference_key_n, p_partner_key_n, pc_group_type_c, pc_group_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_room; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_room (p_venue_key_n, pc_building_code_c, pc_room_number_c, pc_room_name_c, pc_beds_i, pc_max_occupancy_i, pc_bed_charge_n, pc_bed_cost_n, pc_usage_c, pc_gender_preference_c, pc_layout_xpos_i, pc_layout_ypos_i, pc_layout_width_i, pc_layout_height_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_room_alloc; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_room_alloc (pc_key_i, p_venue_key_n, pc_building_code_c, pc_room_number_c, pc_conference_key_n, p_partner_key_n, ph_book_whole_room_l, ph_number_of_beds_i, ph_number_of_overflow_beds_i, ph_gender_c, pc_in_d, pc_out_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_room_attribute_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_room_attribute_type (pc_code_c, pc_desc_c, pc_valid_l, pc_deletable_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_room_attribute; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_room_attribute (p_venue_key_n, pc_building_code_c, pc_room_number_c, pc_room_attr_type_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pc_supplement; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pc_supplement (pc_conference_key_n, pc_xyz_tbd_type_c, pc_supplement_n, pc_apply_discounts_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: ph_booking; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY ph_booking (ph_key_i, p_contact_key_n, a_ledger_number_for_invoice_i, a_ar_invoice_key_i, ph_number_of_adults_i, ph_number_of_children_i, ph_number_of_breakfast_i, ph_number_of_lunch_i, ph_number_of_supper_i, ph_number_of_linen_needed_i, ph_confirmed_d, ph_in_d, ph_out_d, ph_time_arrival_i, ph_time_departure_i, ph_notes_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: ph_room_booking; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY ph_room_booking (ph_booking_key_i, ph_room_alloc_key_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_application_file; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_application_file (p_file_info_key_n, p_partner_key_n, pm_application_key_i, pm_registration_office_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_app_form_types; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_app_form_types (pt_form_name_c, pt_form_sent_indicator_l, pt_form_received_indicator_l, pt_app_used_by_c, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_application_forms; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_application_forms (p_partner_key_n, pm_application_key_i, pm_registration_office_n, pt_form_name_c, pm_form_sent_l, pm_form_received_l, pm_form_delete_flag_l, pm_form_edited_l, pm_form_sent_date_d, pm_form_received_date_d, pm_reference_partner_key_n, pm_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_application_forms_file; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_application_forms_file (p_file_info_key_n, p_partner_key_n, pm_application_key_i, pm_registration_office_n, pt_form_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_application_status_history; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_application_status_history (pm_key_n, p_partner_key_n, pm_application_key_i, pm_registration_office_n, pm_status_code_c, pm_status_date_effective_d, pm_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_document_category; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_document_category (pm_code_c, pm_description_c, pm_extendable_l, pm_unassignable_flag_l, pm_unassignable_date_d, pm_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_document_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_document_type (pm_doc_code_c, pm_doc_category_c, pm_description_c, pm_unassignable_flag_l, pm_unassignable_date_d, pm_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_document; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_document (p_site_key_n, pm_document_key_n, p_partner_key_n, pm_doc_code_c, pm_document_id_c, pm_place_of_issue_c, pm_date_of_issue_d, pm_date_of_start_d, pm_date_of_expiration_d, pm_doc_comment_c, pm_assoc_doc_id_c, pm_contact_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_document_file; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_document_file (p_file_info_key_n, p_site_key_n, pm_document_key_n, p_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_formal_education; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_formal_education (pm_formal_education_key_i, p_partner_key_n, pm_education_category_c, pm_degree_c, pm_year_of_degree_i, pm_institution_c, p_country_code_c, pm_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_interview; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_interview (p_partner_key_n, pm_interview_date_d, pm_interviewer_c, pm_interview_comment_c, pm_intvw_action_to_take_c, pm_interviewed_for_c, pm_interview_unit_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_assignment_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_assignment_type (pt_assignment_type_code_c, pt_assignment_code_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_leaving_code; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_leaving_code (pt_leaving_code_ind_c, pt_leaving_code_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_position; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_position (pt_position_name_c, pt_position_scope_c, pt_position_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_job; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_job (pm_unit_key_n, pt_position_name_c, pt_position_scope_c, um_job_key_i, um_job_type_c, um_from_date_d, um_to_date_d, um_minimum_i, um_maximum_i, um_present_i, um_part_timers_i, um_applications_i, um_part_time_flag_l, um_training_period_c, um_commitment_period_c, um_public_flag_l, um_job_publicity_i, um_previous_internal_exp_req_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_job_assignment; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_job_assignment (p_partner_key_n, pm_unit_key_n, pt_position_name_c, pt_position_scope_c, um_job_key_i, pm_job_assignment_key_i, pt_assistant_to_l, pt_assignment_type_code_c, um_costs_changed_flag_l, pm_from_date_d, pm_to_date_d, pm_leaving_code_c, pm_leaving_code_updated_date_d, pm_hrd_cpy_detail_change_flag_l, pm_deleteable_flag_l, pm_registration_office_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_long_term_support_figures; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_long_term_support_figures (p_partner_key_n, pm_record_number_i, pm_allowance_n, pm_period_c, a_currency_code_c, pm_admin_grant_n, pm_housing_n, pm_vehicle_n, pm_food_n, pm_travel_conferences_n, pm_insurance_n, pm_pension_n, pm_childrens_education_n, pm_home_assignment_holiday_n, pm_medical_dental_care_n, pm_postage_telephone_n, pm_study_training_n, pm_ministry_costs_n, pm_personal_n, pm_other_n, pm_extra_cost1_label_c, pm_extra_cost1_n, pm_extra_cost2_label_c, pm_extra_cost2_n, pm_extra_cost3_label_c, pm_extra_cost3_n, pm_actual_support_figure_n, pm_agreed_support_figure_n, pm_agreement_date_d, pm_review_date_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_valuable_item; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_valuable_item (pt_valuable_item_name_c, pt_valuable_item_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_ownership; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_ownership (p_partner_key_n, pt_valuable_item_name_c, pm_amount_n, a_currency_code_c, pm_identifier_c, pm_marks_of_identification_c, pm_make_c, pm_model_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_passport_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_passport_type (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_passport_details; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_passport_details (p_partner_key_n, pm_passport_number_c, pm_main_passport_l, pm_active_flag_c, pm_full_passport_name_c, pm_passport_dob_d, pm_place_of_birth_c, p_passport_nationality_code_c, pm_date_of_expiration_d, pm_place_of_issue_c, p_country_of_issue_c, pm_date_of_issue_d, pm_passport_details_type_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_past_experience; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_past_experience (p_site_key_n, pm_key_n, p_partner_key_n, pm_start_date_d, pm_end_date_d, pm_prev_location_c, pm_prev_role_c, pm_category_c, pm_other_organisation_c, pm_past_exp_comments_c, pm_prev_work_here_l, pm_prev_work_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_pers_office_specific_data; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_pers_office_specific_data (p_partner_key_n, pm_pers_office_specific_fld1, pm_pers_office_specific_fld2, pm_pers_office_specific_fld3, pm_pers_office_specific_fld4, pm_pers_office_specific_fld5, pm_pers_office_specific_fld6, pm_pers_office_specific_fld7, pm_pers_office_specific_fld8, pm_pers_office_specific_fld9, pm_pers_office_specific_fld10, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_ability_area; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_ability_area (pt_ability_area_name_c, pt_ability_area_descr_c, pt_requirement_area_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_ability_level; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_ability_level (pt_ability_level_i, pt_ability_level_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_person_ability; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_person_ability (p_partner_key_n, pt_ability_area_name_c, pt_ability_level_i, pm_years_of_experience_i, pm_years_of_experience_as_of_d, pm_bringing_instrument_l, pm_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_person_commitment_status; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_person_commitment_status (p_partner_key_n, pm_status_code_c, pm_status_since_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_person_evaluation; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_person_evaluation (p_partner_key_n, pm_evaluation_type_c, pm_evaluation_date_d, pm_evaluation_comments_c, pm_person_eval_action_c, pm_evaluator_c, pm_next_evaluation_date_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_person_file; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_person_file (p_file_info_key_n, p_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_language_level; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_language_level (pt_language_level_i, pt_language_level_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, pt_language_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_person_language; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_person_language (p_partner_key_n, p_language_code_c, pm_years_of_experience_i, pm_years_of_experience_as_of_d, pt_language_level_i, pm_willing_to_translate_l, pm_translate_into_l, pm_translate_out_of_l, pm_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_qualification_area; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_qualification_area (pt_qualification_area_name_c, pt_qualification_area_descr_c, pt_qualification_flag_l, pt_qualification_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_qualification_level; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_qualification_level (pt_qualification_level_i, pt_qualification_level_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_person_qualification; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_person_qualification (p_partner_key_n, pt_qualification_area_name_c, pm_years_of_experience_i, pm_years_of_experience_as_of_d, pm_informal_flag_l, pt_qualification_level_i, pm_comment_c, pm_qualification_date_d, pm_qualification_expiry_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_skill_category; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_skill_category (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_skill_level; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_skill_level (pt_level_i, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_person_skill; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_person_skill (pm_person_skill_key_i, p_partner_key_n, pm_skill_category_code_c, pm_description_english_c, pm_description_local_c, pm_description_language_c, pm_skill_level_i, pm_years_of_experience_i, pm_years_of_experience_as_of_d, pm_professional_skill_l, pm_current_occupation_l, pm_degree_c, pm_year_of_degree_i, pm_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_vision_area; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_vision_area (pt_vision_area_name_c, pt_vision_area_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_vision_level; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_vision_level (pt_vision_level_i, pt_vision_level_descr_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_person_vision; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_person_vision (p_partner_key_n, pt_vision_area_name_c, pt_vision_level_i, pm_vision_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_driver_status; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_driver_status (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_personal_data; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_personal_data (p_partner_key_n, pm_height_cm_i, pm_weight_kg_n, pm_eye_colour_c, pm_hair_colour_c, pm_facial_hair_c, pm_physical_desc_c, pm_blood_type_c, pm_ethnic_origin_c, pm_life_question_1_c, pm_life_answer_1_c, pm_life_question_2_c, pm_life_answer_2_c, pm_life_question_3_c, pm_life_answer_3_c, pm_life_question_4_c, pm_life_answer_4_c, pm_personal_fld1_c, pm_personal_fld2_c, pm_personal_fld3_c, pm_personal_fld4_c, pm_personal_fld5_c, pm_personal_fld6_c, pm_driving_license_number_c, pm_internal_driver_license_l, pm_gen_driver_license_l, pm_driver_status_c, p_language_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_arrival_point; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_arrival_point (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_congress_code; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_congress_code (pt_code_c, pt_description_c, pt_pre_congress_l, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, pt_discounted_l, pt_xyz_tbd_l, pt_conference_l, pt_participant_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_leadership_rating; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_leadership_rating (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_party_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_party_type (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_special_applicant; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_special_applicant (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_travel_type; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_travel_type (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_xyz_tbd_preference_level; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_xyz_tbd_preference_level (pt_code_c, pt_description_c, pt_unassignable_flag_l, pt_unassignable_date_d, pt_deletable_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_short_term_application; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_short_term_application (p_partner_key_n, pm_application_key_i, pm_registration_office_n, pm_st_app_date_d, pm_st_application_type_c, pm_st_basic_xyz_tbd_identifier_c, pm_st_basic_delete_flag_l, pm_st_booking_fee_received_l, pm_st_program_fee_received_l, pm_st_application_on_hold_l, pm_st_application_hold_reason_c, pm_st_scholarship_amount_n, pm_st_scholarship_approved_by_c, pm_st_scholarship_review_date_d, pm_st_scholarship_period_c, pm_st_leadership_rating_c, pm_confirmed_option_code_c, pm_option1_code_c, pm_option2_code_c, pm_st_xyz_tbd_only_flag_l, pm_st_confirmed_option_n, pm_st_option1_n, pm_st_option2_n, pm_st_current_field_n, pm_arrival_details_status_c, pt_arrival_point_code_c, pt_travel_type_to_cong_code_c, pm_arrival_d, pm_arrival_hour_i, pm_arrival_minute_i, pm_to_cong_travel_info_c, pm_arrival_transport_needed_l, pm_arrival_exp_d, pm_arrival_exp_hour_i, pm_arrival_exp_minute_i, pm_arrival_comments_c, pm_departure_details_status_c, pt_departure_point_code_c, pt_travel_type_from_cong_code_c, pm_departure_d, pm_departure_hour_i, pm_departure_minute_i, pm_from_cong_travel_info_c, pm_departure_transport_needed_l, pm_departure_exp_d, pm_departure_exp_hour_i, pm_departure_exp_minute_i, pm_departure_comments_c, pm_transport_interest_l, pm_contact_number_c, pm_st_recruit_efforts_c, pm_st_pre_congress_code_c, pm_st_congress_code_c, pm_st_special_applicant_c, pm_xyz_tbd_role_c, pm_st_party_contact_n, pm_st_party_together_c, pm_st_fg_leader_l, pm_st_fg_code_c, pm_st_cmpgn_special_cost_i, pm_st_cngrss_special_cost_i, pm_st_field_charged_n, pm_st_congress_language_c, pm_st_country_pref_c, pm_st_activity_pref_c, pm_st_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_special_need; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_special_need (p_partner_key_n, pm_medical_comment_c, pm_dietary_comment_c, pm_other_special_need_c, pm_contact_home_office_l, pm_vegetarian_flag_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pm_year_program_application; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pm_year_program_application (p_partner_key_n, pm_application_key_i, pm_registration_office_n, pm_yp_app_date_d, pm_yp_basic_app_type_c, pm_yp_basic_delete_flag_l, pm_yp_app_fee_received_l, pm_ho_orient_conf_booking_key_c, pm_yp_agreed_support_figure_n, pm_yp_agreed_joining_charge_n, pm_yp_scholarship_n, pm_yp_scholarship_athrized_by_c, pm_yp_scholarship_begin_date_d, pm_yp_scholarship_end_date_d, pm_yp_scholarship_review_date_d, pm_yp_scholarship_period_c, pm_yp_support_period_c, pm_yp_joining_conf_i, pm_start_of_commitment_d, pm_end_of_commitment_d, pm_intended_com_length_months_i, pt_position_name_c, pt_position_scope_c, pt_assistant_to_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_office_specific_data_labels; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_office_specific_data_labels (pt_off_specific_label1_c, pt_off_specific_label2_c, pt_off_specific_label3_c, pt_off_specific_label4_c, pt_off_specific_label5_c, pt_off_specific_label6_c, pt_off_specific_label7_c, pt_off_specific_label8_c, pt_off_specific_label9_c, pt_off_specific_label10_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_personal_data_labels; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_personal_data_labels (pt_pers_data_label1_c, pt_pers_data_label2_c, pt_pers_data_label3_c, pt_pers_data_label4_c, pt_pers_data_label5_c, pt_pers_data_label6_c, pt_pers_data_label7_c, pt_pers_data_help_1_c, pt_pers_data_help_2_c, pt_pers_data_help_3_c, pt_pers_data_help_4_c, pt_pers_data_help_5_c, pt_pers_data_help_6_c, pt_pers_data_help_7_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: pt_reports; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY pt_reports (pt_report_name_c, pt_report_description_c, pt_report_program_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_batch_job; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_batch_job (s_file_name_c, s_user_id_c, s_job_type_c, s_minutes_i, s_hours_i, s_day_of_month_c, s_month_of_year_c, s_day_of_week_c, s_parameters_used_l, s_data_c, s_remove_l, s_date_submitted_d, s_time_submitted_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_change_event; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_change_event (s_table_name_c, s_rowid_c, s_change_type_c, s_natural_key_c, s_date_d, s_time_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_volume_partner_group; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_volume_partner_group (s_name_c, s_description_c, s_comment_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_default_file_volume; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_default_file_volume (s_group_name_c, s_area_c, s_volume_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_error_log; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_error_log (s_error_code_c, s_release_number_c, s_user_id_c, s_file_name_c, s_process_id_c, s_date_d, s_time_i, s_message_line_1_c, s_message_line_2_c, s_message_line_3_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_error_message; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_error_message (s_language_code_c, s_error_code_c, s_message_line_1_c, s_message_line_2_c, s_message_line_3_c, s_response_c, s_alert_type_c, s_title_c, s_log_error_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_function; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_function (s_function_id_c, s_module_name_c, s_sub_module_name_c, s_function_name_c, s_filename_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_function_relationship; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_function_relationship (s_function_1_c, s_function_2_c, s_code_to_run_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_cost_centre; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_cost_centre (s_group_id_c, s_group_unit_key_n, a_ledger_number_i, a_cost_centre_code_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_data_label; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_data_label (s_group_id_c, s_group_unit_key_n, p_data_label_key_i, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_extract; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_extract (s_group_id_c, s_group_unit_key_n, m_extract_id_i, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_file_info; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_file_info (s_group_id_c, s_group_unit_key_n, p_file_info_key_n, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_function; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_function (s_group_id_c, s_unit_key_n, s_function_id_c, s_can_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_gift; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_gift (s_group_id_c, s_group_unit_key_n, a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_ledger; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_ledger (s_group_id_c, s_group_unit_key_n, a_ledger_number_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_location; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_location (s_group_id_c, s_group_unit_key_n, p_site_key_n, p_location_key_i, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_module_access_permission; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_module_access_permission (s_group_id_c, s_group_unit_key_n, s_module_id_c, s_can_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_motivation; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_motivation (s_group_id_c, s_group_unit_key_n, a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_partner_contact; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_partner_contact (s_group_id_c, s_group_unit_key_n, p_contact_id_i, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_partner_location; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_partner_location (s_group_id_c, s_group_unit_key_n, p_partner_key_n, p_site_key_n, p_location_key_i, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_partner_reminder; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_partner_reminder (s_group_id_c, s_group_unit_key_n, p_partner_key_n, p_contact_id_i, p_reminder_id_i, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_partner_set; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_partner_set (s_group_id_c, s_group_unit_key_n, p_partner_set_id_c, p_partner_set_unit_key_n, s_inclusive_or_exclusive_l, s_read_access_l, s_write_access_l, s_delete_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_group_table_access_permission; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_group_table_access_permission (s_group_id_c, s_group_unit_key_n, s_table_name_c, s_can_create_l, s_can_modify_l, s_can_delete_l, s_can_inquire_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_job_group; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_job_group (pt_position_name_c, pt_position_scope_c, um_job_key_i, s_group_id_c, s_unit_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_label; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_label (p_label_name_c, s_label_description_c, s_form_name_c, s_top_margin_n, s_side_margin_n, s_vertical_pitch_n, s_horizontal_pitch_n, s_label_height_n, s_label_width_n, s_labels_across_i, s_labels_down_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_language_specific; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_language_specific (s_language_code_c, s_month_name_c_1, s_month_name_c_2, s_month_name_c_3, s_month_name_c_4, s_month_name_c_5, s_month_name_c_6, s_month_name_c_7, s_month_name_c_8, s_month_name_c_9, s_month_name_c_10, s_month_name_c_11, s_month_name_c_12, s_month_name_short_c_1, s_month_name_short_c_2, s_month_name_short_c_3, s_month_name_short_c_4, s_month_name_short_c_5, s_month_name_short_c_6, s_month_name_short_c_7, s_month_name_short_c_8, s_month_name_short_c_9, s_month_name_short_c_10, s_month_name_short_c_11, s_month_name_short_c_12, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_login; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_login (s_user_id_c, s_login_tty_line_c, s_login_time_i, s_login_date_d, s_login_status_c, s_logout_time_i, s_logout_date_d, s_login_process_id_r, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_logon_message; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_logon_message (s_language_code_c, s_logon_message_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
EN	Welcome to the Petra Demo database!	\N	SYSADMIN	\N	\N	\N
\.


--
-- Data for Name: s_module_file; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_module_file (s_module_id_c, s_file_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_patch_log; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_patch_log (s_patch_name_c, s_user_id_c, s_date_run_d, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_report_file; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_report_file (s_user_id_c, s_report_file_name_c, s_rpg_c, s_report_date_d, s_report_time_i, s_report_title_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_report_options; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_report_options (s_user_id_c, s_module_id_c, s_report_file_name_c, s_report_title_c, s_interactive_l, s_rpg_c, s_copies_i, s_from_page_i, s_to_page_i, s_data_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_reports_to_archive; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_reports_to_archive (s_report_title_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_selected_output_destination; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_selected_output_destination (s_user_id_c, s_module_id_c, s_form_name_c, s_printer_name_c, s_batch_job_id_r, s_login_process_id_r, s_paper_source_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_system_defaults; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_system_defaults (s_default_code_c, s_default_description_c, s_default_value_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
LocalisedCountyLabel	LocalisedCountyLabel	County/St&ate	2011-01-11	\N	\N	\N	\N
CurrentDatabaseVersion	the currently installed release number, set by installer/patchtool	0.2.7-0	2011-01-11	\N	\N	\N	\N
SiteKey	there has to be one site key for the database	43000000	2011-01-11	\N	\N	\N	\N
\.


--
-- Data for Name: s_system_status; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_system_status (s_user_id_c, s_system_disabled_date_d, s_system_disabled_time_i, s_system_disabled_reason_c, s_system_available_date_d, s_system_available_time_i, s_system_login_status_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
SYSADMIN	\N	0	\N	\N	0	t	2011-01-11	\N	\N	\N	\N
\.


--
-- Data for Name: s_system_status_log; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_system_status_log (s_user_id_c, s_system_disabled_date_d, s_system_disabled_time_i, s_system_disabled_reason_c, s_system_enabled_date_d, s_system_enabled_time_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_user_defaults; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_user_defaults (s_user_id_c, s_default_code_c, s_default_value_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_user_group; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_user_group (s_user_id_c, s_group_id_c, s_unit_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_user_module_access_permission; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_user_module_access_permission (s_user_id_c, s_module_id_c, s_can_access_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
SYSADMIN	SYSMAN	t	2011-01-11	\N	\N	\N	\N
DEMO	PTNRUSER	t	2011-01-11	\N	\N	\N	\N
DEMO	CONFERENCE	t	2011-01-11	\N	\N	\N	\N
DEMO	DEVUSER	t	2011-01-11	\N	\N	\N	\N
DEMO	PERSONNEL	t	2011-01-11	\N	\N	\N	\N
DEMO	FINANCE-1	t	2011-01-11	\N	\N	\N	\N
DEMO	FINANCE-2	t	2011-01-11	\N	\N	\N	\N
DEMO	FINANCE-3	t	2011-01-11	\N	\N	\N	\N
DEMO	LEDGER0043	t	2011-01-11	\N	\N	\N	\N
\.


--
-- Data for Name: s_user_table_access_permission; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_user_table_access_permission (s_user_id_c, s_table_name_c, s_can_create_l, s_can_modify_l, s_can_delete_l, s_can_inquire_l, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
DEMO	p_partner	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_partner_location	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_partner_type	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_location	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_church	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_family	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_person	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_unit	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_bank	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_venue	t	t	t	t	2011-01-11	\N	\N	\N	\N
DEMO	p_organisation	t	t	t	t	2011-01-11	\N	\N	\N	\N
\.


--
-- Data for Name: s_valid_output_form; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_valid_output_form (s_module_id_c, s_form_name_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_volume_partner_group_partner; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_volume_partner_group_partner (s_group_name_c, p_partner_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_workflow_definition; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_workflow_definition (s_workflow_id_i, s_name_c, s_description_c, s_module_list_c, s_type_of_shared_data_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_workflow_group; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_workflow_group (s_workflow_id_i, s_group_id_c, s_group_unit_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_workflow_instance; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_workflow_instance (s_workflow_instance_id_i, s_workflow_id_i, s_key_data_item_c, s_key_data_item_type_c, s_system_generated_l, s_complete_l, s_note_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_workflow_instance_step; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_workflow_instance_step (s_workflow_instance_id_i, s_step_number_i, s_complete_l, s_user_id_c, s_output_parameters_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_workflow_step; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_workflow_step (s_workflow_id_i, s_step_number_i, s_function_id_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: s_workflow_user; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY s_workflow_user (s_workflow_id_i, s_user_id_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_job_language; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_job_language (pm_unit_key_n, pt_position_name_c, pt_position_scope_c, um_job_key_i, p_language_code_c, um_years_of_experience_i, pt_language_level_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_job_qualification; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_job_qualification (pm_unit_key_n, pt_position_name_c, pt_position_scope_c, um_job_key_i, pt_qualification_area_name_c, um_years_of_experience_i, pm_informal_flag_l, pt_qualification_level_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_job_requirement; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_job_requirement (pm_unit_key_n, pt_position_name_c, pt_position_scope_c, um_job_key_i, pt_ability_area_name_c, um_years_of_experience_i, pt_ability_level_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_job_vision; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_job_vision (pm_unit_key_n, pt_position_name_c, pt_position_scope_c, um_job_key_i, pt_vision_area_name_c, pt_vision_level_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_unit_ability; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_unit_ability (p_partner_key_n, pt_ability_area_name_c, um_years_of_experience_i, pt_ability_level_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_unit_cost; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_unit_cost (p_partner_key_n, um_valid_from_date_d, um_single_costs_period_intl_n, um_couple_costs_period_intl_n, um_child1_costs_period_intl_n, um_child2_costs_period_intl_n, um_child3_costs_period_intl_n, um_adult_joining_charge_intl_n, um_couple_joining_charge_intl_n, um_child_joining_charge_intl_n, a_local_currency_code_c, um_charge_period_c, um_single_costs_period_base_n, um_couple_costs_period_base_n, um_child1_costs_period_base_n, um_child2_costs_period_base_n, um_child3_costs_period_base_n, um_adult_joining_charge_base_n, um_couple_joining_charge_base_n, um_child_joining_charge_base_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_unit_evaluation; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_unit_evaluation (p_partner_key_n, um_date_of_evaluation_d, um_evaluation_number_n, um_evaluator_family_status_c, p_evaluator_home_country_c, um_evaluator_age_n, um_evaluator_sex_c, um_unit_evaluation_data_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_unit_language; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_unit_language (p_partner_key_n, p_language_code_c, pt_language_level_i, um_years_of_experience_i, um_unit_lang_comment_c, um_unit_language_req_c, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- Data for Name: um_unit_structure; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_unit_structure (um_parent_unit_key_n, um_child_unit_key_n, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
1000000	1000000	2011-01-11	\N	\N	\N	\N
1000000	4000000	2011-01-11	\N	\N	\N	\N
1000000	95000000	2011-01-11	\N	\N	\N	\N
1000000	35000000	2011-01-11	\N	\N	\N	\N
1000000	73000000	2011-01-11	\N	\N	\N	\N
1000000	67000000	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd1
67000000	1800501	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd5
67000000	1800502	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bd9
67000000	1800503	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000bdd
1000000	68000000	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be1
68000000	1800504	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be5
68000000	1800505	2011-01-11	DEMO	\N	\N	0000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000be9
\.


--
-- Data for Name: um_unit_vision; Type: TABLE DATA; Schema: public; Owner: Matthias Hobohm
--

COPY um_unit_vision (p_partner_key_n, pt_vision_area_name_c, pt_vision_level_i, s_date_created_d, s_created_by_c, s_date_modified_d, s_modified_by_c, s_modification_id_c) FROM stdin;
\.


--
-- PostgreSQL database dump complete
--

