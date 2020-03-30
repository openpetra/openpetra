#!/bin/bash
#
# description: Starts and stops the OpenPetra server running with Mono
#

export OpenPetraPath=/usr/local/openpetra
export OpenPetraPathBin=/usr/local/openpetra/bin
if [ ! -d $OpenPetraPathBin ]; then
  # non-root installation
  dirname=`dirname $0`
  export OpenPetraPath=$dirname
  export OpenPetraPathBin=$dirname/server/bin
fi
export documentroot=$OpenPetraPath/server
export OPENPETRA_DBPORT=3306
export OPENPETRA_RDBMSType=mysql
export OPENPETRA_PORT=6700
export OPENPETRA_USER_PREFIX=op_
export THIS_SCRIPT=$0
export OPENPETRA_HTTP_URL="http://$OP_CUSTOMER.localhost"

servicefile=/usr/lib/systemd/system/openpetra.service
if [ ! -f $servicefile ]; then
  servicefile=/lib/systemd/system/openpetra.service
fi

if [ -f $servicefile ]; then
  if [[ ! -z "`cat $servicefile | grep postgresql`" ]]; then
    export OPENPETRA_DBPORT=5432
    export OPENPETRA_RDBMSType=postgresql
  fi
fi

generatepwd() {
  dd bs=1024 count=1 if=/dev/urandom status=none | tr -dc 'a-zA-Z0-9#?_' | fold -w 32 | head -n 1
}

getConfigOfCurrentCustomer() {
  config=/home/$OP_CUSTOMER/etc/PetraServerConsole.config
  if [ -f $config ]
  then
    export userName=$OP_CUSTOMER
    export OPENPETRA_RDBMSType=`cat $config | grep RDBMSType | awk -F'"' '{print $4}'`
    export OPENPETRA_DBHOST=`cat $config | grep DBHostOrFile | awk -F'"' '{print $4}'`
    export OPENPETRA_DBUSER=`cat $config | grep DBUserName | awk -F'"' '{print $4}'`
    export OPENPETRA_DBNAME=`cat $config | grep DBName | awk -F'"' '{print $4}'`
    export OPENPETRA_DBPORT=`cat $config | grep DBPort | awk -F'"' '{print $4}'`
    export OPENPETRA_DBPWD=`cat $config | grep DBPassword | awk -F'"' '{print $4}'`
    export OPENPETRA_HTTP_URL=`cat $config | grep Server.Url | awk -F'"' '{print $4}'`

    # previous installations were missing http or https
    if [[ ! $OPENPETRA_HTTP_URL == https://* && ! $OPENPETRA_HTTP_URL == http://* ]]
    then
        export OPENPETRA_HTTP_URL="https://$OPENPETRA_HTTP_URL"
    fi

  elif [ -z $OPENPETRA_DBUSER ]
  then
    echo "cannot find $config"
    exit -1
  fi

  if [ -f /etc/nginx/conf.d/$OP_CUSTOMER.conf ]
  then
    export OPENPETRA_PORT=`cat /etc/nginx/conf.d/$OP_CUSTOMER.conf | grep -m1 listen | sed -e 's#;##' |  awk -F' ' '{print $2}'`
  fi
}

if [ -z "$OP_CUSTOMER" ]
then
  # check if the current user starts with op_
  if [[ "`whoami`" = op_* ]]
  then
    export OP_CUSTOMER=`whoami`
  fi
fi

if [ -z "$OP_CUSTOMER" ]
then
  # we are starting or stopping openpetra, independant of an instance
  export userName=openpetra
  if [ ! -d /usr/local/openpetra ]; then
    # non-root installation
    # get location of this script, to find out the proper user
    dirname=`dirname $THIS_SCRIPT`
    export userName=`basename $dirname`
  fi
elif [ "$1" != "init" ]; then
    getConfigOfCurrentCustomer
fi

if [ -z "$backupfile" ]
then
  export backupfile=/home/$userName/backup/backup-`date +%Y%m%d%H`.sql.gz
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
      LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$documentroot/bin fastcgi-mono-server4 /socket=tcp:127.0.0.1:$OPENPETRA_PORT /applications=/:$documentroot /appconfigfile=$OpenPetraPath/etc/common.config /logfile=/var/log/mono.log /loglevels=Standard >& /dev/null &
      # other options for loglevels: Debug Notice Warning Error Standard(=Notice Warning Error) All(=Debug Standard)

      # improve speed of initial request by user by forcing to load all assemblies now
      sleep 1
      curl --retry 5 --silent http://localhost/api/serverSessionManager.asmx/IsUserLoggedIn > /dev/null

      # this process must not end, otherwise systemd stops the server
      tail -f /dev/null
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
      cd $documentroot/bin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:Stop
    else
      echo "Error: can only stop the server as user $userName"
      exit -1
    fi
}

runAsUser() {
    cmd=$1

    if [ "`whoami`" = "$userName" ]
    then
      bash -c "$cmd" || exit -1
    else
      su $userName -c "$cmd" || exit -1
    fi
}

# load a new database from a yml.gz file. this will overwrite the current database!
loadYmlGz() {
    echo "loading database from $ymlgzfile with config /home/$userName/etc/PetraServerConsole.config"
    runAsUser "cd $OpenPetraPathBin; mono --runtime=v4.0 Ict.Petra.Tools.MSysMan.YmlGzImportExport.exe -C:/home/$userName/etc/PetraServerConsole.config -Action:load -YmlGzFile:$ymlgzfile"
}

# dump the database to a yml.gz file
dumpYmlGz() {
    runAsUser "cd $OpenPetraPathBin; mono --runtime=v4.0 Ict.Petra.Tools.MSysMan.YmlGzImportExport.exe -C:/home/$userName/etc/PetraServerConsole.config -Action:dump -YmlGzFile:$ymlgzfile" || exit -1
}

# display the status to check for logged in users etc
status() {
    runAsUser "cd $OpenPetraPathBin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:ConnectedClients"
}

# display a menu to check for logged in users etc
menu() {
    runAsUser "cd $OpenPetraPathBin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config"
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
mysqlbackup() {
    echo `date` "Writing to " $backupfile
    cat > /home/$userName/.my.cnf <<FINISH
[mysqldump]
user=$OPENPETRA_DBUSER
password="$OPENPETRA_DBPWD"
FINISH
    chown $userName:$userName /home/$userName/.my.cnf
    runAsUser "mysqldump --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT --user=$OPENPETRA_DBUSER $OPENPETRA_DBNAME | gzip > $backupfile"
    echo `date` "Finished!"
}

# restore the mysql database
mysqlrestore() {
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

# backup the postgresql database
postgresqlbackup() {
    echo `date` "Writing to " $backupfile
    # loading of this dump will show errors about existing data tables etc.
    # could have 2 calls: --data-only and --schema-only.
    su - $userName -c "pg_dump -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME | gzip > $backupfile"
    echo `date` "Finished!"
}

# restore the postgresql database
postgresqlrestore() {
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

    #if pgdump was called with data-only, we would need to create the tables here
    #echo "creating tables..."
    #su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $OpenPetraPath/db30/createtables-PostgreSQL.sql"

    echo "loading data..."
    echo $backupfile|grep -qE '\.gz$'
    if [ $? -eq 0 ]
    then
        su - $userName -c "cat $backupfile | gunzip | psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q > /home/$userName/log/pgload.log"
    else
        su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $backupfile > /home/$userName/log/pgload.log"
    fi

    #if pgdump was called with data-only, we would need to create the contraints and indexes here
    #echo "enabling indexes and constraints..."
    #su - $userName -c "psql -h $OPENPETRA_DBHOST -p $OPENPETRA_DBPORT -U $OPENPETRA_DBUSER $OPENPETRA_DBNAME -q -f $OpenPetraPath/db30/createconstraints-PostgreSQL.sql"

    echo `date` "Finished!"
}

backup() {
    if [[ "$OPENPETRA_RDBMSType" == "mysql" ]]; then
        mysqlbackup
    elif [[ "$OPENPETRA_RDBMSType" == "postgresql" ]]; then
        postgresqlbackup
    fi
}

backupall() {
    for d in /home/$OPENPETRA_USER_PREFIX*; do
        if [ -d $d ]; then
            export OP_CUSTOMER=`basename $d`
            export backupfile=/home/$OP_CUSTOMER/backup/backup-`date +%Y%m%d%H`.sql.gz
            $THIS_SCRIPT backup
            rm -f /home/$OP_CUSTOMER/backup/backup-`date --date='5 days ago' +%Y%m%d`*.sql.gz
            rm -f /home/$OP_CUSTOMER/backup/backup-`date --date='6 days ago' +%Y%m%d`*.sql.gz
            rm -f /home/$OP_CUSTOMER/backup/backup-`date --date='7 days ago' +%Y%m%d`*.sql.gz
        fi
    done
}

# this will update the binary files, and each database
updateall() {
    updated_binary=0

    # first try to update the rpm package
    . /etc/os-release
    if [[ "$NAME" == "CentOS Linux" ]]; then
        package=`rpm -qa --qf "%{NAME}\n" | grep openpetranow-mysql`
        if [ ! -z $package ]; then
            updated_binary=1
            # for frequent tests, expire the cache to get the latest package immediately
            # yum clean expire-cache --disablerepo="*" --enablerepo="lbs-solidcharity-openpetra"
            yum -y update --enablerepo="lbs-solidcharity-openpetra" $package || exit -1
        fi
    fi

    # upgrade binary tarball
    if [ $updated_binary -eq 0 ]; then
        cd $OpenPetraPath
        curl --silent --location https://getopenpetra.com/openpetra-latest-bin.tar.gz > openpetra-latest-bin.tar.gz || exit -1
        rm -Rf openpetra-2*
        tar xzf openpetra-latest-bin.tar.gz || exit -1
        for d in openpetra-201*; do
            alias cp=cp
            cp -Rf $d/* $OpenPetraPath
            rm -Rf $d
        done
    fi

    systemctl restart openpetra

    count=1
    for d in /home/$OPENPETRA_USER_PREFIX*; do
        if [ -d $d ]; then
            count=$((count+1))
            if [ $count -gt 30 ]; then
                count=0
                # we have an issue with too many connections to the database server
                systemctl restart openpetra
            fi 
            export OP_CUSTOMER=`basename $d`
            $THIS_SCRIPT upgradedb
        fi
    done
}

rewrite_conf() {
    if [ -z $OPENPETRA_EMAILDOMAIN ]; then
        echo "please define the email domain, eg. OPENPETRA_EMAILDOMAIN=example.org"
        exit -1
    fi
    if [ -z $SMTPHOST ]; then
        echo "please define the variables SMTPHOST, SMTPUSER, SMTPPORT and SMTPPWD"
        exit -1
    fi

    if [ -z $OP_CUSTOMER ]; then
        for d in /home/$OPENPETRA_USER_PREFIX*; do
            if [ -d $d ]; then
                export OP_CUSTOMER=`basename $d`
                rewrite_conf
            fi
        done
    else
        getConfigOfCurrentCustomer
        rm /home/$OP_CUSTOMER/etc/PetraServerConsole.config
        init
    fi
}

init() {
    if [ -z "$OPENPETRA_DBPWD" ]
    then
      echo "please define a password for your OpenPetra database, eg. OPENPETRA_DBPWD=topsecret openpetra-server init"
      exit -1
    fi

    userName=$OP_CUSTOMER

    if [ -f /home/$userName/etc/PetraServerConsole.config ]
    then
      echo "it seems there is already an instance configured"
      exit -1
    fi

    echo "preparing OpenPetra instance..."

    id $userName > /dev/null 2>&1 || useradd --home /home/$userName -G openpetra $userName
    mkdir -p /home/$userName/log
    mkdir -p /home/$userName/tmp
    mkdir -p /home/$userName/etc
    mkdir -p /home/$userName/backup

    # copy config files (server, serveradmin.config) to etc, with adjustments
    cfgfile=/home/$userName/etc/PetraServerConsole.config
    cat $OpenPetraPath/templates/PetraServerConsole.config \
       | sed -e "s/OPENPETRA_PORT/$OPENPETRA_PORT/" \
       | sed -e "s/OPENPETRA_RDBMSType/$OPENPETRA_RDBMSType/" \
       | sed -e "s/OPENPETRA_DBHOST/$OPENPETRA_DBHOST/" \
       | sed -e "s/OPENPETRA_DBUSER/$OPENPETRA_DBUSER/" \
       | sed -e "s/OPENPETRA_DBNAME/$OPENPETRA_DBNAME/" \
       | sed -e "s/OPENPETRA_DBPORT/$OPENPETRA_DBPORT/" \
       | sed -e "s~OPENPETRA_DBPWD~$OPENPETRA_DBPWD~" \
       | sed -e "s~OPENPETRA_URL~$OPENPETRA_HTTP_URL~" \
       | sed -e "s~OPENPETRA_EMAILDOMAIN~$OPENPETRA_EMAILDOMAIN~" \
       | sed -e "s#OPENPETRAPATH#$OpenPetraPath#" \
       > $cfgfile

    if [ ! -z $LICENSECHECKURL ]
    then
        sed -i "s#LICENSECHECK_URL#$LICENSECHECKURL/api/validate.php?instance_number=#" $cfgfile
    else
        sed -i "s#LICENSECHECK_URL##" $cfgfile
    fi

    if [ ! -z $AUTHTOKENINIT ]
    then
        sed -i "s#AUTHTOKENINITIALISATION#$AUTHTOKENINIT#" $cfgfile
    else
        sed -i "s#AUTHTOKENINITIALISATION##" $cfgfile
    fi

    if [ ! -z $SMTPHOST ]
    then
        sed -i "s/SMTP_HOST/$SMTPHOST/" $cfgfile
        sed -i "s/SMTP_PORT/$SMTPPORT/" $cfgfile
        sed -i "s/SMTP_USERNAME/$SMTPUSER/" $cfgfile
        sed -i "s/SMTP_PASSWORD/$SMTPPWD/" $cfgfile
        sed -i "s/SMTP_ENABLESSL/true/" $cfgfile
        sed -i "s/SMTP_AUTHTYPE/config/" $cfgfile
    fi

    sed -i "s/USERNAME/$userName/" $cfgfile

    cat $OpenPetraPath/templates/PetraServerAdminConsole.config \
       | sed -e "s/USERNAME/$userName/" \
       | sed -e "s#/openpetraOPENPETRA_PORT/#:$OPENPETRA_PORT/#" \
       > /home/$userName/etc/PetraServerAdminConsole.config

    nginx_conf_path=/etc/nginx/conf.d/$OP_CUSTOMER.conf
    if [[ "$install_type" == "devenv" ]]; then
        cp $OpenPetraPath/templates/nginx.conf $nginx_conf_path
    else
        # drop location phpMyAdmin
        # drop the redirect for phpMyAdmin
        awk '/location \/phpMyAdmin/ {exit} {print}' $OpenPetraPath/templates/nginx.conf \
            | grep -v phpMyAdmin \
            > $nginx_conf_path
        echo "}" >> $nginx_conf_path
    fi

    SERVERNAME=${OPENPETRA_HTTP_URL/https:\/\//}
    SERVERNAME=${SERVERNAME/http:\/\//}
    sed -i "s#OPENPETRA_SERVERNAME#$SERVERNAME#g" $nginx_conf_path
    sed -i "s#OPENPETRA_PORT#$OPENPETRA_PORT#g" $nginx_conf_path
    sed -i "s#OPENPETRA_HOME#$OpenPetraPath#g" $nginx_conf_path
    sed -i "s#OPENPETRA_URL#$OPENPETRA_HTTP_URL#g" $nginx_conf_path

    touch /home/$userName/log/Server.log
    chown -R $userName:openpetra /home/$userName
    chmod g+r -R /home/$userName
    chmod g+rx /home/$userName
    chmod g+rwx /home/$userName/log
    chmod g+rwx /home/$userName/tmp
    chmod g+rwx /home/$userName/backup
    chmod g+s /home/$userName/log
    chmod g+rw /home/$userName/log/Server.log
}

# this will overwrite all existing data
mysqlinitdb() {
    mkdir -p $OpenPetraPath/tmp

    if [ "$OPENPETRA_DBHOST" == "localhost" -o "$OPENPETRA_DBHOST" == "127.0.0.1" ]
    then
      echo "initialise database"
      systemctl start mariadb
      systemctl enable mariadb
      echo "DROP DATABASE IF EXISTS \`$OPENPETRA_DBNAME\`;" > $OpenPetraPath/tmp/createdb-MySQL.sql
      echo "CREATE DATABASE IF NOT EXISTS \`$OPENPETRA_DBNAME\`;" >> $OpenPetraPath/tmp/createdb-MySQL.sql
      echo "USE \`$OPENPETRA_DBNAME\`;" >> $OpenPetraPath/tmp/createdb-MySQL.sql
      if [ ! -z "$MYSQL_ROOT_PWD" ]; then 
        echo "GRANT ALL ON \`$OPENPETRA_DBNAME\`.* TO \`$OPENPETRA_DBUSER\`@'%' IDENTIFIED BY '$OPENPETRA_DBPWD'" >> $OpenPetraPath/tmp/createdb-MySQL.sql
        mysql -u root --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT --password="$MYSQL_ROOT_PWD" < $OpenPetraPath/tmp/createdb-MySQL.sql || exit -1
      else
        echo "GRANT ALL ON \`$OPENPETRA_DBNAME\`.* TO \`$OPENPETRA_DBUSER\`@localhost IDENTIFIED BY '$OPENPETRA_DBPWD'" >> $OpenPetraPath/tmp/createdb-MySQL.sql
        mysql -u root --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT < $OpenPetraPath/tmp/createdb-MySQL.sql || exit -1
      fi
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
}

# this will overwrite all existing data
postgresqlinitdb() {

    if [ "$OPENPETRA_DBHOST" == "localhost" ]
    then
      echo "initialise database"
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
        su - postgres -c "createdb -p $OPENPETRA_DBPORT -T template0 --encoding UTF8 -O $OPENPETRA_DBUSER $OPENPETRA_DBNAME"
      fi 
    fi

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
}

initdb() {
    if [ -z "$OPENPETRA_DBPWD" ]
    then
      echo "please define a password for your OpenPetra database, eg. OPENPETRA_DBPWD=topsecret openpetra-server init"
      exit -1
    fi

    echo "preparing OpenPetra database..."

    if [[ "$OPENPETRA_RDBMSType" == "mysql" ]]; then
        mysqlinitdb
    elif [[ "$OPENPETRA_RDBMSType" == "postgresql" ]]; then
        postgresqlinitdb
    fi

    if [ -f $OpenPetraPathBin/Ict.Petra.Tools.MSysMan.YmlGzImportExport.exe ]; then

      # load the clean database with sysadmin user but without ledger, partners etc
      ymlgzfile=$OpenPetraPath/db/clean.yml.gz
      loadYmlGz

      # if url does not start with demo.
      if [[ ! $OPENPETRA_HTTP_URL == https://demo.* && ! $OPENPETRA_HTTP_URL == http://demo.* ]]
      then
        mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT \
           -e "UPDATE s_user SET s_password_needs_change_l = 1 WHERE s_user_id_c = 'SYSADMIN'" $OPENPETRA_DBNAME
        mysql -u $OPENPETRA_DBUSER --password="$OPENPETRA_DBPWD" --host=$OPENPETRA_DBHOST --port=$OPENPETRA_DBPORT \
           -e "DELETE FROM s_user WHERE s_user_id_c <> 'SYSADMIN'" $OPENPETRA_DBNAME
      fi

      if [ "$LOCK_SYSADMIN" == 1 ]
      then
        # for hosted environment, the sysadmin user will only be unlocked for a new customer
        su $userName -c "cd $OpenPetraPathBin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:LockSysadmin"
      elif [ -z $SYSADMIN_PWD ]
      then
        SYSADMIN_PWD="CHANGEME"
        echo "For production use, please change the password for user SYSADMIN immediately (initial password: $SYSADMIN_PWD)"
      else
        su $userName -c "cd $OpenPetraPathBin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$userName/etc/PetraServerAdminConsole.config -Command:SetPassword -UserID:SYSADMIN -NewPassword:'$SYSADMIN_PWD' || echo 'ERROR: password was not set. It is probably too weak... Login with password CHANGEME instead, and change it immediately!'"
      fi
    fi
}

# this will update the current database
upgradedb() {
    runAsUser "cd $OpenPetraPathBin; mono --runtime=v4.0 --server PetraServerAdminConsole.exe -C:/home/$OP_CUSTOMER/etc/PetraServerAdminConsole.config -Command:UpgradeDatabase"
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
    backupall)
        backupall
        ;;
    restore)
        if [[ "$OPENPETRA_RDBMSType" == "mysql" ]]; then
            mysqlrestore
        elif [[ "$OPENPETRA_RDBMSType" == "postgresql" ]]; then
            postgresqlrestore
        fi
        ;;
    mysql)
        mysqlscript
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
    update)
        updateall
        ;;
    rewrite_conf)
        rewrite_conf
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
    status)
        status
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|menu|status|mysql|backup|backupall|restore|init|initdb|update|upgradedb|rewrite_conf|loadYmlGz|dumpYmlGz}"
        exit 1
        ;;
esac

exit 0
