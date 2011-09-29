if [ "$OPENPETRA_RDBMSType" == '' ]
then
    echo "Please first run your config.sh!"
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
touch $OpenPetraOrgPath/log30/Server.log
mkdir -p `dirname $OPENPETRA_LocationPublicKeyFile`
ln -s $OpenPetraOrgPath/etc30/publickey.xml $OPENPETRA_LocationPublicKeyFile
mkdir -p /etc/sysconfig/openpetra
ln -s $OpenPetraOrgPath/config.sh /etc/sysconfig/openpetra/OpenPetra`basename $OpenPetraOrgPath`
ln -s $OpenPetraOrgPath/openpetraorg-server.sh /etc/init.d/OpenPetra`basename $OpenPetraOrgPath`
chmod a+x /etc/init.d/OpenPetra`basename $OpenPetraOrgPath`
cat /var/lib/pgsql/9.1/data/pg_hba.conf | grep md5 > /var/lib/pgsql/9.1/data/pg_hba.conf.new
mv /var/lib/pgsql/9.1/data/pg_hba.conf.new /var/lib/pgsql/9.1/data/pg_hba.conf
echo "local  $OPENPETRA_DBNAME $OPENPETRA_DBUSER   md5" >> /var/lib/pgsql/9.1/data/pg_hba.conf
echo "host  $OPENPETRA_DBNAME $OPENPETRA_DBUSER  127.0.0.1/32   md5" >> /var/lib/pgsql/9.1/data/pg_hba.conf
# for postgres user we need the following line:
echo "local   postgres         postgres        ident">> /var/lib/pgsql/9.1/data/pg_hba.conf
/etc/init.d/postgresql-9.1 restart

chown -R $userName $OpenPetraOrgPath

update-rc.d -f /etc/init.d/OpenPetra`basename $OpenPetraOrgPath` defaults

# TODO: install cron job for running the backup each night
# $OpenPetraOrgPath/openpetraorg-server.sh backup