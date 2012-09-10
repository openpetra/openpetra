using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevAge.TestApp.Samples
{
    [Sample("Other controls", 40, "Performance test")]
    public partial class frmSample40 : Form
    {
        public frmSample40()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DevAge.ComponentModel.Validator.ValidatorTypeConverter customValidator = 
                new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(TestDelegate));

            TestDelegate[] values = new TestDelegate[] { Test_RectangleBorder, Test_RectangleBorder1, Test_RectangleBorder2, Test_RectangleBorder3, Test_GDIRectangle, Test_TextGDI, Test_TextRenderer };
            customValidator.StandardValues = values;

            DevAge.ComponentModel.Validator.ValueMapping mapping = new DevAge.ComponentModel.Validator.ValueMapping();
            mapping.DisplayStringList = new string[] { "Test RectangleBorder", "Test RectangleBorder 1", "Test RectangleBorder 2", "Test RectangleBorder 3", "Test GDIRectangle", "Test TextGDI", "Test TextRenderer" };
            mapping.ValueList = values;

            //Bind the ValueMapping to the Validator
            mapping.BindValidator(customValidator);

            cbTestList.Validator = customValidator;
        }

        private delegate void TestDelegate(Graphics gr);

        private void Test_RectangleBorder(Graphics gr)
        {
            DevAge.Drawing.RectangleBorder rct = 
                new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Red, 3),
                                                new DevAge.Drawing.BorderLine(Color.Blue, 3));

            
            using (DevAge.Drawing.GraphicsCache cache = new DevAge.Drawing.GraphicsCache(gr))
            {
                for (int i = 0; i < 100; i++)
                    rct.Draw(cache, panelDraw.ClientRectangle);
            }
        }

        private void Test_RectangleBorder1(Graphics gr)
        {
            DevAge.Drawing.RectangleBorder rct =
                new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Red, 1),
                                                new DevAge.Drawing.BorderLine(Color.Blue, 1),
                                                new DevAge.Drawing.BorderLine(Color.Green, 1),
                                                new DevAge.Drawing.BorderLine(Color.YellowGreen, 1));

            using (DevAge.Drawing.GraphicsCache cache = new DevAge.Drawing.GraphicsCache(gr))
            {
                for (int i = 0; i < 100; i++)
                {
                    rct.Draw(cache, panelDraw.ClientRectangle);

                    Rectangle innerRect = panelDraw.ClientRectangle;
                    innerRect.Inflate(-10, -10);
                    rct.Draw(cache, innerRect);
                }
            }
        }

        private void Test_RectangleBorder2(Graphics gr)
        {
            DevAge.Drawing.RectangleBorder rct =
                new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Red, 2),
                                                new DevAge.Drawing.BorderLine(Color.Blue, 2),
                                                new DevAge.Drawing.BorderLine(Color.Green, 2),
                                                new DevAge.Drawing.BorderLine(Color.YellowGreen, 2));

            using (DevAge.Drawing.GraphicsCache cache = new DevAge.Drawing.GraphicsCache(gr))
            {
                for (int i = 0; i < 100; i++)
                {
                    rct.Draw(cache, panelDraw.ClientRectangle);

                    Rectangle innerRect = panelDraw.ClientRectangle;
                    innerRect.Inflate(-10, -10);
                    rct.Draw(cache, innerRect);
                }
            }
        }

        private void Test_RectangleBorder3(Graphics gr)
        {
            DevAge.Drawing.RectangleBorder rct =
                new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Red, 3),
                                                new DevAge.Drawing.BorderLine(Color.Blue, 3),
                                                new DevAge.Drawing.BorderLine(Color.Green, 3),
                                                new DevAge.Drawing.BorderLine(Color.YellowGreen, 3));

            using (DevAge.Drawing.GraphicsCache cache = new DevAge.Drawing.GraphicsCache(gr))
            {
                for (int i = 0; i < 100; i++)
                {
                    rct.Draw(cache, panelDraw.ClientRectangle);

                    Rectangle innerRect = panelDraw.ClientRectangle;
                    innerRect.Inflate(-10, -10);
                    rct.Draw(cache, innerRect);
                }
            }
        }

        private void Test_GDIRectangle(Graphics gr)
        {
            for (int i = 0; i < 100; i++)
                gr.DrawRectangle(new Pen(Color.Red, 3), panelDraw.ClientRectangle);
        }

        private void Test_TextGDI(Graphics gr)
        {
            DevAge.Drawing.VisualElements.TextGDI text = new DevAge.Drawing.VisualElements.TextGDI("Test text performance");

            using (DevAge.Drawing.GraphicsCache cache = new DevAge.Drawing.GraphicsCache(gr))
            {
                for (int i = 0; i < 1000; i++)
                    text.Draw(cache, panelDraw.ClientRectangle);
            }
        }

        private void Test_TextRenderer(Graphics gr)
        {
            DevAge.Drawing.VisualElements.TextRenderer text = new DevAge.Drawing.VisualElements.TextRenderer("Test text performance");

            using (DevAge.Drawing.GraphicsCache cache = new DevAge.Drawing.GraphicsCache(gr))
            {
                for (int i = 0; i < 1000; i++)
                    text.Draw(cache, panelDraw.ClientRectangle);
            }
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            try
            {
                TestDelegate testMethod = (TestDelegate)cbTestList.Value;

                if (testMethod == null)
                {
                    lblPerfoamnceResult.Text = "No test selected";
                    return;
                }

                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

                using (Graphics gr = panelDraw.CreateGraphics())
                {
                    gr.Clear(Color.White);

                    watch.Start();
                    testMethod.Invoke(gr);
                    watch.Stop();
                }


                lblPerfoamnceResult.Text = watch.ElapsedMilliseconds + "ms";
            }
            catch (Exception ex)
            {
                DevAge.Windows.Forms.ErrorDialog.Show(this, ex, "Error");
            }
        }

        private void panelDraw_Paint(object sender, PaintEventArgs e)
        {
            if (chkUseOnPaint.Checked)
            {
                TestDelegate testMethod = (TestDelegate)cbTestList.Value;

                if (testMethod != null)
                    testMethod.Invoke(e.Graphics);
            }
        }
    }
}