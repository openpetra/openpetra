/*
 * Created by SharpDevelop.
 * User: christiank
 * Date: 20/9/2011
 * Time: 10:02 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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
            
            switch (AVisualStyle) {
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
