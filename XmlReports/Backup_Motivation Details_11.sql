DELETE FROM s_report_template WHERE s_template_id_i=11;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(11,'Motivation Details','Motivation Details template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="11/24/2014 11:51:43" ReportInfo.CreatorVersion="2014.2.1.0">
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
    <TableDataSource Name="MotivationDetail" ReferenceName="MotivationDetail" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_motivation_group_code_c" DataType="System.String"/>
      <Column Name="a_motivation_detail_code_c" DataType="System.String"/>
      <Column Name="a_motivation_detail_audience_c" DataType="System.String"/>
      <Column Name="a_motivation_detail_desc_c" DataType="System.String"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_motivation_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_mailing_cost_n" DataType="System.Decimal"/>
      <Column Name="a_bulk_rate_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_next_response_status_c" DataType="System.String"/>
      <Column Name="a_activate_partner_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_number_sent_i" DataType="System.Int32"/>
      <Column Name="a_number_of_responses_i" DataType="System.Int32"/>
      <Column Name="a_target_number_of_responses_i" DataType="System.Int32"/>
      <Column Name="a_target_amount_n" DataType="System.Decimal"/>
      <Column Name="a_amount_received_n" DataType="System.Decimal"/>
      <Column Name="p_recipient_key_n" DataType="System.Int64"/>
      <Column Name="a_autopopdesc_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_receipt_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_tax_deductible_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_motivation_detail_desc_local_c" DataType="System.String"/>
      <Column Name="a_short_code_c" DataType="System.String"/>
      <Column Name="a_restricted_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_export_to_intranet_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_report_column_c" DataType="System.String"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
      <Column Name="a_tax_deductible_account_c" DataType="System.String"/>
    </TableDataSource>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_ledger_nunmber" DataType="System.Int32"/>
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
    <Parameter Name="param_requested_by" DataType="System.String"/>
    <Parameter Name="param_version" DataType="System.String"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="37.8">
      <TextObject Name="Text1" Left="207.9" Width="255.15" Height="18.9" Text="Gift Motivations" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="463.05" Width="94.5" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="557.55" Width="160.65" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Width="132.3" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text42" Left="207.9" Top="18.9" Width="255.15" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="41.13" Width="718.2" Height="18.9">
      <TextObject Name="Text7" Width="85.05" Height="18.9" Text="Group" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text43" Left="170.1" Width="170.1" Height="18.9" Text="Description" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text44" Left="463.05" Width="75.6" Height="18.9" Text="Account" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text45" Left="538.65" Width="103.95" Height="18.9" Text="Cost Centre" Font="Arial, 10pt, style=Bold"/>
      <TextObject Name="Text46" Left="85.05" Width="85.05" Height="18.9" Text="Code" Font="Arial, 10pt, style=Bold"/>
    </PageHeaderBand>
    <DataBand Name="list" Top="63.37" Width="718.2" Height="18.9" CanGrow="true" KeepChild="true" DataSource="MotivationDetail" PrintIfDetailEmpty="true" PrintIfDatasourceEmpty="true" KeepDetail="true">
      <TextObject Name="Text2" Width="85.05" Height="18.9" Text="[MotivationDetail.a_motivation_group_code_c]">
        <Highlight>
          <Condition Expression="![MotivationDetail.a_motivation_status_l]" TextFill.Color="Gray"/>
        </Highlight>
      </TextObject>
      <TextObject Name="Text3" Left="85.05" Width="85.05" Height="18.9" Text="[MotivationDetail.a_motivation_detail_code_c]">
        <Highlight>
          <Condition Expression="![MotivationDetail.a_motivation_status_l]" TextFill.Color="Gray"/>
        </Highlight>
      </TextObject>
      <TextObject Name="Text4" Left="170.1" Width="292.95" Height="18.9" Text="[MotivationDetail.a_motivation_detail_desc_c]">
        <Highlight>
          <Condition Expression="![MotivationDetail.a_motivation_status_l]" TextFill.Color="Gray"/>
        </Highlight>
      </TextObject>
      <TextObject Name="Text5" Left="463.05" Width="75.6" Height="18.9" Text="[MotivationDetail.a_account_code_c]">
        <Highlight>
          <Condition Expression="![MotivationDetail.a_motivation_status_l]" TextFill.Color="Gray"/>
        </Highlight>
      </TextObject>
      <TextObject Name="Text6" Left="538.65" Width="103.95" Height="18.9" Text="[MotivationDetail.a_cost_centre_code_c]">
        <Highlight>
          <Condition Expression="![MotivationDetail.a_motivation_status_l]" TextFill.Color="Gray"/>
        </Highlight>
      </TextObject>
    </DataBand>
    <PageFooterBand Name="PageFooter1" Top="85.6" Width="718.2" Height="18.9">
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;