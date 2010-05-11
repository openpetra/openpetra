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

. /lib/lsb/init-functions

start() {
    log_daemon_msg "Starting CruiseControl"

    su -u timop -c "cd $CruiseControlPath/Server; mono ccnet.exe" &
    su -u timop -c "cd $CruiseControlPath/WebDashboard; xsp --address 192.168.1.1 --nonstop" &
    status=0
    log_end_msg $status
}

stop() {
    killall -9 mono    
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
    *)
        echo "Usage: $0 {start|stop|restart}"
        exit 1
        ;;
esac

exit 0

