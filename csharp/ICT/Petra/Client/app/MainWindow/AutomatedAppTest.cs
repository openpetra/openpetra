//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Shared.MPartner;


namespace PetraClient_AutomatedAppTest
{
/// <summary>
/// Performs automatic application tests. These tests are to be driven by the PetraMultiStart program.
/// </summary>
public class TAutomatedAppTest : TTestWinForm
{
    private String FUserName;
    private Int64 FClientID;

    #region Properties

    /// <summary>
    /// Client ID.
    /// </summary>
    public long ClientID
    {
        get
        {
            return FClientID;
        }
        set
        {
            FClientID = value;
            TellTaleForm.Client = FUserName + " [" + FClientID.ToString() + "]";
        }
    }

    #endregion

    /// <summary>
    /// Constructor. Just calls the base constructor with the same Arguments.
    /// </summary>
    /// <param name="ATestingFile">the xml file with the instructions for the automatic test</param>
    /// <param name="ATestingParameters">Parameters for the Test (optional, pass in empty string if no parameters).</param>
    /// <param name="ADisconnectTime">time as a given point in the future when to close the form</param>
    /// <param name="AUserName">User Name which will be used to connect to the PetraServer.</param>
    public TAutomatedAppTest(string ATestingFile, string ATestingParameters, DateTime ADisconnectTime, String AUserName) : base(ATestingFile,
                                                                                                                               ATestingParameters,
                                                                                                                               ADisconnectTime)
    {
        FUserName = AUserName;

//            MessageBox.Show(String.Format(
//                "TAutomatedAppTest Constructor:  ATestingFile = {0}; ADisconnectTime = {1}; FUserName = {2}",
//                ATestingFile, ADisconnectTime, FUserName));
    }

    /// <summary>
    /// From the base class: 'This has to be implemented yet; it will create a connection to the PetraServer'
    /// </summary>
    public override bool TestSetup()
    {
        // PetraClient is already connected to PetraServer, so we can just return true...
        return true;
    }

    /// <summary>
    /// Prepares the PetraClient for closing. Gets executed at the end of all tests.
    /// </summary>
    public override void TestTeardown()
    {
//            TCmdMFinance cmd;
//
//            // Close Finance Screen (if open at all)
//            cmd = new TCmdMFinance();
//            cmd.CloseScreenMFinanceMain(TestForm);

        // Force closing of all windows besides self
        TFormsList.GFormsList.CloseAllExceptOne(TestForm);
    }

    /// <summary>
    /// Closes all Petra Screens, except for the Testing Screen.
    /// </summary>
    public override void CloseAllScreens()
    {
        // Force closing of all windows besides self
        TFormsList.GFormsList.CloseAllExceptOne(TestForm);
    }

    /// <summary>
    /// Opens a Partner Edit screen.
    /// </summary>
    /// <param name="AParameter">PartnerKey (as String!).</param>
    public override void OpenPartnerEditScreen(String AParameter)
    {
        int PartnerKeyParamPos;
        int NextParameterStart;
        int PartnerKeyLength;
        Int64 PartnerKey;

        PartnerKeyParamPos = AParameter.IndexOf(TTestWinForm.PARTNERKEY_PARAM);

        if (PartnerKeyParamPos != -1)
        {
            NextParameterStart = AParameter.IndexOf(TTestWinForm.PARAM_SEPARATOR, PartnerKeyParamPos +
                TTestWinForm.PARTNERKEY_PARAM.Length + 1);

            if (NextParameterStart == -1)
            {
                PartnerKeyLength = AParameter.Length - PartnerKeyParamPos - TTestWinForm.PARTNERKEY_PARAM.Length - 1;
//                    MessageBox.Show("PartnerKeyLength #1: " + PartnerKeyLength.ToString());
            }
            else
            {
                PartnerKeyLength = AParameter.Length - PartnerKeyParamPos - 1 - NextParameterStart;
                MessageBox.Show("PartnerKeyLength #2: " + PartnerKeyLength.ToString());
            }

//                MessageBox.Show("PartnerKeyLength: " + PartnerKeyLength.ToString());

//                MessageBox.Show("Parsed 'PartnerKey' parameter: " + "Startpos.: " + PartnerKeyParamPos.ToString() + "; Contents: " +
//                    AParameter.Substring(PartnerKeyParamPos + TTestWinForm.PARTNERKEY_PARAM.Length + 1, PartnerKeyLength));

            PartnerKey = Convert.ToInt64(
                AParameter.Substring(PartnerKeyParamPos + TTestWinForm.PARTNERKEY_PARAM.Length + 1, PartnerKeyLength));
        }
        else
        {
            PartnerKey = Convert.ToInt64(AParameter);
        }

        Ict.Petra.Client.MPartner.Gui.TFrmPartnerEdit PartnerEditForm = new Ict.Petra.Client.MPartner.Gui.TFrmPartnerEdit(null);

        PartnerEditForm.SetParameters(TScreenMode.smEdit, PartnerKey);

        PartnerEditForm.Show();
    }

    /// <summary>
    /// Opens a Partner Find screen.
    /// </summary>
    /// <param name="AParameter">Currently ignored.</param>
    public override void OpenPartnerFindScreen(String AParameter)
    {
//MessageBox.Show("Entering OpenPartnerFindScreen...");
        Ict.Petra.Client.MPartner.Gui.TPartnerFindScreen PartnerFindForm = new Ict.Petra.Client.MPartner.Gui.TPartnerFindScreen(null);

        PartnerFindForm.SetParameters(false, -1);

        PartnerFindForm.Show();
    }

    /// <summary>
    /// Opens the GL Batch screen.
    /// </summary>
    /// <param name="AParameter">LedgerNumber (as String!).</param>
    public override void OpenGLBatchScreen(String AParameter)
    {
//MessageBox.Show("Entering OpenPartnerFindScreen...");\
//             int LedgerNumberParamPos;
        int NextParameterStart;
        int LedgerNumberLength;
        Int32 LedgerNumber;

        LedgerNumberParamPos = AParameter.IndexOf(TTestWinForm.LEDGERNUMBER_PARAM);

        if (LedgerNumberParamPos != -1)
        {
            NextParameterStart = AParameter.IndexOf(TTestWinForm.PARAM_SEPARATOR, LedgerNumberParamPos +
                TTestWinForm.LEDGERNUMBER_PARAM.Length + 1);

            if (NextParameterStart == -1)
            {
                LedgerNumberLength = AParameter.Length - LedgerNumberParamPos - TTestWinForm.LEDGERNUMBER_PARAM.Length - 1;
//                    MessageBox.Show("LedgerNumberLength #1: " + LedgerNumberLength.ToString());
            }
            else
            {
                LedgerNumberLength = AParameter.Length - LedgerNumberParamPos - 1 - NextParameterStart;
//                    MessageBox.Show("LedgerNumberLength #2: " + LedgerNumberLength.ToString());
            }

//                MessageBox.Show("LedgerNumberLength: " + LedgerNumberLength.ToString());

//                MessageBox.Show("Parsed 'LedgerNumber' parameter: " + "Startpos.: " + LedgerNumberParamPos.ToString() + "; Contents: " +
//                    AParameter.Substring(LedgerNumberParamPos + TTestWinForm.LEDGERNUMBER_PARAM.Length + 1, LedgerNumberLength));

            LedgerNumber = Convert.ToInt32(
                AParameter.Substring(LedgerNumberParamPos + TTestWinForm.LEDGERNUMBER_PARAM.Length + 1, LedgerNumberLength));
        }
        else
        {
            LedgerNumber = Convert.ToInt32(AParameter);
        }

        Ict.Petra.Client.MFinance.Gui.GL.TFrmGLBatch GLBatchForm = new Ict.Petra.Client.MFinance.Gui.GL.TFrmGLBatch(null);

        GLBatchForm.LedgerNumber = LedgerNumber;

        GLBatchForm.Show();
    }
}
}