DELETE FROM s_report_template;
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(4,'Cost Centre Hierarchy','Cost Centre Hierarchy template','System',True,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="06/30/2014 15:35:41" ReportInfo.CreatorVersion="2014.2.1.0">
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
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(5,'Account Hierarchy','Account Hierarchy template','System',True,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="06/30/2014 12:24:45" ReportInfo.CreatorVersion="2014.2.1.0">
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
        <TextObject Name="Text2" Left="425.25" Width="207.9" Height="15.12" Text="[AnalysisAttribute.a_analysis_type_code_c]" Font="Arial, 9pt" TextFill.Color="DarkGreen"/>
        <TextObject Name="Text43" Left="160.65" Width="264.6" Height="15.12" Font="Arial, 9pt" TextFill.Color="DarkGreen"/>
        <TextObject Name="Text44" Left="37.8" Width="122.85" Height="15.12" Font="Arial, 9pt" TextFill.Color="DarkGreen"/>
      </DataBand>
    </DataBand>
    <PageFooterBand Name="PageFooter1" Top="78.04" Width="718.2" Height="18.9">
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(26,'Trial Balance','Copy of OpenPetra default template','DEMO',False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="06/27/2014 09:08:33" ReportInfo.CreatorVersion="2014.2.1.0">
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
      <TextObject Name="Text49" Left="47.25" Width="47.25" Height="18.9" Text="TW"/>
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(19,'Balance Sheet Standard','Copy of OpenPetra default template','DEMO',False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="03/25/2014 14:38:31" ReportInfo.CreatorVersion="2013.4.4.0">
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
    <Total Name="TotalLiabPlusEq" Expression="[BalanceSheet.actual]" Evaluator="TransactionBand" EvaluateCondition="[BalanceSheet.accountlevel]==1 &amp;&amp; ([BalanceSheet.accounttype]==&quot;Liability&quot;||[BalanceSheet.accounttype]==&quot;Equity&quot;)&amp;&amp;[BalanceSheet.parentfooter]"/>
    <Total Name="TotalLiabPlusEqLastYear" Expression="[BalanceSheet.actuallastyear]" Evaluator="TransactionBand" EvaluateCondition="[BalanceSheet.accountlevel]==1 &amp;&amp; ([BalanceSheet.accounttype]==&quot;Liability&quot;||[BalanceSheet.accounttype]==&quot;Equity&quot;)&amp;&amp;[BalanceSheet.parentfooter]"/>
  </Dictionary>
  <ReportPage Name="Page1" RawPaperSize="9">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="66.15">
      <TextObject Name="Text1" Left="245.7" Width="226.8" Height="18.9" Text="Balance Sheet" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="510.3" Width="85.05" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="595.35" Width="122.85" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="94.5" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="94.5" Width="113.4" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text21" Top="18.9" Width="94.5" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="94.5" Top="18.9" Width="141.75" Height="18.9" Text="[param_currency_name]"/>
      <TextObject Name="Text22" Left="510.3" Top="18.9" Width="85.05" Height="18.9" Text="Balance at " HorzAlign="Right"/>
      <TextObject Name="Text42" Left="245.7" Top="18.9" Width="226.8" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <LineObject Name="Line1" Left="718.2" Top="56.7" Width="-718.2"/>
      <TextObject Name="Text50" Left="595.35" Top="18.9" Width="207.9" Height="18.9" Text="[OmDate([param_end_date])]"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="69.48" Width="718.2" Height="18.9">
      <TextObject Name="Text3" Left="113.4" Width="151.2" Height="18.9" Text="Description" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text5" Left="349.65" Width="103.95" Height="18.9" Text="Actual" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text41" Left="37.8" Width="56.7" Height="18.9" Text="Acc" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text43" Left="463.05" Width="122.85" Height="18.9" Text="Actual Last Year" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
    </PageHeaderBand>
    <DataBand Name="TransactionBand" Top="91.72" Width="718.2" Height="18.9" BeforePrintEvent="Data_BeforePrint" KeepChild="true" DataSource="BalanceSheet">
      <TextObject Name="ActualField" Left="340.2" Width="113.4" Height="18.9" Text="[BalanceSheet.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
      <TextObject Name="ActualYTDField" Left="472.5" Width="113.4" Height="18.9" Text="[BalanceSheet.actuallastyear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
      <TextObject Name="AccountCodeField" Left="45.36" Width="56.7" Height="18.9" Text="[BalanceSheet.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt"/>
      <TextObject Name="AccountNameField" Left="102.06" Width="236.25" Height="18.9" Text="[BalanceSheet.accountname]" Padding="0, 0, 2, 0" Font="Arial, 9pt"/>
      <ChildBand Name="HeaderBand" Top="113.95" Width="718.2" Height="18.9" Visible="false">
        <TextObject Name="HeaderAccountNameField" Left="30.24" Width="217.35" Height="18.9" Text="[BalanceSheet.accountname]:" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
        <ChildBand Name="FooterBand" Top="136.18" Width="718.2" Height="18.9" Visible="false">
          <TextObject Name="FooterActualField" Left="340.2" Width="113.4" Height="18.9" Text="[BalanceSheet.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
          <TextObject Name="FooterActualYTDField" Left="472.5" Width="113.4" Height="18.9" Text="[BalanceSheet.actuallastyear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
          <TextObject Name="FooterAccountNameField" Left="56.7" Width="274.05" Height="18.9" Text="Total [BalanceSheet.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
          <TextObject Name="FooterAccountCodeField" Width="56.7" Height="18.9" Text="[BalanceSheet.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray"/>
          <ChildBand Name="FooterLevel1" Top="158.42" Width="718.2" Height="37.8">
            <TextObject Name="Level1ActualField" Left="340.2" Width="113.4" Height="18.9" Text="[BalanceSheet.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold, Underline" Trimming="EllipsisCharacter"/>
            <TextObject Name="Level1ActualYTDField" Left="472.5" Width="113.4" Height="18.9" Text="[BalanceSheet.actuallastyear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold, Underline" Trimming="EllipsisCharacter"/>
            <TextObject Name="Level1AccountNameField" Width="207.9" Height="18.9" Text="[BalanceSheet.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
          </ChildBand>
        </ChildBand>
      </ChildBand>
    </DataBand>
    <ReportSummaryBand Name="ReportSummary1" Top="199.55" Width="718.2" Height="18.9">
      <TextObject Name="Text2" Left="340.2" Width="113.4" Height="18.9" Text="[TotalLiabPlusEq]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text52" Width="207.9" Height="18.9" Text="Total Equity + Liabilities" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text4" Left="472.5" Width="113.4" Height="18.9" Text="[TotalLiabPlusEqLastYear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
    </ReportSummaryBand>
    <PageFooterBand Name="PageFooter1" Top="221.78" Width="718.2" Height="18.9">
      <TextObject Name="Text17" Left="557.55" Width="160.65" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(20,'Income Expense Statement','Copy of OpenPetra default template','DEMO',False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="06/27/2014 09:19:06" ReportInfo.CreatorVersion="2014.2.1.0">
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
          }
          else
          {
            FooterBand.Visible = (AccountLevel &gt; 1);
            FooterLevel1.Visible = (AccountLevel &lt;= 1);
            FooterAccountCodeField.Left = Units.Millimeters * (AccountLevel * 4);
            FooterAccountNameField.Left = Units.Millimeters * (15 + (AccountLevel * 4));
          }
        }
        else
        {
          if (ByPeriod)
          {
            PeriodBand.Visible = true;
            PeriodAccountCodeField.Left = Units.Millimeters * (AccountLevel * 4);
            PeriodAccountNameField.Left = Units.Millimeters * (15 + (AccountLevel * 4));
          }
          else
          {
            TransactionBand.Visible = true;
            AccountCodeField.Left = Units.Millimeters * (AccountLevel * 4);
            AccountNameField.Left = Units.Millimeters * (15 + (AccountLevel * 4));
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
  </Dictionary>
  <ReportPage Name="Page1" Landscape="true" PaperWidth="297" PaperHeight="210" RawPaperSize="9">
    <ReportTitleBand Name="ReportTitle1" Width="1047.06" Height="66.15">
      <TextObject Name="Text1" Left="368.55" Width="283.5" Height="18.9" Text="Income Expense Statement" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="831.6" Width="85.05" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="916.65" Width="122.85" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="94.5" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="94.5" Width="113.4" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text21" Top="18.9" Width="94.5" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="94.5" Top="18.9" Width="170.1" Height="18.9" Text="[param_currency_name]"/>
      <TextObject Name="Text22" Left="831.6" Top="18.9" Width="85.05" Height="18.9" Text="[IIf([param_period],&quot; Period :&quot;,&quot; Quarter:&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="916.65" Top="18.9" Width="122.85" Height="18.9" Text="[[param_real_year]+&quot;:&quot;+IIf([param_period],IIf([param_end_period_i]&gt;[param_start_period_i],[param_start_period_i]+&quot;-&quot;+[param_end_period_i],[param_end_period_i]),[param_quarter])]&#13;&#10;" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
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
        <TextObject Name="Text76" Left="264.6" Width="56.7" Height="18.9" Text="P1" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text77" Left="330.75" Width="56.7" Height="18.9" Text="P2" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text78" Left="396.9" Width="56.7" Height="18.9" Text="P3" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text79" Left="463.05" Width="56.7" Height="18.9" Text="P4" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text80" Left="529.2" Width="56.7" Height="18.9" Text="P5" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text81" Left="595.35" Width="56.7" Height="18.9" Text="P6" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text82" Left="661.5" Width="56.7" Height="18.9" Text="P7" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text83" Left="727.65" Width="56.7" Height="18.9" Text="P8" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text84" Left="793.8" Width="56.7" Height="18.9" Text="P9" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text85" Left="859.95" Width="56.7" Height="18.9" Text="P10" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text86" Left="926.1" Width="56.7" Height="18.9" Text="P11" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text87" Left="992.25" Width="56.7" Height="18.9" Text="P12" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      </ChildBand>
    </PageHeaderBand>
    <GroupHeaderBand Name="CostCentreGroup" Top="113.95" Width="1047.06" Height="18.9" BeforePrintEvent="CostCentreGroup_BeforePrint" StartNewPage="true" Condition="CostCentreGroupCondition()" SortOrder="None">
      <TextObject Name="CostCentreHeader" Width="245.7" Height="18.9" Border.Lines="Bottom" Border.Color="DarkBlue" Text="[IncomeExpense.costcentrecode] - [IncomeExpense.costcentrename]" Font="Arial, 10pt, style=Bold" TextFill.Color="DarkBlue"/>
      <GroupHeaderBand Name="AccountTypeGroup" Top="136.18" Width="1047.06" KeepChild="true" KeepWithData="true" Condition="[IncomeExpense.accounttype]" SortOrder="None">
        <DataBand Name="TransactionBand" Top="139.52" Width="1047.06" Height="18.9" BeforePrintEvent="Data_BeforePrint" KeepChild="true" DataSource="IncomeExpense">
          <TextObject Name="ActualField" Left="274.05" Width="94.5" Height="18.9" Text="[IncomeExpense.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="BudgetField" Left="368.55" Width="94.5" Height="18.9" Text="[IncomeExpense.budget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="ActualYTDField" Left="510.3" Width="94.5" Height="18.9" Text="[IncomeExpense.actualytd]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="BudgetYTDField" Left="604.8" Width="94.5" Height="18.9" Text="[IncomeExpense.budgetytd]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="VarianceField" Left="463.05" Width="47.25" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actual],[IncomeExpense.budget])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="VarianceYTDField" Left="699.3" Width="56.7" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actualytd],[IncomeExpense.budgetytd])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="YearBudgetField" Left="756" Width="94.5" Height="18.9" Text="[IncomeExpense.wholeyearbudget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="AccountCodeField" Left="45.36" Width="56.7" Height="18.9" Text="[IIf([param_cost_centre_breakdown],[IncomeExpense.costcentrecode],[IncomeExpense.accountcode])]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="AccountNameField" Left="102.06" Width="207.9" Height="18.9" Text="[IIf([param_cost_centre_breakdown],[IncomeExpense.costcentrename],[IncomeExpense.accountname])]" Padding="0, 0, 2, 0" Font="Arial, 9pt"/>
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
              <TextObject Name="FooterAccountNameField" Left="56.7" Width="207.9" Height="18.9" Text="Total [IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
              <TextObject Name="FooterAccountCodeField" Width="56.7" Height="18.9" Text="[IncomeExpense.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray"/>
              <ChildBand Name="FooterLevel1" Top="206.22" Width="1047.06" Height="28.35">
                <TextObject Name="Level1ActualField" Left="274.05" Width="94.5" Height="18.9" Text="[IncomeExpense.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1BudgetField" Left="368.55" Width="94.5" Height="18.9" Text="[IncomeExpense.budget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1ActualYTDField" Left="510.3" Width="94.5" Height="18.9" Text="[IncomeExpense.actualytd]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1BudgetYTDField" Left="604.8" Width="94.5" Height="18.9" Text="[IncomeExpense.budgetytd]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1VarianceField" Left="463.05" Width="47.25" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actual],[IncomeExpense.budget])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold"/>
                <TextObject Name="Level1VarianceYTDField" Left="699.3" Width="56.7" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actualytd],[IncomeExpense.budgetytd])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold"/>
                <TextObject Name="Level1YearBudgetField" Left="756" Width="94.5" Height="18.9" Text="[IncomeExpense.wholeyearbudget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <TextObject Name="Level1AccountNameField" Width="207.9" Height="18.9" Text="[IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                <LineObject Name="Line2" Left="274.05" Top="18.9" Width="576.45" Border.Width="1.5"/>
                <ChildBand Name="PeriodBand" Top="237.9" Width="1047.06" Height="18.9">
                  <TextObject Name="PeriodAccountNameField" Left="56.7" Width="207.9" Height="18.9" Text="Total [IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                  <TextObject Name="PeriodAccountCodeField" Width="56.7" Height="18.9" Text="[IncomeExpense.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt"/>
                  <TextObject Name="P1" Left="264.6" Width="60.48" Height="18.9" Text="[IncomeExpense.p1]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P2" Left="330.75" Width="60.48" Height="18.9" Text="[IncomeExpense.p2]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P3" Left="396.9" Width="60.48" Height="18.9" Text="[IncomeExpense.p3]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P4" Left="463.05" Width="60.48" Height="18.9" Text="[IncomeExpense.p4]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P5" Left="529.2" Width="60.48" Height="18.9" Text="[IncomeExpense.p5]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P6" Left="595.35" Width="60.48" Height="18.9" Text="[IncomeExpense.p6]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P7" Left="661.5" Width="60.48" Height="18.9" Text="[IncomeExpense.p7]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P8" Left="727.65" Width="60.48" Height="18.9" Text="[IncomeExpense.p8]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P9" Left="793.8" Width="60.48" Height="18.9" Text="[IncomeExpense.p9]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P10" Left="859.95" Width="60.48" Height="18.9" Text="[IncomeExpense.p10]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P11" Left="926.1" Width="60.48" Height="18.9" Text="[IncomeExpense.p11]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <TextObject Name="P12" Left="992.25" Width="60.48" Height="18.9" Text="[IncomeExpense.p12]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                  <ChildBand Name="PeriodFooterBand" Top="260.13" Width="1047.06" Height="18.9">
                    <TextObject Name="PeriodFooterAccountNameField" Left="56.7" Width="207.9" Height="18.9" Text="Total [IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="PeriodFooterAccountCodeField" Width="56.7" Height="18.9" Text="[IncomeExpense.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray"/>
                    <TextObject Name="Text62" Left="264.6" Width="60.48" Height="18.9" Text="[IncomeExpense.p1]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text63" Left="330.75" Width="60.48" Height="18.9" Text="[IncomeExpense.p2]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text64" Left="396.9" Width="60.48" Height="18.9" Text="[IncomeExpense.p3]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text65" Left="463.05" Width="60.48" Height="18.9" Text="[IncomeExpense.p4]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text66" Left="529.2" Width="60.48" Height="18.9" Text="[IncomeExpense.p5]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text67" Left="595.35" Width="60.48" Height="18.9" Text="[IncomeExpense.p6]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text68" Left="661.5" Width="60.48" Height="18.9" Text="[IncomeExpense.p7]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text69" Left="727.65" Width="60.48" Height="18.9" Text="[IncomeExpense.p8]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text70" Left="793.8" Width="60.48" Height="18.9" Text="[IncomeExpense.p9]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text71" Left="859.95" Width="60.48" Height="18.9" Text="[IncomeExpense.p10]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text72" Left="926.1" Width="60.48" Height="18.9" Text="[IncomeExpense.p11]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <TextObject Name="Text73" Left="992.25" Width="60.48" Height="18.9" Text="[IncomeExpense.p12]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
                    <ChildBand Name="PeriodFooterLevel1" Top="282.37" Width="1047.06" Height="28.35">
                      <TextObject Name="PeriodLevel1AccountNameField" Width="207.9" Height="18.9" Text="[IncomeExpense.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text88" Left="264.6" Width="60.48" Height="18.9" Text="[IncomeExpense.p1]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text89" Left="330.75" Width="60.48" Height="18.9" Text="[IncomeExpense.p2]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text90" Left="396.9" Width="60.48" Height="18.9" Text="[IncomeExpense.p3]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text91" Left="463.05" Width="60.48" Height="18.9" Text="[IncomeExpense.p4]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text92" Left="529.2" Width="60.48" Height="18.9" Text="[IncomeExpense.p5]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text93" Left="595.35" Width="60.48" Height="18.9" Text="[IncomeExpense.p6]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text94" Left="661.5" Width="60.48" Height="18.9" Text="[IncomeExpense.p7]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text95" Left="727.65" Width="60.48" Height="18.9" Text="[IncomeExpense.p8]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text96" Left="793.8" Width="60.48" Height="18.9" Text="[IncomeExpense.p9]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text97" Left="859.95" Width="60.48" Height="18.9" Text="[IncomeExpense.p10]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text98" Left="926.1" Width="60.48" Height="18.9" Text="[IncomeExpense.p11]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
                      <TextObject Name="Text99" Left="992.25" Width="60.48" Height="18.9" Text="[IncomeExpense.p12]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" AutoShrink="FontSize" AutoShrinkMinSize="8" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
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
      <TextObject Name="Text17" Left="878.85" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(15,'Account Detail','Tim''s private version','DEMO',False,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="06/27/2014 09:29:32" ReportInfo.CreatorVersion="2014.2.1.0">
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
    
    String GroupHeader ()
    {
      switch ((String)Report.GetParameterValue(&quot;param_sortby&quot;))
      {
        case &quot;Cost Centre&quot; :
        case &quot;Account&quot; : return 
             (String)Report.GetColumnValue(&quot;a_transaction.a_cost_centre_code_c&quot;) + &quot;-&quot;
            +(String)Report.GetColumnValue(&quot;a_transaction.a_account_code_c&quot;) + &quot; &quot;
            +(String)Report.GetColumnValue(&quot;a_transaction.a_account.a_account_code_long_desc_c&quot;) + &quot;, &quot;
            +(String)Report.GetColumnValue(&quot;a_transaction.a_costCentre.a_cost_centre_name_c&quot;);
        case &quot;Reference&quot; : return
            &quot;Reference : &quot; + ((String)Report.GetColumnValue(&quot;a_transaction.a_reference_c&quot;));
        case &quot;Analysis Type&quot; : return
             (String)Report.GetColumnValue(&quot;a_transaction.a_analysis_type_code_c&quot;) + &quot;: &quot;
            +(String)Report.GetColumnValue(&quot;a_transaction.a_analysis_type_description_c&quot;);
        default: return &quot;Group&quot;;
      }
    }
    
    Decimal OpeningBalance()
    {
      String Account = (String)Report.GetColumnValue(&quot;a_transaction.a_account_code_c&quot;);
      string CostCentre = (String)Report.GetColumnValue(&quot;a_transaction.a_cost_centre_code_c&quot;);
      return 0;
    }
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="a_analysis_type" ReferenceName="a_analysis_type" DataType="System.Int32" Enabled="true">
      <Column Name="a_analysis_type_code_c" DataType="System.String"/>
      <Column Name="a_analysis_type_description_c" DataType="System.String"/>
      <Column Name="a_analysis_mode_l" DataType="System.String"/>
      <Column Name="a_analysis_store_c" DataType="System.String"/>
      <Column Name="a_analysis_element_c" DataType="System.String"/>
      <Column Name="a_system_analysis_type_l" DataType="System.String"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <TableDataSource Name="a_trans_anal_attrib" ReferenceName="a_trans_anal_attrib" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_journal_number_i" DataType="System.Int32"/>
      <Column Name="a_transaction_number_i" DataType="System.Int32"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_analysis_type_code_c" DataType="System.String"/>
      <Column Name="a_analysis_attribute_value_c" DataType="System.String"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <TableDataSource Name="a_transaction" ReferenceName="a_transaction" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_journal_number_i" DataType="System.Int32"/>
      <Column Name="a_transaction_number_i" DataType="System.Int32"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_primary_account_code_c" DataType="System.String"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_primary_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_transaction_date_d" DataType="System.DateTime"/>
      <Column Name="a_transaction_amount_n" DataType="System.Decimal"/>
      <Column Name="a_amount_in_base_currency_n" DataType="System.Decimal"/>
      <Column Name="a_analysis_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_reconciled_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_narrative_c" DataType="System.String"/>
      <Column Name="a_debit_credit_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_transaction_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_header_number_i" DataType="System.Int32"/>
      <Column Name="a_detail_number_i" DataType="System.Int32"/>
      <Column Name="a_sub_type_c" DataType="System.String"/>
      <Column Name="a_to_ilt_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_source_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_reference_c" DataType="System.String"/>
      <Column Name="a_source_reference_c" DataType="System.String"/>
      <Column Name="a_system_generated_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_amount_in_intl_currency_n" DataType="System.Decimal"/>
      <Column Name="a_ich_number_i" DataType="System.Int32"/>
      <Column Name="a_key_ministry_key_n" DataType="System.Int64"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
      <Column Name="a_analysis_type_code_c" DataType="System.String"/>
      <Column Name="a_analysis_type_description_c" DataType="System.String"/>
      <Column Name="a_analysis_attribute_value_c" DataType="System.String"/>
    </TableDataSource>
    <TableDataSource Name="a_account" ReferenceName="a_account" DataType="System.Int32" Enabled="true">
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
    </TableDataSource>
    <TableDataSource Name="a_costCentre" ReferenceName="a_costCentre" DataType="System.Int32" Enabled="true">
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
    </TableDataSource>
    <TableDataSource Name="a_ledger" ReferenceName="a_ledger" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_ledger_name_c" DataType="System.String"/>
      <Column Name="a_ledger_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_last_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_last_recurring_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_last_gift_number_i" DataType="System.Int32"/>
      <Column Name="a_last_ap_inv_number_i" DataType="System.Int32"/>
      <Column Name="a_last_header_r_number_i" DataType="System.Int32"/>
      <Column Name="a_last_po_number_i" DataType="System.Int32"/>
      <Column Name="a_last_so_number_i" DataType="System.Int32"/>
      <Column Name="a_max_gift_aid_amount_n" DataType="System.Decimal"/>
      <Column Name="a_min_gift_aid_amount_n" DataType="System.Decimal"/>
      <Column Name="a_number_of_gifts_to_display_i" DataType="System.Int32"/>
      <Column Name="a_tax_type_code_c" DataType="System.String"/>
      <Column Name="a_ilt_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_profit_loss_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_current_accounting_period_i" DataType="System.Int32"/>
      <Column Name="a_number_of_accounting_periods_i" DataType="System.Int32"/>
      <Column Name="a_country_code_c" DataType="System.String"/>
      <Column Name="a_base_currency_c" DataType="System.String"/>
      <Column Name="a_transaction_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_year_end_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_forex_gains_losses_account_c" DataType="System.String"/>
      <Column Name="a_system_interface_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_suspense_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_bank_accounts_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_delete_ledger_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_new_financial_year_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_recalculate_gl_master_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_installation_id_c" DataType="System.String"/>
      <Column Name="a_budget_control_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_budget_data_retention_i" DataType="System.Int32"/>
      <Column Name="a_cost_of_sales_gl_account_c" DataType="System.String"/>
      <Column Name="a_creditor_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_current_financial_year_i" DataType="System.Int32"/>
      <Column Name="a_current_period_i" DataType="System.Int32"/>
      <Column Name="a_date_cr_dr_balances_d" DataType="System.DateTime"/>
      <Column Name="a_debtor_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_fa_depreciation_gl_account_c" DataType="System.String"/>
      <Column Name="a_fa_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_fa_pl_on_sale_gl_account_c" DataType="System.String"/>
      <Column Name="a_fa_prov_for_depn_gl_account_c" DataType="System.String"/>
      <Column Name="a_ilt_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_last_ap_dn_number_i" DataType="System.Int32"/>
      <Column Name="a_last_po_ret_number_i" DataType="System.Int32"/>
      <Column Name="a_last_so_del_number_i" DataType="System.Int32"/>
      <Column Name="a_last_so_ret_number_i" DataType="System.Int32"/>
      <Column Name="a_last_special_gift_number_i" DataType="System.Int32"/>
      <Column Name="a_number_fwd_posting_periods_i" DataType="System.Int32"/>
      <Column Name="a_periods_per_financial_year_i" DataType="System.Int32"/>
      <Column Name="a_discount_allowed_pct_n" DataType="System.Decimal"/>
      <Column Name="a_discount_received_pct_n" DataType="System.Decimal"/>
      <Column Name="a_po_accrual_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_provisional_year_end_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_purchase_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_ret_earnings_gl_account_c" DataType="System.String"/>
      <Column Name="a_sales_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_so_accrual_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_stock_accrual_gl_account_c" DataType="System.String"/>
      <Column Name="a_stock_adj_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_stock_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_tax_excl_incl_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_tax_excl_incl_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_tax_input_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_tax_input_gl_cc_code_c" DataType="System.String"/>
      <Column Name="a_tax_output_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_terms_of_payment_code_c" DataType="System.String"/>
      <Column Name="a_last_po_rec_number_i" DataType="System.Int32"/>
      <Column Name="a_tax_gl_account_number_i" DataType="System.Int32"/>
      <Column Name="a_actuals_data_retention_i" DataType="System.Int32"/>
      <Column Name="p_partner_key_n" DataType="System.Int64"/>
      <Column Name="a_calendar_mode_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_year_end_process_status_i" DataType="System.Int32"/>
      <Column Name="a_last_header_p_number_i" DataType="System.Int32"/>
      <Column Name="a_ilt_processing_centre_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_last_gift_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_intl_currency_c" DataType="System.String"/>
      <Column Name="a_last_rec_gift_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_gift_data_retention_i" DataType="System.Int32"/>
      <Column Name="a_recalculate_all_periods_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_last_ich_number_i" DataType="System.Int32"/>
      <Column Name="a_branch_processing_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_consolidation_ledger_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <TableDataSource Name="balances" ReferenceName="balances" DataType="System.Int32" Enabled="true">
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="OpeningBalance" DataType="System.Decimal"/>
      <Column Name="ClosingBalance" DataType="System.Decimal"/>
    </TableDataSource>
    <Relation Name="a_account_a_transaction" ParentDataSource="a_account" ChildDataSource="a_transaction" ParentColumns="a_account_code_c" ChildColumns="a_account_code_c" Enabled="true"/>
    <Relation Name="a_costCentre_a_transaction" ParentDataSource="a_costCentre" ChildDataSource="a_transaction" ParentColumns="a_cost_centre_code_c" ChildColumns="a_cost_centre_code_c" Enabled="true"/>
    <Relation Name="a_trans_anal_attrib_a_transaction" ParentDataSource="a_trans_anal_attrib" ChildDataSource="a_transaction" ParentColumns="a_ledger_number_i&#13;&#10;a_batch_number_i&#13;&#10;a_journal_number_i&#13;&#10;a_transaction_number_i" ChildColumns="a_ledger_number_i&#13;&#10;a_batch_number_i&#13;&#10;a_journal_number_i&#13;&#10;a_transaction_number_i" Enabled="true"/>
    <Relation Name="a_analysis_type_a_trans_anal_attrib" ParentDataSource="a_analysis_type" ChildDataSource="a_trans_anal_attrib" ParentColumns="a_analysis_type_code_c" ChildColumns="a_analysis_type_code_c" Enabled="true"/>
    <Relation Name="balances_a_transaction" ParentDataSource="balances" ChildDataSource="a_transaction" ParentColumns="a_cost_centre_code_c&#13;&#10;a_account_code_c" ChildColumns="a_cost_centre_code_c&#13;&#10;a_account_code_c" Enabled="true"/>
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
    <Total Name="GroupDebit" Expression="Debits.Value" Evaluator="Transaction" PrintOn="GroupFooter2"/>
    <Total Name="GroupCredit" Expression="Credits.Value" Evaluator="Transaction" PrintOn="GroupFooter2"/>
    <Total Name="OuterGroupDebit" Expression="Debits.Value" Evaluator="Transaction" PrintOn="GroupFooter1"/>
    <Total Name="OuterGroupCredit" Expression="Credits.Value" Evaluator="Transaction" PrintOn="GroupFooter1"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="85.05">
      <TextObject Name="Text1" Left="245.7" Width="207.9" Height="18.9" Text="Account Detail" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="453.6" Width="122.85" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="576.45" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Width="170.1" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text11" Left="576.45" Top="18.9" Width="141.75" Height="18.9" Text="[param_cost_centre_list_title]"/>
      <TextObject Name="Text12" Left="453.6" Top="18.9" Width="122.85" Height="18.9" Text="Cost Centres :" HorzAlign="Right"/>
      <TextObject Name="Text14" Left="453.6" Top="37.8" Width="122.85" Height="18.9" Text="Accounts :" HorzAlign="Right"/>
      <TextObject Name="Text13" Left="576.45" Top="37.8" Width="141.75" Height="18.9" Text="[param_account_list_title]"/>
      <TextObject Name="Text21" Top="18.9" Width="75.6" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="75.6" Top="18.9" Width="170.1" Height="18.9" Text="[param_currency_name]"/>
      <TextObject Name="Text22" Top="37.8" Width="75.6" Height="18.9" Text="[IIf([param_period],&quot;Period :&quot;,&quot;Date :&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="75.6" Top="37.8" Width="198.45" Height="18.9" Text="[IIf([param_period],ToString([param_start_period_i])+&quot; - &quot;+ToString([param_end_period_i]), OmDate([param_start_date]) + &quot; - &quot; + OmDate([param_end_date]))]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text42" Left="245.7" Top="18.9" Width="207.9" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <TextObject Name="Text43" Left="274.05" Top="37.8" Width="179.55" Height="18.9"/>
      <LineObject Name="Line1" Left="718.2" Top="75.6" Width="-718.2"/>
      <TextObject Name="Text50" Left="453.6" Top="56.7" Width="122.85" Height="18.9" Text="Ordered By :" HorzAlign="Right"/>
      <TextObject Name="Text51" Left="576.45" Top="56.7" Width="141.75" Height="18.9" Text="[param_sortby]"/>
      <TextObject Name="Text56" Left="75.6" Top="56.7" Width="198.45" Height="18.9" Text="[OmDate([param_start_date])+&quot; - &quot;+OmDate([param_end_date])]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="88.38" Width="718.2"/>
    <GroupHeaderBand Name="GroupHeader1" Top="91.72" Width="718.2" Condition="Switch(&quot;Account&quot;==[param_sortby],[a_transaction.a_account_code_c], &quot;Cost Centre&quot;==[param_sortby], [a_transaction.a_cost_centre_code_c])" SortOrder="None">
      <GroupHeaderBand Name="GroupHeader2" Top="95.05" Width="718.2" Height="47.25" KeepChild="true" KeepWithData="true" Condition="Switch(&quot;Account&quot;==[param_sortby],[a_transaction.a_cost_centre_code_c], &quot;Cost Centre&quot;==[param_sortby], [a_transaction.a_account_code_c], &quot;Reference&quot;==[param_sortby], [a_transaction.a_reference_c], &quot;Analysis Type&quot;==[param_sortby],[a_transaction.a_analysis_type_code_c])" SortOrder="None" KeepTogether="true">
        <TextObject Name="Text15" Top="9.45" Width="718.2" Height="18.9" CanShrink="true" CanBreak="false" Text="[GroupHeader ()]" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 10pt, style=Bold"/>
        <TextObject Name="Text3" Left="113.4" Top="28.35" Width="85.05" Height="18.9" Text="Date" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text5" Left="198.45" Top="28.35" Width="85.05" Height="18.9" Text="Debits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text7" Left="368.55" Top="28.35" Width="103.95" Height="18.9" Text="Narrative" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text23" Left="283.5" Top="28.35" Width="85.05" Height="18.9" Text="Credits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text40" Top="28.35" Width="9.45" Height="18.9"/>
        <TextObject Name="Text41" Left="9.45" Top="28.35" Width="103.95" Height="18.9" Text="Ref" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <DataBand Name="Transaction" Top="145.63" Width="718.2" Height="18.9" CanGrow="true" KeepChild="true" DataSource="a_transaction" KeepDetail="true">
          <TextObject Name="Text30" Width="9.45" Height="18.9" Text="[a_transaction.a_cost_centre_code_c]" TextFill.Color="White"/>
          <TextObject Name="TransRef" Left="9.45" Width="103.95" Height="18.9" Text="[a_transaction.a_reference_c]" AutoShrink="FontSize" AutoShrinkMinSize="7" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Clip="false"/>
          <TextObject Name="TransDate" Left="113.4" Width="85.05" Height="18.9" Text="[OmDate([a_transaction.a_transaction_date_d])]" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="Debits" Left="198.45" Width="85.05" Height="18.9" Text="[IIf ([a_transaction.a_debit_credit_indicator_l]==true,Switch ([param_currency]==&quot;Base&quot;,[a_transaction.a_amount_in_base_currency_n],[param_currency]==&quot;Transaction&quot;,[a_transaction.a_transaction_amount_n],[param_currency]==&quot;International&quot;,[a_transaction.a_amount_in_intl_currency_n]),0)]" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
            <Formats>
              <NumberFormat UseLocale="false" NegativePattern="1"/>
              <GeneralFormat/>
            </Formats>
            <Highlight>
              <Condition Expression="Value == 0" TextFill.Color="White"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Credits" Left="283.5" Width="85.05" Height="18.9" Text="[IIf ([a_transaction.a_debit_credit_indicator_l]==false,Switch ([param_currency]==&quot;Base&quot;,[a_transaction.a_amount_in_base_currency_n],[param_currency]==&quot;Transaction&quot;,[a_transaction.a_transaction_amount_n],[param_currency]==&quot;International&quot;,[a_transaction.a_amount_in_intl_currency_n]),0)]" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
            <Formats>
              <NumberFormat UseLocale="false" NegativePattern="1"/>
              <GeneralFormat/>
            </Formats>
            <Highlight>
              <Condition Expression="Value == 0" TextFill.Color="White"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Narrative" Left="368.55" Width="340.2" Height="18.9" CanGrow="true" GrowToBottom="true" Text="[a_transaction.a_narrative_c]" Font="Arial, 9pt" Clip="false"/>
          <DataBand Name="Attributes" Top="167.87" Width="718.2" Height="9.45" CanGrow="true" CanShrink="true">
            <TextObject Name="AttrDescr" Left="368.55" Width="349.65" Height="9.45" CanGrow="true" CanShrink="true" CanBreak="false" Text="[a_transaction.a_trans_anal_attrib.a_analysis_type.a_analysis_type_description_c][a_transaction.a_trans_anal_attrib.a_analysis_attribute_value_c]" Font="Arial, 9pt" TextFill.Color="Green"/>
          </DataBand>
        </DataBand>
        <GroupFooterBand Name="GroupFooter2" Top="180.65" Width="718.2" Height="37.8" KeepChild="true" KeepWithData="true">
          <TextObject Name="GroupDebitTotal" Left="198.45" Width="85.05" Height="18.9" Text="[GroupDebit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
          <TextObject Name="GroupCreditTotal" Left="283.5" Width="85.05" Height="18.9" Text="[GroupCredit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
          <TextObject Name="Text24" Left="103.95" Width="94.5" Height="18.9" Text="Sub-Total :" HorzAlign="Right" Font="Arial, 9pt, style=Italic"/>
          <TextObject Name="Text4" Left="198.45" Top="18.9" Width="85.05" Height="18.9" Text="[ToDecimal([GroupDebit]-[GroupCredit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold">
            <Highlight>
              <Condition Expression="Value &lt;= 0" TextFill.Color="White"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text25" Left="283.5" Top="18.9" Width="85.05" Height="18.9" Text="[ToDecimal([GroupCredit]-[GroupDebit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold">
            <Highlight>
              <Condition Expression="Value &lt; 0" TextFill.Color="White"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text26" Left="103.95" Top="18.9" Width="94.5" Height="18.9" Text="Balance :" HorzAlign="Right" Font="Arial, 9pt, style=Italic"/>
          <TextObject Name="Text32" Width="9.45" Height="18.9" TextFill.Color="White"/>
          <TextObject Name="Text33" Left="9.45" Width="9.45" Height="18.9" TextFill.Color="White"/>
          <TextObject Name="Text34" Top="18.9" Width="9.45" Height="18.9" TextFill.Color="White"/>
          <TextObject Name="Text35" Left="9.45" Top="18.9" Width="9.45" Height="18.9" TextFill.Color="White"/>
          <TextObject Name="Text54" Left="453.6" Top="18.9" Width="103.95" Height="18.9" Text="[a_transaction.balances.OpeningBalance]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
          <TextObject Name="Text55" Left="557.55" Top="18.9" Width="103.95" Height="18.9" Text="[a_transaction.balances.ClosingBalance]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
          <TextObject Name="Text52" Left="453.6" Width="103.95" Height="18.9" Text="Opening Balance" HorzAlign="Right" Font="Arial, 8pt, style=Bold"/>
          <TextObject Name="Text53" Left="557.55" Width="103.95" Height="18.9" Text="Closing Balance" HorzAlign="Right" Font="Arial, 8pt, style=Bold"/>
          <LineObject Name="Line2" Left="661.5" Width="-567" Border.Width="1.5"/>
        </GroupFooterBand>
      </GroupHeaderBand>
      <GroupFooterBand Name="GroupFooter1" Top="221.78" Width="718.2" Height="47.25" KeepChild="true" KeepWithData="true">
        <TextObject Name="Text27" Left="198.45" Top="18.9" Width="85.05" Height="18.9" Text="[ToDecimal([OuterGroupDebit]-[OuterGroupCredit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue">
          <Highlight>
            <Condition Expression="Value &lt;= 0" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="Text28" Left="283.5" Top="18.9" Width="85.05" Height="18.9" Text="[ToDecimal([OuterGroupCredit]-[OuterGroupDebit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue">
          <Highlight>
            <Condition Expression="Value &lt; 0" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="f36" Width="9.45" Height="18.9"/>
        <TextObject Name="f37" Left="9.45" Width="9.45" Height="18.9"/>
        <TextObject Name="Text16" Left="103.95" Width="94.5" Height="18.9" CanShrink="true" CanBreak="false" Text="[Switch(&quot;Account&quot;==[param_sortby],[a_transaction.a_account_code_c]+&quot; Total :&quot;,&quot;Cost Centre&quot;==[param_sortby], [a_transaction.a_cost_centre_code_c]+&quot; Total :&quot;,&quot;Reference&quot;==[param_sortby], &quot;Total :&quot;, &quot;Analysis Type&quot;==[param_sortby], &quot;Total :&quot;)]" AutoShrink="FontSize" AutoShrinkMinSize="7" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Italic" TextFill.Color="Blue"/>
        <TextObject Name="OuterGroupDebitTotal" Left="198.45" Width="85.05" Height="18.9" Text="[OuterGroupDebit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue"/>
        <TextObject Name="OuterGroupCreditTotal" Left="283.5" Width="85.05" Height="18.9" Text="[OuterGroupCredit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue"/>
        <LineObject Name="Line3" Left="378" Top="36.8" Width="-283.5" Border.Color="Blue" Border.Width="1.5"/>
        <TextObject Name="f38" Top="18.9" Width="9.45" Height="18.9"/>
        <TextObject Name="f39" Left="9.45" Top="18.9" Width="9.45" Height="18.9"/>
        <TextObject Name="f29" Left="18.9" Top="18.9" Width="85.05" Height="18.9"/>
      </GroupFooterBand>
    </GroupHeaderBand>
    <PageFooterBand Name="PageFooter1" Top="272.37" Width="718.2" Height="18.9">
      <TextObject Name="Text44" Width="9.45" Height="18.9"/>
      <TextObject Name="Text45" Left="9.45" Width="9.45" Height="18.9"/>
      <TextObject Name="Text46" Left="18.9" Width="9.45" Height="18.9"/>
      <TextObject Name="Text47" Left="28.35" Width="9.45" Height="18.9"/>
      <TextObject Name="Text48" Left="37.8" Width="9.45" Height="18.9"/>
      <TextObject Name="Text49" Left="47.25" Width="9.45" Height="18.9" Text="T"/>
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(1,'HOSA','OpenPetra default template','System',True,True,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="03/25/2014 15:56:40" ReportInfo.CreatorVersion="2013.4.4.0">
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
    <TableDataSource Name="a_costCentre" ReferenceName="a_costCentre" DataType="System.Int32" Enabled="true">
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
    </TableDataSource>
    <TableDataSource Name="Gifts" ReferenceName="Gifts" DataType="System.Int32" Enabled="true">
      <Column Name="costcentre" DataType="System.String"/>
      <Column Name="accountcode" DataType="System.String"/>
      <Column Name="giftbaseamount" DataType="System.Decimal"/>
      <Column Name="gifttransactionamount" DataType="System.Decimal"/>
      <Column Name="recipientkey" DataType="System.Int64"/>
      <Column Name="recipientshortname" DataType="System.String"/>
      <Column Name="narrative" DataType="System.String"/>
      <Column Name="GiftIntlAmount" DataType="System.Decimal"/>
      <Column Name="Reference" DataType="System.String"/>
    </TableDataSource>
    <TableDataSource Name="a_account" ReferenceName="a_account" DataType="System.Int32" Enabled="true">
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
    </TableDataSource>
    <TableDataSource Name="a_transaction" ReferenceName="a_transaction" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_journal_number_i" DataType="System.Int32"/>
      <Column Name="a_transaction_number_i" DataType="System.Int32"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_primary_account_code_c" DataType="System.String"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_primary_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_transaction_date_d" DataType="System.DateTime"/>
      <Column Name="a_transaction_amount_n" DataType="System.Decimal"/>
      <Column Name="a_amount_in_base_currency_n" DataType="System.Decimal"/>
      <Column Name="a_analysis_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_reconciled_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_narrative_c" DataType="System.String"/>
      <Column Name="a_debit_credit_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_transaction_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_header_number_i" DataType="System.Int32"/>
      <Column Name="a_detail_number_i" DataType="System.Int32"/>
      <Column Name="a_sub_type_c" DataType="System.String"/>
      <Column Name="a_to_ilt_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_source_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_reference_c" DataType="System.String"/>
      <Column Name="a_source_reference_c" DataType="System.String"/>
      <Column Name="a_system_generated_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_amount_in_intl_currency_n" DataType="System.Decimal"/>
      <Column Name="a_ich_number_i" DataType="System.Int32"/>
      <Column Name="a_key_ministry_key_n" DataType="System.Int64"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
      <Column Name="a_analysis_type_code_c" DataType="System.String"/>
      <Column Name="a_analysis_type_description_c" DataType="System.String"/>
      <Column Name="a_analysis_attribute_value_c" DataType="System.String"/>
    </TableDataSource>
    <Relation Name="a_account_Gifts" ParentDataSource="a_account" ChildDataSource="Gifts" ParentColumns="a_account_code_c" ChildColumns="accountcode" Enabled="true"/>
    <Relation Name="a_account_a_transaction" ParentDataSource="a_account" ChildDataSource="a_transaction" ParentColumns="a_account_code_c" ChildColumns="a_account_code_c" Enabled="true"/>
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
    <Parameter Name="param_filter_cost_centres" DataType="System.String"/>
    <Parameter Name="param_ExcludeInactiveCostCentres" DataType="System.Boolean"/>
    <Parameter Name="param_cost_centre_codes" DataType="System.String"/>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_daterange" DataType="System.String"/>
    <Parameter Name="param_rgrAccounts" DataType="System.String"/>
    <Parameter Name="param_rgrCostCentres" DataType="System.String"/>
    <Parameter Name="param_ich_number" DataType="System.Int32"/>
    <Parameter Name="param_quarter" DataType="System.Boolean"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_currency_formatter" DataType="System.String"/>
    <Parameter Name="param_date_title" DataType="System.String"/>
    <Parameter Name="param_currency_name" DataType="System.String"/>
    <Parameter Name="param_period_breakdown" DataType="System.Boolean"/>
    <Parameter Name="param_real_year" DataType="System.Int32"/>
    <Total Name="GiftDebitsTotal" Expression="GiftDebits.Value" Evaluator="Data1" PrintOn="GiftsFooter" ResetOnReprint="true"/>
    <Total Name="GiftCreditsTotal" Expression="GiftCredits.Value" Evaluator="Data1" PrintOn="GiftsFooter" ResetOnReprint="true"/>
    <Total Name="TransDebitsTotal" Expression="TransDebits.Value" Evaluator="Data3" PrintOn="TransFooter" ResetOnReprint="true"/>
    <Total Name="TransCreditsTotal" Expression="TransCredits.Value" Evaluator="Data3" PrintOn="TransFooter" ResetOnReprint="true"/>
    <Total Name="AllGiftDebits" Expression="GiftDebits.Value" Evaluator="Data1" PrintOn="PageFooter" ResetOnReprint="true"/>
    <Total Name="AllGiftCredits" Expression="GiftCredits.Value" Evaluator="Data1" PrintOn="PageFooter" ResetOnReprint="true"/>
    <Total Name="AllTransDebits" Expression="TransDebits.Value" Evaluator="Data3" PrintOn="PageFooter" ResetOnReprint="true"/>
    <Total Name="AllTransCredits" Expression="TransCredits.Value" Evaluator="Data3" PrintOn="PageFooter" ResetOnReprint="true"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <GroupHeaderBand Name="PageHeader" Width="718.2" Height="66.15" StartNewPage="true" Condition="[a_costCentre.a_cost_centre_code_c]" SortOrder="None">
      <TextObject Name="Text22" Top="37.8" Width="75.6" Height="18.9" Text="[IIf([param_period],&quot;Period :&quot;,&quot;Date :&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="75.6" Top="37.8" Width="245.7" Height="18.9" Text="[param_date_title]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text11" Left="576.45" Top="18.9" Width="141.75" Height="18.9" Text="[a_costCentre.a_cost_centre_code_c]"/>
      <TextObject Name="Text12" Left="453.6" Top="18.9" Width="122.85" Height="18.9" Text="Cost Centre :" HorzAlign="Right"/>
      <TextObject Name="Text21" Top="18.9" Width="75.6" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="75.6" Top="18.9" Width="151.2" Height="18.9" Text="[param_currency]"/>
      <TextObject Name="Text10" Left="75.6" Width="151.2" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text1" Left="226.8" Width="226.8" Height="18.9" Text="HOSA" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="453.6" Width="122.85" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="576.45" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <LineObject Name="Line1" Top="56.7" Width="718.2" Border.Width="2"/>
      <TextObject Name="Text88" Left="226.8" Top="18.9" Width="226.8" Height="18.9" Text="[param_ledger_name] " HorzAlign="Center"/>
      <DataBand Name="GiftsReport" Top="69.48" Width="718.2" DataSource="a_costCentre">
        <GroupHeaderBand Name="GiftsHeader" Top="72.82" Width="718.2" Height="18.9" Condition="[Gifts.accountcode]" SortOrder="None">
          <TextObject Name="Text64" Width="75.6" Height="18.9" Text="[Gifts.costcentre]-[Gifts.accountcode]" AutoShrink="FontSize" AutoShrinkMinSize="7" Font="Arial, 10pt, style=Bold, Italic"/>
          <TextObject Name="Text65" Left="75.6" Width="633.15" Height="18.9" Text="[Gifts.a_account.a_account_code_long_desc_c], [a_costCentre.a_cost_centre_name_c]" WordWrap="false" Font="Arial, 10pt, style=Bold, Italic"/>
          <DataBand Name="Data1" Top="95.05" Width="718.2" Height="18.9" DataSource="Gifts" Filter="[Gifts.costcentre]==[a_costCentre.a_cost_centre_code_c]">
            <TextObject Name="Text66" Left="113.4" Width="113.4" Height="18.9" CanGrow="true" Text="[Gifts.Reference]"/>
            <TextObject Name="GiftCredits" Left="321.3" Width="94.5" Height="18.9" Text="[IIf([param_currency]==&quot;Base&quot;,IIf([Gifts.GiftBaseAmount] &gt; 0,[Gifts.GiftBaseAmount], 0),IIf([Gifts.GiftIntlAmount] &gt; 0,[Gifts.GiftIntlAmount], 0))]" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="Text68" Left="415.8" Width="302.4" Height="18.9" CanGrow="true" Text="[Gifts.narrative]"/>
            <TextObject Name="GiftDebits" Left="226.8" Width="94.5" Height="18.9" Text="[IIf([param_currency]==&quot;Base&quot;,IIf([Gifts.GiftBaseAmount] &lt; 0,[Gifts.GiftBaseAmount], 0),IIf([Gifts.GiftIntlAmount] &lt; 0,[Gifts.GiftIntlAmount], 0))]" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="Text3" Width="113.4" Height="18.9" Text=" "/>
          </DataBand>
          <GroupFooterBand Name="GiftsFooter" Top="117.28" Width="718.2" Height="28.35">
            <TextObject Name="Text70" Left="321.3" Width="94.5" Height="18.9" Text="[GiftCreditsTotal]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="Text71" Left="226.8" Width="94.5" Height="18.9" Text="[GiftDebitsTotal]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <LineObject Name="Line2" Left="226.8" Width="189" Border.Color="Blue"/>
            <TextObject Name="Text83" Left="113.4" Width="113.4" Height="18.9" Text="Sub Total :" HorzAlign="Right"/>
            <TextObject Name="Text84" Width="113.4" Height="18.9" Text=" "/>
          </GroupFooterBand>
        </GroupHeaderBand>
        <GroupHeaderBand Name="TransHeader" Top="148.97" Width="718.2" Height="18.9" Condition="[a_transaction.a_account_code_c]">
          <TextObject Name="Text72" Left="75.6" Width="633.15" Height="18.9" Text="[a_transaction.a_account.a_account_code_long_desc_c], [a_costCentre.a_cost_centre_name_c]" WordWrap="false" Font="Arial, 10pt, style=Bold, Italic"/>
          <TextObject Name="Text73" Width="75.6" Height="18.9" Text="[a_costCentre.a_cost_centre_code_c]-[a_transaction.a_account.a_account_code_c]" AutoShrink="FontSize" AutoShrinkMinSize="7" Font="Arial, 10pt, style=Bold, Italic"/>
          <DataBand Name="Data3" Top="171.2" Width="718.2" Height="18.9" DataSource="a_transaction" Filter="[a_costCentre.a_cost_centre_code_c]==[a_transaction.a_cost_centre_code_c]">
            <TextObject Name="TransDate" Width="113.4" Height="18.9" Text="[OmDate([a_transaction.a_transaction_date_d])]"/>
            <TextObject Name="Text75" Left="415.8" Width="302.4" Height="18.9" CanGrow="true" Text="[a_transaction.a_narrative_c]"/>
            <TextObject Name="Text76" Left="113.4" Width="113.4" Height="18.9" CanGrow="true" Text="[a_transaction.a_reference_c]"/>
            <TextObject Name="TransDebits" Left="226.8" Width="94.5" Height="18.9" Text="[IIf([a_transaction.a_debit_credit_indicator_l]==true,IIf([param_currency]==&quot;Base&quot;,[a_transaction.a_amount_in_base_currency_n],[a_transaction.a_amount_in_intl_currency_n]),0)]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="TransCredits" Left="321.3" Width="94.5" Height="18.9" Text="[IIf([a_transaction.a_debit_credit_indicator_l]==false,IIf([param_currency]==&quot;Base&quot;,[a_transaction.a_amount_in_base_currency_n],[a_transaction.a_amount_in_intl_currency_n]),0)]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
              <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
          </DataBand>
          <GroupFooterBand Name="TransFooter" Top="193.43" Width="718.2" Height="28.35">
            <TextObject Name="Text79" Left="226.8" Width="94.5" Height="18.9" Text="[TransDebitsTotal]" Format="Currency" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
			  <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
			  <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <TextObject Name="Text80" Left="321.3" Width="94.5" Height="18.9" Text="[TransCreditsTotal]" Format="Currency" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
              <Formats>
                <NumberFormat UseLocale="false" NegativePattern="1"/>
                <GeneralFormat/>
              </Formats>
			  <Highlight>
                <Condition Expression="Value == 0" TextFill.Color="White"/>
              </Highlight>
            </TextObject>
            <LineObject Name="Line3" Left="226.8" Width="189"/>
            <TextObject Name="Text82" Left="113.4" Width="113.4" Height="18.9" Text="Sub Total :" HorzAlign="Right"/>
          </GroupFooterBand>
        </GroupHeaderBand>
      </DataBand>
      <GroupFooterBand Name="PageFooter" Top="225.12" Width="718.2" Height="47.25">
        <TextObject Name="AllDebits" Left="226.8" Top="9.45" Width="94.5" Height="18.9" Text="[[AllGiftDebits]+[AllTransDebits]]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
        </TextObject>
        <TextObject Name="AllCredits" Left="321.3" Top="9.45" Width="94.5" Height="18.9" Text="[[AllGiftCredits]+[AllTransCredits]]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Blue" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
        </TextObject>
        <LineObject Name="Line4" Width="718.2" Border.Width="2"/>
        <TextObject Name="OutstandingDebit" Left="226.8" Top="28.35" Width="94.5" Height="18.9" Text="[[AllGiftDebits]+[AllTransDebits]-[AllGiftCredits]-[AllTransCredits]]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Red" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
          </Formats>
          <Highlight>
            <Condition Expression="Value &lt; 0.01" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="OutstandingCredit" Left="321.3" Top="28.35" Width="94.5" Height="18.9" Text="[[AllGiftCredits]+[AllTransCredits]-[AllGiftDebits]-[AllTransDebits]]" HorzAlign="Right" Font="Arial, 10pt, style=Bold" TextFill.Color="Red" Trimming="EllipsisCharacter">
          <Formats>
            <NumberFormat UseLocale="false" NegativePattern="1"/>
            <GeneralFormat/>
            <GeneralFormat/>
          </Formats>
          <Highlight>
            <Condition Expression="Value &lt; 0.01" TextFill.Color="Blue"/>
            <Condition Expression="Value &lt; 0" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="Text2" Left="113.4" Top="9.45" Width="113.4" Height="18.9" Text="Grand Total :" HorzAlign="Right"/>
        <TextObject Name="Text81" Left="113.4" Top="28.35" Width="113.4" Height="18.9" Text="Balance :" HorzAlign="Right"/>
      </GroupFooterBand>
    </GroupHeaderBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(2,'Balance Sheet Standard','OpenPetra default template','System',True,True,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="03/25/2014 16:06:08" ReportInfo.CreatorVersion="2013.4.4.0">
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
    <Total Name="TotalLiabPlusEq" Expression="[BalanceSheet.actual]" Evaluator="TransactionBand" EvaluateCondition="[BalanceSheet.accountlevel]==1 &amp;&amp; ([BalanceSheet.accounttype]==&quot;Liability&quot;||[BalanceSheet.accounttype]==&quot;Equity&quot;)&amp;&amp;[BalanceSheet.parentfooter]"/>
    <Total Name="TotalLiabPlusEqLastYear" Expression="[BalanceSheet.actuallastyear]" Evaluator="TransactionBand" EvaluateCondition="[BalanceSheet.accountlevel]==1 &amp;&amp; ([BalanceSheet.accounttype]==&quot;Liability&quot;||[BalanceSheet.accounttype]==&quot;Equity&quot;)&amp;&amp;[BalanceSheet.parentfooter]"/>
  </Dictionary>
  <ReportPage Name="Page1" RawPaperSize="9">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="66.15">
      <TextObject Name="Text1" Left="245.7" Width="226.8" Height="18.9" Text="Balance Sheet" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="510.3" Width="85.05" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="595.35" Width="122.85" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="94.5" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="94.5" Width="113.4" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text21" Top="18.9" Width="94.5" Height="18.9" Text="Currency :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="94.5" Top="18.9" Width="141.75" Height="18.9" Text="[param_currency_name]"/>
      <TextObject Name="Text22" Left="510.3" Top="18.9" Width="85.05" Height="18.9" Text="Balance at " HorzAlign="Right"/>
      <TextObject Name="Text42" Left="245.7" Top="18.9" Width="226.8" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <LineObject Name="Line1" Left="718.2" Top="56.7" Width="-718.2"/>
      <TextObject Name="Text50" Left="595.35" Top="18.9" Width="207.9" Height="18.9" Text="[OmDate([param_end_date])]"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="69.48" Width="718.2" Height="18.9">
      <TextObject Name="Text3" Left="113.4" Width="151.2" Height="18.9" Text="Description" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text5" Left="349.65" Width="103.95" Height="18.9" Text="Actual" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text41" Left="37.8" Width="56.7" Height="18.9" Text="Acc" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text43" Left="463.05" Width="122.85" Height="18.9" Text="Actual Last Year" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
    </PageHeaderBand>
    <DataBand Name="TransactionBand" Top="91.72" Width="718.2" Height="18.9" BeforePrintEvent="Data_BeforePrint" KeepChild="true" DataSource="BalanceSheet">
      <TextObject Name="ActualField" Left="340.2" Width="113.4" Height="18.9" Text="[BalanceSheet.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
      <TextObject Name="ActualYTDField" Left="472.5" Width="113.4" Height="18.9" Text="[BalanceSheet.actuallastyear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
      <TextObject Name="AccountCodeField" Left="45.36" Width="56.7" Height="18.9" Text="[BalanceSheet.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt"/>
      <TextObject Name="AccountNameField" Left="102.06" Width="236.25" Height="18.9" Text="[BalanceSheet.accountname]" Padding="0, 0, 2, 0" Font="Arial, 9pt"/>
      <ChildBand Name="HeaderBand" Top="113.95" Width="718.2" Height="18.9" Visible="false">
        <TextObject Name="HeaderAccountNameField" Left="30.24" Width="217.35" Height="18.9" Text="[BalanceSheet.accountname]:" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
        <ChildBand Name="FooterBand" Top="136.18" Width="718.2" Height="18.9" Visible="false">
          <TextObject Name="FooterActualField" Left="340.2" Width="113.4" Height="18.9" Text="[BalanceSheet.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
          <TextObject Name="FooterActualYTDField" Left="472.5" Width="113.4" Height="18.9" Text="[BalanceSheet.actuallastyear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
          <TextObject Name="FooterAccountNameField" Left="56.7" Width="274.05" Height="18.9" Text="Total [BalanceSheet.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" TextFill.Color="DarkGray" Trimming="EllipsisCharacter"/>
          <TextObject Name="FooterAccountCodeField" Width="56.7" Height="18.9" Text="[BalanceSheet.accountcode]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt" TextFill.Color="DarkGray"/>
          <ChildBand Name="FooterLevel1" Top="158.42" Width="718.2" Height="37.8">
            <TextObject Name="Level1ActualField" Left="340.2" Width="113.4" Height="18.9" Text="[BalanceSheet.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold, Underline" Trimming="EllipsisCharacter"/>
            <TextObject Name="Level1ActualYTDField" Left="472.5" Width="113.4" Height="18.9" Text="[BalanceSheet.actuallastyear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Bold, Underline" Trimming="EllipsisCharacter"/>
            <TextObject Name="Level1AccountNameField" Width="207.9" Height="18.9" Text="[BalanceSheet.accountname]" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
          </ChildBand>
        </ChildBand>
      </ChildBand>
    </DataBand>
    <ReportSummaryBand Name="ReportSummary1" Top="199.55" Width="718.2" Height="18.9">
      <TextObject Name="Text2" Left="340.2" Width="113.4" Height="18.9" Text="[TotalLiabPlusEq]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text52" Width="217.35" Height="18.9" Text="Total Equity + Liabilities" Padding="0, 0, 2, 0" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" WordWrap="false" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
      <TextObject Name="Text4" Left="472.5" Width="113.4" Height="18.9" Text="[TotalLiabPlusEqLastYear]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" Font="Arial, 9pt, style=Bold" Trimming="EllipsisCharacter"/>
    </ReportSummaryBand>
    <PageFooterBand Name="PageFooter1" Top="221.78" Width="718.2" Height="18.9">
      <TextObject Name="Text17" Left="557.55" Width="160.65" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(0,'Income Expense Statement','OpenPetra default template','System',True,True,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="04/16/2014 13:16:19" ReportInfo.CreatorVersion="2013.4.4.0">
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
  </Dictionary>
  <ReportPage Name="Page1" Landscape="true" PaperWidth="297" PaperHeight="210" RawPaperSize="9">
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
        <DataBand Name="TransactionBand" Top="139.52" Width="1047.06" Height="18.9" BeforePrintEvent="Data_BeforePrint" KeepChild="true" DataSource="IncomeExpense">
          <TextObject Name="ActualField" Left="274.05" Width="94.5" Height="18.9" Text="[IncomeExpense.actual]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="BudgetField" Left="368.55" Width="94.5" Height="18.9" Text="[IncomeExpense.budget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="ActualYTDField" Left="510.3" Width="94.5" Height="18.9" Text="[IncomeExpense.actualytd]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="BudgetYTDField" Left="604.8" Width="94.5" Height="18.9" Text="[IncomeExpense.budgetytd]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="VarianceField" Left="463.05" Width="47.25" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actual],[IncomeExpense.budget])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="VarianceYTDField" Left="699.3" Width="56.7" Height="18.9" Exportable="false" Text="[variance([IncomeExpense.actualytd],[IncomeExpense.budgetytd])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="0" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="YearBudgetField" Left="756" Width="94.5" Height="18.9" Text="[IncomeExpense.wholeyearbudget]" HideZeros="true" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="0" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Trimming="EllipsisCharacter"/>
          <TextObject Name="AccountCodeField" Left="45.36" Width="56.7" Height="18.9" Text="[IIf([param_cost_centre_breakdown],[IncomeExpense.costcentrecode],[IncomeExpense.accountcode])]" Padding="0, 0, 0, 0" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="AccountNameField" Left="102.06" Width="170.1" Height="18.9" Text="[IIf([param_cost_centre_breakdown],[IncomeExpense.costcentrename],[IncomeExpense.accountname])]" Padding="0, 0, 2, 0" Font="Arial, 9pt"/>
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
      <TextObject Name="Text17" Left="878.85" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(3,'Trial Balance','OpenPetra default template','System',True,True,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="06/27/2014 09:08:33" ReportInfo.CreatorVersion="2014.2.1.0">
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
      <TextObject Name="Text49" Left="47.25" Width="47.25" Height="18.9" Text="TW"/>
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(23,'Account Detail','OpenPetra default template','System',True,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="06/30/2014 11:27:23" ReportInfo.CreatorVersion="2014.2.1.0">
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
    
    String GroupHeader ()
    {
      switch ((String)Report.GetParameterValue(&quot;param_sortby&quot;))
      {
        case &quot;Cost Centre&quot; :
        case &quot;Account&quot; : return 
             (String)Report.GetColumnValue(&quot;a_transaction.a_cost_centre_code_c&quot;) + &quot;-&quot;
            +(String)Report.GetColumnValue(&quot;a_transaction.a_account_code_c&quot;) + &quot; &quot;
            +(String)Report.GetColumnValue(&quot;a_transaction.a_account.a_account_code_long_desc_c&quot;) + &quot;, &quot;
            +(String)Report.GetColumnValue(&quot;a_transaction.a_costCentre.a_cost_centre_name_c&quot;);
        case &quot;Reference&quot; : return
            &quot;Reference : &quot; + ((String)Report.GetColumnValue(&quot;a_transaction.a_reference_c&quot;));
        case &quot;Analysis Type&quot; : return
             (String)Report.GetColumnValue(&quot;a_transaction.a_analysis_type_code_c&quot;) + &quot;: &quot;
            +(String)Report.GetColumnValue(&quot;a_transaction.a_analysis_type_description_c&quot;);
        default: return &quot;Group&quot;;
      }
    }
    
    //
    // In C#, the &quot;else&quot; is not evaluated if the &quot;if&quot; is true,
    // but in unfortunately the FastReport expression evaluator, it is.
    // So this is moved to here because a_transaction.balances.OpeningBalance
    // is not provided for reports that are not &quot;by period&quot;.
    Object OpeningBalance()
    {
      if ((bool)(Report.GetParameterValue(&quot;param_date_checked&quot;)))
      {
        return &quot;&quot;;
      }
      else
      {
        return Report.GetColumnValue(&quot;a_transaction.balances.OpeningBalance&quot;);
      }
    }
    
    Object ClosingBalance()
    {
      if ((bool)Report.GetParameterValue(&quot;param_date_checked&quot;))
      {
        return &quot;&quot;;
      }
      else
      {
        return Report.GetColumnValue(&quot;a_transaction.balances.ClosingBalance&quot;);
      }
    }
  }
}
</ScriptText>
  <Dictionary>
    <TableDataSource Name="a_analysis_type" ReferenceName="a_analysis_type" DataType="System.Int32" Enabled="true">
      <Column Name="a_analysis_type_code_c" DataType="System.String"/>
      <Column Name="a_analysis_type_description_c" DataType="System.String"/>
      <Column Name="a_analysis_mode_l" DataType="System.String"/>
      <Column Name="a_analysis_store_c" DataType="System.String"/>
      <Column Name="a_analysis_element_c" DataType="System.String"/>
      <Column Name="a_system_analysis_type_l" DataType="System.String"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <TableDataSource Name="a_trans_anal_attrib" ReferenceName="a_trans_anal_attrib" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_journal_number_i" DataType="System.Int32"/>
      <Column Name="a_transaction_number_i" DataType="System.Int32"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_analysis_type_code_c" DataType="System.String"/>
      <Column Name="a_analysis_attribute_value_c" DataType="System.String"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <TableDataSource Name="a_transaction" ReferenceName="a_transaction" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_journal_number_i" DataType="System.Int32"/>
      <Column Name="a_transaction_number_i" DataType="System.Int32"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="a_primary_account_code_c" DataType="System.String"/>
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_primary_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_transaction_date_d" DataType="System.DateTime"/>
      <Column Name="a_transaction_amount_n" DataType="System.Decimal"/>
      <Column Name="a_amount_in_base_currency_n" DataType="System.Decimal"/>
      <Column Name="a_analysis_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_reconciled_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_narrative_c" DataType="System.String"/>
      <Column Name="a_debit_credit_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_transaction_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_header_number_i" DataType="System.Int32"/>
      <Column Name="a_detail_number_i" DataType="System.Int32"/>
      <Column Name="a_sub_type_c" DataType="System.String"/>
      <Column Name="a_to_ilt_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_source_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_reference_c" DataType="System.String"/>
      <Column Name="a_source_reference_c" DataType="System.String"/>
      <Column Name="a_system_generated_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_amount_in_intl_currency_n" DataType="System.Decimal"/>
      <Column Name="a_ich_number_i" DataType="System.Int32"/>
      <Column Name="a_key_ministry_key_n" DataType="System.Int64"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
      <Column Name="a_analysis_type_code_c" DataType="System.String"/>
      <Column Name="a_analysis_type_description_c" DataType="System.String"/>
      <Column Name="a_analysis_attribute_value_c" DataType="System.String"/>
    </TableDataSource>
    <TableDataSource Name="a_account" ReferenceName="a_account" DataType="System.Int32" Enabled="true">
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
    </TableDataSource>
    <TableDataSource Name="a_costCentre" ReferenceName="a_costCentre" DataType="System.Int32" Enabled="true">
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
    </TableDataSource>
    <TableDataSource Name="a_ledger" ReferenceName="a_ledger" DataType="System.Int32" Enabled="true">
      <Column Name="a_ledger_number_i" DataType="System.Int32"/>
      <Column Name="a_ledger_name_c" DataType="System.String"/>
      <Column Name="a_ledger_status_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_last_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_last_recurring_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_last_gift_number_i" DataType="System.Int32"/>
      <Column Name="a_last_ap_inv_number_i" DataType="System.Int32"/>
      <Column Name="a_last_header_r_number_i" DataType="System.Int32"/>
      <Column Name="a_last_po_number_i" DataType="System.Int32"/>
      <Column Name="a_last_so_number_i" DataType="System.Int32"/>
      <Column Name="a_max_gift_aid_amount_n" DataType="System.Decimal"/>
      <Column Name="a_min_gift_aid_amount_n" DataType="System.Decimal"/>
      <Column Name="a_number_of_gifts_to_display_i" DataType="System.Int32"/>
      <Column Name="a_tax_type_code_c" DataType="System.String"/>
      <Column Name="a_ilt_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_profit_loss_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_current_accounting_period_i" DataType="System.Int32"/>
      <Column Name="a_number_of_accounting_periods_i" DataType="System.Int32"/>
      <Column Name="a_country_code_c" DataType="System.String"/>
      <Column Name="a_base_currency_c" DataType="System.String"/>
      <Column Name="a_transaction_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_year_end_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_forex_gains_losses_account_c" DataType="System.String"/>
      <Column Name="a_system_interface_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_suspense_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_bank_accounts_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_delete_ledger_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_new_financial_year_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_recalculate_gl_master_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_installation_id_c" DataType="System.String"/>
      <Column Name="a_budget_control_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_budget_data_retention_i" DataType="System.Int32"/>
      <Column Name="a_cost_of_sales_gl_account_c" DataType="System.String"/>
      <Column Name="a_creditor_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_current_financial_year_i" DataType="System.Int32"/>
      <Column Name="a_current_period_i" DataType="System.Int32"/>
      <Column Name="a_date_cr_dr_balances_d" DataType="System.DateTime"/>
      <Column Name="a_debtor_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_fa_depreciation_gl_account_c" DataType="System.String"/>
      <Column Name="a_fa_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_fa_pl_on_sale_gl_account_c" DataType="System.String"/>
      <Column Name="a_fa_prov_for_depn_gl_account_c" DataType="System.String"/>
      <Column Name="a_ilt_account_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_last_ap_dn_number_i" DataType="System.Int32"/>
      <Column Name="a_last_po_ret_number_i" DataType="System.Int32"/>
      <Column Name="a_last_so_del_number_i" DataType="System.Int32"/>
      <Column Name="a_last_so_ret_number_i" DataType="System.Int32"/>
      <Column Name="a_last_special_gift_number_i" DataType="System.Int32"/>
      <Column Name="a_number_fwd_posting_periods_i" DataType="System.Int32"/>
      <Column Name="a_periods_per_financial_year_i" DataType="System.Int32"/>
      <Column Name="a_discount_allowed_pct_n" DataType="System.Decimal"/>
      <Column Name="a_discount_received_pct_n" DataType="System.Decimal"/>
      <Column Name="a_po_accrual_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_provisional_year_end_flag_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_purchase_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_ret_earnings_gl_account_c" DataType="System.String"/>
      <Column Name="a_sales_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_so_accrual_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_stock_accrual_gl_account_c" DataType="System.String"/>
      <Column Name="a_stock_adj_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_stock_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_tax_excl_incl_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_tax_excl_incl_indicator_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_tax_input_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_tax_input_gl_cc_code_c" DataType="System.String"/>
      <Column Name="a_tax_output_gl_account_code_c" DataType="System.String"/>
      <Column Name="a_terms_of_payment_code_c" DataType="System.String"/>
      <Column Name="a_last_po_rec_number_i" DataType="System.Int32"/>
      <Column Name="a_tax_gl_account_number_i" DataType="System.Int32"/>
      <Column Name="a_actuals_data_retention_i" DataType="System.Int32"/>
      <Column Name="p_partner_key_n" DataType="System.Int64"/>
      <Column Name="a_calendar_mode_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_year_end_process_status_i" DataType="System.Int32"/>
      <Column Name="a_last_header_p_number_i" DataType="System.Int32"/>
      <Column Name="a_ilt_processing_centre_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_last_gift_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_intl_currency_c" DataType="System.String"/>
      <Column Name="a_last_rec_gift_batch_number_i" DataType="System.Int32"/>
      <Column Name="a_gift_data_retention_i" DataType="System.Int32"/>
      <Column Name="a_recalculate_all_periods_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_last_ich_number_i" DataType="System.Int32"/>
      <Column Name="a_branch_processing_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="a_consolidation_ledger_l" DataType="System.Boolean" BindableControl="CheckBox"/>
      <Column Name="s_date_created_d" DataType="System.DateTime"/>
      <Column Name="s_created_by_c" DataType="System.String"/>
      <Column Name="s_date_modified_d" DataType="System.DateTime"/>
      <Column Name="s_modified_by_c" DataType="System.String"/>
      <Column Name="s_modification_id_t" DataType="System.DateTime"/>
    </TableDataSource>
    <TableDataSource Name="balances" ReferenceName="balances" DataType="System.Int32" Enabled="true">
      <Column Name="a_cost_centre_code_c" DataType="System.String"/>
      <Column Name="a_account_code_c" DataType="System.String"/>
      <Column Name="OpeningBalance" DataType="System.Decimal"/>
      <Column Name="ClosingBalance" DataType="System.Decimal"/>
    </TableDataSource>
    <Relation Name="a_account_a_transaction" ParentDataSource="a_account" ChildDataSource="a_transaction" ParentColumns="a_account_code_c" ChildColumns="a_account_code_c" Enabled="true"/>
    <Relation Name="a_costCentre_a_transaction" ParentDataSource="a_costCentre" ChildDataSource="a_transaction" ParentColumns="a_cost_centre_code_c" ChildColumns="a_cost_centre_code_c" Enabled="true"/>
    <Relation Name="a_trans_anal_attrib_a_transaction" ParentDataSource="a_trans_anal_attrib" ChildDataSource="a_transaction" ParentColumns="a_ledger_number_i&#13;&#10;a_batch_number_i&#13;&#10;a_journal_number_i&#13;&#10;a_transaction_number_i" ChildColumns="a_ledger_number_i&#13;&#10;a_batch_number_i&#13;&#10;a_journal_number_i&#13;&#10;a_transaction_number_i" Enabled="true"/>
    <Relation Name="a_analysis_type_a_trans_anal_attrib" ParentDataSource="a_analysis_type" ChildDataSource="a_trans_anal_attrib" ParentColumns="a_analysis_type_code_c" ChildColumns="a_analysis_type_code_c" Enabled="true"/>
    <Relation Name="balances_a_transaction" ParentDataSource="balances" ChildDataSource="a_transaction" ParentColumns="a_cost_centre_code_c&#13;&#10;a_account_code_c" ChildColumns="a_cost_centre_code_c&#13;&#10;a_account_code_c" Enabled="true"/>
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
    <Total Name="GroupDebit" Expression="Debits.Value" Evaluator="Transaction" PrintOn="GroupFooter2"/>
    <Total Name="GroupCredit" Expression="Credits.Value" Evaluator="Transaction" PrintOn="GroupFooter2"/>
    <Total Name="OuterGroupDebit" Expression="Debits.Value" Evaluator="Transaction" PrintOn="GroupFooter1"/>
    <Total Name="OuterGroupCredit" Expression="Credits.Value" Evaluator="Transaction" PrintOn="GroupFooter1"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="85.05">
      <TextObject Name="Text1" Left="245.7" Width="207.9" Height="18.9" Text="Account Detail" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
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
      <TextObject Name="Text22" Top="37.8" Width="75.6" Height="18.9" Text="[IIf([param_period],&quot;Period :&quot;,&quot;Date :&quot;)]" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="75.6" Top="37.8" Width="198.45" Height="18.9" Text="[IIf([param_period],ToString([param_start_period_i])+&quot; - &quot;+ToString([param_end_period_i]), OmDate([param_start_date]) + &quot; - &quot; + OmDate([param_end_date]))]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text42" Left="245.7" Top="18.9" Width="207.9" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <TextObject Name="Text43" Left="274.05" Top="37.8" Width="179.55" Height="18.9"/>
      <LineObject Name="Line1" Left="718.2" Top="75.6" Width="-718.2"/>
      <TextObject Name="Text50" Left="453.6" Top="56.7" Width="103.95" Height="18.9" Text="Ordered By :" HorzAlign="Right"/>
      <TextObject Name="Text51" Left="557.55" Top="56.7" Width="160.65" Height="18.9" Text="[param_sortby]"/>
      <TextObject Name="Text56" Left="75.6" Top="56.7" Width="198.45" Height="18.9" Text="[IIf([param_period],OmDate([param_start_date])+&quot; - &quot;+OmDate([param_end_date]),&quot;&quot;)]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="88.38" Width="718.2"/>
    <GroupHeaderBand Name="GroupHeader1" Top="91.72" Width="718.2" Condition="Switch(&quot;Account&quot;==[param_sortby],[a_transaction.a_account_code_c], &quot;Cost Centre&quot;==[param_sortby], [a_transaction.a_cost_centre_code_c])" SortOrder="None">
      <GroupHeaderBand Name="GroupHeader2" Top="95.05" Width="718.2" Height="47.25" KeepChild="true" KeepWithData="true" Condition="Switch(&quot;Account&quot;==[param_sortby],[a_transaction.a_cost_centre_code_c], &quot;Cost Centre&quot;==[param_sortby], [a_transaction.a_account_code_c], &quot;Reference&quot;==[param_sortby], [a_transaction.a_reference_c], &quot;Analysis Type&quot;==[param_sortby],[a_transaction.a_analysis_type_code_c])" SortOrder="None" KeepTogether="true">
        <TextObject Name="Text15" Top="9.45" Width="718.2" Height="18.9" CanShrink="true" CanBreak="false" Text="[GroupHeader ()]" AutoShrink="FontSize" AutoShrinkMinSize="7" WordWrap="false" Font="Arial, 10pt, style=Bold"/>
        <TextObject Name="Text3" Left="113.4" Top="28.35" Width="85.05" Height="18.9" Text="Date" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text5" Left="198.45" Top="28.35" Width="85.05" Height="18.9" Text="Debits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text7" Left="368.55" Top="28.35" Width="103.95" Height="18.9" Text="Narrative" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text23" Left="283.5" Top="28.35" Width="85.05" Height="18.9" Text="Credits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <TextObject Name="Text40" Top="28.35" Width="9.45" Height="18.9"/>
        <TextObject Name="Text41" Left="9.45" Top="28.35" Width="103.95" Height="18.9" Text="Ref" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
        <DataBand Name="Transaction" Top="145.63" Width="718.2" Height="18.9" CanGrow="true" KeepChild="true" DataSource="a_transaction" KeepDetail="true">
          <TextObject Name="Text30" Width="9.45" Height="18.9" Text="[a_transaction.a_cost_centre_code_c]" TextFill.Color="White"/>
          <TextObject Name="TransRef" Left="9.45" Width="103.95" Height="18.9" Text="[a_transaction.a_reference_c]" AutoShrink="FontSize" AutoShrinkMinSize="7" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt" Clip="false"/>
          <TextObject Name="TransDate" Left="113.4" Width="85.05" Height="18.9" Text="[OmDate([a_transaction.a_transaction_date_d])]" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt"/>
          <TextObject Name="Debits" Left="198.45" Width="85.05" Height="18.9" Text="[IIf ([a_transaction.a_debit_credit_indicator_l]==true,Switch ([param_currency]==&quot;Base&quot;,[a_transaction.a_amount_in_base_currency_n],[param_currency]==&quot;Transaction&quot;,[a_transaction.a_transaction_amount_n],[param_currency]==&quot;International&quot;,[a_transaction.a_amount_in_intl_currency_n]),0)]" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
            <Formats>
              <NumberFormat UseLocale="false" NegativePattern="1"/>
              <GeneralFormat/>
            </Formats>
            <Highlight>
              <Condition Expression="Value == 0" TextFill.Color="White"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Credits" Left="283.5" Width="85.05" Height="18.9" Text="[IIf ([a_transaction.a_debit_credit_indicator_l]==false,Switch ([param_currency]==&quot;Base&quot;,[a_transaction.a_amount_in_base_currency_n],[param_currency]==&quot;Transaction&quot;,[a_transaction.a_transaction_amount_n],[param_currency]==&quot;International&quot;,[a_transaction.a_amount_in_intl_currency_n]),0)]" HorzAlign="Right" WordWrap="false" Font="Arial, 10pt, style=Bold" Trimming="EllipsisCharacter">
            <Formats>
              <NumberFormat UseLocale="false" NegativePattern="1"/>
              <GeneralFormat/>
            </Formats>
            <Highlight>
              <Condition Expression="Value == 0" TextFill.Color="White"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Narrative" Left="368.55" Width="349.65" Height="18.9" CanGrow="true" GrowToBottom="true" Text="[a_transaction.a_narrative_c]" Font="Arial, 9pt" Clip="false"/>
          <DataBand Name="Attributes" Top="167.87" Width="718.2" Height="9.45" CanGrow="true" CanShrink="true">
            <TextObject Name="AttrDescr" Left="368.55" Width="349.65" Height="9.45" CanGrow="true" CanShrink="true" CanBreak="false" Text="[a_transaction.a_trans_anal_attrib.a_analysis_type.a_analysis_type_description_c][a_transaction.a_trans_anal_attrib.a_analysis_attribute_value_c]" Font="Arial, 9pt" TextFill.Color="Green"/>
          </DataBand>
        </DataBand>
        <GroupFooterBand Name="GroupFooter2" Top="180.65" Width="718.2" Height="37.8" KeepChild="true" KeepWithData="true">
          <TextObject Name="GroupDebitTotal" Left="198.45" Width="85.05" Height="18.9" Text="[GroupDebit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
          <TextObject Name="GroupCreditTotal" Left="283.5" Width="85.05" Height="18.9" Text="[GroupCredit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
          <TextObject Name="Text24" Left="103.95" Width="94.5" Height="18.9" Text="Sub-Total :" HorzAlign="Right" Font="Arial, 9pt, style=Italic"/>
          <TextObject Name="Text4" Left="198.45" Top="18.9" Width="85.05" Height="18.9" Text="[ToDecimal([GroupDebit]-[GroupCredit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold">
            <Highlight>
              <Condition Expression="Value &lt;= 0" TextFill.Color="White"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text25" Left="283.5" Top="18.9" Width="85.05" Height="18.9" Text="[ToDecimal([GroupCredit]-[GroupDebit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold">
            <Highlight>
              <Condition Expression="Value &lt; 0" TextFill.Color="White"/>
            </Highlight>
          </TextObject>
          <TextObject Name="Text26" Left="103.95" Top="18.9" Width="94.5" Height="18.9" Text="Balance :" HorzAlign="Right" Font="Arial, 9pt, style=Italic"/>
          <TextObject Name="Text32" Width="9.45" Height="18.9" TextFill.Color="White"/>
          <TextObject Name="Text33" Left="9.45" Width="9.45" Height="18.9" TextFill.Color="White"/>
          <TextObject Name="Text34" Top="18.9" Width="9.45" Height="18.9" TextFill.Color="White"/>
          <TextObject Name="Text35" Left="9.45" Top="18.9" Width="9.45" Height="18.9" TextFill.Color="White"/>
          <TextObject Name="FooterOpeningBalance" Left="453.6" Top="18.9" Width="103.95" Height="18.9" Text="[OpeningBalance()]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
          <TextObject Name="FooterClosingBalance" Left="557.55" Top="18.9" Width="103.95" Height="18.9" Text="[ClosingBalance()]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
          <TextObject Name="Text52" Left="453.6" Width="103.95" Height="18.9" Text="[IIf([param_date_checked],&quot;&quot;,&quot;Opening Balance&quot;)]" HorzAlign="Right" Font="Arial, 8pt, style=Bold"/>
          <TextObject Name="Text53" Left="557.55" Width="103.95" Height="18.9" Text="[IIf([param_date_checked],&quot;&quot;,&quot;Closing Balance&quot;)]" HorzAlign="Right" Font="Arial, 8pt, style=Bold"/>
          <LineObject Name="Line2" Left="661.5" Width="-567" Border.Width="1.5"/>
        </GroupFooterBand>
      </GroupHeaderBand>
      <GroupFooterBand Name="GroupFooter1" Top="221.78" Width="718.2" Height="47.25" KeepChild="true" KeepWithData="true">
        <TextObject Name="Text27" Left="198.45" Top="18.9" Width="85.05" Height="18.9" Text="[ToDecimal([OuterGroupDebit]-[OuterGroupCredit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue">
          <Highlight>
            <Condition Expression="Value &lt;= 0" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="Text28" Left="283.5" Top="18.9" Width="85.05" Height="18.9" Text="[ToDecimal([OuterGroupCredit]-[OuterGroupDebit])]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue">
          <Highlight>
            <Condition Expression="Value &lt; 0" TextFill.Color="White"/>
          </Highlight>
        </TextObject>
        <TextObject Name="f36" Width="9.45" Height="18.9"/>
        <TextObject Name="f37" Left="9.45" Width="9.45" Height="18.9"/>
        <TextObject Name="Text16" Left="103.95" Width="94.5" Height="18.9" CanShrink="true" CanBreak="false" Text="[Switch(&quot;Account&quot;==[param_sortby],[a_transaction.a_account_code_c]+&quot; Total :&quot;,&quot;Cost Centre&quot;==[param_sortby], [a_transaction.a_cost_centre_code_c]+&quot; Total :&quot;,&quot;Reference&quot;==[param_sortby], &quot;Total :&quot;, &quot;Analysis Type&quot;==[param_sortby], &quot;Total :&quot;)]" AutoShrink="FontSize" AutoShrinkMinSize="7" HorzAlign="Right" WordWrap="false" Font="Arial, 9pt, style=Italic" TextFill.Color="Blue"/>
        <TextObject Name="OuterGroupDebitTotal" Left="198.45" Width="85.05" Height="18.9" Text="[OuterGroupDebit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue"/>
        <TextObject Name="OuterGroupCreditTotal" Left="283.5" Width="85.05" Height="18.9" Text="[OuterGroupCredit]" Format="Number" Format.UseLocale="false" Format.DecimalDigits="2" Format.DecimalSeparator="." Format.GroupSeparator="," Format.NegativePattern="1" HorzAlign="Right" Font="Arial, 9pt, style=Bold" TextFill.Color="Blue"/>
        <LineObject Name="Line3" Left="378" Top="36.8" Width="-283.5" Border.Color="Blue" Border.Width="1.5"/>
        <TextObject Name="f38" Top="18.9" Width="9.45" Height="18.9"/>
        <TextObject Name="f39" Left="9.45" Top="18.9" Width="9.45" Height="18.9"/>
        <TextObject Name="f29" Left="18.9" Top="18.9" Width="85.05" Height="18.9"/>
      </GroupFooterBand>
    </GroupHeaderBand>
    <PageFooterBand Name="PageFooter1" Top="272.37" Width="718.2" Height="18.9">
      <TextObject Name="Text44" Width="9.45" Height="18.9"/>
      <TextObject Name="Text45" Left="9.45" Width="9.45" Height="18.9"/>
      <TextObject Name="Text46" Left="18.9" Width="9.45" Height="18.9"/>
      <TextObject Name="Text47" Left="28.35" Width="9.45" Height="18.9"/>
      <TextObject Name="Text48" Left="37.8" Width="9.45" Height="18.9"/>
      <TextObject Name="Text49" Left="47.25" Width="9.45" Height="18.9"/>
      <TextObject Name="Text17" Left="548.1" Width="170.1" Height="18.9" Text="[PageNofM]" HorzAlign="Right" Font="Arial, 9pt, style=Bold"/>
    </PageFooterBand>
  </ReportPage>
</Report>
');
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(6,'Motivation Details','Motivation Details template','System',True,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="07/01/2014 12:07:58" ReportInfo.CreatorVersion="2014.2.1.0">
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
    </TableDataSource>
    <Parameter Name="param_ledger_number_i" DataType="System.Int32"/>
    <Parameter Name="param_ledger_name" DataType="System.String"/>
    <Parameter Name="param_ledger_nunmber" DataType="System.Int32"/>
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
INSERT INTO s_report_template (s_template_id_i,s_report_type_c,s_report_variant_c,s_author_c,s_default_l,s_readonly_l,s_xml_text_c)
VALUES(7,'AFO','OpenPetra default template','System',True,False,
'﻿<?xml version="1.0" encoding="utf-8"?>
<Report ScriptLanguage="CSharp" DoublePass="true" ReportInfo.Created="11/05/2013 15:46:27" ReportInfo.Modified="07/07/2014 11:10:11" ReportInfo.CreatorVersion="2014.2.1.0">
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
    <Total Name="AllActualDebitBase" Expression="[Accounts.ActualDebitBase]" Evaluator="Transaction" PrintOn="GroupFooter1" ResetOnReprint="true"/>
    <Total Name="AllActualCreditBase" Expression="[Accounts.ActualCreditBase]" Evaluator="Transaction" PrintOn="GroupFooter1" ResetOnReprint="true"/>
    <Total Name="AllActualDebitIntl" Expression="[Accounts.ActualDebitIntl]" Evaluator="Transaction" PrintOn="GroupFooter1" ResetOnReprint="true"/>
    <Total Name="AllActualCreditIntl" Expression="[Accounts.ActualCreditIntl]" Evaluator="Transaction" PrintOn="GroupFooter1" ResetOnReprint="true"/>
  </Dictionary>
  <ReportPage Name="Page1">
    <ReportTitleBand Name="ReportTitle1" Width="718.2" Height="66.15">
      <TextObject Name="Text1" Left="245.7" Width="207.9" Height="18.9" Text="AFO Report" HorzAlign="Center" Font="Arial, 14pt, style=Bold"/>
      <TextObject Name="Text9" Left="453.6" Width="122.85" Height="18.9" Text="Printed :" HorzAlign="Right"/>
      <TextObject Name="Text8" Left="576.45" Width="141.75" Height="18.9" Text="[OmDate([Date])]"/>
      <TextObject Name="Text20" Width="75.6" Height="18.9" Text="Ledger :" HorzAlign="Right"/>
      <TextObject Name="Text10" Left="75.6" Width="170.1" Height="18.9" Text="[param_ledger_number_i] [param_ledger_name]"/>
      <TextObject Name="Text22" Top="18.9" Width="75.6" Height="18.9" Text="Period to:" HorzAlign="Right"/>
      <TextObject Name="Text19" Left="75.6" Top="18.9" Width="198.45" Height="18.9" Text="[ToString([param_end_period_i])]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text42" Left="245.7" Top="18.9" Width="207.9" Height="18.9" Text="[param_ledger_name]" HorzAlign="Center"/>
      <TextObject Name="Text43" Left="274.05" Top="37.8" Width="179.55" Height="18.9"/>
      <LineObject Name="Line1" Left="718.2" Top="56.7" Width="-718.2" Border.Width="2"/>
      <TextObject Name="Text56" Left="75.6" Top="37.8" Width="198.45" Height="18.9" Text="[OmDate([param_end_date])]" AutoShrink="FontSize" AutoShrinkMinSize="5" WordWrap="false"/>
      <TextObject Name="Text61" Left="453.6" Top="18.9" Width="122.85" Height="18.9" Text="Current Period :" HorzAlign="Right"/>
      <TextObject Name="Text18" Left="576.45" Top="18.9" Width="94.5" Height="18.9" Text="[param_current_period]"/>
    </ReportTitleBand>
    <PageHeaderBand Name="PageHeader1" Top="70.15" Width="718.2" Height="18.9">
      <TextObject Name="Text7" Left="378" Width="113.4" Height="18.9" Text="[param_base_currency] Credits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text12" Left="491.4" Width="113.4" Height="18.9" Text="[param_intl_currency] Debits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text14" Left="604.8" Width="113.4" Height="18.9" Text="[param_intl_currency] Credits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
      <TextObject Name="Text5" Left="264.6" Width="113.4" Height="18.9" Text="[param_base_currency] Debits" HorzAlign="Right" Font="Arial, 10pt, style=Bold, Italic"/>
    </PageHeaderBand>
    <GroupHeaderBand Name="GroupHeader1" Top="93.05" Width="718.2" Condition="[AllActualDebitBase]">
      <GroupHeaderBand Name="GroupHeader2" Top="97.05" Width="718.2" Condition="[Accounts.a_account_code_c]">
        <DataBand Name="Transaction" Top="101.05" Width="718.2" Height="18.9" CanGrow="true" KeepChild="true" DataSource="Accounts" KeepDetail="true">
          <TextObject Name="Text2" Width="66.15" Height="18.9" Text="[Accounts.a_account_code_c]"/>
          <TextObject Name="Text3" Left="66.15" Width="198.45" Height="18.9" CanGrow="true" Text="[Accounts.a_account_code_short_desc_c]"/>
          <TextObject Name="Text4" Left="264.6" Width="113.4" Height="18.9" Text="[IIf([Accounts.DebitCreditIndicator],[Accounts.ActualDebitBase],null)]" NullValue=" " Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text6" Left="378" Width="113.4" Height="18.9" Text="[IIf([Accounts.DebitCreditIndicator],null,[Accounts.ActualCreditBase])]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text11" Left="491.4" Width="113.4" Height="18.9" Text="[IIf([Accounts.DebitCreditIndicator],[Accounts.ActualDebitIntl],null)]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
          <TextObject Name="Text13" Left="604.8" Width="113.4" Height="18.9" Text="[IIf([Accounts.DebitCreditIndicator],null,[Accounts.ActualCreditIntl])]" Format="Number" Format.UseLocale="true" HorzAlign="Right" WordWrap="false" Trimming="EllipsisCharacter"/>
        </DataBand>
        <GroupFooterBand Name="GroupFooter2" Top="123.95" Width="718.2"/>
      </GroupHeaderBand>
      <GroupFooterBand Name="GroupFooter1" Top="127.95" Width="718.2" Height="66.15">
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
    <PageFooterBand Name="PageFooter1" Top="198.1" Width="718.2" Height="18.9">
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