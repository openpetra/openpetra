#!/bin/sh
#
# chkconfig: 345 96 24
# description: Starts and stops the CruiseControl server and webdashboard
#
### BEGIN INIT INFO
# Provides:             cruisecontrol
# Required-Start: 
# Required-Stop:
# Should-Start:
# Should-Stop:
# Default-Start:        2 3 4 5
# Default-Stop:         0 1 6
# Short-Description:    CruiseControl
### END INIT INFO

CruiseControlPath=/home/timop/CruiseControl.NET
RunAsUser=timop

. /lib/lsb/init-functions

start() {
    log_daemon_msg "Starting CruiseControl"

    if [[ "`whoami`" = "$RunAsUser" ]]
    then
        sh -c "cd $CruiseControlPath/Server; mono ccnet.exe" &
        sh -c "cd $CruiseControlPath/WebDashboard; xsp --address 192.168.1.1 --nonstop" &
    else
        su $RunAsUser -c "cd $CruiseControlPath/Server; mono ccnet.exe" &
        su $RunAsUser -c "cd $CruiseControlPath/WebDashboard; xsp --address 192.168.1.1 --nonstop" &
    fi
    status=0
    log_end_msg $status
}

stop() {
    killall mono
    rm -Rf /tmp/*
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
    restart-delayed)
        sh -c "sleep 60s; $0 restart" &
        ;;
    check-restart)
        # this should be called by cron job every 10 minutes
        # */10 * * * *  /etc/init.d/cruisecontrol check-restart
        if [ -f /tmp/restartccnet ]
        then
          sh -c "sleep 60s; $0 restart" &
        fi
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|restart-delayed}"
        exit 1
        ;;
esac

exit 0

