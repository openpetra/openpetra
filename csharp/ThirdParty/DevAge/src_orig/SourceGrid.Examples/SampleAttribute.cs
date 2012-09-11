using System;

namespace WindowsFormsSample
{
	/// <summary>
	/// Summary description for SampleAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class SampleAttribute : Attribute
	{
		private string m_category;
		private int m_sampleNumber;
		private string m_description;
		public SampleAttribute(string category, int sampleNumber, string description)
		{
			m_category = category;
			m_sampleNumber = sampleNumber;
			m_description = description;
		}
		public string Category
		{
			get{return m_category;}
		}
		public int SampleNumber
		{
			get{return m_sampleNumber;}
		}
		public string Description
		{
			get{return m_description;}
		}
	}
}
