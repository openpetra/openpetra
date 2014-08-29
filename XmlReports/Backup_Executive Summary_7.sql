DELETE FROM s_report_template WHERE s_template_id_i=7;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(7,'Executive Summary','Executive Summary template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="08/29/2014 14:47:24" ReportInfo.CreatorVersion="2014.2.1.0">
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
    <TableDataSource Name="Accounts" Alias="Executive Summary" ReferenceName="Accounts" DataType="System.Int32" Enabled="true">
      <Column Name="IncomeThisMonth" DataType="System.Decimal"/>
      <Column Name="ExpensesThisMonth" DataType="System.Decimal"/>
      <Column Name="PersonnelCostsThisMonth" DataType="System.Decimal"/>
      <Column Name="SupportIncomeThisMonth" DataType="System.Decimal"/>
      <Column Name="CashAndBankThisMonth" DataType="System.Decimal"/>
      <Column Name="PaymentsDueThisMonth" DataType="System.Decimal"/>
      <Column Name="GiftsForOtherFieldsThisMonth" DataType="System.Decimal"/>
      <Column Name="PersonnelThisMonth" DataType="System.Int32"/>
      <Column Name="PersonnelOtherFieldsThisMonth" DataType="System.Int32"/>
      <Column Name="IncomeActualYTD" DataType="System.Decimal"/>
      <Column Name="ExpensesActualYTD" DataType="System.Decimal"/>
      <Column Name="PersonnelCostsActualYTD" DataType="System.Decimal"/>
      <Column Name="SupportIncomeActualYTD" DataType="System.Decimal"/>
      <Column Name="CashAndBankActualYTD" DataType="System.Decimal"/>
      <Column Name="PaymentsDueActualYTD" DataType="System.Decimal"/>
      <Column Name="GiftsForOtherFieldsActualYTD" DataType="System.Decimal"/>
      <Column Name="PersonnelActualYTD" DataType="System.Int32"/>
      <Column Name="PersonnelOtherFieldsActualYTD" DataType="System.Int32"/>
      <Column Name="IncomeBudgetYTD" DataType="System.Decimal"/>
      <Column Name="ExpensesBudgetYTD" DataType="System.Decimal"/>
      <Column Name="PersonnelCostsBudgetYTD" DataType="System.Decimal"/>
      <Column Name="SupportIncomeBudgetYTD" DataType="System.Decimal"/>
      <Column Name="CashAndBankBudgetYTD" DataType="System.Decimal"/>
      <Column Name="PaymentsDueBudgetYTD" DataType="System.Decimal"/>
      <Column Name="GiftsForOtherFieldsBudgetYTD" DataType="System.Decimal"/>
      <Column Name="PersonnelBudgetYTD" DataType="System.Int32"/>
      <Column Name="PersonnelOtherFieldsBudgetYTD" DataType="System.Int32"/>
      <Column Name="IncomePriorYTD" DataType="System.Decimal"/>
      <Column Name="ExpensesPriorYTD" DataType="System.Decimal"/>
      <Column Name="PersonnelCostsPriorYTD" DataType="System.Decimal"/>
      <Column Name="SupportIncomePriorYTD" DataType="System.Decimal"/>
      <Column Name="CashAndBankPriorYTD" DataType="System.Decimal"/>
      <Column Name="PaymentsDuePriorYTD" DataType="System.Decimal"/>
      <Column Name="GiftsForOtherFieldsPriorYTD" DataType="System.Decimal"/>
      <Column Name="PersonnelPriorYTD" DataType="System.Int32"/>
      <Column Name="PersonnelOtherFieldsPriorYTD" DataType="System.Int32"/>
      <Column Name="FromToICHThisMonth" DataType="System.Decimal"/>
      <Column Name="FromToICHActualYTD" DataType="System.Decimal"/>
      <Column Name="FromToICHBudgetYTD" DataType="System.Decimal"/>
      <Column Name="FromToICHPriorYTD" DataType="System.Decimal"/>
    </TableDataSource>
    <Parameter Name="param_diff_period_i" DataType="System.Int32"/>
    <Parameter Name="param_account_hierarchy_c" DataType="System.String"/>
    <Parameter Name="param_currency" DataType="System.String"/>
    <Parameter Name="param_period" DataType="System.Boolean"/>
    <Parameter Name="param_date_checked" DataType="System.Boolean"/>
    <Parameter Name="param_start_period_i" DataType="System.Int32"/>
    <Parameter Name="param_end_period_i" DataType="System.Int32"/>
    <Parameter Name="param_year_i" DataType="System.Int32"/>
    <Parameter Name="param_start_date" DataType="System.DateTime"/>
    <Parameter Name="param_end_date" DataType="System.DateTime"/>
    <Parameter Name="param_sortby" DataType="System.String"/>
    <Parameter Name="param_account_list_title" DataType="System.String"/>
    <Parameter Name="param_account_codes" DataType="System.String"/>
    <Parameter Name="param_account_code_start" DataType="System.Int32"/>
    <Parameter Name="param_account_code_end" DataType="System.Int32"/>
    <Parameter Name="param_rgrAccounts" DataType="System.String"/>
    <Parameter Name="param_cost_centre_list_title" DataType="System.String"/>
    <Parameter Name="param_cost_centre_codes" DataType="System.String"/>
    <Parameter Name="param_cost_centre_code_start" DataType="System.String"/>
    <Parameter Name="param_cost_centre_code_end" DataType="System.String"/>
    <Parameter Name="param_rgrCostCentres" DataType="System.String"/>
    <Parameter Name="param_depth" DataType="System.String"/>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_with_analysis_attributes" DataType="System.Boolean"/>
    <Parameter Name="param_quarter" DataType="System.String"/>
    <Parameter Name="param_daterange" DataType="System.String"/>
    <Parameter Name="param_groupfield" DataType="System.String"/>
    <Parameter Name="param_currency_formatter" DataType="System.String"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_analyis_type_start" DataType="System.String"/>
    <Parameter Name="param_analyis_type_end" DataType="System.String"/>
    <Parameter Name="param_currency_name" DataType="System.String"/>
    <Parameter Name="param_real_year" DataType="System.Int32"/>
    <Parameter Name="param_period_breakdown" DataType="System.Boolean"/>
    <Parameter Name="param_period_checked" DataType="System.Boolean"/>
    <Parameter Name="param_quarter_checked" DataType="System.Boolean"/>
    <Parameter Name="param_real_year_ending" DataType="System.String"/>
    <Parameter Name="param_ich_number" DataType="System.Int32"/>
    <Parameter Name="param_date_title" DataType="System.String"/>
    <Parameter Name="param_base_currency" DataType="System.String"/>
    <Parameter Name="param_intl_currency" DataType="System.String"/>
    <Parameter Name="param_base_currency_symbol" DataType="System.String"/>
    <Parameter Name="param_intl_currency_symbol" DataType="System.String"/>
    <Parameter Name="param_current_period" DataType="System.Int32"/>
    <Parameter Name="param_current_financial_year" DataType="System.Boolean"/>
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
    <Parameter Name="param_requested_by" DataType="System.String"/>
    <Parameter Name="param_version" DataType="System.String"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="103.95">
      <TextObject Name="Text1" Left="226.8" Width="245.7" Height="28.35" Text="Executive Summary" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="491.4" Width="122.85" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="614.25" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Top="56.7" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Top="56.7" Width="170.1" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text22" Left="311.85" Top="56.7" Width="113.4" Height="18.9" Text="At Period ([ToString([param_end_period_i])]) :" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="425.25" Top="56.7" Width="292.95" Height="18.9" Text="[OmDate([param_start_date])] to [OmDate([param_end_date])][IIf([param_current_financial_year],IIf([param_current_period]&gt;[param_end_period_i],&quot; (CLOSED)&quot;,IIf([param_current_period]&lt;[param_end_period_i],&quot; (FWD PERIOD)&quot;,&quot; (CURRENT)&quot;)),&quot; (CLOSED)&quot;)]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text42" Left="245.7" Top="28.35" Width="207.9" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <LineObject Name="Line1" Left="718.2" Top="94.5" Width="-718.2" Border.Width="2"/>
      <TextObject Name="Text61" Left="302.4" Top="75.6" Width="122.85" Height="18.9" Text="[IIf([param_current_financial_year],&quot;Current Period :&quot;,&quot;&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="425.25" Top="75.6" Width="94.5" Height="18.9" Text="[IIf([param_current_financial_year],[param_current_period],&quot;&quot;)]"/>
      <TextObject Name="Text54" Left="75.6" Top="75.6" Width="151.2" Height="18.9" Text="[param_currency_name]"/>
      <TextObject Name="Text89" Left="-47.25" Top="75.6" Width="122.85" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text90" Left="-9308.25" Top="-9450" Width="103.95" Height="18.9" Text="[param_requested_by]"/>
      <TextObject Name="Text91" Left="-8873.55" Top="-9450" Width="94.5" Height="18.9" Text="[param_version]"/>
      <TextObject Name="Text92" Left="-9450" Top="-9450" Width="141.75" Height="18.9" Text="Report requested by :" HorzAlign="Right"/>
      <TextObject Name="Text93" Left="-8996.4" Top="-9450" Width="122.85" Height="18.9" Text="Version :" HorzAlign="Right"/>
      <TextObject Name="Text94" Left="141.75" Top="18.9" Width="113.4" Height="18.9" Text="[param_requested_by]"/>
      <TextObject Name="Text95" Left="614.25" Top="18.9" Width="94.5" Height="18.9" Text="[param_version]"/>
      <TextObject Name="Text96" Top="18.9" Width="141.75" Height="18.9" Text="Report requested by :" HorzAlign="Right"/>
      <TextObject Name="Text97" Left="491.4" Top="18.9" Width="122.85" Height="18.9" Text="Version :" HorzAlign="Right"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="107.95" Width="718.2" Height="18.9">
      <TextObject Name="Text7" Left="378" Width="113.4" Height="18.9" Text="Actual YTD" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text12" Left="491.4" Width="113.4" Height="18.9" Text="Budget YTD" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text14" Left="604.8" Width="113.4" Height="18.9" Text="Prior YTD" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text5" Left="264.6" Width="113.4" Height="18.9" Text="This Month" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
    </PageHeaderBand>
    <GroupHeaderBand Name="GroupHeader1" Top="130.85" Width="718.2" Condition="[Executive Summary.IncomeThisMonth]">
      <GroupHeaderBand Name="GroupHeader2" Top="134.85" Width="718.2" Condition="[Executive Summary.IncomeThisMonth]">
        <DataBand Name="Transaction" Top="138.85" Width="718.2" Height="349.65" CanGrow="true" KeepChild="true" DataSource="Accounts" KeepDetail="true">
          <TextObject Name="Text2" Width="264.6" Height="18.9" Text="Total Income :"/>
          <TextObject Name="Text4" Left="264.6" Width="113.4" Height="18.9" Text="[Executive Summary.IncomeThisMonth]" NullValue=" " Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text6" Left="378" Width="113.4" Height="18.9" Text="[Executive Summary.IncomeActualYTD]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text11" Left="491.4" Width="113.4" Height="18.9" Text="[Executive Summary.IncomeBudgetYTD]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text13" Left="604.8" Width="113.4" Height="18.9" Text="[Executive Summary.IncomePriorYTD]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text63" Top="18.9" Width="264.6" Height="18.9" Text="Total Expenses :"/>
          <TextObject Name="Text3" Left="264.6" Top="18.9" Width="113.4" Height="18.9" Text="[Executive Summary.ExpensesThisMonth]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text21" Left="378" Top="18.9" Width="113.4" Height="18.9" Text="[Executive Summary.ExpensesActualYTD]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text26" Left="491.4" Top="18.9" Width="113.4" Height="18.9" Text="[Executive Summary.ExpensesBudgetYTD]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text27" Left="604.8" Top="18.9" Width="113.4" Height="18.9" Text="[Executive Summary.ExpensesPriorYTD]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <LineObject Name="Line2" Left="718.2" Top="47.25" Width="-718.2"/>
          <TextObject Name="Text57" Top="56.7" Width="56.7" Height="18.9" Text="Surplus" TextFill.Color="Blue"/>
          <TextObject Name="Text15" Left="264.6" Top="56.7" Width="113.4" Height="18.9" Text="[IIf([Executive Summary.IncomeThisMonth] &gt;= [Executive Summary.ExpensesThisMonth],[Executive Summary.IncomeThisMonth] - [Executive Summary.ExpensesThisMonth],&quot;(&quot; + FormatNumber([Executive Summary.ExpensesThisMonth] - [Executive Summary.IncomeThisMonth], 2) + &quot;)&quot;)]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue">
            <Highlight>
              <Condition Expression="[Executive Summary.IncomeThisMonth] &lt; [Executive Summary.ExpensesThisMonth]"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text64" Left="66.15" Top="56.7" Width="198.45" Height="18.9" Text=" (Deficit) :" TextFill.Color="Red"/>
          <TextObject Name="Text65" Left="56.7" Top="56.7" Width="9.45" Height="18.9" Text="/"/>
          <TextObject Name="Text66" Left="378" Top="56.7" Width="113.4" Height="18.9" Text="[IIf([Executive Summary.IncomeActualYTD] &gt;= [Executive Summary.ExpensesActualYTD],[Executive Summary.IncomeActualYTD] - [Executive Summary.ExpensesActualYTD],&quot;(&quot; + FormatNumber([Executive Summary.ExpensesActualYTD] - [Executive Summary.IncomeActualYTD], 2) + &quot;)&quot;)]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue">
            <Highlight>
              <Condition Expression="[Executive Summary.IncomeActualYTD] &lt; [Executive Summary.ExpensesActualYTD]"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text67" Left="491.4" Top="56.7" Width="113.4" Height="18.9" Text="[IIf([Executive Summary.IncomeBudgetYTD] &gt;= [Executive Summary.ExpensesBudgetYTD],[Executive Summary.IncomeBudgetYTD] - [Executive Summary.ExpensesBudgetYTD],&quot;(&quot; + FormatNumber([Executive Summary.ExpensesBudgetYTD] - [Executive Summary.IncomeBudgetYTD], 2) + &quot;)&quot;)]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue">
            <Highlight>
              <Condition Expression="[Executive Summary.IncomeBudgetYTD] &lt; [Executive Summary.ExpensesBudgetYTD]"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text68" Left="604.8" Top="56.7" Width="113.4" Height="18.9" Text="[IIf([Executive Summary.IncomePriorYTD] &gt;= [Executive Summary.ExpensesPriorYTD],[Executive Summary.IncomePriorYTD] - [Executive Summary.ExpensesPriorYTD],&quot;(&quot; + FormatNumber([Executive Summary.ExpensesPriorYTD] - [Executive Summary.IncomePriorYTD], 2) + &quot;)&quot;)]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue">
            <Highlight>
              <Condition Expression="[Executive Summary.IncomePriorYTD] &lt; [Executive Summary.ExpensesPriorYTD]"/>
            </Highlight>
          </TextObject>
          <LineObject Name="Line3" Left="718.2" Top="85.05" Width="-718.2" Border.Width="2"/>
          <TextObject Name="Text69" Top="302.4" Width="264.6" Height="18.9" Text="Number of Personnel on Field (Adults) :"/>
          <TextObject Name="Text16" Left="264.6" Top="302.4" Width="113.4" Height="18.9" Text="[Executive Summary.PersonnelThisMonth]" HorzAlign="Right"/>
          <TextObject Name="Text17" Left="378" Top="302.4" Width="113.4" Height="18.9" Text="[Executive Summary.PersonnelActualYTD]" HorzAlign="Right"/>
          <TextObject Name="Text24" Left="604.8" Top="302.4" Width="113.4" Height="18.9" Text="[Executive Summary.PersonnelPriorYTD]" HorzAlign="Right"/>
          <TextObject Name="Text70" Top="94.5" Width="264.6" Height="18.9" Text="Personnel Costs per Team Member :"/>
          <TextObject Name="Text23" Left="264.6" Top="94.5" Width="113.4" Height="18.9" Text="[Executive Summary.PersonnelCostsThisMonth]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text25" Left="378" Top="94.5" Width="113.4" Height="18.9" Text="[Executive Summary.PersonnelCostsActualYTD]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text28" Left="604.8" Top="94.5" Width="113.4" Height="18.9" Text="[Executive Summary.PersonnelCostsPriorYTD]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <LineObject Name="Line4" Left="718.2" Top="292.95" Width="-718.2" Border.Width="2"/>
          <TextObject Name="Text71" Top="113.4" Width="274.05" Height="18.9" Text="Support Income as % of Personnel Costs :"/>
          <TextObject Name="Text29" Left="283.5" Top="113.4" Width="94.5" Height="18.9" Text="[Executive Summary.SupportIncomeThisMonth]" Format="Percent" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text30" Left="396.9" Top="113.4" Width="94.5" Height="18.9" Text="[Executive Summary.SupportIncomeActualYTD]" Format="Percent" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text31" Left="510.3" Top="113.4" Width="94.5" Height="18.9" Text="[Executive Summary.SupportIncomeBudgetYTD]" Format="Percent" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text32" Left="623.7" Top="113.4" Width="94.5" Height="18.9" Text="[Executive Summary.SupportIncomePriorYTD]" Format="Percent" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <LineObject Name="Line5" Left="718.2" Top="141.75" Width="-718.2" Border.Width="2"/>
          <TextObject Name="Text72" Top="151.2" Width="264.6" Height="18.9" Text="Total Cash &amp; Bank :"/>
          <TextObject Name="Text73" Top="170.1" Width="37.8" Height="18.9" Text="Due"/>
          <TextObject Name="Text74" Left="37.8" Top="170.1" Width="47.25" Height="18.9" Text="From" TextFill.Color="Blue"/>
          <TextObject Name="Text75" Left="94.5" Top="170.1" Width="37.8" Height="18.9" Text=" (To)" TextFill.Color="Red"/>
          <TextObject Name="Text76" Left="85.05" Top="170.1" Width="9.45" Height="18.9" Text="/"/>
          <TextObject Name="Text77" Left="132.3" Top="170.1" Width="37.8" Height="18.9" Text="ICH :"/>
          <TextObject Name="Text35" Left="378" Top="151.2" Width="113.4" Height="18.9" Text="[Executive Summary.CashAndBankActualYTD]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text37" Left="491.4" Top="151.2" Width="113.4" Height="18.9" Text="[Executive Summary.CashAndBankBudgetYTD]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text39" Left="604.8" Top="151.2" Width="113.4" Height="18.9" Text="[Executive Summary.CashAndBankPriorYTD]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text33" Left="264.6" Top="151.2" Width="113.4" Height="18.9" Text="[Executive Summary.CashAndBankThisMonth]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text34" Left="264.6" Top="170.1" Width="113.4" Height="18.9" Text="[IIf([Executive Summary.FromToICHThisMonth] &gt;= 0, [Executive Summary.FromToICHThisMonth], &quot;(&quot; + FormatNumber(-[Executive Summary.FromToICHThisMonth]) + &quot;)&quot;)]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" TextFill.Color="Blue" Trimming="EllipsisCharacter">
            <Highlight>
              <Condition Expression="[Executive Summary.FromToICHThisMonth] &lt; 0"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text78" Left="378" Top="170.1" Width="113.4" Height="18.9" Text="[IIf([Executive Summary.FromToICHActualYTD] &gt;= 0, [Executive Summary.FromToICHActualYTD], &quot;(&quot; + FormatNumber(-[Executive Summary.FromToICHActualYTD]) + &quot;)&quot;)]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" TextFill.Color="Blue" Trimming="EllipsisCharacter">
            <Highlight>
              <Condition Expression="[Executive Summary.FromToICHActualYTD] &lt; 0"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text79" Left="491.4" Top="170.1" Width="113.4" Height="18.9" Text="[IIf([Executive Summary.FromToICHBudgetYTD] &gt;= 0, [Executive Summary.FromToICHBudgetYTD], &quot;(&quot; + FormatNumber(-[Executive Summary.FromToICHBudgetYTD]) + &quot;)&quot;)]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" TextFill.Color="Blue" Trimming="EllipsisCharacter">
            <Highlight>
              <Condition Expression="[Executive Summary.FromToICHBudgetYTD] &lt; 0"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text80" Left="604.8" Top="170.1" Width="113.4" Height="18.9" Text="[IIf([Executive Summary.FromToICHPriorYTD] &gt;= 0, [Executive Summary.FromToICHPriorYTD], &quot;(&quot; + FormatNumber(-[Executive Summary.FromToICHPriorYTD]) + &quot;)&quot;)]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" TextFill.Color="Blue" Trimming="EllipsisCharacter">
            <Highlight>
              <Condition Expression="[Executive Summary.FromToICHPriorYTD] &lt; 0"/>
            </Highlight>
          </TextObject>
          <LineObject Name="Line6" Left="718.2" Top="198.45" Width="-718.2"/>
          <TextObject Name="Text81" Top="207.9" Width="264.6" Height="18.9" Text="Net Available Funds :"/>
          <TextObject Name="Text82" Left="264.6" Top="207.9" Width="113.4" Height="18.9" Text="[[Executive Summary.CashAndBankThisMonth] + [Executive Summary.FromToICHThisMonth]]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
          <TextObject Name="Text83" Left="378" Top="207.9" Width="113.4" Height="18.9" Text="[[Executive Summary.CashAndBankActualYTD] + [Executive Summary.FromToICHActualYTD]]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
          <TextObject Name="Text84" Left="491.4" Top="207.9" Width="113.4" Height="18.9" Text="[[Executive Summary.CashAndBankBudgetYTD] + [Executive Summary.FromToICHBudgetYTD]]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
          <TextObject Name="Text85" Left="604.8" Top="207.9" Width="113.4" Height="18.9" Text="[[Executive Summary.CashAndBankPriorYTD] + [Executive Summary.FromToICHPriorYTD]]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
          <LineObject Name="Line7" Left="718.2" Top="236.25" Width="-718.2" Border.Width="2"/>
          <TextObject Name="Text86" Top="245.7" Width="264.6" Height="18.9" Text="Payments Due Within 1 Month :"/>
          <TextObject Name="Text36" Left="264.6" Top="245.7" Width="113.4" Height="18.9" Text="[Executive Summary.PaymentsDueThisMonth]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text87" Top="264.6" Width="264.6" Height="18.9" Text="Gifts Processed for Other Fields :"/>
          <TextObject Name="Text38" Left="264.6" Top="264.6" Width="113.4" Height="18.9" Text="[Executive Summary.GiftsForOtherFieldsThisMonth]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text40" Left="378" Top="264.6" Width="113.4" Height="18.9" Text="[Executive Summary.GiftsForOtherFieldsActualYTD]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text41" Left="491.4" Top="264.6" Width="113.4" Height="18.9" Text="[Executive Summary.GiftsForOtherFieldsBudgetYTD]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text50" Left="604.8" Top="264.6" Width="113.4" Height="18.9" Text="[Executive Summary.GiftsForOtherFieldsPriorYTD]" Format="Currency" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text88" Top="321.3" Width="302.4" Height="18.9" Text="Number of Personnel on Other Fields (Adults) :"/>
          <TextObject Name="Text51" Left="283.5" Top="321.3" Width="94.5" Height="18.9" Text="[Executive Summary.PersonnelOtherFieldsThisMonth]" HorzAlign="Right"/>
          <TextObject Name="Text52" Left="378" Top="321.3" Width="113.4" Height="18.9" Text="[Executive Summary.PersonnelOtherFieldsActualYTD]" HorzAlign="Right"/>
          <TextObject Name="Text53" Left="604.8" Top="321.3" Width="113.4" Height="18.9" Text="[Executive Summary.PersonnelOtherFieldsPriorYTD]" HorzAlign="Right"/>
        </DataBand>
        <GroupFooterBand Name="GroupFooter2" Top="492.5" Width="718.2"/>
      </GroupHeaderBand>
      <GroupFooterBand Name="GroupFooter1" Top="496.5" Width="718.2"/>
    </GroupHeaderBand>
    <PageFooterBand Name="PageFooter1" Top="500.5" Width="718.2" Height="18.9">
      <TextObject Name="Text44" Width="9.45" Height="18.9"/>
      <TextObject Name="Text45" Left="9.45" Width="9.45" Height="18.9"/>
      <TextObject Name="Text46" Left="18.9" Width="9.45" Height="18.9"/>
      <TextObject Name="Text47" Left="28.35" Width="9.45" Height="18.9"/>
      <TextObject Name="Text48" Left="37.8" Width="9.45" Height="18.9"/>
      <TextObject Name="Text49" Left="47.25" Width="9.45" Height="18.9"/>
      <TextObject Name="Text62" Left="557.55" Width="160.65" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;