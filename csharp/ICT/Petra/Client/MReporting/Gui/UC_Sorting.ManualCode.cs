//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common.Controls;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// Description of UC_Sorting_ManualCode.
    /// </summary>
    public partial class TFrmUC_Sorting
    {
        /// the number of columns that can be sorted is hard coded at the moment
        public const Int32 NUMBER_SORTBY = 3;

        /// this holds the references to the comboboxes, that way it is easier to program a flexible number of comboboxes
        protected TCmbAutoComplete[] FSortByComboboxes;

        /// the functions and column names that should be displayed in the sorting boxes
        protected ArrayList FAvailableFunctions;

        private void CmbSortBy_SelectedValueChanged(System.Object sender, System.EventArgs e)
        {
            UpdateSortingDetails(sender);
        }

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;
        }

        /// <summary>
        /// set the functions and column names that are available
        /// </summary>
        /// <param name="AAvailableFunctions"></param>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
            FAvailableFunctions = AAvailableFunctions;

            FSortByComboboxes = new TCmbAutoComplete[NUMBER_SORTBY];
            FSortByComboboxes[0] = cmbSortBy1;
            FSortByComboboxes[1] = cmbSortBy2;
            FSortByComboboxes[2] = cmbSortBy3;

            UpdateSortingDetails(null);
        }

        /**
         * This will make the correct options available in the sort comboboxes
         *
         */
        public void UpdateSortingDetails(System.Object ASender)
        {
            System.Int32 counter;
            System.Int32 counter2;
            System.Int32 CurrentIndex;
            bool SelectedAbove;

            // the idea is, that the first combobox offers all possibilities,
            // the following combobox offer all but the ones selected above;
            // if a entry gets selected, the comboboxes below lose that entry,
            // and gain the one that was selected before
            // just fill all the comboboxes
            if (ASender == null)
            {
                for (counter = 0; counter <= NUMBER_SORTBY - 1; counter += 1)
                {
                    FSortByComboboxes[counter].Enabled = (FAvailableFunctions.Count > counter);

                    foreach (TColumnFunction Func in FAvailableFunctions)
                    {
                        FSortByComboboxes[counter].AddStringItem(Func.GetDisplayValue());
                    }
                }
            }
            else
            {
                String[] OrderByArray = new String[NUMBER_SORTBY];
                CurrentIndex = -1;

                for (counter = 0; counter <= NUMBER_SORTBY - 1; counter += 1)
                {
                    OrderByArray[counter] = FSortByComboboxes[counter].GetSelectedString();

                    if (ASender == FSortByComboboxes[counter])
                    {
                        CurrentIndex = counter;
                    }
                }

                // if the combobox ASender had a previous selection, this needs to be added to the other comboboxes
                foreach (TColumnFunction Func in FAvailableFunctions)
                {
                    // add previous selection to the other comboboxes
                    for (counter = CurrentIndex + 1; counter <= NUMBER_SORTBY - 1; counter += 1)
                    {
                        if (FSortByComboboxes[counter].FindStringInComboBox(Func.GetDisplayValue()) == -1)
                        {
                            // is not selected above
                            SelectedAbove = false;

                            for (counter2 = 0; counter2 <= counter - 1; counter2 += 1)
                            {
                                if (OrderByArray[counter2] == Func.GetDisplayValue())
                                {
                                    SelectedAbove = true;
                                }
                            }

                            if (!SelectedAbove)
                            {
                                FSortByComboboxes[counter].AddStringItem(Func.GetDisplayValue());
                            }
                        }
                    }
                }

                // remove the selection of ASender from the comboboxes below
                for (counter = CurrentIndex + 1; counter <= NUMBER_SORTBY - 1; counter += 1)
                {
                    FSortByComboboxes[counter].RemoveStringItem(((TCmbAutoComplete)ASender).GetSelectedString());
                }
            }
        }

        /// <summary>
        /// read the values from the controls and give them to the calculator
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        public void ReadControls(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            String[] OrderByArray = new string[NUMBER_SORTBY];

            for (Int32 counter = 0; counter <= NUMBER_SORTBY - 1; counter += 1)
            {
                OrderByArray[counter] = FSortByComboboxes[counter].GetSelectedString();

                // MessageBox.Show('Sort by '+Counter.ToString+': '+FSortByComboboxes[counter].GetSelectedString());
                // have to encode the number of "order by" in the variable name instead of the column,
                // otherwise the meaning would be changed when columns are switched or deleted
                ACalculator.AddParameter("orderby" + counter.ToString(), OrderByArray[counter]);
            }

            String OrderByReadable = "";

            for (Int32 counter = 0; counter <= NUMBER_SORTBY - 1; counter += 1)
            {
                if (OrderByArray[counter].Length != 0)
                {
                    if (OrderByReadable.Length > 0)
                    {
                        OrderByReadable = OrderByReadable + ", ";
                    }

                    OrderByReadable = OrderByReadable + OrderByArray[counter];
                }
            }

            ACalculator.AddParameter("param_sortby_readable", OrderByReadable);
            ACalculator.SetupSorting();
        }

        /// <summary>
        /// initialise the controls using the parameters
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            for (Int32 counter = 0; counter < NUMBER_SORTBY; counter += 1)
            {
                if (AParameters.Exists("orderby" + counter.ToString()))
                {
                    FSortByComboboxes[counter].SetSelectedString(AParameters.Get("orderby" + counter.ToString()).ToString());
                }
            }
        }
    }
}