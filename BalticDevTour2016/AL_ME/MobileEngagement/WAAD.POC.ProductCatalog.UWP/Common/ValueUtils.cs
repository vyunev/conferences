using System;
using System.Collections.Generic;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public static class ValueUtils
    {
        public static List<string> DeserializeStringList(this string data, string splitCharacter="|")
        {
            try
            {
                var tmp = (data ?? "").Split(new string[] { splitCharacter }, StringSplitOptions.RemoveEmptyEntries);
                if (tmp?.Length > 0)
                    return new List<string>(tmp);
            }
            catch
            { }
            return new List<string>();
        }

        public static string SerializeStringList(this List<string> values, string splitCharacter = "|")
        {
            try
            {
                if (values?.Count > 0)
                    return string.Join(splitCharacter, values);
            }
            catch
            { }
            return "";
        }

    }
}
