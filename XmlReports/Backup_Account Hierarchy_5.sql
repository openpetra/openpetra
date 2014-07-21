DELETE FROM s_report_template WHERE s_template_id_i=5;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(5,'Account Hierarchy','Account Hierarchy template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="07/21/2014 12:52:51" ReportInfo.CreatorVersion="2014.2.1.0">
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
    
    private void Data_BeforePrint(object sender, EventArgs e)
    {
      AccountCode.Left = Units.Millimeters * (10 +(((Int32)Report.GetColumnValue(&quot;AccountHierarchy.AccountLevel&quot;)) * 5));
      AccountName.Left = Units.Millimeters * (10 + (((Int32)Report.GetColumnValue(&quot;AccountHierarchy.AccountLevel&quot;)) * 5))
        + AccountCode.Width;
    }
    private String AnalysisCodes()
    {
      string OneCodePerRow = &quot;&quot;;
      OneCodePerRow += ((String)Report.GetColumnValue(&quot;AccountHierarchy.AnalysisAttribute.a_analysis_type_code_c&quot;));
      return OneCodePerRow;
    }
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="AccountHierarchy" ReferenceName="AccountHierarchy" DataType="System.Int32" Enabled="true">
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
      <Column Name="BankAccountFlag" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="CashAccountFlag" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="AccountPath" DataType="System.String"/>
      <Column Name="AccountLevel" DataType="System.Int32"/>
    </TableDataSource>
    <TableDataSource Name="AnalysisAttribute" ReferenceName="AnalysisAttribute" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_analysis_type_code_c" DataType="System.String"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_active_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <Relation Name="AnalysisAttribute_AccountHierarchy" ParentDataSource="AnalysisAttribute" ChildDataSource="AccountHierarchy" ParentColumns="a_account_code_c" ChildColumns="a_account_code_c" Enabled="true"/>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_ledger_nunmber" DataType="System.Int32"/>
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="37.8">
      <TextObject Name="Text1" Left="207.9" Width="255.15" Height="18.9" Text="Account Hierarchy" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="463.05" Width="94.5" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="557.55" Width="160.65" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Width="132.3" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text42" Left="207.9" Top="18.9" Width="255.15" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
    </ReportTitleBand>
    <DataBand Name="list" Top="41.13" Width="718.2" Height="15.12" CanGrow="true" BeforePrintEvent="Data_BeforePrint" KeepChild="true" DataSource="AccountHierarchy" PrintIfDetailEmpty="true" PrintIfDatasourceEmpty="true" KeepDetail="true">
      <TextObject Name="AccountCode" Left="37.8" Width="122.85" Height="15.12" CanBreak="false" Text="[AccountHierarchy.a_account_code_c]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false" Font="Arial, 9pt" Clip="false">
        <Highlight>
          <Condition Expression="![AccountHierarchy.a_posting_status_l]" Border.Lines="Bottom" Fill.Color="LightGray" ApplyFill="true" ApplyTextFill="false"/>
        </Highlight>
      </TextObject>
      <TextObject Name="AccountName" Left="160.65" Width="264.6" Height="15.12" CanBreak="false" Text="[AccountHierarchy.a_account_code_short_desc_c]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisWord" Clip="false">
        <Highlight>
          <Condition Expression="![AccountHierarchy.a_posting_status_l]" Border.Lines="Bottom" Fill.Color="LightGray" ApplyFill="true" ApplyTextFill="false"/>
        </Highlight>
      </TextObject>
      <TextObject Name="Text3" Width="37.8" Height="15.12" Text="[AccountHierarchy.AccountLevel]" TextFill.Color="White"/>
      <TextObject Name="Text4" Left="425.25" Width="207.9" Height="15.12" Font="Arial, 9pt" TextFill.Color="DarkGreen"/>
      <DataBand Name="Data1" Top="59.59" Width="718.2" Height="15.12" DataSource="AnalysisAttribute" Filter="[AccountHierarchy.a_account_code_c]==[AnalysisAttribute.a_account_code_c]">
        <TextObject Name="Text2" Left="425.25" Width="207.9" Height="15.12" Text="[AnalysisAttribute.a_analysis_type_code_c]" Font="Arial, 9pt" TextFill.Color="DarkGreen">
          <Highlight>
            <Condition Expression="![AnalysisAttribute.a_active_l]" TextFill.Color="DarkGray" Font="Arial, 9.75pt, style=Strikeout" ApplyFont="true"/>
          </Highlight>
        </TextObject>
        <TextObject Name="Text43" Left="160.65" Width="264.6" Height="15.12" Font="Arial, 9pt" TextFill.Color="DarkGreen"/>
        <TextObject Name="Text44" Left="37.8" Width="122.85" Height="15.12" Font="Arial, 9pt" TextFill.Color="DarkGreen"/>
      </DataBand>
    </DataBand>
    <PageFooterBand Name="PageFooter1" Top="78.04" Width="718.2" Height="18.9">
      <TextObject Name="Text17" Left="538.65" Width="179.55" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;