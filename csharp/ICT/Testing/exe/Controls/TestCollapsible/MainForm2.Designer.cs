/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 14:42
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Ict.Common.Controls;

namespace TestCollapsible
{
partial class MainForm
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
        this.tPnlCollapsible1 = new Ict.Common.Controls.TPnlCollapsible();
        this.button1 = new System.Windows.Forms.Button();
        this.tPnlCollapsible2 = new Ict.Common.Controls.TPnlCollapsible(THostedControlKind.hckTaskList,
            "testing alternate constructor",
            TCollapseDirection.cdHorizontal);
        this.SuspendLayout();
        //
        // tPnlCollapsible1
        //
        this.tPnlCollapsible1.AutoSize = true;
        this.tPnlCollapsible1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tPnlCollapsible1.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
        this.tPnlCollapsible1.IsCollapsed = false;
        this.tPnlCollapsible1.Location = new System.Drawing.Point(19, 74);
        this.tPnlCollapsible1.Margin = new System.Windows.Forms.Padding(0);
        this.tPnlCollapsible1.Name = "tPnlCollapsible1";
        this.tPnlCollapsible1.Size = new System.Drawing.Size(380, 164);
        this.tPnlCollapsible1.TabIndex = 0;
        //
        // button1
        //
        this.button1.Location = new System.Drawing.Point(166, 23);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(154, 23);
        this.button1.TabIndex = 1;
        this.button1.Text = "Toggle Direction";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.Button1Click);
        //
        // tPnlCollapsible2
        //
        this.tPnlCollapsible2.AutoSize = true;
        this.tPnlCollapsible2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tPnlCollapsible2.IsCollapsed = false;
        this.tPnlCollapsible2.Location = new System.Drawing.Point(42, 264);
        this.tPnlCollapsible2.Margin = new System.Windows.Forms.Padding(0);
        this.tPnlCollapsible2.Name = "tPnlCollapsible2";
        this.tPnlCollapsible2.Size = new System.Drawing.Size(424, 150);
        this.tPnlCollapsible2.TabIndex = 2;
        //
        // MainForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(621, 477);
        this.Controls.Add(this.tPnlCollapsible2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.tPnlCollapsible1);
        this.Name = "MainForm";
        this.Text = "TestCollapsible";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Ict.Common.Controls.TPnlCollapsible tPnlCollapsible2;
    private System.Windows.Forms.Button button1;
    private Ict.Common.Controls.TPnlCollapsible tPnlCollapsible1;
}
}