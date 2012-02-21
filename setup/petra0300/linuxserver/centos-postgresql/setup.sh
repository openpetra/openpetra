if [ "$OPENPETRA_RDBMSType" == '' ]
then
    if [ ! -f config.sh ]
    then
      cp config-sample.sh config.sh
    fi

    echo "Please first run your config.sh!"
    exit
fi

if [ ! -f config.sh ]
then
  # the admin has run a config.sh file, but we cannot copy it to the destination because it does not exist in the right place
  echo "Please copy the config.sh!"
  exit
fi

mkdir -p $OpenPetraOrgPath
cp -R * $OpenPetraOrgPath
useradd $userName

echo "*:$OPENPETRA_DBPORT:$OPENPETRA_DBNAME:$OPENPETRA_DBUSER:$OPENPETRA_DBPWD" >> /home/$userName/.pgpass
chmod 600 /home/$userName/.pgpass
chown $userName /home/$userName/.pgpass

cd $OpenPetraOrgPath
mkdir -p $OpenPetraOrgPath/log30
mkdir -p $OpenPetraOrgPath/backup30
touch $OpenPetraOrgPath/log30/Server.log
if [ ! -f $OpenPetraOrgPath/etc30/publickey.xml ]
then
  cp $OpenPetraOrgPath/etc30/publickey-sample.xml $OpenPetraOrgPath/etc30/publickey.xml
fi
if [ ! -f $OpenPetraOrgPath/etc30/privatekey.xml ]
then
  cp $OpenPetraOrgPath/etc30/privatekey-sample.xml $OpenPetraOrgPath/etc30/privatekey.xml
fi
mkdir -p `dirname $OPENPETRA_LocationPublicKeyFile`
if [ ! -h $OPENPETRA_LocationPublicKeyFile ]
then
  ln -s $OpenPetraOrgPath/etc30/publickey.xml $OPENPETRA_LocationPublicKeyFile
fi
mkdir -p /etc/sysconfig/openpetra
ln -s $OpenPetraOrgPath/config.sh /etc/sysconfig/openpetra/openpetra`basename $OpenPetraOrgPath`
ln -s $OpenPetraOrgPath/openpetraorg-server.sh /etc/init.d/openpetra`basename $OpenPetraOrgPath`
chmod a+x /etc/init.d/openpetra`basename $OpenPetraOrgPath`

# make sure that our lines are first in the file
echo "local  $OPENPETRA_DBNAME $OPENPETRA_DBUSER   md5" > /var/lib/pgsql/9.1/data/pg_hba.conf.new
echo "host  $OPENPETRA_DBNAME $OPENPETRA_DBUSER  127.0.0.1/32   md5" >> /var/lib/pgsql/9.1/data/pg_hba.conf.new
# for postgres user we need the following line (for creating new databases):
#commented since it should be there by default
#echo "local   postgres         postgres        ident">> /var/lib/pgsql/9.1/data/pg_hba.conf
cat /var/lib/pgsql/9.1/data/pg_hba.conf >> /var/lib/pgsql/9.1/data/pg_hba.conf.new
mv /var/lib/pgsql/9.1/data/pg_hba.conf.new /var/lib/pgsql/9.1/data/pg_hba.conf
/etc/init.d/postgresql-9.1 restart

chown -R $userName $OpenPetraOrgPath

chkconfig openpetra`basename $OpenPetraOrgPath` on

# TODO: install cron job for running the backup each night
# $OpenPetraOrgPath/openpetraorg-server.sh backup

cd -