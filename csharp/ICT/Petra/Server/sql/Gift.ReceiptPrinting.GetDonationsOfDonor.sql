SELECT a_gift_batch.a_gl_effective_date_d AS DateEffective, 
        a_gift_detail.a_gift_transaction_amount_n AS Amount, 
        a_gift_detail.a_gift_comment_one_c AS CommentOne,
        a_account.a_account_code_short_desc_c AS AccountDesc,
        a_cost_centre.a_cost_centre_name_c AS CostCentreDesc
FROM a_gift_batch, a_gift, a_gift_detail, a_motivation_detail, a_account, a_cost_centre
WHERE a_gift_batch.a_ledger_number_i = ?
   AND a_gift_batch.a_batch_status_c = "POSTED"
   AND a_gift.a_ledger_number_i = a_gift_batch.a_ledger_number_i
   AND a_gift.a_batch_number_i = a_gift_batch.a_batch_number_i
   AND a_gift_detail.a_ledger_number_i = a_gift_batch.a_ledger_number_i
   AND a_gift_detail.a_batch_number_i = a_gift_batch.a_batch_number_i
   AND a_gift_detail.a_gift_transaction_number_i = a_gift.a_gift_transaction_number_i
   AND a_gift_batch.a_gl_effective_date_d BETWEEN ? AND ?
   AND a_gift.p_donor_key_n = ?
   AND a_motivation_detail.a_ledger_number_i = a_gift_batch.a_ledger_number_i
   AND a_motivation_detail.a_motivation_group_code_c = a_gift_detail.a_motivation_group_code_c
   AND a_motivation_detail.a_motivation_detail_code_c = a_gift_detail.a_motivation_detail_code_c
   AND a_motivation_detail.a_cost_centre_code_c = a_cost_centre.a_cost_centre_code_c
   AND a_motivation_detail.a_account_code_c = a_account.a_account_code_c
   AND a_motivation_detail.a_tax_deductable_l = 1
   AND a_motivation_detail.a_receipt_l = 1
ORDER BY a_gift_batch.a_gl_effective_date_d ASC