DELETE FROM s_report_template WHERE s_template_id_i=10;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(10,'Stewardship','OpenPetra default template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="11/19/2014 15:19:54" ReportInfo.CreatorVersion="2014.2.1.0">
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
    <TableDataSource Name="Stewardship" ReferenceName="Stewardship" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_period_number_i" DataType="System.Int32"/>
      <Column Name="a_ich_number_i" DataType="System.Int32"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_date_processed_d" DataType="System.DateTime"/>
      <Column Name="a_income_amount_n" DataType="System.Decimal"/>
      <Column Name="a_expense_amount_n" DataType="System.Decimal"/>
      <Column Name="a_direct_xfer_amount_n" DataType="System.Decimal"/>
      <Column Name="a_income_amount_intl_n" DataType="System.Decimal"/>
      <Column Name="a_expense_amount_intl_n" DataType="System.Decimal"/>
      <Column Name="a_direct_xfer_amount_intl_n" DataType="System.Decimal"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
      <Column Name="a_cost_centre_name_c" DataType="System.String"/>
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
    <Parameter Name="param_sortby" DataType="System.Int32"/>
    <Parameter Name="param_account_list_title" DataType="System.String"/>
    <Parameter Name="param_account_codes" DataType="System.String"/>
    <Parameter Name="param_account_code_start" DataType="System.String"/>
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
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
    <Parameter Name="param_current_financial_year" DataType="System.Boolean"/>
    <Parameter Name="param_requested_by" DataType="System.String"/>
    <Parameter Name="param_version" DataType="System.String"/>
    <Parameter Name="param_period_closed" DataType="System.Boolean"/>
    <Parameter Name="param_cmbYearEnding" DataType="System.Int32"/>
    <Parameter Name="param_cmbReportPeriod" DataType="System.Int32"/>
    <Parameter Name="param_cmbICHNumber" DataType="System.Int32"/>
    <Parameter Name="param_chkEmailHOSAReport" DataType="System.Boolean"/>
    <Parameter Name="param_chkHOSAReport" DataType="System.Boolean"/>
    <Parameter Name="param_chkHOSAFile" DataType="System.Boolean"/>
    <Parameter Name="param_txtHOSAPrefix" DataType="System.String"/>
    <Parameter Name="param_chkEmailHOSAFile" DataType="System.Boolean"/>
    <Parameter Name="param_chkEmailStewardshipFileAndReport" DataType="System.Boolean"/>
    <Parameter Name="param_chkStewardshipReport" DataType="System.Boolean"/>
    <Parameter Name="param_chkStewardshipFile" DataType="System.Boolean"/>
    <Parameter Name="param_chkFeesReport" DataType="System.Boolean"/>
    <Parameter Name="param_chkRecipientStatement" DataType="System.Boolean"/>
    <Total Name="Income" Expression="IIf([param_currency]==&quot;International&quot;,[Stewardship.a_income_amount_intl_n],[Stewardship.a_income_amount_n])" Evaluator="Data1" PrintOn="ReportSummary1"/>
    <Total Name="Expenses" Expression="IIf([param_currency]==&quot;International&quot;,[Stewardship.a_expense_amount_intl_n],[Stewardship.a_expense_amount_n])" Evaluator="Data1" PrintOn="ReportSummary1"/>
    <Total Name="Transfers" Expression="IIf([param_currency]==&quot;International&quot;,[Stewardship.a_direct_xfer_amount_intl_n],[Stewardship.a_direct_xfer_amount_n])" Evaluator="Data1" PrintOn="ReportSummary1"/>
    <Total Name="Total" Expression="IIf([param_currency]==&quot;International&quot;,[Stewardship.a_income_amount_intl_n]-[Stewardship.a_expense_amount_intl_n]-[Stewardship.a_direct_xfer_amount_intl_n],[Stewardship.a_income_amount_n]-[Stewardship.a_expense_amount_n]-[Stewardship.a_direct_xfer_amount_n])" Evaluator="Data1" PrintOn="ReportSummary1"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="56.7">
      <TextObject Name="Text1" Left="245.7" Width="207.9" Height="18.9" Text="ICH Stewardship Report" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="472.5" Width="103.95" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="576.45" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Width="170.1" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text21" Top="18.9" Width="75.6" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="75.6" Top="18.9" Width="170.1" Height="18.9" Text="[param_currency]"/>
      <TextObject Name="Text19" Left="75.6" Top="37.8" Width="170.1" Height="18.9" Text="[[param_cmbReportPeriod]]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text42" Left="245.7" Top="18.9" Width="207.9" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <TextObject Name="Text22" Top="37.8" Width="75.6" Height="18.9" Text="Period :" HorzAlign="Right"/>
      <TextObject Name="Text66" Left="472.5" Top="18.9" Width="103.95" Height="18.9" Text="Run Number :" HorzAlign="Right"/>
      <TextObject Name="Text68" Left="576.45" Top="18.9" Width="141.75" Height="18.9" Text="[param_cmbICHNumber]"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="59.05" Width="718.2" Height="28.35">
      <TextObject Name="Text2" Left="330.75" Width="122.85" Height="18.9" Text="Expenses &amp; Fees" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DarkBlue"/>
      <TextObject Name="Text63" Left="453.6" Width="122.85" Height="18.9" Text="Direct Transfers&#13;&#10;" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DarkBlue"/>
      <TextObject Name="Text64" Left="217.35" Width="113.4" Height="18.9" Text="Income" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DarkBlue"/>
      <TextObject Name="Text65" Left="18.9" Width="160.65" Height="18.9" Text="Foreign Cost Centre" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DarkBlue"/>
      <TextObject Name="Text67" Left="595.35" Width="122.85" Height="18.9" Text="Total&#13;&#10;" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DarkBlue"/>
      <LineObject Name="Line1" Left="718.2" Width="-699.3"/>
    </PageHeaderBand>
    <DataBand Name="Data1" Top="89.76" Width="718.2" Height="18.9" DataSource="Stewardship">
      <TextObject Name="Text6" Left="481.95" Width="94.5" Height="18.9" Text="[IIf([param_currency]==&quot;International&quot;,[Stewardship.a_direct_xfer_amount_intl_n],[Stewardship.a_direct_xfer_amount_n])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text3" Left="18.9" Width="207.9" Height="18.9" Text="[Stewardship.a_cost_centre_code_c] [Stewardship.a_cost_centre_name_c]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="7"/>
      <TextObject Name="Text4" Left="236.25" Width="94.5" Height="18.9" Text="[IIf([param_currency]==&quot;International&quot;,[Stewardship.a_income_amount_intl_n],[Stewardship.a_income_amount_n])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text5" Left="359.1" Width="94.5" Height="18.9" Text="[IIf([param_currency]==&quot;International&quot;,[Stewardship.a_expense_amount_intl_n],[Stewardship.a_expense_amount_n])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text7" Left="595.35" Width="122.85" Height="18.9" Text="[IIf([param_currency]==&quot;International&quot;,[Stewardship.a_income_amount_intl_n]-[Stewardship.a_expense_amount_intl_n]-[Stewardship.a_direct_xfer_amount_intl_n],[Stewardship.a_income_amount_n]-[Stewardship.a_expense_amount_n]-[Stewardship.a_direct_xfer_amount_n])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
    </DataBand>
    <ReportSummaryBand Name="ReportSummary1" Top="111.01" Width="718.2" Height="37.8">
      <TextObject Name="Text11" Left="595.35" Top="9.45" Width="122.85" Height="18.9" Text="[Total]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" TextFill.Color="DarkBlue"/>
      <TextObject Name="Text12" Left="481.95" Top="9.45" Width="94.5" Height="18.9" Text="[Transfers]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" TextFill.Color="DarkBlue"/>
      <TextObject Name="Text13" Left="359.1" Top="9.45" Width="94.5" Height="18.9" Text="[Expenses]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" TextFill.Color="DarkBlue"/>
      <TextObject Name="Text14" Left="236.25" Top="9.45" Width="94.5" Height="18.9" Text="[Income]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" TextFill.Color="DarkBlue"/>
      <LineObject Name="Line2" Left="18.9" Width="699.3" Border.Width="1.5"/>
      <TextObject Name="Text69" Left="18.9" Top="9.45" Width="160.65" Height="18.9" Text="Totals" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic" TextFill.Color="DarkBlue"/>
    </ReportSummaryBand>
    <PageFooterBand Name="PageFooter1" Top="151.16" Width="718.2" Height="18.9">
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;