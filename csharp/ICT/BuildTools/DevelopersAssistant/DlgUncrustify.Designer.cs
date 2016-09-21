//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2016 by OM International
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
namespace Ict.Tools.DevelopersAssistant
{
    /// <summary>
    /// Dialog class that shows a tree view containing base folders for Uncrustify
    /// </summary>
    partial class DlgUncrustify
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("BuildTools");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Common");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Client");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Shared");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Server");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Tools");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Petra", new System.Windows.Forms.TreeNode[] {
                    treeNode30,
                    treeNode31,
                    treeNode32,
                    treeNode33
                });
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Testing");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("ICT", new System.Windows.Forms.TreeNode[] {
                    treeNode28,
                    treeNode29,
                    treeNode34,
                    treeNode35
                });
            this.label1 = new System.Windows.Forms.Label();
            this.tvBaseFolder = new System.Windows.Forms.TreeView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 34);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose a base folder.  All files in all folders beneath the selected folder will " +
                               "be uncrustified.";
            //
            // tvBaseFolder
            //
            this.tvBaseFolder.Location = new System.Drawing.Point(13, 51);
            this.tvBaseFolder.Name = "tvBaseFolder";
            treeNode28.Name = "NodeBuildTools";
            treeNode28.Text = "BuildTools";
            treeNode29.Name = "NodeCommon";
            treeNode29.Text = "Common";
            treeNode30.Name = "NodeClient";
            treeNode30.Text = "Client";
            treeNode31.Name = "NodeShared";
            treeNode31.Text = "Shared";
            treeNode32.Name = "NodeServer";
            treeNode32.Text = "Server";
            treeNode33.Name = "NodeTools";
            treeNode33.Text = "Tools";
            treeNode34.Name = "NodePetra";
            treeNode34.Text = "Petra";
            treeNode35.Name = "NodeTesting";
            treeNode35.Text = "Testing";
            treeNode36.Name = "NodeICT";
            treeNode36.Text = "ICT";
            this.tvBaseFolder.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
                    treeNode36
                });
            this.tvBaseFolder.Size = new System.Drawing.Size(196, 175);
            this.tvBaseFolder.TabIndex = 1;
            //
            // btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(224, 51);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(224, 80);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // DlgUncrustify
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(311, 239);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tvBaseFolder);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgUncrustify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Uncrustify";
            this.Load += new System.EventHandler(this.DlgUncrustify_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView tvBaseFolder;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}