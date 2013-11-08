//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, ChristianK
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
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Provides a test framework for WinForms, and can be controlled by an xml file.
    /// These tests are to be driven by the PetraMultiStart program.
    /// </summary>
    /// <remarks>
    /// Possible future extension: make tests and their parameters controllable
    /// through an XML file.
    /// </remarks>
    public class TTestWinForm
    {
        /// <summary>Parameter Separator Character</summary>
        protected const char PARAM_SEPARATOR = ';';
        
        /// <summary>Partner Key Parameter prefix</summary>
        protected const string PARTNERKEY_PARAM = "PartnerKey";
        
        /// <summary>Ledger Number Parameter prefix</summary>
        protected const string LEDGERNUMBER_PARAM = "LedgerNumber";


        /// <summary>
        /// The Form that will host the test.
        /// </summary>
        private Form FTestForm = null;

        private TGuiTestingTelltaleWinForm FTellTaleForm = null;

        /// <summary>
        /// The Type of the Form that will host the test.
        /// </summary>
        private System.Type FTestFormType = null;

        /// <summary>
        /// Time in the future when to close the Form that will host the test.
        /// </summary>
        private DateTime FDisconnectTime;

        /// <summary>
        /// Filename of the xml file with the instructions.
        /// </summary>
        private string FTestingFile;

        /// <summary>
        /// Parameters for the Test (optional).
        /// </summary>
        private string FTestParameters;

        /// <summary>
        /// Controlling thread that is running the tests on the Form (if Start, not Run is called).
        /// </summary>
        private Thread FTestThread;


        #region Properties

        /// <summary>The Form that will host the test.</summary>
        public Form TestForm
        {
            get
            {
                return FTestForm;
            }

            set
            {
                FTestForm = value;
            }
        }


        /// <summary>The Type of the Form that will host the test.</summary>
        public Type TestFormType
        {
            get
            {
                return FTestFormType;
            }

            set
            {
                FTestFormType = value;
            }
        }

        /// <summary>
        /// Instance of the 'Testing Telltale Form'. 
        /// </summary>
        public TGuiTestingTelltaleWinForm TellTaleForm
        {
            get
            {
                return FTellTaleForm;
            }
        }

        /// <summary>Time in the future when to close the Form that will host the test.</summary>
        public DateTime DisconnectTime
        {
            get
            {
                return FDisconnectTime;
            }

            set
            {
                FDisconnectTime = value;
            }
        }

        #endregion


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ATestingFile">the xml file with the instructions for the automatic test</param>
        /// <param name="ATestingParameters">Parameters for the Test (optional, pass in empty string if no parameters).</param>
        /// <param name="ADisconnectTime">time as a given point in the future when to close the form</param>
        public TTestWinForm(string ATestingFile, string ATestingParameters, DateTime ADisconnectTime)
        {
            FTestingFile = ATestingFile;
            FTestParameters = ATestingParameters;
            FDisconnectTime = ADisconnectTime;

            FTellTaleForm = new TGuiTestingTelltaleWinForm();
        }

        /// <summary>
        /// this will create a thread and run the test
        /// </summary>
        public void Start(Form ATestForm)
        {
            FTestForm = ATestForm;
            FTestThread = new Thread(this.Run);
            FTestThread.SetApartmentState(ApartmentState.STA);
            FTestThread.Start();
        }

        /// <summary>
        /// Runs a Test.
        /// </summary>
        public void Run()
        {
//MessageBox.Show("Entering TTestWinForm.Run...");

            const Int32 TIME_REQUIRED_FOR_LOGOUT = 20000;  // we estimate 20 seconds for disconnecting and closing the client; to avoid UserDefaults issue
            const string REPEAT_PARAM = "repeat";

            string TestParameters;
            int NumberOfTests;
            int RepeatsParamPos;
            int RepeatsParamLength;


            FTellTaleForm.Owner = FTestForm;
            FTellTaleForm.TestingFile = FTestingFile;
            FTellTaleForm.DisconnectTime = FDisconnectTime;
            FTellTaleForm.Show();

            // Possible future extension: make tests and their parameters controllable
            // through an XML file, which would need to be loaded here.

            FTestFormType = FTestForm.GetType();

            if (FTestFormType == null)
            {
                MessageBox.Show("no type");
            }

            TLogging.Log("Start of Test Series at " + DateTime.Now.ToString());


            if (TestSetup())
            {                
                // Possible future extension: make tests and their parameters controllable
                // through an XML file, which would need to be evaluated here (number of tests).
    
                if (FTestParameters != String.Empty)
                {
                    TestParameters = FTestParameters;
                }
                else
                {
                    TestParameters = "29064546";  // just use a hardcoded PartnerKey...
                }
    
                RepeatsParamPos = TestParameters.IndexOf(REPEAT_PARAM);
    
                if (RepeatsParamPos != -1)
                {
                    RepeatsParamLength = TestParameters.IndexOf(PARAM_SEPARATOR, RepeatsParamPos + REPEAT_PARAM.Length) - (RepeatsParamPos + REPEAT_PARAM.Length) - 1;
                    
                    if (RepeatsParamLength < 0)
                    {
                        // No PARAM_SEPARATOR found, so assume rest of TestParameters is the REPEAT_PARAM value
                        RepeatsParamLength = TestParameters.Length - (RepeatsParamPos + REPEAT_PARAM.Length + 1);
                    }
                    
//MessageBox.Show("Parsed 'repeats' parameter: " + "Startpos.: " + RepeatsParamPos.ToString() + "; Contents: " +
//    TestParameters.Substring(RepeatsParamPos + REPEAT_PARAM.Length + 1, RepeatsParamLength));
    
                    NumberOfTests = Convert.ToInt32(
                        TestParameters.Substring(RepeatsParamPos + REPEAT_PARAM.Length + 1, RepeatsParamLength));
    
//MessageBox.Show("NumberOfTests: " + NumberOfTests.ToString());
                }
                else
                {
                    NumberOfTests = 1;
                    FTellTaleForm.Repeats = "N/A";
                }
//MessageBox.Show("BreakTime: " + FDisconnectTime.ToString());    
                // Find out when to stop the Test (or Test Series if NumberOfTests = 1)
                Int32 breakTime =
                    Convert.ToInt32((FDisconnectTime -
                                     DateTime.Now).TotalMilliseconds /
                        NumberOfTests) - (TIME_REQUIRED_FOR_LOGOUT / NumberOfTests) - ((TIME_REQUIRED_FOR_LOGOUT / 4) / NumberOfTests);
    
//MessageBox.Show("BreakTime: " + breakTime.ToString());
    
                for (int Counter = 1; Counter <= NumberOfTests; Counter++)
                {
    
                    if (NumberOfTests > 1)
                    {
                        FTellTaleForm.Repeats = Counter.ToString() + " of " + NumberOfTests.ToString();
                    }
    
            /*
                     * Open specified Form
                     *
                     * Possible future extension: make tests and their parameters controllable
                     * through an XML file, which would need to be evaluated here
                     * (which Forms to open and their parameters, if any).
             */
                    Form subform = RunOnScreen(FTestingFile, TestParameters);
    
                    // Use a loop to prevent blocking the Thread (and therefore potentially the GUI) that the Tests are running on
                    // while 'sleeping' before the Test ends.
            if (breakTime > 0)
            {
                        for (int SleepCounter = 0; SleepCounter < ((breakTime / 1000) * 2); SleepCounter++)
                        {
                            Thread.Sleep(500);
                            Application.DoEvents();
                        }
            }

            if (subform != null)
            {
                subform.Close();
            }
                    else
                    {
                        // Force closing of all windows besides self
                        CloseAllScreens();
                    }
    
                    TLogging.Log("Finished Test " + Counter.ToString() + " at " + DateTime.Now.ToString());
                }
    
                TLogging.Log("Finished Test Series at " + DateTime.Now.ToString());
    
    
                TestTeardown();
            }
            else
            {
                TLogging.Log("Setting-up of Test Series FAILED at " + DateTime.Now.ToString());
                TLogging.Log("Finished FAILED Test Series at " + DateTime.Now.ToString());                
            }

            // TODO: signal the multiPetraStart program about client finished?
            FTestForm.Close();
        }

        /// <summary>
        /// This has to be implemented yet; it will create a connection to the PetraServer
        /// </summary>
        /// <description>Note: can be implemented differently by each inheritor!</description>
        public virtual bool TestSetup()
        {
            // empty implementation
            return true;
        }

        /// <summary>
        /// Runs a certain command on the screen.
        /// </summary>
        /// <param name="ACmd">The command determines what will happen on screen.</param>
        /// <param name="AParameter">Optional parameters for running the command.</param>
        /// <returns>Depending on the implementation of the <see cref="OpenPartnerEditScreen" />
        /// and <see cref="OpenPartnerFindScreen" /> Methods, a Form reference might be returned,
        /// or not.</returns>
        public Form RunOnScreen(String ACmd, String AParameter)
        {
            Form ReturnValue;

            ReturnValue = null;

            TClientSettings.AutoTestParameters = AParameter;

//MessageBox.Show("Entering TTestWinForm.RunOnScreen. ACmd=" + ACmd);

            if (ACmd == "PartnerEdit.testing")
            {
                OpenPartnerEditScreen(AParameter);
            }
            else if (ACmd == "PartnerFind.testing")
            {
                OpenPartnerFindScreen(AParameter);
            }
            else if (ACmd == "GLBatch.testing")
            {
                OpenGLBatchScreen(AParameter);
            }

            return ReturnValue;
        }

        /// <summary>
        /// This has to be implemented yet; it will close all Petra Screens, except for the Testing Screen
        /// </summary>
        /// <description>Note: can be implemented differently by each inheritor!</description>
        public virtual void CloseAllScreens()
        {
            // empty implementation
        }

        /// <summary>
        /// This has to be implemented yet; it should prepare the end of the test.
        /// </summary>
        /// <description>Note: can be implemented differently by each inheritor!</description>
        public virtual void TestTeardown()
        {
            // empty implementation
        }

        /// <summary>
        /// This has to be implemented yet; it should open the Partner Edit screen.
        /// </summary>
        /// <description>Note: can be implemented differently by each inheritor (eg. call the Form through .NET or Applink4GL)!</description>
        public virtual void OpenPartnerEditScreen(String AParameter)
        {
            // empty implementation
        }

        /// <summary>
        /// This has to be implemented yet; it should open the Partner Edit screen.
        /// </summary>
        /// <description>Note: can be implemented differently by each inheritor (eg. call the Form through .NET or Applink4GL)!</description>
        public virtual void OpenPartnerFindScreen(String AParameter)
        {
            // empty implementation
        }
        
        /// <summary>
        /// This has to be implemented yet; it should open the GL Batch screen.
        /// </summary>
        /// <param name="AParameter">Currently ignored.</param>
        public virtual void OpenGLBatchScreen(String AParameter)
        {           
            // empty implementation
        }        
    }
}