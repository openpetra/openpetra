#!/bin/bash
cd bin30
mono --server PetraServerAdminConsole.exe -C:../etc30/PetraServerAdminConsole.config -Command:Stop
cd ..