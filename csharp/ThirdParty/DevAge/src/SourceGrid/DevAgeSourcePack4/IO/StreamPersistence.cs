using System;
using System.IO;

namespace DevAge.IO
{
	/// <summary>
	/// A static class to help save and read stream data
	/// </summary>
	public static class StreamPersistence
	{
		#region Write Function
		public static void Write(Stream p_Stream, String p_Value, System.Text.Encoding encoding)
		{
			byte[] bytes = encoding.GetBytes(p_Value);
			Write(p_Stream, bytes );
		}
		public static void Write(Stream p_Stream, Byte p_Value)
		{
			p_Stream.WriteByte(p_Value);
		}
		public static void Write(Stream p_Stream, Guid p_Value)
		{
			byte[] vBits = p_Value.ToByteArray();
			Write(p_Stream, vBits);
		}
		public static void Write(Stream p_Stream, Decimal p_Value)
		{
			int[] vBits = Decimal.GetBits(p_Value);
			Write(p_Stream, vBits[0]);
			Write(p_Stream, vBits[1]);
			Write(p_Stream, vBits[2]);
			Write(p_Stream, vBits[3]);
		}
		public static void Write(Stream p_Stream, DateTime p_Value)
		{
			Write(p_Stream, p_Value.ToOADate());
		}
		public static void Write(Stream p_Stream, Int16 p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, Int32 p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, Int64 p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, Single p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, Double p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, Char p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}
		public static void Write(Stream p_Stream, Boolean p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}
		[CLSCompliant(false)]
		public static void Write(Stream p_Stream, UInt16 p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}
		[CLSCompliant(false)]
		public static void Write(Stream p_Stream, UInt32 p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}
		[CLSCompliant(false)]
		public static void Write(Stream p_Stream, UInt64 p_Value)
		{
			WriteBytes(p_Stream, BitConverter.GetBytes(p_Value));
		}

		public static void Write(Stream p_Stream, Byte[] p_Bytes)
		{
			Write(p_Stream, p_Bytes.Length);
			WriteBytes(p_Stream, p_Bytes);
		}


		public static void Write(Stream stream, Type valueType, object val)
		{
			if (valueType == typeof(String))
				Write(stream, (String)val, System.Text.Encoding.UTF8);
			else if (valueType == typeof(Int16))
				Write(stream, (Int16)val);
			else if (valueType == typeof(Int32))
				Write(stream, (Int32)val);
			else if (valueType == typeof(Int64))
				Write(stream, (Int64)val);
			else if (valueType == typeof(Single))
				Write(stream, (Single)val);
			else if (valueType == typeof(Double))
				Write(stream, (Double)val);
			else if (valueType == typeof(Boolean))
				Write(stream, (Boolean)val);
			else if (valueType == typeof(Decimal))
				Write(stream, (Decimal)val);
			else if (valueType == typeof(Char))
				Write(stream, (Char)val);
			else if (valueType == typeof(Byte))
				Write(stream, (Byte)val);
			else if (valueType == typeof(Byte[]))
				Write(stream, (Byte[])val);
			else if (valueType == typeof(DateTime))
				Write(stream, (DateTime)val);
			else if (valueType == typeof(Guid))
				Write(stream, (Guid)val);
			else
				throw new TypeNotSupportedException(valueType);

		}
		
		public static void WriteBytes(Stream p_Stream, byte[] p_Bytes)
		{
			p_Stream.Write(p_Bytes, 0, p_Bytes.Length);
		}
		#endregion

		#region Read Function
		public static Guid ReadGuid(Stream stream)
		{
			Guid val;
			byte[] bytesArray = ReadByteArray(stream);
			val = new Guid(bytesArray);
			return val;
		}
		public static Decimal ReadDecimal(Stream stream)
		{
			Decimal val;
			Int32 v1 = ReadInt32(stream);
			Int32 v2 = ReadInt32(stream);
			Int32 v3 = ReadInt32(stream);
			Int32 v4 = ReadInt32(stream);
			val = new decimal(new int[]{v1, v2, v3, v4});
			return val;
		}
		public static DateTime ReadDateTime(Stream p_Stream)
		{
			DateTime val;
			double dbl = ReadDouble(p_Stream);
			val = DateTime.FromOADate(dbl);
			return val;
		}
		public static Single ReadSingle(Stream p_Stream)
		{
			Single val;
			byte[] l_tmp = BitConverter.GetBytes((Single)0.0f);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToSingle(l_tmp, 0);
			return val;
		}
		public static Double ReadDouble(Stream p_Stream)
		{
			Double val;
			byte[] l_tmp = BitConverter.GetBytes((Double)0.0f);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToDouble(l_tmp, 0);
			return val;
		}
		public static Int16 ReadInt16(Stream p_Stream)
		{
			Int16 val;
			byte[] l_tmp = BitConverter.GetBytes((Int16)0);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToInt16(l_tmp,0);
			return val;
		}
		public static Int32 ReadInt32(Stream p_Stream)
		{
			Int32 val;
			byte[] l_tmp = BitConverter.GetBytes((Int32)0);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToInt32(l_tmp,0);
			return val;
		}
		public static Int64 ReadInt64(Stream p_Stream)
		{
			Int64 val;
			byte[] l_tmp = BitConverter.GetBytes((Int64)0);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToInt64(l_tmp,0);
			return val;
		}

		[CLSCompliant(false)]
		public static UInt16 ReadUInt16(Stream p_Stream)
		{
			System.UInt16 val;
			byte[] l_tmp = BitConverter.GetBytes((UInt16)0);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToUInt16(l_tmp,0);
			return val;
		}
		
		[CLSCompliant(false)]
		public static UInt32 ReadUInt32(Stream p_Stream)
		{
			System.UInt32 val;
			byte[] l_tmp = BitConverter.GetBytes((UInt32)0);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToUInt32(l_tmp,0);
			return val;
		}
		
		[CLSCompliant(false)]
		public static UInt64 ReadUInt64(Stream p_Stream)
		{
			System.UInt64 val;
			byte[] l_tmp = BitConverter.GetBytes((UInt64)0);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToUInt64(l_tmp,0);
			return val;
		}
		public static Byte ReadByte(Stream p_Stream)
		{
			int byteVal = p_Stream.ReadByte();
			if (byteVal == -1)
				throw new InvalidDataException();

			return (Byte)byteVal;
		}
		public static char ReadChar(Stream p_Stream)
		{
			char val;
			byte[] l_tmp = BitConverter.GetBytes((char)0);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToChar(l_tmp,0);
			return val;
		}
		public static bool ReadBoolean(Stream p_Stream)
		{
			bool val;
			byte[] l_tmp = BitConverter.GetBytes((bool)false);
			ReadBytes(p_Stream, l_tmp);
			val = BitConverter.ToBoolean(l_tmp, 0);
			return val;
		}
		public static string ReadString(Stream p_Stream, System.Text.Encoding encoding)
		{
			string val;
			byte[] bytesString = ReadByteArray(p_Stream);
			val = encoding.GetString(bytesString);
			return val;
		}
		public static byte[] ReadByteArray(Stream p_Stream)
		{
			byte[] val;
			int len = ReadInt32(p_Stream);
			val = new byte[len];
			ReadBytes(p_Stream, val);
			return val;
		}

		public static object Read(Stream stream, Type valueType)
		{
			if (valueType == typeof(String))
				return ReadString(stream, System.Text.Encoding.UTF8);
			else if (valueType == typeof(Char))
				return ReadChar(stream);
			else if (valueType == typeof(Boolean))
				return ReadBoolean(stream);
			else if (valueType == typeof(Decimal))
				return ReadDecimal(stream);
			else if (valueType == typeof(Int16))
				return ReadInt16(stream);
			else if (valueType == typeof(Int32))
				return ReadInt32(stream);
			else if (valueType == typeof(Int64))
				return ReadInt64(stream);
			else if (valueType == typeof(Single))
				return ReadSingle(stream);
			else if (valueType == typeof(Double))
				return ReadDouble(stream);
			else if (valueType == typeof(Byte))
				return ReadByte(stream);
			else if (valueType == typeof(Byte[]))
				return ReadByteArray(stream);
			else if (valueType == typeof(DateTime))
				return ReadDateTime(stream);
			else if (valueType == typeof(Guid))
				return ReadGuid(stream);
			else
				throw new TypeNotSupportedException(valueType);
		}

		public static void ReadBytes(Stream p_Stream, byte[] p_Value)
		{
			if (p_Stream.Read(p_Value,0,p_Value.Length) != p_Value.Length)
				throw new InvalidDataException();
		}
		#endregion
	}


	[Serializable]
	public class InvalidDataException : DevAgeApplicationException  
	{
		public InvalidDataException():
			base("Invalid data exception")
		{
		}

		public InvalidDataException(string p_strErrDescription):
			base(p_strErrDescription)
		{
		}
		public InvalidDataException(string p_strErrDescription, Exception p_InnerException):
			base(p_strErrDescription, p_InnerException)
		{
		}
		protected InvalidDataException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
	}

	[Serializable]
	public class TypeNotSupportedException : DevAgeApplicationException
	{
		public TypeNotSupportedException(Type pType):
			base("Type not supported: " + pType.ToString())
		{
		}

		public TypeNotSupportedException(string p_strErrDescription):
			base(p_strErrDescription)
		{
		}
		public TypeNotSupportedException(string p_strErrDescription, Exception p_InnerException):
			base(p_strErrDescription, p_InnerException)
		{
		}
		protected TypeNotSupportedException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
	}
}