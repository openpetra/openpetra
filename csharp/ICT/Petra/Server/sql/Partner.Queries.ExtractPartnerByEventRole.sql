SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
FROM pub_p_partner, pub_pm_short_term_application, pm_general_application
WHERE pub_pm_short_term_application.pm_st_confirmed_option_n IN (?)
    AND pub_pm_short_term_application.pm_st_congress_code_c IN (?)
    AND NOT pub_pm_short_term_application.pm_st_basic_delete_flag_l
    AND pub_pm_general_application.p_partner_key_n  = pub_pm_short_term_application.p_partner_key_n
    AND pub_pm_general_application.pm_application_key_i = pub_pm_short_term_application.pm_application_key_i
    AND pub_pm_general_application.pm_registration_office_n = pub_pm_short_term_application.pm_registration_office_n
    AND NOT pub_pm_general_application.pm_gen_app_delete_flag_l
    AND pub_p_partner.p_partner_key_n = pub_pm_short_term_application.p_partner_key_n
    AND (   (? AND pub_pm_general_application.pm_gen_application_status_c LIKE 'A%')
        OR (? AND pub_pm_general_application.pm_gen_application_status_c LIKE 'H%')
        OR (? AND pub_pm_general_application.pm_gen_application_status_c LIKE 'E%')
        OR (? AND pub_pm_general_application.pm_gen_application_status_c LIKE 'C%')
        OR (? AND pub_pm_general_application.pm_gen_application_status_c LIKE 'R%'))
ORDER BY pub_p_partner.p_partner_short_name_c
