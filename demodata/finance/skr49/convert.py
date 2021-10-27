#!/bin/python3

import xml.etree.ElementTree as ET
import os

gnucash_skr49_url = "https://raw.githubusercontent.com/Gnucash/gnucash/maint/data/accounts/de_DE/acctchrt_skr49.gnucash-xea"
if not os.path.isfile(os.path.basename(gnucash_skr49_url)):
    print("Please download: curl %s > %s" % (gnucash_skr49_url,os.path.basename(gnucash_skr49_url)))
    exit(-1)

if not ("LANG" in os.environ and os.environ["LANG"] == "de_DE.utf8"):
    print("Please export LANG=de_DE.utf8")
    exit(-1)

mytree = ET.parse(os.path.basename(gnucash_skr49_url))
myroot = mytree.getroot()
namespaces = {'gnc': 'http://www.gnucash.org/XML/gnc','act':'http://www.gnucash.org/XML/act', 'gnc-act':'http://www.gnucash.org/XML/gnc-act'}

class Account(object):
    pass

def export_account(parent, accounts, depth):
    children = {}
    for acct_id in accounts:
        account = accounts[acct_id]
        if account.parent == parent.id:
            children[account.code] = account

    for code in sorted(children):
        account = children[code]

        is_leaf = True
        for acct_child_id in accounts:
            acct_child = accounts[acct_child_id]
            if acct_child.parent == account.id:
                is_leaf = False
                break

        active="True"
        if account.descr == "VERALTET!":
            active = "False"
        line = ((depth*2)*' ')
        line += '%s: {active=%s' % (account.code, active)
        
        if parent.parent is None:
            line += ', type=%s, debitcredit=%s, validcc=All' % (account.type, account.debitcredit)
        else:
            if parent.type != account.type:
                line += ', type=%s' % (account.type,)
            if parent.debitcredit != account.debitcredit:
                line += ', debitcredit=%s' % (account.debitcredit,)
        if is_leaf:
            line += ', validcc=Local'
        if account.descr == account.shortdescr:
            line += ', shortdesc="%s"' % (account.descr)
        else:
            line += ', shortdesc="%s", longdesc="%s"' % (account.shortdescr, account.descr)
        if account.bankaccount == True:
            line += ', bankaccount=true'
        line += "}"
        print(line)
        
        if not is_leaf:
            export_account(account, accounts, depth + 1)


accounts = {}
root_account = None

# parse all accounts
for acct in myroot.findall('.//gnc:account', namespaces):
    obj = Account()
    obj.id = acct.find('act:id', namespaces).text
    if acct.find('act:parent', namespaces) is not None:
        obj.parent = acct.find('act:parent', namespaces).text
    else:
        obj.parent = None
    obj.name = acct.find('act:name', namespaces).text
    if acct.find('act:code', namespaces) is not None:
        obj.code = acct.find('act:code', namespaces).text
    else:
        # bug: there is a missing code, in 4500-4504 Abschreibungen
        obj.code = obj.name.split(' ')[0]
    # bug in account 0674
    if obj.name == "0675 Gegenkonto 0653-0654, 0661-0664, 0670-0672, 0675-0679, 0687-0689, 0697-0699 bei Aufteilung Debitorenkonto":
        obj.code = "0674"
        obj.name = "0674 Gegenkonto 0653-0654, 0661-0664, 0670-0672, 0675-0679, 0687-0689, 0697-0699 bei Aufteilung Debitorenkonto"
    # bug: duplicate code 5490
    if obj.code == "5490" and obj.id == "0467d26561464460851449d59cb02696":
        # ignore this
        continue
    obj.shortdescr = obj.name[len(obj.code) + 1:]
    if acct.find('act:description', namespaces) is not None:
        obj.descr = acct.find('act:description', namespaces).text.replace("\n", " ")
    else:
        obj.descr = ''
    obj.type = acct.find('act:type', namespaces).text
    obj.bankaccount = False
    if obj.type == "ASSET" or obj.type == "ROOT":
        obj.type = "Asset"
        obj.debitcredit = "debit"
    elif obj.type == "INCOME":
        obj.type = "Income"
        obj.debitcredit = "credit"
    elif obj.type == "EXPENSE":
        obj.type = "Expense"
        obj.debitcredit = "debit"
    elif obj.type == "LIABILITY":
        obj.type = "Liability"
        obj.debitcredit = "credit"
    elif obj.type == "CASH" or obj.type == "BANK":
        obj.type = "Asset"
        obj.debitcredit = "debit"
        obj.bankaccount = True
    elif obj.type == "PAYABLE":
        obj.type = "Liability"
        obj.debitcredit = "credit"
    elif obj.type == "RECEIVABLE":
        obj.type = "Asset"
        obj.debitcredit = "debit"
    elif obj.type == "CREDIT":
        obj.type = "Liability"
        obj.debitcredit = "credit"
    else:
        raise Exception("unknown type %s for account %s" % (obj.type,obj.name))

    if obj.parent is None:
        root_account = obj
    else:
        accounts[obj.id] = obj

# export hierarchy
title = myroot.findall('.//gnc-act:title', namespaces)[0].text
shortdesc = myroot.findall('.//gnc-act:short-description', namespaces)[0].text
longdesc = myroot.findall('.//gnc-act:long-description', namespaces)[0].text.replace('\n', '\n# ')

print("# %s" % (title,))
print("#")
print("# %s" % (shortdesc,))
print("#")
print("# %s" % (longdesc,))
print("RootNodeInternal:")
export_account(root_account, accounts, 1)

