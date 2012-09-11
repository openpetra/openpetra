using System;

namespace DevAge.Text.FixedLength
{
	public class Utilities
	{
		public static DevAge.ComponentModel.Validator.IValidator CreateValidator(Type type, ParseFormatAttribute parseAttributes)
		{
            System.ComponentModel.TypeConverter converter;

            //Check for nullable
            bool nullable;
            type = GetBaseType(type, out nullable);

            converter = GetConverterFromPrimitiveType(type, parseAttributes);


			DevAge.ComponentModel.Validator.ValidatorTypeConverter Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(type, converter);
			Validator.CultureInfo = parseAttributes.CultureInfo;
			Validator.NullString = "";
			Validator.NullDisplayString = "";
            Validator.AllowNull = nullable;

			return Validator;
		}

        private static Type GetBaseType(Type type, out bool isNullable)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                isNullable = true;
                return Nullable.GetUnderlyingType(type);
            }
            else
            {
                isNullable = false;
                return type;
            }
        }

        private static System.ComponentModel.TypeConverter GetConverterFromPrimitiveType(Type type, ParseFormatAttribute parseAttributes)
        {
            if (type == typeof(string))
                return new System.ComponentModel.StringConverter();
            else if (type == typeof(int))
                return new System.ComponentModel.Int32Converter();
            else if (type == typeof(double))
                return new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(double), parseAttributes.NumberFormat);
            else if (type == typeof(decimal))
                return new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(decimal), parseAttributes.NumberFormat);
            else if (type == typeof(DateTime))
                return new DevAge.ComponentModel.Converter.DateTimeTypeConverter(parseAttributes.DateTimeFormat, new string[] { parseAttributes.DateTimeFormat });
            else
                throw new TypeNotSupportedException(type);
        }

		public static FieldList ExtractFieldListFromType(Type classType)
		{
			FieldList fields = new FieldList();

			System.Reflection.PropertyInfo[] properties = classType.GetProperties(System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

			foreach (System.Reflection.PropertyInfo prop in properties)
			{
				FieldAttribute fieldAttr = null;
				ParseFormatAttribute parseFormat = null;

				object[] attributes = prop.GetCustomAttributes(typeof(FieldAttribute), true);
				if (attributes.Length > 0)
					fieldAttr = (FieldAttribute)attributes[0];

				attributes = prop.GetCustomAttributes(typeof(ParseFormatAttribute), true);
				if (attributes.Length > 0)
					parseFormat = (ParseFormatAttribute)attributes[0];

				object[] valueMappings = prop.GetCustomAttributes(typeof(ValueMappingAttribute), true);

				object[] standardValues = prop.GetCustomAttributes(typeof(StandardValueAttribute), true);

				if (fieldAttr != null)
				{
					DevAge.ComponentModel.Validator.IValidator validator;
					if (parseFormat == null)
						parseFormat = new ParseFormatAttribute();

					validator = CreateValidator(prop.PropertyType, parseFormat);

					Field field = new Field(fieldAttr.FieldIndex, prop.Name, fieldAttr.Length, validator);
					field.TrimBeforeParse = parseFormat.TrimBeforeParse;
					fields.Add(field);

					//ValueMapping - to convert specific values, can be an array of attribute, one for each conversion
					if (valueMappings.Length > 0)
					{
						ComponentModel.Validator.ValueMapping mapping = new DevAge.ComponentModel.Validator.ValueMapping();
						object[] valList = new object[valueMappings.Length];
						object[] strList = new object[valueMappings.Length];
						for (int i = 0; i < valueMappings.Length; i++)
						{
							//I convert the value assigned to the attribute to ensure that the field is valid and to support DateTime (that cannot be directly assigned to an attribute but must be declared as string)
							valList[i] = validator.ObjectToValue( ((ValueMappingAttribute)valueMappings[i]).FieldValue );

							strList[i] = ((ValueMappingAttribute)valueMappings[i]).StringValue;
						}
						
						mapping.ThrowErrorIfNotFound = false;
						mapping.ValueList = valList;
						mapping.SpecialList = strList;
						mapping.SpecialType = typeof(string);
						mapping.BindValidator(validator);
					}

					//StandardValues
					if (standardValues.Length > 0)
					{
						object[] valList = new object[standardValues.Length];
						for (int i = 0; i < standardValues.Length; i++)
						{
							valList[i] = ((StandardValueAttribute)standardValues[i]).StandardValue;
						}

						validator.StandardValues = valList;
						validator.StandardValuesExclusive = true;
					}
				}
			}

			return fields;
		}

		public static string ValidateRegExpSeparator(char separator)
		{
			if (separator == 0)
				return string.Empty;
			else
			{

				// See MSDN: "Character Escapes"
				// Characters other than . $ ^ { [ ( | ) * + ? \ match themselves.
				switch (separator)
				{
					case '$':
					case '^':
					case '{':
					case '[':
					case '(':
					case '|':
					case ')':
					case '*':
					case '+':
					case '?':
					case '\\':
						return @"\" + separator;
					default:
						return separator.ToString();
				}
			}
		}
	}
}
