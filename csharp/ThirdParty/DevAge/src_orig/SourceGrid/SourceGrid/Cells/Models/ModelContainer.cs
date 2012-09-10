using System;

namespace SourceGrid.Cells.Models
{
	/// <summary>
	/// A container for the model classes. THe only required model is the Value model, that you can assign using the ValueModel property.
	/// </summary>
	public class ModelContainer
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelContainer()
		{
		}

		private ModelList m_ModelList = null;
		/// <summary>
		/// A collection of elements of type IModel
		/// </summary>
		public class ModelList : DevAge.Collections.ListByType<IModel>
		{
		}


		/// <summary>
		/// Returns null if not exist
		/// </summary>
		/// <param name="modelType"></param>
		/// <returns></returns>
		public virtual IModel FindModel(Type modelType)
		{
			if (m_ValueModel != null && modelType.IsAssignableFrom(m_ValueModel.GetType()))
				return m_ValueModel;
			else
			{
				if (m_ModelList == null)
					m_ModelList = new ModelList();

				return m_ModelList.GetByType(modelType);
			}
		}


		public virtual ModelContainer AddModel(IModel model)
		{
			if (model == null)
				throw new ArgumentNullException();
			Type type = model.GetType();

			if (typeof(IValueModel).IsAssignableFrom(type))
				m_ValueModel = (IValueModel)model;
			else
			{
				if (m_ModelList == null)
					m_ModelList = new ModelList();

				m_ModelList.Add(model);
			}
			return this;
		}

		public virtual ModelContainer RemoveModel(IModel model)
		{
			if (object.ReferenceEquals(model, m_ValueModel))
				m_ValueModel = null;
			else if (m_ModelList != null)
				m_ModelList.Remove(model);
			return this;
		}

		private IValueModel m_ValueModel;
		public virtual IValueModel ValueModel
		{
			get{return m_ValueModel;}
			set{m_ValueModel = value;}
		}
	}
}
