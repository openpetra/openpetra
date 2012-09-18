using System;
using System.Windows.Forms;

namespace SourceGrid.Cells.Virtual
{
	/// <summary>
	/// A Cell with a CheckBox. This Cell is of type bool. Abstract, you must override GetValue and SetValue.
	/// </summary>
	public class CheckBox : CellVirtual
	{
		/// <summary>
		/// Constructor of a CheckBox style cell. You must st a valid Model to use this type of cell with this constructor.
		/// </summary>
		public CheckBox()
		{
			View = Views.CheckBox.Default;
			AddController(Controllers.CheckBox.Default);
			AddController(Controllers.MouseInvalidate.Default);

            //I use an editor to validate the value and also because the space key are 
            // directly used by the controller, so must not start an edit operation but must still edit the value
            Editor = new Editors.EditorBase(typeof(bool));
			Editor.EditableMode = SourceGrid.EditableMode.None;

			Model.AddModel(new Models.CheckBox());
		}
	}
}

namespace SourceGrid.Cells
{
	/// <summary>
	/// A Cell with a CheckBox. This Cell is of type bool.
	/// </summary>
	public class CheckBox : Cell
	{
		#region Constructor
	
		/// <summary>
		/// Constrcutor
		/// </summary>
		public CheckBox():this(null, false)
		{
		}

		/// <summary>
		/// Construct a CellCheckBox class with caption and align checkbox in the MiddleLeft, using BehaviorModels.CheckBox.Default
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="checkValue"></param>
		public CheckBox(string caption, bool? checkValue):base(checkValue)
		{
			if (caption != null && caption.Length > 0)
				View = Views.CheckBox.MiddleLeftAlign;
			else
				View = Views.CheckBox.Default;

			Model.AddModel(new Models.CheckBox());
			AddController(Controllers.CheckBox.Default);
			AddController(Controllers.MouseInvalidate.Default);

            //I use an editor to validate the value and also because the space key are 
            // directly used by the controller, so must not start an edit operation but must still edit the value
			Editor = new Editors.EditorBase(typeof(bool));
			Editor.EditableMode = SourceGrid.EditableMode.None;

			Caption = caption;
		}
		#endregion

		#region Properties
		private Models.CheckBox CheckBoxModel
		{
			get{return (Models.CheckBox)Model.FindModel(typeof(Models.CheckBox));}
		}
		/// <summary>
		/// Checked status (equal to the Value property but returns a bool)
		/// </summary>
		public bool? Checked
		{
			get{return CheckBoxModel.GetCheckBoxStatus(GetContext()).Checked;}
			set{CheckBoxModel.SetCheckedValue(GetContext(), value);}
		}

		/// <summary>
		/// Caption of the cell
		/// </summary>
		public string Caption
		{
			get{return CheckBoxModel.Caption;}
			set{CheckBoxModel.Caption = value;}
		}
		#endregion
	}
}
