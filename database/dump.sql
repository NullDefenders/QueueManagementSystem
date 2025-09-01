--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: DepartmentServiceCategory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."DepartmentServiceCategory" (
    "ServiceCategoryId" uuid NOT NULL,
    "DepartmentId" uuid NOT NULL
);


ALTER TABLE public."DepartmentServiceCategory" OWNER TO postgres;

--
-- Name: Departments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Departments" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "FacilityId" uuid NOT NULL,
    "Code" text NOT NULL,
    "Name" text NOT NULL,
    "Address" text NOT NULL,
    "AllowScheduledAppointments" boolean NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone
);


ALTER TABLE public."Departments" OWNER TO postgres;

--
-- Name: Facilities; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Facilities" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Code" text NOT NULL,
    "Name" text NOT NULL,
    "Address" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone
);


ALTER TABLE public."Facilities" OWNER TO postgres;

--
-- Name: NonWorkingPeriod; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."NonWorkingPeriod" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "ScheduleId" uuid NOT NULL,
    "DayOfWeek" integer NOT NULL,
    "StartTime" interval NOT NULL,
    "EndTime" interval NOT NULL
);


ALTER TABLE public."NonWorkingPeriod" OWNER TO postgres;

--
-- Name: Schedules; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Schedules" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "DepartmentId" uuid NOT NULL,
    "Name" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "WorkDayStart" interval NOT NULL,
    "WorkDayEnd" interval NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone
);


ALTER TABLE public."Schedules" OWNER TO postgres;

--
-- Name: ServiceCategories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ServiceCategories" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Code" text NOT NULL,
    "Name" text NOT NULL,
    "Prefix" text NOT NULL,
    "AllowScheduledAppointments" boolean NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone
);


ALTER TABLE public."ServiceCategories" OWNER TO postgres;

--
-- Name: Services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Services" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "ServiceCategoryId" uuid NOT NULL,
    "Code" text NOT NULL,
    "Name" text NOT NULL,
    "Duration" integer NOT NULL,
    "AllowScheduledAppointments" boolean NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone,
    "WorkplaceId" uuid
);


ALTER TABLE public."Services" OWNER TO postgres;

--
-- Name: WorkplaceServiceCategory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."WorkplaceServiceCategory" (
    "ServiceCategoryId" uuid NOT NULL,
    "WorkplaceId" uuid NOT NULL
);


ALTER TABLE public."WorkplaceServiceCategory" OWNER TO postgres;

--
-- Name: Workplaces; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Workplaces" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "DepartmentId" uuid NOT NULL,
    "Code" text NOT NULL,
    "Name" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone
);


ALTER TABLE public."Workplaces" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: DepartmentServiceCategory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."DepartmentServiceCategory" ("ServiceCategoryId", "DepartmentId") FROM stdin;
00c5569a-5dba-4df6-ae99-a9f63f124ae0	5570b277-fa5d-4267-80d5-1dcf47cfc18b
8f9e22f7-8b16-4400-a990-e64d0041f5d7	5570b277-fa5d-4267-80d5-1dcf47cfc18b
4c5469d2-503a-4acf-a952-b7f735730ae7	5570b277-fa5d-4267-80d5-1dcf47cfc18b
88bb0605-c019-4a56-ae51-850a05e0f197	5570b277-fa5d-4267-80d5-1dcf47cfc18b
eac0cb13-d732-4cee-8e77-efabc05b881a	5570b277-fa5d-4267-80d5-1dcf47cfc18b
00c5569a-5dba-4df6-ae99-a9f63f124ae0	9978d1c5-75a4-4082-9eca-50d6b1815394
8f9e22f7-8b16-4400-a990-e64d0041f5d7	9978d1c5-75a4-4082-9eca-50d6b1815394
4c5469d2-503a-4acf-a952-b7f735730ae7	9978d1c5-75a4-4082-9eca-50d6b1815394
88bb0605-c019-4a56-ae51-850a05e0f197	9978d1c5-75a4-4082-9eca-50d6b1815394
8eab4c30-5504-4724-9695-413344f268d3	9978d1c5-75a4-4082-9eca-50d6b1815394
00c5569a-5dba-4df6-ae99-a9f63f124ae0	4fa8236c-801a-45e6-a358-352ed56a88b5
8f9e22f7-8b16-4400-a990-e64d0041f5d7	4fa8236c-801a-45e6-a358-352ed56a88b5
dfaeea11-207c-4e44-8108-c7ed5f925ef7	4fa8236c-801a-45e6-a358-352ed56a88b5
eac0cb13-d732-4cee-8e77-efabc05b881a	4fa8236c-801a-45e6-a358-352ed56a88b5
791740bd-34bb-4d98-bd94-2fb48334a4fe	4fa8236c-801a-45e6-a358-352ed56a88b5
00c5569a-5dba-4df6-ae99-a9f63f124ae0	97237109-8e2e-44db-85c8-7ebae2faf6bc
8f9e22f7-8b16-4400-a990-e64d0041f5d7	97237109-8e2e-44db-85c8-7ebae2faf6bc
4c5469d2-503a-4acf-a952-b7f735730ae7	97237109-8e2e-44db-85c8-7ebae2faf6bc
fee4d224-d896-4c82-9561-853ca6af15f4	97237109-8e2e-44db-85c8-7ebae2faf6bc
791740bd-34bb-4d98-bd94-2fb48334a4fe	97237109-8e2e-44db-85c8-7ebae2faf6bc
00c5569a-5dba-4df6-ae99-a9f63f124ae0	84fdf6d5-4e0b-4077-bb7c-702782da545d
8f9e22f7-8b16-4400-a990-e64d0041f5d7	84fdf6d5-4e0b-4077-bb7c-702782da545d
88bb0605-c019-4a56-ae51-850a05e0f197	84fdf6d5-4e0b-4077-bb7c-702782da545d
eac0cb13-d732-4cee-8e77-efabc05b881a	84fdf6d5-4e0b-4077-bb7c-702782da545d
8eab4c30-5504-4724-9695-413344f268d3	84fdf6d5-4e0b-4077-bb7c-702782da545d
00c5569a-5dba-4df6-ae99-a9f63f124ae0	d4f39531-e7f0-42f8-8168-2ffa417b0dde
8f9e22f7-8b16-4400-a990-e64d0041f5d7	d4f39531-e7f0-42f8-8168-2ffa417b0dde
4c5469d2-503a-4acf-a952-b7f735730ae7	d4f39531-e7f0-42f8-8168-2ffa417b0dde
88bb0605-c019-4a56-ae51-850a05e0f197	d4f39531-e7f0-42f8-8168-2ffa417b0dde
eac0cb13-d732-4cee-8e77-efabc05b881a	d4f39531-e7f0-42f8-8168-2ffa417b0dde
00c5569a-5dba-4df6-ae99-a9f63f124ae0	676885cb-ee0f-40b4-be03-9d25a8a2dca1
8f9e22f7-8b16-4400-a990-e64d0041f5d7	676885cb-ee0f-40b4-be03-9d25a8a2dca1
dfaeea11-207c-4e44-8108-c7ed5f925ef7	676885cb-ee0f-40b4-be03-9d25a8a2dca1
eac0cb13-d732-4cee-8e77-efabc05b881a	676885cb-ee0f-40b4-be03-9d25a8a2dca1
8eab4c30-5504-4724-9695-413344f268d3	676885cb-ee0f-40b4-be03-9d25a8a2dca1
00c5569a-5dba-4df6-ae99-a9f63f124ae0	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e
8f9e22f7-8b16-4400-a990-e64d0041f5d7	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e
4c5469d2-503a-4acf-a952-b7f735730ae7	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e
fee4d224-d896-4c82-9561-853ca6af15f4	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e
791740bd-34bb-4d98-bd94-2fb48334a4fe	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e
00c5569a-5dba-4df6-ae99-a9f63f124ae0	764b87fb-1335-43b6-b753-40c01f3b7bb6
8f9e22f7-8b16-4400-a990-e64d0041f5d7	764b87fb-1335-43b6-b753-40c01f3b7bb6
88bb0605-c019-4a56-ae51-850a05e0f197	764b87fb-1335-43b6-b753-40c01f3b7bb6
eac0cb13-d732-4cee-8e77-efabc05b881a	764b87fb-1335-43b6-b753-40c01f3b7bb6
efe85715-4f41-4747-aa26-a86373add2a7	764b87fb-1335-43b6-b753-40c01f3b7bb6
00c5569a-5dba-4df6-ae99-a9f63f124ae0	775b5d0e-e161-4d6a-8137-74fc1c38e189
8f9e22f7-8b16-4400-a990-e64d0041f5d7	775b5d0e-e161-4d6a-8137-74fc1c38e189
4c5469d2-503a-4acf-a952-b7f735730ae7	775b5d0e-e161-4d6a-8137-74fc1c38e189
88bb0605-c019-4a56-ae51-850a05e0f197	775b5d0e-e161-4d6a-8137-74fc1c38e189
791740bd-34bb-4d98-bd94-2fb48334a4fe	775b5d0e-e161-4d6a-8137-74fc1c38e189
00c5569a-5dba-4df6-ae99-a9f63f124ae0	ab779ac4-a872-4c9a-accd-a1ee340a930a
8f9e22f7-8b16-4400-a990-e64d0041f5d7	ab779ac4-a872-4c9a-accd-a1ee340a930a
dfaeea11-207c-4e44-8108-c7ed5f925ef7	ab779ac4-a872-4c9a-accd-a1ee340a930a
eac0cb13-d732-4cee-8e77-efabc05b881a	ab779ac4-a872-4c9a-accd-a1ee340a930a
8eab4c30-5504-4724-9695-413344f268d3	ab779ac4-a872-4c9a-accd-a1ee340a930a
00c5569a-5dba-4df6-ae99-a9f63f124ae0	01fd2618-6124-4e6b-a140-7130706e2149
8f9e22f7-8b16-4400-a990-e64d0041f5d7	01fd2618-6124-4e6b-a140-7130706e2149
4c5469d2-503a-4acf-a952-b7f735730ae7	01fd2618-6124-4e6b-a140-7130706e2149
88bb0605-c019-4a56-ae51-850a05e0f197	01fd2618-6124-4e6b-a140-7130706e2149
fee4d224-d896-4c82-9561-853ca6af15f4	01fd2618-6124-4e6b-a140-7130706e2149
00c5569a-5dba-4df6-ae99-a9f63f124ae0	3838ae60-f14d-48bf-8115-2e9d37d2fe8d
8f9e22f7-8b16-4400-a990-e64d0041f5d7	3838ae60-f14d-48bf-8115-2e9d37d2fe8d
eac0cb13-d732-4cee-8e77-efabc05b881a	3838ae60-f14d-48bf-8115-2e9d37d2fe8d
8eab4c30-5504-4724-9695-413344f268d3	3838ae60-f14d-48bf-8115-2e9d37d2fe8d
791740bd-34bb-4d98-bd94-2fb48334a4fe	3838ae60-f14d-48bf-8115-2e9d37d2fe8d
00c5569a-5dba-4df6-ae99-a9f63f124ae0	6899f396-3b0b-45cb-b321-e82ec77b15ad
8f9e22f7-8b16-4400-a990-e64d0041f5d7	6899f396-3b0b-45cb-b321-e82ec77b15ad
4c5469d2-503a-4acf-a952-b7f735730ae7	6899f396-3b0b-45cb-b321-e82ec77b15ad
88bb0605-c019-4a56-ae51-850a05e0f197	6899f396-3b0b-45cb-b321-e82ec77b15ad
efe85715-4f41-4747-aa26-a86373add2a7	6899f396-3b0b-45cb-b321-e82ec77b15ad
00c5569a-5dba-4df6-ae99-a9f63f124ae0	50feae97-695f-4779-ad88-5357d1dc2eb8
8f9e22f7-8b16-4400-a990-e64d0041f5d7	50feae97-695f-4779-ad88-5357d1dc2eb8
dfaeea11-207c-4e44-8108-c7ed5f925ef7	50feae97-695f-4779-ad88-5357d1dc2eb8
eac0cb13-d732-4cee-8e77-efabc05b881a	50feae97-695f-4779-ad88-5357d1dc2eb8
791740bd-34bb-4d98-bd94-2fb48334a4fe	50feae97-695f-4779-ad88-5357d1dc2eb8
\.


--
-- Data for Name: Departments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Departments" ("Id", "FacilityId", "Code", "Name", "Address", "AllowScheduledAppointments", "IsActive", "CreatedAt", "DeletedAt") FROM stdin;
5570b277-fa5d-4267-80d5-1dcf47cfc18b	210ff0ab-9c2a-43de-a833-43ac1bcb8e4c	MSC_CENTER	МФЦ "Центральный"	г. Москва, ул. Тверская, д. 15, стр. 1	t	t	2025-08-03 19:02:29.142136+03	\N
d4f39531-e7f0-42f8-8168-2ffa417b0dde	67e85893-140b-439f-abc0-f78eb024a316	SPB_CENTER	МФЦ "Центральный"	г. Санкт-Петербург, Невский проспект, д. 55, лит. А	t	t	2025-08-05 21:44:08.458318+03	\N
676885cb-ee0f-40b4-be03-9d25a8a2dca1	67e85893-140b-439f-abc0-f78eb024a316	SPB_PRIMORSKY	МФЦ "Приморский"	г. Санкт-Петербург, ул. Савушкина, д. 83, корп. 3	t	t	2025-08-05 21:45:03.494638+03	\N
2790ec1b-d8e2-4923-9a05-4a14bbf48c3e	67e85893-140b-439f-abc0-f78eb024a316	SPB_FRUNZENSKY	МФЦ "Фрунзенский"	г. Санкт-Петербург, Бухарестская ул., д. 48, оф. 101	t	t	2025-08-05 21:45:07.42439+03	\N
764b87fb-1335-43b6-b753-40c01f3b7bb6	67e85893-140b-439f-abc0-f78eb024a316	SPB_KALININSKY	МФЦ "Калининский"	г. Санкт-Петербург, Гражданский проспект, д. 104, корп. 2	t	t	2025-08-05 21:45:10.472706+03	\N
775b5d0e-e161-4d6a-8137-74fc1c38e189	67e85893-140b-439f-abc0-f78eb024a316	SPB_KRASNOGVARD	МФЦ "Красногвардейский"	г. Санкт-Петербург, ул. Ленская, д. 12, лит. Б	t	t	2025-08-05 21:45:13.54015+03	\N
9978d1c5-75a4-4082-9eca-50d6b1815394	210ff0ab-9c2a-43de-a833-43ac1bcb8e4c	MSC_SOUTHERN	МФЦ "Южный"	г. Москва, Варшавское шоссе, д. 87, корп. 2	t	t	2025-08-03 19:02:29.142136+03	\N
4fa8236c-801a-45e6-a358-352ed56a88b5	210ff0ab-9c2a-43de-a833-43ac1bcb8e4c	MSC_NORTHERN	МФЦ "Северный"	г. Москва, Ленинградский проспект, д. 62, оф. 104	t	t	2025-08-03 19:02:29.142136+03	\N
97237109-8e2e-44db-85c8-7ebae2faf6bc	210ff0ab-9c2a-43de-a833-43ac1bcb8e4c	MSC_EASTERN	МФЦ "Восточный"	г. Москва, ул. Первомайская, д. 33, стр. 2	t	t	2025-08-03 19:02:29.142136+03	\N
84fdf6d5-4e0b-4077-bb7c-702782da545d	210ff0ab-9c2a-43de-a833-43ac1bcb8e4c	MSC_WESTERN	МФЦ "Западный"	г. Москва, ул. Можайский Вал, д. 10	t	t	2025-08-03 19:02:29.142136+03	\N
ab779ac4-a872-4c9a-accd-a1ee340a930a	18e10b75-8aa2-4282-b89f-bc9a2febade1	EKB_CENTER	МФЦ "Центральный"	г. Екатеринбург, ул. Ленина, д. 24а	t	t	2025-08-05 21:48:01.323875+03	\N
01fd2618-6124-4e6b-a140-7130706e2149	18e10b75-8aa2-4282-b89f-bc9a2febade1	EKB_VERHISET	МФЦ "Верх-Исетский"	г. Екатеринбург, ул. Хомякова, д. 14	t	t	2025-08-05 21:48:09.707867+03	\N
3838ae60-f14d-48bf-8115-2e9d37d2fe8d	18e10b75-8aa2-4282-b89f-bc9a2febade1	EKB_ORDZHON	МФЦ "Орджоникидзевский"	г. Екатеринбург, ул. Бакинских Комиссаров, д. 53	t	t	2025-08-05 21:48:19.291434+03	\N
6899f396-3b0b-45cb-b321-e82ec77b15ad	18e10b75-8aa2-4282-b89f-bc9a2febade1	EKB_CHKALOV	МФЦ "Чкаловский"	г. Екатеринбург, ул. Белинского, д. 222	t	t	2025-08-05 21:48:29.524203+03	\N
50feae97-695f-4779-ad88-5357d1dc2eb8	18e10b75-8aa2-4282-b89f-bc9a2febade1	EKB_KIROV	МФЦ "Кировский"	г. Екатеринбург, ул. Декабристов, д. 16	t	t	2025-08-05 21:48:38.636574+03	\N
\.


--
-- Data for Name: Facilities; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Facilities" ("Id", "Code", "Name", "Address", "IsActive", "CreatedAt", "DeletedAt") FROM stdin;
210ff0ab-9c2a-43de-a833-43ac1bcb8e4c	MFC_MSC	МФЦ "Единое окно"	г. Москва, ул. Ленина, д. 42, корп. 1	t	2025-08-03 18:48:14.105898+03	\N
67e85893-140b-439f-abc0-f78eb024a316	MFC_SPB	МФЦ "Городские услуги"	г. Санкт-Петербург, Невский проспект, д. 100, лит. А	t	2025-08-03 18:48:14.105898+03	\N
18e10b75-8aa2-4282-b89f-bc9a2febade1	MFC_ECB	МФЦ "Регион Сервис"	г. Екатеринбург, ул. Малышева, д. 55, оф. 101	t	2025-08-03 18:48:14.105898+03	\N
\.


--
-- Data for Name: NonWorkingPeriod; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."NonWorkingPeriod" ("Id", "ScheduleId", "DayOfWeek", "StartTime", "EndTime") FROM stdin;
77a6fbf8-ec8c-42b9-aad1-0f9f5b945598	f56aeb47-2692-440b-af11-1a5b7d9b3b14	1	13:00:00	14:00:00
2ee5d9c4-4ae8-427f-8a26-5874e9a354c9	f56aeb47-2692-440b-af11-1a5b7d9b3b14	2	13:00:00	14:00:00
331b2c3c-67c9-4b19-8530-b3d41b0276ac	f56aeb47-2692-440b-af11-1a5b7d9b3b14	3	13:00:00	14:00:00
4c877d4b-4ce4-46ad-8abe-2a481b9c4977	f56aeb47-2692-440b-af11-1a5b7d9b3b14	4	13:00:00	14:00:00
28c72e18-a790-4255-aa46-e6aee765ebc9	f56aeb47-2692-440b-af11-1a5b7d9b3b14	5	13:00:00	14:00:00
23c7f210-4298-4c5b-a567-78800f53540e	f56aeb47-2692-440b-af11-1a5b7d9b3b14	6	00:00:00	23:59:59
20eb1e67-2e05-4960-88f1-752c276e9b6f	f56aeb47-2692-440b-af11-1a5b7d9b3b14	7	00:00:00	23:59:59
\.


--
-- Data for Name: Schedules; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Schedules" ("Id", "DepartmentId", "Name", "IsActive", "WorkDayStart", "WorkDayEnd", "CreatedAt", "DeletedAt") FROM stdin;
f56aeb47-2692-440b-af11-1a5b7d9b3b14	5570b277-fa5d-4267-80d5-1dcf47cfc18b	Основное расписание	t	08:00:00	17:00:00	2025-08-30 23:56:54.241532+03	\N
\.


--
-- Data for Name: ServiceCategories; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."ServiceCategories" ("Id", "Code", "Name", "Prefix", "AllowScheduledAppointments", "IsActive", "CreatedAt", "DeletedAt") FROM stdin;
00c5569a-5dba-4df6-ae99-a9f63f124ae0	PASSPORT	Паспортные услуги	PASP	t	t	2025-08-05 21:55:09.485422+03	\N
8f9e22f7-8b16-4400-a990-e64d0041f5d7	REGISTRATION	Регистрация по месту жительства	REG	t	t	2025-08-05 21:55:18.049435+03	\N
dfaeea11-207c-4e44-8108-c7ed5f925ef7	TAX	Налоговые услуги	TAX	f	t	2025-08-05 21:55:26.504702+03	\N
4c5469d2-503a-4acf-a952-b7f735730ae7	SOCIAL	Социальные выплаты и льготы	SOC	t	t	2025-08-05 21:55:34.110723+03	\N
88bb0605-c019-4a56-ae51-850a05e0f197	PROPERTY	Земельно-имущественные вопросы	PROP	t	t	2025-08-05 21:55:41.731854+03	\N
fee4d224-d896-4c82-9561-853ca6af15f4	LEGAL	Юридические услуги	LEG	f	t	2025-08-05 21:55:49.175859+03	\N
eac0cb13-d732-4cee-8e77-efabc05b881a	MEDICAL	Медицинские справки и документы	MED	t	t	2025-08-05 21:55:57.179172+03	\N
8eab4c30-5504-4724-9695-413344f268d3	TRANSPORT	Транспортные услуги	TRN	t	t	2025-08-05 21:56:05.734554+03	\N
efe85715-4f41-4747-aa26-a86373add2a7	EDUCATION	Образовательные услуги	EDU	f	t	2025-08-05 21:56:13.811061+03	\N
791740bd-34bb-4d98-bd94-2fb48334a4fe	ARCHIVE	Архивные справки и выписки	ARCH	t	t	2025-08-05 21:56:23.33308+03	\N
\.


--
-- Data for Name: Services; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Services" ("Id", "ServiceCategoryId", "Code", "Name", "Duration", "AllowScheduledAppointments", "IsActive", "CreatedAt", "DeletedAt", "WorkplaceId") FROM stdin;
7a8b58f1-4d45-411a-ba02-825a7928b367	00c5569a-5dba-4df6-ae99-a9f63f124ae0	PASP_ISSUE	Выдача паспорта гражданина РФ	30	t	t	2025-08-05 22:16:39.66198+03	\N	\N
02b5be35-0a5f-43c5-8a76-b4a808485958	00c5569a-5dba-4df6-ae99-a9f63f124ae0	PASP_REPLACE	Замена паспорта по возрасту	30	t	t	2025-08-05 22:16:39.66198+03	\N	\N
cfa20ca0-0fc0-4624-aeb4-9f3b085040e4	00c5569a-5dba-4df6-ae99-a9f63f124ae0	PASP_LOST	Восстановление паспорта при утере	45	t	t	2025-08-05 22:16:39.66198+03	\N	\N
d40539d8-d80a-4411-9d6a-bcdc9a4cb8a5	00c5569a-5dba-4df6-ae99-a9f63f124ae0	PASP_CHANGE	Внесение изменений в паспорт	20	t	t	2025-08-05 22:16:39.66198+03	\N	\N
26afaa7e-2dc6-4d7e-8e99-ab9772db9457	00c5569a-5dba-4df6-ae99-a9f63f124ae0	PASP_TEMP	Выдача временного удостоверения	15	t	t	2025-08-05 22:16:39.66198+03	\N	\N
499299c4-e2ee-41dd-b074-430a010a782e	8f9e22f7-8b16-4400-a990-e64d0041f5d7	REG_PERM	Постоянная регистрация по месту жительства	25	t	t	2025-08-05 22:16:50.016259+03	\N	\N
0484b302-af2c-4a40-bb53-27b6b5cc3841	8f9e22f7-8b16-4400-a990-e64d0041f5d7	REG_TEMP	Временная регистрация по месту пребывания	25	t	t	2025-08-05 22:16:50.016259+03	\N	\N
eee7059e-1bfa-4924-9e62-35d19215d08e	8f9e22f7-8b16-4400-a990-e64d0041f5d7	REG_DISCHARGE	Снятие с регистрационного учета	15	t	t	2025-08-05 22:16:50.016259+03	\N	\N
f9f9f658-cf79-4de7-98f0-fc05f0a0d377	8f9e22f7-8b16-4400-a990-e64d0041f5d7	REG_CHILD	Регистрация новорожденного	20	t	t	2025-08-05 22:16:50.016259+03	\N	\N
b4b96bae-812c-4d9b-a3b5-bef252e0b4f1	8f9e22f7-8b16-4400-a990-e64d0041f5d7	REG_CERT	Выдача справки о регистрации	10	t	t	2025-08-05 22:16:50.016259+03	\N	\N
9b3468e6-d109-4c27-9614-0ff4bf0f12da	4c5469d2-503a-4acf-a952-b7f735730ae7	SOC_PENSION	Оформление пенсии	40	t	t	2025-08-05 22:17:10.40071+03	\N	\N
1288eb0d-816b-48d6-a3b2-db6404f6317e	4c5469d2-503a-4acf-a952-b7f735730ae7	SOC_BENEFITS	Оформление социальных пособий	30	t	t	2025-08-05 22:17:10.40071+03	\N	\N
32fc5268-c83c-484e-b6dd-fff2e47dea5b	4c5469d2-503a-4acf-a952-b7f735730ae7	SOC_DISABILITY	Оформление льгот для инвалидов	45	t	t	2025-08-05 22:17:10.40071+03	\N	\N
110eb224-8e4e-4919-9059-d6a279e715c5	4c5469d2-503a-4acf-a952-b7f735730ae7	SOC_CHILD	Оформление детских пособий	25	t	t	2025-08-05 22:17:10.40071+03	\N	\N
b0731103-b45f-46f1-9c16-7c13bdb2283b	4c5469d2-503a-4acf-a952-b7f735730ae7	SOC_CERT	Выдача справок для соцзащиты	15	t	t	2025-08-05 22:17:10.40071+03	\N	\N
0d7684da-9887-4ce6-8ae9-1fb2c03726d0	88bb0605-c019-4a56-ae51-850a05e0f197	PROP_CADASTRE	Постановка на кадастровый учет	50	t	t	2025-08-05 22:17:17.851471+03	\N	\N
0b27a902-f164-4d6e-a8e7-027b5ec8f7b7	88bb0605-c019-4a56-ae51-850a05e0f197	PROP_RIGHTS	Регистрация права собственности	60	t	t	2025-08-05 22:17:17.851471+03	\N	\N
ee3ee52b-8e6e-4f3a-a53b-831da3dfc8e1	88bb0605-c019-4a56-ae51-850a05e0f197	PROP_EXTRACT	Выдача выписки из ЕГРН	15	t	t	2025-08-05 22:17:17.851471+03	\N	\N
b8dbdbbf-87eb-4104-965f-5d3e47406045	88bb0605-c019-4a56-ae51-850a05e0f197	PROP_INHERIT	Оформление наследства	90	t	t	2025-08-05 22:17:17.851471+03	\N	\N
7f79c82f-dcd2-4b01-8a13-17414388a4f6	88bb0605-c019-4a56-ae51-850a05e0f197	PROP_DONATION	Оформление дарственной	60	t	t	2025-08-05 22:17:17.851471+03	\N	\N
1a184967-b3cc-458b-8f44-be0e0b61f61f	eac0cb13-d732-4cee-8e77-efabc05b881a	MED_086	Оформление справки 086/у	20	t	t	2025-08-05 22:17:34.837518+03	\N	\N
ce5b3a02-e3a2-4016-ac6a-43680c40282f	eac0cb13-d732-4cee-8e77-efabc05b881a	MED_VACCINE	Оформление прививочного сертификата	15	t	t	2025-08-05 22:17:34.837518+03	\N	\N
2938f8ad-32ac-44c9-a440-bcf7c201b0a4	eac0cb13-d732-4cee-8e77-efabc05b881a	MED_DISABILITY	Оформление документов для МСЭ	30	t	t	2025-08-05 22:17:34.837518+03	\N	\N
f139abbf-a142-4d6c-93da-29dff9c66b00	eac0cb13-d732-4cee-8e77-efabc05b881a	MED_HEALTH	Выдача медицинской книжки	25	t	t	2025-08-05 22:17:34.837518+03	\N	\N
22847ba3-bfcf-48f7-a1cf-ae6c2d355331	eac0cb13-d732-4cee-8e77-efabc05b881a	MED_CERT	Выдача справок о состоянии здоровья	10	t	t	2025-08-05 22:17:34.837518+03	\N	\N
9b0d3c8b-2deb-4561-b46d-3a457776bd51	8eab4c30-5504-4724-9695-413344f268d3	TRN_LICENSE	Замена водительского удостоверения	30	t	t	2025-08-05 22:17:42.825235+03	\N	\N
34fb6cf3-615e-48a8-8889-37056257400e	8eab4c30-5504-4724-9695-413344f268d3	TRN_REGISTRATION	Регистрация транспортного средства	45	t	t	2025-08-05 22:17:42.825235+03	\N	\N
c4f16a36-aa6d-416e-9caf-861d89d328f8	8eab4c30-5504-4724-9695-413344f268d3	TRN_PLATES	Получение номерных знаков	20	t	t	2025-08-05 22:17:42.825235+03	\N	\N
4bf417e0-65c5-4432-b80d-bf45c94c68a5	8eab4c30-5504-4724-9695-413344f268d3	TRN_INSURANCE	Оформление страхового полиса	25	t	t	2025-08-05 22:17:42.825235+03	\N	\N
6c6d55f4-0729-49d9-b4ec-96b484aba7b7	8eab4c30-5504-4724-9695-413344f268d3	TRN_TAX	Оплата транспортного налога	10	t	t	2025-08-05 22:17:42.825235+03	\N	\N
104d641a-d1d2-40dd-b7bc-696a8f257ad5	791740bd-34bb-4d98-bd94-2fb48334a4fe	ARCH_WORK	Архивная справка о трудовой деятельности	30	t	t	2025-08-05 22:18:00.256057+03	\N	\N
89381375-2a69-4605-b9b6-76e71ce1653e	791740bd-34bb-4d98-bd94-2fb48334a4fe	ARCH_SALARY	Справка о заработной плате	25	t	t	2025-08-05 22:18:00.256057+03	\N	\N
fa4ba44c-0685-4fa9-8b95-c030ab0c01a9	791740bd-34bb-4d98-bd94-2fb48334a4fe	ARCH_HOUSING	Выписка из домовой книги	20	t	t	2025-08-05 22:18:00.256057+03	\N	\N
e597b419-31de-402f-91ac-860d9a4c24f8	791740bd-34bb-4d98-bd94-2fb48334a4fe	ARCH_FAMILY	Справка о составе семьи	15	t	t	2025-08-05 22:18:00.256057+03	\N	\N
ddae4a07-4779-4837-a73f-7f78f8cd5b82	791740bd-34bb-4d98-bd94-2fb48334a4fe	ARCH_HISTORY	Историческая справка	40	t	t	2025-08-05 22:18:00.256057+03	\N	\N
0fa4e27d-19d8-48cb-9491-c916c1eb35e1	dfaeea11-207c-4e44-8108-c7ed5f925ef7	TAX_INN	Постановка на учет и получение ИНН	20	f	t	2025-08-05 22:20:03.916656+03	\N	\N
4f33e5eb-d0b3-48ae-9a4b-9b53dc7360e0	dfaeea11-207c-4e44-8108-c7ed5f925ef7	TAX_CERT	Выдача справок о налогах	15	f	t	2025-08-05 22:20:03.916656+03	\N	\N
a9ee4d4a-43bb-4a02-8544-2c23e6323b7e	dfaeea11-207c-4e44-8108-c7ed5f925ef7	TAX_PROPERTY	Прием деклараций на имущество	25	f	t	2025-08-05 22:20:03.916656+03	\N	\N
679fd1b8-0624-474f-bdf3-f19f3ee809f9	dfaeea11-207c-4e44-8108-c7ed5f925ef7	TAX_DEDUCTION	Оформление налоговых вычетов	30	f	t	2025-08-05 22:20:03.916656+03	\N	\N
c0e13bd8-2bd9-4e0f-a62c-d13cd607ead6	dfaeea11-207c-4e44-8108-c7ed5f925ef7	TAX_CONSULT	Консультация по налоговым вопросам	20	f	t	2025-08-05 22:20:03.916656+03	\N	\N
32201ecc-ac93-496e-88ed-d40b8884f6b3	fee4d224-d896-4c82-9561-853ca6af15f4	LEGAL_NOTARY	Нотариальное заверение документов	30	f	t	2025-08-05 22:20:48.317119+03	\N	\N
d886a247-50ef-4501-9e60-33cba9004c39	fee4d224-d896-4c82-9561-853ca6af15f4	LEGAL_CONSULT	Юридическая консультация	20	f	t	2025-08-05 22:20:48.317119+03	\N	\N
54be3baa-ece4-4560-939d-8b85f359d58b	fee4d224-d896-4c82-9561-853ca6af15f4	LEGAL_POWER	Оформление доверенностей	25	f	t	2025-08-05 22:20:48.317119+03	\N	\N
15b0f07c-0029-46fe-aceb-7588fcceb267	fee4d224-d896-4c82-9561-853ca6af15f4	LEGAL_CONTRACT	Составление договоров	45	f	t	2025-08-05 22:20:48.317119+03	\N	\N
ebbf7646-1416-47e8-938b-326cdbefdb42	fee4d224-d896-4c82-9561-853ca6af15f4	LEGAL_APPEAL	Помощь в составлении обращений	30	f	t	2025-08-05 22:20:48.317119+03	\N	\N
b6490bf4-31bf-4b6f-a3e4-6fba7cfa72ea	efe85715-4f41-4747-aa26-a86373add2a7	EDU_ENROLL	Консультация по зачислению в учебные заведения	20	f	t	2025-08-05 22:20:52.724817+03	\N	\N
5eecfef7-1b2d-4bb3-b4ba-46e39254ae7d	efe85715-4f41-4747-aa26-a86373add2a7	EDU_CERT	Выдача справок об обучении	15	f	t	2025-08-05 22:20:52.724817+03	\N	\N
b302e6de-6f6e-4747-9177-67dc079abd49	efe85715-4f41-4747-aa26-a86373add2a7	EDU_TRANSFER	Оформление перевода между учебными заведениями	30	f	t	2025-08-05 22:20:52.724817+03	\N	\N
1072d0ab-b3d0-41ff-a153-fdb415794885	efe85715-4f41-4747-aa26-a86373add2a7	EDU_BENEFITS	Оформление льгот для студентов	25	f	t	2025-08-05 22:20:52.724817+03	\N	\N
10d074e0-5c30-4d9b-80e1-8d7bb11eefbe	efe85715-4f41-4747-aa26-a86373add2a7	EDU_DOCS	Заверение документов об образовании	20	f	t	2025-08-05 22:20:52.724817+03	\N	\N
\.


--
-- Data for Name: WorkplaceServiceCategory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."WorkplaceServiceCategory" ("ServiceCategoryId", "WorkplaceId") FROM stdin;
00c5569a-5dba-4df6-ae99-a9f63f124ae0	eaf6df22-e874-4cda-99f5-e43ead7b06c5
00c5569a-5dba-4df6-ae99-a9f63f124ae0	f47b767f-9c97-4285-a3f3-60eb8b9422c0
00c5569a-5dba-4df6-ae99-a9f63f124ae0	cb6b7725-81b3-4c9e-9075-5b37ebffc1d3
00c5569a-5dba-4df6-ae99-a9f63f124ae0	96a0654b-5962-4df5-ac35-9a72bcaebc46
8f9e22f7-8b16-4400-a990-e64d0041f5d7	eaf6df22-e874-4cda-99f5-e43ead7b06c5
791740bd-34bb-4d98-bd94-2fb48334a4fe	eaf6df22-e874-4cda-99f5-e43ead7b06c5
8f9e22f7-8b16-4400-a990-e64d0041f5d7	b603d9fb-3a8d-48bb-8333-b5f56eff9d12
8f9e22f7-8b16-4400-a990-e64d0041f5d7	a24e8c56-9f6d-4427-8ba3-7514b9936e36
8f9e22f7-8b16-4400-a990-e64d0041f5d7	02267514-c737-40ce-b01a-42cc2da72aac
00c5569a-5dba-4df6-ae99-a9f63f124ae0	b603d9fb-3a8d-48bb-8333-b5f56eff9d12
88bb0605-c019-4a56-ae51-850a05e0f197	b603d9fb-3a8d-48bb-8333-b5f56eff9d12
dfaeea11-207c-4e44-8108-c7ed5f925ef7	2c5cc27a-3b75-454c-9c4a-280588cb33a5
dfaeea11-207c-4e44-8108-c7ed5f925ef7	ff46bf19-0295-4c4f-854a-79cb57713427
dfaeea11-207c-4e44-8108-c7ed5f925ef7	e297491f-da5f-44e9-a413-19e68dc09902
8eab4c30-5504-4724-9695-413344f268d3	2c5cc27a-3b75-454c-9c4a-280588cb33a5
fee4d224-d896-4c82-9561-853ca6af15f4	2c5cc27a-3b75-454c-9c4a-280588cb33a5
fee4d224-d896-4c82-9561-853ca6af15f4	6bc86d88-782e-48c9-be77-b4496f5f4fb2
fee4d224-d896-4c82-9561-853ca6af15f4	7e79e469-6450-4b0e-b6be-f1f9f168aae7
fee4d224-d896-4c82-9561-853ca6af15f4	123b9d48-11e0-4703-84fd-a462cc0cb37b
88bb0605-c019-4a56-ae51-850a05e0f197	6bc86d88-782e-48c9-be77-b4496f5f4fb2
dfaeea11-207c-4e44-8108-c7ed5f925ef7	6bc86d88-782e-48c9-be77-b4496f5f4fb2
4c5469d2-503a-4acf-a952-b7f735730ae7	ca606594-8910-49c8-9261-98e42037c28b
4c5469d2-503a-4acf-a952-b7f735730ae7	057fb265-0ce0-435a-8d46-1f3130b86744
4c5469d2-503a-4acf-a952-b7f735730ae7	e9e6e9c5-71e6-471d-a2fc-f9bba2441409
eac0cb13-d732-4cee-8e77-efabc05b881a	ca606594-8910-49c8-9261-98e42037c28b
791740bd-34bb-4d98-bd94-2fb48334a4fe	ca606594-8910-49c8-9261-98e42037c28b
88bb0605-c019-4a56-ae51-850a05e0f197	feb351af-b7f6-4300-8c8b-35c8eb78f5ea
88bb0605-c019-4a56-ae51-850a05e0f197	c7fbce35-ba86-49d9-bb14-b6c15fd77674
88bb0605-c019-4a56-ae51-850a05e0f197	c28409f3-1b04-43da-8186-191869ad5212
dfaeea11-207c-4e44-8108-c7ed5f925ef7	feb351af-b7f6-4300-8c8b-35c8eb78f5ea
fee4d224-d896-4c82-9561-853ca6af15f4	feb351af-b7f6-4300-8c8b-35c8eb78f5ea
791740bd-34bb-4d98-bd94-2fb48334a4fe	43f69888-1194-4006-9415-e2704771b32c
791740bd-34bb-4d98-bd94-2fb48334a4fe	c32c7cd3-a0b6-4206-9435-ee8d906061eb
791740bd-34bb-4d98-bd94-2fb48334a4fe	61634435-a572-46cb-ad83-51bb394f697f
4c5469d2-503a-4acf-a952-b7f735730ae7	43f69888-1194-4006-9415-e2704771b32c
00c5569a-5dba-4df6-ae99-a9f63f124ae0	43f69888-1194-4006-9415-e2704771b32c
fee4d224-d896-4c82-9561-853ca6af15f4	7cada732-f0b9-4d1a-a464-90abef643785
fee4d224-d896-4c82-9561-853ca6af15f4	d4740386-5e18-421d-82f2-2cea9403e1b0
fee4d224-d896-4c82-9561-853ca6af15f4	ffea3c90-3d51-434b-872a-4134d088f247
efe85715-4f41-4747-aa26-a86373add2a7	7cada732-f0b9-4d1a-a464-90abef643785
8eab4c30-5504-4724-9695-413344f268d3	7cada732-f0b9-4d1a-a464-90abef643785
\.


--
-- Data for Name: Workplaces; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Workplaces" ("Id", "DepartmentId", "Code", "Name", "IsActive", "CreatedAt", "DeletedAt") FROM stdin;
eaf6df22-e874-4cda-99f5-e43ead7b06c5	5570b277-fa5d-4267-80d5-1dcf47cfc18b	MSC_CENTER_1	Окно 1 (Паспортные услуги)	t	2025-08-06 00:12:59.384177+03	\N
b603d9fb-3a8d-48bb-8333-b5f56eff9d12	5570b277-fa5d-4267-80d5-1dcf47cfc18b	MSC_CENTER_2	Окно 2 (Регистрация)	t	2025-08-06 00:12:59.384177+03	\N
ca606594-8910-49c8-9261-98e42037c28b	5570b277-fa5d-4267-80d5-1dcf47cfc18b	MSC_CENTER_3	Окно 3 (Социальные выплаты)	t	2025-08-06 00:12:59.384177+03	\N
2c5cc27a-3b75-454c-9c4a-280588cb33a5	5570b277-fa5d-4267-80d5-1dcf47cfc18b	MSC_CENTER_4	Окно 4 (Налоговые услуги)	t	2025-08-06 00:12:59.384177+03	\N
6bc86d88-782e-48c9-be77-b4496f5f4fb2	5570b277-fa5d-4267-80d5-1dcf47cfc18b	MSC_CENTER_5	Окно 5 (Юридические консультации)	t	2025-08-06 00:12:59.384177+03	\N
1aa532c3-69c8-4b89-bcf0-b5c51fb85e0f	9978d1c5-75a4-4082-9eca-50d6b1815394	MSC_SOUTH_1	Окно 1 (Общие вопросы)	t	2025-08-06 00:13:10.378576+03	\N
4df87bb6-8d51-48cd-ad64-552b0bf16895	9978d1c5-75a4-4082-9eca-50d6b1815394	MSC_SOUTH_2	Окно 2 (Документы)	t	2025-08-06 00:13:10.378576+03	\N
61da4840-3ce9-4b5f-ba1f-0fbf0e2afb7d	9978d1c5-75a4-4082-9eca-50d6b1815394	MSC_SOUTH_3	Окно 3 (Пенсии)	t	2025-08-06 00:13:10.378576+03	\N
28bf7015-a602-45c2-bc1d-942326352cdf	9978d1c5-75a4-4082-9eca-50d6b1815394	MSC_SOUTH_4	Окно 4 (Справки)	t	2025-08-06 00:13:10.378576+03	\N
43f69888-1194-4006-9415-e2704771b32c	9978d1c5-75a4-4082-9eca-50d6b1815394	MSC_SOUTH_5	Окно 5 (Архив)	t	2025-08-06 00:13:10.378576+03	\N
cc576d6f-030b-4072-a1f5-091931ab9d2e	d4f39531-e7f0-42f8-8168-2ffa417b0dde	SPB_CENTER_1	Окно 1 (Прием документов)	t	2025-08-06 00:13:35.459538+03	\N
ae0d54a2-7a1b-4cbc-8850-c439657f90fa	d4f39531-e7f0-42f8-8168-2ffa417b0dde	SPB_CENTER_2	Окно 2 (Выдача документов)	t	2025-08-06 00:13:35.459538+03	\N
b044eb92-5317-4159-a825-732f5a21365f	d4f39531-e7f0-42f8-8168-2ffa417b0dde	SPB_CENTER_3	Окно 3 (Консультации)	t	2025-08-06 00:13:35.459538+03	\N
2dea97b7-53b1-4405-b1f7-e3f8bdd10e9b	d4f39531-e7f0-42f8-8168-2ffa417b0dde	SPB_CENTER_4	Окно 4 (Оплата услуг)	t	2025-08-06 00:13:35.459538+03	\N
5e8de87d-9ffc-444a-ad3b-1df990bf733f	d4f39531-e7f0-42f8-8168-2ffa417b0dde	SPB_CENTER_5	Окно 5 (Экспресс-услуги)	t	2025-08-06 00:13:35.459538+03	\N
01ec0f6a-215b-4d7f-abf9-611a96aa1fd3	676885cb-ee0f-40b4-be03-9d25a8a2dca1	SPB_PRIM_1	Окно 1 (Паспортный стол)	t	2025-08-06 00:13:43.139717+03	\N
a24e8c56-9f6d-4427-8ba3-7514b9936e36	676885cb-ee0f-40b4-be03-9d25a8a2dca1	SPB_PRIM_2	Окно 2 (Регистрация)	t	2025-08-06 00:13:43.139717+03	\N
ff46bf19-0295-4c4f-854a-79cb57713427	676885cb-ee0f-40b4-be03-9d25a8a2dca1	SPB_PRIM_3	Окно 3 (Налоги)	t	2025-08-06 00:13:43.139717+03	\N
057fb265-0ce0-435a-8d46-1f3130b86744	676885cb-ee0f-40b4-be03-9d25a8a2dca1	SPB_PRIM_4	Окно 4 (Соцзащита)	t	2025-08-06 00:13:43.139717+03	\N
7e79e469-6450-4b0e-b6be-f1f9f168aae7	676885cb-ee0f-40b4-be03-9d25a8a2dca1	SPB_PRIM_5	Окно 5 (Юридические услуги)	t	2025-08-06 00:13:43.139717+03	\N
76848ac8-0d66-4f20-bb7c-24c1ca873ceb	ab779ac4-a872-4c9a-accd-a1ee340a930a	EKB_CENTER_1	Окно 1 (Общий прием)	t	2025-08-06 00:13:52.287998+03	\N
beb1cb18-b469-4d67-8da6-ddbc8ddffd8e	ab779ac4-a872-4c9a-accd-a1ee340a930a	EKB_CENTER_2	Окно 2 (Документы)	t	2025-08-06 00:13:52.287998+03	\N
3aa1df29-db9d-4ecd-b6a4-4ca7bdfeefb7	ab779ac4-a872-4c9a-accd-a1ee340a930a	EKB_CENTER_3	Окно 3 (Платежи)	t	2025-08-06 00:13:52.287998+03	\N
ca712a2e-126e-43f7-9a32-1dcc92d51521	ab779ac4-a872-4c9a-accd-a1ee340a930a	EKB_CENTER_4	Окно 4 (Справки)	t	2025-08-06 00:13:52.287998+03	\N
18bffa9d-cc2d-43cf-a77d-95f9caa351dc	ab779ac4-a872-4c9a-accd-a1ee340a930a	EKB_CENTER_5	Окно 5 (Архив)	t	2025-08-06 00:13:52.287998+03	\N
fadab767-9170-4e49-a034-b22d4c40cc0d	01fd2618-6124-4e6b-a140-7130706e2149	EKB_VERH_1	Окно 1 (Паспортные услуги)	t	2025-08-06 00:14:01.568641+03	\N
2a2ccbfa-c458-499f-823c-a85e11454d7b	01fd2618-6124-4e6b-a140-7130706e2149	EKB_VERH_2	Окно 2 (Регистрация)	t	2025-08-06 00:14:01.568641+03	\N
c7fbce35-ba86-49d9-bb14-b6c15fd77674	01fd2618-6124-4e6b-a140-7130706e2149	EKB_VERH_3	Окно 3 (Недвижимость)	t	2025-08-06 00:14:01.568641+03	\N
894b8563-8f68-42d7-aac6-169ff8246db0	01fd2618-6124-4e6b-a140-7130706e2149	EKB_VERH_4	Окно 4 (Соцуслуги)	t	2025-08-06 00:14:01.568641+03	\N
e910552b-4cf7-482d-9317-024c2a0f1289	01fd2618-6124-4e6b-a140-7130706e2149	EKB_VERH_5	Окно 5 (Консультации)	t	2025-08-06 00:14:01.568641+03	\N
3388dc1f-db65-4682-8abd-8a246cd4a9c9	84fdf6d5-4e0b-4077-bb7c-702782da545d	MSC_WEST_1	Окно 1 (Общий прием)	t	2025-08-06 00:17:59.390372+03	\N
f47b767f-9c97-4285-a3f3-60eb8b9422c0	84fdf6d5-4e0b-4077-bb7c-702782da545d	MSC_WEST_2	Окно 2 (Паспортные услуги)	t	2025-08-06 00:17:59.390372+03	\N
74878f49-4a83-4232-922c-ded8cdd26597	84fdf6d5-4e0b-4077-bb7c-702782da545d	MSC_WEST_3	Окно 3 (Регистрация)	t	2025-08-06 00:17:59.390372+03	\N
feb351af-b7f6-4300-8c8b-35c8eb78f5ea	84fdf6d5-4e0b-4077-bb7c-702782da545d	MSC_WEST_4	Окно 4 (Недвижимость)	t	2025-08-06 00:17:59.390372+03	\N
7cada732-f0b9-4d1a-a464-90abef643785	84fdf6d5-4e0b-4077-bb7c-702782da545d	MSC_WEST_5	Окно 5 (Консультации)	t	2025-08-06 00:17:59.390372+03	\N
a51028ad-c230-482f-8f49-6cd665e513d0	4fa8236c-801a-45e6-a358-352ed56a88b5	MSC_NORTH_1	Окно 1 (Общие вопросы)	t	2025-08-06 00:18:04.513378+03	\N
3f17ba42-cb9d-4d69-bc58-5aa8838c8d77	4fa8236c-801a-45e6-a358-352ed56a88b5	MSC_NORTH_2	Окно 2 (Документы)	t	2025-08-06 00:18:04.513378+03	\N
64f42c49-ca83-476a-8aee-f9de1b185a16	4fa8236c-801a-45e6-a358-352ed56a88b5	MSC_NORTH_3	Окно 3 (Соцуслуги)	t	2025-08-06 00:18:04.513378+03	\N
2ea87ffd-0b90-48c6-8d6b-71eafb1dbe62	4fa8236c-801a-45e6-a358-352ed56a88b5	MSC_NORTH_4	Окно 4 (Платежи)	t	2025-08-06 00:18:04.513378+03	\N
5d759910-f53e-4710-acb7-9b5a8584c4a4	4fa8236c-801a-45e6-a358-352ed56a88b5	MSC_NORTH_5	Окно 5 (Архив)	t	2025-08-06 00:18:04.513378+03	\N
8e65f54e-cf63-4f65-a1fc-b2e926df7948	3838ae60-f14d-48bf-8115-2e9d37d2fe8d	EKB_ORD_1	Окно 1 (Прием документов)	t	2025-08-06 00:18:09.89715+03	\N
d1456fef-ff2f-476c-b486-804b43989483	3838ae60-f14d-48bf-8115-2e9d37d2fe8d	EKB_ORD_2	Окно 2 (Выдача справок)	t	2025-08-06 00:18:09.89715+03	\N
e297491f-da5f-44e9-a413-19e68dc09902	3838ae60-f14d-48bf-8115-2e9d37d2fe8d	EKB_ORD_3	Окно 3 (Налоговые услуги)	t	2025-08-06 00:18:09.89715+03	\N
02267514-c737-40ce-b01a-42cc2da72aac	3838ae60-f14d-48bf-8115-2e9d37d2fe8d	EKB_ORD_4	Окно 4 (Регистрация)	t	2025-08-06 00:18:09.89715+03	\N
123b9d48-11e0-4703-84fd-a462cc0cb37b	3838ae60-f14d-48bf-8115-2e9d37d2fe8d	EKB_ORD_5	Окно 5 (Юридические консультации)	t	2025-08-06 00:18:09.89715+03	\N
43cfb269-e724-49d5-bba3-1b533f5c3097	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e	SPB_FRUN_1	Окно 1 (Паспортный стол)	t	2025-08-06 00:18:15.828519+03	\N
ef842b9b-30e1-4adb-bfdd-495b36dd6c37	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e	SPB_FRUN_2	Окно 2 (Регистрация)	t	2025-08-06 00:18:15.828519+03	\N
c28409f3-1b04-43da-8186-191869ad5212	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e	SPB_FRUN_3	Окно 3 (Недвижимость)	t	2025-08-06 00:18:15.828519+03	\N
fb18c1e1-6c5f-4d84-86c7-4362f87a1fac	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e	SPB_FRUN_4	Окно 4 (Соцзащита)	t	2025-08-06 00:18:15.828519+03	\N
ffea3c90-3d51-434b-872a-4134d088f247	2790ec1b-d8e2-4923-9a05-4a14bbf48c3e	SPB_FRUN_5	Окно 5 (Консультации)	t	2025-08-06 00:18:15.828519+03	\N
506a84cd-79b8-4f04-a54a-6613acb81bc0	775b5d0e-e161-4d6a-8137-74fc1c38e189	SPB_KRAS_1	Окно 1 (Общий прием)	t	2025-08-06 00:18:21.114371+03	\N
4ee375f4-bd9d-4726-b2c3-053ba64b70c9	775b5d0e-e161-4d6a-8137-74fc1c38e189	SPB_KRAS_2	Окно 2 (Документы)	t	2025-08-06 00:18:21.114371+03	\N
25150992-b85e-4a64-bcda-8bec3dedb24e	775b5d0e-e161-4d6a-8137-74fc1c38e189	SPB_KRAS_3	Окно 3 (Пенсии)	t	2025-08-06 00:18:21.114371+03	\N
846a014b-e291-4375-89f9-b8417b946a5e	775b5d0e-e161-4d6a-8137-74fc1c38e189	SPB_KRAS_4	Окно 4 (Справки)	t	2025-08-06 00:18:21.114371+03	\N
c32c7cd3-a0b6-4206-9435-ee8d906061eb	775b5d0e-e161-4d6a-8137-74fc1c38e189	SPB_KRAS_5	Окно 5 (Архив)	t	2025-08-06 00:18:21.114371+03	\N
cb6b7725-81b3-4c9e-9075-5b37ebffc1d3	6899f396-3b0b-45cb-b321-e82ec77b15ad	EKB_CHK_1	Окно 1 (Паспортные услуги)	t	2025-08-06 00:18:26.788619+03	\N
1cdc4116-a67d-457f-95d3-248153fa90b6	6899f396-3b0b-45cb-b321-e82ec77b15ad	EKB_CHK_2	Окно 2 (Регистрация)	t	2025-08-06 00:18:26.788619+03	\N
42e62fb6-5062-4119-9cdd-66d5190cdbb1	6899f396-3b0b-45cb-b321-e82ec77b15ad	EKB_CHK_3	Окно 3 (Недвижимость)	t	2025-08-06 00:18:26.788619+03	\N
e9e6e9c5-71e6-471d-a2fc-f9bba2441409	6899f396-3b0b-45cb-b321-e82ec77b15ad	EKB_CHK_4	Окно 4 (Соцуслуги)	t	2025-08-06 00:18:26.788619+03	\N
d4740386-5e18-421d-82f2-2cea9403e1b0	6899f396-3b0b-45cb-b321-e82ec77b15ad	EKB_CHK_5	Окно 5 (Консультации)	t	2025-08-06 00:18:26.788619+03	\N
7d5b39a8-191b-4393-81c7-0822b39e1c96	50feae97-695f-4779-ad88-5357d1dc2eb8	EKB_KIR_1	Окно 1 (Общий прием)	t	2025-08-06 00:18:33.878259+03	\N
5179a096-e432-4e3c-aecc-83ed46c06e7b	50feae97-695f-4779-ad88-5357d1dc2eb8	EKB_KIR_2	Окно 2 (Документы)	t	2025-08-06 00:18:33.878259+03	\N
975ac032-732c-4a48-a8ea-3bd8ba66faac	50feae97-695f-4779-ad88-5357d1dc2eb8	EKB_KIR_3	Окно 3 (Платежи)	t	2025-08-06 00:18:33.878259+03	\N
44dd477b-7ab6-4288-bfbb-b3d4c8a9f72c	50feae97-695f-4779-ad88-5357d1dc2eb8	EKB_KIR_4	Окно 4 (Справки)	t	2025-08-06 00:18:33.878259+03	\N
61634435-a572-46cb-ad83-51bb394f697f	50feae97-695f-4779-ad88-5357d1dc2eb8	EKB_KIR_5	Окно 5 (Архив)	t	2025-08-06 00:18:33.878259+03	\N
102ae191-b0ef-4bfb-86dd-d7c77acf81ab	97237109-8e2e-44db-85c8-7ebae2faf6bc	MSC_EAST_1	Окно 1 (Прием документов)	t	2025-08-06 00:18:39.339791+03	\N
d2d6eb2a-88e1-4606-b73a-5244f65ffad6	97237109-8e2e-44db-85c8-7ebae2faf6bc	MSC_EAST_2	Окно 2 (Выдача документов)	t	2025-08-06 00:18:39.339791+03	\N
410373a1-abe6-47ec-837a-a0bc493074e2	97237109-8e2e-44db-85c8-7ebae2faf6bc	MSC_EAST_3	Окно 3 (Консультации)	t	2025-08-06 00:18:39.339791+03	\N
3049f2fb-978e-4c69-97db-bd2e7d2780ef	97237109-8e2e-44db-85c8-7ebae2faf6bc	MSC_EAST_4	Окно 4 (Оплата услуг)	t	2025-08-06 00:18:39.339791+03	\N
c4ca25f6-4732-426d-9d92-60e40ed36900	97237109-8e2e-44db-85c8-7ebae2faf6bc	MSC_EAST_5	Окно 5 (Экспресс-услуги)	t	2025-08-06 00:18:39.339791+03	\N
96a0654b-5962-4df5-ac35-9a72bcaebc46	764b87fb-1335-43b6-b753-40c01f3b7bb6	SPB_KAL_1	Окно 1 (Паспортные услуги)	t	2025-08-06 00:18:48.087576+03	\N
3b75cd72-76ca-4e6a-a793-e124fb24b88a	764b87fb-1335-43b6-b753-40c01f3b7bb6	SPB_KAL_2	Окно 2 (Регистрация)	t	2025-08-06 00:18:48.087576+03	\N
ecc26623-50f6-4af1-9fe2-bcbfae63a606	764b87fb-1335-43b6-b753-40c01f3b7bb6	SPB_KAL_3	Окно 3 (Налоговые услуги)	t	2025-08-06 00:18:48.087576+03	\N
5bc55b1f-6a6c-4218-bf7f-340620664f20	764b87fb-1335-43b6-b753-40c01f3b7bb6	SPB_KAL_4	Окно 4 (Соцзащита)	t	2025-08-06 00:18:48.087576+03	\N
aa64c0bb-b928-4a96-b782-b346f4ad4bc1	764b87fb-1335-43b6-b753-40c01f3b7bb6	SPB_KAL_5	Окно 5 (Юридические консультации)	t	2025-08-06 00:18:48.087576+03	\N
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250803154616_InitialCreate	9.0.7
20250803193624_ModifyEntities	9.0.7
20250805202239_WorkplacesServiceCategories	9.0.7
20250810212403_AddScheduleEntity	9.0.7
\.


--
-- Name: DepartmentServiceCategory PK_DepartmentServiceCategory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DepartmentServiceCategory"
    ADD CONSTRAINT "PK_DepartmentServiceCategory" PRIMARY KEY ("ServiceCategoryId", "DepartmentId");


--
-- Name: Departments PK_Departments; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Departments"
    ADD CONSTRAINT "PK_Departments" PRIMARY KEY ("Id");


--
-- Name: Facilities PK_Facilities; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Facilities"
    ADD CONSTRAINT "PK_Facilities" PRIMARY KEY ("Id");


--
-- Name: NonWorkingPeriod PK_NonWorkingPeriod; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."NonWorkingPeriod"
    ADD CONSTRAINT "PK_NonWorkingPeriod" PRIMARY KEY ("Id");


--
-- Name: Schedules PK_Schedules; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Schedules"
    ADD CONSTRAINT "PK_Schedules" PRIMARY KEY ("Id");


--
-- Name: ServiceCategories PK_ServiceCategories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ServiceCategories"
    ADD CONSTRAINT "PK_ServiceCategories" PRIMARY KEY ("Id");


--
-- Name: Services PK_Services; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "PK_Services" PRIMARY KEY ("Id");


--
-- Name: WorkplaceServiceCategory PK_WorkplaceServiceCategory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."WorkplaceServiceCategory"
    ADD CONSTRAINT "PK_WorkplaceServiceCategory" PRIMARY KEY ("ServiceCategoryId", "WorkplaceId");


--
-- Name: Workplaces PK_Workplaces; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Workplaces"
    ADD CONSTRAINT "PK_Workplaces" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_DepartmentServiceCategory_DepartmentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_DepartmentServiceCategory_DepartmentId" ON public."DepartmentServiceCategory" USING btree ("DepartmentId");


--
-- Name: IX_Departments_FacilityId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Departments_FacilityId" ON public."Departments" USING btree ("FacilityId");


--
-- Name: IX_NonWorkingPeriod_ScheduleId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_NonWorkingPeriod_ScheduleId" ON public."NonWorkingPeriod" USING btree ("ScheduleId");


--
-- Name: IX_Schedules_DepartmentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Schedules_DepartmentId" ON public."Schedules" USING btree ("DepartmentId");


--
-- Name: IX_Services_ServiceCategoryId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Services_ServiceCategoryId" ON public."Services" USING btree ("ServiceCategoryId");


--
-- Name: IX_Services_WorkplaceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Services_WorkplaceId" ON public."Services" USING btree ("WorkplaceId");


--
-- Name: IX_WorkplaceServiceCategory_WorkplaceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_WorkplaceServiceCategory_WorkplaceId" ON public."WorkplaceServiceCategory" USING btree ("WorkplaceId");


--
-- Name: IX_Workplaces_DepartmentId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Workplaces_DepartmentId" ON public."Workplaces" USING btree ("DepartmentId");


--
-- Name: DepartmentServiceCategory FK_DepartmentServiceCategory_Departments_DepartmentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DepartmentServiceCategory"
    ADD CONSTRAINT "FK_DepartmentServiceCategory_Departments_DepartmentId" FOREIGN KEY ("DepartmentId") REFERENCES public."Departments"("Id") ON DELETE CASCADE;


--
-- Name: DepartmentServiceCategory FK_DepartmentServiceCategory_ServiceCategories_ServiceCategory~; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DepartmentServiceCategory"
    ADD CONSTRAINT "FK_DepartmentServiceCategory_ServiceCategories_ServiceCategory~" FOREIGN KEY ("ServiceCategoryId") REFERENCES public."ServiceCategories"("Id") ON DELETE CASCADE;


--
-- Name: Departments FK_Departments_Facilities_FacilityId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Departments"
    ADD CONSTRAINT "FK_Departments_Facilities_FacilityId" FOREIGN KEY ("FacilityId") REFERENCES public."Facilities"("Id") ON DELETE RESTRICT;


--
-- Name: NonWorkingPeriod FK_NonWorkingPeriod_Schedules_ScheduleId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."NonWorkingPeriod"
    ADD CONSTRAINT "FK_NonWorkingPeriod_Schedules_ScheduleId" FOREIGN KEY ("ScheduleId") REFERENCES public."Schedules"("Id") ON DELETE CASCADE;


--
-- Name: Schedules FK_Schedules_Departments_DepartmentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Schedules"
    ADD CONSTRAINT "FK_Schedules_Departments_DepartmentId" FOREIGN KEY ("DepartmentId") REFERENCES public."Departments"("Id") ON DELETE RESTRICT;


--
-- Name: Services FK_Services_ServiceCategories_ServiceCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "FK_Services_ServiceCategories_ServiceCategoryId" FOREIGN KEY ("ServiceCategoryId") REFERENCES public."ServiceCategories"("Id") ON DELETE RESTRICT;


--
-- Name: Services FK_Services_Workplaces_WorkplaceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "FK_Services_Workplaces_WorkplaceId" FOREIGN KEY ("WorkplaceId") REFERENCES public."Workplaces"("Id");


--
-- Name: WorkplaceServiceCategory FK_WorkplaceServiceCategory_ServiceCategories_ServiceCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."WorkplaceServiceCategory"
    ADD CONSTRAINT "FK_WorkplaceServiceCategory_ServiceCategories_ServiceCategoryId" FOREIGN KEY ("ServiceCategoryId") REFERENCES public."ServiceCategories"("Id") ON DELETE CASCADE;


--
-- Name: WorkplaceServiceCategory FK_WorkplaceServiceCategory_Workplaces_WorkplaceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."WorkplaceServiceCategory"
    ADD CONSTRAINT "FK_WorkplaceServiceCategory_Workplaces_WorkplaceId" FOREIGN KEY ("WorkplaceId") REFERENCES public."Workplaces"("Id") ON DELETE CASCADE;


--
-- Name: Workplaces FK_Workplaces_Departments_DepartmentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Workplaces"
    ADD CONSTRAINT "FK_Workplaces_Departments_DepartmentId" FOREIGN KEY ("DepartmentId") REFERENCES public."Departments"("Id") ON DELETE RESTRICT;


--
-- PostgreSQL database dump complete
--

