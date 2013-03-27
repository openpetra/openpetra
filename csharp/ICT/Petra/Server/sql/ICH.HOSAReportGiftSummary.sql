-- used for HOSA reprint, Ict.Petra.Server.MFinance.queries.QueryHOSAReport

SELECT
        SUM(GiftDetail.a_gift_amount_n) AS GiftBaseAmount,
        SUM(a_gift_transaction_amount_n) AS GiftTransactionAmount,
        GiftDetail.p_recipient_key_n AS RecipientKey,
        Partner.p_partner_short_name_c AS RecipientShortname
    FROM
        PUB_a_gift_detail AS GiftDetail,
        PUB_a_gift_batch AS GiftBatch,
        PUB_a_motivation_detail AS MotivationDetail,
        PUB_a_gift AS Gift,
        PUB_p_partner AS Partner
{#IFDEF PERSONALHOSA}
        ,PUB_a_valid_ledger_number AS LinkedCostCentre, 
        PUB_p_person AS Person
{#ENDIF PERSONALHOSA}
    WHERE
        GiftDetail.a_ledger_number_i = GiftBatch.a_ledger_number_i 
        AND GiftDetail.a_batch_number_i = GiftBatch.a_batch_number_i 
        AND GiftDetail.a_ledger_number_i = MotivationDetail.a_ledger_number_i 
        AND GiftDetail.a_motivation_group_code_c = MotivationDetail.a_motivation_group_code_c 
        AND GiftDetail.a_motivation_detail_code_c = MotivationDetail.a_motivation_detail_code_c 
        AND GiftDetail.a_ledger_number_i = Gift.a_ledger_number_i 
        AND GiftDetail.a_batch_number_i = Gift.a_batch_number_i 
        AND GiftDetail.a_gift_transaction_number_i = Gift.a_gift_transaction_number_i 
        AND GiftDetail.a_ledger_number_i = ? 
{#IFNDEF PERSONALHOSA}
        AND GiftDetail.a_cost_centre_code_c = ?
{#ENDIFN PERSONALHOSA}
{#IFDEF PERSONALHOSA}
        AND LinkedCostCentre.a_ledger_number_i = GiftDetail.a_ledger_number_i
        AND LinkedCostCentre.a_cost_centre_code_c = ?
        AND Person.p_partner_key_n = LinkedCostCentre.p_partner_key_n
        AND GiftDetail.p_recipient_key_n = Person.p_family_key_n
{#ENDIF PERSONALHOSA}
{#IFNDEF NOT_LIMITED_TO_ICHNUMBER}
        AND GiftDetail.a_ich_number_i = ?
{#ENDIFN NOT_LIMITED_TO_ICHNUMBER}
        AND GiftBatch.a_batch_status_c = ?
{#IFNDEF BYPERIOD}        
        AND GiftBatch.a_gl_effective_date_d >= ? 
        AND GiftBatch.a_gl_effective_date_d <= ?
{#ENDIFN BYPERIOD}        
{#IFDEF BYPERIOD}        
        AND GiftBatch.a_batch_year_i = ?
        AND GiftBatch.a_batch_period_i >= ?
        AND GiftBatch.a_batch_period_i <= ?
{#ENDIF BYPERIOD}        
        AND MotivationDetail.a_account_code_c = ?
        AND Partner.p_partner_key_n = GiftDetail.p_recipient_key_n
    GROUP BY GiftDetail.p_recipient_key_n, Partner.p_partner_short_name_c
    ORDER BY
        Partner.p_partner_short_name_c ASC
-- TODO do we need to group by currency as well?
    