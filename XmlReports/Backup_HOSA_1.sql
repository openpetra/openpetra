DELETE FROM s_report_template WHERE s_template_id_i=1;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(1,'HOSA','OpenPetra default template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="11/04/2014 09:52:22" ReportInfo.CreatorVersion="2014.2.1.0">
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
    String OmDate(DateTime fld)
    {
      return fld.ToString(&quot;dd-MMM-yyyy&quot;);
    }
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="a_costCentre" ReferenceName="a_costCentre" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_cost_centre_to_report_to_c" DataType="System.String"/>
      <Column Name="a_cost_centre_name_c" DataType="System.String"/>
      <Column Name="a_posting_cost_centre_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_cost_centre_active_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_project_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_project_constraint_date_d" DataType="System.DateTime"/>
      <Column Name="a_project_constraint_amount_n" DataType="System.Decimal"/>
      <Column Name="a_system_cost_centre_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_cost_centre_type_c" DataType="System.String"/>
      <Column Name="a_key_focus_area_c" DataType="System.String"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <TableDataSource Name="Gifts" ReferenceName="Gifts" DataType="System.Int32" Enabled="true">
      <Column Name="costcentre" DataType="System.String"/>
      <Column Name="accountcode" DataType="System.String"/>
      <Column Name="giftbaseamount" DataType="System.Decimal"/>
      <Column Name="gifttransactionamount" DataType="System.Decimal"/>
      <Column Name="recipientkey" DataType="System.Int64"/>
      <Column Name="recipientshortname" DataType="System.String"/>
      <Column Name="narrative" DataType="System.String"/>
      <Column Name="GiftIntlAmount" DataType="System.Decimal"/>
      <Column Name="Reference" DataType="System.String"/>
    </TableDataSource>
    <TableDataSource Name="a_account" ReferenceName="a_account" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_account_type_c" DataType="System.String"/>
      <Column Name="a_account_code_long_desc_c" DataType="System.String"/>
      <Column Name="a_account_code_short_desc_c" DataType="System.String"/>
      <Column Name="a_eng_account_code_short_desc_c" DataType="System.String"/>
      <Column Name="a_eng_account_code_long_desc_c" DataType="System.String"/>
      <Column Name="a_debit_credit_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_account_active_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_analysis_attribute_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_standard_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_consolidation_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_intercompany_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_budget_type_code_c" DataType="System.String"/>
      <Column Name="a_posting_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_system_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_budget_control_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_valid_cc_combo_c" DataType="System.String"/>
      <Column Name="a_foreign_currency_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_foreign_currency_code_c" DataType="System.String"/>
      <Column Name="p_banking_details_key_i" DataType="System.Int32"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <TableDataSource Name="a_transaction" ReferenceName="a_transaction" DataType="System.Int32" Enabled="true">
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
      <Column Name="a_analysis_type_code_c" DataType="System.String"/>
      <Column Name="a_analysis_type_description_c" DataType="System.String"/>
      <Column Name="a_analysis_attribute_value_c" DataType="System.String"/>
    </TableDataSource>
    <Relation Name="a_account_Gifts" ParentDataSource="a_account" ChildDataSource="Gifts" ParentColumns="a_account_code_c" ChildColumns="accountcode" Enabled="true"/>
    <Relation Name="a_account_a_transaction" ParentDataSource="a_account" ChildDataSource="a_transaction" ParentColumns="a_account_code_c" ChildColumns="a_account_code_c" Enabled="true"/>
    <Parameter Name="param_diff_period_i" DataType="System.Int32"/>
    <Parameter Name="param_start_period_i" DataType="System.Int32"/>
    <Parameter Name="param_end_period_i" DataType="System.Int32"/>
    <Parameter Name="param_account_hierarchy_c" DataType="System.String"/>
    <Parameter Name="param_currency" DataType="System.String"/>
    <Parameter Name="param_period" DataType="System.Boolean"/>
    <Parameter Name="param_date_checked" DataType="System.Boolean"/>
    <Parameter Name="param_year_i" DataType="System.Int32"/>
    <Parameter Name="param_start_date" DataType="System.DateTime"/>
    <Parameter Name="param_end_date" DataType="System.DateTime"/>
    <Parameter Name="param_filter_cost_centres" DataType="System.String"/>
    <Parameter Name="param_ExcludeInactiveCostCentres" DataType="System.Boolean"/>
    <Parameter Name="param_cost_centre_codes" DataType="System.String"/>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_daterange" DataType="System.String"/>
    <Parameter Name="param_rgrAccounts" DataType="System.String"/>
    <Parameter Name="param_rgrCostCentres" DataType="System.String"/>
    <Parameter Name="param_ich_number" DataType="System.Int32"/>
    <Parameter Name="param_quarter" DataType="System.Boolean"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_currency_formatter" DataType="System.String"/>
    <Parameter Name="param_date_title" DataType="System.String"/>
    <Parameter Name="param_currency_name" DataType="System.String"/>
    <Parameter Name="param_period_breakdown" DataType="System.Boolean"/>
    <Parameter Name="param_real_year" DataType="System.Int32"/>
    <Parameter Name="param_period_checked" DataType="System.Boolean"/>
    <Parameter Name="param_quarter_checked" DataType="System.Boolean"/>
    <Parameter Name="param_real_year_ending" DataType="System.String"/>
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
    <Parameter Name="param_current_financial_year" DataType="System.Boolean"/>
    <Parameter Name="param_requested_by" DataType="System.String"/>
    <Parameter Name="param_version" DataType="System.String"/>
    <Parameter Name="param_current_period" DataType="System.Int32"/>
    <Parameter Name="param_run_number" DataType="System.Int32"/>
    <Total Name="GiftDebitsTotal" Expression="GiftDebits.Value" Evaluator="Data1" PrintOn="GiftsFooter" ResetOnReprint="true"/>
    <Total Name="GiftCreditsTotal" Expression="GiftCredits.Value" Evaluator="Data1" PrintOn="GiftsFooter" ResetOnReprint="true"/>
    <Total Name="TransDebitsTotal" Expression="TransDebits.Value" Evaluator="Data3" PrintOn="TransFooter" ResetOnReprint="true"/>
    <Total Name="TransCreditsTotal" Expression="TransCredits.Value" Evaluator="Data3" PrintOn="TransFooter" ResetOnReprint="true"/>
    <Total Name="AllGiftDebits" Expression="GiftDebits.Value" Evaluator="Data1" PrintOn="PageFooter" ResetOnReprint="true"/>
    <Total Name="AllGiftCredits" Expression="GiftCredits.Value" Evaluator="Data1" PrintOn="PageFooter" ResetOnReprint="true"/>
    <Total Name="AllTransDebits" Expression="TransDebits.Value" Evaluator="Data3" PrintOn="PageFooter" ResetOnReprint="true"/>
    <Total Name="AllTransCredits" Expression="TransCredits.Value" Evaluator="Data3" PrintOn="PageFooter" ResetOnReprint="true"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <GroupHeaderBand Name="PageHeader" Width="718.2" Height="170.1" StartNewPage="true" Condition="[a_costCentre.a_cost_centre_code_c]" SortOrder="None">
      <TextObject Name="Text22" Left="444.15" Top="56.7" Width="56.7" Height="18.9" Text="[IIf([param_period],&quot;Period :&quot;,&quot;Date :&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="500.85" Top="56.7" Width="217.35" Height="18.9" Text="[param_date_title]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text11" Left="94.5" Top="94.5" Width="66.15" Height="18.9" Text="[a_costCentre.a_cost_centre_code_c]"/>
      <TextObject Name="Text12" Left="-9.45" Top="94.5" Width="103.95" Height="18.9" Text="Cost Centre :" HorzAlign="Right"/>
      <TextObject Name="Text21" Left="18.9" Top="75.6" Width="75.6" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="94.5" Top="75.6" Width="151.2" Height="18.9" Text="[param_currency_name]"/>
      <TextObject Name="Text10" Left="94.5" Top="56.7" Width="151.2" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text1" Left="217.35" Width="283.5" Height="28.35" Text="HOSA: [a_costCentre.a_cost_centre_name_c]" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text20" Left="18.9" Top="56.7" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <LineObject Name="Line1" Top="132.3" Width="718.2" Border.Width="2"/>
      <TextObject Name="Text88" Left="245.7" Top="28.35" Width="226.8" Height="18.9" Text="[param_ledger_name] " HorzAlign="Center"/>
      <TextObject Name="Text89" Left="491.4" Width="122.85" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text90" Left="614.25" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text91" Left="141.75" Top="18.9" Width="103.95" Height="18.9" Text="[param_requested_by]"/>
      <TextObject Name="Text26" Left="614.25" Top="18.9" Width="94.5" Height="18.9" Text="[param_version]"/>
      <TextObject Name="Text92" Top="18.9" Width="141.75" Height="18.9" Text="Report requested by :" HorzAlign="Right"/>
      <TextObject Name="Text93" Left="491.4" Top="18.9" Width="122.85" Height="18.9" Text="Version :" HorzAlign="Right"/>
      <TextObject Name="Text61" Left="387.45" Top="94.5" Width="113.4" Height="18.9" Text="[IIf([param_current_financial_year],&quot;Current Period :&quot;,&quot;&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text94" Left="500.85" Top="94.5" Width="103.95" Height="18.9" Text="[IIf([param_current_financial_year],[param_current_period],&quot;&quot;)]"/>
      <TextObject Name="Text4" Left="406.35" Top="113.4" Width="94.5" Height="18.9" Text="Ordered by :" HorzAlign="Right"/>
      <TextObject Name="Text5" Left="500.85" Top="113.4" Width="103.95" Height="18.9" Text="Account"/>
      <TextObject Name="Text95" Left="160.65" Top="94.5" Width="66.15" Height="18.9" Text="Account :" HorzAlign="Right"/>
      <TextObject Name="Text96" Left="226.8" Top="94.5" Width="94.5" Height="18.9" Text="All Accounts"/>
      <TextObject Name="Text6" Left="500.85" Top="75.6" Width="217.35" Height="18.9" Text="[IIf([param_start_period_i] == [param_end_period_i],IIf([param_current_financial_year],IIf([param_current_period]&gt;[param_end_period_i],&quot;(CLOSED)&quot;,IIf([param_current_period]&lt;[param_end_period_i],&quot;(FWD PERIOD)&quot;,&quot;(CURRENT)&quot;)),&quot;(CLOSED)&quot;),&quot;&quot;)]"/>
      <TextObject Name="Text97" Top="113.4" Width="94.5" Height="18.9" Text="Run Number :" HorzAlign="Right"/>
      <TextObject Name="Text7" Left="94.5" Top="113.4" Width="94.5" Height="18.9" Text="[param_run_number]"/>
      <TextObject Name="Text8" Left="226.8" Top="141.75" Width="94.5" Height="18.9" Text="Debits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text98" Left="321.3" Top="141.75" Width="94.5" Height="18.9" Text="Credits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text99" Left="425.25" Top="141.75" Width="302.4" Height="18.9" Text="Recipient Name / Description" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text100" Left="113.4" Top="141.75" Width="94.5" Height="18.9" Text="Reference" Font="Arial, 10pt, style=Bold, Italic"/>
      <DataBand Name="GiftsReport" Top="174.1" Width="718.2" DataSource="a_costCentre">
        <GroupHeaderBand Name="GiftsHeader" Top="178.1" Width="718.2" Height="18.9" Condition="[Gifts.accountcode]" SortOrder="None">
          <TextObject Name="Text64" Width="75.6" Height="18.9" Text="[Gifts.costcentre]-[Gifts.accountcode]" AutoShrink="FontSize" AutoShrinkMinSize="7" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
          <TextObject Name="Text65" Left="75.6" Width="642.6" Height="18.9" Text="[Gifts.a_account.a_account_code_long_desc_c], [a_costCentre.a_cost_centre_name_c]" WordWrap="false" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
          <DataBand Name="Data1" Top="201" Width="718.2" Height="18.9" DataSource="Gifts" Filter="[Gifts.costcentre]==[a_costCentre.a_cost_centre_code_c]">
            <TextObject Name="Text66" Left="113.4" Width="113.4" Height="18.9" CanGrow="true" Text="[Gifts.Reference]"/>
            <TextObject Name="GiftCredits" Left="321.3" Width="94.5" Height="18.9" Text="[IIf([param_currency]==&quot;Base&quot;,IIf([Gifts.GiftBaseAmount] &gt; 0,[Gifts.GiftBaseAmount], 0),IIf([Gifts.GiftIntlAmount] &gt; 0,[Gifts.GiftIntlAmount], 0))]" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="Text68" Left="425.25" Width="292.95" Height="18.9" CanGrow="true" Text="[Gifts.narrative]"/>
            <TextObject Name="GiftDebits" Left="226.8" Width="94.5" Height="18.9" Text="[IIf([param_currency]==&quot;Base&quot;,IIf([Gifts.GiftBaseAmount] &lt; 0,[Gifts.GiftBaseAmount], 0),IIf([Gifts.GiftIntlAmount] &lt; 0,[Gifts.GiftIntlAmount], 0))]" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="Text3" Width="113.4" Height="18.9" Text=" "/>
          </DataBand>
          <GroupFooterBand Name="GiftsFooter" Top="223.9" Width="718.2" Height="28.35">
            <TextObject Name="Text70" Left="321.3" Width="94.5" Height="18.9" Text="[GiftCreditsTotal]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="Text71" Left="226.8" Width="94.5" Height="18.9" Text="[GiftDebitsTotal]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <LineObject Name="Line2" Left="226.8" Width="189"/>
            <TextObject Name="Text83" Left="113.4" Width="113.4" Height="18.9" Text="Account Total :" HorzAlign="Right"/>
            <TextObject Name="Text84" Width="113.4" Height="18.9" Text=" "/>
          </GroupFooterBand>
        </GroupHeaderBand>
        <GroupHeaderBand Name="TransHeader" Top="256.25" Width="718.2" Height="18.9" Condition="[a_transaction.a_account_code_c]">
          <TextObject Name="Text72" Left="75.6" Width="633.15" Height="18.9" Text="[a_transaction.a_account.a_account_code_long_desc_c], [a_costCentre.a_cost_centre_name_c]" WordWrap="false" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
          <TextObject Name="Text73" Width="75.6" Height="18.9" Text="[a_costCentre.a_cost_centre_code_c]-[a_transaction.a_account.a_account_code_c]" AutoShrink="FontSize" AutoShrinkMinSize="7" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
          <DataBand Name="Data3" Top="279.15" Width="718.2" Height="18.9" DataSource="a_transaction" Filter="[a_costCentre.a_cost_centre_code_c]==[a_transaction.a_cost_centre_code_c]">
            <TextObject Name="TransDate" Width="113.4" Height="18.9" Text="[OmDate([a_transaction.a_transaction_date_d])]"/>
            <TextObject Name="Text75" Left="425.25" Width="292.95" Height="18.9" CanGrow="true" Text="[a_transaction.a_narrative_c]"/>
            <TextObject Name="Text76" Left="113.4" Width="113.4" Height="18.9" CanGrow="true" Text="[a_transaction.a_reference_c]"/>
            <TextObject Name="TransDebits" Left="226.8" Width="94.5" Height="18.9" Text="[IIf([a_transaction.a_debit_credit_indicator_l]==true,IIf([param_currency]==&quot;Base&quot;,[a_transaction.a_amount_in_base_currency_n],[a_transaction.a_amount_in_intl_currency_n]),0)]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="TransCredits" Left="321.3" Width="94.5" Height="18.9" Text="[IIf([a_transaction.a_debit_credit_indicator_l]==false,IIf([param_currency]==&quot;Base&quot;,[a_transaction.a_amount_in_base_currency_n],[a_transaction.a_amount_in_intl_currency_n]),0)]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
          </DataBand>
          <GroupFooterBand Name="TransFooter" Top="302.05" Width="718.2" Height="28.35">
            <TextObject Name="Text79" Left="226.8" Width="94.5" Height="18.9" Text="[TransDebitsTotal]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="Text80" Left="321.3" Width="94.5" Height="18.9" Text="[TransCreditsTotal]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <LineObject Name="Line3" Left="226.8" Width="189"/>
            <TextObject Name="Text82" Left="113.4" Width="113.4" Height="18.9" Text="Account Total :" HorzAlign="Right"/>
          </GroupFooterBand>
        </GroupHeaderBand>
      </DataBand>
      <GroupFooterBand Name="PageFooter" Top="334.4" Width="718.2" Height="47.25">
        <TextObject Name="AllDebits" Left="226.8" Top="9.45" Width="94.5" Height="18.9" Text="[[AllGiftDebits]+[AllTransDebits]]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
        </TextObject>
        <TextObject Name="AllCredits" Left="321.3" Top="9.45" Width="94.5" Height="18.9" Text="[[AllGiftCredits]+[AllTransCredits]]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
        </TextObject>
        <LineObject Name="Line4" Width="718.2" Border.Width="2"/>
        <TextObject Name="OutstandingDebit" Left="226.8" Top="28.35" Width="94.5" Height="18.9" Text="[[AllGiftDebits]+[AllTransDebits]-[AllGiftCredits]-[AllTransCredits]]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Red" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
          <Highlight>
            <Condition Expression="Value &lt; 0.01" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="OutstandingCredit" Left="321.3" Top="28.35" Width="94.5" Height="18.9" Text="[[AllGiftCredits]+[AllTransCredits]-[AllGiftDebits]-[AllTransDebits]]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Red" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
            <GeneralFormat/>
          </Formats>
          <Highlight>
            <Condition Expression="Value &lt; 0" Border.Lines="All" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="Text2" Left="113.4" Top="9.45" Width="113.4" Height="18.9" Text="Grand Total :" HorzAlign="Right"/>
        <TextObject Name="Text81" Left="113.4" Top="28.35" Width="113.4" Height="18.9" Text="Balance :" HorzAlign="Right"/>
      </GroupFooterBand>
    </GroupHeaderBand>
  </ReportPage>
</Report>
');

SELECT TRUE;