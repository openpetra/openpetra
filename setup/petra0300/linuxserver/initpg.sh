#!/bin/bash
# you have to run this as user postgres
# eg. su postgres -c initpg.sh
dropdb petra; 
psql -c "DROP OWNED BY petraserver; DROP USER IF EXISTS petraserver; CREATE USER petraserver PASSWORD 'TOBESETBYINSTALLER'; CREATE USER timop PASSWORD 'TOBESETBYINSTALLER'; "
createdb -T template0 petra
pg_restore -d petra db30/demo-pg.bak