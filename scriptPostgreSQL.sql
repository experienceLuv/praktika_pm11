--
-- PostgreSQL database dump
--

\restrict pjeTTx1dkKR4MgNa4cgR23vco6ZIxE0mT20zkvgB8an7jAinysk7UP2wYIxUfQ7

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

-- Started on 2026-04-15 13:15:15

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5161 (class 1262 OID 16388)
-- Name: keeperpro; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE keeperpro WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';


ALTER DATABASE keeperpro OWNER TO postgres;

\unrestrict pjeTTx1dkKR4MgNa4cgR23vco6ZIxE0mT20zkvgB8an7jAinysk7UP2wYIxUfQ7
\connect keeperpro
\restrict pjeTTx1dkKR4MgNa4cgR23vco6ZIxE0mT20zkvgB8an7jAinysk7UP2wYIxUfQ7

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 6 (class 2615 OID 16389)
-- Name: keeperpro; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA keeperpro;


ALTER SCHEMA keeperpro OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 231 (class 1259 OID 16470)
-- Name: application_statuses; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.application_statuses (
    status_id integer NOT NULL,
    status_name character varying(100) NOT NULL
);


ALTER TABLE keeperpro.application_statuses OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 16469)
-- Name: application_statuses_status_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.application_statuses_status_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.application_statuses_status_id_seq OWNER TO postgres;

--
-- TOC entry 5162 (class 0 OID 0)
-- Dependencies: 230
-- Name: application_statuses_status_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.application_statuses_status_id_seq OWNED BY keeperpro.application_statuses.status_id;


--
-- TOC entry 229 (class 1259 OID 16459)
-- Name: application_types; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.application_types (
    application_type_id integer NOT NULL,
    type_name character varying(100) NOT NULL
);


ALTER TABLE keeperpro.application_types OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 16458)
-- Name: application_types_application_type_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.application_types_application_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.application_types_application_type_id_seq OWNER TO postgres;

--
-- TOC entry 5163 (class 0 OID 0)
-- Dependencies: 228
-- Name: application_types_application_type_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.application_types_application_type_id_seq OWNED BY keeperpro.application_types.application_type_id;


--
-- TOC entry 237 (class 1259 OID 16546)
-- Name: application_visitors; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.application_visitors (
    application_visitor_id integer NOT NULL,
    application_id integer NOT NULL,
    visitor_id integer NOT NULL,
    visitor_order integer NOT NULL
);


ALTER TABLE keeperpro.application_visitors OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 16545)
-- Name: application_visitors_application_visitor_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.application_visitors_application_visitor_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.application_visitors_application_visitor_id_seq OWNER TO postgres;

--
-- TOC entry 5164 (class 0 OID 0)
-- Dependencies: 236
-- Name: application_visitors_application_visitor_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.application_visitors_application_visitor_id_seq OWNED BY keeperpro.application_visitors.application_visitor_id;


--
-- TOC entry 235 (class 1259 OID 16497)
-- Name: applications; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.applications (
    application_id integer NOT NULL,
    user_id integer NOT NULL,
    application_type_id integer NOT NULL,
    department_id integer NOT NULL,
    employee_id integer NOT NULL,
    date_start date NOT NULL,
    date_end date NOT NULL,
    visit_purpose text NOT NULL,
    note text NOT NULL,
    status_id integer NOT NULL,
    rejection_reason text,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT chk_dates CHECK ((date_end >= date_start))
);


ALTER TABLE keeperpro.applications OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 16496)
-- Name: applications_application_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.applications_application_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.applications_application_id_seq OWNER TO postgres;

--
-- TOC entry 5165 (class 0 OID 0)
-- Dependencies: 234
-- Name: applications_application_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.applications_application_id_seq OWNED BY keeperpro.applications.application_id;


--
-- TOC entry 225 (class 1259 OID 16426)
-- Name: departments; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.departments (
    department_id integer NOT NULL,
    department_name character varying(255) NOT NULL,
    description text
);


ALTER TABLE keeperpro.departments OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 16425)
-- Name: departments_department_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.departments_department_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.departments_department_id_seq OWNER TO postgres;

--
-- TOC entry 5166 (class 0 OID 0)
-- Dependencies: 224
-- Name: departments_department_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.departments_department_id_seq OWNED BY keeperpro.departments.department_id;


--
-- TOC entry 239 (class 1259 OID 16569)
-- Name: document_types; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.document_types (
    document_type_id integer NOT NULL,
    document_type_name character varying(100) NOT NULL
);


ALTER TABLE keeperpro.document_types OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 16568)
-- Name: document_types_document_type_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.document_types_document_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.document_types_document_type_id_seq OWNER TO postgres;

--
-- TOC entry 5167 (class 0 OID 0)
-- Dependencies: 238
-- Name: document_types_document_type_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.document_types_document_type_id_seq OWNED BY keeperpro.document_types.document_type_id;


--
-- TOC entry 241 (class 1259 OID 16580)
-- Name: documents; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.documents (
    document_id integer NOT NULL,
    application_id integer NOT NULL,
    visitor_id integer,
    document_type_id integer NOT NULL,
    file_name character varying(255) NOT NULL,
    file_path text NOT NULL,
    uploaded_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE keeperpro.documents OWNER TO postgres;

--
-- TOC entry 240 (class 1259 OID 16579)
-- Name: documents_document_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.documents_document_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.documents_document_id_seq OWNER TO postgres;

--
-- TOC entry 5168 (class 0 OID 0)
-- Dependencies: 240
-- Name: documents_document_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.documents_document_id_seq OWNED BY keeperpro.documents.document_id;


--
-- TOC entry 227 (class 1259 OID 16439)
-- Name: employees; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.employees (
    employee_id integer NOT NULL,
    department_id integer NOT NULL,
    last_name character varying(100) NOT NULL,
    first_name character varying(100) NOT NULL,
    middle_name character varying(100),
    "position" character varying(150),
    phone character varying(30),
    email character varying(255),
    is_active boolean DEFAULT true NOT NULL
);


ALTER TABLE keeperpro.employees OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 16438)
-- Name: employees_employee_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.employees_employee_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.employees_employee_id_seq OWNER TO postgres;

--
-- TOC entry 5169 (class 0 OID 0)
-- Dependencies: 226
-- Name: employees_employee_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.employees_employee_id_seq OWNED BY keeperpro.employees.employee_id;


--
-- TOC entry 221 (class 1259 OID 16391)
-- Name: roles; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.roles (
    role_id integer NOT NULL,
    role_name character varying(100) NOT NULL
);


ALTER TABLE keeperpro.roles OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16390)
-- Name: roles_role_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.roles_role_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.roles_role_id_seq OWNER TO postgres;

--
-- TOC entry 5170 (class 0 OID 0)
-- Dependencies: 220
-- Name: roles_role_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.roles_role_id_seq OWNED BY keeperpro.roles.role_id;


--
-- TOC entry 223 (class 1259 OID 16402)
-- Name: users; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.users (
    user_id integer NOT NULL,
    email character varying(255) NOT NULL,
    password_hash character varying(255) NOT NULL,
    role_id integer NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    is_active boolean DEFAULT true NOT NULL
);


ALTER TABLE keeperpro.users OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16401)
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.users_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.users_user_id_seq OWNER TO postgres;

--
-- TOC entry 5171 (class 0 OID 0)
-- Dependencies: 222
-- Name: users_user_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.users_user_id_seq OWNED BY keeperpro.users.user_id;


--
-- TOC entry 243 (class 1259 OID 16611)
-- Name: visit_logs; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.visit_logs (
    visit_log_id integer NOT NULL,
    application_id integer NOT NULL,
    visitor_id integer NOT NULL,
    security_employee_id integer,
    department_employee_id integer,
    entry_time timestamp without time zone,
    exit_time timestamp without time zone,
    comment text
);


ALTER TABLE keeperpro.visit_logs OWNER TO postgres;

--
-- TOC entry 242 (class 1259 OID 16610)
-- Name: visit_logs_visit_log_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.visit_logs_visit_log_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.visit_logs_visit_log_id_seq OWNER TO postgres;

--
-- TOC entry 5172 (class 0 OID 0)
-- Dependencies: 242
-- Name: visit_logs_visit_log_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.visit_logs_visit_log_id_seq OWNED BY keeperpro.visit_logs.visit_log_id;


--
-- TOC entry 233 (class 1259 OID 16481)
-- Name: visitors; Type: TABLE; Schema: keeperpro; Owner: postgres
--

CREATE TABLE keeperpro.visitors (
    visitor_id integer NOT NULL,
    last_name character varying(100) NOT NULL,
    first_name character varying(100) NOT NULL,
    middle_name character varying(100),
    phone character varying(30),
    email character varying(255) NOT NULL,
    organization character varying(255),
    birth_date date NOT NULL,
    passport_series character(4) NOT NULL,
    passport_number character(6) NOT NULL,
    photo_path text
);


ALTER TABLE keeperpro.visitors OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 16480)
-- Name: visitors_visitor_id_seq; Type: SEQUENCE; Schema: keeperpro; Owner: postgres
--

CREATE SEQUENCE keeperpro.visitors_visitor_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE keeperpro.visitors_visitor_id_seq OWNER TO postgres;

--
-- TOC entry 5173 (class 0 OID 0)
-- Dependencies: 232
-- Name: visitors_visitor_id_seq; Type: SEQUENCE OWNED BY; Schema: keeperpro; Owner: postgres
--

ALTER SEQUENCE keeperpro.visitors_visitor_id_seq OWNED BY keeperpro.visitors.visitor_id;


--
-- TOC entry 4920 (class 2604 OID 16473)
-- Name: application_statuses status_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_statuses ALTER COLUMN status_id SET DEFAULT nextval('keeperpro.application_statuses_status_id_seq'::regclass);


--
-- TOC entry 4919 (class 2604 OID 16462)
-- Name: application_types application_type_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_types ALTER COLUMN application_type_id SET DEFAULT nextval('keeperpro.application_types_application_type_id_seq'::regclass);


--
-- TOC entry 4925 (class 2604 OID 16549)
-- Name: application_visitors application_visitor_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_visitors ALTER COLUMN application_visitor_id SET DEFAULT nextval('keeperpro.application_visitors_application_visitor_id_seq'::regclass);


--
-- TOC entry 4922 (class 2604 OID 16500)
-- Name: applications application_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.applications ALTER COLUMN application_id SET DEFAULT nextval('keeperpro.applications_application_id_seq'::regclass);


--
-- TOC entry 4916 (class 2604 OID 16429)
-- Name: departments department_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.departments ALTER COLUMN department_id SET DEFAULT nextval('keeperpro.departments_department_id_seq'::regclass);


--
-- TOC entry 4926 (class 2604 OID 16572)
-- Name: document_types document_type_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.document_types ALTER COLUMN document_type_id SET DEFAULT nextval('keeperpro.document_types_document_type_id_seq'::regclass);


--
-- TOC entry 4927 (class 2604 OID 16583)
-- Name: documents document_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.documents ALTER COLUMN document_id SET DEFAULT nextval('keeperpro.documents_document_id_seq'::regclass);


--
-- TOC entry 4917 (class 2604 OID 16442)
-- Name: employees employee_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.employees ALTER COLUMN employee_id SET DEFAULT nextval('keeperpro.employees_employee_id_seq'::regclass);


--
-- TOC entry 4912 (class 2604 OID 16394)
-- Name: roles role_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.roles ALTER COLUMN role_id SET DEFAULT nextval('keeperpro.roles_role_id_seq'::regclass);


--
-- TOC entry 4913 (class 2604 OID 16405)
-- Name: users user_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.users ALTER COLUMN user_id SET DEFAULT nextval('keeperpro.users_user_id_seq'::regclass);


--
-- TOC entry 4929 (class 2604 OID 16614)
-- Name: visit_logs visit_log_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.visit_logs ALTER COLUMN visit_log_id SET DEFAULT nextval('keeperpro.visit_logs_visit_log_id_seq'::regclass);


--
-- TOC entry 4921 (class 2604 OID 16484)
-- Name: visitors visitor_id; Type: DEFAULT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.visitors ALTER COLUMN visitor_id SET DEFAULT nextval('keeperpro.visitors_visitor_id_seq'::regclass);


--
-- TOC entry 5143 (class 0 OID 16470)
-- Dependencies: 231
-- Data for Name: application_statuses; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.application_statuses (status_id, status_name) FROM stdin;
1	На проверке
2	Одобрена
3	Не одобрена
4	Завершена
\.


--
-- TOC entry 5141 (class 0 OID 16459)
-- Dependencies: 229
-- Data for Name: application_types; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.application_types (application_type_id, type_name) FROM stdin;
1	individual
2	group
\.


--
-- TOC entry 5149 (class 0 OID 16546)
-- Dependencies: 237
-- Data for Name: application_visitors; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.application_visitors (application_visitor_id, application_id, visitor_id, visitor_order) FROM stdin;
1	1	1	1
2	2	2	1
3	3	3	1
4	3	4	2
5	3	5	3
6	3	6	4
7	3	7	5
8	4	8	1
9	5	9	1
10	6	10	1
11	7	11	1
12	8	12	1
13	9	13	1
14	10	14	1
15	11	15	1
16	12	16	1
17	13	17	1
18	14	18	1
19	15	19	1
20	16	20	1
21	17	21	1
22	18	22	1
23	19	23	1
24	19	24	2
25	19	25	3
26	19	26	4
27	19	27	5
28	19	28	6
29	19	29	7
30	20	30	1
31	20	31	2
32	20	32	3
33	20	33	4
34	20	34	5
35	20	35	6
36	20	36	7
37	20	37	8
\.


--
-- TOC entry 5147 (class 0 OID 16497)
-- Dependencies: 235
-- Data for Name: applications; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.applications (application_id, user_id, application_type_id, department_id, employee_id, date_start, date_end, visit_purpose, note, status_id, rejection_reason, created_at, updated_at) FROM stdin;
1	1	1	1	1	2026-04-16	2026-04-17	Собеседование	Личная заявка на посещение отдела кадров	2	\N	2026-04-15 09:30:43.735487	2026-04-15 09:30:43.735487
2	2	1	3	3	2026-04-17	2026-04-18	Встреча с ИТ-отделом	Личная заявка на посещение ИТ-отдела	1	\N	2026-04-15 09:30:43.735487	2026-04-15 09:30:43.735487
3	3	2	5	5	2026-04-18	2026-04-19	Экскурсия	Групповая заявка на посещение предприятия	2	\N	2026-04-15 09:30:43.735487	2026-04-15 09:30:43.735487
4	7	1	6	6	2023-04-24	2023-04-24	Личное посещение предприятия	Индивидуальная заявка 24/04/2023_9367788	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
5	8	1	7	7	2023-04-24	2023-04-24	Личное посещение предприятия	Индивидуальная заявка 24/04/2023_9788737	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
6	9	1	5	8	2023-04-24	2023-04-24	Личное посещение предприятия	Индивидуальная заявка 24/04/2023_9736379	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
7	10	1	6	6	2023-04-25	2023-04-25	Личное посещение предприятия	Индивидуальная заявка 25/04/2023_9367788	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
8	11	1	7	7	2023-04-25	2023-04-25	Личное посещение предприятия	Индивидуальная заявка 25/04/2023_9788737	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
9	12	1	5	8	2023-04-25	2023-04-25	Личное посещение предприятия	Индивидуальная заявка 25/04/2023_9736379	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
10	13	1	6	6	2023-04-26	2023-04-26	Личное посещение предприятия	Индивидуальная заявка 26/04/2023_9367788	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
11	14	1	7	7	2023-04-26	2023-04-26	Личное посещение предприятия	Индивидуальная заявка 26/04/2023_9788737	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
12	15	1	5	8	2023-04-26	2023-04-26	Личное посещение предприятия	Индивидуальная заявка 26/04/2023_9736379	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
13	16	1	6	6	2023-04-27	2023-04-27	Личное посещение предприятия	Индивидуальная заявка 27/04/2023_9367788	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
14	17	1	7	7	2023-04-27	2023-04-27	Личное посещение предприятия	Индивидуальная заявка 27/04/2023_9788737	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
15	18	1	5	8	2023-04-27	2023-04-27	Личное посещение предприятия	Индивидуальная заявка 27/04/2023_9736379	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
16	19	1	6	6	2023-04-28	2023-04-28	Личное посещение предприятия	Индивидуальная заявка 28/04/2023_9367788	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
17	20	1	7	7	2023-04-28	2023-04-28	Личное посещение предприятия	Индивидуальная заявка 28/04/2023_9788737	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
18	21	1	5	8	2023-04-28	2023-04-28	Личное посещение предприятия	Индивидуальная заявка 28/04/2023_9736379	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
19	22	2	6	6	2023-04-24	2023-04-24	Групповое посещение предприятия	Групповая заявка 24/04/2023_Производство_Фомичева_9367788_ГР1	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
20	29	2	6	6	2023-04-24	2023-04-24	Групповое посещение предприятия	Групповая заявка 24/04/2023_Производство_Фомичева_9367788_ГР2	2	\N	2026-04-15 10:01:05.982897	2026-04-15 10:01:05.982897
\.


--
-- TOC entry 5137 (class 0 OID 16426)
-- Dependencies: 225
-- Data for Name: departments; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.departments (department_id, department_name, description) FROM stdin;
1	Отдел кадров	Работа с персоналом
2	Бухгалтерия	Финансовый учет
3	ИТ-отдел	Информационные технологии
4	Служба безопасности	Контроль доступа
5	Администрация	Управление предприятием
6	Производство	Импорт из файла Сессия 1.xlsx
7	Сбыт	Импорт из файла Сессия 1.xlsx
10	Планирование	Импорт из файла Сессия 1.xlsx
11	Общий отдел	Импорт из файла Сессия 1.xlsx
12	Охрана	Импорт из файла Сессия 1.xlsx
\.


--
-- TOC entry 5151 (class 0 OID 16569)
-- Dependencies: 239
-- Data for Name: document_types; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.document_types (document_type_id, document_type_name) FROM stdin;
1	passport_scan
2	visitor_photo
3	group_list_xlsx
\.


--
-- TOC entry 5153 (class 0 OID 16580)
-- Dependencies: 241
-- Data for Name: documents; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.documents (document_id, application_id, visitor_id, document_type_id, file_name, file_path, uploaded_at) FROM stdin;
1	1	1	1	petrov_passport.pdf	/files/docs/petrov_passport.pdf	2026-04-15 09:30:43.735487
2	1	1	2	petrov_photo.jpg	/files/photos/petrov.jpg	2026-04-15 09:30:43.735487
3	2	2	1	sidorova_passport.pdf	/files/docs/sidorova_passport.pdf	2026-04-15 09:30:43.735487
4	2	2	2	sidorova_photo.jpg	/files/photos/sidorova.jpg	2026-04-15 09:30:43.735487
5	3	\N	3	group_list.xlsx	/files/docs/group_list.xlsx	2026-04-15 09:30:43.735487
6	3	3	1	vasiliev_passport.pdf	/files/docs/vasiliev_passport.pdf	2026-04-15 09:30:43.735487
7	3	4	1	morozova_passport.pdf	/files/docs/morozova_passport.pdf	2026-04-15 09:30:43.735487
8	3	5	1	fedorov_passport.pdf	/files/docs/fedorov_passport.pdf	2026-04-15 09:30:43.735487
9	3	6	1	nikitina_passport.pdf	/files/docs/nikitina_passport.pdf	2026-04-15 09:30:43.735487
10	3	7	1	orekhov_passport.pdf	/files/docs/orekhov_passport.pdf	2026-04-15 09:30:43.735487
11	4	8	1	Vlas86_passport.pdf	/files/docs/Vlas86_passport.pdf	2026-04-15 10:01:05.982897
12	4	8	2	Vlas86.jpg	/files/photos/Vlas86.jpg	2026-04-15 10:01:05.982897
13	5	9	1	Prohor156_passport.pdf	/files/docs/Prohor156_passport.pdf	2026-04-15 10:01:05.982897
14	5	9	2	Prohor156.jpg	/files/photos/Prohor156.jpg	2026-04-15 10:01:05.982897
15	6	10	1	YUrin155_passport.pdf	/files/docs/YUrin155_passport.pdf	2026-04-15 10:01:05.982897
16	6	10	2	YUrin155.jpg	/files/photos/YUrin155.jpg	2026-04-15 10:01:05.982897
17	7	11	1	Aljbina33_passport.pdf	/files/docs/Aljbina33_passport.pdf	2026-04-15 10:01:05.982897
18	7	11	2	Aljbina33.jpg	/files/photos/Aljbina33.jpg	2026-04-15 10:01:05.982897
19	8	12	1	Klavdiya113_passport.pdf	/files/docs/Klavdiya113_passport.pdf	2026-04-15 10:01:05.982897
20	8	12	2	Klavdiya113.jpg	/files/photos/Klavdiya113.jpg	2026-04-15 10:01:05.982897
21	9	13	1	Tamara179_passport.pdf	/files/docs/Tamara179_passport.pdf	2026-04-15 10:01:05.982897
22	9	13	2	Tamara179.jpg	/files/photos/Tamara179.jpg	2026-04-15 10:01:05.982897
23	10	14	1	Taras24_passport.pdf	/files/docs/Taras24_passport.pdf	2026-04-15 10:01:05.982897
24	10	14	2	Taras24.jpg	/files/photos/Taras24.jpg	2026-04-15 10:01:05.982897
25	11	15	1	Arkadij123_passport.pdf	/files/docs/Arkadij123_passport.pdf	2026-04-15 10:01:05.982897
26	11	15	2	Arkadij123.jpg	/files/photos/Arkadij123.jpg	2026-04-15 10:01:05.982897
27	12	16	1	Glafira73_passport.pdf	/files/docs/Glafira73_passport.pdf	2026-04-15 10:01:05.982897
28	12	16	2	Glafira73.jpg	/files/photos/Glafira73.jpg	2026-04-15 10:01:05.982897
29	13	17	1	Gavriila68_passport.pdf	/files/docs/Gavriila68_passport.pdf	2026-04-15 10:01:05.982897
30	13	17	2	Gavriila68.jpg	/files/photos/Gavriila68.jpg	2026-04-15 10:01:05.982897
31	14	18	1	Kuzjma124_passport.pdf	/files/docs/Kuzjma124_passport.pdf	2026-04-15 10:01:05.982897
32	14	18	2	Kuzjma124.jpg	/files/photos/Kuzjma124.jpg	2026-04-15 10:01:05.982897
33	15	19	1	Roman89_passport.pdf	/files/docs/Roman89_passport.pdf	2026-04-15 10:01:05.982897
34	15	19	2	Roman89.jpg	/files/photos/Roman89.jpg	2026-04-15 10:01:05.982897
35	16	20	1	Aleksej43_passport.pdf	/files/docs/Aleksej43_passport.pdf	2026-04-15 10:01:05.982897
36	16	20	2	Aleksej43.jpg	/files/photos/Aleksej43.jpg	2026-04-15 10:01:05.982897
37	17	21	1	Nadezhda137_passport.pdf	/files/docs/Nadezhda137_passport.pdf	2026-04-15 10:01:05.982897
38	17	21	2	Nadezhda137.jpg	/files/photos/Nadezhda137.jpg	2026-04-15 10:01:05.982897
39	18	22	1	Bronislava56_passport.pdf	/files/docs/Bronislava56_passport.pdf	2026-04-15 10:01:05.982897
40	18	22	2	Bronislava56.jpg	/files/photos/Bronislava56.jpg	2026-04-15 10:01:05.982897
41	19	23	1	Taisiya177_passport.pdf	/files/docs/Taisiya177_passport.pdf	2026-04-15 10:01:05.982897
42	19	23	2	Taisiya177.jpg	/files/photos/Taisiya177.jpg	2026-04-15 10:01:05.982897
43	19	24	1	Adelaida20_passport.pdf	/files/docs/Adelaida20_passport.pdf	2026-04-15 10:01:05.982897
44	19	24	2	Adelaida20.jpg	/files/photos/Adelaida20.jpg	2026-04-15 10:01:05.982897
45	19	25	1	Lev131_passport.pdf	/files/docs/Lev131_passport.pdf	2026-04-15 10:01:05.982897
46	19	25	2	Lev131.jpg	/files/photos/Lev131.jpg	2026-04-15 10:01:05.982897
47	19	26	1	lzaihtvkdn_passport.pdf	/files/docs/lzaihtvkdn_passport.pdf	2026-04-15 10:01:05.982897
48	19	26	2	lzaihtvkdn.jpg	/files/photos/lzaihtvkdn.jpg	2026-04-15 10:01:05.982897
49	19	27	1	Lyudmila123_passport.pdf	/files/docs/Lyudmila123_passport.pdf	2026-04-15 10:01:05.982897
50	19	27	2	Lyudmila123.jpg	/files/photos/Lyudmila123.jpg	2026-04-15 10:01:05.982897
51	19	28	1	Taisiya176_passport.pdf	/files/docs/Taisiya176_passport.pdf	2026-04-15 10:01:05.982897
52	19	28	2	Taisiya176.jpg	/files/photos/Taisiya176.jpg	2026-04-15 10:01:05.982897
53	19	29	1	Vera195_passport.pdf	/files/docs/Vera195_passport.pdf	2026-04-15 10:01:05.982897
54	19	29	2	Vera195.jpg	/files/photos/Vera195.jpg	2026-04-15 10:01:05.982897
55	20	30	1	YAkov196_passport.pdf	/files/docs/YAkov196_passport.pdf	2026-04-15 10:01:05.982897
56	20	30	2	YAkov196.jpg	/files/photos/YAkov196.jpg	2026-04-15 10:01:05.982897
57	20	31	1	Nina145_passport.pdf	/files/docs/Nina145_passport.pdf	2026-04-15 10:01:05.982897
58	20	31	2	Nina145.jpg	/files/photos/Nina145.jpg	2026-04-15 10:01:05.982897
59	20	32	1	Leontij161_passport.pdf	/files/docs/Leontij161_passport.pdf	2026-04-15 10:01:05.982897
60	20	32	2	Leontij161.jpg	/files/photos/Leontij161.jpg	2026-04-15 10:01:05.982897
61	20	33	1	Serafima169_passport.pdf	/files/docs/Serafima169_passport.pdf	2026-04-15 10:01:05.982897
62	20	33	2	Serafima169.jpg	/files/photos/Serafima169.jpg	2026-04-15 10:01:05.982897
63	20	34	1	Sergej35_passport.pdf	/files/docs/Sergej35_passport.pdf	2026-04-15 10:01:05.982897
64	20	34	2	Sergej35.jpg	/files/photos/Sergej35.jpg	2026-04-15 10:01:05.982897
65	20	35	1	Georgij121_passport.pdf	/files/docs/Georgij121_passport.pdf	2026-04-15 10:01:05.982897
66	20	35	2	Georgij121.jpg	/files/photos/Georgij121.jpg	2026-04-15 10:01:05.982897
67	20	36	1	Elizar30_passport.pdf	/files/docs/Elizar30_passport.pdf	2026-04-15 10:01:05.982897
68	20	36	2	Elizar30.jpg	/files/photos/Elizar30.jpg	2026-04-15 10:01:05.982897
69	20	37	1	Lana117_passport.pdf	/files/docs/Lana117_passport.pdf	2026-04-15 10:01:05.982897
70	20	37	2	Lana117.jpg	/files/photos/Lana117.jpg	2026-04-15 10:01:05.982897
71	19	\N	3	24/04/2023_Производство_Фомичева_9367788_ГР1.xlsx	/files/groups/24/04/2023_Производство_Фомичева_9367788_ГР1.xlsx	2026-04-15 10:01:05.982897
72	20	\N	3	24/04/2023_Производство_Фомичева_9367788_ГР2.xlsx	/files/groups/24/04/2023_Производство_Фомичева_9367788_ГР2.xlsx	2026-04-15 10:01:05.982897
\.


--
-- TOC entry 5139 (class 0 OID 16439)
-- Dependencies: 227
-- Data for Name: employees; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.employees (employee_id, department_id, last_name, first_name, middle_name, "position", phone, email, is_active) FROM stdin;
1	1	Иванов	Петр	Сергеевич	Специалист отдела кадров	+7 (900) 111-11-11	ivanov@keeperpro.local	t
2	2	Смирнова	Анна	Викторовна	Бухгалтер	+7 (900) 222-22-22	smirnova@keeperpro.local	t
3	3	Кузнецов	Дмитрий	Олегович	Системный администратор	+7 (900) 333-33-33	kuznetsov@keeperpro.local	t
4	4	Соколов	Игорь	Павлович	Охранник	+7 (900) 444-44-44	sokolov@keeperpro.local	t
5	5	Орлова	Марина	Игоревна	Секретарь	+7 (900) 555-55-55	orlova@keeperpro.local	t
6	6	Фомичева	Авдотья	Трофимовна	Сотрудник	\N	emp_9367788@keeperpro.local	t
7	7	Гаврилова	Римма	Ефимовна	Сотрудник	\N	emp_9788737@keeperpro.local	t
8	5	Носкова	Наталия	Прохоровна	Сотрудник	\N	emp_9736379@keeperpro.local	t
9	4	Архипов	Тимофей	Васильевич	Сотрудник	\N	emp_9362832@keeperpro.local	t
10	10	Орехова	Вероника	Артемовна	Сотрудник	\N	emp_9737848@keeperpro.local	t
11	11	Савельев	Павел	Степанович	Сотрудник	\N	emp_9768239@keeperpro.local	t
12	12	Чернов	Всеволод	Наумович	Сотрудник	\N	emp_9404040@keeperpro.local	t
\.


--
-- TOC entry 5133 (class 0 OID 16391)
-- Dependencies: 221
-- Data for Name: roles; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.roles (role_id, role_name) FROM stdin;
1	guest
2	general_department_employee
3	security_employee
4	department_employee
5	admin
\.


--
-- TOC entry 5135 (class 0 OID 16402)
-- Dependencies: 223
-- Data for Name: users; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.users (user_id, email, password_hash, role_id, created_at, is_active) FROM stdin;
1	user1@mail.ru	0cef1fb10f60529028a71f58e54ed07b	1	2026-04-15 09:30:43.735487	t
2	user2@mail.ru	022b5ac7ea72a5ee3bfc6b3eb461f2fc	1	2026-04-15 09:30:43.735487	t
3	user3@mail.ru	94ca112be7fc3f3934c45c6809875168	1	2026-04-15 09:30:43.735487	t
4	office1@keeperpro.local	3e8cbc342cfd883c3eafef084abe1e0a	2	2026-04-15 09:30:43.735487	t
5	security1@keeperpro.local	78488d1fa917f6af4cc95df53307cf82	3	2026-04-15 09:30:43.735487	t
6	dept1@keeperpro.local	1395a03cb3f2e0c9813402c91f48e902	4	2026-04-15 09:30:43.735487	t
7	radinka100@yandex.ru	f2eb7eac111e507e1f4310fbca48c1b4	1	2026-04-15 10:01:05.982897	t
8	prohor156@list.ru	6e23aa05abe3646195df7f6c06a59702	1	2026-04-15 10:01:05.982897	t
9	yurin155@gmail.com	f9259b3e7f7e3dddae64f20eb0e6b971	1	2026-04-15 10:01:05.982897	t
10	aljbina33@lenta.ru	b570dc22d2f243d1ff7da92cef593f91	1	2026-04-15 10:01:05.982897	t
11	klavdiya113@live.com	9936c8f149921d756c3c3d2ed78b30f1	1	2026-04-15 10:01:05.982897	t
12	tamara179@live.com	0b1341da3c137913d05e69094afbe755	1	2026-04-15 10:01:05.982897	t
13	taras24@rambler.ru	3fd21450485e41d8b37718ea2ff5dab8	1	2026-04-15 10:01:05.982897	t
14	arkadij123@inbox.ru	56f5a74d5b7013d1cd20d43d67b98c69	1	2026-04-15 10:01:05.982897	t
15	glafira73@outlook.com	409348a3cff8264cbcb4407e55a2eff0	1	2026-04-15 10:01:05.982897	t
16	gavriila68@msn.com	b2df6c2886befac08929c56905b183c3	1	2026-04-15 10:01:05.982897	t
17	kuzjma124@yandex.ru	ec5402ef81b24120e36630b2c4c4ffc7	1	2026-04-15 10:01:05.982897	t
18	roman89@gmail.com	dc07a3f9e45ba90e18a2e3ecef3dcd04	1	2026-04-15 10:01:05.982897	t
19	aleksej43@gmail.com	06e3108cbe003a2992c866117262ee72	1	2026-04-15 10:01:05.982897	t
20	nadezhda137@outlook.com	019239d5a96d7d7eef8fa5f0138f4af1	1	2026-04-15 10:01:05.982897	t
21	bronislava56@yahoo.com	2ecef85244ac1671806d8c1936e27f0d	1	2026-04-15 10:01:05.982897	t
22	taisiya177@lenta.ru	fa3507cad2500ca8f551932b44a1d0ff	1	2026-04-15 10:01:05.982897	t
23	adelaida20@hotmail.com	bc5195aa7e7e9089065b57e17639b86f	1	2026-04-15 10:01:05.982897	t
24	lev131@rambler.ru	8d2434100b4ef155c75e2693842b21a3	1	2026-04-15 10:01:05.982897	t
25	daniil198@bk.ru	af0dc9fe72a0add56dacc0124a184946	1	2026-04-15 10:01:05.982897	t
26	lyudmila123@hotmail.com	28af582469ee65604f082f9297b0a227	1	2026-04-15 10:01:05.982897	t
27	taisiya176@hotmail.com	be630902a553ac8cf2859dd95d5b6606	1	2026-04-15 10:01:05.982897	t
28	vera195@list.ru	3f5b98dde208683b0815c5405a9e8dad	1	2026-04-15 10:01:05.982897	t
29	yakov196@rambler.ru	4ced337387b6e838a5a46e63d19ea292	1	2026-04-15 10:01:05.982897	t
30	nina145@msn.com	446a7f47b0850cd4f7fe00509f5d1f52	1	2026-04-15 10:01:05.982897	t
31	leontij161@mail.ru	ba31a68b36b48f1d65b2cb992655ddde	1	2026-04-15 10:01:05.982897	t
32	serafima169@yahoo.com	3576d70bd143ae6b3285e231abc12833	1	2026-04-15 10:01:05.982897	t
33	sergej35@inbox.ru	e12f4d7805d9a07087594dcd80161f4f	1	2026-04-15 10:01:05.982897	t
34	georgij121@inbox.ru	f8a925501f7949bfc652f90cfe5df903	1	2026-04-15 10:01:05.982897	t
35	elizar30@yandex.ru	85bb40863aea1f729f74f38d9cebfa78	1	2026-04-15 10:01:05.982897	t
36	lana117@outlook.com	a5d100e6c4b1a7f695bcec7ca079b71b	1	2026-04-15 10:01:05.982897	t
\.


--
-- TOC entry 5155 (class 0 OID 16611)
-- Dependencies: 243
-- Data for Name: visit_logs; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.visit_logs (visit_log_id, application_id, visitor_id, security_employee_id, department_employee_id, entry_time, exit_time, comment) FROM stdin;
1	1	1	4	1	2026-04-16 09:30:43.735487	2026-04-16 11:30:43.735487	Посещение завершено
2	3	3	4	5	2026-04-18 09:30:43.735487	2026-04-18 12:30:43.735487	Первый посетитель группы
3	3	4	4	5	2026-04-18 09:30:43.735487	2026-04-18 12:30:43.735487	Второй посетитель группы
4	4	8	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
5	5	9	12	7	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
6	6	10	12	8	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
7	7	11	12	6	2023-04-25 09:00:00	2023-04-25 11:00:00	Импорт из файла Сессия 1.xlsx
8	8	12	12	7	2023-04-25 09:00:00	2023-04-25 11:00:00	Импорт из файла Сессия 1.xlsx
9	9	13	12	8	2023-04-25 09:00:00	2023-04-25 11:00:00	Импорт из файла Сессия 1.xlsx
10	10	14	12	6	2023-04-26 09:00:00	2023-04-26 11:00:00	Импорт из файла Сессия 1.xlsx
11	11	15	12	7	2023-04-26 09:00:00	2023-04-26 11:00:00	Импорт из файла Сессия 1.xlsx
12	12	16	12	8	2023-04-26 09:00:00	2023-04-26 11:00:00	Импорт из файла Сессия 1.xlsx
13	13	17	12	6	2023-04-27 09:00:00	2023-04-27 11:00:00	Импорт из файла Сессия 1.xlsx
14	14	18	12	7	2023-04-27 09:00:00	2023-04-27 11:00:00	Импорт из файла Сессия 1.xlsx
15	15	19	12	8	2023-04-27 09:00:00	2023-04-27 11:00:00	Импорт из файла Сессия 1.xlsx
16	16	20	12	6	2023-04-28 09:00:00	2023-04-28 11:00:00	Импорт из файла Сессия 1.xlsx
17	17	21	12	7	2023-04-28 09:00:00	2023-04-28 11:00:00	Импорт из файла Сессия 1.xlsx
18	18	22	12	8	2023-04-28 09:00:00	2023-04-28 11:00:00	Импорт из файла Сессия 1.xlsx
19	19	23	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
20	19	24	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
21	19	25	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
22	19	26	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
23	19	27	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
24	19	28	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
25	19	29	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
26	20	30	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
27	20	31	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
28	20	32	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
29	20	33	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
30	20	34	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
31	20	35	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
32	20	36	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
33	20	37	12	6	2023-04-24 09:00:00	2023-04-24 11:00:00	Импорт из файла Сессия 1.xlsx
\.


--
-- TOC entry 5145 (class 0 OID 16481)
-- Dependencies: 233
-- Data for Name: visitors; Type: TABLE DATA; Schema: keeperpro; Owner: postgres
--

COPY keeperpro.visitors (visitor_id, last_name, first_name, middle_name, phone, email, organization, birth_date, passport_series, passport_number, photo_path) FROM stdin;
1	Петров	Алексей	Игоревич	+7 (901) 111-11-11	petrov@mail.ru	ООО Альфа	1998-05-10	1234	123456	/files/photos/petrov.jpg
2	Сидорова	Елена	Павловна	+7 (901) 222-22-22	sidorova@mail.ru	ООО Бета	1995-07-12	2345	234567	/files/photos/sidorova.jpg
3	Васильев	Николай	Андреевич	+7 (901) 333-33-33	vasiliev@mail.ru	ООО Гамма	1990-09-15	3456	345678	/files/photos/vasiliev.jpg
4	Морозова	Ольга	Сергеевна	+7 (901) 444-44-44	morozova@mail.ru	ООО Дельта	1992-03-18	4567	456789	/files/photos/morozova.jpg
5	Федоров	Кирилл	Олегович	+7 (901) 555-55-55	fedorov@mail.ru	ООО Омега	1997-11-22	5678	567890	/files/photos/fedorov.jpg
6	Никитина	Мария	Ивановна	+7 (901) 666-66-66	nikitina@mail.ru	ООО Альфа	1999-12-01	6789	678901	/files/photos/nikitina.jpg
7	Орехов	Павел	Романович	+7 (901) 777-77-77	orekhov@mail.ru	ООО Бета	1994-01-14	7890	789012	/files/photos/orekhov.jpg
8	Степанова	Радинка	Власовна	+7 (613) 272-60-62	radinka100@yandex.ru	\N	1986-10-18	0208	530509	/files/photos/Vlas86.jpg
9	Шилов	Прохор	Герасимович	+7 (615) 594-77-66	prohor156@list.ru	\N	1977-10-09	3036	796488	/files/photos/Prohor156.jpg
10	Кононов	Юрин	Романович	+7 (784) 673-51-91	yurin155@gmail.com	\N	1971-10-08	2747	790512	/files/photos/YUrin155.jpg
11	Елисеева	Альбина	Николаевна	+7 (654) 864-77-46	aljbina33@lenta.ru	\N	1983-02-15	5241	213304	/files/photos/Aljbina33.jpg
12	Шарова	Клавдия	Макаровна	+7 (822) 525-82-40	klavdiya113@live.com	\N	1980-07-22	8143	593309	/files/photos/Klavdiya113.jpg
13	Сидорова	Тамара	Григорьевна	+7 (334) 692-79-77	tamara179@live.com	\N	1995-11-22	8143	905520	/files/photos/Tamara179.jpg
14	Петухов	Тарас	Фадеевич	+7 (376) 220-62-51	taras24@rambler.ru	\N	1991-01-05	1609	171096	/files/photos/Taras24.jpg
15	Родионов	Аркадий	Власович	+7 (491) 696-17-11	arkadij123@inbox.ru	\N	1993-08-11	3841	642594	/files/photos/Arkadij123.jpg
16	Горшкова	Глафира	Валентиновна	+7 (553) 343-38-82	glafira73@outlook.com	\N	1978-05-25	9170	402601	/files/photos/Glafira73.jpg
17	Кириллова	Гавриила	Яковна	+7 (648) 700-43-34	gavriila68@msn.com	\N	1992-04-26	9438	379667	/files/photos/Gavriila68.jpg
18	Овчинников	Кузьма	Ефимович	+7 (562) 866-15-27	kuzjma124@yandex.ru	\N	1993-08-02	0766	647226	/files/photos/Kuzjma124.jpg
19	Беляков	Роман	Викторович	+7 (595) 196-56-28	roman89@gmail.com	\N	1991-06-07	2411	478305	/files/photos/Roman89.jpg
20	Лыткин	Алексей	Максимович	+7 (994) 353-29-52	aleksej43@gmail.com	\N	1996-03-07	2383	259825	/files/photos/Aleksej43.jpg
21	Шубина	Надежда	Викторовна	+7 (736) 488-66-95	nadezhda137@outlook.com	\N	1981-09-24	8844	708476	/files/photos/Nadezhda137.jpg
22	Зиновьева	Бронислава	Викторовна	+7 (778) 565-12-18	bronislava56@yahoo.com	\N	1981-03-19	6736	319423	/files/photos/Bronislava56.jpg
23	Самойлова	Таисия	Гермоновна	+7 (891) 555-81-44	taisiya177@lenta.ru	\N	1979-11-14	5193	897719	/files/photos/Taisiya177.jpg
24	Ситникова	Аделаида	Гермоновна	+7 (793) 736-70-31	adelaida20@hotmail.com	\N	1979-01-21	7561	148016	/files/photos/Adelaida20.jpg
25	Исаев	Лев	Юлианович	+7 (675) 593-89-30	lev131@rambler.ru	\N	1994-08-05	1860	680004	/files/photos/Lev131.jpg
26	Никифоров	Даниил	Степанович	+7 (384) 358-77-82	daniil198@bk.ru	\N	1970-12-13	4557	999958	/files/photos/lzaihtvkdn.jpg
27	Титова	Людмила	Якововна	+7 (221) 729-16-84	lyudmila123@hotmail.com	\N	1976-08-21	7715	639425	/files/photos/Lyudmila123.jpg
28	Абрамова	Таисия	Дмитриевна	+7 (528) 312-18-20	taisiya176@hotmail.com	\N	1982-11-20	7310	893510	/files/photos/Taisiya176.jpg
29	Кузьмина	Вера	Максимовна	+7 (598) 583-53-45	vera195@list.ru	\N	1989-12-10	3537	982933	/files/photos/Vera195.jpg
30	Мартынов	Яков	Ростиславович	+7 (546) 159-67-33	yakov196@rambler.ru	\N	1976-12-05	1793	986063	/files/photos/YAkov196.jpg
31	Евсеева	Нина	Павловна	+7 (833) 521-31-50	nina145@msn.com	\N	1994-09-26	9323	745717	/files/photos/Nina145.jpg
32	Голубев	Леонтий	Вячеславович	+7 (160) 527-57-41	leontij161@mail.ru	\N	1990-10-03	1059	822077	/files/photos/Leontij161.jpg
33	Карпова	Серафима	Михаиловна	+7 (459) 930-91-70	serafima169@yahoo.com	\N	1989-11-19	7034	858987	/files/photos/Serafima169.jpg
34	Орехов	Сергей	Емельянович	+7 (669) 603-29-87	sergej35@inbox.ru	\N	1972-02-11	3844	223682	/files/photos/Sergej35.jpg
35	Исаев	Георгий	Павлович	+7 (678) 516-36-86	georgij121@inbox.ru	\N	1987-08-11	4076	629809	/files/photos/Georgij121.jpg
36	Богданов	Елизар	Артемович	+7 (165) 768-30-97	elizar30@yandex.ru	\N	1978-02-02	0573	198559	/files/photos/Elizar30.jpg
37	Тихонова	Лана	Семеновна	+7 (478) 467-75-15	lana117@outlook.com	\N	1990-07-24	8761	609740	/files/photos/Lana117.jpg
\.


--
-- TOC entry 5174 (class 0 OID 0)
-- Dependencies: 230
-- Name: application_statuses_status_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.application_statuses_status_id_seq', 8, true);


--
-- TOC entry 5175 (class 0 OID 0)
-- Dependencies: 228
-- Name: application_types_application_type_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.application_types_application_type_id_seq', 4, true);


--
-- TOC entry 5176 (class 0 OID 0)
-- Dependencies: 236
-- Name: application_visitors_application_visitor_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.application_visitors_application_visitor_id_seq', 37, true);


--
-- TOC entry 5177 (class 0 OID 0)
-- Dependencies: 234
-- Name: applications_application_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.applications_application_id_seq', 20, true);


--
-- TOC entry 5178 (class 0 OID 0)
-- Dependencies: 224
-- Name: departments_department_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.departments_department_id_seq', 12, true);


--
-- TOC entry 5179 (class 0 OID 0)
-- Dependencies: 238
-- Name: document_types_document_type_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.document_types_document_type_id_seq', 6, true);


--
-- TOC entry 5180 (class 0 OID 0)
-- Dependencies: 240
-- Name: documents_document_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.documents_document_id_seq', 72, true);


--
-- TOC entry 5181 (class 0 OID 0)
-- Dependencies: 226
-- Name: employees_employee_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.employees_employee_id_seq', 12, true);


--
-- TOC entry 5182 (class 0 OID 0)
-- Dependencies: 220
-- Name: roles_role_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.roles_role_id_seq', 10, true);


--
-- TOC entry 5183 (class 0 OID 0)
-- Dependencies: 222
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.users_user_id_seq', 36, true);


--
-- TOC entry 5184 (class 0 OID 0)
-- Dependencies: 242
-- Name: visit_logs_visit_log_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.visit_logs_visit_log_id_seq', 33, true);


--
-- TOC entry 5185 (class 0 OID 0)
-- Dependencies: 232
-- Name: visitors_visitor_id_seq; Type: SEQUENCE SET; Schema: keeperpro; Owner: postgres
--

SELECT pg_catalog.setval('keeperpro.visitors_visitor_id_seq', 37, true);


--
-- TOC entry 4950 (class 2606 OID 16477)
-- Name: application_statuses application_statuses_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_statuses
    ADD CONSTRAINT application_statuses_pkey PRIMARY KEY (status_id);


--
-- TOC entry 4952 (class 2606 OID 16479)
-- Name: application_statuses application_statuses_status_name_key; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_statuses
    ADD CONSTRAINT application_statuses_status_name_key UNIQUE (status_name);


--
-- TOC entry 4946 (class 2606 OID 16466)
-- Name: application_types application_types_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_types
    ADD CONSTRAINT application_types_pkey PRIMARY KEY (application_type_id);


--
-- TOC entry 4948 (class 2606 OID 16468)
-- Name: application_types application_types_type_name_key; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_types
    ADD CONSTRAINT application_types_type_name_key UNIQUE (type_name);


--
-- TOC entry 4958 (class 2606 OID 16557)
-- Name: application_visitors application_visitors_application_id_visitor_id_key; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_visitors
    ADD CONSTRAINT application_visitors_application_id_visitor_id_key UNIQUE (application_id, visitor_id);


--
-- TOC entry 4960 (class 2606 OID 16555)
-- Name: application_visitors application_visitors_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_visitors
    ADD CONSTRAINT application_visitors_pkey PRIMARY KEY (application_visitor_id);


--
-- TOC entry 4956 (class 2606 OID 16519)
-- Name: applications applications_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.applications
    ADD CONSTRAINT applications_pkey PRIMARY KEY (application_id);


--
-- TOC entry 4940 (class 2606 OID 16437)
-- Name: departments departments_department_name_key; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.departments
    ADD CONSTRAINT departments_department_name_key UNIQUE (department_name);


--
-- TOC entry 4942 (class 2606 OID 16435)
-- Name: departments departments_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.departments
    ADD CONSTRAINT departments_pkey PRIMARY KEY (department_id);


--
-- TOC entry 4962 (class 2606 OID 16578)
-- Name: document_types document_types_document_type_name_key; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.document_types
    ADD CONSTRAINT document_types_document_type_name_key UNIQUE (document_type_name);


--
-- TOC entry 4964 (class 2606 OID 16576)
-- Name: document_types document_types_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.document_types
    ADD CONSTRAINT document_types_pkey PRIMARY KEY (document_type_id);


--
-- TOC entry 4966 (class 2606 OID 16594)
-- Name: documents documents_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.documents
    ADD CONSTRAINT documents_pkey PRIMARY KEY (document_id);


--
-- TOC entry 4944 (class 2606 OID 16452)
-- Name: employees employees_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.employees
    ADD CONSTRAINT employees_pkey PRIMARY KEY (employee_id);


--
-- TOC entry 4932 (class 2606 OID 16398)
-- Name: roles roles_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (role_id);


--
-- TOC entry 4934 (class 2606 OID 16400)
-- Name: roles roles_role_name_key; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.roles
    ADD CONSTRAINT roles_role_name_key UNIQUE (role_name);


--
-- TOC entry 4936 (class 2606 OID 16419)
-- Name: users users_email_key; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- TOC entry 4938 (class 2606 OID 16417)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- TOC entry 4968 (class 2606 OID 16621)
-- Name: visit_logs visit_logs_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.visit_logs
    ADD CONSTRAINT visit_logs_pkey PRIMARY KEY (visit_log_id);


--
-- TOC entry 4954 (class 2606 OID 16495)
-- Name: visitors visitors_pkey; Type: CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.visitors
    ADD CONSTRAINT visitors_pkey PRIMARY KEY (visitor_id);


--
-- TOC entry 4976 (class 2606 OID 16558)
-- Name: application_visitors application_visitors_application_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_visitors
    ADD CONSTRAINT application_visitors_application_id_fkey FOREIGN KEY (application_id) REFERENCES keeperpro.applications(application_id) ON DELETE CASCADE;


--
-- TOC entry 4977 (class 2606 OID 16563)
-- Name: application_visitors application_visitors_visitor_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.application_visitors
    ADD CONSTRAINT application_visitors_visitor_id_fkey FOREIGN KEY (visitor_id) REFERENCES keeperpro.visitors(visitor_id) ON DELETE CASCADE;


--
-- TOC entry 4971 (class 2606 OID 16525)
-- Name: applications applications_application_type_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.applications
    ADD CONSTRAINT applications_application_type_id_fkey FOREIGN KEY (application_type_id) REFERENCES keeperpro.application_types(application_type_id);


--
-- TOC entry 4972 (class 2606 OID 16530)
-- Name: applications applications_department_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.applications
    ADD CONSTRAINT applications_department_id_fkey FOREIGN KEY (department_id) REFERENCES keeperpro.departments(department_id);


--
-- TOC entry 4973 (class 2606 OID 16535)
-- Name: applications applications_employee_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.applications
    ADD CONSTRAINT applications_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES keeperpro.employees(employee_id);


--
-- TOC entry 4974 (class 2606 OID 16540)
-- Name: applications applications_status_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.applications
    ADD CONSTRAINT applications_status_id_fkey FOREIGN KEY (status_id) REFERENCES keeperpro.application_statuses(status_id);


--
-- TOC entry 4975 (class 2606 OID 16520)
-- Name: applications applications_user_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.applications
    ADD CONSTRAINT applications_user_id_fkey FOREIGN KEY (user_id) REFERENCES keeperpro.users(user_id);


--
-- TOC entry 4978 (class 2606 OID 16595)
-- Name: documents documents_application_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.documents
    ADD CONSTRAINT documents_application_id_fkey FOREIGN KEY (application_id) REFERENCES keeperpro.applications(application_id) ON DELETE CASCADE;


--
-- TOC entry 4979 (class 2606 OID 16605)
-- Name: documents documents_document_type_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.documents
    ADD CONSTRAINT documents_document_type_id_fkey FOREIGN KEY (document_type_id) REFERENCES keeperpro.document_types(document_type_id);


--
-- TOC entry 4980 (class 2606 OID 16600)
-- Name: documents documents_visitor_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.documents
    ADD CONSTRAINT documents_visitor_id_fkey FOREIGN KEY (visitor_id) REFERENCES keeperpro.visitors(visitor_id) ON DELETE CASCADE;


--
-- TOC entry 4970 (class 2606 OID 16453)
-- Name: employees employees_department_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.employees
    ADD CONSTRAINT employees_department_id_fkey FOREIGN KEY (department_id) REFERENCES keeperpro.departments(department_id);


--
-- TOC entry 4969 (class 2606 OID 16420)
-- Name: users users_role_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.users
    ADD CONSTRAINT users_role_id_fkey FOREIGN KEY (role_id) REFERENCES keeperpro.roles(role_id);


--
-- TOC entry 4981 (class 2606 OID 16622)
-- Name: visit_logs visit_logs_application_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.visit_logs
    ADD CONSTRAINT visit_logs_application_id_fkey FOREIGN KEY (application_id) REFERENCES keeperpro.applications(application_id) ON DELETE CASCADE;


--
-- TOC entry 4982 (class 2606 OID 16637)
-- Name: visit_logs visit_logs_department_employee_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.visit_logs
    ADD CONSTRAINT visit_logs_department_employee_id_fkey FOREIGN KEY (department_employee_id) REFERENCES keeperpro.employees(employee_id);


--
-- TOC entry 4983 (class 2606 OID 16632)
-- Name: visit_logs visit_logs_security_employee_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.visit_logs
    ADD CONSTRAINT visit_logs_security_employee_id_fkey FOREIGN KEY (security_employee_id) REFERENCES keeperpro.employees(employee_id);


--
-- TOC entry 4984 (class 2606 OID 16627)
-- Name: visit_logs visit_logs_visitor_id_fkey; Type: FK CONSTRAINT; Schema: keeperpro; Owner: postgres
--

ALTER TABLE ONLY keeperpro.visit_logs
    ADD CONSTRAINT visit_logs_visitor_id_fkey FOREIGN KEY (visitor_id) REFERENCES keeperpro.visitors(visitor_id) ON DELETE CASCADE;


-- Completed on 2026-04-15 13:15:15

--
-- PostgreSQL database dump complete
--

\unrestrict pjeTTx1dkKR4MgNa4cgR23vco6ZIxE0mT20zkvgB8an7jAinysk7UP2wYIxUfQ7

