/*
 * Created by SharpDevelop.
 * User: sbird
 * Date: 8/16/2011
 * Time: 1:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Ict.Common.Controls;

namespace ControlTestBench
{
    partial class TaskListTest
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
            if(disposing) {
                if(components != null) {
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
            this.btnTestDefaultConstructor = new System.Windows.Forms.Button();
            this.btnTestFullConstructor = new System.Windows.Forms.Button();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTestDefaultConstructor
            // 
            this.btnTestDefaultConstructor.Location = new System.Drawing.Point(269, 105);
            this.btnTestDefaultConstructor.Name = "btnTestDefaultConstructor";
            this.btnTestDefaultConstructor.Size = new System.Drawing.Size(147, 23);
            this.btnTestDefaultConstructor.TabIndex = 0;
            this.btnTestDefaultConstructor.Text = "Test Default Constructor";
            this.btnTestDefaultConstructor.UseVisualStyleBackColor = true;
            this.btnTestDefaultConstructor.Click += new System.EventHandler(this.TestDefaultConstructor);
            // 
            // btnTestFullConstructor
            // 
            this.btnTestFullConstructor.Location = new System.Drawing.Point(269, 134);
            this.btnTestFullConstructor.Name = "btnTestFullConstructor";
            this.btnTestFullConstructor.Size = new System.Drawing.Size(147, 23);
            this.btnTestFullConstructor.TabIndex = 1;
            this.btnTestFullConstructor.Text = "Test Full Constructor";
            this.btnTestFullConstructor.UseVisualStyleBackColor = true;
            this.btnTestFullConstructor.Click += new System.EventHandler(this.TestFullConstructor);
            // 
            // btnDisable
            // 
            this.btnDisable.Location = new System.Drawing.Point(269, 192);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(147, 23);
            this.btnDisable.TabIndex = 4;
            this.btnDisable.Text = "Disable/Enable Task";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.DisableItemButtonClick);
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(269, 221);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(147, 23);
            this.btnHide.TabIndex = 5;
            this.btnHide.Text = "Hide/Show Task";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.HideItemButtonClick);
            // 
            // TaskListTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(674, 500);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.btnDisable);
            this.Controls.Add(this.btnTestFullConstructor);
            this.Controls.Add(this.btnTestDefaultConstructor);
            this.Name = "TaskListTest";
            this.Text = "TaskList Test Form Window";
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnTestFullConstructor;
        private System.Windows.Forms.Button btnTestDefaultConstructor;
        private Ict.Common.Controls.TTaskList TaskList1;
        
        

    }
}
