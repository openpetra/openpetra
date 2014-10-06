DELETE FROM s_report_template WHERE s_template_id_i=4;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(4,'Cost Centre Hierarchy','Cost Centre Hierarchy template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="07/07/2014 12:33:06" ReportInfo.CreatorVersion="2014.2.1.0">
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
      CCCode.Left = Units.Millimeters * (10 + (((Int32)Report.GetColumnValue(&quot;CostCentreHierarchy.CostCentreLevel&quot;)) * 5));
      CCName.Left = Units.Millimeters * (10 + (((Int32)Report.GetColumnValue(&quot;CostCentreHierarchy.CostCentreLevel&quot;)) * 5))
        + CCCode.Width;
    }
    
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="CostCentreHierarchy" ReferenceName="CostCentreHierarchy" DataType="System.Int32" Enabled="true">
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
      <Column Name="CostCentrePath" DataType="System.String"/>
      <Column Name="CostCentreLevel" DataType="System.Int32"/>
    </TableDataSource>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_ledger_nunmber" DataType="System.Int32"/>
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="37.8">
      <TextObject Name="Text1" Left="207.9" Width="255.15" Height="18.9" Text="Cost Centre Hierarchy" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="481.95" Width="75.6" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="557.55" Width="160.65" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Width="103.95" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text42" Left="236.25" Top="18.9" Width="207.9" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
    </ReportTitleBand>
    <DataBand Name="list" Top="41.13" Width="718.2" Height="15.12" CanGrow="true" BeforePrintEvent="Data_BeforePrint" KeepChild="true" DataSource="CostCentreHierarchy" KeepDetail="true">
      <TextObject Name="CCCode" Left="37.8" Width="122.85" Height="15.12" CanBreak="false" Text="[CostCentreHierarchy.a_cost_centre_code_c]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false" Font="Arial, 9pt">
        <Highlight>
          <Condition Expression="![CostCentreHierarchy.a_posting_cost_centre_flag_l]" Border.Lines="Bottom" Fill.Color="LightGray" ApplyFill="true" ApplyTextFill="false"/>
        </Highlight>
      </TextObject>
      <TextObject Name="CCName" Left="160.65" Width="274.05" Height="15.12" CanBreak="false" Text="[CostCentreHierarchy.a_cost_centre_name_c]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false" Font="Arial, 9pt">
        <Highlight>
          <Condition Expression="![CostCentreHierarchy.a_posting_cost_centre_flag_l]" Border.Lines="Bottom" Fill.Color="LightGray" ApplyFill="true" ApplyTextFill="false"/>
        </Highlight>
      </TextObject>
      <TextObject Name="Text2" Width="37.8" Height="15.12" Text="[CostCentreHierarchy.CostCentreLevel]" TextFill.Color="White"/>
    </DataBand>
    <PageFooterBand Name="PageFooter1" Top="59.59" Width="718.2" Height="18.9">
      <TextObject Name="Text17" Left="538.65" Width="179.55" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');

SELECT TRUE;