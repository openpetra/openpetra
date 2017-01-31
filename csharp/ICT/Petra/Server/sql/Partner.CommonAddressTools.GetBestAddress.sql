WITH address_state AS (
	SELECT
		p_partner_key_n
		, p_site_key_n
		, p_location_key_i
		, p_date_effective_d
		, p_date_good_until_d
		, p_location_type_c
		, p_send_mail_l
		-- State order is:
		--   Current: 1
		--   Future: 2
		--   Expired:  3
		, CASE
			WHEN p_date_effective_d <= current_date AND (p_date_good_until_d >= current_date OR p_date_good_until_d is null) THEN 1  -- Current
			WHEN p_date_effective_d > current_date THEN 2                                                                            -- Future
			ELSE 3                                                                                                                   -- Expired
		END AS date_state
	FROM
	  p_partner_location
	WHERE
	  p_partner_key_n IN ({DonorList})
)
, best_state AS (
	SELECT
		*
		--
		-- Best selection is:
		--   Current:  maximum p_date_effective_d   (most recent current address)
		--   Future:   minimum p_date_effective_d   (soonest-starting future address)
		--   Expired:  maximum p_date_good_until_d  (most recently active address)
		, row_number() OVER (PARTITION BY p_partner_key_n ORDER BY p_send_mail_l DESC, date_state
			, CASE WHEN date_state = 1 THEN p_date_effective_d END DESC
			, CASE WHEN date_state = 2 THEN p_date_effective_d END
			, CASE WHEN date_state = 3 THEN p_date_good_until_d END DESC
		)
	FROM
		address_state
)
SELECT
	best_state.p_partner_key_n
	, best_state.p_date_effective_d
	, best_state.p_date_good_until_d
	, best_state.p_location_type_c
	, best_state.p_send_mail_l
	, p_location.*
FROM
	best_state INNER JOIN p_location USING (p_site_key_n, p_location_key_i)
WHERE
	row_number = 1


