select p.* 
from pub.p_partner as p 
where p.p_partner_key_n <> ?
AND p.p_partner_class_c = 'PERSON'
and p.p_partner_short_name_c = ?
and p.p_status_code_c <> 'MERGED'

