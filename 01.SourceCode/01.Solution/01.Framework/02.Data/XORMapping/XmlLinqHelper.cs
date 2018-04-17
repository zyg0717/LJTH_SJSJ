using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Framework.Data.XORMapping
{
    public static class XmlLinqHelper
    {
        public static string GetAttributeValue(XElement e, string key, string defaultValue)
        {
            string result = defaultValue;

            XAttribute attr = e.Attribute(key);
            if (attr != null)
            {
                result = attr.Value;
            }

            return result;
        }

        public static int GetAttributeValue(XElement e, string key, int defaultValue)
        {
            int result = defaultValue;

            string str = GetAttributeValue(e, key, string.Empty);
            if (!string.IsNullOrWhiteSpace(str))
            {
                int.TryParse(str, out result);
            }

            return result;
        }

        public static bool GetAttributeValue(XElement e, string key, bool  defaultValue)
        {
            bool result = defaultValue;

            string str = GetAttributeValue(e, key, string.Empty);
            if (!string.IsNullOrWhiteSpace(str))
            {
                bool.TryParse(str, out result);
            }

            return result;
        }
    }
}
