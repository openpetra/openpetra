using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    public partial class DlgDbBuildConfig : Form
    {
        /// <summary>
        /// The exit data specified when the user clicked the OK button
        /// </summary>
        public string ExitData = String.Empty;

        public DlgDbBuildConfig()
        {
            InitializeComponent();

            for (int i=0; i<DbBuildConfiguration.Systems.Length; i++) cboDBMS.Items.Add(DbBuildConfiguration.Systems[i]);
            cboDBMS.SelectedIndex = 0;
        }

        /// <summary>
        /// Call this before ShowDialog to initialise the dialog with entry values to be edited
        /// </summary>
        /// <param name="BranchLocation">The path to the active branch</param>
        /// <param name="Index">The index of the favourite to be edited</param>
        public void InitializeDialog(string BranchLocation, int Index)
        {
            DbBuildConfiguration dbCfg = new DbBuildConfiguration(BranchLocation);
            string dbms, dbName, port, password, location;
            bool isBlank;
            dbCfg.GetStoredConfiguration(Index, out dbms, out dbName, out port, out password, out isBlank, out location);

            cboDBMS.SelectedIndex = DbBuildConfiguration.GetDBMSIndex(dbms);
            txtDBName.Text = dbName;
            txtPort.Text = port;
            txtPassword.Text = password;
            chkBlankPW.Checked = isBlank;
            txtLocation.Text = location;
            
            SetEnabledStates();
        }

        /// <summary>
        /// Call this to set the enabled states of the checkbox and textbox
        /// </summary>
        public void SetEnabledStates()
        {
            txtPassword_TextChanged(null, null);
            chkBlankPW_Click(null, null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string s = txtLocation.Text;
            s = s.Replace('\\', '/');
            ExitData = DbBuildConfiguration.MakeConfigString(
                (cboDBMS.SelectedIndex == 0) ? String.Empty : cboDBMS.Items[cboDBMS.SelectedIndex].ToString(),
                txtDBName.Text,
                txtPort.Text,
                txtPassword.Text,
                chkBlankPW.Checked,
                s);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            chkBlankPW.Enabled = txtPassword.Text == String.Empty;
            if (!chkBlankPW.Enabled) chkBlankPW.Checked = false;
        }

        private void chkBlankPW_Click(object sender, EventArgs e)
        {
            txtPassword.Enabled = (!chkBlankPW.Checked);
        }
    }
}
