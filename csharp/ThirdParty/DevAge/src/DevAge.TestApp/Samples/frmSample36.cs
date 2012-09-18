using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevAge.TestApp
{
    [Sample("Other controls", 36, "VisualElements")]
    public partial class frmSample36 : Form
    {
        private DevAge.Drawing.VisualElements.Text mText = new DevAge.Drawing.VisualElements.Text();
        private DevAge.Drawing.VisualElements.TextGDI mTextGDI = new DevAge.Drawing.VisualElements.TextGDI();
        private DevAge.Drawing.VisualElements.Image mImage = new DevAge.Drawing.VisualElements.Image();
        private DevAge.Drawing.VisualElements.Container mContainer = new DevAge.Drawing.VisualElements.Container();
        private DevAge.Drawing.VisualElements.Container mContainerWithHeader = new DevAge.Drawing.VisualElements.Container();
        private DevAge.Drawing.VisualElements.Container mContainerWithDropDown = new DevAge.Drawing.VisualElements.Container();
        private DevAge.Drawing.VisualElements.TextRenderer mTextRenderer = new DevAge.Drawing.VisualElements.TextRenderer();
        private DevAge.Drawing.VisualElements.ColumnHeaderThemed mColumnHeader = new DevAge.Drawing.VisualElements.ColumnHeaderThemed();
        private DevAge.Drawing.VisualElements.ButtonThemed mButtonThemed = new DevAge.Drawing.VisualElements.ButtonThemed();
        private DevAge.Drawing.VisualElements.CheckBoxThemed mCheckBoxThemed = new DevAge.Drawing.VisualElements.CheckBoxThemed();
        private DevAge.Drawing.VisualElements.Header mHeader = new DevAge.Drawing.VisualElements.Header();
        private DevAge.Drawing.VisualElements.HeaderThemed mHeaderThemed = new DevAge.Drawing.VisualElements.HeaderThemed();
        private DevAge.Drawing.VisualElements.RowHeaderThemed mRowHeaderThemed = new DevAge.Drawing.VisualElements.RowHeaderThemed();
        private DevAge.Drawing.VisualElements.BackgroundLinearGradient mBackgroundLinearGradient = new DevAge.Drawing.VisualElements.BackgroundLinearGradient();

        public frmSample36()
        {
            InitializeComponent();

            txtSize.Value = contentPanel.Size;
            txtMaxSize.Value = new Size(800, 800);

            mContainer.Elements.Add(new DevAge.Drawing.VisualElements.Image(Properties.Resources.SampleSmall1));
            mContainer.Elements[0].AnchorArea = new DevAge.Drawing.AnchorArea();
            mContainer.Elements[0].AnchorArea.Left = 5;
            mContainer.Elements.Add(new DevAge.Drawing.VisualElements.TextGDI("Hello!"));
            mContainer.ElementsDrawMode = DevAge.Drawing.ElementsDrawMode.Align;
            mContainer.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.White, Color.Blue, 45);
            mContainer.Padding = new DevAge.Drawing.Padding(2);
            mContainer.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Blue));

            mContainerWithHeader.Elements.Add(new DevAge.Drawing.VisualElements.Image(Properties.Resources.SampleSmall1));
            mContainerWithHeader.Elements[0].AnchorArea = new DevAge.Drawing.AnchorArea();
            mContainerWithHeader.Elements[0].AnchorArea.Right = 20;
            mContainerWithHeader.Elements.Add(new DevAge.Drawing.VisualElements.TextGDI("Test Header"));
            mContainerWithHeader.ElementsDrawMode = DevAge.Drawing.ElementsDrawMode.Align;
            mContainerWithHeader.Background = new DevAge.Drawing.VisualElements.ColumnHeaderThemed();
            mContainerWithHeader.Padding = new DevAge.Drawing.Padding(2);
            mContainerWithHeader.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Blue));

            mContainerWithDropDown.Elements.Add(new DevAge.Drawing.VisualElements.DropDownButtonThemed());
            mContainerWithDropDown.Elements[0].AnchorArea = new DevAge.Drawing.AnchorArea(float.NaN, 0, 0, 0, false, false);
            mContainerWithDropDown.Elements.Add(new DevAge.Drawing.VisualElements.TextGDI("Test Combo"));
            mContainerWithDropDown.ElementsDrawMode = DevAge.Drawing.ElementsDrawMode.Align;
           
            //mColumnHeader.Elements.Add(new DevAge.Drawing.VisualElements.TextGDI("Hello!"));

            //mButtonThemed.Elements.Add(new DevAge.Drawing.VisualElements.Image(SampleImages.FACE04));
            //mButtonThemed.Elements[0].AnchorArea = new DevAge.Drawing.AnchorArea(0, float.NaN, float.NaN, float.NaN, false, true);
            //mButtonThemed.Elements.Add(new DevAge.Drawing.VisualElements.TextGDI("Hello!"));
            //mButtonThemed.Elements[1].AnchorArea = new DevAge.Drawing.AnchorArea(float.NaN, float.NaN, float.NaN, float.NaN, true, true);
            //mButtonThemed.ElementsDrawMode = DevAge.Drawing.ElementsDrawMode.Align;

            mBackgroundLinearGradient.FirstColor = Color.Red;
            mBackgroundLinearGradient.SecondColor = Color.White;

            cbList.FormattingEnabled = false;

            cbList.Items.Add(this.mText);
            cbList.Items.Add(this.mTextGDI);
            cbList.Items.Add(this.mTextRenderer);
            cbList.Items.Add(this.mImage);
            cbList.Items.Add(this.mContainer);
            cbList.Items.Add(this.mContainerWithHeader);
            cbList.Items.Add(this.mContainerWithDropDown);
            cbList.Items.Add(this.mColumnHeader);
            cbList.Items.Add(this.mButtonThemed);
            cbList.Items.Add(this.mCheckBoxThemed);
            cbList.Items.Add(this.mHeader);
            cbList.Items.Add(this.mHeaderThemed);
            cbList.Items.Add(this.mRowHeaderThemed);
            cbList.Items.Add(this.mBackgroundLinearGradient);

            cbList.SelectedIndex = 0;
        }

        private DevAge.Drawing.VisualElements.IVisualElement GetVisualElement()
        {
            return (DevAge.Drawing.VisualElements.IVisualElement)cbList.SelectedItem;
        }

        private void contentPanel_Paint(object sender, PaintEventArgs e)
        {
            using (DevAge.Drawing.GraphicsCache graphics = new DevAge.Drawing.GraphicsCache(e.Graphics, e.ClipRectangle))
            {
                GetVisualElement().Draw(graphics, contentPanel.ClientRectangle);
            }
        }

        private void btSetSize_Click(object sender, EventArgs e)
        {
            contentPanel.ClientSize = (Size)txtSize.Value;
            contentPanel.Invalidate();
        }

        private void btMeasure_Click(object sender, EventArgs e)
        {
            using (DevAge.Drawing.MeasureHelper helper = new DevAge.Drawing.MeasureHelper(contentPanel))
            {
                SizeF maxSize = new SizeF((Size)txtMaxSize.Value);

                SizeF measureSize = GetVisualElement().Measure(helper, SizeF.Empty, maxSize);
                txtSize.Value = DevAge.Drawing.Utilities.SizeFToSize(measureSize);
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            contentPanel.Invalidate();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            contentPanel.Invalidate();
        }

        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridVisualElement.SelectedObject = GetVisualElement();
            contentPanel.Invalidate();
        }

        private void contentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            using (DevAge.Drawing.MeasureHelper measure = new DevAge.Drawing.MeasureHelper(contentPanel))
            {
                PointF mousePoint = contentPanel.PointToClient(Control.MousePosition);

                DevAge.Drawing.VisualElements.VisualElementList list;
                list = GetVisualElement().GetElementsAtPoint(measure, contentPanel.ClientRectangle, mousePoint);

                string text = "";

                foreach (DevAge.Drawing.VisualElements.IVisualElement element in list)
                    text += element.ToString() + " ->";

                toolTip.SetToolTip(contentPanel, text);
            }
        }
    }
}