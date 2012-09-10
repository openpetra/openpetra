using System;
using System.Data;

namespace DevAge.Data
{
	/// <summary>
	/// Summary description for Manager.
	/// </summary>
	public abstract class FileDataSet
	{
		#region Instance Methdos and Propertis
		public FileDataSet()
		{
		}

		private StreamDataSetFormat mSaveDataFormat = StreamDataSetFormat.Binary;

		/// <summary>
		/// Gets or Sets the format used to save the DataSet
		/// </summary>
		protected StreamDataSetFormat SaveDataFormat
		{
			get{return mSaveDataFormat;}
			set{mSaveDataFormat = value;}
		}

		private StreamDataSetFormat mFileDataFormat;

		/// <summary>
		/// Gets the current format of the File where the data are loaded.
		/// </summary>
		protected StreamDataSetFormat FileDataFormat
		{
			get{return mFileDataFormat;}
		}

		private bool mMergeReadedSchema = false;
		/// <summary>
		/// Gets or Sets if merge the schema of the file with the schema of the DataSet specified. Use true if the DataSet doesn't have a schema definition. Default is false.
		/// </summary>
		protected bool MergeReadedSchema
		{
			get{return mMergeReadedSchema;}
			set{mMergeReadedSchema = value;}
		}


		private string m_FileName;
		public string FileName
		{
			get{return m_FileName;}
			set{m_FileName = value;}
		}
		protected abstract int GetDataVersion();
		protected abstract DataSet CreateData(int version);
		#endregion

		#region Load /Save
		private const string c_DataNamespace = "http://www.devage.com/FileDataSet";
		private const int c_FileVersionNumber = 1;
		private const string c_FileVersion = "fileversion";
		private const string c_DataVersion = "dataversion";
		private const string c_DataFormat = "dataformat";
		protected virtual void SaveToFile(DataSet pDataSet)
		{
			if (m_FileName == null)
				throw new ApplicationException("FileName is null");
			
			byte[] completeByteArray;
			using (System.IO.MemoryStream fileMemStream = new System.IO.MemoryStream())
			{
				System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(fileMemStream, System.Text.Encoding.UTF8);

				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement("filedataset", c_DataNamespace);

				xmlWriter.WriteStartElement("header", c_DataNamespace);
				//File Version
				xmlWriter.WriteAttributeString(c_FileVersion, c_FileVersionNumber.ToString());
				//Data Version
				xmlWriter.WriteAttributeString(c_DataVersion, GetDataVersion().ToString());
				//Data Format
				xmlWriter.WriteAttributeString(c_DataFormat, ((int)mSaveDataFormat).ToString());
				xmlWriter.WriteEndElement();

				xmlWriter.WriteStartElement("data", c_DataNamespace);

				byte[] xmlByteArray;
				using (System.IO.MemoryStream xmlMemStream = new System.IO.MemoryStream())
				{
					StreamDataSet.Write(xmlMemStream, pDataSet, mSaveDataFormat);
					//pDataSet.WriteXml(xmlMemStream);

					xmlByteArray = xmlMemStream.ToArray();
					xmlMemStream.Close();
				}

				xmlWriter.WriteBase64(xmlByteArray, 0, xmlByteArray.Length);
				xmlWriter.WriteEndElement();

				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndDocument();

				xmlWriter.Flush();
				
				completeByteArray = fileMemStream.ToArray();
				fileMemStream.Close();
			}

			//se tutto è andato a buon fine scrivo effettivamente il file
			using (System.IO.FileStream fileStream = new System.IO.FileStream(m_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
			{
				fileStream.Write(completeByteArray, 0, completeByteArray.Length);
				fileStream.Close();
			}
		}


		protected virtual DataSet LoadFromFile()
		{
			if (m_FileName == null)
				throw new ApplicationException("FileName is null");

			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
			xmlDoc.Load(m_FileName);

			System.Xml.XmlNamespaceManager namespaceMan = new System.Xml.XmlNamespaceManager(xmlDoc.NameTable);
			namespaceMan.AddNamespace("fileds", c_DataNamespace);

			System.Xml.XmlNode tmp = xmlDoc.DocumentElement.SelectSingleNode("fileds:header", namespaceMan);
			if (tmp == null)
				throw new ApplicationException("File header not found");
			System.Xml.XmlElement header = (System.Xml.XmlElement)tmp;

			//File Version
			string strFileVersion = header.GetAttribute(c_FileVersion);
			int fileVersion = int.Parse( strFileVersion );
			if (fileVersion == c_FileVersionNumber)
			{
				string strDataFormat = header.GetAttribute(c_DataFormat);
				mFileDataFormat = (StreamDataSetFormat)int.Parse(strDataFormat);
			}
			else if (fileVersion == 0)
			{
				mFileDataFormat = StreamDataSetFormat.XML;
			}
			else if (fileVersion > c_FileVersionNumber)
				throw new ApplicationException("File Version not supported, expected: " + c_FileVersionNumber.ToString());

			//Data Version
			string strDataVersion = header.GetAttribute(c_DataVersion);
			int dataVersion = int.Parse( strDataVersion );
			DataSet dataSet = CreateData(dataVersion);

			//Data
			tmp = xmlDoc.DocumentElement.SelectSingleNode("fileds:data", namespaceMan);
			if (tmp == null)
				throw new ApplicationException("File data not found");
			System.Xml.XmlElement xmlDataNode = (System.Xml.XmlElement)tmp;

			byte[] xmlBuffer = System.Convert.FromBase64String( xmlDataNode.InnerText );
			using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(xmlBuffer))
			{
				StreamDataSet.Read(memStream, dataSet, mFileDataFormat, MergeReadedSchema);
				//dataSet.ReadXml(memStream);
			}

			return dataSet;
		}
		#endregion
	}
}