using System;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// Summary description for BindProperty.
	/// </summary>
	public class BindProperty : ControllerBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_Property">Property of the object that you want to lint to the cell. When the value of the cell changed this behavior call automatically this property.</param>
		/// <param name="p_LinkObject">Instance of the object to link</param>
		public BindProperty(System.Reflection.PropertyInfo p_Property, object p_LinkObject)
		{
			BindValueAtProperty(p_Property, p_LinkObject);
		}

		/// <summary>
		/// OnValueChanged
		/// </summary>
		public override void OnValueChanged(CellContext sender, EventArgs e)
		{
			base.OnValueChanged (sender, e);

			//gestione eventuale link ad una property
			if (m_LinkPropertyInfo != null)
				m_LinkPropertyInfo.SetValue(m_LinkObject, sender.Cell.Model.ValueModel.GetValue(sender),null);
		}


		#region Bind Property
		//proprietà per gestire il link di una cella ad una property
		private System.Reflection.PropertyInfo m_LinkPropertyInfo = null;
		private object m_LinkObject = null;

		/// <summary>
		/// Bind the cell's value with the property p_Property of the object p_LinkObject
		/// when the cell's value change also the property change
		/// </summary>
		/// <param name="p_Property">linked property</param>
		/// <param name="p_LinkObject">Can be null to call static property</param>
		protected virtual void BindValueAtProperty(System.Reflection.PropertyInfo p_Property, object p_LinkObject)
		{
			m_LinkPropertyInfo = p_Property;
			m_LinkObject = p_LinkObject;
		}

		/// <summary>
		/// UnBind the cell with the property
		/// </summary>
		protected virtual void UnBindValueAtProperty()
		{
			m_LinkPropertyInfo = null;
			m_LinkObject = null;
		}

//		/// <summary>
//		/// Method that can be used to refresh the cell value
//		/// </summary>
//		/// <param name="sender"></param>
//		/// <param name="e"></param>
//		public void BindObject_ValueChanged(object sender, EventArgs e)
//		{
//			if (m_LinkPropertyInfo!=null)
//			{
//				object tmp = m_LinkPropertyInfo.GetValue(m_LinkObject,null);
//				if (tmp!=Value)
//					Value = tmp;
//			}
//		}
		#endregion

	}
}
