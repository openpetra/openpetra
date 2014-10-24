DELETE FROM s_report_template WHERE s_template_id_i=8;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(8,'Gift Batch Detail','OpenPetra default template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="10/13/2014 16:11:20" ReportInfo.CreatorVersion="2014.2.1.0">
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
    
    String variance(Decimal left, Decimal right)
    {
      if(right!= 0)
        return (Math.Floor(((decimal)left/(decimal)right)*100 - (Decimal)99.5)).ToString() + &quot;%&quot;;
      else
        return &quot; &quot;;
    }
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="GiftBatchDetail" ReferenceName="GiftBatchDetail" DataType="System.Int32" Enabled="true">
      <Column Name="a_batch_description_c" DataType="System.String"/>
      <Column Name="a_batch_status_c" DataType="System.String"/>
      <Column Name="a_gift_type_c" DataType="System.String"/>
      <Column Name="a_gl_effective_date_d" DataType="System.DateTime"/>
      <Column Name="a_bank_cost_centre_c" DataType="System.String"/>
      <Column Name="a_bank_account_code_c" DataType="System.String"/>
      <Column Name="a_currency_code_c" DataType="System.String"/>
      <Column Name="a_gift_transaction_number_i" DataType="System.Int32"/>
      <Column Name="p_recipient_key_n" DataType="System.Int64"/>
      <Column Name="a_confidential_gift_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_gift_amount_n" DataType="System.Decimal"/>
      <Column Name="a_gift_amount_intl_n" DataType="System.Decimal"/>
      <Column Name="a_gift_transaction_amount_n" DataType="System.Decimal"/>
      <Column Name="a_motivation_group_code_c" DataType="System.String"/>
      <Column Name="a_motivation_detail_code_c" DataType="System.String"/>
      <Column Name="a_recipient_ledger_number_n" DataType="System.Int64"/>
      <Column Name="p_donor_key_n" DataType="System.Int64"/>
      <Column Name="a_reference_c" DataType="System.String"/>
      <Column Name="a_method_of_giving_code_c" DataType="System.String"/>
      <Column Name="a_method_of_payment_code_c" DataType="System.String"/>
      <Column Name="a_receipt_letter_code_c" DataType="System.String"/>
      <Column Name="a_date_entered_d" DataType="System.DateTime"/>
      <Column Name="a_first_time_gift_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="p_partner_class_c" DataType="System.String"/>
      <Column Name="p_partner_short_name_c" DataType="System.String"/>
      <Column Name="p_receipt_letter_frequency_c" DataType="System.String"/>
      <Column Name="p_receipt_each_gift_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="p_partner_class_c1" DataType="System.String"/>
      <Column Name="p_partner_short_name_c1" DataType="System.String"/>
      <Column Name="a_receipt_number_i" DataType="System.Int32"/>
      <Column Name="a_detail_number_i" DataType="System.Int32"/>
      <Column Name="a_hash_total_n" DataType="System.Decimal"/>
      <Column Name="a_batch_total_n" DataType="System.Decimal"/>
      <Column Name="a_gift_comment_one_c" DataType="System.String"/>
      <Column Name="a_gift_comment_two_c" DataType="System.String"/>
      <Column Name="a_gift_comment_three_c" DataType="System.String"/>
      <Column Name="a_tax_deductible_pct_n" DataType="System.Decimal"/>
      <Column Name="readaccess" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="exworker" DataType="System.Boolean" BindableControl="CheckBox"/>
    </TableDataSource>
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
    <Parameter Name="param_costcentreoptions" DataType="System.String"/>
    <Parameter Name="param_cost_centre_codes" DataType="System.String"/>
    <Parameter Name="param_cost_centre_list_title" DataType="System.String"/>
    <Parameter Name="param_cost_centre_summary" DataType="System.Boolean"/>
    <Parameter Name="param_cost_centre_breakdown" DataType="System.Boolean"/>
    <Parameter Name="param_depth" DataType="System.String"/>
    <Parameter Name="param_ytd" DataType="System.String"/>
    <Parameter Name="param_currency_format" DataType="System.String"/>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_quarter" DataType="System.String"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_currency_name" DataType="System.String"/>
    <Parameter Name="param_real_year" DataType="System.Int32"/>
    <Parameter Name="param_period_breakdown" DataType="System.Boolean"/>
    <Parameter Name="param_period_checked" DataType="System.Boolean"/>
    <Parameter Name="param_quarter_checked" DataType="System.Boolean"/>
    <Parameter Name="param_real_year_ending" DataType="System.String"/>
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
    <Parameter Name="param_current_financial_year" DataType="System.Boolean"/>
    <Parameter Name="param_linked_partner_cc" DataType="System.String"/>
    <Parameter Name="param_paginate" DataType="System.Boolean"/>
    <Parameter Name="param_auto_email" DataType="System.Boolean"/>
    <Parameter Name="param_txtBatchNumber" DataType="System.Int32"/>
    <Parameter Name="param_batch_number_i" DataType="System.Int32"/>
    <Parameter Name="param_requested_by" DataType="System.String"/>
    <Parameter Name="param_version" DataType="System.String"/>
    <Parameter Name="param_tax_deductible_pct" DataType="System.Boolean"/>
    <Total Name="Split Gift Total" Expression="[GiftBatchDetail.a_gift_transaction_amount_n]" Evaluator="TransactionBand" PrintOn="GroupFooter2"/>
    <Total Name="Total Gift Recipients" TotalType="Count" Evaluator="TransactionBand" PrintOn="ReportSummary1"/>
    <Total Name="Tax Deductible Total" Expression="([GiftBatchDetail.a_tax_deductible_pct_n] / 100) * [GiftBatchDetail.a_gift_transaction_amount_n]" Evaluator="TransactionBand" PrintOn="ReportSummary1" EvaluateCondition="[param_tax_deductible_pct]"/>
  </Dictionary>
  <ReportPage Name="Page1" Landscape="true" PaperWidth="297" PaperHeight="210" RawPaperSize="9" StartPageEvent="Page1_StartPage">
    <ReportTitleBand Name="ReportTitle1" Width="1047.06" Height="170.1">
      <TextObject Name="Text1" Left="368.55" Width="283.5" Height="18.9" Text="Gift Batch Detail Report" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text20" Left="37.8" Top="56.7" Width="94.5" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="132.3" Top="56.7" Width="113.4" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text21" Left="37.8" Top="75.6" Width="94.5" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="132.3" Top="75.6" Width="113.4" Height="18.9" Text="[param_currency_name]"/>
      <TextObject Name="Text42" Left="368.55" Top="18.9" Width="283.5" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <LineObject Name="Line1" Left="1048.95" Top="160.65" Width="-1048.95" Border.Width="2"/>
      <TextObject Name="Text2" Left="935.55" Top="75.6" Width="113.4" Height="18.9" Text="[GiftBatchDetail.a_bank_cost_centre_c]"/>
      <TextObject Name="Text51" Left="812.7" Top="75.6" Width="122.85" Height="18.9" Text="Cost Centre :" HorzAlign="Right"/>
      <TextObject Name="Text98" Left="-8958.6" Top="-9450" Width="122.85" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text99" Left="-8835.75" Top="-9450" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text94" Left="-9308.25" Top="-9431.1" Width="113.4" Height="18.9" Text="[param_requested_by]"/>
      <TextObject Name="Text95" Left="-8835.75" Top="-9431.1" Width="94.5" Height="18.9" Text="[param_version]"/>
      <TextObject Name="Text96" Left="-9450" Top="-9431.1" Width="141.75" Height="18.9" Text="Report requested by :" HorzAlign="Right"/>
      <TextObject Name="Text97" Left="-8958.6" Top="-9431.1" Width="122.85" Height="18.9" Text="Version :" HorzAlign="Right"/>
      <TextObject Name="Text100" Left="784.35" Width="122.85" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text101" Left="907.2" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text102" Left="141.75" Top="18.9" Width="113.4" Height="18.9" Text="[param_requested_by]"/>
      <TextObject Name="Text103" Left="907.2" Top="18.9" Width="94.5" Height="18.9" Text="[param_version]"/>
      <TextObject Name="Text104" Top="18.9" Width="141.75" Height="18.9" Text="Report requested by :" HorzAlign="Right"/>
      <TextObject Name="Text105" Left="784.35" Top="18.9" Width="122.85" Height="18.9" Text="Version :" HorzAlign="Right"/>
      <TextObject Name="Text4" Left="330.75" Top="75.6" Width="94.5" Height="18.9" Text="[GiftBatchDetail.a_gift_type_c]"/>
      <TextObject Name="Text106" Left="264.6" Top="75.6" Width="66.15" Height="18.9" Text="Type :" HorzAlign="Right"/>
      <TextObject Name="Text6" Left="500.85" Top="75.6" Width="94.5" Height="18.9" Text="[param_batch_number_i]"/>
      <TextObject Name="Text107" Left="434.7" Top="75.6" Width="66.15" Height="18.9" Text="Batch :" HorzAlign="Right"/>
      <TextObject Name="Text7" Left="699.3" Top="75.6" Width="94.5" Height="18.9" Text="[GiftBatchDetail.a_batch_status_c]"/>
      <TextObject Name="Text108" Left="604.8" Top="75.6" Width="94.5" Height="18.9" Text="Batch Status :" HorzAlign="Right"/>
      <TextObject Name="Text109" Left="812.7" Top="94.5" Width="122.85" Height="18.9" Text="Account Code :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="935.55" Top="94.5" Width="113.4" Height="18.9" Text="[GiftBatchDetail.a_bank_account_code_c]"/>
      <TextObject Name="Text9" Left="132.3" Top="94.5" Width="453.6" Height="18.9" Text="[GiftBatchDetail.a_batch_description_c]"/>
      <TextObject Name="Text110" Top="94.5" Width="132.3" Height="18.9" Text="Batch Description :" HorzAlign="Right"/>
      <TextObject Name="Text11" Left="699.3" Top="94.5" Width="94.5" Height="18.9" Text="[GiftBatchDetail.a_gl_effective_date_d]" Format="Date" Format.Format="d"/>
      <TextObject Name="Text111" Left="604.8" Top="94.5" Width="94.5" Height="18.9" Text="Batch Date :" HorzAlign="Right"/>
      <TextObject Name="Text112" Top="132.3" Width="132.3" Height="18.9" Text="Letter Key :" HorzAlign="Right"/>
      <TextObject Name="Text113" Left="132.3" Top="132.3" Width="916.65" Height="18.9" Text="C = Confidential,  E = Receipt Each Gift,  N = New Donor,  X = Ex-Worker,  R = Restricted"/>
      <LineObject Name="Line3" Left="1048.95" Top="122.85" Width="-1048.95"/>
    </ReportTitleBand>
    <PageHeaderBand Name="TransactionPageHeader" Top="174.1" Width="1047.06" Height="37.8" BeforePrintEvent="TransactionPageHeader_BeforePrint">
      <TextObject Name="Text3" Left="47.25" Width="85.05" Height="18.9" Text="Recpt" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text23" Left="538.65" Width="103.95" Height="18.9" Text="Reference" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text41" Width="47.25" Height="18.9" Text="Trans" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text43" Left="727.65" Width="85.05" Height="18.9" Text="M. Paym" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text44" Left="1001.7" Width="47.25" Height="18.9" Text="ENX" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text47" Left="642.6" Width="85.05" Height="18.9" Exportable="false" Text="M. Giving" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text48" Left="916.65" Width="85.05" Height="18.9" Exportable="false" Text="R. Freq" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text49" Left="812.7" Width="103.95" Height="18.9" Text="Letter" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text5" Left="132.3" Width="85.05" Height="18.9" Text="Donor (Key" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text114" Left="217.35" Width="75.6" Height="18.9" Text="Class" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text115" Left="292.95" Width="245.7" Height="18.9" Text="Name)" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text74" Left="264.6" Top="18.9" Width="103.95" Height="18.9" Text="Recipient (Key" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text75" Left="85.05" Top="18.9" Width="85.05" Height="18.9" Text="M. Group" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text79" Left="680.4" Top="18.9" Width="94.5" Height="18.9" Text="Field" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text81" Left="774.9" Top="18.9" Width="85.05" Height="18.9" Text="Date" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text84" Left="878.85" Top="18.9" Width="85.05" Height="18.9" Text="Amount" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text86" Left="963.9" Top="18.9" Width="56.7" Height="18.9" Text="[IIf([param_tax_deductible_pct],&quot;% Tax&quot;,&quot;&quot;)]" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text87" Left="1020.6" Top="18.9" Width="28.35" Height="18.9" Text="C" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text117" Left="170.1" Top="18.9" Width="94.5" Height="18.9" Text="M. Detail" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text119" Left="368.55" Top="18.9" Width="75.6" Height="18.9" Text="Class" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text120" Left="444.15" Top="18.9" Width="236.25" Height="18.9" Text="Name)" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
      <TextObject Name="Text126" Left="9.45" Top="18.9" Width="75.6" Height="18.9" Text="Detail" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DimGray"/>
    </PageHeaderBand>
    <GroupHeaderBand Name="GroupHeader2" Top="215.9" Width="1047.06" Height="28.35" CanGrow="true" Condition="[GiftBatchDetail.a_gift_transaction_number_i]" KeepTogether="true">
      <TextObject Name="Text12" Top="9.45" Width="47.25" Height="18.9" Text="[GiftBatchDetail.a_gift_transaction_number_i]" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text13" Left="132.3" Top="9.45" Width="85.05" Height="18.9" Text="[IIf([GiftBatchDetail.readaccess],[GiftBatchDetail.p_donor_key_n],&quot;&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text14" Left="217.35" Top="9.45" Width="75.6" Height="18.9" Text="[IIf([GiftBatchDetail.readaccess],[GiftBatchDetail.p_partner_class_c],&quot;&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text15" Left="292.95" Top="9.45" Width="245.7" Height="18.9" CanGrow="true" Text="[IIf([GiftBatchDetail.readaccess],[GiftBatchDetail.p_partner_short_name_c],&quot;Confidential&quot;)]" HorzAlign="Right">
        <Highlight>
          <Condition Expression="[GiftBatchDetail.readaccess] == false;" Font="Arial, 9.75pt, style=Bold" ApplyFont="true"/>
        </Highlight>
      </TextObject>
      <TextObject Name="Text16" Left="47.25" Top="9.45" Width="85.05" Height="18.9" Text="[GiftBatchDetail.a_receipt_number_i]" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="538.65" Top="9.45" Width="103.95" Height="18.9" Text="[GiftBatchDetail.a_reference_c]" HorzAlign="Right"/>
      <TextObject Name="Text22" Left="642.6" Top="9.45" Width="85.05" Height="18.9" Text="[GiftBatchDetail.a_method_of_giving_code_c]" HorzAlign="Right"/>
      <TextObject Name="Text24" Left="727.65" Top="9.45" Width="85.05" Height="18.9" Text="[GiftBatchDetail.a_method_of_payment_code_c]" HorzAlign="Right"/>
      <TextObject Name="Text25" Left="812.7" Top="9.45" Width="94.5" Height="18.9" Text="[GiftBatchDetail.a_receipt_letter_code_c]" HorzAlign="Right"/>
      <TextObject Name="Text26" Left="907.2" Top="9.45" Width="94.5" Height="18.9" Text="[GiftBatchDetail.p_receipt_letter_frequency_c]" HorzAlign="Right"/>
      <TextObject Name="Text116" Left="1001.7" Top="9.45" Width="47.25" Height="18.9" Text="[IIf([GiftBatchDetail.p_receipt_each_gift_l],&quot;E&quot;,&quot;&quot;)] [IIf([GiftBatchDetail.readaccess],IIf([GiftBatchDetail.a_first_time_gift_l],&quot;N&quot;,&quot;&quot;),&quot;&quot;)] [IIf([GiftBatchDetail.readaccess],IIf([GiftBatchDetail.exworker],&quot;X&quot;,&quot;&quot;),&quot;&quot;)]" HorzAlign="Right"/>
      <DataBand Name="TransactionBand" Top="248.25" Width="1047.06" Height="75.6" CanGrow="true" CanShrink="true" BeforePrintEvent="Data_BeforePrint" KeepChild="true" DataSource="GiftBatchDetail">
        <TextObject Name="Text27" Left="85.05" Width="85.05" Height="18.9" Text="[GiftBatchDetail.a_motivation_group_code_c]" HorzAlign="Right" TextFill.Color="DimGray"/>
        <TextObject Name="Text31" Left="170.1" Width="94.5" Height="18.9" Text="[GiftBatchDetail.a_motivation_detail_code_c]" HorzAlign="Right" TextFill.Color="DimGray"/>
        <TextObject Name="Text28" Left="264.6" Width="103.95" Height="18.9" Text="[GiftBatchDetail.p_recipient_key_n]" HorzAlign="Right" TextFill.Color="DimGray"/>
        <TextObject Name="Text29" Left="368.55" Width="75.6" Height="18.9" Text="[GiftBatchDetail.p_partner_class_c1]" HorzAlign="Right" TextFill.Color="DimGray"/>
        <TextObject Name="Text30" Left="444.15" Width="236.25" Height="18.9" CanGrow="true" Text="[GiftBatchDetail.p_partner_short_name_c1]" HorzAlign="Right" TextFill.Color="DimGray"/>
        <TextObject Name="Text32" Left="680.4" Width="94.5" Height="18.9" Text="[GiftBatchDetail.a_recipient_ledger_number_n]" HorzAlign="Right" TextFill.Color="DimGray"/>
        <TextObject Name="Text33" Left="774.9" Width="85.05" Height="18.9" Text="[GiftBatchDetail.a_date_entered_d]" Format="Date" Format.Format="d" HorzAlign="Right" TextFill.Color="DimGray"/>
        <TextObject Name="Text34" Left="878.85" Width="85.05" Height="18.9" Text="[GiftBatchDetail.a_gift_transaction_amount_n]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" TextFill.Color="DimGray" Trimming="EllipsisCharacter"/>
        <TextObject Name="Text118" Left="1020.6" Width="28.35" Height="18.9" Text="[IIf([GiftBatchDetail.a_confidential_gift_flag_l],&quot;C&quot;,&quot;&quot;)]" HorzAlign="Right" TextFill.Color="DimGray"/>
        <TextObject Name="Text35" Left="37.8" Width="47.25" Height="18.9" Text="[GiftBatchDetail.a_detail_number_i]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="DimGray"/>
        <TextObject Name="Text50" Left="217.35" Top="18.9" Width="859.95" Height="18.9" CanShrink="true" Text="[GiftBatchDetail.a_gift_comment_one_c]" Font="Arial, 10pt, style=Italic" TextFill.Color="DimGray"/>
        <TextObject Name="Text52" Left="94.5" Top="37.8" Width="122.85" Height="18.9" CanShrink="true" Text="[IIf([GiftBatchDetail.a_gift_comment_two_c] == &quot;&quot;,&quot;&quot;,&quot;Comment Two :&quot;)]" HorzAlign="Right" Font="Arial, 10pt, style=Italic" TextFill.Color="DimGray"/>
        <TextObject Name="Text53" Left="94.5" Top="56.7" Width="122.85" Height="18.9" CanShrink="true" Text="[IIf([GiftBatchDetail.a_gift_comment_three_c] == &quot;&quot;,&quot;&quot;,&quot;Comment Three :&quot;)]" HorzAlign="Right" Font="Arial, 10pt, style=Italic" TextFill.Color="DimGray"/>
        <TextObject Name="Text54" Left="94.5" Top="18.9" Width="122.85" Height="18.9" CanShrink="true" Text="[IIf([GiftBatchDetail.a_gift_comment_one_c] == &quot;&quot;,&quot;&quot;,&quot;Comment One :&quot;)]" HorzAlign="Right" Font="Arial, 10pt, style=Italic" TextFill.Color="DimGray"/>
        <TextObject Name="Text55" Left="217.35" Top="37.8" Width="859.95" Height="18.9" CanShrink="true" Text="[GiftBatchDetail.a_gift_comment_two_c]" Font="Arial, 10pt, style=Italic" TextFill.Color="DimGray"/>
        <TextObject Name="Text56" Left="217.35" Top="56.7" Width="859.95" Height="18.9" CanShrink="true" Text="[GiftBatchDetail.a_gift_comment_three_c]" Font="Arial, 10pt, style=Italic" TextFill.Color="DimGray"/>
        <TextObject Name="Text124" Left="963.9" Width="56.7" Height="18.9" Text="[IIf([param_tax_deductible_pct],[GiftBatchDetail.a_tax_deductible_pct_n],&quot;&quot;)]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="DimGray"/>
        <Sort>
          <Sort Expression="[GiftBatchDetail.a_detail_number_i]"/>
        </Sort>
      </DataBand>
      <GroupFooterBand Name="GroupFooter2" Top="327.85" Width="1047.06" Height="18.9" CanShrink="true" KeepWithData="true">
        <TextObject Name="Text36" Left="595.35" Width="283.5" Height="18.9" CanShrink="true" Text="[IIf([GiftBatchDetail.a_detail_number_i] &gt; 1,&quot;Sub Total for Split Gift :&quot;,&quot;&quot;)]" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text37" Left="878.85" Width="85.05" Height="18.9" CanShrink="true" Text="[IIf([GiftBatchDetail.a_detail_number_i] &gt; 1,[Split Gift Total],&quot;&quot;)]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      </GroupFooterBand>
    </GroupHeaderBand>
    <ReportSummaryBand Name="ReportSummary1" Top="350.75" Width="1047.06" Height="94.5" CanShrink="true">
      <TextObject Name="Text38" Left="699.3" Top="18.9" Width="179.55" Height="18.9" Text="Batch Total :" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <LineObject Name="Line4" Left="1048.95" Top="9.45" Width="-1048.95" Border.Style="Double" Border.Width="2"/>
      <TextObject Name="Text122" Left="236.25" Top="56.7" Width="198.45" Height="18.9" Text="Total Gift Recipients : " HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text39" Left="434.7" Top="56.7" Width="56.7" Height="18.9" Text="[Total Gift Recipients]" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text40" Left="878.85" Top="18.9" Width="85.05" Height="18.9" Text="[GiftBatchDetail.a_batch_total_n]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text123" Left="699.3" Top="56.7" Width="179.55" Height="18.9" Text="Operators Batch Total :" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text45" Left="878.85" Top="56.7" Width="85.05" Height="18.9" Text="[GiftBatchDetail.a_hash_total_n]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text46" Left="472.5" Top="75.6" Width="406.35" Height="18.9" Text="[IIf([GiftBatchDetail.a_hash_total_n] == 0,&quot;No Hash Total&quot;,IIf([GiftBatchDetail.a_hash_total_n] - [GiftBatchDetail.a_batch_total_n] == 0,&quot;TOTALS ARE IN BALANCE&quot;,&quot;TOTALS ARE OUT OF BALANCE BY &quot; + ToString(FormatNumber([GiftBatchDetail.a_hash_total_n] - [GiftBatchDetail.a_batch_total_n], 2))))]" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text125" Left="699.3" Top="37.8" Width="179.55" Height="18.9" CanShrink="true" Text="[IIf([param_tax_deductible_pct],&quot;Tax Deductible Total :&quot;,&quot;&quot;)]" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text58" Left="878.85" Top="37.8" Width="85.05" Height="18.9" CanShrink="true" Text="[Tax Deductible Total]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
    </ReportSummaryBand>
    <PageFooterBand Name="PageFooter1" Top="449.25" Width="1047.06" Height="18.9">
      <TextObject Name="Text17" Left="888.3" Width="160.65" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;