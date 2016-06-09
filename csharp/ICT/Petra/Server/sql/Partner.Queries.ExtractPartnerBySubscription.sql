SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_p_partner, pub_p_subscription ##address_filter_tables##
WHERE pub_p_subscription.p_publication_code_c IN (?)
    AND pub_p_partner.p_partner_key_n = pub_p_subscription.p_partner_key_n
    AND (NOT ? OR pub_p_subscription.p_gratis_subscription_l)
    AND (   (? AND pub_p_subscription.p_subscription_status_c <> 'EXPIRED'
               AND pub_p_subscription.p_subscription_status_c <> 'CANCELLED'
               AND (pub_p_subscription.p_date_cancelled_d IS NULL
                    OR pub_p_subscription.p_date_cancelled_d >= DATE(NOW())))
         OR (NOT ? AND pub_p_subscription.p_subscription_status_c = ?
                   AND ((pub_p_subscription.p_subscription_status_c = 'EXPIRED'
                         OR pub_p_subscription.p_subscription_status_c = 'CANCELLED')
                        OR (pub_p_subscription.p_date_cancelled_d IS NULL
                            OR pub_p_subscription.p_date_cancelled_d >= DATE(NOW())))))
    AND (NOT ? OR pub_p_partner.p_status_code_c = 'ACTIVE')
    AND (NOT ? OR pub_p_partner.p_partner_class_c LIKE 'PERSON%')
    AND (NOT ? OR pub_p_partner.p_partner_class_c LIKE 'FAMILY%')
    AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l)
    AND (pub_p_subscription.p_publication_copies_i >= ?)
    AND (pub_p_subscription.p_publication_copies_i <= ?)
	AND (NOT ?
	    OR pub_p_subscription.p_start_date_d >= ?)
	AND ((NOT ?
	    AND pub_p_subscription.p_start_date_d <= DATE(NOW()))
	    OR pub_p_subscription.p_start_date_d <= ?)
	AND (NOT ?
	    OR p_subscription.p_expiry_date_d >= ?)
	AND (NOT ?
	    OR p_subscription.p_expiry_date_d <= ?)
##address_filter_where_clause##    
ORDER BY pub_p_partner.p_partner_short_name_c
##address_filter_order_by_clause##