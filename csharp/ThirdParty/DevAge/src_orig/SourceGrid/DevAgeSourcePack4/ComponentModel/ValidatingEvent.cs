using System;

namespace DevAge.ComponentModel
{
	public class ConvertingObjectEventArgs : System.EventArgs
	{
		private object m_Value;
		private Type m_DestinationType;
		private ConvertingStatus m_ConvertingStatus = ConvertingStatus.Converting;
		public ConvertingObjectEventArgs(object p_Value, Type p_DestinationType)
		{
			m_Value = p_Value;
			m_DestinationType = p_DestinationType;
		}
		public object Value
		{
			get{return m_Value;}
			set{m_Value = value;}
		}

		/// <summary>
		/// Destination type to convert the Value. Can be null if no destination type is required.
		/// </summary>
		public Type DestinationType
		{
			get{return m_DestinationType;}
		}
		public ConvertingStatus ConvertingStatus
		{
			get{return m_ConvertingStatus;}
			set{m_ConvertingStatus = value;}
		}
	}
	public delegate void ConvertingObjectEventHandler(object sender, ConvertingObjectEventArgs e);

	public enum ConvertingStatus
	{
		Converting = 0,
		Error = 1,
		Completed = 2
	}

	public class ValueEventArgs : System.EventArgs
	{
		private object m_Value;
		public ValueEventArgs(object p_Value)
		{
			m_Value = p_Value;
		}
		public object Value
		{
			get{return m_Value;}
			set{m_Value = value;}
		}

	}
	public delegate void ValueEventHandler(object sender, ValueEventArgs e);

}
