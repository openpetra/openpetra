//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       morayh
//
// Copyright 2004-2012 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Editors;
using Ict.Common.Controls;
using DevAge.ComponentModel.Validator;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Contains logic for the UC_PartnerFoundation UserControl.
    /// </summary>
    public class TUCPartnerFoundationLogic
    {
        private PartnerEditTDS FMainDS;
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private DataView FCurrentFoundationDV;
        private DataView FSubmitByDV;
        private DataView FProposalsDV;
        private DataView FProposalDetailsDV;
        private TSgrdDataGrid FSubmitByDates;

        /// <summary>private int FProposalSequence;</summary>
        private int FProposalDetailSequence;

        /// <summary>todoComment</summary>
        public PartnerEditTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        /// <summary>todoComment</summary>
        public TSgrdDataGrid SubmitByDates
        {
            get
            {
                return FSubmitByDates;
            }

            set
            {
                FSubmitByDates = value;
            }
        }

        /// <summary>todoComment</summary>
        public DataView SubmitByDataView
        {
            get
            {
                return FSubmitByDV;
            }

            set
            {
                FSubmitByDV = value;
            }
        }

        /// <summary>todoComment</summary>
        public DataView ProposalsDataView
        {
            get
            {
                return FProposalsDV;
            }

            set
            {
                FProposalsDV = value;
            }
        }

        /// <summary>todoComment</summary>
        public DataView ProposalDetailsDataView
        {
            get
            {
                return FProposalDetailsDV;
            }

            set
            {
                FProposalDetailsDV = value;
            }
        }


        #region TUCPartnerFoundationLogic

        /// <summary>
        /// constructor
        /// </summary>
        public TUCPartnerFoundationLogic() : base()
        {
            //FProposalSequence = -1;
            FProposalDetailSequence = -1;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="ASourceTable"></param>
        public void CreateColumns(TSgrdDataGrid AGrid, PFoundationDeadlineTable ASourceTable)
        {
            SourceGrid.Cells.Editors.ComboBox FMonthEditor;
            SourceGrid.Cells.Editors.TextBoxNumeric FDayEditor;
            Int32[] MonthDropDownValues;
            String[] MonthDropDownDisplayValues;
            DevAge.ComponentModel.Validator.ValueMapping MonthDropDownMapping;

            // Set up Month ComboBox editor
            MonthDropDownValues = new Int32[] {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
            };
            MonthDropDownDisplayValues = new String[] {
                "JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST",
                "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"
            };
            FMonthEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(Int32), MonthDropDownValues, true);
            FMonthEditor.Control.FormattingEnabled = true;
            MonthDropDownMapping = new DevAge.ComponentModel.Validator.ValueMapping();
            MonthDropDownMapping.SpecialType = typeof(String);
            MonthDropDownMapping.SpecialList = MonthDropDownDisplayValues;
            MonthDropDownMapping.ValueList = MonthDropDownValues;
            MonthDropDownMapping.BindValidator(FMonthEditor);
            FMonthEditor.EditableMode = EditableMode.Focus;

            FDayEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(Int32));
            FDayEditor.EditableMode = EditableMode.Focus;

            AGrid.AddTextColumn("Month", ASourceTable.ColumnDeadlineMonth, 80, FMonthEditor);
            AGrid.AddTextColumn("Day", ASourceTable.ColumnDeadlineDay, -1, FDayEditor);

            // Hook up editor events
            FDayEditor.Control.Validating += new System.ComponentModel.CancelEventHandler(Day_Validating);
            FMonthEditor.Control.Validating += new System.ComponentModel.CancelEventHandler(Month_Validating);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="ASourceTable"></param>
        public void CreateColumns(TSgrdDataGrid AGrid, PFoundationProposalTable ASourceTable)
        {
            AGrid.AddTextColumn("Status", ASourceTable.ColumnProposalStatus);
            AGrid.AddTextColumn("Submitted", ASourceTable.ColumnSubmittedDate);
            AGrid.AddTextColumn("Requested", ASourceTable.ColumnAmountRequested);
            AGrid.AddTextColumn("Approved", ASourceTable.ColumnAmountApproved);
            AGrid.AddTextColumn("Received", ASourceTable.ColumnAmountGranted);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="ASourceTable"></param>
        public void CreateColumns(TSgrdDataGrid AGrid, PFoundationProposalDetailTable ASourceTable)
        {
            SourceGrid.Cells.Editors.ComboBox ProjectEditor;
            SourceGrid.Cells.Editors.TextBoxButton MinistryEditor;
            ProjectEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(String));
            ProjectEditor.EditableMode = EditableMode.Focus;
            MinistryEditor = new SourceGrid.Cells.Editors.TextBoxButton(typeof(Int64));
            MinistryEditor.EditableMode = EditableMode.Focus;
            AGrid.AddTextColumn("Projects", ASourceTable.ColumnProjectMotivationDetail, -1, ProjectEditor);
            AGrid.AddTextColumn("Key Ministries", ASourceTable.ColumnKeyMinistryKey, -1, MinistryEditor);
            AGrid.AddTextColumn("Areas", ASourceTable.ColumnAreaPartnerKey, -1, MinistryEditor);
            AGrid.AddTextColumn("Fields", ASourceTable.ColumnFieldPartnerKey);
        }

        private void Day_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int NewDay;
            int Month;

            NewDay = Convert.ToInt32((sender as Control).Text);
            Month = ((PFoundationDeadlineRow)FSubmitByDV[FSubmitByDates.Selection.ActivePosition.Row - 1].Row).DeadlineMonth;

            try
            {
                e.Cancel = (!ValidateDayMonth(NewDay, Month));
            }
            catch (Exception exp)
            {
                if (exp is EOutOfRangeException)
                {
                    MessageBox.Show(((EOutOfRangeException)exp).Message, ((EOutOfRangeException)exp).Caption);
                    e.Cancel = true;
                }
                else
                {
                    throw;
                }
            }
        }

        private void DeleteDeadlines()
        {
            DataView InspectDV;

            InspectDV = new DataView(FMainDS.PFoundationDeadline, "", "", DataViewRowState.CurrentRows);

            while (InspectDV.Count > 0)
            {
                InspectDV[0].Delete();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ProposalDetail"></param>
        public void DeleteProposalDetail(PFoundationProposalDetailRow ProposalDetail)
        {
            if (MessageBox.Show("You have chosen to delete this record." + Environment.NewLine + Environment.NewLine +
                    "Dou you really want to delete it?", "Delete Proposal Detail?", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                ProposalDetail.Delete();
            }
        }

        /// <summary>
        /// Gets the date a proposal was last submitted.
        ///
        /// </summary>
        /// <returns>void</returns>
        public DateTime Get_LastSubmittedDate()
        {
            DateTime ReturnValue;
            DataView ProposalDV;

            ReturnValue = DateTime.MinValue;
            ProposalDV = FMainDS.PFoundationProposal.DefaultView;
            ProposalDV.Sort = PFoundationProposalTable.GetSubmittedDateDBName() + " DESC";

            if ((ProposalDV.Count > 0) && (ProposalDV != null))
            {
                ReturnValue = (DateTime)ProposalDV[0][PFoundationProposalTable.GetSubmittedDateDBName()];
            }

            return ReturnValue;
        }

        /// <summary>
        /// Gets the next reminder date, if any.
        ///
        /// </summary>
        /// <returns>void</returns>
        public DateTime Get_NextReminderDate()
        {
            // var
            // ReminderDV: DataView;
            return DateTime.MinValue;

            // TODO : Uncomment Partner Reminder code when the table is added to the dataset.
            // ReminderDV := new DataView(FMainDS.PPartnerReminder, '', PPartnerReminderTable.GetNextReminderDateDBName() + ' DESC', DataViewRowState.None);
            // if (ReminderDV.Count > 0) and (ReminderDV[0] <> nil then
            // begin
            // Result := ReminderDV[0][PPartnerReminderTable.GetNextReminderDateDBName()] as DateTime();
            // end;
        }

        /// <summary>
        /// Calclulates a date when the next proposal can be submitted.
        ///
        /// </summary>
        /// <returns>void</returns>
        public DateTime Get_NextSubmitDate()
        {
            DateTime ReturnValue;
            DateTime LastSubmit;
            PFoundationRow CurrentFoundationRow;

            LastSubmit = Get_LastSubmittedDate();

            if (LastSubmit == DateTime.MinValue)
            {
                ReturnValue = PossibleDate(DateTime.Today);
            }
            else
            {
                ReturnValue = PossibleDate(LastSubmit);
                CurrentFoundationRow = (PFoundationRow)(FCurrentFoundationDV[0].Row);

                if (CurrentFoundationRow.SubmitFrequency == "Bi-Annually")
                {
                    LastSubmit = LastSubmit.AddYears(2);

                    if (LastSubmit > DateTime.Today)
                    {
                        ReturnValue = PossibleDate(LastSubmit);
                    }
                    else
                    {
                        ReturnValue = PossibleDate(DateTime.Today);
                    }
                }
                else if (CurrentFoundationRow.SubmitFrequency == "Annually")
                {
                    LastSubmit = LastSubmit.AddYears(1);

                    if (LastSubmit > DateTime.Today)
                    {
                        ReturnValue = PossibleDate(LastSubmit);
                    }
                    else
                    {
                        ReturnValue = PossibleDate(DateTime.Today);
                    }
                }
                else if (CurrentFoundationRow.SubmitFrequency == "No Restrictions")
                {
                    ReturnValue = PossibleDate(DateTime.Today);

                    if (LastSubmit > PreviousDate(DateTime.Today))
                    {
                        ReturnValue = PossibleDate(ReturnValue.AddDays(1));
                    }
                }
                else
                {
                    throw new EOutOfRangeException(
                        "Submit frequency \"" + CurrentFoundationRow.SubmitFrequency + "\" must be bi-annually, annually or no restrictions.",
                        "Invalid Submit Frequency");
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Loads Foundation Data (multiple DataTables) from Petra Server into FMainDS.
        ///
        /// </summary>
        /// <returns>true if successful, otherwise false.
        /// </returns>
        public Boolean LoadDataOnDemand()
        {
            Boolean ReturnValue;

            try
            {
                // Make sure that Typed DataTables are already there at Client side
                if (FMainDS.PFoundation == null)
                {
                    FMainDS.Tables.Add(new PFoundationTable());
                }

                if (FMainDS.PFoundationProposal == null)
                {
                    FMainDS.Tables.Add(new PFoundationProposalTable());
                }

                if (FMainDS.PFoundationProposalDetail == null)
                {
                    FMainDS.Tables.Add(new PFoundationProposalDetailTable());
                }

                if (FMainDS.PFoundationDeadline == null)
                {
                    FMainDS.Tables.Add(new PFoundationDeadlineTable());
                }

                FMainDS.InitVars();
                FCurrentFoundationDV = new DataView(FMainDS.PFoundation, "", "", DataViewRowState.CurrentRows);

                if (TClientSettings.DelayedDataLoading)
                {
                    if (!((FCurrentFoundationDV.Count == 1) && (FCurrentFoundationDV[0].Row.RowState == DataRowState.Added)))
                    {
                        // MessageBox.Show('DelayedDataLoading: getting data from PetraServer and merge it!');
                        FMainDS.Merge(FPartnerEditUIConnector.GetDataFoundation(false));
                        FMainDS.PFoundation.AcceptChanges();
                        FMainDS.PFoundationProposal.AcceptChanges();
                        FMainDS.PFoundationProposalDetail.AcceptChanges();
                        FMainDS.PFoundationDeadline.AcceptChanges();
                    }
                }

                if (FCurrentFoundationDV.Count != 0)
                {
                    ReturnValue = true;
                }
                else
                {
                    ReturnValue = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ReturnValue;
        }

        private void Month_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int Day;
            int NewMonth;
            int Counter;

            if ((sender as DevAge.Windows.Forms.DevAgeComboBox).SelectedItem != null)
            {
                NewMonth = Convert.ToInt32((sender as DevAge.Windows.Forms.DevAgeComboBox).SelectedItem);
                Day = ((PFoundationDeadlineRow)FSubmitByDV[FSubmitByDates.Selection.ActivePosition.Row - 1].Row).DeadlineDay;

                try
                {
                    e.Cancel = (!ValidateDayMonth(Day, NewMonth));
                }
                catch (Exception exp)
                {
                    if (exp is EOutOfRangeException)
                    {
                        MessageBox.Show(((EOutOfRangeException)exp).Message, ((EOutOfRangeException)exp).Caption);
                    }
                    else
                    {
                        throw;
                    }
                }

                for (Counter = 0; Counter <= FSubmitByDV.Count - 1; Counter += 1)
                {
//					MessageBox.Show("DeadlineMonth[" + Counter.ToString() + "]: " + ((PFoundationDeadlineRow)FSubmitByDV[Counter].Row).DeadlineMonth.ToString());
                    if ((Counter != FSubmitByDates.Selection.ActivePosition.Row - 1)
                        && (((PFoundationDeadlineRow)FSubmitByDV[Counter].Row).DeadlineMonth == NewMonth))
                    {
                        MessageBox.Show("Month must be unique.", "Validation Error");
                        e.Cancel = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="Proposal"></param>
        public void NewProposalDetail(PFoundationProposalRow Proposal)
        {
            PFoundationProposalDetailRow NewDetail;

            NewDetail = FMainDS.PFoundationProposalDetail.NewRowTyped(true);
            NewDetail.ProposalDetailId = FProposalDetailSequence;
            FProposalDetailSequence = FProposalDetailSequence - 1;
            NewDetail.FoundationPartnerKey = Proposal.FoundationPartnerKey;
            NewDetail.FoundationProposalKey = (int)Proposal.FoundationProposalKey;
            FMainDS.PFoundationProposalDetail.Rows.Add(NewDetail);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AReviewFrequency"></param>
        public void PopulateDeadlines(string AReviewFrequency)
        {
            int Counter;
            PFoundationDeadlineRow DeadlineRow;
            PFoundationRow CurrentFoundationRow;

            // MessageBox.Show('PopulateDeadlines called');
            CurrentFoundationRow = (PFoundationRow)FCurrentFoundationDV[0].Row;
            try
            {
                if (AReviewFrequency == "Annually")
                {
                    DeleteDeadlines();
                    DeadlineRow = FMainDS.PFoundationDeadline.NewRowTyped(true);
                    DeadlineRow.FoundationPartnerKey = CurrentFoundationRow.PartnerKey;
                    DeadlineRow.FoundationDeadlineKey = 0;
                    DeadlineRow.DeadlineMonth = 1;
                    DeadlineRow.DeadlineDay = 15;
                    FMainDS.PFoundationDeadline.Rows.Add(DeadlineRow);
                }
                else if (AReviewFrequency == "Quarterly")
                {
                    DeleteDeadlines();

                    for (Counter = 0; Counter <= 3; Counter += 1)
                    {
                        DeadlineRow = FMainDS.PFoundationDeadline.NewRowTyped();
                        DeadlineRow.FoundationPartnerKey = CurrentFoundationRow.PartnerKey;
                        DeadlineRow.FoundationDeadlineKey = Counter;
                        DeadlineRow.DeadlineMonth = (Counter * 3) + 1;
                        DeadlineRow.DeadlineDay = 15;
                        FMainDS.PFoundationDeadline.Rows.Add(DeadlineRow);
                    }
                }
                else if (AReviewFrequency == "Monthly")
                {
                    DeleteDeadlines();

                    for (Counter = 0; Counter <= 11; Counter += 1)
                    {
                        DeadlineRow = FMainDS.PFoundationDeadline.NewRowTyped();
                        DeadlineRow.FoundationPartnerKey = CurrentFoundationRow.PartnerKey;
                        DeadlineRow.FoundationDeadlineKey = Counter;
                        DeadlineRow.DeadlineMonth = Counter + 1;
                        DeadlineRow.DeadlineDay = 15;
                        FMainDS.PFoundationDeadline.Rows.Add(DeadlineRow);
                    }
                }
                else
                {
                    throw new EOutOfRangeException("Review frequency must be annually, quarterly or monthly.", "Invalid Review Frequency");
                }
            }
            catch (EOutOfRangeException E)
            {
                MessageBox.Show(E.Message, E.Caption);
            }
        }

        private DateTime PossibleDate(DateTime ACheckDate)
        {
            DateTime ReturnValue;
            DataView ProposalDV;
            DataView CurrentFoundationDeadlinesDV;

            CurrentFoundationDeadlinesDV = new DataView(FMainDS.PFoundationDeadline, "", "", DataViewRowState.CurrentRows);

            if (CurrentFoundationDeadlinesDV.Count > 0)
            {
                ProposalDV = new DataView(FMainDS.PFoundationDeadline,
                    "(p_deadline_month_i = " + ACheckDate.Month.ToString() + " and p_deadline_day_i >= " + ACheckDate.Day.ToString() +
                    ") or (p_deadline_month_i > " + ACheckDate.Month.ToString() + ')', "p_deadline_month_i, p_deadline_day_i",
                    DataViewRowState.CurrentRows);

                if (ProposalDV.Count > 0)
                {
                    ReturnValue = new DateTime(ACheckDate.Year, (int)(ProposalDV[0]["p_deadline_month_i"]), (int)(ProposalDV[0]["p_deadline_day_i"]));
                }
                else
                {
                    ProposalDV.RowFilter = "";
                    ReturnValue =
                        new DateTime(ACheckDate.Year + 1, (int)(ProposalDV[0]["p_deadline_month_i"]), (int)(ProposalDV[0]["p_deadline_day_i"]));
                }
            }
            else
            {
                ReturnValue = DateTime.MinValue;
            }

            return ReturnValue;
        }

        private DateTime PreviousDate(DateTime ACheckDate)
        {
            DateTime ReturnValue;
            DataView ProposalDV;

            ProposalDV = new DataView(FMainDS.PFoundationDeadline,
                "(p_deadline_month_i = " + ACheckDate.Month.ToString() + "and p_deadline_day_i < " + ACheckDate.Day.ToString() +
                ") or (p_deadline_month_i < " + ACheckDate.Month.ToString() + ')', "p_deadline_month_i DESC, p_deadline_day_i DESC",
                DataViewRowState.CurrentRows);

            if (ProposalDV.Count > 0)
            {
                ReturnValue = new DateTime(ACheckDate.Year, (int)(ProposalDV[0]["p_deadline_month_i"]), (int)(ProposalDV[0]["p_deadline_day_i"]));
            }
            else
            {
                ProposalDV.RowFilter = "";
                ReturnValue = new DateTime(ACheckDate.Year - 1, (int)(ProposalDV[0]["p_deadline_month_i"]), (int)(ProposalDV[0]["p_deadline_day_i"]));
            }

            return ReturnValue;
        }

        private Boolean ValidateDayMonth(int ADay, int AMonth)
        {
            Boolean ReturnValue;

            ReturnValue = true;

            if (ADay < 1)
            {
                throw new EOutOfRangeException("Day must be greater than 1.", "Invalid Day");

                //ReturnValue = false;
            }
            else if (((AMonth == 1) || (AMonth == 3) || (AMonth == 5)
                      || (AMonth == 7) || (AMonth == 8) || (AMonth == 10) || (AMonth == 12)) && (ADay > 31))
            {
                throw new EOutOfRangeException("Day must be between 1 and 31.", "Invalid Day");

                //ReturnValue = false;
            }
            else if (((AMonth == 4) || (AMonth == 6) || (AMonth == 9) || (AMonth == 11)) && (ADay > 30))
            {
                throw new EOutOfRangeException("Day must be between 1 and 30.", "Invalid Day");

                //ReturnValue = false;
            }
            else if ((AMonth == 2) && (ADay > 28))
            {
                throw new EOutOfRangeException("Day must be between 1 and 28.", "Invalid Day");

                //ReturnValue = false;
            }

            return ReturnValue;
        }

        #endregion
    }
}