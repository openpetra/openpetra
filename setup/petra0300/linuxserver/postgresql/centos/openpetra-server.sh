#!/bin/sh
#
# description: Starts and stops the OpenPetra server running with Mono
#

if [ -z "$OpenPetraPath" ]
then
  export OpenPetraPath=/usr/local/openpetra
  export documentroot=/var/www/openpetra
  export OPENPETRA_DBPORT=5432
  export OPENPETRA_RDBMSType=postgresql
  export OPENPETRA_DBHOST=localhost

  export userName=openpetra
  export OPENPETRA_DBPWD=@RandomDBPassword@
  export OPENPETRA_DBUSER=petraserver
  export OPENPETRA_DBNAME=openpetra
  export OPENPETRA_PORT=@HostedPort@
fi

if [ -z "$backupfile" ]
then
  export backupfile=/home/$userName/backup/backup-`date +%Y%m%d`.sql.gz
fi

if [ "$2" != "" ]
then
  backupfile=$2
  useremail=$2
  ymlgzfile=$2
fi

# start the openpetra server
start() {
    echo "Starting OpenPetra server"
    if [ "`whoami`" = "$userName" ]
    then
      fastcgi-mono-server4 /socket=tcp:127.0.0.1:$OPENPETRA_PORT /applications=/:$documentroot /appconfigfile=/home/$userName/etc/PetraServerConsole.config /logfile=/home/$userName/log/mono.log /loglevels=Standard >& /dev/null
      # other options for loglevels: Debug Notice Warning Error Standard(=Notice Warning Error) All(=Debug Standard)
    else
      echo "Error: can only start the server as user $userName"
      exit -1
    fi
}

# stop the openpetra server
stop() {
    echo "Stopping OpenPetra server"
    if [ "`whoami`" = "$userName" ]
      cd $OpenPetraPath/bin30; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:Stop
    else
      echo "Error: can only stop the server as user $userName"
      exit -1
    fi
}

# load a new database from a yml.gz file. this will overwrite the current database!
loadYmlGz() {
    su - $userName -c "cd $OpenPetraPath/bin30; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:LoadYmlGz -YmlGzFile:$ymlgzfile"
}

# display a menu to check for logged in users etc
menu() {
    su - $userName -c "cd $OpenPetraPath/bin30; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config"
}

# backup the postgresql database
backup() {
    echo `date` "Writing to " $backupfile
    # loading of this dump will show errors about existing data tables etc.
    # could have 2 calls: --data-only and --schema-only.
    su - $userName -c "pg_dump -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME | gzip > $backupfile"
    echo `date` "Finished!"
}

# restore the postgresql database
restore() {
    echo "This will overwrite your database!!!"
    echo "Please enter 'yes' if that is ok:"
    read response
    if [ "$response" != 'yes' ]
    then
        echo "Cancelled the restore"
        exit
    fi
    echo `date` "Start restoring from " $backupfile
    echo "dropping tables and sequences..."

    delCommand="SELECT 'DROP TABLE ' || n.nspname || '.' || c.relname || ' CASCADE;' FROM pg_catalog.pg_class AS c LEFT JOIN pg_catalog.pg_namespace AS n ON n.oid = c.relnamespace WHERE relkind = 'r' AND n.nspname NOT IN ('pg_catalog', 'pg_toast') AND pg_catalog.pg_table_is_visible(c.oid)" 
    su - $userName -c "psql -t -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -c \"$delCommand\" > /tmp/deleteAllTables.sql"

    delCommand="SELECT 'DROP SEQUENCE ' || n.nspname || '.' || c.relname || ';' FROM pg_catalog.pg_class AS c LEFT JOIN pg_catalog.pg_namespace AS n ON n.oid = c.relnamespace WHERE relkind = 'S' AND n.nspname NOT IN ('pg_catalog', 'pg_toast') AND pg_catalog.pg_table_is_visible(c.oid)" 
    su - $userName -c "psql -t -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -c \"$delCommand\" > /tmp/deleteAllSequences.sql"
    su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f /tmp/deleteAllTables.sql"
    su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f /tmp/deleteAllSequences.sql"

    rm /tmp/deleteAllTables.sql
    rm /tmp/deleteAllSequences.sql

    export PGOPTIONS='--client-min-messages=warning'

    echo "creating tables..."
    su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $OpenPetraPath/db30/createtables-PostgreSQL.sql"

    echo "loading data..."
    echo $backupfile|grep -qE '\.gz$'
    if [ $? -eq 0 ]
    then
        su - $userName -c "cat $backupfile | gunzip | psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q > /home/$userName/log/pgload.log"
    else
        su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $backupfile > /home/$userName/log/pgload.log"
    fi

    echo "enabling indexes and constraints..."
    su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $OpenPetraPath/db30/createconstraints-PostgreSQL.sql"

    echo `date` "Finished!"
}

init() {
    echo "creating database..."
    postgresql-setup initdb
    systemctl start postgresql
    systemctl enable postgresql

    if [ ! "`cat /var/lib/pgsql/data/pg_hba.conf | grep '^host  '$OPENPETRA_DBNAME' '$OPENPETRA_DBUSER'  ::1/128   md5'`" ]; then 
       echo "local  $OPENPETRA_DBNAME $OPENPETRA_DBUSER   md5" > /var/lib/pgsql/data/pg_hba.conf.new 
       echo "host  $OPENPETRA_DBNAME $OPENPETRA_DBUSER  ::1/128   md5" >> /var/lib/pgsql/data/pg_hba.conf.new 
       echo "host  $OPENPETRA_DBNAME $OPENPETRA_DBUSER  127.0.0.1/32   md5" >> /var/lib/pgsql/data/pg_hba.conf.new 
       cat /var/lib/pgsql/data/pg_hba.conf >> /var/lib/pgsql/data/pg_hba.conf.new 
       mv -f /var/lib/pgsql/data/pg_hba.conf.new /var/lib/pgsql/data/pg_hba.conf 
       systemctl restart postgresql
       su - postgres -c "psql -q -p $OPENPETRA_DBPORT -c \"CREATE USER \\\"$OPENPETRA_DBUSER\\\" PASSWORD '$OPENPETRA_DBPWD'\"" 
       su - postgres -c "createdb -p $OPENPETRA_DBPORT -T template0 -O $OPENPETRA_DBUSER $OPENPETRA_DBNAME" 
    else 
       # there has already been an installation. 
       systemctl stop ${NAME}
    fi 

    useradd --home /home/$userName $userName
    mkdir -p /home/$userName/log
    mkdir -p /home/$userName/tmp
    mkdir -p /home/$userName/etc
    mkdir -p /home/$userName/backup
    hostname=`hostname`
    # copy config files (server, serveradmin.config) to etc, with adjustments
    cat $OpenPetraPath/etc30/PetraServerConsole.config \
       | sed -e "s/OPENPETRA_DBHOST/$OPENPETRA_DBHOST/" \
       | sed -e "s/OPENPETRA_DBUSER/$OPENPETRA_DBUSER/" \
       | sed -e "s/OPENPETRA_DBNAME/$OPENPETRA_DBNAME/" \
       | sed -e "s/OPENPETRA_DBPORT/$OPENPETRA_DBPORT/" \
       | sed -e "s/USERNAME/$userName/" \
       > /home/$userName/etc/PetraServerConsole.config
    cat $OpenPetraPath/etc30/PetraServerAdminConsole.config \
       | sed -e "s/USERNAME/$userName/" \
       | sed -e "s/OPENPETRA_PORT/$OPENPETRA_PORT/" \
       > /home/$userName/etc/PetraServerAdminConsole.config

    addPwd=1
    if [ -f /home/$userName/.pgpass ]
    then
        if [ "`cat /home/$userName/.pgpass | grep '^*:'$OPENPETRA_DBPORT':'$OPENPETRA_DBNAME':'$OPENPETRA_DBUSER':'`" ]; then
            addPwd=0
        fi
    fi
    if [ $addPwd -eq 1 ]
    then
        echo "*:$OPENPETRA_DBPORT:$OPENPETRA_DBNAME:$OPENPETRA_DBUSER:$OPENPETRA_DBPWD" >> /home/$userName/.pgpass
    fi
    chown -R $userName:$userName /home/$userName
    chmod 600 /home/$userName/.pgpass
    chown $userName /home/$userName/.pgpass

    echo "creating tables..."
    su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $OpenPetraPath/db30/createtables-PostgreSQL.sql"
    echo "enabling indexes and constraints..."
    su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $OpenPetraPath/db30/createconstraints-PostgreSQL.sql"

    # insert initial data so that loadymlgz will work
    cat > $OpenPetraPath/db30/init-PostgreSQL.sql <<FINISH
insert into s_user(s_user_id_c) values ('SYSADMIN');
insert into s_module(s_module_id_c) values ('SYSMAN');
insert into s_user_module_access_permission(s_user_id_c, s_module_id_c, s_can_access_l) values('SYSADMIN', 'SYSMAN', true);
insert into s_system_status (s_user_id_c, s_system_login_status_l) values ('SYSADMIN', true);
FINISH
    su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $OpenPetraPath/db30/init-PostgreSQL.sql"

    # configure lighttpd
    cat > /etc/lighttpd/vhosts.d/openpetra$OPENPETRA_PORT.conf <<FINISH
\$HTTP["url"] =~ "^/openpetra$OPENPETRA_PORT" {
  var.server_name = "openpetra$OPENPETRA_PORT"

  server.name = "localhost/openpetra$OPENPETRA_PORT"

  server.document-root = "$documentroot"

  fastcgi.server = (
        "/openpetra$OPENPETRA_PORT" => ((
                "host" => "127.0.0.1",
                "port" => $OPENPETRA_PORT,
                "check-local" => "disable"
        ))
  )
}
FINISH
    sed -i 's~"mod_fastcgi",~#"mod_fastcgi",~g' /etc/lighttpd/modules.conf
    sed -i 's~server.modules = (~server.modules = (\n  "mod_fastcgi",~g' /etc/lighttpd/modules.conf
    sed -i 's/server.use-ipv6 = "enable"/server.use-ipv6 = "disable"/g' /etc/lighttpd/lighttpd.conf
    sed -i 's/server.max-connections = 1024/server.max-connections = 512/g' /etc/lighttpd/lighttpd.conf
    sed -i 's~#include_shell "cat /etc/lighttpd/vhosts\.d/\*\.conf"~include_shell "cat /etc/lighttpd/vhosts.d/*.conf"~g' /etc/lighttpd/lighttpd.conf
    systemctl restart lighttpd
    systemctl enable lighttpd
    systemctl enable ${NAME}
    systemctl enable postgresql
    systemctl start ${NAME}

    # load the base database with demo and sysadmin user
    ymlgzfile=$OpenPetraPath/db30/base.yml.gz
    loadYmlGz
}

case "$1" in
    start)
        start
        ;;
    stop)
        stop
        ;;
    restart)
        stop
        sleep 3
        start
        ;;
    backup)
        backup
        ;;
    restore)
        restore
        ;;
    init)
        init
        ;;
    loadYmlGz)
        loadYmlGz
        ;;
    menu)
        menu
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|menu|backup|restore|init|loadYmlGz}"
        exit 1
        ;;
esac

exit 0
