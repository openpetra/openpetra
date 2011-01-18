//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using Ict.Common;

namespace Ict.Petra.Client.App.Gui
{
#if TESTMODE
    /// <summary>
    /// Provides a test framework for WinForms, and can be controlled by an xml file.
    /// This is not working yet.
    /// This class is only used if compiled with directive TESTMODE.
    /// </summary>
    public class TTestWinForm
    {
        /// <summary>
        ///  the form that should be tested
        /// </summary>
        private Form FTestForm = null;

        /// <summary>
        /// time in the future when to close the form that is being tested.
        /// </summary>
        private DateTime FDisconnectTime;

        /// <summary>
        /// filename of the xml file with the instructions
        /// </summary>
        private string FTestingFile;

        /// <summary>
        /// controlling thread that is running the tests on the form
        /// </summary>
        private Thread FTestThread;

        /// <summary>
        /// returns the form that should be tested
        /// </summary>
        public Form TestForm
        {
            get
            {
                return FTestForm;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ATestingFile">the xml file with the instructions for the automatic test</param>
        /// <param name="ADisconnectTime">time as a given point in the future when to close the form</param>
        public TTestWinForm(string ATestingFile, DateTime ADisconnectTime)
        {
            FTestingFile = ATestingFile;
            FDisconnectTime = ADisconnectTime;
        }

        /// <summary>
        /// this will create a thread and run the test
        /// </summary>
        public void Start(Form ATestForm)
        {
            FTestForm = ATestForm;
            FTestThread = new Thread(this.Run);
            FTestThread.Start();
        }

        /// <summary>
        /// to be called by a thread
        /// </summary>
        private void Run()
        {
            // we estimate 20 seconds for disconnecting and closing the client; to avoid UserDefaults issue
            const Int32 TIME_REQUIRED_FOR_LOGOUT = 20000;

            // load the testing script file
            System.Type t = FTestForm.GetType();

            if (t == null)
            {
                MessageBox.Show("no type");
            }

            /*
             * // Open Finance screen (to start AP client, third connection to DB)
             * // careful with several ledgers
             * MethodInfo mi = t.GetMethod("btnAccounts_Click", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
             * if (mi == null)
             * {
             *  // error: cannot find method
             *  TLogging.Log("cannot find method " + "btnAccounts_Click");
             * }
             * else
             * {
             *  try {
             *      // button click
             *      Object[] args = new Object[2];
             *      mi.Invoke(FTestForm, args);
             *      args[0] = FTestForm;
             *      args[1] = null;
             *  }
             *  catch (Exception e)
             *  {
             *      TLogging.Log("Test failed with Exception: " + e.Message);
             *      TLogging.Log("details: " + e.ToString());
             *  }
             * }
             */

            Form subform = RunOnScreen("editpartner", "29064546");
            Int32 breakTime = Convert.ToInt32((FDisconnectTime - DateTime.Now).TotalMilliseconds - TIME_REQUIRED_FOR_LOGOUT);

            if (breakTime > 0)
            {
                Thread.Sleep(breakTime);
            }

            if (subform != null)
            {
                subform.Close();
            }

            // PrepareForClose();
            // TODO: signal the multiPetraStart program about client finished?
            FTestForm.Close();
        }

        /// <summary>
        /// this has to be implemented yet; it will run a certain command on the screen
        /// </summary>
        /// <param name="ACmd">todoComment</param>
        /// <param name="AParameter">todoComment</param>
        /// <returns>todoComment</returns>
        public Form RunOnScreen(String ACmd, String AParameter)
        {
            // empty implementation
            return null;
        }

        /// <summary>
        /// this has to be implemented yet; it should prepare the window for closing
        /// </summary>
        public void PrepareForClose()
        {
            // empty implementation
        }
    }
#endif
}