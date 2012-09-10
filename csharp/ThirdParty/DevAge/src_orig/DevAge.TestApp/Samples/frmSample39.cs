using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevAge.TestApp
{
    [Sample("Other controls", 39, "Drawing colors utilities")]
    public partial class frmSample39 : Form
    {
        public frmSample39()
        {
            InitializeComponent();
        }

        private void trackBarLight_ValueChanged(object sender, EventArgs e)
        {
            panelLightDestination.BackColor = DevAge.Drawing.Utilities.CalculateLightDarkColor(colorPickerSource.SelectedColor, (float)trackBarLight.Value / 100.0f);
        }

        private void colorPickerSource_SelectedColorChanged(object sender, EventArgs e)
        {
            panelLightDestination.BackColor = DevAge.Drawing.Utilities.CalculateLightDarkColor(colorPickerSource.SelectedColor, (float)trackBarLight.Value / 100.0f);
        }


    }
}