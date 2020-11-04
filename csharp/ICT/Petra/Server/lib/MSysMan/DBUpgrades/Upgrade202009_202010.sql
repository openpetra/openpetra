insert into p_type_category(p_code_c, p_description_c, p_deletable_flag_l) values('SPONSORED_CHILD_STATUS', 'Status Options for sponsored child', 0);
update p_type set p_category_code_c = 'SPONSORED_CHILD_STATUS' WHERE p_type_code_c IN ('BOARDING_SCHOOL', 'CHILDREN_HOME', 'CHILD_DIED', 'HOME_BASED', 'PREVIOUS_CHILD');

