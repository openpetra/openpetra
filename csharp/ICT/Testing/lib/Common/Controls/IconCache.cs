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
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;

using Ict.Common;
using Ict.Common.Controls;

namespace Tests.Common.Controls
{
    /// Testing the Icon Cache Class.
    [TestFixture]
    public class TTestIconCache
    {
        const string DEMOICON1_PATH = "../../resources/partner_multires.ico";
        const string DEMOICON2_PATH = "../../resources/Print.ico";

        /// <summary>
        /// Test initialisation.
        /// </summary>
        [SetUp]
        public void Init()
        {
            Catalog.Init();
            new TLogging("../../log/TestCommonControls.log");
        }

        /// <summary>
        /// Testing the Icon Cache Class.
        /// </summary>
        [Test]
        public void TestIconCache()
        {
            Image TestImage;

            new TIconCache();


            // Icon #1 (each icon is loaded specific with the size)
            Assert.IsFalse(TIconCache.IconCache.ContainsIcon(DEMOICON1_PATH, TIconCache.TIconSize.is32by32),
                "IconCache: Must not contain Icon " + DEMOICON1_PATH);

            TestImage = TIconCache.IconCache.AddOrGetExistingIcon(
                DEMOICON1_PATH, TIconCache.TIconSize.is32by32);

            Assert.IsFalse(TIconCache.IconCache.LastIconRequestedWasReturnedFromCache,
                "IconCache: Icon must not have been returned from Cache: " + DEMOICON1_PATH);

            Assert.IsNotNull(TestImage, "IconCache: TestImage must be returned (#1)");

            Assert.IsTrue((TestImage.Size.Width == 32)
                && (TestImage.Size.Height == 32), "TestImage must be 32x32 Pixels");

            Assert.IsTrue(TIconCache.IconCache.ContainsIcon(DEMOICON1_PATH, TIconCache.TIconSize.is32by32),
                "IconCache: Must contain Icon " + DEMOICON1_PATH);

            TestImage = TIconCache.IconCache.AddOrGetExistingIcon(
                DEMOICON1_PATH, TIconCache.TIconSize.is16by16);

            Assert.IsFalse(TIconCache.IconCache.LastIconRequestedWasReturnedFromCache,
                "IconCache: Icon must not have been returned from Cache: " + DEMOICON1_PATH);

            Assert.IsTrue((TestImage.Size.Width == 16)
                && (TestImage.Size.Height == 16), "TestImage must be 16x16 Pixels");


            // Icon #2 (contains only 16x16 pixel icon)
            Assert.IsFalse(TIconCache.IconCache.ContainsIcon(DEMOICON2_PATH, TIconCache.TIconSize.is16by16),
                "IconCache: Must not contain Icon " + DEMOICON2_PATH);

            TIconCache.IconCache.AddIcon(DEMOICON2_PATH, TIconCache.TIconSize.is32by32);

            Assert.IsTrue(TIconCache.IconCache.ContainsIcon(DEMOICON2_PATH,
                    TIconCache.TIconSize.is32by32), "IconCache: Must contain Icon " + DEMOICON2_PATH);

            TestImage = TIconCache.IconCache.GetIcon(DEMOICON2_PATH, TIconCache.TIconSize.is32by32);

            Assert.IsTrue(TIconCache.IconCache.LastIconRequestedWasReturnedFromCache,
                "IconCache: Icon must have been returned from Cache: " + DEMOICON2_PATH);

            Assert.IsNotNull(TestImage, "IconCache: TestImage must be returned (#2)");

            // Check fallback: we requested a 32x32 pixel icon, but this is not available - therefore the closest match
            // (16x16 pixels icon) must be returned
            Assert.IsTrue((TestImage.Size.Width == 16)
                && (TestImage.Size.Height == 16), "Check fallback: TestImage must be 16x16 Pixels");
        }

        /// <summary>
        /// Testing the Icon Cache Class - trowing of EIconNotInCacheException.
        /// </summary>
        [Test]
        public void TryRetrievingIconThatsNotInCache()
        {
            Assert.Throws<EIconNotInCacheException>(() => TIconCache.IconCache.GetIcon("not in existance...!", TIconCache.TIconSize.is32by32));
        }
    }
}
