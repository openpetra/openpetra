#!/bin/bash
# you have to run this as user with sudo rights

echo "creating database..."
sudo -u postgres psql -c "DROP DATABASE IF EXISTS \"openpetra{#ORGNAMEWITHOUTSPACE}\";"
sudo -u postgres psql -c "DROP USER IF EXISTS \"petraserver{#ORGNAMEWITHOUTSPACE}\";"
sudo -u postgres psql -c "CREATE USER \"petraserver{#ORGNAMEWITHOUTSPACE}\" PASSWORD '{#DBPASSWORD}'; "
sudo -u postgres createdb -T template0 openpetra{#ORGNAMEWITHOUTSPACE}

echo "creating tables..."
echo "SET client_min_messages TO warning;" > /tmp/createtables-PostgreSQL.sql
cat db30/createtables-PostgreSQL.sql >> /tmp/createtables-PostgreSQL.sql
sudo -u postgres psql openpetra{#ORGNAMEWITHOUTSPACE} -q < /tmp/createtables-PostgreSQL.sql
rm /tmp/createtables-PostgreSQL.sql

echo "loading initial data..."
#make sure that error messages are caught, independent of the local language of the server
#echo "set lc_messages to 'en_GB.UTF-8';" > /tmp/demodata-PostgreSQL.sql
cat db30/demodata-PostgreSQL.sql >> /tmp/demodata-PostgreSQL.sql
sudo -u postgres psql openpetra{#ORGNAMEWITHOUTSPACE} -q < /tmp/demodata-PostgreSQL.sql | grep -e "ERROR|CONTEXT"
rm /tmp/demodata-PostgreSQL.sql

echo "enabling indexes and constraints..."
echo "SET client_min_messages TO warning;" > /tmp/createconstraints-PostgreSQL.sql
cat db30/createconstraints-PostgreSQL.sql >> /tmp/createconstraints-PostgreSQL.sql
sudo -u postgres psql openpetra{#ORGNAMEWITHOUTSPACE} -q < /tmp/createconstraints-PostgreSQL.sql
rm /tmp/createconstraints-PostgreSQL.sql

echo "grant permissions..."
touch /tmp/grantpermissions-PostgreSQL.sql
chown postgres /tmp/grantpermissions-PostgreSQL.sql
sudo -u postgres psql openpetra{#ORGNAMEWITHOUTSPACE} -c "select 'GRANT SELECT,UPDATE,DELETE,INSERT ON ' || c.relname || ' TO petraserver{#ORGNAMEWITHOUTSPACE};' from pg_class AS c LEFT JOIN pg_namespace n ON n.oid = c.relnamespace where c.relkind = 'r' and n.nspname NOT IN('pg_catalog', 'pg_toast') and pg_table_is_visible(c.oid);" -t -o /tmp/grantpermissions-PostgreSQL.sql
sudo chown `whoami` /tmp/grantpermissions-PostgreSQL.sql
sudo -u postgres psql openpetra{#ORGNAMEWITHOUTSPACE} -q < /tmp/grantpermissions-PostgreSQL.sql
rm /tmp/grantpermissions-PostgreSQL.sql

touch /tmp/grantpermissions-PostgreSQL-seq.sql
chown postgres /tmp/grantpermissions-PostgreSQL-seq.sql
sudo -u postgres psql openpetra{#ORGNAMEWITHOUTSPACE} -c "select 'GRANT SELECT,UPDATE,USAGE ON ' || c.relname || ' TO petraserver{#ORGNAMEWITHOUTSPACE};' from pg_class AS c LEFT JOIN pg_namespace n ON n.oid = c.relnamespace where c.relkind = 'S' and n.nspname NOT IN('pg_catalog', 'pg_toast') and pg_table_is_visible(c.oid);" -t -o /tmp/grantpermissions-PostgreSQL-seq.sql
sudo -u postgres psql openpetra{#ORGNAMEWITHOUTSPACE} -q < /tmp/grantpermissions-PostgreSQL-seq.sql
rm /tmp/grantpermissions-PostgreSQL-seq.sql
