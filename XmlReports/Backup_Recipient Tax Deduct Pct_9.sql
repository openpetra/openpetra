DELETE FROM s_report_template WHERE s_template_id_i=9;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_private_l,s_private_default_l,s_xml_text_c)
VALUES(9,'Recipient Tax Deduct Pct','OpenPetra default template','System',True,False,False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="10/03/2014 15:58:13" ReportInfo.CreatorVersion="2014.2.1.0">
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
    <TableDataSource Name="RecipientTaxDeductPct" ReferenceName="RecipientTaxDeductPct" DataType="System.Int32" Enabled="true">
      <Column Name="p_partner_short_name_c" DataType="System.String"/>
      <Column Name="p_partner_key_n" DataType="System.Int64"/>
      <Column Name="p_date_valid_from_d" DataType="System.DateTime"/>
      <Column Name="p_percentage_tax_deductible_n" DataType="System.Decimal"/>
      <Column Name="p_field_key_n" DataType="System.Int64"/>
      <Column Name="um_parent_unit_key_n" DataType="System.Int64"/>
    </TableDataSource>
    <Parameter Name="param_recipient_selection" DataType="System.String"/>
    <Parameter Name="param_recipient_key" DataType="System.Int32"/>
    <Parameter Name="param_extract_name" DataType="System.String"/>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_design_template" DataType="System.Boolean"/>
    <Parameter Name="param_requested_by" DataType="System.String"/>
    <Parameter Name="param_version" DataType="System.String"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_currency_formatter" DataType="System.String"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="94.5">
      <TextObject Name="Text1" Left="207.9" Width="302.4" Height="47.25" Text="Recipient Tax Deductible Percentages" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="548.1" Width="66.15" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="614.25" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <LineObject Name="Line1" Left="718.2" Top="85.05" Width="-718.2" Border.Width="2"/>
      <TextObject Name="Text21" Left="141.75" Top="18.9" Width="103.95" Height="18.9" Text="[param_requested_by]"/>
      <TextObject Name="Text26" Left="614.25" Top="18.9" Width="94.5" Height="18.9" Text="[param_version]"/>
      <TextObject Name="Text64" Top="18.9" Width="141.75" Height="18.9" Text="Report requested by :" HorzAlign="Right"/>
      <TextObject Name="Text65" Left="491.4" Top="18.9" Width="122.85" Height="18.9" Text="Version :" HorzAlign="Right"/>
      <TextObject Name="Text10" Top="56.7" Width="718.2" Height="18.9" Text="********** CONFIDENTIAL **********" HorzAlign="Center" Font="Arial, 10pt, style=Bold"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="98.5" Width="718.2" Height="18.9">
      <TextObject Name="Text7" Left="387.45" Width="94.5" Height="18.9" Text="Field" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text12" Left="481.95" Width="122.85" Height="18.9" Text="Tax Deductible %" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text14" Left="604.8" Width="113.4" Height="18.9" Text="Date Valid From" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text5" Left="103.95" Width="283.5" Height="18.9" Text="Recipient Name" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text63" Width="103.95" Height="18.9" Text="Recipient Key" Font="Arial, 10pt, style=Bold, Italic"/>
    </PageHeaderBand>
    <GroupHeaderBand Name="GroupHeader2" Top="121.4" Width="718.2" Condition="[RecipientTaxDeductPct.p_percentage_tax_deductible_n]">
      <DataBand Name="Transaction" Top="125.4" Width="718.2" Height="28.35" CanGrow="true" KeepChild="true" DataSource="RecipientTaxDeductPct" KeepDetail="true">
        <TextObject Name="Text2" Width="103.95" Height="18.9" Text="[RecipientTaxDeductPct.p_partner_key_n]" Format="Custom" Format.Format="0000000000"/>
        <TextObject Name="Text3" Left="103.95" Width="283.5" Height="18.9" CanGrow="true" Text="[RecipientTaxDeductPct.p_partner_short_name_c]"/>
        <TextObject Name="Text4" Left="387.45" Width="94.5" Height="18.9" Text="[IIf([RecipientTaxDeductPct.p_field_key_n] != 0,[RecipientTaxDeductPct.p_field_key_n],IIf([RecipientTaxDeductPct.um_parent_unit_key_n] != 0,[RecipientTaxDeductPct.um_parent_unit_key_n],''?''))]" NullValue=" " Format="Custom" Format.Format="0000000000" WordWrap="false" Trimming="EllipsisCharacter"/>
        <TextObject Name="Text6" Left="481.95" Width="122.85" Height="18.9" Text="[RecipientTaxDeductPct.p_percentage_tax_deductible_n]" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
        <TextObject Name="Text11" Left="604.8" Width="113.4" Height="18.9" Text="[RecipientTaxDeductPct.p_date_valid_from_d]" Format="Date" Format.Format="d" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
      </DataBand>
      <GroupFooterBand Name="GroupFooter2" Top="157.75" Width="718.2"/>
    </GroupHeaderBand>
    <PageFooterBand Name="PageFooter1" Top="161.75" Width="718.2" Height="18.9">
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