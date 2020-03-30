-- this will only work for mysql. postgresql should use text
alter table s_report_result modify s_result_html_c longtext;
alter table p_form modify p_template_document_c longtext;
