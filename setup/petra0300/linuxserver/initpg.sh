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

echo "loading initial data..."
#make sure that error messages are caught, independent of the local language of the server
echo "set lc_messages to 'en_GB.UTF-8';" > /tmp/demodata-PostgreSQL.sql
cat db30/demodata-PostgreSQL.sql >> /tmp/demodata-PostgreSQL.sql
sudo -u postgres psql openpetra -q < /tmp/demodata-PostgreSQL.sql | grep -e "ERROR|CONTEXT"

echo "enabling indexes and constraints..."
echo "SET client_min_messages TO warning;" > /tmp/createconstraints-PostgreSQL.sql
cat db30/createconstraints-PostgreSQL.sql >> /tmp/createconstraints-PostgreSQL.sql
sudo -u postgres psql openpetra -q < /tmp/createconstraints-PostgreSQL.sql
