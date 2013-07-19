﻿/**
 * KNOWLEDGE BASE SQL DDL
 */

 

-- @SEQUENCE
-- INCREMENTS THE ARTICLE PK SEQUENCE
CREATE SEQUENCE KB_ART_SEQ START WITH 1 INCREMENT BY 1;

-- @TABLE
-- STORES KNOWLEDGEBASE ARTICLES
CREATE TABLE KB_ART_TBL
(
	KB_ID		NUMERIC(20,0) NOT NULL DEFAULT nextval('KB_ART_SEQ'), -- UNIQUE IDENTIFIER FOR THE KB
	KB_KEY		VARCHAR(40) NOT NULL, -- KEY FOR THE KB
	KB_TYPE		VARCHAR(40) NOT NULL, -- MIME TYPE FOR THE KB ARTICLE
	KB_TEXT		BYTEA NOT NULL, -- TEXT FOR THE KB
	KB_CRT_UTC	TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP, -- TIME THE KB ARTICLE WAS CREATED
	KB_CRT_AUT	VARCHAR(16) NOT NULL, -- IP ADDRESS OF THE CREATION AUTHOR
	KB_LUP_UTC	TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP, -- LAST UPDATE TIME
	KB_LUP_AUT	VARCHAR(16) NOT NULL, -- LAST UPDATE AUTHOR
	CONSTRAINT PK_KB_ART_TBL PRIMARY KEY (KB_ID)
);

-- @INDEX
-- INDEX ON KEY
CREATE UNIQUE INDEX KB_ART_KEY_IDX ON KB_ART_TBL(KB_KEY);

-- @FUNCTION
-- CREATE ARTICLE
CREATE OR REPLACE FUNCTION PUB_KB_ART(
	KB_KEY_IN		IN	VARCHAR,
	KB_TYPE_IN		IN	VARCHAR,
	KB_TEXT_IN		IN	BYTEA,
	KB_AUT_IN		IN	VARCHAR
) RETURNS NUMERIC AS 
$$
DECLARE
	KB_ID_VAL	NUMERIC(20,0);
BEGIN
	SELECT KB_ID INTO KB_ID_VAL FROM KB_ART_TBL WHERE KB_KEY = KB_KEY_IN;
	IF(KB_ID_VAL IS NULL) THEN
		
		KB_ID_VAL := nextval('KB_ART_SEQ');
		INSERT INTO KB_ART_TBL(KB_ID, KB_KEY, KB_TYPE, KB_TEXT, KB_CRT_AUT, KB_LUP_AUT) VALUES
			(KB_ID_VAL, KB_KEY_IN, KB_TYPE_IN, KB_TEXT_IN, KB_AUT_IN, KB_AUT_IN);
	ELSE
		UPDATE KB_ART_TBL SET KB_TYPE = KB_TYPE_IN, KB_LUP_UTC = CURRENT_TIMESTAMP, KB_LUP_AUT = KB_AUT_IN,
			KB_TEXT = KB_TEXT_IN WHERE KB_ID = KB_ID_VAL;
	END IF;
	RETURN KB_ID_VAL;
END
$$ LANGUAGE plpgsql;

-- @FUNCTION
-- GET ARTICLE
CREATE OR REPLACE FUNCTION FND_KB_ART
(
	KB_KEY_IN	IN VARCHAR
) RETURNS SETOF KB_ART_TBL AS
$$
BEGIN
	RETURN QUERY SELECT * FROM KB_ART_TBL WHERE KB_KEY = KB_KEY_IN;
END
$$ LANGUAGE plpgsql;

-- @FUNCTION
-- GET ARTICLE
CREATE OR REPLACE FUNCTION GET_KB_ART
(
	KB_ART_IN	IN NUMERIC
) RETURNS SETOF KB_ART_TBL AS
$$
BEGIN
	RETURN QUERY SELECT * FROM KB_ART_TBL WHERE KB_ID = KB_ID_IN;
END
$$ LANGUAGE plpgsql;