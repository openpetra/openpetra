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
namespace treasurerEmails
{
partial class TFrmDBLogin
{
    /// <summary>
    /// Designer variable used to keep track of non-visual components.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Disposes resources used by the form.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (components != null)
            {
                components.Dispose();
            }
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// This method is required for Windows Forms designer support.
    /// Do not change the method contents inside the source code editor. The Forms designer might
    /// not be able to load this method if it was changed manually.
    /// </summary>
    private void InitializeComponent()
    {
        this.lblUsername = new System.Windows.Forms.Label();
        this.lblPassword = new System.Windows.Forms.Label();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.BtnOk = new System.Windows.Forms.Button();
        this.BtnCancel = new System.Windows.Forms.Button();
        this.SuspendLayout();

        //
        // lblUsername
        //
        this.lblUsername.Location = new System.Drawing.Point(12, 15);
        this.lblUsername.Name = "lblUsername";
        this.lblUsername.Size = new System.Drawing.Size(80, 23);
        this.lblUsername.TabIndex = 0;
        this.lblUsername.Text = "Username:";

        //
        // lblPassword
        //
        this.lblPassword.Location = new System.Drawing.Point(12, 40);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(80, 23);
        this.lblPassword.TabIndex = 1;
        this.lblPassword.Text = "Password:";

        //
        // txtUsername
        //
        this.txtUsername.Location = new System.Drawing.Point(98, 15);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(174, 20);
        this.txtUsername.TabIndex = 2;

        //
        // txtPassword
        //
        this.txtPassword.Location = new System.Drawing.Point(98, 37);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new System.Drawing.Size(174, 20);
        this.txtPassword.TabIndex = 3;

        //
        // BtnOk
        //
        this.BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.BtnOk.Location = new System.Drawing.Point(119, 66);
        this.BtnOk.Name = "BtnOk";
        this.BtnOk.Size = new System.Drawing.Size(75, 23);
        this.BtnOk.TabIndex = 4;
        this.BtnOk.Text = "&Ok";
        this.BtnOk.UseVisualStyleBackColor = true;

        //
        // BtnCancel
        //
        this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.BtnCancel.Location = new System.Drawing.Point(216, 66);
        this.BtnCancel.Name = "BtnCancel";
        this.BtnCancel.Size = new System.Drawing.Size(75, 23);
        this.BtnCancel.TabIndex = 5;
        this.BtnCancel.Text = "&Cancel";
        this.BtnCancel.UseVisualStyleBackColor = true;

        //
        // TFrmDBLogin
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(336, 98);
        this.Controls.Add(this.BtnCancel);
        this.Controls.Add(this.BtnOk);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.lblPassword);
        this.Controls.Add(this.lblUsername);
        this.Name = "TFrmDBLogin";
        this.Text = "Login to Database";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Button BtnCancel;
    private System.Windows.Forms.Button BtnOk;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.Label lblUsername;
}
}