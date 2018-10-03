Description
===========

OpenPetra Client written in Javascript is a client for OpenPetra running in the web browser.

License
=======

This code is licensed under GPL v3 or later.

Copyright 2017 by TBits.net GmbH

Setup for Development
=====================

See the [instructions for the OpenPetra server](https://github.com/openpetra/openpetra#development-setup), which includes the installation of the client.

Running the tests
=================

    npm install
    # run from commandline
    LANG=en CYPRESS_baseUrl=http://localhost ./node_modules/.bin/cypress run --config video=false --spec cypress/integration/partner_edit.js
    # run with GUI (Chrome/Chromium required)
    LANG=en CYPRESS_baseUrl=http://localhost ./node_modules/.bin/cypress open
