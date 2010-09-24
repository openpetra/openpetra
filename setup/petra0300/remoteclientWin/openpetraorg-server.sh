#!/bin/sh
#
# chkconfig: 345 96 24
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
# Short-Description:    OpenPetra.org CRP server
### END INIT INFO

OpenPetraOrgPath=/usr/local/openpetra/{#ORGNAMEWITHOUTSPACE}
CustomerName={#ORGNAME}

. /lib/lsb/init-functions

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
    menu)
        menu
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|menu}"
        exit 1
        ;;
esac

exit 0
