using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace ChatGPTRequest.DataFormatter
{
    public static class JsonFormatter
    {
        public static string JsonPair(string a, string b)
        {
            
            return $"\"{a}\":\"{b}\"";
        }

        public static string JsonPair(string a, object b)
        {
            if (b is IEnumerable<object> enumerable)
            {
                // If b is an IEnumerable, join the elements with commas and wrap them in square brackets
                return $"\"{a}\":[{string.Join(",", enumerable)}]";
            }
            else if (b is string stringable)
            {
                // Otherwise, just add the value directly without wrapping in square brackets
                
                return $"\"{a}\":\"{stringable}\"";
            }
            else if (b is bool boolable)
            {
                return $"\"{a}\":{(boolable ? "true" : "false")}";
            }
            else
            {
                return $"\"{a}\":{b}";
            }
        }
        public static string ReScapeJson(string inputString)
        {
            Dictionary<string, string> escapeChars = new Dictionary<string, string>
        {
            { "\\n", "\n" },
            { "\\r", "\r" },
            { "\\t", "\t" },
            { "\\\"", "\"" }
        };

            foreach (var escapeChar in escapeChars)
            {
                inputString = inputString.Replace(escapeChar.Key, escapeChar.Value);
            }

            return inputString;
        }
        public static string EscapeJson(string inputString)
        {
            Dictionary<char, string> escapeChars = new()
            {
                { '\n', "\\n" },
                { '\r', "\\r" },
                { '\t', "\\t" },
                { '\"', "\\\"" }
            };

            StringBuilder sb = new(inputString.Length);

            foreach (char c in inputString)
            {
                if (escapeChars.TryGetValue(c, out string escaped))
                {
                    sb.Append(escaped);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}