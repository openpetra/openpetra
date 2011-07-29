using System;
using System.Collections.Generic;
//// using System.Linq;
using System.Text;

namespace SampleDataConstructor
{
    class DataLine
    {
        private Dictionary<string, int> existingHeaders;
        private string[] dataLine;

        public void mustSet(ref string s, string fieldName)
        {
            s = getField(fieldName);
        }

        public void mustSet(ref char c, string fieldName)
        {
            string s = getField(fieldName).Trim();
            if (s.Length != 1) throw new Exception(
                "Illegal Data: " + s + " seems to contain several characters");
            c = s[0];
        }

        public void mustSet(ref int i, string fieldName)
        {
            string s = getField(fieldName);
            try
            {
                i = int.Parse(s);
            }
            catch (Exception e)
            {
                throw new Exception("Illegal data: '" + s + "' could not be transformed into an integer ", e);
            }
        }


        public void maySet(ref string s, string fieldName)
        {
            if (headerExists(fieldName))
            {
                mustSet(ref s, fieldName);
            }
        }

        public void tryToSet(ref int i, string fieldName)
        {
            if (headerExists(fieldName))
            {
                mustSet(ref i, fieldName);
            }
        }

        public void tryToSet(ref char c, string fieldName)
        {
            if (headerExists(fieldName))
            {
                mustSet(ref c, fieldName);
            }
        }

        public bool headerExists(string header)
        {
            return existingHeaders.ContainsKey(header);
        }

        public string getField(string fieldName)
        {
            return dataLine[existingHeaders[fieldName]];
        }

        public DataLine(Dictionary<string, int> existingHeaders, string[] dataLine)
        {
            this.existingHeaders = existingHeaders;
            this.dataLine = dataLine;
        }
    }

}
