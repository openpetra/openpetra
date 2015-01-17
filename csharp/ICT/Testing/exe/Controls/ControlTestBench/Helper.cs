//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Ict.Common.Controls;
using Ict.Common.IO;

namespace ControlTestBench
{
    /// <summary>
    /// todoComment
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AVisualStyle"></param>
        /// <returns></returns>
        public static TVisualStylesEnum GetVisualStylesEnumFromString(String AVisualStyle)
        {
            TVisualStylesEnum EnumStyle;

            switch (AVisualStyle)
            {
                case "AccordionPanel":
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel;
                    break;

                case "TaskPanel":
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsTaskPanel;
                    break;

                case "Dashboard":
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsDashboard;
                    break;

                case "Shepherd":
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
                    break;

                case "HorizontalCollapse":
                default:
                    EnumStyle = Ict.Common.Controls.TVisualStylesEnum.vsHorizontalCollapse;
                    break;
            }

            return EnumStyle;
        }
    }
}