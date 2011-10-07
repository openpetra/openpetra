#!/bin/sh
#
# chkconfig: 345 96 24
# description: Starts and stops the openpetraorg server running with Mono
#
### BEGIN INIT INFO
# Provides:             openpetraorg
# Required-Start:       $postgresql
# Required-Stop:
# Should-Start:
# Should-Stop:
# Default-Start:        2 3 4 5
# Default-Stop:         0 1 6
# Short-Description:    OpenPetra.org ERP server
### END INIT INFO

OpenPetraOrgPath=/usr/local/openpetraorg/TOREPLACE
CustomerName=DefaultTOREPLACE
export PGPASSWORD=TOBESETBYINSTALLER

backupfile=$OpenPetraOrgPath/backup-`date +%Y%m%d`.sql.gz

if [ "$2" != "" ]
then
  backupfile=$2
fi

. /lib/lsb/init-functions

#for RedHat/CentOS, activate the following lines
#log_daemon_msg() { logger "$@"; }
#log_end_msg() { [ $1 -eq 0 ] && RES=OK; logger ${RES:=FAIL}; }

# start the openpetraorg server
start() {
    log_daemon_msg "Starting OpenPetra.org server for $CustomerName"

    cd $OpenPetraOrgPath/bin30
    touch $OpenPetraOrgPath/log30/Server.log
    mono --server PetraServerConsole.exe -C:$OpenPetraOrgPath/etc30/PetraServerConsole.config -RunWithoutMenu:true &> /dev/null &
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
    mono --server PetraServerAdminConsole.exe -C:$OpenPetraOrgPath/etc30/PetraServerAdminConsole.config -Command:Stop
    status=0
    log_end_msg $status
}

# backup the postgresql database
backup() {
    echo `date` "Writing to " $backupfile
    pg_dump --data-only -U petraserver openpetra | gzip > $backupfile
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
    sudo -u postgres psql -c "DROP DATABASE IF EXISTS \"openpetra\";"
    sudo -u postgres psql -c "DROP USER IF EXISTS \"petraserver\";"
    sudo -u postgres psql -c "CREATE USER \"petraserver\" PASSWORD '$PGPASSWORD'; "
    sudo -u postgres createdb -T template0 -O petraserver openpetra

    export PGOPTIONS='--client-min-messages=warning'

    echo "creating tables..."
    psql -U petraserver openpetra -q -f $OpenPetraOrgPath/db30/createtables-PostgreSQL.sql

    echo "loading data..."
    echo $backupfile|grep -qE '\.gz$'
    if [ $? -eq 0 ]
    then
        cat $backupfile | gunzip | psql -U petraserver openpetra -q > $OpenPetraOrgPath/log30/pgload.log
    else
        psql -U petraserver openpetra -q -f $backupfile > $OpenPetraOrgPath/log30/pgload.log
    fi

    echo "enabling indexes and constraints..."
    psql -U petraserver openpetra -q -f $OpenPetraOrgPath/db30/createconstraints-PostgreSQL.sql

    echo `date` "Finished!"
}

init() {
    export backupfile=$OpenPetraOrgPath/db30/demodata-PostgreSQL.sql
    restore
}

# display a menu to check for logged in users etc
menu() {
    cd $OpenPetraOrgPath/bin30
    mono --server PetraServerAdminConsole.exe -C:$OpenPetraOrgPath/etc30/PetraServerAdminConsole.config
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
