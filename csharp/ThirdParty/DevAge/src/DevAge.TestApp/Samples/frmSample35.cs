using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevAge.TestApp
{
    [Sample("Other controls", 35, "Validation controls - TextBox, ComboBox, TextBoxButton")]
    public partial class frmSample35 : Form
    {
        public frmSample35()
        {
            InitializeComponent();

            txtDateTime.Value = DateTime.Today;
            txtRectangle.Value = this.ClientRectangle;
            cbEnum.Value = ContentAlignment.BottomCenter;

            //String array
            validatorStringArray.StandardValues = new string[] { "Italy", "France", "Germany", "Spain" };
            cbStringArray.Value = "Italy";

            //Custom mapping
            string[] codes = new string[] { "IT", "FR", "GR", "SP" };
            validatorCustomMapping.StandardValues = codes;
            validatorCustomMapping.StandardValuesExclusive = true;
            DevAge.ComponentModel.Validator.ValueMapping mapping = new DevAge.ComponentModel.Validator.ValueMapping();
            mapping.DisplayStringList = new string[] { "Italy", "France", "Germany", "Spain" };
            mapping.ValueList = codes;
            mapping.SpecialType = typeof(string);
            mapping.SpecialList = mapping.DisplayStringList;
            mapping.ThrowErrorIfNotFound = false;
            mapping.BindValidator(validatorCustomMapping);

            cbCustomMapping.Value = "IT";

            //Custom mapping 2
            int[] codesInt = new int[] { 0, 1, 2, 3 };
            validatorCustomMapping2.StandardValues = codesInt;
            validatorCustomMapping2.StandardValuesExclusive = true;
            DevAge.ComponentModel.Validator.ValueMapping mapping2 = new DevAge.ComponentModel.Validator.ValueMapping();
            mapping2.DisplayStringList = new string[] { "Zero", "One", "Two", "Three" };
            mapping2.ValueList = codesInt;
            //mapping2.SpecialType = typeof(string);
            //mapping2.SpecialList = mapping2.DisplayStringList;
            mapping2.ThrowErrorIfNotFound = false;
            mapping2.BindValidator(validatorCustomMapping2);

            cbCustomMapping2.Value = 0;
        }

        private void cbCustomMapping_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbCustomMapping.Value != null)
                lblCustomValue.Text = cbCustomMapping.Value.ToString();
            else
                lblCustomValue.Text = "";
        }

        private void cbCustomMapping2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbCustomMapping2.Value != null)
                lblCustomValue2.Text = cbCustomMapping2.Value.ToString();
            else
                lblCustomValue2.Text = "";
        }

    }
}