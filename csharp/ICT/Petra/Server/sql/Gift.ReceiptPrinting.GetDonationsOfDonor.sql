SELECT a_gift.a_date_entered_d AS DateEntered,
        a_gift.a_first_time_gift_l AS FirstDonation, 
        a_gift_detail.a_gift_transaction_amount_n AS TransactionAmount,
        a_gift_detail.a_gift_amount_n AS AmountInBaseCurrency,
        a_gift_detail.a_tax_deductible_amount_n AS TaxDeductibleAmount,
        a_gift_detail.a_tax_deductible_amount_base_n AS TaxDeductibleAmountBase,
        a_gift_detail.a_non_deductible_amount_n AS NonDeductibleAmount,
        a_gift_detail.a_non_deductible_amount_base_n AS NonDeductibleAmountBase,
        a_gift_batch.a_currency_code_c AS Currency,
        a_gift_batch.a_gift_type_c AS GiftType,
        a_gift_detail.a_gift_comment_one_c AS CommentOne,
        a_gift_detail.a_comment_one_type_c AS CommentOneType,
        a_gift_detail.a_gift_comment_two_c AS CommentTwo,
        a_gift_detail.a_comment_two_type_c AS CommentTwoType,
        a_gift_detail.a_gift_comment_three_c AS CommentThree,
        a_gift_detail.a_comment_three_type_c AS CommentThreeType,
        a_gift_detail.a_modified_detail_l AS Adjustment,
        a_gift_detail.p_mailing_code_c AS MailingCode,
        a_account.a_account_code_short_desc_c AS AccountDesc,
        a_cost_centre.a_cost_centre_name_c AS CostCentreDesc,
        GiftDestination.p_partner_short_name_c AS FieldName,
        Recipient.p_partner_short_name_c AS RecipientName,
        Recipient.p_partner_key_n AS RecipientKey,
        a_motivation_detail.a_motivation_detail_desc_c AS MotivationDetailDesc
FROM a_gift_batch, a_gift, a_gift_detail, a_motivation_detail, a_account, a_cost_centre, p_partner AS GiftDestination, p_partner AS Recipient
WHERE a_gift_batch.a_ledger_number_i = ?
   AND a_gift_batch.a_batch_status_c = 'Posted'
   AND a_gift.a_ledger_number_i = a_gift_batch.a_ledger_number_i
   AND a_gift.a_batch_number_i = a_gift_batch.a_batch_number_i
   AND a_gift_detail.a_ledger_number_i = a_gift_batch.a_ledger_number_i
   AND a_gift_detail.a_batch_number_i = a_gift_batch.a_batch_number_i
   AND a_gift_detail.a_gift_transaction_number_i = a_gift.a_gift_transaction_number_i
   AND a_gift_batch.a_gl_effective_date_d BETWEEN ? AND ?
   AND a_gift.p_donor_key_n = ?
   AND a_gift.a_print_receipt_l = 1
   AND a_motivation_detail.a_ledger_number_i = a_gift_batch.a_ledger_number_i
   AND a_motivation_detail.a_motivation_group_code_c = a_gift_detail.a_motivation_group_code_c
   AND a_motivation_detail.a_motivation_detail_code_c = a_gift_detail.a_motivation_detail_code_c
   AND a_motivation_detail.a_receipt_l = 1
   AND a_cost_centre.a_ledger_number_i = a_gift_batch.a_ledger_number_i
   AND a_cost_centre.a_cost_centre_code_c = a_motivation_detail.a_cost_centre_code_c 
   AND a_account.a_ledger_number_i = a_gift_batch.a_ledger_number_i
   AND a_account.a_account_code_c = a_motivation_detail.a_account_code_c
   AND GiftDestination.p_partner_key_n = a_gift_detail.a_recipient_ledger_number_n
   AND Recipient.p_partner_key_n = a_gift_detail.p_recipient_key_n
ORDER BY a_gift.a_date_entered_d ASC