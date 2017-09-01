function Init() {

// Partner Status
ComboboxInitValues(
                    "serverMPartner.asmx/TPartnerCacheableWebConnector_GetCacheableTable",
                    {
                    'ACacheableTable': 'PartnerStatusList',
                    'AHashCode': '',
                    },
                    true, // AWithEmptyOption
                    "p_status_code_c",
                    "p_status_code_c",
                    "#partnerstatus");

// Country
ComboboxInitValues(
                    "serverMCommon.asmx/TCommonCacheableWebConnector_GetCacheableTable",
                    {
                    'ACacheableTable': 'CountryList',
                    'AHashCode': '',
                    },
                    true, // AWithEmptyOption
                    "p_country_code_c",
                    "p_country_name_c",
                    "#country");

// Location Type
ComboboxInitValues(
                    "serverMPartner.asmx/TPartnerCacheableWebConnector_GetCacheableTable",
                    {
                    'ACacheableTable': 'LocationTypeList',
                    'AHashCode': '',
                    },
                    true, // AWithEmptyOption
                    "p_code_c",
                    "p_code_c",
                    "#locationtype");
                    
// Publication Code
ComboboxInitValues(
                    "serverMPartner.asmx/TSubscriptionsCacheableWebConnector_GetCacheableTable",
                    {
                    'ACacheableTable': 'PublicationList',
                    'AHashCode': '',
                    },
                    true, // AWithEmptyOption
                    "p_publication_code_c",
                    "p_publication_code_c",
                    "#publicationcode");  

// Reason Given
ComboboxInitValues(
                    "serverMPartner.asmx/TSubscriptionsCacheableWebConnector_GetCacheableTable",
                    {
                    'ACacheableTable': 'ReasonSubscriptionGivenList',
                    'AHashCode': '',
                    },
                    true, // AWithEmptyOption
                    "p_code_c",
                    "p_code_c",
                    "#reasongiven"); 
                    
// Reason Ended
ComboboxInitValues(
                    "serverMPartner.asmx/TSubscriptionsCacheableWebConnector_GetCacheableTable",
                    {
                    'ACacheableTable': 'ReasonSubscriptionCancelledList',
                    'AHashCode': '',
                    },
                    true, // AWithEmptyOption
                    "p_code_c",
                    "p_code_c",
                    "#reasonended");                    
  
}
