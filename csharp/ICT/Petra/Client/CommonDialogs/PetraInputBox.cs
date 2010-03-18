/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2010 by OM International
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
using System;
using System.Drawing;
using System.Windows.Forms;
using Mono.Unix;

namespace Ict.Petra.Client.CommonDialogs
{
    /// <summary>
    /// Description of PetraInputBox.
    /// </summary>
    public partial class PetraInputBox : Form
    {
        /// <summary>
        /// constructor
        /// </summary>
        public PetraInputBox()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
        }

        /// <summary>
        /// constructor
        /// </summary>
        public PetraInputBox(string ATitle, string AQuestion, string ADefaultAnswer, bool AHidePassword)
            : this()
        {
            this.Text = ATitle;
            this.lblQuestion.Text = AQuestion;
            this.txtAnswer.Text = ADefaultAnswer;

            if (AHidePassword)
            {
                this.txtAnswer.PasswordChar = '*';
            }

            this.btnCancel.Text = Catalog.GetString("Cancel");
            this.btnOK.Text = Catalog.GetString("OK");
        }

        /// <summary>
        /// returns the entered text
        /// </summary>
        /// <returns></returns>
        public string GetAnswer()
        {
            return txtAnswer.Text;
        }
    }
}