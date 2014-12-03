function Init() {
// Batches
// year
ComboboxInitValues(
                    "serverMFinance.asmx/TGiftTransactionWebConnector_GetAvailableGiftYears",
                    {
                    // TODO ledgernumber not static
                    'ALedgerNumber': '43'
                    },
                    false, // AWithEmptyOption
                    "YearNumber",
                    "YearDate",
                    "#year",
                    2);
// period
// TODO: needs to depend on the year

// costcentre
ComboboxInitValues(
                    "serverMFinance.asmx/TFinanceCacheableWebConnector_GetCacheableTable2",
                    {
                    'ACacheableTable': 'CostCentreList',
                    'AHashCode': '',
                    // TODO ledgernumber not static
                    'ALedgerNumber': '43'
                    },
                    false, // AWithEmptyOption
                    "a_cost_centre_code_c",
                    "a_cost_centre_code_c",
                    "#costcentre");

// Bank Account
ComboboxInitValues(
                    "serverMFinance.asmx/TFinanceCacheableWebConnector_GetCacheableTable2", 
                    {                                    
                        'ACacheableTable': 'AccountList',
                        'AHashCode': '',
                        'ALedgerNumber': '43'
                      // TODO: only "BankAccountFlag":true   
                    },
                    false, // AWithEmptyOption
                    "a_account_code_c",
                    "a_account_code_c",
                    "#bankaccount");
// Currency Code
ComboboxInitValues(
                    "serverMPartner.asmx/TPartnerCacheableWebConnector_GetCacheableTable",
                    {
                        'ACacheableTable': 'CurrencyCodeList',
                        'AHashCode': '',
                      // TODO: only "BankAccountFlag":true
                    },
                    false, // AWithEmptyOption
                    "a_currency_code_c",
                    "a_currency_name_c",
                    "#currencycode");        
       
// Method of Payment:
ComboboxInitValues(
                    "serverMFinance.asmx/TFinanceCacheableWebConnector_GetCacheableTable",
                    {
                        'ACacheableTable': 'MethodOfPaymentList',
                        'AHashCode': '',
                      // TODO: only "BankAccountFlag":true
                    },
                    false, // AWithEmptyOption
                    
                    "#methodofpayment"); 
// but: the demodatabase does not return any values!

// Details
// Method of Giving
ComboboxInitValues(
                    "serverMFinance.asmx/TFinanceCacheableWebConnector_GetCacheableTable",
                    {
                        'ACacheableTable': 'MethodOfGivingList',
                        'AHashCode': '',
                      // TODO: only "BankAccountFlag":true
                    },
                    false, // AWithEmptyOption

                    "#methodofgiving");
                    
// Method of Payment
ComboboxInitValues(
                    "serverMFinance.asmx/TFinanceCacheableWebConnector_GetCacheableTable",
                    {
                        'ACacheableTable': 'MethodOfPaymentList',
                        'AHashCode': '',
                      // TODO: only "BankAccountFlag":true
                    },
                    false, // AWithEmptyOption

                    "#detailsmethodofpayment"); 

// Key Ministry 

// Motivation Group
ComboboxInitValues(
                    "serverMFinance.asmx/TFinanceCacheableWebConnector_GetCacheableTable2",
                    {
                        'ACacheableTable': 'MotivationGroupList',
                        'AHashCode': '',
                        'ALedgerNumber': '43'
                      // TODO: only "BankAccountFlag":true
                    },
                    false, // AWithEmptyOption
                    "a_motivation_group_code_c",
                    "a_motivation_group_description_c",
                                       
                    "#motivationgroup");    

// Motivation Detail
ComboboxInitValues(
                    "serverMFinance.asmx/TFinanceCacheableWebConnector_GetCacheableTable2",
                    {
                        'ACacheableTable': 'MotivationList',
                        'AHashCode': '',
                        'ALedgerNumber': '43'
                      // TODO: only "BankAccountFlag":true
                    },
                    false, // AWithEmptyOption
                    "a_motivation_group_code_c",
                    "a_motivation_detail_code_c",
                                                          
                    "#motivationdetail");   

// Cost Centre
ComboboxInitValues(
                    "serverMFinance.asmx/TFinanceCacheableWebConnector_GetCacheableTable2",
                    {
                        'ACacheableTable': 'CostCentreList',
                        'AHashCode': '',
                        'ALedgerNumber': '43'
                      // TODO: only "BankAccountFlag":true
                    },
                    false, // AWithEmptyOption
                    "a_cost_centre_code_c",
                    "a_cost_centre_to_report_to_c",
                    
                    "#detailcostcentre");            

// Account                    
ComboboxInitValues(
                    "serverMFinance.asmx/TFinanceCacheableWebConnector_GetCacheableTable2",
                    {
                        'ACacheableTable': 'AccountList',
                        'AHashCode': '',
                        'ALedgerNumber': '43'
                      // TODO: only "BankAccountFlag":true
                    },
                    false, // AWithEmptyOption
                    "a_account_code_c",
                    "a_account_code_short_desc_c",
                    
                    "#detailsaccount");                                                                               

}
