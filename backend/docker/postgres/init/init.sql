CREATE DATABASE keycloak;
CREATE DATABASE financial;

CREATE USER keycloak_user WITH ENCRYPTED PASSWORD 'keycloak';
CREATE USER financial_user WITH PASSWORD 'financial';

GRANT ALL PRIVILEGES ON DATABASE financial TO financial_user;

ALTER DATABASE keycloak OWNER TO keycloak_user;

\connect keycloak

GRANT ALL ON SCHEMA public TO keycloak_user;
ALTER SCHEMA public OWNER TO keycloak_user;

GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO keycloak_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO keycloak_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA public
GRANT ALL ON TABLES TO keycloak_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA public
GRANT ALL ON SEQUENCES TO keycloak_user;


