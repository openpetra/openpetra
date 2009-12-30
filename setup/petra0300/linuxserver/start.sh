#!/bin/bash
cd bin30
touch ../log30/Server.log
mono --server PetraServerConsole.exe -C:../etc30/PetraServerConsole.config -RunWithoutMenu:true &> /dev/null &
# in order to see if the server started successfully, wait a few seconds and then show the end of the log file
sleep 5
tail ../log30/Server.log
cd ..
