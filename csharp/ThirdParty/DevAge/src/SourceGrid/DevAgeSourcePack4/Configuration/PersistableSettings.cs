using System;

namespace DevAge.Configuration
{
	/// <summary>
	/// Summary description for PersistableSettings.
	/// </summary>
	public class PersistableSettings
	{
		public PersistableSettings()
		{
		}

		private PersistableItemDictionary m_Dictionary = new PersistableItemDictionary();
		protected PersistableItemDictionary Dictionary
		{
			get{return m_Dictionary;}
		}

		protected void AddPersistableItem(PersistableItem item)
		{
			m_Dictionary.Add(item.Name, item);
		}

		protected virtual object this[string name]
		{
			get{return m_Dictionary[name].Value;}
			set{m_Dictionary[name].Value = value;}
		}

		[System.ComponentModel.Browsable(false)]
		public virtual bool HasChanges
		{
			get
			{
				foreach (PersistableItem item in m_Dictionary.Values)
				{
					if (item.IsChanged)
						return true;
				}

				return false;
			}
		}

		public virtual void AcceptChangesAsDefault()
		{
			foreach (PersistableItem item in m_Dictionary.Values)
				item.AcceptAsDefault();
		}


		public virtual void Reset()
		{
			foreach (PersistableItem item in m_Dictionary.Values)
				item.Reset();
		}

		#region Read/Write
		protected virtual System.IO.IsolatedStorage.IsolatedStorageFile GetStorage()
		{
			return System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForDomain();
		}

		protected virtual void WriteToIsolatedStorage(string fileName, PersistenceFlags flags)
		{
			using (System.IO.IsolatedStorage.IsolatedStorageFile l_Storage = GetStorage())
			{
				System.IO.IsolatedStorage.IsolatedStorageFileStream l_File = null;
				l_File = new System.IO.IsolatedStorage.IsolatedStorageFileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, l_Storage);
				try
				{
					WriteToStream(l_File, flags);
				}
				finally
				{
					l_File.Close();
				}

				l_Storage.Close();
			}		
		}
		protected virtual void ReadFromIsolatedStorage(string fileName)
		{
			using (System.IO.IsolatedStorage.IsolatedStorageFile l_Storage = GetStorage())
			{
				System.IO.IsolatedStorage.IsolatedStorageFileStream l_File = null;
				try
				{
					l_File = new System.IO.IsolatedStorage.IsolatedStorageFileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, l_Storage);
				}
				catch(System.IO.FileNotFoundException)
				{
					l_File = null;
				}

				if (l_File == null) //file non esiste
				{
				}
				else //file esiste
				{
					try
					{
						ReadFromStream(l_File);
					}
					finally
					{
						l_File.Close();
					}
				}

				l_Storage.Close();
			}
		}

		protected virtual bool IsolatedStorageExists(string fileName)
		{
			fileName = System.IO.Path.GetFileName(fileName).ToLower();
			using (System.IO.IsolatedStorage.IsolatedStorageFile l_Storage = GetStorage())
			{
				string[] files = l_Storage.GetFileNames("*");
				for (int i = 0; i < files.Length; i++)
				{
					string fileFinded = System.IO.Path.GetFileName(files[i]).ToLower();
					if (fileFinded == fileName)
						return true;
				}

				l_Storage.Close();
			}

			return false;
		}

		protected virtual void RemoveIsolatedStorage(string fileName)
		{
			using (System.IO.IsolatedStorage.IsolatedStorageFile l_Storage = GetStorage())
			{
				l_Storage.DeleteFile(fileName);
				l_Storage.Close();
			}
		}
		protected virtual void WriteToStream(System.IO.Stream stream, PersistenceFlags flags)
		{
//			System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);
//			
//			writer.Formatting = System.Xml.Formatting.Indented;
//			writer.WriteStartDocument();
//			writer.WriteStartElement("settings");
//
//			writer.WriteStartElement("items");
//			writer.WriteAttributeString("schemaversion", "1");
//			
//			foreach (PersistableItem item in m_Dictionary.Values)
//			{
//				if ( (flags & PersistenceFlags.OnlyChanges) == PersistenceFlags.OnlyChanges &&
//					item.IsChanged == false)
//					continue;
//
//				writer.WriteStartElement("item");
//				writer.WriteAttributeString("name", item.Name);
//				writer.WriteAttributeString("type", item.Type.FullName);
//				writer.WriteAttributeString("schemaversion", "1");
//				writer.WriteString(item.Validator.ValueToString(item.Value));
//				writer.WriteEndElement();
//			}
//
//			writer.WriteEndElement();
//
//			writer.WriteEndElement();
//			writer.WriteEndDocument();
//			writer.Flush();


			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
			doc.AppendChild(doc.CreateElement("settings"));
			WriteToXmlElement(doc.DocumentElement, flags);

			System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);
			writer.Formatting = System.Xml.Formatting.Indented;
			doc.WriteTo(writer);
			writer.Flush();
		}

		protected virtual void WriteToXmlElement(System.Xml.XmlElement xmlElement, PersistenceFlags flags)
		{
			//settings
			System.Xml.XmlNode xmlsettings = xmlElement.SelectSingleNode("settings");
			if (xmlsettings == null)
			{
				xmlsettings = xmlElement.OwnerDocument.CreateElement("settings");
				xmlElement.AppendChild(xmlsettings);
			}
			//items
			System.Xml.XmlNode xmlitems = xmlsettings.SelectSingleNode("items");
			if (xmlitems == null)
			{
				xmlitems = xmlsettings.OwnerDocument.CreateElement("items");
				xmlsettings.AppendChild(xmlitems);
				xmlitems.Attributes.Append(xmlitems.OwnerDocument.CreateAttribute("schemaversion"));
				xmlitems.Attributes["schemaversion"].Value = "1";
			}

			foreach (PersistableItem item in m_Dictionary.Values)
			{
				string xPath = string.Format("item[@name='{0}' and @type='{1}' and @schemaversion='1']", item.Name, item.Type.FullName);
				System.Xml.XmlNode xmlitem = xmlitems.SelectSingleNode(xPath);

				if ( (flags & PersistenceFlags.OnlyChanges) == PersistenceFlags.OnlyChanges &&
					item.IsChanged == false)
				{ //remove item
					if (xmlitem != null)
						xmlitems.RemoveChild(xmlitem);
				}
				else //add item
				{
					if (xmlitem == null)
					{
						xmlitem = xmlitems.OwnerDocument.CreateElement("item");
						xmlitems.AppendChild(xmlitem);

						xmlitem.Attributes.Append(xmlitem.OwnerDocument.CreateAttribute("name"));
						xmlitem.Attributes.Append(xmlitem.OwnerDocument.CreateAttribute("type"));
						xmlitem.Attributes.Append(xmlitem.OwnerDocument.CreateAttribute("schemaversion"));
						
						xmlitem.Attributes["name"].Value = item.Name;
						xmlitem.Attributes["type"].Value = item.Type.FullName;
						xmlitem.Attributes["schemaversion"].Value = "1";
					}
					xmlitem.InnerText = item.Validator.ValueToString(item.Value);
				}
			}
		}

		protected virtual void ReadFromXmlElement(System.Xml.XmlElement xmlElement)
		{
			foreach (PersistableItem item in m_Dictionary.Values)
			{
				string xPath = string.Format("settings/items/item[@name='{0}' and @type='{1}' and @schemaversion='1']", item.Name, item.Type.FullName);

				System.Xml.XmlNode node = xmlElement.SelectSingleNode(xPath);
				if (node != null)
				{
					item.Value = item.Validator.StringToValue(node.InnerText);
				}
			}
		}

		protected virtual void ReadFromStream(System.IO.Stream stream)
		{
			if (stream.Length == 0)
				return;

			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
			doc.Load(stream);

			ReadFromXmlElement(doc.DocumentElement);
		}

		protected virtual void ReadFromAppSettings(string itemPrefix)
		{
			if (System.Configuration.ConfigurationManager.AppSettings.Count == 0)
				return;

			foreach (PersistableItem item in m_Dictionary.Values)
			{
                string val = System.Configuration.ConfigurationManager.AppSettings[itemPrefix + item.Name];
				if (val != null)
				{
					item.Value = item.Validator.StringToValue(val);
				}
			}
		}

		protected virtual void ReadFromCommandLine(CommandLineArgs commandArguments, string itemPrefix, bool matchCase, bool throwErrorOnUnrecognizedParameter)
		{
//			foreach (PersistableItem item in m_Dictionary.Values)
//			{
//				string val = commandArguments[itemPrefix + item.Name];
//				if (val != null)
//				{
//					item.Value = item.Validator.StringToValue(val);
//				}
//			}
			
			foreach (string key in commandArguments.Keys)
			{
				string cmdParam = key;
				if (matchCase == false)
					cmdParam = cmdParam.ToUpper();

				bool found = false;
				foreach (PersistableItem item in m_Dictionary.Values)
				{
					string itemName = (itemPrefix + item.Name);
					if (matchCase == false)
						itemName = itemName.ToUpper();

					if (cmdParam == itemName)
					{
						item.Value = item.Validator.StringToValue( commandArguments[cmdParam] );
						found = true;
						break;
					}
				}

				if (found == false && throwErrorOnUnrecognizedParameter)
					throw new UnrecognizedCommandLineParametersException(key);
			}
		}

		/// <summary>
		/// Clone all fields using the ValueToString and StringToValue methods
		/// </summary>
		/// <param name="other"></param>
		protected virtual void ReadFromOther(PersistableSettings other)
		{
			foreach (PersistableItem item in m_Dictionary.Values)
			{
				foreach (PersistableItem itemOther in other.m_Dictionary.Values)
				{
					if (item.Name == itemOther.Name)
					{
						string valOther = itemOther.Validator.ValueToString( itemOther.Value );
						item.Value = item.Validator.StringToValue( valOther );
						break;
					}
				}
			}
		}
		#endregion

		public override string ToString()
		{
			string ret = "ApplicationSetting: ";
			foreach (PersistableItem item in m_Dictionary.Values)
				ret += item.ToString() + ", ";

			return ret;
		}
	}

	[Flags]
	public enum PersistenceFlags
	{
		None = 0,
		OnlyChanges = 1
	}
}
