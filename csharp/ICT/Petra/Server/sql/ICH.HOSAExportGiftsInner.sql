-- gi3200-1.i ln48. Export Gift transactions
--Find and total each gift transaction

SELECT
		GiftDetail.a_ledger_number_i,
        GiftDetail.a_batch_number_i,
        GiftDetail.a_gift_transaction_number_i,
        GiftDetail.a_detail_number_i,
        GiftDetail.a_gift_amount_n,
        GiftDetail.a_gift_amount_intl_n,
        GiftDetail.a_motivation_group_code_c,
        GiftDetail.a_motivation_detail_code_c,
        GiftDetail.p_recipient_key_n,
        Gift.a_gift_status_c,
        MotiviationDetail.a_motivation_detail_desc_c,
        GiftBatch.a_batch_description_c
    FROM
        public.a_gift_detail AS GiftDetail,
        public.a_gift_batch AS GiftBatch,
        public.a_motivation_detail AS MotiviationDetail,
        public.a_gift AS Gift
    WHERE
        GiftDetail.a_ledger_number_i = GiftBatch.a_ledger_number_i 
        AND GiftDetail.a_batch_number_i = GiftBatch.a_batch_number_i 
        AND GiftDetail.a_ledger_number_i = MotiviationDetail.a_ledger_number_i 
        AND GiftDetail.a_motivation_group_code_c = MotiviationDetail.a_motivation_group_code_c 
        AND GiftDetail.a_motivation_detail_code_c = MotiviationDetail.a_motivation_detail_code_c 
        AND GiftDetail.a_ledger_number_i = Gift.a_ledger_number_i 
        AND GiftDetail.a_batch_number_i = Gift.a_batch_number_i 
        AND GiftDetail.a_gift_transaction_number_i = Gift.a_gift_transaction_number_i 
        AND GiftDetail.a_ledger_number_i = ? 
        AND GiftDetail.a_cost_centre_code_c = ? 
        AND GiftDetail.a_ich_number_i = ? 
        AND GiftBatch.a_batch_status_c = ? 
        AND GiftBatch.a_gl_effective_date_d >= ? 
        AND GiftBatch.a_gl_effective_date_d <= ? 
        AND MotiviationDetail.a_account_code_c = ?
    ORDER BY
        GiftDetail.p_recipient_key_n ASC,
        GiftDetail.a_motivation_group_code_c ASC,
        GiftDetail.a_motivation_detail_code_c ASC;