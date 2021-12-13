DROP DATABASE melts_base;
DROP DATABASE postgresMelts;
CREATE DATABASE postgresMelts
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Russian_Russia.1251'
    LC_CTYPE = 'Russian_Russia.1251'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;
ALTER DATABASE 	postgresMelts;
DROP TABLE IF EXISTS public.melts;
DROP SEQUENCE IF EXISTS public.melts_id_seq;
CREATE SEQUENCE public.melts_id_seq;
ALTER SEQUENCE public.melts_id_seq
    OWNER TO postgres;
CREATE TABLE public.melts
(
    id integer NOT NULL DEFAULT nextval('melts_id_seq'::regclass),
    npch character varying(2) COLLATE pg_catalog."default",
    nplav character varying(6) COLLATE pg_catalog."default",
    pnplav character varying(11) COLLATE pg_catalog."default",
    dpl date,
    spl character varying(100) COLLATE pg_catalog."default",
    ind character varying(50) COLLATE pg_catalog."default",
    nkompl smallint,
    del smallint,
    tab_n_pl character varying(6) COLLATE pg_catalog."default",
    ntek character varying(20) COLLATE pg_catalog."default",
    nom_insp character varying(50) COLLATE pg_catalog."default",
    kont character varying(50) COLLATE pg_catalog."default",
    pril character varying(3) COLLATE pg_catalog."default",
    pos character varying(5) COLLATE pg_catalog."default",
    nazn character varying(50) COLLATE pg_catalog."default",
    diam smallint,
    alg character varying(50) COLLATE pg_catalog."default",
    npr smallint,
    last_pr boolean,
    tok_pl real,
    gmp boolean,
	CONSTRAINT "PK_public.melts" PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;	
\copy melts from 'meltscopy.csv' DELIMITER ',' CSV;	
ALTER TABLE public.melts
    OWNER to postgres;
CREATE INDEX index
    ON public.id USING btree
    (dateandtime)
    TABLESPACE pg_default;
	