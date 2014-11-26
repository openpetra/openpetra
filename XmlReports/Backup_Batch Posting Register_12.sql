DELETE FROM s_report_template WHERE s_template_id_i=12;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(12,'Batch Posting Register','Batch Posting Register template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/24/2014 11:40:27" ReportInfo.Modified="11/25/2014 16:44:22" ReportInfo.CreatorVersion="2014.2.1.0">
  <ScriptText>using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Globalization;
using FastReport;
using FastReport.Data;
using FastReport.Dialog;
using FastReport.Barcode;
using FastReport.Table;
using FastReport.Utils;

namespace FastReport
{
  public class ReportScript
  {
    String OmCurrency(Decimal Amount)
    {
      return Amount.ToString((String)Report.GetParameterValue(&quot;param_currency_formatter&quot;), CultureInfo.InvariantCulture);
    }
    
    String OmDate(DateTime fld)
    {
      return fld.ToString(&quot;dd-MMM-yyyy&quot;);
    }
    
    String BatchDescription()
    {
      return Report.GetColumnValue(&quot;ABatch.a_batch_description_c&quot;).ToString();
    }
    String PostingStatus()
    {
      return Report.GetColumnValue(&quot;ABatch.a_batch_status_c&quot;).ToString();
    }
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="ABatch" ReferenceName="ABatch" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_batch_description_c" DataType="System.String"/>
      <Column Name="a_batch_control_total_n" DataType="System.Decimal"/>
      <Column Name="a_batch_running_total_n" DataType="System.Decimal"/>
      <Column Name="a_batch_debit_total_n" DataType="System.Decimal"/>
      <Column Name="a_batch_credit_total_n" DataType="System.Decimal"/>
      <Column Name="a_batch_period_i" DataType="System.Int32"/>
      <Column Name="a_batch_year_i" DataType="System.Int32"/>
      <Column Name="a_date_effective_d" DataType="System.DateTime"/>
      <Column Name="a_date_of_entry_d" DataType="System.DateTime"/>
      <Column Name="a_batch_status_c" DataType="System.String"/>
      <Column Name="a_last_journal_i" DataType="System.Int32"/>
      <Column Name="a_gift_batch_number_i" DataType="System.Int32"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <TableDataSource Name="AJournal" ReferenceName="AJournal" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_journal_number_i" DataType="System.Int32"/>
      <Column Name="a_journal_description_c" DataType="System.String"/>
      <Column Name="a_journal_debit_total_n" DataType="System.Decimal"/>
      <Column Name="a_journal_credit_total_n" DataType="System.Decimal"/>
      <Column Name="a_journal_period_i" DataType="System.Int32"/>
      <Column Name="a_date_effective_d" DataType="System.DateTime"/>
      <Column Name="a_transaction_type_code_c" DataType="System.String"/>
      <Column Name="a_last_transaction_number_i" DataType="System.Int32"/>
      <Column Name="a_sub_system_code_c" DataType="System.String"/>
      <Column Name="a_journal_status_c" DataType="System.String"/>
      <Column Name="a_transaction_currency_c" DataType="System.String"/>
      <Column Name="a_base_currency_c" DataType="System.String"/>
      <Column Name="a_exchange_rate_to_base_n" DataType="System.Decimal"/>
      <Column Name="a_exchange_rate_time_i" DataType="System.Int32"/>
      <Column Name="a_date_of_entry_d" DataType="System.DateTime"/>
      <Column Name="a_reversed_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
      <Column Name="JournalDebitTotalBase" DataType="System.Decimal"/>
      <Column Name="JournalCreditTotalBase" DataType="System.Decimal"/>
    </TableDataSource>
    <TableDataSource Name="ATransaction" ReferenceName="ATransaction" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_journal_number_i" DataType="System.Int32"/>
      <Column Name="a_transaction_number_i" DataType="System.Int32"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_primary_account_code_c" DataType="System.String"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_primary_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_transaction_date_d" DataType="System.DateTime"/>
      <Column Name="a_transaction_amount_n" DataType="System.Decimal"/>
      <Column Name="a_amount_in_base_currency_n" DataType="System.Decimal"/>
      <Column Name="a_analysis_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_reconciled_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_narrative_c" DataType="System.String"/>
      <Column Name="a_debit_credit_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_transaction_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_header_number_i" DataType="System.Int32"/>
      <Column Name="a_detail_number_i" DataType="System.Int32"/>
      <Column Name="a_sub_type_c" DataType="System.String"/>
      <Column Name="a_to_ilt_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_source_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_reference_c" DataType="System.String"/>
      <Column Name="a_source_reference_c" DataType="System.String"/>
      <Column Name="a_system_generated_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_amount_in_intl_currency_n" DataType="System.Decimal"/>
      <Column Name="a_ich_number_i" DataType="System.Int32"/>
      <Column Name="a_key_ministry_key_n" DataType="System.Int64"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
      <Column Name="DateEntered" DataType="System.DateTime"/>
      <Column Name="AnalysisAttributes" DataType="System.String"/>
      <Column Name="Percentage" DataType="System.Decimal"/>
    </TableDataSource>
    <Relation Name="ATransaction_AJournal" ParentDataSource="ATransaction" ChildDataSource="AJournal" ParentColumns="a_batch_number_i&#13;&#10;a_journal_number_i" ChildColumns="a_batch_number_i&#13;&#10;a_journal_number_i" Enabled="true"/>
    <Relation Name="ABatch_ATransaction" ParentDataSource="ABatch" ChildDataSource="ATransaction" ParentColumns="a_batch_number_i" ChildColumns="a_batch_number_i" Enabled="true"/>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
    <Parameter Name="param_requested_by" DataType="System.String"/>
    <Parameter Name="param_version" DataType="System.String"/>
    <Parameter Name="param_batch_number_i" DataType="System.Int32"/>
    <Total Name="Credits" Expression="[ATransaction.a_transaction_amount_n]" Evaluator="list" PrintOn="GroupFooter2" EvaluateCondition="[ATransaction.a_debit_credit_indicator_l]==false"/>
    <Total Name="Debits" Expression="[ATransaction.a_transaction_amount_n]" Evaluator="list" PrintOn="GroupFooter2" EvaluateCondition="[ATransaction.a_debit_credit_indicator_l]==true"/>
    <Total Name="BatchDebits" Expression="[ATransaction.a_transaction_amount_n]" Evaluator="list" PrintOn="ReportSummary1" EvaluateCondition="[ATransaction.a_debit_credit_indicator_l]==true"/>
    <Total Name="BatchCredits" Expression="[ATransaction.a_transaction_amount_n]" Evaluator="list" PrintOn="ReportSummary1" EvaluateCondition="[ATransaction.a_debit_credit_indicator_l]==false"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="85.05">
      <TextObject Name="Text1" Left="207.9" Width="255.15" Height="28.35" Text="GL Posting Register" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="491.4" Top="18.9" Width="94.5" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="585.9" Top="18.9" Width="132.3" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Top="18.9" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Top="18.9" Width="132.3" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text42" Left="207.9" Top="28.35" Width="255.15" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <TextObject Name="Text22" Left="585.9" Top="37.8" Width="94.5" Height="18.9" Text="[PostingStatus()]" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text53" Top="37.8" Width="75.6" Height="18.9" Text="Batch :" HorzAlign="Right"/>
      <TextObject Name="Text54" Left="75.6" Top="37.8" Width="132.3" Height="18.9" Text="[param_batch_number_i]"/>
      <TextObject Name="Text12" Left="75.6" Top="56.7" Width="207.9" Height="18.9" CanGrow="true" Text="[BatchDescription()]" Font="Arial, 12pt, style=Bold"/>
      <TextObject Name="Text55" Width="75.6" Height="18.9" Text="User :" HorzAlign="Right"/>
      <TextObject Name="Text13" Left="75.6" Width="132.3" Height="18.9" Text="[param_requested_by]"/>
      <TextObject Name="Text56" Left="491.4" Width="94.5" Height="18.9" Text="Version :" HorzAlign="Right"/>
      <TextObject Name="Text24" Left="585.9" Width="132.3" Height="18.9" Text="[param_version]"/>
    </ReportTitleBand>
    <GroupHeaderBand Name="GroupHeader2" Top="88.38" Width="718.2" Height="47.25" Condition="[ATransaction.a_journal_number_i]">
      <TextObject Name="Text14" Width="283.5" Height="18.9" Text="Journal [ATransaction.a_journal_number_i] : [AJournal.a_journal_description_c]" Font="Arial, 9pt"/>
      <TextObject Name="Text46" Left="37.8" Top="28.35" Width="94.5" Height="18.9" Text="CC-Account" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text7" Top="28.35" Width="37.8" Height="18.9" Text="Trans" Font="Arial, 7pt, style=Bold"/>
      <TextObject Name="Text43" Left="132.3" Top="28.35" Width="179.55" Height="18.9" Text="Description" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text47" Left="311.85" Top="28.35" Width="94.5" Height="18.9" Text="Date" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text48" Left="406.35" Top="28.35" Width="94.5" Height="18.9" Text="Reference" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text49" Left="500.85" Top="28.35" Width="85.05" Height="18.9" Text="Debit" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text50" Left="585.9" Top="28.35" Width="85.05" Height="18.9" Text="Credit" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <LineObject Name="Line1" Left="-9.45" Top="18.9" Width="680.4"/>
      <TextObject Name="Text23" Left="283.5" Width="47.25" Height="18.9" Text="[AJournal.a_transaction_currency_c]"/>
      <DataBand Name="list" Top="138.97" Width="718.2" Height="18.9" CanGrow="true" KeepChild="true" DataSource="ATransaction" Filter="[ATransaction.a_batch_number_i]==[param_batch_number_i]" PrintIfDetailEmpty="true" PrintIfDatasourceEmpty="true" KeepDetail="true">
        <TextObject Name="Text2" Width="37.8" Height="18.9" Text="[ATransaction.a_transaction_number_i]" Font="Arial, 9pt"/>
        <TextObject Name="Text3" Left="37.8" Width="94.5" Height="18.9" CanGrow="true" Text="[ATransaction.a_cost_centre_code_c] - [ATransaction.a_account_code_c]" AutoShrink="FontSize" AutoShrinkMinSize="7" Font="Arial, 9pt"/>
        <TextObject Name="Text4" Left="132.3" Width="179.55" Height="18.9" CanGrow="true" GrowToBottom="true" Text="[ATransaction.a_narrative_c]" AutoShrink="FontSize" AutoShrinkMinSize="7" Font="Arial, 9pt"/>
        <TextObject Name="Text5" Left="311.85" Width="94.5" Height="18.9" Text="[OmDate([ATransaction.a_transaction_date_d])]" Format="Date" Format.Format="d" Font="Arial, 9pt"/>
        <TextObject Name="Text6" Left="406.35" Width="94.5" Height="18.9" Text="[ATransaction.a_reference_c]" AutoShrink="FontSize" AutoShrinkMinSize="7" Font="Arial, 9pt"/>
        <TextObject Name="Text11" Left="500.85" Width="85.05" Height="18.9" Text="[ATransaction.a_transaction_amount_n]" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
          <Highlight>
            <Condition Expression="[ATransaction.a_debit_credit_indicator_l]==false" Visible="false" ApplyTextFill="false"/>
          </Highlight>
        </TextObject>
        <TextObject Name="Text51" Left="585.9" Width="85.05" Height="18.9" Text="[ATransaction.a_transaction_amount_n]" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
          <Highlight>
            <Condition Expression="[ATransaction.a_debit_credit_indicator_l]==true" Visible="false" ApplyTextFill="false"/>
          </Highlight>
        </TextObject>
        <Sort>
          <Sort Expression="[ATransaction.a_batch_number_i]"/>
          <Sort Expression="[ATransaction.a_journal_number_i]"/>
          <Sort Expression="[ATransaction.a_transaction_number_i]"/>
        </Sort>
      </DataBand>
      <GroupFooterBand Name="GroupFooter2" Top="161.2" Width="718.2" Height="37.8">
        <TextObject Name="Text15" Left="500.85" Width="85.05" Height="18.9" Text="[Debits]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
        <TextObject Name="Text16" Left="585.9" Width="85.05" Height="18.9" Text="[Credits]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
        <TextObject Name="Text18" Left="283.5" Width="122.85" Height="18.9" Text="Journal Totals :" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      </GroupFooterBand>
    </GroupHeaderBand>
    <ReportSummaryBand Name="ReportSummary1" Top="202.33" Width="718.2" Height="18.9">
      <TextObject Name="Text52" Left="283.5" Width="122.85" Height="18.9" Text="Batch Totals :" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text19" Left="500.85" Width="85.05" Height="18.9" Text="[BatchDebits]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text21" Left="585.9" Width="85.05" Height="18.9" Text="[BatchCredits]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
    </ReportSummaryBand>
    <PageFooterBand Name="PageFooter1" Top="224.57" Width="718.2" Height="18.9">
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;