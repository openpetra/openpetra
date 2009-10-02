SELECT DISTINCT PUB_a_ap_supplier.a_currency_code_c AS CurrencyCode, PUB_a_ap_supplier.p_partner_key_n AS SupplierKey 
FROM PUB_a_ap_supplier, PUB_a_ap_document, PUB_a_ap_document_payment
WHERE PUB_a_ap_document.p_partner_key_n = PUB_a_ap_supplier.p_partner_key_n
    AND PUB_a_ap_document.a_ledger_number_i = PUB_a_ap_document_payment.a_ledger_number_i
    AND PUB_a_ap_document.a_ap_number_i = PUB_a_ap_document_payment.a_ap_number_i
    AND PUB_a_ap_document_payment.a_ledger_number_i = ?
    AND PUB_a_ap_document_payment.a_payment_number_i = ?