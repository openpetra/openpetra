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

namespace Ict.Petra.Client.CommonForms.Logic
{
	[TestFixture]
	public class Test1
	{
		[Test]
		public void TestTPetraShepherdPagesListConstructor()
		{	
			System.Console.WriteLine("Beginning tests of the Constructor for the TPetraShepherdPagesList constructor.. "); 
			TPetraShepherdPagesList testPetraShepherdPagesList = new TPetraShepherdPagesList("ShepherdChurch.yaml"); 
			System.Console.WriteLine("Testing to see if the count of the pages in the ShepherdPage is correct.. "); 
			Assert.True(testPetraShepherdPagesList.Pages.Count == 3);
			
			System.Console.Write("Testing to see the YAML contains all of the keys it should.. "); 
			Assert.True(testPetraShepherdPagesList.Pages.ContainsKey("5"));
			Assert.True(testPetraShepherdPagesList.Pages.ContainsKey("56"));
			Assert.True(testPetraShepherdPagesList.Pages.ContainsKey("12")); 
			
			System.Console.WriteLine("Testing to make sure that the parser is not grabbing inappropriate keys.. ");
			Assert.False(testPetraShepherdPagesList.Pages.ContainsKey("123"));
			Assert.False(testPetraShepherdPagesList.Pages.ContainsKey("1532"));
			Assert.False(testPetraShepherdPagesList.Pages.ContainsKey("00"));
			
			System.Console.WriteLine("Testing to make sure that the parser is parsing individual pages correctly.. ");
			foreach(KeyValuePair<string, TPetraShepherdPage> pair in testPetraShepherdPagesList.Pages)
    		{
				switch(pair.Key)
				{
					case "5":
						Assert.AreEqual(pair.Value.ID, "5"); 
						Assert.AreEqual(pair.Value.Title, "Personnel Shepherd"); 
						Assert.AreEqual(pair.Value.Note,"This is a test, this is only a test."); 
						Assert.True(pair.Value.Visible); 
						Assert.True(pair.Value.Enabled); 
						Assert.AreEqual(pair.Value.UserControlNamespace,"Ict.Petra.Client.MPartner.Gui");
						Assert.AreEqual(pair.Value.UserControlClassName,"TUC_PartnerDetails_Church");
						Assert.AreEqual(pair.Value.HelpContext, "HELPME!!!");
						break;
				}
			}
		}
		
		[Test]
		public void TestPetraShepherdFormLogicHandleActionNext()
		{
			System.Console.WriteLine("Beginning the tests of the HandleActionNext() button.. "); 
			IPetraShepherdConcreteFormInterface testInterface = null; 
			TPetraShepherdFormLogic testFormLogic = new TPetraShepherdFormLogic("ShepherdChurch.yaml", testInterface); 
			System.Console.WriteLine(testFormLogic.CurrentPage.ID); 
			
			Assert.AreEqual(testFormLogic.CurrentPage.ID, "5"); 
		}
	}
}
