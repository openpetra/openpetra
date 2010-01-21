/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/

namespace Ict.Petra.Server.MCommon.Data.Cascading
{
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Common;
    using Ict.Common.DB;
    using Ict.Common.Verification;
    using Ict.Common.Data;
    using Ict.Petra.Shared.MCommon.Data;
    using Ict.Petra.Server.MCommon.Data.Access;
    using Ict.Petra.Shared.MSysMan.Data;
    using Ict.Petra.Server.MSysMan.Data.Access;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Server.MPartner.Partner.Data.Access;
    using Ict.Petra.Shared.MFinance.Account.Data;
    using Ict.Petra.Server.MFinance.Account.Data.Access;
    using Ict.Petra.Shared.MPartner.Mailroom.Data;
    using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
    using Ict.Petra.Shared.MFinance.AR.Data;
    using Ict.Petra.Server.MFinance.AR.Data.Access;
    using Ict.Petra.Shared.MFinance.Gift.Data;
    using Ict.Petra.Server.MFinance.Gift.Data.Access;
    using Ict.Petra.Shared.MFinance.AP.Data;
    using Ict.Petra.Server.MFinance.AP.Data.Access;
    using Ict.Petra.Shared.MPersonnel.Personnel.Data;
    using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
    using Ict.Petra.Shared.MPersonnel.Units.Data;
    using Ict.Petra.Server.MPersonnel.Units.Data.Access;
    using Ict.Petra.Shared.MConference.Data;
    using Ict.Petra.Server.MConference.Data.Access;
    using Ict.Petra.Shared.MHospitality.Data;
    using Ict.Petra.Server.MHospitality.Data.Access;

    /// auto generated
    public class AFrequencyCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFrequencyCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPublicationTable MyPPublicationTable = PPublicationAccess.LoadViaAFrequency(AFrequencyCode, StringHelper.StrSplit("p_publication_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPublicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPublicationCascading.DeleteUsingTemplate(MyPPublicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringBatchTable MyARecurringBatchTable = ARecurringBatchAccess.LoadViaAFrequency(AFrequencyCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringBatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringBatchCascading.DeleteUsingTemplate(MyARecurringBatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AFrequencyAccess.DeleteByPrimaryKey(AFrequencyCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPublicationTable MyPPublicationTable = PPublicationAccess.LoadViaAFrequencyTemplate(ATemplateRow, StringHelper.StrSplit("p_publication_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPublicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPublicationCascading.DeleteUsingTemplate(MyPPublicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringBatchTable MyARecurringBatchTable = ARecurringBatchAccess.LoadViaAFrequencyTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringBatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringBatchCascading.DeleteUsingTemplate(MyARecurringBatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AFrequencyAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PInternationalPostalTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AInternatPostalTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PCountryTable MyPCountryTable = PCountryAccess.LoadViaPInternationalPostalType(AInternatPostalTypeCode, StringHelper.StrSplit("p_country_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPCountryTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PCountryAccess.DeleteUsingTemplate(MyPCountryTable[countRow], null, ATransaction);
                                }
            }
            PInternationalPostalTypeAccess.DeleteByPrimaryKey(AInternatPostalTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PCountryTable MyPCountryTable = PCountryAccess.LoadViaPInternationalPostalTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_country_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPCountryTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PCountryAccess.DeleteUsingTemplate(MyPCountryTable[countRow], null, ATransaction);
                                }
            }
            PInternationalPostalTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SFormCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFormName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SValidOutputFormTable MySValidOutputFormTable = SValidOutputFormAccess.LoadViaSForm(AFormName, StringHelper.StrSplit("s_module_id_c,s_form_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySValidOutputFormTable.Rows.Count); countRow = (countRow + 1))
                {
                    SValidOutputFormCascading.DeleteUsingTemplate(MySValidOutputFormTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PLabelTable MyPLabelTable = PLabelAccess.LoadViaSForm(AFormName, StringHelper.StrSplit("p_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPLabelTable.Rows.Count); countRow = (countRow + 1))
                {
                    PLabelCascading.DeleteUsingTemplate(MyPLabelTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SLabelTable MySLabelTable = SLabelAccess.LoadViaSForm(AFormName, StringHelper.StrSplit("p_label_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySLabelTable.Rows.Count); countRow = (countRow + 1))
                {
                    SLabelCascading.DeleteUsingTemplate(MySLabelTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SFormAccess.DeleteByPrimaryKey(AFormName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SFormRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SValidOutputFormTable MySValidOutputFormTable = SValidOutputFormAccess.LoadViaSFormTemplate(ATemplateRow, StringHelper.StrSplit("s_module_id_c,s_form_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySValidOutputFormTable.Rows.Count); countRow = (countRow + 1))
                {
                    SValidOutputFormCascading.DeleteUsingTemplate(MySValidOutputFormTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PLabelTable MyPLabelTable = PLabelAccess.LoadViaSFormTemplate(ATemplateRow, StringHelper.StrSplit("p_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPLabelTable.Rows.Count); countRow = (countRow + 1))
                {
                    PLabelCascading.DeleteUsingTemplate(MyPLabelTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SLabelTable MySLabelTable = SLabelAccess.LoadViaSFormTemplate(ATemplateRow, StringHelper.StrSplit("p_label_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySLabelTable.Rows.Count); countRow = (countRow + 1))
                {
                    SLabelCascading.DeleteUsingTemplate(MySLabelTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SFormAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SFileCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFileName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SReportFileTable MySReportFileTable = SReportFileAccess.LoadViaSFile(AFileName, StringHelper.StrSplit("s_report_file_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySReportFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    SReportFileCascading.DeleteUsingTemplate(MySReportFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SBatchJobTable MySBatchJobTable = SBatchJobAccess.LoadViaSFile(AFileName, StringHelper.StrSplit("s_file_name_c,s_user_id_c,s_date_submitted_d,s_time_submitted_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySBatchJobTable.Rows.Count); countRow = (countRow + 1))
                {
                    SBatchJobCascading.DeleteUsingTemplate(MySBatchJobTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PReportsTable MyPReportsTable = PReportsAccess.LoadViaSFile(AFileName, StringHelper.StrSplit("p_report_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPReportsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PReportsCascading.DeleteUsingTemplate(MyPReportsTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ABudgetTypeTable MyABudgetTypeTable = ABudgetTypeAccess.LoadViaSFile(AFileName, StringHelper.StrSplit("a_budget_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyABudgetTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABudgetTypeCascading.DeleteUsingTemplate(MyABudgetTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AMethodOfPaymentTable MyAMethodOfPaymentTable = AMethodOfPaymentAccess.LoadViaSFile(AFileName, StringHelper.StrSplit("a_method_of_payment_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAMethodOfPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AMethodOfPaymentCascading.DeleteUsingTemplate(MyAMethodOfPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ASpecialTransTypeTable MyASpecialTransTypeSpecTransProcessToCallTable = ASpecialTransTypeAccess.LoadViaSFileSpecTransProcessToCall(AFileName, StringHelper.StrSplit("a_sub_system_code_c,a_transaction_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyASpecialTransTypeSpecTransProcessToCallTable.Rows.Count); countRow = (countRow + 1))
                {
                    ASpecialTransTypeCascading.DeleteUsingTemplate(MyASpecialTransTypeSpecTransProcessToCallTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ASpecialTransTypeTable MyASpecialTransTypeSpecTransUndoProcessTable = ASpecialTransTypeAccess.LoadViaSFileSpecTransUndoProcess(AFileName, StringHelper.StrSplit("a_sub_system_code_c,a_transaction_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyASpecialTransTypeSpecTransUndoProcessTable.Rows.Count); countRow = (countRow + 1))
                {
                    ASpecialTransTypeCascading.DeleteUsingTemplate(MyASpecialTransTypeSpecTransUndoProcessTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PtReportsTable MyPtReportsTable = PtReportsAccess.LoadViaSFile(AFileName, StringHelper.StrSplit("pt_report_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPtReportsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PtReportsCascading.DeleteUsingTemplate(MyPtReportsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SFileAccess.DeleteByPrimaryKey(AFileName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SReportFileTable MySReportFileTable = SReportFileAccess.LoadViaSFileTemplate(ATemplateRow, StringHelper.StrSplit("s_report_file_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySReportFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    SReportFileCascading.DeleteUsingTemplate(MySReportFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SBatchJobTable MySBatchJobTable = SBatchJobAccess.LoadViaSFileTemplate(ATemplateRow, StringHelper.StrSplit("s_file_name_c,s_user_id_c,s_date_submitted_d,s_time_submitted_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySBatchJobTable.Rows.Count); countRow = (countRow + 1))
                {
                    SBatchJobCascading.DeleteUsingTemplate(MySBatchJobTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PReportsTable MyPReportsTable = PReportsAccess.LoadViaSFileTemplate(ATemplateRow, StringHelper.StrSplit("p_report_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPReportsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PReportsCascading.DeleteUsingTemplate(MyPReportsTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ABudgetTypeTable MyABudgetTypeTable = ABudgetTypeAccess.LoadViaSFileTemplate(ATemplateRow, StringHelper.StrSplit("a_budget_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyABudgetTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABudgetTypeCascading.DeleteUsingTemplate(MyABudgetTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AMethodOfPaymentTable MyAMethodOfPaymentTable = AMethodOfPaymentAccess.LoadViaSFileTemplate(ATemplateRow, StringHelper.StrSplit("a_method_of_payment_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAMethodOfPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AMethodOfPaymentCascading.DeleteUsingTemplate(MyAMethodOfPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ASpecialTransTypeTable MyASpecialTransTypeSpecTransProcessToCallTable = ASpecialTransTypeAccess.LoadViaSFileSpecTransProcessToCallTemplate(ATemplateRow, StringHelper.StrSplit("a_sub_system_code_c,a_transaction_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyASpecialTransTypeSpecTransProcessToCallTable.Rows.Count); countRow = (countRow + 1))
                {
                    ASpecialTransTypeCascading.DeleteUsingTemplate(MyASpecialTransTypeSpecTransProcessToCallTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ASpecialTransTypeTable MyASpecialTransTypeSpecTransUndoProcessTable = ASpecialTransTypeAccess.LoadViaSFileSpecTransUndoProcessTemplate(ATemplateRow, StringHelper.StrSplit("a_sub_system_code_c,a_transaction_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyASpecialTransTypeSpecTransUndoProcessTable.Rows.Count); countRow = (countRow + 1))
                {
                    ASpecialTransTypeCascading.DeleteUsingTemplate(MyASpecialTransTypeSpecTransUndoProcessTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PtReportsTable MyPtReportsTable = PtReportsAccess.LoadViaSFileTemplate(ATemplateRow, StringHelper.StrSplit("pt_report_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPtReportsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PtReportsCascading.DeleteUsingTemplate(MyPtReportsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SFileAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SUserGroupCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AUserId, String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SUserGroupAccess.DeleteByPrimaryKey(AUserId, AGroupId, AUnitKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SUserGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SUserGroupAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SModuleCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AModuleId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SValidOutputFormTable MySValidOutputFormTable = SValidOutputFormAccess.LoadViaSModule(AModuleId, StringHelper.StrSplit("s_module_id_c,s_form_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySValidOutputFormTable.Rows.Count); countRow = (countRow + 1))
                {
                    SValidOutputFormCascading.DeleteUsingTemplate(MySValidOutputFormTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupModuleAccessPermissionTable MySGroupModuleAccessPermissionTable = SGroupModuleAccessPermissionAccess.LoadViaSModule(AModuleId, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,s_module_id_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupModuleAccessPermissionTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupModuleAccessPermissionCascading.DeleteUsingTemplate(MySGroupModuleAccessPermissionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SUserModuleAccessPermissionTable MySUserModuleAccessPermissionTable = SUserModuleAccessPermissionAccess.LoadViaSModule(AModuleId, StringHelper.StrSplit("s_user_id_c,s_module_id_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySUserModuleAccessPermissionTable.Rows.Count); countRow = (countRow + 1))
                {
                    SUserModuleAccessPermissionCascading.DeleteUsingTemplate(MySUserModuleAccessPermissionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerContactTable MyPPartnerContactTable = PPartnerContactAccess.LoadViaSModule(AModuleId, StringHelper.StrSplit("p_contact_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactCascading.DeleteUsingTemplate(MyPPartnerContactTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerReminderTable MyPPartnerReminderTable = PPartnerReminderAccess.LoadViaSModule(AModuleId, StringHelper.StrSplit("p_partner_key_n,p_contact_id_i,p_reminder_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerReminderTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerReminderCascading.DeleteUsingTemplate(MyPPartnerReminderTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SModuleAccess.DeleteByPrimaryKey(AModuleId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SModuleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SValidOutputFormTable MySValidOutputFormTable = SValidOutputFormAccess.LoadViaSModuleTemplate(ATemplateRow, StringHelper.StrSplit("s_module_id_c,s_form_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySValidOutputFormTable.Rows.Count); countRow = (countRow + 1))
                {
                    SValidOutputFormCascading.DeleteUsingTemplate(MySValidOutputFormTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupModuleAccessPermissionTable MySGroupModuleAccessPermissionTable = SGroupModuleAccessPermissionAccess.LoadViaSModuleTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,s_module_id_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupModuleAccessPermissionTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupModuleAccessPermissionCascading.DeleteUsingTemplate(MySGroupModuleAccessPermissionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SUserModuleAccessPermissionTable MySUserModuleAccessPermissionTable = SUserModuleAccessPermissionAccess.LoadViaSModuleTemplate(ATemplateRow, StringHelper.StrSplit("s_user_id_c,s_module_id_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySUserModuleAccessPermissionTable.Rows.Count); countRow = (countRow + 1))
                {
                    SUserModuleAccessPermissionCascading.DeleteUsingTemplate(MySUserModuleAccessPermissionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerContactTable MyPPartnerContactTable = PPartnerContactAccess.LoadViaSModuleTemplate(ATemplateRow, StringHelper.StrSplit("p_contact_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactCascading.DeleteUsingTemplate(MyPPartnerContactTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerReminderTable MyPPartnerReminderTable = PPartnerReminderAccess.LoadViaSModuleTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_contact_id_i,p_reminder_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerReminderTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerReminderCascading.DeleteUsingTemplate(MyPPartnerReminderTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SModuleAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SValidOutputFormCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AModuleId, String AFormName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SValidOutputFormAccess.DeleteByPrimaryKey(AModuleId, AFormName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SValidOutputFormRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SValidOutputFormAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SModuleFileCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AModuleId, String AFileName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SModuleFileAccess.DeleteByPrimaryKey(AModuleId, AFileName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SModuleFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SModuleFileAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupModuleAccessPermissionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, String AModuleId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupModuleAccessPermissionAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, AModuleId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupModuleAccessPermissionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupModuleAccessPermissionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupTableAccessPermissionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, String ATableName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupTableAccessPermissionAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, ATableName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupTableAccessPermissionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupTableAccessPermissionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SUserModuleAccessPermissionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AUserId, String AModuleId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SUserModuleAccessPermissionAccess.DeleteByPrimaryKey(AUserId, AModuleId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SUserModuleAccessPermissionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SUserModuleAccessPermissionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SUserTableAccessPermissionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AUserId, String ATableName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SUserTableAccessPermissionAccess.DeleteByPrimaryKey(AUserId, ATableName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SUserTableAccessPermissionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SUserTableAccessPermissionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SLanguageSpecificCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ALanguageCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SLanguageSpecificAccess.DeleteByPrimaryKey(ALanguageCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SLanguageSpecificRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SLanguageSpecificAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SLoginCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AUserId, System.DateTime ALoginDate, Int32 ALoginTime, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SLoginAccess.DeleteByPrimaryKey(AUserId, ALoginDate, ALoginTime, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SLoginRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SLoginAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SLogonMessageCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ALanguageCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SLogonMessageAccess.DeleteByPrimaryKey(ALanguageCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SLogonMessageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SLogonMessageAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SPatchLogCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APatchName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SPatchLogAccess.DeleteByPrimaryKey(APatchName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SPatchLogRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SPatchLogAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SReportsToArchiveCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AReportTitle, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SReportsToArchiveAccess.DeleteByPrimaryKey(AReportTitle, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SReportsToArchiveRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SReportsToArchiveAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SReportFileCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AReportFileName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SReportFileAccess.DeleteByPrimaryKey(AReportFileName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SReportFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SReportFileAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SSystemStatusCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AUserId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SSystemStatusAccess.DeleteByPrimaryKey(AUserId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SSystemStatusRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SSystemStatusAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SUserDefaultsCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AUserId, String ADefaultCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SUserDefaultsAccess.DeleteByPrimaryKey(AUserId, ADefaultCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SUserDefaultsRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SUserDefaultsAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SSystemDefaultsCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ADefaultCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SSystemDefaultsAccess.DeleteByPrimaryKey(ADefaultCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SSystemDefaultsRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SSystemDefaultsAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SBatchJobCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFileName, String AUserId, System.DateTime ADateSubmitted, Int32 ATimeSubmitted, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SBatchJobAccess.DeleteByPrimaryKey(AFileName, AUserId, ADateSubmitted, ATimeSubmitted, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SBatchJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SBatchJobAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SErrorMessageCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ALanguageCode, String AErrorCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SErrorMessageAccess.DeleteByPrimaryKey(ALanguageCode, AErrorCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SErrorMessageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SErrorMessageAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SErrorLogCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AErrorCode, String AUserId, System.DateTime ADate, Int32 ATime, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SErrorLogAccess.DeleteByPrimaryKey(AErrorCode, AUserId, ADate, ATime, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SErrorLogRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SErrorLogAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerStatusCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AStatusCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPPartnerStatus(AStatusCode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
            }
            PPartnerStatusAccess.DeleteByPrimaryKey(AStatusCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerStatusRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPPartnerStatusTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
            }
            PPartnerStatusAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PAcquisitionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AAcquisitionCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPAcquisition(AAcquisitionCode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPAcquisition(AAcquisitionCode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PAcquisitionAccess.DeleteByPrimaryKey(AAcquisitionCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PAcquisitionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPAcquisitionTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPAcquisitionTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PAcquisitionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PAddresseeTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AAddresseeTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PTitleTable MyPTitleTable = PTitleAccess.LoadViaPAddresseeType(AAddresseeTypeCode, StringHelper.StrSplit("p_title_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPTitleTable.Rows.Count); countRow = (countRow + 1))
                {
                    PTitleCascading.DeleteUsingTemplate(MyPTitleTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPAddresseeType(AAddresseeTypeCode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
                PFormalityTable MyPFormalityTable = PFormalityAccess.LoadViaPAddresseeType(AAddresseeTypeCode, StringHelper.StrSplit("p_language_code_c,p_country_code_c,p_addressee_type_code_c,p_formality_level_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormalityTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormalityCascading.DeleteUsingTemplate(MyPFormalityTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PAddresseeTypeAccess.DeleteByPrimaryKey(AAddresseeTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PAddresseeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PTitleTable MyPTitleTable = PTitleAccess.LoadViaPAddresseeTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_title_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPTitleTable.Rows.Count); countRow = (countRow + 1))
                {
                    PTitleCascading.DeleteUsingTemplate(MyPTitleTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPAddresseeTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
                PFormalityTable MyPFormalityTable = PFormalityAccess.LoadViaPAddresseeTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_language_code_c,p_country_code_c,p_addressee_type_code_c,p_formality_level_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormalityTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormalityCascading.DeleteUsingTemplate(MyPFormalityTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PAddresseeTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PTitleCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ATitle, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PTitleAccess.DeleteByPrimaryKey(ATitle, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PTitleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PTitleAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerClassesCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APartnerClass, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPPartnerClasses(APartnerClass, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
                PPartnerFieldOfServiceTable MyPPartnerFieldOfServiceTable = PPartnerFieldOfServiceAccess.LoadViaPPartnerClasses(APartnerClass, StringHelper.StrSplit("p_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerFieldOfServiceTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerFieldOfServiceCascading.DeleteUsingTemplate(MyPPartnerFieldOfServiceTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerClassesAccess.DeleteByPrimaryKey(APartnerClass, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerClassesRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPPartnerClassesTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
                PPartnerFieldOfServiceTable MyPPartnerFieldOfServiceTable = PPartnerFieldOfServiceAccess.LoadViaPPartnerClassesTemplate(ATemplateRow, StringHelper.StrSplit("p_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerFieldOfServiceTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerFieldOfServiceCascading.DeleteUsingTemplate(MyPPartnerFieldOfServiceTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerClassesAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PRecentPartnersCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AUserId, Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PRecentPartnersAccess.DeleteByPrimaryKey(AUserId, APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PRecentPartnersRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PRecentPartnersAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerGraphicCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFileInfoKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerGraphicAccess.DeleteByPrimaryKey(AFileInfoKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerGraphicRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerGraphicAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PLocationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 ASiteKey, Int32 ALocationKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerLocationTable MyPPartnerLocationTable = PPartnerLocationAccess.LoadViaPLocation(ASiteKey, ALocationKey, StringHelper.StrSplit("p_partner_key_n,p_site_key_n,p_location_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerLocationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerLocationCascading.DeleteUsingTemplate(MyPPartnerLocationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                MExtractTable MyMExtractTable = MExtractAccess.LoadViaPLocation(ASiteKey, ALocationKey, StringHelper.StrSplit("m_extract_id_i,p_partner_key_n,p_site_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyMExtractTable.Rows.Count); countRow = (countRow + 1))
                {
                    MExtractCascading.DeleteUsingTemplate(MyMExtractTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupLocationTable MySGroupLocationTable = SGroupLocationAccess.LoadViaPLocation(ASiteKey, ALocationKey, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_site_key_n,p_location_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupLocationTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupLocationCascading.DeleteUsingTemplate(MySGroupLocationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PLocationAccess.DeleteByPrimaryKey(ASiteKey, ALocationKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PLocationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerLocationTable MyPPartnerLocationTable = PPartnerLocationAccess.LoadViaPLocationTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_site_key_n,p_location_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerLocationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerLocationCascading.DeleteUsingTemplate(MyPPartnerLocationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                MExtractTable MyMExtractTable = MExtractAccess.LoadViaPLocationTemplate(ATemplateRow, StringHelper.StrSplit("m_extract_id_i,p_partner_key_n,p_site_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyMExtractTable.Rows.Count); countRow = (countRow + 1))
                {
                    MExtractCascading.DeleteUsingTemplate(MyMExtractTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupLocationTable MySGroupLocationTable = SGroupLocationAccess.LoadViaPLocationTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_site_key_n,p_location_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupLocationTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupLocationCascading.DeleteUsingTemplate(MySGroupLocationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PLocationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PLocationTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerLocationTable MyPPartnerLocationTable = PPartnerLocationAccess.LoadViaPLocationType(ACode, StringHelper.StrSplit("p_partner_key_n,p_site_key_n,p_location_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerLocationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerLocationCascading.DeleteUsingTemplate(MyPPartnerLocationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PLocationTypeAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PLocationTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerLocationTable MyPPartnerLocationTable = PPartnerLocationAccess.LoadViaPLocationTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_site_key_n,p_location_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerLocationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerLocationCascading.DeleteUsingTemplate(MyPPartnerLocationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PLocationTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerLocationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int64 ASiteKey, Int32 ALocationKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SGroupPartnerLocationTable MySGroupPartnerLocationTable = SGroupPartnerLocationAccess.LoadViaPPartnerLocation(APartnerKey, ASiteKey, ALocationKey, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_partner_key_n,p_site_key_n,p_location_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupPartnerLocationTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupPartnerLocationCascading.DeleteUsingTemplate(MySGroupPartnerLocationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerLocationAccess.DeleteByPrimaryKey(APartnerKey, ASiteKey, ALocationKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerLocationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SGroupPartnerLocationTable MySGroupPartnerLocationTable = SGroupPartnerLocationAccess.LoadViaPPartnerLocationTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_partner_key_n,p_site_key_n,p_location_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupPartnerLocationTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupPartnerLocationCascading.DeleteUsingTemplate(MySGroupPartnerLocationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerLocationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerAttributeTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerAttributeTable MyPPartnerAttributeTable = PPartnerAttributeAccess.LoadViaPPartnerAttributeType(ACode, StringHelper.StrSplit("p_partner_key_n,p_code_c,p_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerAttributeCascading.DeleteUsingTemplate(MyPPartnerAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerAttributeTypeAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerAttributeTable MyPPartnerAttributeTable = PPartnerAttributeAccess.LoadViaPPartnerAttributeTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_code_c,p_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerAttributeCascading.DeleteUsingTemplate(MyPPartnerAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerAttributeTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerAttributeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String ACode, Int32 ASequence, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerAttributeAccess.DeleteByPrimaryKey(APartnerKey, ACode, ASequence, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerAttributeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UUnitTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AUnitTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PUnitTable MyPUnitTable = PUnitAccess.LoadViaUUnitType(AUnitTypeCode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPUnitTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PUnitAccess.DeleteUsingTemplate(MyPUnitTable[countRow], null, ATransaction);
                                }
                PtPositionTable MyPtPositionTable = PtPositionAccess.LoadViaUUnitType(AUnitTypeCode, StringHelper.StrSplit("pt_position_name_c,pt_position_scope_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPtPositionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PtPositionCascading.DeleteUsingTemplate(MyPtPositionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            UUnitTypeAccess.DeleteByPrimaryKey(AUnitTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PUnitTable MyPUnitTable = PUnitAccess.LoadViaUUnitTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPUnitTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PUnitAccess.DeleteUsingTemplate(MyPUnitTable[countRow], null, ATransaction);
                                }
                PtPositionTable MyPtPositionTable = PtPositionAccess.LoadViaUUnitTypeTemplate(ATemplateRow, StringHelper.StrSplit("pt_position_name_c,pt_position_scope_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPtPositionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PtPositionCascading.DeleteUsingTemplate(MyPtPositionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            UUnitTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmUnitStructureCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AParentUnitKey, Int64 AChildUnitKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitStructureAccess.DeleteByPrimaryKey(AParentUnitKey, AChildUnitKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmUnitStructureRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitStructureAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFamilyCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPersonTable MyPPersonTable = PPersonAccess.LoadViaPFamily(APartnerKey, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPersonTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPersonAccess.DeleteUsingTemplate(MyPPersonTable[countRow], null, ATransaction);
                                }
            }
            PFamilyAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFamilyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPersonTable MyPPersonTable = PPersonAccess.LoadViaPFamilyTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPersonTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPersonAccess.DeleteUsingTemplate(MyPPersonTable[countRow], null, ATransaction);
                                }
            }
            PFamilyAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtMaritalStatusCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFamilyTable MyPFamilyTable = PFamilyAccess.LoadViaPtMaritalStatus(ACode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFamilyTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFamilyCascading.DeleteUsingTemplate(MyPFamilyTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPersonTable MyPPersonTable = PPersonAccess.LoadViaPtMaritalStatus(ACode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPersonTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPersonAccess.DeleteUsingTemplate(MyPPersonTable[countRow], null, ATransaction);
                                }
            }
            PtMaritalStatusAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtMaritalStatusRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFamilyTable MyPFamilyTable = PFamilyAccess.LoadViaPtMaritalStatusTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFamilyTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFamilyCascading.DeleteUsingTemplate(MyPFamilyTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPersonTable MyPPersonTable = PPersonAccess.LoadViaPtMaritalStatusTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPersonTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPersonAccess.DeleteUsingTemplate(MyPPersonTable[countRow], null, ATransaction);
                                }
            }
            PtMaritalStatusAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class POccupationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AOccupationCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPersonTable MyPPersonTable = PPersonAccess.LoadViaPOccupation(AOccupationCode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPersonTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPersonAccess.DeleteUsingTemplate(MyPPersonTable[countRow], null, ATransaction);
                                }
            }
            POccupationAccess.DeleteByPrimaryKey(AOccupationCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(POccupationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPersonTable MyPPersonTable = PPersonAccess.LoadViaPOccupationTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPersonTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPersonAccess.DeleteUsingTemplate(MyPPersonTable[countRow], null, ATransaction);
                                }
            }
            POccupationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PDenominationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ADenominationCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PChurchTable MyPChurchTable = PChurchAccess.LoadViaPDenomination(ADenominationCode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPChurchTable.Rows.Count); countRow = (countRow + 1))
                {
                    PChurchCascading.DeleteUsingTemplate(MyPChurchTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PDenominationAccess.DeleteByPrimaryKey(ADenominationCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PDenominationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PChurchTable MyPChurchTable = PChurchAccess.LoadViaPDenominationTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPChurchTable.Rows.Count); countRow = (countRow + 1))
                {
                    PChurchCascading.DeleteUsingTemplate(MyPChurchTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PDenominationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PChurchCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PChurchAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PChurchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PChurchAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PBusinessCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ABusinessCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                POrganisationTable MyPOrganisationTable = POrganisationAccess.LoadViaPBusiness(ABusinessCode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPOrganisationTable.Rows.Count); countRow = (countRow + 1))
                {
                    POrganisationCascading.DeleteUsingTemplate(MyPOrganisationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PBusinessAccess.DeleteByPrimaryKey(ABusinessCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PBusinessRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                POrganisationTable MyPOrganisationTable = POrganisationAccess.LoadViaPBusinessTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPOrganisationTable.Rows.Count); countRow = (countRow + 1))
                {
                    POrganisationCascading.DeleteUsingTemplate(MyPOrganisationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PBusinessAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class POrganisationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationTable MyPFoundationTable = PFoundationAccess.LoadViaPOrganisation(APartnerKey, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationCascading.DeleteUsingTemplate(MyPFoundationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            POrganisationAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(POrganisationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationTable MyPFoundationTable = PFoundationAccess.LoadViaPOrganisationTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationCascading.DeleteUsingTemplate(MyPFoundationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            POrganisationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PBankCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PBankingDetailsTable MyPBankingDetailsTable = PBankingDetailsAccess.LoadViaPBank(APartnerKey, StringHelper.StrSplit("p_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPBankingDetailsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PBankingDetailsCascading.DeleteUsingTemplate(MyPBankingDetailsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PBankAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PBankRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PBankingDetailsTable MyPBankingDetailsTable = PBankingDetailsAccess.LoadViaPBankTemplate(ATemplateRow, StringHelper.StrSplit("p_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPBankingDetailsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PBankingDetailsCascading.DeleteUsingTemplate(MyPBankingDetailsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PBankAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PVenueCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcBuildingTable MyPcBuildingTable = PcBuildingAccess.LoadViaPVenue(APartnerKey, StringHelper.StrSplit("p_venue_key_n,pc_building_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcBuildingTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcBuildingCascading.DeleteUsingTemplate(MyPcBuildingTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcConferenceVenueTable MyPcConferenceVenueTable = PcConferenceVenueAccess.LoadViaPVenue(APartnerKey, StringHelper.StrSplit("pc_conference_key_n,p_venue_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceVenueTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceVenueCascading.DeleteUsingTemplate(MyPcConferenceVenueTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PVenueAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcBuildingTable MyPcBuildingTable = PcBuildingAccess.LoadViaPVenueTemplate(ATemplateRow, StringHelper.StrSplit("p_venue_key_n,pc_building_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcBuildingTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcBuildingCascading.DeleteUsingTemplate(MyPcBuildingTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcConferenceVenueTable MyPcConferenceVenueTable = PcConferenceVenueAccess.LoadViaPVenueTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,p_venue_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceVenueTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceVenueCascading.DeleteUsingTemplate(MyPcConferenceVenueTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PVenueAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PBankingTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PBankingDetailsTable MyPBankingDetailsTable = PBankingDetailsAccess.LoadViaPBankingType(AId, StringHelper.StrSplit("p_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPBankingDetailsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PBankingDetailsCascading.DeleteUsingTemplate(MyPBankingDetailsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PBankingTypeAccess.DeleteByPrimaryKey(AId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PBankingTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PBankingDetailsTable MyPBankingDetailsTable = PBankingDetailsAccess.LoadViaPBankingTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPBankingDetailsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PBankingDetailsCascading.DeleteUsingTemplate(MyPBankingDetailsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PBankingTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PBankingDetailsCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ABankingDetailsKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerBankingDetailsTable MyPPartnerBankingDetailsTable = PPartnerBankingDetailsAccess.LoadViaPBankingDetails(ABankingDetailsKey, StringHelper.StrSplit("p_partner_key_n,p_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerBankingDetailsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerBankingDetailsCascading.DeleteUsingTemplate(MyPPartnerBankingDetailsTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AEpAccountTable MyAEpAccountTable = AEpAccountAccess.LoadViaPBankingDetails(ABankingDetailsKey, StringHelper.StrSplit("a_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpAccountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpAccountCascading.DeleteUsingTemplate(MyAEpAccountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AEpStatementTable MyAEpStatementTable = AEpStatementAccess.LoadViaPBankingDetails(ABankingDetailsKey, StringHelper.StrSplit("a_statement_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpStatementTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpStatementCascading.DeleteUsingTemplate(MyAEpStatementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AAccountTable MyAAccountTable = AAccountAccess.LoadViaPBankingDetails(ABankingDetailsKey, StringHelper.StrSplit("a_ledger_number_i,a_account_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAccountTable.Rows.Count); countRow = (countRow + 1))
                {
                                    AAccountAccess.DeleteUsingTemplate(MyAAccountTable[countRow], null, ATransaction);
                                }
            }
            PBankingDetailsAccess.DeleteByPrimaryKey(ABankingDetailsKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PBankingDetailsRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerBankingDetailsTable MyPPartnerBankingDetailsTable = PPartnerBankingDetailsAccess.LoadViaPBankingDetailsTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerBankingDetailsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerBankingDetailsCascading.DeleteUsingTemplate(MyPPartnerBankingDetailsTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AEpAccountTable MyAEpAccountTable = AEpAccountAccess.LoadViaPBankingDetailsTemplate(ATemplateRow, StringHelper.StrSplit("a_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpAccountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpAccountCascading.DeleteUsingTemplate(MyAEpAccountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AEpStatementTable MyAEpStatementTable = AEpStatementAccess.LoadViaPBankingDetailsTemplate(ATemplateRow, StringHelper.StrSplit("a_statement_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpStatementTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpStatementCascading.DeleteUsingTemplate(MyAEpStatementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AAccountTable MyAAccountTable = AAccountAccess.LoadViaPBankingDetailsTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_account_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAccountTable.Rows.Count); countRow = (countRow + 1))
                {
                                    AAccountAccess.DeleteUsingTemplate(MyAAccountTable[countRow], null, ATransaction);
                                }
            }
            PBankingDetailsAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerBankingDetailsCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 ABankingDetailsKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PBankingDetailsUsageTable MyPBankingDetailsUsageTable = PBankingDetailsUsageAccess.LoadViaPPartnerBankingDetails(APartnerKey, ABankingDetailsKey, StringHelper.StrSplit("p_partner_key_n,p_banking_details_key_i,p_type_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPBankingDetailsUsageTable.Rows.Count); countRow = (countRow + 1))
                {
                    PBankingDetailsUsageCascading.DeleteUsingTemplate(MyPBankingDetailsUsageTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerBankingDetailsAccess.DeleteByPrimaryKey(APartnerKey, ABankingDetailsKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerBankingDetailsRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PBankingDetailsUsageTable MyPBankingDetailsUsageTable = PBankingDetailsUsageAccess.LoadViaPPartnerBankingDetailsTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_banking_details_key_i,p_type_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPBankingDetailsUsageTable.Rows.Count); countRow = (countRow + 1))
                {
                    PBankingDetailsUsageCascading.DeleteUsingTemplate(MyPBankingDetailsUsageTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerBankingDetailsAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PBankingDetailsUsageTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AType, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PBankingDetailsUsageTable MyPBankingDetailsUsageTable = PBankingDetailsUsageAccess.LoadViaPBankingDetailsUsageType(AType, StringHelper.StrSplit("p_partner_key_n,p_banking_details_key_i,p_type_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPBankingDetailsUsageTable.Rows.Count); countRow = (countRow + 1))
                {
                    PBankingDetailsUsageCascading.DeleteUsingTemplate(MyPBankingDetailsUsageTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PBankingDetailsUsageTypeAccess.DeleteByPrimaryKey(AType, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PBankingDetailsUsageTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PBankingDetailsUsageTable MyPBankingDetailsUsageTable = PBankingDetailsUsageAccess.LoadViaPBankingDetailsUsageTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_banking_details_key_i,p_type_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPBankingDetailsUsageTable.Rows.Count); countRow = (countRow + 1))
                {
                    PBankingDetailsUsageCascading.DeleteUsingTemplate(MyPBankingDetailsUsageTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PBankingDetailsUsageTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PBankingDetailsUsageCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 ABankingDetailsKey, String AType, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PBankingDetailsUsageAccess.DeleteByPrimaryKey(APartnerKey, ABankingDetailsKey, AType, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PBankingDetailsUsageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PBankingDetailsUsageAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AEpAccountCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ABankingDetailsKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AEpAccountAccess.DeleteByPrimaryKey(ABankingDetailsKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AEpAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AEpAccountAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AEpStatementCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AStatementKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpTransactionTable MyAEpTransactionTable = AEpTransactionAccess.LoadViaAEpStatement(AStatementKey, StringHelper.StrSplit("a_statement_key_i,a_order_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpTransactionCascading.DeleteUsingTemplate(MyAEpTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AEpStatementAccess.DeleteByPrimaryKey(AStatementKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AEpStatementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpTransactionTable MyAEpTransactionTable = AEpTransactionAccess.LoadViaAEpStatementTemplate(ATemplateRow, StringHelper.StrSplit("a_statement_key_i,a_order_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpTransactionCascading.DeleteUsingTemplate(MyAEpTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AEpStatementAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AEpMatchCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AEpMatchKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpTransactionTable MyAEpTransactionTable = AEpTransactionAccess.LoadViaAEpMatch(AEpMatchKey, StringHelper.StrSplit("a_statement_key_i,a_order_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpTransactionCascading.DeleteUsingTemplate(MyAEpTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AEpMatchAccess.DeleteByPrimaryKey(AEpMatchKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AEpMatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpTransactionTable MyAEpTransactionTable = AEpTransactionAccess.LoadViaAEpMatchTemplate(ATemplateRow, StringHelper.StrSplit("a_statement_key_i,a_order_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpTransactionCascading.DeleteUsingTemplate(MyAEpTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AEpMatchAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AEpTransactionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AStatementKey, Int32 AOrder, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AEpTransactionAccess.DeleteByPrimaryKey(AStatementKey, AOrder, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AEpTransactionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AEpTransactionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ATypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTypeTable MyPPartnerTypeTable = PPartnerTypeAccess.LoadViaPType(ATypeCode, StringHelper.StrSplit("p_partner_key_n,p_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerTypeCascading.DeleteUsingTemplate(MyPPartnerTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDiscountTable MyAArDiscountTable = AArDiscountAccess.LoadViaPType(ATypeCode, StringHelper.StrSplit("a_ar_discount_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDiscountCascading.DeleteUsingTemplate(MyAArDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PTypeAccess.DeleteByPrimaryKey(ATypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTypeTable MyPPartnerTypeTable = PPartnerTypeAccess.LoadViaPTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerTypeCascading.DeleteUsingTemplate(MyPPartnerTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDiscountTable MyAArDiscountTable = AArDiscountAccess.LoadViaPTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ar_discount_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDiscountCascading.DeleteUsingTemplate(MyAArDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PTypeCategoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PTypeTable MyPTypeTable = PTypeAccess.LoadViaPTypeCategory(ACode, StringHelper.StrSplit("p_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PTypeCascading.DeleteUsingTemplate(MyPTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PTypeCategoryAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PTypeCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PTypeTable MyPTypeTable = PTypeAccess.LoadViaPTypeCategoryTemplate(ATemplateRow, StringHelper.StrSplit("p_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PTypeCascading.DeleteUsingTemplate(MyPTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PTypeCategoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String ATypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerTypeAccess.DeleteByPrimaryKey(APartnerKey, ATypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PRelationCategoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PRelationTable MyPRelationTable = PRelationAccess.LoadViaPRelationCategory(ACode, StringHelper.StrSplit("p_relation_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPRelationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PRelationCascading.DeleteUsingTemplate(MyPRelationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PRelationCategoryAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PRelationCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PRelationTable MyPRelationTable = PRelationAccess.LoadViaPRelationCategoryTemplate(ATemplateRow, StringHelper.StrSplit("p_relation_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPRelationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PRelationCascading.DeleteUsingTemplate(MyPRelationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PRelationCategoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PRelationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ARelationName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerRelationshipTable MyPPartnerRelationshipTable = PPartnerRelationshipAccess.LoadViaPRelation(ARelationName, StringHelper.StrSplit("p_partner_key_n,p_relation_name_c,p_relation_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerRelationshipTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerRelationshipCascading.DeleteUsingTemplate(MyPPartnerRelationshipTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PRelationAccess.DeleteByPrimaryKey(ARelationName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PRelationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerRelationshipTable MyPPartnerRelationshipTable = PPartnerRelationshipAccess.LoadViaPRelationTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_relation_name_c,p_relation_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerRelationshipTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerRelationshipCascading.DeleteUsingTemplate(MyPPartnerRelationshipTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PRelationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerRelationshipCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String ARelationName, Int64 ARelationKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerRelationshipAccess.DeleteByPrimaryKey(APartnerKey, ARelationName, ARelationKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerRelationshipRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerRelationshipAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PReportsCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AReportName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PReportsAccess.DeleteByPrimaryKey(AReportName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PReportsRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PReportsAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerLedgerCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerLedgerAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerLedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerLedgerAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class MExtractMasterCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AExtractId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                MExtractTable MyMExtractTable = MExtractAccess.LoadViaMExtractMaster(AExtractId, StringHelper.StrSplit("m_extract_id_i,p_partner_key_n,p_site_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyMExtractTable.Rows.Count); countRow = (countRow + 1))
                {
                    MExtractCascading.DeleteUsingTemplate(MyMExtractTable[countRow], null, ATransaction, AWithCascDelete);
                }
                MExtractParameterTable MyMExtractParameterTable = MExtractParameterAccess.LoadViaMExtractMaster(AExtractId, StringHelper.StrSplit("m_extract_id_i,m_parameter_code_c,m_value_index_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyMExtractParameterTable.Rows.Count); countRow = (countRow + 1))
                {
                    MExtractParameterCascading.DeleteUsingTemplate(MyMExtractParameterTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFormLetterInsertTable MyPFormLetterInsertTable = PFormLetterInsertAccess.LoadViaMExtractMaster(AExtractId, StringHelper.StrSplit("p_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormLetterInsertTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormLetterInsertCascading.DeleteUsingTemplate(MyPFormLetterInsertTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupExtractTable MySGroupExtractTable = SGroupExtractAccess.LoadViaMExtractMaster(AExtractId, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,m_extract_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupExtractTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupExtractCascading.DeleteUsingTemplate(MySGroupExtractTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            MExtractMasterAccess.DeleteByPrimaryKey(AExtractId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                MExtractTable MyMExtractTable = MExtractAccess.LoadViaMExtractMasterTemplate(ATemplateRow, StringHelper.StrSplit("m_extract_id_i,p_partner_key_n,p_site_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyMExtractTable.Rows.Count); countRow = (countRow + 1))
                {
                    MExtractCascading.DeleteUsingTemplate(MyMExtractTable[countRow], null, ATransaction, AWithCascDelete);
                }
                MExtractParameterTable MyMExtractParameterTable = MExtractParameterAccess.LoadViaMExtractMasterTemplate(ATemplateRow, StringHelper.StrSplit("m_extract_id_i,m_parameter_code_c,m_value_index_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyMExtractParameterTable.Rows.Count); countRow = (countRow + 1))
                {
                    MExtractParameterCascading.DeleteUsingTemplate(MyMExtractParameterTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFormLetterInsertTable MyPFormLetterInsertTable = PFormLetterInsertAccess.LoadViaMExtractMasterTemplate(ATemplateRow, StringHelper.StrSplit("p_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormLetterInsertTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormLetterInsertCascading.DeleteUsingTemplate(MyPFormLetterInsertTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupExtractTable MySGroupExtractTable = SGroupExtractAccess.LoadViaMExtractMasterTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,m_extract_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupExtractTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupExtractCascading.DeleteUsingTemplate(MySGroupExtractTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            MExtractMasterAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class MExtractCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AExtractId, Int64 APartnerKey, Int64 ASiteKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            MExtractAccess.DeleteByPrimaryKey(AExtractId, APartnerKey, ASiteKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(MExtractRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            MExtractAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class MExtractTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                MExtractMasterTable MyMExtractMasterTable = MExtractMasterAccess.LoadViaMExtractType(ACode, StringHelper.StrSplit("m_extract_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyMExtractMasterTable.Rows.Count); countRow = (countRow + 1))
                {
                    MExtractMasterCascading.DeleteUsingTemplate(MyMExtractMasterTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            MExtractTypeAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                MExtractMasterTable MyMExtractMasterTable = MExtractMasterAccess.LoadViaMExtractTypeTemplate(ATemplateRow, StringHelper.StrSplit("m_extract_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyMExtractMasterTable.Rows.Count); countRow = (countRow + 1))
                {
                    MExtractMasterCascading.DeleteUsingTemplate(MyMExtractMasterTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            MExtractTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class MExtractParameterCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AExtractId, String AParameterCode, Int32 AValueIndex, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            MExtractParameterAccess.DeleteByPrimaryKey(AExtractId, AParameterCode, AValueIndex, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(MExtractParameterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            MExtractParameterAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PMailingCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AMailingCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpMatchTable MyAEpMatchTable = AEpMatchAccess.LoadViaPMailing(AMailingCode, StringHelper.StrSplit("a_ep_match_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpMatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpMatchCascading.DeleteUsingTemplate(MyAEpMatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerContactTable MyPPartnerContactTable = PPartnerContactAccess.LoadViaPMailing(AMailingCode, StringHelper.StrSplit("p_contact_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactCascading.DeleteUsingTemplate(MyPPartnerContactTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringGiftDetailTable MyARecurringGiftDetailTable = ARecurringGiftDetailAccess.LoadViaPMailing(AMailingCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftDetailCascading.DeleteUsingTemplate(MyARecurringGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AGiftDetailTable MyAGiftDetailTable = AGiftDetailAccess.LoadViaPMailing(AMailingCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftDetailCascading.DeleteUsingTemplate(MyAGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PMailingAccess.DeleteByPrimaryKey(AMailingCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpMatchTable MyAEpMatchTable = AEpMatchAccess.LoadViaPMailingTemplate(ATemplateRow, StringHelper.StrSplit("a_ep_match_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpMatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpMatchCascading.DeleteUsingTemplate(MyAEpMatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerContactTable MyPPartnerContactTable = PPartnerContactAccess.LoadViaPMailingTemplate(ATemplateRow, StringHelper.StrSplit("p_contact_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactCascading.DeleteUsingTemplate(MyPPartnerContactTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringGiftDetailTable MyARecurringGiftDetailTable = ARecurringGiftDetailAccess.LoadViaPMailingTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftDetailCascading.DeleteUsingTemplate(MyARecurringGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AGiftDetailTable MyAGiftDetailTable = AGiftDetailAccess.LoadViaPMailingTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftDetailCascading.DeleteUsingTemplate(MyAGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PMailingAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PAddressLayoutCodeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PAddressLayoutTable MyPAddressLayoutTable = PAddressLayoutAccess.LoadViaPAddressLayoutCode(ACode, StringHelper.StrSplit("p_country_code_c,p_address_layout_code_c,p_address_line_number_i,p_address_line_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPAddressLayoutTable.Rows.Count); countRow = (countRow + 1))
                {
                    PAddressLayoutCascading.DeleteUsingTemplate(MyPAddressLayoutTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFormLetterDesignTable MyPFormLetterDesignTable = PFormLetterDesignAccess.LoadViaPAddressLayoutCode(ACode, StringHelper.StrSplit("p_design_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormLetterDesignTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormLetterDesignCascading.DeleteUsingTemplate(MyPFormLetterDesignTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PAddressLayoutCodeAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PAddressLayoutTable MyPAddressLayoutTable = PAddressLayoutAccess.LoadViaPAddressLayoutCodeTemplate(ATemplateRow, StringHelper.StrSplit("p_country_code_c,p_address_layout_code_c,p_address_line_number_i,p_address_line_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPAddressLayoutTable.Rows.Count); countRow = (countRow + 1))
                {
                    PAddressLayoutCascading.DeleteUsingTemplate(MyPAddressLayoutTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFormLetterDesignTable MyPFormLetterDesignTable = PFormLetterDesignAccess.LoadViaPAddressLayoutCodeTemplate(ATemplateRow, StringHelper.StrSplit("p_design_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormLetterDesignTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormLetterDesignCascading.DeleteUsingTemplate(MyPFormLetterDesignTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PAddressLayoutCodeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PAddressLayoutCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACountryCode, String AAddressLayoutCode, Int32 AAddressLineNumber, String AAddressLineCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PAddressLayoutAccess.DeleteByPrimaryKey(ACountryCode, AAddressLayoutCode, AAddressLineNumber, AAddressLineCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PAddressLayoutRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PAddressLayoutAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PAddressElementCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AAddressElementCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PAddressLineTable MyPAddressLineTable = PAddressLineAccess.LoadViaPAddressElement(AAddressElementCode, StringHelper.StrSplit("p_address_line_code_c,p_address_element_position_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPAddressLineTable.Rows.Count); countRow = (countRow + 1))
                {
                    PAddressLineCascading.DeleteUsingTemplate(MyPAddressLineTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PAddressElementAccess.DeleteByPrimaryKey(AAddressElementCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PAddressLineTable MyPAddressLineTable = PAddressLineAccess.LoadViaPAddressElementTemplate(ATemplateRow, StringHelper.StrSplit("p_address_line_code_c,p_address_element_position_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPAddressLineTable.Rows.Count); countRow = (countRow + 1))
                {
                    PAddressLineCascading.DeleteUsingTemplate(MyPAddressLineTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PAddressElementAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PAddressLineCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AAddressLineCode, Int32 AAddressElementPosition, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PAddressLineAccess.DeleteByPrimaryKey(AAddressLineCode, AAddressElementPosition, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PAddressLineRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PAddressLineAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PAddresseeTitleOverrideCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ALanguageCode, String ATitle, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PAddresseeTitleOverrideAccess.DeleteByPrimaryKey(ALanguageCode, ATitle, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PAddresseeTitleOverrideRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PAddresseeTitleOverrideAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PCustomisedGreetingCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AUserId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PCustomisedGreetingAccess.DeleteByPrimaryKey(APartnerKey, AUserId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PCustomisedGreetingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PCustomisedGreetingAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFormalityCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ALanguageCode, String ACountryCode, String AAddresseeTypeCode, Int32 AFormalityLevel, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFormalityAccess.DeleteByPrimaryKey(ALanguageCode, ACountryCode, AAddresseeTypeCode, AFormalityLevel, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFormalityRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFormalityAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFormLetterBodyCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ABodyName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFormLetterDesignTable MyPFormLetterDesignTable = PFormLetterDesignAccess.LoadViaPFormLetterBody(ABodyName, StringHelper.StrSplit("p_design_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormLetterDesignTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormLetterDesignCascading.DeleteUsingTemplate(MyPFormLetterDesignTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFormLetterInsertTable MyPFormLetterInsertTable = PFormLetterInsertAccess.LoadViaPFormLetterBody(ABodyName, StringHelper.StrSplit("p_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormLetterInsertTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormLetterInsertCascading.DeleteUsingTemplate(MyPFormLetterInsertTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFormLetterBodyAccess.DeleteByPrimaryKey(ABodyName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFormLetterDesignTable MyPFormLetterDesignTable = PFormLetterDesignAccess.LoadViaPFormLetterBodyTemplate(ATemplateRow, StringHelper.StrSplit("p_design_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormLetterDesignTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormLetterDesignCascading.DeleteUsingTemplate(MyPFormLetterDesignTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFormLetterInsertTable MyPFormLetterInsertTable = PFormLetterInsertAccess.LoadViaPFormLetterBodyTemplate(ATemplateRow, StringHelper.StrSplit("p_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFormLetterInsertTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFormLetterInsertCascading.DeleteUsingTemplate(MyPFormLetterInsertTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFormLetterBodyAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFormLetterDesignCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ADesignName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFormLetterDesignAccess.DeleteByPrimaryKey(ADesignName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFormLetterDesignRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFormLetterDesignAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFormLetterInsertCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ASequence, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFormLetterInsertAccess.DeleteByPrimaryKey(ASequence, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFormLetterInsertRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFormLetterInsertAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PLabelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PLabelAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PLabelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PLabelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PMergeFormCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AMergeFormName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PMergeFieldTable MyPMergeFieldTable = PMergeFieldAccess.LoadViaPMergeForm(AMergeFormName, StringHelper.StrSplit("p_merge_form_name_c,p_merge_field_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPMergeFieldTable.Rows.Count); countRow = (countRow + 1))
                {
                    PMergeFieldCascading.DeleteUsingTemplate(MyPMergeFieldTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PMergeFormAccess.DeleteByPrimaryKey(AMergeFormName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PMergeFieldTable MyPMergeFieldTable = PMergeFieldAccess.LoadViaPMergeFormTemplate(ATemplateRow, StringHelper.StrSplit("p_merge_form_name_c,p_merge_field_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPMergeFieldTable.Rows.Count); countRow = (countRow + 1))
                {
                    PMergeFieldCascading.DeleteUsingTemplate(MyPMergeFieldTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PMergeFormAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PMergeFieldCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AMergeFormName, String AMergeFieldName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PMergeFieldAccess.DeleteByPrimaryKey(AMergeFormName, AMergeFieldName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PMergeFieldRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PMergeFieldAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPostcodeRangeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ARange, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPostcodeRegionTable MyPPostcodeRegionTable = PPostcodeRegionAccess.LoadViaPPostcodeRange(ARange, StringHelper.StrSplit("p_region_c,p_range_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPostcodeRegionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPostcodeRegionCascading.DeleteUsingTemplate(MyPPostcodeRegionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPostcodeRangeAccess.DeleteByPrimaryKey(ARange, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPostcodeRegionTable MyPPostcodeRegionTable = PPostcodeRegionAccess.LoadViaPPostcodeRangeTemplate(ATemplateRow, StringHelper.StrSplit("p_region_c,p_range_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPostcodeRegionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPostcodeRegionCascading.DeleteUsingTemplate(MyPPostcodeRegionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPostcodeRangeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPostcodeRegionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ARegion, String ARange, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPostcodeRegionAccess.DeleteByPrimaryKey(ARegion, ARange, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPostcodeRegionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPostcodeRegionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPublicationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APublicationCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPublicationCostTable MyPPublicationCostTable = PPublicationCostAccess.LoadViaPPublication(APublicationCode, StringHelper.StrSplit("p_publication_code_c,p_date_effective_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPublicationCostTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPublicationCostCascading.DeleteUsingTemplate(MyPPublicationCostTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PSubscriptionTable MyPSubscriptionTable = PSubscriptionAccess.LoadViaPPublication(APublicationCode, StringHelper.StrSplit("p_publication_code_c,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPSubscriptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PSubscriptionCascading.DeleteUsingTemplate(MyPSubscriptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPublicationAccess.DeleteByPrimaryKey(APublicationCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPublicationCostTable MyPPublicationCostTable = PPublicationCostAccess.LoadViaPPublicationTemplate(ATemplateRow, StringHelper.StrSplit("p_publication_code_c,p_date_effective_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPublicationCostTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPublicationCostCascading.DeleteUsingTemplate(MyPPublicationCostTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PSubscriptionTable MyPSubscriptionTable = PSubscriptionAccess.LoadViaPPublicationTemplate(ATemplateRow, StringHelper.StrSplit("p_publication_code_c,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPSubscriptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PSubscriptionCascading.DeleteUsingTemplate(MyPSubscriptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPublicationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPublicationCostCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APublicationCode, System.DateTime ADateEffective, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPublicationCostAccess.DeleteByPrimaryKey(APublicationCode, ADateEffective, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPublicationCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPublicationCostAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PReasonSubscriptionGivenCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PSubscriptionTable MyPSubscriptionTable = PSubscriptionAccess.LoadViaPReasonSubscriptionGiven(ACode, StringHelper.StrSplit("p_publication_code_c,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPSubscriptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PSubscriptionCascading.DeleteUsingTemplate(MyPSubscriptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PReasonSubscriptionGivenAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PSubscriptionTable MyPSubscriptionTable = PSubscriptionAccess.LoadViaPReasonSubscriptionGivenTemplate(ATemplateRow, StringHelper.StrSplit("p_publication_code_c,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPSubscriptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PSubscriptionCascading.DeleteUsingTemplate(MyPSubscriptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PReasonSubscriptionGivenAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PReasonSubscriptionCancelledCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PSubscriptionTable MyPSubscriptionTable = PSubscriptionAccess.LoadViaPReasonSubscriptionCancelled(ACode, StringHelper.StrSplit("p_publication_code_c,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPSubscriptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PSubscriptionCascading.DeleteUsingTemplate(MyPSubscriptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PReasonSubscriptionCancelledAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PSubscriptionTable MyPSubscriptionTable = PSubscriptionAccess.LoadViaPReasonSubscriptionCancelledTemplate(ATemplateRow, StringHelper.StrSplit("p_publication_code_c,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPSubscriptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PSubscriptionCascading.DeleteUsingTemplate(MyPSubscriptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PReasonSubscriptionCancelledAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PSubscriptionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APublicationCode, Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PSubscriptionAccess.DeleteByPrimaryKey(APublicationCode, APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PSubscriptionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PSubscriptionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PContactAttributeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AContactAttributeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PContactAttributeDetailTable MyPContactAttributeDetailTable = PContactAttributeDetailAccess.LoadViaPContactAttribute(AContactAttributeCode, StringHelper.StrSplit("p_contact_attribute_code_c,p_contact_attr_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPContactAttributeDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    PContactAttributeDetailCascading.DeleteUsingTemplate(MyPContactAttributeDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PContactAttributeAccess.DeleteByPrimaryKey(AContactAttributeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PContactAttributeDetailTable MyPContactAttributeDetailTable = PContactAttributeDetailAccess.LoadViaPContactAttributeTemplate(ATemplateRow, StringHelper.StrSplit("p_contact_attribute_code_c,p_contact_attr_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPContactAttributeDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    PContactAttributeDetailCascading.DeleteUsingTemplate(MyPContactAttributeDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PContactAttributeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PContactAttributeDetailCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerContactAttributeTable MyPPartnerContactAttributeTable = PPartnerContactAttributeAccess.LoadViaPContactAttributeDetail(AContactAttributeCode, AContactAttrDetailCode, StringHelper.StrSplit("p_contact_id_i,p_contact_attribute_code_c,p_contact_attr_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactAttributeCascading.DeleteUsingTemplate(MyPPartnerContactAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PContactAttributeDetailAccess.DeleteByPrimaryKey(AContactAttributeCode, AContactAttrDetailCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerContactAttributeTable MyPPartnerContactAttributeTable = PPartnerContactAttributeAccess.LoadViaPContactAttributeDetailTemplate(ATemplateRow, StringHelper.StrSplit("p_contact_id_i,p_contact_attribute_code_c,p_contact_attr_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactAttributeCascading.DeleteUsingTemplate(MyPPartnerContactAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PContactAttributeDetailAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PMethodOfContactCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AMethodOfContactCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerContactTable MyPPartnerContactTable = PPartnerContactAccess.LoadViaPMethodOfContact(AMethodOfContactCode, StringHelper.StrSplit("p_contact_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactCascading.DeleteUsingTemplate(MyPPartnerContactTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PMethodOfContactAccess.DeleteByPrimaryKey(AMethodOfContactCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerContactTable MyPPartnerContactTable = PPartnerContactAccess.LoadViaPMethodOfContactTemplate(ATemplateRow, StringHelper.StrSplit("p_contact_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactCascading.DeleteUsingTemplate(MyPPartnerContactTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PMethodOfContactAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerContactCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AContactId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerContactAttributeTable MyPPartnerContactAttributeTable = PPartnerContactAttributeAccess.LoadViaPPartnerContact(AContactId, StringHelper.StrSplit("p_contact_id_i,p_contact_attribute_code_c,p_contact_attr_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactAttributeCascading.DeleteUsingTemplate(MyPPartnerContactAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerReminderTable MyPPartnerReminderTable = PPartnerReminderAccess.LoadViaPPartnerContact(AContactId, StringHelper.StrSplit("p_partner_key_n,p_contact_id_i,p_reminder_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerReminderTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerReminderCascading.DeleteUsingTemplate(MyPPartnerReminderTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupPartnerContactTable MySGroupPartnerContactTable = SGroupPartnerContactAccess.LoadViaPPartnerContact(AContactId, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_contact_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupPartnerContactTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupPartnerContactCascading.DeleteUsingTemplate(MySGroupPartnerContactTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerContactFileTable MyPPartnerContactFileTable = PPartnerContactFileAccess.LoadViaPPartnerContact(AContactId, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactFileCascading.DeleteUsingTemplate(MyPPartnerContactFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerContactAccess.DeleteByPrimaryKey(AContactId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerContactAttributeTable MyPPartnerContactAttributeTable = PPartnerContactAttributeAccess.LoadViaPPartnerContactTemplate(ATemplateRow, StringHelper.StrSplit("p_contact_id_i,p_contact_attribute_code_c,p_contact_attr_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactAttributeCascading.DeleteUsingTemplate(MyPPartnerContactAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerReminderTable MyPPartnerReminderTable = PPartnerReminderAccess.LoadViaPPartnerContactTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_contact_id_i,p_reminder_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerReminderTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerReminderCascading.DeleteUsingTemplate(MyPPartnerReminderTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupPartnerContactTable MySGroupPartnerContactTable = SGroupPartnerContactAccess.LoadViaPPartnerContactTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_contact_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupPartnerContactTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupPartnerContactCascading.DeleteUsingTemplate(MySGroupPartnerContactTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerContactFileTable MyPPartnerContactFileTable = PPartnerContactFileAccess.LoadViaPPartnerContactTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactFileCascading.DeleteUsingTemplate(MyPPartnerContactFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerContactAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerContactAttributeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AContactId, String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerContactAttributeAccess.DeleteByPrimaryKey(AContactId, AContactAttributeCode, AContactAttrDetailCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerContactAttributeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ASubSystemCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ASubSystemCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ATransactionTypeTable MyATransactionTypeTable = ATransactionTypeAccess.LoadViaASubSystem(ASubSystemCode, StringHelper.StrSplit("a_ledger_number_i,a_sub_system_code_c,a_transaction_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransactionTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransactionTypeCascading.DeleteUsingTemplate(MyATransactionTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ASpecialTransTypeTable MyASpecialTransTypeTable = ASpecialTransTypeAccess.LoadViaASubSystem(ASubSystemCode, StringHelper.StrSplit("a_sub_system_code_c,a_transaction_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyASpecialTransTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    ASpecialTransTypeCascading.DeleteUsingTemplate(MyASpecialTransTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ASystemInterfaceTable MyASystemInterfaceTable = ASystemInterfaceAccess.LoadViaASubSystem(ASubSystemCode, StringHelper.StrSplit("a_ledger_number_i,a_sub_system_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyASystemInterfaceTable.Rows.Count); countRow = (countRow + 1))
                {
                    ASystemInterfaceCascading.DeleteUsingTemplate(MyASystemInterfaceTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ASubSystemAccess.DeleteByPrimaryKey(ASubSystemCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ASubSystemRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ATransactionTypeTable MyATransactionTypeTable = ATransactionTypeAccess.LoadViaASubSystemTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_sub_system_code_c,a_transaction_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransactionTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransactionTypeCascading.DeleteUsingTemplate(MyATransactionTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ASpecialTransTypeTable MyASpecialTransTypeTable = ASpecialTransTypeAccess.LoadViaASubSystemTemplate(ATemplateRow, StringHelper.StrSplit("a_sub_system_code_c,a_transaction_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyASpecialTransTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    ASpecialTransTypeCascading.DeleteUsingTemplate(MyASpecialTransTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ASystemInterfaceTable MyASystemInterfaceTable = ASystemInterfaceAccess.LoadViaASubSystemTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_sub_system_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyASystemInterfaceTable.Rows.Count); countRow = (countRow + 1))
                {
                    ASystemInterfaceCascading.DeleteUsingTemplate(MyASystemInterfaceTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ASubSystemAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ATaxTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ATaxTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ALedgerTable MyALedgerTable = ALedgerAccess.LoadViaATaxType(ATaxTypeCode, StringHelper.StrSplit("a_ledger_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyALedgerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    ALedgerAccess.DeleteUsingTemplate(MyALedgerTable[countRow], null, ATransaction);
                                }
                ATaxTableTable MyATaxTableTable = ATaxTableAccess.LoadViaATaxType(ATaxTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_tax_type_code_c,a_tax_rate_code_c,a_tax_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyATaxTableTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATaxTableCascading.DeleteUsingTemplate(MyATaxTableTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArArticleTable MyAArArticleTable = AArArticleAccess.LoadViaATaxType(ATaxTypeCode, StringHelper.StrSplit("a_ar_article_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArArticleTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArArticleCascading.DeleteUsingTemplate(MyAArArticleTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ATaxTypeAccess.DeleteByPrimaryKey(ATaxTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ALedgerTable MyALedgerTable = ALedgerAccess.LoadViaATaxTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyALedgerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    ALedgerAccess.DeleteUsingTemplate(MyALedgerTable[countRow], null, ATransaction);
                                }
                ATaxTableTable MyATaxTableTable = ATaxTableAccess.LoadViaATaxTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_tax_type_code_c,a_tax_rate_code_c,a_tax_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyATaxTableTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATaxTableCascading.DeleteUsingTemplate(MyATaxTableTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArArticleTable MyAArArticleTable = AArArticleAccess.LoadViaATaxTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ar_article_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArArticleTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArArticleCascading.DeleteUsingTemplate(MyAArArticleTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ATaxTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ATaxTableCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArInvoiceTable MyAArInvoiceTable = AArInvoiceAccess.LoadViaATaxTable(ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, StringHelper.StrSplit("a_ledger_number_i,a_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceCascading.DeleteUsingTemplate(MyAArInvoiceTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDetailTable MyAArInvoiceDetailTable = AArInvoiceDetailAccess.LoadViaATaxTable(ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailCascading.DeleteUsingTemplate(MyAArInvoiceDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ATaxTableAccess.DeleteByPrimaryKey(ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArInvoiceTable MyAArInvoiceTable = AArInvoiceAccess.LoadViaATaxTableTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceCascading.DeleteUsingTemplate(MyAArInvoiceTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDetailTable MyAArInvoiceDetailTable = AArInvoiceDetailAccess.LoadViaATaxTableTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailCascading.DeleteUsingTemplate(MyAArInvoiceDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ATaxTableAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ALedgerInitFlagCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AInitOptionName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ALedgerInitFlagAccess.DeleteByPrimaryKey(ALedgerNumber, AInitOptionName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ALedgerInitFlagRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ALedgerInitFlagAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ABudgetTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ABudgetTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AAccountTable MyAAccountTable = AAccountAccess.LoadViaABudgetType(ABudgetTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_account_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAccountTable.Rows.Count); countRow = (countRow + 1))
                {
                                    AAccountAccess.DeleteUsingTemplate(MyAAccountTable[countRow], null, ATransaction);
                                }
                ABudgetTable MyABudgetTable = ABudgetAccess.LoadViaABudgetType(ABudgetTypeCode, StringHelper.StrSplit("a_budget_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyABudgetTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABudgetCascading.DeleteUsingTemplate(MyABudgetTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ABudgetTypeAccess.DeleteByPrimaryKey(ABudgetTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ABudgetTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AAccountTable MyAAccountTable = AAccountAccess.LoadViaABudgetTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_account_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAccountTable.Rows.Count); countRow = (countRow + 1))
                {
                                    AAccountAccess.DeleteUsingTemplate(MyAAccountTable[countRow], null, ATransaction);
                                }
                ABudgetTable MyABudgetTable = ABudgetAccess.LoadViaABudgetTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_budget_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyABudgetTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABudgetCascading.DeleteUsingTemplate(MyABudgetTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ABudgetTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AAccountPropertyCodeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APropertyCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AAccountPropertyTable MyAAccountPropertyTable = AAccountPropertyAccess.LoadViaAAccountPropertyCode(APropertyCode, StringHelper.StrSplit("a_ledger_number_i,a_account_code_c,a_property_code_c,a_property_value_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAccountPropertyTable.Rows.Count); countRow = (countRow + 1))
                {
                    AAccountPropertyCascading.DeleteUsingTemplate(MyAAccountPropertyTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAccountPropertyCodeAccess.DeleteByPrimaryKey(APropertyCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AAccountPropertyCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AAccountPropertyTable MyAAccountPropertyTable = AAccountPropertyAccess.LoadViaAAccountPropertyCodeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_account_code_c,a_property_code_c,a_property_value_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAccountPropertyTable.Rows.Count); countRow = (countRow + 1))
                {
                    AAccountPropertyCascading.DeleteUsingTemplate(MyAAccountPropertyTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAccountPropertyCodeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AAccountPropertyCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AAccountCode, String APropertyCode, String APropertyValue, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AAccountPropertyAccess.DeleteByPrimaryKey(ALedgerNumber, AAccountCode, APropertyCode, APropertyValue, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AAccountPropertyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AAccountPropertyAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AAccountHierarchyCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AAccountHierarchyCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AAccountHierarchyDetailTable MyAAccountHierarchyDetailTable = AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(ALedgerNumber, AAccountHierarchyCode, StringHelper.StrSplit("a_ledger_number_i,a_account_hierarchy_code_c,a_reporting_account_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAccountHierarchyDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AAccountHierarchyDetailCascading.DeleteUsingTemplate(MyAAccountHierarchyDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAccountHierarchyAccess.DeleteByPrimaryKey(ALedgerNumber, AAccountHierarchyCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AAccountHierarchyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AAccountHierarchyDetailTable MyAAccountHierarchyDetailTable = AAccountHierarchyDetailAccess.LoadViaAAccountHierarchyTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_account_hierarchy_code_c,a_reporting_account_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAccountHierarchyDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AAccountHierarchyDetailCascading.DeleteUsingTemplate(MyAAccountHierarchyDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAccountHierarchyAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AAccountHierarchyDetailCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AAccountHierarchyCode, String AReportingAccountCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AAccountHierarchyDetailAccess.DeleteByPrimaryKey(ALedgerNumber, AAccountHierarchyCode, AReportingAccountCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AAccountHierarchyDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AAccountHierarchyDetailAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ACostCentreTypesCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String ACostCentreType, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ACostCentreTable MyACostCentreTable = ACostCentreAccess.LoadViaACostCentreTypes(ALedgerNumber, ACostCentreType, StringHelper.StrSplit("a_ledger_number_i,a_cost_centre_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyACostCentreTable.Rows.Count); countRow = (countRow + 1))
                {
                                    ACostCentreAccess.DeleteUsingTemplate(MyACostCentreTable[countRow], null, ATransaction);
                                }
            }
            ACostCentreTypesAccess.DeleteByPrimaryKey(ALedgerNumber, ACostCentreType, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ACostCentreTypesRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ACostCentreTable MyACostCentreTable = ACostCentreAccess.LoadViaACostCentreTypesTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_cost_centre_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyACostCentreTable.Rows.Count); countRow = (countRow + 1))
                {
                                    ACostCentreAccess.DeleteUsingTemplate(MyACostCentreTable[countRow], null, ATransaction);
                                }
            }
            ACostCentreTypesAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AValidLedgerNumberCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AValidLedgerNumberAccess.DeleteByPrimaryKey(ALedgerNumber, APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AValidLedgerNumberRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AValidLedgerNumberAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ABudgetRevisionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AYear, Int32 ARevision, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ABudgetTable MyABudgetTable = ABudgetAccess.LoadViaABudgetRevision(ALedgerNumber, AYear, ARevision, StringHelper.StrSplit("a_budget_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyABudgetTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABudgetCascading.DeleteUsingTemplate(MyABudgetTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ABudgetRevisionAccess.DeleteByPrimaryKey(ALedgerNumber, AYear, ARevision, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ABudgetRevisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ABudgetTable MyABudgetTable = ABudgetAccess.LoadViaABudgetRevisionTemplate(ATemplateRow, StringHelper.StrSplit("a_budget_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyABudgetTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABudgetCascading.DeleteUsingTemplate(MyABudgetTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ABudgetRevisionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ABudgetCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ABudgetSequence, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ABudgetPeriodTable MyABudgetPeriodTable = ABudgetPeriodAccess.LoadViaABudget(ABudgetSequence, StringHelper.StrSplit("a_budget_sequence_i,a_period_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyABudgetPeriodTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABudgetPeriodCascading.DeleteUsingTemplate(MyABudgetPeriodTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ABudgetAccess.DeleteByPrimaryKey(ABudgetSequence, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ABudgetRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ABudgetPeriodTable MyABudgetPeriodTable = ABudgetPeriodAccess.LoadViaABudgetTemplate(ATemplateRow, StringHelper.StrSplit("a_budget_sequence_i,a_period_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyABudgetPeriodTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABudgetPeriodCascading.DeleteUsingTemplate(MyABudgetPeriodTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ABudgetAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ABudgetPeriodCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ABudgetSequence, Int32 APeriodNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ABudgetPeriodAccess.DeleteByPrimaryKey(ABudgetSequence, APeriodNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ABudgetPeriodRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ABudgetPeriodAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AAccountingPeriodCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AAccountingPeriodNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AIchStewardshipTable MyAIchStewardshipTable = AIchStewardshipAccess.LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, StringHelper.StrSplit("a_ledger_number_i,a_period_number_i,a_ich_number_i,a_cost_centre_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAIchStewardshipTable.Rows.Count); countRow = (countRow + 1))
                {
                    AIchStewardshipCascading.DeleteUsingTemplate(MyAIchStewardshipTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AProcessedFeeTable MyAProcessedFeeTable = AProcessedFeeAccess.LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i,a_fee_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAProcessedFeeTable.Rows.Count); countRow = (countRow + 1))
                {
                    AProcessedFeeCascading.DeleteUsingTemplate(MyAProcessedFeeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                APreviousYearBatchTable MyAPreviousYearBatchTable = APreviousYearBatchAccess.LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearBatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearBatchCascading.DeleteUsingTemplate(MyAPreviousYearBatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                APreviousYearJournalTable MyAPreviousYearJournalTable = APreviousYearJournalAccess.LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearJournalCascading.DeleteUsingTemplate(MyAPreviousYearJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisYearOldBatchTable MyAThisYearOldBatchTable = AThisYearOldBatchAccess.LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldBatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldBatchCascading.DeleteUsingTemplate(MyAThisYearOldBatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisYearOldJournalTable MyAThisYearOldJournalTable = AThisYearOldJournalAccess.LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldJournalCascading.DeleteUsingTemplate(MyAThisYearOldJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ABatchTable MyABatchTable = ABatchAccess.LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyABatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABatchCascading.DeleteUsingTemplate(MyABatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AJournalTable MyAJournalTable = AJournalAccess.LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AJournalCascading.DeleteUsingTemplate(MyAJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAccountingPeriodAccess.DeleteByPrimaryKey(ALedgerNumber, AAccountingPeriodNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AAccountingPeriodRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AIchStewardshipTable MyAIchStewardshipTable = AIchStewardshipAccess.LoadViaAAccountingPeriodTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_period_number_i,a_ich_number_i,a_cost_centre_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAIchStewardshipTable.Rows.Count); countRow = (countRow + 1))
                {
                    AIchStewardshipCascading.DeleteUsingTemplate(MyAIchStewardshipTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AProcessedFeeTable MyAProcessedFeeTable = AProcessedFeeAccess.LoadViaAAccountingPeriodTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i,a_fee_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAProcessedFeeTable.Rows.Count); countRow = (countRow + 1))
                {
                    AProcessedFeeCascading.DeleteUsingTemplate(MyAProcessedFeeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                APreviousYearBatchTable MyAPreviousYearBatchTable = APreviousYearBatchAccess.LoadViaAAccountingPeriodTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearBatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearBatchCascading.DeleteUsingTemplate(MyAPreviousYearBatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                APreviousYearJournalTable MyAPreviousYearJournalTable = APreviousYearJournalAccess.LoadViaAAccountingPeriodTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearJournalCascading.DeleteUsingTemplate(MyAPreviousYearJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisYearOldBatchTable MyAThisYearOldBatchTable = AThisYearOldBatchAccess.LoadViaAAccountingPeriodTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldBatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldBatchCascading.DeleteUsingTemplate(MyAThisYearOldBatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisYearOldJournalTable MyAThisYearOldJournalTable = AThisYearOldJournalAccess.LoadViaAAccountingPeriodTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldJournalCascading.DeleteUsingTemplate(MyAThisYearOldJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ABatchTable MyABatchTable = ABatchAccess.LoadViaAAccountingPeriodTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyABatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    ABatchCascading.DeleteUsingTemplate(MyABatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AJournalTable MyAJournalTable = AJournalAccess.LoadViaAAccountingPeriodTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AJournalCascading.DeleteUsingTemplate(MyAJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAccountingPeriodAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AAccountingSystemParameterCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AAccountingSystemParameterAccess.DeleteByPrimaryKey(ALedgerNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AAccountingSystemParameterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AAccountingSystemParameterAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AAnalysisStoreTableCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AStoreName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AAnalysisStoreTableAccess.DeleteByPrimaryKey(AStoreName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AAnalysisStoreTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AAnalysisStoreTableAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AAnalysisTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AAnalysisTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AAnalysisAttributeTable MyAAnalysisAttributeTable = AAnalysisAttributeAccess.LoadViaAAnalysisType(AAnalysisTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_account_code_c,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAnalysisAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    AAnalysisAttributeCascading.DeleteUsingTemplate(MyAAnalysisAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AFreeformAnalysisTable MyAFreeformAnalysisTable = AFreeformAnalysisAccess.LoadViaAAnalysisType(AAnalysisTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_analysis_type_code_c,a_analysis_value_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAFreeformAnalysisTable.Rows.Count); countRow = (countRow + 1))
                {
                    AFreeformAnalysisCascading.DeleteUsingTemplate(MyAFreeformAnalysisTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAnalysisTypeAccess.DeleteByPrimaryKey(AAnalysisTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AAnalysisTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AAnalysisAttributeTable MyAAnalysisAttributeTable = AAnalysisAttributeAccess.LoadViaAAnalysisTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_account_code_c,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAAnalysisAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    AAnalysisAttributeCascading.DeleteUsingTemplate(MyAAnalysisAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AFreeformAnalysisTable MyAFreeformAnalysisTable = AFreeformAnalysisAccess.LoadViaAAnalysisTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_analysis_type_code_c,a_analysis_value_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAFreeformAnalysisTable.Rows.Count); countRow = (countRow + 1))
                {
                    AFreeformAnalysisCascading.DeleteUsingTemplate(MyAFreeformAnalysisTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAnalysisTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AAnalysisAttributeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APrevYearTransAnalAttribTable MyAPrevYearTransAnalAttribTable = APrevYearTransAnalAttribAccess.LoadViaAAnalysisAttribute(ALedgerNumber, AAccountCode, AAnalysisTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPrevYearTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    APrevYearTransAnalAttribCascading.DeleteUsingTemplate(MyAPrevYearTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisyearoldTransAnalAttribTable MyAThisyearoldTransAnalAttribTable = AThisyearoldTransAnalAttribAccess.LoadViaAAnalysisAttribute(ALedgerNumber, AAccountCode, AAnalysisTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisyearoldTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisyearoldTransAnalAttribCascading.DeleteUsingTemplate(MyAThisyearoldTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringTransAnalAttribTable MyARecurringTransAnalAttribTable = ARecurringTransAnalAttribAccess.LoadViaAAnalysisAttribute(ALedgerNumber, AAccountCode, AAnalysisTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransAnalAttribCascading.DeleteUsingTemplate(MyARecurringTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ATransAnalAttribTable MyATransAnalAttribTable = ATransAnalAttribAccess.LoadViaAAnalysisAttribute(ALedgerNumber, AAccountCode, AAnalysisTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransAnalAttribCascading.DeleteUsingTemplate(MyATransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AApAnalAttribTable MyAApAnalAttribTable = AApAnalAttribAccess.LoadViaAAnalysisAttribute(ALedgerNumber, AAccountCode, AAnalysisTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_detail_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApAnalAttribCascading.DeleteUsingTemplate(MyAApAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAnalysisAttributeAccess.DeleteByPrimaryKey(ALedgerNumber, AAccountCode, AAnalysisTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APrevYearTransAnalAttribTable MyAPrevYearTransAnalAttribTable = APrevYearTransAnalAttribAccess.LoadViaAAnalysisAttributeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPrevYearTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    APrevYearTransAnalAttribCascading.DeleteUsingTemplate(MyAPrevYearTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisyearoldTransAnalAttribTable MyAThisyearoldTransAnalAttribTable = AThisyearoldTransAnalAttribAccess.LoadViaAAnalysisAttributeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisyearoldTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisyearoldTransAnalAttribCascading.DeleteUsingTemplate(MyAThisyearoldTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringTransAnalAttribTable MyARecurringTransAnalAttribTable = ARecurringTransAnalAttribAccess.LoadViaAAnalysisAttributeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransAnalAttribCascading.DeleteUsingTemplate(MyARecurringTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ATransAnalAttribTable MyATransAnalAttribTable = ATransAnalAttribAccess.LoadViaAAnalysisAttributeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransAnalAttribCascading.DeleteUsingTemplate(MyATransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AApAnalAttribTable MyAApAnalAttribTable = AApAnalAttribAccess.LoadViaAAnalysisAttributeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_detail_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApAnalAttribCascading.DeleteUsingTemplate(MyAApAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AAnalysisAttributeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ACorporateExchangeRateCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFromCurrencyCode, String AToCurrencyCode, System.DateTime ADateEffectiveFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ACorporateExchangeRateAccess.DeleteByPrimaryKey(AFromCurrencyCode, AToCurrencyCode, ADateEffectiveFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ACorporateExchangeRateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ACorporateExchangeRateAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ADailyExchangeRateCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFromCurrencyCode, String AToCurrencyCode, System.DateTime ADateEffectiveFrom, Int32 ATimeEffectiveFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ADailyExchangeRateAccess.DeleteByPrimaryKey(AFromCurrencyCode, AToCurrencyCode, ADateEffectiveFrom, ATimeEffectiveFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ADailyExchangeRateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ADailyExchangeRateAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PEmailCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AEmailAddress, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PEmailAccess.DeleteByPrimaryKey(AEmailAddress, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PEmailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PEmailAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AEmailDestinationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFileCode, String AConditionalValue, Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AEmailDestinationAccess.DeleteByPrimaryKey(AFileCode, AConditionalValue, APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AEmailDestinationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AEmailDestinationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AFeesPayableCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AFeeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AFeesPayableAccess.DeleteByPrimaryKey(ALedgerNumber, AFeeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AFeesPayableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AFeesPayableAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AFeesReceivableCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AFeeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AFeesReceivableAccess.DeleteByPrimaryKey(ALedgerNumber, AFeeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AFeesReceivableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AFeesReceivableAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AFinStatementGroupCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AFinStatementGroup, String AAccountCode, String AReportSection, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AFinStatementGroupAccess.DeleteByPrimaryKey(ALedgerNumber, AFinStatementGroup, AAccountCode, AReportSection, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AFinStatementGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AFinStatementGroupAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AFormCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFormCode, String AFormName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AFormElementTable MyAFormElementTable = AFormElementAccess.LoadViaAForm(AFormCode, AFormName, StringHelper.StrSplit("a_form_code_c,a_form_name_c,a_form_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAFormElementTable.Rows.Count); countRow = (countRow + 1))
                {
                    AFormElementCascading.DeleteUsingTemplate(MyAFormElementTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AFormAccess.DeleteByPrimaryKey(AFormCode, AFormName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AFormRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AFormElementTable MyAFormElementTable = AFormElementAccess.LoadViaAFormTemplate(ATemplateRow, StringHelper.StrSplit("a_form_code_c,a_form_name_c,a_form_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAFormElementTable.Rows.Count); countRow = (countRow + 1))
                {
                    AFormElementCascading.DeleteUsingTemplate(MyAFormElementTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AFormAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AFormElementTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFormCode, String AFormElementTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AFormElementTable MyAFormElementTable = AFormElementAccess.LoadViaAFormElementType(AFormCode, AFormElementTypeCode, StringHelper.StrSplit("a_form_code_c,a_form_name_c,a_form_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAFormElementTable.Rows.Count); countRow = (countRow + 1))
                {
                    AFormElementCascading.DeleteUsingTemplate(MyAFormElementTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AFormElementTypeAccess.DeleteByPrimaryKey(AFormCode, AFormElementTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AFormElementTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AFormElementTable MyAFormElementTable = AFormElementAccess.LoadViaAFormElementTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_form_code_c,a_form_name_c,a_form_sequence_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAFormElementTable.Rows.Count); countRow = (countRow + 1))
                {
                    AFormElementCascading.DeleteUsingTemplate(MyAFormElementTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AFormElementTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AFormElementCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFormCode, String AFormName, Int32 AFormSequence, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AFormElementAccess.DeleteByPrimaryKey(AFormCode, AFormName, AFormSequence, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AFormElementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AFormElementAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AFreeformAnalysisCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APrevYearTransAnalAttribTable MyAPrevYearTransAnalAttribTable = APrevYearTransAnalAttribAccess.LoadViaAFreeformAnalysis(ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPrevYearTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    APrevYearTransAnalAttribCascading.DeleteUsingTemplate(MyAPrevYearTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisyearoldTransAnalAttribTable MyAThisyearoldTransAnalAttribTable = AThisyearoldTransAnalAttribAccess.LoadViaAFreeformAnalysis(ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisyearoldTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisyearoldTransAnalAttribCascading.DeleteUsingTemplate(MyAThisyearoldTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringTransAnalAttribTable MyARecurringTransAnalAttribTable = ARecurringTransAnalAttribAccess.LoadViaAFreeformAnalysis(ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransAnalAttribCascading.DeleteUsingTemplate(MyARecurringTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ATransAnalAttribTable MyATransAnalAttribTable = ATransAnalAttribAccess.LoadViaAFreeformAnalysis(ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransAnalAttribCascading.DeleteUsingTemplate(MyATransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AApAnalAttribTable MyAApAnalAttribTable = AApAnalAttribAccess.LoadViaAFreeformAnalysis(ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_detail_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApAnalAttribCascading.DeleteUsingTemplate(MyAApAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AFreeformAnalysisAccess.DeleteByPrimaryKey(ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APrevYearTransAnalAttribTable MyAPrevYearTransAnalAttribTable = APrevYearTransAnalAttribAccess.LoadViaAFreeformAnalysisTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPrevYearTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    APrevYearTransAnalAttribCascading.DeleteUsingTemplate(MyAPrevYearTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisyearoldTransAnalAttribTable MyAThisyearoldTransAnalAttribTable = AThisyearoldTransAnalAttribAccess.LoadViaAFreeformAnalysisTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisyearoldTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisyearoldTransAnalAttribCascading.DeleteUsingTemplate(MyAThisyearoldTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringTransAnalAttribTable MyARecurringTransAnalAttribTable = ARecurringTransAnalAttribAccess.LoadViaAFreeformAnalysisTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransAnalAttribCascading.DeleteUsingTemplate(MyARecurringTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ATransAnalAttribTable MyATransAnalAttribTable = ATransAnalAttribAccess.LoadViaAFreeformAnalysisTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransAnalAttribCascading.DeleteUsingTemplate(MyATransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AApAnalAttribTable MyAApAnalAttribTable = AApAnalAttribAccess.LoadViaAFreeformAnalysisTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_detail_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApAnalAttribCascading.DeleteUsingTemplate(MyAApAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AFreeformAnalysisAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AGeneralLedgerMasterCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AGlmSequence, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AGeneralLedgerMasterPeriodTable MyAGeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadViaAGeneralLedgerMaster(AGlmSequence, StringHelper.StrSplit("a_glm_sequence_i,a_period_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGeneralLedgerMasterPeriodTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGeneralLedgerMasterPeriodCascading.DeleteUsingTemplate(MyAGeneralLedgerMasterPeriodTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AGeneralLedgerMasterAccess.DeleteByPrimaryKey(AGlmSequence, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AGeneralLedgerMasterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AGeneralLedgerMasterPeriodTable MyAGeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadViaAGeneralLedgerMasterTemplate(ATemplateRow, StringHelper.StrSplit("a_glm_sequence_i,a_period_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGeneralLedgerMasterPeriodTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGeneralLedgerMasterPeriodCascading.DeleteUsingTemplate(MyAGeneralLedgerMasterPeriodTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AGeneralLedgerMasterAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AGeneralLedgerMasterPeriodCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AGlmSequence, Int32 APeriodNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AGeneralLedgerMasterPeriodAccess.DeleteByPrimaryKey(AGlmSequence, APeriodNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AGeneralLedgerMasterPeriodRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AGeneralLedgerMasterPeriodAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AIchStewardshipCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 APeriodNumber, Int32 AIchNumber, String ACostCentreCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AIchStewardshipAccess.DeleteByPrimaryKey(ALedgerNumber, APeriodNumber, AIchNumber, ACostCentreCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AIchStewardshipRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AIchStewardshipAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AMethodOfGivingCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AMethodOfGivingCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpMatchTable MyAEpMatchTable = AEpMatchAccess.LoadViaAMethodOfGiving(AMethodOfGivingCode, StringHelper.StrSplit("a_ep_match_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpMatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpMatchCascading.DeleteUsingTemplate(MyAEpMatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringGiftTable MyARecurringGiftTable = ARecurringGiftAccess.LoadViaAMethodOfGiving(AMethodOfGivingCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftCascading.DeleteUsingTemplate(MyARecurringGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AGiftTable MyAGiftTable = AGiftAccess.LoadViaAMethodOfGiving(AMethodOfGivingCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftCascading.DeleteUsingTemplate(MyAGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AMethodOfGivingAccess.DeleteByPrimaryKey(AMethodOfGivingCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpMatchTable MyAEpMatchTable = AEpMatchAccess.LoadViaAMethodOfGivingTemplate(ATemplateRow, StringHelper.StrSplit("a_ep_match_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpMatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpMatchCascading.DeleteUsingTemplate(MyAEpMatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringGiftTable MyARecurringGiftTable = ARecurringGiftAccess.LoadViaAMethodOfGivingTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftCascading.DeleteUsingTemplate(MyARecurringGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AGiftTable MyAGiftTable = AGiftAccess.LoadViaAMethodOfGivingTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftCascading.DeleteUsingTemplate(MyAGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AMethodOfGivingAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AMethodOfPaymentCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AMethodOfPaymentCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpMatchTable MyAEpMatchTable = AEpMatchAccess.LoadViaAMethodOfPayment(AMethodOfPaymentCode, StringHelper.StrSplit("a_ep_match_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpMatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpMatchCascading.DeleteUsingTemplate(MyAEpMatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringJournalTable MyARecurringJournalTable = ARecurringJournalAccess.LoadViaAMethodOfPayment(AMethodOfPaymentCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringJournalCascading.DeleteUsingTemplate(MyARecurringJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringTransactionTable MyARecurringTransactionTable = ARecurringTransactionAccess.LoadViaAMethodOfPayment(AMethodOfPaymentCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransactionCascading.DeleteUsingTemplate(MyARecurringTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringGiftTable MyARecurringGiftTable = ARecurringGiftAccess.LoadViaAMethodOfPayment(AMethodOfPaymentCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftCascading.DeleteUsingTemplate(MyARecurringGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AGiftTable MyAGiftTable = AGiftAccess.LoadViaAMethodOfPayment(AMethodOfPaymentCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftCascading.DeleteUsingTemplate(MyAGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AMethodOfPaymentAccess.DeleteByPrimaryKey(AMethodOfPaymentCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpMatchTable MyAEpMatchTable = AEpMatchAccess.LoadViaAMethodOfPaymentTemplate(ATemplateRow, StringHelper.StrSplit("a_ep_match_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpMatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpMatchCascading.DeleteUsingTemplate(MyAEpMatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringJournalTable MyARecurringJournalTable = ARecurringJournalAccess.LoadViaAMethodOfPaymentTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringJournalCascading.DeleteUsingTemplate(MyARecurringJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringTransactionTable MyARecurringTransactionTable = ARecurringTransactionAccess.LoadViaAMethodOfPaymentTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransactionCascading.DeleteUsingTemplate(MyARecurringTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringGiftTable MyARecurringGiftTable = ARecurringGiftAccess.LoadViaAMethodOfPaymentTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftCascading.DeleteUsingTemplate(MyARecurringGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AGiftTable MyAGiftTable = AGiftAccess.LoadViaAMethodOfPaymentTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftCascading.DeleteUsingTemplate(MyAGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AMethodOfPaymentAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AMotivationGroupCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AMotivationDetailTable MyAMotivationDetailTable = AMotivationDetailAccess.LoadViaAMotivationGroup(ALedgerNumber, AMotivationGroupCode, StringHelper.StrSplit("a_ledger_number_i,a_motivation_group_code_c,a_motivation_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAMotivationDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AMotivationDetailCascading.DeleteUsingTemplate(MyAMotivationDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AMotivationGroupAccess.DeleteByPrimaryKey(ALedgerNumber, AMotivationGroupCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AMotivationDetailTable MyAMotivationDetailTable = AMotivationDetailAccess.LoadViaAMotivationGroupTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_motivation_group_code_c,a_motivation_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAMotivationDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AMotivationDetailCascading.DeleteUsingTemplate(MyAMotivationDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AMotivationGroupAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AMotivationDetailCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpAccountTable MyAEpAccountTable = AEpAccountAccess.LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, StringHelper.StrSplit("a_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpAccountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpAccountCascading.DeleteUsingTemplate(MyAEpAccountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AEpMatchTable MyAEpMatchTable = AEpMatchAccess.LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, StringHelper.StrSplit("a_ep_match_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpMatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpMatchCascading.DeleteUsingTemplate(MyAEpMatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AMotivationDetailFeeTable MyAMotivationDetailFeeTable = AMotivationDetailFeeAccess.LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, StringHelper.StrSplit("a_ledger_number_i,a_motivation_group_code_c,a_motivation_detail_code_c,a_fee_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAMotivationDetailFeeTable.Rows.Count); countRow = (countRow + 1))
                {
                    AMotivationDetailFeeCascading.DeleteUsingTemplate(MyAMotivationDetailFeeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringGiftDetailTable MyARecurringGiftDetailTable = ARecurringGiftDetailAccess.LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftDetailCascading.DeleteUsingTemplate(MyARecurringGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AGiftDetailTable MyAGiftDetailTable = AGiftDetailAccess.LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftDetailCascading.DeleteUsingTemplate(MyAGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupMotivationTable MySGroupMotivationTable = SGroupMotivationAccess.LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,a_ledger_number_i,a_motivation_group_code_c,a_motivation_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupMotivationTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupMotivationCascading.DeleteUsingTemplate(MySGroupMotivationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFoundationProposalDetailTable MyPFoundationProposalDetailTable = PFoundationProposalDetailAccess.LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i,p_proposal_detail_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalDetailCascading.DeleteUsingTemplate(MyPFoundationProposalDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AMotivationDetailAccess.DeleteByPrimaryKey(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpAccountTable MyAEpAccountTable = AEpAccountAccess.LoadViaAMotivationDetailTemplate(ATemplateRow, StringHelper.StrSplit("a_banking_details_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpAccountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpAccountCascading.DeleteUsingTemplate(MyAEpAccountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AEpMatchTable MyAEpMatchTable = AEpMatchAccess.LoadViaAMotivationDetailTemplate(ATemplateRow, StringHelper.StrSplit("a_ep_match_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpMatchTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpMatchCascading.DeleteUsingTemplate(MyAEpMatchTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AMotivationDetailFeeTable MyAMotivationDetailFeeTable = AMotivationDetailFeeAccess.LoadViaAMotivationDetailTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_motivation_group_code_c,a_motivation_detail_code_c,a_fee_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAMotivationDetailFeeTable.Rows.Count); countRow = (countRow + 1))
                {
                    AMotivationDetailFeeCascading.DeleteUsingTemplate(MyAMotivationDetailFeeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringGiftDetailTable MyARecurringGiftDetailTable = ARecurringGiftDetailAccess.LoadViaAMotivationDetailTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftDetailCascading.DeleteUsingTemplate(MyARecurringGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AGiftDetailTable MyAGiftDetailTable = AGiftDetailAccess.LoadViaAMotivationDetailTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftDetailCascading.DeleteUsingTemplate(MyAGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupMotivationTable MySGroupMotivationTable = SGroupMotivationAccess.LoadViaAMotivationDetailTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,a_ledger_number_i,a_motivation_group_code_c,a_motivation_detail_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupMotivationTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupMotivationCascading.DeleteUsingTemplate(MySGroupMotivationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFoundationProposalDetailTable MyPFoundationProposalDetailTable = PFoundationProposalDetailAccess.LoadViaAMotivationDetailTemplate(ATemplateRow, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i,p_proposal_detail_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalDetailCascading.DeleteUsingTemplate(MyPFoundationProposalDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AMotivationDetailAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AMotivationDetailFeeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AMotivationDetailFeeAccess.DeleteByPrimaryKey(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AMotivationDetailFeeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AProcessedFeeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, String AFeeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AProcessedFeeAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFeeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AProcessedFeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AProcessedFeeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ATransactionTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String ASubSystemCode, String ATransactionTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APreviousYearJournalTable MyAPreviousYearJournalTable = APreviousYearJournalAccess.LoadViaATransactionType(ALedgerNumber, ASubSystemCode, ATransactionTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearJournalCascading.DeleteUsingTemplate(MyAPreviousYearJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisYearOldJournalTable MyAThisYearOldJournalTable = AThisYearOldJournalAccess.LoadViaATransactionType(ALedgerNumber, ASubSystemCode, ATransactionTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldJournalCascading.DeleteUsingTemplate(MyAThisYearOldJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringJournalTable MyARecurringJournalTable = ARecurringJournalAccess.LoadViaATransactionType(ALedgerNumber, ASubSystemCode, ATransactionTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringJournalCascading.DeleteUsingTemplate(MyARecurringJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AJournalTable MyAJournalTable = AJournalAccess.LoadViaATransactionType(ALedgerNumber, ASubSystemCode, ATransactionTypeCode, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AJournalCascading.DeleteUsingTemplate(MyAJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ATransactionTypeAccess.DeleteByPrimaryKey(ALedgerNumber, ASubSystemCode, ATransactionTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ATransactionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APreviousYearJournalTable MyAPreviousYearJournalTable = APreviousYearJournalAccess.LoadViaATransactionTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearJournalCascading.DeleteUsingTemplate(MyAPreviousYearJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AThisYearOldJournalTable MyAThisYearOldJournalTable = AThisYearOldJournalAccess.LoadViaATransactionTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldJournalCascading.DeleteUsingTemplate(MyAThisYearOldJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ARecurringJournalTable MyARecurringJournalTable = ARecurringJournalAccess.LoadViaATransactionTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringJournalCascading.DeleteUsingTemplate(MyARecurringJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AJournalTable MyAJournalTable = AJournalAccess.LoadViaATransactionTypeTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AJournalCascading.DeleteUsingTemplate(MyAJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ATransactionTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class APreviousYearBatchCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APreviousYearJournalTable MyAPreviousYearJournalTable = APreviousYearJournalAccess.LoadViaAPreviousYearBatch(ALedgerNumber, ABatchNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearJournalCascading.DeleteUsingTemplate(MyAPreviousYearJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            APreviousYearBatchAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(APreviousYearBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APreviousYearJournalTable MyAPreviousYearJournalTable = APreviousYearJournalAccess.LoadViaAPreviousYearBatchTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearJournalCascading.DeleteUsingTemplate(MyAPreviousYearJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            APreviousYearBatchAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class APreviousYearJournalCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APreviousYearTransactionTable MyAPreviousYearTransactionTable = APreviousYearTransactionAccess.LoadViaAPreviousYearJournal(ALedgerNumber, ABatchNumber, AJournalNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearTransactionCascading.DeleteUsingTemplate(MyAPreviousYearTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            APreviousYearJournalAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(APreviousYearJournalRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APreviousYearTransactionTable MyAPreviousYearTransactionTable = APreviousYearTransactionAccess.LoadViaAPreviousYearJournalTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPreviousYearTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    APreviousYearTransactionCascading.DeleteUsingTemplate(MyAPreviousYearTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            APreviousYearJournalAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class APreviousYearTransactionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APrevYearTransAnalAttribTable MyAPrevYearTransAnalAttribTable = APrevYearTransAnalAttribAccess.LoadViaAPreviousYearTransaction(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPrevYearTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    APrevYearTransAnalAttribCascading.DeleteUsingTemplate(MyAPrevYearTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            APreviousYearTransactionAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(APreviousYearTransactionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                APrevYearTransAnalAttribTable MyAPrevYearTransAnalAttribTable = APrevYearTransAnalAttribAccess.LoadViaAPreviousYearTransactionTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAPrevYearTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    APrevYearTransAnalAttribCascading.DeleteUsingTemplate(MyAPrevYearTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            APreviousYearTransactionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class APrevYearTransAnalAttribCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber, String AAnalysisTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            APrevYearTransAnalAttribAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, AAnalysisTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(APrevYearTransAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            APrevYearTransAnalAttribAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class APrevYearCorpExRateCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFromCurrencyCode, String AToCurrencyCode, System.DateTime ADateEffectiveFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            APrevYearCorpExRateAccess.DeleteByPrimaryKey(AFromCurrencyCode, AToCurrencyCode, ADateEffectiveFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(APrevYearCorpExRateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            APrevYearCorpExRateAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AThisYearOldBatchCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AThisYearOldJournalTable MyAThisYearOldJournalTable = AThisYearOldJournalAccess.LoadViaAThisYearOldBatch(ALedgerNumber, ABatchNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldJournalCascading.DeleteUsingTemplate(MyAThisYearOldJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AThisYearOldBatchAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AThisYearOldBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AThisYearOldJournalTable MyAThisYearOldJournalTable = AThisYearOldJournalAccess.LoadViaAThisYearOldBatchTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldJournalCascading.DeleteUsingTemplate(MyAThisYearOldJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AThisYearOldBatchAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AThisYearOldJournalCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AThisYearOldTransactionTable MyAThisYearOldTransactionTable = AThisYearOldTransactionAccess.LoadViaAThisYearOldJournal(ALedgerNumber, ABatchNumber, AJournalNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldTransactionCascading.DeleteUsingTemplate(MyAThisYearOldTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AThisYearOldJournalAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AThisYearOldJournalRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AThisYearOldTransactionTable MyAThisYearOldTransactionTable = AThisYearOldTransactionAccess.LoadViaAThisYearOldJournalTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisYearOldTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisYearOldTransactionCascading.DeleteUsingTemplate(MyAThisYearOldTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AThisYearOldJournalAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AThisYearOldTransactionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AThisyearoldTransAnalAttribTable MyAThisyearoldTransAnalAttribTable = AThisyearoldTransAnalAttribAccess.LoadViaAThisYearOldTransaction(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisyearoldTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisyearoldTransAnalAttribCascading.DeleteUsingTemplate(MyAThisyearoldTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AThisYearOldTransactionAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AThisYearOldTransactionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AThisyearoldTransAnalAttribTable MyAThisyearoldTransAnalAttribTable = AThisyearoldTransAnalAttribAccess.LoadViaAThisYearOldTransactionTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAThisyearoldTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AThisyearoldTransAnalAttribCascading.DeleteUsingTemplate(MyAThisyearoldTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AThisYearOldTransactionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AThisyearoldTransAnalAttribCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber, String AAnalysisTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AThisyearoldTransAnalAttribAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, AAnalysisTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AThisyearoldTransAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AThisyearoldTransAnalAttribAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ARecurringBatchCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringJournalTable MyARecurringJournalTable = ARecurringJournalAccess.LoadViaARecurringBatch(ALedgerNumber, ABatchNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringJournalCascading.DeleteUsingTemplate(MyARecurringJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringBatchAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ARecurringBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringJournalTable MyARecurringJournalTable = ARecurringJournalAccess.LoadViaARecurringBatchTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringJournalCascading.DeleteUsingTemplate(MyARecurringJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringBatchAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ARecurringJournalCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringTransactionTable MyARecurringTransactionTable = ARecurringTransactionAccess.LoadViaARecurringJournal(ALedgerNumber, ABatchNumber, AJournalNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransactionCascading.DeleteUsingTemplate(MyARecurringTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringJournalAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ARecurringJournalRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringTransactionTable MyARecurringTransactionTable = ARecurringTransactionAccess.LoadViaARecurringJournalTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransactionCascading.DeleteUsingTemplate(MyARecurringTransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringJournalAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ARecurringTransactionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringTransAnalAttribTable MyARecurringTransAnalAttribTable = ARecurringTransAnalAttribAccess.LoadViaARecurringTransaction(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransAnalAttribCascading.DeleteUsingTemplate(MyARecurringTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringTransactionAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ARecurringTransactionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringTransAnalAttribTable MyARecurringTransAnalAttribTable = ARecurringTransAnalAttribAccess.LoadViaARecurringTransactionTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringTransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringTransAnalAttribCascading.DeleteUsingTemplate(MyARecurringTransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringTransactionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ARecurringTransAnalAttribCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber, String AAnalysisTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ARecurringTransAnalAttribAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, AAnalysisTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ARecurringTransAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ARecurringTransAnalAttribAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ARecurringGiftBatchCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringGiftTable MyARecurringGiftTable = ARecurringGiftAccess.LoadViaARecurringGiftBatch(ALedgerNumber, ABatchNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftCascading.DeleteUsingTemplate(MyARecurringGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringGiftBatchAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringGiftTable MyARecurringGiftTable = ARecurringGiftAccess.LoadViaARecurringGiftBatchTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftCascading.DeleteUsingTemplate(MyARecurringGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringGiftBatchAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ARecurringGiftCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringGiftDetailTable MyARecurringGiftDetailTable = ARecurringGiftDetailAccess.LoadViaARecurringGift(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftDetailCascading.DeleteUsingTemplate(MyARecurringGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringGiftAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ARecurringGiftDetailTable MyARecurringGiftDetailTable = ARecurringGiftDetailAccess.LoadViaARecurringGiftTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyARecurringGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    ARecurringGiftDetailCascading.DeleteUsingTemplate(MyARecurringGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ARecurringGiftAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ARecurringGiftDetailCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ARecurringGiftDetailAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ARecurringGiftDetailAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AGiftBatchCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AGiftTable MyAGiftTable = AGiftAccess.LoadViaAGiftBatch(ALedgerNumber, ABatchNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftCascading.DeleteUsingTemplate(MyAGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AGiftBatchAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AGiftTable MyAGiftTable = AGiftAccess.LoadViaAGiftBatchTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftCascading.DeleteUsingTemplate(MyAGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AGiftBatchAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AGiftCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AGiftDetailTable MyAGiftDetailTable = AGiftDetailAccess.LoadViaAGift(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftDetailCascading.DeleteUsingTemplate(MyAGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupGiftTable MySGroupGiftTable = SGroupGiftAccess.LoadViaAGift(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupGiftCascading.DeleteUsingTemplate(MySGroupGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AGiftAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AGiftDetailTable MyAGiftDetailTable = AGiftDetailAccess.LoadViaAGiftTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAGiftDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AGiftDetailCascading.DeleteUsingTemplate(MyAGiftDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupGiftTable MySGroupGiftTable = SGroupGiftAccess.LoadViaAGiftTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,a_ledger_number_i,a_batch_number_i,a_gift_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupGiftTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupGiftCascading.DeleteUsingTemplate(MySGroupGiftTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AGiftAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AGiftDetailCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AGiftDetailAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AGiftDetailAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ABatchCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AJournalTable MyAJournalTable = AJournalAccess.LoadViaABatch(ALedgerNumber, ABatchNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AJournalCascading.DeleteUsingTemplate(MyAJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ABatchAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ABatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AJournalTable MyAJournalTable = AJournalAccess.LoadViaABatchTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAJournalTable.Rows.Count); countRow = (countRow + 1))
                {
                    AJournalCascading.DeleteUsingTemplate(MyAJournalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ABatchAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AJournalCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ATransactionTable MyATransactionTable = ATransactionAccess.LoadViaAJournal(ALedgerNumber, ABatchNumber, AJournalNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransactionCascading.DeleteUsingTemplate(MyATransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AJournalAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AJournalRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ATransactionTable MyATransactionTable = ATransactionAccess.LoadViaAJournalTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransactionTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransactionCascading.DeleteUsingTemplate(MyATransactionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AJournalAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ATransactionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ATransAnalAttribTable MyATransAnalAttribTable = ATransAnalAttribAccess.LoadViaATransaction(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransAnalAttribCascading.DeleteUsingTemplate(MyATransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ATransactionAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ATransactionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ATransAnalAttribTable MyATransAnalAttribTable = ATransAnalAttribAccess.LoadViaATransactionTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_batch_number_i,a_journal_number_i,a_transaction_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyATransAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    ATransAnalAttribCascading.DeleteUsingTemplate(MyATransAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            ATransactionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ATransAnalAttribCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber, String AAnalysisTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ATransAnalAttribAccess.DeleteByPrimaryKey(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, AAnalysisTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ATransAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ATransAnalAttribAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ASuspenseAccountCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String ASuspenseAccountCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ASuspenseAccountAccess.DeleteByPrimaryKey(ALedgerNumber, ASuspenseAccountCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ASuspenseAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ASuspenseAccountAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ASpecialTransTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ASubSystemCode, String ATransactionTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ASpecialTransTypeAccess.DeleteByPrimaryKey(ASubSystemCode, ATransactionTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ASpecialTransTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ASpecialTransTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ASystemInterfaceCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String ASubSystemCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ASystemInterfaceAccess.DeleteByPrimaryKey(ALedgerNumber, ASubSystemCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ASystemInterfaceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ASystemInterfaceAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ACurrencyLanguageCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACurrencyCode, String ALanguageCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ACurrencyLanguageAccess.DeleteByPrimaryKey(ACurrencyCode, ALanguageCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ACurrencyLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ACurrencyLanguageAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AApSupplierCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AApDocumentTable MyAApDocumentTable = AApDocumentAccess.LoadViaAApSupplier(APartnerKey, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApDocumentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApDocumentCascading.DeleteUsingTemplate(MyAApDocumentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AApSupplierAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AApDocumentTable MyAApDocumentTable = AApDocumentAccess.LoadViaAApSupplierTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApDocumentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApDocumentCascading.DeleteUsingTemplate(MyAApDocumentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AApSupplierAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AApDocumentCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ACrdtNoteInvoiceLinkTable MyACrdtNoteInvoiceLinkCreditNoteNumberTable = ACrdtNoteInvoiceLinkAccess.LoadViaAApDocumentCreditNoteNumber(ALedgerNumber, AApNumber, StringHelper.StrSplit("a_ledger_number_i,a_credit_note_number_i,a_invoice_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyACrdtNoteInvoiceLinkCreditNoteNumberTable.Rows.Count); countRow = (countRow + 1))
                {
                    ACrdtNoteInvoiceLinkCascading.DeleteUsingTemplate(MyACrdtNoteInvoiceLinkCreditNoteNumberTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ACrdtNoteInvoiceLinkTable MyACrdtNoteInvoiceLinkInvoiceNumberTable = ACrdtNoteInvoiceLinkAccess.LoadViaAApDocumentInvoiceNumber(ALedgerNumber, AApNumber, StringHelper.StrSplit("a_ledger_number_i,a_credit_note_number_i,a_invoice_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyACrdtNoteInvoiceLinkInvoiceNumberTable.Rows.Count); countRow = (countRow + 1))
                {
                    ACrdtNoteInvoiceLinkCascading.DeleteUsingTemplate(MyACrdtNoteInvoiceLinkInvoiceNumberTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AApDocumentDetailTable MyAApDocumentDetailTable = AApDocumentDetailAccess.LoadViaAApDocument(ALedgerNumber, AApNumber, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApDocumentDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApDocumentDetailCascading.DeleteUsingTemplate(MyAApDocumentDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AApDocumentPaymentTable MyAApDocumentPaymentTable = AApDocumentPaymentAccess.LoadViaAApDocument(ALedgerNumber, AApNumber, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_payment_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApDocumentPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApDocumentPaymentCascading.DeleteUsingTemplate(MyAApDocumentPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AEpDocumentPaymentTable MyAEpDocumentPaymentTable = AEpDocumentPaymentAccess.LoadViaAApDocument(ALedgerNumber, AApNumber, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_payment_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpDocumentPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpDocumentPaymentCascading.DeleteUsingTemplate(MyAEpDocumentPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AApDocumentAccess.DeleteByPrimaryKey(ALedgerNumber, AApNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ACrdtNoteInvoiceLinkTable MyACrdtNoteInvoiceLinkCreditNoteNumberTable = ACrdtNoteInvoiceLinkAccess.LoadViaAApDocumentCreditNoteNumberTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_credit_note_number_i,a_invoice_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyACrdtNoteInvoiceLinkCreditNoteNumberTable.Rows.Count); countRow = (countRow + 1))
                {
                    ACrdtNoteInvoiceLinkCascading.DeleteUsingTemplate(MyACrdtNoteInvoiceLinkCreditNoteNumberTable[countRow], null, ATransaction, AWithCascDelete);
                }
                ACrdtNoteInvoiceLinkTable MyACrdtNoteInvoiceLinkInvoiceNumberTable = ACrdtNoteInvoiceLinkAccess.LoadViaAApDocumentInvoiceNumberTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_credit_note_number_i,a_invoice_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyACrdtNoteInvoiceLinkInvoiceNumberTable.Rows.Count); countRow = (countRow + 1))
                {
                    ACrdtNoteInvoiceLinkCascading.DeleteUsingTemplate(MyACrdtNoteInvoiceLinkInvoiceNumberTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AApDocumentDetailTable MyAApDocumentDetailTable = AApDocumentDetailAccess.LoadViaAApDocumentTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApDocumentDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApDocumentDetailCascading.DeleteUsingTemplate(MyAApDocumentDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AApDocumentPaymentTable MyAApDocumentPaymentTable = AApDocumentPaymentAccess.LoadViaAApDocumentTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_payment_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApDocumentPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApDocumentPaymentCascading.DeleteUsingTemplate(MyAApDocumentPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AEpDocumentPaymentTable MyAEpDocumentPaymentTable = AEpDocumentPaymentAccess.LoadViaAApDocumentTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_payment_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpDocumentPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpDocumentPaymentCascading.DeleteUsingTemplate(MyAEpDocumentPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AApDocumentAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class ACrdtNoteInvoiceLinkCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ACrdtNoteInvoiceLinkAccess.DeleteByPrimaryKey(ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            ACrdtNoteInvoiceLinkAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AApDocumentDetailCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AApAnalAttribTable MyAApAnalAttribTable = AApAnalAttribAccess.LoadViaAApDocumentDetail(ALedgerNumber, AApNumber, ADetailNumber, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_detail_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApAnalAttribCascading.DeleteUsingTemplate(MyAApAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AApDocumentDetailAccess.DeleteByPrimaryKey(ALedgerNumber, AApNumber, ADetailNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AApAnalAttribTable MyAApAnalAttribTable = AApAnalAttribAccess.LoadViaAApDocumentDetailTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_detail_number_i,a_analysis_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApAnalAttribTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApAnalAttribCascading.DeleteUsingTemplate(MyAApAnalAttribTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AApDocumentDetailAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AApPaymentCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AApDocumentPaymentTable MyAApDocumentPaymentTable = AApDocumentPaymentAccess.LoadViaAApPayment(ALedgerNumber, APaymentNumber, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_payment_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApDocumentPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApDocumentPaymentCascading.DeleteUsingTemplate(MyAApDocumentPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AApPaymentAccess.DeleteByPrimaryKey(ALedgerNumber, APaymentNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AApDocumentPaymentTable MyAApDocumentPaymentTable = AApDocumentPaymentAccess.LoadViaAApPaymentTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_payment_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAApDocumentPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AApDocumentPaymentCascading.DeleteUsingTemplate(MyAApDocumentPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AApPaymentAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AApDocumentPaymentCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AApDocumentPaymentAccess.DeleteByPrimaryKey(ALedgerNumber, AApNumber, APaymentNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AApDocumentPaymentAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AEpPaymentCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpDocumentPaymentTable MyAEpDocumentPaymentTable = AEpDocumentPaymentAccess.LoadViaAEpPayment(ALedgerNumber, APaymentNumber, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_payment_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpDocumentPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpDocumentPaymentCascading.DeleteUsingTemplate(MyAEpDocumentPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AEpPaymentAccess.DeleteByPrimaryKey(ALedgerNumber, APaymentNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AEpDocumentPaymentTable MyAEpDocumentPaymentTable = AEpDocumentPaymentAccess.LoadViaAEpPaymentTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_ap_number_i,a_payment_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAEpDocumentPaymentTable.Rows.Count); countRow = (countRow + 1))
                {
                    AEpDocumentPaymentCascading.DeleteUsingTemplate(MyAEpDocumentPaymentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AEpPaymentAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AEpDocumentPaymentCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AEpDocumentPaymentAccess.DeleteByPrimaryKey(ALedgerNumber, AApNumber, APaymentNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AEpDocumentPaymentAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AApAnalAttribCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AApAnalAttribAccess.DeleteByPrimaryKey(ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AApAnalAttribAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArCategoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AArCategoryCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArArticleTable MyAArArticleTable = AArArticleAccess.LoadViaAArCategory(AArCategoryCode, StringHelper.StrSplit("a_ar_article_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArArticleTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArArticleCascading.DeleteUsingTemplate(MyAArArticleTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDiscountPerCategoryTable MyAArDiscountPerCategoryTable = AArDiscountPerCategoryAccess.LoadViaAArCategory(AArCategoryCode, StringHelper.StrSplit("a_ar_category_code_c,a_ar_discount_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDiscountPerCategoryTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDiscountPerCategoryCascading.DeleteUsingTemplate(MyAArDiscountPerCategoryTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDefaultDiscountTable MyAArDefaultDiscountTable = AArDefaultDiscountAccess.LoadViaAArCategory(AArCategoryCode, StringHelper.StrSplit("a_ar_category_code_c,a_ar_discount_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDefaultDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDefaultDiscountCascading.DeleteUsingTemplate(MyAArDefaultDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArCategoryAccess.DeleteByPrimaryKey(AArCategoryCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArArticleTable MyAArArticleTable = AArArticleAccess.LoadViaAArCategoryTemplate(ATemplateRow, StringHelper.StrSplit("a_ar_article_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArArticleTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArArticleCascading.DeleteUsingTemplate(MyAArArticleTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDiscountPerCategoryTable MyAArDiscountPerCategoryTable = AArDiscountPerCategoryAccess.LoadViaAArCategoryTemplate(ATemplateRow, StringHelper.StrSplit("a_ar_category_code_c,a_ar_discount_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDiscountPerCategoryTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDiscountPerCategoryCascading.DeleteUsingTemplate(MyAArDiscountPerCategoryTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDefaultDiscountTable MyAArDefaultDiscountTable = AArDefaultDiscountAccess.LoadViaAArCategoryTemplate(ATemplateRow, StringHelper.StrSplit("a_ar_category_code_c,a_ar_discount_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDefaultDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDefaultDiscountCascading.DeleteUsingTemplate(MyAArDefaultDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArCategoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArArticleCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AArArticleCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArArticlePriceTable MyAArArticlePriceTable = AArArticlePriceAccess.LoadViaAArArticle(AArArticleCode, StringHelper.StrSplit("a_ar_article_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArArticlePriceTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArArticlePriceCascading.DeleteUsingTemplate(MyAArArticlePriceTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDiscountTable MyAArDiscountTable = AArDiscountAccess.LoadViaAArArticle(AArArticleCode, StringHelper.StrSplit("a_ar_discount_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDiscountCascading.DeleteUsingTemplate(MyAArDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDetailTable MyAArInvoiceDetailTable = AArInvoiceDetailAccess.LoadViaAArArticle(AArArticleCode, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailCascading.DeleteUsingTemplate(MyAArInvoiceDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArArticleAccess.DeleteByPrimaryKey(AArArticleCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArArticlePriceTable MyAArArticlePriceTable = AArArticlePriceAccess.LoadViaAArArticleTemplate(ATemplateRow, StringHelper.StrSplit("a_ar_article_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArArticlePriceTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArArticlePriceCascading.DeleteUsingTemplate(MyAArArticlePriceTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDiscountTable MyAArDiscountTable = AArDiscountAccess.LoadViaAArArticleTemplate(ATemplateRow, StringHelper.StrSplit("a_ar_discount_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDiscountCascading.DeleteUsingTemplate(MyAArDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDetailTable MyAArInvoiceDetailTable = AArInvoiceDetailAccess.LoadViaAArArticleTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailCascading.DeleteUsingTemplate(MyAArInvoiceDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArArticleAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArArticlePriceCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArInvoiceDetailTable MyAArInvoiceDetailTable = AArInvoiceDetailAccess.LoadViaAArArticlePrice(AArArticleCode, AArDateValidFrom, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailCascading.DeleteUsingTemplate(MyAArInvoiceDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArArticlePriceAccess.DeleteByPrimaryKey(AArArticleCode, AArDateValidFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArInvoiceDetailTable MyAArInvoiceDetailTable = AArInvoiceDetailAccess.LoadViaAArArticlePriceTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailCascading.DeleteUsingTemplate(MyAArInvoiceDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArArticlePriceAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArDiscountCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArDiscountPerCategoryTable MyAArDiscountPerCategoryTable = AArDiscountPerCategoryAccess.LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, StringHelper.StrSplit("a_ar_category_code_c,a_ar_discount_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDiscountPerCategoryTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDiscountPerCategoryCascading.DeleteUsingTemplate(MyAArDiscountPerCategoryTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDefaultDiscountTable MyAArDefaultDiscountTable = AArDefaultDiscountAccess.LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, StringHelper.StrSplit("a_ar_category_code_c,a_ar_discount_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDefaultDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDefaultDiscountCascading.DeleteUsingTemplate(MyAArDefaultDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDiscountTable MyAArInvoiceDiscountTable = AArInvoiceDiscountAccess.LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_ar_discount_code_c,a_ar_discount_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDiscountCascading.DeleteUsingTemplate(MyAArInvoiceDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDetailDiscountTable MyAArInvoiceDetailDiscountTable = AArInvoiceDetailDiscountAccess.LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i,a_ar_discount_code_c,a_ar_discount_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailDiscountCascading.DeleteUsingTemplate(MyAArInvoiceDetailDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArDiscountAccess.DeleteByPrimaryKey(AArDiscountCode, AArDateValidFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArDiscountPerCategoryTable MyAArDiscountPerCategoryTable = AArDiscountPerCategoryAccess.LoadViaAArDiscountTemplate(ATemplateRow, StringHelper.StrSplit("a_ar_category_code_c,a_ar_discount_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDiscountPerCategoryTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDiscountPerCategoryCascading.DeleteUsingTemplate(MyAArDiscountPerCategoryTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArDefaultDiscountTable MyAArDefaultDiscountTable = AArDefaultDiscountAccess.LoadViaAArDiscountTemplate(ATemplateRow, StringHelper.StrSplit("a_ar_category_code_c,a_ar_discount_code_c,a_ar_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArDefaultDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArDefaultDiscountCascading.DeleteUsingTemplate(MyAArDefaultDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDiscountTable MyAArInvoiceDiscountTable = AArInvoiceDiscountAccess.LoadViaAArDiscountTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_ar_discount_code_c,a_ar_discount_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDiscountCascading.DeleteUsingTemplate(MyAArInvoiceDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDetailDiscountTable MyAArInvoiceDetailDiscountTable = AArInvoiceDetailDiscountAccess.LoadViaAArDiscountTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i,a_ar_discount_code_c,a_ar_discount_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailDiscountCascading.DeleteUsingTemplate(MyAArInvoiceDetailDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArDiscountAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArDiscountPerCategoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AArCategoryCode, String AArDiscountCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AArDiscountPerCategoryAccess.DeleteByPrimaryKey(AArCategoryCode, AArDiscountCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AArDiscountPerCategoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArDefaultDiscountCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AArDefaultDiscountAccess.DeleteByPrimaryKey(AArCategoryCode, AArDiscountCode, AArDateValidFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AArDefaultDiscountAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArInvoiceCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArInvoiceDetailTable MyAArInvoiceDetailTable = AArInvoiceDetailAccess.LoadViaAArInvoice(ALedgerNumber, AKey, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailCascading.DeleteUsingTemplate(MyAArInvoiceDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDiscountTable MyAArInvoiceDiscountTable = AArInvoiceDiscountAccess.LoadViaAArInvoice(ALedgerNumber, AKey, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_ar_discount_code_c,a_ar_discount_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDiscountCascading.DeleteUsingTemplate(MyAArInvoiceDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PhBookingTable MyPhBookingTable = PhBookingAccess.LoadViaAArInvoice(ALedgerNumber, AKey, StringHelper.StrSplit("ph_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPhBookingTable.Rows.Count); countRow = (countRow + 1))
                {
                    PhBookingCascading.DeleteUsingTemplate(MyPhBookingTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArInvoiceAccess.DeleteByPrimaryKey(ALedgerNumber, AKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArInvoiceDetailTable MyAArInvoiceDetailTable = AArInvoiceDetailAccess.LoadViaAArInvoiceTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailCascading.DeleteUsingTemplate(MyAArInvoiceDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                AArInvoiceDiscountTable MyAArInvoiceDiscountTable = AArInvoiceDiscountAccess.LoadViaAArInvoiceTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_ar_discount_code_c,a_ar_discount_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDiscountCascading.DeleteUsingTemplate(MyAArInvoiceDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PhBookingTable MyPhBookingTable = PhBookingAccess.LoadViaAArInvoiceTemplate(ATemplateRow, StringHelper.StrSplit("ph_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPhBookingTable.Rows.Count); countRow = (countRow + 1))
                {
                    PhBookingCascading.DeleteUsingTemplate(MyPhBookingTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArInvoiceAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArInvoiceDetailCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArInvoiceDetailDiscountTable MyAArInvoiceDetailDiscountTable = AArInvoiceDetailDiscountAccess.LoadViaAArInvoiceDetail(ALedgerNumber, AInvoiceKey, ADetailNumber, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i,a_ar_discount_code_c,a_ar_discount_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailDiscountCascading.DeleteUsingTemplate(MyAArInvoiceDetailDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArInvoiceDetailAccess.DeleteByPrimaryKey(ALedgerNumber, AInvoiceKey, ADetailNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                AArInvoiceDetailDiscountTable MyAArInvoiceDetailDiscountTable = AArInvoiceDetailDiscountAccess.LoadViaAArInvoiceDetailTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_invoice_key_i,a_detail_number_i,a_ar_discount_code_c,a_ar_discount_date_valid_from_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyAArInvoiceDetailDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    AArInvoiceDetailDiscountCascading.DeleteUsingTemplate(MyAArInvoiceDetailDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            AArInvoiceDetailAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArInvoiceDiscountCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AArInvoiceDiscountAccess.DeleteByPrimaryKey(ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AArInvoiceDiscountAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AArInvoiceDetailDiscountCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AArInvoiceDetailDiscountAccess.DeleteByPrimaryKey(ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            AArInvoiceDetailDiscountAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtApplicantStatusCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmGeneralApplicationTable MyPmGeneralApplicationTable = PmGeneralApplicationAccess.LoadViaPtApplicantStatus(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmGeneralApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmGeneralApplicationCascading.DeleteUsingTemplate(MyPmGeneralApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationStatusHistoryTable MyPmApplicationStatusHistoryTable = PmApplicationStatusHistoryAccess.LoadViaPtApplicantStatus(ACode, StringHelper.StrSplit("pm_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationStatusHistoryTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationStatusHistoryCascading.DeleteUsingTemplate(MyPmApplicationStatusHistoryTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtApplicantStatusAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtApplicantStatusRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmGeneralApplicationTable MyPmGeneralApplicationTable = PmGeneralApplicationAccess.LoadViaPtApplicantStatusTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmGeneralApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmGeneralApplicationCascading.DeleteUsingTemplate(MyPmGeneralApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationStatusHistoryTable MyPmApplicationStatusHistoryTable = PmApplicationStatusHistoryAccess.LoadViaPtApplicantStatusTemplate(ATemplateRow, StringHelper.StrSplit("pm_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationStatusHistoryTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationStatusHistoryCascading.DeleteUsingTemplate(MyPmApplicationStatusHistoryTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtApplicantStatusAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtApplicationTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AAppTypeName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmGeneralApplicationTable MyPmGeneralApplicationTable = PmGeneralApplicationAccess.LoadViaPtApplicationType(AAppTypeName, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmGeneralApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmGeneralApplicationCascading.DeleteUsingTemplate(MyPmGeneralApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtApplicationTypeAccess.DeleteByPrimaryKey(AAppTypeName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtApplicationTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmGeneralApplicationTable MyPmGeneralApplicationTable = PmGeneralApplicationAccess.LoadViaPtApplicationTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmGeneralApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmGeneralApplicationCascading.DeleteUsingTemplate(MyPmGeneralApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtApplicationTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtContactCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AContactName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmGeneralApplicationTable MyPmGeneralApplicationGenContact1Table = PmGeneralApplicationAccess.LoadViaPtContactGenContact1(AContactName, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmGeneralApplicationGenContact1Table.Rows.Count); countRow = (countRow + 1))
                {
                    PmGeneralApplicationCascading.DeleteUsingTemplate(MyPmGeneralApplicationGenContact1Table[countRow], null, ATransaction, AWithCascDelete);
                }
                PmGeneralApplicationTable MyPmGeneralApplicationGenContact2Table = PmGeneralApplicationAccess.LoadViaPtContactGenContact2(AContactName, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmGeneralApplicationGenContact2Table.Rows.Count); countRow = (countRow + 1))
                {
                    PmGeneralApplicationCascading.DeleteUsingTemplate(MyPmGeneralApplicationGenContact2Table[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtContactAccess.DeleteByPrimaryKey(AContactName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmGeneralApplicationTable MyPmGeneralApplicationGenContact1Table = PmGeneralApplicationAccess.LoadViaPtContactGenContact1Template(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmGeneralApplicationGenContact1Table.Rows.Count); countRow = (countRow + 1))
                {
                    PmGeneralApplicationCascading.DeleteUsingTemplate(MyPmGeneralApplicationGenContact1Table[countRow], null, ATransaction, AWithCascDelete);
                }
                PmGeneralApplicationTable MyPmGeneralApplicationGenContact2Table = PmGeneralApplicationAccess.LoadViaPtContactGenContact2Template(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmGeneralApplicationGenContact2Table.Rows.Count); countRow = (countRow + 1))
                {
                    PmGeneralApplicationCascading.DeleteUsingTemplate(MyPmGeneralApplicationGenContact2Table[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtContactAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmGeneralApplicationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AApplicationKey, Int64 ARegistrationOffice, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmApplicationStatusHistoryTable MyPmApplicationStatusHistoryTable = PmApplicationStatusHistoryAccess.LoadViaPmGeneralApplication(APartnerKey, AApplicationKey, ARegistrationOffice, StringHelper.StrSplit("pm_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationStatusHistoryTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationStatusHistoryCascading.DeleteUsingTemplate(MyPmApplicationStatusHistoryTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPmGeneralApplication(APartnerKey, AApplicationKey, ARegistrationOffice, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmYearProgramApplicationTable MyPmYearProgramApplicationTable = PmYearProgramApplicationAccess.LoadViaPmGeneralApplication(APartnerKey, AApplicationKey, ARegistrationOffice, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmYearProgramApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmYearProgramApplicationCascading.DeleteUsingTemplate(MyPmYearProgramApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationFormsTable MyPmApplicationFormsTable = PmApplicationFormsAccess.LoadViaPmGeneralApplication(APartnerKey, AApplicationKey, ARegistrationOffice, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n,pt_form_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFormsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFormsCascading.DeleteUsingTemplate(MyPmApplicationFormsTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PDataLabelValueApplicationTable MyPDataLabelValueApplicationTable = PDataLabelValueApplicationAccess.LoadViaPmGeneralApplication(APartnerKey, AApplicationKey, ARegistrationOffice, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n,p_data_label_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelValueApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelValueApplicationCascading.DeleteUsingTemplate(MyPDataLabelValueApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationFileTable MyPmApplicationFileTable = PmApplicationFileAccess.LoadViaPmGeneralApplication(APartnerKey, AApplicationKey, ARegistrationOffice, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFileCascading.DeleteUsingTemplate(MyPmApplicationFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmGeneralApplicationAccess.DeleteByPrimaryKey(APartnerKey, AApplicationKey, ARegistrationOffice, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmGeneralApplicationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmApplicationStatusHistoryTable MyPmApplicationStatusHistoryTable = PmApplicationStatusHistoryAccess.LoadViaPmGeneralApplicationTemplate(ATemplateRow, StringHelper.StrSplit("pm_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationStatusHistoryTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationStatusHistoryCascading.DeleteUsingTemplate(MyPmApplicationStatusHistoryTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPmGeneralApplicationTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmYearProgramApplicationTable MyPmYearProgramApplicationTable = PmYearProgramApplicationAccess.LoadViaPmGeneralApplicationTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmYearProgramApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmYearProgramApplicationCascading.DeleteUsingTemplate(MyPmYearProgramApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationFormsTable MyPmApplicationFormsTable = PmApplicationFormsAccess.LoadViaPmGeneralApplicationTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n,pt_form_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFormsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFormsCascading.DeleteUsingTemplate(MyPmApplicationFormsTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PDataLabelValueApplicationTable MyPDataLabelValueApplicationTable = PDataLabelValueApplicationAccess.LoadViaPmGeneralApplicationTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n,p_data_label_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelValueApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelValueApplicationCascading.DeleteUsingTemplate(MyPDataLabelValueApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationFileTable MyPmApplicationFileTable = PmApplicationFileAccess.LoadViaPmGeneralApplicationTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFileCascading.DeleteUsingTemplate(MyPmApplicationFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmGeneralApplicationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmApplicationStatusHistoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmApplicationStatusHistoryAccess.DeleteByPrimaryKey(AKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmApplicationStatusHistoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmApplicationStatusHistoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtSpecialApplicantCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPtSpecialApplicant(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtSpecialApplicantAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtSpecialApplicantRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPtSpecialApplicantTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtSpecialApplicantAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtLeadershipRatingCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPtLeadershipRating(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtLeadershipRatingAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtLeadershipRatingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPtLeadershipRatingTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtLeadershipRatingAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtArrivalPointCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationArrivalPointCodeTable = PmShortTermApplicationAccess.LoadViaPtArrivalPointArrivalPointCode(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationArrivalPointCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationArrivalPointCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationDeparturePointCodeTable = PmShortTermApplicationAccess.LoadViaPtArrivalPointDeparturePointCode(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationDeparturePointCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationDeparturePointCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtArrivalPointAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtArrivalPointRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationArrivalPointCodeTable = PmShortTermApplicationAccess.LoadViaPtArrivalPointArrivalPointCodeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationArrivalPointCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationArrivalPointCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationDeparturePointCodeTable = PmShortTermApplicationAccess.LoadViaPtArrivalPointDeparturePointCodeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationDeparturePointCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationDeparturePointCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtArrivalPointAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtXyzTbdPreferenceLevelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationStCountryPrefTable = PmShortTermApplicationAccess.LoadViaPtXyzTbdPreferenceLevelStCountryPref(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationStCountryPrefTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationStCountryPrefTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationStActivityPrefTable = PmShortTermApplicationAccess.LoadViaPtXyzTbdPreferenceLevelStActivityPref(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationStActivityPrefTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationStActivityPrefTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtXyzTbdPreferenceLevelAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtXyzTbdPreferenceLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationStCountryPrefTable = PmShortTermApplicationAccess.LoadViaPtXyzTbdPreferenceLevelStCountryPrefTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationStCountryPrefTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationStCountryPrefTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationStActivityPrefTable = PmShortTermApplicationAccess.LoadViaPtXyzTbdPreferenceLevelStActivityPrefTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationStActivityPrefTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationStActivityPrefTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtXyzTbdPreferenceLevelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtCongressCodeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationStPreCongressCodeTable = PmShortTermApplicationAccess.LoadViaPtCongressCodeStPreCongressCode(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationStPreCongressCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationStPreCongressCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationStCongressCodeTable = PmShortTermApplicationAccess.LoadViaPtCongressCodeStCongressCode(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationStCongressCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationStCongressCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationXyzTbdRoleTable = PmShortTermApplicationAccess.LoadViaPtCongressCodeXyzTbdRole(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationXyzTbdRoleTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationXyzTbdRoleTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtCongressCodeAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtCongressCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationStPreCongressCodeTable = PmShortTermApplicationAccess.LoadViaPtCongressCodeStPreCongressCodeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationStPreCongressCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationStPreCongressCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationStCongressCodeTable = PmShortTermApplicationAccess.LoadViaPtCongressCodeStCongressCodeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationStCongressCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationStCongressCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationXyzTbdRoleTable = PmShortTermApplicationAccess.LoadViaPtCongressCodeXyzTbdRoleTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationXyzTbdRoleTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationXyzTbdRoleTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtCongressCodeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtPartyTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPtPartyType(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtPartyTypeAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtPartyTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationTable = PmShortTermApplicationAccess.LoadViaPtPartyTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtPartyTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtTravelTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationTravelTypeToCongCodeTable = PmShortTermApplicationAccess.LoadViaPtTravelTypeTravelTypeToCongCode(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTravelTypeToCongCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTravelTypeToCongCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationTravelTypeFromCongCodeTable = PmShortTermApplicationAccess.LoadViaPtTravelTypeTravelTypeFromCongCode(ACode, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTravelTypeFromCongCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTravelTypeFromCongCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtTravelTypeAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtTravelTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmShortTermApplicationTable MyPmShortTermApplicationTravelTypeToCongCodeTable = PmShortTermApplicationAccess.LoadViaPtTravelTypeTravelTypeToCongCodeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTravelTypeToCongCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTravelTypeToCongCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmShortTermApplicationTable MyPmShortTermApplicationTravelTypeFromCongCodeTable = PmShortTermApplicationAccess.LoadViaPtTravelTypeTravelTypeFromCongCodeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmShortTermApplicationTravelTypeFromCongCodeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmShortTermApplicationCascading.DeleteUsingTemplate(MyPmShortTermApplicationTravelTypeFromCongCodeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtTravelTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmShortTermApplicationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AApplicationKey, Int64 ARegistrationOffice, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmShortTermApplicationAccess.DeleteByPrimaryKey(APartnerKey, AApplicationKey, ARegistrationOffice, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmShortTermApplicationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmShortTermApplicationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmYearProgramApplicationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AApplicationKey, Int64 ARegistrationOffice, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmYearProgramApplicationAccess.DeleteByPrimaryKey(APartnerKey, AApplicationKey, ARegistrationOffice, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmYearProgramApplicationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmYearProgramApplicationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtAppFormTypesCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFormName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmApplicationFormsTable MyPmApplicationFormsTable = PmApplicationFormsAccess.LoadViaPtAppFormTypes(AFormName, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n,pt_form_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFormsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFormsCascading.DeleteUsingTemplate(MyPmApplicationFormsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtAppFormTypesAccess.DeleteByPrimaryKey(AFormName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtAppFormTypesRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmApplicationFormsTable MyPmApplicationFormsTable = PmApplicationFormsAccess.LoadViaPtAppFormTypesTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n,pt_form_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFormsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFormsCascading.DeleteUsingTemplate(MyPmApplicationFormsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtAppFormTypesAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmApplicationFormsCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AApplicationKey, Int64 ARegistrationOffice, String AFormName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmApplicationFormsFileTable MyPmApplicationFormsFileTable = PmApplicationFormsFileAccess.LoadViaPmApplicationForms(APartnerKey, AApplicationKey, ARegistrationOffice, AFormName, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFormsFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFormsFileCascading.DeleteUsingTemplate(MyPmApplicationFormsFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmApplicationFormsAccess.DeleteByPrimaryKey(APartnerKey, AApplicationKey, ARegistrationOffice, AFormName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmApplicationFormsRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmApplicationFormsFileTable MyPmApplicationFormsFileTable = PmApplicationFormsFileAccess.LoadViaPmApplicationFormsTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFormsFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFormsFileCascading.DeleteUsingTemplate(MyPmApplicationFormsFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmApplicationFormsAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmDocumentCategoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmDocumentTypeTable MyPmDocumentTypeTable = PmDocumentTypeAccess.LoadViaPmDocumentCategory(ACode, StringHelper.StrSplit("pm_doc_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmDocumentTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmDocumentTypeCascading.DeleteUsingTemplate(MyPmDocumentTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmDocumentCategoryAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmDocumentCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmDocumentTypeTable MyPmDocumentTypeTable = PmDocumentTypeAccess.LoadViaPmDocumentCategoryTemplate(ATemplateRow, StringHelper.StrSplit("pm_doc_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmDocumentTypeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmDocumentTypeCascading.DeleteUsingTemplate(MyPmDocumentTypeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmDocumentCategoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmDocumentTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ADocCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmDocumentTable MyPmDocumentTable = PmDocumentAccess.LoadViaPmDocumentType(ADocCode, StringHelper.StrSplit("p_site_key_n,pm_document_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmDocumentTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmDocumentCascading.DeleteUsingTemplate(MyPmDocumentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmDocumentTypeAccess.DeleteByPrimaryKey(ADocCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmDocumentTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmDocumentTable MyPmDocumentTable = PmDocumentAccess.LoadViaPmDocumentTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_site_key_n,pm_document_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmDocumentTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmDocumentCascading.DeleteUsingTemplate(MyPmDocumentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmDocumentTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmDocumentCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 ASiteKey, Int64 ADocumentKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmDocumentFileTable MyPmDocumentFileTable = PmDocumentFileAccess.LoadViaPmDocument(ASiteKey, ADocumentKey, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmDocumentFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmDocumentFileCascading.DeleteUsingTemplate(MyPmDocumentFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmDocumentAccess.DeleteByPrimaryKey(ASiteKey, ADocumentKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmDocumentFileTable MyPmDocumentFileTable = PmDocumentFileAccess.LoadViaPmDocumentTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmDocumentFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmDocumentFileCascading.DeleteUsingTemplate(MyPmDocumentFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmDocumentAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtPassportTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPassportDetailsTable MyPmPassportDetailsTable = PmPassportDetailsAccess.LoadViaPtPassportType(ACode, StringHelper.StrSplit("p_partner_key_n,pm_passport_number_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPassportDetailsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPassportDetailsCascading.DeleteUsingTemplate(MyPmPassportDetailsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtPassportTypeAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtPassportTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPassportDetailsTable MyPmPassportDetailsTable = PmPassportDetailsAccess.LoadViaPtPassportTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_passport_number_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPassportDetailsTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPassportDetailsCascading.DeleteUsingTemplate(MyPmPassportDetailsTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtPassportTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPassportDetailsCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String APassportNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPassportDetailsAccess.DeleteByPrimaryKey(APartnerKey, APassportNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPassportDetailsRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPassportDetailsAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmLongTermSupportFiguresCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 ARecordNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmLongTermSupportFiguresAccess.DeleteByPrimaryKey(APartnerKey, ARecordNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmLongTermSupportFiguresRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmLongTermSupportFiguresAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtLanguageLevelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALanguageLevel, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonLanguageTable MyPmPersonLanguageTable = PmPersonLanguageAccess.LoadViaPtLanguageLevel(ALanguageLevel, StringHelper.StrSplit("p_partner_key_n,p_language_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonLanguageTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonLanguageCascading.DeleteUsingTemplate(MyPmPersonLanguageTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobLanguageTable MyUmJobLanguageTable = UmJobLanguageAccess.LoadViaPtLanguageLevel(ALanguageLevel, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,p_language_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobLanguageTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobLanguageCascading.DeleteUsingTemplate(MyUmJobLanguageTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitLanguageTable MyUmUnitLanguageTable = UmUnitLanguageAccess.LoadViaPtLanguageLevel(ALanguageLevel, StringHelper.StrSplit("p_partner_key_n,p_language_code_c,pt_language_level_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitLanguageTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitLanguageCascading.DeleteUsingTemplate(MyUmUnitLanguageTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtLanguageLevelAccess.DeleteByPrimaryKey(ALanguageLevel, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonLanguageTable MyPmPersonLanguageTable = PmPersonLanguageAccess.LoadViaPtLanguageLevelTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_language_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonLanguageTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonLanguageCascading.DeleteUsingTemplate(MyPmPersonLanguageTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobLanguageTable MyUmJobLanguageTable = UmJobLanguageAccess.LoadViaPtLanguageLevelTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,p_language_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobLanguageTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobLanguageCascading.DeleteUsingTemplate(MyUmJobLanguageTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitLanguageTable MyUmUnitLanguageTable = UmUnitLanguageAccess.LoadViaPtLanguageLevelTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_language_code_c,pt_language_level_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitLanguageTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitLanguageCascading.DeleteUsingTemplate(MyUmUnitLanguageTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtLanguageLevelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersonLanguageCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String ALanguageCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonLanguageAccess.DeleteByPrimaryKey(APartnerKey, ALanguageCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersonLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonLanguageAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtValuableItemCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AValuableItemName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmOwnershipTable MyPmOwnershipTable = PmOwnershipAccess.LoadViaPtValuableItem(AValuableItemName, StringHelper.StrSplit("p_partner_key_n,pt_valuable_item_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmOwnershipTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmOwnershipCascading.DeleteUsingTemplate(MyPmOwnershipTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtValuableItemAccess.DeleteByPrimaryKey(AValuableItemName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtValuableItemRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmOwnershipTable MyPmOwnershipTable = PmOwnershipAccess.LoadViaPtValuableItemTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_valuable_item_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmOwnershipTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmOwnershipCascading.DeleteUsingTemplate(MyPmOwnershipTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtValuableItemAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmOwnershipCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AValuableItemName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmOwnershipAccess.DeleteByPrimaryKey(APartnerKey, AValuableItemName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmOwnershipRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmOwnershipAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPastExperienceCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 ASiteKey, Int64 AKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPastExperienceAccess.DeleteByPrimaryKey(ASiteKey, AKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPastExperienceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPastExperienceAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtAbilityAreaCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AAbilityAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonAbilityTable MyPmPersonAbilityTable = PmPersonAbilityAccess.LoadViaPtAbilityArea(AAbilityAreaName, StringHelper.StrSplit("p_partner_key_n,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonAbilityTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonAbilityCascading.DeleteUsingTemplate(MyPmPersonAbilityTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobRequirementTable MyUmJobRequirementTable = UmJobRequirementAccess.LoadViaPtAbilityArea(AAbilityAreaName, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobRequirementTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobRequirementCascading.DeleteUsingTemplate(MyUmJobRequirementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitAbilityTable MyUmUnitAbilityTable = UmUnitAbilityAccess.LoadViaPtAbilityArea(AAbilityAreaName, StringHelper.StrSplit("p_partner_key_n,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitAbilityTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitAbilityCascading.DeleteUsingTemplate(MyUmUnitAbilityTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtAbilityAreaAccess.DeleteByPrimaryKey(AAbilityAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonAbilityTable MyPmPersonAbilityTable = PmPersonAbilityAccess.LoadViaPtAbilityAreaTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonAbilityTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonAbilityCascading.DeleteUsingTemplate(MyPmPersonAbilityTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobRequirementTable MyUmJobRequirementTable = UmJobRequirementAccess.LoadViaPtAbilityAreaTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobRequirementTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobRequirementCascading.DeleteUsingTemplate(MyUmJobRequirementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitAbilityTable MyUmUnitAbilityTable = UmUnitAbilityAccess.LoadViaPtAbilityAreaTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitAbilityTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitAbilityCascading.DeleteUsingTemplate(MyUmUnitAbilityTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtAbilityAreaAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtAbilityLevelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AAbilityLevel, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonAbilityTable MyPmPersonAbilityTable = PmPersonAbilityAccess.LoadViaPtAbilityLevel(AAbilityLevel, StringHelper.StrSplit("p_partner_key_n,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonAbilityTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonAbilityCascading.DeleteUsingTemplate(MyPmPersonAbilityTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobRequirementTable MyUmJobRequirementTable = UmJobRequirementAccess.LoadViaPtAbilityLevel(AAbilityLevel, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobRequirementTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobRequirementCascading.DeleteUsingTemplate(MyUmJobRequirementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitAbilityTable MyUmUnitAbilityTable = UmUnitAbilityAccess.LoadViaPtAbilityLevel(AAbilityLevel, StringHelper.StrSplit("p_partner_key_n,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitAbilityTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitAbilityCascading.DeleteUsingTemplate(MyUmUnitAbilityTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtAbilityLevelAccess.DeleteByPrimaryKey(AAbilityLevel, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonAbilityTable MyPmPersonAbilityTable = PmPersonAbilityAccess.LoadViaPtAbilityLevelTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonAbilityTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonAbilityCascading.DeleteUsingTemplate(MyPmPersonAbilityTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobRequirementTable MyUmJobRequirementTable = UmJobRequirementAccess.LoadViaPtAbilityLevelTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobRequirementTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobRequirementCascading.DeleteUsingTemplate(MyUmJobRequirementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitAbilityTable MyUmUnitAbilityTable = UmUnitAbilityAccess.LoadViaPtAbilityLevelTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitAbilityTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitAbilityCascading.DeleteUsingTemplate(MyUmUnitAbilityTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtAbilityLevelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersonAbilityCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AAbilityAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonAbilityAccess.DeleteByPrimaryKey(APartnerKey, AAbilityAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersonAbilityRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonAbilityAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtQualificationAreaCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AQualificationAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonQualificationTable MyPmPersonQualificationTable = PmPersonQualificationAccess.LoadViaPtQualificationArea(AQualificationAreaName, StringHelper.StrSplit("p_partner_key_n,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonQualificationCascading.DeleteUsingTemplate(MyPmPersonQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobQualificationTable MyUmJobQualificationTable = UmJobQualificationAccess.LoadViaPtQualificationArea(AQualificationAreaName, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobQualificationCascading.DeleteUsingTemplate(MyUmJobQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtQualificationAreaAccess.DeleteByPrimaryKey(AQualificationAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonQualificationTable MyPmPersonQualificationTable = PmPersonQualificationAccess.LoadViaPtQualificationAreaTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonQualificationCascading.DeleteUsingTemplate(MyPmPersonQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobQualificationTable MyUmJobQualificationTable = UmJobQualificationAccess.LoadViaPtQualificationAreaTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobQualificationCascading.DeleteUsingTemplate(MyUmJobQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtQualificationAreaAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtQualificationLevelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AQualificationLevel, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonQualificationTable MyPmPersonQualificationTable = PmPersonQualificationAccess.LoadViaPtQualificationLevel(AQualificationLevel, StringHelper.StrSplit("p_partner_key_n,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonQualificationCascading.DeleteUsingTemplate(MyPmPersonQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobQualificationTable MyUmJobQualificationTable = UmJobQualificationAccess.LoadViaPtQualificationLevel(AQualificationLevel, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobQualificationCascading.DeleteUsingTemplate(MyUmJobQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtQualificationLevelAccess.DeleteByPrimaryKey(AQualificationLevel, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtQualificationLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonQualificationTable MyPmPersonQualificationTable = PmPersonQualificationAccess.LoadViaPtQualificationLevelTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonQualificationCascading.DeleteUsingTemplate(MyPmPersonQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobQualificationTable MyUmJobQualificationTable = UmJobQualificationAccess.LoadViaPtQualificationLevelTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobQualificationCascading.DeleteUsingTemplate(MyUmJobQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtQualificationLevelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersonQualificationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AQualificationAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonQualificationAccess.DeleteByPrimaryKey(APartnerKey, AQualificationAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersonQualificationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonQualificationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtSkillCategoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonSkillTable MyPmPersonSkillTable = PmPersonSkillAccess.LoadViaPtSkillCategory(ACode, StringHelper.StrSplit("pm_person_skill_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonSkillTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonSkillCascading.DeleteUsingTemplate(MyPmPersonSkillTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtSkillCategoryAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtSkillCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonSkillTable MyPmPersonSkillTable = PmPersonSkillAccess.LoadViaPtSkillCategoryTemplate(ATemplateRow, StringHelper.StrSplit("pm_person_skill_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonSkillTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonSkillCascading.DeleteUsingTemplate(MyPmPersonSkillTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtSkillCategoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtSkillLevelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ALevel, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonSkillTable MyPmPersonSkillTable = PmPersonSkillAccess.LoadViaPtSkillLevel(ALevel, StringHelper.StrSplit("pm_person_skill_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonSkillTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonSkillCascading.DeleteUsingTemplate(MyPmPersonSkillTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtSkillLevelAccess.DeleteByPrimaryKey(ALevel, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtSkillLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonSkillTable MyPmPersonSkillTable = PmPersonSkillAccess.LoadViaPtSkillLevelTemplate(ATemplateRow, StringHelper.StrSplit("pm_person_skill_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonSkillTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonSkillCascading.DeleteUsingTemplate(MyPmPersonSkillTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtSkillLevelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersonSkillCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 APersonSkillKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonSkillAccess.DeleteByPrimaryKey(APersonSkillKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersonSkillRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonSkillAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmFormalEducationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AFormalEducationKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmFormalEducationAccess.DeleteByPrimaryKey(AFormalEducationKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmFormalEducationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmFormalEducationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtDriverStatusCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonalDataTable MyPmPersonalDataTable = PmPersonalDataAccess.LoadViaPtDriverStatus(ACode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonalDataTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonalDataCascading.DeleteUsingTemplate(MyPmPersonalDataTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtDriverStatusAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtDriverStatusRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonalDataTable MyPmPersonalDataTable = PmPersonalDataAccess.LoadViaPtDriverStatusTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonalDataTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonalDataCascading.DeleteUsingTemplate(MyPmPersonalDataTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtDriverStatusAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersonalDataCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonalDataAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersonalDataRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonalDataAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersOfficeSpecificDataCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersOfficeSpecificDataAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersOfficeSpecificDataRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersOfficeSpecificDataAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PDataLabelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PDataLabelUseTable MyPDataLabelUseTable = PDataLabelUseAccess.LoadViaPDataLabel(AKey, StringHelper.StrSplit("p_data_label_key_i,p_use_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelUseTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelUseCascading.DeleteUsingTemplate(MyPDataLabelUseTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PDataLabelValuePartnerTable MyPDataLabelValuePartnerTable = PDataLabelValuePartnerAccess.LoadViaPDataLabel(AKey, StringHelper.StrSplit("p_partner_key_n,p_data_label_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelValuePartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelValuePartnerCascading.DeleteUsingTemplate(MyPDataLabelValuePartnerTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PDataLabelValueApplicationTable MyPDataLabelValueApplicationTable = PDataLabelValueApplicationAccess.LoadViaPDataLabel(AKey, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n,p_data_label_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelValueApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelValueApplicationCascading.DeleteUsingTemplate(MyPDataLabelValueApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupDataLabelTable MySGroupDataLabelTable = SGroupDataLabelAccess.LoadViaPDataLabel(AKey, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_data_label_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupDataLabelTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupDataLabelCascading.DeleteUsingTemplate(MySGroupDataLabelTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PDataLabelAccess.DeleteByPrimaryKey(AKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PDataLabelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PDataLabelUseTable MyPDataLabelUseTable = PDataLabelUseAccess.LoadViaPDataLabelTemplate(ATemplateRow, StringHelper.StrSplit("p_data_label_key_i,p_use_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelUseTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelUseCascading.DeleteUsingTemplate(MyPDataLabelUseTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PDataLabelValuePartnerTable MyPDataLabelValuePartnerTable = PDataLabelValuePartnerAccess.LoadViaPDataLabelTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_data_label_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelValuePartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelValuePartnerCascading.DeleteUsingTemplate(MyPDataLabelValuePartnerTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PDataLabelValueApplicationTable MyPDataLabelValueApplicationTable = PDataLabelValueApplicationAccess.LoadViaPDataLabelTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n,p_data_label_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelValueApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelValueApplicationCascading.DeleteUsingTemplate(MyPDataLabelValueApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupDataLabelTable MySGroupDataLabelTable = SGroupDataLabelAccess.LoadViaPDataLabelTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_data_label_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupDataLabelTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupDataLabelCascading.DeleteUsingTemplate(MySGroupDataLabelTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PDataLabelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PDataLabelUseCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ADataLabelKey, String AUse, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PDataLabelUseAccess.DeleteByPrimaryKey(ADataLabelKey, AUse, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PDataLabelUseRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PDataLabelUseAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PDataLabelValuePartnerCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 ADataLabelKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PDataLabelValuePartnerAccess.DeleteByPrimaryKey(APartnerKey, ADataLabelKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PDataLabelValuePartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PDataLabelValuePartnerAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PDataLabelValueApplicationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AApplicationKey, Int64 ARegistrationOffice, Int32 ADataLabelKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PDataLabelValueApplicationAccess.DeleteByPrimaryKey(APartnerKey, AApplicationKey, ARegistrationOffice, ADataLabelKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PDataLabelValueApplicationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PDataLabelValueApplicationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PDataLabelLookupCategoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACategoryCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PDataLabelTable MyPDataLabelTable = PDataLabelAccess.LoadViaPDataLabelLookupCategory(ACategoryCode, StringHelper.StrSplit("p_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelCascading.DeleteUsingTemplate(MyPDataLabelTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PDataLabelLookupTable MyPDataLabelLookupTable = PDataLabelLookupAccess.LoadViaPDataLabelLookupCategory(ACategoryCode, StringHelper.StrSplit("p_category_code_c,p_value_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelLookupTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelLookupCascading.DeleteUsingTemplate(MyPDataLabelLookupTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PDataLabelLookupCategoryAccess.DeleteByPrimaryKey(ACategoryCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PDataLabelLookupCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PDataLabelTable MyPDataLabelTable = PDataLabelAccess.LoadViaPDataLabelLookupCategoryTemplate(ATemplateRow, StringHelper.StrSplit("p_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelCascading.DeleteUsingTemplate(MyPDataLabelTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PDataLabelLookupTable MyPDataLabelLookupTable = PDataLabelLookupAccess.LoadViaPDataLabelLookupCategoryTemplate(ATemplateRow, StringHelper.StrSplit("p_category_code_c,p_value_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPDataLabelLookupTable.Rows.Count); countRow = (countRow + 1))
                {
                    PDataLabelLookupCascading.DeleteUsingTemplate(MyPDataLabelLookupTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PDataLabelLookupCategoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PDataLabelLookupCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACategoryCode, String AValueCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PDataLabelLookupAccess.DeleteByPrimaryKey(ACategoryCode, AValueCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PDataLabelLookupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PDataLabelLookupAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmInterviewCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, System.DateTime AInterviewDate, String AInterviewer, String AInterviewedFor, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmInterviewAccess.DeleteByPrimaryKey(APartnerKey, AInterviewDate, AInterviewer, AInterviewedFor, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmInterviewRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmInterviewAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersonEvaluationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, System.DateTime AEvaluationDate, String AEvaluator, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonEvaluationAccess.DeleteByPrimaryKey(APartnerKey, AEvaluationDate, AEvaluator, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersonEvaluationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonEvaluationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtVisionAreaCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AVisionAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonVisionTable MyPmPersonVisionTable = PmPersonVisionAccess.LoadViaPtVisionArea(AVisionAreaName, StringHelper.StrSplit("p_partner_key_n,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonVisionCascading.DeleteUsingTemplate(MyPmPersonVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobVisionTable MyUmJobVisionTable = UmJobVisionAccess.LoadViaPtVisionArea(AVisionAreaName, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobVisionCascading.DeleteUsingTemplate(MyUmJobVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitVisionTable MyUmUnitVisionTable = UmUnitVisionAccess.LoadViaPtVisionArea(AVisionAreaName, StringHelper.StrSplit("p_partner_key_n,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitVisionCascading.DeleteUsingTemplate(MyUmUnitVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtVisionAreaAccess.DeleteByPrimaryKey(AVisionAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonVisionTable MyPmPersonVisionTable = PmPersonVisionAccess.LoadViaPtVisionAreaTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonVisionCascading.DeleteUsingTemplate(MyPmPersonVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobVisionTable MyUmJobVisionTable = UmJobVisionAccess.LoadViaPtVisionAreaTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobVisionCascading.DeleteUsingTemplate(MyUmJobVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitVisionTable MyUmUnitVisionTable = UmUnitVisionAccess.LoadViaPtVisionAreaTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitVisionCascading.DeleteUsingTemplate(MyUmUnitVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtVisionAreaAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtVisionLevelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AVisionLevel, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonVisionTable MyPmPersonVisionTable = PmPersonVisionAccess.LoadViaPtVisionLevel(AVisionLevel, StringHelper.StrSplit("p_partner_key_n,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonVisionCascading.DeleteUsingTemplate(MyPmPersonVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobVisionTable MyUmJobVisionTable = UmJobVisionAccess.LoadViaPtVisionLevel(AVisionLevel, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobVisionCascading.DeleteUsingTemplate(MyUmJobVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitVisionTable MyUmUnitVisionTable = UmUnitVisionAccess.LoadViaPtVisionLevel(AVisionLevel, StringHelper.StrSplit("p_partner_key_n,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitVisionCascading.DeleteUsingTemplate(MyUmUnitVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtVisionLevelAccess.DeleteByPrimaryKey(AVisionLevel, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmPersonVisionTable MyPmPersonVisionTable = PmPersonVisionAccess.LoadViaPtVisionLevelTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonVisionCascading.DeleteUsingTemplate(MyPmPersonVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobVisionTable MyUmJobVisionTable = UmJobVisionAccess.LoadViaPtVisionLevelTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobVisionCascading.DeleteUsingTemplate(MyUmJobVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmUnitVisionTable MyUmUnitVisionTable = UmUnitVisionAccess.LoadViaPtVisionLevelTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmUnitVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmUnitVisionCascading.DeleteUsingTemplate(MyUmUnitVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtVisionLevelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersonVisionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AVisionAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonVisionAccess.DeleteByPrimaryKey(APartnerKey, AVisionAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersonVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonVisionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmSpecialNeedCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmSpecialNeedAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmSpecialNeedRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmSpecialNeedAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmStaffDataCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 ASiteKey, Int64 AKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerFieldOfServiceTable MyPPartnerFieldOfServiceTable = PPartnerFieldOfServiceAccess.LoadViaPmStaffData(ASiteKey, AKey, StringHelper.StrSplit("p_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerFieldOfServiceTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerFieldOfServiceCascading.DeleteUsingTemplate(MyPPartnerFieldOfServiceTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmStaffDataAccess.DeleteByPrimaryKey(ASiteKey, AKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmStaffDataRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerFieldOfServiceTable MyPPartnerFieldOfServiceTable = PPartnerFieldOfServiceAccess.LoadViaPmStaffDataTemplate(ATemplateRow, StringHelper.StrSplit("p_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerFieldOfServiceTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerFieldOfServiceCascading.DeleteUsingTemplate(MyPPartnerFieldOfServiceTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmStaffDataAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmCommitmentStatusCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmStaffDataTable MyPmStaffDataTable = PmStaffDataAccess.LoadViaPmCommitmentStatus(ACode, StringHelper.StrSplit("p_site_key_n,pm_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmStaffDataTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmStaffDataCascading.DeleteUsingTemplate(MyPmStaffDataTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmPersonCommitmentStatusTable MyPmPersonCommitmentStatusTable = PmPersonCommitmentStatusAccess.LoadViaPmCommitmentStatus(ACode, StringHelper.StrSplit("p_partner_key_n,pm_status_code_c,pm_status_since_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonCommitmentStatusTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonCommitmentStatusCascading.DeleteUsingTemplate(MyPmPersonCommitmentStatusTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmCommitmentStatusAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmCommitmentStatusRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmStaffDataTable MyPmStaffDataTable = PmStaffDataAccess.LoadViaPmCommitmentStatusTemplate(ATemplateRow, StringHelper.StrSplit("p_site_key_n,pm_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmStaffDataTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmStaffDataCascading.DeleteUsingTemplate(MyPmStaffDataTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmPersonCommitmentStatusTable MyPmPersonCommitmentStatusTable = PmPersonCommitmentStatusAccess.LoadViaPmCommitmentStatusTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_status_code_c,pm_status_since_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonCommitmentStatusTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonCommitmentStatusCascading.DeleteUsingTemplate(MyPmPersonCommitmentStatusTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PmCommitmentStatusAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersonCommitmentStatusCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AStatusCode, System.DateTime AStatusSince, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonCommitmentStatusAccess.DeleteByPrimaryKey(APartnerKey, AStatusCode, AStatusSince, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersonCommitmentStatusRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonCommitmentStatusAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtReportsCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AReportName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PtReportsAccess.DeleteByPrimaryKey(AReportName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtReportsRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PtReportsAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtPositionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APositionName, String APositionScope, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmYearProgramApplicationTable MyPmYearProgramApplicationTable = PmYearProgramApplicationAccess.LoadViaPtPosition(APositionName, APositionScope, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmYearProgramApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmYearProgramApplicationCascading.DeleteUsingTemplate(MyPmYearProgramApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobTable MyUmJobTable = UmJobAccess.LoadViaPtPosition(APositionName, APositionScope, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobCascading.DeleteUsingTemplate(MyUmJobTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtPositionAccess.DeleteByPrimaryKey(APositionName, APositionScope, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtPositionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmYearProgramApplicationTable MyPmYearProgramApplicationTable = PmYearProgramApplicationAccess.LoadViaPtPositionTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_application_key_i,pm_registration_office_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmYearProgramApplicationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmYearProgramApplicationCascading.DeleteUsingTemplate(MyPmYearProgramApplicationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobTable MyUmJobTable = UmJobAccess.LoadViaPtPositionTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobCascading.DeleteUsingTemplate(MyUmJobTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtPositionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmJobCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                UmJobRequirementTable MyUmJobRequirementTable = UmJobRequirementAccess.LoadViaUmJob(AUnitKey, APositionName, APositionScope, AJobKey, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobRequirementTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobRequirementCascading.DeleteUsingTemplate(MyUmJobRequirementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobLanguageTable MyUmJobLanguageTable = UmJobLanguageAccess.LoadViaUmJob(AUnitKey, APositionName, APositionScope, AJobKey, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,p_language_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobLanguageTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobLanguageCascading.DeleteUsingTemplate(MyUmJobLanguageTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobQualificationTable MyUmJobQualificationTable = UmJobQualificationAccess.LoadViaUmJob(AUnitKey, APositionName, APositionScope, AJobKey, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobQualificationCascading.DeleteUsingTemplate(MyUmJobQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobVisionTable MyUmJobVisionTable = UmJobVisionAccess.LoadViaUmJob(AUnitKey, APositionName, APositionScope, AJobKey, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobVisionCascading.DeleteUsingTemplate(MyUmJobVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmJobAssignmentTable MyPmJobAssignmentTable = PmJobAssignmentAccess.LoadViaUmJob(AUnitKey, APositionName, APositionScope, AJobKey, StringHelper.StrSplit("p_partner_key_n,pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pm_job_assignment_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmJobAssignmentTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmJobAssignmentCascading.DeleteUsingTemplate(MyPmJobAssignmentTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SJobGroupTable MySJobGroupTable = SJobGroupAccess.LoadViaUmJob(AUnitKey, APositionName, APositionScope, AJobKey, StringHelper.StrSplit("pt_position_name_c,pt_position_scope_c,um_job_key_i,s_group_id_c,s_unit_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySJobGroupTable.Rows.Count); countRow = (countRow + 1))
                {
                    SJobGroupCascading.DeleteUsingTemplate(MySJobGroupTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            UmJobAccess.DeleteByPrimaryKey(AUnitKey, APositionName, APositionScope, AJobKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                UmJobRequirementTable MyUmJobRequirementTable = UmJobRequirementAccess.LoadViaUmJobTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_ability_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobRequirementTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobRequirementCascading.DeleteUsingTemplate(MyUmJobRequirementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobLanguageTable MyUmJobLanguageTable = UmJobLanguageAccess.LoadViaUmJobTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,p_language_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobLanguageTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobLanguageCascading.DeleteUsingTemplate(MyUmJobLanguageTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobQualificationTable MyUmJobQualificationTable = UmJobQualificationAccess.LoadViaUmJobTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_qualification_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobQualificationTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobQualificationCascading.DeleteUsingTemplate(MyUmJobQualificationTable[countRow], null, ATransaction, AWithCascDelete);
                }
                UmJobVisionTable MyUmJobVisionTable = UmJobVisionAccess.LoadViaUmJobTemplate(ATemplateRow, StringHelper.StrSplit("pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pt_vision_area_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyUmJobVisionTable.Rows.Count); countRow = (countRow + 1))
                {
                    UmJobVisionCascading.DeleteUsingTemplate(MyUmJobVisionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmJobAssignmentTable MyPmJobAssignmentTable = PmJobAssignmentAccess.LoadViaUmJobTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pm_job_assignment_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmJobAssignmentTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmJobAssignmentCascading.DeleteUsingTemplate(MyPmJobAssignmentTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SJobGroupTable MySJobGroupTable = SJobGroupAccess.LoadViaUmJobTemplate(ATemplateRow, StringHelper.StrSplit("pt_position_name_c,pt_position_scope_c,um_job_key_i,s_group_id_c,s_unit_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySJobGroupTable.Rows.Count); countRow = (countRow + 1))
                {
                    SJobGroupCascading.DeleteUsingTemplate(MySJobGroupTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            UmJobAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmJobRequirementCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmJobRequirementAccess.DeleteByPrimaryKey(AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmJobRequirementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmJobRequirementAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmJobLanguageCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmJobLanguageAccess.DeleteByPrimaryKey(AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmJobLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmJobLanguageAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmJobQualificationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmJobQualificationAccess.DeleteByPrimaryKey(AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmJobQualificationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmJobQualificationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmJobVisionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmJobVisionAccess.DeleteByPrimaryKey(AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmJobVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmJobVisionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtAssignmentTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AAssignmentTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmJobAssignmentTable MyPmJobAssignmentTable = PmJobAssignmentAccess.LoadViaPtAssignmentType(AAssignmentTypeCode, StringHelper.StrSplit("p_partner_key_n,pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pm_job_assignment_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmJobAssignmentTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmJobAssignmentCascading.DeleteUsingTemplate(MyPmJobAssignmentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtAssignmentTypeAccess.DeleteByPrimaryKey(AAssignmentTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtAssignmentTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmJobAssignmentTable MyPmJobAssignmentTable = PmJobAssignmentAccess.LoadViaPtAssignmentTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pm_job_assignment_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmJobAssignmentTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmJobAssignmentCascading.DeleteUsingTemplate(MyPmJobAssignmentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtAssignmentTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PtLeavingCodeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ALeavingCodeInd, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmJobAssignmentTable MyPmJobAssignmentTable = PmJobAssignmentAccess.LoadViaPtLeavingCode(ALeavingCodeInd, StringHelper.StrSplit("p_partner_key_n,pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pm_job_assignment_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmJobAssignmentTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmJobAssignmentCascading.DeleteUsingTemplate(MyPmJobAssignmentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtLeavingCodeAccess.DeleteByPrimaryKey(ALeavingCodeInd, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PtLeavingCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PmJobAssignmentTable MyPmJobAssignmentTable = PmJobAssignmentAccess.LoadViaPtLeavingCodeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,pm_unit_key_n,pt_position_name_c,pt_position_scope_c,um_job_key_i,pm_job_assignment_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmJobAssignmentTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmJobAssignmentCascading.DeleteUsingTemplate(MyPmJobAssignmentTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PtLeavingCodeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmJobAssignmentCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, Int32 AJobAssignmentKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmJobAssignmentAccess.DeleteByPrimaryKey(APartnerKey, AUnitKey, APositionName, APositionScope, AJobKey, AJobAssignmentKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmJobAssignmentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmJobAssignmentAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmUnitAbilityCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AAbilityAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitAbilityAccess.DeleteByPrimaryKey(APartnerKey, AAbilityAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmUnitAbilityRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitAbilityAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmUnitLanguageCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitLanguageAccess.DeleteByPrimaryKey(APartnerKey, ALanguageCode, ALanguageLevel, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmUnitLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitLanguageAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmUnitVisionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AVisionAreaName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitVisionAccess.DeleteByPrimaryKey(APartnerKey, AVisionAreaName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmUnitVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitVisionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmUnitCostCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, System.DateTime AValidFromDate, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitCostAccess.DeleteByPrimaryKey(APartnerKey, AValidFromDate, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmUnitCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitCostAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class UmUnitEvaluationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitEvaluationAccess.DeleteByPrimaryKey(APartnerKey, ADateOfEvaluation, AEvaluationNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(UmUnitEvaluationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            UmUnitEvaluationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcConferenceCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcConferenceOptionTable MyPcConferenceOptionTable = PcConferenceOptionAccess.LoadViaPcConference(AConferenceKey, StringHelper.StrSplit("pc_conference_key_n,pc_option_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceOptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceOptionCascading.DeleteUsingTemplate(MyPcConferenceOptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcDiscountTable MyPcDiscountTable = PcDiscountAccess.LoadViaPcConference(AConferenceKey, StringHelper.StrSplit("pc_conference_key_n,pc_discount_criteria_code_c,pc_cost_type_code_c,pc_validity_c,pc_up_to_age_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcDiscountCascading.DeleteUsingTemplate(MyPcDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcAttendeeTable MyPcAttendeeTable = PcAttendeeAccess.LoadViaPcConference(AConferenceKey, StringHelper.StrSplit("pc_conference_key_n,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcAttendeeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcAttendeeCascading.DeleteUsingTemplate(MyPcAttendeeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcConferenceCostTable MyPcConferenceCostTable = PcConferenceCostAccess.LoadViaPcConference(AConferenceKey, StringHelper.StrSplit("pc_conference_key_n,pc_option_days_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceCostTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceCostCascading.DeleteUsingTemplate(MyPcConferenceCostTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcEarlyLateTable MyPcEarlyLateTable = PcEarlyLateAccess.LoadViaPcConference(AConferenceKey, StringHelper.StrSplit("pc_conference_key_n,pc_applicable_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcEarlyLateTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcEarlyLateCascading.DeleteUsingTemplate(MyPcEarlyLateTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcSupplementTable MyPcSupplementTable = PcSupplementAccess.LoadViaPcConference(AConferenceKey, StringHelper.StrSplit("pc_conference_key_n,pc_xyz_tbd_type_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcSupplementTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcSupplementCascading.DeleteUsingTemplate(MyPcSupplementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcConferenceVenueTable MyPcConferenceVenueTable = PcConferenceVenueAccess.LoadViaPcConference(AConferenceKey, StringHelper.StrSplit("pc_conference_key_n,p_venue_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceVenueTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceVenueCascading.DeleteUsingTemplate(MyPcConferenceVenueTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcConferenceAccess.DeleteByPrimaryKey(AConferenceKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcConferenceOptionTable MyPcConferenceOptionTable = PcConferenceOptionAccess.LoadViaPcConferenceTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,pc_option_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceOptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceOptionCascading.DeleteUsingTemplate(MyPcConferenceOptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcDiscountTable MyPcDiscountTable = PcDiscountAccess.LoadViaPcConferenceTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,pc_discount_criteria_code_c,pc_cost_type_code_c,pc_validity_c,pc_up_to_age_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcDiscountCascading.DeleteUsingTemplate(MyPcDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcAttendeeTable MyPcAttendeeTable = PcAttendeeAccess.LoadViaPcConferenceTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcAttendeeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcAttendeeCascading.DeleteUsingTemplate(MyPcAttendeeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcConferenceCostTable MyPcConferenceCostTable = PcConferenceCostAccess.LoadViaPcConferenceTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,pc_option_days_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceCostTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceCostCascading.DeleteUsingTemplate(MyPcConferenceCostTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcEarlyLateTable MyPcEarlyLateTable = PcEarlyLateAccess.LoadViaPcConferenceTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,pc_applicable_d", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcEarlyLateTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcEarlyLateCascading.DeleteUsingTemplate(MyPcEarlyLateTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcSupplementTable MyPcSupplementTable = PcSupplementAccess.LoadViaPcConferenceTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,pc_xyz_tbd_type_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcSupplementTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcSupplementCascading.DeleteUsingTemplate(MyPcSupplementTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcConferenceVenueTable MyPcConferenceVenueTable = PcConferenceVenueAccess.LoadViaPcConferenceTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,p_venue_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceVenueTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceVenueCascading.DeleteUsingTemplate(MyPcConferenceVenueTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcConferenceAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcCostTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACostTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcDiscountTable MyPcDiscountTable = PcDiscountAccess.LoadViaPcCostType(ACostTypeCode, StringHelper.StrSplit("pc_conference_key_n,pc_discount_criteria_code_c,pc_cost_type_code_c,pc_validity_c,pc_up_to_age_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcDiscountCascading.DeleteUsingTemplate(MyPcDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcExtraCostTable MyPcExtraCostTable = PcExtraCostAccess.LoadViaPcCostType(ACostTypeCode, StringHelper.StrSplit("pc_conference_key_n,p_partner_key_n,pc_extra_cost_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcExtraCostTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcExtraCostCascading.DeleteUsingTemplate(MyPcExtraCostTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcCostTypeAccess.DeleteByPrimaryKey(ACostTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcDiscountTable MyPcDiscountTable = PcDiscountAccess.LoadViaPcCostTypeTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,pc_discount_criteria_code_c,pc_cost_type_code_c,pc_validity_c,pc_up_to_age_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcDiscountCascading.DeleteUsingTemplate(MyPcDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcExtraCostTable MyPcExtraCostTable = PcExtraCostAccess.LoadViaPcCostTypeTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,p_partner_key_n,pc_extra_cost_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcExtraCostTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcExtraCostCascading.DeleteUsingTemplate(MyPcExtraCostTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcCostTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcConferenceOptionTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AOptionTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcConferenceOptionTable MyPcConferenceOptionTable = PcConferenceOptionAccess.LoadViaPcConferenceOptionType(AOptionTypeCode, StringHelper.StrSplit("pc_conference_key_n,pc_option_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceOptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceOptionCascading.DeleteUsingTemplate(MyPcConferenceOptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcConferenceOptionTypeAccess.DeleteByPrimaryKey(AOptionTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcConferenceOptionTable MyPcConferenceOptionTable = PcConferenceOptionAccess.LoadViaPcConferenceOptionTypeTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,pc_option_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcConferenceOptionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcConferenceOptionCascading.DeleteUsingTemplate(MyPcConferenceOptionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcConferenceOptionTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcConferenceOptionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcConferenceOptionAccess.DeleteByPrimaryKey(AConferenceKey, AOptionTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcConferenceOptionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcDiscountCriteriaCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ADiscountCriteriaCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcDiscountTable MyPcDiscountTable = PcDiscountAccess.LoadViaPcDiscountCriteria(ADiscountCriteriaCode, StringHelper.StrSplit("pc_conference_key_n,pc_discount_criteria_code_c,pc_cost_type_code_c,pc_validity_c,pc_up_to_age_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcDiscountCascading.DeleteUsingTemplate(MyPcDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcDiscountCriteriaAccess.DeleteByPrimaryKey(ADiscountCriteriaCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcDiscountTable MyPcDiscountTable = PcDiscountAccess.LoadViaPcDiscountCriteriaTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,pc_discount_criteria_code_c,pc_cost_type_code_c,pc_validity_c,pc_up_to_age_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcDiscountTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcDiscountCascading.DeleteUsingTemplate(MyPcDiscountTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcDiscountCriteriaAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcDiscountCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcDiscountAccess.DeleteByPrimaryKey(AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcDiscountAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcAttendeeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcExtraCostTable MyPcExtraCostTable = PcExtraCostAccess.LoadViaPcAttendee(AConferenceKey, APartnerKey, StringHelper.StrSplit("pc_conference_key_n,p_partner_key_n,pc_extra_cost_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcExtraCostTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcExtraCostCascading.DeleteUsingTemplate(MyPcExtraCostTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcGroupTable MyPcGroupTable = PcGroupAccess.LoadViaPcAttendee(AConferenceKey, APartnerKey, StringHelper.StrSplit("pc_conference_key_n,p_partner_key_n,pc_group_type_c,pc_group_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcGroupTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcGroupCascading.DeleteUsingTemplate(MyPcGroupTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcRoomAllocTable MyPcRoomAllocTable = PcRoomAllocAccess.LoadViaPcAttendee(AConferenceKey, APartnerKey, StringHelper.StrSplit("pc_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomAllocTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomAllocCascading.DeleteUsingTemplate(MyPcRoomAllocTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcAttendeeAccess.DeleteByPrimaryKey(AConferenceKey, APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcExtraCostTable MyPcExtraCostTable = PcExtraCostAccess.LoadViaPcAttendeeTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,p_partner_key_n,pc_extra_cost_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcExtraCostTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcExtraCostCascading.DeleteUsingTemplate(MyPcExtraCostTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcGroupTable MyPcGroupTable = PcGroupAccess.LoadViaPcAttendeeTemplate(ATemplateRow, StringHelper.StrSplit("pc_conference_key_n,p_partner_key_n,pc_group_type_c,pc_group_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcGroupTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcGroupCascading.DeleteUsingTemplate(MyPcGroupTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcRoomAllocTable MyPcRoomAllocTable = PcRoomAllocAccess.LoadViaPcAttendeeTemplate(ATemplateRow, StringHelper.StrSplit("pc_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomAllocTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomAllocCascading.DeleteUsingTemplate(MyPcRoomAllocTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcAttendeeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcConferenceCostCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcConferenceCostAccess.DeleteByPrimaryKey(AConferenceKey, AOptionDays, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcConferenceCostAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcExtraCostCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcExtraCostAccess.DeleteByPrimaryKey(AConferenceKey, APartnerKey, AExtraCostKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcExtraCostAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcEarlyLateCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcEarlyLateAccess.DeleteByPrimaryKey(AConferenceKey, AApplicable, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcEarlyLateAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcGroupCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcGroupAccess.DeleteByPrimaryKey(AConferenceKey, APartnerKey, AGroupType, AGroupName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcGroupAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcSupplementCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcSupplementAccess.DeleteByPrimaryKey(AConferenceKey, AXyzTbdType, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcSupplementAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcBuildingCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcRoomTable MyPcRoomTable = PcRoomAccess.LoadViaPcBuilding(AVenueKey, ABuildingCode, StringHelper.StrSplit("p_venue_key_n,pc_building_code_c,pc_room_number_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomCascading.DeleteUsingTemplate(MyPcRoomTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcBuildingAccess.DeleteByPrimaryKey(AVenueKey, ABuildingCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcRoomTable MyPcRoomTable = PcRoomAccess.LoadViaPcBuildingTemplate(ATemplateRow, StringHelper.StrSplit("p_venue_key_n,pc_building_code_c,pc_room_number_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomCascading.DeleteUsingTemplate(MyPcRoomTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcBuildingAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcRoomCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcRoomAllocTable MyPcRoomAllocTable = PcRoomAllocAccess.LoadViaPcRoom(AVenueKey, ABuildingCode, ARoomNumber, StringHelper.StrSplit("pc_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomAllocTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomAllocCascading.DeleteUsingTemplate(MyPcRoomAllocTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcRoomAttributeTable MyPcRoomAttributeTable = PcRoomAttributeAccess.LoadViaPcRoom(AVenueKey, ABuildingCode, ARoomNumber, StringHelper.StrSplit("p_venue_key_n,pc_building_code_c,pc_room_number_c,pc_room_attr_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomAttributeCascading.DeleteUsingTemplate(MyPcRoomAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcRoomAccess.DeleteByPrimaryKey(AVenueKey, ABuildingCode, ARoomNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcRoomAllocTable MyPcRoomAllocTable = PcRoomAllocAccess.LoadViaPcRoomTemplate(ATemplateRow, StringHelper.StrSplit("pc_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomAllocTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomAllocCascading.DeleteUsingTemplate(MyPcRoomAllocTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PcRoomAttributeTable MyPcRoomAttributeTable = PcRoomAttributeAccess.LoadViaPcRoomTemplate(ATemplateRow, StringHelper.StrSplit("p_venue_key_n,pc_building_code_c,pc_room_number_c,pc_room_attr_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomAttributeCascading.DeleteUsingTemplate(MyPcRoomAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcRoomAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcRoomAllocCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PhRoomBookingTable MyPhRoomBookingTable = PhRoomBookingAccess.LoadViaPcRoomAlloc(AKey, StringHelper.StrSplit("ph_booking_key_i,ph_room_alloc_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPhRoomBookingTable.Rows.Count); countRow = (countRow + 1))
                {
                    PhRoomBookingCascading.DeleteUsingTemplate(MyPhRoomBookingTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcRoomAllocAccess.DeleteByPrimaryKey(AKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PhRoomBookingTable MyPhRoomBookingTable = PhRoomBookingAccess.LoadViaPcRoomAllocTemplate(ATemplateRow, StringHelper.StrSplit("ph_booking_key_i,ph_room_alloc_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPhRoomBookingTable.Rows.Count); countRow = (countRow + 1))
                {
                    PhRoomBookingCascading.DeleteUsingTemplate(MyPhRoomBookingTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcRoomAllocAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcRoomAttributeTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcRoomAttributeTable MyPcRoomAttributeTable = PcRoomAttributeAccess.LoadViaPcRoomAttributeType(ACode, StringHelper.StrSplit("p_venue_key_n,pc_building_code_c,pc_room_number_c,pc_room_attr_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomAttributeCascading.DeleteUsingTemplate(MyPcRoomAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcRoomAttributeTypeAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PcRoomAttributeTable MyPcRoomAttributeTable = PcRoomAttributeAccess.LoadViaPcRoomAttributeTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_venue_key_n,pc_building_code_c,pc_room_number_c,pc_room_attr_type_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPcRoomAttributeTable.Rows.Count); countRow = (countRow + 1))
                {
                    PcRoomAttributeCascading.DeleteUsingTemplate(MyPcRoomAttributeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PcRoomAttributeTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcRoomAttributeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcRoomAttributeAccess.DeleteByPrimaryKey(AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcRoomAttributeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PcConferenceVenueCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcConferenceVenueAccess.DeleteByPrimaryKey(AConferenceKey, AVenueKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PcConferenceVenueAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PhRoomBookingCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 ABookingKey, Int32 ARoomAllocKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PhRoomBookingAccess.DeleteByPrimaryKey(ABookingKey, ARoomAllocKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PhRoomBookingAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PhBookingCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PhRoomBookingTable MyPhRoomBookingTable = PhRoomBookingAccess.LoadViaPhBooking(AKey, StringHelper.StrSplit("ph_booking_key_i,ph_room_alloc_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPhRoomBookingTable.Rows.Count); countRow = (countRow + 1))
                {
                    PhRoomBookingCascading.DeleteUsingTemplate(MyPhRoomBookingTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PhBookingAccess.DeleteByPrimaryKey(AKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PhRoomBookingTable MyPhRoomBookingTable = PhRoomBookingAccess.LoadViaPhBookingTemplate(ATemplateRow, StringHelper.StrSplit("ph_booking_key_i,ph_room_alloc_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPhRoomBookingTable.Rows.Count); countRow = (countRow + 1))
                {
                    PhRoomBookingCascading.DeleteUsingTemplate(MyPhRoomBookingTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PhBookingAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PTaxCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String ATaxType, String ATaxRef, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PTaxAccess.DeleteByPrimaryKey(APartnerKey, ATaxType, ATaxRef, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PTaxRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PTaxAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PInterestCategoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACategory, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PInterestTable MyPInterestTable = PInterestAccess.LoadViaPInterestCategory(ACategory, StringHelper.StrSplit("p_interest_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPInterestTable.Rows.Count); countRow = (countRow + 1))
                {
                    PInterestCascading.DeleteUsingTemplate(MyPInterestTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PInterestCategoryAccess.DeleteByPrimaryKey(ACategory, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PInterestCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PInterestTable MyPInterestTable = PInterestAccess.LoadViaPInterestCategoryTemplate(ATemplateRow, StringHelper.StrSplit("p_interest_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPInterestTable.Rows.Count); countRow = (countRow + 1))
                {
                    PInterestCascading.DeleteUsingTemplate(MyPInterestTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PInterestCategoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PInterestCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AInterest, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerInterestTable MyPPartnerInterestTable = PPartnerInterestAccess.LoadViaPInterest(AInterest, StringHelper.StrSplit("p_partner_key_n,p_interest_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerInterestTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerInterestCascading.DeleteUsingTemplate(MyPPartnerInterestTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PInterestAccess.DeleteByPrimaryKey(AInterest, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PInterestRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerInterestTable MyPPartnerInterestTable = PPartnerInterestAccess.LoadViaPInterestTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_interest_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerInterestTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerInterestCascading.DeleteUsingTemplate(MyPPartnerInterestTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PInterestAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerInterestCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AInterestNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerInterestAccess.DeleteByPrimaryKey(APartnerKey, AInterestNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerInterestRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerInterestAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerMergeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AMergeFrom, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerMergeAccess.DeleteByPrimaryKey(AMergeFrom, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerMergeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerMergeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PReminderCategoryCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerReminderTable MyPPartnerReminderTable = PPartnerReminderAccess.LoadViaPReminderCategory(ACode, StringHelper.StrSplit("p_partner_key_n,p_contact_id_i,p_reminder_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerReminderTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerReminderCascading.DeleteUsingTemplate(MyPPartnerReminderTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PReminderCategoryAccess.DeleteByPrimaryKey(ACode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PReminderCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerReminderTable MyPPartnerReminderTable = PPartnerReminderAccess.LoadViaPReminderCategoryTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_contact_id_i,p_reminder_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerReminderTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerReminderCascading.DeleteUsingTemplate(MyPPartnerReminderTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PReminderCategoryAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerReminderCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AContactId, Int32 AReminderId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerActionTable MyPPartnerActionTable = PPartnerActionAccess.LoadViaPPartnerReminder(APartnerKey, AContactId, AReminderId, StringHelper.StrSplit("p_partner_key_n,p_action_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerActionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerActionCascading.DeleteUsingTemplate(MyPPartnerActionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupPartnerReminderTable MySGroupPartnerReminderTable = SGroupPartnerReminderAccess.LoadViaPPartnerReminder(APartnerKey, AContactId, AReminderId, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_partner_key_n,p_contact_id_i,p_reminder_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupPartnerReminderTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupPartnerReminderCascading.DeleteUsingTemplate(MySGroupPartnerReminderTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerReminderAccess.DeleteByPrimaryKey(APartnerKey, AContactId, AReminderId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerReminderRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerActionTable MyPPartnerActionTable = PPartnerActionAccess.LoadViaPPartnerReminderTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_action_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerActionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerActionCascading.DeleteUsingTemplate(MyPPartnerActionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupPartnerReminderTable MySGroupPartnerReminderTable = SGroupPartnerReminderAccess.LoadViaPPartnerReminderTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_partner_key_n,p_contact_id_i,p_reminder_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupPartnerReminderTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupPartnerReminderCascading.DeleteUsingTemplate(MySGroupPartnerReminderTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerReminderAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerFieldOfServiceCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerFieldOfServiceAccess.DeleteByPrimaryKey(AKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerFieldOfServiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerFieldOfServiceAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerShortCodeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String APartnerShortCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerShortCodeAccess.DeleteByPrimaryKey(APartnerKey, APartnerShortCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerShortCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerShortCodeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PProcessCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AProcessCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PStateTable MyPStateTable = PStateAccess.LoadViaPProcess(AProcessCode, StringHelper.StrSplit("p_process_code_c,p_state_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPStateTable.Rows.Count); countRow = (countRow + 1))
                {
                    PStateCascading.DeleteUsingTemplate(MyPStateTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PActionTable MyPActionTable = PActionAccess.LoadViaPProcess(AProcessCode, StringHelper.StrSplit("p_process_code_c,p_action_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPActionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PActionCascading.DeleteUsingTemplate(MyPActionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PProcessAccess.DeleteByPrimaryKey(AProcessCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PProcessRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PStateTable MyPStateTable = PStateAccess.LoadViaPProcessTemplate(ATemplateRow, StringHelper.StrSplit("p_process_code_c,p_state_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPStateTable.Rows.Count); countRow = (countRow + 1))
                {
                    PStateCascading.DeleteUsingTemplate(MyPStateTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PActionTable MyPActionTable = PActionAccess.LoadViaPProcessTemplate(ATemplateRow, StringHelper.StrSplit("p_process_code_c,p_action_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyPActionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PActionCascading.DeleteUsingTemplate(MyPActionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PProcessAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PStateCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AProcessCode, String AStateCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerStateTable MyPPartnerStateTable = PPartnerStateAccess.LoadViaPState(AProcessCode, AStateCode, StringHelper.StrSplit("p_partner_key_n,p_state_index_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerStateTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerStateCascading.DeleteUsingTemplate(MyPPartnerStateTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PStateAccess.DeleteByPrimaryKey(AProcessCode, AStateCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PStateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerStateTable MyPPartnerStateTable = PPartnerStateAccess.LoadViaPStateTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_state_index_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerStateTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerStateCascading.DeleteUsingTemplate(MyPPartnerStateTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PStateAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PActionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AProcessCode, String AActionCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerActionTable MyPPartnerActionTable = PPartnerActionAccess.LoadViaPAction(AProcessCode, AActionCode, StringHelper.StrSplit("p_partner_key_n,p_action_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerActionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerActionCascading.DeleteUsingTemplate(MyPPartnerActionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PActionAccess.DeleteByPrimaryKey(AProcessCode, AActionCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PActionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerActionTable MyPPartnerActionTable = PPartnerActionAccess.LoadViaPActionTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n,p_action_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerActionTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerActionCascading.DeleteUsingTemplate(MyPPartnerActionTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PActionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerStateCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AStateIndex, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerStateAccess.DeleteByPrimaryKey(APartnerKey, AStateIndex, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerStateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerStateAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerActionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AActionNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerActionAccess.DeleteByPrimaryKey(APartnerKey, AActionNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerActionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerActionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFirstContactCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFirstContactCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPFirstContact(AFirstContactCode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
            }
            PFirstContactAccess.DeleteByPrimaryKey(AFirstContactCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFirstContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerTable MyPPartnerTable = PPartnerAccess.LoadViaPFirstContactTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                                    PPartnerAccess.DeleteUsingTemplate(MyPPartnerTable[countRow], null, ATransaction);
                                }
            }
            PFirstContactAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class AKeyFocusAreaCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AKeyFocusArea, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ACostCentreTable MyACostCentreTable = ACostCentreAccess.LoadViaAKeyFocusArea(AKeyFocusArea, StringHelper.StrSplit("a_ledger_number_i,a_cost_centre_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyACostCentreTable.Rows.Count); countRow = (countRow + 1))
                {
                                    ACostCentreAccess.DeleteUsingTemplate(MyACostCentreTable[countRow], null, ATransaction);
                                }
            }
            AKeyFocusAreaAccess.DeleteByPrimaryKey(AKeyFocusArea, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(AKeyFocusAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                ACostCentreTable MyACostCentreTable = ACostCentreAccess.LoadViaAKeyFocusAreaTemplate(ATemplateRow, StringHelper.StrSplit("a_ledger_number_i,a_cost_centre_code_c", ","), ATransaction);
                for (countRow = 0; (countRow != MyACostCentreTable.Rows.Count); countRow = (countRow + 1))
                {
                                    ACostCentreAccess.DeleteUsingTemplate(MyACostCentreTable[countRow], null, ATransaction);
                                }
            }
            AKeyFocusAreaAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupFunctionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AUnitKey, String AFunctionId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupFunctionAccess.DeleteByPrimaryKey(AGroupId, AUnitKey, AFunctionId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupFunctionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupFunctionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SFunctionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFunctionId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SGroupFunctionTable MySGroupFunctionTable = SGroupFunctionAccess.LoadViaSFunction(AFunctionId, StringHelper.StrSplit("s_group_id_c,s_unit_key_n,s_function_id_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupFunctionTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupFunctionCascading.DeleteUsingTemplate(MySGroupFunctionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SWorkflowStepTable MySWorkflowStepTable = SWorkflowStepAccess.LoadViaSFunction(AFunctionId, StringHelper.StrSplit("s_workflow_id_i,s_step_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowStepTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowStepCascading.DeleteUsingTemplate(MySWorkflowStepTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SFunctionRelationshipTable MySFunctionRelationshipFunction1Table = SFunctionRelationshipAccess.LoadViaSFunctionFunction1(AFunctionId, StringHelper.StrSplit("s_function_1_c,s_function_2_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySFunctionRelationshipFunction1Table.Rows.Count); countRow = (countRow + 1))
                {
                    SFunctionRelationshipCascading.DeleteUsingTemplate(MySFunctionRelationshipFunction1Table[countRow], null, ATransaction, AWithCascDelete);
                }
                SFunctionRelationshipTable MySFunctionRelationshipFunction2Table = SFunctionRelationshipAccess.LoadViaSFunctionFunction2(AFunctionId, StringHelper.StrSplit("s_function_1_c,s_function_2_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySFunctionRelationshipFunction2Table.Rows.Count); countRow = (countRow + 1))
                {
                    SFunctionRelationshipCascading.DeleteUsingTemplate(MySFunctionRelationshipFunction2Table[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SFunctionAccess.DeleteByPrimaryKey(AFunctionId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SFunctionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SGroupFunctionTable MySGroupFunctionTable = SGroupFunctionAccess.LoadViaSFunctionTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_unit_key_n,s_function_id_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupFunctionTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupFunctionCascading.DeleteUsingTemplate(MySGroupFunctionTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SWorkflowStepTable MySWorkflowStepTable = SWorkflowStepAccess.LoadViaSFunctionTemplate(ATemplateRow, StringHelper.StrSplit("s_workflow_id_i,s_step_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowStepTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowStepCascading.DeleteUsingTemplate(MySWorkflowStepTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SFunctionRelationshipTable MySFunctionRelationshipFunction1Table = SFunctionRelationshipAccess.LoadViaSFunctionFunction1Template(ATemplateRow, StringHelper.StrSplit("s_function_1_c,s_function_2_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySFunctionRelationshipFunction1Table.Rows.Count); countRow = (countRow + 1))
                {
                    SFunctionRelationshipCascading.DeleteUsingTemplate(MySFunctionRelationshipFunction1Table[countRow], null, ATransaction, AWithCascDelete);
                }
                SFunctionRelationshipTable MySFunctionRelationshipFunction2Table = SFunctionRelationshipAccess.LoadViaSFunctionFunction2Template(ATemplateRow, StringHelper.StrSplit("s_function_1_c,s_function_2_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySFunctionRelationshipFunction2Table.Rows.Count); countRow = (countRow + 1))
                {
                    SFunctionRelationshipCascading.DeleteUsingTemplate(MySFunctionRelationshipFunction2Table[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SFunctionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SJobGroupCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APositionName, String APositionScope, Int32 AJobKey, String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SJobGroupAccess.DeleteByPrimaryKey(APositionName, APositionScope, AJobKey, AGroupId, AUnitKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SJobGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SJobGroupAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupPartnerSetCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, String APartnerSetId, Int64 APartnerSetUnitKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupPartnerSetAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, APartnerSetId, APartnerSetUnitKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupPartnerSetRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupPartnerSetAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerSetCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APartnerSetId, Int64 AUnitKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SGroupPartnerSetTable MySGroupPartnerSetTable = SGroupPartnerSetAccess.LoadViaPPartnerSet(APartnerSetId, AUnitKey, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_partner_set_id_c,p_partner_set_unit_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupPartnerSetTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupPartnerSetCascading.DeleteUsingTemplate(MySGroupPartnerSetTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerSetPartnerTable MyPPartnerSetPartnerTable = PPartnerSetPartnerAccess.LoadViaPPartnerSet(APartnerSetId, AUnitKey, StringHelper.StrSplit("p_partner_set_id_c,p_partner_set_unit_key_n,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerSetPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerSetPartnerCascading.DeleteUsingTemplate(MyPPartnerSetPartnerTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerSetAccess.DeleteByPrimaryKey(APartnerSetId, AUnitKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerSetRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SGroupPartnerSetTable MySGroupPartnerSetTable = SGroupPartnerSetAccess.LoadViaPPartnerSetTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_partner_set_id_c,p_partner_set_unit_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupPartnerSetTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupPartnerSetCascading.DeleteUsingTemplate(MySGroupPartnerSetTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerSetPartnerTable MyPPartnerSetPartnerTable = PPartnerSetPartnerAccess.LoadViaPPartnerSetTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_set_id_c,p_partner_set_unit_key_n,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerSetPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerSetPartnerCascading.DeleteUsingTemplate(MyPPartnerSetPartnerTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PPartnerSetAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerSetPartnerCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String APartnerSetId, Int64 APartnerSetUnitKey, Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerSetPartnerAccess.DeleteByPrimaryKey(APartnerSetId, APartnerSetUnitKey, APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerSetPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerSetPartnerAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupGiftCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupGiftAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupGiftAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupMotivationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupMotivationAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupMotivationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupMotivationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupPartnerContactCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int32 AContactId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupPartnerContactAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, AContactId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupPartnerContactAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupPartnerReminderCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int64 APartnerKey, Int32 AContactId, Int32 AReminderId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupPartnerReminderAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, APartnerKey, AContactId, AReminderId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupPartnerReminderRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupPartnerReminderAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupLocationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int64 ASiteKey, Int32 ALocationKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupLocationAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, ASiteKey, ALocationKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupLocationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupLocationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupPartnerLocationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int64 APartnerKey, Int64 ASiteKey, Int32 ALocationKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupPartnerLocationAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, APartnerKey, ASiteKey, ALocationKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupPartnerLocationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupPartnerLocationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupDataLabelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int32 ADataLabelKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupDataLabelAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, ADataLabelKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupDataLabelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupDataLabelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupLedgerCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int32 ALedgerNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupLedgerAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, ALedgerNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupLedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupLedgerAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupCostCentreCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupCostCentreAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, ALedgerNumber, ACostCentreCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupCostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupCostCentreAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupExtractCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int32 AExtractId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupExtractAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, AExtractId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupExtractRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupExtractAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SChangeEventCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ATableName, String ARowid, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SChangeEventAccess.DeleteByPrimaryKey(ATableName, ARowid, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SChangeEventRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SChangeEventAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SLabelCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ALabelName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SLabelAccess.DeleteByPrimaryKey(ALabelName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SLabelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SLabelAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerCommentCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, Int32 AIndex, Int32 ASequence, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerCommentAccess.DeleteByPrimaryKey(APartnerKey, AIndex, ASequence, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerCommentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerCommentAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PProposalSubmissionTypeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String ASubmissionTypeCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationTable MyPFoundationTable = PFoundationAccess.LoadViaPProposalSubmissionType(ASubmissionTypeCode, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationCascading.DeleteUsingTemplate(MyPFoundationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PProposalSubmissionTypeAccess.DeleteByPrimaryKey(ASubmissionTypeCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PProposalSubmissionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationTable MyPFoundationTable = PFoundationAccess.LoadViaPProposalSubmissionTypeTemplate(ATemplateRow, StringHelper.StrSplit("p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationCascading.DeleteUsingTemplate(MyPFoundationTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PProposalSubmissionTypeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFoundationCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationProposalTable MyPFoundationProposalTable = PFoundationProposalAccess.LoadViaPFoundation(APartnerKey, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalCascading.DeleteUsingTemplate(MyPFoundationProposalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFoundationProposalDetailTable MyPFoundationProposalDetailTable = PFoundationProposalDetailAccess.LoadViaPFoundation(APartnerKey, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i,p_proposal_detail_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalDetailCascading.DeleteUsingTemplate(MyPFoundationProposalDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFoundationDeadlineTable MyPFoundationDeadlineTable = PFoundationDeadlineAccess.LoadViaPFoundation(APartnerKey, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_deadline_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationDeadlineTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationDeadlineCascading.DeleteUsingTemplate(MyPFoundationDeadlineTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFoundationAccess.DeleteByPrimaryKey(APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFoundationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationProposalTable MyPFoundationProposalTable = PFoundationProposalAccess.LoadViaPFoundationTemplate(ATemplateRow, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalCascading.DeleteUsingTemplate(MyPFoundationProposalTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFoundationProposalDetailTable MyPFoundationProposalDetailTable = PFoundationProposalDetailAccess.LoadViaPFoundationTemplate(ATemplateRow, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i,p_proposal_detail_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalDetailCascading.DeleteUsingTemplate(MyPFoundationProposalDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PFoundationDeadlineTable MyPFoundationDeadlineTable = PFoundationDeadlineAccess.LoadViaPFoundationTemplate(ATemplateRow, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_deadline_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationDeadlineTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationDeadlineCascading.DeleteUsingTemplate(MyPFoundationDeadlineTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFoundationAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFoundationProposalStatusCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AStatusCode, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationProposalTable MyPFoundationProposalTable = PFoundationProposalAccess.LoadViaPFoundationProposalStatus(AStatusCode, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalCascading.DeleteUsingTemplate(MyPFoundationProposalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFoundationProposalStatusAccess.DeleteByPrimaryKey(AStatusCode, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFoundationProposalStatusRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationProposalTable MyPFoundationProposalTable = PFoundationProposalAccess.LoadViaPFoundationProposalStatusTemplate(ATemplateRow, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalCascading.DeleteUsingTemplate(MyPFoundationProposalTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFoundationProposalStatusAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFoundationProposalCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFoundationPartnerKey, Int32 AFoundationProposalKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationProposalDetailTable MyPFoundationProposalDetailTable = PFoundationProposalDetailAccess.LoadViaPFoundationProposal(AFoundationPartnerKey, AFoundationProposalKey, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i,p_proposal_detail_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalDetailCascading.DeleteUsingTemplate(MyPFoundationProposalDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFoundationProposalAccess.DeleteByPrimaryKey(AFoundationPartnerKey, AFoundationProposalKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFoundationProposalRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFoundationProposalDetailTable MyPFoundationProposalDetailTable = PFoundationProposalDetailAccess.LoadViaPFoundationProposalTemplate(ATemplateRow, StringHelper.StrSplit("p_foundation_partner_key_n,p_foundation_proposal_key_i,p_proposal_detail_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFoundationProposalDetailTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFoundationProposalDetailCascading.DeleteUsingTemplate(MyPFoundationProposalDetailTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFoundationProposalAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFoundationProposalDetailCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFoundationPartnerKey, Int32 AFoundationProposalKey, Int32 AProposalDetailId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFoundationProposalDetailAccess.DeleteByPrimaryKey(AFoundationPartnerKey, AFoundationProposalKey, AProposalDetailId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFoundationProposalDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFoundationProposalDetailAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFoundationDeadlineCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFoundationPartnerKey, Int32 AFoundationDeadlineKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFoundationDeadlineAccess.DeleteByPrimaryKey(AFoundationPartnerKey, AFoundationDeadlineKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFoundationDeadlineRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PFoundationDeadlineAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SWorkflowDefinitionCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AWorkflowId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SWorkflowUserTable MySWorkflowUserTable = SWorkflowUserAccess.LoadViaSWorkflowDefinition(AWorkflowId, StringHelper.StrSplit("s_workflow_id_i,s_user_id_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowUserTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowUserCascading.DeleteUsingTemplate(MySWorkflowUserTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SWorkflowGroupTable MySWorkflowGroupTable = SWorkflowGroupAccess.LoadViaSWorkflowDefinition(AWorkflowId, StringHelper.StrSplit("s_workflow_id_i,s_group_id_c,s_group_unit_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowGroupTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowGroupCascading.DeleteUsingTemplate(MySWorkflowGroupTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SWorkflowStepTable MySWorkflowStepTable = SWorkflowStepAccess.LoadViaSWorkflowDefinition(AWorkflowId, StringHelper.StrSplit("s_workflow_id_i,s_step_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowStepTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowStepCascading.DeleteUsingTemplate(MySWorkflowStepTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SWorkflowInstanceTable MySWorkflowInstanceTable = SWorkflowInstanceAccess.LoadViaSWorkflowDefinition(AWorkflowId, StringHelper.StrSplit("s_workflow_instance_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowInstanceTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowInstanceCascading.DeleteUsingTemplate(MySWorkflowInstanceTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SWorkflowDefinitionAccess.DeleteByPrimaryKey(AWorkflowId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SWorkflowDefinitionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SWorkflowUserTable MySWorkflowUserTable = SWorkflowUserAccess.LoadViaSWorkflowDefinitionTemplate(ATemplateRow, StringHelper.StrSplit("s_workflow_id_i,s_user_id_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowUserTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowUserCascading.DeleteUsingTemplate(MySWorkflowUserTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SWorkflowGroupTable MySWorkflowGroupTable = SWorkflowGroupAccess.LoadViaSWorkflowDefinitionTemplate(ATemplateRow, StringHelper.StrSplit("s_workflow_id_i,s_group_id_c,s_group_unit_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowGroupTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowGroupCascading.DeleteUsingTemplate(MySWorkflowGroupTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SWorkflowStepTable MySWorkflowStepTable = SWorkflowStepAccess.LoadViaSWorkflowDefinitionTemplate(ATemplateRow, StringHelper.StrSplit("s_workflow_id_i,s_step_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowStepTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowStepCascading.DeleteUsingTemplate(MySWorkflowStepTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SWorkflowInstanceTable MySWorkflowInstanceTable = SWorkflowInstanceAccess.LoadViaSWorkflowDefinitionTemplate(ATemplateRow, StringHelper.StrSplit("s_workflow_instance_id_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowInstanceTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowInstanceCascading.DeleteUsingTemplate(MySWorkflowInstanceTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SWorkflowDefinitionAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SWorkflowUserCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AWorkflowId, String AUserId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SWorkflowUserAccess.DeleteByPrimaryKey(AWorkflowId, AUserId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SWorkflowUserRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SWorkflowUserAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SWorkflowGroupCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AWorkflowId, String AGroupId, Int64 AGroupUnitKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SWorkflowGroupAccess.DeleteByPrimaryKey(AWorkflowId, AGroupId, AGroupUnitKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SWorkflowGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SWorkflowGroupAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SWorkflowStepCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AWorkflowId, Int32 AStepNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SWorkflowStepAccess.DeleteByPrimaryKey(AWorkflowId, AStepNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SWorkflowStepRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SWorkflowStepAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SFunctionRelationshipCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AFunction1, String AFunction2, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SFunctionRelationshipAccess.DeleteByPrimaryKey(AFunction1, AFunction2, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SFunctionRelationshipRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SFunctionRelationshipAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SWorkflowInstanceCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AWorkflowInstanceId, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SWorkflowInstanceStepTable MySWorkflowInstanceStepTable = SWorkflowInstanceStepAccess.LoadViaSWorkflowInstance(AWorkflowInstanceId, StringHelper.StrSplit("s_workflow_instance_id_i,s_step_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowInstanceStepTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowInstanceStepCascading.DeleteUsingTemplate(MySWorkflowInstanceStepTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SWorkflowInstanceAccess.DeleteByPrimaryKey(AWorkflowInstanceId, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SWorkflowInstanceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SWorkflowInstanceStepTable MySWorkflowInstanceStepTable = SWorkflowInstanceStepAccess.LoadViaSWorkflowInstanceTemplate(ATemplateRow, StringHelper.StrSplit("s_workflow_instance_id_i,s_step_number_i", ","), ATransaction);
                for (countRow = 0; (countRow != MySWorkflowInstanceStepTable.Rows.Count); countRow = (countRow + 1))
                {
                    SWorkflowInstanceStepCascading.DeleteUsingTemplate(MySWorkflowInstanceStepTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SWorkflowInstanceAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SWorkflowInstanceStepCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int32 AWorkflowInstanceId, Int32 AStepNumber, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SWorkflowInstanceStepAccess.DeleteByPrimaryKey(AWorkflowInstanceId, AStepNumber, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SWorkflowInstanceStepRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SWorkflowInstanceStepAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PFileInfoCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerGraphicTable MyPPartnerGraphicTable = PPartnerGraphicAccess.LoadViaPFileInfo(AKey, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerGraphicTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerGraphicCascading.DeleteUsingTemplate(MyPPartnerGraphicTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerFileTable MyPPartnerFileTable = PPartnerFileAccess.LoadViaPFileInfo(AKey, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerFileCascading.DeleteUsingTemplate(MyPPartnerFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmPersonFileTable MyPmPersonFileTable = PmPersonFileAccess.LoadViaPFileInfo(AKey, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonFileCascading.DeleteUsingTemplate(MyPmPersonFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerContactFileTable MyPPartnerContactFileTable = PPartnerContactFileAccess.LoadViaPFileInfo(AKey, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactFileCascading.DeleteUsingTemplate(MyPPartnerContactFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmDocumentFileTable MyPmDocumentFileTable = PmDocumentFileAccess.LoadViaPFileInfo(AKey, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmDocumentFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmDocumentFileCascading.DeleteUsingTemplate(MyPmDocumentFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationFileTable MyPmApplicationFileTable = PmApplicationFileAccess.LoadViaPFileInfo(AKey, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFileCascading.DeleteUsingTemplate(MyPmApplicationFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationFormsFileTable MyPmApplicationFormsFileTable = PmApplicationFormsFileAccess.LoadViaPFileInfo(AKey, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFormsFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFormsFileCascading.DeleteUsingTemplate(MyPmApplicationFormsFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupFileInfoTable MySGroupFileInfoTable = SGroupFileInfoAccess.LoadViaPFileInfo(AKey, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupFileInfoTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupFileInfoCascading.DeleteUsingTemplate(MySGroupFileInfoTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFileInfoAccess.DeleteByPrimaryKey(AKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PFileInfoRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PPartnerGraphicTable MyPPartnerGraphicTable = PPartnerGraphicAccess.LoadViaPFileInfoTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerGraphicTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerGraphicCascading.DeleteUsingTemplate(MyPPartnerGraphicTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerFileTable MyPPartnerFileTable = PPartnerFileAccess.LoadViaPFileInfoTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerFileCascading.DeleteUsingTemplate(MyPPartnerFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmPersonFileTable MyPmPersonFileTable = PmPersonFileAccess.LoadViaPFileInfoTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmPersonFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmPersonFileCascading.DeleteUsingTemplate(MyPmPersonFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PPartnerContactFileTable MyPPartnerContactFileTable = PPartnerContactFileAccess.LoadViaPFileInfoTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPPartnerContactFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PPartnerContactFileCascading.DeleteUsingTemplate(MyPPartnerContactFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmDocumentFileTable MyPmDocumentFileTable = PmDocumentFileAccess.LoadViaPFileInfoTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmDocumentFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmDocumentFileCascading.DeleteUsingTemplate(MyPmDocumentFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationFileTable MyPmApplicationFileTable = PmApplicationFileAccess.LoadViaPFileInfoTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFileCascading.DeleteUsingTemplate(MyPmApplicationFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                PmApplicationFormsFileTable MyPmApplicationFormsFileTable = PmApplicationFormsFileAccess.LoadViaPFileInfoTemplate(ATemplateRow, StringHelper.StrSplit("p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPmApplicationFormsFileTable.Rows.Count); countRow = (countRow + 1))
                {
                    PmApplicationFormsFileCascading.DeleteUsingTemplate(MyPmApplicationFormsFileTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SGroupFileInfoTable MySGroupFileInfoTable = SGroupFileInfoAccess.LoadViaPFileInfoTemplate(ATemplateRow, StringHelper.StrSplit("s_group_id_c,s_group_unit_key_n,p_file_info_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySGroupFileInfoTable.Rows.Count); countRow = (countRow + 1))
                {
                    SGroupFileInfoCascading.DeleteUsingTemplate(MySGroupFileInfoTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            PFileInfoAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerFileCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFileInfoKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerFileAccess.DeleteByPrimaryKey(AFileInfoKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerFileAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmPersonFileCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFileInfoKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonFileAccess.DeleteByPrimaryKey(AFileInfoKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmPersonFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmPersonFileAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PPartnerContactFileCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFileInfoKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerContactFileAccess.DeleteByPrimaryKey(AFileInfoKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PPartnerContactFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PPartnerContactFileAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmDocumentFileCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFileInfoKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmDocumentFileAccess.DeleteByPrimaryKey(AFileInfoKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmDocumentFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmDocumentFileAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmApplicationFileCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFileInfoKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmApplicationFileAccess.DeleteByPrimaryKey(AFileInfoKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmApplicationFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmApplicationFileAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class PmApplicationFormsFileCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(Int64 AFileInfoKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmApplicationFormsFileAccess.DeleteByPrimaryKey(AFileInfoKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(PmApplicationFormsFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            PmApplicationFormsFileAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SVolumeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFileInfoTable MyPFileInfoTable = PFileInfoAccess.LoadViaSVolume(AName, StringHelper.StrSplit("p_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFileInfoTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFileInfoCascading.DeleteUsingTemplate(MyPFileInfoTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SVolumeTable MySVolumeTable = SVolumeAccess.LoadViaSVolume(AName, StringHelper.StrSplit("s_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySVolumeTable.Rows.Count); countRow = (countRow + 1))
                {
                    SVolumeCascading.DeleteUsingTemplate(MySVolumeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SDefaultFileVolumeTable MySDefaultFileVolumeTable = SDefaultFileVolumeAccess.LoadViaSVolume(AName, StringHelper.StrSplit("s_group_name_c,s_area_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySDefaultFileVolumeTable.Rows.Count); countRow = (countRow + 1))
                {
                    SDefaultFileVolumeCascading.DeleteUsingTemplate(MySDefaultFileVolumeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SVolumeAccess.DeleteByPrimaryKey(AName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SVolumeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                PFileInfoTable MyPFileInfoTable = PFileInfoAccess.LoadViaSVolumeTemplate(ATemplateRow, StringHelper.StrSplit("p_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MyPFileInfoTable.Rows.Count); countRow = (countRow + 1))
                {
                    PFileInfoCascading.DeleteUsingTemplate(MyPFileInfoTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SVolumeTable MySVolumeTable = SVolumeAccess.LoadViaSVolumeTemplate(ATemplateRow, StringHelper.StrSplit("s_name_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySVolumeTable.Rows.Count); countRow = (countRow + 1))
                {
                    SVolumeCascading.DeleteUsingTemplate(MySVolumeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SDefaultFileVolumeTable MySDefaultFileVolumeTable = SDefaultFileVolumeAccess.LoadViaSVolumeTemplate(ATemplateRow, StringHelper.StrSplit("s_group_name_c,s_area_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySDefaultFileVolumeTable.Rows.Count); countRow = (countRow + 1))
                {
                    SDefaultFileVolumeCascading.DeleteUsingTemplate(MySDefaultFileVolumeTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SVolumeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SDefaultFileVolumeCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupName, String AArea, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SDefaultFileVolumeAccess.DeleteByPrimaryKey(AGroupName, AArea, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SDefaultFileVolumeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SDefaultFileVolumeAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SVolumePartnerGroupCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AName, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SDefaultFileVolumeTable MySDefaultFileVolumeTable = SDefaultFileVolumeAccess.LoadViaSVolumePartnerGroup(AName, StringHelper.StrSplit("s_group_name_c,s_area_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySDefaultFileVolumeTable.Rows.Count); countRow = (countRow + 1))
                {
                    SDefaultFileVolumeCascading.DeleteUsingTemplate(MySDefaultFileVolumeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SVolumePartnerGroupPartnerTable MySVolumePartnerGroupPartnerTable = SVolumePartnerGroupPartnerAccess.LoadViaSVolumePartnerGroup(AName, StringHelper.StrSplit("s_group_name_c,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySVolumePartnerGroupPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                    SVolumePartnerGroupPartnerCascading.DeleteUsingTemplate(MySVolumePartnerGroupPartnerTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SVolumePartnerGroupAccess.DeleteByPrimaryKey(AName, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SVolumePartnerGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            int countRow;
            if ((AWithCascDelete == true))
            {
                SDefaultFileVolumeTable MySDefaultFileVolumeTable = SDefaultFileVolumeAccess.LoadViaSVolumePartnerGroupTemplate(ATemplateRow, StringHelper.StrSplit("s_group_name_c,s_area_c", ","), ATransaction);
                for (countRow = 0; (countRow != MySDefaultFileVolumeTable.Rows.Count); countRow = (countRow + 1))
                {
                    SDefaultFileVolumeCascading.DeleteUsingTemplate(MySDefaultFileVolumeTable[countRow], null, ATransaction, AWithCascDelete);
                }
                SVolumePartnerGroupPartnerTable MySVolumePartnerGroupPartnerTable = SVolumePartnerGroupPartnerAccess.LoadViaSVolumePartnerGroupTemplate(ATemplateRow, StringHelper.StrSplit("s_group_name_c,p_partner_key_n", ","), ATransaction);
                for (countRow = 0; (countRow != MySVolumePartnerGroupPartnerTable.Rows.Count); countRow = (countRow + 1))
                {
                    SVolumePartnerGroupPartnerCascading.DeleteUsingTemplate(MySVolumePartnerGroupPartnerTable[countRow], null, ATransaction, AWithCascDelete);
                }
            }
            SVolumePartnerGroupAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SVolumePartnerGroupPartnerCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupName, Int64 APartnerKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SVolumePartnerGroupPartnerAccess.DeleteByPrimaryKey(AGroupName, APartnerKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SVolumePartnerGroupPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SVolumePartnerGroupPartnerAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }

    /// auto generated
    public class SGroupFileInfoCascading : TTypedDataAccess
    {
        /// cascading delete
        public static void DeleteByPrimaryKey(String AGroupId, Int64 AGroupUnitKey, Int64 AFileInfoKey, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupFileInfoAccess.DeleteByPrimaryKey(AGroupId, AGroupUnitKey, AFileInfoKey, ATransaction);
        }

        /// cascading delete
        public static void DeleteUsingTemplate(SGroupFileInfoRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
        {
            SGroupFileInfoAccess.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        }
    }
}