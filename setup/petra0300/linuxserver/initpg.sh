#!/bin/bash
# you have to run this as user with sudo rights

echo "creating database..."
sudo -u postgres psql -c "DROP DATABASE IF EXISTS openpetra;"
sudo -u postgres psql -c "DROP OWNED BY petraserver; DROP USER IF EXISTS petraserver; CREATE USER petraserver PASSWORD 'TOBESETBYINSTALLER'; "
sudo -u postgres createdb -T template0 openpetra

echo "creating tables..."
echo "SET client_min_messages TO warning;" > /tmp/createtables-PostgreSQL.sql
cat db30/createtables-PostgreSQL.sql >> /tmp/createtables-PostgreSQL.sql
sudo -u postgres psql openpetra -q < /tmp/createtables-PostgreSQL.sql
rm /tmp/createtables-PostgreSQL.sql

echo "loading initial data..."
#make sure that error messages are caught, independent of the local language of the server
echo "set lc_messages to 'en_GB.UTF-8';" > /tmp/demodata-PostgreSQL.sql
cat db30/demodata-PostgreSQL.sql >> /tmp/demodata-PostgreSQL.sql
sudo -u postgres psql openpetra -q < /tmp/demodata-PostgreSQL.sql | grep -e "ERROR|CONTEXT"
rm /tmp/demodata-PostgreSQL.sql

echo "enabling indexes and constraints..."
echo "SET client_min_messages TO warning;" > /tmp/createconstraints-PostgreSQL.sql
cat db30/createconstraints-PostgreSQL.sql >> /tmp/createconstraints-PostgreSQL.sql
sudo -u postgres psql openpetra -q < /tmp/createconstraints-PostgreSQL.sql
rm /tmp/createconstraints-PostgreSQL.sql

echo "grant permissions..."
sudo -u postgres psql openpetra -c "select 'GRANT SELECT,UPDATE,DELETE,INSERT ON ' || c.relname || ' TO petraserver;' from pg_class AS c LEFT JOIN pg_namespace n ON n.oid = c.relnamespace where c.relkind = 'r' and n.nspname NOT IN('pg_catalog', 'pg_toast') and pg_table_is_visible(c.oid);" -t -o /tmp/grantpermissions-PostgreSQL.sql
sudo chown `whoami` /tmp/grantpermissions-PostgreSQL.sql
echo "GRANT SELECT,UPDATE,USAGE ON seq_modification1 TO petraserver;" >> /tmp/grantpermissions-PostgreSQL.sql
echo "GRANT SELECT,UPDATE,USAGE ON seq_modification2 TO petraserver;" >> /tmp/grantpermissions-PostgreSQL.sql
echo "GRANT SELECT,UPDATE,USAGE ON s_login_s_login_process_id_r_seq TO petraserver;" >> /tmp/grantpermissions-PostgreSQL.sql
echo "GRANT SELECT,UPDATE,USAGE ON seq_location_number TO petraserver;" >> /tmp/grantpermissions-PostgreSQL.sql
echo "GRANT SELECT,UPDATE,USAGE ON seq_general_ledger_master TO petraserver;" >> /tmp/grantpermissions-PostgreSQL.sql
sudo -u postgres psql openpetra -q < /tmp/grantpermissions-PostgreSQL.sql
rm /tmp/grantpermissions-PostgreSQL.sql
