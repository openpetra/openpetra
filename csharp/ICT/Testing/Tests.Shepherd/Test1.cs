/*
 * Created by SharpDevelop.
 * User: Paul
 * Date: 2/18/2011
 * Time: 9:37 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using Ict.Common;
using Ict.Common.Data;
using System.Xml; 
using System.Collections.Generic; 
using Ict.Petra.Client.MPartner.Gui; 
using Tests.Shepherd; 

namespace Ict.Petra.Client.CommonForms.Logic
{
	[TestFixture]
	public class Test1
	{
		public TPetraShepherdFormLogic LogicStarter() {
			Tests.Shepherd.TestInterface testShephredLogic = new Tests.Shepherd.TestInterface(); 
			TPetraShepherdFormLogic testFormLogicGeneric = new TPetraShepherdFormLogic("ShepherdChurch.yaml", testShephredLogic); 
			return testFormLogicGeneric;
		}
		[Test]
		public void TestTPetraShepherdPagesListConstructor()
		{	
			TPetraShepherdPagesList testPetraShepherdPagesList = new TPetraShepherdPagesList("ShepherdChurch.yaml");
			Assert.True(testPetraShepherdPagesList.Pages.Count == 3, "Wrong shepherd page count.");
	
			Assert.True(testPetraShepherdPagesList.Pages.ContainsKey("5"), "Shepherd did not contain key 5");
			Assert.True(testPetraShepherdPagesList.Pages.ContainsKey("56"), "Shepherd did not contain key 56");
			Assert.True(testPetraShepherdPagesList.Pages.ContainsKey("12"), "Shepherd did not contain key 12"); 
			
			Assert.False(testPetraShepherdPagesList.Pages.ContainsKey("123"), "Shepherd inadvertently contained key 123");
			Assert.False(testPetraShepherdPagesList.Pages.ContainsKey("1532"), "Shepherd inadvertently contained key 1532");
			Assert.False(testPetraShepherdPagesList.Pages.ContainsKey("00"), "Shepherd inadvertently contained key 00");
			
			foreach(KeyValuePair<string, TPetraShepherdPage> pair in testPetraShepherdPagesList.Pages)
    		{
				switch(pair.Key)
				{
					case "5":
						Assert.AreEqual(pair.Value.ID, "5", "The shepherd page 5 did not have the key 5"); 
						Assert.AreEqual(pair.Value.Title, "Personnel Shepherd", "The shepherd page 5 did not have the name Personnel Shepherd"); 
						Assert.AreEqual(pair.Value.Note,"This is a test, this is only a test.", "The note did not match: This is a test, this ois only a test."); 
						Assert.True(pair.Value.Visible, "The page was not visible.");
						Assert.True(pair.Value.Enabled, "The page was not enabled");
						Assert.AreEqual(pair.Value.UserControlNamespace,"Ict.Petra.Client.MPartner.Gui", "The namespace was not: Ict.Petra.Client.MPartner.Gui");
						Assert.AreEqual(pair.Value.UserControlClassName,"TUC_PartnerDetails_Church", "The user control class name was not: TUC_PartnerDetails_Church");
						Assert.AreEqual(pair.Value.HelpContext, "HELPME!!!", "The help context was not: HELPME!!!");
						break;
				}
			}
		}


		[Test]
		public void TestPetraShepherdFormLogicHandleActionNext()
		{
			
			
			Tests.Shepherd.TestInterface testShepherdLogic = new Tests.Shepherd.TestInterface(); 
			TPetraShepherdFormLogic testFormLogic = LogicStarter();
			
			//TESTS to show that the HandleActionNext() method moves from page to page under normal operating circumstances..
			
			Assert.AreEqual(testFormLogic.CurrentPage.ID, "5","The current page ID was not 5"); 
			testFormLogic.HandleActionNext(); 
			Assert.AreEqual(testFormLogic.CurrentPage.ID,"56","The current page ID was not 56"); 
			testFormLogic.HandleActionNext(); 
			Assert.AreEqual(testFormLogic.CurrentPage.ID,"12", "The current page ID was not 12"); 
			
			
			testFormLogic.HandleActionNext(); 
			Assert.AreEqual(testFormLogic.CurrentPage.ID,"12", "The current page ID was not 12"); 
		}
		
		[Test]
		public void TestPetraShepherdFormHandleActionNextOnInvisible()
		{
			//TESTS to show that the HandleActionNext() method moves from page to page when there is a Page that is not visible
			Tests.Shepherd.TestInterface testShepherdLogic = new Tests.Shepherd.TestInterface(); 
			TPetraShepherdFormLogic testNotVisibleLogic = new TPetraShepherdFormLogic("ShepherdChurch.yaml", testShepherdLogic);
			
			foreach(KeyValuePair<string, TPetraShepherdPage> pair in testNotVisibleLogic.ShepherdPages.Pages)
    		{
				switch(pair.Key)
				{
					case "56":
						pair.Value.Visible = false; 
						break;
				}
			}
			
			
			Assert.True(testNotVisibleLogic.CurrentPage.Visible);
			testNotVisibleLogic.HandleActionNext(); 
			Assert.AreEqual(testNotVisibleLogic.CurrentPage.ID,"12", "The current page I is not 12"); 
		}
		
		[Test]
		public void TestPetraShepherdFormHandleActionNextUnEnabledPage()
		{

			//TESTS to show that the HandleActionNext() method moves from page to page when there is a Page that is not enabled
			
			TPetraShepherdFormLogic testNotEnabledLogic = LogicStarter();
			
			foreach(KeyValuePair<string, TPetraShepherdPage> pair in testNotEnabledLogic.ShepherdPages.Pages)
    		{
				switch(pair.Key)
				{
					case "56":
						pair.Value.Enabled = false; 
						break;
				}
			}
			
			
			Assert.True(testNotEnabledLogic.CurrentPage.Enabled);
			
			
			testNotEnabledLogic.HandleActionNext(); 
			Assert.AreEqual(testNotEnabledLogic.CurrentPage.ID,"12"); 	
		}
		
		[Test]
		public void TestPetraShepherdFormHandleActionNextUnEnableOrNotVisible()
		{
			
			//TESTS to show that the HandleActionNext() method moves from page to page when there is a Page that is not visible nor enabled
			
			TPetraShepherdFormLogic testNotVisibleOrEnabledLogic = LogicStarter();
			
			foreach(KeyValuePair<string, TPetraShepherdPage> pair in testNotVisibleOrEnabledLogic.ShepherdPages.Pages)
    		{
				switch(pair.Key)
				{
					case "56":
						pair.Value.Visible = false; 
						pair.Value.Enabled = false; 
						break;
				}
			}
			
			
			Assert.True(testNotVisibleOrEnabledLogic.CurrentPage.Visible);
			
			
			testNotVisibleOrEnabledLogic.HandleActionNext(); 
			Assert.AreEqual(testNotVisibleOrEnabledLogic.CurrentPage.ID,"12", "The current page I was not 12"); 

		}		
/*		
		[Test]
		public void TestPetraShepherdFormLogicSwitchToStartPage()
		{
			Tests.Shepherd.TestInterface testShepherdLogic = new Tests.Shepherd.TestInterface(); 
			TPetraShepherdFormLogic testFormLogicSwitchToStartPage = new TPetraShepherdFormLogic("ShepherdChurch.yaml", testShepherdLogic);
			
			Assert.AreEqual(testFormLogicSwitchToStartPage.CurrentPage, "5"); 
			testFormLogicSwitchToStartPage.HandleActionNext(); 
			testFormLogicSwitchToStartPage; 
		}
		*/
		[Test]
		public void TestPetraShepherdFormLOgicSwitchToBackPage() 
		{
			
			TPetraShepherdFormLogic testFormLogicBackButton =  LogicStarter();
			Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "5", "The first page of the shephred was not 5, as expected."); 
			testFormLogicBackButton.HandleActionNext(); 
			Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "56", "The second page of the shepherd was not 56, as expected."); 
			testFormLogicBackButton.HandleActionBack(); 
			Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "5", "The shepherd should have jumped back to page 5, but didn't."); 
			testFormLogicBackButton.HandleActionNext(); 
			testFormLogicBackButton.HandleActionNext(); 
			testFormLogicBackButton.HandleActionBack(); 
			Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "56", "The shepherd should have jumped back to page 56, but didn't."); 
			testFormLogicBackButton.HandleActionBack(); 
			testFormLogicBackButton.HandleActionBack();
			Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "5", "The shepherd should not have jumped farther back than 5."); 
		}
		
		
	}
	
}
