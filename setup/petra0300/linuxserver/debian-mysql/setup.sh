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

mkdir /home/$userName
echo "[client]\npassword=$OPENPETRA_DBPWD" > /home/$userName/my.cnf
chown -R $userName:$userName /home/$userName
chmod 600 /home/$userName/my.cnf
chown $userName /home/$userName/my.cnf

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

chown -R $userName $OpenPetraOrgPath

update-rc.d -f /etc/init.d/openpetra`basename $OpenPetraOrgPath` defaults

# TODO: install cron job for running the backup each night
# $OpenPetraOrgPath/openpetraorg-server.sh backup

cd -