using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
    /// <summary>
    /// DevAgeComboBox has a typed Value property and the validating features using the Validator property.
    /// Set the Validator property and then call the ApplyValidatorRules method.
    /// </summary>
    public class DevAgeComboBox : System.Windows.Forms.ComboBox
    {
        #region Generic validation methods
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            object val;
            if (IsValidValue(out val) == false)
            {
                e.Cancel = true;
            }
            else
            {
                if (FormatValue && Validator != null)
                {
                    Text = Validator.ValueToDisplayString(val);
                }
            }
        }

        private bool mFormatValue = false;
        /// <summary>
        /// Gets or sets a property to enable or disable the automatic format of the Text when validating the control.
        /// Default false.
        /// </summary>
        [DefaultValue(false)]
        public bool FormatValue
        {
            get { return mFormatValue; }
            set { mFormatValue = value; }
        }

        private DevAge.ComponentModel.Validator.IValidator mValidator = null;
        /// <summary>
        /// Gets or sets the Validator class useded to validate the value and convert the text when using the Value property.
        /// You can use the ApplyValidatorRules method to apply the settings of the Validator directly to the ComboBox, for example the list of values.
        /// </summary>
        [DefaultValue(null)]
        public DevAge.ComponentModel.Validator.IValidator Validator
        {
            get { return mValidator; }
            set
            {
                if (mValidator != value)
                {
                    if (mValidator != null)
                        mValidator.Changed -= mValidator_Changed;

                    mValidator = value;
                    mValidator.Changed += mValidator_Changed;
                    ApplyValidatorRules();
                }
            }
        }

        void mValidator_Changed(object sender, EventArgs e)
        {
            ApplyValidatorRules();
        }

        ///// <summary>
        ///// Apply the current Validator rules. This method is automatically fired when the Validator change.
        ///// </summary>
        //protected virtual void ApplyValidatorRules()
        //{

        //}

        /// <summary>
        /// Check if the selected value is valid based on the current validator and returns the value.
        /// </summary>
        /// <param name="convertedValue"></param>
        /// <returns></returns>
        public bool IsValidValue(out object convertedValue)
        {
            //Note:
            // SelectedValue is only valid when data binding is active otherwise
            // you must use SelectedItem

            object valToCheck;
            if (this.SelectedValue != null)
                valToCheck = this.SelectedValue;
            else if (this.SelectedItem != null)
                valToCheck = this.SelectedItem;
            else
                valToCheck = this.Text;

            if (Validator != null)
            {
                if (Validator.IsValidObject(valToCheck, out convertedValue))
                    return true;
                else
                    return false;
            }
            else
            {
                convertedValue = valToCheck;
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the typed value for the control, using the Validator class.
        /// If the Validator is ull the Text property is used.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                object val;
                if (IsValidValue(out val))
                    return val;
                else
                    throw new ArgumentOutOfRangeException("Text");
            }
            set
            {
                if (Validator != null)
                {
                    Text = Validator.ValueToDisplayString(value);
                }
                else
                {
                    if (value == null)
                        Text = "";
                    else
                        Text = value.ToString();
                }
            }
        }

        #endregion

        /// <summary>
        /// Loads the Items from the StandardValues and the DropDownStyle based on the parameters of the validator.
        /// Apply the current Validator rules. This method is automatically fired when the Validator change.
        /// </summary>
        protected virtual void ApplyValidatorRules()
        {
            Items.Clear();
            if (Validator != null && Validator.StandardValues != null)
            {
                foreach (object val in Validator.StandardValues)
                    Items.Add(val);

                if (Validator.IsStringConversionSupported())
                    DropDownStyle = ComboBoxStyle.DropDown;
                else
                    DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        protected override void OnFormat(ListControlConvertEventArgs e)
        {
            base.OnFormat(e);

            // The method converts only to string type. 
            if (e.DesiredType != typeof(string) || Validator == null)
                return;

            e.Value = Validator.ValueToDisplayString(e.ListItem);
        }

    }
}
