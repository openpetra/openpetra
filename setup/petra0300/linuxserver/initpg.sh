#!/bin/bash
# you have to run this as user with sudo rights
# see also openpetraorg-server.sh init

echo "creating database..."
sudo -u postgres psql -c "DROP DATABASE IF EXISTS \"openpetra\";"
sudo -u postgres psql -c "DROP USER IF EXISTS \"petraserver\";"
sudo -u postgres psql -c "CREATE USER \"petraserver\" PASSWORD 'TOBESETBYINSTALLER'; "
sudo -u postgres createdb -T template0 -O petraserver openpetra

export PGOPTIONS='--client-min-messages=warning'

echo "creating tables..."
psql -U petraserver openpetra -q -f db30/createtables-PostgreSQL.sql

echo "loading initial data..."
psql -U petraserver openpetra -q -f db30/demodata-PostgreSQL.sql > log30/pgload.log

echo "enabling indexes and constraints..."
psql -U petraserver openpetra -q -f db30/createconstraints-PostgreSQL.sql
