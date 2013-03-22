//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections;
using System.Windows.Forms;

namespace Ict.Common.Controls
{
    /// <summary>
    /// General utility functions for ICT applications that work with Controls.
    /// </summary>
    public class ControlsUtilities
    {
        /// <summary>
        /// Add another TabPage next to the given Tabpage
        /// </summary>
        /// <param name="ATabControl">parent control containing the tabpages</param>
        /// <param name="AAddTabPage">the new tabpage</param>
        /// <param name="ANextToTabPage">the neighbour</param>
        public static void AddTabNextToTab(TabControl ATabControl, TabPage AAddTabPage, String ANextToTabPage)
        {
            int ExchangeCounter;
            int RemainingExchangeCounter1;
            int RemainingExchangeCounter2;

            TabPage[, ] FollowingTabPages = null;

            if (AAddTabPage != null)
            {
                /*
                 * First add specified TabPage
                 */
                if (!ATabControl.Controls.Contains(AAddTabPage))
                {
                    ATabControl.Controls.Add(AAddTabPage);
                }
                else
                {
                    return;
                }

                /*
                 * Now place it right of the desired TabPage and re-order the following Tabpages...
                 */

                // Determine TabPages that need to change order
                for (ExchangeCounter = 0; ExchangeCounter <= ATabControl.TabCount - 1; ExchangeCounter += 1)
                {
                    // MessageBox.Show('ExchangeCounter: ' + ExchangeCounter.ToString);
                    if (ATabControl.TabPages[ExchangeCounter].Name == ANextToTabPage)
                    {
                        if (ExchangeCounter < ATabControl.TabCount)
                        {
                            FollowingTabPages = new TabPage[ATabControl.TabCount - (ExchangeCounter - 1), 2];

                            // MessageBox.Show('Length(FollowingTabPages): ' + Convert.ToString(ATabControl.TabCount  ExchangeCounter  1));
                            FollowingTabPages[0, 0] = AAddTabPage;
                            FollowingTabPages[0, 1] = ATabControl.TabPages[ExchangeCounter + 1];

                            // MessageBox.Show('FollowingTabPages[0]: ' + FollowingTabPages[0, 0].Name + '; ' + FollowingTabPages[0, 1].Name);
                            // MessageBox.Show('Adding ' + Convert.ToString((ATabControl.TabCount  1)  (ExchangeCounter + 2)) + ' further TabPages...');
                            for (RemainingExchangeCounter1 = 1;
                                 RemainingExchangeCounter1 <= (ATabControl.TabCount - 1) - (ExchangeCounter + 2);
                                 RemainingExchangeCounter1 += 1)
                            {
                                FollowingTabPages[RemainingExchangeCounter1, 0] = ATabControl.TabPages[RemainingExchangeCounter1 + ExchangeCounter];
                                FollowingTabPages[RemainingExchangeCounter1,
                                                  1] = ATabControl.TabPages[RemainingExchangeCounter1 + ExchangeCounter + 1];

                                // MessageBox.Show('FollowingTabPages[' + RemainingExchangeCounter1.ToString + ']: ' + FollowingTabPages[RemainingExchangeCounter1, 0].Name + '; ' + FollowingTabPages[RemainingExchangeCounter1, 1].Name);
                            }

                            break;
                        }
                    }
                }

                // Exchange positions of determined TabPages
                ATabControl.SuspendLayout();

                for (RemainingExchangeCounter2 = 0;
                     RemainingExchangeCounter2 <= (ATabControl.TabCount - 1) - (ExchangeCounter + 2);
                     RemainingExchangeCounter2 += 1)
                {
                    // MessageBox.Show('TabPage swap #' + RemainingExchangeCounter2.ToString + '...');
                    SwapTabPages(ATabControl, FollowingTabPages[RemainingExchangeCounter2, 0], FollowingTabPages[RemainingExchangeCounter2, 1]);
                }

                ATabControl.ResumeLayout();
            }
        }

        /// <summary>
        /// hide a specified group of tab pages
        /// </summary>
        /// <param name="ATabControl">parent control</param>
        /// <param name="ATabsToHide">pages to hide</param>
        public static void HideTabs(TabControl ATabControl, ArrayList ATabsToHide)
        {
            Int16 Counter;
            IEnumerator TabPagesEnumerator;

            // TODO 1 oChristianK cReUse : Move this procedure to Ict.Petra.Client.App.Gui!
            // MessageBox.Show('ATabsToHide.Count: ' + ATabsToHide.Count.ToString);
            for (Counter = 0; Counter <= ATabsToHide.Count - 1; Counter += 1)
            {
                TabPagesEnumerator = ATabControl.TabPages.GetEnumerator();

                while (TabPagesEnumerator.MoveNext())
                {
                    // MessageBox.Show('(TabPagesEnumerator.Current as TabPage).Name: ' + (TabPagesEnumerator.Current as TabPage).Name + '; ATabsToHide[' + Counter.ToString + ']: ' + ATabsToHide[Counter].ToString);
                    if (((TabPage)TabPagesEnumerator.Current).Name == ATabsToHide[Counter].ToString())
                    {
                        // MessageBox.Show('Tab ''' + (TabPagesEnumerator.Current as TabPage).Name + ''' gets removed.');
                        ATabControl.TabPages.Remove(((TabPage)TabPagesEnumerator.Current));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// swap the order of 2 tabbed pages
        /// </summary>
        /// <param name="ATabControl">parent control</param>
        /// <param name="ATabPage1">first tab page</param>
        /// <param name="ATabPage2">second tab page</param>
        public static void SwapTabPages(TabControl ATabControl, TabPage ATabPage1, TabPage ATabPage2)
        {
            Int16 Index1;
            Int16 Index2;

            Index1 = (short)ATabControl.TabPages.IndexOf(ATabPage1);
            Index2 = (short)ATabControl.TabPages.IndexOf(ATabPage2);
            ATabControl.TabPages[Index1] = ATabPage2;
            ATabControl.TabPages[Index2] = ATabPage1;
        }

        /// <summary>
        /// swap 2 tabbed pages, specified by their index
        /// </summary>
        /// <param name="ATabControl">parent control</param>
        /// <param name="ATabPageIndex1">index of first tabbed page</param>
        /// <param name="ATabPageIndex2">index of second tabbed page</param>
        public static void SwapTabPages(TabControl ATabControl, Int32 ATabPageIndex1, Int32 ATabPageIndex2)
        {
            TabPage TabPage1;
            TabPage TabPage2;

            if (ATabPageIndex1 < 0)
            {
                return;
            }
            TabPage1 = ATabControl.TabPages[ATabPageIndex1];
            TabPage2 = ATabControl.TabPages[ATabPageIndex2];
            ATabControl.TabPages[ATabPageIndex1] = TabPage2;
            ATabControl.TabPages[ATabPageIndex2] = TabPage1;
        }
    }
}