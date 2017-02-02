#!/bin/sh
#
# description: Starts and stops the OpenPetra server running with Mono
#

export OpenPetraPath=/usr/local/openpetra
export documentroot=/var/www/openpetra
export OPENPETRA_DBPORT=5432
export OPENPETRA_RDBMSType=mysql

if [ -z "$NAME" ]
then
  export NAME=openpetra-server
  export userName=openpetra
  export OPENPETRA_DBPWD=@RandomDBPassword@
  export OPENPETRA_DBUSER=petraserver
  export OPENPETRA_DBNAME=openpetra
  export OPENPETRA_PORT=@HostedPort@
  export OPENPETRA_DBHOST=localhost
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
      cd /var/www/openpetra
      LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$OpenPetraPath/bin30 fastcgi-mono-server4 /socket=tcp:127.0.0.1:$OPENPETRA_PORT /applications=/:$documentroot /appconfigfile=/home/$userName/etc/PetraServerConsole.config /logfile=/home/$userName/log/mono.log /loglevels=Standard >& /dev/null
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
    then
      cd $OpenPetraPath/bin30; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:Stop
    else
      echo "Error: can only stop the server as user $userName"
      exit -1
    fi
}

# load a new database from a yml.gz file. this will overwrite the current database!
loadYmlGz() {
    su - $userName -c "cd $OpenPetraPath/bin30; mono --runtime=v4.0 /Ict.Petra.Tools.MSysMan.YmlGzImportExport.exe -C:/home/$userName/etc/PetraServerConsole.config -Action:load -YmlGzFile:$ymlgzfile"
}

# dump the database to a yml.gz file
dumpYmlGz() {
    su - $userName -c "cd $OpenPetraPath/bin30; mono --runtime=v4.0 /Ict.Petra.Tools.MSysMan.YmlGzImportExport.exe -C:/home/$userName/etc/PetraServerConsole.config -Action:dump -YmlGzFile:$ymlgzfile"
}

# display a menu to check for logged in users etc
menu() {
    su - $userName -c "cd $OpenPetraPath/bin30; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config"
}

# backup the mysql database
backup() {
    echo `date` "Writing to " $backupfile
    su $userName -c "mysqldump --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT --user=$OPENPETRA_DBUSER $OPENPETRA_DBNAME | gzip > $backupfile"
    echo `date` "Finished!"
}

# restore the mysql database
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
    echo "creating database..."

    echo "drop database if exists $OPENPETRA_DBNAME;" > $OpenPetraPath/tmp30/createtables-MySQL.sql
    echo "create database if not exists $OPENPETRA_DBNAME;" >> $OpenPetraPath/tmp30/createtables-MySQL.sql
    echo "use $OPENPETRA_DBNAME;" >> $OpenPetraPath/tmp30/createtables-MySQL.sql
    cat $OpenPetraPath/db30/createtables-MySQL.sql >> $OpenPetraPath/tmp30/createtables-MySQL.sql
    echo "GRANT SELECT,UPDATE,DELETE,INSERT ON * TO $OPENPETRA_DBUSER IDENTIFIED BY '$OPENPETRA_DBPWD'" >> $OpenPetraPath/tmp30/createtables-MySQL.sql
    mysql -u root --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT < $OpenPetraPath/tmp30/createtables-MySQL.sql
    rm $OpenPetraPath/tmp30/createtables-MySQL.sql

    echo "loading data..."
    echo $backupfile|grep -qE '\.gz$'
    if [ $? -eq 0 ]
    then
        cat $backupfile | gunzip | mysql -u $OPENPETRA_DBUSER --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME > $OpenPetraPath/log30/mysqlload.log
    else
        mysql -u $OPENPETRA_DBUSER --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $backupfile > $OpenPetraPath/log30/mysqlload.log
    fi

    echo "enabling indexes and constraints..."
    mysql -u $OPENPETRA_DBUSER --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $OpenPetraPath/db30/createconstraints-MySQL.sql

    echo `date` "Finished!"
}

init() {
    echo "preparing OpenPetra..."

    mkdir -p $OpenPetraPath/tmp30

    if [ "$OPENPETRA_DBHOST" == "localhost" ]
    then
      echo "initialise database"
      systemctl start mariadb
      systemctl enable mariadb
      echo "drop database if exists $OPENPETRA_DBNAME;" > $OpenPetraPath/tmp30/createdb-MySQL.sql
      echo "create database if not exists $OPENPETRA_DBNAME;" >> $OpenPetraPath/tmp30/createdb-MySQL.sql
      echo "USE $OPENPETRA_DBNAME;" >> $OpenPetraPath/tmp30/createdb-MySQL.sql
      echo "GRANT ALL ON $OPENPETRA_DBNAME.* TO $OPENPETRA_DBUSER@localhost IDENTIFIED BY '$OPENPETRA_DBPWD'" >> $OpenPetraPath/tmp30/createdb-MySQL.sql
      mysql -u root --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT < $OpenPetraPath/tmp30/createdb-MySQL.sql
      rm -f $OpenPetraPath/tmp30/createdb-MySQL.sql
    fi

    useradd --home /home/$userName $userName
    mkdir -p /home/$userName/log
    mkdir -p /home/$userName/tmp
    mkdir -p /home/$userName/etc
    mkdir -p /home/$userName/backup
    # copy config files (server, serveradmin.config) to etc, with adjustments
    cat $OpenPetraPath/etc30/PetraServerConsole.config \
       | sed -e "s/OPENPETRA_RDBMSType/$OPENPETRA_RDBMSType/" \
       | sed -e "s/OPENPETRA_DBHOST/$OPENPETRA_DBHOST/" \
       | sed -e "s/OPENPETRA_DBUSER/$OPENPETRA_DBUSER/" \
       | sed -e "s/OPENPETRA_DBNAME/$OPENPETRA_DBNAME/" \
       | sed -e "s/OPENPETRA_DBPORT/$OPENPETRA_DBPORT/" \
       | sed -e "s~PG_OPENPETRA_DBPWD~$OPENPETRA_DBPWD~" \
       | sed -e "s/USERNAME/$userName/" \
       > /home/$userName/etc/PetraServerConsole.config
    cat $OpenPetraPath/etc30/PetraServerAdminConsole.config \
       | sed -e "s/USERNAME/$userName/" \
       | sed -e "s/OPENPETRA_PORT/$OPENPETRA_PORT/" \
       > /home/$userName/etc/PetraServerAdminConsole.config

    chown -R $userName:$userName /home/$userName

    echo "creating tables..."
    mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $OpenPetraPath/db30/createtables-MySQL.sql

    echo "initial data..."
    # insert initial data so that loadymlgz will work
    cat > $OpenPetraPath/tmp30/init-MySQL.sql <<FINISH
insert into s_user(s_user_id_c) values ('SYSADMIN');
insert into s_module(s_module_id_c) values ('SYSMAN');
insert into s_user_module_access_permission(s_user_id_c, s_module_id_c, s_can_access_l) values('SYSADMIN', 'SYSMAN', true);
insert into s_system_status (s_user_id_c, s_system_login_status_l) values ('SYSADMIN', true);
FINISH
    mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $OpenPetraPath/tmp30/init-MySQL.sql

    echo "enabling indexes and constraints..."
    mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $OpenPetraPath/db30/createconstraints-MySQL.sql

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
    dumpYmlGz)
        dumpYmlGz
        ;;
    menu)
        menu
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|menu|backup|restore|init|loadYmlGz|dumpYmlGz}"
        exit 1
        ;;
esac

exit 0
