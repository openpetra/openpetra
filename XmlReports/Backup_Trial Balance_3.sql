DELETE FROM s_report_template WHERE s_template_id_i=3;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(3,'Trial Balance','OpenPetra default template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="07/07/2014 12:17:24" ReportInfo.CreatorVersion="2014.2.1.0">
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
    
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="TrialBalance" ReferenceName="TrialBalance" DataType="System.Int32" Enabled="true">
      <Column Name="year" DataType="System.Int32"/>
      <Column Name="period" DataType="System.Int32"/>
      <Column Name="costcentrecode" DataType="System.String"/>
      <Column Name="costcentrename" DataType="System.String"/>
      <Column Name="isdebit" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="balance" DataType="System.Decimal"/>
      <Column Name="accountcode" DataType="System.String"/>
      <Column Name="accountname" DataType="System.String"/>
      <Column Name="debit" DataType="System.Decimal"/>
      <Column Name="credit" DataType="System.Decimal"/>
      <Column Name="costcentretype" DataType="System.String"/>
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
    <Parameter Name="param_account_code_start" DataType="System.String"/>
    <Parameter Name="param_account_code_end" DataType="System.String"/>
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
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
    <Total Name="GroupDebit" Expression="Debits.Value" Evaluator="list" PrintOn="GroupFooter1"/>
    <Total Name="GroupCredit" Expression="Credits.Value" Evaluator="list" PrintOn="GroupFooter1"/>
    <Total Name="OuterGroupDebit" Expression="Debits.Value" Evaluator="list" PrintOn="ReportSummary1"/>
    <Total Name="OuterGroupCredit" Expression="Credits.Value" Evaluator="list" PrintOn="ReportSummary1"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="85.05">
      <TextObject Name="Text1" Left="245.7" Width="207.9" Height="18.9" Text="Trial Balance" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="453.6" Width="103.95" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="557.55" Width="160.65" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Width="170.1" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="HeaderCostCentreList" Left="557.55" Top="18.9" Width="160.65" Height="18.9" Text="[param_cost_centre_list_title]" AutoShrink="FontSize" AutoShrinkMinSize="6"/>
      <TextObject Name="Text12" Left="453.6" Top="18.9" Width="103.95" Height="18.9" Text="Cost Centres :" HorzAlign="Right"/>
      <TextObject Name="Text14" Left="453.6" Top="37.8" Width="103.95" Height="18.9" Text="Accounts :" HorzAlign="Right"/>
      <TextObject Name="HeaderAccountsList" Left="557.55" Top="37.8" Width="160.65" Height="18.9" Text="[param_account_list_title]" AutoShrink="FontSize" AutoShrinkMinSize="6"/>
      <TextObject Name="Text21" Top="18.9" Width="75.6" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="75.6" Top="18.9" Width="170.1" Height="18.9" Text="[param_currency_name]"/>
      <TextObject Name="Text19" Left="75.6" Top="37.8" Width="170.1" Height="18.9" Text="[param_end_period_i]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text42" Left="245.7" Top="18.9" Width="207.9" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <LineObject Name="Line1" Left="718.2" Top="75.6" Width="-718.2"/>
      <TextObject Name="Text50" Left="453.6" Top="56.7" Width="103.95" Height="18.9" Text="Ordered By :" HorzAlign="Right"/>
      <TextObject Name="Text51" Left="557.55" Top="56.7" Width="160.65" Height="18.9" Text="[param_sortby]"/>
      <TextObject Name="Text56" Left="75.6" Top="56.7" Width="198.45" Height="18.9" Text="[OmDate([param_end_date])]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text43" Left="274.05" Top="56.7" Width="179.55" Height="18.9"/>
      <TextObject Name="Text22" Top="37.8" Width="75.6" Height="18.9" Text="Period :" HorzAlign="Right"/>
    </ReportTitleBand>
    <GroupHeaderBand Name="GroupHeader1" Top="88.38" Width="718.2" Height="18.9" Condition="IIf([param_sortby]==&quot;Account&quot;,[TrialBalance.accountcode],[TrialBalance.costcentrecode])" SortOrder="None">
      <TextObject Name="Text57" Width="236.25" Height="18.9" Text="[IIf(&quot;Account&quot;==[param_sortby],[TrialBalance.accountcode]+&quot; - &quot;+[TrialBalance.accountname],[TrialBalance.costcentrecode]+&quot; - &quot;+[TrialBalance.costcentrename])]" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt, style=Bold, Italic" Clip="false"/>
      <DataBand Name="list" Top="110.62" Width="718.2" Height="18.9" CanGrow="true" KeepChild="true" DataSource="TrialBalance" KeepDetail="true">
        <TextObject Name="Text30" Left="47.25" Width="132.3" Height="18.9" Text="[TrialBalance.costcentrecode] - [TrialBalance.accountcode]" AutoShrink="FontSize" AutoShrinkMinSize="7" Font="Arial, 9pt"/>
        <TextObject Name="TransRef" Left="179.55" Width="217.35" Height="18.9" Text="[IIf(&quot;Account&quot;==[param_sortby],[TrialBalance.costcentrename],[TrialBalance.accountname])]" AutoShrink="FontSize" AutoShrinkMinSize="7" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Clip="false"/>
        <TextObject Name="Debits" Left="396.9" Width="122.85" Height="18.9" Text="[TrialBalance.debit]" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
          <Highlight>
            <Condition Expression="Value == 0" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="Credits" Left="519.75" Width="122.85" Height="18.9" Text="[TrialBalance.credit]" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
          <Highlight>
            <Condition Expression="Value == 0" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
      </DataBand>
      <GroupFooterBand Name="GroupFooter1" Top="132.85" Width="718.2" Height="47.25" KeepChild="true" KeepWithData="true">
        <TextObject Name="Text27" Left="396.9" Top="18.9" Width="122.85" Height="18.9" Text="[ToDecimal([GroupDebit]-[GroupCredit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue">
          <Highlight>
            <Condition Expression="Value &lt;= 0" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="Text28" Left="519.75" Top="18.9" Width="122.85" Height="18.9" Text="[ToDecimal([GroupCredit]-[GroupDebit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue">
          <Highlight>
            <Condition Expression="Value &lt; 0" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="f36" Width="9.45" Height="18.9"/>
        <TextObject Name="f37" Left="9.45" Width="9.45" Height="18.9"/>
        <TextObject Name="Text16" Left="217.35" Width="179.55" Height="18.9" CanShrink="true" CanBreak="false" Text="[IIf(&quot;Account&quot;==[param_sortby],[TrialBalance.accountcode]+&quot; Total :&quot;, [TrialBalance.costcentrecode]+&quot; Total :&quot;)]" AutoShrink="FontSize" AutoShrinkMinSize="7" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Italic" TextFill.Color="Blue"/>
        <TextObject Name="OuterGroupDebitTotal" Left="396.9" Width="122.85" Height="18.9" Text="[GroupDebit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue"/>
        <TextObject Name="OuterGroupCreditTotal" Left="519.75" Width="122.85" Height="18.9" Text="[GroupCredit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue"/>
        <LineObject Name="Line3" Left="642.6" Top="36.8" Width="-245.7" Border.Color="Blue" Border.Width="1.5"/>
        <TextObject Name="f38" Top="18.9" Width="9.45" Height="18.9"/>
        <TextObject Name="f39" Left="9.45" Top="18.9" Width="9.45" Height="18.9"/>
        <TextObject Name="f29" Left="18.9" Top="18.9" Width="85.05" Height="18.9"/>
      </GroupFooterBand>
    </GroupHeaderBand>
    <ReportSummaryBand Name="ReportSummary1" Top="183.43" Width="718.2" Height="37.8">
      <TextObject Name="Text58" Left="396.9" Top="18.9" Width="122.85" Height="18.9" Text="[ToDecimal([OuterGroupDebit]-[OuterGroupCredit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue">
        <Highlight>
          <Condition Expression="Value &lt;= 0" TextFill.Color="White"/>
        </Highlight>
      </TextObject>
      <TextObject Name="Text59" Left="519.75" Top="18.9" Width="122.85" Height="18.9" Text="[ToDecimal([OuterGroupCredit]-[OuterGroupDebit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue">
        <Highlight>
          <Condition Expression="Value &lt; 0" TextFill.Color="White"/>
        </Highlight>
      </TextObject>
      <TextObject Name="Text60" Left="217.35" Width="179.55" Height="18.9" CanShrink="true" CanBreak="false" Text="[IIf(&quot;Account&quot;==[param_sortby],&quot;All Accounts Total :&quot;, &quot;All Cost Centres Total :&quot;)]" AutoShrink="FontSize" AutoShrinkMinSize="7" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Italic" TextFill.Color="Blue"/>
      <TextObject Name="Text61" Left="396.9" Width="122.85" Height="18.9" Text="[OuterGroupDebit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue"/>
      <TextObject Name="Text62" Left="519.75" Width="122.85" Height="18.9" Text="[OuterGroupCredit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue"/>
    </ReportSummaryBand>
    <PageFooterBand Name="PageFooter1" Top="224.57" Width="718.2" Height="18.9">
      <TextObject Name="Text44" Width="9.45" Height="18.9"/>
      <TextObject Name="Text45" Left="9.45" Width="9.45" Height="18.9"/>
      <TextObject Name="Text46" Left="18.9" Width="9.45" Height="18.9"/>
      <TextObject Name="Text47" Left="28.35" Width="9.45" Height="18.9"/>
      <TextObject Name="Text48" Left="37.8" Width="9.45" Height="18.9"/>
      <TextObject Name="Text49" Left="47.25" Width="47.25" Height="18.9"/>
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;