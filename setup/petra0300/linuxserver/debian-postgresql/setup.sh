if [ "$OPENPETRA_RDBMSType" == '' ]
then
    if [ ! -f config.sh ]
    then
      cp config-sample.sh config.sh
    fi

    echo "Please first run your config.sh!"
    exit
fi

mkdir -p $OpenPetraOrgPath
cp -R * $OpenPetraOrgPath
useradd --home /home/$userName $userName

echo "*:$OPENPETRA_DBPORT:$OPENPETRA_DBNAME:$OPENPETRA_DBUSER:$OPENPETRA_DBPWD" >> /home/$userName/.pgpass
mkdir /home/$userName
chown $userName:$userName /home/$userName
chmod 600 /home/$userName/.pgpass
chown $userName /home/$userName/.pgpass

cd $OpenPetraOrgPath
mkdir -p $OpenPetraOrgPath/log30
touch $OpenPetraOrgPath/log30/Server.log
cp $OpenPetraOrgPath/etc30/publickey-sample.xml $OpenPetraOrgPath/etc30/publickey.xml
cp $OpenPetraOrgPath/etc30/privatekey-sample.xml $OpenPetraOrgPath/etc30/privatekey.xml
mkdir -p `dirname $OPENPETRA_LocationPublicKeyFile`
ln -s $OpenPetraOrgPath/etc30/publickey.xml $OPENPETRA_LocationPublicKeyFile
mkdir -p /etc/openpetra
ln -s $OpenPetraOrgPath/config.sh /etc/openpetra/openpetra`basename $OpenPetraOrgPath`
ln -s $OpenPetraOrgPath/openpetraorg-server.sh /etc/init.d/openpetra`basename $OpenPetraOrgPath`
chmod a+x /etc/init.d/openpetra`basename $OpenPetraOrgPath`
cat /etc/postgresql/9.1/main/pg_hba.conf | grep md5 > /etc/postgresql/9.1/main/pg_hba.conf.new
mv /etc/postgresql/9.1/main/pg_hba.conf.new /etc/postgresql/9.1/main/pg_hba.conf
echo "local  $OPENPETRA_DBNAME $OPENPETRA_DBUSER   md5" >> /etc/postgresql/9.1/main/pg_hba.conf
echo "host  $OPENPETRA_DBNAME $OPENPETRA_DBUSER  127.0.0.1/32   md5" >> /etc/postgresql/9.1/main/pg_hba.conf
# for postgres user we need the following line:
echo "local   postgres         postgres        ident">> /etc/postgresql/9.1/main/pg_hba.conf
/etc/init.d/postgresql restart

chown -R $userName $OpenPetraOrgPath

update-rc.d -f openpetra`basename $OpenPetraOrgPath` defaults

# TODO: install cron job for running the backup each night
# $OpenPetraOrgPath/openpetraorg-server.sh backup

cd -