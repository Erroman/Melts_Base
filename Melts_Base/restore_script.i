DROP TABLE IF EXISTS public.vdp15;
DROP SEQUENCE IF EXISTS public.vdp15_id_seq;
CREATE SEQUENCE public.vdp15_id_seq;
ALTER SEQUENCE public.vdp15_id_seq
    OWNER TO postgres;
CREATE TABLE public.vdp15
(
    id integer NOT NULL DEFAULT nextval('vdp15_id_seq'::regclass),
    dateandtime timestamp without time zone,
    mks integer,
    tagname character varying(50) COLLATE pg_catalog."default",
    val double precision,
    CONSTRAINT "PK_public.vdp15" PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;
\copy vdp15 from 'vdp15copy.csv' DELIMITER ',' CSV;	
ALTER TABLE public.vdp15
    OWNER to postgres;
CREATE INDEX time15
    ON public.vdp15 USING btree
    (dateandtime)
    TABLESPACE pg_default;
