#!/bin/bash
#
# description: Starts and stops the openpetraorg server running with Mono
#
### BEGIN INIT INFO
# Provides:             openpetraorg
# Required-Start:       $mysql
# Required-Stop:
# Should-Start:
# Should-Stop:
# Default-Start:        2 3 4 5
# Default-Stop:         0 1 6
# Short-Description:    OpenPetra.org ERP server
### END INIT INFO

# Find the name of the script
NAME=`basename $0`
if [ ${NAME:0:1} = "S" -o ${NAME:0:1} = "K" ]
then
        NAME=${NAME:3}
fi

if [ ! -d $OpenPetraOrgPath ]
then
  export mono=mono
  export OpenPetraOrgPath=/usr/local/openpetraorg
  export CustomerName=DefaultTOREPLACE
  export OPENPETRA_LocationKeyFile=
  export OPENPETRA_RDBMSType=mysql
  export OPENPETRA_DBPWD=TOBESETBYINSTALLER
  export OPENPETRA_DBHOST=localhost
  export OPENPETRA_DBPORT=5432
  export OPENPETRA_DBUSER=petraserver
  export OPENPETRA_DBNAME=openpetra
  export OPENPETRA_PORT=9000
  export backupfile=$OpenPetraOrgPath/backup30/backup-`date +%Y%m%d`.sql.gz
fi

# Override defaults from /etc/sysconfig/openpetra if file is present
[ -f /etc/openpetra/${NAME} ] && . /etc/openpetra/${NAME}

if [ "$2" != "" ]
then
  backupfile=$2
fi

. /lib/lsb/init-functions

# start the openpetraorg server
start() {
    log_daemon_msg "Starting OpenPetra.org server for $CustomerName"

    cd $OpenPetraOrgPath/bin30
    parameters="-Server.DBPassword:ENV_OPENPETRA_DBPWD -Server.DBUserName:$OPENPETRA_DBUSER -Server.DBName:$OPENPETRA_DBNAME -Server.DBPort:$OPENPETRA_DBPORT -Server.DBHostOrFile:$OPENPETRA_DBHOST -Server.Port:$OPENPETRA_PORT -Server.RDBMSType:$OPENPETRA_RDBMSType -Server.ChannelEncryption.PrivateKeyfile:$OPENPETRA_LocationPrivateKeyFile"
    su $userName -c "$mono --runtime=v4.0 --server PetraServerConsole.exe -C:$OpenPetraOrgPath/etc30/PetraServerConsole.config $parameters -RunWithoutMenu:true &> /dev/null &"
    # in order to see if the server started successfully, wait a few seconds and then show the end of the log file
    sleep 5
    tail $OpenPetraOrgPath/log30/Server.log

    # TODO: check Server.log for errors
    #status=´ps xaf | grep $CustomerName´
    status=0
    log_end_msg $status
}

# stop the openpetraorg server
stop() {
    log_daemon_msg "Stopping OpenPetra.org server for $CustomerName"
    cd $OpenPetraOrgPath/bin30
    parameters="-Server.Port:$OPENPETRA_PORT -Server.ChannelEncryption.PublicKeyfile:$OPENPETRA_LocationPublicKeyFile"
    su $userName -c "$mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:$OpenPetraOrgPath/etc30/PetraServerAdminConsole.config $parameters -Command:Stop"
    status=0
    log_end_msg $status
}

# display a menu to check for logged in users etc
menu() {
    cd $OpenPetraOrgPath/bin30
    parameters="-Server.Port:$OPENPETRA_PORT -Server.ChannelEncryption.PublicKeyfile:$OPENPETRA_LocationPublicKeyFile"
    su $userName -c "$mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:$OpenPetraOrgPath/etc30/PetraServerAdminConsole.config $parameters"
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

    echo "drop database if exists $OPENPETRA_DBNAME;" > $OpenPetraOrgPath/tmp30/createtables-MySQL.sql
    echo "create database if not exists $OPENPETRA_DBNAME;" >> $OpenPetraOrgPath/tmp30/createtables-MySQL.sql
    echo "use $OPENPETRA_DBNAME;" >> $OpenPetraOrgPath/tmp30/createtables-MySQL.sql
    cat $OpenPetraOrgPath/db30/createtables-MySQL.sql >> $OpenPetraOrgPath/tmp30/createtables-MySQL.sql
    echo "GRANT SELECT,UPDATE,DELETE,INSERT ON * TO $OPENPETRA_DBUSER IDENTIFIED BY 'OPENPETRA_DBPWD'" >> $OpenPetraOrgPath/tmp30/createtables-MySQL.sql
    mysql -u root --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT -p < $OpenPetraOrgPath/tmp30/createtables-MySQL.sql
    rm $OpenPetraOrgPath/tmp30/createtables-MySQL.sql

    echo "loading data..."
    echo $backupfile|grep -qE '\.gz$'
    if [ $? -eq 0 ]
    then
        cat $backupfile | gunzip | mysql -u $OPENPETRA_DBUSER --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME > $OpenPetraOrgPath/log30/mysqlload.log
    else
        mysql -u $OPENPETRA_DBUSER --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $backupfile > $OpenPetraOrgPath/log30/mysqlload.log
    fi

    echo "enabling indexes and constraints..."
    mysql -u $OPENPETRA_DBUSER --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $OpenPetraOrgPath/db30/createconstraints-MySQL.sql

    echo `date` "Finished!"
}

init() {
    export backupfile=$OpenPetraOrgPath/db30/demodata-MySQL.sql
    restore
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
    menu)
        menu
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|menu|backup|restore}"
        exit 1
        ;;
esac

exit 0
