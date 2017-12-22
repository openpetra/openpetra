OpenPetra
=========

INTRODUCTION
------------
The goal of OpenPetra is to provide an easy to use software system for the administration of non-profit organisations.

To find out more about OpenPetra, please visit the webpage [www.openpetra.org](http://www.openpetra.org)

The code for the Javascript client is in a separate repository, at [github.com/openpetra/openpetra-client-js](https://github.com/openpetra/openpetra-client-js).

DEVELOPMENT SETUP
-----------------

These are the steps required to setup a development environment on CentOS7:

```
# install required packages
yum install epel-release git
yum install mono-devel nant nunit xsp libsodium

# clone the code repositories
git clone --depth 10 https://github.com/openpetra/openpetra.git
git clone https://github.com/openpetra/openpetra-client-js.git

cd openpetra

# setup the basic configuration
vi OpenPetra.build.config

    <?xml version="1.0"?>
    <project name="OpenPetra-userconfig">
        <property name="DBMS.Type" value="sqlite"/>
        <property name="Server.DebugLevel" value="0"/>
    </project>

# this will take a couple of minutes while code is generated and the solution gets compiled
nant generateSolution

# create a fresh sqlite database
nant recreateDatabase resetDatabase

# run the server with xsp4
nant start
# stop the server
nant stop
```

You can test your OpenPetra installation at http://localhost:9000 and http://localhost:9000/api

LICENSE
-------
All code written by the OpenPetra development team is licensed under the GPL v3 or later.
For third-party code, please see the license references in the respective directories (see csharp/ThirdParty).
