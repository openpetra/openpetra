DELETE FROM s_report_template WHERE s_template_id_i=2;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(2,'Balance Sheet Standard','OpenPetra default template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="08/29/2014 14:54:01" ReportInfo.CreatorVersion="2014.2.1.0">
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
    
    private void Data_BeforePrint(object sender, EventArgs e)
    {
      Int32 AccountLevel = ((Int32)Report.GetColumnValue(&quot;BalanceSheet.accountlevel&quot;));
      HeaderBand.Visible = false;
      TransactionBand.Visible = false;
      FooterBand.Visible = false;
      FooterLevel1.Visible = false;

      if(((Boolean)Report.GetColumnValue(&quot;BalanceSheet.haschildren&quot;)))
      {
        HeaderBand.Visible = (AccountLevel &gt; 0);
        HeaderAccountNameField.Left = Units.Millimeters * (AccountLevel * 4);
      }
      else
      {
        if(((Boolean)Report.GetColumnValue(&quot;BalanceSheet.parentfooter&quot;)))
        {
          FooterBand.Visible = (AccountLevel &gt; 1);
          FooterLevel1.Visible = (AccountLevel &lt;= 1);
          FooterAccountCodeField.Left = Units.Millimeters * (AccountLevel * 4);
          FooterAccountNameField.Left = Units.Millimeters * (15 + (AccountLevel * 4));
        }
        else
        {
          TransactionBand.Visible = true;
          AccountCodeField.Left = Units.Millimeters * (AccountLevel * 4);
          AccountNameField.Left = Units.Millimeters * (15 + (AccountLevel * 4));
        }
      }
    }
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="BalanceSheet" ReferenceName="BalanceSheet" DataType="System.Int32" Enabled="true">
      <Column Name="seq" DataType="System.Int32"/>
      <Column Name="accountlevel" DataType="System.Int32"/>
      <Column Name="accountpath" DataType="System.String"/>
      <Column Name="accounttype" DataType="System.String"/>
      <Column Name="year" DataType="System.Int32"/>
      <Column Name="period" DataType="System.Int32"/>
      <Column Name="accountcode" DataType="System.String"/>
      <Column Name="accountname" DataType="System.String"/>
      <Column Name="yearstart" DataType="System.Decimal"/>
      <Column Name="actual" DataType="System.Decimal"/>
      <Column Name="actualytd" DataType="System.Decimal"/>
      <Column Name="actuallastyear" DataType="System.Decimal"/>
      <Column Name="haschildren" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="parentfooter" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="accountissummary" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="accounttypeorder" DataType="System.Int32"/>
      <Column Name="costcentrecode" DataType="System.String"/>
      <Column Name="debitcredit" DataType="System.Boolean" BindableControl="CheckBox"/>
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
    <Parameter Name="param_cost_centre_list_title" DataType="System.Int32"/>
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
    <Parameter Name="param_requested_by" DataType="System.String"/>
    <Parameter Name="param_version" DataType="System.String"/>
    <Parameter Name="param_current_period" DataType="System.Int32"/>
    <Parameter Name="param_base_currency" DataType="System.String"/>
    <Total Name="TotalLiabPlusEq" Expression="[BalanceSheet.actual]" Evaluator="TransactionBand" EvaluateCondition="[BalanceSheet.accountlevel]==1 &amp;&amp; ([BalanceSheet.accounttype]==&quot;Liability&quot;||[BalanceSheet.accounttype]==&quot;Equity&quot;)&amp;&amp;[BalanceSheet.parentfooter]"/>
    <Total Name="TotalLiabPlusEqLastYear" Expression="[BalanceSheet.actuallastyear]" Evaluator="TransactionBand" EvaluateCondition="[BalanceSheet.accountlevel]==1 &amp;&amp; ([BalanceSheet.accounttype]==&quot;Liability&quot;||[BalanceSheet.accounttype]==&quot;Equity&quot;)&amp;&amp;[BalanceSheet.parentfooter]"/>
  </Dictionary>
  <ReportPage Name="Page1" RawPaperSize="9">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="103.95">
      <TextObject Name="Text1" Left="245.7" Width="226.8" Height="18.9" Text="Balance Sheet" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="519.75" Width="85.05" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="604.8" Width="113.4" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Top="56.7" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Top="56.7" Width="113.4" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text21" Top="75.6" Width="75.6" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="75.6" Top="75.6" Width="141.75" Height="18.9" Text="[param_currency_name] ([param_currency])"/>
      <TextObject Name="Text22" Left="444.15" Top="56.7" Width="85.05" Height="18.9" Text="Balance at :" HorzAlign="Right"/>
      <TextObject Name="Text42" Left="245.7" Top="28.35" Width="226.8" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <LineObject Name="Line1" Left="718.2" Top="94.5" Width="-718.2"/>
      <TextObject Name="Text19" Left="529.2" Top="56.7" Width="189" Height="18.9" Text="[OmDate([param_end_date])][IIf([param_current_financial_year],IIf([param_current_period]&gt;[param_end_period_i],&quot; (CLOSED)&quot;,IIf([param_current_period]&lt;[param_end_period_i],&quot; (FWD PERIOD)&quot;,&quot; (CURRENT)&quot;)),&quot; (CLOSED)&quot;)]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text94" Left="141.75" Top="18.9" Width="113.4" Height="18.9" Text="[param_requested_by]"/>
      <TextObject Name="Text95" Left="604.8" Top="18.9" Width="94.5" Height="18.9" Text="[param_version]"/>
      <TextObject Name="Text96" Top="18.9" Width="141.75" Height="18.9" Text="Report requested by :" HorzAlign="Right"/>
      <TextObject Name="Text97" Left="481.95" Top="18.9" Width="122.85" Height="18.9" Text="Version :" HorzAlign="Right"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="107.95" Width="718.2" Height="18.9">
      <TextObject Name="Text3" Left="113.4" Width="151.2" Height="18.9" Text="Description" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text5" Left="349.65" Width="103.95" Height="18.9" Text="Actual" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text41" Left="37.8" Width="56.7" Height="18.9" Text="Acc" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text43" Left="463.05" Width="122.85" Height="18.9" Text="Actual Last Year" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
    </PageHeaderBand>
    <DataBand Name="TransactionBand" Top="130.85" Width="718.2" Height="18.9" BeforePrintEvent="Data_BeforePrint" KeepChild="true" DataSource="BalanceSheet">
      <TextObject Name="ActualField" Left="340.2" Width="113.4" Height="18.9" Text="[BalanceSheet.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
      <TextObject Name="ActualYTDField" Left="472.5" Width="113.4" Height="18.9" Text="[BalanceSheet.actuallastyear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
      <TextObject Name="AccountCodeField" Left="45.36" Width="56.7" Height="18.9" Text="[BalanceSheet.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt"/>
      <TextObject Name="AccountNameField" Left="102.06" Width="236.25" Height="18.9" Text="[BalanceSheet.accountname]" Padding="0, 0, 2, 0" Font="Arial, 9pt"/>
      <ChildBand Name="HeaderBand" Top="153.75" Width="718.2" Height="18.9" Visible="false">
        <TextObject Name="HeaderAccountNameField" Left="30.24" Width="217.35" Height="18.9" Text="[BalanceSheet.accountname]:" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
        <ChildBand Name="FooterBand" Top="176.65" Width="718.2" Height="18.9" Visible="false">
          <TextObject Name="FooterActualField" Left="340.2" Width="113.4" Height="18.9" Text="[BalanceSheet.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
          <TextObject Name="FooterActualYTDField" Left="472.5" Width="113.4" Height="18.9" Text="[BalanceSheet.actuallastyear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
          <TextObject Name="FooterAccountNameField" Left="56.7" Width="274.05" Height="18.9" Text="Total [BalanceSheet.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
          <TextObject Name="FooterAccountCodeField" Width="56.7" Height="18.9" Text="[BalanceSheet.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray"/>
          <ChildBand Name="FooterLevel1" Top="199.55" Width="718.2" Height="37.8">
            <TextObject Name="Level1ActualField" Left="340.2" Width="113.4" Height="18.9" Text="[BalanceSheet.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold, Underline" Trimming="EllipsisCharacter"/>
            <TextObject Name="Level1ActualYTDField" Left="472.5" Width="113.4" Height="18.9" Text="[BalanceSheet.actuallastyear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold, Underline" Trimming="EllipsisCharacter"/>
            <TextObject Name="Level1AccountNameField" Width="207.9" Height="18.9" Text="[BalanceSheet.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
          </ChildBand>
        </ChildBand>
      </ChildBand>
    </DataBand>
    <ReportSummaryBand Name="ReportSummary1" Top="241.35" Width="718.2" Height="18.9">
      <TextObject Name="Text2" Left="340.2" Width="113.4" Height="18.9" Text="[TotalLiabPlusEq]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text52" Width="217.35" Height="18.9" Text="Total Equity + Liabilities" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text4" Left="472.5" Width="113.4" Height="18.9" Text="[TotalLiabPlusEqLastYear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
    </ReportSummaryBand>
    <PageFooterBand Name="PageFooter1" Top="264.25" Width="718.2" Height="18.9">
      <TextObject Name="Text17" Left="567" Width="151.2" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;