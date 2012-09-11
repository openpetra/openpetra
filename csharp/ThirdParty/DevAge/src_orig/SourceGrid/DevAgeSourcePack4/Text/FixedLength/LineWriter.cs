using System;

namespace DevAge.Text.FixedLength
{
	/// <summary>
	/// A class used to create fixed lenght string.
	/// </summary>
	public class LineWriter
	{
		private FieldList mFields;
		private IField[] mSortedList;
		public LineWriter(FieldList fields)
		{
			mFields = fields;
		}
		public LineWriter(Type lineClassType)
		{
			mFields = Utilities.ExtractFieldListFromType(lineClassType);
		}

		public FieldList Fields
		{
			get{return mFields;}
		}
		private char mSeparator = '\0';
		public char Separator
		{
			get{return mSeparator;}
			set{mSeparator = value;}
		}

		private object[] mLineValues;
		public void Reset()
		{
			mLineValues = null;
		}

		public void SetValue(string fieldName, object val)
		{
			if (mLineValues == null)
				mLineValues = new object[mFields.Count];
			if (mSortedList == null)
				mSortedList = mFields.GetSortedList();

			mLineValues[mFields[fieldName].Index] = val;
		}

		public string CreateLine()
		{
			if (mLineValues == null)
				throw new ArgumentNullException("mLineValues", "SetValue not called");
			if (mSortedList == null)
				mSortedList = mFields.GetSortedList();

			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			for (int i = 0; i < mSortedList.Length; i++)
			{
				builder.Append( mSortedList[i].ValueToString(mLineValues[i]) );
	
				if (Separator != '\0')
					builder.Append(Separator);
			}

			return builder.ToString();
		}

		public string CreateLineFromClass(object schemaClass)
		{
			System.Reflection.PropertyInfo[] properties = schemaClass.GetType().GetProperties(System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

			foreach (System.Reflection.PropertyInfo prop in properties)
			{
				object[] attributes = prop.GetCustomAttributes(typeof(FieldAttribute), true);
				if (attributes.Length > 0)
				{
					//FieldAttribute fieldAttr = (FieldAttribute)attributes[0];
					SetValue(prop.Name, prop.GetValue(schemaClass, null));
				}
			}

			return CreateLine();
		}
	}
}
