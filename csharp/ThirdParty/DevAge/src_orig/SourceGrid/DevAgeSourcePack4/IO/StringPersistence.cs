using System;

namespace DevAge.IO
{
//	/// <summary>
//	/// Summary description for StringPersistence.
//	/// </summary>
//	public class StringPersistence
//	{
//		public StringPersistence()
//		{
//		}
//
//		private static System.Globalization.NumberFormatInfo InvariantNumberFormat
//		{
//			get{return System.Globalization.CultureInfo.InvariantCulture.NumberFormat;}
//		}
//		private static System.Globalization.DateTimeFormatInfo InvariantDateFormat
//		{
//			get{return System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat;}
//		}
//
//		#region Write
//		public static string Write(int val)
//		{
//			return val.ToString(InvariantNumberFormat);
//		}
//		public static string Write(double val)
//		{
//			return val.ToString(InvariantNumberFormat);
//		}
//		public static string Write(decimal val)
//		{
//			return val.ToString(InvariantNumberFormat);
//		}
//		public static string Write(string val)
//		{
//			return val;
//		}
//		public static string Write(char val)
//		{
//			return val.ToString();
//		}
//		public static string Write(long val)
//		{
//			return val.ToString(InvariantNumberFormat);
//		}
//		public static string Write(DateTime val)
//		{
//			return val.ToString(InvariantDateFormat);
//		}
//		/// <summary>
//		/// Use Base64 encoding
//		/// </summary>
//		/// <param name="val"></param>
//		/// <returns></returns>
//		public static string Write(byte val)
//		{
//			return System.Convert.ToBase64String(new byte[]{val});
//		}
//		/// <summary>
//		/// Use Base64 encoding
//		/// </summary>
//		/// <param name="val"></param>
//		/// <returns></returns>
//		public static string Write(byte[] val)
//		{
//			return System.Convert.ToBase64String(val);
//		}
//
//		public static string Write(Type pType, object val)
//		{
//			if (pType.Equals(typeof(int)))
//				return Write((int)val);
//
//			if (pType.Equals(typeof(double)))
//				return Write((double)val);
//
//			if (pType.Equals(typeof(decimal)))
//				return Write((decimal)val);
//
//			if (pType.Equals(typeof(string)))
//				return Write((string)val);
//
//			if (pType.Equals(typeof(char)))
//				return Write((char)val);
//
//			if (pType.Equals(typeof(long)))
//				return Write((long)val);
//
//			if (pType.Equals(typeof(DateTime)))
//				return Write((DateTime)val);
//
//			if (pType.Equals(typeof(byte)))
//				return Write((byte)val);
//
//			if (pType.Equals(typeof(byte[])))
//				return Write((byte[])val);
//
//			throw new TypeNotSupportedException(pType);
//		}
//		#endregion
//
//		#region Read
//		public static void Read(string source, out int val)
//		{
//			val = int.Parse(source, InvariantNumberFormat);
//		}
//		public static void Read(string source, out double val)
//		{
//			val = double.Parse(source, InvariantNumberFormat);
//		}
//		public static void Read(string source, out decimal val)
//		{
//			val = decimal.Parse(source, InvariantNumberFormat);
//		}
//		public static void Read(string source, out string val)
//		{
//			val = source;
//		}
//		public static void Read(string source, out char val)
//		{
//			val = char.Parse(source);
//		}
//		public static void Read(string source, out long val)
//		{
//			val = long.Parse(source, InvariantNumberFormat);
//		}
//		public static void Read(string source, out DateTime val)
//		{
//			val = DateTime.Parse(source, InvariantDateFormat);
//		}
//		/// <summary>
//		/// Use Base64 encoding
//		/// </summary>
//		/// <param name="val"></param>
//		/// <returns></returns>
//		public static void Read(string source, out byte val)
//		{
//			val = Convert.FromBase64String(source)[0];
//		}
//		/// <summary>
//		/// Use Base64 encoding
//		/// </summary>
//		/// <param name="val"></param>
//		/// <returns></returns>
//		public static void Read(string source, out byte[] val)
//		{
//			val = Convert.FromBase64String(source);
//		}
//
//		public static object Read(Type pType, string source)
//		{
//			if (pType.Equals(typeof(int)))
//			{
//				int val;
//				Read(source, out val);
//				return val;
//			}
//
//			if (pType.Equals(typeof(double)))
//			{
//				double val;
//				Read(source, out val);
//				return val;
//			}
//
//			if (pType.Equals(typeof(decimal)))
//			{
//				decimal val;
//				Read(source, out val);
//				return val;
//			}
//
//			if (pType.Equals(typeof(string)))
//			{
//				string val;
//				Read(source, out val);
//				return val;
//			}
//
//			if (pType.Equals(typeof(char)))
//			{
//				char val;
//				Read(source, out val);
//				return val;
//			}
//
//			if (pType.Equals(typeof(long)))
//			{
//				long val;
//				Read(source, out val);
//				return val;
//			}
//
//			if (pType.Equals(typeof(DateTime)))
//			{
//				DateTime val;
//				Read(source, out val);
//				return val;
//			}
//
//			if (pType.Equals(typeof(byte)))
//			{
//				byte val;
//				Read(source, out val);
//				return val;
//			}
//
//			if (pType.Equals(typeof(byte[])))
//			{
//				byte[] val;
//				Read(source, out val);
//				return val;
//			}
//
//			throw new TypeNotSupportedException(pType);
//		}
//		#endregion
//
//	}
}
