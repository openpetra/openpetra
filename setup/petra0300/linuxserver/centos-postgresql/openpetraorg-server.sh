#!/bin/sh
#
# chkconfig: 345 96 24
# description: Starts and stops the openpetraorg server running with Mono
#

# Find the name of the script
NAME=`basename $0`
if [ ${NAME:0:1} = "S" -o ${NAME:0:1} = "K" ]
then
        NAME=${NAME:3}
fi

if [ ! -d $OpenPetraOrgPath ]
then
  export mono=/opt/novell/mono/bin
  export OpenPetraOrgPath=/usr/local/openpetraorg
  export CustomerName=DefaultTOREPLACE
  export OPENPETRA_LocationKeyFile=
  export OPENPETRA_RDBMSType=postgresql
  export OPENPETRA_DBPWD=TOBESETBYINSTALLER
  export OPENPETRA_DBHOST=localhost
  export OPENPETRA_DBPORT=5432
  export OPENPETRA_DBUSER=petraserver
  export OPENPETRA_DBNAME=openpetra
  export OPENPETRA_PORT=9000
  export backupfile=$OpenPetraOrgPath/backup-`date +%Y%m%d`.sql.gz
fi

# Override defaults from /etc/sysconfig/openpetra if file is present
[ -f /etc/sysconfig/openpetra/${NAME} ] && . /etc/sysconfig/openpetra/${NAME}

if [ "$2" != "" ]
then
  backupfile=$2
fi

. /lib/lsb/init-functions

log_daemon_msg() { logger "$@"; echo "$@"; }
log_end_msg() { [ $1 -eq 0 ] && RES=OK; logger ${RES:=FAIL}; }

# start the openpetraorg server
start() {
    log_daemon_msg "Starting OpenPetra.org server for $CustomerName"

    cd $OpenPetraOrgPath/bin30
    parameters="-Server.DBPassword:PG_OPENPETRA_DBPWD -Server.DBUserName:$OPENPETRA_DBUSER -Server.DBName:$OPENPETRA_DBNAME -Server.DBPort:$OPENPETRA_DBPORT -Server.DBHostOrFile:$OPENPETRA_DBHOST -Server.IPBasePort:$OPENPETRA_PORT -Server.RDBMSType:$OPENPETRA_RDBMSType -Server.ChannelEncryption.PrivateKeyfile:$OPENPETRA_LocationPrivateKeyFile"
    su $userName -c "$mono --server PetraServerConsole.exe -C:$OpenPetraOrgPath/etc30/PetraServerConsole.config $parameters -RunWithoutMenu:true &> /dev/null &"
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
    parameters="-Server.IPBasePort:$OPENPETRA_PORT -Server.ChannelEncryption.PublicKeyfile:$OPENPETRA_LocationPublicKeyFile"
    su $userName -c "$mono --server PetraServerAdminConsole.exe -C:$OpenPetraOrgPath/etc30/PetraServerAdminConsole.config $parameters -Command:Stop"
    status=0
    log_end_msg $status
}

# display a menu to check for logged in users etc
menu() {
    cd $OpenPetraOrgPath/bin30
    parameters="-Server.IPBasePort:$OPENPETRA_PORT -Server.ChannelEncryption.PublicKeyfile:$OPENPETRA_LocationPublicKeyFile"
    su $userName -c "$mono --server PetraServerAdminConsole.exe -C:$OpenPetraOrgPath/etc30/PetraServerAdminConsole.config $parameters"
}

# backup the postgresql database
backup() {
    echo `date` "Writing to " $backupfile
    su $userName -c "pg_dump --data-only -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME | gzip > $backupfile"
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
    echo "creating database..."
    su postgres -c "psql -q -p $OPENPETRA_DBPORT -c \"DROP DATABASE IF EXISTS \\\"$OPENPETRA_DBNAME\\\"\""
    su postgres -c "psql -q -p $OPENPETRA_DBPORT -c \"DROP USER IF EXISTS \\\"$OPENPETRA_DBUSER\\\"\""
    su postgres -c "psql -q -p $OPENPETRA_DBPORT -c \"CREATE USER \\\"$OPENPETRA_DBUSER\\\" PASSWORD '$OPENPETRA_DBPWD'\""
    su postgres -c "createdb -p $OPENPETRA_DBPORT -T template0 -O $OPENPETRA_DBUSER $OPENPETRA_DBNAME"

    export PGOPTIONS='--client-min-messages=warning'

    echo "creating tables..."
    su $userName -c "psql -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $OpenPetraOrgPath/db30/createtables-PostgreSQL.sql"

    echo "loading data..."
    echo $backupfile|grep -qE '\.gz$'
    if [ $? -eq 0 ]
    then
        su $userName -c "cat $backupfile | gunzip | psql -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q > $OpenPetraOrgPath/log30/pgload.log"
    else
        su $userName -c "psql -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $backupfile > $OpenPetraOrgPath/log30/pgload.log"
    fi

    echo "enabling indexes and constraints..."
    su $userName -c "psql -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $OpenPetraOrgPath/db30/createconstraints-PostgreSQL.sql"

    echo `date` "Finished!"
}

init() {
    export backupfile=$OpenPetraOrgPath/db30/demodata-PostgreSQL.sql
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
