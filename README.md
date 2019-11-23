OpenPetra
=========

INTRODUCTION
------------
The goal of OpenPetra is to provide an easy to use software system for the administration of non-profit organisations.

To find out more about OpenPetra, please visit the webpage [www.openpetra.org](http://www.openpetra.org)

DEVELOPMENT SETUP
-----------------

These are the steps required to setup a development environment on CentOS7:

```
curl https://getopenpetra.com | bash -s devenv
```

You can test your OpenPetra installation at http://localhost and http://localhost/api/. 
The default user is DEMO and password DEMO, or user SYSADMIN and password CHANGEME.

For development, do this to get a list of available commands:

```
su - op_dev
cd openpetra
nant help
```

LICENSE
-------
All code written by the OpenPetra development team is licensed under the GPL v3 or later.
For third-party code, please see the license references in the respective directories (see csharp/ThirdParty).
