//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>Delegate forward declaration</summary>
    public delegate int DelegateGetIconIndexForRow(int Arow);

    /// <summary>
    /// A column that is not editable.
    ///
    /// </summary>
    public class TDataGridTextBoxColumnNotEditable : DataGridTextBoxColumn
    {
        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASource"></param>
        /// <param name="ARowNum"></param>
        /// <param name="ABounds"></param>
        /// <param name="AReadOnly"></param>
        /// <param name="AInstantText"></param>
        /// <param name="ACellIsVisible"></param>
        protected override void Edit(System.Windows.Forms.CurrencyManager ASource,
            int ARowNum,
            System.Drawing.Rectangle ABounds,
            Boolean AReadOnly,
            string AInstantText,
            Boolean ACellIsVisible)
        {
            // don't call baseclass > prevents editing!
        }
    }

    /// <summary>
    /// A column that is not editable.
    /// Additionally, it prevents cursor-down and cursor-up keys to be pressed when
    /// in last/first row and also prevents cursor-left and cursor-right to change
    /// the column.
    ///
    /// </summary>
    public class TDataGridTextBoxColumnNotEditableNotNavigable : DataGridTextBoxColumn
    {
        private TKeyTrapTextBox FKeyTrapTextBox;
        private Boolean FIsEditing;

        /// <summary>todoComment</summary>
        public static int RowCount;

        #region TDataGridTextBoxColumnNotEditableNotNavigable

        /// <summary>
        /// constructor
        /// </summary>
        public TDataGridTextBoxColumnNotEditableNotNavigable() : base()
        {
            this.FKeyTrapTextBox = new TKeyTrapTextBox();
            this.FKeyTrapTextBox.BorderStyle = BorderStyle.None;

            // Include(this.FKeyTrapTextBox.Leave, LeaveKeyTrapTextBox);
            // Include(this.FKeyTrapTextBox.KeyPress, TextBoxEditStarted);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        protected override Boolean Commit(System.Windows.Forms.CurrencyManager dataSource, int rowNum)
        {
            if (this.FIsEditing)
            {
                this.FIsEditing = false;
                SetColumnValueAtRow(dataSource, rowNum, this.FKeyTrapTextBox.Text);
            }

            return true;
        }

        /// <summary>
        /// procedure TextBoxEditStarted(sender: System.Object; e: KeyPressEventArgs);    procedure LeaveKeyTrapTextBox(sender: System.Object; e: EventArgs);
        /// </summary>
        /// <returns>void</returns>
        protected override void Edit(System.Windows.Forms.CurrencyManager source,
            int rowNum,
            System.Drawing.Rectangle bounds,
            Boolean readOnly,
            string instantText,
            Boolean cellIsVisible)
        {
            RowCount = source.Count;
            base.Edit(source, rowNum, bounds, true, instantText, false);

            // readOnly, instantText, cellIsVisible
            this.FKeyTrapTextBox.Parent = this.TextBox.Parent;
            this.FKeyTrapTextBox.Location = this.TextBox.Location;
            this.FKeyTrapTextBox.Size = this.TextBox.Size;
            this.FKeyTrapTextBox.Text = this.TextBox.Text;
            this.TextBox.Visible = false;
            this.FKeyTrapTextBox.Visible = true;
            this.FKeyTrapTextBox.ReadOnly = true;
            this.FKeyTrapTextBox.BringToFront();
            this.FKeyTrapTextBox.Focus();
        }

        #endregion
    }

    /// <summary>
    /// TextBox that prevents cursor-down and cursor-up keys to be pressed when in
    /// last/first row.
    /// @see Used by TDataGridTextBoxColumnNotEditableNotNavigable.
    ///
    /// </summary>
    public class TKeyTrapTextBox : TextBox
    {
        private const Int32 WM_KEYDOWN = 0x100;

        #region TKeyTrapTextBox

        /// <summary>
        /// WM_KEYUP: Integer = 0x101;    WM_CHAR: Integer = 0x102;
        /// </summary>
        /// <returns>void</returns>
        public override Boolean PreProcessMessage(ref Message AMsg)
        {
            Boolean ReturnValue = false;
            Keys KeyCode;

            KeyCode = ((Keys)AMsg.WParam.ToInt32()) & Keys.KeyCode;

            if (((AMsg.Msg == WM_KEYDOWN) && (KeyCode == Keys.Up)) || ((AMsg.Msg == WM_KEYDOWN) && (KeyCode == Keys.Down)))
            {
                if (KeyCode == Keys.Down)
                {
                    // don't allow cursordown when in last row (would lead to deselection of row!)
                    if (((DataGrid) this.Parent).CurrentRowIndex !=
                        (((DataGrid) this.Parent).BindingContext[((DataGrid) this.Parent).DataSource, ((DataGrid) this.Parent).DataMember].Count) - 1)
                    {
                        // (this.Parent as DataGrid).VisibleRowCount
                        ReturnValue = base.PreProcessMessage(ref AMsg);
                    }
                    else
                    {
                        ReturnValue = true;
                    }
                }

                if (KeyCode == Keys.Up)
                {
                    // don't allow cursorup when in first row (would lead to deselection of row!)
                    if (((DataGrid) this.Parent).CurrentRowIndex != 0)
                    {
                        ReturnValue = base.PreProcessMessage(ref AMsg);
                    }
                    else
                    {
                        ReturnValue = true;
                    }
                }
            }
            else if (AMsg.Msg == WM_KEYDOWN)
            {
                ReturnValue = true;
            }
            else
            {
                ReturnValue = base.PreProcessMessage(ref AMsg);
            }

            return ReturnValue;
        }

        #endregion
    }

    /// <summary>
    /// A column that displays an icon.
    ///
    /// Each row can have a different icon - the icon for each row needs to be
    /// determined in a function that is called as a Delegate for each row.
    ///
    /// @see For an implementation look in UC_PartnerEdit_PartnerTabSet.pas!
    ///
    /// </summary>
    public class TDataGridIconOnlyColumn : DataGridTextBoxColumn
    {
        private ImageList FIcons;
        private DelegateGetIconIndexForRow FGetIconIndex;

        #region TDataGridIconOnlyColumn

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Icons"></param>
        /// <param name="getIconIndex"></param>
        public TDataGridIconOnlyColumn(ImageList Icons, DelegateGetIconIndexForRow getIconIndex)
            : base()
        {
            this.FIcons = Icons;
            this.FGetIconIndex = getIconIndex;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        /// <param name="source"></param>
        /// <param name="rowNum"></param>
        /// <param name="backBrush"></param>
        /// <param name="foreBrush"></param>
        /// <param name="alignToRight"></param>
        protected override void Paint(System.Drawing.Graphics g,
            System.Drawing.Rectangle bounds,
            System.Windows.Forms.CurrencyManager source,
            int rowNum,
            System.Drawing.Brush backBrush,
            System.Drawing.Brush foreBrush,
            Boolean alignToRight)
        {
            try
            {
                g.FillRectangle(backBrush, bounds);
                g.DrawImage(this.FIcons.Images[FGetIconIndex(rowNum)], bounds);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rowNum"></param>
        /// <param name="bounds"></param>
        /// <param name="readOnly"></param>
        /// <param name="instantText"></param>
        /// <param name="cellIsVisible"></param>
        protected override void Edit(System.Windows.Forms.CurrencyManager source,
            int rowNum,
            System.Drawing.Rectangle bounds,
            Boolean readOnly,
            string instantText,
            Boolean cellIsVisible)
        {
            if (this.MappingName == "Icon")
            {
                return;
            }

            base.Edit(source, rowNum, bounds, readOnly, instantText, cellIsVisible);
        }

        #endregion
    }

    /// <summary>
    /// Delphi implementation of
    /// 'HOW TO: Extend the Windows Form DataGridTextBoxColumn to Display Data From
    /// Other Tables by Using Visual C# .NET'.
    ///
    /// @comment Be sure to have read and understood the article
    /// http://support.microsoft.com/default.aspx?scid=kb;en-us;Q319076 before
    /// using this quited advanced column!
    /// @see For an implementation look in UC_PartnerEdit_PartnerTabSet.pas!
    ///
    /// </summary>
    public class TDataGridJoinTextBoxColumn : DataGridTextBoxColumn
    {
        private string FRelationName;
        private DataColumn FParentField;

        /// <summary>todoComment</summary>
        public new Boolean ReadOnly
        {
            get
            {
                return true;
            }
        }


        #region TDataGridJoinTextBoxColumn

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ARelationName"></param>
        /// <param name="AParentField"></param>
        public TDataGridJoinTextBoxColumn(string ARelationName, DataColumn AParentField)
            : base()
        {
            this.FRelationName = ARelationName;
            this.FParentField = AParentField;
            base.ReadOnly = true;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACm"></param>
        /// <param name="ARowNum"></param>
        /// <returns></returns>
        protected override System.Object GetColumnValueAtRow(CurrencyManager ACm, int ARowNum)
        {
            System.Object ReturnValue;
            DataRow DrParent;
            DataRow Dr;

            // Get the current DataRow from the CurrencyManager.
            // Use the GetParentRow and the DataRelation name to get
            // the parent row.
            // Return the field value from the parent row.
            try
            {
                Dr = ((DataView)ACm.List)[ARowNum].Row;
                DrParent = Dr.GetParentRow(this.FRelationName);
                ReturnValue = DrParent[this.FParentField].ToString();
            }
            catch (System.Exception)
            {
                // Necessary when adding rows.
                ReturnValue = "";
                return ReturnValue;
            }
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ACm"></param>
        /// <param name="ARowNum"></param>
        /// <returns></returns>
        protected override Boolean Commit(CurrencyManager ACm, int ARowNum)
        {
            return false;
        }

        #endregion
    }
}