We are using the SKR49 file from GnuCash, convert it from XML to our yml format with a Python script, and then you can import it into OpenPetra.

```
curl https://raw.githubusercontent.com/Gnucash/gnucash/maint/data/accounts/de_DE/acctchrt_skr49.gnucash-xea > acctchrt_skr49.gnucash-xea
export LANG=de_DE.utf8
python3 convert.py > skr49_accounts.yml
```

Now import that file `skr49_accounts.yml` to your OpenPetra instance, https://openpetra.example.org/Finance/Setup/GL/AccountTree
