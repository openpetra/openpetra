jayrock-0.9.8316\src\Jayrock.Json\Json\Conversion\Converters\DateTimeImporter.cs

        protected override object ImportFromString(ImportContext context, JsonReader reader)
        {
            Debug.Assert(context != null);
            Debug.Assert(reader != null);

            try
            {
                return ReadReturning(reader, XmlConvert.ToDateTime(reader.Text, System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
            }
            catch (FormatException e)
            {
                throw new JsonException("Error importing JSON String as System.DateTime.", e);
            }
        }
