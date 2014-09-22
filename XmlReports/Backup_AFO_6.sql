DELETE FROM s_report_template WHERE s_template_id_i=6;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(6,'AFO','AFO template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="08/29/2014 13:57:23" ReportInfo.CreatorVersion="2014.2.1.0">
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
    <TableDataSource Name="Accounts" ReferenceName="Accounts" DataType="System.Int32" Enabled="true">
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_account_code_short_desc_c" DataType="System.String"/>
      <Column Name="ActualDebitBase" DataType="System.Decimal"/>
      <Column Name="ActualCreditBase" DataType="System.Decimal"/>
      <Column Name="ActualDebitIntl" DataType="System.Decimal"/>
      <Column Name="ActualCreditIntl" DataType="System.Decimal"/>
      <Column Name="DebitCreditIndicator" DataType="System.Boolean" PropName="Column"/>
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
    <Total Name="AllActualDebitBase" Expression="[Accounts.ActualDebitBase]" Evaluator="Transaction" PrintOn="GroupFooter1" ResetOnReprint="true"/>
    <Total Name="AllActualCreditBase" Expression="[Accounts.ActualCreditBase]" Evaluator="Transaction" PrintOn="GroupFooter1" ResetOnReprint="true"/>
    <Total Name="AllActualDebitIntl" Expression="[Accounts.ActualDebitIntl]" Evaluator="Transaction" PrintOn="GroupFooter1" ResetOnReprint="true"/>
    <Total Name="AllActualCreditIntl" Expression="[Accounts.ActualCreditIntl]" Evaluator="Transaction" PrintOn="GroupFooter1" ResetOnReprint="true"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="103.95">
      <TextObject Name="Text1" Left="245.7" Width="207.9" Height="28.35" Text="AFO Report" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="491.4" Width="122.85" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="614.25" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Left="37.8" Top="56.7" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="113.4" Top="56.7" Width="170.1" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text22" Left="425.25" Top="56.7" Width="75.6" Height="18.9" Text="Period to:" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="500.85" Top="56.7" Width="217.35" Height="18.9" Text="[OmDate([param_end_date])] ([ToString([param_end_period_i])])[IIf([param_current_financial_year],IIf([param_current_period]&gt;[param_end_period_i],&quot; (CLOSED)&quot;,IIf([param_current_period]&lt;[param_end_period_i],&quot; (FWD PERIOD)&quot;,&quot; (CURRENT)&quot;)),&quot; (CLOSED)&quot;)]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text42" Left="245.7" Top="28.35" Width="207.9" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <LineObject Name="Line1" Left="718.2" Top="94.5" Width="-718.2" Border.Width="2"/>
      <TextObject Name="Text61" Top="75.6" Width="113.4" Height="18.9" Text="[IIf([param_current_financial_year],&quot;Current Period :&quot;,&quot;&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="113.4" Top="75.6" Width="94.5" Height="18.9" Text="[IIf([param_current_financial_year],[param_current_period],&quot;&quot;)]"/>
      <TextObject Name="Text21" Left="141.75" Top="18.9" Width="103.95" Height="18.9" Text="[param_requested_by]"/>
      <TextObject Name="Text26" Left="614.25" Top="18.9" Width="94.5" Height="18.9" Text="[param_version]"/>
      <TextObject Name="Text64" Top="18.9" Width="141.75" Height="18.9" Text="Report requested by :" HorzAlign="Right"/>
      <TextObject Name="Text65" Left="491.4" Top="18.9" Width="122.85" Height="18.9" Text="Version :" HorzAlign="Right"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="107.95" Width="718.2" Height="18.9">
      <TextObject Name="Text7" Left="378" Width="113.4" Height="18.9" Text="[param_base_currency] Credits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text12" Left="491.4" Width="113.4" Height="18.9" Text="[param_intl_currency] Debits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text14" Left="604.8" Width="113.4" Height="18.9" Text="[param_intl_currency] Credits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text5" Left="264.6" Width="113.4" Height="18.9" Text="[param_base_currency] Debits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text63" Width="66.15" Height="18.9" Text="Account" Font="Arial, 10pt, style=Bold, Italic"/>
    </PageHeaderBand>
    <GroupHeaderBand Name="GroupHeader1" Top="130.85" Width="718.2" Condition="[AllActualDebitBase]">
      <GroupHeaderBand Name="GroupHeader2" Top="134.85" Width="718.2" Condition="[Accounts.a_account_code_c]">
        <DataBand Name="Transaction" Top="138.85" Width="718.2" Height="18.9" CanGrow="true" KeepChild="true" DataSource="Accounts" KeepDetail="true">
          <TextObject Name="Text2" Width="66.15" Height="18.9" Text="[Accounts.a_account_code_c]"/>
          <TextObject Name="Text3" Left="66.15" Width="198.45" Height="18.9" CanGrow="true" Text="[Accounts.a_account_code_short_desc_c]"/>
          <TextObject Name="Text4" Left="264.6" Width="113.4" Height="18.9" Text="[IIf([Accounts.DebitCreditIndicator],[Accounts.ActualDebitBase],null)]" NullValue=" " Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text6" Left="378" Width="113.4" Height="18.9" Text="[IIf([Accounts.DebitCreditIndicator],null,[Accounts.ActualCreditBase])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text11" Left="491.4" Width="113.4" Height="18.9" Text="[IIf([Accounts.DebitCreditIndicator],[Accounts.ActualDebitIntl],null)]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text13" Left="604.8" Width="113.4" Height="18.9" Text="[IIf([Accounts.DebitCreditIndicator],null,[Accounts.ActualCreditIntl])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
        </DataBand>
        <GroupFooterBand Name="GroupFooter2" Top="161.75" Width="718.2"/>
      </GroupHeaderBand>
      <GroupFooterBand Name="GroupFooter1" Top="165.75" Width="718.2" Height="66.15">
        <TextObject Name="Text15" Left="264.6" Top="18.9" Width="113.4" Height="18.9" Text="[AllActualDebitBase]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold"/>
        <TextObject Name="Text16" Left="378" Top="18.9" Width="113.4" Height="18.9" Text="[AllActualCreditBase]" HorzAlign="Right" Font="Arial, 10pt, style=Bold">
          <Formats>
            <NumberFormat/>
            <NumberFormat/>
          </Formats>
        </TextObject>
        <TextObject Name="Text17" Left="491.4" Top="18.9" Width="113.4" Height="18.9" Text="[AllActualDebitIntl]" HorzAlign="Right" Font="Arial, 10pt, style=Bold">
          <Formats>
            <NumberFormat/>
            <NumberFormat/>
          </Formats>
        </TextObject>
        <TextObject Name="Text23" Left="604.8" Top="18.9" Width="113.4" Height="18.9" Text="[AllActualCreditIntl]" HorzAlign="Right" Font="Arial, 10pt, style=Bold">
          <Formats>
            <NumberFormat/>
            <NumberFormat/>
          </Formats>
        </TextObject>
        <LineObject Name="Line2" Left="718.2" Top="9.45" Width="-718.2" Border.Width="2"/>
        <TextObject Name="Text24" Left="170.1" Top="18.9" Width="94.5" Height="18.9" Text="Grand total :"/>
        <TextObject Name="Text57" Left="170.1" Top="47.25" Width="94.5" Height="18.9" Text="Net balance :"/>
        <TextObject Name="Text25" Left="264.6" Top="47.25" Width="113.4" Height="18.9" Text="[IIf([AllActualDebitBase] - [AllActualCreditBase] &gt; 0,[AllActualDebitBase] - [AllActualCreditBase],&quot;&quot;)]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Red"/>
        <TextObject Name="Text58" Left="378" Top="47.25" Width="113.4" Height="18.9" Text="[IIf([AllActualCreditBase] - [AllActualDebitBase] &gt;= 0,[AllActualCreditBase] - [AllActualDebitBase],&quot;&quot;)]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue"/>
        <TextObject Name="Text59" Left="491.4" Top="47.25" Width="113.4" Height="18.9" Text="[IIf([AllActualDebitIntl] - [AllActualCreditIntl] &gt; 0,[AllActualDebitIntl] - [AllActualCreditIntl],&quot;&quot;)]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Red"/>
        <TextObject Name="Text60" Left="604.8" Top="47.25" Width="113.4" Height="18.9" Text="[IIf([AllActualCreditIntl] - [AllActualDebitIntl] &gt;= 0,[AllActualCreditIntl] - [AllActualDebitIntl],&quot;&quot;)]" Format="Number" Format.UseLocale="true" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue"/>
      </GroupFooterBand>
    </GroupHeaderBand>
    <PageFooterBand Name="PageFooter1" Top="235.9" Width="718.2" Height="18.9">
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