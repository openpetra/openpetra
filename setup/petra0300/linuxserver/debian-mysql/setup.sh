if [ "$OPENPETRA_RDBMSType" == '' ]
then
    echo "Please first run your config.sh!"
    exit
fi

mkdir -p $OpenPetraOrgPath
cp -R * $OpenPetraOrgPath
useradd $userName

echo "[client]\npassword=$OPENPETRA_DBPWD" > /home/$userName/my.cnf
chmod 600 /home/$userName/my.cnf
chown $userName /home/$userName/my.cnf

cd $OpenPetraOrgPath
mkdir -p $OpenPetraOrgPath/log30
touch $OpenPetraOrgPath/log30/Server.log
mkdir -p `dirname $OPENPETRA_LocationPublicKeyFile`
ln -s $OpenPetraOrgPath/etc30/publickey.xml $OPENPETRA_LocationPublicKeyFile
mkdir -p /etc/sysconfig/openpetra
ln -s $OpenPetraOrgPath/config.sh /etc/sysconfig/openpetra/OpenPetra`basename $OpenPetraOrgPath`
ln -s $OpenPetraOrgPath/openpetraorg-server.sh /etc/init.d/OpenPetra`basename $OpenPetraOrgPath`
chmod a+x /etc/init.d/OpenPetra`basename $OpenPetraOrgPath`

chown -R $userName $OpenPetraOrgPath

update-rc.d -f /etc/init.d/OpenPetra`basename $OpenPetraOrgPath` defaults

# TODO: install cron job for running the backup each night
# $OpenPetraOrgPath/openpetraorg-server.sh backup