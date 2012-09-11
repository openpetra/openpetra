using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevAge.TestApp
{
    /// <summary>
    /// Summary description for frmSample37.
    /// </summary>
    [Sample("Other controls", 37, "Xml Digital Sign")]
    public class frmSample37 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btGenerateKeys;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPubPriKey;
        private System.Windows.Forms.TextBox txtPubKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSampleXml;
        private System.Windows.Forms.Button btSignXml;
        private System.Windows.Forms.TextBox txtSignedXml;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btVerifyData;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btExtract;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtOriginalXmlData;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSignedXml2;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmSample37()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPubPriKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPubKey = new System.Windows.Forms.TextBox();
            this.btGenerateKeys = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSampleXml = new System.Windows.Forms.TextBox();
            this.btSignXml = new System.Windows.Forms.Button();
            this.txtSignedXml = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btVerifyData = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btExtract = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtOriginalXmlData = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSignedXml2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtPubPriKey
            // 
            this.txtPubPriKey.Location = new System.Drawing.Point(256, 8);
            this.txtPubPriKey.Multiline = true;
            this.txtPubPriKey.Name = "txtPubPriKey";
            this.txtPubPriKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPubPriKey.Size = new System.Drawing.Size(296, 48);
            this.txtPubPriKey.TabIndex = 0;
            this.txtPubPriKey.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(184, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Public + private key";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(184, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Public key";
            // 
            // txtPubKey
            // 
            this.txtPubKey.Location = new System.Drawing.Point(256, 64);
            this.txtPubKey.Multiline = true;
            this.txtPubKey.Name = "txtPubKey";
            this.txtPubKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPubKey.Size = new System.Drawing.Size(296, 48);
            this.txtPubKey.TabIndex = 2;
            this.txtPubKey.Text = "";
            // 
            // btGenerateKeys
            // 
            this.btGenerateKeys.Location = new System.Drawing.Point(8, 32);
            this.btGenerateKeys.Name = "btGenerateKeys";
            this.btGenerateKeys.Size = new System.Drawing.Size(112, 23);
            this.btGenerateKeys.TabIndex = 4;
            this.btGenerateKeys.Text = "Generate keys ->";
            this.btGenerateKeys.Click += new System.EventHandler(this.btGenerateKeys_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 32);
            this.label3.TabIndex = 5;
            this.label3.Text = "Generate the keys using RSA with a 1024 key size";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 120);
            this.label4.Name = "label4";
            this.label4.TabIndex = 6;
            this.label4.Text = "Sample Xml Data";
            // 
            // txtSampleXml
            // 
            this.txtSampleXml.Location = new System.Drawing.Point(8, 144);
            this.txtSampleXml.Multiline = true;
            this.txtSampleXml.Name = "txtSampleXml";
            this.txtSampleXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSampleXml.Size = new System.Drawing.Size(192, 64);
            this.txtSampleXml.TabIndex = 7;
            this.txtSampleXml.Text = "<data>Ciao ciao</data>";
            // 
            // btSignXml
            // 
            this.btSignXml.Location = new System.Drawing.Point(224, 144);
            this.btSignXml.Name = "btSignXml";
            this.btSignXml.Size = new System.Drawing.Size(112, 23);
            this.btSignXml.TabIndex = 8;
            this.btSignXml.Text = "Sign Xml ->";
            this.btSignXml.Click += new System.EventHandler(this.btSignXml_Click);
            // 
            // txtSignedXml
            // 
            this.txtSignedXml.Location = new System.Drawing.Point(360, 144);
            this.txtSignedXml.Multiline = true;
            this.txtSignedXml.Name = "txtSignedXml";
            this.txtSignedXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSignedXml.Size = new System.Drawing.Size(192, 64);
            this.txtSignedXml.TabIndex = 9;
            this.txtSignedXml.Text = "";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(216, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 40);
            this.label5.TabIndex = 10;
            this.label5.Text = "Sign the xml creating a new xml (using the private key)";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(360, 120);
            this.label6.Name = "label6";
            this.label6.TabIndex = 11;
            this.label6.Text = "Signed Xml";
            // 
            // btVerifyData
            // 
            this.btVerifyData.Location = new System.Drawing.Point(48, 344);
            this.btVerifyData.Name = "btVerifyData";
            this.btVerifyData.Size = new System.Drawing.Size(112, 23);
            this.btVerifyData.TabIndex = 12;
            this.btVerifyData.Text = "Verify Sign";
            this.btVerifyData.Click += new System.EventHandler(this.btVerifyData_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(32, 368);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 40);
            this.label7.TabIndex = 13;
            this.label7.Text = "Verify the signed xml (using the public key)";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(208, 296);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 40);
            this.label8.TabIndex = 15;
            this.label8.Text = "Extract the xml data from the signed xml";
            // 
            // btExtract
            // 
            this.btExtract.Location = new System.Drawing.Point(224, 272);
            this.btExtract.Name = "btExtract";
            this.btExtract.Size = new System.Drawing.Size(112, 23);
            this.btExtract.TabIndex = 14;
            this.btExtract.Text = "Extract Xml Data ->";
            this.btExtract.Click += new System.EventHandler(this.btExtract_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(352, 248);
            this.label9.Name = "label9";
            this.label9.TabIndex = 17;
            this.label9.Text = "Original Xml";
            // 
            // txtOriginalXmlData
            // 
            this.txtOriginalXmlData.Location = new System.Drawing.Point(352, 272);
            this.txtOriginalXmlData.Multiline = true;
            this.txtOriginalXmlData.Name = "txtOriginalXmlData";
            this.txtOriginalXmlData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOriginalXmlData.Size = new System.Drawing.Size(192, 64);
            this.txtOriginalXmlData.TabIndex = 16;
            this.txtOriginalXmlData.Text = "";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(8, 248);
            this.label10.Name = "label10";
            this.label10.TabIndex = 19;
            this.label10.Text = "Signed Xml";
            // 
            // txtSignedXml2
            // 
            this.txtSignedXml2.Location = new System.Drawing.Point(8, 272);
            this.txtSignedXml2.Multiline = true;
            this.txtSignedXml2.Name = "txtSignedXml2";
            this.txtSignedXml2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSignedXml2.Size = new System.Drawing.Size(192, 64);
            this.txtSignedXml2.TabIndex = 18;
            this.txtSignedXml2.Text = "";
            // 
            // frmSample35
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(568, 462);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtSignedXml2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtOriginalXmlData);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btExtract);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btVerifyData);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSignedXml);
            this.Controls.Add(this.btSignXml);
            this.Controls.Add(this.txtSampleXml);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btGenerateKeys);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPubKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPubPriKey);
            this.Name = "frmSample35";
            this.Text = "Xml Digital Sign";
            this.ResumeLayout(false);

        }
        #endregion

        private void btGenerateKeys_Click(object sender, System.EventArgs e)
        {
            try
            {
                string pubKey, pubPriKey;

                DevAge.Security.Cryptography.Utilities.XmlDigitalSign.GenerateKeys(out pubPriKey, out pubKey);
                txtPubPriKey.Text = pubPriKey;
                txtPubKey.Text = pubKey;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void btSignXml_Click(object sender, System.EventArgs e)
        {
            try
            {
                System.Xml.XmlDocument xmlToSign = new System.Xml.XmlDocument();
                xmlToSign.LoadXml(txtSampleXml.Text);

                System.Xml.XmlDocument signedXml = DevAge.Security.Cryptography.Utilities.XmlDigitalSign.CreateSignedDoc(xmlToSign, txtPubPriKey.Text);
                txtSignedXml.Text = signedXml.OuterXml;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void btExtract_Click(object sender, System.EventArgs e)
        {
            try
            {
                System.Xml.XmlDocument xmlSigned = new System.Xml.XmlDocument();
                xmlSigned.LoadXml(txtSignedXml2.Text);

                System.Xml.XmlDocument notSignedDoc = DevAge.Security.Cryptography.Utilities.XmlDigitalSign.CreateDocWithoutSignature(xmlSigned);
                txtOriginalXmlData.Text = notSignedDoc.OuterXml;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void btVerifyData_Click(object sender, System.EventArgs e)
        {
            try
            {
                System.Xml.XmlDocument xmlSigned = new System.Xml.XmlDocument();
                xmlSigned.LoadXml(txtSignedXml2.Text);

                if (DevAge.Security.Cryptography.Utilities.XmlDigitalSign.CheckSignature(xmlSigned, txtPubKey.Text))
                    MessageBox.Show(this, "Signature OK");
                else
                    MessageBox.Show(this, "Signature FAILED");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}
