SELECT a_gift.a_date_entered_d AS DateEntered,
 a_gift_detail.a_motivation_group_code_c AS MotivationGroupCode, 
 a_gift_detail.a_motivation_detail_code_c AS MotivationDetailCode,
 a_gift.a_receipt_number_i AS ReceiptNumber,
 a_gift_detail.a_gift_amount_n AS GiftAmount, 
 a_gift_detail.a_gift_amount_intl_n AS GiftAmountIntl, 
 a_gift_detail.a_confidential_gift_flag_l AS ConfidentialGiftFlag, 
 a_gift_batch.a_batch_number_i AS BatchNumber, 
 a_gift.a_gift_transaction_number_i AS GiftTransactionNumber, 
 p_partner.p_partner_short_name_c AS RecipientDescription, 
 a_gift.a_reference_c AS Reference, 
 a_gift_detail.a_gift_comment_one_c AS GiftCommentOne, 
 a_gift_detail.a_comment_one_type_c AS CommentOneType, 
 a_gift_detail.a_recipient_ledger_number_n AS RecipientLedgerNumber, 
 a_gift_detail.p_recipient_key_n AS RecipientKey, 
 a_gift_detail.a_charge_flag_l AS ChargeFlag, 
 a_gift.a_method_of_payment_code_c AS MethodOfPaymentCode, 
 a_gift.a_method_of_giving_code_c AS MethodOfGivingCode,
 a_gift.p_donor_key_n AS DonorKey,
 a_gift_detail.a_cost_centre_code_c AS CostCentreCode, 
 a_gift_detail.a_gift_comment_two_c AS GiftCommentTwo, 
 a_gift_detail.a_gift_comment_three_c AS GiftCommentThree, 
 a_gift_detail.p_mailing_code_c AS MailingCode
FROM a_gift_batch, a_gift, a_gift_detail, p_partner 
WHERE a_gift.a_ledger_number_i = a_gift_batch.a_ledger_number_i
  AND a_gift.a_batch_number_i = a_gift_batch.a_batch_number_i
  AND a_gift_detail.a_ledger_number_i = a_gift_batch.a_ledger_number_i
  AND a_gift_detail.a_batch_number_i = a_gift_batch.a_batch_number_i
  AND a_gift_detail.a_gift_transaction_number_i = a_gift.a_gift_transaction_number_i 
  AND p_partner.p_partner_key_n = a_gift_detail.p_recipient_key_n 
  AND a_gift_batch.a_batch_status_c = 'Posted' 
  AND a_gift_batch.a_ledger_number_i = ? 
  AND (? OR a_gift.p_donor_key_n = ?)
  AND (? OR a_gift_detail.p_recipient_key_n = ?)
  AND (? OR a_gift.a_date_entered_d BETWEEN CAST(? || ' 00:00:00' AS TIMESTAMP) AND CAST(? || ' 23:59:59' AS TIMESTAMP))
  AND (? OR a_gift_detail.a_motivation_group_code_c LIKE (? || '%'))
  AND (? OR a_gift_detail.a_motivation_detail_code_c LIKE (? || '%'))
ORDER BY a_gift.a_date_entered_d DESC
