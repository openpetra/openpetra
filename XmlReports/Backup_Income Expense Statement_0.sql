DELETE FROM s_report_template WHERE s_template_id_i=0;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(0,'Income Expense Statement','OpenPetra default template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="08/28/2014 16:12:43" ReportInfo.CreatorVersion="2014.2.1.0">
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
      Int32 AccountLevel = ((Int32)Report.GetColumnValue(&quot;IncomeExpense.accountlevel&quot;));
      Boolean ByPeriod = ((Boolean)Report.GetParameterValue(&quot;param_period_breakdown&quot;));
      HeaderBand.Visible = false;
      TransactionBand.Visible = false;
      FooterBand.Visible = false;
      FooterLevel1.Visible = false;
      PeriodBand.Visible = false;
      PeriodFooterBand.Visible = false;
      PeriodFooterLevel1.Visible = false;

      if(((Boolean)Report.GetColumnValue(&quot;IncomeExpense.haschildren&quot;)))
      {
        HeaderBand.Visible = (AccountLevel &gt; 0);
        HeaderAccountNameField.Left = Units.Millimeters * (AccountLevel * 4);
        float Width = HeaderAccountNameField.Width;
//      HeaderAccountNameField.Width = Width - Units.Millimeters * (AccountLevel * 4);
      }
      else
      {
        if(((Boolean)Report.GetColumnValue(&quot;IncomeExpense.parentfooter&quot;)))
        {
          if (ByPeriod)
          {
            PeriodFooterBand.Visible = (AccountLevel &gt; 1);
            PeriodFooterLevel1.Visible = (AccountLevel &lt;= 1);
            PeriodFooterAccountCodeField.Left = Units.Millimeters * (AccountLevel * 4);
            PeriodFooterAccountNameField.Left = Units.Millimeters * (15 + (AccountLevel * 4));
            float Width = PeriodFooterAccountNameField.Width;
//          PeriodFooterAccountNameField.Width = Width - Units.Millimeters * (AccountLevel * 4);
          }
          else
          {
            FooterBand.Visible = (AccountLevel &gt; 1);
            FooterLevel1.Visible = (AccountLevel &lt;= 1);
            FooterAccountCodeField.Left = Units.Millimeters * (AccountLevel * 4);
            FooterAccountNameField.Left = Units.Millimeters * (15 + (AccountLevel * 4));
            float Width = FooterAccountNameField.Width;
//          FooterAccountNameField.Width = Width - Units.Millimeters * (AccountLevel * 4);
          }
        }
        else
        {
          if (ByPeriod)
          {
            PeriodBand.Visible = true;
            PeriodAccountCodeField.Left = Units.Millimeters * (AccountLevel * 4);
            PeriodAccountNameField.Left = Units.Millimeters * (15 + (AccountLevel * 4));
            float Width = PeriodAccountNameField.Width;
//          PeriodAccountNameField.Width = Width - (Units.Millimeters * (AccountLevel * 4));
          }
          else
          {
            TransactionBand.Visible = true;
            AccountCodeField.Left = Units.Millimeters * (AccountLevel * 4);
            AccountNameField.Left = Units.Millimeters * (15 + (AccountLevel * 4));
            float Width = AccountNameField.Width;
//          AccountNameField.Width = Width - Units.Millimeters * (AccountLevel * 4);
            if ((Boolean)Report.GetColumnValue(&quot;IncomeExpense.breakdown&quot;))
            {
              AccountCodeField.Left += Units.Millimeters * 8;
              AccountNameField.Left += Units.Millimeters * 5;
            }
          }
        }
      }
    }

    private void TransactionPageHeader_BeforePrint(object sender, EventArgs e)
    {
      Boolean ByPeriod = ((Boolean)Report.GetParameterValue(&quot;param_period_breakdown&quot;));
      
      if (ByPeriod)
      {
        TransactionPageHeader.Visible = false;
      }
      else
      {
        PeriodPageHeader.Visible = false;
      }
    }

    // I should be able to achieve this using the expression in the Data Band itself, 
    // but after trying several different things I couldn''t make that work.
    // This C# solution works predictably.
    //
    // Returns the condition that the report engine uses to create a new group.
    private String CostCentreGroupCondition()
    {
      if(((Boolean)Report.GetParameterValue(&quot;param_cost_centre_breakdown&quot;))
        &amp;&amp; !((Boolean)Report.GetParameterValue(&quot;param_period_breakdown&quot;)))
      {
        return &quot;&quot;;  // For cost_centre_breakdown, don''t group by CostCentre.
      }
      else
      {
        return ((String)Report.GetColumnValue(&quot;IncomeExpense.costcentrecode&quot;));
      }
    }

    // Related to the method above, this shows or hides the Cost Centre group header.
    //
    // I should be able to achieve this using the expression in the Data Band itself, 
    // but after trying several different things I couldn''t make that work.
    // This C# solution works predictably.
    private void CostCentreGroup_BeforePrint(object sender, EventArgs e)
    {
      if(((Boolean)Report.GetParameterValue(&quot;param_cost_centre_breakdown&quot;))
        &amp;&amp; !((Boolean)Report.GetParameterValue(&quot;param_period_breakdown&quot;)))
      {
        CostCentreGroup.Visible = false;
      }
    }
    
    //
    // When the page starts, if there''s a linked_partner filter, I''ll set it
    // (And then only one Partner''s Cost Centre will be printed.)
    
    private void Page1_StartPage(object sender, EventArgs e)
    {
      CostCentreGroup.StartNewPage = ((Boolean)Report.GetParameterValue(&quot;param_paginate&quot;));
//    MessageBox.Show(&quot;Page1_StartPage&quot;);
      String LinkedPartner = (String)Report.GetParameterValue(&quot;param_linked_partner_cc&quot;);
      if (LinkedPartner != &quot;&quot;)
      {
        TransactionBand.Filter = &quot;[IncomeExpense.CostCentreCode]==[param_linked_partner_cc]&quot;;
        Report.SetParameterValue(&quot;param_cost_centre_list_title&quot;, LinkedPartner);
      }
    }
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="IncomeExpense" ReferenceName="IncomeExpense" DataType="System.Int32" Enabled="true">
      <Column Name="seq" DataType="System.Int32"/>
      <Column Name="accountlevel" DataType="System.Int32"/>
      <Column Name="accountpath" DataType="System.String"/>
      <Column Name="accounttype" DataType="System.String"/>
      <Column Name="year" DataType="System.Int32"/>
      <Column Name="period" DataType="System.Int32"/>
      <Column Name="costcentrecode" DataType="System.String"/>
      <Column Name="costcentrename" DataType="System.String"/>
      <Column Name="accountcode" DataType="System.String"/>
      <Column Name="accountname" DataType="System.String"/>
      <Column Name="yearstart" DataType="System.Decimal"/>
      <Column Name="actual" DataType="System.Decimal"/>
      <Column Name="actualytd" DataType="System.Decimal"/>
      <Column Name="actuallastyear" DataType="System.Decimal"/>
      <Column Name="budgetlastyear" DataType="System.Decimal"/>
      <Column Name="haschildren" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="parentfooter" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="wholeyearbudget" DataType="System.Decimal"/>
      <Column Name="accountissummary" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="breakdown" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="p1" DataType="System.Decimal"/>
      <Column Name="p2" DataType="System.Decimal"/>
      <Column Name="p3" DataType="System.Decimal"/>
      <Column Name="p4" DataType="System.Decimal"/>
      <Column Name="p5" DataType="System.Decimal"/>
      <Column Name="p6" DataType="System.Decimal"/>
      <Column Name="p7" DataType="System.Decimal"/>
      <Column Name="p8" DataType="System.Decimal"/>
      <Column Name="p9" DataType="System.Decimal"/>
      <Column Name="p10" DataType="System.Decimal"/>
      <Column Name="p11" DataType="System.Decimal"/>
      <Column Name="p12" DataType="System.Decimal"/>
      <Column Name="budget" DataType="System.Decimal"/>
      <Column Name="budgetytd" DataType="System.Decimal"/>
      <Column Name="accounttypeorder" DataType="System.Int32"/>
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
  </Dictionary>
  <ReportPage Name="Page1" Landscape="true" PaperWidth="297" PaperHeight="210" RawPaperSize="9" StartPageEvent="Page1_StartPage">
    <ReportTitleBand Name="ReportTitle1" Width="1047.06" Height="66.15">
      <TextObject Name="Text1" Left="368.55" Width="283.5" Height="18.9" Text="Income Expense Statement" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="831.6" Width="85.05" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="916.65" Width="122.85" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="94.5" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="94.5" Width="113.4" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text21" Top="18.9" Width="94.5" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="94.5" Top="18.9" Width="170.1" Height="18.9" Text="[param_currency_name]"/>
      <TextObject Name="Text22" Left="793.8" Top="18.9" Width="122.85" Height="18.9" Text="[IIf([param_period_breakdown],&quot;Year Ending:&quot;,IIf([param_period],&quot;Period :&quot;,&quot;Quarter:&quot;))]" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="916.65" Top="18.9" Width="122.85" Height="18.9" Text="[[param_real_year]+&quot; &quot;+IIf([param_period_breakdown], &quot;&quot;,&quot;:&quot;+IIf([param_period],IIf([param_end_period_i]&gt;[param_start_period_i],[param_start_period_i]+&quot;-&quot;+[param_end_period_i],[param_end_period_i]),[param_quarter]))]&#13;&#10;" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text42" Left="368.55" Top="18.9" Width="283.5" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <LineObject Name="Line1" Left="1039.5" Top="56.7" Width="-1039.5"/>
      <TextObject Name="Text50" Left="831.6" Top="37.8" Width="207.9" Height="18.9" Text="[OmDate([param_start_date])] - [OmDate([param_end_date])]" HorzAlign="Right"/>
      <TextObject Name="Text2" Left="94.5" Top="37.8" Width="557.55" Height="18.9" Text="[param_cost_centre_list_title][IIf([param_cost_centre_breakdown], &quot; with Cost Centre Breakdown&quot;,&quot;&quot;)]"/>
      <TextObject Name="Text51" Top="37.8" Width="94.5" Height="18.9" Text="Cost Centre :" HorzAlign="Right"/>
    </ReportTitleBand>
    <PageHeaderBand Name="TransactionPageHeader" Top="69.48" Width="1047.06" Height="18.9" BeforePrintEvent="TransactionPageHeader_BeforePrint">
      <TextObject Name="Text3" Left="113.4" Width="151.2" Height="18.9" Text="Description" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text5" Left="274.05" Width="94.5" Height="18.9" Text="Actual" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text23" Left="368.55" Width="94.5" Height="18.9" Text="Budget" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text41" Left="37.8" Width="56.7" Height="18.9" Text="Acc" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text43" Left="510.3" Width="94.5" Height="18.9" Text="Actual YTD" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text44" Left="756" Width="94.5" Height="18.9" Text="Year Budget" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text47" Left="463.05" Width="47.25" Height="18.9" Exportable="false" Text="Var.%" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text48" Left="699.3" Width="56.7" Height="18.9" Exportable="false" Text="Var.%" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text49" Left="604.8" Width="94.5" Height="18.9" Text="Budget YTD" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <ChildBand Name="PeriodPageHeader" Top="91.72" Width="1047.06" Height="18.9">
        <TextObject Name="Text74" Left="113.4" Width="151.2" Height="18.9" Text="Description" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text75" Left="37.8" Width="56.7" Height="18.9" Text="Acc" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text76" Left="368.55" Width="56.7" Height="18.9" Text="P1" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text77" Left="425.25" Width="56.7" Height="18.9" Text="P2" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text78" Left="481.95" Width="56.7" Height="18.9" Text="P3" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text79" Left="538.65" Width="56.7" Height="18.9" Text="P4" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text80" Left="595.35" Width="56.7" Height="18.9" Text="P5" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text81" Left="652.05" Width="56.7" Height="18.9" Text="P6" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text82" Left="708.75" Width="56.7" Height="18.9" Text="P7" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text83" Left="765.45" Width="56.7" Height="18.9" Text="P8" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text84" Left="822.15" Width="56.7" Height="18.9" Text="P9" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text85" Left="878.85" Width="56.7" Height="18.9" Text="P10" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text86" Left="935.55" Width="56.7" Height="18.9" Text="P11" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text87" Left="992.25" Width="56.7" Height="18.9" Text="P12" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      </ChildBand>
    </PageHeaderBand>
    <GroupHeaderBand Name="CostCentreGroup" Top="113.95" Width="1047.06" Height="18.9" BeforePrintEvent="CostCentreGroup_BeforePrint" StartNewPage="true" Condition="CostCentreGroupCondition()" SortOrder="None">
      <TextObject Name="CostCentreHeader" Width="245.7" Height="18.9" Border.Lines="Bottom" Border.Color="DarkBlue" Text="[IncomeExpense.costcentrecode] - [IncomeExpense.costcentrename]" Font="Arial, 10pt, style=Bold" TextFill.Color="DarkBlue"/>
      <GroupHeaderBand Name="AccountTypeGroup" Top="136.18" Width="1047.06" KeepChild="true" KeepWithData="true" Condition="[IncomeExpense.accounttype]" SortOrder="None">
        <DataBand Name="TransactionBand" Top="139.52" Width="1047.06" Height="18.9" CanGrow="true" BeforePrintEvent="Data_BeforePrint" KeepChild="true" DataSource="IncomeExpense">
          <TextObject Name="ActualField" Left="274.05" Width="94.5" Height="18.9" Text="[IncomeExpense.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="BudgetField" Left="368.55" Width="94.5" Height="18.9" Text="[IncomeExpense.budget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="ActualYTDField" Left="510.3" Width="94.5" Height="18.9" Text="[IncomeExpense.actualytd]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="BudgetYTDField" Left="604.8" Width="94.5" Height="18.9" Text="[IncomeExpense.budgetytd]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="VarianceField" Left="463.05" Width="47.25" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actual],[IncomeExpense.budget])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="VarianceYTDField" Left="699.3" Width="56.7" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actualytd],[IncomeExpense.budgetytd])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="YearBudgetField" Left="756" Width="94.5" Height="18.9" Text="[IncomeExpense.wholeyearbudget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="AccountCodeField" Left="45.36" Width="56.7" Height="18.9" Text="[IIf([param_cost_centre_breakdown],[IncomeExpense.costcentrecode],[IncomeExpense.accountcode])]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="AccountNameField" Left="102.06" Width="170.1" Height="18.9" CanGrow="true" Text="[IIf([param_cost_centre_breakdown],[IncomeExpense.costcentrename],[IncomeExpense.accountname])]" Padding="0, 0, 2, 0" Font="Arial, 9pt"/>
          <ChildBand Name="HeaderBand" Top="161.75" Width="1047.06" Height="18.9" Visible="false">
            <TextObject Name="HeaderAccountNameField" Left="30.24" Width="217.35" Height="18.9" Text="[IncomeExpense.accountname]:" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
            <ChildBand Name="FooterBand" Top="183.98" Width="1047.06" Height="18.9" Visible="false">
              <TextObject Name="FooterActualField" Left="274.05" Width="94.5" Height="18.9" Text="[IncomeExpense.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
              <TextObject Name="FooterBudgetField" Left="368.55" Width="94.5" Height="18.9" Text="[IncomeExpense.budget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
              <TextObject Name="FooterActualYTDField" Left="510.3" Width="94.5" Height="18.9" Text="[IncomeExpense.actualytd]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
              <TextObject Name="FooterBudgetYTDField" Left="604.8" Width="94.5" Height="18.9" Text="[IncomeExpense.budgetytd]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
              <TextObject Name="FooterVarianceField" Left="463.05" Width="47.25" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actual],[IncomeExpense.budget])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray"/>
              <TextObject Name="FooterVarianceYTDField" Left="699.3" Width="56.7" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actualytd],[IncomeExpense.budgetytd])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray"/>
              <TextObject Name="FooterYearBudgetField" Left="756" Width="94.5" Height="18.9" Text="[IncomeExpense.wholeyearbudget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
              <TextObject Name="FooterAccountNameField" Left="56.7" Width="189" Height="18.9" Text="Total [IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
              <TextObject Name="FooterAccountCodeField" Width="56.7" Height="18.9" Text="[IncomeExpense.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray"/>
              <ChildBand Name="FooterLevel1" Top="206.22" Width="1047.06" Height="28.35">
                <TextObject Name="Level1ActualField" Left="274.05" Width="94.5" Height="18.9" Text="[IncomeExpense.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1BudgetField" Left="368.55" Width="94.5" Height="18.9" Text="[IncomeExpense.budget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1ActualYTDField" Left="510.3" Width="94.5" Height="18.9" Text="[IncomeExpense.actualytd]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1BudgetYTDField" Left="604.8" Width="94.5" Height="18.9" Text="[IncomeExpense.budgetytd]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1VarianceField" Left="463.05" Width="47.25" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actual],[IncomeExpense.budget])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold"/>
                <TextObject Name="Level1VarianceYTDField" Left="699.3" Width="56.7" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actualytd],[IncomeExpense.budgetytd])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold"/>
                <TextObject Name="Level1YearBudgetField" Left="756" Width="94.5" Height="18.9" Text="[IncomeExpense.wholeyearbudget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1AccountNameField" Width="207.9" Height="18.9" Text="[IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <LineObject Name="Line2" Left="274.05" Top="18.9" Width="576.45" Border.Width="1.5"/>
                <ChildBand Name="PeriodBand" Top="237.9" Width="1047.06" Height="18.9">
                  <TextObject Name="PeriodAccountNameField" Left="56.7" Width="217.35" Height="18.9" Text="Total [IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                  <TextObject Name="PeriodAccountCodeField" Width="56.7" Height="18.9" Text="[IncomeExpense.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt"/>
                  <TextObject Name="P1" Left="368.55" Width="56.7" Height="18.9" Text="[IncomeExpense.p1]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P2" Left="425.25" Width="56.7" Height="18.9" Text="[IncomeExpense.p2]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P3" Left="481.95" Width="56.7" Height="18.9" Text="[IncomeExpense.p3]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P4" Left="538.65" Width="56.7" Height="18.9" Text="[IncomeExpense.p4]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P5" Left="595.35" Width="56.7" Height="18.9" Text="[IncomeExpense.p5]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P6" Left="652.05" Width="56.7" Height="18.9" Text="[IncomeExpense.p6]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P7" Left="708.75" Width="56.7" Height="18.9" Text="[IncomeExpense.p7]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P8" Left="765.45" Width="56.7" Height="18.9" Text="[IncomeExpense.p8]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P9" Left="822.15" Width="56.7" Height="18.9" Text="[IncomeExpense.p9]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P10" Left="878.85" Width="56.7" Height="18.9" Text="[IncomeExpense.p10]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P11" Left="935.55" Width="56.7" Height="18.9" Text="[IncomeExpense.p11]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P12" Left="992.25" Width="56.7" Height="18.9" Text="[IncomeExpense.p12]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                  <ChildBand Name="PeriodFooterBand" Top="260.13" Width="1047.06" Height="18.9">
                    <TextObject Name="PeriodFooterAccountNameField" Left="56.7" Width="217.35" Height="18.9" Text="Total [IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="PeriodFooterAccountCodeField" Width="56.7" Height="18.9" Text="[IncomeExpense.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray"/>
                    <TextObject Name="Text62" Left="368.55" Width="56.7" Height="18.9" Text="[IncomeExpense.p1]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text63" Left="425.25" Width="56.7" Height="18.9" Text="[IncomeExpense.p2]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text64" Left="481.95" Width="56.7" Height="18.9" Text="[IncomeExpense.p3]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text65" Left="538.65" Width="56.7" Height="18.9" Text="[IncomeExpense.p4]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text66" Left="595.35" Width="56.7" Height="18.9" Text="[IncomeExpense.p5]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text67" Left="652.05" Width="56.7" Height="18.9" Text="[IncomeExpense.p6]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text68" Left="708.75" Width="56.7" Height="18.9" Text="[IncomeExpense.p7]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text69" Left="765.45" Width="56.7" Height="18.9" Text="[IncomeExpense.p8]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text70" Left="822.15" Width="56.7" Height="18.9" Text="[IncomeExpense.p9]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text71" Left="878.85" Width="56.7" Height="18.9" Text="[IncomeExpense.p10]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text72" Left="935.55" Width="56.7" Height="18.9" Text="[IncomeExpense.p11]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text73" Left="992.25" Width="56.7" Height="18.9" Text="[IncomeExpense.p12]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <ChildBand Name="PeriodFooterLevel1" Top="282.37" Width="1047.06" Height="28.35">
                      <TextObject Name="PeriodLevel1AccountNameField" Width="217.35" Height="18.9" Text="[IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text88" Left="368.55" Width="56.7" Height="18.9" Text="[IncomeExpense.p1]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text89" Left="425.25" Width="56.7" Height="18.9" Text="[IncomeExpense.p2]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text90" Left="481.95" Width="56.7" Height="18.9" Text="[IncomeExpense.p3]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text91" Left="538.65" Width="56.7" Height="18.9" Text="[IncomeExpense.p4]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text92" Left="595.35" Width="56.7" Height="18.9" Text="[IncomeExpense.p5]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text93" Left="652.05" Width="56.7" Height="18.9" Text="[IncomeExpense.p6]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text94" Left="708.75" Width="56.7" Height="18.9" Text="[IncomeExpense.p7]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text95" Left="765.45" Width="56.7" Height="18.9" Text="[IncomeExpense.p8]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text96" Left="822.15" Width="56.7" Height="18.9" Text="[IncomeExpense.p9]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text97" Left="878.85" Width="56.7" Height="18.9" Text="[IncomeExpense.p10]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text98" Left="935.55" Width="56.7" Height="18.9" Text="[IncomeExpense.p11]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text99" Left="992.25" Width="56.7" Height="18.9" Text="[IncomeExpense.p12]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
                      <LineObject Name="Line3" Left="264.6" Top="18.9" Width="784.35" Border.Width="1.5"/>
                    </ChildBand>
                  </ChildBand>
                </ChildBand>
              </ChildBand>
            </ChildBand>
          </ChildBand>
        </DataBand>
        <GroupFooterBand Name="GroupFooter1" Top="314.05" Width="1047.06" Height="9.45" KeepChild="true" KeepWithData="true"/>
      </GroupHeaderBand>
      <GroupFooterBand Name="GroupFooter3" Top="326.83" Width="1047.06"/>
    </GroupHeaderBand>
    <PageFooterBand Name="PageFooter1" Top="330.17" Width="1047.06" Height="18.9">
      <TextObject Name="Text17" Left="888.3" Width="160.65" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;