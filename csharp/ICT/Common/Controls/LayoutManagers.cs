//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using System.Windows.Forms;

namespace Ict.Common.Controls.Formatting
{
    /// <summary>
    /// WinForms Layout Managers that can be used with any container
    /// control (eg. TabPage, Panel, GroupBox) to control how the controls that are
    /// placed within the container control are layed out.
    /// </summary>
    public class TSingleLineFlow
    {
        /// <summary>
        /// Indicator for starting a new 'group' of Controls (who are set off from the previous layed out Controls by SpacerDistance).
        /// </summary>
        public const String BeginGroupIndicator = "BeginGroup";
        
        private Control FContainer;
        private int FMargin;
        private int FLeftMargin;
        private int FRightMargin;
        private int FTopMargin;
        private int FDistance;
        private int FSpacerDistance;

        /// <summary>todoComment</summary>
        public int Margin
        {
            get
            {
                return FMargin;
            }

            set
            {
                FMargin = value;
            }
        }

        /// <summary>todoComment</summary>
        public int TopMargin
        {
            get
            {
                return FTopMargin;
            }

            set
            {
                FTopMargin = value;
            }
        }

        /// <summary>todoComment</summary>
        public int LeftMargin
        {
            get
            {
                return FLeftMargin;
            }

            set
            {
                FLeftMargin = value;
            }
        }

        /// <summary>todoComment</summary>
        public int RightMargin
        {
            get
            {
                return FRightMargin;
            }

            set
            {
                FRightMargin = value;
            }
        }

        /// <summary>todoComment</summary>
        public int Distance
        {
            get
            {
                return FDistance;
            }

            set
            {
                FDistance = value;
            }
        }

        /// <summary>todoComment</summary>
        public int SpacerDistance
        {
            get
            {
                return FSpacerDistance;
            }

            set
            {
                FSpacerDistance = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="margin"></param>
        /// <param name="distance"></param>
        public TSingleLineFlow(Control parent, int margin, int distance)
        {
            this.FContainer = parent;
            this.FLeftMargin = margin;
            this.FRightMargin = margin;
            this.FTopMargin = margin;
            this.FDistance = distance;

            // Default value
            this.FSpacerDistance = 10;
            this.FContainer.Layout += new LayoutEventHandler(this.UpdateLayout);
            UpdateLayout(this, null);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateLayout(System.Object sender, System.Windows.Forms.LayoutEventArgs e)
        {
            Int16 Counter;
            int y;

            y = 0;

            for (Counter = 0; Counter <= FContainer.Controls.Count - 1; Counter += 1)
            {
                if (Counter == 0)
                {
                    y = FTopMargin;
                }

                // else
                // begin
                // y := y + FDistance;
                // end;
                if (FContainer.Controls[Counter].Tag != null)
                {
                    if (FContainer.Controls[Counter].Tag.ToString().Contains(TSingleLineFlow.BeginGroupIndicator))
                    {
                        y = y + FSpacerDistance;
                    }
                }

                if (FContainer.Controls[Counter].Visible == true)
                {
                    FContainer.Controls[Counter].Left = FLeftMargin;
                    FContainer.Controls[Counter].Top = y;
                    FContainer.Controls[Counter].Width = FContainer.Width - FLeftMargin - FRightMargin;

                    // FContainer.Controls[Counter].Height := FDistance;
                    FContainer.Controls[Counter].TabIndex = Counter;
                    y = y + FContainer.Controls[Counter].Height + FDistance;
                }
            }
        }
    }
}