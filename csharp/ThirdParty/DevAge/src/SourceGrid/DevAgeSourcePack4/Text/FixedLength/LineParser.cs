using System;

namespace DevAge.Text.FixedLength
{
	/// <summary>
	/// A class for parsing fixed length string and loading the fields into a class.
	/// </summary>
	public class LineParser
	{
		private FieldList mFields;
		private System.Text.RegularExpressions.Regex mRegEx;
		/// <summary>
		/// Constructor. Fill the Fields list fot specify the columns.
		/// </summary>
		public LineParser()
		{
			mFields = new FieldList();
		}

		/// <summary>
		/// Load the parser fields with the properties specified in the type. You must use the FieldAttribute and ParseFormatAttribute to specify additional informations like the field length.
		/// </summary>
		/// <param name="lineClassType"></param>
		public LineParser(Type lineClassType)
		{
			mFields = Utilities.ExtractFieldListFromType(lineClassType);
		}

		/// <summary>
		/// Gets a collection of fields.
		/// </summary>
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

		/// <summary>
		/// Reset the parser
		/// </summary>
		public void Reset()
		{
			mRegEx = null;
			mRegExMatch = null;
		}

		private System.Text.RegularExpressions.Match mRegExMatch;
		/// <summary>
		/// Load the specified line in the parser.
		/// </summary>
		/// <param name="line"></param>
		public void LoadLine(string line)
		{
			if (mRegEx == null)
				mRegEx = CreateLineRegExp();

			mRegExMatch = mRegEx.Match(line);
		}

		/// <summary>
		/// Get a specified field value.
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public object GetValue(string fieldName)
		{
			if (mRegExMatch == null)
				throw new ArgumentNullException("mRegExMatch");

			System.Text.RegularExpressions.Group group = mRegExMatch.Groups[fieldName];
			if (group.Success == false)
				throw new RegExException(fieldName);
			
			return mFields[fieldName].StringToValue( group.Value );
		}

		private System.Text.RegularExpressions.Regex CreateLineRegExp()
		{
			//esempio: @"^(?<colA>.{6})(?<colB>.{8})(?<colC>.+)$"
			// * Specifies zero or more matches; for example, \w* or (abc)*. Equivalent to {0,}. 
			// + Specifies one or more matches; for example, \w+ or (abc)+. Equivalent to {1,}. 

			IField[] fields = mFields.GetSortedList();

			string pattern = "^";
			for (int i = 0; i < fields.Length; i++)
			{
				pattern += fields[i].RegularExpressionPattern;
				pattern += Utilities.ValidateRegExpSeparator(Separator);
			}
			//pattern += "$"; //commettato per non cercare necessariamente la fine riga, che quindi necessitava di specificare tutti i campi precisi.

			return new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Singleline);
		}


		/// <summary>
		/// Fill the properties of the specified class with the values of the line has defined by the Fields collection.
		/// </summary>
		/// <param name="schemaClass"></param>
		/// <returns>Returns the same class specified in the schemaClass parameter, this is useful if you have struct or value types.</returns>
		public object FillLineClass(object schemaClass)
		{
			if (mRegExMatch == null)
				throw new ArgumentNullException("mRegExMatch", "LoadLine not called");

			System.Reflection.PropertyInfo[] properties = schemaClass.GetType().GetProperties(System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

			foreach (System.Reflection.PropertyInfo prop in properties)
			{
				object[] attributes = prop.GetCustomAttributes(typeof(FieldAttribute), true);
				if (attributes.Length > 0)
				{
					//FieldAttribute fieldAttr = (FieldAttribute)attributes[0];
					prop.SetValue(schemaClass, GetValue(prop.Name), null);
				}
			}

			return schemaClass;
		}
	}
}
