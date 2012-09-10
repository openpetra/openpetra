using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;
using System.Drawing.Design;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// A TextBoxTypedButton that uase the UITypeEditor associated with the type.
	/// </summary>
	public class TextBoxUITypeEditor : DevAgeTextBoxButton, IServiceProvider, System.Windows.Forms.Design.IWindowsFormsEditorService, ITypeDescriptorContext
	{
		private System.ComponentModel.IContainer components = null;

		public TextBoxUITypeEditor()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		public override void ShowDialog()
		{
			try
			{
				OnDialogOpen(EventArgs.Empty);
				if (m_UITypeEditor != null)
				{
					UITypeEditorEditStyle style = m_UITypeEditor.GetEditStyle();
                    if (style == UITypeEditorEditStyle.DropDown ||
                        style == UITypeEditorEditStyle.Modal)
					{
						object editObject;
                        //Try to read the actual value, if the function failed I edit the default value
                        if (IsValidValue(out editObject) == false)
                        {
                            if (Validator != null)
                                editObject = Validator.DefaultValue;
                            else
                                editObject = null;
                        }

                        object tmp = m_UITypeEditor.EditValue(this, this, editObject);
						Value = tmp;
					}
				}

				OnDialogClosed(EventArgs.Empty);
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message,"Error");
			}
		}

		private UITypeEditor m_UITypeEditor;

        /// <summary>
        /// Gets or sets the UITypeEditor to use. If you have specified a validator the TypeDescriptor.GetEditor method is used based on the Validator.ValueType.
        /// </summary>
        [DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UITypeEditor UITypeEditor
		{
			get{return m_UITypeEditor;}
			set{m_UITypeEditor = value;}
		}

        //public bool ShouldSerializeUITypeEditor()
        //{
        //    return m_UITypeEditor != m_DefaultUITypeEditor;
        //}

        protected override void ApplyValidatorRules()
        {
            base.ApplyValidatorRules();

            if (m_UITypeEditor == null && Validator != null)
            {
                object tmp = System.ComponentModel.TypeDescriptor.GetEditor(Validator.ValueType, typeof(UITypeEditor));
                if (tmp is UITypeEditor)
                    m_UITypeEditor = (UITypeEditor)tmp;
            }
        }

		#region IServiceProvider Members
		System.Object IServiceProvider.GetService ( System.Type serviceType )
		{
			//modal
			if (serviceType == typeof(System.Windows.Forms.Design.IWindowsFormsEditorService))
				return this;

			return null;
		}
		#endregion

		#region System.Windows.Forms.Design.IWindowsFormsEditorService
		private DevAge.Windows.Forms.DropDown m_dropDown = null;
		public virtual void CloseDropDown ()
		{
			if (m_dropDown != null)
			{
				m_dropDown.CloseDropDown();
			}
		}

		public virtual void DropDownControl ( System.Windows.Forms.Control control )
		{
            using (m_dropDown = new DevAge.Windows.Forms.DropDown(control, this, this.ParentForm))
            {
                m_dropDown.DropDownFlags = DevAge.Windows.Forms.DropDownFlags.CloseOnEscape;

                m_dropDown.ShowDropDown();

                m_dropDown.Close();
            }
            m_dropDown = null;
        }

		public virtual System.Windows.Forms.DialogResult ShowDialog ( System.Windows.Forms.Form dialog )
		{
			return dialog.ShowDialog(this);
		}
		#endregion

		#region ITypeDescriptorContext Members

		void ITypeDescriptorContext.OnComponentChanged()
		{
			
		}

		IContainer ITypeDescriptorContext.Container
		{
			get
			{
				return base.Container;
			}
		}

		bool ITypeDescriptorContext.OnComponentChanging()
		{
			return true;
		}

		object ITypeDescriptorContext.Instance
		{
			get
			{
				return Value;
			}
		}

		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		#endregion
	}
}

