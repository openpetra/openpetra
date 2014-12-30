/*
 * Created by SharpDevelop.
 * User: christiank
 * Date: 26/08/2013
 * Time: 17:21
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlTestBench
{
/// <summary>
/// Description of RTBwithHyperlinksUCTest.
/// </summary>
public partial class RTBwithHyperlinksUCTest : Form
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public RTBwithHyperlinksUCTest()
    {
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent();

        UpdateTextBoxHeight(Convert.ToInt32(txtNumberOfLines.Text));

        AnyRadioButton_TestCaseCheckedChanged(null, null);
    }

    void AnyRadioButton_TestCaseCheckedChanged(object sender, EventArgs e)
    {
        if (rbtTestCaseMultiLineDifferentHLTypes.Checked)
        {
            rtbHyperlinksTest.Text = "E-Mail: ||email||\"Christian K\" <christian.kendel@om.org>" + Environment.NewLine +
                                     "Business E-mail: ||email||a@b.com" + Environment.NewLine +
                                     "Blog: ||hyperlink||http://www.here.com" + Environment.NewLine +
                                     "Web Site: ||securehyperlink||https://linkedin.com/login" + Environment.NewLine +
                                     "Web Site: ||FTP||ftp://downloads.thesite.com" + Environment.NewLine +
                                     "Business Skype: ||skype||christank"; // works!
        }
        else if (rbtTestCaseSimpleSingleEmail.Checked)
        {
            rtbHyperlinksTest.Text = "||email||here@there.com";     // works!
        }
        else if (rbtTestCaseSimpleEmails1.Checked)
        {
            rtbHyperlinksTest.Text = "||email||test@test.com, abc,||email||here@there.com, asdf";     // works!
        }
        else if (rbtTestCaseSimpleEmails2.Checked)
        {
            rtbHyperlinksTest.Text = "aaa||email||test@test.com, ||email||here@there.com; asdf";     // works!
        }
        else if (rbtTestCaseSimpleEmails3.Checked)
        {
            rtbHyperlinksTest.Text = "||email||test@test.com; ||email||here@there.com,asdf";     // works!
        }
        else if (rbtTestCaseEmailWithName1.Checked)
        {
            rtbHyperlinksTest.Text = "aaa||email||\"Christian K\" <christian.kendel@om.org>" + Environment.NewLine + "||email||a@b.com; " +
                                     "||hyperlink||http://www.here.com,||securehyperlink||https://linkedin.com/login,||FTP||ftp://downloads.thesite.com;"
                                     +
                                     "||skype||christank"; // works!
        }
        else if (rbtTestCaseEmailWithName2.Checked)
        {
            rtbHyperlinksTest.Text = "aaa ||email||\"Christian K\" <christian.kendel@om.org>" + Environment.NewLine + "a ||email||a@b.com" +
                                     Environment.NewLine +
                                     "a ||hyperlink||http://www.here.com,||securehyperlink||https://linkedin.com/login,||FTP||ftp://downloads.thesite.com;"
                                     +
                                     "||skype||christank"; // BREAKS!
        }
    }

    void AnyRadioButton_DisplayOptionCheckedChanged(object sender, EventArgs e)
    {
        if (rbtSingleLine.Checked)
        {
            UpdateTextBoxHeight(1);
            txtNumberOfLines.Enabled = false;
        }
        else
        {
            UpdateTextBoxHeight(Convert.ToInt32(txtNumberOfLines.Text));
            txtNumberOfLines.Enabled = true;
        }
    }

    private void UpdateTextBoxHeight(int ANumberOfTextLines)
    {
        rtbHyperlinksTest.Height = ANumberOfTextLines * 23;
    }

    void TxtNumberOfLinesValidating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        UpdateTextBoxHeight(Convert.ToInt32(txtNumberOfLines.Text));
    }

    void RtbHyperlinksTestLinkClicked(string ALinkText, string ALinkType, int ALinkEnd)
    {
        lblClickedLinkInfo.Text = ALinkText + "  [LinkType: " + ALinkType + "; LinkEnd: " + ALinkEnd.ToString() + "]";

        Application.DoEvents();
        System.Threading.Thread.Sleep(2000);
        lblClickedLinkInfo.Text = "";
    }
}
}