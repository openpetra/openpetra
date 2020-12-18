OpenPetra
=========

[![Build status](https://ci.appveyor.com/api/projects/status/github/openpetra/openpetra?branch=test&svg=true)](https://ci.openpetra.org)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

INTRODUCTION
------------
The goal of OpenPetra is to provide an easy to use software system for the administration of non-profit organisations.

You can manage your contacts (CRM) and your accounts (ERP) with OpenPetra. We have a special focus on processing and receipting donations. More features are being implemented as the demand for OpenPetra grows.

To find out more about OpenPetra, please visit the webpage [www.openpetra.org](http://www.openpetra.org)

DEMO
----

You are welcome to try the demo at https://demo.openpetra.org. This is a public service, so don't enter any real data!

OPENPETRA AS A SERVICE
----------------------

Please have a look at https://www.openpetra.com for your own free test installation of OpenPetra, with unlimited testing period!

COMMUNITY
---------

There is an english forum at https://forum.openpetra.org, and for german speakers we have https://forum.openpetra.de

DEVELOPMENT SETUP
-----------------

These are the steps required to setup a development environment on CentOS7:

```
curl https://get.openpetra.org | bash -s devenv
```

For development, do this to get a list of available commands:

```
su - op_dev
cd openpetra
nant help
```

You can test your OpenPetra installation at http://localhost and http://localhost/api/. 
The default user is DEMO and password DEMO, or user SYSADMIN and password CHANGEME.

LICENSE
-------
All code written by the OpenPetra development team is licensed under the GPL v3 or later.
For third-party code, please see the license references in the respective directories (see csharp/ThirdParty).
