#!/bin/sh
#
# description: Starts and stops the OpenPetra server running with Mono
#

export OpenPetraPath=/usr/local/openpetra
export documentroot=$OpenPetraPath/server
export OPENPETRA_DBPORT=3306
export OPENPETRA_RDBMSType=mysql

generatepwd() {
  dd bs=1024 count=1 if=/dev/urandom status=none | tr -dc 'a-zA-Z0-9#?_' | fold -w 32 | head -n 1
}

if [ -z "$OP_CUSTOMER" ]
then
  export OP_CUSTOMER=openpetra
  export userName=openpetra
  export OPENPETRA_DBUSER=petraserver
  export OPENPETRA_DBNAME=openpetra
  export OPENPETRA_URL=localhost
  export OPENPETRA_HTTP_URL=http://localhost
  export OPENPETRA_PORT=9000
  export OPENPETRA_HTTP_PORT=80
  export OPENPETRA_DBHOST=localhost
  config=/home/$OP_CUSTOMER/etc/PetraServerConsole.config
  if [ -f $config ]
  then
    export OPENPETRA_DBPWD=`cat $config | grep DBPassword | awk -F'"' '{print $4}'`
  fi
else
  config=/home/$OP_CUSTOMER/etc/PetraServerConsole.config
  if [ -f $config ]
  then
    export userName=$OP_CUSTOMER
    export OPENPETRA_DBHOST=`cat $config | grep DBHostOrFile | awk -F'"' '{print $4}'`
    export OPENPETRA_DBUSER=`cat $config | grep DBUserName | awk -F'"' '{print $4}'`
    export OPENPETRA_DBNAME=`cat $config | grep DBName | awk -F'"' '{print $4}'`
    export OPENPETRA_DBPORT=`cat $config | grep DBPort | awk -F'"' '{print $4}'`
    export OPENPETRA_DBPWD=`cat $config | grep DBPassword | awk -F'"' '{print $4}'`
    export OPENPETRA_PORT=`cat $config | grep "Server.Port" | awk -F'"' '{print $4}'`
  elif [ -z $OPENPETRA_DBUSER ]
  then
    echo "cannot find $config"
    exit -1
  fi
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
      cd $documentroot
      LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$OpenPetraPath/bin fastcgi-mono-server4 /socket=tcp:127.0.0.1:$OPENPETRA_PORT /applications=/:$documentroot /appconfigfile=/home/$userName/etc/PetraServerConsole.config /logfile=/home/$userName/log/mono.log /loglevels=Standard >& /dev/null
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
      cd $OpenPetraPath/bin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:Stop
    else
      echo "Error: can only stop the server as user $userName"
      exit -1
    fi
}

# load a new database from a yml.gz file. this will overwrite the current database!
loadYmlGz() {
    su - $userName -c "cd $OpenPetraPath/bin; mono --runtime=v4.0 Ict.Petra.Tools.MSysMan.YmlGzImportExport.exe -C:/home/$userName/etc/PetraServerConsole.config -Action:load -YmlGzFile:$ymlgzfile"
}

# dump the database to a yml.gz file
dumpYmlGz() {
    su - $userName -c "cd $OpenPetraPath/bin; mono --runtime=v4.0 Ict.Petra.Tools.MSysMan.YmlGzImportExport.exe -C:/home/$userName/etc/PetraServerConsole.config -Action:dump -YmlGzFile:$ymlgzfile"
}

# display the status to check for logged in users etc
status() {
    su - $userName -c "cd $OpenPetraPath/bin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:ConnectedClients"
}

# display a menu to check for logged in users etc
menu() {
    su - $userName -c "cd $OpenPetraPath/bin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config"
}

# export variables for debugging to use mysql on the command line
mysqlscript() {
    export DBHost=$OPENPETRA_DBHOST
    export DBUser=$OPENPETRA_DBUSER
    export DBName=$OPENPETRA_DBNAME
    export DBPort=$OPENPETRA_DBPORT
    export DBPwd=$OPENPETRA_DBPWD

    if [ -z "$MYSQL_CMD" ]; then
      echo "visit http://localhost/phpMyAdmin, with user $DBUser and password $DBPwd"
      echo "calling: mysql -u $DBUser -h $DBHost --port=$DBPort --password=\"$DBPwd\" $DBName --default-character-set=utf8"
      mysql -u $DBUser -h $DBHost --port=$DBPort --password="$DBPwd" $DBName --default-character-set=utf8
    else
      echo $MYSQL_CMD | mysql -u $DBUser -h $DBHost --port=$DBPort --password="$DBPwd" $DBName --default-character-set=utf8
    fi
}

# backup the mysql database
backup() {
    echo `date` "Writing to " $backupfile
    cat > /home/$userName/.my.cnf <<FINISH
[mysqldump]
user=$OPENPETRA_DBUSER
password=$OPENPETRA_DBPWD
FINISH
    chown $userName:$userName /home/$userName/.my.cnf
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

    echo "DROP DATABASE IF EXISTS \`$OPENPETRA_DBNAME\`;" > $OpenPetraPath/tmp/createtables-MySQL.sql
    echo "CREATE DATABASE IF NOT EXISTS \`$OPENPETRA_DBNAME\`;" >> $OpenPetraPath/tmp/createtables-MySQL.sql
    echo "USE \`$OPENPETRA_DBNAME\`;" >> $OpenPetraPath/tmp/createtables-MySQL.sql
    cat $OpenPetraPath/db/createtables-MySQL.sql >> $OpenPetraPath/tmp/createtables-MySQL.sql
    echo "GRANT ALL ON \`$OPENPETRA_DBNAME\`.* TO \`$OPENPETRA_DBUSER\` IDENTIFIED BY '$OPENPETRA_DBPWD'" >> $OpenPetraPath/tmp/createtables-MySQL.sql
    mysql -u root --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT < $OpenPetraPath/tmp/createtables-MySQL.sql
    rm $OpenPetraPath/tmp/createtables-MySQL.sql

    echo "loading data and constraints and indexes..."
    echo $backupfile|grep -qE '\.gz$'
    if [ $? -eq 0 ]
    then
        cat $backupfile | gunzip | mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME > /home/$userName/log/mysqlload.log
    else
        mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $backupfile > /home/$userName/log/mysqlload.log
    fi

    echo `date` "Finished!"
}

init() {
    if [ -z "$OPENPETRA_URL" ]
    then
      echo "please define the URL for your OpenPetra, eg. OPENPETRA_URL=demo.openpetra.org openpetra-server init"
      exit -1
    fi

    if [ -z "$OPENPETRA_DBPWD" ]
    then
      echo "please define a password for your OpenPetra database, eg. OPENPETRA_PWD=topsecret openpetra-server init"
      exit -1
    fi

    if [ -f /home/$userName/etc/PetraServerConsole.config ]
    then
      echo "it seems there is already an instance configured"
      exit -1
    fi

    echo "preparing OpenPetra server..."

    useradd --home /home/$userName $userName
    mkdir -p /home/$userName/log
    mkdir -p /home/$userName/tmp
    mkdir -p /home/$userName/etc
    mkdir -p /home/$userName/backup

    # copy config files (server, serveradmin.config) to etc, with adjustments
    cat $OpenPetraPath/etc/PetraServerConsole.config \
       | sed -e "s/OPENPETRA_PORT/$OPENPETRA_PORT/" \
       | sed -e "s/OPENPETRA_RDBMSType/$OPENPETRA_RDBMSType/" \
       | sed -e "s/OPENPETRA_DBHOST/$OPENPETRA_DBHOST/" \
       | sed -e "s/OPENPETRA_DBUSER/$OPENPETRA_DBUSER/" \
       | sed -e "s/OPENPETRA_DBNAME/$OPENPETRA_DBNAME/" \
       | sed -e "s/OPENPETRA_DBPORT/$OPENPETRA_DBPORT/" \
       | sed -e "s~PG_OPENPETRA_DBPWD~$OPENPETRA_DBPWD~" \
       | sed -e "s~OPENPETRA_PORT~$OPENPETRA_PORT~" \
       | sed -e "s~OPENPETRA_URL~$OPENPETRA_URL~" \
       | sed -e "s~OPENPETRA_EMAILDOMAIN~$OPENPETRA_EMAILDOMAIN~" \
       | sed -e "s/USERNAME/$userName/" \
       > /home/$userName/etc/PetraServerConsole.config
    cat $OpenPetraPath/etc/PetraServerAdminConsole.config \
       | sed -e "s/USERNAME/$userName/" \
       | sed -e "s#/openpetraOPENPETRA_PORT/#:$OPENPETRA_HTTP_PORT/#" \
       > /home/$userName/etc/PetraServerAdminConsole.config

    chown -R $userName:$userName /home/$userName

    # configure nginx
    if [ $OPENPETRA_HTTP_PORT == 80 ]
    then
      # let the default nginx server run on another port
      sed -i "s/listen\(.*\)80/listen\181/g" /etc/nginx/nginx.conf
    fi

    if [[ "`grep SCRIPT_FILENAME /etc/nginx/fastcgi_params`" == "" ]]
    then
      cat >> /etc/nginx/fastcgi_params <<FINISH
fastcgi_param  PATH_INFO          "";
fastcgi_param  SCRIPT_FILENAME    \$document_root\$fastcgi_script_name;
FINISH
    fi

    mkdir -p /etc/nginx/conf.d
    cat > /etc/nginx/conf.d/$userName.conf <<FINISH
server {
    listen $OPENPETRA_HTTP_PORT;
    server_name $OPENPETRA_URL;

    root $OpenPetraPath/client;

    location / {
         rewrite ^/Settings.*$ /;
         rewrite ^/Partner.*$ /;
         rewrite ^/Finance.*$ /;
         rewrite ^/CrossLedger.*$ /;
         rewrite ^/System.*$ /;
         rewrite ^/.git/.*$ / redirect;
         rewrite ^/etc/.*$ / redirect;
    }

    location /api {
         index index.html index.htm default.aspx Default.aspx;
         fastcgi_index Default.aspx;
         fastcgi_pass 127.0.0.1:$OPENPETRA_PORT;
         include /etc/nginx/fastcgi_params;
         sub_filter_types text/html text/css text/xml;
         sub_filter 'http://127.0.0.1:$OPENPETRA_PORT' '$OPENPETRA_HTTP_URL/api';
         sub_filter 'http://localhost/api' '$OPENPETRA_HTTP_URL/api';
    }
}
FINISH
    systemctl restart nginx
    systemctl enable nginx

    if [[ "$OP_CUSTOMER" != "openpetra" ]]
    then
      # create the service script
      cp /usr/lib/systemd/system/openpetra.service /usr/lib/systemd/system/${OP_CUSTOMER}.service
      sed -i "s~OpenPetra Server~OpenPetra Server for $userName~g" /usr/lib/systemd/system/${OP_CUSTOMER}.service
      sed -i "s~User=openpetra~User=$userName\nEnvironment=OP_CUSTOMER=$userName~g" /usr/lib/systemd/system/${OP_CUSTOMER}.service
    fi
    systemctl enable ${OP_CUSTOMER}
    systemctl start ${OP_CUSTOMER}

}

# this will overwrite all existing data
initdb() {
    if [ -z "$OPENPETRA_DBPWD" ]
    then
      echo "please define a password for your OpenPetra database, eg. OPENPETRA_PWD=topsecret openpetra-server init"
      exit -1
    fi

    echo "preparing OpenPetra database..."

    mkdir -p $OpenPetraPath/tmp

    if [ "$OPENPETRA_DBHOST" == "localhost" ]
    then
      echo "initialise database"
      systemctl start mariadb
      systemctl enable mariadb
      echo "DROP DATABASE IF EXISTS \`$OPENPETRA_DBNAME\`;" > $OpenPetraPath/tmp/createdb-MySQL.sql
      echo "CREATE DATABASE IF NOT EXISTS \`$OPENPETRA_DBNAME\`;" >> $OpenPetraPath/tmp/createdb-MySQL.sql
      echo "USE \`$OPENPETRA_DBNAME\`;" >> $OpenPetraPath/tmp/createdb-MySQL.sql
      echo "GRANT ALL ON \`$OPENPETRA_DBNAME\`.* TO \`$OPENPETRA_DBUSER\`@localhost IDENTIFIED BY '$OPENPETRA_DBPWD'" >> $OpenPetraPath/tmp/createdb-MySQL.sql
      mysql -u root --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT < $OpenPetraPath/tmp/createdb-MySQL.sql
      rm -f $OpenPetraPath/tmp/createdb-MySQL.sql
    fi
    echo "creating tables..."
    mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $OpenPetraPath/db/createdb-MySQL.sql

    echo "initial data..."
    # insert initial data so that loadymlgz will work
    cat > $OpenPetraPath/tmp/init-MySQL.sql <<FINISH
insert into s_user(s_user_id_c) values ('SYSADMIN');
insert into s_module(s_module_id_c) values ('SYSMAN');
insert into s_user_module_access_permission(s_user_id_c, s_module_id_c, s_can_access_l) values('SYSADMIN', 'SYSMAN', true);
insert into s_system_status (s_user_id_c, s_system_login_status_l) values ('SYSADMIN', true);
FINISH
    mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT $OPENPETRA_DBNAME < $OpenPetraPath/tmp/init-MySQL.sql

    # load the clean database with sysadmin user but without ledger, partners etc
    ymlgzfile=$OpenPetraPath/db/clean.yml.gz
    loadYmlGz

    # if url does not start with demo.
    if [[ ! $OPENPETRA_URL == demo.* ]]
    then
      mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT \
           -e "UPDATE s_user SET s_password_needs_change_l = 1 WHERE s_user_id_c = 'SYSADMIN'" $OPENPETRA_DBNAME
      mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT \
           -e "DELETE FROM s_user WHERE s_user_id_c <> 'SYSADMIN'" $OPENPETRA_DBNAME
    fi

    if [ -z $SYSADMIN_PWD ]
    then
      SYSADMIN_PWD="CHANGEME"
    fi

    su - $userName -c "cd $OpenPetraPath/bin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:SetPassword -UserID:SYSADMIN -NewPassword:'$SYSADMIN_PWD'"

    echo "For production use, please change the password for user SYSADMIN immediately (initial password: $SYSADMIN_PWD)"
}

# this will update the current database
upgradedb() {
    su - $userName -c "cd $OpenPetraPath/bin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:UpgradeDatabase"
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
    generatepwd)
        generatepwd
        ;;
    init)
        init
        ;;
    initdb)
        initdb
        ;;
    upgradedb)
        upgradedb
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
    mysql)
        mysqlscript
        ;;
    status)
        status
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|menu|status|mysql|backup|restore|init|initdb|upgradedb|loadYmlGz|dumpYmlGz}"
        exit 1
        ;;
esac

exit 0
