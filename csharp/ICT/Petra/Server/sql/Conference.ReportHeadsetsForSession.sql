SELECT DISTINCT
       PUB_p_partner_contact.p_partner_key_n AS PartnerKey, 
       PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c AS SessionName,
       PUB_p_partner_contact.p_contact_code_c AS RentedOutOrReturned,
       PUB_p_partner.p_partner_short_name_c AS ShortName,
       PUB_pm_short_term_application.pm_st_fg_code_c AS FellowshipGroup,
       PUB_pm_short_term_application.pm_st_congress_code_c AS Role,
       PUB_p_unit.p_unit_name_c AS Country
FROM PUB_p_partner_contact, PUB_p_partner_contact_attribute, PUB_p_partner, PUB_pm_short_term_application, PUB_p_unit
WHERE (PUB_p_partner_contact.p_contact_code_c = 'HEADSET_OUT' OR PUB_p_partner_contact.p_contact_code_c = 'HEADSET_RETURN')
AND PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i
AND PUB_p_partner_contact_attribute.p_contact_attribute_code_c = 'SESSION'
AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = ?
AND PUB_p_partner.p_partner_key_n = PUB_p_partner_contact.p_partner_key_n
AND PUB_pm_short_term_application.p_partner_key_n = PUB_p_partner_contact.p_partner_key_n
AND PUB_pm_short_term_application.pm_confirmed_option_code_c = ?
AND PUB_p_unit.p_partner_key_n = PUB_pm_short_term_application.pm_registration_office_n
