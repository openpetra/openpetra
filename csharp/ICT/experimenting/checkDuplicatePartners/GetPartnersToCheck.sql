select p.* 
from pub.pm_short_term_application as s, pub.pm_general_application as g, pub.p_partner as p 
where g.p_partner_key_n = s.p_partner_key_n 
and g.pm_application_key_i = s.pm_application_key_i 
and g.pm_registration_office_n = s.pm_registration_office_n 
and pm_confirmed_option_code_c = 'TS101CNGRS.07' 
AND p.p_partner_key_n = g.p_partner_key_n 
order by p.p_partner_short_name_c

