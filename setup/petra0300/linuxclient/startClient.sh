#!/bin/bash
cd bin30
. /opt/mono-openpetra/env.sh
if [ ! -z "$1" ]
then
	# replace hostname in config file; need to run as root with permissions
	if [ -w ../etc30/PetraClientRemote.config ]
	then
		sed -i "s#http://.*9000#http://$1/openpetra9000#g" ../etc30/PetraClientRemote.config
	fi
	mono PetraClient.exe -C:../etc30/PetraClientRemote.config -OpenPetra.HTTPServer:http://$1/openpetra9000 &
else
	mono PetraClient.exe -C:../etc30/PetraClientRemote.config &
fi
cd ..
